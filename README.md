# TH06 - WinForms + ADO.NET

Solution gồm 2 project:

- `TH06a`: bài thực hành 06a
  - Quản lý Môn học
  - Quản lý Sinh viên
  - Quản lý Điểm
  - Quản lý Lớp
- `TH06b`: bài thực hành 06b
  - Sinh viên dùng ComboBox khóa ngoại + kiểm tra khóa
  - Điểm dùng ComboBox + kiểm tra khóa chính/khóa ngoại
  - Danh sách Lớp bằng ListView
  - Danh sách Điểm theo Môn học + tính tổng điểm
  - Bài tập QLCuaHang: Tạo tồn kho, Thêm sản phẩm, Bán hàng

## 1. Chuẩn bị SQL Server Docker

Theo cấu hình giảng viên:

```powershell
docker pull mcr.microsoft.com/mssql/server:2022-latest

docker run -d `
  --name mssql2022 `
  --hostname mssql2022 `
  -e "ACCEPT_EULA=Y" `
  -e "MSSQL_PID=Developer" `
  -e "MSSQL_SA_PASSWORD=SqlServer@" `
  -p 127.0.0.1:1433:1433 `
  -v mssql_data:/var/opt/mssql `
  --restart unless-stopped `
  mcr.microsoft.com/mssql/server:2022-latest
```

## 2. Tạo CSDL

Mở SSMS/DBeaver và chạy lần lượt:

1. `Database/01_QLSinhVien.sql`
2. `Database/02_QLCuaHang.sql` (dùng cho bài tập về nhà của TH06b)

Các script đã tạo sẵn dữ liệu mẫu để test.

## 3. Mở project

Mở `TH06_ADONET.sln` bằng Visual Studio 2022.

Visual Studio sẽ tự Restore NuGet package `Microsoft.Data.SqlClient`.

Nếu SQL Server của bạn dùng port/user/password khác, sửa:

- `TH06a/Db.cs`
- `TH06b/Db.cs`

Chuỗi mặc định:

```text
Server=localhost,1433
User Id=sa
Password=SqlServer@
```

## 4. Ghi chú cách làm

- Bài 06a tập trung `SqlConnection`, `SqlCommand`, `ExecuteNonQuery` cho Thêm/Xóa/Sửa.
- Bài 06b bổ sung `ExecuteScalar` để kiểm tra khóa và `SqlDataReader` để nạp dữ liệu ComboBox/ListView.
- Các câu SQL trong project dùng **parameter** (`@ma`, `@ten`, ...) thay vì nối chuỗi trực tiếp. Vẫn là ADO.NET/SqlCommand nhưng an toàn và dễ kiểm tra lỗi hơn.

### Điểm cần chú ý ở bài QLCuaHang

Tài liệu ghi công thức tồn kho:

`SLTon = SLDau + SLNhap - SLBan`

nhưng ở phần bán hàng lại ghi cập nhật `SLBan = SLBan - số lượng`. Hai ý này mâu thuẫn về mặt tính toán. Project dùng `SLBan = SLBan + số lượng bán` để số tồn giảm đúng theo công thức trên.

Tài liệu có nút **Hủy** nhưng không mô tả chi tiết hành vi. Project chọn cách: Hủy hóa đơn chưa thanh toán, xóa chi tiết và hoàn lại `SLBan` tương ứng.

## 5. Cấu trúc

```text
TH06_ADONET/
├─ TH06_ADONET.sln
├─ Database/
│  ├─ 01_QLSinhVien.sql
│  └─ 02_QLCuaHang.sql
├─ TH06a/
│  ├─ TH06a.csproj
│  ├─ Program.cs
│  ├─ Db.cs
│  ├─ FrmMain.cs
│  ├─ FrmMonHoc.cs
│  ├─ FrmSinhVien.cs
│  ├─ FrmDiem.cs
│  └─ FrmLop.cs
└─ TH06b/
   ├─ TH06b.csproj
   ├─ Program.cs
   ├─ Db.cs
   ├─ FrmMain.cs
   ├─ FrmSinhVien.cs
   ├─ FrmDiem.cs
   ├─ FrmDanhSachLop.cs
   ├─ FrmDanhSachDiem.cs
   ├─ FrmCuaHangMain.cs
   ├─ FrmTonKho.cs
   ├─ FrmSanPham.cs
   └─ FrmBanHang.cs
```
