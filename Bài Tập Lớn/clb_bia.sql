IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CLB_bia')
BEGIN
    CREATE DATABASE CLB_bia;
END
GO

USE CLB_bia;
GO

-- =========================
-- BẢNG NHÂN VIÊN
-- =========================
CREATE TABLE nhan_vien (
    ma_nv VARCHAR(10) PRIMARY KEY,
    ho_ten NVARCHAR(100) NOT NULL,
    sdt VARCHAR(15),
    gioi_tinh NVARCHAR(10) NOT NULL CHECK (gioi_tinh IN (N'Nam', N'Nữ', N'Khác')),
    chuc_vu NVARCHAR(50),
    ngay_sinh DATE
);

-- =========================
-- BẢNG TÀI KHOẢN
-- =========================
CREATE TABLE tai_khoan (
    ma_tk VARCHAR(10) PRIMARY KEY,
    ten_dang_nhap VARCHAR(50) NOT NULL UNIQUE,
    mat_khau VARCHAR(255) NOT NULL,
    vai_tro NVARCHAR(20) NOT NULL CHECK (vai_tro IN (N'Admin', N'Nhân viên')),
    ma_nv VARCHAR(10) UNIQUE,
    FOREIGN KEY (ma_nv) REFERENCES nhan_vien(ma_nv)
);

-- =========================
-- BẢNG KHÁCH HÀNG
-- =========================
CREATE TABLE khach_hang (
    ma_kh VARCHAR(10) PRIMARY KEY,
    ho_ten NVARCHAR(100) NOT NULL,
    sdt VARCHAR(15),
    dia_chi NVARCHAR(255),
    diem_tich_luy INT DEFAULT 0 CHECK (diem_tich_luy >= 0),
    ngay_dang_ky DATE DEFAULT GETDATE()
);

-- =========================
-- BẢNG NHÀ CUNG CẤP
-- =========================
CREATE TABLE nha_cung_cap (
    ma_ncc VARCHAR(10) PRIMARY KEY,
    ten_cong_ty NVARCHAR(100) NOT NULL,
    sdt VARCHAR(15),
    dia_chi NVARCHAR(255),
    email VARCHAR(100),
    nguoi_lien_he NVARCHAR(100)
);

-- =========================
-- BẢNG SẢN PHẨM
-- =========================
CREATE TABLE san_pham (
    ma_sp VARCHAR(10) PRIMARY KEY,
    ten_sp NVARCHAR(100) NOT NULL,
    loai NVARCHAR(20) NOT NULL CHECK (loai IN (N'Đồ ăn', N'Đồ uống', N'Dụng cụ')),
    gia_ban DECIMAL(12,2) NOT NULL CHECK (gia_ban >= 0),
    so_luong_ton INT DEFAULT 0 CHECK (so_luong_ton >= 0),
	hinh_anh NVARCHAR(255),
    ma_ncc VARCHAR(10),
    FOREIGN KEY (ma_ncc) REFERENCES nha_cung_cap(ma_ncc)
);

-- =========================
-- BẢNG BÀN BIDA
-- =========================
CREATE TABLE ban_bida (
    ma_ban VARCHAR(10) PRIMARY KEY,
    ten_ban NVARCHAR(50) NOT NULL,
    loai_ban NVARCHAR(10) NOT NULL CHECK (loai_ban IN (N'Thường', N'VIP')),
    gia_theo_gio DECIMAL(12,2) NOT NULL CHECK (gia_theo_gio >= 0),
    trang_thai NVARCHAR(20) NOT NULL CHECK (trang_thai IN (N'Trống', N'Đang chơi', N'Bảo trì'))
);

-- =========================
-- BẢNG PHIÊN CHƠI
-- =========================
CREATE TABLE phien_choi (
    ma_phien VARCHAR(10) PRIMARY KEY,
    ma_ban VARCHAR(10) NOT NULL,
    ma_nv VARCHAR(10) NOT NULL,
    thoi_gian_bat_dau DATETIME NOT NULL,
    thoi_gian_ket_thuc DATETIME,
    trang_thai NVARCHAR(20) NOT NULL CHECK (trang_thai IN (N'Đang chơi', N'Đã kết thúc', N'Đã hủy')),
    FOREIGN KEY (ma_ban) REFERENCES ban_bida(ma_ban),
    FOREIGN KEY (ma_nv) REFERENCES nhan_vien(ma_nv),
    CHECK (thoi_gian_ket_thuc IS NULL OR thoi_gian_ket_thuc >= thoi_gian_bat_dau)
);

