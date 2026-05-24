using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class BanBidaDAL
    {
        public List<BanBidaDTO> LayTatCaBan()
        {
            string sql = "SELECT ma_ban AS MaBan, ten_ban AS TenBan, loai_ban AS LoaiBan, gia_theo_gio AS GiaTheoGio, trang_thai AS TrangThai FROM ban_bida ORDER BY loai_ban, ten_ban";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<BanBidaDTO>(sql).ToList();
            }
            catch (Exception ex) { throw new Exception("Lỗi khi lấy danh sách bàn: " + ex.Message); }
        }

        public BanBidaDTO TimTheoBan(string maBan)
        {
            string sql = "SELECT ma_ban AS MaBan, ten_ban AS TenBan, loai_ban AS LoaiBan, gia_theo_gio AS GiaTheoGio, trang_thai AS TrangThai FROM ban_bida WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.QueryFirstOrDefault<BanBidaDTO>(sql, new { MaBan = maBan });
            }
            catch (Exception ex) { throw new Exception("Lỗi khi tìm bàn: " + ex.Message); }
        }

        public bool CapNhatTrangThai(string maBan, string trangThai)
        {
            string sql = "UPDATE ban_bida SET trang_thai = @TrangThai WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Execute(sql, new { MaBan = maBan, TrangThai = trangThai }) > 0;
            }
            catch (Exception ex) { throw new Exception("Lỗi khi cập nhật trạng thái bàn: " + ex.Message); }
        }
    }
}
