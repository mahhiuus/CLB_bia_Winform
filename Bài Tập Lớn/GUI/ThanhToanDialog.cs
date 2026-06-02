using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using iTextSharp.text.pdf;
// Alias để tránh xung đột với System.Drawing.Font
using PdfDocument = iTextSharp.text.Document;
using PdfFont = iTextSharp.text.Font;
using PdfPageSize = iTextSharp.text.PageSize;
using PdfParagraph = iTextSharp.text.Paragraph;
using PdfChunk = iTextSharp.text.Chunk;
using PdfPhrase = iTextSharp.text.Phrase;
using PdfElement = iTextSharp.text.Element;
using PdfBaseColor = iTextSharp.text.BaseColor;
using PdfBaseFont = iTextSharp.text.pdf.BaseFont;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.Session;
using Bài_Tập_Lớn.UI; // [MỚI] Thêm thư viện UI để dùng OverlayForm

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    //  THANH TOÁN DIALOG  (v6)
    //  THAY ĐỔI SO VỚI v5:
    //  1. Thêm ComboBox cmbKhachHang: dropdown tên khách hàng thân thiết
    //     - Mặc định "(Không chọn)"
    //     - Nếu KH ≥ 50 điểm → tự động giảm 10% tổng bill + cộng 1 lần tích lũy
    //  2. Thêm ComboBox cmbGiamGiaSuKien: nhân viên chọn % giảm giá sự kiện
    //     - Các mức: Không giảm / 5% / 10% / 15% / 20%
    //  3. Hai khoản giảm cộng dồn: GiảmThânThiết% + GiảmSựKiện%
    //  4. Hiển thị lblGiamGia (dòng chiết khấu) và cập nhật lblTongTien realtime
    //  5. PDF xuất thêm dòng chiết khấu nếu có
    // ═══════════════════════════════════════════════════════════════
    public partial class ThanhToanDialog : Form
    {
        // ── Public result ──────────────────────────────────────────
        public bool IsPaid { get; private set; } = false;
        public HoaDonBanDTO HoaDonDaTao { get; private set; } = null;

        /// <summary>Mã hóa đơn vừa được tạo sau khi thanh toán thành công.</summary>
        public string MaHoaDon => HoaDonDaTao?.MaHDB;

        /// <summary>Tổng tiền thực tế đã thanh toán (sau chiết khấu).</summary>
        public double TongTien => HoaDonDaTao?.TongTien ?? 0;

        // ── Data ───────────────────────────────────────────────────
        private readonly BanBidaDTO _ban;
        private readonly PhienChoiDTO _phien;
        private readonly List<ChiTietPhienDTO> _dsChiTiet;

        // ── Cache tên SP (truyền từ ngoài) ────────────────────────
        private readonly Dictionary<string, string> _cacheTenSP;

        // ── Chế độ preview (hiện hóa đơn đã xong, chỉ đọc) ───────
        private readonly bool _isPreviewMode = false;
        private readonly HoaDonBanDTO _hoaDonPreview = null;

        // ── BLL ────────────────────────────────────────────────────
        private readonly PhienChoiBLL _phienBLL = new PhienChoiBLL();
        private readonly BanBidaBLL _banBLL = new BanBidaBLL();
        private readonly HoaDonBanBLL _hoaDonBLL = new HoaDonBanBLL();
        private readonly KhachHangBLL _khachHangBLL = new KhachHangBLL();
        private readonly SanPhamBLL _sanPhamBLL = new SanPhamBLL();

        // ── Live timer ─────────────────────────────────────────────
        private System.Windows.Forms.Timer _clock;

        // ── Drag state ─────────────────────────────────────────────
        private bool _dragging = false;
        private Point _dragStart = Point.Empty;

        // ── Khách hàng đang chọn ──────────────────────────────────
        private KhachHangDTO _khachHangDuocChon = null;  // null = không chọn KH
        private const int DIEM_TICH_LUY_TOI_THIEU = 50; // Ngưỡng điểm hưởng 10%
        private const double GIAM_THAN_THIET = 0.10;    // 10% cho KH thân thiết

        // ── [MỚI] Overlay ─────────────────────────────────────────
        private OverlayForm _overlay;

        // ── Colours ───────────────────────────────────────────────
        static readonly Color GREEN_DARK = Color.FromArgb(43, 78, 35);
        static readonly Color GREEN_LIGHT = Color.FromArgb(121, 174, 111);

        // ══════════════════════════════════════════════════════════
        //  Constructor chính — chế độ thanh toán
        // ══════════════════════════════════════════════════════════
        public ThanhToanDialog(
            BanBidaDTO ban,
            PhienChoiDTO phien,
            List<ChiTietPhienDTO> dsChiTiet = null,
            Dictionary<string, string> cacheTenSP = null)
        {
            _ban = ban;
            _phien = phien;
            _dsChiTiet = dsChiTiet ?? new List<ChiTietPhienDTO>();
            _cacheTenSP = cacheTenSP ?? new Dictionary<string, string>();

            InitializeComponent();
            PopulateStaticLabels();
        }

        // ══════════════════════════════════════════════════════════
        //  Constructor overload — chế độ preview hóa đơn (chỉ đọc)
        // ══════════════════════════════════════════════════════════
        public ThanhToanDialog(
            BanBidaDTO ban,
            PhienChoiDTO phien,
            List<ChiTietPhienDTO> dsChiTiet,
            Dictionary<string, string> cacheTenSP,
            HoaDonBanDTO hoaDonPreview)
        {
            _ban = ban;
            _phien = phien;
            _dsChiTiet = dsChiTiet ?? new List<ChiTietPhienDTO>();
            _cacheTenSP = cacheTenSP ?? new Dictionary<string, string>();
            _isPreviewMode = true;
            _hoaDonPreview = hoaDonPreview;

            InitializeComponent();
            PopulateStaticLabels();
        }

        // ══════════════════════════════════════════════════════════
        //  [MỚI] HIỆN OVERLAY
        // ══════════════════════════════════════════════════════════
        public void ShowOverlay(Form parent)
        {
            _overlay = new OverlayForm();
            _overlay.Show(parent);
            _overlay.StartFade();
        }

        // ══════════════════════════════════════════════════════════
        //  LOAD
        // ══════════════════════════════════════════════════════════
        private void ThanhToanDialog_Load(object sender, EventArgs e)
        {
            ApplyRoundedRegion(this, 20);
            NapDanhSachKhachHang();

            if (_isPreviewMode)
            {
                // Chế độ xem hóa đơn: ẩn các control nhập liệu
                if (_hoaDonPreview != null)
                {
                    TimeSpan elapsed = _hoaDonPreview.NgayBan - _phien.ThoiGianBatDau;
                    lblThoiGian.Text = elapsed.ToString(@"hh\:mm\:ss");
                    lblTienGio.Text = _hoaDonPreview.TienBida.ToString("N0") + " đ";
                    lblTienSP.Text = _hoaDonPreview.TienSanPham.ToString("N0") + " đ";
                    lblTongTien.Text = _hoaDonPreview.TongTien.ToString("N0") + " đ";

                    // Ẩn dòng giảm giá nếu không có chiết khấu
                    if (_hoaDonPreview.TongTien < (_hoaDonPreview.TienBida + _hoaDonPreview.TienSanPham))
                    {
                        double giamGia = (_hoaDonPreview.TienBida + _hoaDonPreview.TienSanPham) - _hoaDonPreview.TongTien;
                        lblGiamGia.Text = "-" + giamGia.ToString("N0") + " đ";
                        lblLGiamGia.Visible = true;
                        lblGiamGia.Visible = true;
                    }
                    else
                    {
                        lblLGiamGia.Visible = false;
                        lblGiamGia.Visible = false;
                    }
                }

                // Ẩn toàn bộ control chọn KH & giảm giá sự kiện
                lblLKhachHang.Visible = false;
                cmbKhachHang.Visible = false;
                lblThongTinKH.Visible = false;
                lblLGiamSuKien.Visible = false;
                cmbGiamGiaSuKien.Visible = false;

                btnPay.Visible = false;
                btnCancel.Text = "✕  Đóng";
            }
            else
            {
                StartClock();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  NẠP DANH SÁCH KHÁCH HÀNG VÀO COMBOBOX
        // ══════════════════════════════════════════════════════════
        private void NapDanhSachKhachHang()
        {
            cmbKhachHang.Items.Clear();

            // Phần tử đầu tiên: không chọn
            cmbKhachHang.Items.Add(new ComboBoxKhachHangItem(null));

            try
            {
                var dsKH = _khachHangBLL.LayTatCaKhachHang();
                if (dsKH != null)
                    foreach (var kh in dsKH)
                        cmbKhachHang.Items.Add(new ComboBoxKhachHangItem(kh));
            }
            catch
            {
                // Nếu BLL chưa có hoặc lỗi DB → chỉ giữ "(Không chọn)"
            }

            cmbKhachHang.SelectedIndex = 0;
        }

        // ══════════════════════════════════════════════════════════
        //  COMBOBOX KHÁCH HÀNG ĐỔI LỰA CHỌN
        // ══════════════════════════════════════════════════════════
        private void CmbKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbKhachHang.SelectedItem is ComboBoxKhachHangItem item)
            {
                _khachHangDuocChon = item.KhachHang;
            }
            else
            {
                _khachHangDuocChon = null;
            }

            CapNhatThongTinKH();
            UpdateTotals(); // Tính lại tổng tiền sau khi chọn KH
        }

        // ══════════════════════════════════════════════════════════
        //  COMBOBOX GIẢM GIÁ SỰ KIỆN ĐỔI LỰA CHỌN
        // ══════════════════════════════════════════════════════════
        private void CmbGiamGiaSuKien_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotals(); // Tính lại tổng khi đổi % sự kiện
        }

        // ══════════════════════════════════════════════════════════
        //  HIỂN THỊ THÔNG TIN KHÁCH HÀNG BÊN DƯỚI COMBOBOX
        // ══════════════════════════════════════════════════════════
        private void CapNhatThongTinKH()
        {
            if (_khachHangDuocChon == null)
            {
                lblThongTinKH.Visible = false;
                return;
            }

            lblThongTinKH.Visible = true;
            bool duDiem = _khachHangDuocChon.DiemTichLuy >= DIEM_TICH_LUY_TOI_THIEU;

            if (duDiem)
                lblThongTinKH.Text = $"⭐ Thân thiết  |  {_khachHangDuocChon.DiemTichLuy} điểm  →  Giảm 10% + thêm 1 lượt tích lũy";
            else
                lblThongTinKH.Text = $"👤 {_khachHangDuocChon.DiemTichLuy} điểm  (cần ≥ {DIEM_TICH_LUY_TOI_THIEU} để hưởng ưu đãi)";

            lblThongTinKH.ForeColor = duDiem ? Color.FromArgb(43, 120, 43) : Color.FromArgb(150, 100, 30);
        }

        // ══════════════════════════════════════════════════════════
        //  LẤY % GIẢM GIÁ SỰ KIỆN TỪ COMBOBOX
        // ══════════════════════════════════════════════════════════
        private double LayPhanTramGiamSuKien()
        {
            if (cmbGiamGiaSuKien.SelectedItem == null) return 0;
            var text = cmbGiamGiaSuKien.SelectedItem.ToString();
            // Phần tử dạng "Không giảm" hoặc "5%" / "10%" / ...
            if (text.EndsWith("%") && double.TryParse(text.TrimEnd('%'), out double pct))
                return pct / 100.0;
            return 0;
        }

        // ══════════════════════════════════════════════════════════
        //  TÍNH TỔNG GIẢM GIÁ CỘNG DỒN
        // ══════════════════════════════════════════════════════════
        private double TinhTongPhanTramGiam()
        {
            double giam = 0;

            // Giảm thân thiết 10% nếu KH đủ điểm
            if (_khachHangDuocChon != null && _khachHangDuocChon.DiemTichLuy >= DIEM_TICH_LUY_TOI_THIEU)
                giam += GIAM_THAN_THIET;

            // Giảm sự kiện
            giam += LayPhanTramGiamSuKien();

            // Tối đa 100%
            return Math.Min(giam, 1.0);
        }

        // ══════════════════════════════════════════════════════════
        //  GÁN GIÁ TRỊ TĨNH VÀO LABELS
        // ══════════════════════════════════════════════════════════
        private void PopulateStaticLabels()
        {
            lblVBan.Text = $"{_ban.TenBan}  ({_ban.LoaiBan})";
            lblVNhanVien.Text = SessionManager.Instance.TaiKhoanHienTai?.TenDangNhap
                                  ?? "(không xác định)";
            lblVMaPhien.Text = _phien.MaPhien;
            lblVBatDau.Text = _phien.ThoiGianBatDau.ToString("HH:mm:ss  dd/MM/yyyy");
            lblVGiaGio.Text = _ban.GiaTheoGio.ToString("N0") + " đ";

            double tienSP = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            lblTienSP.Text = tienSP.ToString("N0") + " đ";
        }

        // ══════════════════════════════════════════════════════════
        //  KÉO FORM
        // ══════════════════════════════════════════════════════════
        private void PanelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            _dragging = true;
            _dragStart = e.Location;
        }

        private void PanelHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_dragging) return;
            Location = new Point(Location.X + e.X - _dragStart.X,
                                 Location.Y + e.Y - _dragStart.Y);
        }

        private void PanelHeader_MouseUp(object sender, MouseEventArgs e)
            => _dragging = false;

        // ══════════════════════════════════════════════════════════
        //  LIVE CLOCK
        // ══════════════════════════════════════════════════════════
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

            TimeSpan elapsed = DateTime.Now - _phien.ThoiGianBatDau;
            double hours = elapsed.TotalHours;

            double tienGio = Math.Ceiling(hours * _ban.GiaTheoGio / 1000.0) * 1000.0;
            double tienSP = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            double tongBill = tienGio + tienSP;

            // Tính giảm giá cộng dồn 
            double pctGiam = TinhTongPhanTramGiam();
            double soTienGiam = Math.Round(tongBill * pctGiam / 1000.0) * 1000.0; // Làm tròn 1.000đ
            double tongSauGiam = tongBill - soTienGiam;

            lblThoiGian.Text = elapsed.ToString(@"hh\:mm\:ss");
            lblTienGio.Text = tienGio.ToString("N0") + " đ";
            lblTienSP.Text = tienSP.ToString("N0") + " đ";

            // Dòng giảm giá: chỉ hiện khi có giảm
            if (soTienGiam > 0)
            {
                double pctHienThi = pctGiam * 100;
                lblGiamGia.Text = $"-{soTienGiam:N0} đ  ({pctHienThi:0}%)";
                lblLGiamGia.Visible = true;
                lblGiamGia.Visible = true;
            }
            else
            {
                lblLGiamGia.Visible = false;
                lblGiamGia.Visible = false;
            }

            lblTongTien.Text = tongSauGiam.ToString("N0") + " đ";
        }

        // ══════════════════════════════════════════════════════════
        //  XÁC NHẬN THANH TOÁN
        // ══════════════════════════════════════════════════════════
        private void BtnPay_Click(object sender, EventArgs e)
        {
            _clock?.Stop();
            DateTime now = DateTime.Now;

            TimeSpan elapsed = now - _phien.ThoiGianBatDau;
            double tienGio = Math.Ceiling(elapsed.TotalHours * _ban.GiaTheoGio / 1000.0) * 1000.0;
            double tienSP = _dsChiTiet.Sum(ct => ct.SoLuong * ct.DonGia);
            double tongBill = tienGio + tienSP;

            // Tính giảm cộng dồn 
            double pctGiam = TinhTongPhanTramGiam();
            double soTienGiam = Math.Round(tongBill * pctGiam / 1000.0) * 1000.0;
            double tongSauGiam = tongBill - soTienGiam;

            var tk = SessionManager.Instance.TaiKhoanHienTai;
            string maNV = tk?.MaNV ?? tk?.TenDangNhap ?? "";

            // Ghi chú giảm giá vào hóa đơn 
            string ghiChu = null;
            if (pctGiam > 0)
            {
                var notes = new List<string>();
                if (_khachHangDuocChon != null && _khachHangDuocChon.DiemTichLuy >= DIEM_TICH_LUY_TOI_THIEU)
                    notes.Add("Giảm 10% KH thân thiết");
                double pctSuKien = LayPhanTramGiamSuKien();
                if (pctSuKien > 0)
                    notes.Add($"Giảm {pctSuKien * 100:0}% sự kiện");
                ghiChu = string.Join(" + ", notes);
            }

            try
            {
                // 1. Kết thúc phiên chơi
                _phienBLL.KetThucPhien(_phien.MaPhien, now);

                // 2. Cập nhật bàn → TRỐNG
                _banBLL.CapNhatTrangThai(_ban.MaBan, "TRONG");

                // 3. Tạo & lưu hóa đơn
                var hdb = new HoaDonBanDTO
                {
                    MaHDB = _hoaDonBLL.SinhMaMoi(),
                    MaPhien = _phien.MaPhien,
                    MaKH = _khachHangDuocChon?.MaKH,
                    MaNV = maNV,
                    NgayBan = now,
                    TienBida = tienGio,
                    TienSanPham = tienSP,
                    TongTien = tongSauGiam,
                    GhiChu = ghiChu
                };
                _hoaDonBLL.Them(hdb);
                HoaDonDaTao = hdb;
                IsPaid = true;

                // 4. Trừ tồn kho DB cho từng sản phẩm đã order
                //    GiamTonKho trong DAL đã có kiểm tra đủ hàng — nếu thiếu sẽ throw Exception
                if (_dsChiTiet != null && _dsChiTiet.Count > 0)
                {
                    var loiTonKho = new System.Text.StringBuilder();
                    foreach (var ctSP in _dsChiTiet)
                    {
                        if (string.IsNullOrWhiteSpace(ctSP.MaSP) || ctSP.SoLuong <= 0) continue;
                        try
                        {
                            _sanPhamBLL.GiamTonKho(ctSP.MaSP, ctSP.SoLuong);
                        }
                        catch (Exception exGiam)
                        {
                            // Ghi nhận lỗi nhưng không dừng — các SP khác vẫn tiếp tục trừ
                            loiTonKho.AppendLine($"• {ctSP.MaSP}: {exGiam.Message}");
                        }
                    }

                    // Nếu có SP nào lỗi → thông báo nhưng KHÔNG rollback hóa đơn
                    // (hóa đơn đã lưu, chỉ cảnh báo nhân viên kiểm tra lại tồn kho)
                    if (loiTonKho.Length > 0)
                        MessageBox.Show(
                            "Thanh toán thành công nhưng một số sản phẩm trừ tồn kho thất bại:\n" + loiTonKho,
                            "Cảnh báo tồn kho", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // 5. Cộng 1 lần tích lũy nếu KH thân thiết đủ điểm
                if (_khachHangDuocChon != null && _khachHangDuocChon.DiemTichLuy >= DIEM_TICH_LUY_TOI_THIEU)
                {
                    try
                    {
                        _khachHangBLL.CongTichLuy(_khachHangDuocChon.MaKH, 1);
                    }
                    catch
                    {
                        // Không ảnh hưởng thanh toán nếu cộng điểm lỗi
                    }
                }

                // 6. Hỏi xuất PDF
                if (MessageBox.Show(
                        "Bạn có muốn lưu hóa đơn ra file PDF không?",
                        "Xuất hóa đơn",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    XuatVaXemPdf(hdb);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _clock?.Start();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  NÚT HỦY
        // ══════════════════════════════════════════════════════════
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ══════════════════════════════════════════════════════════
        //  XUẤT PDF + MỞ PREVIEW
        // ══════════════════════════════════════════════════════════
        private void XuatVaXemPdf(HoaDonBanDTO hdb)
        {
            using (var sfd = new SaveFileDialog
            {
                Title = "Lưu hóa đơn PDF",
                Filter = "PDF Documents (*.pdf)|*.pdf",
                FileName = $"HoaDon_{hdb.MaHDB}_{hdb.NgayBan:yyyyMMdd_HHmm}.pdf",
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    ExportToPdf(sfd.FileName, hdb);
                    Process.Start(new ProcessStartInfo(sfd.FileName)
                    { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất PDF:\n" + ex.Message,
                        "Lỗi xuất PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ══════════════════════════════════════════════════════════
        //  XUẤT PDF — khổ receipt nhiệt 80mm
        // ══════════════════════════════════════════════════════════
        private void ExportToPdf(string filePath, HoaDonBanDTO hdb)
        {
            // Tính chiều cao nội dung
            int soMon = _dsChiTiet.Count;
            float rowH = 16f;
            float headerH = 120f;
            float infoH = 130f;
            float tableHeaderH = 22f;
            float tableBodyH = soMon * rowH + 8f;

            bool coGiam = hdb.TongTien < (hdb.TienBida + hdb.TienSanPham);
            float totalH = coGiam ? 96f : 80f;
            float footerH = 50f;

            float pageHeight = headerH + infoH + tableHeaderH + tableBodyH + totalH + footerH;
            float pageWidth = 226.77f;
            float marginLR = 10f;
            float marginTB = 12f;

            var pageSize = new iTextSharp.text.Rectangle(pageWidth, pageHeight);
            var doc = new PdfDocument(pageSize, marginLR, marginLR, marginTB, marginTB);
            var writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // ── Fonts ───────────────────────────────────────────────
            string fontPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
            PdfBaseFont bf = PdfBaseFont.CreateFont(fontPath,
                PdfBaseFont.IDENTITY_H, PdfBaseFont.EMBEDDED);

            var fShopName = new PdfFont(bf, 11f, PdfFont.BOLD, PdfBaseColor.BLACK);
            var fTitle = new PdfFont(bf, 9f, PdfFont.BOLD, new PdfBaseColor(43, 78, 35));
            var fSub = new PdfFont(bf, 7f, PdfFont.NORMAL, new PdfBaseColor(120, 120, 120));
            var fLabel = new PdfFont(bf, 7.5f, PdfFont.NORMAL, PdfBaseColor.BLACK);
            var fValue = new PdfFont(bf, 7.5f, PdfFont.BOLD, PdfBaseColor.BLACK);
            var fColHead = new PdfFont(bf, 7f, PdfFont.BOLD, PdfBaseColor.WHITE);
            var fCell = new PdfFont(bf, 7f, PdfFont.NORMAL, PdfBaseColor.BLACK);
            var fTotal = new PdfFont(bf, 10f, PdfFont.BOLD, new PdfBaseColor(200, 55, 55));
            var fDiscount = new PdfFont(bf, 8f, PdfFont.BOLD, new PdfBaseColor(43, 120, 43));
            var fFooter = new PdfFont(bf, 7f, PdfFont.ITALIC, new PdfBaseColor(140, 140, 140));

            var sepGray = new iTextSharp.text.pdf.draw.LineSeparator(
                0.4f, 100f, PdfBaseColor.LIGHT_GRAY, PdfElement.ALIGN_CENTER, 1);
            var sepGreen = new iTextSharp.text.pdf.draw.LineSeparator(
                0.6f, 100f, new PdfBaseColor(43, 78, 35), PdfElement.ALIGN_CENTER, 1);

            // ── Header ──────────────────────────────────────────────
            doc.Add(new PdfParagraph("DOUBLE2N BILLIARDS", fShopName)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph("HÓA ĐƠN THANH TOÁN", fTitle)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph(hdb.NgayBan.ToString("HH:mm  dd/MM/yyyy"), fSub)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 4f });
            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Thông tin phiên ─────────────────────────────────────
            AddReceiptRow(doc, "Mã HĐ", hdb.MaHDB, fLabel, fValue);
            AddReceiptRow(doc, "Nhân viên", hdb.MaNV, fLabel, fValue);
            AddReceiptRow(doc, "Bàn", _ban.TenBan + $" ({_ban.LoaiBan})", fLabel, fValue);
            AddReceiptRow(doc, "Mã phiên", hdb.MaPhien, fLabel, fValue);
            AddReceiptRow(doc, "Giá/giờ", _ban.GiaTheoGio.ToString("N0") + " đ", fLabel, fValue);

            // In tên KH nếu có
            if (_khachHangDuocChon != null)
                AddReceiptRow(doc, "Khách hàng", _khachHangDuocChon.HoTen, fLabel, fValue);

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Bảng sản phẩm ───────────────────────────────────────
            if (_dsChiTiet.Count > 0)
            {
                var tbl = new PdfPTable(4) { WidthPercentage = 100, SpacingAfter = 2f };
                tbl.SetWidths(new float[] { 38f, 10f, 22f, 22f });

                foreach ((string txt, bool right) in new[]{
                    ("Sản phẩm", false), ("SL", true), ("Đơn giá", true), ("T.tiền", true)})
                {
                    tbl.AddCell(new PdfPCell(new PdfPhrase(txt, fColHead))
                    {
                        BackgroundColor = new PdfBaseColor(43, 78, 35),
                        Padding = 4f,
                        HorizontalAlignment = right ? PdfElement.ALIGN_RIGHT : PdfElement.ALIGN_LEFT,
                        BorderColor = PdfBaseColor.WHITE,
                    });
                }

                bool alt = false;
                foreach (var ct in _dsChiTiet)
                {
                    var bg = alt ? new PdfBaseColor(245, 250, 245) : PdfBaseColor.WHITE;
                    string tenSP = LayTenSPTuCache(ct.MaSP);
                    tbl.AddCell(ReceiptCell(tenSP, fCell, bg, false));
                    tbl.AddCell(ReceiptCell(ct.SoLuong.ToString(), fCell, bg, true));
                    tbl.AddCell(ReceiptCell(ct.DonGia.ToString("N0") + "đ", fCell, bg, true));
                    tbl.AddCell(ReceiptCell((ct.SoLuong * ct.DonGia).ToString("N0") + "đ", fCell, bg, true));
                    alt = !alt;
                }
                doc.Add(tbl);
            }

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Tổng kết ────────────────────────────────────────────
            AddReceiptRow(doc, "Tiền giờ chơi", hdb.TienBida.ToString("N0") + " đ", fLabel, fValue);
            AddReceiptRow(doc, "Tiền sản phẩm", hdb.TienSanPham.ToString("N0") + " đ", fLabel, fValue);

            // In dòng chiết khấu nếu có
            if (coGiam)
            {
                double soTienGiam = (hdb.TienBida + hdb.TienSanPham) - hdb.TongTien;
                string ghiChuGiam = string.IsNullOrWhiteSpace(hdb.GhiChu) ? "Chiết khấu" : hdb.GhiChu;
                AddReceiptRow(doc, ghiChuGiam, "-" + soTienGiam.ToString("N0") + " đ", fLabel, fDiscount);
            }

            doc.Add(new PdfParagraph(" ") { SpacingAfter = 2f });
            doc.Add(new PdfChunk(sepGreen));
            doc.Add(new PdfParagraph(" "));

            doc.Add(new PdfParagraph($"TỔNG TIỀN:  {hdb.TongTien:N0} đ", fTotal)
            { Alignment = PdfElement.ALIGN_RIGHT, SpacingAfter = 4f });

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Footer ──────────────────────────────────────────────
            doc.Add(new PdfParagraph("Cảm ơn quý khách và hẹn gặp lại!", fFooter)
            { Alignment = PdfElement.ALIGN_CENTER });

            doc.Close();
        }

        // ── Receipt helper: 1 dòng label – value ──────────────────
        private static void AddReceiptRow(PdfDocument doc, string label, string value,
                                          PdfFont fLabel, PdfFont fValue)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 100, SpacingAfter = 1f };
            tbl.SetWidths(new float[] { 45f, 55f });
            tbl.AddCell(new PdfPCell(new PdfPhrase(label, fLabel))
            { Border = PdfPCell.NO_BORDER, Padding = 2f });
            tbl.AddCell(new PdfPCell(new PdfPhrase(value, fValue))
            {
                Border = PdfPCell.NO_BORDER,
                Padding = 2f,
                HorizontalAlignment = PdfElement.ALIGN_RIGHT
            });
            doc.Add(tbl);
        }

        // ── Receipt helper: 1 cell bảng SP ───────────────────────
        private static PdfPCell ReceiptCell(string text, PdfFont font,
                                             PdfBaseColor bg, bool right)
        {
            return new PdfPCell(new PdfPhrase(text, font))
            {
                BackgroundColor = bg,
                Padding = 3f,
                HorizontalAlignment = right ? PdfElement.ALIGN_RIGHT : PdfElement.ALIGN_LEFT,
                BorderColor = new PdfBaseColor(230, 230, 230),
            };
        }

        // ══════════════════════════════════════════════════════════
        //  Helper: Lấy tên SP từ cache
        // ══════════════════════════════════════════════════════════
        private string LayTenSPTuCache(string maSP)
        {
            if (_cacheTenSP != null && _cacheTenSP.TryGetValue(maSP, out string ten)
                && !string.IsNullOrWhiteSpace(ten))
                return ten;
            return maSP;
        }

        // ── Bo tròn form ──────────────────────────────────────────
        private static void ApplyRoundedRegion(Form frm, int radius)
        {
            var path = new GraphicsPath();
            int d = radius * 2;
            path.AddArc(0, 0, d, d, 180, 90);
            path.AddArc(frm.Width - d, 0, d, d, 270, 90);
            path.AddArc(frm.Width - d, frm.Height - d, d, d, 0, 90);
            path.AddArc(0, frm.Height - d, d, d, 90, 90);
            path.CloseFigure();
            frm.Region = new Region(path);
        }

        // ══════════════════════════════════════════════════════════
        //  CLEANUP
        // ══════════════════════════════════════════════════════════
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _clock?.Stop();
            _clock?.Dispose();
            base.OnFormClosed(e);

            // [MỚI] Đóng overlay khi form đóng
            _overlay?.Close();
            _overlay = null;
        }

        private void lblLGiamSuKien_Click(object sender, EventArgs e)
        {

        }

        private void lblGiamGia_Click(object sender, EventArgs e)
        {

        }

        private void tableBody_Paint(object sender, PaintEventArgs e)
        {

        }
    }

    // ══════════════════════════════════════════════════════════════
    //  Wrapper item cho ComboBox khách hàng
    //  → ToString() hiển thị tên, nhưng vẫn giữ reference KhachHangDTO
    // ══════════════════════════════════════════════════════════════
    internal class ComboBoxKhachHangItem
    {
        public KhachHangDTO KhachHang { get; }

        public ComboBoxKhachHangItem(KhachHangDTO kh)
        {
            KhachHang = kh;
        }

        public override string ToString()
        {
            if (KhachHang == null) return "(Không chọn)";
            string star = KhachHang.DiemTichLuy >= 50 ? "⭐ " : "";
            // In tên KH
            return $"{star}{KhachHang.HoTen}  [{KhachHang.DiemTichLuy} điểm]";
        }
    }
}