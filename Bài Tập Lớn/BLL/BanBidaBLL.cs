using System;
using System.Collections.Generic;
using System.Linq;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{

    public class BanBidaBLL
    {

        private readonly BanBidaDAL _banBidaDAL = new BanBidaDAL();

        public string SinhMaMoi()
        {
            return _banBidaDAL.SinhMaMoi();
        }

        public bool ThemBan(BanBidaDTO ban)
        {
            if (ban == null)
            {
                throw new Exception("Dữ liệu bàn bida không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ban.MaBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ban.TenBan))
            {
                throw new Exception("Tên bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ban.LoaiBan))
            {
                throw new Exception("Loại bàn không được để trống!");
            }

            if (ban.GiaTheoGio <= 0)
            {
                throw new Exception("Giá theo giờ của bàn phải lớn hơn 0!");
            }

            if (_banBidaDAL.TimTheoMaBan(ban.MaBan) != null)
            {
                throw new Exception("Mã bàn bida này đã tồn tại trong hệ thống!");
            }

            return _banBidaDAL.ThemBan(ban);
        }

        public bool CapNhatBan(BanBidaDTO ban)
        {
            if (ban == null)
            {
                throw new Exception("Dữ liệu bàn bida không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ban.MaBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ban.TenBan))
            {
                throw new Exception("Tên bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(ban.LoaiBan))
            {
                throw new Exception("Loại bàn không được để trống!");
            }

            if (ban.GiaTheoGio <= 0)
            {
                throw new Exception("Giá theo giờ của bàn phải lớn hơn 0!");
            }

            return _banBidaDAL.CapNhatBan(ban);
        }

        public bool CapNhatTrangThai(string maBan, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception("Trạng thái không được để trống!");
            }

            return _banBidaDAL.CapNhatTrangThai(maBan, trangThai);
        }

        public bool XoaBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }


            BanBidaDTO banHienTai = _banBidaDAL.TimTheoMaBan(maBan);
            if (banHienTai != null && banHienTai.TrangThai == "Có khách")
            {
                throw new Exception("Không thể xóa bàn bida đang có khách sử dụng!");
            }

            return _banBidaDAL.XoaBan(maBan);
        }

        public List<BanBidaDTO> LayTatCaBan()
        {
            return _banBidaDAL.LayTatCaBan();
        }

        public BanBidaDTO TimTheoMaBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            return _banBidaDAL.TimTheoMaBan(maBan);
        }

        public List<BanBidaDTO> TimTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception("Trạng thái không được để trống!");
            }

            return _banBidaDAL.TimTheoTrangThai(trangThai);
        }

        public List<BanBidaDTO> TimTheoLoaiBan(string loaiBan)
        {
            if (string.IsNullOrWhiteSpace(loaiBan))
            {
                throw new Exception("Loại bàn không được để trống!");
            }

            return _banBidaDAL.TimTheoLoaiBan(loaiBan);
        }
        public List<BanBidaDTO> TimKiem(string keyword)
        {
            return _banBidaDAL.TimKiem(keyword);
        }
    }
}