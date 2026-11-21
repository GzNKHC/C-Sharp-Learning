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
            Ui.Start("QLTB · Quản lý thiết bị",
                new Entry("a. Phiếu nhập thiết bị", Imports),
                new Entry("b. Mượn thiết bị", Loans),
                new Entry("c. Trả thiết bị", Returns),
                new Entry("Danh mục thiết bị", store => new CatalogForm<ThietBi>(store, options: new CatalogOptions<ThietBi>
                {
                    Defaults = () => new ThietBi { TinhTrang = "Tốt" }, Locked = new[] { "SoLuong", "SoLuongHong" }
                })));
        }

        private static Form Imports(DataStore store)
        {
            var form = new RecordGrid<PhieuNhap>("Phiếu nhập thiết bị", true);
            Action refresh = () => form.Display(store.Read(s => s.List<PhieuNhap>()).OrderBy(row => row.MaPN));
            Ui.Button(form.Actions, "Lập phiếu", () =>
            {
                if (!Ui.Ask(form, "Lập phiếu nhập thiết bị?"))
                    return;
                var proposed = new PhieuNhap { NgayNhap = DateTime.Today, SoLuong = 1, TinhTrang = "Tốt" };
                if (EditDialog.Show(form, "Lập phiếu nhập thiết bị", store, proposed, row =>
                    store.Write(s =>
                    {
                        row.MaPN = s.NextCode("PN");
                        ChangeImport(s, row, null);
                        s.Insert(row);
                    }), hidden: new[] { "MaPN" }))
                    refresh();
            }, true);
            Ui.Button(form.Actions, "Chỉnh sửa", () =>
            {
                PhieuNhap old = form.RequireRow();
                if (!Ui.Ask(form, "Hiệu chỉnh phiếu " + old.MaPN + "?"))
                    return;
                if (EditDialog.Show(form, "Hiệu chỉnh phiếu nhập", store, old, row =>
                    store.Write(s =>
                    {
                        PhieuNhap actual = s.Find<PhieuNhap>(x => x.MaPN == old.MaPN);
                        if (actual == null)
                            throw new InvalidOperationException("Phiếu nhập không còn tồn tại.");
                        ChangeImport(s, row, actual);
                        s.Update(row);
                    }), original: old))
                    refresh();
            });
            Ui.Button(form.Actions, "Xóa", () =>
            {
                PhieuNhap old = form.RequireRow();
                if (!Ui.Ask(form, "Xóa phiếu nhập " + old.MaPN + "?"))
                    return;
                store.Write(s =>
                {
                    PhieuNhap actual = s.Find<PhieuNhap>(x => x.MaPN == old.MaPN);
                    if (actual == null)
                        throw new InvalidOperationException("Phiếu nhập không còn tồn tại.");
                    ChangeImport(s, null, actual);
                    s.Delete(actual);
                });
                refresh();
            });
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, refresh);
            return form;
        }

        private static void ChangeImport(DataSession session, PhieuNhap next, PhieuNhap old)
        {
            if (next != null && next.NgayNhap > DateTime.Today)
                throw new InvalidOperationException("Ngày nhập không thể sau ngày hiện tại.");

            string[] codes = new[] { old?.MaTB, next?.MaTB }.Where(code => code != null).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
            foreach (string code in codes)
            {
                PhieuNhap removed = string.Equals(old?.MaTB, code, StringComparison.OrdinalIgnoreCase) ? old : null;
                PhieuNhap added = string.Equals(next?.MaTB, code, StringComparison.OrdinalIgnoreCase) ? next : null;
                int good = Units(added, "Tốt") - Units(removed, "Tốt");
                int damaged = Units(added, "Hỏng") - Units(removed, "Hỏng");
                AdjustStock(session, code, good, damaged);
            }
        }

        private static int Units(PhieuNhap row, string condition) => row?.TinhTrang == condition ? row.SoLuong : 0;

        private static void AdjustStock(DataSession session, string code, int good, int damaged)
        {
            ThietBi device = session.Find<ThietBi>(row => row.MaTB == code);
            if (device == null)
                throw new InvalidOperationException("Thiết bị không tồn tại.");
            int available = checked(device.SoLuong + good);
            int broken = checked(device.SoLuongHong + damaged);
            if (available < 0 || broken < 0)
                throw new InvalidOperationException("Thao tác làm số lượng tồn của thiết bị " + code + " bị âm.");

            device.SoLuong = available;
            device.SoLuongHong = broken;
            device.TinhTrang = available > 0 ? "Tốt" : broken > 0 ? "Hỏng" : device.TinhTrang;
            session.Update(device);
        }

        private static Form Loans(DataStore store)
        {
            var form = new RecordGrid<PhieuMuon>("Mượn thiết bị", true);
            var completed = new CheckBox { Text = "Hiện cả phiếu đã trả đủ", AutoSize = true, Margin = new Padding(0, 8, 5, 4) };
            form.Filter.Controls.Add(completed);
            Action refresh = () =>
            {
                var rows = store.Read(s =>
                {
                    var returned = s.List<PhieuTra>().Where(row => row.MaPM != null)
                        .GroupBy(row => row.MaPM, StringComparer.OrdinalIgnoreCase)
                        .ToDictionary(group => group.Key, group => group.Sum(row => row.SoLuong), StringComparer.OrdinalIgnoreCase);
                    return s.List<PhieuMuon>().Where(row => completed.Checked ||
                        !returned.TryGetValue(row.MaPM, out int count) || count < row.SoLuong).OrderBy(row => row.MaPM).ToList();
                });
                form.Display(rows);
            };
            completed.CheckedChanged += (sender, args) => Ui.Run(form, refresh);
            Ui.Button(form.Actions, "Mượn thiết bị", () =>
            {
                if (!Ui.Ask(form, "Lập phiếu mượn thiết bị?"))
                    return;
                var proposed = new PhieuMuon { NgayMuon = DateTime.Today, SoLuong = 1, TinhTrang = "Tốt" };
                if (EditDialog.Show(form, "Mượn thiết bị", store, proposed, row =>
                    store.Write(s =>
                    {
                        if (row.NgayMuon > DateTime.Today)
                            throw new InvalidOperationException("Ngày mượn không được sau hôm nay.");
                        ThietBi device = s.Find<ThietBi>(d => d.MaTB == row.MaTB);
                        if (device == null || device.TinhTrang != "Tốt" || device.SoLuong < row.SoLuong)
                            throw new InvalidOperationException("Thiết bị phải ở tình trạng tốt và còn đủ số lượng.");
                        row.MaPM = s.NextCode("PM");
                        AdjustStock(s, row.MaTB, -row.SoLuong, 0);
                        s.Insert(row);
                    }), hidden: new[] { "MaPM" }, locked: new[] { "TinhTrang" }))
                    refresh();
            }, true);
            Ui.Button(form.Actions, "Xóa phiếu", () =>
            {
                PhieuMuon selected = form.RequireRow();
                if (!Ui.Ask(form, "Xóa phiếu mượn " + selected.MaPM + " đã trả đủ thiết bị?"))
                    return;
                store.Write(s =>
                {
                    PhieuMuon loan = s.Find<PhieuMuon>(row => row.MaPM == selected.MaPM);
                    if (loan == null)
                        throw new InvalidOperationException("Phiếu mượn không còn tồn tại.");
                    var returns = s.List<PhieuTra>(row => row.MaPM == loan.MaPM);
                    if (returns.Sum(row => row.SoLuong) != loan.SoLuong)
                        throw new InvalidOperationException("Chỉ xóa phiếu mượn đã trả đủ thiết bị.");
                    foreach (PhieuTra item in returns)
                    {
                        item.MaPM = null;
                        s.Update(item);
                    }
                    s.Delete(loan);
                });
                refresh();
            });
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, refresh);
            return form;
        }

        private static Form Returns(DataStore store)
        {
            var form = new RecordGrid<PhieuTra>("Trả thiết bị", true, "MaPT", "SoPhieuMuonGoc", "MaTB", "NVTra", "NVNhan", "NgayTra", "SoLuong", "TinhTrang", "MaPhong");
            ComboBox loanChoice = Ui.Combo(form.Filter, "Phiếu mượn còn thiết bị", 270);
            Action refresh = () =>
            {
                string selected = Ui.Code(loanChoice);
                var data = store.Read(s =>
                {
                    var returns = s.List<PhieuTra>();
                    var loans = s.List<PhieuMuon>().OrderBy(row => row.MaPM).ToList();
                    return new { returns, loans };
                });
                var choices = data.loans.Select(loan => new
                {
                    Loan = loan, Remaining = loan.SoLuong - data.returns.Where(r => r.MaPM == loan.MaPM).Sum(r => r.SoLuong)
                }).Where(item => item.Remaining > 0).Select(item => new Choice(item.Loan.MaPM,
                    item.Loan.MaPM + " – " + item.Loan.MaTB + " (còn " + item.Remaining + ")"));
                Ui.Choices(loanChoice, choices, first: false);
                Ui.Select(loanChoice, selected);
                form.Display(data.returns.OrderBy(row => row.MaPT));
            };
            Ui.Button(form.Actions, "Lập phiếu trả", () =>
            {
                string code = Ui.Require(Ui.Code(loanChoice), "phiếu mượn");
                PhieuMuon loan = store.Read(s => s.Find<PhieuMuon>(row => row.MaPM == code));
                if (loan == null || !Ui.Ask(form, "Lập phiếu trả cho " + code + "?"))
                    return;
                var defaults = new PhieuTra
                {
                    MaPM = code, SoPhieuMuonGoc = code, MaTB = loan.MaTB, MaPhong = loan.MaPhong,
                    NVTra = loan.NVMuon, NgayTra = DateTime.Today, TinhTrang = "Tốt", SoLuong = 1
                };
                if (EditDialog.Show(form, "Lập phiếu trả thiết bị", store, defaults, row =>
                    store.Write(s =>
                    {
                        PhieuMuon original = s.Find<PhieuMuon>(item => item.MaPM == code);
                        int received = s.List<PhieuTra>(item => item.MaPM == code).Sum(item => item.SoLuong);
                        if (original == null || row.SoLuong > original.SoLuong - received)
                            throw new InvalidOperationException("Số lượng trả lớn hơn số đang mượn.");
                        if (row.NgayTra < original.NgayMuon || row.NgayTra > DateTime.Today)
                            throw new InvalidOperationException("Ngày trả phải từ ngày mượn đến hôm nay.");

                        row.MaPT = s.NextCode("PT");
                        AdjustStock(s, row.MaTB, row.TinhTrang == "Tốt" ? row.SoLuong : 0, row.TinhTrang == "Hỏng" ? row.SoLuong : 0);
                        s.Insert(row);
                    }), hidden: new[] { "MaPT", "SoPhieuMuonGoc", "MaPM" }, locked: new[] { "MaTB", "MaPhong" }))
                    refresh();
            }, true);
            Ui.Button(form.Actions, "Xóa phiếu", () =>
            {
                PhieuTra selected = form.RequireRow();
                if (!Ui.Ask(form, "Xóa phiếu trả " + selected.MaPT + "?"))
                    return;
                store.Write(s =>
                {
                    PhieuTra row = s.Find<PhieuTra>(item => item.MaPT == selected.MaPT);
                    if (row == null || row.MaPM == null)
                        throw new InvalidOperationException("Không thể hủy phiếu trả sau khi phiếu mượn đã được xóa.");
                    AdjustStock(s, row.MaTB, row.TinhTrang == "Tốt" ? -row.SoLuong : 0, row.TinhTrang == "Hỏng" ? -row.SoLuong : 0);
                    s.Delete(row);
                });
                refresh();
            });
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, refresh);
            return form;
        }
    }
}
