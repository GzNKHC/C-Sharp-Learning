using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.COSO")]
    [EntityTitle("Cơ sở")]
    public sealed class CoSo
    {
        [Input("Mã cơ sở")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Macoso { get; set; }

        [Input("Tên cơ sở")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Tencoso { get; set; }
    }

    [Table(Name = "dbo.DONVI")]
    [EntityTitle("Đơn vị đào tạo")]
    public sealed class DonVi
    {
        [Input("Mã đơn vị")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Madonvi { get; set; }

        [Input("Tên đơn vị")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Tendonvi { get; set; }

        [Input("Mã cơ sở", Lookup = typeof(CoSo), LookupLabel = "Tencoso")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Macoso { get; set; }
    }

    [Table(Name = "dbo.GV")]
    [EntityTitle("Giáo viên")]
    public sealed class GiaoVien
    {
        [Input("Mã giáo viên")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string Magv { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Sdt { get; set; }

        [Input("Ghi chú")]
        [Column(DbType = "nvarchar(255)", CanBeNull = true)]
        public string Ghichu { get; set; }

        [Input("Đơn vị", Lookup = typeof(DonVi), LookupLabel = "Tendonvi")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string Madonvi { get; set; }
    }
}
