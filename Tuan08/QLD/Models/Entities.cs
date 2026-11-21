using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.Lop")]
    [EntityTitle("Danh mục lớp")]
    public sealed class Lop
    {
        [Input("Mã lớp")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaL { get; set; }

        [Input("Tên lớp")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenL { get; set; }
    }

    [Table(Name = "dbo.SV")]
    [EntityTitle("Sinh viên")]
    public sealed class SinhVien
    {
        [Input("Mã sinh viên")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaSV { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string Hoten { get; set; }

        [Input("Giới tính")]
        [Column(DbType = "nvarchar(8) NOT NULL", CanBeNull = false)]
        public string GT { get; set; }

        [Input("Ngày sinh")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NS { get; set; }

        [Input("Quê quán")]
        [Column(DbType = "nvarchar(80) NOT NULL", CanBeNull = false)]
        public string QQ { get; set; }

        [Input("Lớp", Lookup = typeof(Lop), LookupLabel = "TenL")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaL { get; set; }
    }

    [Table(Name = "dbo.HP")]
    [EntityTitle("Học phần")]
    public sealed class HocPhan
    {
        [Input("Mã học phần")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaHP { get; set; }

        [Input("Tên học phần")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenHP { get; set; }

        [Input("Số tín chỉ", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoTC { get; set; }

        [Input("Học kỳ", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int HK { get; set; }
    }

    [Table(Name = "dbo.DangKyHocPhan")]
    [EntityTitle("Đăng ký học phần")]
    public sealed class DangKy
    {
        [Input("Sinh viên", Lookup = typeof(SinhVien), LookupLabel = "Hoten")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaSV { get; set; }

        [Input("Học phần", Lookup = typeof(HocPhan), LookupLabel = "TenHP")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaHP { get; set; }
    }

    [Table(Name = "dbo.Diem")]
    [EntityTitle("Điểm học phần")]
    public sealed class KetQua
    {
        [Input("Sinh viên", Lookup = typeof(SinhVien), LookupLabel = "Hoten")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaSV { get; set; }

        [Input("Học phần", Lookup = typeof(HocPhan), LookupLabel = "TenHP")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaHP { get; set; }

        [Input("Điểm", Minimum = 0, Maximum = 10)]
        [Column(DbType = "decimal(4,2) NOT NULL", CanBeNull = false)]
        public decimal Diem { get; set; }
    }
}
