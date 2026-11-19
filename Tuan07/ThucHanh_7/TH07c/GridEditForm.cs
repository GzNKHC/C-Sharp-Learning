using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal sealed class GridEditForm : Form
    {
        private enum Mode { View, Add, Edit }

        private readonly GridStore store;
        private readonly BindingSource binding = new BindingSource();
        private readonly DataGridView grid = Ui.Grid();
        private readonly Button add, edit, delete, save, cancel, print;
        private readonly Label instructions = new Label
        {
            AutoSize = true, ForeColor = SystemColors.GrayText, Padding = new Padding(4, 3, 4, 6)
        };
        private DataTable data;
        private Mode mode;
        private bool loading;

        public GridEditForm(string title, GridStore store)
        {
            this.store = store;
            Ui.SetWindow(this, title, 915, 550);
            grid.AutoGenerateColumns = false;
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            var toolbar = Ui.Actions();
            add = Ui.Button(toolbar, "Thêm", (sender, args) => Begin(Mode.Add));
            delete = Ui.Button(toolbar, "Xóa", (sender, args) => Ui.Run(this, Delete));
            edit = Ui.Button(toolbar, "Sửa", (sender, args) => Begin(Mode.Edit));
            save = Ui.Button(toolbar, "Lưu", (sender, args) => Ui.Run(this, Save));
            cancel = Ui.Button(toolbar, "Bỏ qua", (sender, args) => CancelEdits());
            cancel.CausesValidation = false;
            print = Ui.Button(toolbar, "Xem in", (sender, args) => Ui.Run(this, () => PrintGrid.Show(this, grid, title)));
            Ui.Button(toolbar, "Đóng", (sender, args) => Close()).CausesValidation = false;

            var fields = Ui.Fields();
            for (int i = 0; i < store.Fields.Length; i++)
            {
                GridField field = store.Fields[i];
                Control detail = field.LookupTable == null ? (Control)Ui.TextBox() : Ui.Combo();
                detail.Enabled = false;
                detail.Name = "view" + field.Name;
                Ui.Field(fields, i, field.Caption, detail);
            }
            Ui.Arrange(this, grid, toolbar, fields, instructions);

            grid.CurrentCellChanged += (sender, args) => UpdateState();
            grid.SelectionChanged += (sender, args) => UpdateState();
            grid.CellBeginEdit += CheckEdit;
            grid.DataError += (sender, args) =>
            {
                args.ThrowException = false;
                args.Cancel = true;
                if ((args.Context & (DataGridViewDataErrorContexts.Parsing |
                    DataGridViewDataErrorContexts.Commit)) != 0)
                {
                    string caption = args.ColumnIndex >= 0 ? grid.Columns[args.ColumnIndex].HeaderText : "dữ liệu";
                    MessageBox.Show(this, "Giá trị không hợp lệ ở cột " + caption +
                        ". Ngày sinh nhập theo dd/MM/yyyy, điểm thập phân dùng dấu phẩy.",
                        "Kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
            FormClosing += (sender, args) =>
            {
                if (mode != Mode.View && !Ui.Confirm(this, "Đóng và bỏ các thay đổi chưa lưu?"))
                    args.Cancel = true;
            };
            Load += (sender, args) => Ui.Run(this, () => InitializeData(fields));
            UpdateState();
        }

        private void InitializeData(TableLayoutPanel fields)
        {
            data = store.ReadRows();
            binding.DataSource = data;
            foreach (GridField field in store.Fields)
            {
                Control detail = fields.Controls["view" + field.Name];
                DataGridViewColumn column;
                if (detail is ComboBox combo)
                {
                    DataTable choices = store.ReadChoices(field);
                    Ui.Bind(combo, choices, field.Name, field.LookupCaption);
                    combo.DataBindings.Add("SelectedValue", binding, field.Name, true, DataSourceUpdateMode.Never);
                    column = new DataGridViewComboBoxColumn
                    {
                        DataSource = choices.Copy(),
                        DisplayMember = field.LookupCaption,
                        ValueMember = field.Name,
                        DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing,
                        FlatStyle = FlatStyle.Flat
                    };
                }
                else
                {
                    detail.DataBindings.Add("Text", binding, field.Name, true, DataSourceUpdateMode.Never,
                        "", field.Format);
                    column = new DataGridViewTextBoxColumn
                    {
                        MaxInputLength = field.MaxLength > 0 ? field.MaxLength : 20
                    };
                }
                column.Name = field.Name;
                column.HeaderText = field.Caption;
                column.DataPropertyName = field.Name;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
                column.DefaultCellStyle.Format = field.Format ?? "";
                grid.Columns.Add(column);
            }
            grid.DataSource = binding;
            Ui.ClearSelection(grid);
            UpdateState();
        }

        private void Reload()
        {
            loading = true;
            try
            {
                data = store.ReadRows();
                binding.DataSource = data;
                Ui.ClearSelection(grid);
            }
            finally
            {
                loading = false;
            }
            UpdateState();
        }

        private void Begin(Mode next)
        {
            mode = next;
            grid.ReadOnly = false;
            grid.AllowUserToAddRows = mode == Mode.Add;
            foreach (GridField field in store.Fields)
                grid.Columns[field.Name].ReadOnly = mode == Mode.Edit && field.PrimaryKey;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            if (mode == Mode.Add)
                grid.CurrentCell = grid.Rows[grid.NewRowIndex].Cells[0];
            else if (grid.CurrentRow != null)
            {
                GridField firstEditable = store.Fields.First(field => !field.PrimaryKey);
                grid.CurrentCell = grid.CurrentRow.Cells[firstEditable.Name];
            }
            grid.Focus();
            UpdateState();
        }

        private void CheckEdit(object sender, DataGridViewCellCancelEventArgs args)
        {
            DataGridViewRow row = grid.Rows[args.RowIndex];
            var dataRow = row.DataBoundItem as DataRowView;
            bool newRow = row.IsNewRow || (dataRow != null && dataRow.IsNew) ||
                (dataRow != null && dataRow.Row.RowState == DataRowState.Added);
            args.Cancel = mode == Mode.View || (mode == Mode.Add && !newRow) ||
                (mode == Mode.Edit && newRow);
        }

        private void UpdateState()
        {
            if (loading)
                return;
            bool viewing = mode == Mode.View;
            bool hasRow = Ui.Current(grid) != null;
            add.Enabled = viewing && data != null;
            edit.Enabled = delete.Enabled = viewing && hasRow;
            print.Enabled = viewing && data != null;
            save.Enabled = cancel.Enabled = !viewing;
            switch (mode)
            {
                case Mode.Add:
                    instructions.Text = "Nhập ở dòng mới cuối bảng. Nhấn F2 hoặc nhấp đúp để sửa ô.";
                    break;
                case Mode.Edit:
                    instructions.Text = "Nhấn F2 hoặc nhấp đúp để sửa ô. Cột khóa được giữ nguyên.";
                    break;
                default:
                    instructions.Text = "Chọn một dòng để xem thông tin hoặc dùng Thêm để nhập dòng mới.";
                    break;
            }
        }

        private void EndMode()
        {
            mode = Mode.View;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            UpdateState();
        }

        private void CancelEdits()
        {
            grid.CancelEdit();
            binding.CancelEdit();
            data.RejectChanges();
            EndMode();
        }

        private void Save()
        {
            if (!grid.EndEdit())
                return;

            binding.EndEdit();
            DataRow[] changed = data.Rows.Cast<DataRow>().Where(row =>
                row.RowState == DataRowState.Added || row.RowState == DataRowState.Modified).ToArray();
            if (changed.Length == 0)
                throw new InvalidOperationException("Chưa có dòng nào thay đổi để lưu.");

            store.Save(changed);
            data.AcceptChanges();
            EndMode();
            MessageBox.Show(this, "Đã lưu " + changed.Length + " dòng.", "Thông báo");
            Reload();
        }

        private void Delete()
        {
            DataRow row = Ui.Current(grid);
            if (row == null || !Ui.Confirm(this, "Bạn có muốn xóa dòng đang chọn?"))
                return;

            store.Delete(row);
            MessageBox.Show(this, "Xóa thành công.", "Thông báo");
            Reload();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                binding.Dispose();
            base.Dispose(disposing);
        }
    }
}
