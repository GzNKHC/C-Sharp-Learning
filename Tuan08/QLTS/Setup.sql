-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLTS') IS NULL
    EXEC(N'CREATE DATABASE [QLTS]');
GO
USE [QLTS];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.UT', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[UT] (
        [MaUT] nvarchar(20) NOT NULL,
        [TenUT] nvarchar(100) NOT NULL,
        [DC] decimal(4,2) NOT NULL,
        CONSTRAINT [PK_UT] PRIMARY KEY ([MaUT]),
        CHECK ([DC] >= 0 AND [DC] <= 10)
    );
    INSERT dbo.[UT] ([MaUT], [TenUT], [DC]) VALUES
        (N'UT00', N'Không ưu tiên', 0),
        (N'UT01', N'Ưu tiên khu vực', 0.75),
        (N'UT02', N'Ưu tiên đối tượng', 1.5);
END;

IF OBJECT_ID(N'dbo.NH', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[NH] (
        [MaN] nvarchar(20) NOT NULL,
        [TenN] nvarchar(100) NOT NULL,
        [SoCT] int NOT NULL,
        CONSTRAINT [PK_NH] PRIMARY KEY ([MaN]),
        CHECK ([SoCT] >= 0)
    );
    INSERT dbo.[NH] ([MaN], [TenN], [SoCT]) VALUES
        (N'CNTT', N'Công nghệ thông tin', 100),
        (N'KT', N'Kế toán', 60),
        (N'QTKD', N'Quản trị kinh doanh', 80);
END;

IF OBJECT_ID(N'dbo.TS', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[TS] (
        [SBD] nvarchar(20) NOT NULL,
        [Hoten] nvarchar(100) NOT NULL,
        [NS] date NOT NULL,
        [GT] nvarchar(8) NOT NULL,
        [QQ] nvarchar(80) NOT NULL,
        [DT] decimal(4,2) NULL,
        [DL] decimal(4,2) NULL,
        [DH] decimal(4,2) NULL,
        [SP] int NOT NULL,
        [MaUT] nvarchar(20) NULL,
        [MaN] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_TS] PRIMARY KEY ([SBD]),
        CHECK ([DT] >= 0 AND [DT] <= 10),
        CHECK ([DL] >= 0 AND [DL] <= 10),
        CHECK ([DH] >= 0 AND [DH] <= 10),
        CHECK ([SP] >= 1),
        CONSTRAINT [FK_TS_MaUT] FOREIGN KEY ([MaUT]) REFERENCES dbo.[UT] ([MaUT]),
        CONSTRAINT [FK_TS_MaN] FOREIGN KEY ([MaN]) REFERENCES dbo.[NH] ([MaN])
    );
    INSERT dbo.[TS] ([SBD], [Hoten], [NS], [GT], [QQ], [DT], [DL], [DH], [SP], [MaUT], [MaN]) VALUES
        (N'001', N'Nguyễn Minh Anh', '20061212', N'Nữ', N'Nam Định', NULL, NULL, NULL, 1, N'UT01', N'CNTT'),
        (N'002', N'Nguyễn Linh Giang', '20060301', N'Nữ', N'Hà Nội', NULL, NULL, NULL, 1, N'UT00', N'KT'),
        (N'003', N'Trần Gia Bảo', '20060413', N'Nam', N'Đắk Lắk', 7.5, 8, 8.25, 2, N'UT02', N'CNTT'),
        (N'004', N'Lê Hoàng Nam', '20060619', N'Nam', N'Đồng Nai', 6, 7, 8, 2, N'UT00', N'QTKD');
END;

COMMIT TRANSACTION;
GO
