using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL = new TaiKhoanDAL();

        public string SinhMaMoi()
        {
            return _taiKhoanDAL.SinhMaMoi();
        }

        public void TaoAdminMacDinh()
        {
            _taiKhoanDAL.TaoAdminMacDinh();
        }

        public void ThemTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null || string.IsNullOrWhiteSpace(tk.TenDangNhap) || string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                throw new ArgumentException("Tài khoản hoặc các trường bắt buộc không được để trống!");
            }
            _taiKhoanDAL.ThemTaiKhoan(tk);
        }

        public void XoaTaiKhoanTheoMaTK(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
            {
                throw new ArgumentException("Mã tài khoản không được để trống!");
            }
            _taiKhoanDAL.XoaTaiKhoanTheoMaTK(maTK);
        }

        public void DoiMatKhau(string maTK, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK) || string.IsNullOrWhiteSpace(matKhauMoi))
            {
                throw new ArgumentException("Dữ liệu đổi mật khẩu không hợp lệ!");
            }
            _taiKhoanDAL.DoiMatKhau(maTK, matKhauMoi);
        }

        public void DoiTenDangNhap(string maTK, string tenDangNhapMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK) || string.IsNullOrWhiteSpace(tenDangNhapMoi))
            {
                throw new ArgumentException("Dữ liệu đổi tên đăng nhập không hợp lệ!");
            }
            _taiKhoanDAL.DoiTenDangNhap(maTK, tenDangNhapMoi);
        }

        public void CapNhatVaiTro(string maTK, string vaiTro)
        {
            _taiKhoanDAL.CapNhatVaiTro(maTK, vaiTro);
        }

        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhau))
            {
                throw new ArgumentException("Tên đăng nhập và mật khẩu không được bỏ trống!");
            }
            return _taiKhoanDAL.DangNhap(tenDangNhap, matKhau);
        }

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            return _taiKhoanDAL.LayTatCaTaiKhoan();
        }

        public TaiKhoanDTO LayTheoMaTK(string maTK)
        {
            return _taiKhoanDAL.LayTheoMaTK(maTK);
        }

        public TaiKhoanDTO LayTheoTenDangNhap(string tenDangNhap)
        {
            return _taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);
        }

        public bool KiemTraTenDangNhapTonTai(string tenDangNhap)
        {
            return _taiKhoanDAL.KiemTraTenDangNhapTonTai(tenDangNhap);
        }

        public bool DatLaiMatKhau(string tenDangNhap, string matKhauMoi)
        {
            return _taiKhoanDAL.DatLaiMatKhau(tenDangNhap, matKhauMoi);
        }

        public void CapNhatToanBoTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null || string.IsNullOrWhiteSpace(tk.MaTK))
            {
                throw new ArgumentException("Dữ liệu cập nhật không hợp lệ!");
            }
            _taiKhoanDAL.CapNhatToanBoTaiKhoan(tk);
        }

        // Các hàm Alias gọi nhanh tương đương
        public void XoaTaiKhoan(string maTK) => XoaTaiKhoanTheoMaTK(maTK);
        public List<TaiKhoanDTO> GetAllTaiKhoan() => LayTatCaTaiKhoan();
    }
}