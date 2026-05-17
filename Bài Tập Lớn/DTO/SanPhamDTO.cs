using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class SanPhamDTO
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string Loai { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public string HinhAnh { get; set; }
        public string MaNCC { get; set; }

        public SanPhamDTO()
        {
            MaSP = "";
            TenSP = "";
            Loai = "";
            GiaBan = 0;
            SoLuongTon = 0;
            HinhAnh = "";
            MaNCC = "";
        }
        public SanPhamDTO(string maSP, string tenSP, string loai, decimal giaBan, int soLuongTon, string hinhAnh, string maNCC)
        {
            this.MaSP = maSP;
            this.TenSP = tenSP;
            this.Loai = loai;
            this.GiaBan = giaBan;
            this.SoLuongTon = soLuongTon;
            this.HinhAnh = hinhAnh;
            this.MaNCC = maNCC;
        }
    }
}