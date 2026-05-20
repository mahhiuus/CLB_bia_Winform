using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class SanPhamBLL
    {
        private readonly SanPhamDAL sanPhamDAL = new SanPhamDAL();

        public string SinhMaMoi()
        {
            return sanPhamDAL.SinhMaMoi();
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (sp == null)
            {
                throw new Exception("Dữ liệu sản phẩm không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(sp.MaSP))
            {
                throw new Exception("Mã sản phẩm không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sp.TenSP))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(sp.Loai))
            {
                throw new Exception("Loại sản phẩm không được để trống!");
            }

            if (sp.GiaBan < 0)
            {
                throw new Exception("Giá bán không được nhỏ hơn 0!");
            }

            if (sp.SoLuongTon < 0)
            {
                throw new Exception("Số lượng tồn kho không được nhỏ hơn 0!");
            }

            return sanPhamDAL.ThemSanPham(sp);
        }

        public List<SanPhamDTO> TimTheoMaSanPham(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham))
            {
                throw new Exception("Mã sản phẩm không được để trống!");
            }
            return sanPhamDAL.TimKiemTheoMa(maSanPham);
        }

        public List<SanPhamDTO> TimTheoTenSanPham(string tenSanPham)
        {
            if (string.IsNullOrWhiteSpace(tenSanPham))
            {
                throw new Exception("Tên sản phẩm không được để trống!");
            }
            return sanPhamDAL.TimKiemTheoTen(tenSanPham);
        }
    }
}