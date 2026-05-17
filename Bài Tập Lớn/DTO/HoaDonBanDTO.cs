using System;
using System.Data;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.DTO
{
    public class HoaDonBanDTO
    {
        public string MaHDB { get; set; }
        public string MaPhien { get; set; }
        public string MaKH { get; set; }
        public string MaNV { get; set; }
        public DateTime NgayBan { get; set; }
        public decimal TienBida { get; set; }
        public decimal TienSanPham { get; set; }
        public decimal TongTien { get; set; }
        public string GhiChu { get; set; }

        public HoaDonBanDTO()
        {
            MaHDB = "";
            MaPhien = "";
            MaKH = "";
            MaNV = "";
            NgayBan = DateTime.Now;
            TienBida = 0;
            TienSanPham = 0;
            TongTien = 0;
            GhiChu = "";
        }

        public HoaDonBanDTO(string maHDB, string maPhien, string maKH, string maNV, DateTime ngayBan, decimal tienBida, decimal tienSanPham, decimal tongTien, string ghiChu)
        {
            this.MaHDB = maHDB;
            this.MaPhien = maPhien;
            this.MaKH = maKH;
            this.MaNV = maNV;
            this.NgayBan = ngayBan;
            this.TienBida = tienBida;
            this.TienSanPham = tienSanPham;
            this.TongTien = tongTien;
            this.GhiChu = ghiChu;
        }
    }
}