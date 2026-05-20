using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    internal class NhaCungCapBLL
    {
        private NhaCungCapDAL nccDAL;

        public NhaCungCapBLL()
        {
            nccDAL = new NhaCungCapDAL();
        }

        // Sinh mã mới
        public string SinhMaMoi()
        {
            return nccDAL.SinhMaMoi();
        }

        // Lấy tất cả nhà cung cấp
        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            return nccDAL.LayTatCaNhaCungCap();
        }

        // Tìm theo mã
        public NhaCungCapDTO TimTheoMaNhaCungCap(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
            {
                throw new Exception("Mã nhà cung cấp không được để trống!");
            }

            return nccDAL.TimTheoMaNhaCungCap(maNCC);
        }

        // Tìm kiếm
        public List<NhaCungCapDTO> TimKiem(string keyword)
        {
            return nccDAL.TimKiem(keyword);
        }

        // Thêm nhà cung cấp
        public bool ThemNhaCungCap(NhaCungCapDTO ncc)
        {
            if (ncc == null)
            {
                throw new Exception("Dữ liệu nhà cung cấp không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
            {
                throw new Exception("Mã nhà cung cấp không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ncc.TenCongTy))
            {
                throw new Exception("Tên công ty không được để trống!");
            }

            return nccDAL.ThemNhaCungCap(ncc);
        }

        // Cập nhật nhà cung cấp
        public bool CapNhatNhaCungCap(NhaCungCapDTO ncc)
        {
            if (ncc == null)
            {
                throw new Exception("Dữ liệu nhà cung cấp không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ncc.MaNCC))
            {
                throw new Exception("Mã nhà cung cấp không được để trống!");
            }

            return nccDAL.CapNhatNhaCungCap(ncc);
        }

        // Xóa nhà cung cấp
        public bool XoaNhaCungCap(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
            {
                throw new Exception("Mã nhà cung cấp không được để trống!");
            }

            return nccDAL.XoaNhaCungCap(maNCC);
        }
    }
}