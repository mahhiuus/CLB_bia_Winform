using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Bài_Tập_Lớn.DTO; 

namespace Bài_Tập_Lớn.DAL
{
    internal class SanPhamDAL
    {
        public string SinhMaMoi()
        {
            string ma = "SP01";
            string sql = @"SELECT TOP 1 ma_sp FROM san_pham ORDER BY ma_sp DESC";
            try
            {
                using (SqlConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string maHienTai = dr["ma_sp"].ToString();
                                int soThuTu = int.Parse(maHienTai.Substring(2)) + 1;
                                ma = $"SP{soThuTu:D2}";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi sinh mã sản phẩm: " + ex.Message);
            }
            return ma;
        }

        public bool ThemSanPham(SanPhamDTO sp)
        {
            if (sp == null || string.IsNullOrWhiteSpace(sp.MaSP))
            {
                throw new ArgumentException("Mã sản phẩm không được để trống!");
            }

            string sql = @"INSERT INTO san_pham (ma_sp, ten_sp, loai, gia_ban, so_luong_ton, ma_ncc, hinh_anh) 
                           VALUES (@MaSP, @TenSP, @LoaiSP, @GiaBan, @SoLuongTon, @MaNCC, @HinhAnh)";
            try
            {
                using (SqlConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@MaSP", sp.MaSP);
                        cmd.Parameters.AddWithValue("@TenSP", sp.TenSP);
                        cmd.Parameters.AddWithValue("@LoaiSP", sp.Loai);
                        cmd.Parameters.AddWithValue("@GiaBan", sp.GiaBan);
                        cmd.Parameters.AddWithValue("@SoLuongTon", sp.SoLuongTon);
                        cmd.Parameters.AddWithValue("@MaNCC", sp.MaNCC ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@HinhAnh", sp.HinhAnh ?? (object)DBNull.Value);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        public List<SanPhamDTO> TimKiemTheoMa(string ma)
        {
            List<SanPhamDTO> ds = new List<SanPhamDTO>(); 
            string sql = "SELECT * FROM san_pham WHERE ma_sp = @ma";
            try
            {
                using (SqlConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ma", ma);
                        using (SqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                SanPhamDTO sp = new SanPhamDTO(); 
                                sp.MaSP = rs["ma_sp"].ToString();
                                sp.TenSP = rs["ten_sp"]?.ToString();
                                sp.Loai = rs["loai"]?.ToString();
                                sp.GiaBan = rs["gia_ban"] != DBNull.Value ? Convert.ToInt32(rs["gia_ban"]) : 0;
                                sp.SoLuongTon = rs["so_luong_ton"] != DBNull.Value ? Convert.ToInt32(rs["so_luong_ton"]) : 0;
                                ds.Add(sp);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ds;
        }

        public List<SanPhamDTO> TimKiemTheoTen(string ten)
        {
            List<SanPhamDTO> ds = new List<SanPhamDTO>();
            string sql = "SELECT * FROM san_pham WHERE ten_sp LIKE @ten";
            try
            {
                using (SqlConnection conn = DBConnection.Instance.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ten", "%" + ten + "%");
                        using (SqlDataReader rs = cmd.ExecuteReader())
                        {
                            while (rs.Read())
                            {
                                SanPhamDTO sp = new SanPhamDTO();
                                sp.MaSP = rs["ma_sp"].ToString();
                                sp.TenSP = rs["ten_sp"]?.ToString();
                                sp.Loai = rs["loai"]?.ToString();
                                sp.GiaBan = rs["gia_ban"] != DBNull.Value ? Convert.ToInt32(rs["gia_ban"]) : 0;
                                sp.SoLuongTon = rs["so_luong_ton"] != DBNull.Value ? Convert.ToInt32(rs["so_luong_ton"]) : 0;
                                ds.Add(sp);
                            }
                        }
                    }
                }
            }
            catch { }
            return ds;
        }
    }
}