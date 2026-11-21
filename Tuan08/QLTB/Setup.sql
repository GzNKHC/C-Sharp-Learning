-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLTB') IS NULL
    EXEC(N'CREATE DATABASE [QLTB]');
GO
USE [QLTB];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.tblPhong', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhong] (
        [MaPhong] nvarchar(12) NOT NULL,
        [TenPhong] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_tblPhong] PRIMARY KEY ([MaPhong])
    );
    INSERT dbo.[tblPhong] ([MaPhong], [TenPhong]) VALUES
        (N'P01', N'Phòng thực hành'),
        (N'P02', N'Kho thiết bị');
END;

IF OBJECT_ID(N'dbo.tblNhanVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblNhanVien] (
        [MaNV] nvarchar(12) NOT NULL,
        [TenNV] nvarchar(100) NOT NULL,
        [ChucVu] nvarchar(50) NOT NULL,
        [MaPhong] nvarchar(12) NOT NULL,
        CONSTRAINT [PK_tblNhanVien] PRIMARY KEY ([MaNV]),
        CONSTRAINT [FK_tblNhanVien_MaPhong] FOREIGN KEY ([MaPhong]) REFERENCES dbo.[tblPhong] ([MaPhong])
    );
    INSERT dbo.[tblNhanVien] ([MaNV], [TenNV], [ChucVu], [MaPhong]) VALUES
        (N'NV01', N'Nguyễn Minh Anh', N'Quản lý kho', N'P02'),
        (N'NV02', N'Trần Thanh Bình', N'Kỹ thuật viên', N'P01');
END;

IF OBJECT_ID(N'dbo.tblThietBi', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblThietBi] (
        [MaTB] nvarchar(12) NOT NULL,
        [TenTB] nvarchar(100) NOT NULL,
        [TinhTrang] nvarchar(50) NOT NULL,
        [SoLuong] int NOT NULL,
        [GhiChu] nvarchar(255) NULL,
        [SoLuongHong] int NOT NULL,
        CONSTRAINT [PK_tblThietBi] PRIMARY KEY ([MaTB]),
        CHECK ([SoLuong] >= 0),
        CHECK ([SoLuongHong] >= 0)
    );
    INSERT dbo.[tblThietBi] ([MaTB], [TenTB], [TinhTrang], [SoLuong], [GhiChu], [SoLuongHong]) VALUES
        (N'TB01', N'Máy chiếu', N'Tốt', 8, NULL, 0),
        (N'TB02', N'Máy đo điện', N'Tốt', 12, N'Kệ 2', 1);
END;

IF OBJECT_ID(N'dbo.tblPhieuNhap', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhieuNhap] (
        [MaPN] nvarchar(12) NOT NULL,
        [MaNV] nvarchar(12) NOT NULL,
        [MaTB] nvarchar(12) NOT NULL,
        [NgayNhap] date NOT NULL,
        [SoLuong] int NOT NULL,
        [TinhTrang] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_tblPhieuNhap] PRIMARY KEY ([MaPN]),
        CONSTRAINT [FK_tblPhieuNhap_MaNV] FOREIGN KEY ([MaNV]) REFERENCES dbo.[tblNhanVien] ([MaNV]),
        CONSTRAINT [FK_tblPhieuNhap_MaTB] FOREIGN KEY ([MaTB]) REFERENCES dbo.[tblThietBi] ([MaTB]),
        CHECK ([SoLuong] >= 1)
    );
    INSERT dbo.[tblPhieuNhap] ([MaPN], [MaNV], [MaTB], [NgayNhap], [SoLuong], [TinhTrang]) VALUES
        (N'PN001', N'NV01', N'TB01', '20251006', 8, N'Tốt');
END;

IF OBJECT_ID(N'dbo.tblPhieuMuon', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhieuMuon] (
        [MaPM] nvarchar(12) NOT NULL,
        [NVLapPhieu] nvarchar(12) NOT NULL,
        [MaTB] nvarchar(12) NOT NULL,
        [TinhTrang] nvarchar(50) NOT NULL,
        [NgayMuon] date NOT NULL,
        [NVMuon] nvarchar(12) NOT NULL,
        [SoLuong] int NOT NULL,
        [MaPhong] nvarchar(12) NOT NULL,
        CONSTRAINT [PK_tblPhieuMuon] PRIMARY KEY ([MaPM]),
        CONSTRAINT [FK_tblPhieuMuon_NVLapPhieu] FOREIGN KEY ([NVLapPhieu]) REFERENCES dbo.[tblNhanVien] ([MaNV]),
        CONSTRAINT [FK_tblPhieuMuon_MaTB] FOREIGN KEY ([MaTB]) REFERENCES dbo.[tblThietBi] ([MaTB]),
        CONSTRAINT [FK_tblPhieuMuon_NVMuon] FOREIGN KEY ([NVMuon]) REFERENCES dbo.[tblNhanVien] ([MaNV]),
        CHECK ([SoLuong] >= 1),
        CONSTRAINT [FK_tblPhieuMuon_MaPhong] FOREIGN KEY ([MaPhong]) REFERENCES dbo.[tblPhong] ([MaPhong])
    );
    INSERT dbo.[tblPhieuMuon] ([MaPM], [NVLapPhieu], [MaTB], [TinhTrang], [NgayMuon], [NVMuon], [SoLuong], [MaPhong]) VALUES
        (N'PM001', N'NV01', N'TB02', N'Tốt', '20251007', N'NV02', 2, N'P01');
END;

IF OBJECT_ID(N'dbo.tblPhieuTra', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[tblPhieuTra] (
        [MaPT] nvarchar(12) NOT NULL,
        [MaPM] nvarchar(12) NULL,
        [MaTB] nvarchar(12) NOT NULL,
        [NVTra] nvarchar(12) NOT NULL,
        [NVNhan] nvarchar(12) NOT NULL,
        [NgayTra] date NOT NULL,
        [SoLuong] int NOT NULL,
        [TinhTrang] nvarchar(50) NOT NULL,
        [MaPhong] nvarchar(12) NOT NULL,
        [SoPhieuMuonGoc] nvarchar(12) NOT NULL,
        CONSTRAINT [PK_tblPhieuTra] PRIMARY KEY ([MaPT]),
        CONSTRAINT [FK_tblPhieuTra_MaPM] FOREIGN KEY ([MaPM]) REFERENCES dbo.[tblPhieuMuon] ([MaPM]),
        CONSTRAINT [FK_tblPhieuTra_MaTB] FOREIGN KEY ([MaTB]) REFERENCES dbo.[tblThietBi] ([MaTB]),
        CONSTRAINT [FK_tblPhieuTra_NVTra] FOREIGN KEY ([NVTra]) REFERENCES dbo.[tblNhanVien] ([MaNV]),
        CONSTRAINT [FK_tblPhieuTra_NVNhan] FOREIGN KEY ([NVNhan]) REFERENCES dbo.[tblNhanVien] ([MaNV]),
        CHECK ([SoLuong] >= 1),
        CONSTRAINT [FK_tblPhieuTra_MaPhong] FOREIGN KEY ([MaPhong]) REFERENCES dbo.[tblPhong] ([MaPhong])
    );
END;

COMMIT TRANSACTION;
GO
