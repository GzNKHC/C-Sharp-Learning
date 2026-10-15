namespace TH06a;

public class FrmMain : Form
{
    public FrmMain()
    {
        Text = "TH06a - Tương tác CSDL";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(460, 280);
        Font = new Font("Segoe UI", 10F);

        var title = new Label
        {
            Text = "THỰC HÀNH 06a - ADO.NET",
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            AutoSize = true,
            Left = 78,
            Top = 28
        };
        Controls.Add(title);

        AddButton("1. Quản lý môn học", 55, 90, () => new FrmMonHoc().ShowDialog());
        AddButton("2. Quản lý sinh viên", 245, 90, () => new FrmSinhVien().ShowDialog());
        AddButton("3. Quản lý điểm", 55, 150, () => new FrmDiem().ShowDialog());
        AddButton("4. Quản lý lớp", 245, 150, () => new FrmLop().ShowDialog());
        AddButton("Thoát", 150, 215, Close);
    }

    private void AddButton(string text, int x, int y, Action action)
    {
        var b = new Button { Text = text, Left = x, Top = y, Width = 160, Height = 40 };
        b.Click += (_, _) => action();
        Controls.Add(b);
    }
}
