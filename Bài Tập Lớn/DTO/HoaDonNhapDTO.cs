using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class HoaDonNhapDTO
    {
        public string MaHDN { get; set; }
        public string MaNCC { get; set; }
        public string MaNV { get; set; }
        public DateTime NgayNhap { get; set; }
        public double TongTien { get; set; }
        public string GhiChu { get; set; }

        public HoaDonNhapDTO()
        {
            MaHDN = "";
            MaNCC = "";
            MaNV = "";
            NgayNhap = DateTime.Now;
            TongTien = 0;
            GhiChu = "";
        }

        public HoaDonNhapDTO(string maHDN, string maNCC, string maNV, DateTime ngayNhap, double tongTien, string ghiChu)
        {
            this.MaHDN = maHDN;
            this.MaNCC = maNCC;
            this.MaNV = maNV;
            this.NgayNhap = ngayNhap;
            this.TongTien = tongTien;
            this.GhiChu = ghiChu;
        }
    }
}