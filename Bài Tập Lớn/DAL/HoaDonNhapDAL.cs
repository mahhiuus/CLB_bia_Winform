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
    }
}