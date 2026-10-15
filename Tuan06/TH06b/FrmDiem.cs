using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmDiem : Form
{
    private readonly ComboBox cboSV = Ui.C("cboSinhVien", 150, 45);
    private readonly ComboBox cboMH = Ui.C("cboMonHoc", 150, 85);
    private readonly TextBox txtDiem = Ui.T("txtDiem", 150, 125);

    public FrmDiem()
    {
        Text = "Cập nhật dữ liệu bảng Điểm";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 240);
        Font = new Font("Segoe UI", 10F);
        Controls.AddRange(new Control[]
        {
            Ui.L("Sinh viên", 35, 45), cboSV,
            Ui.L("Môn học", 35, 85), cboMH,
            Ui.L("Điểm", 35, 125), txtDiem,
            Ui.B("Thêm", 80, 175, BtnThem_Click),
            Ui.B("Xóa", 205, 175, BtnXoa_Click),
            Ui.B("Sửa", 330, 175, BtnSua_Click)
        });
        Load += FrmDiem_Load;
    }

    private void FrmDiem_Load(object? sender, EventArgs e)
    {
        LoadCombo(cboSV, "SELECT MaSinhVien FROM SinhVien ORDER BY MaSinhVien", "MaSinhVien");
        LoadCombo(cboMH, "SELECT MaMonHoc FROM MonHoc ORDER BY MaMonHoc", "MaMonHoc");
    }

    private static void LoadCombo(ComboBox cbo, string sql, string col)
    {
        cbo.Items.Clear();
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand(sql, conn);
        using var rd = cmd.ExecuteReader();
        while (rd.Read()) cbo.Items.Add(rd[col].ToString());
        if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
    }

    private bool KiemTraNhap(out decimal diem)
    {
        diem = 0;
        if (cboSV.SelectedItem is null || cboMH.SelectedItem is null || string.IsNullOrWhiteSpace(txtDiem.Text))
        {
            MessageBox.Show("Tất cả thông tin phải được nhập dữ liệu.");
            return false;
        }
        if (!decimal.TryParse(txtDiem.Text, out diem) || diem < 0 || diem > 10)
        {
            MessageBox.Show("Điểm phải là số trong khoảng 0 đến 10.");
            return false;
        }
        return true;
    }

    private bool KhoaChinhTonTai(string sv, string mh)
    {
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM Diem WHERE MaSinhVien=@sv AND MaMonHoc=@mh", conn);
        cmd.Parameters.AddWithValue("@sv", sv);
        cmd.Parameters.AddWithValue("@mh", mh);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (!KiemTraNhap(out decimal diem)) return;
        string sv = cboSV.SelectedItem!.ToString()!;
        string mh = cboMH.SelectedItem!.ToString()!;
        if (KhoaChinhTonTai(sv, mh)) { MessageBox.Show("Trùng khóa chính (Mã sinh viên, Mã môn học)."); return; }

        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("INSERT INTO Diem(MaSinhVien,MaMonHoc,Diem) VALUES(@sv,@mh,@diem)", conn);
        cmd.Parameters.AddWithValue("@sv", sv);
        cmd.Parameters.AddWithValue("@mh", mh);
        cmd.Parameters.AddWithValue("@diem", diem);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Thêm điểm thành công.");
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        if (cboSV.SelectedItem is null || cboMH.SelectedItem is null) { MessageBox.Show("Chọn Sinh viên và Môn học."); return; }
        string sv = cboSV.SelectedItem!.ToString()!;
        string mh = cboMH.SelectedItem!.ToString()!;
        if (!KhoaChinhTonTai(sv, mh)) { MessageBox.Show("Dòng điểm không tồn tại."); return; }

        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("DELETE FROM Diem WHERE MaSinhVien=@sv AND MaMonHoc=@mh", conn);
        cmd.Parameters.AddWithValue("@sv", sv);
        cmd.Parameters.AddWithValue("@mh", mh);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Xóa điểm thành công.");
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        if (!KiemTraNhap(out decimal diem)) return;
        string sv = cboSV.SelectedItem!.ToString()!;
        string mh = cboMH.SelectedItem!.ToString()!;
        if (!KhoaChinhTonTai(sv, mh)) { MessageBox.Show("Dòng điểm không tồn tại."); return; }

        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("UPDATE Diem SET Diem=@diem WHERE MaSinhVien=@sv AND MaMonHoc=@mh", conn);
        cmd.Parameters.AddWithValue("@diem", diem);
        cmd.Parameters.AddWithValue("@sv", sv);
        cmd.Parameters.AddWithValue("@mh", mh);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Sửa điểm thành công.");
    }
}
