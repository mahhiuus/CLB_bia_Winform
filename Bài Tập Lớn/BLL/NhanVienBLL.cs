using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL _nhanVienDAL = new NhanVienDAL();

        public string SinhMaMoi()
        {
            return _nhanVienDAL.SinhMaMoi();
        }

        public void ThemNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
            {
                throw new ArgumentException("Nhân viên hoặc các trường bắt buộc không được để trống!");
            }
            _nhanVienDAL.ThemNhanVien(nv);
        }

        public void XoaNhanVien(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống!");
            }
            _nhanVienDAL.XoaNhanVien(maNV);
        }

        public void CapNhatNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
            {
                throw new ArgumentException("Nhân viên hoặc các trường bắt buộc không được để trống!");
            }
            _nhanVienDAL.CapNhatNhanVien(nv);
        }

        public List<NhanVienDTO> LayTatCaNhanVien()
        {
            return _nhanVienDAL.LayTatCaNhanVien();
        }

        public NhanVienDTO TimTheoMaNhanVien(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
            {
                throw new ArgumentException("Mã nhân viên không được để trống!");
            }
            return _nhanVienDAL.TimTheoMaNhanVien(maNV);
        }

        public List<NhanVienDTO> TimKiem(string keyword)
        {
            return _nhanVienDAL.TimKiem(keyword);
        }
    }
}