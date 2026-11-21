using System;
using System.Collections.Generic;
using System.Globalization;
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
            Ui.Start("QLBD · Quản lý thuê băng đĩa",
                new Entry("a. Quản lý băng đĩa", store => new CatalogForm<BangDia>(store,
                    options: new CatalogOptions<BangDia> { Validate = CheckCatalogStock })),
                new Entry("b. Quản lý khách hàng thuê băng đĩa", Rentals),
                new Entry("c. Thanh toán thuê băng đĩa", Pay));
        }

        private static void CheckCatalogStock(DataSession session, BangDia row, BangDia old)
        {
            if (old != null && session.List<Thue>(rental => rental.MaBD == row.MaBD && rental.NgayThanhToan == null)
                .Sum(rental => rental.Soluongthue) > row.Soluong)
                throw new InvalidOperationException("Số lượng băng đĩa không thể nhỏ hơn số đang cho thuê.");
        }

        private sealed class RentalRow
        {
            public Thue Rental { get; set; }
            public KhachHang Customer { get; set; }
            public string MaThue => Rental.MaThue;
            public string MaKH => Customer.MaKH;
            public string HoTen => Customer.Hoten;
            public string DienThoai => Customer.Dienthoai;
            public string MaBD => Rental.MaBD;
            public int SoLuong => Rental.Soluongthue;
            public DateTime NgayThue => Rental.Ngaythue;
            public int? SoNgay => Rental.Songaythue;
            public decimal? ThanhTien => Rental.ThanhTien;
            public DateTime? NgayThanhToan => Rental.NgayThanhToan;
        }

        private static List<RentalRow> ReadRentals(DataStore store, string disc = "", string name = "")
        {
            return store.Read(s =>
            {
                var customers = s.List<KhachHang>(row => name == "" || row.Hoten.Contains(name))
                    .ToDictionary(row => row.MaKH, StringComparer.OrdinalIgnoreCase);
                return s.List<Thue>(row => disc == "" || row.MaBD == disc).Where(row => customers.ContainsKey(row.MaKH))
                    .OrderBy(row => row.MaThue).Select(row => new RentalRow
                    {
                        Rental = row, Customer = customers[row.MaKH]
                    }).ToList();
            });
        }

        private static DataGridView RentalGrid()
        {
            var grid = Ui.Grid();
            Ui.Columns(grid, "MaThue", "Mã lượt thuê", "MaKH", "Mã khách", "HoTen", "Họ tên", "DienThoai", "Điện thoại",
                "MaBD", "Mã băng đĩa", "SoLuong", "Số lượng thuê", "NgayThue", "Ngày thuê", "SoNgay", "Số ngày thuê",
                "ThanhTien", "Thành tiền", "NgayThanhToan", "Ngày thanh toán");
            grid.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
            return grid;
        }

        private static Form Rentals(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Quản lý khách hàng thuê băng đĩa");
            var filter = Ui.Actions();
            var buttons = Ui.Actions();
            ComboBox disc = Ui.Combo(filter, "Mã băng đĩa", 150);
            TextBox title = Ui.TextBox(filter, "Tên băng đĩa", 190);
            var inventory = Ui.Grid();
            Ui.Columns<BangDia>(inventory);
            DataGridView grid = RentalGrid();

            Action<string, string> lookupDiscs = (code, name) => Ui.Bind(inventory,
                store.Read(s => s.List<BangDia>(row => (code == "" || row.MaBD == code) && (name == "" || row.Tenbangdia.Contains(name))))
                    .OrderBy(row => row.Tenbangdia).ToList());
            Action reload = () => Ui.Bind(grid, ReadRentals(store));
            Ui.Button(filter, "Xem theo mã băng đĩa", () => lookupDiscs(Ui.Require(Ui.Code(disc), "mã băng đĩa"), ""));
            Ui.Button(filter, "Xem theo tên băng đĩa", () => lookupDiscs("", Ui.Require(title.Text, "tên băng đĩa")));
            Ui.Button(filter, "Hiện hết", () => lookupDiscs("", ""));

            Ui.Button(buttons, "Nhập", () =>
            {
                string code = Ui.Selected<BangDia>(inventory)?.MaBD ?? "";
                if (EditRental(form, store, null, code))
                    reload();
            }, true);
            Ui.Button(buttons, "Sửa", () =>
            {
                RentalRow row = Ui.Selected<RentalRow>(grid) ?? throw new InvalidOperationException("Hãy chọn một lượt thuê.");
                if (row.NgayThanhToan.HasValue)
                    throw new InvalidOperationException("Lượt thuê đã thanh toán.");
                if (EditRental(form, store, row, row.MaBD))
                    reload();
            });
            Ui.Button(buttons, "Xóa", () =>
            {
                RentalRow row = Ui.Selected<RentalRow>(grid) ?? throw new InvalidOperationException("Hãy chọn một lượt thuê.");
                if (!Ui.Ask(form, "Xóa lượt thuê " + row.MaThue + "?"))
                    return;
                store.Write(s =>
                {
                    Thue actual = s.Find<Thue>(item => item.MaThue == row.MaThue);
                    if (actual == null || actual.NgayThanhToan.HasValue)
                        throw new InvalidOperationException("Lượt thuê không tồn tại hoặc đã thanh toán.");
                    s.Delete(actual);
                });
                reload();
            });
            Ui.Button(buttons, "Thoát", form.Close);
            Ui.Arrange(form, Ui.TwoGrids("Chi tiết băng đĩa", inventory, "Chi tiết khách thuê", grid),
                Ui.Heading("Quản lý khách thuê băng đĩa"), filter, buttons);

            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                Ui.Choices(disc, Discs(store), first: false);
                lookupDiscs("", "");
                reload();
            });
            form.FormClosing += (sender, args) => args.Cancel = !Ui.Ask(form, "Bạn có muốn thoát khỏi form?");
            return form;
        }

        private static IEnumerable<Choice> Discs(DataStore store)
        {
            return store.Read(s => s.List<BangDia>()).OrderBy(row => row.MaBD)
                .Select(row => new Choice(row.MaBD, row.MaBD + " – " + row.Tenbangdia));
        }

        private static bool EditRental(Form owner, DataStore store, RentalRow old, string disc)
        {
            using (var form = new Form())
            {
                Ui.Window(form, old == null ? "Nhập thông tin khách thuê" : "Sửa thông tin khách thuê", 760, 260);
                var layout = Ui.Layout();
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                var fields = Ui.Actions();
                TextBox key = Ui.TextBox(fields, "Mã khách", 129);
                TextBox name = Ui.TextBox(fields, "Họ tên khách", 230);
                TextBox phone = Ui.TextBox(fields, "Điện thoại", 140);
                ComboBox product = Ui.Combo(fields, "Mã băng đĩa", 225);
                TextBox count = Ui.TextBox(fields, "Số lượng thuê", 80);
                key.MaxLength = 20;
                name.MaxLength = 100;
                phone.MaxLength = 20;
                key.Text = old?.MaKH ?? "";
                key.ReadOnly = old != null;
                name.Text = old?.HoTen ?? "";
                phone.Text = old?.DienThoai ?? "";
                count.Text = old?.SoLuong.ToString(CultureInfo.InvariantCulture) ?? "1";
                Ui.Choices(product, Discs(store), first: false);
                Ui.Select(product, disc);

                var commands = Ui.Actions();
                Ui.Button(commands, "Lưu", () =>
                {
                    string customerCode = Ui.Require(key.Text, "mã khách hàng");
                    string customerName = Ui.Require(name.Text, "họ tên khách hàng");
                    string telephone = Ui.Require(phone.Text, "điện thoại");
                    string itemCode = Ui.Require(Ui.Code(product), "mã băng đĩa");
                    if (!int.TryParse(count.Text.Trim(), out int units) || units <= 0)
                        throw new InvalidOperationException("Số lượng thuê phải là số nguyên dương.");

                    store.Write(s => SaveRental(s, old?.Rental, new KhachHang
                    {
                        MaKH = customerCode, Hoten = customerName, Dienthoai = telephone
                    }, itemCode, units));
                    form.DialogResult = DialogResult.OK;
                }, true);
                Ui.Button(commands, "Hủy", () => form.DialogResult = DialogResult.Cancel);
                layout.Controls.Add(Ui.Heading(form.Text));
                layout.Controls.Add(fields);
                layout.Controls.Add(commands);
                form.Controls.Add(layout);
                return form.ShowDialog(owner) == DialogResult.OK;
            }
        }

        private static void SaveRental(DataSession session, Thue original, KhachHang customer, string disc, int units)
        {
            BangDia item = session.Find<BangDia>(row => row.MaBD == disc);
            if (item == null)
                throw new InvalidOperationException("Băng đĩa không tồn tại.");

            Thue actual = original == null ? null : session.Find<Thue>(row => row.MaThue == original.MaThue);
            if (original != null && (actual == null || actual.NgayThanhToan.HasValue))
                throw new InvalidOperationException("Lượt thuê đã thanh toán hoặc đã bị xóa.");

            int rented = session.List<Thue>(row => row.MaBD == disc && row.NgayThanhToan == null)
                .Where(row => row.MaThue != actual?.MaThue).Sum(row => row.Soluongthue);
            if (units > item.Soluong - rented)
                throw new InvalidOperationException("Số lượng thuê vượt quá số băng đĩa còn lại.");

            KhachHang found = session.Find<KhachHang>(row => row.MaKH == customer.MaKH);
            if (found == null)
                session.Insert(customer);
            else if (original != null)
                session.Update(customer);
            else if (found.Hoten != customer.Hoten || found.Dienthoai != customer.Dienthoai)
                throw new InvalidOperationException("Mã khách đã tồn tại. Hãy nhập thông tin khớp với khách hàng đó.");

            if (actual == null)
            {
                session.Insert(new Thue
                {
                    MaThue = session.NextCode("T"), MaKH = customer.MaKH, MaBD = disc,
                    Ngaythue = DateTime.Today, Soluongthue = units
                });
            }
            else
            {
                actual.MaBD = disc;
                actual.Soluongthue = units;
                session.Update(actual);
            }
        }

        private static Form Pay(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Thanh toán thuê băng đĩa");
            var filter = Ui.Actions();
            var summary = Ui.Actions();
            var grid = RentalGrid();
            ComboBox code = Ui.Combo(filter, "Mã băng đĩa", 195);
            TextBox customer = Ui.TextBox(filter, "Họ tên khách", 185);
            TextBox paidDisc = Ui.TextBox(summary, "Mã băng đĩa thuê", 100);
            TextBox days = Ui.TextBox(summary, "Số ngày thuê", 70);
            TextBox money = Ui.TextBox(summary, "Tổng tiền", 125);
            foreach (TextBox box in new[] { paidDisc, days, money })
                box.ReadOnly = true;
            string currentDisc = "", currentCustomer = "";
            Action reload = () => Ui.Bind(grid, ReadRentals(store, currentDisc, currentCustomer));

            Ui.Button(filter, "Tìm theo mã băng đĩa", () =>
            {
                currentDisc = Ui.Require(Ui.Code(code), "mã băng đĩa");
                currentCustomer = "";
                reload();
            });
            Ui.Button(filter, "Tìm theo họ tên", () =>
            {
                currentCustomer = Ui.Require(customer.Text, "họ tên khách");
                currentDisc = "";
                reload();
            });
            Ui.Button(summary, "Tính tiền", () =>
            {
                RentalRow selected = Ui.Selected<RentalRow>(grid) ?? throw new InvalidOperationException("Hãy tìm và chọn một lượt thuê.");
                Thue paid = null;
                store.Write(s =>
                {
                    paid = s.Find<Thue>(row => row.MaThue == selected.MaThue);
                    if (paid == null)
                        throw new InvalidOperationException("Lượt thuê không còn tồn tại.");
                    if (paid.NgayThanhToan.HasValue)
                        return;
                    int total = (DateTime.Today - paid.Ngaythue.Date).Days + 1;
                    if (total <= 0)
                        throw new InvalidOperationException("Ngày thanh toán phải từ ngày thuê trở đi.");
                    paid.Songaythue = total;
                    paid.ThanhTien = total * 5000m;
                    paid.NgayThanhToan = DateTime.Today;
                    s.Update(paid);
                });
                reload();
                Ui.SelectRow<RentalRow>(grid, row => row.MaThue == paid.MaThue);
                paidDisc.Text = paid.MaBD;
                days.Text = paid.Songaythue?.ToString(CultureInfo.CurrentCulture) ?? "";
                money.Text = paid.ThanhTien?.ToString("N0", CultureInfo.CurrentCulture) ?? "";
            }, true);
            Ui.Button(summary, "Thoát", form.Close);
            Ui.Arrange(form, grid, Ui.Heading("Thanh toán thuê băng đĩa"), filter, summary);
            form.Load += (sender, args) => Ui.Run(form, () => Ui.Choices(code, Discs(store), first: false));
            form.FormClosing += (sender, args) => args.Cancel = !Ui.Ask(form, "Bạn có muốn thoát khỏi form?");
            return form;
        }
    }
}
