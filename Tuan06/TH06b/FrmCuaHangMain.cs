namespace TH06b;

public class FrmCuaHangMain : Form
{
    public FrmCuaHangMain()
    {
        Text = "QL Cửa hàng";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 280);
        Font = new Font("Segoe UI", 10F);

        Controls.Add(new Label
        {
            Text = "QUẢN LÝ CỬA HÀNG",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            AutoSize = true,
            Left = 125,
            Top = 30
        });
        Add("Tạo tồn kho hôm nay", 70, 95, () => new FrmTonKho().ShowDialog());
        Add("Thêm sản phẩm", 270, 95, () => new FrmSanPham().ShowDialog());
        Add("Bán hàng", 70, 165, () => new FrmBanHang().ShowDialog());
        Add("Đóng", 270, 165, Close);
    }

    private void Add(string text, int x, int y, Action action)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = 160, Height = 44 };
        b.Click += (_, _) => action();
        Controls.Add(b);
    }
}
