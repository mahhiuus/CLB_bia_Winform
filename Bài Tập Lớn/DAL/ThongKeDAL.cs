using System;
using System.Data;
using Dapper;

namespace Bài_Tập_Lớn.DAL
{
    internal class ThongKeDAL
    {
        public decimal TongDoanhThu()
        {
            string sql = @"
                SELECT ISNULL(SUM(tong_tien),0)
                FROM hoa_don_ban";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<decimal>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng doanh thu: " + ex.Message);
            }
        }

        public decimal TongTienNhap()
        {
            string sql = @"
                SELECT ISNULL(SUM(tong_tien),0)
                FROM hoa_don_nhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<decimal>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền nhập: " + ex.Message);
            }
        }

        public DataTable TopSanPhamBanChay()
        {
            string sql = @"
                SELECT TOP 5
                    sp.ma_sp,
                    sp.ten_sp,
                    SUM(ct.so_luong) AS tong_so_luong
                FROM chi_tiet_hoa_don_ban ct
                INNER JOIN san_pham sp
                    ON ct.ma_sp = sp.ma_sp
                GROUP BY
                    sp.ma_sp,
                    sp.ten_sp
                ORDER BY tong_so_luong DESC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var reader = conn.ExecuteReader(sql);

                    DataTable dt = new DataTable();

                    dt.Load(reader);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê sản phẩm bán chạy: " + ex.Message);
            }
        }

        public int TongSoBan()
        {
            string sql = "SELECT COUNT(*) FROM ban_bida";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê số bàn: " + ex.Message);
            }
        }

        public int TongSoNhanVien()
        {
            string sql = "SELECT COUNT(*) FROM nhan_vien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê nhân viên: " + ex.Message);
            }
        }

        public int TongSoKhachHang()
        {
            string sql = "SELECT COUNT(*) FROM khach_hang";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê khách hàng: " + ex.Message);
            }
        }

        public int TongSoSanPham()
        {
            string sql = "SELECT COUNT(*) FROM san_pham";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thống kê sản phẩm: " + ex.Message);
            }
        }
    }
}