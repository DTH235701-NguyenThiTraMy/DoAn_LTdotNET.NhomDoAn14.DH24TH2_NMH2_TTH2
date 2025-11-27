-- ============================
--       TẠO DATABASE
-- ============================
USE master;
GO

-- Đảm bảo không có kết nối nào đang giữ database
IF DB_ID('QLGV') IS NOT NULL
BEGIN
    ALTER DATABASE QLGV SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLGV;
END
GO

CREATE DATABASE QLGV;
GO
USE QLGV;
GO

--------------------------------------------------------------------------------
--                              BẢNG MÔN HỌC
--------------------------------------------------------------------------------
CREATE TABLE MonHoc (
    MaMon VARCHAR(10) PRIMARY KEY,
    TenMon NVARCHAR(100) NOT NULL
);

INSERT INTO MonHoc VALUES
('TOAN', N'Toán học'),
('VAN', N'Ngữ văn'),
('ANH', N'Tiếng Anh'),
('LY', N'Vật lý'),
('HOA', N'Hóa học'),
('SINH', N'Sinh học'),
('SU', N'Lịch sử'),
('DIA', N'Địa lý'),
('GDCD', N'Giáo dục công dân'),
('TD', N'Thể dục');

--------------------------------------------------------------------------------
--                              BẢNG GIÁO VIÊN
--------------------------------------------------------------------------------
CREATE TABLE GiaoVien (
    MaGV VARCHAR(10) PRIMARY KEY,
    HoLot NVARCHAR(100) NOT NULL,
    Ten NVARCHAR(50) NOT NULL,
    GioiTinh NVARCHAR(10) CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    NgaySinh DATE,
    SDT VARCHAR(15),
    DiaChi NVARCHAR(200)
);

INSERT INTO GiaoVien VALUES
('GV01', N'Nguyễn Văn', N'An', N'Nam', '1985-02-10', '0912345678', N'An Giang'),
('GV02', N'Trần Thị', N'Bình', N'Nữ', '1990-09-05', '0988776655', N'Cần Thơ'),
('GV03', N'Phạm Minh', N'Khoa',N'Nam', '1983-12-20', '0911122233', N'Kiên Giang'),
('GV04', N'Lê Thị', N'Chi', N'Nữ', '1988-05-15', '0922334455', N'Hậu Giang'),
('GV05', N'Hoàng', N'Hùng', N'Nam', '1979-03-03', '0911223344', N'Sóc Trăng'),
('GV06', N'Ngô', N'Lan', N'Nữ', '1992-07-07', '0909876543', N'Vĩnh Long'),
('GV07', N'Phan', N'Tuấn', N'Nam', '1986-08-08', '0911334455', N'Trà Vinh'),
('GV08', N'Bùi', N'Vy', N'Nữ', '1987-11-11', '0988123456', N'Bạc Liêu'),
('GV09', N'Tống', N'Hải', N'Nam', '1984-06-06', '0912998877', N'Cà Mau'),
('GV10', N'Đặng', N'Hạnh', N'Nữ', '1991-04-04', '0922113344', N'Kiên Giang'),
('GV11', N'Nhâm Phương', N'Nam', N'Nam', '1996-02-10', '0867966991', N'An Giang'),
('GV12', N'Bùi Bích', N'Phương', N'Nữ', '1989-10-01', '0825369147', N'An Giang'),
('GV13', N'Đặng Thành', N'An',N'Nam', '1996-12-20', '0901234567', N'Kiên Giang'),
('GV14', N'Phương Mỹ', N'Chi', N'Nữ', '2003-01-13', '0838112309', N'Hậu Giang'),
('GV15', N'Trần Tất', N'Vũ', N'Nam', '1990-04-01', '0345678901', N'Sóc Trăng'),
('GV16', N'Ngô Lan', N'Hương', N'Nữ', '1999-08-28', '0777888999', N'Vĩnh Long'),
('GV17', N'Nguyễn Thành', N'Dương', N'Nam', '1986-02-20', '0862468135', N'Trà Vinh'),
('GV18', N'Trần Diệu', N'Huyền', N'Nữ', '2003-03-11', '0912003456', N'Bạc Liêu'),
('GV19', N'Bùi Duy', N'Ngọc', N'Nam', '1996-10-06', '0587901234', N'Cà Mau'),
('GV20', N'Khương Hoàn', N'Mỹ', N'Nữ', '1996-05-04', '0825369147', N'Đồng Tháp');
Go

