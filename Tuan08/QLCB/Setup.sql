-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLCB') IS NULL
    EXEC(N'CREATE DATABASE [QLCB]');
GO
USE [QLCB];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.PHONGBAN', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[PHONGBAN] (
        [Maphong] nvarchar(20) NOT NULL,
        [Tenphong] nvarchar(100) NOT NULL,
        [Vitri] nvarchar(80) NOT NULL,
        CONSTRAINT [PK_PHONGBAN] PRIMARY KEY ([Maphong])
    );
    INSERT dbo.[PHONGBAN] ([Maphong], [Tenphong], [Vitri]) VALUES
        (N'PB01', N'Phòng kỹ thuật', N'Tầng 2'),
        (N'PB02', N'Phòng hành chính', N'Tầng 1');
END;

IF OBJECT_ID(N'dbo.NGACH', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[NGACH] (
        [MaNg] nvarchar(20) NOT NULL,
        [TenNg] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_NGACH] PRIMARY KEY ([MaNg])
    );
    INSERT dbo.[NGACH] ([MaNg], [TenNg]) VALUES
        (N'N01', N'Chuyên viên'),
        (N'N02', N'Nhân viên');
END;

IF OBJECT_ID(N'dbo.CHVU', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[CHVU] (
        [MaCV] nvarchar(20) NOT NULL,
        [TenCV] nvarchar(100) NOT NULL,
        [Hesophucap] decimal(8,2) NOT NULL,
        CONSTRAINT [PK_CHVU] PRIMARY KEY ([MaCV]),
        CHECK ([Hesophucap] >= 0)
    );
    INSERT dbo.[CHVU] ([MaCV], [TenCV], [Hesophucap]) VALUES
        (N'CV01', N'Trưởng phòng', 0.5),
        (N'CV02', N'Chuyên viên', 0.25);
END;

IF OBJECT_ID(N'dbo.HOSO', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[HOSO] (
        [manv] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [Gioitinh] nvarchar(8) NOT NULL,
        [Ngaysinh] date NOT NULL,
        [Diachi] nvarchar(150) NOT NULL,
        [Ngayvn] date NOT NULL,
        [MaNg] nvarchar(20) NOT NULL,
        [MaCV] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_HOSO] PRIMARY KEY ([manv]),
        CONSTRAINT [FK_HOSO_MaNg] FOREIGN KEY ([MaNg]) REFERENCES dbo.[NGACH] ([MaNg]),
        CONSTRAINT [FK_HOSO_MaCV] FOREIGN KEY ([MaCV]) REFERENCES dbo.[CHVU] ([MaCV]),
        CHECK ([Ngayvn] >= [Ngaysinh])
    );
    INSERT dbo.[HOSO] ([manv], [Hoten], [Gioitinh], [Ngaysinh], [Diachi], [Ngayvn], [MaNg], [MaCV]) VALUES
        (N'NV01', N'Nguyễn Minh Anh', N'Nam', '19900611', N'TP. Hồ Chí Minh', '20141105', N'N01', N'CV01'),
        (N'NV02', N'Trần Hải Lan', N'Nữ', '19990222', N'Đồng Nai', '20210904', N'N02', N'CV02');
END;

IF OBJECT_ID(N'dbo.LUONG', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[LUONG] (
        [manv] nvarchar(20) NOT NULL,
        [maphong] nvarchar(20) NOT NULL,
        [Hesoluong] decimal(8,2) NOT NULL,
        [ngayLL] date NOT NULL,
        CONSTRAINT [PK_LUONG] PRIMARY KEY ([manv]),
        CONSTRAINT [FK_LUONG_manv] FOREIGN KEY ([manv]) REFERENCES dbo.[HOSO] ([manv]),
        CONSTRAINT [FK_LUONG_maphong] FOREIGN KEY ([maphong]) REFERENCES dbo.[PHONGBAN] ([Maphong]),
        CHECK ([Hesoluong] >= 0.01)
    );
    INSERT dbo.[LUONG] ([manv], [maphong], [Hesoluong], [ngayLL]) VALUES
        (N'NV01', N'PB01', 4.32, '20251101'),
        (N'NV02', N'PB02', 2.67, '20260901');
END;

COMMIT TRANSACTION;
GO
