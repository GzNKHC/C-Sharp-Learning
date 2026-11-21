using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.Thuoc")]
    [EntityTitle("Thuốc")]
    public sealed class MatHang
    {
        [Input("Mã thuốc")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaThuoc { get; set; }

        [Input("Tên thuốc")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenThuoc { get; set; }

        [Input("Công dụng")]
        [Column(DbType = "nvarchar(120) NOT NULL", CanBeNull = false)]
        public string CongDung { get; set; }

        [Input("Ngày sản xuất")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgaySanXuat { get; set; }

        [Input("Ngày hết hạn")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayHetHanDung { get; set; }

        [Input("Số lượng sản xuất", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuongSanXuat { get; set; }

        [Input("Đơn vị tính")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string DonViTinh { get; set; }
    }

    [Table(Name = "dbo.KhachHang")]
    [EntityTitle("Khách hàng")]
    public sealed class DoiTac
    {
        [Input("Mã khách hàng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaKhachHang { get; set; }

        [Input("Tên khách hàng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenKhachHang { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string DiaChi { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string SoDienThoai { get; set; }
    }

    [Table(Name = "dbo.DonHang")]
    [EntityTitle("Đơn hàng")]
    public sealed class DonHang
    {
        [Input("Mã đơn hàng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaDonHang { get; set; }

        [Input("Mã khách hàng", Lookup = typeof(DoiTac), LookupLabel = "TenKhachHang")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaKhachHang { get; set; }

        [Input("Người lập")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string NguoiLap { get; set; }

        [Input("Ngày lập")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayLap { get; set; }

        [Input("Ngày nhận hàng")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayNhanHang { get; set; }
    }

    [Table(Name = "dbo.ChiTietDonHang")]
    [EntityTitle("Chi tiết đơn hàng")]
    public sealed class ChiTietDonHang
    {
        [Input("Đơn hàng", Lookup = typeof(DonHang), LookupLabel = "MaDonHang")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaDonHang { get; set; }

        [Input("Mã thuốc", Lookup = typeof(MatHang), LookupLabel = "TenThuoc")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaThuoc { get; set; }

        [Input("Số lượng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Đơn vị tính")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string DonViTinh { get; set; }

        [Input("Đơn giá", Minimum = 0)]
        [Column(DbType = "decimal(16,2) NOT NULL", CanBeNull = false)]
        public decimal DonGia { get; set; }
    }
}
