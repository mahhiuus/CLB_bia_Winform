using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.Session;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class SoDoBanUi : Form
    {
        // ── BLL ──
        private readonly BanBidaBLL _banBLL = new BanBidaBLL();
        private readonly PhienChoiBLL _phienBLL = new PhienChoiBLL();
        private readonly ChiTietPhienBLL _chiTietPhienBLL = new ChiTietPhienBLL();
        private readonly SanPhamBLL _sanPhamBLL = new SanPhamBLL();

        // ── Colors ──
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_ACTIVE_BG = Color.FromArgb(232, 245, 232);
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        static readonly Color BORDER_IDLE = Color.FromArgb(210, 220, 210);

        // ── VIP Colors ──
        static readonly Color VIP_ACTIVE_BG = Color.FromArgb(51, 255, 145, 77); // #ff914d opacity 30%
        static readonly Color VIP_ACTIVE_TEXT = ColorTranslator.FromHtml("#f17100");
        static readonly Color VIP_ACTIVE_NONE = ColorTranslator.FromHtml("");

        // ── Tab state ──
        private string _activeTab = "THUONG"; // "THUONG" | "VIP"
        private Label _lblTabThuong, _lblTabVip;
        private Panel _underlineThuong, _underlineVip;

        // ── Grid panels ──
        private FlowLayoutPanel _gridThuong, _gridVip;
        private Panel _panelGridContainer;

        // ── Image cache ──
        private Image _imgActive, _imgDisable;

        // ── Event thông báo ra ngoài khi 1 bàn được mở phiên ──
        /// <summary>
        /// Bắn ra MaBan vừa được mở phiên (DANG_CHOI).
        /// Maindashboard subscribe để tự động chọn bàn đó ở MenuSanPham.
        /// </summary>
        public event EventHandler<string> BanDuocMo;

        public SoDoBanUi()
        {
            InitializeComponent();
            this.Load += SoDoBanUi_Load;
            this.VisibleChanged += SoDoBanUi_VisibleChanged;
        }

        private void SoDoBanUi_Load(object sender, EventArgs e)
        {
            LoadImages();
            BuildLayout();
            RefreshMap();
        }


        // ════════════════════════════════════════════════════════
        //  Load ảnh bàn bida
        // ════════════════════════════════════════════════════════
        private void LoadImages()
        {
            try
            {
                // Ưu tiên Resources, fallback sang thư mục image
                _imgActive = Bài_Tập_Lớn.Properties.Resources.BiaActived;
                _imgDisable = Bài_Tập_Lớn.Properties.Resources.BiaDisabled;
            }
            catch
            {
                try
                {
                    _imgActive = Image.FromFile("image/BiaActive.png");
                    _imgDisable = Image.FromFile("image/BiaDisable.png");
                }
                catch
                {
                    _imgActive = _imgDisable = null; // fallback placeholder
                }
            }
        }

        // ════════════════════════════════════════════════════════
        //  Build toàn bộ layout vào guna2Panel2
        // ════════════════════════════════════════════════════════
        private void BuildLayout()
        {
            guna2Panel2.Controls.Clear();
            guna2Panel2.BackColor = CREAM;
            guna2Panel2.Padding = new Padding(0);

            // Root layout: tab header + grid
            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                RowCount = 2,
                ColumnCount = 1
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 52f));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ── Tab header ──
            var tabHeader = BuildTabHeader();
            root.Controls.Add(tabHeader, 0, 0);

            // ── Grid container (scroll) ──
            _panelGridContainer = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

            // Grid Thường
            _gridThuong = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(24, 20, 24, 20)
            };

            // Grid VIP
            _gridVip = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(24, 20, 24, 20),
                Visible = false
            };

            _panelGridContainer.Controls.Add(_gridThuong);
            _panelGridContainer.Controls.Add(_gridVip);
            root.Controls.Add(_panelGridContainer, 0, 1);

            guna2Panel2.Controls.Add(root);
        }

        // ── Tab header với underline xanh custom ──
        private Panel BuildTabHeader()
        {
            var header = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = CREAM,
                Padding = new Padding(24, 0, 0, 0)
            };

            // Separator line dưới header
            var sepLine = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(220, 220, 220)
            };
            header.Controls.Add(sepLine);

            // Tab Thường
            var tabThuong = new Panel { Width = 180, Dock = DockStyle.Left, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _lblTabThuong = new Label
            {
                Text = "KV Bàn Thường",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = GREEN_DARK,
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            _underlineThuong = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = GREEN_DARK };
            tabThuong.Controls.Add(_lblTabThuong);
            tabThuong.Controls.Add(_underlineThuong);
            tabThuong.Click += (s, e) => SwitchTab("THUONG");
            _lblTabThuong.Click += (s, e) => SwitchTab("THUONG");
            _underlineThuong.Click += (s, e) => SwitchTab("THUONG");
            header.Controls.Add(tabThuong);

            // Tab VIP
            var tabVip = new Panel { Width = 160, Dock = DockStyle.Left, BackColor = Color.Transparent, Cursor = Cursors.Hand };
            _lblTabVip = new Label
            {
                Text = "KV Bàn VIP",
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.FromArgb(160, 160, 160),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };
            _underlineVip = new Panel { Dock = DockStyle.Bottom, Height = 3, BackColor = Color.Transparent };
            tabVip.Controls.Add(_lblTabVip);
            tabVip.Controls.Add(_underlineVip);
            tabVip.Click += (s, e) => SwitchTab("VIP");
            _lblTabVip.Click += (s, e) => SwitchTab("VIP");
            _underlineVip.Click += (s, e) => SwitchTab("VIP");
            header.Controls.Add(tabVip);

            return header;
        }

        private void SwitchTab(string tab)
        {
            _activeTab = tab;

            bool isThuong = tab == "THUONG";

            // Tab Thường
            _lblTabThuong.Font = new Font("Segoe UI", 11, isThuong ? FontStyle.Bold : FontStyle.Regular);
            _lblTabThuong.ForeColor = isThuong ? GREEN_DARK : Color.FromArgb(160, 160, 160);
            _underlineThuong.BackColor = isThuong ? GREEN_DARK : Color.Transparent;

            // Tab VIP
            _lblTabVip.Font = new Font("Segoe UI", 11, !isThuong ? FontStyle.Bold : FontStyle.Regular);
            _lblTabVip.ForeColor = !isThuong ? GREEN_DARK : Color.FromArgb(160, 160, 160);
            _underlineVip.BackColor = !isThuong ? GREEN_DARK : Color.Transparent;

            // Hiện/ẩn grid
            _gridThuong.Visible = isThuong;
            _gridVip.Visible = !isThuong;
        }

        // ════════════════════════════════════════════════════════
        //  Refresh: load data từ DB và render card
        // ════════════════════════════════════════════════════════
        public void RefreshMap()
        {
            _gridThuong.Controls.Clear();
            _gridVip.Controls.Clear();

            try
            {
                List<BanBidaDTO> dsBan = _banBLL.LayTatCaBan();
                foreach (var ban in dsBan)
                {
                    var card = CreateBanCard(ban);
                    if (ban.LoaiBan?.ToUpper() == "VIP")
                        _gridVip.Controls.Add(card);
                    else
                        _gridThuong.Controls.Add(card);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu bàn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        //  Tạo card 1 bàn — 5 cột cố định, tự tính width
        // ════════════════════════════════════════════════════════
        private Panel CreateBanCard(BanBidaDTO ban)
        {
            bool isActive = ban.TrangThai?.ToUpper() == "DANG_CHOI";
            bool isVip = ban.LoaiBan?.ToUpper() == "VIP";

            // Cấu hình màu nền, chữ, viền linh hoạt theo VIP và trạng thái
            Color currentActiveBg = isVip ? VIP_ACTIVE_BG : GREEN_ACTIVE_BG;
            Color currentActiveText = isVip ? VIP_ACTIVE_TEXT : GREEN_DARK;
            Color currentActiveBorder = isVip ? VIP_ACTIVE_NONE : GREEN_LIGHT;

            // Tính width card = (panel width - padding*2 - gap*4) / 5
            int panelW = guna2Panel2.Width > 0 ? guna2Panel2.Width : 900;
            int cardW = Math.Max(140, (panelW - 48 - 4 * 16) / 5);
            int cardH = (int)(cardW * 1.2);

            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                Margin = new Padding(8),
                BackColor = isActive ? currentActiveBg : Color.White,
                Cursor = Cursors.Hand,
                Tag = ban
            };

            // Border bo tròn — vẽ bằng Paint
            card.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Color borderColor = isActive ? currentActiveBorder : BORDER_IDLE;
                int bw = isActive ? 2 : 1;
                using (var pen = new Pen(borderColor, bw))
                using (var path = RoundedPath(new Rectangle(1, 1, card.Width - 2, card.Height - 2), 14))
                    g.DrawPath(pen, path);

                // Clip vùng bo tròn
                using (var clip = RoundedPath(new Rectangle(0, 0, card.Width, card.Height), 14))
                    g.SetClip(clip);

                // Nền
                using (var bg = new SolidBrush(card.BackColor))
                    g.FillRectangle(bg, card.ClientRectangle);
            };

            // ── Ảnh bàn bida ──
            int imgW = (int)(cardW * 0.80);
            int imgH = (int)(imgW * 0.58);
            var picBox = new PictureBox
            {
                Size = new Size(imgW, imgH),
                Location = new Point((cardW - imgW) / 2, (int)(cardH * 0.08)),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Image = isActive ? _imgActive : _imgDisable
            };
            // Placeholder nếu không có ảnh
            if (picBox.Image == null)
            {
                picBox.Paint += (s, e) =>
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    using (var b = new SolidBrush(isActive ? Color.FromArgb(200, currentActiveBorder) : Color.FromArgb(200, 200, 200)))
                        e.Graphics.FillEllipse(b, 10, 10, imgW - 20, imgH - 20);
                };
            }
            card.Controls.Add(picBox);

            // ── Tên bàn ──
            int lblY = picBox.Bottom + 6;
            var lblName = new Label
            {
                Text = ban.TenBan,
                Location = new Point(0, lblY),
                Size = new Size(cardW, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = isActive ? currentActiveText : Color.FromArgb(180, 120, 10),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblName);

            // ── Trạng thái ──
            var lblStatus = new Label
            {
                Text = isActive ? "(đang chơi)" : "(trống)",
                Location = new Point(0, lblName.Bottom + 1),
                Size = new Size(cardW, 24), // Đã tăng chiều cao lên 24
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = isActive ? currentActiveText : Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblStatus);

            // ── Click handler ──
            EventHandler onClick = (s, e) => HandleCardClick(ban, isActive);
            card.Click += onClick;
            picBox.Click += onClick;
            lblName.Click += onClick;
            lblStatus.Click += onClick;

            // Hover effect
            card.MouseEnter += (s, e) =>
            {
                if (isActive)
                {
                    card.BackColor = isVip ? Color.FromArgb(100, 255, 145, 77) : Color.FromArgb(210, 240, 210);
                }
                else
                {
                    card.BackColor = Color.FromArgb(245, 250, 245);
                }
            };

            card.MouseLeave += (s, e) => card.BackColor = isActive ? currentActiveBg : Color.White;

            return card;
        }

        // ════════════════════════════════════════════════════════
        //  Xử lý click bàn
        // ════════════════════════════════════════════════════════
        private void HandleCardClick(BanBidaDTO ban, bool isActive)
        {
            if (!isActive)
            {
                // Bàn trống → bắt đầu tính giờ
                var result = MessageBox.Show(
                    $"Bắt đầu tính giờ cho {ban.TenBan}?\nGiá: {ban.GiaTheoGio:N0} đ/giờ",
                    "Xác nhận mở bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var phien = new PhienChoiDTO
                        {
                            MaPhien = _phienBLL.SinhMaMoi(),
                            MaBan = ban.MaBan,
                            MaNV = SessionManager.Instance.TaiKhoanHienTai?.MaNV,
                            ThoiGianBatDau = DateTime.Now,
                            TrangThai = "DANG_CHOI"
                        };
                        _phienBLL.ThemPhien(phien);
                        _banBLL.CapNhatTrangThai(ban.MaBan, "DANG_CHOI");
                        RefreshMap();
                        // Thông báo ra ngoài để MenuSanPham cập nhật ComboBox
                        BanDuocMo?.Invoke(this, ban.MaBan);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi mở bàn: " + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                // Bàn đang chơi → nạp đủ dữ liệu rồi mở ThanhToanDialog
                try
                {
                    var phien = _phienBLL.TimPhienDangChoiTheoBan(ban.MaBan);
                    if (phien == null)
                    {
                        var fix = MessageBox.Show(
                            "Bàn đang chơi nhưng không tìm thấy phiên trong DB.\nĐặt lại thành Trống?",
                            "Lỗi dữ liệu", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (fix == DialogResult.Yes)
                        {
                            _banBLL.CapNhatTrangThai(ban.MaBan, "TRONG");
                            RefreshMap();
                        }
                        return;
                    }

                    // ── Nạp chi tiết phiên ──────────────────────────────
                    var dsChiTiet = _chiTietPhienBLL.TimTheoMaPhien(phien.MaPhien)
                                    ?? new List<ChiTietPhienDTO>();

                    // ── Build cache tên SP ───────────────────────────────
                    var cacheTenSP = new Dictionary<string, string>();
                    try
                    {
                        var dsSP = _sanPhamBLL.TimKiem("") ?? new List<SanPhamDTO>();
                        foreach (var sp in dsSP)
                            if (!string.IsNullOrEmpty(sp.MaSP))
                                cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;
                    }
                    catch { /* fallback: cache rỗng, PDF dùng MaSP */ }
                    var chiTietHoaDonBanBLL = new Bài_Tập_Lớn.BLL.ChiTietHoaDonBanBLL();
                    // ── Mở ThanhToanDialog đầy đủ tham số ───────────────
                    using (var dialog = new ThanhToanDialog(ban, phien, dsChiTiet, cacheTenSP))
                    {
                        // THÊM: Hiện Overlay làm tối nền trước khi mở dialog
                        dialog.ShowOverlay(this);

                        dialog.ShowDialog(this);
                        if (dialog.IsPaid)
                        {
                            if (dialog.HoaDonDaTao != null && dsChiTiet != null)
                            {
                                foreach (var item in dsChiTiet)
                                {
                                    var cthd = new ChiTietHoaDonBanDTO
                                    {
                                        MaHDB = dialog.HoaDonDaTao.MaHDB, // Lấy mã hóa đơn vừa được Dialog tạo thành công
                                        MaSP = item.MaSP,
                                        SoLuong = item.SoLuong,
                                        DonGiaBan = item.DonGia // Đơn giá từ chi tiết phiên chuyển sang đơn giá bán
                                    };

                                    // Gọi xuống tầng BLL để thực thi lệnh lưu vào SQL Server
                                    chiTietHoaDonBanBLL.ThemChiTiet(cthd);
                                }
                            }
                            RefreshMap();
                            // Hiển thị hóa đơn dạng preview (giống giao diện ThanhToanDialog)
                            if (dialog.HoaDonDaTao != null)
                                HienThiHoaDonSauThanhToan(dialog.HoaDonDaTao, ban, phien, dsChiTiet, cacheTenSP);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ════════════════════════════════════════════════════════
        //  Hiển thị hóa đơn sau khi thanh toán xong
        //  (mở lại ThanhToanDialog ở chế độ preview — chỉ đọc)
        // ════════════════════════════════════════════════════════
        private void HienThiHoaDonSauThanhToan(
            HoaDonBanDTO hoaDon,
            BanBidaDTO ban,
            PhienChoiDTO phien,
            List<ChiTietPhienDTO> dsChiTiet,
            Dictionary<string, string> cacheTenSP)
        {
            using (var preview = new ThanhToanDialog(ban, phien, dsChiTiet, cacheTenSP, hoaDon))
            {
                // THÊM: Hiện Overlay cho màn hình preview hóa đơn
                preview.ShowOverlay(this);
                preview.ShowDialog(this);
            }
        }

        // ── Helper bo tròn ──
        private GraphicsPath RoundedPath(Rectangle bounds, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void SoDoBanUi_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                RefreshMap();
            }
        }

        // ── Stub event handlers ──
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}