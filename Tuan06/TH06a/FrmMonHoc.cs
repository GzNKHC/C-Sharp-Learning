using Microsoft.Data.SqlClient;
using System.Data;

namespace TH06a;

public class FrmMonHoc : Form
{
    private readonly TextBox txtMa = Ui.T("txtMaMonHoc", 145, 45);
    private readonly TextBox txtTen = Ui.T("txtTenMonHoc", 145, 85);

    public FrmMonHoc()
    {
        Text = "Quản lý môn học";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 210);
        Font = new Font("Segoe UI", 10F);

        Controls.AddRange(new Control[]
        {
            Ui.L("Mã môn học", 35, 45), txtMa,
            Ui.L("Tên môn học", 35, 85), txtTen,
            Ui.B("Thêm", 80, 140, BtnThem_Click),
            Ui.B("Xóa", 205, 140, BtnXoa_Click),
            Ui.B("Sửa", 330, 140, BtnSua_Click)
        });
    }

    private bool DuLieuThemSuaHopLe()
    {
        if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text))
        {
            MessageBox.Show("Mã môn học và Tên môn học không được để trống.");
            return false;
        }
        return true;
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (!DuLieuThemSuaHopLe()) return;
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            const string sql = "INSERT INTO MonHoc(MaMonHoc, TenMonHoc) VALUES(@ma, @ten)";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ma", txtMa.Text.Trim());
            cmd.Parameters.AddWithValue("@ten", txtTen.Text.Trim());
            cmd.ExecuteNonQuery();
            MessageBox.Show("Thêm môn học thành công.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Không thể thêm dữ liệu. Có thể mã môn học đã tồn tại.\n" + ex.Message);
        }
    }

    private void BtnXoa_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtMa.Text))
        {
            MessageBox.Show("Mã môn học không được để trống.");
            txtMa.Focus();
            return;
        }
        if (!string.IsNullOrWhiteSpace(txtTen.Text))
        {
            MessageBox.Show("Khi xóa chỉ nhập Mã môn học; Tên môn học để trống.");
            return;
        }
        try
        {
            using var conn = Db.CreateConnection();
            conn.Open();
            using var cmd = new SqlCommand("DELETE FROM MonHoc WHERE MaMonHoc=@ma", conn);
            cmd.Parameters.AddWithValue("@ma", txtMa.Text.Trim());
            int n = cmd.ExecuteNonQuery();
            MessageBox.Show(n > 0 ? "Xóa thành công." : "Không tìm thấy mã môn học.");
        }
        catch (SqlException ex)
        {
            MessageBox.Show("Không thể xóa. Môn học có thể đang được tham chiếu trong bảng Điểm.\n" + ex.Message);
        }
    }

    private void BtnSua_Click(object? sender, EventArgs e)
    {
        if (!DuLieuThemSuaHopLe()) return;
        using var conn = Db.CreateConnection();
        conn.Open();
        using var cmd = new SqlCommand("UPDATE MonHoc SET TenMonHoc=@ten WHERE MaMonHoc=@ma", conn);
        cmd.Parameters.AddWithValue("@ten", txtTen.Text.Trim());
        cmd.Parameters.AddWithValue("@ma", txtMa.Text.Trim());
        int n = cmd.ExecuteNonQuery();
        MessageBox.Show(n > 0 ? "Sửa thành công." : "Không tìm thấy mã môn học.");
    }
}
