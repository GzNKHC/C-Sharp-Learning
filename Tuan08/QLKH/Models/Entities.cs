using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.HANGSX")]
    [EntityTitle("Hãng sản xuất")]
    public sealed class Hang
    {
        [Input("Mã hãng")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string mahang { get; set; }

        [Input("Tên hãng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string tenhang { get; set; }
    }

    [Table(Name = "dbo.DIENTHOAI")]
    [EntityTitle("Điện thoại")]
    public sealed class DienThoai
    {
        [Input("Mã điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string ma { get; set; }

        [Input("Tên điện thoại")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string ten { get; set; }

        [Input("Đơn giá", Minimum = 0)]
        [Column(DbType = "decimal(16,2) NOT NULL", CanBeNull = false)]
        public decimal dongia { get; set; }

        [Input("Tồn kho", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int tonkho { get; set; }

        [Input("Hãng sản xuất", Lookup = typeof(Hang), LookupLabel = "tenhang")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string mahang { get; set; }
    }
}
