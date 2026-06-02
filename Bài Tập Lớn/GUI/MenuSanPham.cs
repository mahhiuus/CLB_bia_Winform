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
    //  MENU SAN PHAM  (v4 — no full re-render)
    //  THAY ĐỔI SO VỚI v3:
    //  1. Card sản phẩm KHÔNG bị reload khi thêm món:
    //     – Cập nhật label tồn kho trực tiếp trên card hiện có
    //     – Không gọi HienThiTrangHienTai() sau khi thêm món
    //  2. Right bar cập nhật tại chỗ (patch), KHÔNG rebuild toàn bộ:
    //     – PatchDonHang(maSP): tìm row đang có → chỉ đổi text SL + thành tiền
    //     – ThemDongMoiVaoDon(ct): thêm 1 dòng mới vào cuối, không xóa list
    //  3. Nút − giảm SoLuong 1; nếu = 0 mới xóa khỏi DB
    //  4. _tonKhoTam: giữ nguyên logic RAM, không trừ DB khi order
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
        private BanBidaDTO _banDangChon = null;
        private PhienChoiDTO _phienHienTai = null;
        private List<ChiTietPhienDTO> _dsChiTiet = new List<ChiTietPhienDTO>();
        private Dictionary<string, string> _cacheTenSP = new Dictionary<string, string>();

        // ── Tồn kho tạm trong RAM (không trừ DB khi order) ───────
        // Key = MaSP, Value = số lượng đã order nhưng chưa thanh toán
        private Dictionary<string, int> _tonKhoTam = new Dictionary<string, int>();

        // ── Event ra ngoài ────────────────────────────────────────
        public event EventHandler<SanPhamDTO> SanPhamDuocChon;

        // ── Tag dùng để nhận diện các sub-control bên trong row ──
        private const string TAG_LBL_SL = "lbl_sl";      // "2 × 15,000 ₫"
        private const string TAG_LBL_THANH = "lbl_thanh";   // "30,000 ₫"

        // ══════════════════════════════════════════════════════════
        //  [THÊM MỚI] POLLING TIMER — real-time sync qua mạng LAN
        //  Cơ chế: mỗi 5 giây fetch DB 1 lần, so sánh với snapshot
        //  cũ. Nếu số lượng SP hoặc thông tin SP thay đổi (thêm/
        //  sửa/xóa từ máy khác) → tự động reload danh sách card.
        //  Không rebuild right bar vì right bar phụ thuộc phiên chơi
        //  (không bị ảnh hưởng bởi thay đổi danh mục sản phẩm).
        //  ──────────────────────────────────────────────────────────
        //  [THÊM MỚI v2] PHÁT HIỆN THANH TOÁN — trong cùng Tick:
        //  Nếu bàn đang chọn có phiên, kiểm tra TimPhienDangChoiTheoBan.
        //  Nếu trả về null hoặc MaPhien khác → phiên đã đóng (đã
        //  thanh toán từ máy khác) → tự reset combobox về index 0,
        //  clear right bar, cập nhật tồn kho card.
        // ══════════════════════════════════════════════════════════
        private System.Windows.Forms.Timer _timerPolling;
        private const int POLLING_INTERVAL_MS = 5000; // 5 giây

        /// <summary>
        /// Snapshot hash để phát hiện thay đổi mà không cần so sánh
        /// từng field. Hash = ghép MaSP|TenSP|GiaBan|SoLuongTon|Loai
        /// của toàn bộ danh sách, sort theo MaSP cho ổn định.
        /// </summary>
        private string _snapshotHash = "";

        // ═════════════════════════════════════════════════════════
        //  Khởi tạo
        // ═════════════════════════════════════════════════════════
        public MenuSanPham()
        {
            _tabBtns = new Guna2Button[DS_LOAI_LABEL.Length];
            InitializeComponent();
            TaoTabLoc();
            TaoPhanTrang();

            // [THÊM MỚI] Khởi tạo polling timer
            KhoiTaoPollingTimer();

            this.Load += (s, e) =>
            {
                NapDanhSachBan();
                TaiDanhSach();
                CapNhatTabUI(0);
                RefreshMap();

                // [THÊM MỚI] Bắt đầu polling sau khi form load xong
                _timerPolling.Start();
            };

            // [THÊM MỚI] Dừng timer khi form đóng, tránh memory leak
            this.FormClosed += (s, e) => _timerPolling?.Stop();
        }

        // ══════════════════════════════════════════════════════════
        //  [THÊM MỚI] KHỞI TẠO POLLING TIMER
        // ══════════════════════════════════════════════════════════
        private void KhoiTaoPollingTimer()
        {
            _timerPolling = new System.Windows.Forms.Timer
            {
                Interval = POLLING_INTERVAL_MS,
                Enabled = false,
            };
            _timerPolling.Tick += PollingTimer_Tick;
        }

        /// <summary>
        /// Tick mỗi 5 giây:
        /// [1] Kiểm tra phiên đang chọn — nếu đã thanh toán từ máy
        ///     khác thì reset combobox về '-- Chọn bàn --' và clear
        ///     right bar, cập nhật tồn kho tất cả card.
        /// [2] Fetch DB sản phẩm → tính hash → nếu khác snapshot cũ
        ///     thì cập nhật _dsDayDu và reload card (giữ trang hiện tại
        ///     nếu có thể, giữ nguyên _tonKhoTam).
        /// </summary>
        private void PollingTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // ══ [THÊM MỚI] Kiểm tra phiên đang chọn có bị thanh toán không ══
                if (_banDangChon != null && _phienHienTai != null)
                {
                    var phienCheck = _phienBll.TimPhienDangChoiTheoBan(_banDangChon.MaBan);

                    // Phiên đã đóng nếu: không còn phiên active (null)
                    // hoặc MaPhien đã đổi (phiên mới được tạo sau khi thanh toán)
                    bool daThanhToan = (phienCheck == null)
                        || !string.Equals(phienCheck.MaPhien, _phienHienTai.MaPhien,
                                          StringComparison.OrdinalIgnoreCase);

                    if (daThanhToan)
                    {
                        // Reset toàn bộ state phiên/bàn
                        _banDangChon = null;
                        _phienHienTai = null;
                        _dsChiTiet = new List<ChiTietPhienDTO>();
                        _tonKhoTam.Clear();

                        // Cập nhật tồn kho card theo DB thật
                        CapNhatTatCaCardSauThanhToan();

                        // Reset combobox + right bar phải chạy trên UI thread
                        // (Windows.Forms.Timer Tick đã chạy trên UI thread,
                        //  nhưng giữ Invoke để an toàn nếu code được di chuyển)
                        if (cboBan.InvokeRequired)
                            cboBan.Invoke(new Action(ResetComboVaRightBar));
                        else
                            ResetComboVaRightBar();

                        return; // Không cần check hash SP trong tick này
                    }
                }

                // ══ (giữ nguyên) Kiểm tra thay đổi danh sách SP ══
                var dsMoi = _bll.TimKiem("") ?? new List<SanPhamDTO>();
                string hashMoi = TinhSnapshotHash(dsMoi);

                if (hashMoi == _snapshotHash)
                    return; // Không có gì thay đổi → bỏ qua

                // Có thay đổi → cập nhật cache tên SP
                foreach (var sp in dsMoi)
                    if (!string.IsNullOrEmpty(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;

                // Cập nhật _dsDayDu (giữ _tonKhoTam nguyên vẹn)
                _dsDayDu = dsMoi;
                _snapshotHash = hashMoi;

                // Reload card — giữ trang hiện tại nếu còn hợp lệ
                LocVaHienThi_GiuTrang();
            }
            catch
            {
                // Bỏ qua lỗi network tạm thời, tick sau sẽ thử lại
            }
        }

        // ══════════════════════════════════════════════════════════
        //  [THÊM MỚI] RESET COMBO VÀ RIGHT BAR SAU KHI THANH TOÁN
        //  Gọi trên UI thread khi polling phát hiện phiên đã đóng.
        //  - NapDanhSachBan(): bàn vừa thanh toán sẽ tự biến mất
        //    vì TrangThai không còn là DANG_CHOI.
        //  - cboBan.SelectedIndex = 0: về '-- Chọn bàn --'.
        //  - HienThiDonHang(): reset right bar về trạng thái trống.
        // ══════════════════════════════════════════════════════════
        private void ResetComboVaRightBar()
        {
            // Nạp lại danh sách bàn — bàn đã thanh toán tự biến mất
            NapDanhSachBan();

            // Về '-- Chọn bàn --' (index 0)
            if (cboBan.Items.Count > 0)
                cboBan.SelectedIndex = 0;

            // Clear right bar (giống khi chọn '-- Chọn bàn --')
            HienThiDonHang();
        }

        /// <summary>
        /// Tính hash đơn giản từ danh sách SP để phát hiện thay đổi.
        /// Sort theo MaSP để hash ổn định dù DB trả về thứ tự khác.
        /// </summary>
        private static string TinhSnapshotHash(List<SanPhamDTO> ds)
        {
            var parts = ds
                .OrderBy(sp => sp.MaSP)
                .Select(sp =>
                    $"{sp.MaSP}|{sp.TenSP}|{sp.GiaBan}|{sp.SoLuongTon}|{sp.Loai}|{sp.HinhAnh}");
            return string.Join(";", parts);
        }

        /// <summary>
        /// Giống LocVaHienThi() nhưng cố giữ trang hiện tại nếu còn
        /// hợp lệ sau khi filter. Tránh nhảy về trang 1 mỗi lần poll.
        /// </summary>
        private void LocVaHienThi_GiuTrang()
        {
            string loaiFilter = DS_LOAI_VALUE[_tabChon];
            string keyword = (txtTimKiem?.Text ?? "").Trim().ToLower();

            _dsHienThi = _dsDayDu.Where(sp =>
                (string.IsNullOrEmpty(loaiFilter) ||
                 string.Equals(sp.Loai, loaiFilter, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(keyword) ||
                 (sp.TenSP ?? "").ToLower().Contains(keyword))
            ).ToList();

            // Giữ trang cũ nếu còn hợp lệ, nếu vượt quá thì về trang cuối
            if (_trangHienTai > TongSoTrang)
                _trangHienTai = Math.Max(1, TongSoTrang);

            HienThiTrangHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  (giữ nguyên toàn bộ code cũ bên dưới)
        // ══════════════════════════════════════════════════════════

        private void RefreshMap()
        {
            string maBanCu = _banDangChon?.MaBan;
            NapDanhSachBan();
            TaiDanhSach();
            if (!string.IsNullOrEmpty(maBanCu))
            {
                foreach (ComboItemBan item in cboBan.Items)
                {
                    if (item.MaBan == maBanCu) { cboBan.SelectedItem = item; break; }
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
                if (cboBan.Items.Count > 0) cboBan.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("NapDanhSachBan lỗi: " + ex.Message);
            }
        }

        public void ChonBan(string maBan)
        {
            NapDanhSachBan();
            if (string.IsNullOrEmpty(maBan)) return;
            foreach (ComboItemBan item in cboBan.Items)
                if (item.MaBan == maBan) { cboBan.SelectedItem = item; return; }
        }

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
                _tonKhoTam.Clear();
                HienThiDonHang();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  TẢI chi tiết phiên đang chơi — chỉ gọi khi cần rebuild
        //  toàn bộ right bar (đổi bàn, reload, thanh toán xong)
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
                _dsChiTiet = _phienHienTai != null
                    ? (_chiTietPhienBll.TimTheoMaPhien(_phienHienTai.MaPhien) ?? new List<ChiTietPhienDTO>())
                    : new List<ChiTietPhienDTO>();

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
        //  HIỂN THỊ ĐƠN HÀNG — rebuild toàn bộ right bar
        //  Chỉ gọi khi: đổi bàn / reload / thanh toán xong
        //  KHÔNG gọi khi: thêm / trừ món (dùng Patch thay thế)
        // ══════════════════════════════════════════════════════════
        private void HienThiDonHang()
        {
            flowOrderItems.SuspendLayout();
            flowOrderItems.Controls.Clear();

            if (_phienHienTai == null || _dsChiTiet.Count == 0)
            {
                flowOrderItems.Controls.Add(new Label
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
                });
                lblTongTienVal.Text = "0 ₫";
                flowOrderItems.ResumeLayout();
                CapNhatNutThanhToan();
                return;
            }

            foreach (var ct in _dsChiTiet)
                flowOrderItems.Controls.Add(TaoOrderRow(ct, LayTenSP(ct.MaSP)));

            flowOrderItems.ResumeLayout();
            CapNhatTongTien();
            CapNhatNutThanhToan();
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] PATCH RIGHT BAR — cập nhật tại chỗ, không rebuild
        // ══════════════════════════════════════════════════════════

        /// <summary>
        /// Gọi sau khi ThemVaoChiTietPhien thành công.
        /// Nếu MaSP đã có row → cập nhật text tại chỗ.
        /// Nếu chưa có → thêm row mới vào cuối.
        /// </summary>
        private void PatchThemMon(ChiTietPhienDTO ct)
        {
            var row = TimRowTheoMaSP(ct.MaSP);
            if (row != null)
            {
                // Cập nhật text tại chỗ
                CapNhatTextRow(row, ct);
            }
            else
            {
                // Thêm row mới vào cuối (không xóa list)
                // Nếu đang hiển thị label "chưa gọi món" → xóa label đó trước
                XoaLabelTrong();
                flowOrderItems.Controls.Add(TaoOrderRow(ct, LayTenSP(ct.MaSP)));
            }

            CapNhatTongTien();
            CapNhatNutThanhToan();
        }

        /// <summary>
        /// Gọi sau khi GiamHoacXoaChiTietPhien thành công.
        /// ct == null  → dòng đã bị xóa khỏi DB → xóa row.
        /// ct != null  → cập nhật text tại chỗ.
        /// </summary>
        private void PatchXoaMon(string maSP, ChiTietPhienDTO ct)
        {
            var row = TimRowTheoMaSP(maSP);
            if (row == null) return;

            if (ct == null)
            {
                // Xóa hẳn row
                flowOrderItems.Controls.Remove(row);
                row.Dispose();

                // Nếu không còn món nào → hiện label trống
                if (flowOrderItems.Controls.Count == 0)
                    HienThiDonHang();
            }
            else
            {
                CapNhatTextRow(row, ct);
            }

            CapNhatTongTien();
            CapNhatNutThanhToan();
        }

        // ── Tìm Panel row theo MaSP (dùng row.Name = maSP) ───────
        private Panel TimRowTheoMaSP(string maSP)
        {
            foreach (Control c in flowOrderItems.Controls)
                if (c is Panel p && p.Name == maSP)
                    return p;
            return null;
        }

        // ── Xóa label "chưa gọi món" nếu còn tồn tại ─────────────
        private void XoaLabelTrong()
        {
            var labels = flowOrderItems.Controls.OfType<Label>().ToList();
            foreach (var l in labels) { flowOrderItems.Controls.Remove(l); l.Dispose(); }
        }

        // ── Cập nhật text của 2 label bên trong row ───────────────
        private static void CapNhatTextRow(Panel row, ChiTietPhienDTO ct)
        {
            foreach (Control c in row.Controls)
            {
                if (c.Tag as string == TAG_LBL_SL)
                    c.Text = $"{ct.SoLuong} × {ct.DonGia:N0} ₫";
                else if (c.Tag as string == TAG_LBL_THANH)
                    c.Text = (ct.SoLuong * ct.DonGia).ToString("N0") + " ₫";
            }
        }

        // ── Tính lại tổng tiền từ _dsChiTiet ─────────────────────
        private void CapNhatTongTien()
        {
            double tong = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            lblTongTienVal.Text = tong.ToString("N0") + " ₫";
        }

        // ══════════════════════════════════════════════════════════
        //  TẠO 1 DÒNG MÓN trong right bar
        //  row.Name = MaSP  ← dùng để tra cứu khi patch
        // ══════════════════════════════════════════════════════════
        private Panel TaoOrderRow(ChiTietPhienDTO ct, string tenSP)
        {
            var row = new Panel
            {
                Name = ct.MaSP,           // ← key để TimRowTheoMaSP
                Width = flowOrderItems.Width - 20,
                Height = 44,
                BackColor = Color.White,
            };

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
            string maSPCapture = ct.MaSP;
            btnXoa.Click += (s, e) => GiamHoacXoaMon(maSPCapture);

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

            // TAG để patch tìm được
            var lblGia = new Label
            {
                Text = $"{ct.SoLuong} × {ct.DonGia:N0} ₫",
                Tag = TAG_LBL_SL,
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
                Tag = TAG_LBL_THANH,
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
        //  CẬP NHẬT NÚT THANH TOÁN (chỉ hiển thị giá, không click)
        // ══════════════════════════════════════════════════════════
        private void CapNhatNutThanhToan()
        {
            double tong = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            bool coMon = _phienHienTai != null && _dsChiTiet.Count > 0;

            btnThanhToan.Enabled = false;
            btnThanhToan.Text = coMon ? tong.ToString("N0") + " ₫" : "0 ₫";
            btnThanhToan.FillColor = coMon ? CLR_ACCENT : Color.FromArgb(215, 215, 210);
            btnThanhToan.ForeColor = coMon ? Color.White : Color.FromArgb(160, 160, 158);
            btnThanhToan.Cursor = Cursors.Default;
        }

        private void btnThanhToan_Click(object sender, EventArgs e) { /* chỉ trang trí */ }

        // ══════════════════════════════════════════════════════════
        //  NHẬN SỰ KIỆN từ card "+" — KHÔNG gọi HienThiTrangHienTai
        // ══════════════════════════════════════════════════════════
        private void Card_OnThemVaoGio(object sender, SanPhamDTO sp)
        {
            if (_phienHienTai != null)
                ThemVaoChiTietPhien(sp);
            else if (_banDangChon != null)
                MessageBox.Show(
                    $"Bàn \"{_banDangChon.TenBan}\" chưa có phiên đang chơi.\nKhởi động phiên trước rồi thêm món.",
                    "Chưa có phiên", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            SanPhamDuocChon?.Invoke(this, sp);
        }

        // ══════════════════════════════════════════════════════════
        //  THÊM MÓN vào ChiTietPhien
        //  – Trừ _tonKhoTam (RAM) ngay lập tức
        //  – Cập nhật DB
        //  – Patch right bar tại chỗ (không rebuild card)
        // ══════════════════════════════════════════════════════════
        private void ThemVaoChiTietPhien(SanPhamDTO sp)
        {
            // ── Kiểm tra tồn kho tạm ────────────────────────────
            var spGoc = _dsDayDu.FirstOrDefault(x => x.MaSP == sp.MaSP);
            int tonTam = _tonKhoTam.ContainsKey(sp.MaSP) ? _tonKhoTam[sp.MaSP] : 0;
            int tonConLai = (spGoc?.SoLuongTon ?? 0) - tonTam;

            if (tonConLai <= 0)
            {
                MessageBox.Show($"Sản phẩm \"{sp.TenSP}\" đã hết hàng!",
                    "Hết hàng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ── Trừ tồn kho TẠM trong RAM ────────────────────────
            _tonKhoTam[sp.MaSP] = tonTam + 1;

            try
            {
                var existing = _dsChiTiet.FirstOrDefault(
                    ct => ct.MaSP == sp.MaSP && ct.MaPhien == _phienHienTai.MaPhien);

                if (existing != null)
                {
                    // Cộng dồn SL
                    existing.SoLuong += 1;
                    _chiTietPhienBll.CapNhatChiTietPhien(existing);

                    // Patch right bar tại chỗ
                    PatchThemMon(existing);
                }
                else
                {
                    // Món mới
                    var ct = new ChiTietPhienDTO
                    {
                        MaCTP = _chiTietPhienBll.SinhMaMoi(),
                        MaPhien = _phienHienTai.MaPhien,
                        MaSP = sp.MaSP,
                        SoLuong = 1,
                        DonGia = sp.GiaBan,
                    };
                    _chiTietPhienBll.ThemChiTietPhien(ct);
                    _dsChiTiet.Add(ct);

                    if (!_cacheTenSP.ContainsKey(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;

                    // Patch right bar: thêm dòng mới
                    PatchThemMon(ct);
                }

                // ── Cập nhật label tồn kho trực tiếp trên card ──
                CapNhatTonKhoTrenCard(sp.MaSP);
            }
            catch (Exception ex)
            {
                // Hoàn tác tồn kho tạm
                _tonKhoTam[sp.MaSP] = Math.Max(0, _tonKhoTam[sp.MaSP] - 1);
                MessageBox.Show("Lỗi thêm món vào phiên:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  GIẢM HOẶC XÓA MÓN (nút −)
        //  SoLuong > 1 → giảm 1 và cập nhật DB
        //  SoLuong = 1 → xóa khỏi DB và _dsChiTiet
        // ══════════════════════════════════════════════════════════
        private void GiamHoacXoaMon(string maSP)
        {
            var ct = _dsChiTiet.FirstOrDefault(x => x.MaSP == maSP);
            if (ct == null) return;

            try
            {
                if (ct.SoLuong > 1)
                {
                    ct.SoLuong -= 1;
                    _chiTietPhienBll.CapNhatChiTietPhien(ct);

                    // Hoàn tác tồn kho tạm
                    if (_tonKhoTam.ContainsKey(maSP) && _tonKhoTam[maSP] > 0)
                        _tonKhoTam[maSP]--;

                    // Patch right bar tại chỗ
                    PatchXoaMon(maSP, ct);
                }
                else
                {
                    // Xóa hẳn
                    _chiTietPhienBll.XoaChiTietPhien(ct.MaCTP);
                    _dsChiTiet.Remove(ct);

                    if (_tonKhoTam.ContainsKey(maSP) && _tonKhoTam[maSP] > 0)
                        _tonKhoTam[maSP]--;

                    // Patch right bar: xóa row
                    PatchXoaMon(maSP, null);
                }

                // Cập nhật label tồn kho trên card
                CapNhatTonKhoTrenCard(maSP);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi giảm/xóa món:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] CẬP NHẬT LABEL TỒN KHO TRỰC TIẾP TRÊN CARD
        //  Không gọi HienThiTrangHienTai — chỉ tìm card đang có
        //  và gọi card.CapNhatTonKho(tonHienThi)
        // ══════════════════════════════════════════════════════════
        private void CapNhatTonKhoTrenCard(string maSP)
        {
            var spGoc = _dsDayDu.FirstOrDefault(x => x.MaSP == maSP);
            if (spGoc == null) return;

            int tonTam = _tonKhoTam.ContainsKey(maSP) ? _tonKhoTam[maSP] : 0;
            int tonHienThi = spGoc.SoLuongTon - tonTam;

            foreach (Control c in flowCards.Controls)
            {
                if (c is MenuSanPhamCard card && card.MaSP == maSP)
                {
                    card.CapNhatTonKho(tonHienThi);
                    break;
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] FETCH LẠI DB SAU THANH TOÁN VÀ CẬP NHẬT TẤT CẢ CARD
        //  - Tải lại _dsDayDu từ DB (tồn kho thật sau khi đã trừ)
        //  - Duyệt từng card đang render: cập nhật số tồn / hiện overlay hết hàng
        // ══════════════════════════════════════════════════════════
        private void CapNhatTatCaCardSauThanhToan()
        {
            try
            {
                // Fetch tồn kho thật từ DB
                var dsThatSuDB = _bll.TimKiem("") ?? new System.Collections.Generic.List<SanPhamDTO>();

                // Cập nhật _dsDayDu bằng dữ liệu mới từ DB
                _dsDayDu = dsThatSuDB;
                foreach (var sp in _dsDayDu)
                    if (!string.IsNullOrEmpty(sp.MaSP))
                        _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;

                // Cập nhật snapshot hash để polling không reload lại ngay
                _snapshotHash = TinhSnapshotHash(_dsDayDu);

                // Duyệt từng card đang hiển thị và cập nhật tồn kho thật
                // (_tonKhoTam đã được Clear() trước khi gọi hàm này)
                foreach (Control c in flowCards.Controls)
                {
                    if (!(c is MenuSanPhamCard card)) continue;

                    var spMoi = _dsDayDu.FirstOrDefault(x => x.MaSP == card.MaSP);
                    if (spMoi == null) continue;

                    int tonThucTe = spMoi.SoLuongTon;  // tồn kho thật sau khi DB đã trừ
                    card.CapNhatTonKho(tonThucTe);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CapNhatTatCaCardSauThanhToan lỗi: " + ex.Message);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Lấy tên SP từ cache
        // ══════════════════════════════════════════════════════════
        private string LayTenSP(string maSP)
        {
            if (_cacheTenSP.TryGetValue(maSP, out string ten)) return ten;
            try
            {
                var ds = _bll.TimKiem(maSP);
                var sp = ds?.FirstOrDefault(x => x.MaSP == maSP);
                string tenSP = sp?.TenSP ?? maSP;
                _cacheTenSP[maSP] = tenSP;
                return tenSP;
            }
            catch { _cacheTenSP[maSP] = maSP; return maSP; }
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

                // [THÊM MỚI] Cập nhật snapshot sau mỗi lần tải thủ công
                _snapshotHash = TinhSnapshotHash(_dsDayDu);

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
                 string.Equals(sp.Loai, loaiFilter, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(keyword) ||
                 (sp.TenSP ?? "").ToLower().Contains(keyword))
            ).ToList();

            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  HIỂN THỊ TRANG CARD SẢN PHẨM
        //  Chỉ gọi khi: load lần đầu / đổi trang / tìm kiếm / reload
        //  KHÔNG gọi khi thêm/xóa món
        // ══════════════════════════════════════════════════════════
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
                    // Tính tồn kho hiển thị (trừ tạm)
                    int tonTam = _tonKhoTam.ContainsKey(sp.MaSP) ? _tonKhoTam[sp.MaSP] : 0;
                    var spHienThi = CloneSPVoiTon(sp, sp.SoLuongTon - tonTam);

                    var card = new MenuSanPhamCard();
                    card.NapDuLieu(spHienThi);
                    card.OnThemVaoGio += Card_OnThemVaoGio;
                    flowCards.Controls.Add(card);
                }
            }

            flowCards.ResumeLayout();
            CapNhatPhanTrang();
        }

        // ── Clone DTO với tồn kho mới (không sửa object gốc) ─────
        private static SanPhamDTO CloneSPVoiTon(SanPhamDTO sp, int tonMoi) =>
            new SanPhamDTO
            {
                MaSP = sp.MaSP,
                TenSP = sp.TenSP,
                Loai = sp.Loai,
                GiaBan = sp.GiaBan,
                HinhAnh = sp.HinhAnh,
                SoLuongTon = tonMoi,
            };

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
                btn.Click += (s, e) => { _tabChon = idx; CapNhatTabUI(idx); LocVaHienThi(); };
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
            _tonKhoTam.Clear();
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
            int radius = 10, d = radius * 2;
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