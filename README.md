# 📘 ĐỒ ÁN LẬP TRÌNH .NET – QUẢN LÝ GIÁO VIÊN THPT

<p align="center">
Nhóm Đồ Án 14 - DH24TH2 - Nhóm Môn Học 2 - Tổ Thực Hành 2
</p>

## 👨‍💻 Công nghệ sử dụng

* **Ngôn ngữ:** C#, .NET Framework / WinForms
* **CSDL:** SQL Server (T-SQL)
* **Thư viện hỗ trợ:**

  * ClosedXML (xuất Excel)
  * System.Data.SqlClient

---

## 🏫 Chủ đề: **Quản lý giáo viên THPT**

Phần mềm hỗ trợ trường THPT trong việc quản lý thông tin giáo viên, phân công giảng dạy và tạo thời khóa biểu tự động/dựa theo ràng buộc.

---

## 🎯 Mục tiêu chức năng

### 1. Quản lý giáo viên

* Thêm, sửa, xóa, tìm kiếm giáo viên
* Quản lý thông tin: Mã GV, họ tên, giới tính, số điện thoại, địa chỉ...

### 2. Quản lý môn học

* Thêm, sửa, xóa, tìm kiếm môn học
* Mã môn, tên môn.

### 3. Quản lý lớp học

* Thông tin lớp: mã lớp, tên lớp, sĩ số

### 4. Phân công giảng dạy

* Chọn GV – Môn – Lớp – Số tiết
* Kiểm tra ràng buộc giáo viên (trùng lịch, trùng môn)
* Tự động cập nhật dữ liệu TKB

### 5. Thời khóa biểu (TKB)

* Xem TKB theo:

  * Giáo viên
  * Lớp học
* Ràng buộc:

  * Không trùng tiết của cùng giáo viên
  * Không trùng phòng/lớp
  * Không vượt số tiết môn trong tuần

### 6. Đăng nhập – phân quyền

* Admin: toàn quyền dữ liệu
* User (giáo viên): chỉ xem TKB của mình

### 7. Xuất Excel

* Xuất danh sách giáo viên
* Xuất thời khóa biểu

---

## 🗂️ Cấu trúc thư mục

```
QLGV_THPT/
│── Database.cs
│── fLogin.cs
│── fGiaoVien.cs
│── fMonHoc.cs
│── fLop.cs
│── fPhanCong.cs
│── fTKB.cs
│── Program.cs
│── App.config
└── fTKB_Edit.cs
```

---

## 📦 Hướng dẫn cài đặt

### 1. Cài SQL Server + SSMS

### 2. Chạy script tạo CSDL

* File: **db.sql**
* Copy script trong mục Database vào SSMS rồi chạy.

### 3. Cấu hình chuỗi kết nối

Trong *App.config*:

```xml
<connectionStrings>
  <add name="conn" connectionString="Data Source=.;Initial Catalog=QLGV;Integrated Security=True" />
</connectionStrings>
```

### 4. Chạy chương trình

* Mở solution bằng Visual Studio
* Nhấn **Start (F5)**

---

## 🖼️ Một số giao diện chính

* Form đăng nhập
* Form quản lý giáo viên
* Form phân công
* Form thời khóa biểu dạng lưới (DataGridView)

---

## 📝 Tác giả

| Tên               | MSSV      | Lớp     |
| ----------------- | --------- | ------- |
| Nguyễn Thị Trà My | DTH235701 | DH24TH2 |
| La Thanh Pats     | DTH235727 | DH24TH2 |

---
## 📬 Liên hệ
- 📧 Email: <p>  - my_dth235701@student.agu.edu.vn</p>- pats_dth235727@student.agu.edu.vn

- 📌 GitHub:<p>  - [DTH235701-NguyenThiTraMy](https://github.com/DTH235701-NguyenThiTraMy) </p>- [DTH235727LATHANHPATS](https://github.com/DTH235727LATHANHPATS)
