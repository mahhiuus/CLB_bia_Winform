using System;
using System.Collections.Generic;
using Bài_Tập_Lớn.DAL;
using Bài_Tập_Lớn.DTO;
using BCrypt.Net; // ← THÊM MỚI: cần cài NuGet BCrypt.Net-Next

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

        // ── THAY ĐỔI: verify bằng BCrypt thay vì so sánh plain text ──
        public TaiKhoanDTO DangNhap(string tenDangNhap, string matKhau)
        {
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new Exception("Mật khẩu không được để trống!");

            // Lấy tài khoản theo tên đăng nhập (DAL trả về DTO kèm hash)
            TaiKhoanDTO tk = taiKhoanDAL.LayTheoTenDangNhap(tenDangNhap);

            if (tk == null)
                return null;

            // So sánh mật khẩu nhập với hash lưu trong DB
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

        // ── THAY ĐỔI: hash mật khẩu trước khi lưu ──
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

            // Hash mật khẩu trước khi lưu
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

        // ── THAY ĐỔI: hash mật khẩu mới trước khi đổi ──
        public bool DoiMatKhau(string maTK, string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(maTK))
                throw new Exception("Mã tài khoản không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new Exception("Mật khẩu mới không được để trống!");

            // Hash mật khẩu mới trước khi lưu
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

        // ══════════════════════════════════════════════════════════
        //  ĐĂNG KÝ TÀI KHOẢN — tạo NhanVien trước, sau đó tạo TaiKhoan
        // ══════════════════════════════════════════════════════════
        public bool DangKyTaiKhoan(string tenDangNhap, string matKhau, string nhapLaiMatKhau)
        {
            // ── 1. Validate đầu vào ──────────────────────────────
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                throw new Exception("Tên đăng nhập không được để trống!");
            if (string.IsNullOrWhiteSpace(matKhau))
                throw new Exception("Mật khẩu không được để trống!");
            if (matKhau != nhapLaiMatKhau)
                throw new Exception("Mật khẩu nhập lại không trùng khớp!");
            if (KiemTraTenDangNhapTonTai(tenDangNhap))
                throw new Exception("Tên đăng nhập này đã tồn tại trong hệ thống!");

            // ── 2. Sinh mã mới ───────────────────────────────────
            string maTaiKhoanMoi = taiKhoanDAL.SinhMaMoi();

            NhanVienBLL nvBLL = new NhanVienBLL();
            string maNhanVienMoi = nvBLL.SinhMaMoi();

            // ── 3. TẠO NHÂN VIÊN TRƯỚC (fix lỗi FK) ────────────
            //     Chỉ điền MaNV + HoTen (bắt buộc),
            //     các cột sdt/gioi_tinh/chuc_vu/ngay_sinh để null/rỗng
            //     → nhân viên có thể cập nhật hồ sơ đầy đủ sau
            NhanVienDTO nhanVienMoi = new NhanVienDTO
            {
                MaNV = maNhanVienMoi,
                HoTen = tenDangNhap,   // dùng tên đăng nhập làm tên tạm
                Sdt = "",
                GioiTinh = "Nam",   // giá trị mặc định, user cập nhật sau
                ChucVu = "Nhân viên",
                NgaySinh = null
            };

            nvBLL.ThemNhanVien(nhanVienMoi); // ← INSERT nhan_vien TRƯỚC

            // ── 4. TẠO TÀI KHOẢN SAU ────────────────────────────
            string matKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhau);

            TaiKhoanDTO taiKhoanMoi = new TaiKhoanDTO
            {
                MaTK = maTaiKhoanMoi,
                TenDangNhap = tenDangNhap,
                MatKhau = matKhauHash,  // ← lưu hash, không lưu raw
                VaiTro = "Nhân viên",
                MaNV = maNhanVienMoi // ← đã tồn tại trong nhan_vien → FK hợp lệ
            };

            return taiKhoanDAL.ThemTaiKhoan(taiKhoanMoi);
        }
    }
}