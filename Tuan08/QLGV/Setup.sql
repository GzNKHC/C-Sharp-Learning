-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLGV') IS NULL
    EXEC(N'CREATE DATABASE [QLGV]');
GO
USE [QLGV];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.COSO', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[COSO] (
        [Macoso] nvarchar(20) NOT NULL,
        [Tencoso] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_COSO] PRIMARY KEY ([Macoso])
    );
    INSERT dbo.[COSO] ([Macoso], [Tencoso]) VALUES
        (N'CS01', N'Cơ sở Nguyễn Đình Chiểu'),
        (N'CS02', N'Cơ sở Điện Biên Phủ');
END;

IF OBJECT_ID(N'dbo.DONVI', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DONVI] (
        [Madonvi] nvarchar(20) NOT NULL,
        [Tendonvi] nvarchar(100) NOT NULL,
        [Macoso] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_DONVI] PRIMARY KEY ([Madonvi]),
        CONSTRAINT [FK_DONVI_Macoso] FOREIGN KEY ([Macoso]) REFERENCES dbo.[COSO] ([Macoso])
    );
    INSERT dbo.[DONVI] ([Madonvi], [Tendonvi], [Macoso]) VALUES
        (N'DV01', N'Công nghệ thông tin', N'CS01'),
        (N'DV02', N'Kinh tế', N'CS01'),
        (N'DV03', N'Ngoại ngữ', N'CS02');
END;

IF OBJECT_ID(N'dbo.GV', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[GV] (
        [Magv] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [Sdt] nvarchar(20) NOT NULL,
        [Ghichu] nvarchar(255) NULL,
        [Madonvi] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_GV] PRIMARY KEY ([Magv]),
        CONSTRAINT [FK_GV_Madonvi] FOREIGN KEY ([Madonvi]) REFERENCES dbo.[DONVI] ([Madonvi])
    );
    INSERT dbo.[GV] ([Magv], [Hoten], [Sdt], [Ghichu], [Madonvi]) VALUES
        (N'GV01', N'Trần Thanh Bình', N'0901234101', N'Phụ trách cơ sở dữ liệu', N'DV01'),
        (N'GV02', N'Nguyễn Ngọc Lan', N'0901234102', NULL, N'DV01'),
        (N'GV03', N'Lê Việt Anh', N'0901234103', NULL, N'DV03');
END;

COMMIT TRANSACTION;
GO
