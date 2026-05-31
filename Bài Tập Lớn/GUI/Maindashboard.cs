using Bài_Tập_Lớn.UI;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ════════════════════════════════════════════════════════
    //  SKELETON OVERLAY — xuất hiện khi form đang preload
    //  Vẽ các thanh shimmer giống skeleton screen hiện đại
    // ════════════════════════════════════════════════════════
    internal sealed class SkeletonOverlay : Control
    {
        private System.Windows.Forms.Timer _shimTimer;
        private float _shimOffset = 0f;
        private const int SHIM_WIDTH = 300;
        private readonly Color _base = Color.FromArgb(230, 230, 225);
        private readonly Color _shim1 = Color.FromArgb(245, 245, 241);
        private readonly Color _shim2 = Color.FromArgb(255, 255, 251);

        public SkeletonOverlay()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.FromArgb(255, 255, 251);
            Dock = DockStyle.Fill;

            _shimTimer = new System.Windows.Forms.Timer { Interval = 16 }; // ~60fps
            _shimTimer.Tick += (s, e) =>
            {
                _shimOffset += 6f;
                if (_shimOffset > Width + SHIM_WIDTH) _shimOffset = -SHIM_WIDTH;
                Invalidate();
            };
            _shimTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.Clear(BackColor);

            // Vẽ các block skeleton
            DrawSkeletonBlock(g, 24, 24, Width - 48, 36, 8);   // tiêu đề
            DrawSkeletonBlock(g, 24, 76, (Width - 48) / 3 - 8, 80, 12);  // card 1
            DrawSkeletonBlock(g, 24 + (Width - 48) / 3 + 4, 76, (Width - 48) / 3 - 8, 80, 12);  // card 2
            DrawSkeletonBlock(g, 24 + (Width - 48) / 3 * 2 + 8, 76, (Width - 48) / 3 - 8, 80, 12); // card 3

            DrawSkeletonBlock(g, 24, 176, Width - 48, Height - 224, 12); // bảng lớn

            // Shimmer sweep
            using (var brush = new LinearGradientBrush(
                new PointF(_shimOffset - SHIM_WIDTH, 0),
                new PointF(_shimOffset + SHIM_WIDTH, 0),
                Color.Transparent, Color.Transparent))
            {
                var blend = new ColorBlend(3);
                blend.Colors = new[] { Color.Transparent, Color.FromArgb(80, _shim2), Color.Transparent };
                blend.Positions = new[] { 0f, 0.5f, 1f };
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, 24, 24, Width - 48, Height - 48);
            }
        }

        private void DrawSkeletonBlock(Graphics g, int x, int y, int w, int h, int radius)
        {
            if (w <= 0 || h <= 0) return;
            using (var path = RoundedRect(x, y, w, h, radius))
            using (var fill = new SolidBrush(_base))
                g.FillPath(fill, path);
        }

        private static GraphicsPath RoundedRect(int x, int y, int w, int h, int r)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _shimTimer?.Stop(); _shimTimer?.Dispose(); }
            base.Dispose(disposing);
        }
    }

    // ════════════════════════════════════════════════════════
    //  TRANSITION ENGINE
    //  - Easing: ease-out cubic thay vì linear → mượt hơn hẳn
    //  - Opacity: FADE_START(0.5) → 1.0 trong ~250ms
    //  - Skeleton hiện ngay khi click, ẩn trước khi fade-in
    // ════════════════════════════════════════════════════════
    internal sealed class TransitionEngine : IDisposable
    {
        // ── Cấu hình ─────────────────────────────────────────
        private const double FADE_START = 0.50;   // opacity bắt đầu
        private const double FADE_END = 1.00;
        private const int DURATION_MS = 250;    // tổng thời gian fade
        private const int TICK_MS = 10;     // ~100fps → rất mượt

        private readonly Panel _container;          // ParentMainContent

        // ── Runtime state ─────────────────────────────────────
        private System.Windows.Forms.Timer _timer;
        private TaskCompletionSource<bool> _tcs;
        private Form _target;
        private DateTime _startTime;
        private SkeletonOverlay _skeleton;

        public TransitionEngine(Panel container)
        {
            _container = container;
        }

        // ── Hiện skeleton ngay khi user click ─────────────────
        public void ShowSkeleton()
        {
            HideSkeleton();
            _skeleton = new SkeletonOverlay();
            _container.Controls.Add(_skeleton);
            _skeleton.BringToFront();
        }

        public void HideSkeleton()
        {
            if (_skeleton == null) return;
            _container.Controls.Remove(_skeleton);
            _skeleton.Dispose();
            _skeleton = null;
        }

        // ── Fade opacity FADE_START → 1.0 với easing cubic ────
        public Task FadeIn(Form form)
        {
            StopTimer();
            _tcs = new TaskCompletionSource<bool>();
            _target = form;

            // Đặt ngay trước khi timer chạy — không có frame flash
            SafeSetOpacity(form, FADE_START);

            _startTime = DateTime.UtcNow;
            _timer = new System.Windows.Forms.Timer { Interval = TICK_MS };
            _timer.Tick += OnTick;
            _timer.Start();

            return _tcs.Task;
        }

        private void OnTick(object sender, EventArgs e)
        {
            double elapsed = (DateTime.UtcNow - _startTime).TotalMilliseconds;
            double t = Math.Min(elapsed / DURATION_MS, 1.0);

            // Ease-out cubic: t' = 1 - (1-t)^3
            double tEased = 1.0 - Math.Pow(1.0 - t, 3);
            double opacity = FADE_START + (FADE_END - FADE_START) * tEased;

            SafeSetOpacity(_target, opacity);

            if (t >= 1.0)
            {
                SafeSetOpacity(_target, 1.0);
                StopTimer();
                _tcs?.TrySetResult(true);
            }
        }

        private static void SafeSetOpacity(Form form, double opacity)
        {
            if (form == null || form.IsDisposed) return;
            try { form.Opacity = opacity; } catch { /* form đang dispose */ }
        }

        private void StopTimer()
        {
            if (_timer == null) return;
            _timer.Stop();
            _timer.Tick -= OnTick;
            _timer.Dispose();
            _timer = null;
            _target = null;
        }

        public void Dispose()
        {
            StopTimer();
            HideSkeleton();
        }
    }

    // ════════════════════════════════════════════════════════
    //  MAINDASHBOARD — chỉ phần thay đổi
    //  (giữ nguyên toàn bộ code cũ, chỉ thay 3 vùng:
    //   1. Khai báo field
    //   2. maindashboard_Load — khởi tạo engine
    //   3. OpenChildForm — logic mới
    //   4. OnFormClosing — dispose engine)
    // ════════════════════════════════════════════════════════
    public partial class Maindashboard : Form
    {
        // ════════════════════════════════════════════════════════
        //  STATE  (giữ nguyên tất cả field cũ, chỉ THAY phần fade)
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

        private Panel _whiteOverlay = null;

        // ── THAY: dùng TransitionEngine thay vì fade timer thủ công ──
        private TransitionEngine _transition;

        // ════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════
        public Maindashboard()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = Color.FromArgb(38, 68, 20);
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint, true);
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

            // ── Khởi tạo engine ────────────────────────────────
            _transition = new TransitionEngine(ParentMainContent);

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

                int topY = btnQLBAN.Top, bottomY = btnNhanVien.Bottom;
                int chieuCaoTong = bottomY - topY;
                var picTrangTri = new Guna.UI2.WinForms.Guna2PictureBox();
                int w95 = (int)(btnQLBAN.Width * 0.95), h95 = (int)(chieuCaoTong * 0.95);
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
        //  NAV BUTTONS  (giữ nguyên toàn bộ)
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
            DialogResult dr = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?",
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
        //  OPEN CHILD FORM — logic mới, mượt, không flash
        // ════════════════════════════════════════════════════════
        private async void OpenChildForm(Form childForm, object btnSender)
        {
            // ── Guard ──────────────────────────────────────────
            if (_isTransitioning) return;
            if (_activeForm == childForm) return;

            _isTransitioning = true;
            UpdateNavButton(btnSender);

            // 1) Ẩn form cũ ngay — overlay trắng lộ ra, không giật
            if (_activeForm != null && !_activeForm.IsDisposed)
            {
                _activeForm.Visible = false;
                _activeForm.Opacity = 0;          // reset sẵn cho lần sau
            }

            bool isFirstLoad = !ParentMainContent.Controls.Contains(childForm);

            if (isFirstLoad)
            {
                // 2a) LẦN ĐẦU: Hiện skeleton ngay → người dùng thấy phản hồi tức thì
                _transition.ShowSkeleton();

                // Chuẩn bị form hoàn toàn ẩn trước khi add vào container
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                childForm.BackColor = Color.FromArgb(255, 255, 251);
                childForm.Opacity = 0;
                childForm.Visible = false;

                ParentMainContent.SuspendLayout();
                ParentMainContent.Controls.Add(childForm);

                // Trigger OnLoad ngầm (Show rồi Hide ngay) — không vẽ gì lên màn hình
                childForm.Show();
                childForm.Hide();

                ParentMainContent.ResumeLayout(false);

                // Nhường UI thread 2 frames (~32ms) để form hoàn tất render nội bộ
                // Đủ để DataGridView, chart, binding chạy xong mà không block UI
                await Task.Delay(32);

                // 3) Ẩn skeleton — form đã sẵn sàng
                _transition.HideSkeleton();
            }
            else
            {
                // 2b) ĐÃ CÓ: Refresh data nếu implement IRefreshable
                if (childForm is IRefreshable r) r.RefreshData();
                childForm.Dock = DockStyle.Fill;
                childForm.Opacity = 0;
                childForm.Visible = false;

                // 1 frame để layout ổn định, không cần skeleton
                await Task.Delay(16);
            }

            // 4) Reveal: set opacity=FADE_START, BringToFront, rồi fade lên 1.0
            //    Không bao giờ có frame opacity=0 hiển thị → không flash đen/trắng
            _activeForm = childForm;
            childForm.Opacity = 0.5;
            childForm.Visible = true;
            childForm.BringToFront();

            await _transition.FadeIn(childForm);

            _isTransitioning = false;
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
            _transition?.Dispose();
            _whiteOverlay?.Dispose();
            _thongKeForm?.Dispose();
            _soDoBanForm?.Dispose();
            _hoaDonUi?.Dispose();
            base.OnFormClosing(e);
        }

        // ════════════════════════════════════════════════════════
        //  STUB EVENT HANDLERS  (giữ nguyên)
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
    }

    // ════════════════════════════════════════════════════════
    //  INTERFACE
    // ════════════════════════════════════════════════════════
    public interface IRefreshable { void RefreshData(); }
}