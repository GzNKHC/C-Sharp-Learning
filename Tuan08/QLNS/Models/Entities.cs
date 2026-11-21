using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.tblPhongBan")]
    [EntityTitle("Phòng ban")]
    public sealed class PhongBan
    {
        [Input("Mã phòng ban")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaPhongBan { get; set; }

        [Input("Tên phòng ban")]
        [Column(DbType = "nvarchar(25) NOT NULL", CanBeNull = false)]
        public string TenPhongBan { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string SoDTPB { get; set; }
    }

    [Table(Name = "dbo.tblChucVu")]
    [EntityTitle("Chức vụ")]
    public sealed class ChucVu
    {
        [Input("Mã chức vụ")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaChucVu { get; set; }

        [Input("Tên chức vụ")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string TenChucVu { get; set; }

        [Input("Phụ cấp", Minimum = 0)]
        [Column(DbType = "decimal(16,2) NOT NULL", CanBeNull = false)]
        public decimal PhuCapChucVu { get; set; }
    }

    [Table(Name = "dbo.tblNhanVien")]
    [EntityTitle("Nhân viên")]
    public sealed class NhanVien
    {
        [Input("Mã nhân viên")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaNhanVien { get; set; }

        [Input("Họ và tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string HoTenNhanVien { get; set; }

        [Input("Phòng ban", Lookup = typeof(PhongBan), LookupLabel = "TenPhongBan")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaPhongBan { get; set; }

        [Input("Chức vụ", Lookup = typeof(ChucVu), LookupLabel = "TenChucVu")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaChucVu { get; set; }

        [Input("Số CMND/CCCD")]
        [Column(DbType = "nvarchar(15)", CanBeNull = true)]
        public string SCMND { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string DiaChi { get; set; }

        [Input("Giới tính")]
        [Column(DbType = "nvarchar(8) NOT NULL", CanBeNull = false)]
        public string GioiTinh { get; set; }

        [Input("Quê quán")]
        [Column(DbType = "nvarchar(80) NOT NULL", CanBeNull = false)]
        public string QueQuan { get; set; }

        [Input("Ngày sinh")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgaySinh { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string SoDT { get; set; }

        [Input("Ngày vào làm")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayVaoLam { get; set; }

        [Input("Số BHYT")]
        [Column(DbType = "nvarchar(20)", CanBeNull = true)]
        public string SoBHYT { get; set; }

        [Input("Số BHXH")]
        [Column(DbType = "nvarchar(20)", CanBeNull = true)]
        public string SoBHXH { get; set; }

        [Input("Hình ảnh")]
        [Column(DbType = "nvarchar(255)", CanBeNull = true)]
        public string HinhAnh { get; set; }

        [Input("Ghi chú")]
        [Column(DbType = "nvarchar(255)", CanBeNull = true)]
        public string GhiChu { get; set; }
    }
}
