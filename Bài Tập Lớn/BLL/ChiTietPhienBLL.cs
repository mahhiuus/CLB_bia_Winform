using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class ChiTietPhienBLL
    {
        private readonly ChiTietPhienDAL chiTietPhienDAL = new ChiTietPhienDAL();

        public string SinhMaMoi()
        {
            return chiTietPhienDAL.SinhMaMoi();
        }

        public List<ChiTietPhienDTO> LayTatCaChiTietPhien()
        {
            return chiTietPhienDAL.LayTatCaChiTietPhien();
        }

        public List<ChiTietPhienDTO> TimTheoMaPhien(string maPhien)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            return chiTietPhienDAL.TimTheoMaPhien(maPhien);
        }

        public double TinhTongTienTheoPhien(string maPhien)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            return chiTietPhienDAL.TinhTongTienTheoPhien(maPhien);
        }

        public bool ThemChiTietPhien(ChiTietPhienDTO ct)
        {
            if (ct == null)
            {
                throw new Exception("Dữ liệu chi tiết phiên không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaCTP))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaPhien))
            {
                throw new Exception("Mã phiên không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaSP))
            {
                throw new Exception("Mã sản phẩm không được để trống!");
            }

            if (ct.SoLuong <= 0)
            {
                throw new Exception("Số lượng phải lớn hơn 0!");
            }

            if (ct.DonGia < 0)
            {
                throw new Exception("Đơn giá không hợp lệ!");
            }

            return chiTietPhienDAL.ThemChiTietPhien(ct);
        }

        public bool CapNhatChiTietPhien(ChiTietPhienDTO ct)
        {
            if (ct == null)
            {
                throw new Exception("Dữ liệu chi tiết phiên không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaCTP))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            return chiTietPhienDAL.CapNhatChiTietPhien(ct);
        }

        public bool XoaChiTietPhien(string maChiTiet)
        {
            if (string.IsNullOrWhiteSpace(maChiTiet))
            {
                throw new Exception("Mã chi tiết không được để trống!");
            }

            return chiTietPhienDAL.XoaChiTietPhien(maChiTiet);
        }
    }
}