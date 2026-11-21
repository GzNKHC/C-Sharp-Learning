-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLTX') IS NULL
    EXEC(N'CREATE DATABASE [QLTX]');
GO
USE [QLTX];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.tblXe', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblXe] (
        [Maxe] nvarchar(20) NOT NULL,
        [Sokhung] nvarchar(30) NOT NULL,
        [Somay] nvarchar(30) NOT NULL,
        [HangSX] nvarchar(50) NOT NULL,
        [NamSX] int NOT NULL,
        [SoLuong] int NOT NULL,
        CONSTRAINT [PK_tblXe] PRIMARY KEY ([Maxe]),
        CHECK ([NamSX] >= 1900 AND [NamSX] <= 9999),
        CHECK ([SoLuong] >= 0),
        UNIQUE ([Sokhung]),
        UNIQUE ([Somay])
    );
    INSERT dbo.[tblXe] ([Maxe], [Sokhung], [Somay], [HangSX], [NamSX], [SoLuong]) VALUES
        (N'XE01', N'SK00001', N'SM00001', N'Toyota', 2024, 3),
        (N'XE02', N'SK00002', N'SM00002', N'Honda', 2025, 2);
END;

IF OBJECT_ID(N'dbo.tblKH', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblKH] (
        [MaK] nvarchar(20) NOT NULL,
        [TenK] nvarchar(100) NOT NULL,
        [Coquan] nvarchar(100) NULL,
        [Diachi] nvarchar(150) NOT NULL,
        [Tel] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_tblKH] PRIMARY KEY ([MaK])
    );
    INSERT dbo.[tblKH] ([MaK], [TenK], [Coquan], [Diachi], [Tel]) VALUES
        (N'KH01', N'Nguyễn Minh Anh', NULL, N'TP. Hồ Chí Minh', N'0901234101'),
        (N'KH02', N'Trần Hải Lan', N'Công ty Bình Minh', N'Đồng Nai', N'0901234102');
END;

IF OBJECT_ID(N'dbo.tblYeuCau', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblYeuCau] (
        [Maxe] nvarchar(20) NOT NULL,
        [MaK] nvarchar(20) NOT NULL,
        [Ngaymuon] date NOT NULL,
        [Ngaytra] date NOT NULL,
        CONSTRAINT [PK_tblYeuCau] PRIMARY KEY ([Maxe], [MaK], [Ngaymuon]),
        CONSTRAINT [FK_tblYeuCau_Maxe] FOREIGN KEY ([Maxe]) REFERENCES dbo.[tblXe] ([Maxe]),
        CONSTRAINT [FK_tblYeuCau_MaK] FOREIGN KEY ([MaK]) REFERENCES dbo.[tblKH] ([MaK]),
        CHECK ([Ngaytra] >= [Ngaymuon])
    );
    INSERT dbo.[tblYeuCau] ([Maxe], [MaK], [Ngaymuon], [Ngaytra]) VALUES
        (N'XE01', N'KH01', '20261124', '20261126');
END;

COMMIT TRANSACTION;
GO
