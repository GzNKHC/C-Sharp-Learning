using Microsoft.Data.SqlClient;

namespace TH06a;

public class FrmDiem : Form
{
    private readonly TextBox txtMaSV = Ui.T("txtMaSV", 150, 45);
    private readonly TextBox txtMaMH = Ui.T("txtMaMH", 150, 85);
    private readonly TextBox txtDiem = Ui.T("txtDiem", 150, 125);

    public FrmDiem()
    {
        Text = "Quản lý điểm";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 240);
        Font = new Font("Segoe UI", 10F);
        Controls.AddRange(new Control[]
        {
            Ui.L("Mã sinh viên", 35, 45), txtMaSV,
            Ui.L("Mã môn học", 35, 85), txtMaMH,
            Ui.L("Điểm", 35, 125), txtDiem,
            Ui.B("Thêm", 80, 175, BtnThem_Click),
            Ui.B("Xóa", 205, 175, BtnXoa_Click),
            Ui.B("Sửa", 330, 175, BtnSua_Click)
        });
    }

    private bool KiemTraDiem(out decimal diem)
    {
        if (!decimal.TryParse(txtDiem.Text, out diem))
        {
            MessageBox.Show("Điểm phải là số.");
            return false;
        }
        return true;
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtMaMH.Text) || string.IsNullOrWhiteSpace(txtDiem.Text))
        {
            MessageBox.Show("Mã sinh viên, Mã môn học và Điểm không được để trống.");
            return;
        }
        if (!KiemTraDiem(out decimal diem)) return;
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO Diem(MaSinhVien,MaMonHoc,Diem) VALUES(@sv,@mh,@diem)", conn);
            cmd.Parameters.AddWithValue("@sv", txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@mh", txtMaMH.Text.Trim());
            cmd.Parameters.AddWithValue("@diem", diem);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Thêm điểm thành công.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Thêm thất bại. Kiểm tra khóa chính/khóa ngoại.\n" + ex.Message);
        }
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtMaMH.Text))
        {
            MessageBox.Show("Mã sinh viên và Mã môn học không được để trống.");
            return;
        }
        if (!string.IsNullOrWhiteSpace(txtDiem.Text))
        {
            MessageBox.Show("Khi xóa, Điểm không nhập dữ liệu.");
            return;
        }
        using var conn = Db.CreateConnection();
        conn.Open();
        using var cmd = new SqlCommand("DELETE FROM Diem WHERE MaSinhVien=@sv AND MaMonHoc=@mh", conn);
        cmd.Parameters.AddWithValue("@sv", txtMaSV.Text.Trim());
        cmd.Parameters.AddWithValue("@mh", txtMaMH.Text.Trim());
        int n = cmd.ExecuteNonQuery();
        MessageBox.Show(n > 0 ? "Xóa thành công." : "Không tìm thấy dòng điểm.");
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtMaMH.Text) || string.IsNullOrWhiteSpace(txtDiem.Text))
        {
            MessageBox.Show("Mã sinh viên, Mã môn học và Điểm không được để trống.");
            return;
        }
        if (!KiemTraDiem(out decimal diem)) return;
        using var conn = Db.CreateConnection();
        conn.Open();
        using var cmd = new SqlCommand("UPDATE Diem SET Diem=@diem WHERE MaSinhVien=@sv AND MaMonHoc=@mh", conn);
        cmd.Parameters.AddWithValue("@diem", diem);
        cmd.Parameters.AddWithValue("@sv", txtMaSV.Text.Trim());
        cmd.Parameters.AddWithValue("@mh", txtMaMH.Text.Trim());
        int n = cmd.ExecuteNonQuery();
        MessageBox.Show(n > 0 ? "Sửa thành công." : "Không tìm thấy dòng điểm.");
    }
}
