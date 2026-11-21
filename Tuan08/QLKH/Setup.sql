-- Chạy toàn bộ script trong SSMS hoặc SQL Server Object Explorer.
USE master;
GO
IF DB_ID(N'QLKH') IS NULL
    EXEC(N'CREATE DATABASE [QLKH]');
GO
USE [QLKH];
GO
SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.SeqMaTuDong', N'SO') IS NULL
    EXEC(N'CREATE SEQUENCE dbo.SeqMaTuDong AS bigint START WITH 1000 INCREMENT BY 1');

IF OBJECT_ID(N'dbo.HANGSX', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[HANGSX] (
        [mahang] nvarchar(20) NOT NULL,
        [tenhang] nvarchar(100) NOT NULL,
        CONSTRAINT [PK_HANGSX] PRIMARY KEY ([mahang])
    );
    INSERT dbo.[HANGSX] ([mahang], [tenhang]) VALUES
        (N'H01', N'Samsung'),
        (N'H02', N'Apple'),
        (N'H03', N'Xiaomi');
END;

IF OBJECT_ID(N'dbo.DIENTHOAI', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.[DIENTHOAI] (
        [ma] nvarchar(20) NOT NULL,
        [ten] nvarchar(100) NOT NULL,
        [dongia] decimal(16,2) NOT NULL,
        [tonkho] int NOT NULL,
        [mahang] nvarchar(20) NOT NULL,
        CONSTRAINT [PK_DIENTHOAI] PRIMARY KEY ([ma]),
        CHECK ([dongia] >= 0),
        CHECK ([tonkho] >= 0),
        CONSTRAINT [FK_DIENTHOAI_mahang] FOREIGN KEY ([mahang]) REFERENCES dbo.[HANGSX] ([mahang])
    );
    INSERT dbo.[DIENTHOAI] ([ma], [ten], [dongia], [tonkho], [mahang]) VALUES
        (N'DT01', N'Galaxy A56', 8990000, 10, N'H01'),
        (N'DT02', N'iPhone 16', 19990000, 6, N'H02'),
        (N'DT03', N'Redmi Note 14', 5490000, 18, N'H03');
END;

COMMIT TRANSACTION;
GO
