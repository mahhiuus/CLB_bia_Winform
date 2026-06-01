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
            string sql = @"SELECT COALESCE(MAX(CAST(SUBSTRING(ma_sp, 3, LEN(ma_sp)) AS INT)), 0) FROM san_pham";
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
                throw new ArgumentException("Mã sản phẩm không được để trống!");

            string sql = @"INSERT INTO san_pham (ma_sp, ten_sp, loai, gia_ban, so_luong_ton, ma_ncc, hinh_anh)
                           VALUES (@MaSP, @TenSP, @Loai, @GiaBan, @SoLuongTon, @MaNCC, @HinhAnh)";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, sp) > 0;
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
                throw new ArgumentException("Mã sản phẩm không hợp lệ để cập nhật!");

            string sql = @"UPDATE san_pham
                           SET ten_sp       = @TenSP,
                               loai         = @Loai,
                               gia_ban      = @GiaBan,
                               so_luong_ton = @SoLuongTon,
                               ma_ncc       = @MaNCC,
                               hinh_anh     = @HinhAnh
                           WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, sp) > 0;
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
                throw new ArgumentException("Mã sản phẩm không được để trống!");

            string sql = "DELETE FROM san_pham WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, new { MaSP = maSP }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }

        public List<SanPhamDTO> TimKiemTheoMa(string ma)
        {
            string sql = @"SELECT ma_sp AS MaSP, ten_sp AS TenSP, loai,
                                  gia_ban AS GiaBan, so_luong_ton AS SoLuongTon,
                                  ma_ncc AS MaNCC, hinh_anh AS HinhAnh
                           FROM san_pham WHERE ma_sp = @Ma";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<SanPhamDTO>(sql, new { Ma = ma }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm theo mã: " + ex.Message);
            }
        }

        public List<SanPhamDTO> TimKiemTheoTen(string ten)
        {
            string sql = @"SELECT ma_sp AS MaSP, ten_sp AS TenSP, loai,
                                  gia_ban AS GiaBan, so_luong_ton AS SoLuongTon,
                                  ma_ncc AS MaNCC, hinh_anh AS HinhAnh
                           FROM san_pham WHERE ten_sp LIKE @Ten";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<SanPhamDTO>(sql, new { Ten = "%" + ten + "%" }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm theo tên: " + ex.Message);
            }
        }

        public List<SanPhamDTO> TimKiem(string keyword)
        {
            string sql = @"SELECT
                               ma_sp        AS MaSP,
                               ten_sp       AS TenSP,
                               loai,
                               gia_ban      AS GiaBan,
                               so_luong_ton AS SoLuongTon,
                               ma_ncc       AS MaNCC,
                               hinh_anh     AS HinhAnh
                           FROM san_pham
                           WHERE ma_sp   LIKE @Keyword
                              OR ten_sp  COLLATE Vietnamese_CI_AI LIKE @Keyword COLLATE Vietnamese_CI_AI";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("Keyword", "%" + (keyword ?? "").Trim() + "%", DbType.String);
                    return conn.Query<SanPhamDTO>(sql, p).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tìm kiếm sản phẩm: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  THÊM MỚI: Tăng tồn kho khi NHẬP HÀNG xác nhận
        //  Gọi sau khi TaoPhieuNhap thành công
        // ══════════════════════════════════════════════════════════
        public bool TangTonKho(string maSP, int soLuong)
        {
            if (string.IsNullOrWhiteSpace(maSP) || soLuong <= 0)
                throw new ArgumentException("Mã SP hoặc số lượng không hợp lệ!");

            string sql = @"UPDATE san_pham
                           SET so_luong_ton = so_luong_ton + @SoLuong
                           WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    return conn.Execute(sql, new { MaSP = maSP, SoLuong = soLuong }) > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tăng tồn kho SP {maSP}: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  THÊM MỚI: Giảm tồn kho khi THANH TOÁN hóa đơn bán
        //  Chỉ gọi khi IsPaid = true, KHÔNG gọi khi chỉ order
        // ══════════════════════════════════════════════════════════
        public bool GiamTonKho(string maSP, int soLuong)
        {
            if (string.IsNullOrWhiteSpace(maSP) || soLuong <= 0)
                throw new ArgumentException("Mã SP hoặc số lượng không hợp lệ!");

            // Kiểm tra tồn kho đủ trước khi trừ
            string sqlCheck = "SELECT so_luong_ton FROM san_pham WHERE ma_sp = @MaSP";
            string sqlUpdate = @"UPDATE san_pham
                                 SET so_luong_ton = so_luong_ton - @SoLuong
                                 WHERE ma_sp = @MaSP AND so_luong_ton >= @SoLuong";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                {
                    int tonHienTai = conn.ExecuteScalar<int>(sqlCheck, new { MaSP = maSP });
                    if (tonHienTai < soLuong)
                        throw new Exception($"Tồn kho SP {maSP} không đủ (còn {tonHienTai}, cần {soLuong})!");

                    return conn.Execute(sqlUpdate, new { MaSP = maSP, SoLuong = soLuong }) > 0;
                }
            }
            catch (Exception ex) when (!ex.Message.Contains("Tồn kho"))
            {
                throw new Exception($"Lỗi khi giảm tồn kho SP {maSP}: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  THÊM MỚI: Lấy tồn kho hiện tại theo mã SP
        // ══════════════════════════════════════════════════════════
        public int LayTonKho(string maSP)
        {
            string sql = "SELECT ISNULL(so_luong_ton, 0) FROM san_pham WHERE ma_sp = @MaSP";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.ExecuteScalar<int>(sql, new { MaSP = maSP });
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy tồn kho: " + ex.Message);
            }
        }


        public List<SanPhamDTO> LayTatCa()
        {
            string sql = @"SELECT ma_sp        AS MaSP,
                          ten_sp       AS TenSP,
                          loai,
                          gia_ban      AS GiaBan,
                          so_luong_ton AS SoLuongTon,
                          ma_ncc       AS MaNCC,
                          hinh_anh     AS HinhAnh
                   FROM san_pham
                   ORDER BY ma_sp";
            try
            {
                using (IDbConnection conn = DBConnection.Instance.GetConnection())
                    return conn.Query<SanPhamDTO>(sql).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }
    }

}