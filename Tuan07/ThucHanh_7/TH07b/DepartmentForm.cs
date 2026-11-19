using System;
using System.Data;
using System.Data.SqlClient;

namespace ThucHanh7
{
    internal sealed class DepartmentForm : EditForm
    {
        public DepartmentForm() : base("Quản lý khoa", "Mã khoa", "Tên khoa", false)
        {
            Ui.Arrange(this, Grid, Toolbar, Fields);
        }

        protected override string CodeName => "MaKhoa";
        protected override string NameColumn => "TenKhoa";

        protected override DataTable ReadRows()
        {
            return Db.Read("SELECT MaKhoa, TenKhoa FROM dbo.Khoa ORDER BY MaKhoa");
        }

        protected override void Store(string code, string name, bool insert)
        {
            Change(code, name, insert, false);
        }

        protected override void Remove(string code)
        {
            Change(code, null, false, true);
        }

        private static void Change(string code, string name, bool insert, bool remove)
        {
            Db.InTransaction((connection, transaction) =>
            {
                if (remove && Db.Exists(connection, transaction, "SELECT 1 FROM dbo.Lop WHERE MaKhoa = @ma",
                    Db.Text("@ma", code)))
                    throw new InvalidOperationException("Khoa đang có lớp, không thể xóa.");

                using (var select = Db.Command(connection, "SELECT MaKhoa, TenKhoa FROM dbo.Khoa WHERE MaKhoa = @ma",
                    transaction, Db.Text("@ma", code)))
                using (var adapter = new SqlDataAdapter(select) { MissingSchemaAction = MissingSchemaAction.AddWithKey })
                using (var builder = new SqlCommandBuilder(adapter))
                using (var data = new DataSet())
                {
                    adapter.Fill(data, "Khoa");
                    DataTable departments = data.Tables["Khoa"];
                    DataRow selected = departments.Rows.Find(code);
                    if (insert)
                    {
                        if (departments.Rows.Count > 0)
                            throw new InvalidOperationException("Mã khoa đã tồn tại.");
                        DataRow department = departments.NewRow();
                        department["MaKhoa"] = code;
                        department["TenKhoa"] = name;
                        departments.Rows.Add(department);
                    }
                    else
                    {
                        if (departments.Rows.Count == 0)
                            throw new InvalidOperationException("Khoa này không còn trong cơ sở dữ liệu.");
                        selected = selected ?? departments.Rows[0];
                        if (remove)
                            selected.Delete();
                        else
                            selected["TenKhoa"] = name;
                    }
                    adapter.Update(data, "Khoa");
                }
            });
        }
    }
}
