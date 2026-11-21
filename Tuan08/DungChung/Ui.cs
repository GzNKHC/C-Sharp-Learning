using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class Choice
    {
        public Choice(object value, string text) { Value = value; Text = text; }
        public object Value { get; }
        public string Text { get; }
        public override string ToString() => Text;
    }

    internal static class Ui
    {
        public static void Window(Form form, string title, int width = 1100, int height = 680)
        {
            form.Text = title;
            form.Font = new Font("Segoe UI", 9F);
            form.BackColor = Color.White;
            form.Padding = new Padding(14);
            form.AutoScaleMode = AutoScaleMode.Font;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimumSize = new Size(Math.Min(width, 790), Math.Min(height, 480));
            form.ClientSize = new Size(width, height);
        }

        public static TableLayoutPanel Layout(int columns = 1)
        {
            return new TableLayoutPanel
            {
                ColumnCount = columns, Dock = DockStyle.Top, AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink, Margin = new Padding(0)
            };
        }

        public static void Arrange(Form form, Control content, params Control[] top)
        {
            var layout = Layout();
            layout.AutoSize = false;
            layout.Dock = DockStyle.Fill;
            layout.RowCount = top.Length + 1;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < top.Length; i++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                top[i].Dock = DockStyle.Top;
                layout.Controls.Add(top[i], 0, i);
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            content.Dock = DockStyle.Fill;
            layout.Controls.Add(content, 0, top.Length);
            form.Controls.Add(layout);
        }

        public static Label Heading(string text)
        {
            return new Label
            {
                Text = text, AutoSize = true, ForeColor = Color.FromArgb(25, 64, 108),
                Font = new Font("Segoe UI Semibold", 14F), Padding = new Padding(0, 0, 0, 8)
            };
        }

        public static FlowLayoutPanel Actions()
        {
            return new FlowLayoutPanel
            {
                AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink, Dock = DockStyle.Top,
                Padding = new Padding(0, 4, 0, 5), Margin = new Padding(0), WrapContents = true
            };
        }

        public static Button Button(Control parent, string text, Action action, bool primary = false)
        {
            var button = new Button
            {
                Text = text, AutoSize = true, MinimumSize = new Size(84, 29),
                Margin = new Padding(0, 3, 8, 3)
            };
            if (primary)
            {
                button.BackColor = Color.FromArgb(28, 81, 135);
                button.ForeColor = Color.White;
                button.FlatStyle = FlatStyle.Flat;
            }
            button.Click += (sender, args) => Run(button.FindForm(), action);
            parent.Controls.Add(button);
            return button;
        }

        public static void Label(FlowLayoutPanel parent, string caption)
        {
            parent.Controls.Add(new Label { Text = caption, AutoSize = true, Margin = new Padding(3, 10, 8, 3) });
        }

        public static TextBox TextBox(FlowLayoutPanel parent, string label, int width = 170)
        {
            Label(parent, label);
            var control = new TextBox { Width = width, Margin = new Padding(0, 6, 12, 4) };
            parent.Controls.Add(control);
            return control;
        }

        public static ComboBox Combo(FlowLayoutPanel parent = null, string label = null, int width = 190)
        {
            var combo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList, Width = width, Margin = new Padding(0, 6, 12, 4)
            };
            if (parent != null)
            {
                Label(parent, label);
                parent.Controls.Add(combo);
            }
            return combo;
        }

        public static DateTimePicker Date(bool time = false)
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom, CustomFormat = time ? "dd/MM/yyyy HH:mm:ss" : "dd/MM/yyyy",
                MinDate = new DateTime(1900, 1, 1)
            };
        }

        public static void Choices(ComboBox combo, IEnumerable<Choice> choices, string all = null, bool first = true)
        {
            combo.BeginUpdate();
            combo.Items.Clear();
            if (all != null)
                combo.Items.Add(new Choice(null, all));
            combo.Items.AddRange(choices.Cast<object>().ToArray());
            combo.SelectedIndex = first && combo.Items.Count > 0 ? 0 : -1;
            combo.EndUpdate();
        }

        public static object ChoiceValue(ComboBox combo) => (combo.SelectedItem as Choice)?.Value;

        public static string Code(ComboBox combo) => ChoiceValue(combo) as string ?? "";

        public static void Select(ComboBox combo, object value)
        {
            combo.SelectedIndex = -1;
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (Equals(((Choice)combo.Items[i]).Value, value))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }
        }

        public static DataGridView Grid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill, BackgroundColor = Color.White,
                AutoGenerateColumns = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                RowHeadersWidth = 24, RowTemplate = { Height = 26 },
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(245, 248, 252) }
            };
        }

        public static void Columns<T>(DataGridView grid, params string[] names)
        {
            Field[] fields = TableInfo.Fields(typeof(T));
            foreach (Field field in names.Length == 0 ? fields : names.Select(name => TableInfo.Find(typeof(T), name)))
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = field.Name, DataPropertyName = field.Name, HeaderText = field.Header,
                    MinimumWidth = 62, SortMode = DataGridViewColumnSortMode.NotSortable
                };
                if (field.Type == typeof(DateTime))
                    column.DefaultCellStyle.Format = field.IsDateTime ? "dd/MM/yyyy HH:mm:ss" : "dd/MM/yyyy";
                else if (field.Type == typeof(decimal))
                    column.DefaultCellStyle.Format = "0.##";
                grid.Columns.Add(column);
            }
        }

        public static void Columns(DataGridView grid, params string[] paired)
        {
            for (int i = 0; i < paired.Length; i += 2)
            {
                var column = new DataGridViewTextBoxColumn
                {
                    Name = paired[i], DataPropertyName = paired[i], HeaderText = paired[i + 1],
                    MinimumWidth = 67, SortMode = DataGridViewColumnSortMode.NotSortable
                };
                if (paired[i] == "NgaySinh" || paired[i].StartsWith("Ngay", StringComparison.Ordinal))
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                grid.Columns.Add(column);
            }
        }

        public static void Bind(DataGridView grid, IList values) { grid.DataSource = values; }

        public static T Selected<T>(DataGridView grid) where T : class => grid.CurrentRow?.DataBoundItem as T;

        public static void SelectRow<T>(DataGridView grid, Func<T, bool> matches) where T : class
        {
            foreach (DataGridViewRow row in grid.Rows)
                if (row.DataBoundItem is T value && matches(value))
                {
                    grid.ClearSelection();
                    grid.CurrentCell = row.Cells.Cast<DataGridViewCell>().First(cell => cell.Visible);
                    row.Selected = true;
                    return;
                }
        }

        public static void Navigator(FlowLayoutPanel actions, DataGridView grid)
        {
            Action<int> go = index =>
            {
                if (grid.Rows.Count == 0)
                    return;
                index = Math.Max(0, Math.Min(grid.Rows.Count - 1, index));
                grid.CurrentCell = grid.Rows[index].Cells.Cast<DataGridViewCell>().First(cell => cell.Visible);
            };

            Button(actions, "Về đầu", () => go(0));
            Button(actions, "Trước", () => go((grid.CurrentRow?.Index ?? 0) - 1));
            Button(actions, "Tiếp", () => go((grid.CurrentRow?.Index ?? -1) + 1));
            Button(actions, "Về cuối", () => go(grid.Rows.Count - 1));
        }

        public static Control TwoGrids(string firstTitle, Control first, string secondTitle, Control second, bool side = false)
        {
            var split = new SplitContainer
            {
                Orientation = side ? Orientation.Vertical : Orientation.Horizontal,
                Dock = DockStyle.Fill, BorderStyle = BorderStyle.FixedSingle, Margin = new Padding(0, 8, 0, 0),
                SplitterWidth = 6
            };
            bool sized = false;
            split.SizeChanged += (sender, args) =>
            {
                int extent = side ? split.ClientSize.Width : split.ClientSize.Height;
                if (!sized && extent >= 300)
                {
                    split.SplitterDistance = side && first is ListBox ? extent / 4 : extent / 2;
                    sized = true;
                }
            };
            AddGridCaption(split.Panel1, firstTitle, first);
            AddGridCaption(split.Panel2, secondTitle, second);
            return split;
        }

        private static void AddGridCaption(Control target, string title, Control content)
        {
            content.Dock = DockStyle.Fill;
            target.Controls.Add(content);
            target.Controls.Add(new Label
            {
                Text = title, Dock = DockStyle.Top, Height = 26, Padding = new Padding(6, 4, 0, 0),
                ForeColor = Color.FromArgb(25, 64, 108), BackColor = Color.FromArgb(234, 242, 250)
            });
        }

        public static bool Ask(Form form, string message)
        {
            return MessageBox.Show(form, message, "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public static string Require(string value, string label)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException("Chưa nhập hoặc chọn " + label + ".");
            return value.Trim();
        }

        public static void Run(Form form, Action action)
        {
            try
            {
                action();
            }
            catch (Exception error)
            {
                SqlException sql = null;
                for (Exception current = error; current != null; current = current.InnerException)
                    if (current is SqlException found)
                    {
                        sql = found;
                        break;
                    }

                string message = error.GetBaseException().Message;
                if (sql != null)
                {
                    switch (sql.Number)
                    {
                        case 2601:
                        case 2627: message = "Mã hoặc tổ hợp mã đã tồn tại."; break;
                        case 547: message = "Dữ liệu đang được sử dụng hoặc vi phạm ràng buộc."; break;
                        case 1205: message = "Dữ liệu đang được cập nhật ở cửa sổ khác. Hãy thử lại."; break;
                        case 2:
                        case 26:
                        case 53:
                        case 4060:
                        case 18456:
                            message = "Kiểm tra kết nối trong App.config và chạy Setup.sql.\n\n" + sql.Message;
                            break;
                    }
                }
                MessageBox.Show(form, message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void Start(string title, params Entry[] entries)
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var menu = new Form())
            {
                Window(menu, title, 580, 135 + entries.Length * 42);
                menu.StartPosition = FormStartPosition.CenterScreen;
                var controls = Layout();
                controls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                controls.Controls.Add(Heading(title));
                var top = Actions();
                controls.Controls.Add(top);
                ComboBox method = Combo(top, "Cách kết nối dữ liệu", 210);
                Choices(method, new[]
                {
                    new Choice(DataMethod.AdoNet, "ADO.NET"), new Choice(DataMethod.LinqToSql, "LINQ to SQL"),
                    new Choice(DataMethod.EntityFramework, "Entity Framework (EDM)")
                }, first: false);
                method.SelectedIndex = 1;
                foreach (Entry entry in entries)
                {
                    Button button = Button(controls, entry.Caption, () =>
                    {
                        var store = new DataStore((DataMethod)ChoiceValue(method));
                        using (Form screen = entry.Open(store))
                            screen.ShowDialog(menu);
                    });
                    button.Dock = DockStyle.Top;
                    button.Height = 34;
                }
                menu.Controls.Add(controls);
                Application.Run(menu);
            }
        }
    }

    internal sealed class Entry
    {
        public Entry(string caption, Func<DataStore, Form> open) { Caption = caption; Open = open; }
        public string Caption { get; }
        public Func<DataStore, Form> Open { get; }
    }
}
