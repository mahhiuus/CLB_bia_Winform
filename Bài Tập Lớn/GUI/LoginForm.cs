using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class LoginUI : Form
    {
        public LoginUI()
        {
            InitializeComponent();
        }
     
        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPW.Text.Trim();

            try
            {
                TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
                TaiKhoanDTO taiKhoanHopLe = taiKhoanBLL.DangNhap(username, password);

                if (taiKhoanHopLe != null)
                {
                    LichSuHeThong.TenDangNhap = taiKhoanHopLe.TenDangNhap;
                    LichSuHeThong.QuyenTruyCap = taiKhoanHopLe.VaiTro;

                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
    }
}
