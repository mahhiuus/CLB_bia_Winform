using System;
using System.Data;

namespace Bài_Tập_Lớn.DTO
{
    public class BanBidaDTO
    {
        public string MaBan { get; set; }
        public string TenBan { get; set; }
        public string LoaiBan { get; set; }
        public double GiaTheoGio { get; set; }
        public string TrangThai { get; set; }

        public BanBidaDTO()
        {
            MaBan = "";
            TenBan = "";
            LoaiBan = "";
            GiaTheoGio = 0;
            TrangThai = "";
        }
        public BanBidaDTO(string maBan, string tenBan, string loaiBan, double giaTheoGio, string trangThai)
        {
            this.MaBan = maBan;
            this.TenBan = tenBan;
            this.LoaiBan = loaiBan;
            this.GiaTheoGio = giaTheoGio;
            this.TrangThai = trangThai;
        }
    }
}