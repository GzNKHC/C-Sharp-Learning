using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmDanhSachDiem : Form
{
    private readonly ComboBox cboMon = Ui.C("cboMonHoc", 120, 22, 180);
    private readonly ListView lv = new()
    {
        Left = 25, Top = 70, Width = 720, Height = 330,
        View = View.Details, FullRowSelect = true, GridLines = true
    };
    private readonly TextBox txtTong = Ui.T("txtTongDiem", 605, 420, 140);

    public FrmDanhSachDiem()
    {
        Text = "Danh sách điểm sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(775, 475);
        Font = new Font("Segoe UI", 10F);

        lv.Columns.Add("STT", 50);
        lv.Columns.Add("Tên SV", 180);
        lv.Columns.Add("Ngày sinh", 120);
        lv.Columns.Add("Tên môn học", 220);
        lv.Columns.Add("Điểm", 90);
        txtTong.ReadOnly = true;

        Controls.AddRange(new Control[]
        {
            Ui.L("Môn học", 25, 22, 85), cboMon,
            Ui.B("Lọc dữ liệu", 330, 18, BtnLoc_Click, 115),
            lv,
            Ui.L("Tổng điểm", 515, 420, 85), txtTong
        });
        Load += FrmDanhSachDiem_Load;
    }

    private void FrmDanhSachDiem_Load(object? sender, EventArgs e)
    {
        cboMon.Items.Add(" ");
        using (var conn = Db.SinhVienConn())
        {
            conn.Open();
            using var cmd = new SqlCommand("SELECT MaMonHoc FROM MonHoc ORDER BY MaMonHoc", conn);
            using var rd = cmd.ExecuteReader();
            while (rd.Read()) cboMon.Items.Add(rd["MaMonHoc"].ToString());
        }
        cboMon.SelectedIndex = 0;
        HienThi(null);
    }

    private void BtnLoc_Click(object? sender, EventArgs e)
    {
        string? maMon = cboMon.SelectedItem?.ToString()?.Trim();
        HienThi(string.IsNullOrWhiteSpace(maMon) ? null : maMon);
    }

    private void HienThi(string? maMon)
    {
        lv.Items.Clear();
        decimal tong = 0;
        using var conn = Db.SinhVienConn();
        conn.Open();
        string sql = @"
SELECT sv.HoTen, sv.NgaySinh, mh.TenMonHoc, d.Diem
FROM Diem d
JOIN SinhVien sv ON sv.MaSinhVien=d.MaSinhVien
JOIN MonHoc mh ON mh.MaMonHoc=d.MaMonHoc
WHERE (@maMon IS NULL OR d.MaMonHoc=@maMon)
ORDER BY sv.HoTen, mh.TenMonHoc";
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@maMon", (object?)maMon ?? DBNull.Value);
        using var rd = cmd.ExecuteReader();
        int stt = 1;
        while (rd.Read())
        {
            decimal diem = Convert.ToDecimal(rd["Diem"]);
            tong += diem;
            var item = new ListViewItem(stt++.ToString());
            item.SubItems.Add(rd["HoTen"].ToString());
            item.SubItems.Add(Convert.ToDateTime(rd["NgaySinh"]).ToString("dd/MM/yyyy"));
            item.SubItems.Add(rd["TenMonHoc"].ToString());
            item.SubItems.Add(diem.ToString("0.##"));
            lv.Items.Add(item);
        }
        txtTong.Text = tong.ToString("0.##");
    }
}
