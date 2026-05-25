using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class KhachHangDAL
    {
        // Sinh mã khách hàng mới
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_kh, 3, LEN(ma_kh)) AS INT)),0) 
                           FROM khach_hang";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"KH{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã khách hàng: " + ex.Message);
            }
        }

        // Lấy tất cả khách hàng
        public List<KhachHangDTO> LayTatCaKhachHang()
        {
            string sql = "SELECT * FROM khach_hang";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<KhachHangDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách khách hàng: " + ex.Message);
            }
        }

        // Tìm theo mã khách hàng
        public KhachHangDTO TimTheoMaKhachHang(string maKH)
        {
            string sql = "SELECT * FROM khach_hang WHERE ma_kh = @MaKH";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<KhachHangDTO>(
                        sql,
                        new { MaKH = maKH }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm khách hàng: " + ex.Message);
            }
        }

        // Tìm kiếm khách hàng
        public List<KhachHangDTO> TimKiem(string keyword)
        {
            string sql = @"SELECT * 
                           FROM khach_hang                   
                           WHERE ma_kh LIKE @Keyword                   
                           OR ho_ten COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + keyword.Trim() + "%", System.Data.DbType.String);

                    return conn.Query<KhachHangDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm khách hàng: " + ex.Message);
            }
        }

        // Thêm khách hàng
        public bool ThemKhachHang(KhachHangDTO kh)
        {
            string sql = @"INSERT INTO khach_hang
                           (ma_kh, ho_ten, sdt, dia_chi, diem_tich_luy, ngay_dang_ky)
                           VALUES
                           (@MaKH, @HoTen, @Sdt, @DiaChi, @DiemTichLuy, @NgayDangKy)";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, kh);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm khách hàng: " + ex.Message);
            }
        }

        // Cập nhật khách hàng
        public bool CapNhatKhachHang(KhachHangDTO kh)
        {
            string sql = @"UPDATE khach_hang SET
                           ho_ten = @HoTen,
                           sdt = @Sdt,
                           dia_chi = @DiaChi,
                           diem_tich_luy = @DiemTichLuy,
                           ngay_dang_ky = @NgayDangKy
                           WHERE ma_kh = @MaKH";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, kh);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật khách hàng: " + ex.Message);
            }
        }

        // Xóa khách hàng
        public bool XoaKhachHang(string maKH)
        {
            string sql = "DELETE FROM khach_hang WHERE ma_kh = @MaKH";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaKH = maKH });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa khách hàng: " + ex.Message);
            }
        }
    }
}