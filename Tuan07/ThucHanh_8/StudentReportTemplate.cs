using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ThucHanh8
{
    internal static class StudentReportData
    {
        public const string TableName = "DanhSachSinhVien";

        public static DataTable ReadStudents(string classCode = "", string facultyCode = "")
        {
            return Db.Read(@"SELECT s.MaSinhVien, s.HoTen, s.NgaySinh, l.MaLop, l.TenLop
                FROM dbo.SinhVien s INNER JOIN dbo.Lop l ON l.MaLop = s.MaLop
                WHERE (@lop = N'' OR l.MaLop = @lop) AND (@khoa = N'' OR l.MaKhoa = @khoa)
                ORDER BY l.MaLop, s.MaSinhVien", Db.Text("@lop", classCode), Db.Text("@khoa", facultyCode));
        }

        public static DataTable ReadClasses()
        {
            return Db.Read("SELECT MaLop, TenLop FROM dbo.Lop ORDER BY TenLop, MaLop");
        }

        public static DataTable ForReport(IEnumerable<DataRow> students)
        {
            var table = new DataTable(TableName);
            table.Columns.Add("MaLop", typeof(string)).MaxLength = 20;
            table.Columns.Add("Lop", typeof(string)).MaxLength = 140;
            table.Columns.Add("STT", typeof(string)).MaxLength = 12;
            table.Columns.Add("MaSinhVien", typeof(string)).MaxLength = 20;
            table.Columns.Add("HoTen", typeof(string)).MaxLength = 100;
            table.Columns.Add("NgaySinh", typeof(string)).MaxLength = 10;
            table.Columns.Add("TongSoTheoLop", typeof(string)).MaxLength = 12;

            foreach (var group in students.GroupBy(row => Db.Value(row["MaLop"]), StringComparer.Ordinal)
                .OrderBy(group => group.Key, StringComparer.Ordinal))
            {
                DataRow[] rows = group.OrderBy(row => Db.Value(row["MaSinhVien"]), StringComparer.Ordinal).ToArray();
                for (int index = 0; index < rows.Length; index++)
                {
                    DataRow row = rows[index];
                    table.Rows.Add(group.Key, group.Key + " – " + Db.Value(row["TenLop"]), (index + 1).ToString(CultureInfo.InvariantCulture),
                        Db.Value(row["MaSinhVien"]), Db.Value(row["HoTen"]),
                        ((DateTime)row["NgaySinh"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        rows.Length.ToString(CultureInfo.InvariantCulture));
                }
            }
            return table;
        }
    }
}
