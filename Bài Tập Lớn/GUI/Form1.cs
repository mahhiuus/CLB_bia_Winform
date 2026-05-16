using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string taiKhoan = textBox1.Text;
            string matKhau = textBox2.Text;

            // 1. Gọi hàm test ở tầng BLL
            bool canLogin = TaiKhoanBLL.Instance.kiemtraDangNhap(taiKhoan, matKhau);

            // 2. Kiểm tra kết quả trả về để hiển thị MessageBox tương ứng
            if (canLogin)
            {
                // Hàm hiển thị thông báo thành công chuẩn chỉ của C# Windows Form
                MessageBox.Show("Đăng nhập vào hệ thống thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // (Tùy chọn) Nếu thành công thì mở Form chính của ứng dụng lên
                // MainForm f = new MainForm();
                // f.Show();
                // this.Hide();
            }
            else
            {
                // Thông báo thất bại kèm icon Cảnh báo (Warning)
                MessageBox.Show("Tài khoản hoặc Mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
