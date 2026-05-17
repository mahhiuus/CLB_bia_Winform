using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class ChiTietHoaDonBanDTO
    {
        public string MaCTHDB { get; set; }
        public string MaHDB { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGiaBan { get; set; }

        public ChiTietHoaDonBanDTO()
        {
            MaCTHDB = "";
            MaHDB = "";
            MaSP = "";
            SoLuong = 0;
            DonGiaBan = 0;
        }

        public ChiTietHoaDonBanDTO(string MaCTHDB, string maHDB, string maSP, int soLuong, decimal donGiaBan)
        {
            this.MaCTHDB = MaCTHDB;
            this.MaHDB = maHDB;
            this.MaSP = maSP;
            this.SoLuong = soLuong;
            this.DonGiaBan = donGiaBan;
        }
    }
}