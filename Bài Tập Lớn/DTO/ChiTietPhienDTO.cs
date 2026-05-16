using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class ChiTietPhienDTO
    {
        public string MaCTP { get; set; }
        public string MaPhien { get; set; }
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public ChiTietPhienDTO()
        {
            MaCTP = "";
            MaPhien = "";
            MaSP = "";
            SoLuong = 0;
            DonGia = 0;
        }

        public ChiTietPhienDTO(string maCTP, string maPhien, string maSP, int soLuong, decimal donGia)
        {
            this.MaCTP = maCTP;
            this.MaPhien = maPhien;
            this.MaSP = maSP;
            this.SoLuong = soLuong;
            this.DonGia = donGia;
        }

        public ChiTietPhienDTO(DataRow row)
        {
            this.MaCTP = row["ma_ctp"].ToString();
            this.MaPhien = row["ma_phien"].ToString();
            this.MaSP = row["ma_sp"].ToString();
            this.SoLuong = Convert.ToInt32(row["so_luong"]);
            this.DonGia = Convert.ToDecimal(row["don_gia"]);
        }
    }
}