-- =========================
-- BẢNG CHI TIẾT PHIÊN
-- =========================
CREATE TABLE chi_tiet_phien (
    ma_ctp VARCHAR(10) PRIMARY KEY,
    ma_phien VARCHAR(10) NOT NULL,
    ma_sp VARCHAR(10) NOT NULL,
    so_luong INT NOT NULL CHECK (so_luong > 0),
    don_gia DECIMAL(12,2) NOT NULL CHECK (don_gia >= 0),
    FOREIGN KEY (ma_phien) REFERENCES phien_choi(ma_phien),
    FOREIGN KEY (ma_sp) REFERENCES san_pham(ma_sp)
);

-- =========================
-- BẢNG HÓA ĐƠN NHẬP
-- =========================
CREATE TABLE hoa_don_nhap (
    ma_hdn VARCHAR(10) PRIMARY KEY,
    ma_ncc VARCHAR(10),
    ma_nv VARCHAR(10),
    ngay_nhap DATE NOT NULL DEFAULT GETDATE(),
    tong_tien DECIMAL(12,2) DEFAULT 0 CHECK (tong_tien >= 0),
    ghi_chu NVARCHAR(255),
    FOREIGN KEY (ma_ncc) REFERENCES nha_cung_cap(ma_ncc),
    FOREIGN KEY (ma_nv) REFERENCES nhan_vien(ma_nv)
);

-- =========================
-- BẢNG CHI TIẾT HÓA ĐƠN NHẬP
-- =========================
CREATE TABLE chi_tiet_hoa_don_nhap (
    ma_ct_hdn VARCHAR(10) PRIMARY KEY,
    ma_hdn VARCHAR(10) NOT NULL,
    ma_sp VARCHAR(10) NOT NULL,
    so_luong INT NOT NULL CHECK (so_luong > 0),
    don_gia_nhap DECIMAL(12,2) NOT NULL CHECK (don_gia_nhap >= 0),
    FOREIGN KEY (ma_hdn) REFERENCES hoa_don_nhap(ma_hdn),
    FOREIGN KEY (ma_sp) REFERENCES san_pham(ma_sp)
);

-- =========================
-- BẢNG HÓA ĐƠN BÁN
-- =========================
CREATE TABLE hoa_don_ban (
    ma_hdb VARCHAR(10) PRIMARY KEY,
    ma_phien VARCHAR(10) UNIQUE,
    ma_kh VARCHAR(10),
    ma_nv VARCHAR(10),
    ngay_ban DATE NOT NULL DEFAULT GETDATE(),
    tien_bida DECIMAL(12,2) DEFAULT 0 CHECK (tien_bida >= 0),
    tien_san_pham DECIMAL(12,2) DEFAULT 0 CHECK (tien_san_pham >= 0),
    tong_tien DECIMAL(12,2) DEFAULT 0 CHECK (tong_tien >= 0),
    ghi_chu NVARCHAR(255),
    FOREIGN KEY (ma_phien) REFERENCES phien_choi(ma_phien),
    FOREIGN KEY (ma_kh) REFERENCES khach_hang(ma_kh),
    FOREIGN KEY (ma_nv) REFERENCES nhan_vien(ma_nv)
);

-- =========================
-- BẢNG CHI TIẾT HÓA ĐƠN BÁN
-- =========================
CREATE TABLE chi_tiet_hoa_don_ban (
    ma_ct_hdb VARCHAR(10) PRIMARY KEY,
    ma_hdb VARCHAR(10) NOT NULL,
    ma_sp VARCHAR(10) NOT NULL,
    so_luong INT NOT NULL CHECK (so_luong > 0),
    don_gia_ban DECIMAL(12,2) NOT NULL CHECK (don_gia_ban >= 0),
    FOREIGN KEY (ma_hdb) REFERENCES hoa_don_ban(ma_hdb),
    FOREIGN KEY (ma_sp) REFERENCES san_pham(ma_sp)
);

-- ==============
-- Insert dữ liệu
-- ==============
INSERT INTO nhan_vien VALUES
('NV001', N'Trần Mạnh Hiếu', '0987654321', N'Nam', N'Quản lý', '2004-05-12'),
('NV002', N'Nguyễn Văn An', '0912345678', N'Nam', N'Nhân viên', '2003-08-21'),
('NV003', N'Lê Thị Mai', '0909123456', N'Nữ', N'Thu ngân', '2002-11-15');

-- =========================
-- DỮ LIỆU TÀI KHOẢN
-- =========================
INSERT INTO tai_khoan VALUES
('TK001', 'admin', '123456', N'Admin', 'NV001'),
('TK002', 'an', '123456', N'Nhân viên', 'NV002'),
('TK003', 'mai', '123456', N'Nhân viên', 'NV003');

-- =========================
-- DỮ LIỆU KHÁCH HÀNG
-- =========================
INSERT INTO khach_hang VALUES
('KH001', N'Phạm Minh Đức', '0977123456', N'Hà Nội', 120, '2025-01-10'),
('KH002', N'Ngô Quang Huy', '0966234567', N'Hải Phòng', 80, '2025-02-15'),
('KH003', N'Đỗ Thu Trang', '0933456789', N'Nam Định', 200, '2025-03-01');

