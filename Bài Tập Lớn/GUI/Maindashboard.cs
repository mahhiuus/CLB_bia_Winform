using Bài_Tập_Lớn.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private bool _isTransitioning = false;

        // ── Skeleton ─────────────────────────────────────────────
        private Panel _skeletonRoot = null;
        private readonly List<Control> _shimTargets = new List<Control>();

        private static readonly Color _skelBase = Color.FromArgb(198, 220, 185);
        private static readonly Color _skelShine = Color.FromArgb(232, 244, 224);

        private System.Windows.Forms.Timer _pulseTimer = null;
        private float _pulsePhase = 0f;
        private bool _pulseAscend = true;
        private const int PULSE_MS = 16;

        // ── Fade ─────────────────────────────────────────────────
        private System.Windows.Forms.Timer _fadeTimer = null;
        private TaskCompletionSource<bool> _fadeTcs = null;
        private Form _fadingForm = null;
        private double _fadeOpacity = 0.0;
        private const double FADE_STEP = 0.06;
        private const int FADE_TICK_MS = 12;    // ~83fps

        // ── Timing ───────────────────────────────────────────────
        private const int LOAD_DELAY_MS = 150;
        private const int SKELETON_FLASH_MS = 100;

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

            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);
        }

        // ════════════════════════════════════════════════════════
        //  FORM LOAD
        // ════════════════════════════════════════════════════════

        private void maindashboard_Load(object sender, EventArgs e)
        {
            RemoveButtonGrayEffect(this);

            _skeletonRoot = BuildSkeletonOverlay();
            ParentMainContent.Controls.Add(_skeletonRoot);
            _skeletonRoot.BringToFront();

            btnTrangChu_Click(btnTrangChu, EventArgs.Empty);
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
        //  NAV BUTTONS
        // ════════════════════════════════════════════════════════

        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            if (_thongKeForm == null || _thongKeForm.IsDisposed)
                _thongKeForm = new ThongKeUi();
            OpenChildForm(_thongKeForm, sender);
        }

        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            if (_soDoBanForm == null || _soDoBanForm.IsDisposed)
            {
                _soDoBanForm = new SoDoBanUi();

                // Khi SoDoBanUi mở 1 bàn → tự động cập nhật ComboBox ở MenuSanPham
                _soDoBanForm.BanDuocMo += (s, maBan) =>
                {
                    // Tạo MenuSanPham nếu chưa có
                    if (_menuSanPham == null || _menuSanPham.IsDisposed)
                        _menuSanPham = new MenuSanPham();

                    // Nếu MenuSanPham đang hiển thị thì chọn bàn luôn,
                    // nếu không thì chỉ cập nhật thầm để khi người dùng mở ra đã đúng bàn
                    _menuSanPham.ChonBan(maBan);
                };
            }
            OpenChildForm(_soDoBanForm, sender);
        }

        // ════════════════════════════════════════════════════════
        //  OPEN CHILD FORM
        // ════════════════════════════════════════════════════════

        private async void OpenChildForm(Form childForm, object btnSender)
        {
            if (_isTransitioning) return;
            if (_activeForm == childForm) return;

            UpdateNavButton(btnSender);
            _isTransitioning = true;

            bool isFirstLoad = !ParentMainContent.Controls.Contains(childForm);
            if (isFirstLoad) await LoadNewChildForm(childForm);
            else await SwitchCachedChildForm(childForm);

            _isTransitioning = false;
        }

        // ════════════════════════════════════════════════════════
        //  LOAD LẦN ĐẦU
        // ════════════════════════════════════════════════════════

        private async Task LoadNewChildForm(Form childForm)
        {
            _activeForm?.Hide();

            // Skeleton lên trước — Dock.Fill che toàn bộ
            ShowSkeleton();

            // Chuẩn bị form con: Dock.Fill NGAY từ đầu, opacity=0
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;   // ← Fill luôn, không resize sau
            childForm.BackColor = Color.FromArgb(255, 255, 251);
            childForm.Opacity = 0;
            childForm.Visible = false;

            ParentMainContent.Controls.Add(childForm);

            // Skeleton phải ở trên form con
            _skeletonRoot.BringToFront();

            // Trigger OnLoad của form con trong khi skeleton che
            childForm.Show();
            childForm.Hide();

            await Task.Delay(LOAD_DELAY_MS);

            // Ẩn skeleton
            HideSkeleton();

            // Fade in — không slide để tránh resize flicker
            _activeForm = childForm;
            childForm.Visible = true;
            childForm.BringToFront();
            await FadeIn(childForm);
        }

        // ════════════════════════════════════════════════════════
        //  CHUYỂN TRANG CÓ CACHE
        // ════════════════════════════════════════════════════════

        private async Task SwitchCachedChildForm(Form childForm)
        {
            _activeForm?.Hide();

            ShowSkeleton();
            await Task.Delay(SKELETON_FLASH_MS);

            if (childForm is IRefreshable r) r.RefreshData();

            HideSkeleton();

            _activeForm = childForm;
            childForm.Dock = DockStyle.Fill;
            childForm.Opacity = 0;
            childForm.Visible = true;
            childForm.BringToFront();
            await FadeIn(childForm);
        }

        // ════════════════════════════════════════════════════════
        //  SKELETON OVERLAY — Dock.Fill, luôn che đúng kích thước
        // ════════════════════════════════════════════════════════

        private Panel BuildSkeletonOverlay()
        {
            _shimTargets.Clear();

            // Root overlay — Dock.Fill = khớp 100% với ParentMainContent
            var overlay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(255, 255, 251),
            };

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(20),
                RowCount = 2,
                ColumnCount = 1,
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 110f));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ── Hàng 1: 4 stat cards ─────────────────────────────
            var statRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 0, 12),
            };
            for (int i = 0; i < 4; i++)
                statRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
            statRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            for (int i = 0; i < 4; i++)
            {
                var card = MakeStatCard();
                card.Margin = new Padding(i == 0 ? 0 : 8, 0, 0, 0);
                statRow.Controls.Add(card, i, 0);
            }
            root.Controls.Add(statRow, 0, 0);

            // ── Hàng 2: chart 60% | right 40% ───────────────────
            var chartRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
            };
            chartRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60f));
            chartRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40f));
            chartRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            var leftChart = MakeChartCard();
            leftChart.Margin = new Padding(0, 0, 8, 0);
            chartRow.Controls.Add(leftChart, 0, 0);

            var rightCol = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent,
            };
            rightCol.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            rightCol.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            rightCol.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            var pie = MakeChartCard();
            pie.Margin = new Padding(0, 0, 0, 8);
            rightCol.Controls.Add(pie, 0, 0);
            rightCol.Controls.Add(MakeChartCard(), 0, 1);

            chartRow.Controls.Add(rightCol, 1, 0);
            root.Controls.Add(chartRow, 0, 1);

            overlay.Controls.Add(root);
            return overlay;
        }

        // Stat card: icon + số + label
        private Guna.UI2.WinForms.Guna2Panel MakeStatCard()
        {
            var card = MakeGuna(14);
            card.Dock = DockStyle.Fill;
            card.Padding = new Padding(16);

            // Icon vuông
            var icon = MakeBar(38, 38);
            icon.Location = new Point(16, 16);
            card.Controls.Add(icon);

            // Số lớn
            var num = MakeBar(80, 18);
            num.Location = new Point(16, 60);
            card.Controls.Add(num);

            // Label nhỏ
            var lbl = MakeBar(55, 10);
            lbl.Location = new Point(16, 84);
            card.Controls.Add(lbl);

            return card;
        }

        // Chart card: title + vùng trống
        private Guna.UI2.WinForms.Guna2Panel MakeChartCard()
        {
            var card = MakeGuna(16);
            card.Dock = DockStyle.Fill;
            card.Padding = new Padding(16);

            var title = MakeBar(130, 14);
            title.Location = new Point(16, 16);
            card.Controls.Add(title);

            var sub = MakeBar(80, 10);
            sub.Location = new Point(16, 36);
            card.Controls.Add(sub);

            return card;
        }

        // Guna panel bo góc — đăng ký shimmer
        private Guna.UI2.WinForms.Guna2Panel MakeGuna(int radius)
        {
            var p = new Guna.UI2.WinForms.Guna2Panel
            {
                FillColor = _skelBase,
                BorderRadius = radius,
            };
            _shimTargets.Add(p);
            return p;
        }

        // Dải nhỏ bên trong card (icon / text line)
        private Panel MakeBar(int w, int h)
        {
            var p = new Panel
            {
                Size = new Size(w, h),
                BackColor = Color.FromArgb(
                    _skelBase.R - 10,
                    _skelBase.G - 10,
                    _skelBase.B - 10),
            };
            _shimTargets.Add(p);
            return p;
        }

        // ════════════════════════════════════════════════════════
        //  SKELETON SHOW / HIDE
        // ════════════════════════════════════════════════════════

        private void ShowSkeleton()
        {
            if (_skeletonRoot == null || _skeletonRoot.IsDisposed)
            {
                _skeletonRoot = BuildSkeletonOverlay();
                ParentMainContent.Controls.Add(_skeletonRoot);
            }
            _skeletonRoot.BringToFront();
            _skeletonRoot.Visible = true;
            if (_pulseTimer == null) StartPulse();
        }

        private void HideSkeleton()
        {
            StopPulse();
            if (_skeletonRoot != null && !_skeletonRoot.IsDisposed)
                _skeletonRoot.Visible = false;
        }

        // ════════════════════════════════════════════════════════
        //  PULSE — smooth-step, cả card lớn lẫn dải nhỏ
        // ════════════════════════════════════════════════════════

        private void StartPulse()
        {
            _pulsePhase = 0f;
            _pulseAscend = true;
            _pulseTimer = new System.Windows.Forms.Timer { Interval = PULSE_MS };
            _pulseTimer.Tick += PulseTick;
            _pulseTimer.Start();
        }

        private void PulseTick(object sender, EventArgs e)
        {
            const float STEP = 0.02f;
            if (_pulseAscend) { _pulsePhase += STEP; if (_pulsePhase >= 1f) { _pulsePhase = 1f; _pulseAscend = false; } }
            else { _pulsePhase -= STEP; if (_pulsePhase <= 0f) { _pulsePhase = 0f; _pulseAscend = true; } }

            float s = _pulsePhase * _pulsePhase * (3f - 2f * _pulsePhase);

            foreach (var ctrl in _shimTargets)
            {
                if (ctrl.IsDisposed) continue;
                Color from = ctrl is Guna.UI2.WinForms.Guna2Panel ? _skelBase
                           : Color.FromArgb(_skelBase.R - 10, _skelBase.G - 10, _skelBase.B - 10);
                Color to = _skelShine;
                Color c = Color.FromArgb(255,
                    (int)(from.R + (to.R - from.R) * s),
                    (int)(from.G + (to.G - from.G) * s),
                    (int)(from.B + (to.B - from.B) * s));

                if (ctrl is Guna.UI2.WinForms.Guna2Panel gp) gp.FillColor = c;
                else ctrl.BackColor = c;
            }
        }

        private void StopPulse()
        {
            if (_pulseTimer == null) return;
            _pulseTimer.Stop();
            _pulseTimer.Tick -= PulseTick;
            _pulseTimer.Dispose();
            _pulseTimer = null;
        }

        // ════════════════════════════════════════════════════════
        //  FADE IN — ease-out cubic, không slide để tránh resize
        // ════════════════════════════════════════════════════════

        private Task FadeIn(Form form)
        {
            StopFade();
            _fadeTcs = new TaskCompletionSource<bool>();
            _fadingForm = form;
            _fadeOpacity = 0.0;

            _fadeTimer = new System.Windows.Forms.Timer { Interval = FADE_TICK_MS };
            _fadeTimer.Tick += FadeTick;
            _fadeTimer.Start();
            return _fadeTcs.Task;
        }

        private void FadeTick(object sender, EventArgs e)
        {
            _fadeOpacity += FADE_STEP;
            double t = Math.Min(_fadeOpacity, 1.0);
            // Ease-out cubic
            double eased = 1.0 - Math.Pow(1.0 - t, 3.0);

            if (_fadingForm != null && !_fadingForm.IsDisposed)
                _fadingForm.Opacity = eased;

            if (_fadeOpacity >= 1.0)
            {
                if (_fadingForm != null && !_fadingForm.IsDisposed)
                    _fadingForm.Opacity = 1.0;
                StopFade();
                _fadeTcs?.TrySetResult(true);
            }
        }

        private void StopFade()
        {
            if (_fadeTimer == null) return;
            _fadeTimer.Stop();
            _fadeTimer.Tick -= FadeTick;
            _fadeTimer.Dispose();
            _fadeTimer = null;
            _fadingForm = null;
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
            StopPulse();
            StopFade();
            _skeletonRoot?.Dispose();
            _thongKeForm?.Dispose();
            _soDoBanForm?.Dispose();
            _hoaDonUi?.Dispose();
            base.OnFormClosing(e);
        }

        // ════════════════════════════════════════════════════════
        //  STUB EVENT HANDLERS
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
        private void guna2Button3_Click(object sender, EventArgs e)
        {

            if (_menuSanPham == null || _menuSanPham.IsDisposed)
                _menuSanPham = new MenuSanPham();
            OpenChildForm(_menuSanPham, sender);

        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if (_hoaDonUi == null || _hoaDonUi.IsDisposed)
                _hoaDonUi = new HoaDonUi();
            OpenChildForm(_hoaDonUi, sender);
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {

            if (_taiKhoanPanel == null || _taiKhoanPanel.IsDisposed)
                _taiKhoanPanel = new TaiKhoanPanel();
            OpenChildForm(_taiKhoanPanel, sender);
        }
        private void guna2Button6_Click(object sender, EventArgs e)
        {
            if (_hoaDonNhapUi == null || _hoaDonNhapUi.IsDisposed)
                _hoaDonNhapUi = new HoaDonNhapUi();
            OpenChildForm(_hoaDonNhapUi, sender);
        }
        private void guna2Button7_Click(object sender, EventArgs e) { }
        private void guna2Button8_Click(object sender, EventArgs e)
        {

            if (_sanPhamPanel == null || _sanPhamPanel.IsDisposed)
                _sanPhamPanel = new SanPhamPanel();
            OpenChildForm(_sanPhamPanel, sender);

        }
        private void guna2Button9_Click(object sender, EventArgs e)
        {

            if (_banBiaPanel == null || _banBiaPanel.IsDisposed)
                _banBiaPanel = new BanBiaPanel();
            OpenChildForm(_banBiaPanel, sender);

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if (_khachHangPanel == null || _khachHangPanel.IsDisposed)
                _khachHangPanel = new KhachHangPanel();
            OpenChildForm(_khachHangPanel, sender);
        }
        private void menutxt_Click(object sender, EventArgs e) { }
        private void ParentMainContent_Paint(object sender, PaintEventArgs e) { }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (_nhanVienUI == null || _nhanVienUI.IsDisposed)
                _nhanVienUI = new NhanVienUI();
            OpenChildForm(_nhanVienUI, sender);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (_nhaCungCapPanel == null || _nhaCungCapPanel.IsDisposed)
                _nhaCungCapPanel = new NhaCungCapPanel();
            OpenChildForm(_nhaCungCapPanel, sender);
        }
    }

    // ════════════════════════════════════════════════════════
    //  INTERFACE
    // ════════════════════════════════════════════════════════

    public interface IRefreshable
    {
        void RefreshData();
    }
}