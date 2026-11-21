using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.UT")]
    [EntityTitle("Danh mục ưu tiên")]
    public sealed class UuTien
    {
        [Input("Mã ưu tiên")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaUT { get; set; }

        [Input("Tên ưu tiên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenUT { get; set; }

        [Input("Điểm cộng", Minimum = 0, Maximum = 10)]
        [Column(DbType = "decimal(4,2) NOT NULL", CanBeNull = false)]
        public decimal DC { get; set; }
    }

    [Table(Name = "dbo.NH")]
    [EntityTitle("Danh sách ngành học")]
    public sealed class Nganh
    {
        [Input("Mã ngành")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaN { get; set; }

        [Input("Tên ngành")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenN { get; set; }

        [Input("Chỉ tiêu", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoCT { get; set; }
    }

    [Table(Name = "dbo.TS")]
    [EntityTitle("Thí sinh")]
    public sealed class ThiSinh
    {
        [Input("Số báo danh")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string SBD { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Ngày sinh")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NS { get; set; }

        [Input("Giới tính")]
        [Column(DbType = "nvarchar(8) NOT NULL", CanBeNull = false)]
        public string GT { get; set; }

        [Input("Quê quán")]
        [Column(DbType = "nvarchar(80) NOT NULL", CanBeNull = false)]
        public string QQ { get; set; }

        [Input("Điểm Toán", Minimum = 0, Maximum = 10)]
        [Column(DbType = "decimal(4,2)", CanBeNull = true)]
        public decimal? DT { get; set; }

        [Input("Điểm Lý", Minimum = 0, Maximum = 10)]
        [Column(DbType = "decimal(4,2)", CanBeNull = true)]
        public decimal? DL { get; set; }

        [Input("Điểm Hóa", Minimum = 0, Maximum = 10)]
        [Column(DbType = "decimal(4,2)", CanBeNull = true)]
        public decimal? DH { get; set; }

        [Input("Số phòng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SP { get; set; }

        [Input("Ưu tiên", Lookup = typeof(UuTien), LookupLabel = "TenUT")]
        [Column(DbType = "nvarchar(20)", CanBeNull = true)]
        public string MaUT { get; set; }

        [Input("Ngành học", Lookup = typeof(Nganh), LookupLabel = "TenN")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaN { get; set; }
    }
}
