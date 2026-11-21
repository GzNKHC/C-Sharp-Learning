-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLD') IS NULL
    EXEC(N'CREATE DATABASE [QLD]');
GO
USE [QLD];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.Lop', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Lop] (
        [MaL] nvarchar(20) NOT NULL,
        [TenL] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_Lop] PRIMARY KEY ([MaL])
    );
    INSERT dbo.[Lop] ([MaL], [TenL]) VALUES
        (N'24CNTT1', N'Công nghệ thông tin 24.1'),
        (N'24CNTT2', N'Công nghệ thông tin 24.2');
END;

IF OBJECT_ID(N'dbo.SV', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[SV] (
        [MaSV] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [GT] nvarchar(8) NOT NULL,
        [NS] date NOT NULL,
        [QQ] nvarchar(80) NOT NULL,
        [MaL] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_SV] PRIMARY KEY ([MaSV]),
        CONSTRAINT [FK_SV_MaL] FOREIGN KEY ([MaL]) REFERENCES dbo.[Lop] ([MaL])
    );
    INSERT dbo.[SV] ([MaSV], [Hoten], [GT], [NS], [QQ], [MaL]) VALUES
        (N'SV001', N'Nguyễn Minh Anh', N'Nữ', '20060115', N'Đắk Lắk', N'24CNTT1'),
        (N'SV002', N'Trần Thanh Tùng', N'Nam', '20060712', N'TP. Hồ Chí Minh', N'24CNTT1'),
        (N'SV003', N'Lê Gia Bảo', N'Nam', '20060921', N'Đồng Nai', N'24CNTT2');
END;

IF OBJECT_ID(N'dbo.HP', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[HP] (
        [MaHP] nvarchar(20) NOT NULL,
        [TenHP] nvarchar(100) NOT NULL,
        [SoTC] int NOT NULL,
        [HK] int NOT NULL,
        CONSTRAINT [PK_HP] PRIMARY KEY ([MaHP]),
        CHECK ([SoTC] >= 1),
        CHECK ([HK] >= 1)
    );
    INSERT dbo.[HP] ([MaHP], [TenHP], [SoTC], [HK]) VALUES
        (N'CSDL', N'Cơ sở dữ liệu', 3, 1),
        (N'CS', N'Lập trình C#', 3, 2),
        (N'TCC', N'Toán cao cấp', 3, 1);
END;

IF OBJECT_ID(N'dbo.DangKyHocPhan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DangKyHocPhan] (
        [MaSV] nvarchar(20) NOT NULL,
        [MaHP] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_DangKyHocPhan] PRIMARY KEY ([MaSV], [MaHP]),
        CONSTRAINT [FK_DangKyHocPhan_MaSV] FOREIGN KEY ([MaSV]) REFERENCES dbo.[SV] ([MaSV]),
        CONSTRAINT [FK_DangKyHocPhan_MaHP] FOREIGN KEY ([MaHP]) REFERENCES dbo.[HP] ([MaHP])
    );
    INSERT dbo.[DangKyHocPhan] ([MaSV], [MaHP]) VALUES
        (N'SV001', N'CS'),
        (N'SV001', N'CSDL'),
        (N'SV002', N'CS'),
        (N'SV002', N'CSDL'),
        (N'SV003', N'CS'),
        (N'SV003', N'TCC');
END;

IF OBJECT_ID(N'dbo.Diem', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[Diem] (
        [MaSV] nvarchar(20) NOT NULL,
        [MaHP] nvarchar(20) NOT NULL,
        [Diem] decimal(4,2) NOT NULL,
        CONSTRAINT [PK_Diem] PRIMARY KEY ([MaSV], [MaHP]),
        CONSTRAINT [FK_Diem_MaSV] FOREIGN KEY ([MaSV]) REFERENCES dbo.[SV] ([MaSV]),
        CONSTRAINT [FK_Diem_MaHP] FOREIGN KEY ([MaHP]) REFERENCES dbo.[HP] ([MaHP]),
        CHECK ([Diem] >= 0 AND [Diem] <= 10),
        CONSTRAINT FK_Diem_DangKy FOREIGN KEY ([MaSV], [MaHP]) REFERENCES dbo.DangKyHocPhan([MaSV], [MaHP])
    );
    INSERT dbo.[Diem] ([MaSV], [MaHP], [Diem]) VALUES
        (N'SV001', N'CSDL', 8.5),
        (N'SV002', N'CSDL', 7.75),
        (N'SV003', N'TCC', 9);
END;

COMMIT TRANSACTION;
GO
