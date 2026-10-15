using Microsoft.Data.SqlClient;

namespace TH06a;

public class FrmSinhVien : Form
{
    private readonly TextBox txtMaLop = Ui.T("txtMaLop", 150, 35);
    private readonly TextBox txtMaSV = Ui.T("txtMaSV", 150, 75);
    private readonly TextBox txtHoTen = Ui.T("txtHoTen", 150, 115);
    private readonly DateTimePicker dtNgaySinh = new() { Left = 150, Top = 155, Width = 220, Format = DateTimePickerFormat.Short, ShowCheckBox = true };

    public FrmSinhVien()
    {
        Text = "Quản lý sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(520, 280);
        Font = new Font("Segoe UI", 10F);
        dtNgaySinh.Checked = false;

        Controls.AddRange(new Control[]
        {
            Ui.L("Mã lớp", 35, 35), txtMaLop,
            Ui.L("Mã sinh viên", 35, 75), txtMaSV,
            Ui.L("Họ tên", 35, 115), txtHoTen,
            Ui.L("Ngày sinh", 35, 155), dtNgaySinh,
            Ui.B("Thêm", 95, 215, BtnThem_Click),
            Ui.B("Xóa", 215, 215, BtnXoa_Click),
            Ui.B("Sửa", 335, 215, BtnSua_Click)
        });
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaLop.Text) || string.IsNullOrWhiteSpace(txtMaSV.Text) ||
            string.IsNullOrWhiteSpace(txtHoTen.Text) || !dtNgaySinh.Checked)
        {
            MessageBox.Show("Mã lớp, Mã sinh viên, Họ tên và Ngày sinh không được để trống.");
            return;
        }
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("INSERT INTO SinhVien(MaSinhVien,HoTen,NgaySinh,MaLop) VALUES(@masv,@hoten,@ngaysinh,@malop)", conn);
            cmd.Parameters.AddWithValue("@masv", txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@hoten", txtHoTen.Text.Trim());
            cmd.Parameters.AddWithValue("@ngaysinh", dtNgaySinh.Value.Date);
            cmd.Parameters.AddWithValue("@malop", txtMaLop.Text.Trim());
            cmd.ExecuteNonQuery();
            MessageBox.Show("Thêm sinh viên thành công.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Thêm thất bại. Kiểm tra trùng Mã sinh viên hoặc Mã lớp có tồn tại.\n" + ex.Message);
        }
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
        {
            MessageBox.Show("Khi xóa phải nhập Mã sinh viên và Họ tên.");
            return;
        }
        if (!string.IsNullOrWhiteSpace(txtMaLop.Text) || dtNgaySinh.Checked)
        {
            MessageBox.Show("Khi xóa, Mã lớp và Ngày sinh không nhập dữ liệu.");
            return;
        }

        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM SinhVien WHERE MaSinhVien=@masv AND HoTen=@hoten", conn);
            cmd.Parameters.AddWithValue("@masv", txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@hoten", txtHoTen.Text.Trim());
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show(n > 0 ? "Xóa thành công." : "Không tìm thấy sinh viên phù hợp.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Không thể xóa, sinh viên có thể đang có dữ liệu điểm.\n" + ex.Message);
        }
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text))
        {
            MessageBox.Show("Mã sinh viên không được để trống.");
            return;
        }
        bool coMaLop = !string.IsNullOrWhiteSpace(txtMaLop.Text);
        bool coHoTen = !string.IsNullOrWhiteSpace(txtHoTen.Text);
        bool coNgaySinh = dtNgaySinh.Checked;
        if (!coMaLop && !coHoTen && !coNgaySinh)
        {
            MessageBox.Show("Phải nhập ít nhất một trong các trường: Mã lớp, Họ tên, Ngày sinh.");
            return;
        }

        var sets = new List<string>();
        using var conn = Db.CreateConnection();
        using var cmd = new SqlCommand { Connection = conn };
        if (coMaLop) { sets.Add("MaLop=@malop"); cmd.Parameters.AddWithValue("@malop", txtMaLop.Text.Trim()); }
        if (coHoTen) { sets.Add("HoTen=@hoten"); cmd.Parameters.AddWithValue("@hoten", txtHoTen.Text.Trim()); }
        if (coNgaySinh) { sets.Add("NgaySinh=@ngaysinh"); cmd.Parameters.AddWithValue("@ngaysinh", dtNgaySinh.Value.Date); }
        cmd.CommandText = $"UPDATE SinhVien SET {string.Join(",", sets)} WHERE MaSinhVien=@masv";
        cmd.Parameters.AddWithValue("@masv", txtMaSV.Text.Trim());

        try
        {
            conn.Open();
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show(n > 0 ? "Sửa thành công." : "Không tìm thấy sinh viên.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Sửa thất bại. Kiểm tra Mã lớp có tồn tại.\n" + ex.Message);
        }
    }
}
