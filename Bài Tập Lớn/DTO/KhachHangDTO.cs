using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class KhachHangDTO
    {
        public string MaKH { get; set; }
        public string HoTen { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public int DiemTichLuy { get; set; }
        public DateTime NgayDangKy { get; set; }

        public KhachHangDTO()
        {
            MaKH = "";
            HoTen = "";
            Sdt = "";
            DiaChi = "";
            DiemTichLuy = 0;
            NgayDangKy = DateTime.Now;
        }
        public KhachHangDTO(string maKH, string hoTen, string sdt, string diaChi, int diemTichLuy, DateTime ngayDangKy)
        {
            this.MaKH = maKH;
            this.HoTen = hoTen;
            this.Sdt = sdt;
            this.DiaChi = diaChi;
            this.DiemTichLuy = diemTichLuy;
            this.NgayDangKy = ngayDangKy;
        }
    }
}