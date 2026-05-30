using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    //  MENU SAN PHAM  (v2 — có Right Bar)
    //  – Hiển thị sản phẩm dạng card lưới
    //  – Lọc theo Loại, Tìm kiếm, Phân trang 16 card/trang
    //  – Right bar 260px: ComboBox chọn bàn → hiển thị món đã gọi
    //    trong phiên đang chơi → tổng tiền SP → nút Thanh Toán
    //  – Nút "+" card → thêm vào ChiTietPhien CỦA bàn đang chọn
    //                 → đồng thời bắn event SanPhamDuocChon ra form cha
    // ═══════════════════════════════════════════════════════════════
    public partial class MenuSanPham : Form
    {
        // ── Màu sắc ──────────────────────────────────────────────
        internal static readonly Color CLR_PRIMARY = Color.FromArgb(43, 78, 35);
        internal static readonly Color CLR_ACCENT = Color.FromArgb(121, 174, 111);
        internal static readonly Color CLR_BG = Color.FromArgb(255, 255, 251);
        internal static readonly Color CLR_GREY = Color.FromArgb(245, 245, 242);
        internal static readonly Color CLR_GREY2 = Color.FromArgb(235, 235, 232);
        internal static readonly Color CLR_BORDER = Color.FromArgb(215, 215, 210);
        internal static readonly Color CLR_TEXT_SUB = Color.FromArgb(140, 140, 135);

        // ── BLL ──────────────────────────────────────────────────
        private readonly SanPhamBLL _bll = new SanPhamBLL();
        private readonly BanBidaBLL _banBll = new BanBidaBLL();
        private readonly PhienChoiBLL _phienBll = new PhienChoiBLL();
        private readonly ChiTietPhienBLL _chiTietPhienBll = new ChiTietPhienBLL();

        // ── Dữ liệu sản phẩm ─────────────────────────────────────
        private List<SanPhamDTO> _dsDayDu = new List<SanPhamDTO>();
        private List<SanPhamDTO> _dsHienThi = new List<SanPhamDTO>();

        // ── Phân trang ────────────────────────────────────────────
        private int _trangHienTai = 1;
        private const int SO_CARD_MOI_TRANG = 16;
        private int TongSoTrang => Math.Max(1,
            (int)Math.Ceiling((double)_dsHienThi.Count / SO_CARD_MOI_TRANG));

        // ── Pager controls ───────────────────────────────────────
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        // ── Tab lọc Loại ─────────────────────────────────────────
        private static readonly string[] DS_LOAI_LABEL =
            { "Tất cả", "Đồ ăn", "Đồ uống", "Dụng cụ" };
        private static readonly string[] DS_LOAI_VALUE =
            { "",       "DO_AN", "DO_UONG", "DUNG_CU" };
        private readonly Guna2Button[] _tabBtns;
        private int _tabChon = 0;

        // ── Right bar state ───────────────────────────────────────
        /// <summary>Bàn đang được chọn ở right bar (null = chưa chọn)</summary>
        private BanBidaDTO _banDangChon = null;
        /// <summary>Phiên đang chơi của bàn đang chọn (null = bàn trống)</summary>
        private PhienChoiDTO _phienHienTai = null;
        /// <summary>Danh sách chi tiết món đã gọi trong phiên hiện tại</summary>
        private List<ChiTietPhienDTO> _dsChiTiet = new List<ChiTietPhienDTO>();
        /// <summary>Cache tên SP theo MaSP để hiển thị trong right bar</summary>
        private Dictionary<string, string> _cacheTenSP = new Dictionary<string, string>();

        // ── Event ra ngoài ────────────────────────────────────────
        /// <summary>
        /// Form cha subscribe event này để nhận SanPhamDTO khi người dùng nhấn "+" trên card.
        /// </summary>
        public event EventHandler<SanPhamDTO> SanPhamDuocChon;

        // ═════════════════════════════════════════════════════════
        //  Khởi tạo
        // ═════════════════════════════════════════════════════════
        public MenuSanPham()
        {
            _tabBtns = new Guna2Button[DS_LOAI_LABEL.Length];
            InitializeComponent();
            TaoTabLoc();
            TaoPhanTrang();
            this.Load += (s, e) =>
            {
                NapDanhSachBan();
                TaiDanhSach();
                CapNhatTabUI(0);
                RefreshMap();
            };
        }

        private void RefreshMap()
        {
            // Lưu lại bàn đang chọn để khôi phục sau khi nạp lại danh sách
            string maBanCu = _banDangChon?.MaBan;

            // Nạp lại dữ liệu bàn và sản phẩm
            NapDanhSachBan();
            TaiDanhSach();

            // Khôi phục lại lựa chọn ComboBox nếu bàn cũ vẫn tồn tại
            if (!string.IsNullOrEmpty(maBanCu))
            {
                foreach (ComboItemBan item in cboBan.Items)
                {
                    if (item.MaBan == maBanCu)
                    {
                        cboBan.SelectedItem = item;
                        break;
                    }
                }
            }

            // Làm mới lại giỏ hàng (Right bar) dựa trên bàn đang chọn
            TaiChiTietPhienHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  NẠP DANH SÁCH BÀN vào ComboBox
        // ══════════════════════════════════════════════════════════
        private void NapDanhSachBan()
        {
            try
            {
                cboBan.Items.Clear();
                cboBan.Items.Add(new ComboItemBan { MaBan = "", TenHienThi = "-- Chọn bàn --" });

                var dsBan = _banBll.LayTatCaBan() ?? new List<BanBidaDTO>();
                foreach (var ban in dsBan)
                {
                    // Chỉ hiện bàn đang active (DANG_CHOI)
                    if (!string.Equals(ban.TrangThai, "DANG_CHOI", StringComparison.OrdinalIgnoreCase))
                        continue;

                    cboBan.Items.Add(new ComboItemBan
                    {
                        MaBan = ban.MaBan,
                        TenHienThi = ban.TenBan + " 🟢",
                        BanDto = ban,
                    });
                }

                if (cboBan.Items.Count > 0)
                    cboBan.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("NapDanhSachBan lỗi: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  CHỌN BÀN TỪ BÊN NGOÀI (gọi từ Maindashboard)
        // ══════════════════════════════════════════════════════════
        /// <summary>
        /// Maindashboard gọi hàm này sau khi SoDoBanUi mở 1 bàn,
        /// để ComboBox tự động select đúng bàn đó.
        /// </summary>
        public void ChonBan(string maBan)
        {
            // Nạp lại danh sách bàn trước (phiên vừa mở cần có trong list)
            NapDanhSachBan();

            if (string.IsNullOrEmpty(maBan)) return;

            foreach (ComboItemBan item in cboBan.Items)
            {
                if (item.MaBan == maBan)
                {
                    cboBan.SelectedItem = item;
                    return;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        //  SỰ KIỆN: ComboBox chọn bàn
        // ══════════════════════════════════════════════════════════
        private void cboBan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBan.SelectedItem is ComboItemBan item && !string.IsNullOrEmpty(item.MaBan))
            {
                _banDangChon = item.BanDto;
                TaiChiTietPhienHienTai();
            }
            else
            {
                _banDangChon = null;
                _phienHienTai = null;
                _dsChiTiet = new List<ChiTietPhienDTO>();
                HienThiDonHang();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  TẢI chi tiết phiên đang chơi của bàn đang chọn
        // ══════════════════════════════════════════════════════════
        private void TaiChiTietPhienHienTai()
        {
            try
            {
                if (_banDangChon == null)
                {
                    _phienHienTai = null;
                    _dsChiTiet = new List<ChiTietPhienDTO>();
                    HienThiDonHang();
                    return;
                }

                // Tìm phiên đang chơi của bàn (TrangThai = "Đang chơi")
                _phienHienTai = _phienBll.TimPhienDangChoiTheoBan(_banDangChon.MaBan);

                if (_phienHienTai != null)
                {
                    _dsChiTiet = _chiTietPhienBll.TimTheoMaPhien(_phienHienTai.MaPhien)
                                 ?? new List<ChiTietPhienDTO>();
                }
                else
                {
                    _dsChiTiet = new List<ChiTietPhienDTO>();
                }

                HienThiDonHang();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("TaiChiTietPhienHienTai lỗi: " + ex.Message);
                _dsChiTiet = new List<ChiTietPhienDTO>();
                HienThiDonHang();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  HIỂN THỊ đơn hàng trong right bar
        // ══════════════════════════════════════════════════════════
        private void HienThiDonHang()
        {
            flowOrderItems.SuspendLayout();
            flowOrderItems.Controls.Clear();

            if (_phienHienTai == null || _dsChiTiet.Count == 0)
            {
                // Hiện thông báo trống
                var lbl = new Label
                {
                    Text = _banDangChon == null
                                ? "Chọn bàn để xem đơn hàng"
                                : "Bàn chưa có phiên hoặc chưa gọi món",
                    Font = new Font("Segoe UI", 9f),
                    ForeColor = CLR_TEXT_SUB,
                    AutoSize = false,
                    Width = flowOrderItems.Width - 20,
                    Height = 48,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                };
                flowOrderItems.Controls.Add(lbl);
                lblTongTienVal.Text = "0 ₫";
                flowOrderItems.ResumeLayout();
                return;
            }

            double tongTien = 0;
            foreach (var ct in _dsChiTiet)
            {
                // Lấy tên SP từ cache hoặc BLL
                string tenSP = LayTenSP(ct.MaSP);

                var row = TaoOrderRow(ct, tenSP);
                flowOrderItems.Controls.Add(row);
                tongTien += ct.SoLuong * ct.DonGia;
            }

            lblTongTienVal.Text = tongTien.ToString("N0") + " ₫";
            flowOrderItems.ResumeLayout();
        }

        /// <summary>Tạo 1 dòng hiển thị 1 món trong right bar</summary>
        private Panel TaoOrderRow(ChiTietPhienDTO ct, string tenSP)
        {
            var row = new Panel
            {
                Width = flowOrderItems.Width - 20,
                Height = 44,
                BackColor = Color.White,
                Padding = new Padding(0),
            };

            // Nút xóa (−)
            var btnXoa = new Guna2Button
            {
                Text = "−",
                Size = new Size(26, 26),
                Location = new Point(row.Width - 30, 9),
                BorderRadius = 6,
                FillColor = Color.FromArgb(255, 235, 235),
                ForeColor = Color.FromArgb(200, 60, 60),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };
            btnXoa.HoverState.FillColor = Color.FromArgb(220, 60, 60);
            btnXoa.HoverState.ForeColor = Color.White;
            string maCTP = ct.MaCTP; // capture
            btnXoa.Click += (s, e) => XoaChiTietPhien(maCTP);

            // Tên SP
            var lblTen = new Label
            {
                Text = tenSP,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 40, 40),
                Location = new Point(4, 4),
                Size = new Size(row.Width - 80, 20),
                AutoSize = false,
                BackColor = Color.Transparent,
            };

            // SL × giá
            var lblGia = new Label
            {
                Text = $"{ct.SoLuong} × {ct.DonGia:N0} ₫",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = CLR_TEXT_SUB,
                Location = new Point(4, 24),
                Size = new Size(row.Width - 80, 16),
                AutoSize = false,
                BackColor = Color.Transparent,
            };

            // Thành tiền
            var lblThanhTien = new Label
            {
                Text = (ct.SoLuong * ct.DonGia).ToString("N0") + " ₫",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = CLR_PRIMARY,
                Location = new Point(row.Width - 100, 4),
                Size = new Size(66, 20),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
            };

            // Đường kẻ dưới
            var line = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = CLR_BORDER,
            };

            row.Controls.Add(btnXoa);
            row.Controls.Add(lblThanhTien);
            row.Controls.Add(lblGia);
            row.Controls.Add(lblTen);
            row.Controls.Add(line);

            return row;
        }

        // ══════════════════════════════════════════════════════════
        //  XÓA 1 món khỏi phiên
        // ══════════════════════════════════════════════════════════
        private void XoaChiTietPhien(string maCTP)
        {
            try
            {
                _chiTietPhienBll.XoaChiTietPhien(maCTP);
                TaiChiTietPhienHienTai(); // reload right bar
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa món:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  THANH TOÁN
        // ══════════════════════════════════════════════════════════
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_phienHienTai == null)
            {
                MessageBox.Show("Bàn này chưa có phiên đang chơi!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_dsChiTiet.Count == 0)
            {
                MessageBox.Show("Chưa có món nào trong đơn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // TODO: Mở form thanh toán hoặc xử lý nghiệp vụ thanh toán ở đây
            // Ví dụ: new FormThanhToan(_phienHienTai, _dsChiTiet).ShowDialog();
            MessageBox.Show(
                $"Thanh toán bàn: {_banDangChon.TenBan}\n" +
                $"Phiên: {_phienHienTai.MaPhien}\n" +
                $"Tổng tiền SP: {lblTongTienVal.Text}",
                "Thanh Toán",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        // ══════════════════════════════════════════════════════════
        //  NHẬN SỰ KIỆN từ card "+" → thêm vào phiên + bắn event
        // ══════════════════════════════════════════════════════════
        private void Card_OnThemVaoGio(object sender, SanPhamDTO sp)
        {
            // 1. Thêm vào ChiTietPhien nếu đang có phiên
            if (_phienHienTai != null)
            {
                ThemVaoChiTietPhien(sp);
            }
            else if (_banDangChon != null)
            {
                MessageBox.Show(
                    $"Bàn \"{_banDangChon.TenBan}\" chưa có phiên đang chơi.\nKhởi động phiên trước rồi thêm món.",
                    "Chưa có phiên", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            // Nếu chưa chọn bàn → chỉ bắn event ra form cha (không lưu DB)

            // 2. Bắn event ra form cha
            SanPhamDuocChon?.Invoke(this, sp);
        }

        /// <summary>Thêm hoặc cộng dồn SL vào ChiTietPhien</summary>
        private void ThemVaoChiTietPhien(SanPhamDTO sp)
        {
            try
            {
                // Kiểm tra đã có trong phiên chưa
                var existing = _dsChiTiet.FirstOrDefault(
                    ct => ct.MaSP == sp.MaSP && ct.MaPhien == _phienHienTai.MaPhien);

                if (existing != null)
                {
                    // Cộng thêm 1 số lượng
                    existing.SoLuong += 1;
                    _chiTietPhienBll.CapNhatChiTietPhien(existing);
                }
                else
                {
                    // Thêm mới
                    var ct = new ChiTietPhienDTO
                    {
                        MaCTP = _chiTietPhienBll.SinhMaMoi(),
                        MaPhien = _phienHienTai.MaPhien,
                        MaSP = sp.MaSP,
                        SoLuong = 1,
                        DonGia = sp.GiaBan,
                    };
                    _chiTietPhienBll.ThemChiTietPhien(ct);

                    // Cache tên SP
                    if (!_cacheTenSP.ContainsKey(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;
                }

                // Reload right bar
                TaiChiTietPhienHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm món vào phiên:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Lấy tên SP từ cache (tránh gọi DB liên tục)
        // ══════════════════════════════════════════════════════════
        private string LayTenSP(string maSP)
        {
            if (_cacheTenSP.TryGetValue(maSP, out string ten))
                return ten;

            try
            {
                var ds = _bll.TimKiem(maSP);
                var sp = ds?.FirstOrDefault(x => x.MaSP == maSP);
                string tenSP = sp?.TenSP ?? maSP;
                _cacheTenSP[maSP] = tenSP;
                return tenSP;
            }
            catch
            {
                _cacheTenSP[maSP] = maSP;
                return maSP;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  TẢI DANH SÁCH SẢN PHẨM (giữ nguyên logic cũ)
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                _dsDayDu = _bll.TimKiem("") ?? new List<SanPhamDTO>();
                // Build cache tên SP từ toàn bộ danh sách luôn
                foreach (var sp in _dsDayDu)
                    if (!string.IsNullOrEmpty(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;

                LocVaHienThi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LocVaHienThi()
        {
            string loaiFilter = DS_LOAI_VALUE[_tabChon];
            string keyword = (txtTimKiem?.Text ?? "").Trim().ToLower();

            _dsHienThi = _dsDayDu.Where(sp =>
                (string.IsNullOrEmpty(loaiFilter) ||
                 string.Equals(sp.Loai, loaiFilter, StringComparison.OrdinalIgnoreCase))
                &&
                (string.IsNullOrEmpty(keyword) ||
                 (sp.TenSP ?? "").ToLower().Contains(keyword))
            ).ToList();

            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        private void HienThiTrangHienTai()
        {
            flowCards.SuspendLayout();

            foreach (Control c in flowCards.Controls)
                if (c is MenuSanPhamCard old)
                    old.OnThemVaoGio -= Card_OnThemVaoGio;

            flowCards.Controls.Clear();

            var dsTrang = _dsHienThi
                .Skip((_trangHienTai - 1) * SO_CARD_MOI_TRANG)
                .Take(SO_CARD_MOI_TRANG)
                .ToList();

            if (dsTrang.Count == 0)
            {
                flowCards.Controls.Add(new Label
                {
                    Text = "Không tìm thấy sản phẩm nào.",
                    Font = new Font("Segoe UI", 11f),
                    ForeColor = CLR_TEXT_SUB,
                    AutoSize = true,
                    Margin = new Padding(24, 30, 0, 0),
                });
            }
            else
            {
                foreach (var sp in dsTrang)
                {
                    var card = new MenuSanPhamCard();
                    card.NapDuLieu(sp);
                    card.OnThemVaoGio += Card_OnThemVaoGio;
                    flowCards.Controls.Add(card);
                }
            }

            flowCards.ResumeLayout();
            CapNhatPhanTrang();
        }

        // ══════════════════════════════════════════════════════════
        //  Tab lọc loại (giữ nguyên)
        // ══════════════════════════════════════════════════════════
        private void TaoTabLoc()
        {
            int x = 0;
            for (int i = 0; i < DS_LOAI_LABEL.Length; i++)
            {
                int idx = i;
                var btn = new Guna2Button
                {
                    Text = DS_LOAI_LABEL[i],
                    Size = new Size(118, 34),
                    Location = new Point(x, 20),
                    BorderRadius = 10,
                    Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    FillColor = CLR_GREY,
                    ForeColor = CLR_TEXT_SUB,
                    BorderColor = CLR_BORDER,
                    BorderThickness = 1,
                };
                btn.HoverState.FillColor = CLR_ACCENT;
                btn.HoverState.ForeColor = Color.White;
                btn.Click += (s, e) =>
                {
                    _tabChon = idx;
                    CapNhatTabUI(idx);
                    LocVaHienThi();
                };
                _tabBtns[i] = btn;
                panelTabLoc.Controls.Add(btn);
                x += 126;
            }
        }

        private void CapNhatTabUI(int activeIdx)
        {
            for (int i = 0; i < _tabBtns.Length; i++)
            {
                if (_tabBtns[i] == null) continue;
                bool on = (i == activeIdx);
                _tabBtns[i].FillColor = on ? CLR_PRIMARY : CLR_GREY;
                _tabBtns[i].ForeColor = on ? Color.White : CLR_TEXT_SUB;
                _tabBtns[i].BorderColor = on ? CLR_PRIMARY : CLR_BORDER;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Phân trang (giữ nguyên)
        // ══════════════════════════════════════════════════════════
        private void TaoPhanTrang()
        {
            _btnPrev = new Guna2Button
            {
                Text = "‹",
                Size = new Size(34, 30),
                Location = new Point(8, 7),
                BorderRadius = 8,
                FillColor = CLR_GREY2,
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = CLR_BORDER,
                BorderThickness = 1,
            };
            _btnPrev.HoverState.FillColor = CLR_ACCENT;
            _btnPrev.HoverState.ForeColor = Color.White;
            _btnPrev.Click += (s, e) => ChuyenTrang(_trangHienTai - 1);

            _lblTrangInfo = new Label
            {
                Text = "Trang 1 / 1",
                Size = new Size(120, 30),
                Location = new Point(48, 7),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = CLR_PRIMARY,
                BackColor = Color.Transparent,
            };

            _btnNext = new Guna2Button
            {
                Text = "›",
                Size = new Size(34, 30),
                Location = new Point(174, 7),
                BorderRadius = 8,
                FillColor = CLR_GREY2,
                ForeColor = Color.FromArgb(70, 70, 70),
                Font = new Font("Segoe UI", 13f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = CLR_BORDER,
                BorderThickness = 1,
            };
            _btnNext.HoverState.FillColor = CLR_ACCENT;
            _btnNext.HoverState.ForeColor = Color.White;
            _btnNext.Click += (s, e) => ChuyenTrang(_trangHienTai + 1);

            panelPager.Controls.Add(_btnPrev);
            panelPager.Controls.Add(_lblTrangInfo);
            panelPager.Controls.Add(_btnNext);
        }

        private void ChuyenTrang(int trang)
        {
            if (trang < 1 || trang > TongSoTrang) return;
            _trangHienTai = trang;
            HienThiTrangHienTai();
        }

        private void CapNhatPhanTrang()
        {
            _lblTrangInfo.Text = $"Trang {_trangHienTai} / {TongSoTrang}";
            SetPagerBtn(_btnPrev, _trangHienTai > 1);
            SetPagerBtn(_btnNext, _trangHienTai < TongSoTrang);
        }

        private static void SetPagerBtn(Guna2Button btn, bool enabled)
        {
            btn.Enabled = enabled;
            btn.FillColor = enabled ? Color.FromArgb(235, 235, 232) : Color.FromArgb(220, 220, 218);
            btn.ForeColor = enabled ? Color.FromArgb(70, 70, 70) : Color.FromArgb(190, 190, 188);
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện toolbar (giữ nguyên)
        // ══════════════════════════════════════════════════════════
        private void btnReload_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            _tabChon = 0;
            CapNhatTabUI(0);
            NapDanhSachBan();       // refresh combobox bàn luôn
            TaiDanhSach();
            TaiChiTietPhienHienTai(); // refresh right bar
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => LocVaHienThi();

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) LocVaHienThi();
        }

        // ── Event stubs ───────────────────────────────────────────
        private void panelTabLoc_Paint(object sender, PaintEventArgs e) { }
        private void panelPager_Paint(object sender, PaintEventArgs e) { }
        private void flowCards_Paint(object sender, PaintEventArgs e) { }

        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            int radius = 10; // Độ bo tròn 10px
            int d = radius * 2;

            var path = new System.Drawing.Drawing2D.GraphicsPath();

            // Vẽ 4 góc bo tròn
            path.AddArc(0, 0, d, d, 180, 90); // Góc trên trái
            path.AddArc(panelHeader.Width - d, 0, d, d, 270, 90); // Góc trên phải
            path.AddArc(panelHeader.Width - d, panelHeader.Height - d, d, d, 0, 90); // Góc dưới phải
            path.AddArc(0, panelHeader.Height - d, d, d, 90, 90); // Góc dưới trái
            path.CloseFigure();

            // Cắt panel theo đường cong vừa vẽ
            panelHeader.Region = new Region(path);

            // (Tùy chọn) Nếu bạn muốn vẽ thêm chữ tiêu đề như các form trước, bạn có thể thêm code DrawString ở ngay dưới dòng này.
        }
    }

    // ══════════════════════════════════════════════════════════════
    //  Helper class cho ComboBox bàn
    // ══════════════════════════════════════════════════════════════
    internal class ComboItemBan
    {
        public string MaBan { get; set; }
        public string TenHienThi { get; set; }
        public BanBidaDTO BanDto { get; set; }

        public override string ToString() => TenHienThi;
    }
}