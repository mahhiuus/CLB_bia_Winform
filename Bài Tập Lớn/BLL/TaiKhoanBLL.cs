using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _taiKhoanDAL =
            new TaiKhoanDAL();

        public string SinhMaMoi()
        {
            return _taiKhoanDAL.SinhMaMoi();
        }

        public bool TaoAdminMacDinh()
        {
            return _taiKhoanDAL.TaoAdminMacDinh();
        }

        public TaiKhoanDTO DangNhap(
            string tenDangNhap,
            string matKhau
        )
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                throw new Exception(
                    "Tên đăng nhập không được để trống!"
                );
            }

            if (string.IsNullOrWhiteSpace(matKhau))
            {
                throw new Exception(
                    "Mật khẩu không được để trống!"
                );
            }

            return _taiKhoanDAL.DangNhap(
                tenDangNhap,
                matKhau
            );
        }

        public List<TaiKhoanDTO> LayTatCaTaiKhoan()
        {
            return _taiKhoanDAL.LayTatCaTaiKhoan();
        }

        public TaiKhoanDTO LayTheoMaTK(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
            {
                throw new Exception(
                    "Mã tài khoản không được để trống!"
                );
            }

            return _taiKhoanDAL.LayTheoMaTK(maTK);
        }

        public TaiKhoanDTO LayTheoTenDangNhap(
            string tenDangNhap
        )
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
            {
                throw new Exception(
                    "Tên đăng nhập không được để trống!"
                );
            }

            return _taiKhoanDAL.LayTheoTenDangNhap(
                tenDangNhap
            );
        }

        public bool KiemTraTenDangNhapTonTai(
            string tenDangNhap
        )
        {
            return _taiKhoanDAL
                .KiemTraTenDangNhapTonTai(
                    tenDangNhap
                );
        }

        public bool ThemTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null)
            {
                throw new Exception(
                    "Dữ liệu tài khoản không hợp lệ!"
                );
            }

            if (string.IsNullOrWhiteSpace(tk.MaTK))
            {
                throw new Exception(
                    "Mã tài khoản không được để trống!"
                );
            }

            if (string.IsNullOrWhiteSpace(tk.TenDangNhap))
            {
                throw new Exception(
                    "Tên đăng nhập không được để trống!"
                );
            }

            if (string.IsNullOrWhiteSpace(tk.MatKhau))
            {
                throw new Exception(
                    "Mật khẩu không được để trống!"
                );
            }

            if (KiemTraTenDangNhapTonTai(
                tk.TenDangNhap
            ))
            {
                throw new Exception(
                    "Tên đăng nhập đã tồn tại!"
                );
            }

            return _taiKhoanDAL.ThemTaiKhoan(tk);
        }
        public bool CapNhatTaiKhoan(TaiKhoanDTO tk)
        {
            if (tk == null)
            {
                throw new Exception(
                    "Dữ liệu tài khoản không hợp lệ!"
                );
            }

            if (string.IsNullOrWhiteSpace(tk.MaTK))
            {
                throw new Exception(
                    "Mã tài khoản không được để trống!"
                );
            }

            return _taiKhoanDAL.CapNhatTaiKhoan(tk);
        }

        public bool DoiMatKhau(
            string maTK,
            string matKhauMoi
        )
        {
            if (string.IsNullOrWhiteSpace(maTK))
            {
                throw new Exception(
                    "Mã tài khoản không được để trống!"
                );
            }

            if (string.IsNullOrWhiteSpace(matKhauMoi))
            {
                throw new Exception(
                    "Mật khẩu mới không được để trống!"
                );
            }

            return _taiKhoanDAL.DoiMatKhau(
                maTK,
                matKhauMoi
            );
        }

        public bool XoaTaiKhoan(string maTK)
        {
            if (string.IsNullOrWhiteSpace(maTK))
            {
                throw new Exception(
                    "Mã tài khoản không được để trống!"
                );
            }

            return _taiKhoanDAL.XoaTaiKhoan(maTK);
        }

        public List<TaiKhoanDTO> TimKiem(string keyword)
        {
            return _taiKhoanDAL.TimKiem(keyword);
        }
    }
}