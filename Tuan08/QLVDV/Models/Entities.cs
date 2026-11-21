using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.tblThanhvien")]
    [EntityTitle("Thành viên")]
    public sealed class ThanhVien
    {
        [Input("Mã thành viên")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaThanhvien { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Ngày sinh")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime Ngaysinh { get; set; }

        [Input("Giới tính")]
        [Column(DbType = "nvarchar(8) NOT NULL", CanBeNull = false)]
        public string Gioitinh { get; set; }

        [Input("Đơn vị")]
        [Column(DbType = "nvarchar(60) NOT NULL", CanBeNull = false)]
        public string Donvi { get; set; }

        [Input("Sở thích")]
        [Column(DbType = "nvarchar(100)", CanBeNull = true)]
        public string Sothich { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string Diachi { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Dienthoai { get; set; }

        [Input("Quyền hạn")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string Quyenhan { get; set; }
    }

    [Table(Name = "dbo.tblHoatdong")]
    [EntityTitle("Hoạt động")]
    public sealed class HoatDong
    {
        [Input("Mã hoạt động")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Mahoatdong { get; set; }

        [Input("Tên hoạt động")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Tenhoatdong { get; set; }

        [Input("Nội dung")]
        [Column(DbType = "nvarchar(255) NOT NULL", CanBeNull = false)]
        public string Noidung { get; set; }

        [Input("Thời gian")]
        [Column(DbType = "datetime2(0) NOT NULL", CanBeNull = false)]
        public DateTime Thoigian { get; set; }

        [Input("Kinh phí", Minimum = 0)]
        [Column(DbType = "decimal(16,2) NOT NULL", CanBeNull = false)]
        public decimal Kinhphi { get; set; }
    }

    [Table(Name = "dbo.tblPhieu")]
    [EntityTitle("Hoạt động của thành viên")]
    public sealed class Phieu
    {
        [Input("Hoạt động", Lookup = typeof(HoatDong), LookupLabel = "Tenhoatdong")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Mahoatdong { get; set; }

        [Input("Thành viên", Lookup = typeof(ThanhVien), LookupLabel = "Hoten")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Mathanhvien { get; set; }

        [Input("Bắt đầu")]
        [Column(DbType = "datetime2(0) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public DateTime ThoigianBD { get; set; }

        [Input("Kết thúc")]
        [Column(DbType = "datetime2(0) NOT NULL", CanBeNull = false)]
        public DateTime ThoigianKT { get; set; }

        [Input("Đánh giá")]
        [Column(DbType = "nvarchar(150)", CanBeNull = true)]
        public string Danhgia { get; set; }
    }
}
