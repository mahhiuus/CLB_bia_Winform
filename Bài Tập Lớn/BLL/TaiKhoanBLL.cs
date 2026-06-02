using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;
using BCrypt.Net;
namespace Bài_Tập_Lớn.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL taiKhoanDAL = new TaiKhoanDAL();

        public string SinhMaMoi()
        {
            return taiKhoanDAL.SinhMaMoi();
        }

        public bool TaoAdminMacDinh()
        {
            return taiKhoanDAL.TaoAdminMacDinh();
        }
        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new Exception("Mật khẩu không được để trống!");

            TaiKhoanDTO tk = taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);

            if (tk == null)
                return null;
            bool hopLe = BCrypt.Net.BCrypt.Verify(matKhau, tk.MatKhau);
            return hopLe ? tk : null;
        }

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            return taiKhoanDAL.LayTatCaTaiKhoan();
        }

        public TaiKhoanDTO LayTheoMaTK(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new Exception("Mã tài khoản không được để trống!");
            return taiKhoanDAL.LayTheoMaTK(maTK);
        }

        public TaiKhoanDTO LayTheoTenDangNhap(string tenDangNhap)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            return taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);
        }

        public bool KiemTraTenDangNhapTonTai(string tenDangNhap)
        {
            return taiKhoanDAL.KiemTraTenDangNhapTonTai(tenDangNhap);
        }
        public bool ThemTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null)
                throw new Exception("Dữ liệu tài khoản không hợp lệ!");
            if (string.IsNullOrWhiteSpace(tk.MaTK))
                throw new Exception("Mã tài khoản không được để trống!");
            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(tk.MatKhau))
                throw new Exception("Mật khẩu không được để trống!");
            if (KiemTraTenDangNhapTonTai(tk.TenDangNhap))
                throw new Exception("Tên đăng nhập đã tồn tại!");

            tk.MatKhau = BCrypt.Net.BCrypt.HashPassword(tk.MatKhau);

            return taiKhoanDAL.ThemTaiKhoan(tk);
        }

        public bool CapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null)
                throw new Exception("Dữ liệu tài khoản không hợp lệ!");
            if (string.IsNullOrWhiteSpace(tk.MaTK))
                throw new Exception("Mã tài khoản không được để trống!");
            return taiKhoanDAL.CapNhatTaiKhoan(tk);
        }
        public bool DoiMatKhau(string maTK, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new Exception("Mã tài khoản không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new Exception("Mật khẩu mới không được để trống!");
            string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

            return taiKhoanDAL.DoiMatKhau(maTK, matKhauHash);
        }

        public bool XoaTaiKhoan(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new Exception("Mã tài khoản không được để trống!");
            return taiKhoanDAL.XoaTaiKhoan(maTK);
        }

        public List<TaiKhoanDTO> TimKiem(string keyword)
        {
            return taiKhoanDAL.TimKiem(keyword);
        }
        public bool DangKyTaiKhoan(string tenDangNhap, string matKhau, string nhapLaiMatKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new Exception("Mật khẩu không được để trống!");
            if (matKhau != nhapLaiMatKhau)
                throw new Exception("Mật khẩu nhập lại không trùng khớp!");
            if (KiemTraTenDangNhapTonTai(tenDangNhap))
                throw new Exception("Tên đăng nhập này đã tồn tại trong hệ thống!");

            string maTaiKhoanMoi = taiKhoanDAL.SinhMaMoi();

            NhanVienBLL nvBLL = new NhanVienBLL();
            string maNhanVienMoi = nvBLL.SinhMaMoi();
            NhanVienDTO nhanVienMoi = new NhanVienDTO
            {
                MaNV = maNhanVienMoi,
                HoTen = tenDangNhap,
                Sdt = "",
                GioiTinh = "Nam",
                ChucVu = "Nhân viên",
                NgaySinh = null
            };

            nvBLL.ThemNhanVien(nhanVienMoi);
            string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhau);

            TaiKhoanDTO taiKhoanMoi = new TaiKhoanDTO
            {
                MaTK = maTaiKhoanMoi,
                TenDangNhap = tenDangNhap,
                MatKhau = matKhauHash,
                VaiTro = "Nhân viên",
                MaNV = maNhanVienMoi
            };

            return taiKhoanDAL.ThemTaiKhoan(taiKhoanMoi);
        }
    }
}