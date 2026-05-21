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
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(MaBan, 2, LEN(MaBan)) AS INT)), 0) 
                           FROM BanBida";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"B{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã bàn bida: " + ex.Message);
            }
        }

        public bool ThemBan(BanBidaDTO ban)
        {
            string sql = @"INSERT INTO BanBida(MaBan, TenBan, LoaiBan, GiaTheoGio, TrangThai) 
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
            string sql = @"UPDATE BanBida SET 
                              TenBan = @TenBan, 
                              LoaiBan = @LoaiBan, 
                              GiaTheoGio = @GiaTheoGio, 
                              TrangThai = @TrangThai 
                           WHERE MaBan = @MaBan";
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
            string sql = @"UPDATE BanBida 
                           SET TrangThai = @TrangThai 
                           WHERE MaBan = @MaBan";
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
            string sql = @"DELETE FROM BanBida 
                           WHERE MaBan = @MaBan";
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
            string sql = @"SELECT * FROM BanBida 
                           ORDER BY MaBan"; 
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
            string sql = @"SELECT * FROM BanBida 
                           WHERE MaBan = @MaBan";
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
            string sql = @"SELECT * FROM BanBida 
                           WHERE TrangThai = @TrangThai";
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
            string sql = @"SELECT * FROM BanBida 
                           WHERE LoaiBan = @LoaiBan";
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
    }
}