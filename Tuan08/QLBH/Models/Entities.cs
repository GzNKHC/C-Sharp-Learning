using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.MatHang")]
    [EntityTitle("Mặt hàng")]
    public sealed class MatHang
    {
        [Input("Mã mặt hàng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaMatHang { get; set; }

        [Input("Tên mặt hàng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenMatHang { get; set; }

        [Input("Xuất xứ")]
        [Column(DbType = "nvarchar(60) NOT NULL", CanBeNull = false)]
        public string XuatXu { get; set; }

        [Input("Đơn vị tính")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string DonViTinh { get; set; }
    }

    [Table(Name = "dbo.DaiLy")]
    [EntityTitle("Đại lý")]
    public sealed class DoiTac
    {
        [Input("Mã đại lý")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaDaiLy { get; set; }

        [Input("Tên đại lý")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenDaiLy { get; set; }

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

        [Input("Mã đại lý", Lookup = typeof(DoiTac), LookupLabel = "TenDaiLy")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaDaiLy { get; set; }

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

        [Input("Mã mặt hàng", Lookup = typeof(MatHang), LookupLabel = "TenMatHang")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaMatHang { get; set; }

        [Input("Số lượng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Đơn giá", Minimum = 0)]
        [Column(DbType = "decimal(16,2) NOT NULL", CanBeNull = false)]
        public decimal Gia { get; set; }
    }
}
