using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class ChiTietHoaDonNhapDTO
    {
        public string MaCTHDN { get; set; }
        public string MaHDN { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaNhap { get; set; }

        public ChiTietHoaDonNhapDTO()
        {
            MaCTHDN = "";
            MaHDN = "";
            MaSP = "";
            SoLuong = 0;
            DonGiaNhap = 0;
        }

        public ChiTietHoaDonNhapDTO(string MaCTHDN, string maHDN, string maSP, int soLuong, decimal donGiaNhap)
        {
            this.MaCTHDN = MaCTHDN;
            this.MaHDN = maHDN;
            this.MaSP = maSP;
            this.SoLuong = soLuong;
            this.DonGiaNhap = donGiaNhap;
        }
    }
}