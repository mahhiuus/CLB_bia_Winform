// ╔══════════════════════════════════════════════════════════╗
// ║  File: Maindashboard.cs                                  ║
// ║  Thay THẾ HOÀN TOÀN file Maindashboard.cs cũ             ║
// ║  Maindashboard.Designer.cs GIỮ NGUYÊN, không đụng vào   ║
// ╚══════════════════════════════════════════════════════════╝
using Bài_Tập_Lớn.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class Maindashboard : Form
    {
        // ════════════════════════════════════════════════════════
        //  STATE
        // ════════════════════════════════════════════════════════
        private Form _activeForm = null;
        private bool _isTransitioning = false;
        private Guna.UI2.WinForms.Guna2Button _currentActiveButton = null;

        private ThongKeUi _thongKeForm = null;
        private SoDoBanUi _soDoBanForm = null;
        private HoaDonUi _hoaDonUi = null;
        private HoaDonNhapUi _hoaDonNhapUi = null;
        private NhanVienUI _nhanVienUI = null;
        private BanBiaPanel _banBiaPanel = null;
        private KhachHangPanel _khachHangPanel = null;
        private NhaCungCapPanel _nhaCungCapPanel = null;
        private TaiKhoanPanel _taiKhoanPanel = null;
        private SanPhamPanel _sanPhamPanel = null;
        private MenuSanPham _menuSanPham = null;
        private NhapHangPanel _nhapHangPanel   =null;
        private Panel _whiteOverlay = null;

        // ════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════
        public Maindashboard()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.Manual;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.FromArgb(38, 68, 20);

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.WindowState = FormWindowState.Maximized;
        }

        // ════════════════════════════════════════════════════════
        //  FORM LOAD
        // ════════════════════════════════════════════════════════
        private void maindashboard_Load(object sender, EventArgs e)
        {
            RemoveButtonGrayEffect(this);

            _whiteOverlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 255, 251),
            };
            ParentMainContent.Controls.Add(_whiteOverlay);
            _whiteOverlay.SendToBack();

            btnTrangChu_Click(btnTrangChu, EventArgs.Empty);

            if (string.Equals(LichSuHeThong.QuyenTruyCap, "Nhân viên",
                              StringComparison.OrdinalIgnoreCase))
            {
                btnTaiKhoan.Visible = false;
                btnKhachHang.Visible = false;
                btnNhaCungCap.Visible = false;
                btnSanPham.Visible = false;
                btnNhanVien.Visible = false;
                admintxt.Visible = false;
                btnQLBAN.Visible = false;

                int topY = btnQLBAN.Top;
                int bottomY = btnNhanVien.Bottom;
                int chieuCaoTong = bottomY - topY;

                var picTrangTri = new Guna.UI2.WinForms.Guna2PictureBox();
                int w95 = (int)(btnQLBAN.Width * 0.95);
                int h95 = (int)(chieuCaoTong * 0.95);
                picTrangTri.Size = new Size(w95, h95);
                picTrangTri.Location = new Point(
                    btnQLBAN.Left + (btnQLBAN.Width - w95) / 2,
                    topY + (chieuCaoTong - h95) / 2);
                picTrangTri.BorderRadius = 15;
                picTrangTri.SizeMode = PictureBoxSizeMode.StretchImage;
                picTrangTri.Image = global::Bài_Tập_Lớn.Properties.Resources.photodecor;
                MainSideBar.Controls.Add(picTrangTri);
            }
        }

        private void RemoveButtonGrayEffect(Control parent)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl is Guna.UI2.WinForms.Guna2Button btn)
                {
                    btn.PressedColor = Color.Transparent;
                    btn.HoverState.FillColor = Color.Transparent;
                    btn.CheckedState.FillColor = Color.Transparent;
                }
                if (ctrl.HasChildren) RemoveButtonGrayEffect(ctrl);
            }
        }

        // ════════════════════════════════════════════════════════
        //  ROUNDED REGION
        // ════════════════════════════════════════════════════════
        private void ApplyRoundedRegion(Form form, int radius)
        {
            int w = ParentMainContent.Width;
            int h = ParentMainContent.Height;
            if (w <= 0 || h <= 0) return;

            var path = new GraphicsPath();
            path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
            path.AddArc(w - radius * 2, 0, radius * 2, radius * 2, 270, 90);
            path.AddArc(w - radius * 2, h - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(0, h - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();
            form.Region = new Region(path);
        }

        // ════════════════════════════════════════════════════════
        //  NAV BUTTONS
        // ════════════════════════════════════════════════════════
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            if (_thongKeForm == null || _thongKeForm.IsDisposed) _thongKeForm = new ThongKeUi();
            OpenChildForm(_thongKeForm, sender);
        }

        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            if (_soDoBanForm == null || _soDoBanForm.IsDisposed)
            {
                _soDoBanForm = new SoDoBanUi();
                _soDoBanForm.BanDuocMo += (s, maBan) =>
                {
                    if (_menuSanPham == null || _menuSanPham.IsDisposed) _menuSanPham = new MenuSanPham();
                    _menuSanPham.ChonBan(maBan);
                };
            }
            OpenChildForm(_soDoBanForm, sender);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (_menuSanPham == null || _menuSanPham.IsDisposed) _menuSanPham = new MenuSanPham();
            OpenChildForm(_menuSanPham, sender);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (_hoaDonUi == null || _hoaDonUi.IsDisposed) _hoaDonUi = new HoaDonUi();
            OpenChildForm(_hoaDonUi, sender);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            if (_taiKhoanPanel == null || _taiKhoanPanel.IsDisposed) _taiKhoanPanel = new TaiKhoanPanel();
            OpenChildForm(_taiKhoanPanel, sender);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (_hoaDonNhapUi == null || _hoaDonNhapUi.IsDisposed) _hoaDonNhapUi = new HoaDonNhapUi();
            OpenChildForm(_hoaDonNhapUi, sender);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            if (_sanPhamPanel == null || _sanPhamPanel.IsDisposed) _sanPhamPanel = new SanPhamPanel();
            OpenChildForm(_sanPhamPanel, sender);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (_banBiaPanel == null || _banBiaPanel.IsDisposed) _banBiaPanel = new BanBiaPanel();
            OpenChildForm(_banBiaPanel, sender);
        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (_khachHangPanel == null || _khachHangPanel.IsDisposed) _khachHangPanel = new KhachHangPanel();
            OpenChildForm(_khachHangPanel, sender);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (_nhanVienUI == null || _nhanVienUI.IsDisposed) _nhanVienUI = new NhanVienUI();
            OpenChildForm(_nhanVienUI, sender);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (_nhaCungCapPanel == null || _nhaCungCapPanel.IsDisposed) _nhaCungCapPanel = new NhaCungCapPanel();
            OpenChildForm(_nhaCungCapPanel, sender);
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                LichSuHeThong.TenDangNhap = null;
                LichSuHeThong.QuyenTruyCap = null;
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        // ════════════════════════════════════════════════════════
        //  OPEN CHILD FORM — smooth transition + rounded corners
        // ════════════════════════════════════════════════════════
        private async void OpenChildForm(Form childForm, object btnSender)
        {
            if (_isTransitioning) return;
            if (_activeForm == childForm) return;

            _isTransitioning = true;
            UpdateNavButton(btnSender);

            // 1) Ẩn form cũ ngay lập tức
            if (_activeForm != null && !_activeForm.IsDisposed)
            {
                _activeForm.Visible = false;
                _activeForm.Opacity = 0;
            }

            bool isFirstLoad = !ParentMainContent.Controls.Contains(childForm);

            if (isFirstLoad)
            {
                // 2a) Lần đầu load
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                childForm.BackColor = Color.FromArgb(255, 255, 251);
                childForm.Opacity = 0;
                childForm.Visible = false;

                ParentMainContent.SuspendLayout();
                ParentMainContent.Controls.Add(childForm);

                childForm.Show();
                childForm.Hide();

                ParentMainContent.ResumeLayout(false);

                // Bo tròn 15px
                ApplyRoundedRegion(childForm, 15);

                await Task.Delay(32);
            }
            else
            {
                // 2b) Đã load rồi
                if (childForm is IRefreshable r) r.RefreshData();
                childForm.Dock = DockStyle.Fill;
                childForm.Opacity = 0;
                childForm.Visible = false;

                // Cập nhật lại region (phòng resize)
                ApplyRoundedRegion(childForm, 15);

                await Task.Delay(16);
            }

            // 3) Reveal: fade opacity 0.5 → 1.0 bằng Timer (~100ms)
            _activeForm = childForm;
            childForm.Opacity = 0.5;
            childForm.Visible = true;
            childForm.BringToFront();

            var fadeTimer = new System.Windows.Forms.Timer();
            fadeTimer.Interval = 16; // ~60fps
            fadeTimer.Tick += (t, args) =>
            {
                if (childForm.IsDisposed)
                {
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                    _isTransitioning = false;
                    return;
                }

                childForm.Opacity += 0.08;

                if (childForm.Opacity >= 1.0)
                {
                    childForm.Opacity = 1.0;
                    fadeTimer.Stop();
                    fadeTimer.Dispose();
                    _isTransitioning = false;
                }
            };
            fadeTimer.Start();
        }

        // ════════════════════════════════════════════════════════
        //  NAV BUTTON STATE
        // ════════════════════════════════════════════════════════
        private void UpdateNavButton(object btnSender)
        {
            if (!(btnSender is Guna.UI2.WinForms.Guna2Button btn)) return;
            if (_currentActiveButton != null) _currentActiveButton.Checked = false;
            _currentActiveButton = btn;
            _currentActiveButton.Checked = true;
        }

        // ════════════════════════════════════════════════════════
        //  CLEANUP
        // ════════════════════════════════════════════════════════
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _whiteOverlay?.Dispose();
            _thongKeForm?.Dispose();
            _soDoBanForm?.Dispose();
            _hoaDonUi?.Dispose();
            base.OnFormClosing(e);
        }

        // ════════════════════════════════════════════════════════
        //  STUB EVENT HANDLERS (Designer generated — không xóa)
        // ════════════════════════════════════════════════════════
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void admintxt_Click(object sender, EventArgs e) { }
        private void guna2Panel9_Paint(object sender, PaintEventArgs e) { }
        private void guna2ControlBox2_Click(object sender, EventArgs e) { }
        private void guna2ControlBox1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void menutxt_Click(object sender, EventArgs e) { }
        private void ParentMainContent_Paint(object sender, PaintEventArgs e) { }
        private void guna2Button1_click(object sender, PaintEventArgs e) { }
        private void guna2Button1_Click_1(object sender, EventArgs e) {
            if(_nhapHangPanel == null || _nhapHangPanel.IsDisposed) _nhapHangPanel = new NhapHangPanel();
            OpenChildForm( _nhapHangPanel, sender);

        }
    }

    // ════════════════════════════════════════════════════════
    //  INTERFACE
    // ════════════════════════════════════════════════════════
    public interface IRefreshable { void RefreshData(); }
}