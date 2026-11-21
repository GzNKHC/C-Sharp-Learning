using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh9
{
    internal sealed class RowEditor<T> where T : class, new()
    {
        private readonly Dictionary<Field, Control> controls = new Dictionary<Field, Control>();
        private readonly DataStore store;
        private readonly T defaults;
        private readonly HashSet<string> locked;
        private Button chooseImage;
        public TableLayoutPanel Panel { get; }

        public RowEditor(DataStore store, T defaults = null, string[] hidden = null, string[] locked = null)
        {
            this.store = store;
            this.defaults = defaults;
            this.locked = new HashSet<string>(locked ?? new string[0]);
            Panel = Ui.Layout(4);
            Panel.Padding = new Padding(0, 0, 0, 5);
            Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            Panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            Panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            foreach (Field field in TableInfo.Fields(typeof(T)))
            {
                if (hidden != null && hidden.Contains(field.Name))
                    continue;
                Control editor = ControlFor(field);
                int column = controls.Count % 2 * 2;
                int row = controls.Count / 2;
                Panel.RowCount = row + 1;
                var label = new Label
                {
                    Text = field.Header, AutoSize = true, Anchor = AnchorStyles.Left,
                    Margin = new Padding(2, 6, 7, 5)
                };
                editor.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                editor.Margin = new Padding(0, 4, 15, 4);
                Panel.Controls.Add(label, column, row);
                Control container = field.Name == "HinhAnh" ? ImageInput((TextBox)editor) : editor;
                Panel.Controls.Add(container, column + 1, row);
                controls.Add(field, editor);
            }
        }

        private Control ImageInput(TextBox path)
        {
            var panel = Ui.Actions();
            path.Width = 178;
            panel.Controls.Add(path);
            chooseImage = Ui.Button(panel, "Chọn ảnh", () =>
            {
                using (var dialog = new OpenFileDialog
                {
                    Filter = "Ảnh (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
                    CheckFileExists = true
                })
                {
                    if (dialog.ShowDialog(panel.FindForm()) == DialogResult.OK)
                        path.Text = dialog.FileName;
                }
            });
            var preview = new PictureBox { Size = new Size(64, 54), SizeMode = PictureBoxSizeMode.Zoom };
            panel.Controls.Add(preview);
            path.TextChanged += (sender, args) =>
            {
                preview.Image?.Dispose();
                preview.Image = null;
                try
                {
                    if (File.Exists(path.Text))
                        using (Image source = Image.FromFile(path.Text))
                            preview.Image = new Bitmap(source);
                }
                catch (ArgumentException) { }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            };
            preview.Disposed += (sender, args) => preview.Image?.Dispose();
            return panel;
        }

        private static Control ControlFor(Field field)
        {
            if (field.Input.Lookup != null)
                return Ui.Combo();
            if (field.Name == "GT" || field.Name == "GioiTinh")
            {
                var choices = Ui.Combo();
                Ui.Choices(choices, new[] { new Choice("Nam", "Nam"), new Choice("Nữ", "Nữ") });
                return choices;
            }
            if (field.Name == "TinhTrang")
            {
                var choices = Ui.Combo();
                Ui.Choices(choices, new[] { new Choice("Tốt", "Tốt"), new Choice("Hỏng", "Hỏng") });
                return choices;
            }
            if (field.Type == typeof(DateTime))
            {
                DateTimePicker date = Ui.Date(field.IsDateTime);
                date.ShowCheckBox = field.Optional;
                return date;
            }
            return new TextBox { MaxLength = field.Type == typeof(string) ? field.Size : 24 };
        }

        public void LoadChoices()
        {
            var perTable = new Dictionary<Type, List<object>>();
            foreach (var pair in controls.Where(item => item.Key.Input.Lookup != null))
            {
                Field field = pair.Key;
                Type type = field.Input.Lookup;
                if (!perTable.TryGetValue(type, out List<object> rows))
                {
                    rows = store.Read(session => session.List(type));
                    perTable.Add(type, rows);
                }

                Field key = TableInfo.Fields(type).Single(column => column.IsKey);
                Field label = TableInfo.Find(type, field.Input.LookupLabel);
                var choices = rows.OrderBy(label.Get).ThenBy(key.Get).Select(row =>
                {
                    string code = key.Format(key.Get(row));
                    string description = label.Format(label.Get(row));
                    return new Choice(key.Get(row), key.Name == label.Name ? code : code + " – " + description);
                });
                Ui.Choices((ComboBox)pair.Value, choices, field.Optional ? "Không chọn" : null, false);
            }
        }

        public void Display(T row)
        {
            T source = row ?? defaults;
            foreach (var pair in controls)
            {
                object value = source == null ? null : pair.Key.Get(source);
                if (pair.Value is ComboBox combo)
                    Ui.Select(combo, value);
                else if (pair.Value is DateTimePicker date)
                {
                    DateTime actual = value is DateTime day && day >= date.MinDate ? day : DateTime.Today;
                    date.Value = actual;
                    if (date.ShowCheckBox)
                        date.Checked = value != null;
                }
                else
                    pair.Value.Text = pair.Key.Format(value);
            }
        }

        public void Enable(bool editing, bool adding)
        {
            foreach (var pair in controls)
                pair.Value.Enabled = editing && (adding || !pair.Key.IsKey) && !locked.Contains(pair.Key.Name);
            if (chooseImage != null)
                chooseImage.Enabled = editing && !locked.Contains("HinhAnh");
        }

        public T Read(T original = null)
        {
            var output = new T();
            T source = original ?? defaults;
            foreach (Field field in TableInfo.Fields(typeof(T)))
            {
                if ((original != null && field.IsKey) || locked.Contains(field.Name) || !controls.ContainsKey(field))
                {
                    if (source != null)
                        field.Set(output, field.Get(source));
                    continue;
                }

                Control editor = controls[field];
                object input = editor is ComboBox combo ? Ui.ChoiceValue(combo)
                    : editor is DateTimePicker date ? (object)(date.ShowCheckBox && !date.Checked ? null : (DateTime?)date.Value)
                    : editor.Text;
                field.Set(output, field.Parse(input));
            }
            return output;
        }
    }

    internal static class EditDialog
    {
        public static bool Show<T>(Form owner, string title, DataStore store, T defaults,
            Action<T> save, T original = null, string[] hidden = null, string[] locked = null) where T : class, new()
        {
            using (var dialog = new Form())
            {
                Ui.Window(dialog, title, 820, 450);
                dialog.MinimumSize = new System.Drawing.Size(720, 350);
                var editor = new RowEditor<T>(store, defaults, hidden, locked);
                var buttons = Ui.Actions();
                Ui.Button(buttons, "Lưu", () =>
                {
                    save(editor.Read(original));
                    dialog.DialogResult = DialogResult.OK;
                }, true);
                Button cancel = Ui.Button(buttons, "Hủy", () => dialog.DialogResult = DialogResult.Cancel);
                dialog.CancelButton = cancel;
                var layout = Ui.Layout();
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                layout.Controls.Add(Ui.Heading(title));
                layout.Controls.Add(editor.Panel);
                layout.Controls.Add(buttons);
                var scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
                scroll.Controls.Add(layout);
                dialog.Controls.Add(scroll);
                editor.LoadChoices();
                editor.Display(original);
                editor.Enable(true, original == null);
                return dialog.ShowDialog(owner) == DialogResult.OK;
            }
        }
    }
}
