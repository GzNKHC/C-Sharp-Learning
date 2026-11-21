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
            Ui.Start("QLD · Quản lý điểm",
                new Entry("a. Quản lý sinh viên", store => new CatalogForm<SinhVien>(store, "Quản lý sinh viên",
                    new CatalogOptions<SinhVien> { SeparateSave = true, Search = new[] { "MaSV" } })),
                new Entry("b. Quản lý học phần", store => new CatalogForm<HocPhan>(store, "Quản lý học phần")),
                new Entry("c. Nhập và hiệu chỉnh điểm học phần", Grades),
                new Entry("d. Tra cứu điểm theo sinh viên và kỳ", store => LookupGrades(store, true)),
                new Entry("e. Tra cứu điểm theo sinh viên và học phần", store => LookupGrades(store, false)),
                new Entry("Đăng ký học phần", store => new CatalogForm<DangKy>(store, options: new CatalogOptions<DangKy> { Edit = false })));
        }

        private static Form Grades(DataStore store)
        {
            ComboBox subject = null;
            string course = "";
            var form = new ScoreEntryForm("Quản lý điểm học phần của sinh viên", store,
                new[] { "STT", "Mã sinh viên", "Họ tên", "Lớp" }, new[] { "Điểm" }, () =>
                {
                    return store.Read(session =>
                    {
                        var scores = session.List<KetQua>(row => row.MaHP == course)
                            .ToDictionary(row => row.MaSV, row => row.Diem, StringComparer.OrdinalIgnoreCase);
                        var enrollments = new HashSet<string>(session.List<DangKy>(row => row.MaHP == course).Select(row => row.MaSV),
                            StringComparer.OrdinalIgnoreCase);
                        return session.List<SinhVien>().Where(row => enrollments.Contains(row.MaSV)).OrderBy(row => row.MaSV)
                            .Select((row, index) => new ScoreLine(row.MaSV,
                                new object[] { index + 1, row.MaSV, row.Hoten, row.MaL },
                                scores.TryGetValue(row.MaSV, out decimal score) ? (decimal?)score : null)).ToList();
                    });
                }, (session, change) =>
                {
                    string student = change.Line.Code;
                    KetQua found = session.Find<KetQua>(row => row.MaSV == student && row.MaHP == course);
                    if (found?.Diem != change.Line.Grades[0])
                        throw new InvalidOperationException("Điểm của " + student + " đã thay đổi. Hãy tải lại.");
                    var grade = new KetQua { MaSV = student, MaHP = course, Diem = change.Scores[0] };
                    if (found == null)
                        session.Insert(grade);
                    else
                        session.Update(grade);
                });
            ComboBox semester = Ui.Combo(form.Selectors, "Học kỳ", 120);
            subject = Ui.Combo(form.Selectors, "Học phần", 250);
            Button find = Ui.Button(form.Selectors, "Tìm kiếm", () =>
            {
                course = Ui.Require(Ui.Code(subject), "học phần");
                form.LoadSelection();
            }, true);
            find.Enabled = false;
            semester.SelectedIndexChanged += (sender, args) => Ui.Run(form, () =>
            {
                form.ResetSelection();
                if (Ui.ChoiceValue(semester) is int term)
                {
                    var subjects = store.Read(s => s.List<HocPhan>(row => row.HK == term));
                    Ui.Choices(subject, subjects.OrderBy(row => row.TenHP).Select(row => new Choice(row.MaHP, row.TenHP)), first: false);
                }
            });
            subject.SelectedIndexChanged += (sender, args) =>
            {
                form.ResetSelection();
                find.Enabled = Ui.Code(subject).Length > 0;
            };
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var terms = store.Read(s => s.List<HocPhan>()).Select(row => row.HK).Distinct().OrderBy(value => value);
                Ui.Choices(semester, terms.Select(term => new Choice(term, term.ToString())), first: false);
            });
            return form;
        }

        private static Form LookupGrades(DataStore store, bool byTerm)
        {
            string title = byTerm ? "Tra cứu điểm theo sinh viên và kỳ" : "Tra cứu điểm theo sinh viên và học phần";
            var form = new Form();
            Ui.Window(form, title);
            var actions = Ui.Actions();
            var info = Ui.Actions();
            var grid = Ui.Grid();
            Ui.Columns(grid, "TenHP", "Tên học phần", "Diem", "Điểm", "HK", "Học kỳ");
            ComboBox student = Ui.Combo(actions, "Mã sinh viên", 180);
            var all = new RadioButton { Text = "Tất cả", AutoSize = true, Checked = true, Margin = new Padding(0, 8, 8, 0) };
            var one = new RadioButton
            {
                Text = byTerm ? "Một kỳ" : "Một học phần", AutoSize = true, Margin = new Padding(0, 8, 8, 0)
            };
            actions.Controls.Add(all);
            actions.Controls.Add(one);
            ComboBox value = Ui.Combo(actions, byTerm ? "Kỳ" : "Học phần", byTerm ? 110 : 210);
            TextBox name = Ui.TextBox(info, "Tên sinh viên", 235);
            TextBox birth = Ui.TextBox(info, "Ngày sinh", 112);
            TextBox home = Ui.TextBox(info, "Quê quán", 190);
            foreach (TextBox box in new[] { name, birth, home })
                box.ReadOnly = true;
            Button find = Ui.Button(actions, "Tìm kiếm", () =>
            {
                string id = Ui.Code(student);
                object criterion = one.Checked ? Ui.ChoiceValue(value) : null;
                var rows = store.Read(s =>
                {
                    var grades = s.List<KetQua>(row => row.MaSV == id);
                    var subjects = s.List<HocPhan>();
                    return (from grade in grades
                            join subject in subjects on grade.MaHP equals subject.MaHP
                            where criterion == null || (byTerm ? subject.HK.Equals(criterion) : subject.MaHP.Equals(criterion))
                            orderby subject.HK, subject.TenHP
                            select new { subject.TenHP, grade.Diem, subject.HK }).ToList();
                });
                Ui.Bind(grid, rows);
            }, true);
            Ui.Button(actions, "Kết thúc", form.Close);
            Action refresh = () =>
            {
                string id = Ui.Code(student);
                bool active = id.Length > 0;
                all.Enabled = one.Enabled = active;
                value.Enabled = active && one.Checked;
                find.Enabled = active && (!one.Checked || Ui.ChoiceValue(value) != null);
                grid.Enabled = active;
                info.Enabled = active;
            };
            one.CheckedChanged += (sender, args) => refresh();
            value.SelectedIndexChanged += (sender, args) => refresh();
            student.SelectedIndexChanged += (sender, args) => Ui.Run(form, () =>
            {
                string id = Ui.Code(student);
                SinhVien row = id.Length == 0 ? null : store.Read(s => s.Find<SinhVien>(sv => sv.MaSV == id));
                name.Text = row?.Hoten ?? "";
                birth.Text = row?.NS.ToString("dd/MM/yyyy") ?? "";
                home.Text = row?.QQ ?? "";
                grid.DataSource = null;
                refresh();
            });
            Ui.Arrange(form, grid, Ui.Heading(title), actions, info);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var students = store.Read(s => s.List<SinhVien>());
                Ui.Choices(student, students.OrderBy(row => row.MaSV).Select(row => new Choice(row.MaSV, row.MaSV)), first: false);
                var courses = store.Read(s => s.List<HocPhan>());
                var choices = byTerm
                    ? courses.Select(row => row.HK).Distinct().OrderBy(term => term).Select(term => new Choice(term, term.ToString()))
                    : courses.OrderBy(row => row.TenHP).Select(row => new Choice(row.MaHP, row.TenHP));
                Ui.Choices(value, choices, first: false);
                refresh();
            });
            refresh();
            return form;
        }
    }
}
