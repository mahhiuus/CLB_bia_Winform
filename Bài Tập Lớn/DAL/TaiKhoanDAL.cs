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
            // Chuyển LIMIT 1 của MySQL thành TOP 1 của SQL Server
            string sql = "SELECT TOP 1 ma_tk FROM tai_khoan ORDER BY ma_tk DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    string maCuoi = conn.QueryFirstOrDefault<string>(sql);
                    if (!string.IsNullOrEmpty(maCuoi))
                    {
                        int soThuTu = int.Parse(maCuoi.Substring(2)) + 1;
                        return string.Format("TK{0:D2}", soThuTu);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi sinh mã mới tài khoản: " + e.Message, e);
            }
            return "TK01";
        }

        public void TaoAdminMacDinh()
        {
            string kiemTraSql = "SELECT COUNT(*) FROM tai_khoan WHERE vai_tro = 'ADMIN'";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int count = conn.ExecuteScalar<int>(kiemTraSql);
                    if (count == 0)
                    {
                        string insertSql = "INSERT INTO tai_khoan (ma_tk, ten_dang_nhap, mat_khau, vai_tro, ma_nv) " +
                                           "VALUES ('TK001', 'admin', 'admin123', 'ADMIN', 'NV001')";
                        conn.Execute(insertSql);
                        Console.WriteLine("Tạo thành công ADMIN mặc định (Tên đăng nhập: admin / Password: admin123)!");
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tạo tài khoản ADMIN mặc định: " + e.Message, e);
            }
        }

        public void ThemTaiKhoan(TaiKhoanDTO tk)
        {
            string sql = "INSERT INTO tai_khoan (ma_tk, ten_dang_nhap, mat_khau, vai_tro, ma_nv) VALUES (@MaTK, @TenDangNhap, @MatKhau, @VaiTro, @MaNV)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, tk);
                    Console.WriteLine("Thêm tài khoản thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi thêm tài khoản: " + e.Message, e);
            }
        }

        public void XoaTaiKhoanTheoMaTK(string maTK)
        {
            string sql = "DELETE FROM tai_khoan WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { MaTK = maTK });
                    Console.WriteLine("Xóa tài khoản thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi xóa tài khoản: " + e.Message, e);
            }
        }

        public void DoiMatKhau(string maTK, string matKhauMoi)
        {
            string sql = "UPDATE tai_khoan SET mat_khau = @MatKhauMoi WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { MatKhauMoi = matKhauMoi, MaTK = maTK });
                    Console.WriteLine("Đổi mật khẩu thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi đổi mật khẩu: " + e.Message, e);
            }
        }

        public void DoiTenDangNhap(string maTK, string tenDangNhapMoi)
        {
            string sql = "UPDATE tai_khoan SET ten_dang_nhap = @TenDangNhapMoi WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { TenDangNhapMoi = tenDangNhapMoi, MaTK = maTK });
                    Console.WriteLine("Đổi tên đăng nhập thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi đổi tên đăng nhập: " + e.Message, e);
            }
        }

        public void CapNhatVaiTro(string maTK, string vaiTro)
        {
            string sql = "UPDATE tai_khoan SET vai_tro = @VaiTro WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { VaiTro = vaiTro, MaTK = maTK });
                    Console.WriteLine("Cập nhật vai trò thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi cập nhật vai trò: " + e.Message, e);
            }
        }

        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            string sql = "SELECT * FROM tai_khoan WHERE ten_dang_nhap = @TenDangNhap AND mat_khau = @MatKhau";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(sql, new { TenDangNhap = tenDangNhap, MatKhau = matKhau });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi chương trình thực hiện đăng nhập: " + e.Message, e);
            }
        }

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            string sql = "SELECT * FROM tai_khoan";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<TaiKhoanDTO>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách tài khoản: " + e.Message, e);
            }
        }

        public TaiKhoanDTO LayTheoMaTK(string maTK)
        {
            string sql = "SELECT * FROM tai_khoan WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(sql, new { MaTK = maTK });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi chương trình lấy theo mã tài khoản: " + e.Message, e);
            }
        }

        public TaiKhoanDTO LayTheoTenDangNhap(string tenDangNhap)
        {
            string sql = "SELECT * FROM tai_khoan WHERE ten_dang_nhap = @TenDangNhap";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<TaiKhoanDTO>(sql, new { TenDangNhap = tenDangNhap });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi chương trình lấy theo tên đăng nhập: " + e.Message, e);
            }
        }

        public bool KiemTraTenDangNhapTonTai(string tenDangNhap)
        {
            string sql = "SELECT COUNT(*) FROM tai_khoan WHERE ten_dang_nhap = @TenDangNhap";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int count = conn.ExecuteScalar<int>(sql, new { TenDangNhap = tenDangNhap });
                    return count > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DatLaiMatKhau(string tenDangNhap, string matKhauMoi)
        {
            string sql = "UPDATE tai_khoan SET mat_khau = @MatKhauMoi WHERE ten_dang_nhap = @TenDangNhap";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, new { MatKhauMoi = matKhauMoi, TenDangNhap = tenDangNhap }) > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void CapNhatToanBoTaiKhoan(TaiKhoanDTO tk)
        {
            string sql = "UPDATE tai_khoan SET ten_dang_nhap = @TenDangNhap, mat_khau = @MatKhau, vai_tro = @VaiTro, ma_nv = @MaNV WHERE ma_tk = @MaTK";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, tk);
                    Console.WriteLine("Cập nhật toàn bộ tài khoản thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi cập nhật toàn bộ tài khoản: " + e.Message, e);
            }
        }
    }
}