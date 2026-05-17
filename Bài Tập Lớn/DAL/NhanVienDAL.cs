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
            string sql = "SELECT TOP 1 ma_nv FROM nhan_vien ORDER BY ma_nv DESC";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    string maCuoi = conn.QueryFirstOrDefault<string>(sql);
                    if (!string.IsNullOrEmpty(maCuoi))
                    {
                        int soThuTu = int.Parse(maCuoi.Substring(2)) + 1;
                        return string.Format("NV{0:D2}", soThuTu);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi sinh mã mới nhân viên: " + e.Message, e);
            }
            return "NV01";
        }

        public void ThemNhanVien(NhanVienDTO nv)
        {
            string sql = "INSERT INTO nhan_vien (ma_nv, ho_ten, sdt, gioi_tinh, chuc_vu, ngay_sinh) " +
                         "VALUES (@MaNV, @HoTen, @Sdt, @GioiTinh, @ChucVu, @NgaySinh)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, nv);
                    Console.WriteLine("Thêm nhân viên thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi thêm nhân viên: " + e.Message, e);
            }
        }

        public void XoaNhanVien(string maNV)
        {
            string sql = "DELETE FROM nhan_vien WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, new { MaNV = maNV });
                    Console.WriteLine("Xóa nhân viên thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi xóa nhân viên: " + e.Message, e);
            }
        }

        public void CapNhatNhanVien(NhanVienDTO nv)
        {
            string sql = "UPDATE nhan_vien SET ho_ten = @HoTen, sdt = @Sdt, gioi_tinh = @GioiTinh, " +
                         "chuc_vu = @ChucVu, ngay_sinh = @NgaySinh WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Execute(sql, nv);
                    Console.WriteLine("Cập nhật nhân viên thành công!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi cập nhật nhân viên: " + e.Message, e);
            }
        }

        public List<NhanVienDTO> LayTatCaNhanVien()
        {
            string sql = "SELECT * FROM nhan_vien";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<NhanVienDTO>(sql).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi lấy danh sách nhân viên: " + e.Message, e);
            }
        }

        public NhanVienDTO TimTheoMaNhanVien(string maNV)
        {
            string sql = "SELECT * FROM nhan_vien WHERE ma_nv = @MaNV";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<NhanVienDTO>(sql, new { MaNV = maNV });
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm theo mã nhân viên: " + e.Message, e);
            }
        }

        public List<NhanVienDTO> TimKiem(string keyword)
        {
            string sql = "SELECT * FROM nhan_vien WHERE ma_nv LIKE @Keyword OR ho_ten LIKE @Keyword";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    string likeKeyword = "%" + (keyword ?? "") + "%";
                    return conn.Query<NhanVienDTO>(sql, new { Keyword = likeKeyword }).ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Lỗi khi tìm kiếm nhân viên: " + e.Message, e);
            }
        }
    }
}