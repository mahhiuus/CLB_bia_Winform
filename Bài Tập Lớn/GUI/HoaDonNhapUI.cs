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
    public partial class HoaDonNhapUi : Form
    {
        private readonly HoaDonNhapBLL _hoaDonNhapBLL = new HoaDonNhapBLL();
        private readonly ChiTietHoaDonNhapBLL _chiTietHDNBLL = new ChiTietHoaDonNhapBLL();
        private readonly SanPhamBLL _sanPhamBLL = new SanPhamBLL();
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_HOVER = ColorTranslator.FromHtml("#e8f5e8");
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        private TableLayoutPanel rootTable;
        private Guna2DataGridView gridHoaDonNhap;
        private Guna2Button btnExportPdf, btnPrint, btnLoc, btnCancelLoc, btnReload;
        private Guna2DateTimePicker dtpTuNgay, dtpDenNgay;
        private Label lblPageInfo;
        private Guna2Button btnPrev, btnNext;
        private List<HoaDonNhapDTO> _allInvoices = new List<HoaDonNhapDTO>();
        private int _currentPage = 1;
        private int _pageSize = 12;
        private int _totalPages = 1;
        private HoaDonNhapDTO _selectedHoaDon = null;
        private int _rowToDeselect = -1;
        private Dictionary<string, string> _cacheTenSP = new Dictionary<string, string>();

        public HoaDonNhapUi()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            BuildLayout();
            this.Load += HoaDonNhapUi_Load;
        }

        private void HoaDonNhapUi_Load(object sender, EventArgs e)
        {
            BuildCacheTenSP();
            ResetDatePickers();
            LoadData();
        }
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

            // 1. TOOLBAR PANEL 9 CỘT
            TableLayoutPanel tlpToolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 9,
                Margin = new Padding(0)
            };
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 0: dtpTuNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 1: lblDivider
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 2: dtpDenNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 3: btnLoc
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 4: btnCancelLoc
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 5: btnReload
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // 6: spacer
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 7: btnExportPdf
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));       // 8: btnPrint

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

            btnReload = CreateToolbarButton("Làm mới", 110);
            btnReload.Click += BtnReload_Click;

            tlpToolbar.Controls.Add(dtpTuNgay, 0, 0);
            tlpToolbar.Controls.Add(lblDivider, 1, 0);
            tlpToolbar.Controls.Add(dtpDenNgay, 2, 0);
            tlpToolbar.Controls.Add(btnLoc, 3, 0);
            tlpToolbar.Controls.Add(btnCancelLoc, 4, 0);
            tlpToolbar.Controls.Add(btnReload, 5, 0);

            btnExportPdf = CreateToolbarButton("Xuất PDF", 130);
            btnExportPdf.Click += BtnExportPdf_Click;

            btnPrint = CreateToolbarButton("In Hóa Đơn", 130);
            btnPrint.Click += BtnPrint_Click;
            btnPrint.Margin = new Padding(15, 0, 0, 0);

            tlpToolbar.Controls.Add(btnExportPdf, 7, 0);
            tlpToolbar.Controls.Add(btnPrint, 8, 0);

            rootTable.Controls.Add(tlpToolbar, 0, 0);

            gridHoaDonNhap = new Guna2DataGridView
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

            gridHoaDonNhap.ThemeStyle.HeaderStyle.BackColor = GREEN_DARK;
            gridHoaDonNhap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            gridHoaDonNhap.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 11f, FontStyle.Bold);
            gridHoaDonNhap.ThemeStyle.HeaderStyle.Height = 45;

            gridHoaDonNhap.ThemeStyle.RowsStyle.BackColor = Color.White;
            gridHoaDonNhap.ThemeStyle.AlternatingRowsStyle.BackColor = CREAM;
            gridHoaDonNhap.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 10.5f);
            gridHoaDonNhap.ThemeStyle.RowsStyle.Height = 40;
            gridHoaDonNhap.ThemeStyle.RowsStyle.SelectionBackColor = GREEN_HOVER;
            gridHoaDonNhap.ThemeStyle.RowsStyle.SelectionForeColor = GREEN_DARK;

            gridHoaDonNhap.CellMouseDown += GridHoaDonNhap_CellMouseDown;
            gridHoaDonNhap.CellMouseUp += GridHoaDonNhap_CellMouseUp;
            gridHoaDonNhap.SelectionChanged += GridHoaDonNhap_SelectionChanged;
            gridHoaDonNhap.CellMouseEnter += GridHoaDonNhap_CellMouseEnter;
            gridHoaDonNhap.CellMouseLeave += GridHoaDonNhap_CellMouseLeave;

            gridHoaDonNhap.Columns.Add("MaHDN", "Mã Hóa Đơn");
            gridHoaDonNhap.Columns.Add("NgayNhap", "Ngày Nhập");
            gridHoaDonNhap.Columns.Add("MaNV", "Nhân Viên");
            gridHoaDonNhap.Columns.Add("MaNCC", "Nhà Cung Cấp");
            gridHoaDonNhap.Columns.Add("TongTien", "Tổng Tiền Nhập");

            rootTable.Controls.Add(gridHoaDonNhap, 0, 1);

            Guna2Elipse gridElipse = new Guna2Elipse
            {
                TargetControl = gridHoaDonNhap,
                BorderRadius = 15
            };

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
                _allInvoices = _hoaDonNhapBLL.LayTheoNgay(tuNgay, denNgay) ?? new List<HoaDonNhapDTO>();
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
        private void BtnReload_Click(object sender, EventArgs e)
        {
            ResetDatePickers();
            LoadData();
        }
        private void GridHoaDonNhap_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
                _rowToDeselect = gridHoaDonNhap.Rows[e.RowIndex].Selected ? e.RowIndex : -1;
        }
        private void GridHoaDonNhap_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex == _rowToDeselect && e.Button == MouseButtons.Left)
            {
                gridHoaDonNhap.ClearSelection();
                _rowToDeselect = -1;
            }
        }
        private void GridHoaDonNhap_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDonNhap.Rows[e.RowIndex].Selected)
                gridHoaDonNhap.Rows[e.RowIndex].DefaultCellStyle.BackColor = GREEN_HOVER;
        }

        private void GridHoaDonNhap_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDonNhap.Rows[e.RowIndex].Selected)
                gridHoaDonNhap.Rows[e.RowIndex].DefaultCellStyle.BackColor =
                    (e.RowIndex % 2 == 0) ? Color.White : CREAM;
        }
        private void LoadData()
        {
            try
            {
                _allInvoices = _hoaDonNhapBLL.LayTatCaHoaDonNhap() ?? new List<HoaDonNhapDTO>();
                _totalPages = (int)Math.Ceiling((double)_allInvoices.Count / _pageSize);
                if (_totalPages == 0) _totalPages = 1;
                ChangePage(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu hóa đơn nhập: " + ex.Message,
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ChangePage(int newPage)
        {
            if (newPage < 1 || newPage > _totalPages) return;
            _currentPage = newPage;

            var pagedData = _allInvoices
                .Skip((_currentPage - 1) * _pageSize)
                .Take(_pageSize)
                .ToList();

            gridHoaDonNhap.Rows.Clear();
            foreach (var hdn in pagedData)
            {
                gridHoaDonNhap.Rows.Add(
                    hdn.MaHDN,
                    hdn.NgayNhap.ToString("dd/MM/yyyy HH:mm"),
                    hdn.MaNV,
                    hdn.MaNCC,
                    hdn.TongTien.ToString("N0") + " đ"
                );
                gridHoaDonNhap.Rows[gridHoaDonNhap.Rows.Count - 1].Tag = hdn;
            }

            lblPageInfo.Text = $"Trang {_currentPage} / {_totalPages}";
            btnNext.Location = new Point(lblPageInfo.Right + 10, 10);
            btnPrev.Enabled = _currentPage > 1;
            btnNext.Enabled = _currentPage < _totalPages;

            gridHoaDonNhap.ClearSelection();
            SetActionButtonsState(false);
        }
        private void GridHoaDonNhap_SelectionChanged(object sender, EventArgs e)
        {
            if (gridHoaDonNhap.SelectedRows.Count > 0)
            {
                _selectedHoaDon = gridHoaDonNhap.SelectedRows[0].Tag as HoaDonNhapDTO;
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
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (_selectedHoaDon == null) return;

            using (var sfd = new SaveFileDialog
            {
                Title = "Lưu phiếu nhập hàng PDF",
                Filter = "PDF Documents (*.pdf)|*.pdf",
                FileName = $"HoaDonNhap_{_selectedHoaDon.MaHDN}_{_selectedHoaDon.NgayNhap:yyyyMMdd_HHmm}.pdf"
            })
            {
                if (sfd.ShowDialog() != DialogResult.OK) return;

                try
                {
                    List<ChiTietHoaDonNhapDTO> dsChiTiet = new List<ChiTietHoaDonNhapDTO>();
                    try
                    {
                        dsChiTiet = _chiTietHDNBLL.TimTheoMaHDN(_selectedHoaDon.MaHDN)
                                    ?? new List<ChiTietHoaDonNhapDTO>();
                    }
                    catch {}

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
            MessageBox.Show("Chức năng in hóa đơn nhập đang kết nối tới máy in văn phòng...",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ExportToPdf(string filePath, HoaDonNhapDTO hdn,
                                  List<ChiTietHoaDonNhapDTO> dsChiTiet)
        {
            int soMon = dsChiTiet.Count;
            float rowH = 16f;
            float headerH = 120f;
            float infoH = !string.IsNullOrWhiteSpace(hdn.GhiChu) ? 150f : 130f;
            float tableHeaderH = soMon > 0 ? 22f : 0f;
            float tableBodyH = soMon > 0 ? soMon * rowH + 8f : 0f;
            float totalH = 80f;
            float signH = 80f;
            float footerH = 50f;

            float pageHeight = headerH + infoH + tableHeaderH + tableBodyH + totalH + signH + footerH;
            float pageWidth = 226.77f;
            float marginLR = 10f;
            float marginTB = 12f;

            var pageSize = new iTextSharp.text.Rectangle(pageWidth, pageHeight);
            var doc = new PdfDocument(pageSize, marginLR, marginLR, marginTB, marginTB);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

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
            var fTotal = new PdfFont(bf, 10f, PdfFont.BOLD, new PdfBaseColor(43, 78, 35));
            var fSign = new PdfFont(bf, 7f, PdfFont.NORMAL, PdfBaseColor.BLACK);
            var fFooter = new PdfFont(bf, 7f, PdfFont.ITALIC, new PdfBaseColor(140, 140, 140));

            var sepGray = new iTextSharp.text.pdf.draw.LineSeparator(
                0.4f, 100f, PdfBaseColor.LIGHT_GRAY, PdfElement.ALIGN_CENTER, 1);
            var sepGreen = new iTextSharp.text.pdf.draw.LineSeparator(
                0.6f, 100f, new PdfBaseColor(43, 78, 35), PdfElement.ALIGN_CENTER, 1);

            // ── Header ───────────────────────────────────────────────
            doc.Add(new PdfParagraph("DOUBLE2N BILLIARDS", fShopName)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph("PHIẾU NHẬP HÀNG", fTitle)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 2f });
            doc.Add(new PdfParagraph(hdn.NgayNhap.ToString("HH:mm  dd/MM/yyyy"), fSub)
            { Alignment = PdfElement.ALIGN_CENTER, SpacingAfter = 4f });
            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            AddReceiptRow(doc, "Mã phiếu nhập", hdn.MaHDN, fLabel, fValue);
            AddReceiptRow(doc, "Nhân viên", hdn.MaNV, fLabel, fValue);
            AddReceiptRow(doc, "Nhà cung cấp", hdn.MaNCC, fLabel, fValue);
            AddReceiptRow(doc, "Ngày nhập", hdn.NgayNhap.ToString("dd/MM/yyyy HH:mm"), fLabel, fValue);

            if (!string.IsNullOrWhiteSpace(hdn.GhiChu))
                AddReceiptRow(doc, "Ghi chú", hdn.GhiChu, fLabel, fValue);

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            if (dsChiTiet.Count > 0)
            {
                var tbl = new PdfPTable(4) { WidthPercentage = 100, SpacingAfter = 2f };
                tbl.SetWidths(new float[] { 38f, 10f, 22f, 22f });

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

                bool alt = false;
                foreach (var ct in dsChiTiet)
                {
                    var bg = alt ? new PdfBaseColor(245, 250, 245) : PdfBaseColor.WHITE;
                    string ten = LayTenSP(ct.MaSP);
                    tbl.AddCell(ReceiptCell(ten, fCell, bg, false));
                    tbl.AddCell(ReceiptCell(ct.SoLuong.ToString(), fCell, bg, true));
                    tbl.AddCell(ReceiptCell(ct.DonGiaNhap.ToString("N0") + "đ", fCell, bg, true));
                    tbl.AddCell(ReceiptCell((ct.SoLuong * ct.DonGiaNhap).ToString("N0") + "đ", fCell, bg, true));
                    alt = !alt;
                }
                doc.Add(tbl);
            }

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            doc.Add(new PdfParagraph(" ") { SpacingAfter = 2f });
            doc.Add(new PdfChunk(sepGreen));
            doc.Add(new PdfParagraph(" "));

            doc.Add(new PdfParagraph($"TỔNG TIỀN THANH TOÁN:  {hdn.TongTien:N0} đ", fTotal)
            { Alignment = PdfElement.ALIGN_RIGHT, SpacingAfter = 4f });

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            var tblSign = new PdfPTable(2) { WidthPercentage = 100, SpacingAfter = 4f };
            tblSign.SetWidths(new float[] { 50f, 50f });
            tblSign.AddCell(new PdfPCell(new PdfPhrase("Người Lập Phiếu\n(Ký, họ tên)", fSign))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfElement.ALIGN_CENTER,
                PaddingTop = 2f,
                PaddingBottom = 2f
            });
            tblSign.AddCell(new PdfPCell(new PdfPhrase("Người Giao Hàng\n(Ký, họ tên)", fSign))
            {
                Border = PdfPCell.NO_BORDER,
                HorizontalAlignment = PdfElement.ALIGN_CENTER,
                PaddingTop = 2f,
                PaddingBottom = 2f
            });
            doc.Add(tblSign);

            doc.Add(new PdfChunk(sepGray));
            doc.Add(new PdfParagraph(" "));

            // ── Footer ───────────────────────────────────────────────
            doc.Add(new PdfParagraph("Cảm ơn và hẹn gặp lại!", fFooter)
            { Alignment = PdfElement.ALIGN_CENTER });

            doc.Close();
        }

        private string LayTenSP(string maSP)
        {
            if (_cacheTenSP != null &&
                _cacheTenSP.TryGetValue(maSP, out string ten) &&
                !string.IsNullOrWhiteSpace(ten))
                return ten;
            return maSP;
        }

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