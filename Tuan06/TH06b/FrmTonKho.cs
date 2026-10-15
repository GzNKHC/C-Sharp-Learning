using Microsoft.Data.SqlClient;

namespace TH06b;

public class FrmTonKho : Form
{
    private readonly Label lbl = new() { Left = 35, Top = 95, Width = 430, Height = 70 };

    public FrmTonKho()
    {
        Text = "Tạo tồn kho";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(500, 220);
        Font = new Font("Segoe UI", 10F);
        Controls.Add(new Label { Text = "Tạo tồn kho cho ngày hiện tại (chỉ thực hiện một lần/ngày)", Left = 35, Top = 35, Width = 430 });
        Controls.Add(Ui.B("Tạo tồn kho", 35, 65, BtnTao_Click, 130));
        Controls.Add(lbl);
    }

    private void BtnTao_Click(object? sender, EventArgs e)
    {
        using var conn = Db.CuaHangConn();
        conn.Open();
        using var check = new SqlCommand("SELECT COUNT(*) FROM TONKHO WHERE NgayTon=CAST(GETDATE() AS date)", conn);
        if (Convert.ToInt32(check.ExecuteScalar()) > 0)
        {
            lbl.Text = "Tồn kho hôm nay đã được tạo trước đó.";
            return;
        }

        string sql = @"
INSERT INTO TONKHO(MaSP,NgayTon,SLDau,SLNhap,SLBan)
SELECT sp.MaSP, CAST(GETDATE() AS date),
       ISNULL(prev.SLDau + prev.SLNhap - prev.SLBan, 0), 0, 0
FROM SANPHAM sp
OUTER APPLY (
    SELECT TOP 1 tk.SLDau, tk.SLNhap, tk.SLBan
    FROM TONKHO tk
    WHERE tk.MaSP=sp.MaSP AND tk.NgayTon < CAST(GETDATE() AS date)
    ORDER BY tk.NgayTon DESC
) prev;";
        using var cmd = new SqlCommand(sql, conn);
        int n = cmd.ExecuteNonQuery();
        lbl.Text = $"Đã tạo tồn kho hôm nay cho {n} sản phẩm.";
    }
}
