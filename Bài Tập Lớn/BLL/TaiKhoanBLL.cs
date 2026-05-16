using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bài_Tập_Lớn.BLL
{
    internal class TaiKhoanBLL
    {
        private static TaiKhoanBLL instance;

        // 2. Tạo thuộc tính public Instance để lớp khác (như Form) có thể gọi tới
        public static TaiKhoanBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TaiKhoanBLL();
                }
                return instance;
            }
        }

        // 3. Hàm khởi tạo private để không cho phép dùng lệnh "new TaiKhoanBLL()" ở ngoài
        private TaiKhoanBLL() { }
        public bool kiemtraDangNhap(string tendangnhap, string matkhau)
        {
            if (tendangnhap == "admin" && matkhau == "123456")
            {
                return true;
            }
            return false;
        }
    }
}
