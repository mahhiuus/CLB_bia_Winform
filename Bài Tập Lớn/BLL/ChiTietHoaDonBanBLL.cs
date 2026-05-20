using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class ChiTietHoaDonBanBLL
    {
        private readonly ChiTietHoaDonBanDAL chiTietDAL = new ChiTietHoaDonBanDAL();

        public string SinhMaMoi()
        {
            return chiTietDAL.SinhMaMoi();
        }

        public void ThemChiTiet(ChiTietHoaDonBanDTO ct)
        {
            if (ct == null)
            {
                throw new ArgumentException("Dữ liệu chi tiết hóa đơn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ct.MaHDB) || string.IsNullOrWhiteSpace(ct.MaSP))
            {
                throw new ArgumentException("Mã hóa đơn và mã sản phẩm không được để trống!");
            }

            if (ct.SoLuong <= 0)
            {
                throw new ArgumentException("Số lượng sản phẩm bán phải lớn hơn 0!");
            }

            if (ct.DonGiaBan < 0)
            {
                throw new ArgumentException("Đơn giá bán không được âm!");
            }

            // Tự động sinh mã nếu trống (Ủy quyền hoàn toàn xử lý tại đây)
            if (string.IsNullOrWhiteSpace(ct.MaChiTiet))
            {
                ct.MaChiTiet = chiTietDAL.SinhMaMoi();
            }

            chiTietDAL.ThemChiTiet(ct);
        }

        public void XoaChiTiet(string maChiTiet)
        {
            if (string.IsNullOrWhiteSpace(maChiTiet))
            {
                throw new ArgumentException("Mã chi tiết cần xóa không được để trống!");
            }
            chiTietDAL.XoaChiTiet(maChiTiet);
        }

        public void XoaTheoHoaDon(string maHDB)
        {
            if (string.IsNullOrWhiteSpace(maHDB))
            {
                throw new ArgumentException("Mã hóa đơn để xóa các chi tiết không được để trống!");
            }
            chiTietDAL.XoaTheoHoaDon(maHDB);
        }

        public List<ChiTietHoaDonBanDTO> LayTheoHoaDon(string maHDB)
        {
            if (string.IsNullOrWhiteSpace(maHDB))
            {
                throw new ArgumentException("Mã hóa đơn tìm kiếm không được để trống!");
            }
            return chiTietDAL.LayTheoHoaDon(maHDB);
        }

        public ChiTietHoaDonBanDTO LayTheoId(string maChiTiet)
        {
            if (string.IsNullOrWhiteSpace(maChiTiet))
            {
                throw new ArgumentException("Mã chi tiết tìm kiếm không được để trống!");
            }
            return chiTietDAL.LayTheoId(maChiTiet);
        }

        public double TinhTongTien(string maHDB)
        {
            if (string.IsNullOrWhiteSpace(maHDB))
            {
                throw new ArgumentException("Mã hóa đơn tính tiền không được để trống!");
            }
            return chiTietDAL.TinhTongTien(maHDB);
        }
    }
}