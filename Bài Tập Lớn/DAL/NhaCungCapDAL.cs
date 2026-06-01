using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
    internal class NhaCungCapDAL
    {
        public string SinhMaMoi()
        {
            string sql = @"SELECT ISNULL(MAX(CAST(SUBSTRING(ma_ncc, 4, LEN(ma_ncc)) AS INT)),0)
                           FROM nha_cung_cap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
                    return $"NCC{soThuTu:D2}";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã nhà cung cấp: " + ex.Message);
            }
        }
        public List<NhaCungCapDTO> LayTatCaNhaCungCap()
        {
            string sql = "SELECT * FROM nha_cung_cap";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Query<NhaCungCapDTO>(sql).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhà cung cấp: " + ex.Message);
            }
        }
        public NhaCungCapDTO TimTheoMaNhaCungCap(string maNCC)
        {
            string sql = "SELECT * FROM nha_cung_cap WHERE ma_ncc = @MaNCC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.QueryFirstOrDefault<NhaCungCapDTO>(
                        sql,
                        new { MaNCC = maNCC }
                    );
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm nhà cung cấp: " + ex.Message);
            }
        }
        public List<NhaCungCapDTO> TimKiem(string keyword)
        {
            string sql = @"SELECT * 
                        FROM nha_cung_cap                   
                        WHERE ma_ncc LIKE @Keyword                   
                           OR ten_cong_ty COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("Keyword", "%" + keyword.Trim() + "%", System.Data.DbType.String);

                    return conn.Query<NhaCungCapDTO>(sql, dynamicParams).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm nhà cung cấp: " + ex.Message);
            }
        }

        public bool ThemNhaCungCap(NhaCungCapDTO ncc)
        {
            string sql = @"INSERT INTO nha_cung_cap
                           (ma_ncc, ten_cong_ty, sdt, dia_chi, email, nguoi_lien_he)
                           VALUES
                           (@MaNCC, @TenCongTy, @Sdt, @DiaChi, @Email, @NguoiLienHe)";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, ncc);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm nhà cung cấp: " + ex.Message);
            }
        }
        public bool CapNhatNhaCungCap(NhaCungCapDTO ncc)
        {
            string sql = @"UPDATE nha_cung_cap SET
                           ten_cong_ty = @TenCongTy,
                           sdt = @Sdt,
                           dia_chi = @DiaChi,
                           email = @Email,
                           nguoi_lien_he = @NguoiLienHe
                           WHERE ma_ncc = @MaNCC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, ncc);
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật nhà cung cấp: " + ex.Message);
            }
        }
        public bool XoaNhaCungCap(string maNCC)
        {
            string sql = "DELETE FROM nha_cung_cap WHERE ma_ncc = @MaNCC";

            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int rows = conn.Execute(sql, new { MaNCC = maNCC });
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa nhà cung cấp: " + ex.Message);
            }
        }
    }
}