# 🏋️ Gym Management System (Hệ Thống Quản Lý Phòng Gym & Fitness)

Hệ thống Quản lý Phòng Gym & Fitness được xây dựng trên nền tảng **Windows Forms (.NET Framework)** kết hợp cùng **Entity Framework 6** và hệ quản trị cơ sở dữ liệu **Microsoft SQL Server**. Dự án áp dụng mô hình phân quyền chặt chẽ theo vai trò (**Role-Based Access Control - RBAC**) và cơ chế mã hóa mật khẩu an toàn (**SHA-256**).

---

## 📌 Mục lục
- [Giới thiệu](#-giới-thiệu)
- [Tính năng nổi bật](#-tính-năng-nổi-bật)
- [Phân quyền người dùng (RBAC)](#-phân-quyền-người-dùng-rbac)
- [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
- [Cấu trúc thư mục](#-cấu-trúc-thư-mục)
- [Hướng dẫn cài đặt & khởi chạy](#-hướng-dẫn-cài-đặt--khởi-chạy)
- [Tài khoản mặc định](#-tài-khoản-mặc-định)
- [Phân công công việc nhóm](#-phân-công-công-việc-nhóm)

---

## 📖 Giới thiệu

Phần mềm hỗ trợ quản trị viên, nhân viên lễ tân và huấn luyện viên quản lý các hoạt động thường nhật của trung tâm thể hình:
- Quản lý thông tin tài khoản, phân quyền thao tác theo chức vụ.
- Quản lý danh sách hội viên, đăng ký thẻ và gói tập thể hình.
- Quản lý danh mục gói dịch vụ, giá bán và thời hạn sử dụng.
- Quản lý trang thiết bị, tình trạng cơ sở vật chất phòng tập.
- Lập và theo dõi hóa đơn thanh toán, doanh thu dịch vụ.

---

## ✨ Tính năng nổi bật

### 1. Hệ thống & Bảo mật
- **Đăng nhập / Đăng xuất an toàn:** Quản lý phiên làm việc thông qua `UserSession`.
- **Mã hóa mật khẩu:** Tích hợp thuật toán hàm băm **SHA-256** bảo mật thông tin tài khoản trong cơ sở dữ liệu.
- **Đổi mật khẩu:** Cho phép người dùng chủ động thay đổi mật khẩu định kỳ.
- **Giao diện MDI đa nhiệm:** Thiết kế MDI Container chuyên nghiệp, tự động căn chỉnh mở form con, tránh mở trùng lặp.

### 2. Quản trị phân quyền (Admin)
- **Quản lý người dùng (Users):** Thêm, sửa, khóa/mở khóa tài khoản, tìm kiếm theo tên hoặc mã.
- **Quản lý vai trò (Roles):** Cấu hình các nhóm vai trò và quyền thao tác trên menu/chức năng.

### 3. Nghiệp vụ Hội viên & Gói tập
- Quản lý hồ sơ hội viên (Họ tên, SĐT, Email, Mã QR check-in).
- Quản lý gói tập (Gói theo tháng, quý, năm, gói kèm PT cá nhân).
- Đăng ký gói dịch vụ cho hội viên, theo dõi ngày bắt đầu và kết thúc gói.

### 4. Thu ngân & Hóa đơn
- Tạo và xuất hóa đơn dịch vụ khi hội viên đăng ký hoặc gia hạn gói tập.
- Lưu trữ lịch sử giao dịch và chi tiết hóa đơn theo nhân viên tiếp nhận.

### 5. Quản lý thiết bị & Huấn luyện viên (PT)
- Quản lý danh sách huấn luyện viên cá nhân và chuyên môn.
- Theo dõi số lượng, tình trạng hoạt động của máy tập, thiết bị thể hình.
- Báo cáo và thống kê doanh thu phòng gym theo mốc thời gian.

---

## 👥 Phân quyền người dùng (RBAC)

Hệ thống tự động bật/tắt menu và nút chức năng dựa trên vai trò của người đăng nhập:

| Chức năng / Phân hệ | Quản trị viên (Admin) | Nhân viên Lễ tân | Huấn luyện viên (PT) |
| :--- | :---: | :---: | :---: |
| **Quản trị Tài khoản & Phân quyền** |  Toàn quyền |  Bị ẩn |  Bị ẩn |
| **Đăng ký Gói tập & Lập hóa đơn** |  Toàn quyền |  Cho phép |  Bị khóa |
| **Quản lý Hội viên & Danh mục Gói** |  Toàn quyền |  Cho phép |  Chỉ xem |
| **Quản lý Thiết bị & Tra cứu HLV** |  Toàn quyền |  Cho phép |  Cho phép |
| **Xem Báo cáo & Thống kê Doanh thu**|  Toàn quyền |  Bị khóa |  Bị khóa |

---

## 🛠 Công nghệ sử dụng

- **Ngôn ngữ:** C# (.NET Framework 4.7.2)
- **Giao diện:** Windows Forms (MDI Application)
- **ORM / Truy vấn:** Entity Framework 6 (Database-First)
- **Cơ sở dữ liệu:** Microsoft SQL Server
- **Bảo mật:** SHA-256 Hashing (`SecurityHelper.cs`)
- **IDE đề xuất:** Visual Studio 2019 / 2022

---

## 📁 Cấu trúc thư mục

```text
GymManagementSystem/
│
├── GymManagementDB.sql            # Script tạo Database & Dữ liệu mẫu (Seed Data)
├── GymManagementSystem.sln        # Solution chính của dự án Visual Studio
│
├── LTWCHARLIEKODE/                # Project WinForms chính
│   ├── App.config                 # Chuỗi kết nối Database & Cấu hình Entity Framework
│   ├── Program.cs                 # Điểm khởi chạy ứng dụng
│   ├── UserSession.cs             # Quản lý phiên làm việc của người dùng đăng nhập
│   ├── SecurityHelper.cs          # Mã hóa mật khẩu chuẩn SHA-256
│   │
│   ├── GymModel.edmx              # Sơ đồ Entity Framework Data Model
│   │
│   ├── Form_Login.cs              # Giao diện Đăng nhập hệ thống
│   ├── Form_Main.cs               # Giao diện chính MDI điều hướng & phân quyền
│   ├── Form_ChangePassword.cs     # Giao diện Đổi mật khẩu
│   ├── Form_UserManagement.cs     # Giao diện Quản lý Người dùng
│   ├── Form_RoleManagement.cs     # Giao diện Quản lý Vai trò
│   │
│   └── Properties/                # Assembly, Resources và Cài đặt
└── packages/                      # Thư viện NuGet (EntityFramework 6.x)
```

---

## 🚀 Hướng dẫn cài đặt & khởi chạy

### Bước 1: Khởi tạo Cơ sở dữ liệu (SQL Server)
1. Mở công cụ **SQL Server Management Studio (SSMS)**.
2. Kết nối tới SQL Server instance của bạn (ví dụ: `.\SQLEXPRESS` hoặc `localhost`).
3. Mở tệp `GymManagementDB.sql` nằm ở thư mục gốc của dự án.
4. Nhấn **Execute (F5)** để tự động tạo cơ sở dữ liệu `GymManagementDB` cùng các bảng và dữ liệu mẫu khởi tạo.

### Bước 2: Cấu hình chuỗi kết nối (Connection String)
Mở tệp `LTWCHARLIEKODE/App.config` và kiểm tra lại `connectionString` sao cho phù hợp với máy của bạn:

```xml
<connectionStrings>
  <add name="GymManagementDBEntities1" 
       connectionString="metadata=res://*/GymModel.csdl|res://*/GymModel.ssdl|res://*/GymModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=.\SQLEXPRESS;initial catalog=GymManagementDB;integrated security=True;encrypt=False;trustservercertificate=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" 
       providerName="System.Data.EntityClient" />
</connectionStrings>
```
*(Nếu SQL Server của bạn có tên instance khác `.\SQLEXPRESS`, hãy cập nhật lại tham số `data source` tương ứng).*

### Bước 3: Mở và chạy dự án
1. Mở file `GymManagementSystem.sln` bằng **Visual Studio**.
2. Nhấn chuột phải vào Solution chọn **Restore NuGet Packages** để nạp Entity Framework.
3. Chọn cấu hình **Debug** hoặc **Release**, sau đó nhấn **F5** (hoặc nút **Start**) để khởi chạy phần mềm.

---

## 🔑 Tài khoản mặc định

Sau khi chạy script `GymManagementDB.sql`, hệ thống được nạp sẵn các tài khoản demo:

| Tên đăng nhập | Mật khẩu mặc định | Họ và tên | Vai trò | Ghi chú |
| :--- | :---: | :--- | :--- | :--- |
| `admin` | `123456` | Quản Trị Viên | **Admin** | Toàn quyền kiểm soát hệ thống |
| `tuan` | `123456` | Hứa Nhựt Tuấn | **Admin** | Tài khoản quản trị viên chính |
| `letan` | `123456` | Nguyễn Văn Lễ Tân | **Lễ Tân** | Nghiệp vụ bán gói & lập hóa đơn |
| `pt_tuan` | `123456` | Hứa Nhựt Tuấn (PT) | **PT** | Xem hội viên & thiết bị phòng tập |

---

## 👥 Phân công công việc nhóm

Dự án được phân chia theo từng module rõ ràng nhằm đảm bảo hiệu quả làm việc nhóm:

1. **Trưởng nhóm (Team Leader):**
   - Thiết kế kiến trúc tổng thể, cơ sở dữ liệu SQL Server.
   - Module Hệ thống: Đăng nhập (`Form_Login`), Đổi mật khẩu (`Form_ChangePassword`), Form chính MDI (`Form_Main`), Phân quyền (`RBAC`), Quản lý tài khoản (`Form_UserManagement`) và Quản lý vai trò (`Form_RoleManagement`).
2. **Thành viên 2:**
   - Quản lý Hội viên (`Form_MemberManagement`).
   - Quản lý Danh mục Gói tập (`Form_PackageManagement`).
3. **Thành viên 3:**
   - Đăng ký gói tập cho Hội viên (`Form_RegisterPackage`).
   - Lập và Quản lý Hóa đơn thanh toán (`Form_InvoiceManagement`).
4. **Thành viên 4:**
   - Quản lý Huấn luyện viên / PT (`Form_TrainerManagement`).
   - Quản lý Thiết bị & Máy tập (`Form_EquipmentManagement`).
   - Báo cáo thống kê & Doanh thu (`Form_RevenueReport`).

---

## 📄 Bản quyền & Đóng góp
Dự án được thực hiện phục vụ học tập và nghiên cứu môn học **Lập trình Windows (LTW)**. Mọi đóng góp và cải tiến vui lòng tạo Pull Request hoặc liên hệ người quản lý dự án.