-- =========================
-- DỮ LIỆU NHÀ CUNG CẤP
-- =========================
INSERT INTO nha_cung_cap VALUES
('NCC001', N'Công ty Pepsi', '0243123456', N'Hà Nội', 'pepsi@gmail.com', N'Nguyễn Hải'),
('NCC002', N'Công ty Vinamilk', '0243987654', N'Hồ Chí Minh', 'vinamilk@gmail.com', N'Trần Long'),
('NCC003', N'Công ty Đồ ăn nhanh', '0243555666', N'Đà Nẵng', 'fastfood@gmail.com', N'Lê Minh');

-- =========================
-- DỮ LIỆU SẢN PHẨM
-- =========================
INSERT INTO san_pham VALUES
('SP001', N'Pepsi', N'Đồ uống', 15000, 100, NULL, 'NCC001'),
('SP002', N'Coca Cola', N'Đồ uống', 15000, 120, NULL, 'NCC001'),
('SP003', N'Sữa Vinamilk', N'Đồ uống', 20000, 80, NULL, 'NCC002'),
('SP004', N'Mì ly', N'Đồ ăn', 25000, 50, NULL, 'NCC003'),
('SP005', N'Xúc xích', N'Đồ ăn', 30000, 40, NULL, 'NCC003'),
('SP006', N'Găng tay bida', N'Dụng cụ', 50000, 20, NULL, 'NCC003');

-- =========================
-- DỮ LIỆU BÀN BIDA
-- =========================
INSERT INTO ban_bida VALUES
('BAN001', N'Bàn số 1', N'Thường', 70000, N'Trống'),
('BAN002', N'Bàn số 2', N'Thường', 70000, N'Đang chơi'),
('BAN003', N'Bàn VIP 1', N'VIP', 120000, N'Trống'),
('BAN004', N'Bàn VIP 2', N'VIP', 120000, N'Bảo trì');

-- =========================
-- DỮ LIỆU PHIÊN CHƠI
-- =========================
INSERT INTO phien_choi VALUES
('PC001', 'BAN002', 'NV002', '2026-05-16 18:00:00', NULL, N'Đang chơi'),
('PC002', 'BAN001', 'NV003', '2026-05-15 14:00:00', '2026-05-15 16:00:00', N'Đã kết thúc');

-- =========================
-- DỮ LIỆU CHI TIẾT PHIÊN
-- =========================
INSERT INTO chi_tiet_phien VALUES
('CTP001', 'PC001', 'SP001', 2, 15000),
('CTP002', 'PC001', 'SP004', 1, 25000),
('CTP003', 'PC002', 'SP002', 3, 15000);

-- =========================
-- DỮ LIỆU HÓA ĐƠN NHẬP
-- =========================
INSERT INTO hoa_don_nhap VALUES
('HDN001', 'NCC001', 'NV001', '2026-05-01', 3000000, N'Nhập nước ngọt'),
('HDN002', 'NCC003', 'NV002', '2026-05-05', 2000000, N'Nhập đồ ăn');

-- =========================
-- DỮ LIỆU CHI TIẾT HÓA ĐƠN NHẬP
-- =========================
INSERT INTO chi_tiet_hoa_don_nhap VALUES
('CTHDN001', 'HDN001', 'SP001', 100, 10000),
('CTHDN002', 'HDN001', 'SP002', 100, 10000),
('CTHDN003', 'HDN002', 'SP004', 50, 18000),
('CTHDN004', 'HDN002', 'SP005', 40, 22000);

-- =========================
-- DỮ LIỆU HÓA ĐƠN BÁN
-- =========================
INSERT INTO hoa_don_ban VALUES
('HDB001', 'PC002', 'KH001', 'NV003', '2026-05-15', 140000, 45000, 185000, N'Khách thanh toán đủ');

-- =========================
-- DỮ LIỆU CHI TIẾT HÓA ĐƠN BÁN
-- =========================
INSERT INTO chi_tiet_hoa_don_ban VALUES
('CTHDB001', 'HDB001', 'SP002', 3, 15000);

-- =========================
-- SELECT TOÀN BỘ DỮ LIỆU
-- =========================

SELECT * FROM nhan_vien;

SELECT * FROM tai_khoan;

SELECT * FROM khach_hang;

SELECT * FROM nha_cung_cap;

SELECT * FROM san_pham;

SELECT * FROM ban_bida;

SELECT * FROM phien_choi;

SELECT * FROM chi_tiet_phien;

SELECT * FROM hoa_don_nhap;

SELECT * FROM chi_tiet_hoa_don_nhap;

SELECT * FROM hoa_don_ban;

SELECT * FROM chi_tiet_hoa_don_ban;