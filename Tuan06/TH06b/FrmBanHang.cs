using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmBanHang : Form
{
    private readonly TextBox txtMaHD = Ui.T("txtMaHD", 155, 85, 170);
    private readonly ComboBox cboSP = Ui.C("cboSanPham", 155, 125, 230);
    private readonly NumericUpDown nudSoLuong = new() { Left = 155, Top = 165, Width = 130, Minimum = 1, Maximum = 100000 };
    private readonly Button btnThem;
    private readonly Button btnHuy;
    private readonly Button btnThanhToan;
    private int? maHDHienTai;

    public FrmBanHang()
    {
        Text = "Bán hàng";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(550, 315);
        Font = new Font("Segoe UI", 10F);

        txtMaHD.ReadOnly = true;
        btnThem = Ui.B("Thêm", 155, 215, BtnThem_Click, 95);
        btnHuy = Ui.B("Hủy", 265, 215, BtnHuy_Click, 95);
        btnThanhToan = Ui.B("Thanh toán", 375, 215, BtnThanhToan_Click, 110);

        Controls.AddRange(new Control[]
        {
            Ui.B("Tạo hóa đơn mới", 330, 25, BtnTaoHoaDon_Click, 155),
            Ui.L("Mã hóa đơn", 35, 85), txtMaHD,
            Ui.L("Mã sản phẩm", 35, 125), cboSP,
            Ui.L("Số lượng", 35, 165), nudSoLuong,
            btnThem, btnHuy, btnThanhToan
        });
        Load += (_, _) => SetEditing(false);
    }

    private void SetEditing(bool enabled)
    {
        cboSP.Enabled = enabled;
        nudSoLuong.Enabled = enabled;
        btnThem.Enabled = enabled;
        btnHuy.Enabled = enabled;
        btnThanhToan.Enabled = enabled;
    }

    private void LoadSanPham()
    {
        cboSP.Items.Clear();
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var cmd = new SqlCommand("SELECT MaSP FROM SANPHAM ORDER BY MaSP", conn);
        using var rd = cmd.ExecuteReader();
        while (rd.Read()) cboSP.Items.Add(rd["MaSP"].ToString());
        if (cboSP.Items.Count > 0) cboSP.SelectedIndex = 0;
    }

    private void BtnTaoHoaDon_Click(object? sender, EventArgs e)
    {
        if (maHDHienTai.HasValue)
        {
            MessageBox.Show("Đang có một hóa đơn chưa kết thúc. Hãy Thanh toán hoặc Hủy trước.");
            return;
        }
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var cmd = new SqlCommand("INSERT INTO HOADON(NgayTao,TinhTrangThanhToan) OUTPUT INSERTED.MaHD VALUES(CAST(GETDATE() AS date),0)", conn);
        maHDHienTai = Convert.ToInt32(cmd.ExecuteScalar());
        txtMaHD.Text = maHDHienTai.Value.ToString();
        LoadSanPham();
        SetEditing(true);
    }

    private int SoLuongTon(int maSP, SqlConnection conn, SqlTransaction tran)
    {
        using var cmd = new SqlCommand(@"
SELECT TOP 1 SLDau + SLNhap - SLBan
FROM TONKHO
WHERE MaSP=@sp AND NgayTon=CAST(GETDATE() AS date)
ORDER BY NgayTon DESC", conn, tran);
        cmd.Parameters.AddWithValue("@sp", maSP);
        object? value = cmd.ExecuteScalar();
        return value is null || value == DBNull.Value ? -1 : Convert.ToInt32(value);
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (!maHDHienTai.HasValue || cboSP.SelectedItem is null) return;
        int maSP = int.Parse(cboSP.SelectedItem.ToString()!);
        int sl = (int)nudSoLuong.Value;

        using var conn = Db.CuaHangConn();
        conn.Open();
        using var tran = conn.BeginTransaction();
        try
        {
            int ton = SoLuongTon(maSP, conn, tran);
            if (ton < 0) { MessageBox.Show("Chưa tạo tồn kho hôm nay cho sản phẩm này."); tran.Rollback(); return; }
            if (sl <= 0 || sl > ton) { MessageBox.Show($"Số lượng bán phải > 0 và không vượt tồn kho ({ton})."); tran.Rollback(); return; }

            using var kt = new SqlCommand("SELECT COUNT(*) FROM CHITIET_HOADON WHERE MaHD=@hd AND MaSP=@sp", conn, tran);
            kt.Parameters.AddWithValue("@hd", maHDHienTai.Value);
            kt.Parameters.AddWithValue("@sp", maSP);
            if (Convert.ToInt32(kt.ExecuteScalar()) > 0)
            {
                MessageBox.Show("Sản phẩm đã có trong hóa đơn (trùng khóa chính)." );
                tran.Rollback();
                return;
            }

            using var ct = new SqlCommand("INSERT INTO CHITIET_HOADON(MaHD,MaSP,SoLuong) VALUES(@hd,@sp,@sl)", conn, tran);
            ct.Parameters.AddWithValue("@hd", maHDHienTai.Value);
            ct.Parameters.AddWithValue("@sp", maSP);
            ct.Parameters.AddWithValue("@sl", sl);
            ct.ExecuteNonQuery();

            // Tài liệu ghi 'SLBan = SLBan - số lượng', nhưng để công thức tồn kho
            // SLDau + SLNhap - SLBan đúng nghĩa, số lượng bán cần được cộng dồn.
            using var tk = new SqlCommand("UPDATE TONKHO SET SLBan=SLBan+@sl WHERE MaSP=@sp AND NgayTon=CAST(GETDATE() AS date)", conn, tran);
            tk.Parameters.AddWithValue("@sl", sl);
            tk.Parameters.AddWithValue("@sp", maSP);
            tk.ExecuteNonQuery();

            tran.Commit();
            MessageBox.Show("Đã thêm sản phẩm vào hóa đơn.");
        }
        catch (Exception ex)
        {
            tran.Rollback();
            MessageBox.Show("Thêm chi tiết hóa đơn thất bại.\n" + ex.Message);
        }
    }

    private void BtnHuy_Click(object? sender, EventArgs e)
    {
        if (!maHDHienTai.HasValue) return;
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var tran = conn.BeginTransaction();
        try
        {
            using var restore = new SqlCommand(@"
UPDATE tk SET tk.SLBan=tk.SLBan-ct.SoLuong
FROM TONKHO tk
JOIN CHITIET_HOADON ct ON ct.MaSP=tk.MaSP
WHERE ct.MaHD=@hd AND tk.NgayTon=CAST(GETDATE() AS date);", conn, tran);
            restore.Parameters.AddWithValue("@hd", maHDHienTai.Value);
            restore.ExecuteNonQuery();

            using var delct = new SqlCommand("DELETE FROM CHITIET_HOADON WHERE MaHD=@hd", conn, tran);
            delct.Parameters.AddWithValue("@hd", maHDHienTai.Value);
            delct.ExecuteNonQuery();
            using var delhd = new SqlCommand("DELETE FROM HOADON WHERE MaHD=@hd AND TinhTrangThanhToan=0", conn, tran);
            delhd.Parameters.AddWithValue("@hd", maHDHienTai.Value);
            delhd.ExecuteNonQuery();
            tran.Commit();
            ResetHoaDon();
            MessageBox.Show("Đã hủy hóa đơn.");
        }
        catch (Exception ex)
        {
            tran.Rollback();
            MessageBox.Show("Hủy hóa đơn thất bại.\n" + ex.Message);
        }
    }

    private void BtnThanhToan_Click(object? sender, EventArgs e)
    {
        if (!maHDHienTai.HasValue) return;
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var kt = new SqlCommand("SELECT COUNT(*) FROM CHITIET_HOADON WHERE MaHD=@hd", conn);
        kt.Parameters.AddWithValue("@hd", maHDHienTai.Value);
        if (Convert.ToInt32(kt.ExecuteScalar()) == 0)
        {
            MessageBox.Show("Hóa đơn chưa có sản phẩm.");
            return;
        }
        using var cmd = new SqlCommand("UPDATE HOADON SET TinhTrangThanhToan=1 WHERE MaHD=@hd", conn);
        cmd.Parameters.AddWithValue("@hd", maHDHienTai.Value);
        cmd.ExecuteNonQuery();
        MessageBox.Show("Thanh toán thành công.");
        ResetHoaDon();
    }

    private void ResetHoaDon()
    {
        maHDHienTai = null;
        txtMaHD.Clear();
        cboSP.Items.Clear();
        nudSoLuong.Value = 1;
        SetEditing(false);
    }
}
