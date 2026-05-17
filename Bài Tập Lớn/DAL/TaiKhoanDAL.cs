using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    public class TaiKhoanDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"
                SELECT ISNULL(
                    MAX(CAST(SUBSTRING(ma_tk, 3, LEN(ma_tk)) AS INT)),
                    0
                )
                FROM tai_khoan";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;

                    return $"TK{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi sinh mã tài khoản: " + ex.Message
                );
            }
        }

        public bool TaoAdminMacDinh()
        {
            string kiemTraSql = @"
                SELECT COUNT(*)
                FROM tai_khoan
                WHERE vai_tro = N'Admin'";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int count = conn.ExecuteScalar<int>(kiemTraSql);

                    if (count == 0)
                    {
                        string insertSql = @"
                            INSERT INTO tai_khoan
                            (
                                ma_tk,
                                ten_dang_nhap,
                                mat_khau,
                                vai_tro,
                                ma_nv
                            )
                            VALUES
                            (
                                'TK01',
                                'admin',
                                'admin123',
                                N'Admin',
                                NULL
                            )";

                        return conn.Execute(insertSql) > 0;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi tạo admin mặc định: " + ex.Message
                );
            }
        }

        public TaiKhoanDTO DangNhap(
            string tenDangNhap,
            string matKhau
        )
        {
            string sql = @"
                SELECT *
                FROM tai_khoan
                WHERE ten_dang_nhap = @TenDangNhap
                  AND mat_khau = @MatKhau";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(
                        sql,
                        new
                        {
                            TenDangNhap = tenDangNhap,
                            MatKhau = matKhau
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi đăng nhập: " + ex.Message
                );
            }
        }

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            string sql = @"
                SELECT *
                FROM tai_khoan
                ORDER BY ma_tk";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<TaiKhoanDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi lấy danh sách tài khoản: " + ex.Message
                );
            }
        }

        public TaiKhoanDTO LayTheoMaTK(string maTK)
        {
            string sql = @"
                SELECT *
                FROM tai_khoan
                WHERE ma_tk = @MaTK";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(
                        sql,
                        new { MaTK = maTK }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi lấy tài khoản theo mã: " + ex.Message
                );
            }
        }

        public TaiKhoanDTO LayTheoTenDangNhap(
            string tenDangNhap
        )
        {
            string sql = @"
                SELECT *
                FROM tai_khoan
                WHERE ten_dang_nhap = @TenDangNhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(
                        sql,
                        new
                        {
                            TenDangNhap = tenDangNhap
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi lấy tài khoản theo tên đăng nhập: " + ex.Message
                );
            }
        }

        public bool KiemTraTenDangNhapTonTai(
            string tenDangNhap
        )
        {
            string sql = @"
                SELECT COUNT(*)
                FROM tai_khoan
                WHERE ten_dang_nhap = @TenDangNhap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int count = conn.ExecuteScalar<int>(
                        sql,
                        new
                        {
                            TenDangNhap = tenDangNhap
                        }
                    );

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi kiểm tra tên đăng nhập: " + ex.Message
                );
            }
        }

        public bool ThemTaiKhoan(TaiKhoanDTO tk)
        {
            string sql = @"
                INSERT INTO tai_khoan
                (
                    ma_tk,
                    ten_dang_nhap,
                    mat_khau,
                    vai_tro,
                    ma_nv
                )
                VALUES
                (
                    @MaTK,
                    @TenDangNhap,
                    @MatKhau,
                    @VaiTro,
                    @MaNV
                )";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, tk);

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi thêm tài khoản: " + ex.Message
                );
            }
        }

        public bool CapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            string sql = @"
                UPDATE tai_khoan
                SET
                    ten_dang_nhap = @TenDangNhap,
                    mat_khau = @MatKhau,
                    vai_tro = @VaiTro,
                    ma_nv = @MaNV
                WHERE ma_tk = @MaTK";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, tk);

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi cập nhật tài khoản: " + ex.Message
                );
            }
        }

        public bool DoiMatKhau(
            string maTK,
            string matKhauMoi
        )
        {
            string sql = @"
                UPDATE tai_khoan
                SET mat_khau = @MatKhauMoi
                WHERE ma_tk = @MaTK";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new
                        {
                            MatKhauMoi = matKhauMoi,
                            MaTK = maTK
                        }
                    );

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi đổi mật khẩu: " + ex.Message
                );
            }
        }

        public bool XoaTaiKhoan(string maTK)
        {
            string sql = @"
                DELETE FROM tai_khoan
                WHERE ma_tk = @MaTK";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new { MaTK = maTK }
                    );

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi xóa tài khoản: " + ex.Message
                );
            }
        }

        public List<TaiKhoanDTO> TimKiem(string keyword)
        {
            string sql = @"
                SELECT *
                FROM tai_khoan
                WHERE ma_tk LIKE @Keyword
                   OR ten_dang_nhap LIKE @Keyword
                   OR vai_tro LIKE @Keyword";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<TaiKhoanDTO>(
                        sql,
                        new
                        {
                            Keyword = "%" + (keyword ?? "") + "%"
                        }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi tìm kiếm tài khoản: " + ex.Message
                );
            }
        }
    }
}