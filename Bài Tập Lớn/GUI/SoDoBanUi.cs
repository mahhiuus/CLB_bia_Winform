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
        //  Update 1 card tại chỗ — không reload toàn bộ grid
        // ════════════════════════════════════════════════════════
        private void UpdateCardInPlace(string maBan)
        {
            try
            {
                // Lấy trạng thái mới nhất từ DB
                var dsBan = _banBLL.LayTatCaBan();
                var banMoi = dsBan.FirstOrDefault(b => b.MaBan == maBan);
                if (banMoi == null) return;

                var grid = banMoi.LoaiBan?.ToUpper() == "VIP" ? _gridVip : _gridThuong;

                // Tìm Card hiện tại đang hiển thị
                Panel card = grid.Controls.OfType<Panel>().FirstOrDefault(p => p.Tag is BanBidaDTO b && b.MaBan == maBan);
                if (card == null) return;

                // Cập nhật Dữ liệu ngầm cho thẻ
                card.Tag = banMoi;

                bool isActive = banMoi.TrangThai?.ToUpper() == "DANG_CHOI";
                bool isVip = banMoi.LoaiBan?.ToUpper() == "VIP";

                Color currentActiveBg = isVip ? VIP_ACTIVE_BG : GREEN_ACTIVE_BG;
                Color currentActiveText = isVip ? VIP_ACTIVE_TEXT : GREEN_DARK;
                Color idleBg = Color.FromArgb(255, 255, 251);

                // 1. Đổi màu nền Thẻ
                card.BackColor = isActive ? currentActiveBg : idleBg;

                // 2. Đổi Hình ảnh (Bật/Tắt đèn bàn)
                if (card.Controls["picBox"] is PictureBox picBox)
                    picBox.Image = isActive ? _imgActive : _imgDisable;

                // 3. Đổi Màu chữ Tên bàn
                if (card.Controls["lblName"] is Label lblName)
                    lblName.ForeColor = isActive ? currentActiveText : Color.FromArgb(180, 120, 10);

                // 4. Đổi Text và Màu chữ Trạng thái
                if (card.Controls["lblStatus"] is Label lblStatus)
                {
                    lblStatus.Text = isActive ? "(đang chơi)" : "(trống)";
                    lblStatus.ForeColor = isActive ? currentActiveText : Color.FromArgb(150, 150, 150);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật thẻ UI: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        // ════════════════════════════════════════════════════════
                 //  Tạo card 1 bàn — 5 cột cố định, tự tính width
                 // ════════════════════════════════════════════════════════
                 // Thêm 2 tham số fixedW và fixedH vào chữ ký hàm
        private Panel CreateBanCard(BanBidaDTO ban)
        {
            bool isActive = ban.TrangThai?.ToUpper() == "DANG_CHOI";
            bool isVip = ban.LoaiBan?.ToUpper() == "VIP";

            Color currentActiveBg = isVip ? VIP_ACTIVE_BG : GREEN_ACTIVE_BG;
            Color currentActiveText = isVip ? VIP_ACTIVE_TEXT : GREEN_DARK;
            Color idleBg = Color.FromArgb(255, 255, 251);

            // ── Kích thước panel cha ──────────────────────────────────────
            int panelW = (guna2Panel2 != null && guna2Panel2.Width > 10)
                         ? guna2Panel2.Width
                         : 960;  // fallback an toàn

            int calculatedW = (panelW - 48) / 5 - 16;
            int cardW = Math.Max(140, Math.Min(calculatedW, 220));
            int cardH = (int)(cardW * 1.2);

            // ── Tính margin căn giữa ─────────────────────────────────────
            int widthKhaDung = Math.Max(panelW - 48, 1); // tránh âm
            int cardPlusGap = cardW + 16;
            int soCot = Math.Max(1, widthKhaDung / cardPlusGap); // KHÔNG BAO GIỜ = 0

            int khoangTrong = widthKhaDung - (cardW * soCot);
            int denominator = soCot * 2;
            int marginLR = denominator > 0
                               ? Math.Max(8, khoangTrong / denominator)
                               : 8; // fallback nếu vẫn lạ

            // ── Tạo card ─────────────────────────────────────────────────
            var card = new Panel
            {
                Width = cardW,
                Height = cardH,
                Margin = new Padding(marginLR, 15, marginLR, 15),
                BackColor = isActive ? currentActiveBg : idleBg,
                Cursor = Cursors.Hand,
                Tag = ban
            };

            // ── Bo góc an toàn ───────────────────────────────────────────
            try
            {
                using (var pathRegion = RoundedPath(new Rectangle(0, 0, cardW, cardH), 14))
                {
                    if (pathRegion != null)
                        card.Region = new Region(pathRegion);
                }
            }
            catch { /* bỏ qua nếu bo góc lỗi, card vẫn hiện */ }

            // ── Ảnh bàn bida ─────────────────────────────────────────────
            int imgW = (int)(cardW * 0.80);
            int imgH = (int)(imgW * 0.58);
            var picBox = new PictureBox
            {
                Name = "picBox",
                Size = new Size(imgW, imgH),
                Location = new Point((cardW - imgW) / 2, (int)(cardH * 0.08)),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Image = isActive ? _imgActive : _imgDisable
            };
            card.Controls.Add(picBox);

            // ── Tên bàn ──────────────────────────────────────────────────
            int lblY = picBox.Bottom + 6;
            var lblName = new Label
            {
                Name = "lblName",
                Text = ban.TenBan,
                Location = new Point(0, lblY),
                Size = new Size(cardW, 22),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = isActive ? currentActiveText : Color.FromArgb(180, 120, 10),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblName);

            // ── Trạng thái ───────────────────────────────────────────────
            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = isActive ? "(đang chơi)" : "(trống)",
                Location = new Point(0, lblName.Bottom + 1),
                Size = new Size(cardW, 24),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = isActive ? currentActiveText : Color.FromArgb(150, 150, 150),
                BackColor = Color.Transparent
            };
            card.Controls.Add(lblStatus);


            // ── Click & Hover ─────────────────────────────────────────────
            EventHandler onClick = (s, e) => HandleCardClick(ban, ban.TrangThai?.ToUpper() == "DANG_CHOI");
            card.Click += onClick;
            picBox.Click += onClick;
            lblName.Click += onClick;
            lblStatus.Click += onClick;

            card.MouseEnter += (s, e) =>
            {
                bool active = ((BanBidaDTO)card.Tag).TrangThai?.ToUpper() == "DANG_CHOI";
                card.BackColor = active
                    ? (isVip ? Color.FromArgb(100, 255, 145, 77) : Color.FromArgb(210, 240, 210))
                    : Color.FromArgb(245, 250, 245);
            };
            card.MouseLeave += (s, e) =>
            {
                bool active = ((BanBidaDTO)card.Tag).TrangThai?.ToUpper() == "DANG_CHOI";
                card.BackColor = active ? currentActiveBg : idleBg;
            };

            return card;
        }                 // ════════════════════════════════════════════════════════
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

                        // Cập nhật trạng thái object trên RAM để lần click sau không bị hỏi mở bàn nữa
                        ban.TrangThai = "DANG_CHOI"; // <--- THÊM DÒNG NÀY

                        // Chỉ update đúng card này, không reload toàn bộ
                        UpdateCardInPlace(ban.MaBan);

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

                            // Sửa cả trên bộ nhớ để tránh kẹt trạng thái
                            ban.TrangThai = "TRONG"; // <--- THÊM DÒNG NÀY

                            // Chỉ update đúng card này, không reload toàn bộ
                            UpdateCardInPlace(ban.MaBan);
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
                                        MaHDB = dialog.HoaDonDaTao.MaHDB,
                                        MaSP = item.MaSP,
                                        SoLuong = item.SoLuong,
                                        DonGiaBan = item.DonGia
                                    };
                                    chiTietHoaDonBanBLL.ThemChiTiet(cthd);
                                }
                            }

                            // Cập nhật lại object thành TRỐNG sau khi đã thanh toán thành công
                            ban.TrangThai = "TRONG"; // <--- THÊM DÒNG NÀY

                            // Đã sửa dòng này từ RefreshMap() thành UpdateCardInPlace
                            UpdateCardInPlace(ban.MaBan);

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
        }        // ════════════════════════════════════════════════════════
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
                // Nếu giao diện chưa có thẻ nào thì mới Load mới toàn bộ
                if (_gridThuong.Controls.Count == 0 && _gridVip.Controls.Count == 0)
                {
                    RefreshMap();
                }
                else
                {
                    // Nếu đã có thẻ rồi, chỉ cập nhật trạng thái (Bật/Tắt) tại chỗ
                    try
                    {
                        var dsBan = _banBLL.LayTatCaBan();
                        foreach (var ban in dsBan)
                        {
                            UpdateCardInPlace(ban.MaBan);
                        }
                    }
                    catch
                    {
                        // Fallback an toàn nếu có lỗi ngầm
                        RefreshMap();
                    }
                }
            }
        }
        // ── Stub event handlers ──
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}