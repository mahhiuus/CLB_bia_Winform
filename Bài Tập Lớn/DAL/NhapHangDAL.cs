using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.DAL
{
	internal class NhapHangDAL
	{
		// ══════════════════════════════════════════════════════════
		//  Sinh mã mới
		// ══════════════════════════════════════════════════════════
		public string SinhMaHDNMoi()
		{
			string sql = @"SELECT COALESCE(MAX(CAST(SUBSTRING(ma_hdn, 4, LEN(ma_hdn)) AS INT)), 0) FROM hoa_don_nhap";
			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
					return $"HDN{soThuTu:D3}";
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi sinh mã hóa đơn nhập: " + ex.Message);
			}
		}

		public string SinhMaCTHDNMoi()
		{
			string sql = @"SELECT COALESCE(MAX(CAST(SUBSTRING(ma_cthdn, 5, LEN(ma_cthdn)) AS INT)), 0) FROM chi_tiet_hoa_don_nhap";
			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					int soThuTu = conn.ExecuteScalar<int>(sql) + 1;
					return $"CTHN{soThuTu:D3}";
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi sinh mã chi tiết hóa đơn nhập: " + ex.Message);
			}
		}

		// ══════════════════════════════════════════════════════════
		//  Thêm hóa đơn nhập + chi tiết (Transaction)
		// ══════════════════════════════════════════════════════════
		public bool ThemPhieuNhap(HoaDonNhapDTO hdn, List<ChiTietHoaDonNhapDTO> dsChiTiet)
		{
			if (hdn == null) throw new ArgumentNullException(nameof(hdn));
			if (dsChiTiet == null || dsChiTiet.Count == 0)
				throw new ArgumentException("Phiếu nhập phải có ít nhất 1 sản phẩm!");

			string sqlHDN = @"INSERT INTO hoa_don_nhap (ma_hdn, ma_ncc, ma_nv, ngay_nhap, tong_tien, ghi_chu)
                               VALUES (@MaHDN, @MaNCC, @MaNV, @NgayNhap, @TongTien, @GhiChu)";

			string sqlCT = @"INSERT INTO chi_tiet_hoa_don_nhap (ma_cthdn, ma_hdn, ma_sp, so_luong, don_gia_nhap)
                              VALUES (@MaCTHDN, @MaHDN, @MaSP, @SoLuong, @DonGiaNhap)";

			string sqlCapNhatTon = @"UPDATE san_pham 
                                     SET so_luong_ton = so_luong_ton + @SoLuong 
                                     WHERE ma_sp = @MaSP";

			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					if (conn.State != ConnectionState.Open)
						conn.Open();

					using (IDbTransaction tx = conn.BeginTransaction())
					{
						try
						{
							// 1. Insert hóa đơn nhập
							conn.Execute(sqlHDN, hdn, tx);

							// 2. Insert từng chi tiết + cập nhật tồn kho
							foreach (var ct in dsChiTiet)
							{
								conn.Execute(sqlCT, ct, tx);
								conn.Execute(sqlCapNhatTon, new { ct.SoLuong, ct.MaSP }, tx);
							}

							tx.Commit();
							return true;
						}
						catch
						{
							tx.Rollback();
							throw;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi thêm phiếu nhập hàng: " + ex.Message);
			}
		}

		// ══════════════════════════════════════════════════════════
		//  Lấy danh sách hóa đơn nhập
		// ══════════════════════════════════════════════════════════
		public List<HoaDonNhapDTO> LayTatCa()
		{
			string sql = @"SELECT ma_hdn AS MaHDN, ma_ncc AS MaNCC, ma_nv AS MaNV,
                                  ngay_nhap AS NgayNhap, tong_tien AS TongTien, ghi_chu AS GhiChu
                           FROM hoa_don_nhap
                           ORDER BY ngay_nhap DESC";
			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					return conn.Query<HoaDonNhapDTO>(sql).ToList();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi tải danh sách phiếu nhập: " + ex.Message);
			}
		}

		public List<HoaDonNhapDTO> TimKiem(string keyword)
		{
			string sql = @"SELECT ma_hdn AS MaHDN, ma_ncc AS MaNCC, ma_nv AS MaNV,
                                  ngay_nhap AS NgayNhap, tong_tien AS TongTien, ghi_chu AS GhiChu
                           FROM hoa_don_nhap
                           WHERE ma_hdn LIKE @Keyword OR ma_ncc LIKE @Keyword OR ma_nv LIKE @Keyword
                           ORDER BY ngay_nhap DESC";
			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					return conn.Query<HoaDonNhapDTO>(sql, new { Keyword = "%" + (keyword ?? "").Trim() + "%" }).ToList();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi tìm kiếm phiếu nhập: " + ex.Message);
			}
		}

		// ══════════════════════════════════════════════════════════
		//  Lấy chi tiết theo mã hóa đơn nhập
		// ══════════════════════════════════════════════════════════
		public List<ChiTietHoaDonNhapDTO> LayChiTietTheoMaHDN(string maHDN)
		{
			string sql = @"SELECT ma_cthdn AS MaCTHDN, ma_hdn AS MaHDN, ma_sp AS MaSP,
                                  so_luong AS SoLuong, don_gia_nhap AS DonGiaNhap
                           FROM chi_tiet_hoa_don_nhap
                           WHERE ma_hdn = @MaHDN";
			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					return conn.Query<ChiTietHoaDonNhapDTO>(sql, new { MaHDN = maHDN }).ToList();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi tải chi tiết phiếu nhập: " + ex.Message);
			}
		}

		// ══════════════════════════════════════════════════════════
		//  Xóa hóa đơn nhập
		// ══════════════════════════════════════════════════════════
		public bool XoaPhieuNhap(string maHDN)
		{
			if (string.IsNullOrWhiteSpace(maHDN))
				throw new ArgumentException("Mã hóa đơn nhập không được để trống!");

			string sqlCT = "DELETE FROM chi_tiet_hoa_don_nhap WHERE ma_hdn = @MaHDN";
			string sqlHDN = "DELETE FROM hoa_don_nhap WHERE ma_hdn = @MaHDN";

			try
			{
				using (IDbConnection conn = DBConnection.Instance.GetConnection())
				{
					if (conn.State != ConnectionState.Open)
						conn.Open();

					using (IDbTransaction tx = conn.BeginTransaction())
					{
						try
						{
							conn.Execute(sqlCT, new { MaHDN = maHDN }, tx);
							int rows = conn.Execute(sqlHDN, new { MaHDN = maHDN }, tx);
							tx.Commit();
							return rows > 0;
						}
						catch
						{
							tx.Rollback();
							throw;
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Lỗi khi xóa phiếu nhập: " + ex.Message);
			}
		}
	}
}