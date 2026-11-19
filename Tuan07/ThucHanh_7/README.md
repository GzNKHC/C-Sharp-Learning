# Thực hành 7 – Lập trình tương tác cơ sở dữ liệu nâng cao

Ba project C# Windows Forms dùng .NET Framework 4.8 và SQL Server. Mỗi project có thể mở và chạy riêng hoặc chạy từ solution `ThucHanh_7.sln`.

| Project | Màn hình và cách thao tác |
| --- | --- |
| `TH07a` | Quản lý lớp, sinh viên, điểm, danh mục hàng hóa và các màn hình mẫu. Lọc danh sách bằng combobox, thêm/xóa/sửa với `DataSet`, `SqlDataAdapter` và `SqlCommandBuilder`. Danh mục hàng hóa có tìm kiếm và đính kèm ảnh. |
| `TH07b` | Danh sách sinh viên lọc theo khoa và lớp, quản lý khoa mẫu, phiếu nhập hàng. Chọn Thêm hoặc Sửa, nhập trên các ô của biểu mẫu rồi bấm Lưu. |
| `TH07c` | Nhập điểm, nhập lớp và quản lý sinh viên mẫu. Nhập trực tiếp trên `DataGridView`. Có kiểm tra khóa, ràng buộc tham chiếu và lưu các dòng thay đổi trong một giao dịch. |

## Mở và chạy trên Windows

1. Cài workload **.NET desktop development** và **.NET Framework 4.8 targeting pack** cho Visual Studio. Cài SQL Server Express LocalDB hoặc dùng một SQL Server có sẵn.
2. Dùng SQL Server Management Studio hoặc SQL Server Object Explorer trong Visual Studio kết nối `(localdb)\MSSQLLocalDB`. Mở và chạy toàn bộ `Setup.sql` trong từng project cần chạy. Để chạy cả ba project, chạy cả ba script. Các script dùng chung CSDL `QLSinhVien_ThucHanh7` và chỉ tạo bảng, nạp dữ liệu mẫu khi bảng chưa tồn tại.
3. Nếu dùng instance khác, sửa **Data Source** trong `App.config` của các project cần chạy. Kết nối mặc định dùng xác thực Windows. Tài khoản chạy script cần có quyền tạo CSDL hoặc CSDL phải được tạo sẵn.
4. Mở `ThucHanh_7.sln`, nhấp phải vào `TH07a`, `TH07b` hoặc `TH07c`, chọn **Set as Startup Project** rồi nhấn **F5**. Chọn màn hình bài tập từ menu khi chương trình mở.

## Thao tác với dữ liệu mẫu

- **07a · Lớp và sinh viên.** Chọn bộ lọc phía trên lưới. Khi thêm mới, điền mã, tên và chọn khoa/lớp trong phần nhập liệu. Khi sửa hoặc xóa, chọn dòng tương ứng. Màn hình điểm lọc kết hợp sinh viên với môn học.
- **07a · Hàng hóa.** Bấm Thêm hoặc chọn dòng rồi bấm Sửa. Bấm Mở để sao chép ảnh vào `%LocalAppData%\ThucHanh_7\HinhHang` và hiển thị ảnh xem trước. Bấm Lưu để ghi tên ảnh vào CSDL. Để tìm kiếm, bấm Bỏ qua để xóa bộ lọc và các ô, điền tiêu chí rồi bấm Tìm kiếm.
- **07b · Sinh viên.** Chọn khoa, chọn lớp và bấm Tìm kiếm. Phần nhập liệu có ô chọn lớp riêng. Có thể thử xóa `SV001` để xem ràng buộc khi sinh viên đã có điểm.
- **07b · Phiếu nhập.** Nhập mã mới, chọn nhà cung cấp rồi bấm Tạo phiếu nhập. Chọn mã sản phẩm, nhập số lượng và bấm Thêm sản phẩm. Thêm lại cùng sản phẩm sẽ cộng dồn số lượng. Nhấp chuột phải vào một dòng để sửa số lượng hoặc xóa. Thành tiền được tính lại từ tổng các dòng đã lưu.
- **07c · Lưới.** Bấm Thêm để nhập các dòng mới ở cuối bảng, hoặc bấm Sửa để chỉnh các cột ngoài khóa của các dòng đã có. Nhấp đúp hoặc nhấn **F2** để nhập một ô. Ngày dùng định dạng `dd/MM/yyyy`, điểm thập phân dùng dấu phẩy, ví dụ `8,25`. Bấm Lưu để ghi các thay đổi hoặc Bỏ qua để hủy.
