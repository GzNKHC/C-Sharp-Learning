-- Chạy toàn bộ script trên SQL Server hoặc SQL Server Express LocalDB.
-- Có thể chạy lại. Script giữ nguyên dữ liệu đã có trong các bảng.
USE master;
GO
IF DB_ID(N'QLSinhVien_ThucHanh7') IS NULL
    EXEC(N'CREATE DATABASE QLSinhVien_ThucHanh7');
GO
USE QLSinhVien_ThucHanh7;
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.Khoa', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Khoa (
        MaKhoa nvarchar(20) NOT NULL CONSTRAINT PK_Khoa PRIMARY KEY,
        TenKhoa nvarchar(100) NOT NULL
    );
    INSERT dbo.Khoa (MaKhoa, TenKhoa) VALUES
        (N'CNTT', N'Công nghệ thông tin'),
        (N'KT', N'Kế toán'),
        (N'QTKD', N'Quản trị kinh doanh');
END;

IF OBJECT_ID(N'dbo.Lop', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Lop (
        MaLop nvarchar(20) NOT NULL CONSTRAINT PK_Lop PRIMARY KEY,
        TenLop nvarchar(100) NOT NULL,
        MaKhoa nvarchar(20) NOT NULL,
        CONSTRAINT FK_Lop_Khoa FOREIGN KEY (MaKhoa) REFERENCES dbo.Khoa (MaKhoa)
    );
    INSERT dbo.Lop (MaLop, TenLop, MaKhoa) VALUES
        (N'24CNTT1', N'Công nghệ thông tin K24.1', N'CNTT'),
        (N'24CNTT2', N'Công nghệ thông tin K24.2', N'CNTT'),
        (N'24KT1', N'Kế toán K24.1', N'KT'),
        (N'24QTKD1', N'Quản trị kinh doanh K24.1', N'QTKD');
END;

IF OBJECT_ID(N'dbo.SinhVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.SinhVien (
        MaSinhVien nvarchar(20) NOT NULL CONSTRAINT PK_SinhVien PRIMARY KEY,
        HoTen nvarchar(100) NOT NULL,
        NgaySinh date NOT NULL,
        MaLop nvarchar(20) NOT NULL,
        CONSTRAINT FK_SinhVien_Lop FOREIGN KEY (MaLop) REFERENCES dbo.Lop (MaLop)
    );
    INSERT dbo.SinhVien (MaSinhVien, HoTen, NgaySinh, MaLop) VALUES
        (N'SV001', N'Nguyễn Minh Anh', '20060115', N'24CNTT1'),
        (N'SV002', N'Trần Thanh Tùng', '20060712', N'24CNTT1'),
        (N'SV003', N'Lê Gia Bảo', '20060921', N'24CNTT2'),
        (N'SV004', N'Phạm Ngọc Hân', '20060318', N'24KT1'),
        (N'SV005', N'Vũ Hoàng Nam', '20061105', N'24QTKD1');
END;

IF OBJECT_ID(N'dbo.MonHoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.MonHoc (
        MaMonHoc nvarchar(20) NOT NULL CONSTRAINT PK_MonHoc PRIMARY KEY,
        TenMonHoc nvarchar(100) NOT NULL
    );
    INSERT dbo.MonHoc (MaMonHoc, TenMonHoc) VALUES
        (N'CSDL', N'Cơ sở dữ liệu'),
        (N'CS', N'Ngôn ngữ lập trình C#'),
        (N'TCC', N'Toán cao cấp');
END;

IF OBJECT_ID(N'dbo.Diem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Diem (
        MaSinhVien nvarchar(20) NOT NULL,
        MaMonHoc nvarchar(20) NOT NULL,
        Diem decimal(4,2) NOT NULL,
        CONSTRAINT PK_Diem PRIMARY KEY (MaSinhVien, MaMonHoc),
        CONSTRAINT FK_Diem_SinhVien FOREIGN KEY (MaSinhVien) REFERENCES dbo.SinhVien (MaSinhVien),
        CONSTRAINT FK_Diem_MonHoc FOREIGN KEY (MaMonHoc) REFERENCES dbo.MonHoc (MaMonHoc),
        CONSTRAINT CK_Diem_Khoang CHECK (Diem BETWEEN 0 AND 10)
    );
    INSERT dbo.Diem (MaSinhVien, MaMonHoc, Diem) VALUES
        (N'SV001', N'CS', 9),
        (N'SV001', N'CSDL', 8.5),
        (N'SV002', N'CS', 7),
        (N'SV002', N'TCC', 8.25),
        (N'SV003', N'CSDL', 8),
        (N'SV004', N'TCC', 9.5);
END;

IF OBJECT_ID(N'dbo.DMChatLieu', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DMChatLieu (
        MaChatLieu nvarchar(20) NOT NULL CONSTRAINT PK_DMChatLieu PRIMARY KEY,
        TenChatLieu nvarchar(100) NOT NULL
    );
    INSERT dbo.DMChatLieu (MaChatLieu, TenChatLieu) VALUES
        (N'CL01', N'Bông'), (N'CL02', N'Gỗ'), (N'CL03', N'Nhựa');
END;

IF OBJECT_ID(N'dbo.DMHang', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DMHang (
        MaHang nvarchar(20) NOT NULL CONSTRAINT PK_DMHang PRIMARY KEY,
        TenHang nvarchar(100) NOT NULL,
        MaChatLieu nvarchar(20) NOT NULL,
        SoLuong int NOT NULL,
        DonGiaNhap decimal(14,2) NOT NULL,
        DonGiaBan decimal(14,2) NOT NULL,
        Hinh nvarchar(255) NOT NULL CONSTRAINT DF_DMHang_Hinh DEFAULT N'',
        GhiChu nvarchar(255) NOT NULL CONSTRAINT DF_DMHang_GhiChu DEFAULT N'',
        CONSTRAINT FK_DMHang_DMChatLieu FOREIGN KEY (MaChatLieu) REFERENCES dbo.DMChatLieu (MaChatLieu),
        CONSTRAINT CK_DMHang_GiaSoLuong CHECK (SoLuong >= 0 AND DonGiaNhap >= 0 AND DonGiaBan >= 0)
    );
    INSERT dbo.DMHang (MaHang, TenHang, MaChatLieu, SoLuong, DonGiaNhap, DonGiaBan, GhiChu) VALUES
        (N'H01', N'Gối bông', N'CL01', 24, 80000, 120000, N'Vỏ gối tháo được'),
        (N'H02', N'Kệ gỗ để bàn', N'CL02', 12, 155000, 220000, N'Gỗ màu tự nhiên'),
        (N'H03', N'Hộp đựng bút', N'CL03', 35, 18000, 29000, N'Hàng có sẵn');
END;

COMMIT TRANSACTION;
GO
