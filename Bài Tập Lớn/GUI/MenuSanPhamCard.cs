using Bài_Tập_Lớn.DTO;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    //  MENU SAN PHAM CARD  –  UserControl hiển thị 1 sản phẩm
    // ═══════════════════════════════════════════════════════════════
    public partial class MenuSanPhamCard : UserControl
    {
        // ── Màu ──────────────────────────────────────────────────
        private static readonly Color CLR_PRIMARY = Color.FromArgb(43, 78, 35);
        private static readonly Color CLR_ACCENT = Color.FromArgb(121, 174, 111);
        private static readonly Color CLR_CARD = Color.FromArgb(247, 247, 244);
        private static readonly Color CLR_CARD_HOV = Color.FromArgb(238, 245, 236);
        private static readonly Color CLR_BORDER = Color.FromArgb(215, 215, 210);
        private static readonly Color CLR_ANH = Color.FromArgb(228, 228, 225);
        private static readonly Color CLR_ICON = Color.FromArgb(180, 180, 175);
        private static readonly Color CLR_SL_OK = Color.FromArgb(55, 130, 55);
        private static readonly Color CLR_SL_LOW = Color.DarkOrange;
        private static readonly Color CLR_SL_NONE = Color.FromArgb(185, 50, 50);

        // ── Field declarations (dùng chung với Designer.cs) ──────
        private Panel _panelAnh;
        private PictureBox _picAnh;
        private Label _lblLoai;
        private Label _lblTen;
        private Label _lblGia;
        private Label _lblSoLuong;
        private Guna2Button _btnThem;

        // ── State ─────────────────────────────────────────────────
        private SanPhamDTO _sp;
        private bool _isHover;

        // ── Event ra ngoài ────────────────────────────────────────
        public event EventHandler<SanPhamDTO> OnThemVaoGio;

        // ═════════════════════════════════════════════════════════
        //  Constructor
        // ═════════════════════════════════════════════════════════
        public MenuSanPhamCard()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.ResizeRedraw, true);

            // Gán Paint events tại đây — KHÔNG để trong Designer.cs
            _panelAnh.Paint += PanelAnh_Paint;
            _picAnh.Paint += PicAnh_Paint;
            _lblLoai.Paint += LblLoai_Paint;

            // HoverState nút "+"
            _btnThem.HoverState.FillColor = Color.FromArgb(56, 100, 46);
            _btnThem.HoverState.ForeColor = Color.White;
            _btnThem.ShadowDecoration.Enabled = true;
            _btnThem.ShadowDecoration.Color = Color.FromArgb(30, 43, 78, 35);
            _btnThem.ShadowDecoration.Depth = 6;
            _btnThem.ShadowDecoration.BorderRadius = 12;

            // Bubble hover từ child controls lên card
            foreach (Control c in this.Controls)
            {
                c.MouseEnter += (s, e) => base.OnMouseEnter(e);
                c.MouseLeave += (s, e) => base.OnMouseLeave(e);
            }
            foreach (Control c in _panelAnh.Controls)
            {
                c.MouseEnter += (s, e) => base.OnMouseEnter(e);
                c.MouseLeave += (s, e) => base.OnMouseLeave(e);
            }
        }

        // ═════════════════════════════════════════════════════════
        //  Nạp dữ liệu
        // ═════════════════════════════════════════════════════════
        public void NapDuLieu(SanPhamDTO sp)
        {
            if (sp == null) return;
            _sp = sp;

            _lblTen.Text = sp.TenSP ?? "–";
            _lblLoai.Text = FormatLoai(sp.Loai);
            _lblGia.Text = sp.GiaBan.ToString("N0") + " ₫";

            int sl = sp.SoLuongTon;
            if (sl <= 0)
            {
                _lblSoLuong.Text = "Hết hàng";
                _lblSoLuong.ForeColor = CLR_SL_NONE;
                _btnThem.Enabled = false;
                _btnThem.FillColor = Color.FromArgb(200, 200, 200);
            }
            else if (sl <= 5)
            {
                _lblSoLuong.Text = $"Còn ít: {sl}";
                _lblSoLuong.ForeColor = CLR_SL_LOW;
                _btnThem.Enabled = true;
                _btnThem.FillColor = CLR_PRIMARY;
            }
            else
            {
                _lblSoLuong.Text = $"Còn: {sl}";
                _lblSoLuong.ForeColor = CLR_SL_OK;
                _btnThem.Enabled = true;
                _btnThem.FillColor = CLR_PRIMARY;
            }

            HienThiAnh(sp.HinhAnh);
        }

        // ── Tải ảnh ───────────────────────────────────────────────
        private void HienThiAnh(string tenFile)
        {
            if (_picAnh.Image != null)
            {
                _picAnh.Image.Dispose();
                _picAnh.Image = null;
            }
            if (string.IsNullOrWhiteSpace(tenFile)) return;
            try
            {
                string fullPath = Path.Combine(Application.StartupPath, "Images", tenFile);
                if (File.Exists(fullPath))
                    _picAnh.Image = Image.FromFile(fullPath);
            }
            catch { /* bỏ qua — hiện placeholder */ }
        }

        // ── Nút "+" ───────────────────────────────────────────────
        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (_sp != null)
                OnThemVaoGio?.Invoke(this, _sp);
        }

        // ═════════════════════════════════════════════════════════
        //  Paint events
        // ═════════════════════════════════════════════════════════
        private void PanelAnh_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, _panelAnh.Width - 1, _panelAnh.Height - 1);
            int rad = 10;

            using (var path = RoundedPath(r, rad))
            {
                g.SetClip(path);
                using (var br = new SolidBrush(CLR_ANH))
                    g.FillPath(br, path);
                g.ResetClip();

                if (_picAnh.Image == null)
                {
                    using (var sf = new StringFormat
                    { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    using (var fnt = new Font("Segoe UI", 24f))
                    using (var brG = new SolidBrush(CLR_ICON))
                        g.DrawString("🖼", fnt, brG,
                            new RectangleF(0, 0, _panelAnh.Width, _panelAnh.Height), sf);
                }

                using (var pen = new Pen(Color.FromArgb(210, 210, 205), 1f))
                    g.DrawPath(pen, path);
            }
        }

        private void PicAnh_Paint(object sender, PaintEventArgs e)
        {
            if (_picAnh.Image == null) return;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, _picAnh.Width, _picAnh.Height);
            using (var path = RoundedPath(r, 10))
            {
                g.SetClip(path);
                g.DrawImage(_picAnh.Image, r);
                g.ResetClip();
            }
        }

        private void LblLoai_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var r = new Rectangle(0, 0, _lblLoai.Width - 1, _lblLoai.Height - 1);
            using (var path = RoundedPath(r, 6))
            {
                using (var br = new SolidBrush(CLR_ACCENT))
                    g.FillPath(br, path);
            }
            using (var sf = new StringFormat
            { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (var br = new SolidBrush(Color.White))
                g.DrawString(_lblLoai.Text, _lblLoai.Font, br,
                    new RectangleF(0, 0, _lblLoai.Width, _lblLoai.Height), sf);
        }

        // ═════════════════════════════════════════════════════════
        //  OnPaint card (bo tròn + shadow + hover)
        // ═════════════════════════════════════════════════════════
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Shadow
            using (var path = RoundedPath(new Rectangle(4, 4, Width - 8, Height - 8), 14))
            using (var br = new SolidBrush(Color.FromArgb(18, 0, 0, 0)))
                g.FillPath(br, path);

            // Nền card
            using (var path = RoundedPath(new Rectangle(2, 2, Width - 6, Height - 6), 14))
            {
                using (var br = new SolidBrush(_isHover ? CLR_CARD_HOV : CLR_CARD))
                    g.FillPath(br, path);
                using (var pen = new Pen(_isHover ? CLR_ACCENT : CLR_BORDER, _isHover ? 1.5f : 1f))
                    g.DrawPath(pen, path);
            }
        }

        // ═════════════════════════════════════════════════════════
        //  Hover
        // ═════════════════════════════════════════════════════════
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!ClientRectangle.Contains(PointToClient(MousePosition)))
            {
                _isHover = false;
                Invalidate();
            }
        }

        // ═════════════════════════════════════════════════════════
        //  Helpers
        // ═════════════════════════════════════════════════════════
        private static GraphicsPath RoundedPath(Rectangle r, int rad)
        {
            var p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad * 2, rad * 2, 180, 90);
            p.AddArc(r.Right - rad * 2, r.Y, rad * 2, rad * 2, 270, 90);
            p.AddArc(r.Right - rad * 2, r.Bottom - rad * 2, rad * 2, rad * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - rad * 2, rad * 2, rad * 2, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static string FormatLoai(string loai)
        {
            switch ((loai ?? "").ToUpperInvariant())
            {
                case "DO_AN": return "Đồ ăn";
                case "DO_UONG": return "Đồ uống";
                case "DUNG_CU": return "Dụng cụ";
                default: return string.IsNullOrWhiteSpace(loai) ? "–" : loai;
            }
        }

        private void _lblGia_Click(object sender, EventArgs e)
        {

        }
    }
}