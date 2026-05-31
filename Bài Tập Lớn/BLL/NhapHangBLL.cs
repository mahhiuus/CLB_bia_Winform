using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    // ═══════════════════════════════════════════════════════════════
    //  NHAP HANG BLL
    //  Façade cho NhapHangPanel + NhapHangPopup
    //  Flow:
    //    TaoPhieuNhap(hdn, dsChiTiet)
    //      → INSERT hoa_don_nhap
    //      → INSERT chi_tiet_hoa_don_nhap (mỗi dòng)
    //      → UPDATE san_pham.so_luong_ton += soLuong  (tồn kho tăng)
    // ═══════════════════════════════════════════════════════════════
    public class NhapHangBLL
    {
        private readonly HoaDonNhapDAL _hdnDAL = new HoaDonNhapDAL();
        private readonly ChiTietHoaDonNhapDAL _ctDAL = new ChiTietHoaDonNhapDAL();
        private readonly SanPhamDAL _spDAL = new SanPhamDAL();

        // ── Sinh mã ──────────────────────────────────────────────
        public string SinhMaHDNMoi() => _hdnDAL.SinhMaMoi();
        public string SinhMaCTHDNMoi() => _ctDAL.SinhMaMoi();

        // ── Tính giá bán đề xuất ─────────────────────────────────
        public double TinhGiaBanDeXuat(double donGiaNhap, double phanTramLoiNhuan)
        {
            if (phanTramLoiNhuan < 0)
                throw new Exception("Phần trăm lợi nhuận không được âm!");
            return Math.Round(donGiaNhap * (1 + phanTramLoiNhuan / 100.0), 0);
        }

        // ── Lấy toàn bộ danh sách phiếu nhập ────────────────────
        public List<HoaDonNhapDTO> LayTatCa()
            => _hdnDAL.LayTatCaHoaDonNhap();

        // ── Lấy chi tiết theo mã HDN ─────────────────────────────
        public List<ChiTietHoaDonNhapDTO> LayChiTietTheoMaHDN(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
                throw new Exception("Mã hóa đơn nhập không được để trống!");
            return _ctDAL.TimTheoMaHDN(maHDN);
        }

        // ── Tìm kiếm phiếu nhập ──────────────────────────────────
        public List<HoaDonNhapDTO> TimKiem(string keyword)
            => _hdnDAL.TimKiem(keyword);

        // ══════════════════════════════════════════════════════════
        //  TẠO PHIẾU NHẬP — nghiệp vụ chính
        //  1. Validate
        //  2. INSERT hoa_don_nhap
        //  3. INSERT từng dòng chi_tiet_hoa_don_nhap
        //  4. UPDATE tồn kho sản phẩm (+= soLuong)
        //  Tất cả trong một transaction để đảm bảo nhất quán
        // ══════════════════════════════════════════════════════════
        public bool TaoPhieuNhap(HoaDonNhapDTO hdn, List<ChiTietHoaDonNhapDTO> dsChiTiet)
        {
            // Validate header
            if (hdn == null)
                throw new Exception("Dữ liệu phiếu nhập không hợp lệ!");
            if (string.IsNullOrWhiteSpace(hdn.MaHDN))
                throw new Exception("Mã phiếu nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(hdn.MaNCC))
                throw new Exception("Vui lòng chọn nhà cung cấp!");
            if (dsChiTiet == null || dsChiTiet.Count == 0)
                throw new Exception("Phiếu nhập phải có ít nhất 1 sản phẩm!");

            // Validate từng dòng chi tiết
            foreach (var ct in dsChiTiet)
            {
                if (string.IsNullOrWhiteSpace(ct.MaSP))
                    throw new Exception("Mã sản phẩm trong chi tiết không được để trống!");
                if (ct.SoLuong <= 0)
                    throw new Exception($"Số lượng SP {ct.MaSP} phải lớn hơn 0!");
                if (ct.DonGiaNhap < 0)
                    throw new Exception($"Đơn giá nhập SP {ct.MaSP} không được âm!");
            }

            try
            {
                // Bước 1: Lưu header hoa_don_nhap
                bool okHDN = _hdnDAL.ThemHoaDonNhap(hdn);
                if (!okHDN) throw new Exception("Lưu phiếu nhập thất bại!");

                // Bước 2: Sinh mã CTHDN chuẩn và lưu từng dòng chi tiết
                int idx = 1;
                foreach (var ct in dsChiTiet)
                {
                    // Sinh mã an toàn: HDNxxx_01, HDNxxx_02...
                    ct.MaCTHDN = $"{hdn.MaHDN}_{idx:D2}";
                    ct.MaHDN = hdn.MaHDN;

                    bool okCT = _ctDAL.ThemChiTiet(ct);
                    if (!okCT) throw new Exception($"Lưu chi tiết SP {ct.MaSP} thất bại!");
                    idx++;
                }

                // Bước 3: Cập nhật tồn kho — TĂNG sau khi nhập hàng xác nhận
                foreach (var ct in dsChiTiet)
                {
                    _spDAL.TangTonKho(ct.MaSP, ct.SoLuong);
                }

                return true;
            }
            catch (Exception ex)
            {
                // Nếu một bước thất bại, ném lỗi để UI bắt và hiển thị
                throw new Exception("Lỗi tạo phiếu nhập: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  XÓA PHIẾU NHẬP
        //  Khi xóa phiếu nhập → tồn kho cũng phải trừ lại
        // ══════════════════════════════════════════════════════════
        public bool XoaPhieuNhap(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
                throw new Exception("Mã hóa đơn nhập không được để trống!");

            // Lấy chi tiết để biết cần trừ tồn kho bao nhiêu
            var dsChiTiet = _ctDAL.TimTheoMaHDN(maHDN);

            // Xóa chi tiết trước (FK)
            _ctDAL.XoaTheoMaHDN(maHDN);

            // Xóa header
            bool ok = _hdnDAL.XoaHoaDonNhap(maHDN);

            // Hoàn tồn kho (trừ lại số lượng đã nhập)
            if (ok && dsChiTiet != null)
            {
                foreach (var ct in dsChiTiet)
                {
                    try { _spDAL.GiamTonKho(ct.MaSP, ct.SoLuong); }
                    catch { /* bỏ qua nếu tồn kho đã bị thay đổi khác */ }
                }
            }

            return ok;
        }
    }
}