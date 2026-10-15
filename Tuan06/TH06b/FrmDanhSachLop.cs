using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmDanhSachLop : Form
{
    private readonly ListView lv = new()
    {
        Left = 25, Top = 65, Width = 610, Height = 280,
        View = View.Details, FullRowSelect = true, GridLines = true
    };

    public FrmDanhSachLop()
    {
        Text = "Danh sách Lớp";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(660, 410);
        Font = new Font("Segoe UI", 10F);

        lv.Columns.Add("STT", 55);
        lv.Columns.Add("Mã lớp", 130);
        lv.Columns.Add("Tên lớp", 270);
        lv.Columns.Add("Mã khoa", 120);

        Controls.Add(Ui.B("Hiển thị dữ liệu", 25, 20, BtnHienThi_Click, 140));
        Controls.Add(lv);
        Controls.Add(Ui.B("Thoát", 545, 355, (_, _) => Close()));
    }

    private void BtnHienThi_Click(object? sender, EventArgs e)
    {
        lv.Items.Clear();
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT MaLop,TenLop,MaKhoa FROM Lop ORDER BY MaLop", conn);
        using var rd = cmd.ExecuteReader();
        int stt = 1;
        while (rd.Read())
        {
            var item = new ListViewItem(stt++.ToString());
            item.SubItems.Add(rd["MaLop"].ToString());
            item.SubItems.Add(rd["TenLop"].ToString());
            item.SubItems.Add(rd["MaKhoa"].ToString());
            lv.Items.Add(item);
        }
    }
}
