using System;
using System.Data;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal abstract class EditForm : Form
    {
        protected readonly DataGridView Grid = Ui.Grid();
        protected readonly TableLayoutPanel Fields = Ui.Fields();
        protected readonly TextBox Code = Ui.TextBox(20);
        protected readonly TextBox RecordName = Ui.TextBox(100);
        private readonly Button add, edit, delete, save, cancel;
        private readonly string codeCaption, nameCaption;
        private bool loading;
        private bool ready;
        private bool editing;
        private bool adding;
        protected DataRow SelectedRow { get; private set; }

        protected EditForm(string title, string codeCaption, string nameCaption, bool print)
        {
            this.codeCaption = codeCaption;
            this.nameCaption = nameCaption;
            Ui.SetWindow(this, title, 930, 540);
            Ui.Field(Fields, 0, codeCaption, Code);
            Ui.Field(Fields, 1, nameCaption, RecordName);
            Toolbar = Ui.Actions();
            add = Ui.Button(Toolbar, "Thêm", (sender, args) => Ui.Run(this, () => BeginEdit(true)));
            delete = Ui.Button(Toolbar, "Xóa", (sender, args) => Ui.Run(this, Delete));
            edit = Ui.Button(Toolbar, "Sửa", (sender, args) => Ui.Run(this, () => BeginEdit(false)));
            save = Ui.Button(Toolbar, "Lưu", (sender, args) => Ui.Run(this, Save));
            cancel = Ui.Button(Toolbar, "Bỏ qua", (sender, args) => EndEdit());
            if (print)
                Ui.Button(Toolbar, "Xem in", (sender, args) => Ui.Run(this, () => PrintGrid.Show(this, Grid, title)));
            Ui.Button(Toolbar, "Đóng", (sender, args) => Close());
            Grid.SelectionChanged += (sender, args) =>
            {
                if (!loading && !editing)
                    DisplaySelection();
            };
            Load += (sender, args) => Ui.Run(this, () =>
            {
                SetState();
                InitializeChoices();
                Reload();
            });
        }

        protected FlowLayoutPanel Toolbar { get; }
        protected abstract string CodeName { get; }
        protected abstract string NameColumn { get; }
        protected virtual void InitializeChoices() { }
        protected virtual void ShowOtherFields(DataRow row) { }
        protected virtual void EnableOtherFields(bool editing) { }
        protected virtual void FormatOtherColumns() { }
        protected abstract DataTable ReadRows();
        protected abstract void Store(string code, string name, bool insert);
        protected abstract void Remove(string code);

        protected void Reload()
        {
            DataTable rows = ReadRows();
            loading = true;
            try
            {
                Grid.DataSource = rows;
                Grid.Columns[CodeName].HeaderText = codeCaption;
                Grid.Columns[NameColumn].HeaderText = nameCaption;
                FormatOtherColumns();
                Ui.ClearSelection(Grid);
                ready = true;
            }
            finally
            {
                loading = false;
            }
            DisplaySelection();
        }

        private void DisplaySelection()
        {
            SelectedRow = Ui.Current(Grid);
            Code.Text = SelectedRow == null ? "" : Db.Value(SelectedRow[CodeName]);
            RecordName.Text = SelectedRow == null ? "" : Db.Value(SelectedRow[NameColumn]);
            ShowOtherFields(SelectedRow);
            SetState();
        }

        private void SetState()
        {
            add.Enabled = ready && !editing;
            edit.Enabled = delete.Enabled = SelectedRow != null && !editing;
            save.Enabled = cancel.Enabled = editing;
            Code.Enabled = editing && adding;
            RecordName.Enabled = editing;
            Grid.Enabled = !editing;
            EnableOtherFields(editing);
        }

        private void BeginEdit(bool insert)
        {
            if (insert)
            {
                Ui.ClearSelection(Grid);
                DisplaySelection();
            }
            editing = true;
            adding = insert;
            SetState();
            if (insert)
                Code.Focus();
            else
                RecordName.Focus();
        }

        private void EndEdit()
        {
            editing = adding = false;
            DisplaySelection();
        }

        private void Save()
        {
            string code = Code.Text.Trim();
            string name = RecordName.Text.Trim();
            if (code.Length == 0 || name.Length == 0)
                throw new InvalidOperationException("Vui lòng nhập đủ mã và tên.");

            Store(code, name, adding);
            editing = adding = false;
            Ui.ClearSelection(Grid);
            DisplaySelection();
            MessageBox.Show(this, "Lưu thành công.", "Thông báo");
            Reload();
        }

        private void Delete()
        {
            string code = Db.Value(SelectedRow[CodeName]);
            if (!Ui.Confirm(this, "Bạn có muốn xóa " + code + "?"))
                return;

            Remove(code);
            Ui.ClearSelection(Grid);
            DisplaySelection();
            MessageBox.Show(this, "Xóa thành công.", "Thông báo");
            Reload();
        }
    }
}
