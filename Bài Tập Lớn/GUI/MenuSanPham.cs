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
    //  MENU SAN PHAM  (v3 — liên kết ThanhToanDialog)
    //  THAY ĐỔI SO VỚI v2:
    //  1. btnThanhToan_Click → mở ThanhToanDialog thật sự
    //     (truyền _ban, _phien, _dsChiTiet, _cacheTenSP)
    //  2. Sau khi dialog đóng (IsPaid = true):
    //     – Hiển thị thông báo thành công kèm mã hóa đơn + tổng tiền
    //     – Refresh comboBox bàn (bỏ bàn vừa thanh toán vì trạng thái → TRONG)
    //     – Xóa đơn hàng right bar (reset về trạng thái chưa chọn bàn)
    //  3. btnThanhToan chỉ Enabled khi: có phiên đang chơi VÀ ≥ 1 món
    //     → CapNhatNutThanhToan() được gọi sau mỗi lần HienThiDonHang()
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
        private const int SO_CARD_MOI_TRANG = 8;
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
        /// <summary>Cache tên SP theo MaSP để hiển thị trong right bar VÀ truyền sang ThanhToanDialog</summary>
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
            // Lưu lại bàn đang chọn để khôi phục sau khi nạp lại
            string maBanCu = _banDangChon?.MaBan;

            NapDanhSachBan();
            TaiDanhSach();

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
        public void ChonBan(string maBan)
        {
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

                // ── [MỚI] Cập nhật trạng thái nút Thanh Toán ──
                CapNhatNutThanhToan();
                return;
            }

            double tongTien = 0;
            foreach (var ct in _dsChiTiet)
            {
                string tenSP = LayTenSP(ct.MaSP);
                var row = TaoOrderRow(ct, tenSP);
                flowOrderItems.Controls.Add(row);
                tongTien += ct.SoLuong * ct.DonGia;
            }

            lblTongTienVal.Text = tongTien.ToString("N0") + " ₫";
            flowOrderItems.ResumeLayout();

            // ── [MỚI] Cập nhật trạng thái nút Thanh Toán ──
            CapNhatNutThanhToan();
        }

        // ══════════════════════════════════════════════════════════
        //  CẬP NHẬT NÚT GIÁ TIỀN (chỉ hiển thị, không click)
        // ══════════════════════════════════════════════════════════
        private void CapNhatNutThanhToan()
        {
            double tongTien = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            bool coMon = _phienHienTai != null && _dsChiTiet.Count > 0;

            btnThanhToan.Enabled = false;
            btnThanhToan.Text = coMon
                ? tongTien.ToString("N0") + " ₫"
                : "0 ₫";
            btnThanhToan.FillColor = coMon
                ? CLR_ACCENT
                : Color.FromArgb(215, 215, 210);
            btnThanhToan.ForeColor = coMon
                ? Color.White
                : Color.FromArgb(160, 160, 158);
            btnThanhToan.Cursor = Cursors.Default;
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
                TaiChiTietPhienHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa món:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  btnThanhToan chỉ là hiển thị giá — không xử lý click
        // ══════════════════════════════════════════════════════════
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Nút chỉ trang trí, không làm gì
        }

        // ══════════════════════════════════════════════════════════
        //  NHẬN SỰ KIỆN từ card "+" → thêm vào phiên + bắn event
        // ══════════════════════════════════════════════════════════
        private void Card_OnThemVaoGio(object sender, SanPhamDTO sp)
        {
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

            SanPhamDuocChon?.Invoke(this, sp);
        }

        /// <summary>Thêm hoặc cộng dồn SL vào ChiTietPhien</summary>
        private void ThemVaoChiTietPhien(SanPhamDTO sp)
        {
            try
            {
                var existing = _dsChiTiet.FirstOrDefault(
                    ct => ct.MaSP == sp.MaSP && ct.MaPhien == _phienHienTai.MaPhien);

                if (existing != null)
                {
                    existing.SoLuong += 1;
                    _chiTietPhienBll.CapNhatChiTietPhien(existing);
                }
                else
                {
                    var ct = new ChiTietPhienDTO
                    {
                        MaCTP = _chiTietPhienBll.SinhMaMoi(),
                        MaPhien = _phienHienTai.MaPhien,
                        MaSP = sp.MaSP,
                        SoLuong = 1,
                        DonGia = sp.GiaBan,
                    };
                    _chiTietPhienBll.ThemChiTietPhien(ct);

                    if (!_cacheTenSP.ContainsKey(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;
                }

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
        //  TẢI DANH SÁCH SẢN PHẨM
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                _dsDayDu = _bll.TimKiem("") ?? new List<SanPhamDTO>();
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
        //  Tab lọc loại
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
        //  Phân trang
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
        //  Sự kiện toolbar
        // ══════════════════════════════════════════════════════════
        private void btnReload_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            _tabChon = 0;
            CapNhatTabUI(0);
            NapDanhSachBan();
            TaiDanhSach();
            TaiChiTietPhienHienTai();
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
            int radius = 10;
            int d = radius * 2;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(panelHeader.Width - d, 0, d, d, 270, 90);
            path.AddArc(panelHeader.Width - d, panelHeader.Height - d, d, d, 0, 90);
            path.AddArc(0, panelHeader.Height - d, d, d, 90, 90);
            path.CloseFigure();
            panelHeader.Region = new Region(path);
        }

        private void tableLayoutOuter_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutMain_Paint(object sender, PaintEventArgs e) { }
        private void panelToolbar_Paint(object sender, PaintEventArgs e) { }
        private void spacer1_Paint(object sender, PaintEventArgs e) { }
        private void txtTimKiem_TextChanged(object sender, EventArgs e) { }
        private void panelCardWrap_Paint(object sender, PaintEventArgs e) { }
        private void panelRightBar_Paint(object sender, PaintEventArgs e) { }
        private void tlRight_Paint(object sender, PaintEventArgs e) { }
        private void panelRightHeader_Paint(object sender, PaintEventArgs e) { }
        private void lblRightTitle_Click(object sender, EventArgs e) { }
        private void panelSelectBan_Paint(object sender, PaintEventArgs e) { }
        private void lblChonBan_Click(object sender, EventArgs e) { }
        private void panelDonHang_Paint(object sender, PaintEventArgs e) { }
        private void panelOrderList_Paint(object sender, PaintEventArgs e) { }
        private void flowOrderItems_Paint(object sender, PaintEventArgs e) { }
        private void lblDonHangTitle_Click(object sender, EventArgs e) { }
        private void panelRightFooter_Paint(object sender, PaintEventArgs e) { }
        private void lblTongTienVal_Click(object sender, EventArgs e) { }
        private void lblTongTien_Click(object sender, EventArgs e) { }
        private void separatorFooter_Paint(object sender, PaintEventArgs e) { }
        private void lblTitle_Click(object sender, EventArgs e) { }
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