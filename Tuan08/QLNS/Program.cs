using System;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLNS · Quản lý nhân sự",
                new Entry("a. Quản lý nhân viên", store => new CatalogForm<NhanVien>(store, "Quản lý nhân viên",
                    new CatalogOptions<NhanVien>
                    {
                        Filter = "MaPhongBan", SortName = "HoTenNhanVien", ConfirmChange = true,
                        Validate = (session, row, old) =>
                        {
                            if (row.NgaySinh > DateTime.Today || row.NgayVaoLam < row.NgaySinh)
                                throw new InvalidOperationException("Ngày sinh hoặc ngày vào làm chưa hợp lệ.");
                        }
                    })),
                new Entry("b. Danh mục phòng ban", store => new InlineCatalogForm<PhongBan>(store, "Danh mục phòng ban")),
                new Entry("Danh mục chức vụ", store => new CatalogForm<ChucVu>(store)));
        }
    }
}
