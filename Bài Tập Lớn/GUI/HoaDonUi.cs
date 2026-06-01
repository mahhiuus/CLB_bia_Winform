using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using iTextSharp.text.pdf;

// Alias để tránh xung đột với System.Drawing.Font — giống hệt ThanhToanDialog
using PdfDocument = iTextSharp.text.Document;
using PdfFont = iTextSharp.text.Font;
using PdfParagraph = iTextSharp.text.Paragraph;
using PdfChunk = iTextSharp.text.Chunk;
using PdfPhrase = iTextSharp.text.Phrase;
using PdfElement = iTextSharp.text.Element;
using PdfBaseColor = iTextSharp.text.BaseColor;
using PdfBaseFont = iTextSharp.text.pdf.BaseFont;

using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.GUI
{
    public partial class HoaDonUi : Form
    {
        // ── BLL ──
        private readonly HoaDonBanBLL _hoaDonBLL = new HoaDonBanBLL();
        private readonly ChiTietPhienBLL _chiTietPhienBLL = new ChiTietPhienBLL();
        private readonly SanPhamBLL _sanPhamBLL = new SanPhamBLL();

        // ── Colors ──
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_HOVER = ColorTranslator.FromHtml("#e8f5e8");
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);

        // ── Controls ──
        private TableLayoutPanel rootTable;
        private Guna2DataGridView gridHoaDon;
        private Guna2Button btnExportPdf, btnPrint, btnLoc, btnCancelLoc;
        private Guna2DateTimePicker dtpTuNgay, dtpDenNgay;
        private Label lblPageInfo;
        private Guna2Button btnPrev, btnNext;

        // ── Pagination & Logic State ──
        private List<HoaDonBanDTO> _allInvoices = new List<HoaDonBanDTO>();
        private int _currentPage = 1;
        private int _pageSize = 12;
        private int _totalPages = 1;
        private HoaDonBanDTO _selectedHoaDon = null;
        private int _rowToDeselect = -1;

        // ── Cache tên SP (MaSP → TenSP) ──
        private Dictionary<string, string> _cacheTenSP = new Dictionary<string, string>();

        public HoaDonUi()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            BuildLayout();
            this.Load += HoaDonUi_Load;
        }

        private void HoaDonUi_Load(object sender, EventArgs e)
        {
            BuildCacheTenSP();
            ResetDatePickers();
            LoadData();
        }

        // Nạp toàn bộ tên SP vào cache một lần khi form load
        private void BuildCacheTenSP()
        {
            try
            {
                var dsSP = _sanPhamBLL.LayTatCa();
                if (dsSP != null)
                    foreach (var sp in dsSP)
                        if (!string.IsNullOrWhiteSpace(sp.MaSP))
                            _cacheTenSP[sp.MaSP] = sp.TenSP ?? sp.MaSP;
            }
            catch { /* Nếu lỗi thì PDF sẽ in MaSP thay vì tên */ }
        }

        private void ResetDatePickers()
        {
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpDenNgay.Value = DateTime.Now;
        }

        // ════════════════════════════════════════════════════════
        //  XÂY DỰNG GIAO DIỆN GUNA 2 (UI BUILDER)
        // ════════════════════════════════════════════════════════
        private void BuildLayout()
        {
            this.BackColor = CREAM;
            this.Padding = new Padding(15);

            rootTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 3,
                ColumnCount = 1,
                BackColor = Color.Transparent
            };
            rootTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));
            rootTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            rootTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50f));

            // 1. TOOLBAR PANEL 8 CỘT
            TableLayoutPanel tlpToolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 8,
                Margin = new Padding(0)
            };
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 0: dtpTuNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 1: lblDivider
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 2: dtpDenNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 3: btnLoc
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 4: btnCancelLoc
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // 5: spacer
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 6: btnExportPdf
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 7: btnPrint

            dtpTuNgay = CreateDatePicker();
            Label lblDivider = new Label
            {
                Text = "-",
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = GREEN_DARK,
                Anchor = AnchorStyles.None
            };
            dtpDenNgay = CreateDatePicker();

            btnLoc = CreateToolbarButton("Lọc", 80);
            btnLoc.Click += BtnLoc_Click;

            btnCancelLoc = CreateToolbarButton("Hủy", 80);
            btnCancelLoc.BorderColor = Color.FromArgb(220, 80, 80);
            btnCancelLoc.ForeColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.HoverState.FillColor = Color.FromArgb(255, 240, 240);
            btnCancelLoc.HoverState.BorderColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.HoverState.ForeColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.Click += BtnCancelLoc_Click;

            tlpToolbar.Controls.Add(dtpTuNgay, 0, 0);
            tlpToolbar.Controls.Add(lblDivider, 1, 0);
            tlpToolbar.Controls.Add(dtpDenNgay, 2, 0);
            tlpToolbar.Controls.Add(btnLoc, 3, 0);
            tlpToolbar.Controls.Add(btnCancelLoc, 4, 0);

            btnExportPdf = CreateToolbarButton("Xuất PDF", 130);
            btnExportPdf.Click += BtnExportPdf_Click;

            btnPrint = CreateToolbarButton("In Hóa Đơn", 130);
            btnPrint.Click += BtnPrint_Click;
            btnPrint.Margin = new Padding(15, 0, 0, 0);

            tlpToolbar.Controls.Add(btnExportPdf, 6, 0);
            tlpToolbar.Controls.Add(btnPrint, 7, 0);

            rootTable.Controls.Add(tlpToolbar, 0, 0);

            // 2. Guna2DataGridView
            gridHoaDon = new Guna2DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = CREAM,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                Cursor = Cursors.Hand,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                Margin = new Padding(0, 10, 0, 10)
            };

            gridHoaDon.ThemeStyle.HeaderStyle.BackColor = GREEN_DARK;
            gridHoaDon.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            gridHoaDon.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11f, FontStyle.Bold);
            gridHoaDon.ThemeStyle.HeaderStyle.Height = 45;

            gridHoaDon.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridHoaDon.ThemeStyle.AlternatingRowsStyle.BackColor = CREAM;
            gridHoaDon.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5f);
            gridHoaDon.ThemeStyle.RowsStyle.Height = 40;
            gridHoaDon.ThemeStyle.RowsStyle.SelectionBackColor = GREEN_HOVER;
            gridHoaDon.ThemeStyle.RowsStyle.SelectionForeColor = GREEN_DARK;

            gridHoaDon.CellMouseDown += GridHoaDon_CellMouseDown;
            gridHoaDon.CellMouseUp += GridHoaDon_CellMouseUp;
            gridHoaDon.SelectionChanged += GridHoaDon_SelectionChanged;
            gridHoaDon.CellMouseEnter += GridHoaDon_CellMouseEnter;
            gridHoaDon.CellMouseLeave += GridHoaDon_CellMouseLeave;

            gridHoaDon.Columns.Add("MaHDB", "Mã Hóa Đơn");
            gridHoaDon.Columns.Add("NgayBan", "Ngày Tạo");
            gridHoaDon.Columns.Add("MaNV", "Nhân Viên");
            gridHoaDon.Columns.Add("MaPhien", "Mã Phiên");
            gridHoaDon.Columns.Add("TongTien", "Tổng Tiền");

            rootTable.Controls.Add(gridHoaDon, 0, 1);

            Guna2Elipse gridElipse = new Guna2Elipse
            {
                TargetControl = gridHoaDon,
                BorderRadius = 15
            };

            // 3. Pager Panel
            Panel panelBottom = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };

            btnPrev = CreatePagerButton("<");
            btnPrev.Location = new Point(0, 10);
            btnPrev.Click += (s, e) => ChangePage(_currentPage - 1);

            lblPageInfo = new Label
            {
                Text = "Trang 1 / 1",
                Location = new Point(btnPrev.Right + 10, 15),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = GREEN_DARK
            };

            btnNext = CreatePagerButton(">");
            btnNext.Location = new Point(lblPageInfo.Right + 10, 10);
            btnNext.Click += (s, e) => ChangePage(_currentPage + 1);

            panelBottom.Controls.Add(btnPrev);
            panelBottom.Controls.Add(lblPageInfo);
            panelBottom.Controls.Add(btnNext);
            rootTable.Controls.Add(panelBottom, 0, 2);

            this.Controls.Add(rootTable);
            SetActionButtonsState(false);
        }

        private Guna2DateTimePicker CreateDatePicker()
        {
            return new Guna2DateTimePicker
            {
                Size = new Size(160, 42),
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = GREEN_LIGHT,
                FillColor = Color.White,
                ForeColor = GREEN_DARK,
                Format = DateTimePickerFormat.Short,
                Cursor = Cursors.Hand,
                HoverState = { BorderColor = GREEN_DARK },
                CheckedState = {
                    FillColor   = Color.White,
                    BorderColor = GREEN_DARK,
                    ForeColor   = GREEN_DARK
                }
            };
        }

        private Guna2Button CreateToolbarButton(string text, int width)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(width, 42),
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = GREEN_LIGHT,
                FillColor = Color.White,
                ForeColor = GREEN_DARK,
                Font = new System.Drawing.Font("Segoe UI", 10.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                HoverState = {
                    FillColor   = GREEN_HOVER,
                    BorderColor = GREEN_DARK,
                    ForeColor   = GREEN_DARK
                }
            };
        }

        private Guna2Button CreatePagerButton(string text)
        {
            return new Guna2Button
            {
                Text = text,
                Size = new Size(40, 35),
                BorderRadius = 4,
                FillColor = GREEN_DARK,
                ForeColor = Color.White,
                Font = new System.Drawing.Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand,
                HoverState = { FillColor = GREEN_LIGHT }
            };
        }

        // ════════════════════════════════════════════════════════
        //  SỰ KIỆN LỌC VÀ HỦY LỌC NGÀY
        // ════════════════════════════════════════════════════════
        private void BtnLoc_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            if (tuNgay > denNgay)
            {
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _allInvoices = _hoaDonBLL.LayTheoNgay(tuNgay, denNgay) ?? new List<HoaDonBanDTO>();
                _totalPages = (int)Math.Ceiling((double)_allInvoices.Count / _pageSize);
                if (_totalPages == 0) _totalPages = 1;
                ChangePage(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancelLoc_Click(object sender, EventArgs e)
        {
            ResetDatePickers();
            LoadData();
        }

        // ════════════════════════════════════════════════════════
        //  TOGGLE SELECTION
        // ════════════════════════════════════════════════════════
        private void GridHoaDon_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
                _rowToDeselect = gridHoaDon.Rows[e.RowIndex].Selected ? e.RowIndex : -1;
        }

        private void GridHoaDon_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex == _rowToDeselect && e.Button == MouseButtons.Left)
            {
                gridHoaDon.ClearSelection();
                _rowToDeselect = -1;
            }
        }

        // ════════════════════════════════════════════════════════
        //  HOVER
        // ════════════════════════════════════════════════════════
        private void GridHoaDon_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDon.Rows[e.RowIndex].Selected)
                gridHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor = GREEN_HOVER;
        }

        private void GridHoaDon_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDon.Rows[e.RowIndex].Selected)
                gridHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                    (e.RowIndex % 2 == 0) ? Color.White : CREAM;
        }

        // ════════════════════════════════════════════════════════
        //  DATA & PAGINATION
        // ════════════════════════════════════════════════════════
        private void LoadData()
        {
            _allInvoices = _hoaDonBLL.LayTatCa() ?? new List<HoaDonBanDTO>();
            _totalPages = (int)Math.Ceiling((double)_allInvoices.Count / _pageSize);
            if (_totalPages == 0) _totalPages = 1;
            ChangePage(1);
        }

        private void ChangePage(int newPage)
        {
            if (newPage < 1 || newPage > _totalPages) return;
            _currentPage = newPage;

            var pagedData = _allInvoices
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            gridHoaDon.Rows.Clear();
            foreach (var hd in pagedData)
            {
                gridHoaDon.Rows.Add(
                    hd.MaHDB,
                    hd.NgayBan.ToString("dd/MM/yyyy HH:mm"),
                    hd.MaNV,
                    hd.MaPhien,
                    hd.TongTien.ToString("N0") + " đ"
                );
                gridHoaDon.Rows[gridHoaDon.Rows.Count - 1].Tag = hd;
            }

            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}";
            btnNext.Location = new Point(lblPageInfo.Right + 10, 10);
            btnPrev.Enabled = _currentPage > 1;
            btnNext.Enabled = _currentPage < _totalPages;

            gridHoaDon.ClearSelection();
            SetActionButtonsState(false);
        }

        private void GridHoaDon_SelectionChanged(object sender, EventArgs e)
        {
            if (gridHoaDon.SelectedRows.Count > 0)
            {
                _selectedHoaDon = gridHoaDon.SelectedRows[0].Tag as HoaDonBanDTO;
                SetActionButtonsState(true);
            }
            else
            {
                _selectedHoaDon = null;
                SetActionButtonsState(false);
            }
        }

        private void SetActionButtonsState(bool isEnabled)
        {
            btnExportPdf.Enabled = isEnabled;
            btnPrint.Enabled = isEnabled;
            btnExportPdf.FillColor = isEnabled ? Color.White : Color.FromArgb(240, 240, 240);
            btnPrint.FillColor = isEnabled ? Color.White : Color.FromArgb(240, 240, 240);
        }

        // ════════════════════════════════════════════════════════
        //  XUẤT PDF & MỞ PREVIEW
        // ════════════════════════════════════════════════════════
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (_selectedHoaDon == null) return;

            using (var sfd = new SaveFileDialog
            {
                Title = "Lưu hóa đơn PDF",
                Filter = "PDF Documents (*.pdf)|*.pdf",
                FileName = $"HoaDon_{_selectedHoaDon.MaHDB}_{_selectedHoaDon.NgayBan:yyyyMMdd_HHmm}.pdf"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    // Load chi tiết SP theo MaPhien của hóa đơn
                    List<ChiTietPhienDTO> dsChiTiet = new List<ChiTietPhienDTO>();
                    try
                    {
                        dsChiTiet = _chiTietPhienBLL.LayTheoPhien(_selectedHoaDon.MaPhien)
                                    ?? new List<ChiTietPhienDTO>();
                    }
                    catch { /* Nếu lỗi load SP thì vẫn xuất PDF, chỉ thiếu bảng SP */ }

                    ExportToPdf(sfd.FileName, _selectedHoaDon, dsChiTiet);
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất PDF. File có thể đang được mở.\n" + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng in đang kết nối tới máy in...",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ════════════════════════════════════════════════════════
        //  XUẤT PDF — khổ receipt nhiệt 80mm
        //  Y HỆT ThanhToanDialog.ExportToPdf
        // ════════════════════════════════════════════════════════
        private void ExportToPdf(string filePath, HoaDonBanDTO hd,
                                  List<ChiTietPhienDTO> dsChiTiet)
        {
            int soMon = dsChiTiet.Count;
            float rowH = 16f;
            float headerH = 120f;
            float infoH = 130f;
            float tableHeaderH = 22f;
            float tableBodyH = soMon * rowH + 8f;

            bool coGiam = hd.TongTien < (hd.TienBida + hd.TienSanPham);
            float totalH = coGiam ? 96f : 80f;
            float footerH = 50f;

            float pageHeight = headerH + infoH + tableHeaderH + tableBodyH + totalH + footerH;
            float pageWidth = 226.77f;
            float marginLR = 10f;
            float marginTB = 12f;

            var pageSize = new iTextSharp.text.Rectangle(pageWidth, pageHeight);
            var doc = new PdfDocument(pageSize, marginLR, marginLR, marginTB, marginTB);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // ── Fonts ────────────────────────────────────────────────
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

            // ── Header ───────────────────────────────────────────────
            doc.Add(new PdfParagraph("DOUBLE2N BILLIARDS", fShopName)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph("HÓA ĐƠN THANH TOÁN", fTitle)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph(hd.NgayBan.ToString("HH:mm  dd/MM/yyyy"), fSub)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 4f });
            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Thông tin phiên ──────────────────────────────────────
            AddReceiptRow(doc, "Mã HĐ", hd.MaHDB, fLabel, fValue);
            AddReceiptRow(doc, "Nhân viên", hd.MaNV, fLabel, fValue);
            AddReceiptRow(doc, "Mã phiên", hd.MaPhien, fLabel, fValue);

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Bảng sản phẩm ────────────────────────────────────────
            if (dsChiTiet.Count > 0)
            {
                var tbl = new PdfPTable(4) { WidthPercentage = 100, SpacingAfter = 2f };
                tbl.SetWidths(new float[] { 38f, 10f, 22f, 22f });

                // Header bảng
                foreach ((string txt, bool right) in new[]
                {
                    ("Sản phẩm", false),
                    ("SL",       true),
                    ("Đơn giá",  true),
                    ("T.tiền",   true)
                })
                {
                    tbl.AddCell(new PdfPCell(new PdfPhrase(txt, fColHead))
                    {
                        BackgroundColor = new PdfBaseColor(43, 78, 35),
                        Padding = 4f,
                        HorizontalAlignment = right ? PdfElement.ALIGN_RIGHT : PdfElement.ALIGN_LEFT,
                        BorderColor = PdfBaseColor.WHITE
                    });
                }

                // Rows sản phẩm
                bool alt = false;
                foreach (var ct in dsChiTiet)
                {
                    var bg = alt ? new PdfBaseColor(245, 250, 245) : PdfBaseColor.WHITE;
                    string ten = LayTenSP(ct.MaSP);
                    tbl.AddCell(ReceiptCell(ten, fCell, bg, false));
                    tbl.AddCell(ReceiptCell(ct.SoLuong.ToString(), fCell, bg, true));
                    tbl.AddCell(ReceiptCell(ct.DonGia.ToString("N0") + "đ", fCell, bg, true));
                    tbl.AddCell(ReceiptCell((ct.SoLuong * ct.DonGia).ToString("N0") + "đ", fCell, bg, true));
                    alt = !alt;
                }
                doc.Add(tbl);
            }

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Tổng kết ─────────────────────────────────────────────
            AddReceiptRow(doc, "Tiền giờ chơi", hd.TienBida.ToString("N0") + " đ", fLabel, fValue);
            AddReceiptRow(doc, "Tiền sản phẩm", hd.TienSanPham.ToString("N0") + " đ", fLabel, fValue);

            if (coGiam)
            {
                double soTienGiam = (hd.TienBida + hd.TienSanPham) - hd.TongTien;
                string ghiChuGiam = string.IsNullOrWhiteSpace(hd.GhiChu) ? "Chiết khấu" : hd.GhiChu;
                AddReceiptRow(doc, ghiChuGiam, "-" + soTienGiam.ToString("N0") + " đ", fLabel, fDiscount);
            }

            doc.Add(new PdfParagraph(" ") { SpacingAfter = 2f });
            doc.Add(new PdfChunk(sepGreen));
            doc.Add(new PdfParagraph(" "));

            doc.Add(new PdfParagraph($"TỔNG TIỀN:  {hd.TongTien:N0} đ", fTotal)
            { Alignment = PdfElement.ALIGN_RIGHT, SpacingAfter = 4f });

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Footer ───────────────────────────────────────────────
            doc.Add(new PdfParagraph("Cảm ơn quý khách và hẹn gặp lại!", fFooter)
            { Alignment = PdfElement.ALIGN_CENTER });

            doc.Close();
        }

        // ── Helper: tra tên SP từ cache ──────────────────────────
        private string LayTenSP(string maSP)
        {
            if (_cacheTenSP != null &&
                _cacheTenSP.TryGetValue(maSP, out string ten) &&
                !string.IsNullOrWhiteSpace(ten))
                return ten;
            return maSP;
        }

        // ── Receipt helper: 1 dòng label – value ─────────────────
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
                BorderColor = new PdfBaseColor(230, 230, 230)
            };
        }
    }
}
