using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.PHONGBAN")]
    [EntityTitle("Phòng ban")]
    public sealed class PhongBan
    {
        [Input("Mã phòng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Maphong { get; set; }

        [Input("Tên phòng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Tenphong { get; set; }

        [Input("Vị trí")]
        [Column(DbType = "nvarchar(80) NOT NULL", CanBeNull = false)]
        public string Vitri { get; set; }
    }

    [Table(Name = "dbo.NGACH")]
    [EntityTitle("Ngạch")]
    public sealed class Ngach
    {
        [Input("Mã ngạch")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaNg { get; set; }

        [Input("Tên ngạch")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenNg { get; set; }
    }

    [Table(Name = "dbo.CHVU")]
    [EntityTitle("Chức vụ")]
    public sealed class ChucVu
    {
        [Input("Mã chức vụ")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaCV { get; set; }

        [Input("Tên chức vụ")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenCV { get; set; }

        [Input("Hệ số phụ cấp", Minimum = 0)]
        [Column(DbType = "decimal(8,2) NOT NULL", CanBeNull = false)]
        public decimal Hesophucap { get; set; }
    }

    [Table(Name = "dbo.HOSO")]
    [EntityTitle("Hồ sơ nhân viên")]
    public sealed class HoSo
    {
        [Input("Mã nhân viên")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string manv { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Giới tính")]
        [Column(DbType = "nvarchar(8) NOT NULL", CanBeNull = false)]
        public string Gioitinh { get; set; }

        [Input("Ngày sinh")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime Ngaysinh { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string Diachi { get; set; }

        [Input("Ngày vào ngành")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime Ngayvn { get; set; }

        [Input("Ngạch", Lookup = typeof(Ngach), LookupLabel = "TenNg")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaNg { get; set; }

        [Input("Chức vụ", Lookup = typeof(ChucVu), LookupLabel = "TenCV")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaCV { get; set; }
    }

    [Table(Name = "dbo.LUONG")]
    [EntityTitle("Lương nhân viên")]
    public sealed class Luong
    {
        [Input("Nhân viên", Lookup = typeof(HoSo), LookupLabel = "Hoten")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string manv { get; set; }

        [Input("Phòng ban", Lookup = typeof(PhongBan), LookupLabel = "Tenphong")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string maphong { get; set; }

        [Input("Hệ số lương", Minimum = 0.01)]
        [Column(DbType = "decimal(8,2) NOT NULL", CanBeNull = false)]
        public decimal Hesoluong { get; set; }

        [Input("Ngày lên lương lần cuối")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime ngayLL { get; set; }
    }
}
