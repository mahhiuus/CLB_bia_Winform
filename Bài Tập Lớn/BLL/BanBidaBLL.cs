using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class BanBidaBLL
    {
        private readonly BanBidaDAL banBidaDAL = new BanBidaDAL();

        public List<BanBidaDTO> LayTatCaBan() => banBidaDAL.LayTatCaBan();

        public BanBidaDTO TimTheoBan(string maBan)
        {
            if (string.IsNullOrWhiteSpace(maBan)) throw new Exception("Mã bàn không được để trống!");
            return banBidaDAL.TimTheoBan(maBan);
        }

        public bool CapNhatTrangThai(string maBan, string trangThai)
        {
            if (string.IsNullOrWhiteSpace(maBan)) throw new Exception("Mã bàn không được để trống!");
            if (string.IsNullOrWhiteSpace(trangThai)) throw new Exception("Trạng thái không được để trống!");
            return banBidaDAL.CapNhatTrangThai(maBan, trangThai);
        }
    }
}
