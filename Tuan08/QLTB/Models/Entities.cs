using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.tblPhong")]
    [EntityTitle("Phòng ban")]
    public sealed class Phong
    {
        [Input("Mã phòng")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaPhong { get; set; }

        [Input("Tên phòng")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenPhong { get; set; }
    }

    [Table(Name = "dbo.tblNhanVien")]
    [EntityTitle("Nhân viên")]
    public sealed class NhanVien
    {
        [Input("Mã nhân viên")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaNV { get; set; }

        [Input("Tên nhân viên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenNV { get; set; }

        [Input("Chức vụ")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string ChucVu { get; set; }

        [Input("Phòng", Lookup = typeof(Phong), LookupLabel = "TenPhong")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaPhong { get; set; }
    }

    [Table(Name = "dbo.tblThietBi")]
    [EntityTitle("Thiết bị")]
    public sealed class ThietBi
    {
        [Input("Mã thiết bị")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaTB { get; set; }

        [Input("Tên thiết bị")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenTB { get; set; }

        [Input("Tình trạng")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string TinhTrang { get; set; }

        [Input("Số lượng tốt", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Ghi chú")]
        [Column(DbType = "nvarchar(255)", CanBeNull = true)]
        public string GhiChu { get; set; }

        [Input("Số lượng hỏng", Minimum = 0)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuongHong { get; set; }
    }

    [Table(Name = "dbo.tblPhieuNhap")]
    [EntityTitle("Phiếu nhập thiết bị")]
    public sealed class PhieuNhap
    {
        [Input("Mã phiếu nhập")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaPN { get; set; }

        [Input("Nhân viên", Lookup = typeof(NhanVien), LookupLabel = "TenNV")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaNV { get; set; }

        [Input("Thiết bị", Lookup = typeof(ThietBi), LookupLabel = "TenTB")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaTB { get; set; }

        [Input("Ngày nhập")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayNhap { get; set; }

        [Input("Số lượng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Tình trạng")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string TinhTrang { get; set; }
    }

    [Table(Name = "dbo.tblPhieuMuon")]
    [EntityTitle("Phiếu mượn thiết bị")]
    public sealed class PhieuMuon
    {
        [Input("Mã phiếu mượn")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaPM { get; set; }

        [Input("NV lập phiếu", Lookup = typeof(NhanVien), LookupLabel = "TenNV")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string NVLapPhieu { get; set; }

        [Input("Thiết bị", Lookup = typeof(ThietBi), LookupLabel = "TenTB")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaTB { get; set; }

        [Input("Tình trạng")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string TinhTrang { get; set; }

        [Input("Ngày mượn")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayMuon { get; set; }

        [Input("Nhân viên mượn", Lookup = typeof(NhanVien), LookupLabel = "TenNV")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string NVMuon { get; set; }

        [Input("Số lượng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Phòng", Lookup = typeof(Phong), LookupLabel = "TenPhong")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaPhong { get; set; }
    }

    [Table(Name = "dbo.tblPhieuTra")]
    [EntityTitle("Phiếu trả thiết bị")]
    public sealed class PhieuTra
    {
        [Input("Mã phiếu trả")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaPT { get; set; }

        [Input("Phiếu mượn", Lookup = typeof(PhieuMuon), LookupLabel = "MaPM")]
        [Column(DbType = "nvarchar(12)", CanBeNull = true)]
        public string MaPM { get; set; }

        [Input("Thiết bị", Lookup = typeof(ThietBi), LookupLabel = "TenTB")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaTB { get; set; }

        [Input("Nhân viên trả", Lookup = typeof(NhanVien), LookupLabel = "TenNV")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string NVTra { get; set; }

        [Input("Nhân viên nhận", Lookup = typeof(NhanVien), LookupLabel = "TenNV")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string NVNhan { get; set; }

        [Input("Ngày trả")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayTra { get; set; }

        [Input("Số lượng", Minimum = 1)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int SoLuong { get; set; }

        [Input("Tình trạng")]
        [Column(DbType = "nvarchar(50) NOT NULL", CanBeNull = false)]
        public string TinhTrang { get; set; }

        [Input("Phòng", Lookup = typeof(Phong), LookupLabel = "TenPhong")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string MaPhong { get; set; }

        [Input("Số phiếu mượn gốc")]
        [Column(DbType = "nvarchar(12) NOT NULL", CanBeNull = false)]
        public string SoPhieuMuonGoc { get; set; }
    }
}
