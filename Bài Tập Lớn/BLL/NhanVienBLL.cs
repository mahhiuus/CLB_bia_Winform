using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class NhanVienBLL
    {
        private readonly NhanVienDAL _dal = new NhanVienDAL();

        public string SinhMaMoi()
        {
            NhanVienDAL nvdal = new NhanVienDAL();
            return nvdal.SinhMaMoi();
        }

        public List<NhanVienDTO> LayTatCaNhanVien() => _dal.LayTatCaNhanVien();

        public List<NhanVienDTO> TimKiem(string keyword) => _dal.TimKiem(keyword);

        public NhanVienDTO TimTheoMa(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
                throw new ArgumentException("Mã nhân viên không được để trống!");
            return _dal.TimTheoMaNhanVien(maNV);
        }

        public void ThemNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
                throw new ArgumentException("Mã nhân viên và họ tên không được để trống!");
            _dal.ThemNhanVien(nv);
        }

        public void CapNhatNhanVien(NhanVienDTO nv)
        {
            if (nv == null || string.IsNullOrWhiteSpace(nv.MaNV) || string.IsNullOrWhiteSpace(nv.HoTen))
                throw new ArgumentException("Mã nhân viên và họ tên không được để trống!");
            _dal.CapNhatNhanVien(nv);
        }

        public void XoaNhanVien(string maNV)
        {
            if (string.IsNullOrWhiteSpace(maNV))
                throw new ArgumentException("Mã nhân viên không được để trống!");
            _dal.XoaNhanVien(maNV);
        }
    }
}