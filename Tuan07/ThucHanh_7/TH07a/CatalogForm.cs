using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal sealed class CatalogForm : Form
    {
        private readonly string table;
        private readonly string listSql;
        private readonly RecordField[] fields;
        private readonly string[] keyNames;
        private readonly string dependentTable;
        private readonly DataGridView grid = Ui.Grid();
        private readonly Dictionary<RecordField, ComboBox> filters = new Dictionary<RecordField, ComboBox>();
        private readonly BindingSource dataSource = new BindingSource();
        private readonly Button add, edit, delete, save;
        private readonly TableLayoutPanel editors = Ui.Fields();
        private readonly PictureBox preview;
        private DataTable allRows;
        private string[] originalKey;
        private string pendingImage;
        private bool loading;
        private bool goodsEditing;
        private bool goodsAdding;
        private bool goodsSearching;

        private static string ImageFolder => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ThucHanh_7", "HinhHang");

        public CatalogForm(string title, string table, string listSql, string[] keyNames,
            string dependentTable, params RecordField[] fields)
        {
            this.table = table;
            this.listSql = listSql;
            this.fields = fields;
            this.keyNames = keyNames;
            this.dependentTable = dependentTable;
            Ui.SetWindow(this, title, 1000, 590);

            var filterBar = Ui.Fields();
            foreach (RecordField field in fields.Where(item => item.AllCaption != null))
            {
                var combo = Ui.Combo();
                Ui.Field(filterBar, filters.Count, "Lọc " + field.Caption.ToLower(), combo);
                filters.Add(field, combo);
                combo.SelectedValueChanged += (sender, args) =>
                {
                    if (!loading && allRows != null)
                        Ui.Run(this, ApplyFilter);
                };
            }

            for (int i = 0; i < fields.Length; i++)
                Ui.Field(editors, i, fields[i].Caption, fields[i].Editor);

            if (HasImage)
            {
                RecordField image = fields.Single(field => field.Name == "Hinh");
                var open = new FlowLayoutPanel { AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink };
                Ui.Button(open, "Mở…", (sender, args) => Ui.Run(this, OpenImage));
                preview = new PictureBox { Size = new Size(92, 60), SizeMode = PictureBoxSizeMode.Zoom };
                open.Controls.Add(preview);
                Ui.Field(editors, fields.Length, "Ảnh đính kèm", open);
                image.Editor.TextChanged += (sender, args) => ShowImage();
            }

            var actions = Ui.Actions();
            add = Ui.Button(actions, "Thêm", (sender, args) => Ui.Run(this, Add));
            delete = Ui.Button(actions, "Xóa", (sender, args) => Ui.Run(this, Delete));
            edit = Ui.Button(actions, "Sửa", (sender, args) => Ui.Run(this, Edit));
            if (HasImage)
            {
                save = Ui.Button(actions, "Lưu", (sender, args) => Ui.Run(this, SaveGoods));
                Ui.Button(actions, "Bỏ qua", (sender, args) => Ui.Run(this, Reset));
                Ui.Button(actions, "Tìm kiếm", (sender, args) => Ui.Run(this, SearchGoods));
            }
            else
            {
                Ui.Button(actions, "Làm mới", (sender, args) => Ui.Run(this, Reset));
            }
            Ui.Button(actions, "Đóng", (sender, args) => Close());
            if (filters.Count > 0)
                Ui.Arrange(this, grid, filterBar, editors, actions);
            else
                Ui.Arrange(this, grid, editors, actions);

            grid.SelectionChanged += (sender, args) => ShowSelection();
            SetButtons();
            Load += (sender, args) => Ui.Run(this, InitializeData);
        }

        private bool HasImage => table == "DMHang";

        private void InitializeData()
        {
            loading = true;
            try
            {
                foreach (RecordField field in fields.Where(item => item.LookupTable != null))
                {
                    DataTable options = Db.Read("SELECT " + field.Name + ", " + field.LookupCaption +
                        " FROM dbo." + field.LookupTable + " ORDER BY " + field.LookupCaption + ", " + field.Name);
                    Ui.Bind((ComboBox)field.Editor, options, field.Name, field.LookupCaption);
                    if (filters.TryGetValue(field, out ComboBox filter))
                        Ui.Bind(filter, options, field.Name, field.LookupCaption, field.AllCaption);
                }
            }
            finally
            {
                loading = false;
            }
            Reload();
        }

        private void Reload(string[] selectKey = null)
        {
            allRows = Db.Read(listSql);
            ApplyFilter(selectKey);
        }

        private void ApplyFilter()
        {
            ApplyFilter(null);
        }

        private void ApplyFilter(string[] selectKey)
        {
            DataTable visible = allRows.Clone();
            RecordField[] criteria = HasImage && goodsSearching
                ? fields.Where(field => field.Input.Length > 0).ToArray()
                : new RecordField[0];
            foreach (DataRow row in allRows.Rows)
            {
                if (filters.All(filter => Ui.Selected(filter.Value).Length == 0 ||
                    Db.Value(row[filter.Key.Name]) == Ui.Selected(filter.Value)) &&
                    criteria.All(field => field.Matches(row[field.Name])))
                    visible.ImportRow(row);
            }

            loading = true;
            try
            {
                dataSource.DataSource = visible;
                grid.DataSource = dataSource;
                ApplyHeaders();
                Ui.ClearSelection(grid);
                if (selectKey != null)
                    Ui.SelectRow(grid, row => Key(row).SequenceEqual(selectKey, StringComparer.OrdinalIgnoreCase));
            }
            finally
            {
                loading = false;
            }
            ShowSelection();
        }

        private void ApplyHeaders()
        {
            foreach (RecordField field in fields)
            {
                string format = field.Name == "NgaySinh" ? "dd/MM/yyyy" : null;
                Ui.Header(grid, field.Name, field.Caption, format);
                if (field.LookupCaption != null && grid.Columns.Contains(field.LookupCaption))
                    Ui.Header(grid, field.LookupCaption, field.Caption.Replace("Mã ", "Tên "));
            }
            if (grid.Columns.Contains("HoTen") && table == "Diem")
                Ui.Header(grid, "HoTen", "Họ tên sinh viên");
            if (grid.Columns.Contains("TenLop"))
                Ui.Header(grid, "TenLop", "Tên lớp");
        }

        private string[] Key(DataRow row)
        {
            return keyNames.Select(name => Db.Value(row[name])).ToArray();
        }

        private void ShowSelection()
        {
            if (loading || goodsEditing)
                return;

            DataRow row = Ui.Current(grid);
            originalKey = row == null ? null : Key(row);
            foreach (RecordField field in fields)
                field.Show(row == null ? null : row[field.Name]);
            SetButtons();
        }

        private void SetButtons()
        {
            add.Enabled = allRows != null && !goodsEditing;
            edit.Enabled = originalKey != null && !goodsEditing;
            delete.Enabled = edit.Enabled;
            if (save != null)
            {
                save.Enabled = goodsEditing;
                grid.Enabled = !goodsEditing;
                var code = fields.Single(field => field.Name == "MaHang");
                code.Editor.Enabled = !goodsEditing || goodsAdding;
            }
        }

        private void Add()
        {
            if (!HasImage)
            {
                Change("Thêm", true, false);
                return;
            }
            Reset();
            goodsEditing = goodsAdding = true;
            SetButtons();
            fields[0].Editor.Focus();
        }

        private void Edit()
        {
            if (!HasImage)
            {
                Change("Sửa", false, false);
                return;
            }
            goodsEditing = true;
            goodsAdding = false;
            goodsSearching = false;
            SetButtons();
            fields[1].Editor.Focus();
        }

        private void SaveGoods()
        {
            Change(goodsAdding ? "Thêm" : "Sửa", goodsAdding, false);
        }

        private void Delete()
        {
            if (originalKey != null && Ui.Confirm(this, "Bạn có muốn xóa dòng đang chọn?"))
                Change("Xóa", false, true);
        }

        private void Change(string action, bool insert, bool remove)
        {
            var values = new Dictionary<string, object>();
            if (!remove)
            {
                foreach (RecordField field in fields)
                    values.Add(field.Name, field.Read());
            }

            if (!remove && HasImage)
            {
                string name = Db.Value(values["Hinh"]);
                if (name.Length > 0 && (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                    name != Path.GetFileName(name) || !File.Exists(Path.Combine(ImageFolder, name))))
                    throw new InvalidOperationException("Hãy dùng nút Mở để chọn tệp ảnh hợp lệ.");
            }

            string[] key = insert ? keyNames.Select(name => Db.Value(values[name])).ToArray() : originalKey;
            if (key == null)
                throw new InvalidOperationException("Hãy chọn dòng cần cập nhật.");
            if (!insert && !remove && keyNames.Where((name, i) =>
                !string.Equals(Db.Value(values[name]), key[i], StringComparison.Ordinal)).Any())
                throw new InvalidOperationException("Mã của dòng đang sửa phải giữ nguyên. Hãy chọn lại dòng.");

            Db.InTransaction((connection, transaction) =>
            {
                if (!remove)
                {
                    foreach (RecordField field in fields.Where(item => item.LookupTable != null))
                    {
                        if (!Db.Exists(connection, transaction, "SELECT 1 FROM dbo." + field.LookupTable +
                            " WHERE " + field.Name + " = @ma", Db.Text("@ma", Db.Value(values[field.Name]))))
                            throw new InvalidOperationException(field.Caption + " không tồn tại.");
                    }
                }
                if (remove && dependentTable != null &&
                    Db.Exists(connection, transaction, "SELECT 1 FROM dbo." + dependentTable +
                        " WHERE " + keyNames[0] + " = @ma", Db.Text("@ma", key[0])))
                    throw new InvalidOperationException("Dữ liệu đang được sử dụng, không thể xóa.");

                string select = "SELECT " + string.Join(", ", fields.Select(field => field.Name)) +
                    " FROM dbo." + table + " WHERE " + string.Join(" AND ",
                        keyNames.Select((name, i) => name + " = @ma" + i));
                var parameters = key.Select((part, i) => Db.Text("@ma" + i, part)).ToArray();
                using (var command = Db.Command(connection, select, transaction, parameters))
                using (var adapter = new SqlDataAdapter(command) { MissingSchemaAction = MissingSchemaAction.AddWithKey })
                using (var builder = new SqlCommandBuilder(adapter))
                using (var dataSet = new DataSet())
                {
                    adapter.Fill(dataSet, table);
                    DataTable data = dataSet.Tables[table];
                    DataRow current = data.Rows.Find(key);
                    if (insert && data.Rows.Count > 0)
                        throw new InvalidOperationException("Mã hoặc tổ hợp mã này đã tồn tại.");
                    if (!insert && data.Rows.Count == 0)
                        throw new InvalidOperationException("Dòng này không còn trong cơ sở dữ liệu.");
                    if (!insert && current == null)
                        current = data.Rows[0];

                    if (insert)
                    {
                        current = data.NewRow();
                        foreach (var item in values)
                            current[item.Key] = item.Value;
                        data.Rows.Add(current);
                    }
                    else if (remove)
                    {
                        current.Delete();
                    }
                    else
                    {
                        foreach (var item in values.Where(item => !keyNames.Contains(item.Key)))
                            current[item.Key] = item.Value;
                    }
                    adapter.Update(dataSet, table);
                }
            });

            if (HasImage && values.ContainsKey("Hinh") && Db.Value(values["Hinh"]) == pendingImage)
                pendingImage = null;
            DiscardImage();
            goodsEditing = goodsAdding = goodsSearching = false;
            if (!remove)
            {
                loading = true;
                try
                {
                    foreach (var item in filters)
                    {
                        if (Ui.Selected(item.Value).Length != 0)
                            item.Value.SelectedValue = values[item.Key.Name];
                    }
                }
                finally
                {
                    loading = false;
                }
            }
            SetButtons();
            MessageBox.Show(this, action + " thành công.", "Thông báo");
            Reload(remove ? null : key);
        }

        private void SearchGoods()
        {
            if (goodsEditing)
                throw new InvalidOperationException("Hãy lưu hoặc bỏ qua thay đổi trước khi tìm kiếm.");
            if (allRows == null)
            {
                InitializeData();
                return;
            }
            goodsSearching = true;
            try
            {
                ApplyFilter();
            }
            finally
            {
                goodsSearching = false;
            }
        }

        private void Reset()
        {
            DiscardImage();
            goodsEditing = goodsAdding = goodsSearching = false;
            if (allRows == null)
            {
                InitializeData();
                return;
            }
            loading = true;
            try
            {
                foreach (ComboBox combo in filters.Values)
                    combo.SelectedIndex = 0;
            }
            finally
            {
                loading = false;
            }
            Reload();
        }

        private void OpenImage()
        {
            if (!goodsEditing)
                throw new InvalidOperationException("Hãy chọn Thêm hoặc Sửa để đính kèm ảnh.");
            using (var dialog = new OpenFileDialog
            {
                Filter = "Tệp hình ảnh (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                CheckFileExists = true
            })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                using (var check = Image.FromFile(dialog.FileName))
                {
                    Directory.CreateDirectory(ImageFolder);
                    string name = Guid.NewGuid().ToString("N") + Path.GetExtension(dialog.FileName);
                    File.Copy(dialog.FileName, Path.Combine(ImageFolder, name));
                    DiscardImage();
                    pendingImage = name;
                    fields.Single(field => field.Name == "Hinh").Show(pendingImage);
                }
            }
        }

        private void ShowImage()
        {
            if (preview == null)
                return;
            preview.Image?.Dispose();
            preview.Image = null;
            string name = fields.Single(field => field.Name == "Hinh").Input;
            if (name.Length == 0 || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||
                name != Path.GetFileName(name))
                return;
            string path = Path.Combine(ImageFolder, name);
            try
            {
                if (File.Exists(path))
                {
                    using (Image source = Image.FromFile(path))
                        preview.Image = new Bitmap(source);
                }
            }
            catch (ArgumentException)
            {
                // Tệp cũ có thể không còn là ảnh đọc được.
            }
            catch (IOException)
            {
            }
        }

        private void DiscardImage()
        {
            if (pendingImage != null)
            {
                try
                {
                    File.Delete(Path.Combine(ImageFolder, pendingImage));
                }
                catch (IOException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                pendingImage = null;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DiscardImage();
                if (preview != null)
                    preview.Image?.Dispose();
                dataSource.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
