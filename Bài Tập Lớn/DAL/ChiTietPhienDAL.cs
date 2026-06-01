using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class ChiTietPhienDAL
    {
        static ChiTietPhienDAL()
        {
            Dapper.SqlMapper.SetTypeMap(typeof(Bài_Tập_Lớn.DTO.ChiTietPhienDTO),
                new Dapper.CustomPropertyTypeMap(
                    typeof(Bài_Tập_Lớn.DTO.ChiTietPhienDTO),
                    (type, columnName) => type.GetProperties().FirstOrDefault(p =>
                        string.Equals(p.Name, columnName.Replace("_", ""), StringComparison.OrdinalIgnoreCase)
                        || (columnName == "ma_ctp" && p.Name == "MaCTP")
                        || (columnName == "ma_sp" && p.Name == "MaSP")
                        || (columnName == "ma_phien" && p.Name == "MaPhien")
                        || (columnName == "so_luong" && p.Name == "SoLuong")
                        || (columnName == "don_gia" && p.Name == "DonGia")
                    )
                )
            );
        }
      public  string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_ctp, 4, LEN(ma_ctp)) AS INT)), 0)
                   FROM chi_tiet_phien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"CTP{soThuTu:D3}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã chi tiết phiên: " + ex.Message);
            }
        }

        public List<ChiTietPhienDTO> LayTatCaChiTietPhien()
        {
            string sql = "SELECT * FROM chi_tiet_phien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietPhienDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách chi tiết phiên: " + ex.Message);
            }
        }

        public List<ChiTietPhienDTO> TimTheoMaPhien(string maPhien)
        {
            string sql = @"SELECT * FROM chi_tiet_phien
                           WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<ChiTietPhienDTO>(
                        sql,
                        new { MaPhien = maPhien }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm chi tiết phiên: " + ex.Message);
            }
        }

        public double TinhTongTienTheoPhien(string maPhien)
        {
            string sql = @"
                SELECT ISNULL(SUM(so_luong * don_gia),0)
                FROM chi_tiet_phien
                WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.ExecuteScalar<double>(
                        sql,
                        new { MaPhien = maPhien }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng tiền: " + ex.Message);
            }
        }

        public bool ThemChiTietPhien(ChiTietPhienDTO ct)
        {
            string sql = @"
                INSERT INTO chi_tiet_phien
                (
                    ma_ctp,
                    ma_phien,
                    ma_sp,
                    so_luong,
                    don_gia
                )
                VALUES
                (
                    @MaCTP,
                    @MaPhien,
                    @MaSP,
                    @SoLuong,
                    @DonGia
                )";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        ct.MaCTP,
                        ct.MaPhien,
                        ct.MaSP,
                        ct.SoLuong,
                        ct.DonGia
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm chi tiết phiên: " + ex.Message);
            }
        }

        public bool CapNhatChiTietPhien(ChiTietPhienDTO ct)
        {
            string sql = @"
                UPDATE chi_tiet_phien
                SET
                    ma_phien = @MaPhien,
                    ma_sp = @MaSP,
                    so_luong = @SoLuong,
                    don_gia = @DonGia
                WHERE ma_ctp = @MaCTP";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new
                    {
                        ct.MaCTP,
                        ct.MaPhien,
                        ct.MaSP,
                        ct.SoLuong,
                        ct.DonGia
                    });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật chi tiết phiên: " + ex.Message);
            }
        }

        public bool XoaChiTietPhien(string maChiTiet)
        {
            string sql = @"DELETE FROM chi_tiet_phien
                           WHERE ma_ctp = @MaChiTiet";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new { MaChiTiet = maChiTiet });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa chi tiết phiên: " + ex.Message);
            }
        }
    }
}