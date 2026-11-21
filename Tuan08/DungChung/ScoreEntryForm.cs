using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class ScoreLine
    {
        public ScoreLine(string code, object[] identity, params decimal?[] grades)
        {
            Code = code;
            Identity = identity;
            Grades = grades;
        }
        public string Code { get; }
        public object[] Identity { get; }
        public decimal?[] Grades { get; }
        public bool HasGrades => Grades.Any(grade => grade.HasValue);
    }

    internal sealed class ScoreChange
    {
        public ScoreChange(ScoreLine line, decimal[] scores) { Line = line; Scores = scores; }
        public ScoreLine Line { get; }
        public decimal[] Scores { get; }
    }

    internal sealed class ScoreEntryForm : Form
    {
        private readonly DataGridView grid = Ui.Grid();
        private readonly Button add, edit, save, cancel;
        private readonly int firstGrade;
        private bool editing, entering;
        private bool loaded;
        private readonly DataStore store;
        private readonly Func<List<ScoreLine>> load;
        private readonly Action<DataSession, ScoreChange> write;
        public FlowLayoutPanel Selectors { get; } = Ui.Actions();

        public ScoreEntryForm(string title, DataStore store, string[] identity, string[] scoreNames,
            Func<List<ScoreLine>> load, Action<DataSession, ScoreChange> write)
        {
            this.store = store;
            this.load = load;
            this.write = write;
            firstGrade = identity.Length;
            Ui.Window(this, title);
            foreach (string caption in identity.Concat(scoreNames))
                grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = caption, SortMode = DataGridViewColumnSortMode.NotSortable });
            var actions = Ui.Actions();
            add = Ui.Button(actions, "Nhập mới", () => Start(true));
            edit = Ui.Button(actions, "Hiệu chỉnh", () => Start(false));
            save = Ui.Button(actions, "Lưu", Save, true);
            save.CausesValidation = false;
            cancel = Ui.Button(actions, "Hủy", LoadSelection);
            cancel.CausesValidation = false;
            Ui.Button(actions, "Thoát", Close);
            Ui.Arrange(this, grid, Ui.Heading(title), Selectors, actions);
            grid.CellBeginEdit += (sender, args) =>
            {
                var line = (ScoreLine)grid.Rows[args.RowIndex].Tag;
                args.Cancel = !editing || args.ColumnIndex < firstGrade || line.HasGrades == entering;
            };
            FormClosing += (sender, args) =>
            {
                if (editing && !Ui.Ask(this, "Thoát và bỏ các điểm chưa lưu?"))
                    args.Cancel = true;
            };
            grid.Enabled = false;
            UpdateButtons();
        }

        public void ResetSelection()
        {
            editing = loaded = false;
            grid.Rows.Clear();
            UpdateButtons();
        }

        public void LoadSelection()
        {
            List<ScoreLine> lines = load();
            grid.CancelEdit();
            editing = false;
            loaded = true;
            grid.Rows.Clear();
            foreach (ScoreLine line in lines)
            {
                object[] numbers = line.Grades.Select(value => (object)(value?.ToString("0.##", CultureInfo.CurrentCulture) ?? "")).ToArray();
                int index = grid.Rows.Add(line.Identity.Concat(numbers).ToArray());
                grid.Rows[index].Tag = line;
            }
            UpdateButtons();
        }

        private void Start(bool isNew)
        {
            editing = true;
            entering = isNew;
            UpdateButtons();
            DataGridViewRow first = grid.Rows.Cast<DataGridViewRow>()
                .First(row => ((ScoreLine)row.Tag).HasGrades != entering);
            grid.CurrentCell = first.Cells[firstGrade];
            grid.Focus();
            grid.BeginEdit(true);
        }

        private void Save()
        {
            if (!grid.EndEdit())
                return;
            var changes = new List<ScoreChange>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                var line = (ScoreLine)row.Tag;
                if (line.HasGrades == entering)
                    continue;

                string[] cells = row.Cells.Cast<DataGridViewCell>().Skip(firstGrade)
                    .Select(cell => Convert.ToString(cell.Value, CultureInfo.CurrentCulture)?.Trim() ?? "").ToArray();
                if (entering && cells.All(string.IsNullOrEmpty))
                    continue;
                string[] previous = line.Grades.Select(grade => grade?.ToString("0.##",
                    CultureInfo.CurrentCulture) ?? "").ToArray();
                if (cells.SequenceEqual(previous))
                    continue;
                decimal[] values = cells.Select(text => ParseGrade(text, line.Code)).ToArray();
                if (!values.Select(number => (decimal?)number).SequenceEqual(line.Grades))
                    changes.Add(new ScoreChange(line, values));
            }

            if (changes.Count == 0)
                throw new InvalidOperationException("Chưa có điểm thay đổi để lưu.");

            store.Write(session =>
            {
                foreach (ScoreChange change in changes)
                    write(session, change);
            });
            editing = false;
            UpdateButtons();
            LoadSelection();
        }

        private static decimal ParseGrade(string text, string code)
        {
            if (decimal.TryParse(text, NumberStyles.AllowDecimalPoint, CultureInfo.CurrentCulture, out decimal value)
                && value >= 0 && value <= 10 && decimal.Round(value, 2) == value)
                return value;
            throw new InvalidOperationException("Điểm của " + code + " phải từ 0 đến 10 và có tối đa hai chữ số thập phân.");
        }

        private void UpdateButtons()
        {
            var rows = grid.Rows.Cast<DataGridViewRow>().Select(row => (ScoreLine)row.Tag).ToArray();
            grid.Enabled = loaded;
            grid.ReadOnly = !editing;
            Selectors.Enabled = !editing;
            add.Enabled = loaded && !editing && rows.Any(line => !line.HasGrades);
            edit.Enabled = loaded && !editing && rows.Any(line => line.HasGrades);
            save.Enabled = cancel.Enabled = editing;
        }
    }
}
