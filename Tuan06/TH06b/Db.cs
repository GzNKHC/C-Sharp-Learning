using Microsoft.Data.SqlClient;

namespace TH06b;

internal static class Db
{
    public const string QLSinhVien =
        "Server=localhost,1433;Database=QLSinhVien;User Id=sa;Password=SqlServer@;TrustServerCertificate=True;Encrypt=False;";

    public const string QLCuaHang =
        "Server=localhost,1433;Database=QLCuaHang;User Id=sa;Password=SqlServer@;TrustServerCertificate=True;Encrypt=False;";

    public static SqlConnection SinhVienConn() => new(QLSinhVien);
    public static SqlConnection CuaHangConn() => new(QLCuaHang);
}
