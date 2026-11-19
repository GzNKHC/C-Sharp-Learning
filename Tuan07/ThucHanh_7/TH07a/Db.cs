using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;

namespace ThucHanh7
{
    internal static class Db
    {
        public static SqlConnection Connect()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["ThucHanh7"].ConnectionString);
        }

        public static SqlCommand Command(SqlConnection connection, string sql, SqlTransaction transaction,
            params SqlParameter[] parameters)
        {
            var command = new SqlCommand(sql, connection, transaction);
            command.Parameters.AddRange(parameters);
            return command;
        }

        public static SqlParameter Text(string name, string value, int length = 20)
        {
            return new SqlParameter(name, SqlDbType.NVarChar, length) { Value = value };
        }

        public static SqlParameter Number(string name, SqlDbType type, object value)
        {
            return new SqlParameter(name, type) { Value = value };
        }

        public static DataTable Read(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Connect())
            using (var command = Command(connection, sql, null, parameters))
            using (var adapter = new SqlDataAdapter(command))
            {
                var data = new DataSet();
                adapter.Fill(data, "KetQua");
                return data.Tables["KetQua"];
            }
        }

        public static DataTable ReadWithReader(string sql, params SqlParameter[] parameters)
        {
            using (var connection = Connect())
            using (var command = Command(connection, sql, null, parameters))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }

        public static bool Exists(SqlConnection connection, SqlTransaction transaction, string sql,
            params SqlParameter[] parameters)
        {
            using (var command = Command(connection, sql, transaction, parameters))
            using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                return reader.Read();
        }

        public static int Execute(SqlConnection connection, SqlTransaction transaction, string sql,
            params SqlParameter[] parameters)
        {
            using (var command = Command(connection, sql, transaction, parameters))
                return command.ExecuteNonQuery();
        }

        public static void InTransaction(Action<SqlConnection, SqlTransaction> change,
            IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            using (var connection = Connect())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(isolation))
                {
                    change(connection, transaction);
                    transaction.Commit();
                }
            }
        }

        public static string Value(object value)
        {
            return Convert.ToString(value, CultureInfo.CurrentCulture) ?? "";
        }
    }
}
