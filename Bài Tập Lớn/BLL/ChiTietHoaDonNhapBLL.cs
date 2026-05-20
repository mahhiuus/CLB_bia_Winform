using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class ChiTietHoaDonNhapBLL
    {
        private readonly ChiTietHoaDonNhapDAL chiTietDAL = new ChiTietHoaDonNhapDAL();

        public string SinhMaMoi()
        {
            return chiTietDAL.SinhMaMoi();
        }

        public List<ChiTietHoaDonNhapDTO> LayTatCaChiTiet()
        {
            return chiTietDAL.LayTatCaChiTiet();
        }

        public List<ChiTietHoaDonNhapDTO> TimTheoMaHDN(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
            {
                throw new Exception("Mã hóa đơn nhập không được để trống!");
            }

            return chiTietDAL.TimTheoMaHDN(maHDN);
        }

        public decimal TinhTongTien(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
            {
                throw new Exception("Mã hóa đơn nhập không được để trống!");
            }

            return chiTietDAL.TinhTongTien(maHDN);
        }

        public bool ThemChiTiet(ChiTietHoaDonNhapDTO ct)
        {
            if (ct == null)
            {
                throw new Exception("Dữ liệu chi tiết không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaCTHDN))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            if (ct.SoLuong <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0!");
            }

            return chiTietDAL.ThemChiTiet(ct);
        }

        public bool CapNhatChiTiet(ChiTietHoaDonNhapDTO ct)
        {
            if (ct == null)
            {
                throw new Exception("Dữ liệu chi tiết không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaCTHDN))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            return chiTietDAL.CapNhatChiTiet(ct);
        }

        public bool XoaChiTiet(string maCTHDN)
        {
            if (string.IsNullOrWhiteSpace(maCTHDN))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            return chiTietDAL.XoaChiTiet(maCTHDN);
        }
    }
}