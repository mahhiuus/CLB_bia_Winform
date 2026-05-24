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
        // ── Public result ──
        public bool IsPaid { get; private set; } = false;

        // ── Data ──
        private readonly BanBidaDTO _ban;
        private readonly PhienChoiDTO _phien;
        private readonly PhienChoiBLL _phienBLL = new PhienChoiBLL();
        private readonly BanBidaBLL _banBLL = new BanBidaBLL();

        // ── Live timer ──
        private System.Windows.Forms.Timer _clock;
        private Label lblThoiGian, lblTienGio, lblTongTien;

        // ── Colors ──
        static readonly Color GREEN_DARK  = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color CREAM       = Color.FromArgb(255, 255, 251);
        static readonly Color GRAY_TEXT   = Color.FromArgb(90, 90, 90);

        public ThanhToanDialog(BanBidaDTO ban, PhienChoiDTO phien)
        {
            _ban   = ban;
            _phien = phien;
            BuildUI();
            StartClock();
        }

        private void BuildUI()
        {
            // ── Form ──
            Text            = "Thanh Toán — " + _ban.TenBan;
            Size            = new Size(460, 520);
            StartPosition   = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.None;
            BackColor       = CREAM;
            ShowInTaskbar   = false;

            // Bo tròn form
            int r = 20;
            var path = new GraphicsPath();
            path.AddArc(0, 0, r*2, r*2, 180, 90);
            path.AddArc(Width-r*2, 0, r*2, r*2, 270, 90);
            path.AddArc(Width-r*2, Height-r*2, r*2, r*2, 0, 90);
            path.AddArc(0, Height-r*2, r*2, r*2, 90, 90);
            path.CloseFigure();
            Region = new Region(path);

            // ── Header ──
            var header = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = GREEN_DARK };
            var headerPath = new GraphicsPath();
            headerPath.AddArc(0, 0, r*2, r*2, 180, 90);
            headerPath.AddArc(header.Width - r*2, 0, r*2, r*2, 270, 90);
            headerPath.AddLine(header.Width, header.Height, 0, header.Height);
            headerPath.CloseFigure();
            header.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var b = new SolidBrush(GREEN_DARK))
                using (var hp = new GraphicsPath()) {
                    hp.AddArc(0, 0, r*2, r*2, 180, 90);
                    hp.AddArc(header.Width-r*2, 0, r*2, r*2, 270, 90);
                    hp.AddLine(header.Width, header.Height, 0, header.Height);
                    hp.CloseFigure();
                    e.Graphics.FillPath(b, hp);
                }
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                using (var font = new Font("Segoe UI", 16, FontStyle.Bold))
                    e.Graphics.DrawString("💳  THANH TOÁN", font, Brushes.White, new RectangleF(0,0,header.Width,header.Height), sf);
            };

            // Nút đóng X
            var btnClose = new Button {
                Text = "✕", Size = new Size(32,32),
                Location = new Point(Width-44, 18),
                FlatStyle = FlatStyle.Flat, BackColor = Color.Transparent,
                ForeColor = Color.White, Font = new Font("Segoe UI",10,FontStyle.Bold), Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s,e) => Close();
            header.Controls.Add(btnClose);
            Controls.Add(header);

            // ── Body ──
            var body = new TableLayoutPanel {
                Dock = DockStyle.Fill, BackColor = Color.Transparent,
                Padding = new Padding(30, 16, 30, 20),
                RowCount = 7, ColumnCount = 2
            };
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            body.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            for (int i = 0; i < 6; i++)
                body.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            body.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            int row = 0;

            // Tên bàn
            AddRow(body, row++, "🎱  Bàn", _ban.TenBan + $" ({_ban.LoaiBan})", boldRight: true);

            // Giờ bắt đầu
            AddRow(body, row++, "⏱  Bắt đầu", _phien.ThoiGianBatDau.ToString("HH:mm:ss  dd/MM/yyyy"));

            // Thời gian chơi (live)
            lblThoiGian = MakeValueLabel("--:--:--");
            AddRowWithLabel(body, row++, "🕐  Thời gian", lblThoiGian);

            // Giá / giờ
            AddRow(body, row++, "💰  Giá/giờ", _ban.GiaTheoGio.ToString("N0") + " đ");

            // Separator
            var sep = new Panel { Height = 1, BackColor = Color.FromArgb(220,220,220), Margin = new Padding(0,8,0,8) };
            body.Controls.Add(sep, 0, row);
            body.SetColumnSpan(sep, 2);
            row++;

            // Tiền giờ chơi (live)
            lblTienGio = MakeValueLabel("0 đ", GREEN_DARK, 13, FontStyle.Bold);
            AddRowWithLabel(body, row++, "⏳  Tiền giờ", lblTienGio);

            // Tổng tiền (live)
            lblTongTien = MakeValueLabel("0 đ", GREEN_DARK, 16, FontStyle.Bold);
            AddRowWithLabel(body, row++, "💵  TỔNG TIỀN", lblTongTien, labelBig: true);

            Controls.Add(body);

            // ── Footer: nút thanh toán ──
            var footer = new Panel { Dock = DockStyle.Bottom, Height = 70, BackColor = Color.Transparent, Padding = new Padding(30,10,30,10) };

            var btnPay = new Button {
                Text = "✔  XÁC NHẬN THANH TOÁN",
                Dock = DockStyle.Fill, FlatStyle = FlatStyle.Flat,
                BackColor = GREEN_DARK, ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.Region = new Region(RoundedPath(btnPay.ClientRectangle, 10));
            btnPay.Paint += (s,e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (var b = new SolidBrush(GREEN_DARK))
                using (var p = RoundedPath(new Rectangle(0,0,btnPay.Width,btnPay.Height), 10))
                    e.Graphics.FillPath(b, p);
                using (var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    e.Graphics.DrawString(btnPay.Text, btnPay.Font, Brushes.White, new RectangleF(0,0,btnPay.Width,btnPay.Height), sf);
            };
            btnPay.Click += BtnPay_Click;
            footer.Controls.Add(btnPay);
            Controls.Add(footer);

            // Kéo form bằng header
            bool dragging = false; Point dragStart = Point.Empty;
            header.MouseDown += (s,e) => { dragging = true; dragStart = e.Location; };
            header.MouseMove += (s,e) => { if(dragging) Location = new Point(Location.X+e.X-dragStart.X, Location.Y+e.Y-dragStart.Y); };
            header.MouseUp   += (s,e) => dragging = false;
        }

        private void StartClock()
        {
            UpdateTotals();
            _clock = new System.Windows.Forms.Timer { Interval = 1000 };
            _clock.Tick += (s, e) => UpdateTotals();
            _clock.Start();
        }

        private void UpdateTotals()
        {
            if (lblThoiGian == null || lblTienGio == null || lblTongTien == null) return;
            TimeSpan elapsed = DateTime.Now - _phien.ThoiGianBatDau;
            double hours     = elapsed.TotalHours;
            double tienGio   = Math.Ceiling(hours * _ban.GiaTheoGio / 1000) * 1000; // làm tròn 1000đ

            lblThoiGian.Text = elapsed.ToString(@"hh\:mm\:ss");
            lblTienGio.Text  = tienGio.ToString("N0") + " đ";
            lblTongTien.Text = tienGio.ToString("N0") + " đ";  // có thể cộng thêm đồ ăn sau
        }

        private void BtnPay_Click(object sender, EventArgs e)
        {
            _clock?.Stop();
            DateTime now = DateTime.Now;
            try
            {
                _phienBLL.KetThucPhien(_phien.MaPhien, now);
                _banBLL.CapNhatTrangThai(_ban.MaBan, "TRONG");
                IsPaid = true;
                MessageBox.Show("Thanh toán thành công!\nCảm ơn quý khách.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _clock?.Start();
            }
        }

        private void AddRow(TableLayoutPanel tbl, int row, string label, string value, bool boldRight = false)
        {
            tbl.Controls.Add(MakeLabelLeft(label), 0, row);
            tbl.Controls.Add(MakeValueLabel(value, boldValue: boldRight), 1, row);
        }

        private void AddRowWithLabel(TableLayoutPanel tbl, int row, string label, Label valueCtrl, bool labelBig = false)
        {
            var lbl = MakeLabelLeft(label, labelBig);
            tbl.Controls.Add(lbl, 0, row);
            tbl.Controls.Add(valueCtrl, 1, row);
        }

        private Label MakeLabelLeft(string text, bool big = false) => new Label {
            Text = text, AutoSize = true, Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", big ? 12 : 10, big ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = GRAY_TEXT, Margin = new Padding(0, 8, 0, 8),
            TextAlign = ContentAlignment.MiddleLeft
        };

        private Label MakeValueLabel(string text, Color? color = null, float size = 11,
            FontStyle style = FontStyle.Regular, bool boldValue = false) => new Label {
            Text = text, AutoSize = true, Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", size, boldValue ? FontStyle.Bold : style),
            ForeColor = color ?? GRAY_TEXT, Margin = new Padding(0, 8, 0, 8),
            TextAlign = ContentAlignment.MiddleRight
        };

        private GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            var p = new GraphicsPath();
            int d = radius * 2;
            p.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            p.AddArc(bounds.Right-d, bounds.Y, d, d, 270, 90);
            p.AddArc(bounds.Right-d, bounds.Bottom-d, d, d, 0, 90);
            p.AddArc(bounds.X, bounds.Bottom-d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clock?.Stop(); _clock?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
