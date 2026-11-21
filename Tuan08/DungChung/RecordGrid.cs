using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class RecordGrid<T> : Form where T : class, new()
    {
        public RecordGrid(string title, bool confirmExit = false, params string[] columns)
        {
            Ui.Window(this, title);
            Ui.Columns<T>(Grid, columns);
            Ui.Arrange(this, Grid, Ui.Heading(title), Filter, Actions);

            if (confirmExit)
                FormClosing += (sender, args) => args.Cancel = !Ui.Ask(this, "Bạn có muốn thoát khỏi form?");
        }

        public DataGridView Grid { get; } = Ui.Grid();
        public FlowLayoutPanel Filter { get; } = Ui.Actions();
        public FlowLayoutPanel Actions { get; } = Ui.Actions();
        public T Current => Ui.Selected<T>(Grid);

        public void Display(IEnumerable<T> rows, string selectedKey = null)
        {
            Ui.Bind(Grid, rows.ToList());
            if (selectedKey != null)
                Ui.SelectRow<T>(Grid, row => TableInfo.KeyText(row) == selectedKey);
        }

        public T RequireRow()
        {
            return Current ?? throw new InvalidOperationException("Hãy chọn một dòng trên lưới.");
        }
    }
}
