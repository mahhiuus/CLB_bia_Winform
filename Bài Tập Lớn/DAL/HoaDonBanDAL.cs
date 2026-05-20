using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class HoaDonBanDAL
    {
        public string SinhMaMoi()
        {
            // Tối ưu bằng SQL: Lấy số lớn nhất sau chữ 'HDB', nếu chưa có thì trả về 0
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_hdb, 4, LEN(ma_hdb)) AS INT)), 0) FROM hoa_don_ban WHERE ma_hdb LIKE 'HDB%'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"HDB{soThuTu:D3}"; // Định dạng 3 chữ số ví dụ: HDB001
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã hóa đơn bán: " + ex.Message);
            }
        }

        public bool Them(HoaDonBanDTO hdb)
        {
            string sql = @"INSERT INTO hoa_don_ban (ma_hdb, ma_phien, ma_kh, ma_nv, ngay_ban, tien_bida, tien_san_pham, tong_tien, ghi_chu) 
                           VALUES (@MaHDB, @MaPhien, @MaKH, @MaNV, @NgayBan, @TienBida, @TienSanPham, @TongTien, @GhiChu)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, hdb);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm hóa đơn bán: " + ex.Message);
            }
        }

        public bool CapNhat(HoaDonBanDTO hdb)
        {
            string sql = @"UPDATE hoa_don_ban SET 
                            tien_bida = @TienBida, 
                            tien_san_pham = @TienSanPham, 
                            tong_tien = @TongTien, 
                            ma_kh = @MaKH, 
                            ma_nv = @MaNV 
                           WHERE ma_hdb = @MaHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, hdb);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hóa đơn bán: " + ex.Message);
            }
        }

        public bool Xoa(string maHDB)
        {
            string sql = "DELETE FROM hoa_don_ban WHERE ma_hdb = @MaHDB";
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
                throw new Exception("Lỗi khi xóa hóa đơn bán: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTatCa()
        {
            string sql = "SELECT * FROM hoa_don_ban ORDER BY ma_hdb DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách hóa đơn bán: " + ex.Message);
            }
        }

        public HoaDonBanDTO LayTheoMa(string maHDB)
        {
            string sql = "SELECT * FROM hoa_don_ban WHERE ma_hdb = @MaHDB";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<HoaDonBanDTO>(sql, new { MaHDB = maHDB });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo mã: " + ex.Message);
            }
        }

        public HoaDonBanDTO LayTheoMaPhien(string maPhien)
        {
            string sql = "SELECT * FROM hoa_don_ban WHERE ma_phien = @MaPhien";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<HoaDonBanDTO>(sql, new { MaPhien = maPhien });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo mã phiên: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTheoNgay(DateTime tuNgay, DateTime denNgay)
        {
            string sql = "SELECT * FROM hoa_don_ban WHERE ngay_ban BETWEEN @TuNgay AND @DenNgay ORDER BY ma_hdb DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { TuNgay = tuNgay, DenNgay = denNgay }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo khoảng ngày: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTheoKhachHang(string maKH)
        {
            string sql = "SELECT * FROM hoa_don_ban WHERE ma_kh = @MaKH ORDER BY ma_hdb DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { MaKH = maKH }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo mã khách hàng: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTheoNhanVien(string maNV)
        {
            string sql = "SELECT * FROM hoa_don_ban WHERE ma_nv = @MaNV ORDER BY ma_hdb DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { MaNV = maNV }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy hóa đơn theo mã nhân viên: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTopHoaDon(int limit)
        {
            string sql = "SELECT TOP (@Limit) * FROM hoa_don_ban ORDER BY tong_tien DESC"; // Dùng TOP thay vì LIMIT (vì cú pháp SQL Server giống hàm sinh mã của bạn)
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy top hóa đơn: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTopHoaDonTheoNgay(DateTime ngay, int limit)
        {
            string sql = "SELECT TOP (@Limit) * FROM hoa_don_ban WHERE ngay_ban = @Ngay ORDER BY tong_tien DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { Ngay = ngay, Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy top hóa đơn theo ngày: " + ex.Message);
            }
        }

        public List<HoaDonBanDTO> LayTopHoaDonTheoThang(int thang, int nam, int limit)
        {
            // Đổi MONTH() và YEAR() tương thích với SQL Server
            string sql = "SELECT TOP (@Limit) * FROM hoa_don_ban WHERE MONTH(ngay_ban) = @Thang AND YEAR(ngay_ban) = @Nam ORDER BY tong_tien DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<HoaDonBanDTO>(sql, new { Thang = thang, Nam = nam, Limit = limit }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy top hóa đơn theo tháng: " + ex.Message);
            }
        }
    }
}