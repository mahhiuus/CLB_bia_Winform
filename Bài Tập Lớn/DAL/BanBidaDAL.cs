using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    public class BanBidaDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"
                SELECT ISNULL(MAX(CAST(
                    SUBSTRING(ma_ban, PATINDEX('%[0-9]%', ma_ban), LEN(ma_ban))
                AS INT)), 0)
                FROM ban_bida";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"AN{soThuTu:D3}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã bàn bida: " + ex.Message);
            }
        }

        public bool ThemBan(BanBidaDTO ban)
        {
            string sql = @"INSERT INTO ban_bida(ma_ban, ten_ban, loai_ban, gia_theo_gio, trang_thai) 
                           VALUES(@MaBan, @TenBan, @LoaiBan, @GiaTheoGio, @TrangThai)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        MaBan = ban.MaBan,
                        TenBan = ban.TenBan,
                        LoaiBan = ban.LoaiBan,
                        GiaTheoGio = ban.GiaTheoGio,
                        TrangThai = ban.TrangThai
                    });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm bàn bida: " + ex.Message);
            }
        }

        public bool CapNhatBan(BanBidaDTO ban)
        {
            string sql = @"UPDATE ban_bida SET 
                              ten_ban     = @TenBan, 
                              loai_ban    = @LoaiBan, 
                              gia_theo_gio = @GiaTheoGio, 
                              trang_thai  = @TrangThai 
                           WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        TenBan = ban.TenBan,
                        LoaiBan = ban.LoaiBan,
                        GiaTheoGio = ban.GiaTheoGio,
                        TrangThai = ban.TrangThai,
                        MaBan = ban.MaBan
                    });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật bàn bida: " + ex.Message);
            }
        }

        public bool CapNhatTrangThai(string maBan, string trangThai)
        {
            string sql = @"UPDATE ban_bida 
                           SET trang_thai = @TrangThai 
                           WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaBan = maBan, TrangThai = trangThai });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật trạng thái bàn: " + ex.Message);
            }
        }

        public bool XoaBan(string maBan)
        {
            string sql = @"DELETE FROM ban_bida 
                           WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaBan = maBan });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa bàn bida: " + ex.Message);
            }
        }

        public List<BanBidaDTO> LayTatCaBan()
        {
            string sql = @"SELECT 
                            ma_ban       AS MaBan, 
                            ten_ban      AS TenBan, 
                            loai_ban     AS LoaiBan, 
                            gia_theo_gio AS GiaTheoGio, 
                            trang_thai   AS TrangThai 
                           FROM ban_bida 
                           ORDER BY ma_ban";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<BanBidaDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách bàn bida: " + ex.Message);
            }
        }

        public BanBidaDTO TimTheoMaBan(string maBan)
        {
            string sql = @"SELECT 
                            ma_ban       AS MaBan, 
                            ten_ban      AS TenBan, 
                            loai_ban     AS LoaiBan, 
                            gia_theo_gio AS GiaTheoGio, 
                            trang_thai   AS TrangThai 
                           FROM ban_bida 
                           WHERE ma_ban = @MaBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<BanBidaDTO>(sql, new { MaBan = maBan });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm bàn theo mã: " + ex.Message);
            }
        }

        public List<BanBidaDTO> TimTheoTrangThai(string trangThai)
        {
            string sql = @"SELECT 
                            ma_ban       AS MaBan, 
                            ten_ban      AS TenBan, 
                            loai_ban     AS LoaiBan, 
                            gia_theo_gio AS GiaTheoGio, 
                            trang_thai   AS TrangThai 
                           FROM ban_bida 
                           WHERE trang_thai = @TrangThai";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<BanBidaDTO>(sql, new { TrangThai = trangThai }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm bàn theo trạng thái: " + ex.Message);
            }
        }

        public List<BanBidaDTO> TimTheoLoaiBan(string loaiBan)
        {
            string sql = @"SELECT 
                            ma_ban       AS MaBan, 
                            ten_ban      AS TenBan, 
                            loai_ban     AS LoaiBan, 
                            gia_theo_gio AS GiaTheoGio, 
                            trang_thai   AS TrangThai 
                           FROM ban_bida 
                           WHERE loai_ban = @LoaiBan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<BanBidaDTO>(sql, new { LoaiBan = loaiBan }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm bàn theo loại: " + ex.Message);
            }
        }

        public List<BanBidaDTO> TimKiem(string keyword)
        {
            string sql = @"
                SELECT 
                    ma_ban AS MaBan, 
                    ten_ban AS TenBan, 
                    loai_ban AS LoaiBan, 
                    gia_theo_gio AS GiaTheoGio, 
                    trang_thai AS TrangThai 
                FROM ban_bida 
                WHERE ma_ban LIKE @Keyword 
                   OR ten_ban COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI
                   OR loai_ban COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI
                ORDER BY ma_ban";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + (keyword ?? "").Trim() + "%", System.Data.DbType.String);

                    return conn.Query<BanBidaDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm bàn bida: " + ex.Message);
            }
        }
    }
}