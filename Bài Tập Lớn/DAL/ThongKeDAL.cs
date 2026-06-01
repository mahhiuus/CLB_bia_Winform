using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace Bài_Tập_Lớn.DAL
{
    internal class ThongKeDAL
    {
        // ══════════════════════════════════════════════════════════
        //  HELPER: Tháng/Năm của ngày gần nhất có hóa đơn
        //  → Dùng chung cho tất cả query "tháng hiện tại"
        //  → Nếu DB trống thì fallback về tháng hệ thống
        // ══════════════════════════════════════════════════════════
        private (int Thang, int Nam) LayThangNamGanNhat()
        {
            string sql = @"
                SELECT TOP 1 MONTH(ngay_ban) AS Thang, YEAR(ngay_ban) AS Nam
                FROM hoa_don_ban
                ORDER BY ngay_ban DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault<dynamic>(sql);
                    if (row != null)
                    {
                        int thang = Convert.ToInt32(row.Thang);
                        int nam = Convert.ToInt32(row.Nam);
                        return (thang, nam);
                    }
                }
            }
            catch { }
            return (DateTime.Now.Month, DateTime.Now.Year);
        }

        // ══════════════════════════════════════════════════════════
        //  1. Doanh thu – tháng gần nhất có dữ liệu
        //     Doanh Thu = SUM(tong_tien) của tất cả hóa đơn
        // ══════════════════════════════════════════════════════════
        public double GetDoanhThuThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT ISNULL(SUM(tong_tien), 0)
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang
                  AND YEAR(ngay_ban)  = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu tháng hiện tại: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  2. Số hóa đơn – tháng gần nhất có dữ liệu
        // ══════════════════════════════════════════════════════════
        public int GetSoHoaDonThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT COUNT(*)
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang
                  AND YEAR(ngay_ban)  = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số hóa đơn tháng hiện tại: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  3. Số khách hàng mới THÁNG HIỆN TẠI (giữ nguyên GETDATE)
        // ══════════════════════════════════════════════════════════
        public int GetKhachHangMoiThangHienTai()
        {
            string sql = @"
                SELECT COUNT(*)
                FROM khach_hang
                WHERE MONTH(ngay_dang_ky) = MONTH(GETDATE())
                  AND YEAR(ngay_dang_ky)  = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số khách hàng mới: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  4. Số bàn đang hoạt động
        // ══════════════════════════════════════════════════════════
        public int GetSoBanDangHoatDong()
        {
            string sql = "SELECT COUNT(*) FROM phien_choi WHERE trang_thai != 'DA_KET_THUC'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy số bàn đang hoạt động: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  5. Giá vốn – tháng gần nhất có dữ liệu
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap
                    GROUP BY ma_sp
                )
                SELECT ISNULL(SUM(c.so_luong * ISNULL(g.don_gia_von, 0)), 0)
                FROM chi_tiet_hoa_don_ban c
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb
                LEFT JOIN gia_von_tb g ON g.ma_sp = c.ma_sp
                WHERE MONTH(h.ngay_ban) = @Thang
                  AND YEAR(h.ngay_ban)  = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính giá vốn tháng hiện tại: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  6. Giá vốn THEO NGÀY
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonTheoNgay(DateTime ngay)
        {
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap
                    GROUP BY ma_sp
                )
                SELECT ISNULL(SUM(c.so_luong * ISNULL(g.don_gia_von, 0)), 0)
                FROM chi_tiet_hoa_don_ban c
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb
                LEFT JOIN gia_von_tb g ON g.ma_sp = c.ma_sp
                WHERE CAST(h.ngay_ban AS DATE) = CAST(@Ngay AS DATE)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Ngay = ngay });
            }
            catch { return 0; }
        }

        // ══════════════════════════════════════════════════════════
        //  7. Giá vốn THEO THÁNG
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonTheoThang(int thang, int nam)
        {
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap
                    GROUP BY ma_sp
                )
                SELECT ISNULL(SUM(c.so_luong * ISNULL(g.don_gia_von, 0)), 0)
                FROM chi_tiet_hoa_don_ban c
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb
                LEFT JOIN gia_von_tb g ON g.ma_sp = c.ma_sp
                WHERE MONTH(h.ngay_ban) = @Thang
                  AND YEAR(h.ngay_ban)  = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch { return 0; }
        }

        // ══════════════════════════════════════════════════════════
        //  8. Biểu đồ THEO NGÀY
        //  FIX: GROUP BY CAST(ngay_ban AS DATE)
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetDuLieuBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    CAST(ngay_ban AS DATE) AS NgayBan,
                    SUM(tong_tien)         AS DoanhThu
                FROM hoa_don_ban
                WHERE CAST(ngay_ban AS DATE)
                      BETWEEN CAST(@TuNgay AS DATE) AND CAST(@DenNgay AS DATE)
                GROUP BY CAST(ngay_ban AS DATE)
                ORDER BY NgayBan ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<dynamic>(sql, new { TuNgay = tuNgay, DenNgay = denNgay }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu biểu đồ theo ngày: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  9. Biểu đồ THEO THÁNG
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetDuLieuBieuDoTheoThang(int nam)
        {
            string sql = @"
                SELECT
                    MONTH(ngay_ban) AS Thang,
                    SUM(tong_tien)  AS DoanhThu
                FROM hoa_don_ban
                WHERE YEAR(ngay_ban) = @Nam
                GROUP BY MONTH(ngay_ban)
                ORDER BY Thang ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<dynamic>(sql, new { Nam = nam }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy dữ liệu biểu đồ theo tháng: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  10. [MỚI] Tổng TienBida và TienSanPham tháng gần nhất
        //      → Dùng cho Pie Chart (tỉ lệ thực từ DB)
        //      → Lợi nhuận = SUM(tien_bida) + SUM(tien_san_pham)
        //        (toàn bộ doanh thu không trừ giá vốn theo yêu cầu)
        // ══════════════════════════════════════════════════════════
        public (double TienBida, double TienSanPham) GetTienBidaVaTienSanPhamThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT
                    ISNULL(SUM(tien_bida),      0) AS TienBida,
                    ISNULL(SUM(tien_san_pham),  0) AS TienSanPham
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang
                  AND YEAR(ngay_ban)  = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault<dynamic>(sql, new { Thang = thang, Nam = nam });
                    if (row == null) return (0, 0);
                    return (Convert.ToDouble(row.TienBida), Convert.ToDouble(row.TienSanPham));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tien_bida / tien_san_pham: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  11. [MỚI] Snapshot phát hiện thay đổi dữ liệu
        //      Trả về: số hóa đơn trong tháng + thời điểm hóa đơn
        //      mới nhất. UI so sánh 2 lần liên tiếp để quyết định
        //      có cần reload hay không → KHÔNG reload thừa.
        // ══════════════════════════════════════════════════════════
        public (int SoHoaDon, DateTime NgayMoiNhat, int SoBanHoatDong) GetSnapshotThayDoi()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sqlHD = @"
                SELECT COUNT(*) AS SoHD,
                       ISNULL(MAX(ngay_ban), '1900-01-01') AS NgayMoiNhat
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang
                  AND YEAR(ngay_ban)  = @Nam";
            string sqlBan = "SELECT COUNT(*) FROM phien_choi WHERE trang_thai != 'DA_KET_THUC'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault<dynamic>(sqlHD, new { Thang = thang, Nam = nam });
                    int soBan = conn.ExecuteScalar<int>(sqlBan);
                    int soHD = row != null ? Convert.ToInt32(row.SoHD) : 0;
                    DateTime dt = row != null ? Convert.ToDateTime(row.NgayMoiNhat) : DateTime.MinValue;
                    return (soHD, dt, soBan);
                }
            }
            catch { return (0, DateTime.MinValue, 0); }
        }
    }
}