using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.GUI
{
    public class ThanhToanDialog : Form
    {
        public bool IsPaid { get; private set; } = false;

        private readonly BanBidaDTO _ban;
        private readonly PhienChoiDTO _phien;
        private readonly PhienChoiBLL _phienBLL = new PhienChoiBLL();
        private readonly BanBidaBLL _banBLL = new BanBidaBLL();

        private System.Windows.Forms.Timer _clock;
        private Label lblThoiGian, lblTienGio, lblTongTien;

        // Colors
        static readonly Color C_DARK = ColorTranslator.FromHtml("#1e3d18");
        static readonly Color C_MID = ColorTranslator.FromHtml("#2b5220");
        static readonly Color C_ACCENT = ColorTranslator.FromHtml("#4a9e3f");
        static readonly Color C_GOLD = ColorTranslator.FromHtml("#c9a84c");
        static readonly Color C_CREAM = ColorTranslator.FromHtml("#f5f7f4");
        static readonly Color C_GRAY = ColorTranslator.FromHtml("#6b7566");
        static readonly Color C_BORDER = ColorTranslator.FromHtml("#dde8db");

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ClassStyle |= 0x20000; // CS_DROPSHADOW
                return cp;
            }
        }

        public ThanhToanDialog(BanBidaDTO ban, PhienChoiDTO phien)
        {
            _ban = ban;
            _phien = phien;
            BuildUI();
            StartClock();
        }

        private void BuildUI()
        {
            Text = "Thanh Toán";
            Size = new Size(460, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = C_CREAM;
            ShowInTaskbar = false;
            Region = new Region(RoundRect(new Rectangle(0, 0, Width, Height), 18));

            // ── HEADER ──
            var header = new Panel { Dock = DockStyle.Top, Height = 86, BackColor = Color.Transparent };
            header.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (var br = new LinearGradientBrush(
                    new Rectangle(0, 0, header.Width, header.Height + 10),
                    C_DARK, C_MID, LinearGradientMode.ForwardDiagonal))
                using (var path = RoundRectTop(new Rectangle(0, 0, header.Width, header.Height), 18))
                    g.FillPath(br, path);

                // Gold divider
                using (var pen = new Pen(Color.FromArgb(160, C_GOLD), 1.5f))
                    g.DrawLine(pen, 20, header.Height - 1, header.Width - 20, header.Height - 1);

                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    using (var f = new Font("Segoe UI", 16f, FontStyle.Bold))
                        g.DrawString("THANH TOÁN", f, Brushes.White,
                            new RectangleF(0, 0, header.Width, 54), sf);
                    using (var f2 = new Font("Segoe UI", 9f))
                    using (var br2 = new SolidBrush(Color.FromArgb(190, 255, 255, 255)))
                        g.DrawString(_ban.TenBan + "  ·  " + _ban.LoaiBan, f2, br2,
                            new RectangleF(0, 50, header.Width, 30), sf);
                }
            };

            var btnX = new Button
            {
                Text = "✕",
                Size = new Size(32, 32),
                Location = new Point(Width - 44, 14),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 255, 255, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnX.FlatAppearance.BorderSize = 0;
            btnX.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 255, 255, 255);
            btnX.Region = new Region(RoundRect(new Rectangle(0, 0, 32, 32), 8));
            btnX.Click += (s, e) => Close();
            header.Controls.Add(btnX);

            bool drag = false; Point ds = Point.Empty;
            header.MouseDown += (s, e) => { drag = true; ds = e.Location; };
            header.MouseMove += (s, e) => { if (drag) Location = new Point(Location.X + e.X - ds.X, Location.Y + e.Y - ds.Y); };
            header.MouseUp += (s, e) => drag = false;
            Controls.Add(header);

            // ── BODY PANEL ──
            var body = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent, Padding = new Padding(22, 14, 22, 0) };
            Controls.Add(body);

            // ── CARD 1: Thông tin phiên ──
            var card1 = MakeCard(160);
            body.Controls.Add(card1);

            int iy = 18;
            AddRow(card1, iy, "🕐  Bắt đầu", _phien.ThoiGianBatDau.ToString("HH:mm:ss   dd/MM/yyyy"), C_DARK, false); iy += 36;
            lblThoiGian = AddRowLive(card1, iy, "⏱  Thời gian", "--:--:--", C_GRAY); iy += 36;
            AddRow(card1, iy, "🎱  Loại bàn", _ban.LoaiBan, C_GRAY, false); iy += 36;
            AddRow(card1, iy, "💰  Đơn giá/giờ", _ban.GiaTheoGio.ToString("N0") + " đ", C_MID, true);

            // ── CARD 2: Tổng tiền ──
            var card2 = MakeCard(148);
            card2.Top = card1.Bottom + 14;
            body.Controls.Add(card2);

            // Tiền giờ row
            var lk1 = InfoLbl("⏳  Tiền giờ chơi", 10f, C_GRAY);
            lk1.Location = new Point(18, 18);
            lk1.Size = new Size(180, 28);
            card2.Controls.Add(lk1);

            lblTienGio = ValLbl("0 đ", 12f, C_MID, bold: true);
            lblTienGio.Location = new Point(card2.Width - 18 - 180, 18);
            lblTienGio.Size = new Size(180, 28);
            card2.Controls.Add(lblTienGio);

            // Gold separator
            var sep = new Panel { Location = new Point(18, 60), Size = new Size(card2.Width - 36, 1), BackColor = C_GOLD };
            card2.Controls.Add(sep);

            // Tổng tiền
            var lk2 = InfoLbl("TỔNG THANH TOÁN", 11f, C_DARK, bold: true);
            lk2.Location = new Point(18, 72);
            lk2.Size = new Size(180, 40);
            card2.Controls.Add(lk2);

            lblTongTien = ValLbl("0 đ", 20f, C_DARK, bold: true);
            lblTongTien.Location = new Point(card2.Width - 18 - 190, 68);
            lblTongTien.Size = new Size(190, 48);
            card2.Controls.Add(lblTongTien);

            // ── FOOTER: Nút thanh toán ──
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 78, BackColor = Color.Transparent, Padding = new Padding(22, 12, 22, 16) };
            var btnPay = new Button
            {
                Text = "✔   XÁC NHẬN THANH TOÁN",
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                BackColor = C_DARK,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(0, 0, btnPay.Width, btnPay.Height);
                bool hot = r.Contains(btnPay.PointToClient(Cursor.Position));
                Color top = hot ? C_MID : C_DARK;
                Color bot = ColorTranslator.FromHtml("#152b10");
                using (var br = new LinearGradientBrush(r, top, bot, 90f))
                using (var p = RoundRect(r, 12))
                    g.FillPath(br, p);
                using (var pen = new Pen(Color.FromArgb(100, C_GOLD), 1f))
                using (var p = RoundRect(new Rectangle(1, 1, r.Width - 2, r.Height - 2), 11))
                    g.DrawPath(pen, p);
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    g.DrawString(btnPay.Text, btnPay.Font, Brushes.White, new RectangleF(0, 0, btnPay.Width, btnPay.Height), sf);
            };
            btnPay.MouseEnter += (s, e) => btnPay.Invalidate();
            btnPay.MouseLeave += (s, e) => btnPay.Invalidate();
            btnPay.Click += BtnPay_Click;
            footer.Controls.Add(btnPay);
            Controls.Add(footer);
        }

        // ── Tạo card trắng bo góc (tự layout trong body) ──
        private Panel MakeCard(int height)
        {
            int cardW = 460 - 22 - 22; // Width - padding*2
            var card = new Panel
            {
                Location = new Point(0, 0), // body.Padding sẽ offset
                Size = new Size(cardW, height),
                BackColor = Color.White
            };
            card.Region = new Region(RoundRect(new Rectangle(0, 0, cardW, height), 14));
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                // Accent stripe xanh top
                using (var br = new LinearGradientBrush(new Rectangle(0, 0, card.Width, 5), C_ACCENT, ColorTranslator.FromHtml("#a8d5a0"), 0f))
                using (var p = RoundRectTop(new Rectangle(0, 0, card.Width, 5), 14))
                    g.FillPath(br, p);
                // Border
                using (var pen = new Pen(C_BORDER, 1.2f))
                using (var p = RoundRect(new Rectangle(0, 0, card.Width - 1, height - 1), 14))
                    g.DrawPath(pen, p);
            };
            return card;
        }

        private void AddRow(Panel card, int y, string key, string val, Color valColor, bool valBold)
        {
            var lKey = InfoLbl(key, 9.5f, C_GRAY);
            lKey.Location = new Point(18, y);
            lKey.Size = new Size(200, 26);
            card.Controls.Add(lKey);

            var lVal = ValLbl(val, 9.5f, valColor, valBold);
            lVal.Location = new Point(card.Width - 18 - 200, y);
            lVal.Size = new Size(200, 26);
            card.Controls.Add(lVal);
        }

        private Label AddRowLive(Panel card, int y, string key, string initVal, Color valColor)
        {
            var lKey = InfoLbl(key, 9.5f, C_GRAY);
            lKey.Location = new Point(18, y);
            lKey.Size = new Size(200, 26);
            card.Controls.Add(lKey);

            var lVal = ValLbl(initVal, 9.5f, valColor, true);
            lVal.Location = new Point(card.Width - 18 - 200, y);
            lVal.Size = new Size(200, 26);
            card.Controls.Add(lVal);
            return lVal;
        }

        private Label InfoLbl(string text, float size, Color color, bool bold = false) => new Label
        {
            Text = text,
            AutoSize = false,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = color,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft
        };

        private Label ValLbl(string text, float size, Color color, bool bold) => new Label
        {
            Text = text,
            AutoSize = false,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = color,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleRight
        };

        private void StartClock()
        {
            UpdateTotals();
            _clock = new System.Windows.Forms.Timer { Interval = 1000 };
            _clock.Tick += (s, e) => UpdateTotals();
            _clock.Start();
        }

        private void UpdateTotals()
        {
            if (lblThoiGian == null) return;
            var elapsed = DateTime.Now - _phien.ThoiGianBatDau;
            double tienGio = Math.Ceiling(elapsed.TotalHours * _ban.GiaTheoGio / 1000) * 1000;
            lblThoiGian.Text = elapsed.ToString(@"hh\:mm\:ss");
            lblTienGio.Text = tienGio.ToString("N0") + " đ";
            lblTongTien.Text = tienGio.ToString("N0") + " đ";
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            _clock?.Stop();
            try
            {
                _phienBLL.KetThucPhien(_phien.MaPhien, DateTime.Now);
                _banBLL.CapNhatTrangThai(_ban.MaBan, "TRONG");
                IsPaid = true;
                MessageBox.Show(
                    $"Thanh toán thành công!\n\nBàn: {_ban.TenBan}\nSố tiền: {lblTongTien.Text}\n\nCảm ơn quý khách!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _clock?.Start();
            }
        }

        private GraphicsPath RoundRect(Rectangle b, int r)
        {
            var p = new GraphicsPath(); int d = r * 2;
            p.AddArc(b.X, b.Y, d, d, 180, 90);
            p.AddArc(b.Right - d, b.Y, d, d, 270, 90);
            p.AddArc(b.Right - d, b.Bottom - d, d, d, 0, 90);
            p.AddArc(b.X, b.Bottom - d, d, d, 90, 90);
            p.CloseFigure(); return p;
        }

        private GraphicsPath RoundRectTop(Rectangle b, int r)
        {
            var p = new GraphicsPath(); int d = r * 2;
            p.AddArc(b.X, b.Y, d, d, 180, 90);
            p.AddArc(b.Right - d, b.Y, d, d, 270, 90);
            p.AddLine(b.Right, b.Bottom, b.X, b.Bottom);
            p.CloseFigure(); return p;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clock?.Stop(); _clock?.Dispose();
            base.OnFormClosed(e);
        }
    }
}