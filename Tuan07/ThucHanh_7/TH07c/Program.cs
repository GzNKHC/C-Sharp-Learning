using System;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Ui.Start("Thực hành 07c – Nhập liệu trên lưới",
                new[] { "Nhập điểm", "Nhập lớp", "Mẫu · Quản lý sinh viên" },
                GradeForm, ClassForm, StudentForm);
        }

        private static Form GradeForm()
        {
            return new GridEditForm("Nhập điểm", new GridStore("Diem", null,
                new GridField("MaSinhVien", "Sinh viên", SqlDbType.NVarChar, 20, true, "SinhVien", "HoTen"),
                new GridField("MaMonHoc", "Môn học", SqlDbType.NVarChar, 20, true, "MonHoc", "TenMonHoc"),
                new GridField("Diem", "Điểm", SqlDbType.Decimal, format: "0.##")));
        }

        private static Form ClassForm()
        {
            return new GridEditForm("Nhập lớp", new GridStore("Lop", "SinhVien",
                new GridField("MaLop", "Mã lớp", SqlDbType.NVarChar, 20, true),
                new GridField("TenLop", "Tên lớp", SqlDbType.NVarChar, 100),
                new GridField("MaKhoa", "Khoa", SqlDbType.NVarChar, 20, false, "Khoa", "TenKhoa")));
        }

        private static Form StudentForm()
        {
            return new GridEditForm("Quản lý sinh viên", new GridStore("SinhVien", "Diem",
                new GridField("MaSinhVien", "Mã sinh viên", SqlDbType.NVarChar, 20, true),
                new GridField("HoTen", "Họ tên sinh viên", SqlDbType.NVarChar, 100),
                new GridField("NgaySinh", "Ngày sinh", SqlDbType.Date, format: "dd/MM/yyyy"),
                new GridField("MaLop", "Lớp", SqlDbType.NVarChar, 20, false, "Lop", "TenLop")));
        }
    }
}
