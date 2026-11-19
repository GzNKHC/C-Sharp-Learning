using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal static class Program
    {
        private const string LopSql = @"SELECT l.MaLop, l.TenLop, l.MaKhoa, k.TenKhoa
            FROM dbo.Lop l INNER JOIN dbo.Khoa k ON k.MaKhoa = l.MaKhoa
            ORDER BY l.MaLop";

        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Ui.Start("Thực hành 07a – Tương tác cơ sở dữ liệu",
                new[]
                {
                    "Quản lý lớp theo khoa", "Quản lý sinh viên theo lớp", "Danh mục hàng hóa",
                    "Quản lý điểm", "Mẫu · Danh mục khoa", "Mẫu · Danh sách lớp"
                },
                ClassForm, StudentForm, GoodsForm, GradeForm, DepartmentForm, ClassList);
        }

        private static Form ClassForm()
        {
            return new CatalogForm("Quản lý lớp theo khoa", "Lop", LopSql, new[] { "MaLop" }, "SinhVien",
                new RecordField("MaLop", "Mã lớp", 20),
                new RecordField("TenLop", "Tên lớp", 100),
                new RecordField("MaKhoa", "Mã khoa", "Khoa", "TenKhoa", "Tất cả khoa"));
        }

        private static Form StudentForm()
        {
            return new CatalogForm("Quản lý sinh viên theo lớp", "SinhVien",
                @"SELECT s.MaSinhVien, s.HoTen, s.NgaySinh, s.MaLop, l.TenLop
                  FROM dbo.SinhVien s INNER JOIN dbo.Lop l ON l.MaLop = s.MaLop ORDER BY s.MaSinhVien",
                new[] { "MaSinhVien" }, "Diem",
                new RecordField("MaSinhVien", "Mã sinh viên", 20),
                new RecordField("HoTen", "Họ tên sinh viên", 100),
                RecordField.Date("NgaySinh", "Ngày sinh"),
                new RecordField("MaLop", "Mã lớp", "Lop", "TenLop", "Tất cả lớp"));
        }

        private static Form GoodsForm()
        {
            return new CatalogForm("Danh mục hàng hóa", "DMHang",
                @"SELECT h.MaHang, h.TenHang, h.MaChatLieu, c.TenChatLieu, h.SoLuong,
                         h.DonGiaNhap, h.DonGiaBan, h.Hinh, h.GhiChu
                  FROM dbo.DMHang h INNER JOIN dbo.DMChatLieu c ON c.MaChatLieu = h.MaChatLieu
                  ORDER BY h.MaHang", new[] { "MaHang" }, null,
                new RecordField("MaHang", "Mã hàng", 20),
                new RecordField("TenHang", "Tên hàng", 100),
                new RecordField("MaChatLieu", "Mã chất liệu", "DMChatLieu", "TenChatLieu"),
                RecordField.Numeric("SoLuong", "Số lượng", int.MaxValue, true),
                RecordField.Numeric("DonGiaNhap", "Đơn giá nhập", 999999999999.99m),
                RecordField.Numeric("DonGiaBan", "Đơn giá bán", 999999999999.99m),
                new RecordField("Hinh", "Tên tệp ảnh", 255, false),
                new RecordField("GhiChu", "Ghi chú", 255, false));
        }

        private static Form GradeForm()
        {
            return new CatalogForm("Quản lý điểm sinh viên", "Diem",
                @"SELECT d.MaSinhVien, s.HoTen, l.TenLop, d.MaMonHoc, m.TenMonHoc, d.Diem
                  FROM dbo.Diem d INNER JOIN dbo.SinhVien s ON s.MaSinhVien = d.MaSinhVien
                  INNER JOIN dbo.Lop l ON l.MaLop = s.MaLop
                  INNER JOIN dbo.MonHoc m ON m.MaMonHoc = d.MaMonHoc
                  ORDER BY d.MaSinhVien, d.MaMonHoc", new[] { "MaSinhVien", "MaMonHoc" }, null,
                new RecordField("MaSinhVien", "Mã sinh viên", "SinhVien", "HoTen", "Tất cả sinh viên"),
                new RecordField("MaMonHoc", "Mã môn học", "MonHoc", "TenMonHoc", "Tất cả môn học"),
                RecordField.Numeric("Diem", "Điểm", 10));
        }

        private static Form DepartmentForm()
        {
            return new CatalogForm("Danh mục khoa", "Khoa",
                "SELECT MaKhoa, TenKhoa FROM dbo.Khoa ORDER BY MaKhoa", new[] { "MaKhoa" }, "Lop",
                new RecordField("MaKhoa", "Mã khoa", 20), new RecordField("TenKhoa", "Tên khoa", 100));
        }

        private static Form ClassList()
        {
            var form = new Form();
            Ui.SetWindow(form, "Danh sách lớp học", 830, 510);
            var grid = Ui.Grid();
            Ui.Arrange(form, grid);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                grid.DataSource = Db.Read(LopSql);
                Ui.Header(grid, "MaLop", "Mã lớp");
                Ui.Header(grid, "TenLop", "Tên lớp");
                Ui.Header(grid, "MaKhoa", "Mã khoa");
                Ui.Header(grid, "TenKhoa", "Tên khoa");
            });
            return form;
        }
    }
}
