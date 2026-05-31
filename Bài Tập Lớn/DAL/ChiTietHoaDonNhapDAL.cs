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
        // ══════════════════════════════════════════════════════════
        //  Sinh mã mới
        //  FIX: DB column = ma_ct_hdn (có dấu gạch)
        // ══════════════════════════════════════════════════════════
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_ct_hdn, 6, LEN(ma_ct_hdn)) AS INT)), 0)
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
            // FIX: dùng alias để Dapper map đúng property DTO
            string sql = @"SELECT
                               ma_ct_hdn    AS MaCTHDN,
                               ma_hdn       AS MaHDN,
                               ma_sp        AS MaSP,
                               so_luong     AS SoLuong,
                               don_gia_nhap AS DonGiaNhap
                           FROM chi_tiet_hoa_don_nhap";
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
            string sql = @"SELECT
                               ma_ct_hdn    AS MaCTHDN,
                               ma_hdn       AS MaHDN,
                               ma_sp        AS MaSP,
                               so_luong     AS SoLuong,
                               don_gia_nhap AS DonGiaNhap
                           FROM chi_tiet_hoa_don_nhap
                           WHERE ma_hdn = @MaHDN";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietHoaDonNhapDTO>(sql, new { MaHDN = maHDN }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm chi tiết hóa đơn nhập: " + ex.Message);
            }
        }

        public double TinhTongTien(string maHDN)
        {
            // FIX: don_gia → don_gia_nhap
            string sql = @"SELECT ISNULL(SUM(so_luong * don_gia_nhap), 0)
                           FROM chi_tiet_hoa_don_nhap
                           WHERE ma_hdn = @MaHDN";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql, new { MaHDN = maHDN });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền: " + ex.Message);
            }
        }

        public bool ThemChiTiet(ChiTietHoaDonNhapDTO ct)
        {
            // FIX: ma_cthdn → ma_ct_hdn, don_gia → don_gia_nhap
            //      anonymous object để @DonGiaNhap map đúng ct.DonGiaNhap
            string sql = @"INSERT INTO chi_tiet_hoa_don_nhap
                               (ma_ct_hdn, ma_hdn, ma_sp, so_luong, don_gia_nhap)
                           VALUES
                               (@MaCTHDN, @MaHDN, @MaSP, @SoLuong, @DonGiaNhap)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        ct.MaCTHDN,
                        ct.MaHDN,
                        ct.MaSP,
                        ct.SoLuong,
                        ct.DonGiaNhap
                    });
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
            string sql = @"UPDATE chi_tiet_hoa_don_nhap
                           SET ma_hdn       = @MaHDN,
                               ma_sp        = @MaSP,
                               so_luong     = @SoLuong,
                               don_gia_nhap = @DonGiaNhap
                           WHERE ma_ct_hdn = @MaCTHDN";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        ct.MaHDN,
                        ct.MaSP,
                        ct.SoLuong,
                        ct.DonGiaNhap,
                        ct.MaCTHDN
                    });
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
            string sql = "DELETE FROM chi_tiet_hoa_don_nhap WHERE ma_ct_hdn = @MaCTHDN";
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

        public bool XoaTheoMaHDN(string maHDN)
        {
            string sql = "DELETE FROM chi_tiet_hoa_don_nhap WHERE ma_hdn = @MaHDN";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { MaHDN = maHDN });
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết theo mã HDN: " + ex.Message);
            }
        }
    }
}