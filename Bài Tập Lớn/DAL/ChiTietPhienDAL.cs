using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class ChiTietPhienDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_chi_tiet, 3, LEN(ma_chi_tiet)) AS INT)),0)
                           FROM chi_tiet_phien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;

                    return $"CT{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã chi tiết phiên: " + ex.Message);
            }
        }

        public List<ChiTietPhienDTO> LayTatCaChiTietPhien()
        {
            string sql = "SELECT * FROM chi_tiet_phien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietPhienDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết phiên: " + ex.Message);
            }
        }

        public List<ChiTietPhienDTO> TimTheoMaPhien(string maPhien)
        {
            string sql = @"SELECT * FROM chi_tiet_phien
                           WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietPhienDTO>(
                        sql,
                        new { MaPhien = maPhien }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm chi tiết phiên: " + ex.Message);
            }
        }

        public double TinhTongTienTheoPhien(string maPhien)
        {
            string sql = @"
                SELECT ISNULL(SUM(so_luong * don_gia),0)
                FROM chi_tiet_phien
                WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(
                        sql,
                        new { MaPhien = maPhien }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền: " + ex.Message);
            }
        }

        public bool ThemChiTietPhien(ChiTietPhienDTO ct)
        {
            string sql = @"
                INSERT INTO chi_tiet_phien
                (
                    ma_chi_tiet,
                    ma_phien,
                    ma_san_pham,
                    so_luong,
                    don_gia
                )
                VALUES
                (
                    @MaChiTiet,
                    @MaPhien,
                    @MaSanPham,
                    @SoLuong,
                    @DonGia
                )";

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
                throw new Exception("Lỗi khi thêm chi tiết phiên: " + ex.Message);
            }
        }

        public bool CapNhatChiTietPhien(ChiTietPhienDTO ct)
        {
            string sql = @"
                UPDATE chi_tiet_phien
                SET
                    ma_phien = @MaPhien,
                    ma_san_pham = @MaSanPham,
                    so_luong = @SoLuong,
                    don_gia = @DonGia
                WHERE ma_chi_tiet = @MaChiTiet";

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
                throw new Exception("Lỗi khi cập nhật chi tiết phiên: " + ex.Message);
            }
        }

        public bool XoaChiTietPhien(string maChiTiet)
        {
            string sql = @"DELETE FROM chi_tiet_phien
                           WHERE ma_chi_tiet = @MaChiTiet";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new { MaChiTiet = maChiTiet });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết phiên: " + ex.Message);
            }
        }
    }
}