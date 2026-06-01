using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class HoaDonBanBLL
    {
        private readonly HoaDonBanDAL hoaDonBanDAL = new HoaDonBanDAL();
        private readonly PhienChoiDAL phienChoiDAL = new PhienChoiDAL();
        private readonly ChiTietPhienDAL chiTietPhienDAL = new ChiTietPhienDAL();

        public string SinhMaMoi()
        {
            return hoaDonBanDAL.SinhMaMoi();
        }

        public HoaDonBanDTO TaoTuPhien(string maPhien, string maKH, string maNV, double tienBida)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new ArgumentException("Mã phiên không được để trống!");
            }

            var pc = phienChoiDAL.TimTheoMaPhien(maPhien);
            if (pc == null) return null;

            double tienSanPham = chiTietPhienDAL.TinhTongTienTheoPhien(maPhien);
            double tongTien = tienBida + tienSanPham;

            HoaDonBanDTO hdb = new HoaDonBanDTO
            {
                MaHDB = SinhMaMoi(),
                MaPhien = maPhien,
                MaKH = maKH,
                MaNV = maNV,
                NgayBan = DateTime.Today,
                TienBida = tienBida,
                TienSanPham = tienSanPham,
                TongTien = tongTien,
                GhiChu = ""
            };

            return hdb;
        }

        public void Them(HoaDonBanDTO hdb)
        {
            if (hdb == null || string.IsNullOrWhiteSpace(hdb.MaHDB))
            {
                throw new ArgumentException("Dữ liệu hóa đơn bán không hợp lệ!");
            }
            hoaDonBanDAL.Them(hdb);
        }

        public void CapNhat(HoaDonBanDTO hdb)
        {
            if (hdb == null || string.IsNullOrWhiteSpace(hdb.MaHDB))
            {
                throw new ArgumentException("Dữ liệu hóa đơn cần cập nhật không hợp lệ!");
            }
            hoaDonBanDAL.CapNhat(hdb);
        }

        public void Xoa(string maHDB)
        {
            if (string.IsNullOrWhiteSpace(maHDB))
            {
                throw new ArgumentException("Mã hóa đơn không được để trống!");
            }
            hoaDonBanDAL.Xoa(maHDB);
        }

        public List<HoaDonBanDTO> LayTatCa()
        {
            return hoaDonBanDAL.LayTatCa();
        }

        public HoaDonBanDTO LayTheoMa(string maHDB)
        {
            if (string.IsNullOrWhiteSpace(maHDB))
            {
                throw new ArgumentException("Mã hóa đơn không được để trống!");
            }
            return hoaDonBanDAL.LayTheoMa(maHDB);
        }

        public HoaDonBanDTO LayTheoMaPhien(string maPhien)
        {
            if (string.IsNullOrWhiteSpace(maPhien))
            {
                throw new ArgumentException("Mã phiên không được để trống!");
            }
            return hoaDonBanDAL.LayTheoMaPhien(maPhien);
        }

        public List<HoaDonBanDTO> LayTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
            {
                throw new ArgumentException("Từ ngày không được lớn hơn Đến ngày!");
            }
            return hoaDonBanDAL.LayTheoNgay(tuNgay, denNgay);
        }

        public List<HoaDonBanDTO> LayTheoKhachHang(string maKH)
        {
            return hoaDonBanDAL.LayTheoKhachHang(maKH);
        }

        public List<HoaDonBanDTO> LayTheoNhanVien(string maNV)
        {
            return hoaDonBanDAL.LayTheoNhanVien(maNV);
        }

        public List<HoaDonBanDTO> LayTopHoaDon(int limit)
        {
            if (limit <= 0) limit = 10;
            return hoaDonBanDAL.LayTopHoaDon(limit);
        }

        public List<HoaDonBanDTO> LayTopHoaDonTheoNgay(DateTime ngay, int limit)
        {
            if (limit <= 0) limit = 10;
            return hoaDonBanDAL.LayTopHoaDonTheoNgay(ngay, limit);
        }

        public List<HoaDonBanDTO> LayTopHoaDonTheoThang(int thang, int nam, int limit)
        {
            if (thang < 1 || thang > 12 || nam < 1)
            {
                throw new ArgumentException("Tháng hoặc năm không hợp lệ!");
            }
            if (limit <= 0) limit = 10;
            return hoaDonBanDAL.LayTopHoaDonTheoThang(thang, nam, limit);
        }

        public List<HoaDonBanDTO> TimKiem(string keyword)
        {
            return hoaDonBanDAL.TimKiem(keyword);
        }
    }
}