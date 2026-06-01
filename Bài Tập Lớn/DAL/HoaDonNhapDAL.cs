using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class HoaDonNhapDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_hdn, 4, LEN(ma_hdn)) AS INT)),0)
                           FROM hoa_don_nhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"HDN{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã hóa đơn nhập: " + ex.Message);
            }
        }

        public List<HoaDonNhapDTO> LayTatCaHoaDonNhap()
        {
            string sql = "SELECT * FROM hoa_don_nhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonNhapDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn nhập: " + ex.Message);
            }
        }

        public HoaDonNhapDTO TimTheoMaHDN(string maHDN)
        {
            string sql = "SELECT * FROM hoa_don_nhap WHERE ma_hdn = @MaHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<HoaDonNhapDTO>(
                        sql,
                        new { MaHDN = maHDN }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm hóa đơn nhập: " + ex.Message);
            }
        }

        public bool ThemHoaDonNhap(HoaDonNhapDTO hdn)
        {
            string sql = @"
                INSERT INTO hoa_don_nhap
                (
                    ma_hdn,
                    ma_ncc,
                    ngay_nhap,
                    tong_tien
                )
                VALUES
                (
                    @MaHDN,
                    @MaNCC,
                    @NgayNhap,
                    @TongTien
                )";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, hdn);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hóa đơn nhập: " + ex.Message);
            }
        }

        public bool CapNhatHoaDonNhap(HoaDonNhapDTO hdn)
        {
            string sql = @"
                UPDATE hoa_don_nhap
                SET
                    ma_ncc = @MaNCC,
                    ngay_nhap = @NgayNhap,
                    tong_tien = @TongTien
                WHERE ma_hdn = @MaHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, hdn);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hóa đơn nhập: " + ex.Message);
            }
        }

        public bool XoaHoaDonNhap(string maHDN)
        {
            string sql = "DELETE FROM hoa_don_nhap WHERE ma_hdn = @MaHDN";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaHDN = maHDN });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa hóa đơn nhập: " + ex.Message);
            }
        }

        public List<HoaDonNhapDTO> TimKiem(string keyword)
        {
            string sql = @"
                SELECT * FROM hoa_don_nhap 
                WHERE ma_hdn LIKE @Keyword 
                   OR ma_ncc LIKE @Keyword";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + (keyword ?? "").Trim() + "%", System.Data.DbType.String);

                    return conn.Query<HoaDonNhapDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm hóa đơn nhập: " + ex.Message);
            }
        }
        public List<HoaDonNhapDTO> LayTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            string sql = "SELECT * FROM hoa_don_nhap WHERE ngay_nhap BETWEEN @TuNgay AND @DenNgay";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonNhapDTO>(sql, new { TuNgay = tuNgay, DenNgay = denNgay }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn nhập theo khoảng ngày: " + ex.Message);
            }
        }

        public List<HoaDonNhapDTO> LayTopHoaDon(int limit)
        {
            string sql = "SELECT TOP (@Limit) * FROM hoa_don_nhap ORDER BY ngay_nhap DESC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonNhapDTO>(sql, new { Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách top hóa đơn nhập: " + ex.Message);
            }
        }

        public List<HoaDonNhapDTO> LayTopHoaDonTheoNgay(DateTime ngay, int limit)
        {
            string sql = @"SELECT TOP (@Limit) * FROM hoa_don_nhap 
                           WHERE CAST(ngay_nhap AS DATE) = CAST(@Ngay AS DATE)
                           ORDER BY ngay_nhap DESC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonNhapDTO>(sql, new { Ngay = ngay, Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy top hóa đơn nhập theo ngày: " + ex.Message);
            }
        }

        public List<HoaDonNhapDTO> LayTopHoaDonTheoThang(int thang, int nam, int limit)
        {
            string sql = @"SELECT TOP (@Limit) * FROM hoa_don_nhap 
                           WHERE MONTH(ngay_nhap) = @Thang AND YEAR(ngay_nhap) = @Nam
                           ORDER BY ngay_nhap DESC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonNhapDTO>(sql, new { Thang = thang, Nam = nam, Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy top hóa đơn nhập theo tháng/nam: " + ex.Message);
            }
        }
    }
}