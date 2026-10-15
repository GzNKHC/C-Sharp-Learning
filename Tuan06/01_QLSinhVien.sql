IF DB_ID(N'QLSinhVien') IS NULL
    CREATE DATABASE QLSinhVien;
GO
USE QLSinhVien;
GO

IF OBJECT_ID(N'Diem', N'U') IS NOT NULL DROP TABLE Diem;
IF OBJECT_ID(N'SinhVien', N'U') IS NOT NULL DROP TABLE SinhVien;
IF OBJECT_ID(N'MonHoc', N'U') IS NOT NULL DROP TABLE MonHoc;
IF OBJECT_ID(N'Lop', N'U') IS NOT NULL DROP TABLE Lop;
IF OBJECT_ID(N'Khoa', N'U') IS NOT NULL DROP TABLE Khoa;
GO

CREATE TABLE Khoa(
    MaKhoa varchar(20) NOT NULL PRIMARY KEY,
    TenKhoa nvarchar(100) NOT NULL
);

CREATE TABLE Lop(
    MaLop varchar(20) NOT NULL PRIMARY KEY,
    TenLop nvarchar(100) NOT NULL,
    MaKhoa varchar(20) NOT NULL,
    CONSTRAINT FK_Lop_Khoa FOREIGN KEY(MaKhoa) REFERENCES Khoa(MaKhoa)
);

CREATE TABLE SinhVien(
    MaSinhVien varchar(20) NOT NULL PRIMARY KEY,
    HoTen nvarchar(100) NOT NULL,
    NgaySinh date NOT NULL,
    MaLop varchar(20) NOT NULL,
    CONSTRAINT FK_SinhVien_Lop FOREIGN KEY(MaLop) REFERENCES Lop(MaLop)
);

CREATE TABLE MonHoc(
    MaMonHoc varchar(20) NOT NULL PRIMARY KEY,
    TenMonHoc nvarchar(100) NOT NULL
);

CREATE TABLE Diem(
    MaSinhVien varchar(20) NOT NULL,
    MaMonHoc varchar(20) NOT NULL,
    Diem decimal(4,2) NOT NULL,
    CONSTRAINT PK_Diem PRIMARY KEY(MaSinhVien, MaMonHoc),
    CONSTRAINT FK_Diem_SinhVien FOREIGN KEY(MaSinhVien) REFERENCES SinhVien(MaSinhVien),
    CONSTRAINT FK_Diem_MonHoc FOREIGN KEY(MaMonHoc) REFERENCES MonHoc(MaMonHoc),
    CONSTRAINT CK_Diem CHECK (Diem >= 0 AND Diem <= 10)
);
GO

INSERT INTO Khoa VALUES
('CNTT', N'Công nghệ thông tin'),
('KT', N'Kế toán'),
('QTKD', N'Quản trị kinh doanh'),
('NN', N'Ngoại ngữ'),
('DL', N'Du lịch');

INSERT INTO Lop VALUES
('05DHTH1', N'Công nghệ phần mềm khóa 05', 'CNTT'),
('05DHTH2', N'Kỹ thuật phần mềm khóa 05', 'CNTT'),
('06DHTH1', N'Lập trình mạng khóa 06', 'CNTT'),
('05DHKH1', N'Khoa học dữ liệu khóa 05', 'CNTT'),
('05DHKT1', N'Kế toán doanh nghiệp khóa 05', 'KT');

INSERT INTO MonHoc VALUES
('MH01', N'Cơ sở dữ liệu'),
('MH02', N'Lập trình C#'),
('MH03', N'Mạng máy tính'),
('MH04', N'Cấu trúc dữ liệu'),
('MH05', N'Phân tích thiết kế hệ thống');

INSERT INTO SinhVien VALUES
('SV001', N'Trần Thanh Tùng', '2005-10-12', '05DHTH1'),
('SV002', N'Trần Thanh Tính', '2005-10-12', '05DHTH1'),
('SV003', N'Nguyễn Thanh Bình', '2005-04-24', '05DHTH2'),
('SV004', N'Nguyễn Thanh Bình', '2004-04-24', '06DHTH1'),
('SV005', N'Nguyễn Đạt Nhân', '2005-03-15', '05DHKH1');

INSERT INTO Diem VALUES
('SV001','MH01',7),
('SV001','MH02',4),
('SV002','MH01',7),
('SV002','MH03',6),
('SV003','MH01',9),
('SV003','MH02',10),
('SV004','MH02',6),
('SV005','MH01',9);
GO
