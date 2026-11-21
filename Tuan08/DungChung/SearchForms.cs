using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class Criterion
    {
        public Criterion(string column, object value, bool exact) { Column = column; Value = value; Exact = exact; }
        public string Column { get; }
        public object Value { get; }
        public bool Exact { get; }
    }

    internal static class QueryFilter
    {
        public static Expression<Func<T, bool>> For<T>(IEnumerable<Criterion> filters)
        {
            var row = Expression.Parameter(typeof(T), "row");
            Expression result = null;
            foreach (Criterion filter in filters)
            {
                if (filter.Value == null || string.IsNullOrEmpty(filter.Value as string) && filter.Value is string)
                    continue;
                Field field = TableInfo.Find(typeof(T), filter.Column);
                Expression member = Expression.Property(row, field.Property);
                Expression value = Expression.Constant(filter.Value, field.Type);
                if (field.Type != field.Property.PropertyType)
                    value = Expression.Convert(value, field.Property.PropertyType);
                Expression match = !filter.Exact && field.Type == typeof(string)
                    ? Expression.AndAlso(Expression.NotEqual(member, Expression.Constant(null, typeof(string))),
                        Expression.Call(member, nameof(string.Contains), Type.EmptyTypes, value))
                    : Expression.Equal(member, value);
                result = result == null ? match : Expression.AndAlso(result, match);
            }
            return result == null ? null : Expression.Lambda<Func<T, bool>>(result, row);
        }

        public static IEnumerable<Choice> Lookup(DataStore store, Field field)
        {
            Type parent = field.Input.Lookup;
            Field name = TableInfo.Find(parent, field.Input.LookupLabel);
            Field key = TableInfo.Fields(parent).Single(item => item.IsKey);
            return store.Read(s => s.List(parent)).OrderBy(row => name.Format(name.Get(row))).Select(row =>
                new Choice(key.Get(row), key.Format(key.Get(row)) + " – " + name.Format(name.Get(row))));
        }
    }

    internal sealed class SearchForm<T> : Form where T : class, new()
    {
        private readonly DataStore store;
        private readonly DataGridView grid = Ui.Grid();
        private readonly (Field Field, Control Control)[] inputs;
        private readonly bool selectOne;

        public SearchForm(DataStore store, string title, bool selectOne, bool comboChoices, params string[] fields)
        {
            this.store = store;
            this.selectOne = selectOne;
            Ui.Window(this, title);
            Ui.Columns<T>(grid);
            var search = Ui.Actions();
            inputs = fields.Select(name =>
            {
                Field field = TableInfo.Find(typeof(T), name);
                Control control = comboChoices ? (Control)Ui.Combo(search, field.Header, 200) : Ui.TextBox(search, field.Header);
                return (field, control);
            }).ToArray();

            if (selectOne)
            {
                var choice = Ui.Combo(search, "Tiêu chí", 142);
                Ui.Choices(choice, inputs.Select(input => new Choice(input.Field.Name, input.Field.Header)));
                Action setVisibility = () =>
                {
                    foreach (var input in inputs)
                    {
                        bool visible = input.Field.Name == Ui.Code(choice);
                        input.Control.Visible = visible;
                        search.Controls[search.Controls.GetChildIndex(input.Control) - 1].Visible = visible;
                    }
                };
                choice.SelectedIndexChanged += (sender, args) => setVisibility();
                Shown += (sender, args) => setVisibility();
            }

            Ui.Button(search, "Tìm", () => Reload(true), true);
            Ui.Button(search, "Hiện hết", () => Reload(false));
            Ui.Button(search, "Thoát", Close);
            Ui.Arrange(this, grid, Ui.Heading(title), search);
            Load += (sender, args) => Ui.Run(this, () => Reload(false));
        }

        private void Reload(bool filter)
        {
            IEnumerable<Criterion> conditions = inputs.Where(input => !selectOne || input.Control.Visible)
                .Select(input => new Criterion(input.Field.Name, input.Control is ComboBox combo
                    ? Ui.ChoiceValue(combo) : input.Control.Text.Trim(), input.Control is ComboBox));
            List<T> rows = store.Read(s => s.List(filter ? QueryFilter.For<T>(conditions) : null));
            Ui.Bind(grid, rows.OrderBy(TableInfo.KeyText).ToList());

            if (!filter)
            {
                foreach (var input in inputs)
                {
                    if (input.Control is ComboBox combo)
                    {
                        var values = rows.Select(input.Field.Get).Where(value => value != null)
                            .Distinct().OrderBy(input.Field.Format).Select(value => new Choice(value, input.Field.Format(value)));
                        Ui.Choices(combo, values, "Tất cả");
                    }
                    else
                        input.Control.Text = "";
                }
            }
        }
    }

    internal sealed class RelationForm<TParent, TChild> : Form
        where TParent : class, new() where TChild : class, new()
    {
        private readonly DataStore store;
        private readonly Func<DataSession, TParent, List<TChild>> children;
        private bool loading;
        public DataGridView Parents { get; } = Ui.Grid();
        public DataGridView Children { get; } = Ui.Grid();
        public FlowLayoutPanel ControlsBar { get; } = Ui.Actions();
        public FlowLayoutPanel Actions { get; } = Ui.Actions();
        public TParent ParentRow => Ui.Selected<TParent>(Parents);
        public TChild ChildRow => Ui.Selected<TChild>(Children);
        public Action<TParent> SelectedParentChanged { get; set; }

        public RelationForm(DataStore store, string title, string parentsLabel, string childrenLabel,
            Func<DataSession, TParent, List<TChild>> children)
        {
            this.store = store;
            this.children = children;
            Ui.Window(this, title);
            Ui.Columns<TParent>(Parents);
            Ui.Columns<TChild>(Children);
            Ui.Arrange(this, Ui.TwoGrids(parentsLabel, Parents, childrenLabel, Children), Ui.Heading(title), ControlsBar, Actions);
            Parents.CurrentCellChanged += (sender, args) =>
            {
                if (!loading)
                    Ui.Run(this, ReloadChildren);
            };
        }

        public void ReloadParents(Expression<Func<TParent, bool>> predicate = null, string selectedKey = null)
        {
            loading = true;
            try
            {
                Ui.Bind(Parents, store.Read(s => s.List(predicate)).OrderBy(TableInfo.KeyText).ToList());
                if (selectedKey != null)
                    Ui.SelectRow<TParent>(Parents, row => TableInfo.KeyText(row) == selectedKey);
            }
            finally { loading = false; }
            ReloadChildren();
        }

        public void ReloadChildren()
        {
            TParent current = ParentRow;
            List<TChild> rows = current == null ? new List<TChild>() : store.Read(s => children(s, current));
            Ui.Bind(Children, rows.OrderBy(TableInfo.KeyText).ToList());
            SelectedParentChanged?.Invoke(current);
        }

        public void BindParentCombo(ComboBox combo, string selectedKey = null)
        {
            string old = selectedKey ?? (ParentRow == null ? "" : TableInfo.KeyText(ParentRow));
            var records = store.Read(s => s.List<TParent>()).OrderBy(TableInfo.KeyText);
            Ui.Choices(combo, records.Select(row => new Choice(TableInfo.KeyText(row), TableInfo.KeyText(row))), first: false);
            Ui.Select(combo, old);
        }
    }
}
