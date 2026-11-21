using System;
using System.Collections.Generic;
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
            Ui.Start("QLTV · Quản lý thư viện",
                new Entry("a. Quản lý độc giả", store => new CatalogForm<DocGia>(store, options: new CatalogOptions<DocGia> { Navigate = true })),
                new Entry("b. Quản lý sách", store => new CatalogForm<Sach>(store)),
                new Entry("c. Quản lý phiếu mượn", store => new CatalogForm<PhieuMuon>(store, options: new CatalogOptions<PhieuMuon>
                {
                    Navigate = true, Edit = false, Delete = false, Defaults = () => new PhieuMuon { NgayMuon = DateTime.Today },
                    Validate = (session, loan, old) => CheckLoan(loan)
                })),
                new Entry("d. Quản lý chi tiết phiếu mượn", LoanDetails),
                new Entry("e. Tìm kiếm sách", store => new SearchForm<Sach>(store, "Tìm kiếm sách", false, true, "TenSach", "TacGia")),
                new Entry("f. Nhập chi tiết phiếu mượn", AddDetails),
                new Entry("g. Nhận trả sách", ReceiveBook),
                new Entry("h. Lập phiếu mượn và kiểm tra quá hạn", NewLoan));
        }

        private static void CheckLoan(PhieuMuon loan)
        {
            if (loan.NgayMuon > DateTime.Today)
                throw new InvalidOperationException("Ngày mượn không thể sau hôm nay.");
        }

        private static void BorrowBook(DataSession session, ChiTietPhieuMuon line)
        {
            PhieuMuon loan = session.Find<PhieuMuon>(row => row.SoPhieu == line.SoPhieu);
            if (loan == null)
                throw new InvalidOperationException("Phiếu mượn không tồn tại.");
            if (line.NgayHenTra < loan.NgayMuon)
                throw new InvalidOperationException("Ngày hẹn trả không được trước ngày mượn.");
            if (session.List<ChiTietPhieuMuon>(row => row.MaSach == line.MaSach && row.NgayTra == null).Count > 0)
                throw new InvalidOperationException("Bản sách " + line.MaSach + " đang được mượn.");
            session.Insert(line);
        }

        private static RelationForm<PhieuMuon, ChiTietPhieuMuon> Borrowed(DataStore store, string title)
        {
            return new RelationForm<PhieuMuon, ChiTietPhieuMuon>(store, title, "Phiếu mượn", "Chi tiết phiếu mượn",
                (session, loan) => session.List<ChiTietPhieuMuon>(line => line.SoPhieu == loan.SoPhieu));
        }

        private static Form LoanDetails(DataStore store)
        {
            var form = Borrowed(store, "Quản lý chi tiết phiếu mượn");
            Ui.Navigator(form.Actions, form.Parents);
            Ui.Button(form.Actions, "Thêm phiếu và sách", () =>
            {
                PhieuMuon draft = null;
                if (!EditDialog.Show(form, "Thông tin phiếu mượn", store, new PhieuMuon { NgayMuon = DateTime.Today }, row =>
                {
                    CheckLoan(row);
                    draft = row;
                }))
                    return;
                var defaults = new ChiTietPhieuMuon { NgayHenTra = draft.NgayMuon.AddDays(14), SoPhieu = draft.SoPhieu };
                if (EditDialog.Show(form, "Chọn sách đưa vào phiếu " + draft.SoPhieu, store, defaults, line =>
                    store.Write(s =>
                    {
                        s.Insert(draft);
                        BorrowBook(s, line);
                    }), hidden: new[] { "NgayTra", "SoPhieu" }))
                    form.ReloadParents(selectedKey: draft.SoPhieu);
            }, true);
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, () => form.ReloadParents());
            return form;
        }

        private static Form AddDetails(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Nhập chi tiết phiếu mượn");
            var filters = Ui.Actions();
            var actions = Ui.Actions();
            ComboBox loan = Ui.Combo(filters, "Số phiếu", 155);
            DateTimePicker due = Ui.Date();
            Ui.Label(filters, "Ngày hẹn trả");
            due.Width = 118;
            due.Value = DateTime.Today.AddDays(14);
            filters.Controls.Add(due);
            DataGridView books = Ui.Grid();
            Ui.Columns<Sach>(books);
            DataGridView lines = Ui.Grid();
            Ui.Columns<ChiTietPhieuMuon>(lines);
            var pending = new List<ChiTietPhieuMuon>();

            Action refresh = () =>
            {
                List<ChiTietPhieuMuon> saved = store.Read(s => s.List<ChiTietPhieuMuon>());
                Ui.Bind(lines, saved.Concat(pending).OrderBy(row => row.SoPhieu).ThenBy(row => row.MaSach).ToList());
                foreach (DataGridViewRow row in lines.Rows)
                    if (row.DataBoundItem is ChiTietPhieuMuon item && pending.Contains(item))
                        row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            };
            Ui.Button(actions, "Mượn", () =>
            {
                string code = Ui.Require(Ui.Code(loan), "số phiếu");
                Sach book = Ui.Selected<Sach>(books) ?? throw new InvalidOperationException("Hãy chọn sách ở lưới bên trái.");
                if (pending.Any(row => row.MaSach == book.MaSach) ||
                    store.Read(s => s.List<ChiTietPhieuMuon>(line => line.MaSach == book.MaSach && line.NgayTra == null)).Count > 0)
                    throw new InvalidOperationException("Bản sách này đang được mượn hoặc đã đưa vào danh sách chờ lưu.");
                var lineItem = new ChiTietPhieuMuon { SoPhieu = code, MaSach = book.MaSach, NgayHenTra = due.Value.Date };
                pending.Add(lineItem);
                refresh();
            });
            Ui.Button(actions, "Lưu", () =>
            {
                if (pending.Count == 0)
                    throw new InvalidOperationException("Chưa có chi tiết chờ lưu.");
                store.Write(s =>
                {
                    foreach (ChiTietPhieuMuon entry in pending)
                        BorrowBook(s, entry);
                });
                pending.Clear();
                refresh();
            }, true);
            Ui.Button(actions, "Bỏ dòng chờ", () =>
            {
                ChiTietPhieuMuon selected = Ui.Selected<ChiTietPhieuMuon>(lines);
                if (!pending.Remove(selected))
                    throw new InvalidOperationException("Hãy chọn một dòng chờ lưu.");
                refresh();
            });
            Ui.Button(actions, "Thoát", form.Close);
            Ui.Arrange(form, Ui.TwoGrids("Sách", books, "Chi tiết phiếu mượn", lines, true),
                Ui.Heading("Nhập chi tiết phiếu mượn"), filters, actions);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var all = store.Read(s => s.List<PhieuMuon>());
                Ui.Choices(loan, all.OrderBy(row => row.SoPhieu).Select(row => new Choice(row.SoPhieu, row.SoPhieu)), first: false);
                Ui.Bind(books, store.Read(s => s.List<Sach>()).OrderBy(row => row.MaSach).ToList());
                refresh();
            });
            form.FormClosing += (sender, args) =>
                args.Cancel = pending.Count > 0 && !Ui.Ask(form, "Thoát và bỏ các chi tiết đang chờ lưu?");
            return form;
        }

        private static Form ReceiveBook(DataStore store)
        {
            var form = Borrowed(store, "Nhận trả sách");
            TextBox input = Ui.TextBox(form.ControlsBar, "Số phiếu", 145);
            ComboBox combo = Ui.Combo(form.ControlsBar, "Chọn số phiếu", 145);
            combo.SelectedIndexChanged += (sender, args) =>
            {
                if (Ui.Code(combo).Length > 0)
                    input.Text = Ui.Code(combo);
            };
            string filter = "";
            Action refresh = () =>
            {
                if (filter.Length > 0)
                    form.ReloadParents(row => row.SoPhieu == filter, filter);
                else
                    form.ReloadParents();
            };
            Ui.Button(form.ControlsBar, "Tìm", () => { filter = Ui.Require(input.Text, "số phiếu"); refresh(); }, true);
            Ui.Button(form.ControlsBar, "Hiện hết", () => { filter = ""; input.Clear(); refresh(); });
            Ui.Button(form.Actions, "Trả", () =>
            {
                ChiTietPhieuMuon selected = form.ChildRow ?? throw new InvalidOperationException("Hãy chọn sách cần trả.");
                store.Write(s =>
                {
                    ChiTietPhieuMuon actual = s.Find<ChiTietPhieuMuon>(row => row.SoPhieu == selected.SoPhieu && row.MaSach == selected.MaSach);
                    if (actual == null || actual.NgayTra.HasValue)
                        throw new InvalidOperationException("Sách đã trả hoặc chi tiết phiếu không còn tồn tại.");
                    PhieuMuon header = s.Find<PhieuMuon>(row => row.SoPhieu == selected.SoPhieu);
                    if (header == null || header.NgayMuon > DateTime.Today)
                        throw new InvalidOperationException("Phiếu mượn không hợp lệ.");
                    actual.NgayTra = DateTime.Today;
                    s.Update(actual);
                });
                form.ReloadChildren();
            }, true);
            Ui.Button(form.Actions, "Thoát", form.Close);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                refresh();
                form.BindParentCombo(combo);
            });
            return form;
        }

        private static Form NewLoan(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Lập phiếu mượn");
            var controls = Ui.Actions();
            ComboBox reader = Ui.Combo(controls, "Mã độc giả", 265);
            var records = Ui.Grid();
            Ui.Columns<PhieuMuon>(records);
            var overdue = Ui.Grid();
            Ui.Columns(overdue, "SoPhieu", "Số phiếu", "MaDocGia", "Mã độc giả", "HoTen", "Họ tên", "MaSach", "Mã sách",
                "TenSach", "Tên sách", "NgayHenTra", "Ngày hẹn trả", "NgayTra", "Ngày trả");

            Action refresh = () => Ui.Bind(records, store.Read(s => s.List<PhieuMuon>()).OrderBy(row => row.SoPhieu).ToList());
            Ui.Button(controls, "Kiểm tra quá hạn", () =>
            {
                string id = Ui.Require(Ui.Code(reader), "độc giả");
                DateTime today = DateTime.Today;
                var rows = store.Read(s =>
                {
                    var loans = s.List<PhieuMuon>(row => row.MaDocGia == id);
                    DocGia member = s.Find<DocGia>(row => row.MaDocGia == id);
                    var books = s.List<Sach>();
                    var details = s.List<ChiTietPhieuMuon>(row => row.NgayTra == null && row.NgayHenTra < today);
                    return (from line in details
                            join loan in loans on line.SoPhieu equals loan.SoPhieu
                            join book in books on line.MaSach equals book.MaSach
                            orderby line.NgayHenTra
                            select new { line.SoPhieu, loan.MaDocGia, member.HoTen, line.MaSach, book.TenSach,
                                line.NgayHenTra, line.NgayTra }).ToList();
                });
                Ui.Bind(overdue, rows);
            });
            reader.SelectedIndexChanged += (sender, args) => overdue.DataSource = null;
            Ui.Button(controls, "Lập phiếu", () =>
            {
                string id = Ui.Require(Ui.Code(reader), "độc giả");
                if (EditDialog.Show(form, "Lập phiếu mượn sách", store, new PhieuMuon { MaDocGia = id, NgayMuon = DateTime.Today }, row =>
                    store.Write(s =>
                    {
                        row.SoPhieu = s.NextCode("PM");
                        row.NgayMuon = DateTime.Today;
                        s.Insert(row);
                    }), hidden: new[] { "SoPhieu" }, locked: new[] { "MaDocGia", "NgayMuon" }))
                    refresh();
            }, true);
            Ui.Button(controls, "Thoát", form.Close);
            Ui.Arrange(form, Ui.TwoGrids("Sách mượn quá hạn chưa trả", overdue, "Phiếu mượn", records),
                Ui.Heading("Lập phiếu mượn theo độc giả"), controls);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var people = store.Read(s => s.List<DocGia>()).OrderBy(row => row.MaDocGia)
                    .Select(row => new Choice(row.MaDocGia, row.MaDocGia + " – " + row.HoTen));
                Ui.Choices(reader, people, first: false);
                refresh();
            });
            return form;
        }
    }
}
