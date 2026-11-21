-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLVDV') IS NULL
    EXEC(N'CREATE DATABASE [QLVDV]');
GO
USE [QLVDV];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.tblThanhvien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblThanhvien] (
        [MaThanhvien] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [Ngaysinh] date NOT NULL,
        [Gioitinh] nvarchar(8) NOT NULL,
        [Donvi] nvarchar(60) NOT NULL,
        [Sothich] nvarchar(100) NULL,
        [Diachi] nvarchar(150) NOT NULL,
        [Dienthoai] nvarchar(20) NOT NULL,
        [Quyenhan] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_tblThanhvien] PRIMARY KEY ([MaThanhvien])
    );
    INSERT dbo.[tblThanhvien] ([MaThanhvien], [Hoten], [Ngaysinh], [Gioitinh], [Donvi], [Sothich], [Diachi], [Dienthoai], [Quyenhan]) VALUES
        (N'TV01', N'Nguyễn Minh Anh', '20040115', N'Nam', N'Đội 1', N'Chạy bộ', N'TP. Hồ Chí Minh', N'0901234101', N'Thành viên'),
        (N'TV02', N'Trần Hải Lan', '20050506', N'Nữ', N'Đội 2', N'Bơi', N'Đồng Nai', N'0901234102', N'Đội trưởng');
END;

IF OBJECT_ID(N'dbo.tblHoatdong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblHoatdong] (
        [Mahoatdong] nvarchar(20) NOT NULL,
        [Tenhoatdong] nvarchar(100) NOT NULL,
        [Noidung] nvarchar(255) NOT NULL,
        [Thoigian] datetime2(0) NOT NULL,
        [Kinhphi] decimal(16,2) NOT NULL,
        CONSTRAINT [PK_tblHoatdong] PRIMARY KEY ([Mahoatdong]),
        CHECK ([Kinhphi] >= 0)
    );
    INSERT dbo.[tblHoatdong] ([Mahoatdong], [Tenhoatdong], [Noidung], [Thoigian], [Kinhphi]) VALUES
        (N'HD01', N'Tập luyện cuối tuần', N'Tập luyện theo nhóm', '2026-11-15T08:00:00', 500000),
        (N'HD02', N'Giao lưu thể thao', N'Thi đấu giao hữu', '2026-11-21T09:00:00', 1500000);
END;

IF OBJECT_ID(N'dbo.tblPhieu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhieu] (
        [Mahoatdong] nvarchar(20) NOT NULL,
        [Mathanhvien] nvarchar(20) NOT NULL,
        [ThoigianBD] datetime2(0) NOT NULL,
        [ThoigianKT] datetime2(0) NOT NULL,
        [Danhgia] nvarchar(150) NULL,
        CONSTRAINT [PK_tblPhieu] PRIMARY KEY ([Mahoatdong], [Mathanhvien], [ThoigianBD]),
        CONSTRAINT [FK_tblPhieu_Mahoatdong] FOREIGN KEY ([Mahoatdong]) REFERENCES dbo.[tblHoatdong] ([Mahoatdong]),
        CONSTRAINT [FK_tblPhieu_Mathanhvien] FOREIGN KEY ([Mathanhvien]) REFERENCES dbo.[tblThanhvien] ([MaThanhvien]),
        CHECK ([ThoigianKT] > [ThoigianBD])
    );
    INSERT dbo.[tblPhieu] ([Mahoatdong], [Mathanhvien], [ThoigianBD], [ThoigianKT], [Danhgia]) VALUES
        (N'HD01', N'TV01', '2026-11-15T08:00:00', '2026-11-15T11:00:00', N'Hoàn thành');
END;

COMMIT TRANSACTION;
GO
