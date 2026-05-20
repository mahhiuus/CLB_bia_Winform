using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class PhienChoiDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_phien, 2, LEN(ma_phien)) AS INT)),0)
                           FROM phien_choi";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;

                    return $"P{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã phiên chơi: " + ex.Message);
            }
        }

        public List<PhienChoiDTO> LayTatCaPhien()
        {
            string sql = "SELECT * FROM phien_choi";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<PhienChoiDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách phiên chơi: " + ex.Message);
            }
        }

        public PhienChoiDTO TimTheoMaPhien(string maPhien)
        {
            string sql = "SELECT * FROM phien_choi WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<PhienChoiDTO>(
                        sql,
                        new { MaPhien = maPhien }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm phiên chơi: " + ex.Message);
            }
        }

        public List<PhienChoiDTO> LayPhienTheoBan(string maBan)
        {
            string sql = @"SELECT * FROM phien_choi
                           WHERE ma_ban = @MaBan";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<PhienChoiDTO>(
                        sql,
                        new { MaBan = maBan }
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy phiên theo bàn: " + ex.Message);
            }
        }

        public PhienChoiDTO TimPhienDangChoiTheoBan(string maBan)
        {
            string sql = @"
                SELECT TOP 1 *
                FROM phien_choi
                WHERE ma_ban = @MaBan
                AND trang_thai = 'DANG_CHOI'
                ORDER BY thoi_gian_bat_dau DESC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<PhienChoiDTO>(
                        sql,
                        new { MaBan = maBan }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm phiên đang chơi: " + ex.Message);
            }
        }

        public bool ThemPhien(PhienChoiDTO phien)
        {
            string sql = @"
                INSERT INTO phien_choi
                (
                    ma_phien,
                    ma_ban,
                    thoi_gian_bat_dau,
                    thoi_gian_ket_thuc,
                    trang_thai
                )
                VALUES
                (
                    @MaPhien,
                    @MaBan,
                    @ThoiGianBatDau,
                    @ThoiGianKetThuc,
                    @TrangThaiPhien
                )";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, phien);

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm phiên chơi: " + ex.Message);
            }
        }

        public bool CapNhatPhien(PhienChoiDTO phien)
        {
            string sql = @"
                UPDATE phien_choi
                SET
                    ma_ban = @MaBan,
                    thoi_gian_bat_dau = @ThoiGianBatDau,
                    thoi_gian_ket_thuc = @ThoiGianKetThuc,
                    trang_thai = @TrangThaiPhien
                WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, phien);

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật phiên chơi: " + ex.Message);
            }
        }

        public bool KetThucPhien(string maPhien, DateTime thoiGianKetThuc)
        {
            string sql = @"
                UPDATE phien_choi
                SET
                    thoi_gian_ket_thuc = @ThoiGianKetThuc,
                    trang_thai = 'DA_KET_THUC'
                WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new
                        {
                            MaPhien = maPhien,
                            ThoiGianKetThuc = thoiGianKetThuc
                        });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi kết thúc phiên chơi: " + ex.Message);
            }
        }

        public bool XoaPhien(string maPhien)
        {
            string sql = "DELETE FROM phien_choi WHERE ma_phien = @MaPhien";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(
                        sql,
                        new { MaPhien = maPhien });

                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa phiên chơi: " + ex.Message);
            }
        }
    }
}