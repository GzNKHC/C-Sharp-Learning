using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmSinhVien : Form
{
    private readonly ComboBox cboLop = Ui.C("cboLop", 155, 35);
    private readonly TextBox txtMaSV = Ui.T("txtMaSV", 155, 75);
    private readonly TextBox txtHoTen = Ui.T("txtHoTen", 155, 115);
    private readonly DateTimePicker dtNgaySinh = new() { Left = 155, Top = 155, Width = 220, Format = DateTimePickerFormat.Short };

    public FrmSinhVien()
    {
        Text = "Sinh viên - kiểm tra khóa chính/khóa ngoại";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(535, 280);
        Font = new Font("Segoe UI", 10F);

        Controls.AddRange(new Control[]
        {
            Ui.L("Mã lớp", 35, 35), cboLop,
            Ui.L("Mã sinh viên", 35, 75), txtMaSV,
            Ui.L("Họ tên", 35, 115), txtHoTen,
            Ui.L("Ngày sinh", 35, 155), dtNgaySinh,
            Ui.B("Thêm", 95, 215, BtnThem_Click),
            Ui.B("Xóa", 220, 215, BtnXoa_Click),
            Ui.B("Sửa", 345, 215, BtnSua_Click)
        });
        Load += FrmSinhVien_Load;
    }

    private void FrmSinhVien_Load(object? sender, EventArgs e) => LoadLop();

    private void LoadLop()
    {
        cboLop.Items.Clear();
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT MaLop FROM Lop ORDER BY MaLop", conn);
        using var rd = cmd.ExecuteReader();
        while (rd.Read()) cboLop.Items.Add(rd["MaLop"].ToString());
        if (cboLop.Items.Count > 0) cboLop.SelectedIndex = 0;
    }

    private bool MaSinhVienChuaTonTai(string ma)
    {
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM SinhVien WHERE MaSinhVien=@ma", conn);
        cmd.Parameters.AddWithValue("@ma", ma);
        return Convert.ToInt32(cmd.ExecuteScalar()) == 0;
    }

    private bool MaSinhVienTonTai(string ma)
    {
        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT COUNT(*) FROM SinhVien WHERE MaSinhVien=@ma", conn);
        cmd.Parameters.AddWithValue("@ma", ma);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (cboLop.SelectedItem is null || string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
        {
            MessageBox.Show("Tất cả thông tin phải được nhập dữ liệu.");
            return;
        }
        if (DateTime.Today.Year - dtNgaySinh.Value.Year < 17)
        {
            MessageBox.Show("Sinh viên phải từ 17 tuổi trở lên (chỉ tính theo năm sinh).");
            return;
        }
        if (!MaSinhVienChuaTonTai(txtMaSV.Text.Trim()))
        {
            MessageBox.Show("Trùng Mã sinh viên.");
            return;
        }

        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("INSERT INTO SinhVien(MaSinhVien,HoTen,NgaySinh,MaLop) VALUES(@sv,@ten,@ngay,@lop)", conn);
        cmd.Parameters.AddWithValue("@sv", txtMaSV.Text.Trim());
        cmd.Parameters.AddWithValue("@ten", txtHoTen.Text.Trim());
        cmd.Parameters.AddWithValue("@ngay", dtNgaySinh.Value.Date);
        cmd.Parameters.AddWithValue("@lop", cboLop.SelectedItem!.ToString()!);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Thêm sinh viên thành công.");
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        string ma = txtMaSV.Text.Trim();
        if (ma == "") { MessageBox.Show("Phải nhập Mã sinh viên."); return; }
        if (!MaSinhVienTonTai(ma)) { MessageBox.Show("Mã sinh viên không tồn tại."); return; }

        try
        {
            using var conn = Db.SinhVienConn();
            conn.Open();
            using var check = new SqlCommand("SELECT COUNT(*) FROM Diem WHERE MaSinhVien=@sv", conn);
            check.Parameters.AddWithValue("@sv", ma);
            if (Convert.ToInt32(check.ExecuteScalar()) > 0)
            {
                MessageBox.Show("Không thể xóa: sinh viên đang được tham chiếu trong bảng Điểm.");
                return;
            }
            using var cmd = new SqlCommand("DELETE FROM SinhVien WHERE MaSinhVien=@sv", conn);
            cmd.Parameters.AddWithValue("@sv", ma);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Xóa sinh viên thành công.");
        }
        catch (SqlException ex) { MessageBox.Show("Xóa thất bại.\n" + ex.Message); }
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        string ma = txtMaSV.Text.Trim();
        if (ma == "") { MessageBox.Show("Phải nhập Mã sinh viên."); return; }
        if (!MaSinhVienTonTai(ma)) { MessageBox.Show("Mã sinh viên không tồn tại."); return; }
        if (cboLop.SelectedItem is null || string.IsNullOrWhiteSpace(txtHoTen.Text))
        {
            MessageBox.Show("Mã lớp và Họ tên phải có dữ liệu khi sửa trong form này.");
            return;
        }

        using var conn = Db.SinhVienConn();
        conn.Open();
        using var cmd = new SqlCommand("UPDATE SinhVien SET MaLop=@lop,HoTen=@ten,NgaySinh=@ngay WHERE MaSinhVien=@sv", conn);
        cmd.Parameters.AddWithValue("@lop", cboLop.SelectedItem!.ToString()!);
        cmd.Parameters.AddWithValue("@ten", txtHoTen.Text.Trim());
        cmd.Parameters.AddWithValue("@ngay", dtNgaySinh.Value.Date);
        cmd.Parameters.AddWithValue("@sv", ma);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Sửa sinh viên thành công.");
    }
}