--------------------------------------------------------------------------------
--                              BẢNG LỚP
--------------------------------------------------------------------------------
CREATE TABLE Lop (
    MaLop VARCHAR(10) PRIMARY KEY,
    TenLop NVARCHAR(50) NOT NULL,
    SiSo INT CHECK (SiSo >= 0)
);

INSERT INTO Lop VALUES
('10A1', N'Lớp 10A1', 45),
('10A2', N'Lớp 10A2', 42),
('10A3', N'Lớp 10A3', 40),
('11A1', N'Lớp 11A1', 44),
('11A2', N'Lớp 11A2', 41),
('11A3', N'Lớp 11A3', 39),
('12A1', N'Lớp 12A1', 43),
('12A2', N'Lớp 12A2', 40),
('12A3', N'Lớp 12A3', 38),
('12A4', N'Lớp 12A4', 36);

--------------------------------------------------------------------------------
--                              BẢNG PHÂN CÔNG
--------------------------------------------------------------------------------
CREATE TABLE PhanCong (
    MaPC INT IDENTITY(1,1) PRIMARY KEY,
    MaGV VARCHAR(10) NOT NULL,
    MaMon VARCHAR(10) NOT NULL,
    MaLop VARCHAR(10) NOT NULL,
    FOREIGN KEY (MaGV) REFERENCES GiaoVien(MaGV),
    FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    -- Ràng buộc: Một GV chỉ dạy một môn duy nhất tại một lớp duy nhất
    CONSTRAINT UQ_PhanCong UNIQUE (MaGV, MaMon, MaLop)
);

-- CHỈ SỬ DỤNG PHẦN INSERT ĐẦU TIÊN ĐỂ TRÁNH LỖI TRÙNG KHÓA UNIQUE (MaGV, MaMon, MaLop)
INSERT INTO PhanCong (MaGV, MaMon, MaLop) VALUES
-- Lớp 10A1
('GV01', 'TOAN', '10A1'), -- Nguyễn Văn An - Toán 10A1
('GV02', 'VAN',  '10A1'), -- Trần Thị Bình - Văn 10A1
('GV03', 'LY',   '10A1'),
('GV04', 'HOA',  '10A1'),
('GV05', 'SINH', '10A1'),
('GV06', 'ANH',  '10A1'),
('GV07', 'TD',   '10A1'),

-- Lớp 10A2
('GV01', 'TOAN', '10A2'),
('GV02', 'VAN',  '10A2'),
('GV03', 'LY',   '10A2'),
('GV04', 'HOA',  '10A2'),
('GV05', 'SINH', '10A2'),
('GV06', 'ANH',  '10A2'),
('GV07', 'TD',   '10A2'),

-- Lớp 11A1
('GV08', 'TOAN', '11A1'),
('GV09', 'VAN',  '11A1'),
('GV10', 'LY',   '11A1'),
('GV04', 'HOA',  '11A1'),
('GV05', 'SINH', '11A1'),
('GV06', 'ANH',  '11A1'),
('GV07', 'TD',   '11A1'),

-- Lớp 12A1
('GV08', 'TOAN', '12A1'),
('GV09', 'VAN',  '12A1'),
('GV10', 'LY',   '12A1'),
('GV04', 'HOA',  '12A1'),
('GV05', 'SINH', '12A1'),
('GV06', 'ANH',  '12A1'),
('GV07', 'TD',   '12A1'),

-- Lớp 12A2
('GV11', 'TOAN', '12A2'), -- Nhâm Phương Nam - Toán 12A2
('GV12', 'VAN',  '12A2'),
('GV13', 'LY',   '12A2'),
('GV14', 'HOA',  '12A2'),
('GV15', 'SINH', '12A2'),
('GV16', 'ANH',  '12A2'),
('GV17', 'TD',   '12A2');
GO

