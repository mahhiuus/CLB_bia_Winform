using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class NhapHangBLL
    {
        private readonly HoaDonNhapDAL _hdnDAL = new HoaDonNhapDAL();
        private readonly ChiTietHoaDonNhapDAL _ctDAL = new ChiTietHoaDonNhapDAL();
        private readonly SanPhamDAL _spDAL = new SanPhamDAL();
        public string SinhMaHDNMoi() => _hdnDAL.SinhMaMoi();
        public string SinhMaCTHDNMoi() => _ctDAL.SinhMaMoi();
        public double TinhGiaBanDeXuat(double donGiaNhap, double phanTramLoiNhuan)
        {
            if (phanTramLoiNhuan < 0)
                throw new Exception("Phần trăm lợi nhuận không được âm!");
            return Math.Round(donGiaNhap * (1 + phanTramLoiNhuan / 100.0), 0);
        }
        public List<HoaDonNhapDTO> LayTatCa()
            => _hdnDAL.LayTatCaHoaDonNhap();
        public List<ChiTietHoaDonNhapDTO> LayChiTietTheoMaHDN(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
                throw new Exception("Mã hóa đơn nhập không được để trống!");
            return _ctDAL.TimTheoMaHDN(maHDN);
        }
        public List<HoaDonNhapDTO> TimKiem(string keyword)
            => _hdnDAL.TimKiem(keyword);
        public bool TaoPhieuNhap(HoaDonNhapDTO hdn, List<ChiTietHoaDonNhapDTO> dsChiTiet)
        {
            if (hdn == null)
                throw new Exception("Dữ liệu phiếu nhập không hợp lệ!");
            if (string.IsNullOrWhiteSpace(hdn.MaHDN))
                throw new Exception("Mã phiếu nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(hdn.MaNCC))
                throw new Exception("Vui lòng chọn nhà cung cấp!");
            if (dsChiTiet == null || dsChiTiet.Count == 0)
                throw new Exception("Phiếu nhập phải có ít nhất 1 sản phẩm!");

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
                bool okHDN = _hdnDAL.ThemHoaDonNhap(hdn);
                if (!okHDN) throw new Exception("Lưu phiếu nhập thất bại!");

                int idx = 1;
                foreach (var ct in dsChiTiet)
                {
                    ct.MaCTHDN = $"{hdn.MaHDN}_{idx:D2}";
                    ct.MaHDN = hdn.MaHDN;

                    bool okCT = _ctDAL.ThemChiTiet(ct);
                    if (!okCT) throw new Exception($"Lưu chi tiết SP {ct.MaSP} thất bại!");
                    idx++;
                }
                foreach (var ct in dsChiTiet)
                {
                    _spDAL.TangTonKho(ct.MaSP, ct.SoLuong);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi tạo phiếu nhập: " + ex.Message);
            }
        }
        public bool XoaPhieuNhap(string maHDN)
        {
            if (string.IsNullOrWhiteSpace(maHDN))
                throw new Exception("Mã hóa đơn nhập không được để trống!");

            var dsChiTiet = _ctDAL.TimTheoMaHDN(maHDN);

            _ctDAL.XoaTheoMaHDN(maHDN);

            bool ok = _hdnDAL.XoaHoaDonNhap(maHDN);

            if (ok && dsChiTiet != null)
            {
                foreach (var ct in dsChiTiet)
                {
                    try { _spDAL.GiamTonKho(ct.MaSP, ct.SoLuong); }
                    catch {}
                }
            }
            return ok;
        }
    }
}