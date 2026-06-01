using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class SanPhamBLL
    {
        private readonly SanPhamDAL sanPhamDAL = new SanPhamDAL();

        public string SinhMaMoi() => sanPhamDAL.SinhMaMoi();
            public List<SanPhamDTO> LayTatCa()
            {
                return sanPhamDAL.LayTatCa();
            }
        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (sp == null) throw new Exception("Dữ liệu sản phẩm không hợp lệ!");
            if (string.IsNullOrWhiteSpace(sp.MaSP)) throw new Exception("Mã sản phẩm không được để trống!");
            if (string.IsNullOrWhiteSpace(sp.TenSP)) throw new Exception("Tên sản phẩm không được để trống!");
            if (string.IsNullOrWhiteSpace(sp.Loai)) throw new Exception("Loại sản phẩm không được để trống!");
            if (sp.GiaBan < 0) throw new Exception("Giá bán không được nhỏ hơn 0!");
            if (sp.SoLuongTon < 0) throw new Exception("Số lượng tồn kho không được nhỏ hơn 0!");
            return sanPhamDAL.ThemSanPham(sp);
        }

        public List<SanPhamDTO> TimTheoMaSanPham(string maSanPham)
        {
            if (string.IsNullOrWhiteSpace(maSanPham)) throw new Exception("Mã sản phẩm không được để trống!");
            return sanPhamDAL.TimKiemTheoMa(maSanPham);
        }

        public List<SanPhamDTO> TimTheoTenSanPham(string tenSanPham)
        {
            if (string.IsNullOrWhiteSpace(tenSanPham)) throw new Exception("Tên sản phẩm không được để trống!");
            return sanPhamDAL.TimKiemTheoTen(tenSanPham);
        }

        public List<SanPhamDTO> TimKiem(string keyword) => sanPhamDAL.TimKiem(keyword);

        public bool CapNhatSanPham(SanPhamDTO sp)
        {
            if (sp == null || string.IsNullOrWhiteSpace(sp.MaSP))
                throw new Exception("Dữ liệu sản phẩm không hợp lệ!");
            return sanPhamDAL.CapNhatSanPham(sp);
        }

        public bool XoaSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP)) throw new Exception("Mã sản phẩm không được để trống!");
            return sanPhamDAL.XoaSanPham(maSP);
        }
        public bool TangTonKho(string maSP, int soLuong)
        {
            if (string.IsNullOrWhiteSpace(maSP)) throw new Exception("Mã SP không được để trống!");
            if (soLuong <= 0) throw new Exception("Số lượng phải > 0!");
            return sanPhamDAL.TangTonKho(maSP, soLuong);
        }
        public bool GiamTonKho(string maSP, int soLuong)
        {
            if (string.IsNullOrWhiteSpace(maSP)) throw new Exception("Mã SP không được để trống!");
            if (soLuong <= 0) throw new Exception("Số lượng phải > 0!");
            return sanPhamDAL.GiamTonKho(maSP, soLuong);
        }
        public void GiamTonKhoNhieu(List<ChiTietHoaDonBanDTO> dsChiTiet)
        {
            if (dsChiTiet == null || dsChiTiet.Count == 0) return;
            foreach (var ct in dsChiTiet)
            {
                if (!string.IsNullOrWhiteSpace(ct.MaSP) && ct.SoLuong > 0)
                    sanPhamDAL.GiamTonKho(ct.MaSP, ct.SoLuong);
            }
        }
        public int LayTonKho(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP)) return 0;
            return sanPhamDAL.LayTonKho(maSP);
        }
    }
}