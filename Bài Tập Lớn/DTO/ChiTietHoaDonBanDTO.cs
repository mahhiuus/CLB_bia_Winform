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

        public ChiTietHoaDonBanDTO(DataRow row)
        {
            this.MaCTHDB = row["ma_ct_hdb"].ToString();
            this.MaHDB = row["ma_hdb"].ToString();
            this.MaSP = row["ma_sp"].ToString();
            this.SoLuong = Convert.ToInt32(row["so_luong"]);
            this.DonGiaBan = Convert.ToDecimal(row["don_gia_ban"]);
        }
    }
}