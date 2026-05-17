using System;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.Session
{
    public class SessionManager
    {
        private static SessionManager instance;

        public TaiKhoanDTO TaiKhoanHienTai { get; private set; }
        public bool IsLoggedIn => TaiKhoanHienTai != null;

        public static SessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SessionManager();
                }
                return instance;
            }
        }

        private SessionManager() { }

        public void Login(TaiKhoanDTO tk)
        {
            if (tk == null) throw new ArgumentNullException(nameof(tk), "Tài khoản không hợp lệ!");
            TaiKhoanHienTai = tk;
        }

        public void Logout()
        {
            TaiKhoanHienTai = null;
        }

        public bool IsAdmin()
        {
            return IsLoggedIn && TaiKhoanHienTai.VaiTro?.ToUpper() == "ADMIN";
        }
    }
}