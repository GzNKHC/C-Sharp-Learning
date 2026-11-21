# Thực hành 09 – LINQ to SQL

Solution `ThucHanh_9.sln` có 13 project C# Windows Forms. Mỗi project sử dụng cơ sở dữ liệu SQL Server cùng tên project và có menu chọn **ADO.NET**, **LINQ to SQL** hoặc **Entity Framework (EDM)**. Các lựa chọn đọc và ghi vào cùng dữ liệu của project nên có thể đổi cách kết nối để đối chiếu kết quả.

| Câu | Project | Nội dung |
| --- | --- | --- |
| 1 | `QLTS` | Nhập và hiệu chỉnh điểm thi theo phòng, ưu tiên tuyển sinh, ngành học, tra cứu theo số báo danh hoặc họ tên. |
| 2 | `QLGV` | Lọc giáo viên theo cơ sở và đơn vị, xem chi tiết hoặc xóa giáo viên từ menu chuột phải, quản lý đơn vị. |
| 3 | `QLD` | Quản lý sinh viên, học phần và đăng ký học phần. Nhập điểm, tra cứu theo học kỳ hoặc học phần. |
| 4 | `QLTB` | Lập, sửa, xóa phiếu nhập. Lập phiếu mượn và phiếu trả, cập nhật số thiết bị tốt và hỏng. |
| 5 | `QLNS` | Quản lý nhân viên, lọc theo phòng ban, sắp xếp theo tên và cập nhật danh mục phòng ban. |
| 6 | `QLBD` | Quản lý băng đĩa, khách thuê và thanh toán tiền thuê. |
| 7 | `QLBT` | Quản lý đơn bán thuốc, danh mục thuốc, chi tiết đơn hàng, tìm kiếm theo tên hoặc công dụng, lập đơn cho khách. |
| 8 | `QLTV` | Quản lý độc giả, sách, phiếu mượn và chi tiết mượn. Tìm sách, nhận trả sách và kiểm tra quá hạn. |
| 9 | `QLBH` | Quản lý mặt hàng, đại lý và đơn hàng. Lập chi tiết, tra cứu và hủy nhiều đơn theo đại lý. |
| 10 | `QLKH` | Thêm, sửa, xóa và tìm kiếm hãng sản xuất, điện thoại. |
| 11 | `QLVDV` | Thêm, sửa, xóa và tìm kiếm thành viên, hoạt động và phiếu tham gia. |
| 12 | `QLTX` | Thêm, sửa, xóa và tìm kiếm xe, khách hàng, yêu cầu thuê. Kiểm tra số xe còn trống theo khoảng ngày. |
| 13 | `QLCB` | Quản lý và tra cứu hệ số lương theo nhân viên hoặc phòng ban, kèm hồ sơ và danh mục liên quan. |

## Chạy trên Windows

1. Cài workload **.NET desktop development** và **.NET Framework 4.8 Developer Pack** trong Visual Studio. Cài SQL Server Express LocalDB hoặc dùng instance SQL Server sẵn có.
2. Mở `ThucHanh_9.sln` sau khi giải nén và cho Visual Studio khôi phục gói NuGet **EntityFramework 6.5.1**. Có thể chọn **Restore NuGet Packages** từ menu chuột phải của solution.
3. Kết nối với `(localdb)\MSSQLLocalDB` từ SSMS hoặc SQL Server Object Explorer. Chạy toàn bộ file `Setup.sql` của project muốn mở. Mỗi script tạo bảng và nạp dữ liệu mẫu khi bảng chưa tồn tại. Nếu dùng instance khác, đổi `Data Source` hoặc thông tin xác thực trong `App.config` của project.
4. Nhấp phải vào project, chọn **Set as Startup Project**, sau đó nhấn **F5**. Chọn cách kết nối ở menu và mở màn hình cần chạy.

## Mô hình dữ liệu

Mỗi thư mục `Models` có `Entities.cs` và sơ đồ LINQ to SQL `*.dbml`. LINQ to SQL dùng lớp đã ánh xạ qua attribute `Table` và `Column`. EDM có `Model.edmx` cùng ba file `Model.csdl`, `Model.ssdl` và `Model.msl` mà Entity Framework đọc khi chạy. Designer của Visual Studio cần thành phần **LINQ to SQL tools** hoặc **Entity Framework 6 tools** tương ứng để xem sơ đồ. Thư mục `DungChung` chứa mã truy cập dữ liệu và các điều khiển được liên kết vào từng project.

## Thao tác thử

- **QLTS** · Phòng 1 có thí sinh chưa nhập điểm. Phòng 2 có thí sinh đã có điểm. Tra cứu **003** để đối chiếu điểm cộng và tổng điểm.
- **QLD** · Chọn kỳ 1 và học phần Cơ sở dữ liệu để xem điểm đã có. Chọn kỳ 2 và Lập trình C# để thử nhập điểm cho các sinh viên đã đăng ký.
- **QLTB** · `PM001` còn 2 thiết bị đang mượn. Có thể trả mỗi lần một thiết bị và chọn tình trạng Tốt hoặc Hỏng.
- **QLBD** · Tìm `BD01`, chọn lượt thuê rồi bấm **Tính tiền**. Số ngày gồm ngày thuê và ngày thanh toán. Đơn giá theo đề là 5.000 đồng/ngày.
- **QLTV** · Độc giả `DG01` có sách chưa trả ở phiếu `PM001`. Thử kiểm tra quá hạn, sau đó mở **Nhận trả sách**. Ở màn hình nhập chi tiết, bấm **Mượn** để đưa sách vào danh sách chờ rồi bấm **Lưu**.
- **QLBH** · Chọn đại lý `DL01` để xem đơn và chi tiết. Màn hình hủy đơn hỗ trợ chọn nhiều dòng bằng Ctrl hoặc Shift.

## Quy ước theo đề

- `QLTS` thêm mã ngành và mã ưu tiên vào thông tin thí sinh để thực hiện phép tra cứu mà đề yêu cầu.
- `QLD` có danh mục lớp và bảng đăng ký học phần để xác định danh sách sinh viên được nhập điểm theo môn.
- `QLTB` có mã riêng cho mỗi phiếu để theo dõi nhiều lần mượn hoặc trả. Khi xóa phiếu mượn đã trả đủ, phiếu trả giữ số phiếu mượn gốc để tra cứu.
- `QLBD` lưu ngày thanh toán và thành tiền trên từng lượt thuê trong `THUE`. Một khách hàng có thể có nhiều lượt thuê.
