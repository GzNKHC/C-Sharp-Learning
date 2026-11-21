using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.tblXe")]
    [EntityTitle("Xe")]
    public sealed class Xe
    {
        [Input("Mã xe")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Maxe { get; set; }

        [Input("Số khung")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string Sokhung { get; set; }

        [Input("Số máy")]
        [Column(DbType = "nvarchar(30) NOT NULL", CanBeNull = false)]
        public string Somay { get; set; }

        [Input("Hãng sản xuất")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string HangSX { get; set; }

        [Input("Năm sản xuất", Minimum = 1900, Maximum = 9999)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int NamSX { get; set; }

        [Input("Số lượng", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }
    }

    [Table(Name = "dbo.tblKH")]
    [EntityTitle("Khách hàng")]
    public sealed class KhachHang
    {
        [Input("Mã khách")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaK { get; set; }

        [Input("Tên khách hàng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenK { get; set; }

        [Input("Cơ quan")]
        [Column(DbType = "nvarchar(100)", CanBeNull = true)]
        public string Coquan { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string Diachi { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Tel { get; set; }
    }

    [Table(Name = "dbo.tblYeuCau")]
    [EntityTitle("Yêu cầu thuê xe")]
    public sealed class YeuCau
    {
        [Input("Xe", Lookup = typeof(Xe), LookupLabel = "HangSX")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Maxe { get; set; }

        [Input("Khách hàng", Lookup = typeof(KhachHang), LookupLabel = "TenK")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaK { get; set; }

        [Input("Ngày mượn")]
        [Column(DbType = "date NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public DateTime Ngaymuon { get; set; }

        [Input("Ngày trả")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime Ngaytra { get; set; }
    }
}
