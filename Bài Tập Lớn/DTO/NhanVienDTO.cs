using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class NhanVienDTO
    {
        public string MaNV { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public string GioiTinh { get; set; }
        public string ChucVu { get; set; }
        public DateTime? NgaySinh { get; set; }

        public NhanVienDTO()
        {
            MaNV = "";
            HoTen = "";
            Sdt = "";
            GioiTinh = "";
            ChucVu = "";
            NgaySinh = DateTime.Now;
        }
        public NhanVienDTO(string maNV, string hoTen, string sdt, string gioiTinh, string chucVu, DateTime? ngaySinh)
        {
            this.MaNV = maNV;
            this.HoTen = hoTen;
            this.Sdt = sdt;
            this.GioiTinh = gioiTinh;
            this.ChucVu = chucVu;
            this.NgaySinh = ngaySinh;
        }
    }
}