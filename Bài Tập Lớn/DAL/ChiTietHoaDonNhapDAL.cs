using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class ChiTietHoaDonNhapDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_cthdn, 6, LEN(ma_cthdn)) AS INT)),0)
                           FROM chi_tiet_hoa_don_nhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"CTHDN{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public List<ChiTietHoaDonNhapDTO> LayTatCaChiTiet()
        {
            string sql = "SELECT * FROM chi_tiet_hoa_don_nhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietHoaDonNhapDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public List<ChiTietHoaDonNhapDTO> TimTheoMaHDN(string maHDN)
        {
            string sql = @"SELECT * FROM chi_tiet_hoa_don_nhap
                           WHERE ma_hdn = @MaHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietHoaDonNhapDTO>(
                        sql,
                        new { MaHDN = maHDN }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public decimal TinhTongTien(string maHDN)
        {
            string sql = @"
                SELECT ISNULL(SUM(so_luong * don_gia),0)
                FROM chi_tiet_hoa_don_nhap
                WHERE ma_hdn = @MaHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<decimal>(
                        sql,
                        new { MaHDN = maHDN }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền: " + ex.Message);
            }
        }

        public bool ThemChiTiet(ChiTietHoaDonNhapDTO ct)
        {
            string sql = @"
                INSERT INTO chi_tiet_hoa_don_nhap
                (
                    ma_cthdn,
                    ma_hdn,
                    ma_sp,
                    so_luong,
                    don_gia
                )
                VALUES
                (
                    @MaCTHDN,
                    @MaHDN,
                    @MaSP,
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
                throw new Exception("Lỗi khi thêm chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public bool CapNhatChiTiet(ChiTietHoaDonNhapDTO ct)
        {
            string sql = @"
                UPDATE chi_tiet_hoa_don_nhap
                SET
                    ma_hdn = @MaHDN,
                    ma_sp = @MaSP,
                    so_luong = @SoLuong,
                    don_gia = @DonGia
                WHERE ma_cthdn = @MaCTHDN";

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
                throw new Exception("Lỗi khi cập nhật chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public bool XoaChiTiet(string maCTHDN)
        {
            string sql = @"DELETE FROM chi_tiet_hoa_don_nhap
                           WHERE ma_cthdn = @MaCTHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaCTHDN = maCTHDN });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết hóa đơn nhập: " + ex.Message);
            }
        }
    }
}