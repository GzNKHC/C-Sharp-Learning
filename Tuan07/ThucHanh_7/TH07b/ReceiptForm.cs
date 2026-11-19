using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ThucHanh7
{
    internal sealed class ReceiptForm : Form
    {
        private readonly TextBox receiptCode = Ui.TextBox(20);
        private readonly DateTimePicker receiptDate = Ui.DatePicker();
        private readonly ComboBox supplier = Ui.Combo();
        private readonly TextBox total = Ui.TextBox();
        private readonly ComboBox receipts = Ui.Combo();
        private readonly ComboBox products = Ui.Combo();
        private readonly TextBox price = Ui.TextBox();
        private readonly TextBox quantity = Ui.TextBox(10);
        private readonly DataGridView grid = Ui.Grid();
        private readonly GroupBox headerGroup;
        private readonly GroupBox detailGroup;
        private readonly Button create, save, addLine, cancelEdit, print;
        private readonly ToolStripMenuItem editLine, deleteLine;
        private readonly ContextMenuStrip context = new ContextMenuStrip();
        private bool loading;
        private bool ready;
        private bool editing;
        private decimal savedTotal;
        private int originalQuantity;
        private DataTable detailRows;

        public ReceiptForm()
        {
            Ui.SetWindow(this, "Phiếu nhập hàng", 1020, 675);
            MinimumSize = new Size(820, 560);
            total.Enabled = price.Enabled = false;
            total.TextAlign = price.TextAlign = quantity.TextAlign = HorizontalAlignment.Right;
            total.Text = price.Text = quantity.Text = "0";

            var headerFields = Ui.Fields();
            Ui.Field(headerFields, 0, "Mã phiếu nhập", receiptCode);
            Ui.Field(headerFields, 1, "Nhà cung cấp", supplier);
            Ui.Field(headerFields, 2, "Ngày nhập", receiptDate);
            Ui.Field(headerFields, 3, "Thành tiền", total);

            var headerActions = Ui.Actions();
            create = Ui.Button(headerActions, "Tạo phiếu nhập", (sender, args) => Ui.Run(this, () => SaveHeader(true)));
            save = Ui.Button(headerActions, "Lưu phiếu nhập", (sender, args) => Ui.Run(this, () => SaveHeader(false)));
            Ui.Button(headerActions, "Phiếu mới", (sender, args) => Ui.Run(this, NewReceipt));
            headerGroup = Group("Phiếu nhập", headerFields, headerActions);

            var detailFields = Ui.Fields();
            Ui.Field(detailFields, 0, "Mã phiếu nhập", receipts);
            Ui.Field(detailFields, 1, "Mã sản phẩm", products);
            Ui.Field(detailFields, 2, "Đơn giá", price);
            Ui.Field(detailFields, 3, "Số lượng", quantity);

            var detailActions = Ui.Actions();
            addLine = Ui.Button(detailActions, "Thêm sản phẩm", (sender, args) => Ui.Run(this, SaveLine));
            cancelEdit = Ui.Button(detailActions, "Bỏ qua sửa", (sender, args) => ResetLine());
            print = Ui.Button(detailActions, "In phiếu nhập", (sender, args) => Ui.Run(this, Print));
            detailGroup = Group("Chi tiết phiếu nhập", detailFields, detailActions);
            Ui.Arrange(this, grid, headerGroup, detailGroup);

            editLine = new ToolStripMenuItem("Sửa", null, (sender, args) => Ui.Run(this, BeginLineEdit));
            deleteLine = new ToolStripMenuItem("Xóa", null, (sender, args) => Ui.Run(this, DeleteLine));
            context.Items.AddRange(new ToolStripItem[] { editLine, deleteLine });
            context.Opening += (sender, args) => args.Cancel = Ui.Current(grid) == null || editing;
            grid.ContextMenuStrip = context;
            grid.CellMouseDown += (sender, args) =>
            {
                if (!editing && args.Button == MouseButtons.Right && args.RowIndex >= 0 && args.ColumnIndex >= 0)
                    grid.CurrentCell = grid.Rows[args.RowIndex].Cells[args.ColumnIndex];
            };
            grid.CellClick += (sender, args) =>
            {
                if (!editing && args.RowIndex >= 0)
                    Ui.Run(this, () => DisplayLine(false));
            };

            receipts.SelectedValueChanged += (sender, args) =>
            {
                if (!loading && ready)
                    Ui.Run(this, LoadSelection);
            };
            products.SelectedValueChanged += (sender, args) =>
            {
                if (!loading && ready)
                {
                    quantity.Text = "0";
                    UpdatePreview();
                }
            };
            quantity.TextChanged += (sender, args) =>
            {
                if (!loading && ready)
                    UpdatePreview();
            };
            receiptCode.TextChanged += (sender, args) =>
            {
                if (!loading && ready)
                    UpdatePreview();
            };
            Load += (sender, args) => Ui.Run(this, InitializeData);
            Shown += (sender, args) => receiptCode.Focus();
            SetButtons();
        }

        private static GroupBox Group(string title, Control fields, Control actions)
        {
            var group = new GroupBox
            {
                Text = title, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(8, 5, 8, 2)
            };
            var panel = Ui.Layout(1);
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            panel.Controls.Add(fields, 0, 0);
            panel.Controls.Add(actions, 0, 1);
            group.Controls.Add(panel);
            return group;
        }

        private void InitializeData()
        {
            loading = true;
            try
            {
                Ui.Bind(supplier, Db.ReadWithReader("SELECT MaNCC, TenNCC FROM dbo.NCC ORDER BY MaNCC"),
                    "MaNCC", "MaNCC");
                Ui.Bind(products, Db.ReadWithReader("SELECT MaSanPham, TenSanPham, DonGia FROM dbo.SanPham ORDER BY MaSanPham"),
                    "MaSanPham", "MaSanPham");
                BindReceipts("");
            }
            finally
            {
                loading = false;
            }
            ready = true;
            LoadSelection();
        }

        private void BindReceipts(string selected)
        {
            Ui.Bind(receipts, Db.ReadWithReader("SELECT MaPhieuNhap FROM dbo.PhieuNhap ORDER BY MaPhieuNhap"),
                "MaPhieuNhap", "MaPhieuNhap");
            if (selected.Length != 0)
                receipts.SelectedValue = selected;
        }

        private void LoadSelection()
        {
            string code = Ui.Selected(receipts);
            DataTable header = Db.Read(@"SELECT NgayNhap, MaNCC, ThanhTien
                FROM dbo.PhieuNhap WHERE MaPhieuNhap = @ma", Db.Text("@ma", code));
            DataTable details = Db.Read(@"SELECT MaPhieuNhap, MaSanPham, DonGia, SoLuong
                FROM dbo.ChiTietPhieuNhap WHERE @ma = N'' OR MaPhieuNhap = @ma
                ORDER BY MaPhieuNhap, MaSanPham", Db.Text("@ma", code));

            loading = true;
            try
            {
                DataRow row = header.Rows.Count == 0 ? null : header.Rows[0];
                receiptCode.Text = row == null ? "" : code;
                receiptDate.Value = row == null ? DateTime.Today : (DateTime)row["NgayNhap"];
                supplier.SelectedValue = row == null ? "" : Db.Value(row["MaNCC"]);
                savedTotal = row == null ? 0 : (decimal)row["ThanhTien"];
                detailRows = details;
                grid.DataSource = details;
                Ui.Header(grid, "MaPhieuNhap", "Mã phiếu nhập");
                Ui.Header(grid, "MaSanPham", "Mã sản phẩm");
                Ui.Header(grid, "DonGia", "Đơn giá", "N2");
                Ui.Header(grid, "SoLuong", "Số lượng");
                Ui.ClearSelection(grid);
                ResetLine();
            }
            finally
            {
                loading = false;
            }
            UpdatePreview();
        }

        private void ResetLine()
        {
            editing = false;
            originalQuantity = 0;
            products.SelectedIndex = -1;
            quantity.Text = "0";
            UpdatePreview();
        }

        private void NewReceipt()
        {
            if (!ready)
            {
                InitializeData();
                return;
            }
            if (Ui.Selected(receipts).Length != 0)
                receipts.SelectedIndex = -1;
            else
                LoadSelection();
            receiptCode.Focus();
        }

        private bool HeaderMatches => Ui.Selected(receipts).Length > 0 &&
            string.Equals(Ui.Selected(receipts), receiptCode.Text.Trim(), StringComparison.OrdinalIgnoreCase);

        private decimal LinePrice()
        {
            string receipt = Ui.Selected(receipts);
            string product = Ui.Selected(products);
            if (receipt.Length > 0 && product.Length > 0 && detailRows != null)
            {
                foreach (DataRow row in detailRows.Rows)
                    if (Db.Value(row["MaPhieuNhap"]) == receipt && Db.Value(row["MaSanPham"]) == product)
                        return (decimal)row["DonGia"];
            }
            return products.SelectedItem is DataRowView selected ? (decimal)selected["DonGia"] : 0;
        }

        private void UpdatePreview()
        {
            decimal unitPrice = LinePrice();
            price.Text = unitPrice.ToString("N2");
            decimal preview = HeaderMatches ? savedTotal : 0;
            if (HeaderMatches && Ui.Selected(products).Length != 0 &&
                int.TryParse(quantity.Text, NumberStyles.None, CultureInfo.CurrentCulture, out int amount))
                preview += (amount - (editing ? originalQuantity : 0)) * unitPrice;
            total.Text = preview.ToString("N2");
            SetButtons();
        }

        private void SetButtons()
        {
            create.Enabled = ready && !HeaderMatches && !editing;
            save.Enabled = ready && HeaderMatches && !editing;
            addLine.Enabled = ready && HeaderMatches && Ui.Selected(products).Length > 0;
            addLine.Text = editing ? "Lưu chi tiết" : "Thêm sản phẩm";
            cancelEdit.Enabled = editing;
            receipts.Enabled = ready && !editing;
            products.Enabled = ready && !editing;
            grid.Enabled = !editing;
            receiptCode.Enabled = !editing;
            print.Enabled = ready && HeaderMatches && !editing;
        }

        private void SaveHeader(bool insert)
        {
            string code = receiptCode.Text.Trim();
            string supplierCode = Ui.Selected(supplier);
            if (code.Length == 0 || supplierCode.Length == 0)
                throw new InvalidOperationException("Vui lòng nhập mã phiếu và chọn nhà cung cấp.");

            ReceiptStore.SaveHeader(code, supplierCode, receiptDate.Value.Date, insert);
            MessageBox.Show(this, insert ? "Tạo phiếu nhập thành công." : "Lưu phiếu nhập thành công.", "Thông báo");
            loading = true;
            try
            {
                BindReceipts(insert ? code : Ui.Selected(receipts));
            }
            finally
            {
                loading = false;
            }
            LoadSelection();
        }

        private void DisplayLine(bool startEdit)
        {
            DataRow row = Ui.Current(grid);
            if (row == null)
                return;

            string receipt = Db.Value(row["MaPhieuNhap"]);
            string product = Db.Value(row["MaSanPham"]);
            int rowQuantity = (int)row["SoLuong"];
            if (!HeaderMatches || receipt != Ui.Selected(receipts))
            {
                if (receipt == Ui.Selected(receipts))
                    LoadSelection();
                else
                    receipts.SelectedValue = receipt;
                Ui.SelectRow(grid, item => Db.Value(item["MaSanPham"]) == product);
            }

            loading = true;
            try
            {
                products.SelectedValue = product;
                originalQuantity = rowQuantity;
                editing = startEdit;
                quantity.Text = rowQuantity.ToString(CultureInfo.CurrentCulture);
            }
            finally
            {
                loading = false;
            }
            UpdatePreview();
            // Khi chỉ chọn một dòng, hiển thị tổng đã lưu. Gõ số lượng mới sẽ xem trước phần cộng thêm.
            if (!startEdit)
                total.Text = savedTotal.ToString("N2");
        }

        private void BeginLineEdit()
        {
            DisplayLine(true);
            quantity.Focus();
            quantity.SelectAll();
        }

        private void SaveLine()
        {
            if (!HeaderMatches || !int.TryParse(quantity.Text, NumberStyles.None, CultureInfo.CurrentCulture,
                    out int amount) || amount <= 0)
                throw new InvalidOperationException("Hãy chọn phiếu và nhập số lượng nguyên lớn hơn 0.");

            string receipt = Ui.Selected(receipts);
            string product = Ui.Selected(products);
            ReceiptStore.SaveLine(receipt, product, amount, editing, false);
            ResetLine();
            MessageBox.Show(this, "Lưu chi tiết phiếu nhập thành công.", "Thông báo");
            LoadSelection();
        }

        private void DeleteLine()
        {
            DataRow row = Ui.Current(grid);
            if (row == null || !Ui.Confirm(this, "Bạn có muốn xóa sản phẩm khỏi phiếu nhập?"))
                return;
            ReceiptStore.SaveLine(Db.Value(row["MaPhieuNhap"]), Db.Value(row["MaSanPham"]), 0, false, true);
            MessageBox.Show(this, "Xóa chi tiết phiếu nhập thành công.", "Thông báo");
            LoadSelection();
        }

        private void Print()
        {
            PrintGrid.Show(this, grid, "PHIẾU NHẬP HÀNG", "Mã phiếu " + Ui.Selected(receipts) +
                "     Ngày nhập " + receiptDate.Value.ToString("dd/MM/yyyy") +
                "\nNhà cung cấp " + Ui.Selected(supplier) + "     Thành tiền " + savedTotal.ToString("N2"));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                context.Dispose();
            base.Dispose(disposing);
        }
    }
}