--------------------------------------------------------------------------------
--                              BẢNG THỜI KHÓA BIỂU
--------------------------------------------------------------------------------
CREATE TABLE ThoiKhoaBieu (
    MaTKB INT IDENTITY(1,1) PRIMARY KEY,
    MaLop VARCHAR(10) NOT NULL,
    Thu INT NOT NULL CHECK (Thu BETWEEN 2 AND 7),
    Tiet INT NOT NULL CHECK (Tiet BETWEEN 1 AND 10),
    MaGV VARCHAR(10) NOT NULL,
    MaMon VARCHAR(10) NOT NULL,
    FOREIGN KEY (MaLop) REFERENCES Lop(MaLop),
    FOREIGN KEY (MaGV) REFERENCES GiaoVien(MaGV),
    FOREIGN KEY (MaMon) REFERENCES MonHoc(MaMon),
    CONSTRAINT UQ_TKB UNIQUE (MaLop, Thu, Tiet), -- Một lớp chỉ có 1 môn tại 1 thời điểm
    CONSTRAINT UQ_TKB_GV UNIQUE (MaGV, Thu, Tiet) -- Một GV chỉ dạy 1 tiết tại 1 thời điểm
);

-- Lớp 10A1: Dạy bởi các GV01-GV07
INSERT INTO ThoiKhoaBieu (MaLop, Thu, Tiet, MaGV, MaMon)
SELECT '10A1', 2, 1, MaGV, MaMon FROM PhanCong WHERE MaLop='10A1' AND MaMon='TOAN'
UNION ALL
SELECT '10A1', 2, 2, MaGV, MaMon FROM PhanCong WHERE MaLop='10A1' AND MaMon='VAN' -- Sửa từ TOAN sang VAN để tránh trùng tiết GV01
UNION ALL
SELECT '10A1', 2, 3, MaGV, MaMon FROM PhanCong WHERE MaLop='10A1' AND MaMon='LY'
UNION ALL
SELECT '10A1', 2, 4, MaGV, MaMon FROM PhanCong WHERE MaLop='10A1' AND MaMon='HOA'
UNION ALL
SELECT '10A1', 2, 5, MaGV, MaMon FROM PhanCong WHERE MaLop='10A1' AND MaMon='SINH';

-- Lớp 10A2: Dạy bởi các GV01-GV07
INSERT INTO ThoiKhoaBieu (MaLop, Thu, Tiet, MaGV, MaMon)
SELECT '10A2', 3, 1, MaGV, MaMon FROM PhanCong WHERE MaLop='10A2' AND MaMon='TOAN'
UNION ALL
SELECT '10A2', 3, 2, MaGV, MaMon FROM PhanCong WHERE MaLop='10A2' AND MaMon='VAN'
UNION ALL
SELECT '10A2', 3, 3, MaGV, MaMon FROM PhanCong WHERE MaLop='10A2' AND MaMon='LY'
UNION ALL
SELECT '10A2', 3, 4, MaGV, MaMon FROM PhanCong WHERE MaLop='10A2' AND MaMon='HOA'
UNION ALL
SELECT '10A2', 3, 5, MaGV, MaMon FROM PhanCong WHERE MaLop='10A2' AND MaMon='SINH';

-- Lớp 11A1: Dạy bởi các GV08-GV10, GV04-GV07
INSERT INTO ThoiKhoaBieu (MaLop, Thu, Tiet, MaGV, MaMon)
SELECT '11A1', 4, 1, MaGV, MaMon FROM PhanCong WHERE MaLop='11A1' AND MaMon='TOAN'
UNION ALL
SELECT '11A1', 4, 2, MaGV, MaMon FROM PhanCong WHERE MaLop='11A1' AND MaMon='VAN'
UNION ALL
SELECT '11A1', 4, 3, MaGV, MaMon FROM PhanCong WHERE MaLop='11A1' AND MaMon='LY'
UNION ALL
SELECT '11A1', 4, 4, MaGV, MaMon FROM PhanCong WHERE MaLop='11A1' AND MaMon='HOA'
UNION ALL
SELECT '11A1', 4, 5, MaGV, MaMon FROM PhanCong WHERE MaLop='11A1' AND MaMon='SINH';

