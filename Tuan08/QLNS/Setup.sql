-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLNS') IS NULL
    EXEC(N'CREATE DATABASE [QLNS]');
GO
USE [QLNS];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.tblPhongBan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhongBan] (
        [MaPhongBan] nvarchar(12) NOT NULL,
        [TenPhongBan] nvarchar(25) NOT NULL,
        [SoDTPB] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_tblPhongBan] PRIMARY KEY ([MaPhongBan])
    );
    INSERT dbo.[tblPhongBan] ([MaPhongBan], [TenPhongBan], [SoDTPB]) VALUES
        (N'PB01', N'Kỹ thuật', N'02873000001'),
        (N'PB02', N'Hành chính', N'02873000002');
END;

IF OBJECT_ID(N'dbo.tblChucVu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblChucVu] (
        [MaChucVu] nvarchar(12) NOT NULL,
        [TenChucVu] nvarchar(30) NOT NULL,
        [PhuCapChucVu] decimal(16,2) NOT NULL,
        CONSTRAINT [PK_tblChucVu] PRIMARY KEY ([MaChucVu]),
        CHECK ([PhuCapChucVu] >= 0)
    );
    INSERT dbo.[tblChucVu] ([MaChucVu], [TenChucVu], [PhuCapChucVu]) VALUES
        (N'CV01', N'Nhân viên', 0),
        (N'CV02', N'Trưởng phòng', 2000000);
END;

IF OBJECT_ID(N'dbo.tblNhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblNhanVien] (
        [MaNhanVien] nvarchar(12) NOT NULL,
        [HoTenNhanVien] nvarchar(100) NOT NULL,
        [MaPhongBan] nvarchar(12) NOT NULL,
        [MaChucVu] nvarchar(12) NOT NULL,
        [SCMND] nvarchar(15) NULL,
        [DiaChi] nvarchar(150) NOT NULL,
        [GioiTinh] nvarchar(8) NOT NULL,
        [QueQuan] nvarchar(80) NOT NULL,
        [NgaySinh] date NOT NULL,
        [SoDT] nvarchar(20) NOT NULL,
        [NgayVaoLam] date NOT NULL,
        [SoBHYT] nvarchar(20) NULL,
        [SoBHXH] nvarchar(20) NULL,
        [HinhAnh] nvarchar(255) NULL,
        [GhiChu] nvarchar(255) NULL,
        CONSTRAINT [PK_tblNhanVien] PRIMARY KEY ([MaNhanVien]),
        CONSTRAINT [FK_tblNhanVien_MaPhongBan] FOREIGN KEY ([MaPhongBan]) REFERENCES dbo.[tblPhongBan] ([MaPhongBan]),
        CONSTRAINT [FK_tblNhanVien_MaChucVu] FOREIGN KEY ([MaChucVu]) REFERENCES dbo.[tblChucVu] ([MaChucVu]),
        CHECK ([NgayVaoLam] >= [NgaySinh])
    );
    INSERT dbo.[tblNhanVien] ([MaNhanVien], [HoTenNhanVien], [MaPhongBan], [MaChucVu], [SCMND], [DiaChi], [GioiTinh], [QueQuan], [NgaySinh], [SoDT], [NgayVaoLam], [SoBHYT], [SoBHXH], [HinhAnh], [GhiChu]) VALUES
        (N'NV01', N'Nguyễn Hoàng An', N'PB01', N'CV02', NULL, N'TP. Hồ Chí Minh', N'Nam', N'Đắk Lắk', '19920516', N'0901234001', '20180701', NULL, NULL, NULL, NULL),
        (N'NV02', N'Trần Thị Lan', N'PB02', N'CV01', NULL, N'TP. Hồ Chí Minh', N'Nữ', N'Đồng Nai', '19990225', N'0901234002', '20220315', NULL, NULL, NULL, NULL);
END;

COMMIT TRANSACTION;
GO
