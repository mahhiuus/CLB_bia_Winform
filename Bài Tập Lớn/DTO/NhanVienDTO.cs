using System;

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
            NgaySinh = null;
        }

        public NhanVienDTO(string maNV, string hoTen, string sdt,
                           string gioiTinh, string chucVu, DateTime? ngaySinh)
        {
            MaNV = maNV;
            HoTen = hoTen;
            Sdt = sdt;
            GioiTinh = gioiTinh;
            ChucVu = chucVu;
            NgaySinh = ngaySinh;
        }
    }
}