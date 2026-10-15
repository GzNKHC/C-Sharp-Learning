namespace TH06b;

public class FrmMain : Form
{
    public FrmMain()
    {
        Text = "TH06b - Tương tác CSDL";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(650, 390);
        Font = new Font("Segoe UI", 10F);

        Controls.Add(new Label
        {
            Text = "THỰC HÀNH 06b - ADO.NET",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            AutoSize = true,
            Left = 165,
            Top = 24
        });

        Add("1. Sinh viên (ComboBox)", 55, 85, () => new FrmSinhVien().ShowDialog());
        Add("2. Điểm (ComboBox)", 350, 85, () => new FrmDiem().ShowDialog());
        Add("3. Danh sách lớp", 55, 145, () => new FrmDanhSachLop().ShowDialog());
        Add("4. Danh sách điểm", 350, 145, () => new FrmDanhSachDiem().ShowDialog());
        Add("5. QL Cửa hàng", 55, 205, () => new FrmCuaHangMain().ShowDialog());
        Add("Thoát", 350, 205, Close);

        Controls.Add(new Label
        {
            Text = "CSDL dùng chung: chạy script trong thư mục Database trước khi mở các form.",
            Left = 55, Top = 285, Width = 535, Height = 45
        });
    }

    private void Add(string text, int x, int y, Action action)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = 245, Height = 42 };
        b.Click += (_, _) => action();
        Controls.Add(b);
    }
}
