using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal static class Ui
    {
        public static void SetWindow(Form form, string title, int width, int height)
        {
            form.Text = title;
            form.ClientSize = new Size(width, height);
            form.MinimumSize = new Size(730, 460);
            form.StartPosition = FormStartPosition.CenterParent;
            form.Font = new Font("Segoe UI", 9F);
            form.Padding = new Padding(12);
            form.BackColor = SystemColors.Window;
        }

        public static DataGridView Grid()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = SystemColors.Window,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersWidth = 28,
                RowTemplate = { Height = 26 }
            };
        }

        public static TableLayoutPanel Layout(int columns)
        {
            return new TableLayoutPanel
            {
                ColumnCount = columns, Dock = DockStyle.Top,
                AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
        }

        public static TableLayoutPanel Fields()
        {
            var layout = Layout(4);
            layout.Padding = new Padding(2, 5, 2, 5);
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            return layout;
        }

        public static void Field(TableLayoutPanel layout, int index, string caption, Control control)
        {
            int column = index % 2 * 2;
            int row = index / 2;
            if (row >= layout.RowCount)
            {
                layout.RowCount = row + 1;
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }
            layout.Controls.Add(new Label
            {
                Text = caption,
                Anchor = AnchorStyles.Left,
                AutoSize = true,
                Margin = new Padding(4, 8, 8, 6)
            }, column, row);
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            control.Margin = new Padding(3, 4, 12, 4);
            layout.Controls.Add(control, column + 1, row);
        }

        public static void Arrange(Form form, DataGridView grid, params Control[] top)
        {
            var layout = Layout(1);
            layout.AutoSize = false;
            layout.Dock = DockStyle.Fill;
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowCount = top.Length + 1;
            for (int row = 0; row < top.Length; row++)
            {
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                top[row].Dock = DockStyle.Top;
                layout.Controls.Add(top[row], 0, row);
            }
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.Controls.Add(grid, 0, top.Length);
            form.Controls.Add(layout);
        }

        public static FlowLayoutPanel Actions()
        {
            return new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Dock = DockStyle.Top,
                Padding = new Padding(0, 3, 0, 6),
                WrapContents = true
            };
        }

        public static Button Button(Control container, string text, EventHandler handler)
        {
            var button = new Button { AutoSize = true, Text = text, MinimumSize = new Size(82, 28) };
            button.Click += handler;
            container.Controls.Add(button);
            return button;
        }

        public static TextBox TextBox(int maxLength = 255)
        {
            return new TextBox { MaxLength = maxLength };
        }

        public static DateTimePicker DatePicker()
        {
            return new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy",
                MinDate = new DateTime(1753, 1, 1)
            };
        }

        public static ComboBox Combo()
        {
            return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        }

        public static void Bind(ComboBox combo, DataTable table, string value, string display,
            string all = null)
        {
            if (all != null)
            {
                table = table.Copy();
                DataRow first = table.NewRow();
                first[value] = "";
                first[display] = all;
                table.Rows.InsertAt(first, 0);
            }

            combo.DataSource = null;
            combo.DisplayMember = display;
            combo.ValueMember = value;
            combo.DataSource = table;
            combo.SelectedIndex = all == null ? -1 : 0;
        }

        public static string Selected(ComboBox combo)
        {
            return combo.SelectedValue as string ?? "";
        }

        public static DataRow Current(DataGridView grid)
        {
            return (grid.CurrentRow?.DataBoundItem as DataRowView)?.Row;
        }

        public static void ClearSelection(DataGridView grid)
        {
            grid.CurrentCell = null;
            grid.ClearSelection();
        }

        public static void SelectRow(DataGridView grid, Func<DataRow, bool> test)
        {
            foreach (DataGridViewRow row in grid.Rows)
            {
                var data = row.DataBoundItem as DataRowView;
                if (data != null && test(data.Row))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Visible)
                        {
                            grid.CurrentCell = cell;
                            row.Selected = true;
                            return;
                        }
                    }
                }
            }
        }

        public static void Header(DataGridView grid, string name, string title, string format = null)
        {
            var column = grid.Columns[name];
            column.HeaderText = title;
            if (format != null)
                column.DefaultCellStyle.Format = format;
        }

        public static bool Confirm(Form form, string message)
        {
            return MessageBox.Show(form, message, "Xác nhận", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        public static void Run(Form form, Action action)
        {
            try
            {
                action();
            }
            catch (SqlException error)
            {
                string message = error.Message;
                switch (error.Number)
                {
                    case 2601:
                    case 2627:
                        message = "Mã hoặc tổ hợp mã này đã tồn tại.";
                        break;
                    case 547:
                        message = "Dữ liệu đang được sử dụng hoặc có giá trị tham chiếu chưa hợp lệ.";
                        break;
                    case -1:
                    case 2:
                    case 26:
                    case 40:
                    case 53:
                    case 4060:
                    case 18456:
                        message = "Không kết nối được SQL Server. Hãy chạy Setup.sql và kiểm tra App.config.\n\n" + message;
                        break;
                }
                MessageBox.Show(form, message, "SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception error)
            {
                MessageBox.Show(form, error.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static void Start(string title, string[] labels, params Func<Form>[] forms)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var menu = new Form())
            {
                SetWindow(menu, title, 460, 28 + labels.Length * 49);
                menu.MinimumSize = new Size(410, 180);
                menu.ClientSize = new Size(460, 28 + labels.Length * 49);
                menu.StartPosition = FormStartPosition.CenterScreen;

                var layout = Layout(1);
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                for (int i = 0; i < labels.Length; i++)
                {
                    Func<Form> create = forms[i];
                    var button = Button(layout, labels[i], (sender, args) =>
                    {
                        using (Form screen = create())
                            screen.ShowDialog(menu);
                    });
                    button.Dock = DockStyle.Top;
                    button.Height = 36;
                    button.Margin = new Padding(0, 3, 0, 7);
                }
                menu.Controls.Add(layout);
                Application.Run(menu);
            }
        }
    }
}