-- Lớp 12A1: Dạy bởi các GV08-GV10, GV04-GV07
INSERT INTO ThoiKhoaBieu (MaLop, Thu, Tiet, MaGV, MaMon)
SELECT '12A1', 5, 1, MaGV, MaMon FROM PhanCong WHERE MaLop='12A1' AND MaMon='TOAN'
UNION ALL
SELECT '12A1', 5, 2, MaGV, MaMon FROM PhanCong WHERE MaLop='12A1' AND MaMon='VAN'
UNION ALL
SELECT '12A1', 5, 3, MaGV, MaMon FROM PhanCong WHERE MaLop='12A1' AND MaMon='LY'
UNION ALL
SELECT '12A1', 5, 4, MaGV, MaMon FROM PhanCong WHERE MaLop='12A1' AND MaMon='HOA'
UNION ALL
SELECT '12A1', 5, 5, MaGV, MaMon FROM PhanCong WHERE MaLop='12A1' AND MaMon='SINH';
Go

--------------------------------------------------------------------------------
--                              BẢNG TÀI KHOẢN
--------------------------------------------------------------------------------
CREATE TABLE TaiKhoan (
    Username VARCHAR(50) PRIMARY KEY,
    Password VARCHAR(100) NOT NULL,
    Role VARCHAR(20) DEFAULT 'user' CHECK (Role IN ('admin','user')),
    MaGV VARCHAR(10) NULL,
    FOREIGN KEY (MaGV) REFERENCES GiaoVien(MaGV)
);

INSERT INTO TaiKhoan VALUES
('admin', '123', 'admin', NULL),
('gv01', '123456', 'user', 'GV01'),
('gv02', '123456', 'user', 'GV02'),
('gv03', '123456', 'user', 'GV03'),
('gv04', '123456', 'user', 'GV04'),
('gv05', '123456', 'user', 'GV05'),
('gv06', '123456', 'user', 'GV06'),
('gv07', '123456', 'user', 'GV07'),
('gv08', '123456', 'user', 'GV08'),
('gv09', '123456', 'user', 'GV09'),
('gv10', '123456', 'user', 'GV10'),
('gv11', '123456', 'user', 'GV11'),
('gv12', '123456', 'user', 'GV12'),
('gv13', '123456', 'user', 'GV13'),
('gv14', '123456', 'user', 'GV14'),
('gv15', '123456', 'user', 'GV15'),
('gv16', '123456', 'user', 'GV16'),
('gv17', '123456', 'user', 'GV17'),
('gv18', '123456', 'user', 'GV18'),
('gv19', '123456', 'user', 'GV19'),
('gv20', '123456', 'user', 'GV20');
Go

--------------------------------------------------------------------------------
--                              VIEW HỖ TRỢ
--------------------------------------------------------------------------------
CREATE VIEW vw_PhanCong AS
SELECT 
    PC.MaPC,
    PC.MaGV,
    GV.HoLot,
    GV.Ten,
    GV.HoLot + ' ' + GV.Ten AS HoTen,
    PC.MaMon,
    MH.TenMon,
    PC.MaLop,
    L.TenLop
FROM PhanCong PC
JOIN GiaoVien GV ON PC.MaGV = GV.MaGV
JOIN MonHoc MH ON PC.MaMon = MH.MaMon
JOIN Lop L ON PC.MaLop = L.MaLop;
GO

CREATE VIEW vw_TKB AS
SELECT T.MaTKB, T.MaLop, L.TenLop, 
    T.Thu, T.Tiet,
    T.MaGV, GV.HoLot, GV.Ten,
    T.MaMon, MH.TenMon
FROM ThoiKhoaBieu T
JOIN Lop L ON T.MaLop = L.MaLop
JOIN GiaoVien GV ON T.MaGV = GV.MaGV
JOIN MonHoc MH ON T.MaMon = MH.MaMon;
GO

-- Xóa dữ liệu TKB
DELETE FROM ThoiKhoaBieu;
DBCC CHECKIDENT ('ThoiKhoaBieu', RESEED, 0);
-- Xóa dữ liệu phân công
DELETE FROM PhanCong;
DBCC CHECKIDENT ('PhanCong', RESEED, 0);