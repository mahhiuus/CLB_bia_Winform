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
        public decimal TongTien { get; set; }
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

        public HoaDonNhapDTO(string maHDN, string maNCC, string maNV, DateTime ngayNhap, decimal tongTien, string ghiChu)
        {
            this.MaHDN = maHDN;
            this.MaNCC = maNCC;
            this.MaNV = maNV;
            this.NgayNhap = ngayNhap;
            this.TongTien = tongTien;
            this.GhiChu = ghiChu;
        }

        public HoaDonNhapDTO(DataRow row)
        {
            this.MaHDN = row["ma_hdn"].ToString();
            this.MaNCC = row["ma_ncc"] != DBNull.Value ? row["ma_ncc"].ToString() : "";
            this.MaNV = row["ma_nv"] != DBNull.Value ? row["ma_nv"].ToString() : "";
            this.NgayNhap = Convert.ToDateTime(row["ngay_nhap"]);
            this.TongTien = Convert.ToDecimal(row["tong_tien"]);
            this.GhiChu = row["ghi_chu"] != DBNull.Value ? row["ghi_chu"].ToString() : "";
        }
    }
}