-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLBH') IS NULL
    EXEC(N'CREATE DATABASE [QLBH]');
GO
USE [QLBH];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.MatHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[MatHang] (
        [MaMatHang] nvarchar(20) NOT NULL,
        [TenMatHang] nvarchar(100) NOT NULL,
        [XuatXu] nvarchar(60) NOT NULL,
        [DonViTinh] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_MatHang] PRIMARY KEY ([MaMatHang])
    );
    INSERT dbo.[MatHang] ([MaMatHang], [TenMatHang], [XuatXu], [DonViTinh]) VALUES
        (N'MH01', N'Màn hình văn phòng', N'Việt Nam', N'Chiếc'),
        (N'MH02', N'Bàn phím có dây', N'Trung Quốc', N'Chiếc'),
        (N'MH03', N'Chuột không dây', N'Việt Nam', N'Chiếc');
END;

IF OBJECT_ID(N'dbo.DaiLy', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DaiLy] (
        [MaDaiLy] nvarchar(20) NOT NULL,
        [TenDaiLy] nvarchar(100) NOT NULL,
        [DiaChi] nvarchar(150) NOT NULL,
        [SoDienThoai] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_DaiLy] PRIMARY KEY ([MaDaiLy])
    );
    INSERT dbo.[DaiLy] ([MaDaiLy], [TenDaiLy], [DiaChi], [SoDienThoai]) VALUES
        (N'DL01', N'Đại lý An Phát', N'TP. Hồ Chí Minh', N'02873008001'),
        (N'DL02', N'Đại lý Bình Minh', N'Đồng Nai', N'02517300800');
END;

IF OBJECT_ID(N'dbo.DonHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DonHang] (
        [MaDonHang] nvarchar(20) NOT NULL,
        [MaDaiLy] nvarchar(20) NOT NULL,
        [NguoiLap] nvarchar(100) NOT NULL,
        [NgayLap] date NOT NULL,
        [NgayNhanHang] date NOT NULL,
        CONSTRAINT [PK_DonHang] PRIMARY KEY ([MaDonHang]),
        CONSTRAINT [FK_DonHang_MaDaiLy] FOREIGN KEY ([MaDaiLy]) REFERENCES dbo.[DaiLy] ([MaDaiLy]),
        CHECK ([NgayNhanHang] > [NgayLap])
    );
    INSERT dbo.[DonHang] ([MaDonHang], [MaDaiLy], [NguoiLap], [NgayLap], [NgayNhanHang]) VALUES
        (N'DH01', N'DL01', N'Nguyễn Minh Anh', '20261010', '20261017'),
        (N'DH02', N'DL02', N'Lê Gia Bảo', '20261015', '20261022');
END;

IF OBJECT_ID(N'dbo.ChiTietDonHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[ChiTietDonHang] (
        [MaDonHang] nvarchar(20) NOT NULL,
        [MaMatHang] nvarchar(20) NOT NULL,
        [SoLuong] int NOT NULL,
        [Gia] decimal(16,2) NOT NULL,
        CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY ([MaDonHang], [MaMatHang]),
        CONSTRAINT [FK_ChiTietDonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES dbo.[DonHang] ([MaDonHang]) ON DELETE CASCADE,
        CONSTRAINT [FK_ChiTietDonHang_MaMatHang] FOREIGN KEY ([MaMatHang]) REFERENCES dbo.[MatHang] ([MaMatHang]),
        CHECK ([SoLuong] >= 1),
        CHECK ([Gia] >= 0)
    );
    INSERT dbo.[ChiTietDonHang] ([MaDonHang], [MaMatHang], [SoLuong], [Gia]) VALUES
        (N'DH01', N'MH01', 10, 2750000),
        (N'DH02', N'MH02', 20, 210000);
END;

COMMIT TRANSACTION;
GO
