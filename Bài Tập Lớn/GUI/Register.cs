using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices; // [THÊM MỚI] Để gọi thư viện Windows API
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;

namespace Bài_Tập_Lớn.GUI
{
    public partial class RegisterUI : Form
    {
        // [THÊM MỚI] Gọi hàm API của Windows để kiểm tra mức độ DPI
        [DllImport("shcore.dll")]
        private static extern int GetProcessDpiAwareness(IntPtr hprocess, out int awareness);

        // ── THÊM MỚI: trạng thái toggle 2 ô mật khẩu ──
        private bool _hienMatKhau1 = false; // txtMatKhauDangKi   (guna2Button2)
        private bool _hienMatKhau2 = false; // txtMatKhauDangKiMoi (guna2Button1)

        public RegisterUI()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái Manifest khi Form vừa load

        }

        // Các hàm ẩn phía dưới giữ nguyên không thay đổi...
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Bật thuật toán khử răng cưa và làm mịn ảnh tối đa
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            Application.OpenForms["LoginUI"]?.Show();
            this.Close();
        }

        private void guna2Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        // ── THÊM MỚI: guna2Button2 → toggle txtMatKhauDangKi ──
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _hienMatKhau1 = !_hienMatKhau1;
            txtMatKhauDangKi.PasswordChar = _hienMatKhau1 ? '\0' : '*';
            guna2Button2.Image = _hienMatKhau1
                ? global::Bài_Tập_Lớn.Properties.Resources.eshow
                : global::Bài_Tập_Lớn.Properties.Resources.ehidden;
        }

        // ── THÊM MỚI: guna2Button1 → toggle txtMatKhauDangKiMoi ──
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            _hienMatKhau2 = !_hienMatKhau2;
            txtMatKhauDangKiMoi.PasswordChar = _hienMatKhau2 ? '\0' : '*';
            guna2Button1.Image = _hienMatKhau2
                ? global::Bài_Tập_Lớn.Properties.Resources.eshow
                : global::Bài_Tập_Lớn.Properties.Resources.ehidden;
        }

        private void btnDangKi_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangKi.Text.Trim();
            string matKhau = txtMatKhauDangKi.Text.Trim();
            string nhapLaiMatKhau = txtMatKhauDangKiMoi.Text.Trim();

            try
            {
                TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();
                LoginUI login = new LoginUI();
                bool kq = taiKhoanBLL.DangKyTaiKhoan(tenDangNhap, matKhau, nhapLaiMatKhau);

                if (kq)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đăng ký thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}