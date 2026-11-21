using System;
using System.Data.Linq.Mapping;
using ThucHanh9;

namespace ThucHanh9.Models
{
    [Table(Name = "dbo.Sach")]
    [EntityTitle("Sách")]
    public sealed class Sach
    {
        [Input("Mã sách")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaSach { get; set; }

        [Input("Tên sách")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TenSach { get; set; }

        [Input("Tác giả")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string TacGia { get; set; }

        [Input("Nhà xuất bản")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string NhaXuatBan { get; set; }

        [Input("Năm xuất bản", Minimum = 1000, Maximum = 9999)]
        [Column(DbType = "int NOT NULL", CanBeNull = false)]
        public int NamXB { get; set; }
    }

    [Table(Name = "dbo.DocGia")]
    [EntityTitle("Độc giả")]
    public sealed class DocGia
    {
        [Input("Mã độc giả")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaDocGia { get; set; }

        [Input("Họ tên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string HoTen { get; set; }

        [Input("Địa chỉ")]
        [Column(DbType = "nvarchar(150) NOT NULL", CanBeNull = false)]
        public string DiaChi { get; set; }

        [Input("Điện thoại")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string SoDienThoai { get; set; }
    }

    [Table(Name = "dbo.PhieuMuonSach")]
    [EntityTitle("Phiếu mượn sách")]
    public sealed class PhieuMuon
    {
        [Input("Số phiếu")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string SoPhieu { get; set; }

        [Input("Độc giả", Lookup = typeof(DocGia), LookupLabel = "HoTen")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false)]
        public string MaDocGia { get; set; }

        [Input("Họ tên nhân viên")]
        [Column(DbType = "nvarchar(100) NOT NULL", CanBeNull = false)]
        public string HoTenNhanVien { get; set; }

        [Input("Ngày mượn")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayMuon { get; set; }
    }

    [Table(Name = "dbo.ChiTietPhieuMuon")]
    [EntityTitle("Chi tiết phiếu mượn")]
    public sealed class ChiTietPhieuMuon
    {
        [Input("Số phiếu", Lookup = typeof(PhieuMuon), LookupLabel = "SoPhieu")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string SoPhieu { get; set; }

        [Input("Sách", Lookup = typeof(Sach), LookupLabel = "TenSach")]
        [Column(DbType = "nvarchar(20) NOT NULL", CanBeNull = false, IsPrimaryKey = true)]
        public string MaSach { get; set; }

        [Input("Ngày hẹn trả")]
        [Column(DbType = "date NOT NULL", CanBeNull = false)]
        public DateTime NgayHenTra { get; set; }

        [Input("Ngày trả")]
        [Column(DbType = "date", CanBeNull = true)]
        public DateTime? NgayTra { get; set; }
    }
}
