using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class PhienChoiDTO
    {
        public string MaPhien { get; set; }
        public string MaBan { get; set; }
        public string MaNV { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime? ThoiGianKetThuc { get; set; }
        public string TrangThai { get; set; }

        public PhienChoiDTO()
        {
            MaPhien = "";
            MaBan = "";
            MaNV = "";
            ThoiGianBatDau = DateTime.Now;
            ThoiGianKetThuc = null;
            TrangThai = "";
        }

        public PhienChoiDTO(string maPhien, string maBan, string maNV, DateTime thoiGianBatDau, DateTime? thoiGianKetThuc, string trangThai)
        {
            this.MaPhien = maPhien;
            this.MaBan = maBan;
            this.MaNV = maNV;
            this.ThoiGianBatDau = thoiGianBatDau;
            this.ThoiGianKetThuc = thoiGianKetThuc;
            this.TrangThai = trangThai;
        }
    }
}