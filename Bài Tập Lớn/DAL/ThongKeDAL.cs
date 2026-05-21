using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Bài_Tập_Lớn.DAL
{
    internal class ThongKeDAL
    {
        // 1. Lấy Doanh thu của THÁNG HIỆN TẠI
        public double GetDoanhThuThangHienTai()
        {
            string sql = @"SELECT ISNULL(SUM(tong_tien), 0) 
                           FROM hoa_don_ban 
                           WHERE MONTH(ngay_ban) = MONTH(GETDATE()) 
                             AND YEAR(ngay_ban) = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu tháng hiện tại: " + ex.Message);
            }
        }

        // 2. Lấy Tổng số hóa đơn của THÁNG HIỆN TẠI
        public int GetSoHoaDonThangHienTai()
        {
            string sql = @"SELECT COUNT(*) 
                           FROM hoa_don_ban 
                           WHERE MONTH(ngay_ban) = MONTH(GETDATE()) 
                             AND YEAR(ngay_ban) = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số hóa đơn tháng hiện tại: " + ex.Message);
            }
        }

        // 3. Lấy Số khách hàng mới của THÁNG HIỆN TẠI
        public int GetKhachHangMoiThangHienTai()
        {
            string sql = @"SELECT COUNT(*) 
                           FROM khach_hang 
                           WHERE MONTH(ngay_dang_ky) = MONTH(GETDATE()) 
                             AND YEAR(ngay_dang_ky) = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số khách hàng mới: " + ex.Message);
            }
        }

        // 4. Lấy số Bàn đang hoạt động
        public int GetSoBanDangHoatDong()
        {
            string sql = "SELECT COUNT(*) FROM phien_choi WHERE trang_thai != 'DA_KET_THUC'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<int>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số bàn đang hoạt động: " + ex.Message);
            }
        }

        // 5. Tính tổng Tiền Vốn của các món hàng đã bán trong THÁNG NÀY
        public double GetGiaVonThangHienTai()
        {
            string sql = @"
                SELECT ISNULL(SUM(c.so_luong * ISNULL((
                    SELECT SUM(n.so_luong * n.don_gia_nhap) / NULLIF(SUM(n.so_luong), 0) 
                    FROM chi_tiet_hoa_don_nhap n 
                    WHERE n.ma_sp = c.ma_sp
                ), 0)), 0)
                FROM chi_tiet_hoa_don_ban c 
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb 
                WHERE MONTH(h.ngay_ban) = MONTH(GETDATE()) 
                  AND YEAR(h.ngay_ban) = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá vốn tháng hiện tại: " + ex.Message);
            }
        }

        // Lấy tiền vốn đã bán theo TỪNG NGÀY (Hàm bổ trợ)
        public double GetGiaVonTheoNgay(DateTime ngay)
        {
            string sql = @"
                SELECT ISNULL(SUM(c.so_luong * ISNULL((
                    SELECT SUM(n.so_luong * n.don_gia_nhap) / NULLIF(SUM(n.so_luong), 0) 
                    FROM chi_tiet_hoa_don_nhap n 
                    WHERE n.ma_sp = c.ma_sp
                ), 0)), 0)
                FROM chi_tiet_hoa_don_ban c 
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb 
                WHERE CAST(h.ngay_ban AS DATE) = CAST(@Ngay AS DATE)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql, new { Ngay = ngay });
                }
            }
            catch
            {
                return 0;
            }
        }

        // Lấy tiền vốn đã bán theo TỪNG THÁNG (Hàm bổ trợ)
        public double GetGiaVonTheoThang(int thang, int nam)
        {
            string sql = @"
                SELECT ISNULL(SUM(c.so_luong * ISNULL((
                    SELECT SUM(n.so_luong * n.don_gia_nhap) / NULLIF(SUM(n.so_luong), 0) 
                    FROM chi_tiet_hoa_don_nhap n 
                    WHERE n.ma_sp = c.ma_sp
                ), 0)), 0)
                FROM chi_tiet_hoa_don_ban c 
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb 
                WHERE MONTH(h.ngay_ban) = @Thang AND YEAR(h.ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
                }
            }
            catch
            {
                return 0;
            }
        }

        // 6. Lấy dữ liệu Biểu đồ (Doanh Thu vs Lợi Nhuận) THEO NGÀY
        public List<dynamic> GetDuLieuBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"SELECT ngay_ban AS NgayBan, SUM(tong_tien) as DoanhThu 
                           FROM hoa_don_ban 
                           WHERE ngay_ban BETWEEN @TuNgay AND @DenNgay 
                           GROUP BY ngay_ban 
                           ORDER BY ngay_ban ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    // Dapper trả về danh sách đối tượng dynamic, ta có thể dễ dàng đọc thuộc tính
                    return conn.Query<dynamic>(sql, new { TuNgay = tuNgay, DenNgay = denNgay }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu biểu đồ theo ngày: " + ex.Message);
            }
        }

        // 7. Lấy dữ liệu Biểu đồ (Doanh Thu vs Lợi Nhuận) THEO THÁNG
        public List<dynamic> GetDuLieuBieuDoTheoThang(int nam)
        {
            string sql = @"SELECT MONTH(ngay_ban) as Thang, SUM(tong_tien) as DoanhThu 
                           FROM hoa_don_ban 
                           WHERE YEAR(ngay_ban) = @Nam 
                           GROUP BY MONTH(ngay_ban) 
                           ORDER BY Thang ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<dynamic>(sql, new { Nam = nam }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu biểu đồ theo tháng: " + ex.Message);
            }
        }
    }
}