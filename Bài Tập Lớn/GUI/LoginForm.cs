using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.Session;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class LoginUI : Form
    {
        // ── Trạng thái toggle hiện/ẩn mật khẩu ──────────────────
        private bool _hienMatKhau = false;

        public LoginUI()
        {
            InitializeComponent();
        }

        // ══════════════════════════════════════════════════════════
        //  LOAD FORM
        // ══════════════════════════════════════════════════════════
        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Đảm bảo txtPW ẩn mật khẩu khi mở form
            txtPW.PasswordChar = '*';

            // Wire sự kiện click cho nút mắt (guna2Button1)
            guna2Button1.Click += guna2Button1_Click;
        }

        // ══════════════════════════════════════════════════════════
        //  TOGGLE HIỆN / ẨN MẬT KHẨU
        // ══════════════════════════════════════════════════════════
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            _hienMatKhau = !_hienMatKhau;

            txtPW.PasswordChar = _hienMatKhau ? '\0' : '*';

            guna2Button1.Image = _hienMatKhau
                ? global::Bài_Tập_Lớn.Properties.Resources.eshow
                : global::Bài_Tập_Lớn.Properties.Resources.ehidden;
        }

        // ══════════════════════════════════════════════════════════
        //  NÚT ĐĂNG NHẬP — tích hợp TaiKhoanBLL + BCrypt
        // ══════════════════════════════════════════════════════════
        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPW.Text;           // KHÔNG Trim mật khẩu — tránh mất ký tự khoảng trắng

            try
            {
                TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

                // DangNhap() bên trong đã dùng BCrypt.Verify() để so sánh hash
                TaiKhoanDTO taiKhoanHopLe = taiKhoanBLL.DangNhap(username, password);

                if (taiKhoanHopLe != null)
                {
                    LichSuHeThong.TenDangNhap = taiKhoanHopLe.TenDangNhap;
                    LichSuHeThong.QuyenTruyCap = taiKhoanHopLe.VaiTro;
                    SessionManager.Instance.Login(taiKhoanHopLe);
                    this.DialogResult = DialogResult.OK;
                    this.Hide();
                }
                else
                {
                    MessageBox.Show(
                        "Tài khoản hoặc mật khẩu không chính xác!",
                        "Đăng nhập thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    txtPW.Focus();
                    txtPW.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  NÚT ĐĂNG KÝ — mở RegisterUI
        // ══════════════════════════════════════════════════════════
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }

        // ── Enter trên txtUserName → nhảy sang txtPW ─────────────
        private void txtUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPW.Focus();
                e.SuppressKeyPress = true;
            }
        }

        // ── Enter trên txtPW → trigger đăng nhập ─────────────────
        private void txtPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                guna2GradientButton2_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        // ── Event stubs ────────────────────────────────────────────
        private void LoginForm_Load2(object sender, EventArgs e) { }
        private void pictureBox1_Click(object sender, EventArgs e) { }
        private void txtUser_TextChanged(object sender, EventArgs e) { }
        private void guna2Panel10_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }

        private void guna2Panel11_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}