using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ThucHanh7
{
    internal sealed class GridStore
    {
        private readonly string table;
        private readonly string dependentTable;
        private readonly GridField[] keys;
        public GridField[] Fields { get; }

        public GridStore(string table, string dependentTable, params GridField[] fields)
        {
            this.table = table;
            this.dependentTable = dependentTable;
            Fields = fields;
            keys = fields.Where(field => field.PrimaryKey).ToArray();
        }

        public DataTable ReadRows()
        {
            var rows = Db.Read("SELECT " + string.Join(", ", Fields.Select(field => field.Name)) +
                " FROM dbo." + table + " ORDER BY " + string.Join(", ", keys.Select(key => key.Name)));
            // Dữ liệu nhập dở chỉ nằm trong bộ nhớ. Kiểm tra tất cả các ô khi bấm Lưu.
            foreach (DataColumn column in rows.Columns)
                column.AllowDBNull = true;
            return rows;
        }

        public DataTable ReadChoices(GridField field)
        {
            return Db.ReadWithReader("SELECT " + field.Name + ", " + field.LookupCaption +
                " FROM dbo." + field.LookupTable + " ORDER BY " + field.LookupCaption + ", " + field.Name);
        }

        private string KeyCondition => " WHERE " + string.Join(" AND ",
            keys.Select((key, index) => key.Name + " = @key" + index));

        private SqlParameter[] KeyParameters(DataRow row, DataRowVersion version)
        {
            return keys.Select((key, index) =>
                key.Parameter("@key" + index, Db.Value(row[key.Name, version]).Trim())).ToArray();
        }

        public void Save(DataRow[] changed)
        {
            string[] names = Fields.Select(field => field.Name).ToArray();
            string insertSql = "INSERT dbo." + table + " (" + string.Join(", ", names) + ") VALUES (" +
                string.Join(", ", names.Select(name => "@" + name)) + ")";
            string updateSql = "UPDATE dbo." + table + " SET " + string.Join(", ",
                Fields.Where(field => !field.PrimaryKey).Select(field => field.Name + " = @" + field.Name)) +
                KeyCondition;

            Db.InTransaction((connection, transaction) =>
            {
                foreach (DataRow row in changed)
                {
                    object[] values = Fields.Select(field => field.Check(row)).ToArray();
                    bool insert = row.RowState == DataRowState.Added;
                    for (int i = 0; i < Fields.Length; i++)
                    {
                        GridField field = Fields[i];
                        if (field.LookupTable != null &&
                            !Db.Exists(connection, transaction, "SELECT 1 FROM dbo." + field.LookupTable +
                                " WHERE " + field.Name + " = @ma", field.Parameter("@ma", values[i])))
                            throw new InvalidOperationException(field.Caption + " không tồn tại.");
                    }

                    if (insert && Db.Exists(connection, transaction,
                        "SELECT 1 FROM dbo." + table + KeyCondition, KeyParameters(row, DataRowVersion.Current)))
                        throw new InvalidOperationException("Mã hoặc tổ hợp mã này đã tồn tại.");

                    SqlParameter[] parameters = Fields.Select((field, index) =>
                        new { Field = field, Value = values[index] })
                        .Where(item => insert || !item.Field.PrimaryKey)
                        .Select(item => item.Field.Parameter("@" + item.Field.Name, item.Value)).ToArray();
                    if (!insert)
                        parameters = parameters.Concat(KeyParameters(row, DataRowVersion.Original)).ToArray();

                    if (Db.Execute(connection, transaction, insert ? insertSql : updateSql, parameters) != 1)
                        throw new InvalidOperationException("Dòng cần sửa không còn trong cơ sở dữ liệu.");
                }
            }, IsolationLevel.Serializable);
        }

        public void Delete(DataRow row)
        {
            Db.InTransaction((connection, transaction) =>
            {
                if (dependentTable != null && Db.Exists(connection, transaction, "SELECT 1 FROM dbo." +
                    dependentTable + " WHERE " + keys[0].Name + " = @ma", keys[0].Parameter("@ma", row[keys[0].Name])))
                    throw new InvalidOperationException("Dữ liệu đang được sử dụng, không thể xóa.");

                if (Db.Execute(connection, transaction, "DELETE dbo." + table + KeyCondition,
                    KeyParameters(row, DataRowVersion.Current)) != 1)
                    throw new InvalidOperationException("Dòng này không còn trong cơ sở dữ liệu.");
            });
        }
    }
}
