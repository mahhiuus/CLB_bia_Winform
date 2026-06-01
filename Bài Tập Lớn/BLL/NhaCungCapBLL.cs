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

        public string SinhMaMoi()
        {
            return nccDAL.SinhMaMoi();
        }

        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            return nccDAL.LayTatCaNhaCungCap();
        }

        public NhaCungCapDTO TimTheoMaNhaCungCap(string maNCC)
        {
            if (string.IsNullOrWhiteSpace(maNCC))
            {
                throw new Exception("Mã nhà cung cấp không được để trống!");
            }

            return nccDAL.TimTheoMaNhaCungCap(maNCC);
        }
        public List<NhaCungCapDTO> TimKiem(string keyword)
        {
            return nccDAL.TimKiem(keyword);
        }
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