using System;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLVDV · Quản lý hoạt động câu lạc bộ",
                new Entry("Quản lý thành viên", store => new CatalogForm<ThanhVien>(store, options: new CatalogOptions<ThanhVien>
                {
                    Search = new[] { "MaThanhvien", "Hoten", "Donvi" },
                    Validate = (s, row, old) =>
                    {
                        if (row.Ngaysinh > DateTime.Today)
                            throw new InvalidOperationException("Ngày sinh không thể sau hôm nay.");
                    }
                })),
                new Entry("Quản lý hoạt động", store => new CatalogForm<HoatDong>(store, options: new CatalogOptions<HoatDong>
                {
                    Search = new[] { "Mahoatdong", "Tenhoatdong" }
                })),
                new Entry("Hoạt động của thành viên", store => new CatalogForm<Phieu>(store, options: new CatalogOptions<Phieu>
                {
                    Filter = "Mahoatdong", Search = new[] { "Mathanhvien", "Danhgia" },
                    Defaults = () => new Phieu { ThoigianBD = DateTime.Now, ThoigianKT = DateTime.Now.AddHours(1) },
                    Validate = (s, row, old) =>
                    {
                        if (row.ThoigianKT <= row.ThoigianBD)
                            throw new InvalidOperationException("Thời gian kết thúc phải sau thời gian bắt đầu.");
                    }
                })));
        }
    }
}
