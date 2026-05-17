using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class TaiKhoanDTO
    {
        public string MaTK { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string VaiTro { get; set; }
        public string MaNV { get; set; }
        public TaiKhoanDTO()
        {
            MaTK = "";
            TenDangNhap = "";
            MatKhau = "";
            VaiTro = "";
            MaNV = "";
        }
        public TaiKhoanDTO(string maTK, string tenDangNhap, string matKhau, string vaiTro, string maNV)
        {
            this.MaTK = maTK;
            this.TenDangNhap = tenDangNhap;
            this.MatKhau = matKhau;
            this.VaiTro = vaiTro;
            this.MaNV = maNV;
        }
    }
}