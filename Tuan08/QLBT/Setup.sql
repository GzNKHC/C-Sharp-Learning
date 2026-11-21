-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLBT') IS NULL
    EXEC(N'CREATE DATABASE [QLBT]');
GO
USE [QLBT];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.Thuoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Thuoc] (
        [MaThuoc] nvarchar(20) NOT NULL,
        [TenThuoc] nvarchar(100) NOT NULL,
        [CongDung] nvarchar(120) NOT NULL,
        [NgaySanXuat] date NOT NULL,
        [NgayHetHanDung] date NOT NULL,
        [SoLuongSanXuat] int NOT NULL,
        [DonViTinh] nvarchar(30) NOT NULL,
        CONSTRAINT [PK_Thuoc] PRIMARY KEY ([MaThuoc]),
        CHECK ([SoLuongSanXuat] >= 0),
        CHECK ([NgayHetHanDung] > [NgaySanXuat])
    );
    INSERT dbo.[Thuoc] ([MaThuoc], [TenThuoc], [CongDung], [NgaySanXuat], [NgayHetHanDung], [SoLuongSanXuat], [DonViTinh]) VALUES
        (N'T01', N'Mặt hàng mẫu A', N'Nhóm A', '20250110', '20290110', 120, N'Hộp'),
        (N'T02', N'Mặt hàng mẫu B', N'Nhóm B', '20250212', '20290212', 80, N'Vỉ');
END;

IF OBJECT_ID(N'dbo.KhachHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[KhachHang] (
        [MaKhachHang] nvarchar(20) NOT NULL,
        [TenKhachHang] nvarchar(100) NOT NULL,
        [DiaChi] nvarchar(150) NOT NULL,
        [SoDienThoai] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_KhachHang] PRIMARY KEY ([MaKhachHang])
    );
    INSERT dbo.[KhachHang] ([MaKhachHang], [TenKhachHang], [DiaChi], [SoDienThoai]) VALUES
        (N'KH001', N'Nguyễn Minh Anh', N'TP. Hồ Chí Minh', N'0901234101'),
        (N'KH002', N'Trần Thanh Bình', N'Đồng Nai', N'0901234102');
END;

IF OBJECT_ID(N'dbo.DonHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DonHang] (
        [MaDonHang] nvarchar(20) NOT NULL,
        [MaKhachHang] nvarchar(20) NOT NULL,
        [NguoiLap] nvarchar(100) NOT NULL,
        [NgayLap] date NOT NULL,
        [NgayNhanHang] date NOT NULL,
        CONSTRAINT [PK_DonHang] PRIMARY KEY ([MaDonHang]),
        CONSTRAINT [FK_DonHang_MaKhachHang] FOREIGN KEY ([MaKhachHang]) REFERENCES dbo.[KhachHang] ([MaKhachHang]),
        CHECK ([NgayNhanHang] > [NgayLap])
    );
    INSERT dbo.[DonHang] ([MaDonHang], [MaKhachHang], [NguoiLap], [NgayLap], [NgayNhanHang]) VALUES
        (N'DH01', N'KH001', N'Nguyễn Minh Anh', '20261010', '20261017'),
        (N'DH02', N'KH002', N'Lê Gia Bảo', '20261015', '20261022');
END;

IF OBJECT_ID(N'dbo.ChiTietDonHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[ChiTietDonHang] (
        [MaDonHang] nvarchar(20) NOT NULL,
        [MaThuoc] nvarchar(20) NOT NULL,
        [SoLuong] int NOT NULL,
        [DonViTinh] nvarchar(30) NOT NULL,
        [DonGia] decimal(16,2) NOT NULL,
        CONSTRAINT [PK_ChiTietDonHang] PRIMARY KEY ([MaDonHang], [MaThuoc]),
        CONSTRAINT [FK_ChiTietDonHang_MaDonHang] FOREIGN KEY ([MaDonHang]) REFERENCES dbo.[DonHang] ([MaDonHang]) ON DELETE CASCADE,
        CONSTRAINT [FK_ChiTietDonHang_MaThuoc] FOREIGN KEY ([MaThuoc]) REFERENCES dbo.[Thuoc] ([MaThuoc]),
        CHECK ([SoLuong] >= 1),
        CHECK ([DonGia] >= 0)
    );
    INSERT dbo.[ChiTietDonHang] ([MaDonHang], [MaThuoc], [SoLuong], [DonViTinh], [DonGia]) VALUES
        (N'DH01', N'T01', 3, N'Hộp', 25000),
        (N'DH02', N'T02', 5, N'Vỉ', 9000);
END;

COMMIT TRANSACTION;
GO
