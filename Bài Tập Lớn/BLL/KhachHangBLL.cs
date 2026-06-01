using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL dal = new KhachHangDAL();
        public string SinhMaMoi()
        {
            return dal.SinhMaMoi();
        }
        public List<KhachHangDTO> LayTatCaKhachHang()
        {
            return dal.LayTatCaKhachHang();
        }
        public bool ThemKhachHang(KhachHangDTO kh)
        {
            if (kh == null)
            {
                throw new Exception("Dữ liệu khách hàng không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(kh.HoTen))
            {
                throw new Exception("Tên khách hàng không được để trống!");
            }

            return dal.ThemKhachHang(kh);
        }
        public bool XoaKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
            {
                throw new Exception("Mã khách hàng không hợp lệ!");
            }

            return dal.XoaKhachHang(maKH);
        }
        public bool CapNhatKhachHang(KhachHangDTO kh)
        {
            if (kh == null)
            {
                throw new Exception("Dữ liệu khách hàng không hợp lệ!");
            }

            return dal.CapNhatKhachHang(kh);
        }
        public KhachHangDTO TimTheoMaKhachHang(string maKH)
        {
            return dal.TimTheoMaKhachHang(maKH);
        }
        public List<KhachHangDTO> TimKiem(string keyword)
        {
            return dal.TimKiem(keyword);
        }
        public bool CongTichLuy(string maKH, int soLan = 1)
        {
            if (string.IsNullOrWhiteSpace(maKH))
                throw new Exception("Mã khách hàng không hợp lệ!");

            var kh = dal.TimTheoMaKhachHang(maKH);
            if (kh == null)
                throw new Exception($"Không tìm thấy khách hàng: {maKH}");

            kh.DiemTichLuy += soLan;
            return dal.CapNhatKhachHang(kh);
        }
    }
}