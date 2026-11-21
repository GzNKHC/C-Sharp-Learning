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
            Ui.Start("QLGV · Quản lý giáo viên",
                new Entry("a–b. Danh sách và chi tiết giáo viên", Teachers),
                new Entry("c. Quản lý đơn vị", store => new CatalogForm<DonVi>(store, "Quản lý đơn vị")));
        }

        private static Form Teachers(DataStore store)
        {
            var form = new Form();
            Ui.Window(form, "Danh sách giáo viên", 810, 555);
            var toolbar = Ui.Actions();
            ComboBox campus = Ui.Combo(toolbar, "Cơ sở", 230);
            ComboBox section = Ui.Combo(toolbar, "Đơn vị đào tạo", 230);
            Ui.Button(toolbar, "Thoát", form.Close);
            DataGridView grid = Ui.Grid();
            Ui.Columns(grid, "STT", "STT", "Hoten", "Họ và tên giáo viên");
            grid.Columns["STT"].FillWeight = 16;
            Ui.Arrange(form, grid, Ui.Heading("Danh sách giáo viên"), toolbar);

            Func<GiaoVien> selected = () => Ui.Selected<TeacherRow>(grid)?.Teacher;
            Action reload = () =>
            {
                string unit = Ui.Code(section);
                List<GiaoVien> teachers = unit.Length == 0 ? new List<GiaoVien>() : store.Read(s => s.List<GiaoVien>(row => row.Madonvi == unit));
                Ui.Bind(grid, teachers.OrderBy(row => row.Hoten, StringComparer.CurrentCultureIgnoreCase)
                    .ThenBy(row => row.Magv).Select((row, index) => new TeacherRow { STT = index + 1, Teacher = row }).ToList());
                grid.ClearSelection();
                grid.CurrentCell = null;
            };

            campus.SelectedIndexChanged += (sender, args) => Ui.Run(form, () =>
            {
                string key = Ui.Code(campus);
                var units = key.Length == 0 ? new List<DonVi>() : store.Read(s => s.List<DonVi>(row => row.Macoso == key));
                Ui.Choices(section, units.OrderBy(row => row.Tendonvi, StringComparer.CurrentCultureIgnoreCase)
                    .Select(row => new Choice(row.Madonvi, row.Tendonvi)), first: false);
                reload();
            });
            section.SelectedIndexChanged += (sender, args) => Ui.Run(form, reload);
            form.Load += (sender, args) => Ui.Run(form, () =>
            {
                var campuses = store.Read(s => s.List<CoSo>());
                Ui.Choices(campus, campuses.OrderBy(row => row.Tencoso).Select(row => new Choice(row.Macoso, row.Tencoso)), first: false);
            });

            Action detail = () =>
            {
                GiaoVien teacher = selected();
                if (teacher != null)
                    ShowDetail(form, store, teacher);
            };
            var context = new ContextMenuStrip();
            ToolStripItem open = context.Items.Add("Hiển thị thông tin chi tiết của GV", null, (sender, args) => Ui.Run(form, detail));
            ToolStripItem delete = context.Items.Add("Xóa GV đang chọn", null, (sender, args) => Ui.Run(form, () =>
            {
                GiaoVien row = selected();
                if (row == null || !Ui.Ask(form, "Xóa giáo viên " + row.Hoten + " khỏi cơ sở dữ liệu?"))
                    return;
                store.Write(s => s.Delete(row));
                reload();
            }));
            context.Opening += (sender, args) => open.Enabled = delete.Enabled = selected() != null;
            grid.ContextMenuStrip = context;
            grid.MouseDown += (sender, args) =>
            {
                if (args.Button != MouseButtons.Right)
                    return;
                int row = grid.HitTest(args.X, args.Y).RowIndex;
                grid.CurrentCell = row < 0 ? null : grid.Rows[row].Cells[1];
            };
            grid.CellDoubleClick += (sender, args) => { if (args.RowIndex >= 0) Ui.Run(form, detail); };
            form.Disposed += (sender, args) => context.Dispose();
            return form;
        }

        private static void ShowDetail(Form owner, DataStore store, GiaoVien teacher)
        {
            var info = store.Read(s =>
            {
                DonVi unit = s.Find<DonVi>(row => row.Madonvi == teacher.Madonvi);
                CoSo campus = s.Find<CoSo>(row => row.Macoso == unit.Macoso);
                return new[] { teacher.Hoten, teacher.Sdt, teacher.Ghichu, unit.Tendonvi, campus.Tencoso };
            });
            using (var form = new Form())
            {
                Ui.Window(form, "Thông tin chi tiết giáo viên", 600, 360);
                var controls = Ui.Layout();
                controls.Controls.Add(Ui.Heading("Thông tin chi tiết của giáo viên"));
                string[] labels = { "Họ và tên", "Điện thoại", "Ghi chú", "Đơn vị đào tạo", "Cơ sở" };
                for (int i = 0; i < labels.Length; i++)
                {
                    var row = Ui.Actions();
                    TextBox box = Ui.TextBox(row, labels[i], 360);
                    box.Text = info[i] ?? "";
                    box.ReadOnly = true;
                    controls.Controls.Add(row);
                }
                Ui.Button(controls, "Quay về", form.Close);
                form.Controls.Add(controls);
                form.ShowDialog(owner);
            }
        }

        private sealed class TeacherRow
        {
            public int STT { get; set; }
            public GiaoVien Teacher { get; set; }
            public string Hoten => Teacher.Hoten;
        }
    }
}
