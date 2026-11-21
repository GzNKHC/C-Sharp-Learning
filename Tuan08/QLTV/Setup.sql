-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLTV') IS NULL
    EXEC(N'CREATE DATABASE [QLTV]');
GO
USE [QLTV];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.Sach', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Sach] (
        [MaSach] nvarchar(20) NOT NULL,
        [TenSach] nvarchar(100) NOT NULL,
        [TacGia] nvarchar(100) NOT NULL,
        [NhaXuatBan] nvarchar(100) NOT NULL,
        [NamXB] int NOT NULL,
        CONSTRAINT [PK_Sach] PRIMARY KEY ([MaSach]),
        CHECK ([NamXB] >= 1000 AND [NamXB] <= 9999)
    );
    INSERT dbo.[Sach] ([MaSach], [TenSach], [TacGia], [NhaXuatBan], [NamXB]) VALUES
        (N'S01', N'Cơ sở dữ liệu', N'Lê Xuân Liên', N'Giáo dục', 2019),
        (N'S02', N'Cơ sở dữ liệu', N'Nguyễn Mạnh Tường', N'Đại học Quốc gia', 2022),
        (N'S03', N'Lập trình C#', N'Trần Minh Anh', N'Khoa học và Kỹ thuật', 2023);
END;

IF OBJECT_ID(N'dbo.DocGia', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DocGia] (
        [MaDocGia] nvarchar(20) NOT NULL,
        [HoTen] nvarchar(100) NOT NULL,
        [DiaChi] nvarchar(150) NOT NULL,
        [SoDienThoai] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_DocGia] PRIMARY KEY ([MaDocGia])
    );
    INSERT dbo.[DocGia] ([MaDocGia], [HoTen], [DiaChi], [SoDienThoai]) VALUES
        (N'DG01', N'Nguyễn Minh Anh', N'TP. Hồ Chí Minh', N'0901234101'),
        (N'DG02', N'Trần Thanh Bình', N'Đồng Nai', N'0901234102');
END;

IF OBJECT_ID(N'dbo.PhieuMuonSach', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[PhieuMuonSach] (
        [SoPhieu] nvarchar(20) NOT NULL,
        [MaDocGia] nvarchar(20) NOT NULL,
        [HoTenNhanVien] nvarchar(100) NOT NULL,
        [NgayMuon] date NOT NULL,
        CONSTRAINT [PK_PhieuMuonSach] PRIMARY KEY ([SoPhieu]),
        CONSTRAINT [FK_PhieuMuonSach_MaDocGia] FOREIGN KEY ([MaDocGia]) REFERENCES dbo.[DocGia] ([MaDocGia])
    );
    INSERT dbo.[PhieuMuonSach] ([SoPhieu], [MaDocGia], [HoTenNhanVien], [NgayMuon]) VALUES
        (N'PM001', N'DG01', N'Lê Gia Bảo', '20261005'),
        (N'PM002', N'DG02', N'Lê Gia Bảo', '20261009');
END;

IF OBJECT_ID(N'dbo.ChiTietPhieuMuon', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[ChiTietPhieuMuon] (
        [SoPhieu] nvarchar(20) NOT NULL,
        [MaSach] nvarchar(20) NOT NULL,
        [NgayHenTra] date NOT NULL,
        [NgayTra] date NULL,
        CONSTRAINT [PK_ChiTietPhieuMuon] PRIMARY KEY ([SoPhieu], [MaSach]),
        CONSTRAINT [FK_ChiTietPhieuMuon_SoPhieu] FOREIGN KEY ([SoPhieu]) REFERENCES dbo.[PhieuMuonSach] ([SoPhieu]),
        CONSTRAINT [FK_ChiTietPhieuMuon_MaSach] FOREIGN KEY ([MaSach]) REFERENCES dbo.[Sach] ([MaSach]) ON DELETE CASCADE
    );
    INSERT dbo.[ChiTietPhieuMuon] ([SoPhieu], [MaSach], [NgayHenTra], [NgayTra]) VALUES
        (N'PM001', N'S01', '20261019', NULL),
        (N'PM002', N'S02', '20261023', '20261015');
END;

COMMIT TRANSACTION;
GO
