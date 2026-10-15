using Microsoft.Data.SqlClient;

namespace TH06a;

public class FrmLop : Form
{
    private readonly TextBox txtMaKhoa = Ui.T("txtMaKhoa", 150, 35);
    private readonly TextBox txtMaLop = Ui.T("txtMaLop", 150, 75);
    private readonly TextBox txtTenLop = Ui.T("txtTenLop", 150, 115);

    public FrmLop()
    {
        Text = "Quản lý lớp";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 240);
        Font = new Font("Segoe UI", 10F);
        Controls.AddRange(new Control[]
        {
            Ui.L("Mã khoa", 35, 35), txtMaKhoa,
            Ui.L("Mã lớp", 35, 75), txtMaLop,
            Ui.L("Tên lớp", 35, 115), txtTenLop,
            Ui.B("Thêm", 80, 175, BtnThem_Click),
            Ui.B("Xóa", 205, 175, BtnXoa_Click),
            Ui.B("Sửa", 330, 175, BtnSua_Click)
        });
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaKhoa.Text) || string.IsNullOrWhiteSpace(txtMaLop.Text) || string.IsNullOrWhiteSpace(txtTenLop.Text))
        {
            MessageBox.Show("Mã khoa, Mã lớp, Tên lớp không được để trống.");
            return;
        }
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO Lop(MaLop,TenLop,MaKhoa) VALUES(@malop,@tenlop,@makhoa)", conn);
            cmd.Parameters.AddWithValue("@malop", txtMaLop.Text.Trim());
            cmd.Parameters.AddWithValue("@tenlop", txtTenLop.Text.Trim());
            cmd.Parameters.AddWithValue("@makhoa", txtMaKhoa.Text.Trim());
            cmd.ExecuteNonQuery();
            MessageBox.Show("Thêm lớp thành công.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Thêm thất bại. Kiểm tra Mã lớp hoặc Mã khoa.\n" + ex.Message);
        }
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaLop.Text))
        {
            MessageBox.Show("Mã lớp không được để trống.");
            return;
        }
        if (!string.IsNullOrWhiteSpace(txtMaKhoa.Text) || !string.IsNullOrWhiteSpace(txtTenLop.Text))
        {
            MessageBox.Show("Khi xóa chỉ nhập Mã lớp; Mã khoa và Tên lớp để trống.");
            return;
        }
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM Lop WHERE MaLop=@malop", conn);
            cmd.Parameters.AddWithValue("@malop", txtMaLop.Text.Trim());
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show(n > 0 ? "Xóa thành công." : "Không tìm thấy lớp.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Không thể xóa, lớp có thể đang có sinh viên.\n" + ex.Message);
        }
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaLop.Text))
        {
            MessageBox.Show("Mã lớp không được để trống.");
            return;
        }
        bool coKhoa = !string.IsNullOrWhiteSpace(txtMaKhoa.Text);
        bool coTen = !string.IsNullOrWhiteSpace(txtTenLop.Text);
        if (!coKhoa && !coTen)
        {
            MessageBox.Show("Mã khoa và Tên lớp phải có ít nhất một trường có dữ liệu.");
            return;
        }

        var sets = new List<string>();
        using var conn = Db.CreateConnection();
        using var cmd = new SqlCommand { Connection = conn };
        if (coKhoa) { sets.Add("MaKhoa=@makhoa"); cmd.Parameters.AddWithValue("@makhoa", txtMaKhoa.Text.Trim()); }
        if (coTen) { sets.Add("TenLop=@tenlop"); cmd.Parameters.AddWithValue("@tenlop", txtTenLop.Text.Trim()); }
        cmd.CommandText = $"UPDATE Lop SET {string.Join(",", sets)} WHERE MaLop=@malop";
        cmd.Parameters.AddWithValue("@malop", txtMaLop.Text.Trim());
        try
        {
            conn.Open();
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show(n > 0 ? "Sửa thành công." : "Không tìm thấy lớp.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Sửa thất bại. Kiểm tra Mã khoa.\n" + ex.Message);
        }
    }
}
