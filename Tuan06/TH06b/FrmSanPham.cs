using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmSanPham : Form
{
    private readonly TextBox txtTen = Ui.T("txtTenSP", 145, 55, 260);

    public FrmSanPham()
    {
        Text = "Thêm sản phẩm mới";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(480, 190);
        Font = new Font("Segoe UI", 10F);
        Controls.AddRange(new Control[]
        {
            Ui.L("Tên sản phẩm", 30, 55), txtTen,
            Ui.B("Thêm", 145, 105, BtnThem_Click, 100)
        });
    }

    private void BtnThem_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show("Tên sản phẩm không được để trống."); return; }
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var tran = conn.BeginTransaction();
        try
        {
            using var cmd = new SqlCommand("INSERT INTO SANPHAM(TenSP) OUTPUT INSERTED.MaSP VALUES(@ten)", conn, tran);
            cmd.Parameters.AddWithValue("@ten", txtTen.Text.Trim());
            int ma = Convert.ToInt32(cmd.ExecuteScalar());

            using var ton = new SqlCommand("INSERT INTO TONKHO(MaSP,NgayTon,SLDau,SLNhap,SLBan) VALUES(@ma,CAST(GETDATE() AS date),0,0,0)", conn, tran);
            ton.Parameters.AddWithValue("@ma", ma);
            ton.ExecuteNonQuery();
            tran.Commit();
            MessageBox.Show($"Thêm sản phẩm thành công. Mã sản phẩm: {ma}");
            txtTen.Clear();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            MessageBox.Show("Thêm sản phẩm thất bại.\n" + ex.Message);
        }
    }
}
