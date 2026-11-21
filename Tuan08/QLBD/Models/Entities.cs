using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.BD")]
    [EntityTitle("Băng đĩa")]
    public sealed class BangDia
    {
        [Input("Mã băng đĩa")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaBD { get; set; }

        [Input("Tên băng đĩa")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Tenbangdia { get; set; }

        [Input("Số lượng", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int Soluong { get; set; }
    }

    [Table(Name = "dbo.KH")]
    [EntityTitle("Khách hàng")]
    public sealed class KhachHang
    {
        [Input("Mã khách hàng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaKH { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Dienthoai { get; set; }
    }

    [Table(Name = "dbo.THUE")]
    [EntityTitle("Chi tiết thuê")]
    public sealed class Thue
    {
        [Input("Mã lượt thuê")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaThue { get; set; }

        [Input("Khách hàng", Lookup = typeof(KhachHang), LookupLabel = "Hoten")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaKH { get; set; }

        [Input("Băng đĩa", Lookup = typeof(BangDia), LookupLabel = "Tenbangdia")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaBD { get; set; }

        [Input("Số lượng thuê", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int Soluongthue { get; set; }

        [Input("Ngày thuê")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime Ngaythue { get; set; }

        [Input("Số ngày thuê", Minimum = 1)]
        [Column(DbType = "int", CanBeNull = true)]
        public int? Songaythue { get; set; }

        [Input("Thành tiền", Minimum = 0)]
        [Column(DbType = "decimal(16,2)", CanBeNull = true)]
        public decimal? ThanhTien { get; set; }

        [Input("Ngày thanh toán")]
        [Column(DbType = "date", CanBeNull = true)]
        public DateTime? NgayThanhToan { get; set; }
    }
}
