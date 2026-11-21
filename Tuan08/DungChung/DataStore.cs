using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Linq;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ThucHanh9
{
    internal enum DataMethod { AdoNet, LinqToSql, EntityFramework }

    internal sealed class DataStore
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Db"].ConnectionString;

        public DataStore(DataMethod method) { Method = method; }
        public DataMethod Method { get; }

        public TResult Read<TResult>(Func<DataSession, TResult> read)
        {
            using (DataSession session = Open(false))
                return read(session);
        }

        public void Write(Action<DataSession> change)
        {
            using (DataSession session = Open(true))
            {
                change(session);
                session.Commit();
            }
        }

        private DataSession Open(bool writing)
        {
            if (Method == DataMethod.EntityFramework)
                return new EfSession(connectionString, writing);

            var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                SqlTransaction transaction = writing ? connection.BeginTransaction(IsolationLevel.Serializable) : null;
                return Method == DataMethod.LinqToSql
                    ? (DataSession)new LinqSession(connection, transaction)
                    : new AdoSession(connection, transaction);
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }
    }

    internal abstract class DataSession : IDisposable
    {
        private static readonly MethodInfo UntypedRead = typeof(DataSession).GetMethod(nameof(ReadObjects),
            BindingFlags.Instance | BindingFlags.NonPublic);
        public abstract List<T> List<T>(Expression<Func<T, bool>> filter = null) where T : class, new();
        public abstract void Insert<T>(T value) where T : class, new();
        public abstract void Update<T>(T value) where T : class, new();
        public abstract void Delete<T>(T value) where T : class, new();
        public abstract void Commit();
        public abstract void Dispose();
        protected abstract SqlConnection SqlConnection { get; }
        protected abstract SqlTransaction SqlTransaction { get; }

        public T Find<T>(Expression<Func<T, bool>> filter) where T : class, new() => List(filter).SingleOrDefault();

        public List<object> List(Type type)
        {
            try
            {
                return (List<object>)UntypedRead.MakeGenericMethod(type).Invoke(this, null);
            }
            catch (TargetInvocationException exception) when (exception.InnerException != null)
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                throw;
            }
        }

        private List<object> ReadObjects<T>() where T : class, new() => List<T>().Cast<object>().ToList();

        public string NextCode(string prefix)
        {
            using (var command = new SqlCommand("SELECT NEXT VALUE FOR dbo.SeqMaTuDong", SqlConnection, SqlTransaction))
                return prefix + Convert.ToInt64(command.ExecuteScalar()).ToString("D5",
                    System.Globalization.CultureInfo.InvariantCulture);
        }

        protected T Existing<T>(T value) where T : class, new()
        {
            return Find(TableInfo.ByKey(value)) ?? throw new InvalidOperationException("Bản ghi đã bị xóa hoặc thay đổi.");
        }
    }

    internal sealed class LinqSession : DataSession
    {
        private readonly SqlConnection connection;
        private readonly SqlTransaction transaction;
        private readonly DataContext context;

        public LinqSession(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
            context = new DataContext(connection) { Transaction = transaction };
        }

        protected override SqlConnection SqlConnection => connection;
        protected override SqlTransaction SqlTransaction => transaction;

        public override List<T> List<T>(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = context.GetTable<T>();
            return (filter == null ? query : query.Where(filter)).ToList();
        }

        public override void Insert<T>(T value)
        {
            context.GetTable<T>().InsertOnSubmit(value);
            context.SubmitChanges();
        }

        public override void Update<T>(T value)
        {
            TableInfo.Copy(value, Existing(value));
            context.SubmitChanges();
        }

        public override void Delete<T>(T value)
        {
            context.GetTable<T>().DeleteOnSubmit(Existing(value));
            context.SubmitChanges();
        }

        public override void Commit() => transaction?.Commit();

        public override void Dispose()
        {
            context.Dispose();
            transaction?.Dispose();
            connection.Dispose();
        }
    }

    internal sealed class EfSession : DataSession
    {
        private readonly EntityConnection connection;
        private readonly EntityTransaction transaction;
        private readonly DbContext context;

        public EfSession(string connectionString, bool writing)
        {
            string model = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "Model");
            var settings = new EntityConnectionStringBuilder
            {
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = connectionString,
                Metadata = model + ".csdl|" + model + ".ssdl|" + model + ".msl"
            };
            connection = new EntityConnection(settings.ConnectionString);
            try
            {
                connection.Open();
                transaction = writing ? connection.BeginTransaction(IsolationLevel.Serializable) : null;
                context = new DbContext(connection, false);
            }
            catch
            {
                connection.Dispose();
                throw;
            }
        }

        protected override SqlConnection SqlConnection => (SqlConnection)connection.StoreConnection;
        protected override SqlTransaction SqlTransaction => (SqlTransaction)transaction?.StoreTransaction;

        public override List<T> List<T>(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = context.Set<T>();
            return (filter == null ? query : query.Where(filter)).ToList();
        }

        public override void Insert<T>(T value)
        {
            context.Set<T>().Add(value);
            context.SaveChanges();
        }

        public override void Update<T>(T value)
        {
            TableInfo.Copy(value, Existing(value));
            context.SaveChanges();
        }

        public override void Delete<T>(T value)
        {
            context.Set<T>().Remove(Existing(value));
            context.SaveChanges();
        }

        public override void Commit() => transaction?.Commit();

        public override void Dispose()
        {
            context.Dispose();
            transaction?.Dispose();
            connection.Dispose();
        }
    }

    internal sealed class AdoSession : DataSession
    {
        private readonly SqlConnection connection;
        private readonly SqlTransaction transaction;

        public AdoSession(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        protected override SqlConnection SqlConnection => connection;
        protected override SqlTransaction SqlTransaction => transaction;

        public override List<T> List<T>(Expression<Func<T, bool>> filter = null)
        {
            Field[] fields = TableInfo.Fields(typeof(T));
            string columns = string.Join(", ", fields.Select(field => TableInfo.Quote(field.Name)));
            var output = new List<T>();
            using (var query = new SqlCommand("SELECT " + columns + " FROM " + TableInfo.Table(typeof(T)), connection, transaction))
            {
                if (filter != null)
                    query.CommandText += " WHERE " + SqlCondition.Build(query, filter);
                using (SqlDataReader reader = query.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new T();
                        for (int i = 0; i < fields.Length; i++)
                            fields[i].Set(row, reader.IsDBNull(i) ? null : reader.GetValue(i));
                        output.Add(row);
                    }
                }
            }
            return output;
        }

        public override void Insert<T>(T value)
        {
            using (var command = new SqlCommand("", connection, transaction))
            {
                Field[] fields = TableInfo.Fields(typeof(T));
                string columns = string.Join(", ", fields.Select(field => TableInfo.Quote(field.Name)));
                string args = string.Join(", ", fields.Select(field => SqlCondition.Parameter(command, field.Get(value), field)));
                command.CommandText = "INSERT INTO " + TableInfo.Table(typeof(T)) + " (" + columns + ") VALUES (" + args + ")";
                command.ExecuteNonQuery();
            }
        }

        public override void Update<T>(T value)
        {
            using (var command = new SqlCommand("", connection, transaction))
            {
                string changes = string.Join(", ", TableInfo.Fields(typeof(T)).Where(field => !field.IsKey).Select(field =>
                    TableInfo.Quote(field.Name) + " = " + SqlCondition.Parameter(command, field.Get(value), field)));
                string where = SqlCondition.Build(command, TableInfo.ByKey(value));
                command.CommandText = "UPDATE " + TableInfo.Table(typeof(T)) + " SET " + changes + " WHERE " + where;
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Bản ghi không còn tồn tại.");
            }
        }

        public override void Delete<T>(T value)
        {
            using (var command = new SqlCommand("", connection, transaction))
            {
                command.CommandText = "DELETE FROM " + TableInfo.Table(typeof(T)) + " WHERE " +
                    SqlCondition.Build(command, TableInfo.ByKey(value));
                if (command.ExecuteNonQuery() != 1)
                    throw new InvalidOperationException("Bản ghi không còn tồn tại.");
            }
        }

        public override void Commit() => transaction?.Commit();

        public override void Dispose()
        {
            transaction?.Dispose();
            connection.Dispose();
        }
    }
}
