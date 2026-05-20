using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    internal class BanBida
    {
        BanBidaDAL dal = new BanBidaDAL();

        public string SinhMaMoi()
        {
            return dal.SinhMaMoi();
        }

        public void ThemBan(BanBidaDTO ban)
        {
            if (ban == null)
            {
                throw new Exception("Dữ liệu bàn bida không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ban.MaBan) ||
                 string.IsNullOrWhiteSpace(ban.TenBan) ||
                 string.IsNullOrWhiteSpace(ban.LoaiBan)) // Bỏ TrangThai ra khỏi IsNullOrWhiteSpace
            {
                throw new Exception("Không được để trống dữ liệu!");
            }

            dal.ThemBan(ban);
        }

        public void CapNhatBan(BanBidaDTO ban)
        {
            if (ban == null)
            {
                throw new Exception("Dữ liệu bàn bida không hợp lệ!");
            }

            if (string.IsNullOrWhiteSpace(ban.MaBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            dal.CapNhatBan(ban);
        }

        public void CapNhatTrangThai(string maBan, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception("Trạng thái không được để trống!");
            }

            dal.CapNhatTrangThai(maBan, trangThai);
        }

     
        public void XoaBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            dal.XoaBan(maBan);
        }

   
        public List<BanBidaDTO> LayTatCaBan()
        {
            return dal.LayTatCaBan();
        }

      
        public BanBidaDTO TimTheoMaBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan))
            {
                throw new Exception("Mã bàn không được để trống!");
            }

            return dal.TimTheoMaBan(maBan);
        }

      
        public List<BanBidaDTO> TimTheoTrangThai(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception("Trạng thái không được để trống!");
            }

            return dal.TimTheoTrangThai(trangThai);
        }

   
        public List<BanBidaDTO> TimTheoLoaiBan(string loaiBan)
        {
            if (string.IsNullOrWhiteSpace(loaiBan))
            {
                throw new Exception("Loại bàn không được để trống!");
            }

            return dal.TimTheoLoaiBan(loaiBan);
        }
    }
}
