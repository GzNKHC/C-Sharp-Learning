using System;
using System.Collections.Generic;
using System.Data;
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
            Ui.Start("QLTS · Quản lý tuyển sinh",
                new Entry("a. Nhập điểm thi theo phòng", ExamScores),
                new Entry("b. Danh mục ưu tiên", store => new InlineCatalogForm<UuTien>(store, "Danh mục ưu tiên tuyển sinh")),
                new Entry("c. Quản lý ngành học", store => new CatalogForm<Nganh>(store, "Quản lý ngành học",
                    new CatalogOptions<Nganh> { SeparateSave = true, Search = new[] { "MaN" } })),
                new Entry("d. Tra cứu điểm thi", SearchScores));
        }

        private static Form ExamScores(DataStore store)
        {
            ComboBox rooms = null;
            var form = new ScoreEntryForm("Nhập điểm thi thí sinh theo phòng", store,
                new[] { "Số báo danh", "Họ tên", "Ngày sinh", "Giới tính", "Quê quán" },
                new[] { "Điểm Toán", "Điểm Lý", "Điểm Hóa" },
                () =>
                {
                    int room = (int)Ui.ChoiceValue(rooms);
                    return store.Read(s => s.List<ThiSinh>(row => row.SP == room)).OrderBy(row => row.SBD)
                        .Select(row => new ScoreLine(row.SBD,
                            new object[] { row.SBD, row.Hoten, row.NS.ToString("dd/MM/yyyy"), row.GT, row.QQ },
                            row.DT, row.DL, row.DH)).ToList();
                },
                (session, change) =>
                {
                    string sbd = change.Line.Code;
                    ThiSinh row = session.Find<ThiSinh>(r => r.SBD == sbd);
                    if (row == null || !new[] { row.DT, row.DL, row.DH }.SequenceEqual(change.Line.Grades))
                        throw new InvalidOperationException("Điểm thí sinh " + sbd + " đã thay đổi. Hủy và tải lại.");
                    row.DT = change.Scores[0];
                    row.DL = change.Scores[1];
                    row.DH = change.Scores[2];
                    session.Update(row);
                });

            rooms = Ui.Combo(form.Selectors, "Số phòng");
            rooms.SelectedIndexChanged += (sender, args) =>
            {
                if (Ui.ChoiceValue(rooms) != null)
                    Ui.Run(form, form.LoadSelection);
            };
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var numbers = store.Read(s => s.List<ThiSinh>()).Select(row => row.SP).Distinct().OrderBy(room => room);
                Ui.Choices(rooms, numbers.Select(room => new Choice(room, room.ToString())), first: false);
            });
            return form;
        }

        private static Form SearchScores(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Tra cứu điểm thi");
            var toolbar = Ui.Actions();
            TextBox code = Ui.TextBox(toolbar, "Số báo danh");
            TextBox name = Ui.TextBox(toolbar, "Họ tên", 235);
            DataGridView grid = Ui.Grid();
            Ui.Columns(grid, "SBD", "Số báo danh", "Hoten", "Họ tên", "NgaySinh", "Ngày sinh", "Nganh", "Tên ngành",
                "DT", "Toán", "DL", "Lý", "DH", "Hóa", "DC", "Điểm cộng", "TongDiem", "Tổng điểm");

            Button search = Ui.Button(toolbar, "Tìm kiếm", () =>
            {
                var criteria = QueryFilter.For<ThiSinh>(new[]
                {
                    new Criterion("SBD", code.Text.Trim(), true), new Criterion("Hoten", name.Text.Trim(), false)
                });

                var results = store.Read(session =>
                {
                    var majors = session.List<Nganh>().ToDictionary(row => row.MaN, StringComparer.OrdinalIgnoreCase);
                    var priorities = session.List<UuTien>().ToDictionary(row => row.MaUT, StringComparer.OrdinalIgnoreCase);
                    return session.List(criteria).OrderBy(row => row.SBD).Select(row =>
                    {
                        decimal bonus = row.MaUT != null && priorities.TryGetValue(row.MaUT, out UuTien priority) ? priority.DC : 0;
                        return new
                        {
                            row.SBD, row.Hoten, NgaySinh = row.NS, Nganh = majors[row.MaN].TenN,
                            row.DT, row.DL, row.DH, DC = bonus, TongDiem = row.DT + row.DL + row.DH + bonus
                        };
                    }).ToList();
                });
                Ui.Bind(grid, results);
            }, true);
            search.Enabled = false;
            EventHandler enable = (sender, args) => search.Enabled = code.Text.Trim().Length > 0 || name.Text.Trim().Length > 0;
            code.TextChanged += enable;
            name.TextChanged += enable;
            Ui.Button(toolbar, "Thoát", form.Close);
            Ui.Arrange(form, grid, Ui.Heading("Tra cứu điểm thi"), toolbar);
            return form;
        }
    }
}
