using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class NhaCungCapDTO
    {
        public string MaNCC { get; set; }
        public string TenCongTy { get; set; }
        public string Sdt { get; set; }
        public string DiaChi { get; set; }
        public string Email { get; set; }
        public string NguoiLienHe { get; set; }

        public NhaCungCapDTO()
        {
            MaNCC = "";
            TenCongTy = "";
            Sdt = "";
            DiaChi = "";
            Email = "";
            NguoiLienHe = "";
        }

        public NhaCungCapDTO(string maNCC, string tenCongTy, string sdt, string diaChi, string email, string nguoiLienHe)
        {
            this.MaNCC = maNCC;
            this.TenCongTy = tenCongTy;
            this.Sdt = sdt;
            this.DiaChi = diaChi;
            this.Email = email;
            this.NguoiLienHe = nguoiLienHe;
        }

        public NhaCungCapDTO(DataRow row)
        {
            this.MaNCC = row["ma_ncc"].ToString();
            this.TenCongTy = row["ten_cong_ty"].ToString();
            this.Sdt = row["sdt"] != DBNull.Value ? row["sdt"].ToString() : "";
            this.DiaChi = row["dia_chi"] != DBNull.Value ? row["dia_chi"].ToString() : "";
            this.Email = row["email"] != DBNull.Value ? row["email"].ToString() : "";
            this.NguoiLienHe = row["nguoi_lien_he"] != DBNull.Value ? row["nguoi_lien_he"].ToString() : "";
        }
    }
}