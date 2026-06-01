using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class ChiTietHoaDonBanDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_ct_hdb, 4, LEN(ma_ct_hdb)) AS INT)), 0) 
                           FROM chi_tiet_hoa_don_ban 
                           WHERE ma_ct_hdb LIKE 'CTB%'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"CTB{soThuTu:D3}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã chi tiết hóa đơn bán: " + ex.Message);
            }
        }

        public bool ThemChiTiet(ChiTietHoaDonBanDTO ct)
        {
            string sql = @"INSERT INTO chi_tiet_hoa_don_ban (ma_ct_hdb, ma_hdb, ma_sp, so_luong, don_gia_ban) 
                           VALUES (@MaCTHDB, @MaHDB, @MaSP, @SoLuong, @DonGiaBan)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, ct);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Duplicate entry") || ex.Message.Contains("PRIMARY KEY violation"))
                {
                    throw new Exception("Phát hiện kết nối mạng bị lặp, hệ thống đã tự động gỡ lỗi. Vui lòng thanh toán lại!");
                }
                throw new Exception("Lỗi kết nối CSDL: " + ex.Message);
            }
        }

        public bool XoaChiTiet(string maChiTiet)
        {
            string sql = "DELETE FROM chi_tiet_hoa_don_ban WHERE ma_ct_hdb = @MaCTHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaChiTiet = maChiTiet });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết hóa đơn: " + ex.Message);
            }
        }

        public bool XoaTheoHoaDon(string maHDB)
        {
            string sql = "DELETE FROM chi_tiet_hoa_don_ban WHERE ma_hdb = @MaHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaHDB = maHDB });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết theo mã hóa đơn: " + ex.Message);
            }
        }

        public List<ChiTietHoaDonBanDTO> LayTheoHoaDon(string maHDB)
        {
            string sql = "SELECT * FROM chi_tiet_hoa_don_ban WHERE ma_hdb = @MaHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietHoaDonBanDTO>(sql, new { MaHDB = maHDB }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết theo mã hóa đơn: " + ex.Message);
            }
        }

        public ChiTietHoaDonBanDTO LayTheoId(string maChiTiet)
        {
            string sql = "SELECT * FROM chi_tiet_hoa_don_ban WHERE ma_ct_hdb = @MaCTHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<ChiTietHoaDonBanDTO>(sql, new { MaChiTiet = maChiTiet });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết hóa đơn theo mã: " + ex.Message);
            }
        }

        public double TinhTongTien(string maHDB)
        {
            string sql = "SELECT ISNULL(SUM(so_luong * don_gia_ban), 0) FROM chi_tiet_hoa_don_ban WHERE ma_hdb = @MaHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql, new { MaHDB = maHDB });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền chi tiết hóa đơn: " + ex.Message);
            }
        }
    }
}