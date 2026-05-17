using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL nhanVienDAL = new NhanVienDAL();

        public string SinhMaMoi()
        {
            return nhanVienDAL.SinhMaMoi();
        }

        public void ThemNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
            {
                throw new ArgumentException("Nhân viên hoặc các trường bắt buộc không được để trống!");
            }
            nhanVienDAL.ThemNhanVien(nv);
        }

        public void XoaNhanVien(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống!");
            }
            nhanVienDAL.XoaNhanVien(maNV);
        }

        public void CapNhatNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
            {
                throw new ArgumentException("Nhân viên hoặc các trường bắt buộc không được để trống!");
            }
            nhanVienDAL.CapNhatNhanVien(nv);
        }

        public List<NhanVienDTO> LayTatCaNhanVien()
        {
            return nhanVienDAL.LayTatCaNhanVien();
        }

        public NhanVienDTO TimTheoMaNhanVien(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống!");
            }
            return nhanVienDAL.TimTheoMaNhanVien(maNV);
        }

        public List<NhanVienDTO> TimKiem(string keyword)
        {
            return nhanVienDAL.TimKiem(keyword);
        }
    }
}