using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL dal = new KhachHangDAL();

        // Sinh mã mới
        public string SinhMaMoi()
        {
            return dal.SinhMaMoi();
        }

        // Lấy danh sách khách hàng
        public List<KhachHangDTO> LayTatCaKhachHang()
        {
            return dal.LayTatCaKhachHang();
        }

        // Thêm khách hàng
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

        // Xóa khách hàng
        public bool XoaKhachHang(string maKH)
        {
            if (string.IsNullOrWhiteSpace(maKH))
            {
                throw new Exception("Mã khách hàng không hợp lệ!");
            }

            return dal.XoaKhachHang(maKH);
        }

        // Cập nhật khách hàng
        public bool CapNhatKhachHang(KhachHangDTO kh)
        {
            if (kh == null)
            {
                throw new Exception("Dữ liệu khách hàng không hợp lệ!");
            }

            return dal.CapNhatKhachHang(kh);
        }

        // Tìm theo mã khách hàng
        public KhachHangDTO TimTheoMaKhachHang(string maKH)
        {
            return dal.TimTheoMaKhachHang(maKH);
        }

        // Tìm kiếm khách hàng
        public List<KhachHangDTO> TimKiem(string keyword)
        {
            return dal.TimKiem(keyword);
        }

        // [MỚI] Cộng điểm tích lũy cho khách hàng thân thiết
        // Lấy KH hiện tại, cộng thêm soLan điểm rồi cập nhật lại
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