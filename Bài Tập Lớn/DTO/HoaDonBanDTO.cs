using System;

namespace Bài_Tập_Lớn.DTO
{
    public class HoaDonBanDTO
    {
        public string MaHDB { get; set; }
        public string MaPhien { get; set; }
        public string MaKH { get; set; }   // nullable – khách vãng lai không có mã
        public string MaNV { get; set; }
        public DateTime NgayBan { get; set; }
        public double TienBida { get; set; }
        public double TienSanPham { get; set; }
        public double TongTien { get; set; }
        public string GhiChu { get; set; }   // nullable

        // Constructor mặc định – MaKH và GhiChu để null
        // tránh FK violation với bảng khach_hang khi không có khách hàng
        public HoaDonBanDTO()
        {
            MaHDB = "";
            MaPhien = "";
            MaKH = null;   // ← null thay vì "" → SQL Server sẽ lưu NULL
            MaNV = "";
            NgayBan = DateTime.Now;
            TienBida = 0;
            TienSanPham = 0;
            TongTien = 0;
            GhiChu = null;   // ← null thay vì ""
        }

        public HoaDonBanDTO(string maHDB, string maPhien, string maKH, string maNV,
                            DateTime ngayBan, double tienBida, double tienSanPham,
                            double tongTien, string ghiChu)
        {
            MaHDB = maHDB;
            MaPhien = maPhien;
            MaKH = string.IsNullOrWhiteSpace(maKH) ? null : maKH;   // chuẩn hoá ""→null
            MaNV = maNV;
            NgayBan = ngayBan;
            TienBida = tienBida;
            TienSanPham = tienSanPham;
            TongTien = tongTien;
            GhiChu = string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu;
        }
    }
}