using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    public class NhanVienDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_nv, 3, LEN(ma_nv)) AS INT)), 0) 
                   FROM tai_khoan 
                   WHERE ma_nv LIKE 'NV%'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"NV{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã nhân viên: " + ex.Message);
            }
        }

        public List<NhanVienDTO> LayTatCaNhanVien()
        {
            string sql = "SELECT ma_nv AS MaNV, ho_ten AS HoTen, sdt AS Sdt, gioi_tinh AS GioiTinh, chuc_vu AS ChucVu, ngay_sinh AS NgaySinh FROM nhan_vien";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<NhanVienDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhân viên: " + ex.Message);
            }
        }

        public NhanVienDTO TimTheoMaNhanVien(string maNV)
        {
            string sql = "SELECT ma_nv AS MaNV, ho_ten AS HoTen, sdt AS Sdt, gioi_tinh AS GioiTinh, chuc_vu AS ChucVu, ngay_sinh AS NgaySinh FROM nhan_vien WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<NhanVienDTO>(sql, new { MaNV = maNV });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm nhân viên: " + ex.Message);
            }
        }

        public List<NhanVienDTO> TimKiem(string keyword)
        {
            string sql = @"SELECT 
                           ma_nv AS MaNV, 
                           ho_ten AS HoTen, 
                           sdt AS Sdt, 
                           gioi_tinh AS GioiTinh, 
                           chuc_vu AS ChucVu, 
                           ngay_sinh AS NgaySinh                   
                        FROM nhan_vien                   
                        WHERE ma_nv LIKE @Keyword 
                        OR ho_ten COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + keyword.Trim() + "%", System.Data.DbType.String);

                    return conn.Query<NhanVienDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm nhân viên: " + ex.Message);
            }
        }

        public bool ThemNhanVien(NhanVienDTO nv)
        {
            string sql = @"INSERT INTO nhan_vien (ma_nv, ho_ten, sdt, gioi_tinh, chuc_vu, ngay_sinh)
                           VALUES (@MaNV, @HoTen, @Sdt, @GioiTinh, @ChucVu, @NgaySinh)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, nv) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhân viên: " + ex.Message);
            }
        }

        public bool CapNhatNhanVien(NhanVienDTO nv)
        {
            string sql = @"UPDATE nhan_vien SET
                           ho_ten   = @HoTen,
                           sdt      = @Sdt,
                           gioi_tinh = @GioiTinh,
                           chuc_vu  = @ChucVu,
                           ngay_sinh = @NgaySinh
                           WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, nv) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhân viên: " + ex.Message);
            }
        }

        public bool XoaNhanVien(string maNV)
        {
            string sql = "DELETE FROM nhan_vien WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, new { MaNV = maNV }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhân viên: " + ex.Message);
            }
        }
    }
}