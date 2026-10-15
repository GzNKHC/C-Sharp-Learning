using Microsoft.Data.SqlClient;

namespace TH06a;

internal static class Db
{
    // Theo cấu hình Docker mà giảng viên hướng dẫn.
    // Nếu bạn đổi mật khẩu/port, sửa chuỗi này.
    public const string ConnectionString =
        "Server=localhost,1433;Database=QLSinhVien;User Id=sa;Password=SqlServer@;TrustServerCertificate=True;Encrypt=False;";

    public static SqlConnection CreateConnection() => new(ConnectionString);
}
