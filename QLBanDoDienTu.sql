USE master;
GO

-- Xóa database nếu tồn tại
IF DB_ID('QLBanDoDienTu') IS NOT NULL
BEGIN
    ALTER DATABASE QLBanDoDienTu SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLBanDoDienTu;
END
GO

-- Tạo database mới
CREATE DATABASE QLBanDoDienTu;
GO

USE QLBanDoDienTu;
GO


-- SANPHAM
CREATE TABLE SANPHAM (
    MaSP       VARCHAR(10)     NOT NULL PRIMARY KEY,
    TenSP      NVARCHAR(100)   NOT NULL,
    DonGia     DECIMAL(18,2)   NOT NULL CHECK (DonGia > 0),
    SoLuong    INT             NOT NULL CHECK (SoLuong >= 0)
);
GO


-- KHACHHANG
CREATE TABLE KHACHHANG (
    MaKH       VARCHAR(10)     NOT NULL PRIMARY KEY,
    HoTen      NVARCHAR(100)   NOT NULL,
    SDT        VARCHAR(15)     NOT NULL,
    DiaChi     NVARCHAR(200)   NULL
);
GO


-- NHANVIEN
CREATE TABLE NHANVIEN (
    MaNV       VARCHAR(10)     NOT NULL PRIMARY KEY,
    HoTen      NVARCHAR(100)   NOT NULL,
    ChucVu     NVARCHAR(50)    NOT NULL,
    SDT        VARCHAR(15)     NOT NULL,
    DiaChi     NVARCHAR(200)   NULL
);
GO


-- HOADON
CREATE TABLE HOADON (
    MaHD       VARCHAR(10)     NOT NULL PRIMARY KEY,
    MaKH       VARCHAR(10)     NOT NULL,
    MaNV       VARCHAR(10)     NOT NULL,
    NgayLap    DATE            NOT NULL,
    TongTien   DECIMAL(18,2)   NOT NULL CHECK (TongTien >= 0),

    CONSTRAINT FK_HOADON_KHACHHANG FOREIGN KEY (MaKH)
        REFERENCES KHACHHANG(MaKH)
        ON UPDATE CASCADE
        ON DELETE NO ACTION,

    CONSTRAINT FK_HOADON_NHANVIEN FOREIGN KEY (MaNV)
        REFERENCES NHANVIEN(MaNV)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO


-- CHITIETHOADON
CREATE TABLE CHITIETHOADON (
    MaCTHD     VARCHAR(10)     NOT NULL PRIMARY KEY,
    MaHD       VARCHAR(10)     NOT NULL,
    MaSP       VARCHAR(10)     NOT NULL,
    DonGia     DECIMAL(18,2)   NOT NULL,
    SoLuong    INT             NOT NULL CHECK (SoLuong > 0),
    ThanhTien  DECIMAL(18,2)   NOT NULL,

    CONSTRAINT FK_CTHD_HOADON FOREIGN KEY (MaHD)
        REFERENCES HOADON(MaHD)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT FK_CTHD_SANPHAM FOREIGN KEY (MaSP)
        REFERENCES SANPHAM(MaSP)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO


-- TAIKHOAN
CREATE TABLE TAIKHOAN (
    TenDangNhap   VARCHAR(50)   NOT NULL PRIMARY KEY,
    MatKhau       VARCHAR(50)   NOT NULL,
    Quyen         NVARCHAR(20)  NOT NULL
);
GO



/*==========================================================
                    INSERT DỮ LIỆU — FULL COLUMN LIST
==========================================================*/

-- SANPHAM
INSERT INTO SANPHAM (MaSP, TenSP, DonGia, SoLuong) VALUES
('SP01', N'Điện thoại Samsung S24', 21000000, 50),
('SP02', N'Điện thoại iPhone 15 Pro', 28900000, 40),
('SP03', N'Laptop Dell Inspiron 15', 18500000, 30),
('SP04', N'Tai nghe AirPods Pro 2', 5200000, 100),
('SP05', N'Chuột Logitech G102', 390000, 150);


-- KHACHHANG
INSERT INTO KHACHHANG (MaKH, HoTen, SDT, DiaChi) VALUES
('KH01', N'Nguyễn Văn Anh', '0912345678', N'Hà Nội'),
('KH02', N'Trần Thị Bích', '0988123456', N'Hồ Chí Minh'),
('KH03', N'Lê Hoàng Minh', '0909123123', N'Đà Nẵng'),
('KH04', N'Phạm Thu Hà', '0977665544', N'Hải Phòng'),
('KH05', N'Đỗ Quốc Khánh', '0933557799', N'Cần Thơ');


-- NHANVIEN
INSERT INTO NHANVIEN (MaNV, HoTen, ChucVu, SDT, DiaChi) VALUES
('NV01', N'Nguyễn Thành Long', N'Quản lý', '0909000001', N'Hà Nội'),
('NV02', N'Phạm Quốc Vinh', N'Bán hàng', '0909000002', N'HCM'),
('NV03', N'Lê Thị Hồng', N'Bán hàng', '0909000003', N'Đà Nẵng'),
('NV04', N'Hồ Trọng Nghĩa', N'Kế toán', '0909000004', N'Hà Nội'),
('NV05', N'Vũ Minh Tuấn', N'Thu ngân', '0909000005', N'HCM');


-- HOADON
INSERT INTO HOADON (MaHD, MaKH, MaNV, NgayLap, TongTien) VALUES
('HD01', 'KH01', 'NV01', '2024-01-10', 21000000),
('HD02', 'KH03', 'NV02', '2024-01-11', 28900000),
('HD03', 'KH02', 'NV03', '2024-01-12', 5200000),
('HD04', 'KH05', 'NV05', '2024-01-13', 18500000),
('HD05', 'KH04', 'NV04', '2024-01-14', 390000);


-- CHITIETHOADON
INSERT INTO CHITIETHOADON (MaCTHD, MaHD, MaSP, DonGia, SoLuong, ThanhTien) VALUES
('CT01', 'HD01', 'SP01', 21000000, 1, 21000000),
('CT02', 'HD02', 'SP02', 28900000, 1, 28900000),
('CT03', 'HD03', 'SP04', 5200000, 1, 5200000),
('CT04', 'HD04', 'SP03', 18500000, 1, 18500000),
('CT05', 'HD05', 'SP05', 390000, 1, 390000);


-- TAIKHOAN
INSERT INTO TAIKHOAN (TenDangNhap, MatKhau, Quyen) VALUES
('admin', '123456', N'Quản trị'),
('nv01', '111111', N'Nhân viên'),
('nv02', '222222', N'Nhân viên'),
('nv03', '333333', N'Nhân viên'),
('ketoan', '444444', N'Kế toán');
GO

select *from KHACHHANG 
select *from NHANVIEN
select *from SANPHAM