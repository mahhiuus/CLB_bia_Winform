using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class SanPhamDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT COALESCE(MAX(CAST(SUBSTRING(ma_sp FROM 3) AS INT)), 0) FROM san_pham";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"SP{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã sản phẩm: " + ex.Message);
            }
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (sp == null || string.IsNullOrWhiteSpace(sp.MaSP))
            {
                throw new ArgumentException("Mã sản phẩm không được để trống!");
            }

            string sql = @"INSERT INTO san_pham (ma_sp, ten_sp, loai, gia_ban, so_luong_ton, ma_ncc, hinh_anh) 
                           VALUES (@MaSP, @TenSP, @Loai, @GiaBan, @SoLuongTon, @MaNCC, @HinhAnh)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {

                    int rows = conn.Execute(sql, sp);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        public bool CapNhatSanPham(SanPhamDTO sp)
        {
            if (sp == null || string.IsNullOrWhiteSpace(sp.MaSP))
            {
                throw new ArgumentException("Mã sản phẩm không hợp lệ để cập nhật!");
            }

            string sql = @"UPDATE san_pham 
                           SET ten_sp = @TenSP, 
                               loai = @Loai, 
                               gia_ban = @GiaBan, 
                               so_luong_ton = @SoLuongTon, 
                               ma_ncc = @MaNCC, 
                               hinh_anh = @HinhAnh 
                           WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, sp);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật sản phẩm: " + ex.Message);
            }
        }

        public bool XoaSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
            {
                throw new ArgumentException("Mã sản phẩm không được để trống!");
            }

            string sql = "DELETE FROM san_pham WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaSP = maSP });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        public List<SanPhamDTO> TimKiemTheoMa(string ma)
        {
            string sql = "SELECT ma_sp AS MaSP, ten_sp AS TenSP, loai, gia_ban AS GiaBan, so_luong_ton AS SoLuongTon, ma_ncc AS MaNCC, hinh_anh AS HinhAnh FROM san_pham WHERE ma_sp = @Ma";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {

                    return conn.Query<SanPhamDTO>(sql, new { Ma = ma }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm theo mã: " + ex.Message);
            }
        }


        public List<SanPhamDTO> TimKiemTheoTen(string ten)
        {
            string sql = "SELECT ma_sp AS MaSP, ten_sp AS TenSP, loai, gia_ban AS GiaBan, so_luong_ton AS SoLuongTon, ma_ncc AS MaNCC, hinh_anh AS HinhAnh FROM san_pham WHERE ten_sp LIKE @Ten";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<SanPhamDTO>(sql, new { Ten = "%" + ten + "%" }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm theo tên: " + ex.Message);
            }
        }
        public List<SanPhamDTO> TimKiem(string keyword)
        {
            string sql = @"SELECT 
            ma_sp AS MaSP, 
            ten_sp AS TenSP, 
            loai, 
            gia_ban AS GiaBan, 
            so_luong_ton AS SoLuongTon, 
            ma_ncc AS MaNCC, 
            hinh_anh AS HinhAnh 
        FROM san_pham 
        WHERE ma_sp LIKE @Keyword 
           OR ten_sp COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + (keyword ?? "").Trim() + "%", System.Data.DbType.String);

                    return conn.Query<SanPhamDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }
    }
}