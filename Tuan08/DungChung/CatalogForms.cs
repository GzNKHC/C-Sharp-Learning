using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class CatalogOptions<T> where T : class, new()
    {
        public bool Add { get; set; } = true;
        public bool Edit { get; set; } = true;
        public bool Delete { get; set; } = true;
        public bool Navigate { get; set; }
        public bool SeparateSave { get; set; }
        public bool ConfirmChange { get; set; }
        public string[] Search { get; set; } = new string[0];
        public string Filter { get; set; }
        public string SortName { get; set; }
        public string[] Hidden { get; set; }
        public string[] Locked { get; set; }
        public Func<T> Defaults { get; set; } = () => new T();
        public Action<DataSession, T> PrepareInsert { get; set; }
        public Action<DataSession, T, T> Validate { get; set; }
    }

    internal sealed class CatalogForm<T> : Form where T : class, new()
    {
        private enum State { View, Add, Edit }
        private readonly DataStore store;
        private readonly CatalogOptions<T> options;
        private readonly DataGridView grid = Ui.Grid();
        private readonly RowEditor<T> editor;
        private readonly Button add, edit, remove, save, sort;
        private readonly Control filters;
        private readonly ListBox nameList;
        private T selected;
        private State state;
        private bool loading, ready, sorted, syncing;
        private Expression<Func<T, bool>> search;

        public CatalogForm(DataStore store, string title = null, CatalogOptions<T> options = null)
        {
            this.store = store;
            this.options = options ?? new CatalogOptions<T>();
            title = title ?? TableInfo.Title(typeof(T));
            Ui.Window(this, title);
            Ui.Columns<T>(grid);
            editor = new RowEditor<T>(store, this.options.Defaults(), this.options.Hidden, this.options.Locked);

            var actions = Ui.Actions();
            if (this.options.Navigate)
                Ui.Navigator(actions, grid);

            add = Ui.Button(actions, "Thêm", () =>
            {
                if (state != State.View && !this.options.SeparateSave)
                    Save();
                else
                    Begin(State.Add);
            }, true);
            edit = Ui.Button(actions, "Sửa", () =>
            {
                if (state != State.View)
                    Cancel();
                else
                    Begin(State.Edit);
            });
            save = Ui.Button(actions, "Lưu", Save, true);
            save.Visible = this.options.SeparateSave;
            remove = Ui.Button(actions, "Xóa", Delete);
            if (this.options.SortName != null)
                sort = Ui.Button(actions, "Sắp xếp theo tên", () => { sorted = true; Reload(); });
            Ui.Button(actions, "Thoát", Close);

            var searchBar = Ui.Actions();
            filters = searchBar;
            ComboBox group = null;
            if (this.options.Filter != null)
            {
                Field field = TableInfo.Find(typeof(T), this.options.Filter);
                group = Ui.Combo(searchBar, field.Header, 220);
                group.SelectedIndexChanged += (sender, args) =>
                {
                    if (ready && !loading)
                    {
                        search = QueryFilter.For<T>(new[] { new Criterion(field.Name, Ui.ChoiceValue(group), true) });
                        Ui.Run(this, () => Reload());
                    }
                };
            }

            if (this.options.Search.Length > 0)
            {
                Field[] fields = this.options.Search.Select(name => TableInfo.Find(typeof(T), name)).ToArray();
                var criteria = Ui.Combo(searchBar, "Tìm theo", 145);
                Ui.Choices(criteria, fields.Select(field => new Choice(field.Name, field.Header)));
                TextBox keyword = Ui.TextBox(searchBar, "Từ khóa");
                Action find = () =>
                {
                    search = QueryFilter.For<T>(new[]
                    {
                        new Criterion(Ui.Code(criteria), Ui.Require(keyword.Text, "từ khóa"), false),
                        new Criterion(this.options.Filter, group == null ? null : Ui.ChoiceValue(group), true)
                    });
                    Reload();
                };
                Ui.Button(searchBar, "Tìm kiếm", find);
                Ui.Button(searchBar, "Hiện hết", () =>
                {
                    keyword.Clear();
                    if (group != null)
                        Ui.Select(group, null);
                    search = null;
                    Reload();
                });
            }

            Control main = grid;
            if (this.options.SortName != null)
            {
                nameList = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
                nameList.SelectedIndexChanged += (sender, args) =>
                {
                    if (!syncing && nameList.SelectedItem is Choice choice)
                        Ui.SelectRow<T>(grid, row => ReferenceEquals(row, choice.Value));
                };
                main = Ui.TwoGrids("Danh sách nhân viên", nameList, "Thông tin nhân viên", grid, true);
            }

            Ui.Arrange(this, main, Ui.Heading(title), searchBar, editor.Panel, actions);
            grid.CurrentCellChanged += (sender, args) => SelectionChanged();
            FormClosing += (sender, args) =>
            {
                if (!Ui.Ask(this, state == State.View ? "Bạn có muốn thoát khỏi form?" : "Thoát và bỏ thay đổi đang nhập?"))
                    args.Cancel = true;
            };

            Load += (sender, args) => Ui.Run(this, () =>
            {
                editor.LoadChoices();
                if (group != null)
                {
                    Field field = TableInfo.Find(typeof(T), this.options.Filter);
                    Ui.Choices(group, QueryFilter.Lookup(store, field), "Tất cả");
                }
                Reload();
            });
            SetState();
        }

        private void Reload(string key = null)
        {
            List<T> items = store.Read(session => session.List(search));
            items = sorted
                ? items.OrderBy(row => LastName(TableInfo.Find(typeof(T), options.SortName).Format(
                    TableInfo.Find(typeof(T), options.SortName).Get(row))), StringComparer.CurrentCultureIgnoreCase)
                    .ThenBy(row => TableInfo.KeyText(row)).ToList()
                : items.OrderBy(TableInfo.KeyText).ToList();

            loading = true;
            try
            {
                Ui.Bind(grid, items);
                if (nameList != null)
                {
                    nameList.Items.Clear();
                    Field name = TableInfo.Find(typeof(T), options.SortName);
                    nameList.Items.AddRange(items.Select(row => (object)new Choice(row, name.Format(name.Get(row)))).ToArray());
                }
                if (key != null)
                    Ui.SelectRow<T>(grid, row => TableInfo.KeyText(row) == key);
                ready = true;
            }
            finally { loading = false; }
            SelectionChanged();
        }

        private static string LastName(string name)
        {
            int space = name.LastIndexOf(' ');
            return space < 0 ? name : name.Substring(space + 1);
        }

        private void SelectionChanged()
        {
            if (loading || state != State.View)
                return;

            selected = Ui.Selected<T>(grid);
            editor.Display(selected);
            if (nameList != null)
            {
                syncing = true;
                nameList.SelectedIndex = grid.CurrentRow?.Index ?? -1;
                syncing = false;
            }
            SetState();
        }

        private void SetState()
        {
            bool viewing = state == State.View;
            add.Visible = options.Add || !options.SeparateSave && options.Edit;
            edit.Visible = options.Edit || options.Add && !options.SeparateSave;
            remove.Visible = options.Delete;
            add.Text = options.SeparateSave ? "Nhập mới" : viewing ? "Thêm" : "Lưu";
            edit.Text = viewing ? (options.SeparateSave ? "Hiệu chỉnh" : "Sửa") : "Hủy";
            add.Enabled = ready && (!viewing && !options.SeparateSave || viewing && options.Add);
            edit.Enabled = !viewing || ready && options.Edit && selected != null;
            save.Enabled = !viewing;
            remove.Enabled = ready && viewing && selected != null;
            if (sort != null)
                sort.Enabled = ready && viewing;
            editor.Enable(!viewing, state == State.Add);
            grid.Enabled = filters.Enabled = viewing;
            if (nameList != null)
                nameList.Enabled = viewing;
        }

        private void Begin(State mode)
        {
            state = mode;
            if (mode == State.Add)
                editor.Display(options.Defaults());
            SetState();
        }

        private void Cancel()
        {
            state = State.View;
            SelectionChanged();
        }

        private void Save()
        {
            bool insert = state == State.Add;
            T old = insert ? null : selected;
            T row = editor.Read(old);
            if (options.ConfirmChange && !Ui.Ask(this, insert ? "Bạn có muốn thêm bản ghi?" : "Bạn có muốn lưu thay đổi?"))
                return;
            store.Write(session =>
            {
                if (insert)
                    options.PrepareInsert?.Invoke(session, row);
                options.Validate?.Invoke(session, row, old);
                if (insert)
                    session.Insert(row);
                else
                    session.Update(row);
            });
            state = State.View;
            Reload(TableInfo.KeyText(row));
        }

        private void Delete()
        {
            if (selected != null && Ui.Ask(this, "Xóa bản ghi " + TableInfo.KeyText(selected) + "?"))
            {
                store.Write(session => session.Delete(selected));
                Reload();
            }
        }
    }

    internal sealed class InlineCatalogForm<T> : Form where T : class, new()
    {
        private readonly DataStore store;
        private readonly DataGridView grid = Ui.Grid();
        private readonly FlowLayoutPanel navigator = Ui.Actions();
        private readonly Field[] fields = TableInfo.Fields(typeof(T));
        private readonly Button add, edit, remove;
        private DataGridViewRow draft;
        private bool adding;

        public InlineCatalogForm(DataStore store, string title)
        {
            this.store = store;
            Ui.Window(this, title, 850, 550);
            foreach (Field field in fields)
                grid.Columns.Add(field.Name, field.Header);
            Ui.Navigator(navigator, grid);
            var actions = Ui.Actions();
            add = Ui.Button(actions, "Thêm", () => { if (draft == null) Begin(true); else Save(); }, true);
            edit = Ui.Button(actions, "Sửa", () => { if (draft == null) Begin(false); else Reload(); });
            remove = Ui.Button(actions, "Xóa", Delete);
            Ui.Button(actions, "Thoát", Close);
            Ui.Arrange(this, grid, Ui.Heading(title), navigator, actions);
            grid.CellBeginEdit += (sender, args) =>
                args.Cancel = grid.Rows[args.RowIndex] != draft || !adding && fields[args.ColumnIndex].IsKey;
            grid.CurrentCellChanged += (sender, args) => UpdateButtons();
            Load += (sender, args) => Ui.Run(this, Reload);
            FormClosing += (sender, args) => args.Cancel = !Ui.Ask(this, "Bạn có muốn thoát khỏi form?");
            UpdateButtons();
        }

        private void Reload()
        {
            List<T> rows = store.Read(session => session.List<T>()).OrderBy(TableInfo.KeyText).ToList();
            grid.CancelEdit();
            draft = null;
            grid.ReadOnly = true;
            grid.Rows.Clear();
            foreach (T value in rows)
            {
                int index = grid.Rows.Add(fields.Select(field => (object)field.Format(field.Get(value))).ToArray());
                grid.Rows[index].Tag = value;
            }
            UpdateButtons();
        }

        private void Begin(bool insert)
        {
            adding = insert;
            draft = insert ? grid.Rows[grid.Rows.Add()] : grid.CurrentRow;
            if (draft == null)
                return;
            grid.ReadOnly = false;
            grid.CurrentCell = draft.Cells[insert ? 0 : Array.FindIndex(fields, field => !field.IsKey)];
            grid.Focus();
            grid.BeginEdit(true);
            UpdateButtons();
        }

        private void Save()
        {
            if (!grid.EndEdit())
                return;
            var value = new T();
            for (int i = 0; i < fields.Length; i++)
            {
                Field field = fields[i];
                field.Set(value, !adding && field.IsKey ? field.Get(draft.Tag) : field.Parse(draft.Cells[i].Value));
            }
            store.Write(session =>
            {
                if (adding)
                    session.Insert(value);
                else
                    session.Update(value);
            });
            Reload();
        }

        private void Delete()
        {
            if (grid.CurrentRow?.Tag is T row && Ui.Ask(this, "Xóa " + TableInfo.KeyText(row) + "?"))
            {
                store.Write(session => session.Delete(row));
                Reload();
            }
        }

        private void UpdateButtons()
        {
            add.Text = draft == null ? "Thêm" : "Lưu";
            edit.Text = draft == null ? "Sửa" : "Hủy";
            edit.Enabled = draft != null || grid.CurrentRow != null;
            navigator.Enabled = draft == null;
            remove.Enabled = draft == null && grid.CurrentRow?.Tag != null;
        }
    }
}
