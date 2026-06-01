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
        //  HELPER: tháng/năm của hóa đơn gần nhất
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
                        return (Convert.ToInt32(row.Thang), Convert.ToInt32(row.Nam));
                }
            }
            catch { }
            return (DateTime.Now.Month, DateTime.Now.Year);
        }

        // ══════════════════════════════════════════════════════════
        //  1. Doanh thu tháng gần nhất
        // ══════════════════════════════════════════════════════════
        public double GetDoanhThuThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT ISNULL(SUM(tong_tien), 0)
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang AND YEAR(ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex) { throw new Exception("Lỗi doanh thu tháng: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  2. Số hóa đơn tháng gần nhất
        // ══════════════════════════════════════════════════════════
        public int GetSoHoaDonThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT COUNT(*)
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang AND YEAR(ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex) { throw new Exception("Lỗi số hóa đơn: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  3. Khách hàng mới tháng hiện tại
        // ══════════════════════════════════════════════════════════
        public int GetKhachHangMoiThangHienTai()
        {
            string sql = @"
                SELECT COUNT(*) FROM khach_hang
                WHERE MONTH(ngay_dang_ky) = MONTH(GETDATE())
                  AND YEAR(ngay_dang_ky)  = YEAR(GETDATE())";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql);
            }
            catch (Exception ex) { throw new Exception("Lỗi khách hàng mới: " + ex.Message); }
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
            catch (Exception ex) { throw new Exception("Lỗi số bàn hoạt động: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  5. Giá vốn tháng gần nhất
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap GROUP BY ma_sp
                )
                SELECT ISNULL(SUM(c.so_luong * ISNULL(g.don_gia_von, 0)), 0)
                FROM chi_tiet_hoa_don_ban c
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb
                LEFT JOIN gia_von_tb g ON g.ma_sp = c.ma_sp
                WHERE MONTH(h.ngay_ban) = @Thang AND YEAR(h.ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch (Exception ex) { throw new Exception("Lỗi giá vốn tháng: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  6. Giá vốn theo ngày (giữ nguyên)
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonTheoNgay(DateTime ngay)
        {
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap GROUP BY ma_sp
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
        //  7. Giá vốn theo tháng (giữ nguyên)
        // ══════════════════════════════════════════════════════════
        public double GetGiaVonTheoThang(int thang, int nam)
        {
            string sql = @"
                WITH gia_von_tb AS (
                    SELECT ma_sp,
                           SUM(so_luong * don_gia_nhap) / NULLIF(SUM(so_luong), 0) AS don_gia_von
                    FROM chi_tiet_hoa_don_nhap GROUP BY ma_sp
                )
                SELECT ISNULL(SUM(c.so_luong * ISNULL(g.don_gia_von, 0)), 0)
                FROM chi_tiet_hoa_don_ban c
                JOIN hoa_don_ban h ON c.ma_hdb = h.ma_hdb
                LEFT JOIN gia_von_tb g ON g.ma_sp = c.ma_sp
                WHERE MONTH(h.ngay_ban) = @Thang AND YEAR(h.ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<double>(sql, new { Thang = thang, Nam = nam });
            }
            catch { return 0; }
        }

        // ══════════════════════════════════════════════════════════
        //  8. Biểu đồ THEO NGÀY
        //  [SỬA] Thêm SUM(tien_bida), SUM(tien_san_pham)
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetDuLieuBieuDoTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            string sql = @"
                SELECT
                    CAST(ngay_ban AS DATE)         AS NgayBan,
                    ISNULL(SUM(tong_tien),      0) AS DoanhThu,
                    ISNULL(SUM(tien_bida),      0) AS TienBida,
                    ISNULL(SUM(tien_san_pham),  0) AS TienSanPham
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
            catch (Exception ex) { throw new Exception("Lỗi biểu đồ ngày: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  9. Biểu đồ THEO THÁNG
        //  [SỬA] Thêm SUM(tien_bida), SUM(tien_san_pham)
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetDuLieuBieuDoTheoThang(int nam)
        {
            string sql = @"
                SELECT
                    MONTH(ngay_ban)                AS Thang,
                    ISNULL(SUM(tong_tien),      0) AS DoanhThu,
                    ISNULL(SUM(tien_bida),      0) AS TienBida,
                    ISNULL(SUM(tien_san_pham),  0) AS TienSanPham
                FROM hoa_don_ban
                WHERE YEAR(ngay_ban) = @Nam
                GROUP BY MONTH(ngay_ban)
                ORDER BY Thang ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<dynamic>(sql, new { Nam = nam }).ToList();
            }
            catch (Exception ex) { throw new Exception("Lỗi biểu đồ tháng: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  10. TienBida + TienSanPham tháng gần nhất (Pie Chart)
        // ══════════════════════════════════════════════════════════
        public (double TienBida, double TienSanPham) GetTienBidaVaTienSanPhamThangHienTai()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT
                    ISNULL(SUM(tien_bida),     0) AS TienBida,
                    ISNULL(SUM(tien_san_pham), 0) AS TienSanPham
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang AND YEAR(ngay_ban) = @Nam";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var row = conn.QueryFirstOrDefault<dynamic>(sql, new { Thang = thang, Nam = nam });
                    if (row == null) return (0, 0);
                    return (Convert.ToDouble(row.TienBida), Convert.ToDouble(row.TienSanPham));
                }
            }
            catch (Exception ex) { throw new Exception("Lỗi tien_bida/tien_san_pham: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  11. Snapshot phát hiện thay đổi
        // ══════════════════════════════════════════════════════════
        public (int SoHoaDon, DateTime NgayMoiNhat, int SoBanHoatDong) GetSnapshotThayDoi()
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sqlHD = @"
                SELECT COUNT(*) AS SoHD,
                       ISNULL(MAX(ngay_ban), '1900-01-01') AS NgayMoiNhat
                FROM hoa_don_ban
                WHERE MONTH(ngay_ban) = @Thang AND YEAR(ngay_ban) = @Nam";
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

        // ══════════════════════════════════════════════════════════
        //  12. [MỚI] Biểu đồ THEO NĂM
        //      Lấy tất cả năm có hóa đơn trong DB
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetDuLieuBieuDoTheoNam()
        {
            string sql = @"
                SELECT
                    YEAR(ngay_ban)                 AS Nam,
                    ISNULL(SUM(tong_tien),      0) AS DoanhThu,
                    ISNULL(SUM(tien_bida),      0) AS TienBida,
                    ISNULL(SUM(tien_san_pham),  0) AS TienSanPham
                FROM hoa_don_ban
                GROUP BY YEAR(ngay_ban)
                ORDER BY Nam ASC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<dynamic>(sql).ToList();
            }
            catch (Exception ex) { throw new Exception("Lỗi biểu đồ năm: " + ex.Message); }
        }

        // ══════════════════════════════════════════════════════════
        //  13. [MỚI] Top máy doanh thu cao nhất tháng gần nhất
        //      Giả sử bảng: phien_choi (ma_may, tien_bida, ngay_bat_dau)
        //      JOIN may_bida (ma_may, ten_may)
        //      ⚠️ Đổi tên bảng/cột cho khớp DB thực tế của bạn
        // ══════════════════════════════════════════════════════════
        public List<dynamic> GetTopMayDoanhThu(int top = 3)
        {
            var (thang, nam) = LayThangNamGanNhat();
            string sql = @"
                SELECT TOP (@Top)
                    mb.ten_may                     AS TenMay,
                    ISNULL(SUM(pc.tien_bida),   0) AS DoanhThu
                FROM phien_choi pc
                JOIN may_bida mb ON pc.ma_may = mb.ma_may
                WHERE MONTH(pc.ngay_bat_dau) = @Thang
                  AND YEAR(pc.ngay_bat_dau)  = @Nam
                  AND pc.trang_thai = 'DA_KET_THUC'
                GROUP BY mb.ten_may
                ORDER BY DoanhThu DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<dynamic>(sql, new { Top = top, Thang = thang, Nam = nam }).ToList();
            }
            catch
            {
                // Trả về list rỗng → UI sẽ không hiển thị card top máy
                return new List<dynamic>();
            }
        }
    }
}