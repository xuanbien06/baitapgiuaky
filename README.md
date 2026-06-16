# BÀI TẬP KẾT NỐI ORM 1 - QUẢN LÝ SÁCH

## 👤 Thông tin sinh viên
- **Họ và tên:** Lại Xuân Biển
- **Mã sinh viên:** BIT240041

## 📝 Mô tả dự án
Dự án được nâng cấp từ mã nguồn nền tảng `Lesson3-CNLTWeb` sang cấu trúc kết nối cơ sở dữ liệu thực tế sử dụng **Entity Framework Core (ORM)** và **SQL Server**, thay thế hoàn toàn việc lưu trữ tạm thời bằng `ArrayList`.

### Các tính năng đã hoàn thiện:
- [x] **Xem danh sách sách:** Lấy dữ liệu động trực tiếp từ bảng `Book` trong SQL Server.
- [x] **Thêm sách mới:** Kiểm tra tính hợp lệ (Validation) và lưu xuống CSDL bằng `DbContext.SaveChanges()`.
- [x] **Sửa thông tin sách:** Giữ luồng định danh bằng `input type="hidden" asp-for="Id"` để cập nhật chính xác bản ghi.
- [x] **Xóa sách:** Chức năng xóa an toàn có giao diện xác nhận trước khi thực thi lệnh `DELETE`.
- [x] **Tìm kiếm & Sắp xếp nâng cao:** Lọc dữ liệu theo Tên sách/Tác giả và sắp xếp Giá tiền tăng/giảm dần bằng cú pháp truy vấn LINQ.

## 🎥 Video thuyết minh báo cáo 
Bấm vào đường link dưới đây để xem video quá trình chạy thử ứng dụng và giải thích mã nguồn:
👉 [XEM VIDEO THUYẾT MINH BÀI TẬP TẠI ĐÂY](https://drive.google.com/file/d/1cta3YalnRyYd9gaJmzzwYgoseXgUZthN/view?usp=sharing)

## 🛠️ Cấu hình môi trường chạy
- **Hệ quản trị CSDL:** SQL Server Management Studio (SSMS)
- **Tên Server kết nối:** `XUANBIEN` (Windows Authentication)
- **Framework:** .NET Core (ASP.NET Core MVC)