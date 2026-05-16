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

        public PhienChoiDTO(DataRow row)
        {
            this.MaPhien = row["ma_phien"].ToString();
            this.MaBan = row["ma_ban"].ToString();
            this.MaNV = row["ma_nv"].ToString();
            this.ThoiGianBatDau = Convert.ToDateTime(row["thoi_gian_bat_dau"]);
            this.ThoiGianKetThuc = row["thoi_gian_ket_thuc"] != DBNull.Value ? Convert.ToDateTime(row["thoi_gian_ket_thuc"]) : (DateTime?)null;
            this.TrangThai = row["trang_thai"].ToString();
        }
    }
}