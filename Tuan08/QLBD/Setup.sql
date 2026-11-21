-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLBD') IS NULL
    EXEC(N'CREATE DATABASE [QLBD]');
GO
USE [QLBD];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.BD', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[BD] (
        [MaBD] nvarchar(20) NOT NULL,
        [Tenbangdia] nvarchar(100) NOT NULL,
        [Soluong] int NOT NULL,
        CONSTRAINT [PK_BD] PRIMARY KEY ([MaBD]),
        CHECK ([Soluong] >= 0)
    );
    INSERT dbo.[BD] ([MaBD], [Tenbangdia], [Soluong]) VALUES
        (N'BD01', N'Nhạc hòa tấu', 12),
        (N'BD02', N'Phim tài liệu', 8),
        (N'BD03', N'Ca nhạc Việt Nam', 5);
END;

IF OBJECT_ID(N'dbo.KH', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[KH] (
        [MaKH] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [Dienthoai] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_KH] PRIMARY KEY ([MaKH])
    );
    INSERT dbo.[KH] ([MaKH], [Hoten], [Dienthoai]) VALUES
        (N'KH01', N'Nguyễn Hoàng An', N'0901234001'),
        (N'KH02', N'Trần Thị Lan', N'0901234002');
END;

IF OBJECT_ID(N'dbo.THUE', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[THUE] (
        [MaThue] nvarchar(20) NOT NULL,
        [MaKH] nvarchar(20) NOT NULL,
        [MaBD] nvarchar(20) NOT NULL,
        [Soluongthue] int NOT NULL,
        [Ngaythue] date NOT NULL,
        [Songaythue] int NULL,
        [ThanhTien] decimal(16,2) NULL,
        [NgayThanhToan] date NULL,
        CONSTRAINT [PK_THUE] PRIMARY KEY ([MaThue]),
        CONSTRAINT [FK_THUE_MaKH] FOREIGN KEY ([MaKH]) REFERENCES dbo.[KH] ([MaKH]),
        CONSTRAINT [FK_THUE_MaBD] FOREIGN KEY ([MaBD]) REFERENCES dbo.[BD] ([MaBD]),
        CHECK ([Soluongthue] >= 1),
        CHECK ([Songaythue] >= 1),
        CHECK ([ThanhTien] >= 0)
    );
    INSERT dbo.[THUE] ([MaThue], [MaKH], [MaBD], [Soluongthue], [Ngaythue], [Songaythue], [ThanhTien], [NgayThanhToan]) VALUES
        (N'T001', N'KH01', N'BD01', 2, '20261110', NULL, NULL, NULL),
        (N'T002', N'KH02', N'BD02', 1, '20261109', NULL, NULL, NULL);
END;

COMMIT TRANSACTION;
GO
