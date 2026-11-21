using System;
using System.Linq;
using System.Windows.Forms;
using ThucHanh9.Models;

namespace ThucHanh9
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Ui.Start("QLCB · Quản lý lương cán bộ",
                new Entry("Quản lý lương nhân viên", store => new CatalogForm<Luong>(store, options: new CatalogOptions<Luong>
                {
                    Search = new[] { "manv", "maphong" }, Filter = "maphong",
                    Validate = (s, row, old) =>
                    {
                        if (row.ngayLL > DateTime.Today)
                            throw new InvalidOperationException("Ngày lên lương không thể sau ngày hiện tại.");
                    }
                })),
                new Entry("Tra cứu lương theo nhân viên và phòng", SearchSalaries),
                new Entry("Hồ sơ nhân viên", store => new CatalogForm<HoSo>(store, options: new CatalogOptions<HoSo>
                {
                    Search = new[] { "manv", "Hoten" }
                })),
                new Entry("Danh mục phòng ban", store => new CatalogForm<PhongBan>(store)),
                new Entry("Danh mục ngạch", store => new CatalogForm<Ngach>(store)),
                new Entry("Danh mục chức vụ", store => new CatalogForm<ChucVu>(store)));
        }

        private static Form SearchSalaries(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Tra cứu lương theo nhân viên và phòng");
            var filters = Ui.Actions();
            var departments = Ui.Combo(filters, "Phòng ban", 235);
            var name = Ui.TextBox(filters, "Mã nhân viên");
            var grid = Ui.Grid();
            Ui.Columns(grid, "manv", "Mã nhân viên", "Hoten", "Họ tên", "Tenphong", "Phòng ban", "TenNg", "Ngạch",
                "TenCV", "Chức vụ", "Hesoluong", "Hệ số lương", "Hesophucap", "Hệ số phụ cấp", "NgayLenLuong", "Ngày lên lương");
            Action show = () =>
            {
                string id = name.Text.Trim();
                string department = Ui.Code(departments);
                var results = store.Read(s =>
                {
                    var salaries = s.List<Luong>(row => (id == "" || id == row.manv) && (department == "" || row.maphong == department));
                    var people = s.List<HoSo>();
                    var sections = s.List<PhongBan>();
                    var grades = s.List<Ngach>();
                    var roles = s.List<ChucVu>();
                    return (from salary in salaries
                            join person in people on salary.manv equals person.manv
                            join section in sections on salary.maphong equals section.Maphong
                            join grade in grades on person.MaNg equals grade.MaNg
                            join role in roles on person.MaCV equals role.MaCV
                            orderby person.Hoten, person.manv
                            select new { salary.manv, person.Hoten, section.Tenphong, grade.TenNg, role.TenCV,
                                salary.Hesoluong, role.Hesophucap, NgayLenLuong = salary.ngayLL }).ToList();
                });
                Ui.Bind(grid, results);
            };
            Ui.Button(filters, "Tìm kiếm", show, true);
            Ui.Button(filters, "Hiện hết", () => { name.Clear(); Ui.Select(departments, null); show(); });
            Ui.Button(filters, "Thoát", form.Close);
            Ui.Arrange(form, grid, Ui.Heading("Tra cứu lương nhân viên"), filters);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                Ui.Choices(departments, store.Read(s => s.List<PhongBan>()).OrderBy(r => r.Tenphong)
                    .Select(r => new Choice(r.Maphong, r.Tenphong)), "Tất cả");
                show();
            });
            return form;
        }
    }
}
