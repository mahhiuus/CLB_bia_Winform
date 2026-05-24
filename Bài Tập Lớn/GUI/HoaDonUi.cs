using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using iTextSharp.text;
using iTextSharp.text.pdf;

// Using các namespace BLL và DTO của bạn
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.GUI
{
    public partial class HoaDonUi : Form
    {
        // ── BLL ──
        private readonly HoaDonBanBLL _hoaDonBLL = new HoaDonBanBLL();

        // ── Colors ──
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_HOVER = ColorTranslator.FromHtml("#e8f5e8");
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);

        // ── Controls ──
        private TableLayoutPanel rootTable;
        private Guna2DataGridView gridHoaDon;
        private Guna2Button btnExportPdf, btnPrint, btnLoc, btnCancelLoc; // [MỚI] Thêm btnCancelLoc
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
            ResetDatePickers();
            LoadData();
        }

        // Hàm đặt lại ngày mặc định (Đầu tháng đến hiện tại)
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

            // 1. TOOLBAR PANEL CẬP NHẬT 8 CỘT
            TableLayoutPanel tlpToolbar = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 1,
                ColumnCount = 8, // [CẬP NHẬT] Tăng lên 8 cột để chứa nút Cancel
                Margin = new Padding(0)
            };

            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // dtpTuNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // lblDivider
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // dtpDenNgay
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // btnLoc
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // btnCancelLoc [MỚI]
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f)); // Khoảng trống đẩy sang phải
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // btnExportPdf
            tlpToolbar.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // btnPrint

            // --- BÊN TRÁI: BỘ LỌC TÌM KIẾM ---
            dtpTuNgay = CreateDatePicker();
            Label lblDivider = new Label { Text = "-", AutoSize = true, Font = new System.Drawing.Font("Segoe UI", 12, FontStyle.Bold), ForeColor = GREEN_DARK, Anchor = AnchorStyles.None };
            dtpDenNgay = CreateDatePicker();

            btnLoc = CreateToolbarButton("Lọc", 80);
            btnLoc.Click += BtnLoc_Click;

            // [MỚI] Khởi tạo nút Cancel cho bộ chọn ngày
            btnCancelLoc = CreateToolbarButton("Hủy", 80);
            btnCancelLoc.BorderColor = Color.FromArgb(220, 80, 80); // Viền đỏ nhẹ cho nút Cancel phân biệt
            btnCancelLoc.ForeColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.HoverState.FillColor = Color.FromArgb(255, 240, 240);
            btnCancelLoc.HoverState.BorderColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.HoverState.ForeColor = Color.FromArgb(180, 40, 40);
            btnCancelLoc.Click += BtnCancelLoc_Click;

            tlpToolbar.Controls.Add(dtpTuNgay, 0, 0);
            tlpToolbar.Controls.Add(lblDivider, 1, 0);
            tlpToolbar.Controls.Add(dtpDenNgay, 2, 0);
            tlpToolbar.Controls.Add(btnLoc, 3, 0);
            tlpToolbar.Controls.Add(btnCancelLoc, 4, 0); // Đưa nút cancel vào cột số 4

            // --- BÊN PHẢI: NÚT THAO TÁC ---
            btnExportPdf = CreateToolbarButton("Xuất PDF", 130);
            btnExportPdf.Click += BtnExportPdf_Click;

            btnPrint = CreateToolbarButton("In Hóa Đơn", 130);
            btnPrint.Click += BtnPrint_Click;
            btnPrint.Margin = new Padding(15, 0, 0, 0);

            tlpToolbar.Controls.Add(btnExportPdf, 6, 0); // Đẩy sang cột 6
            tlpToolbar.Controls.Add(btnPrint, 7, 0);    // Đẩy sang cột 7

            rootTable.Controls.Add(tlpToolbar, 0, 0);

            // 2. Guna2DataGridView - Bảng Dữ Liệu Bo Tròn
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

            // [CẬP NHẬT] Màu khi click chọn: Đổi thành màu dịu nhẹ y hệt như khi Hover
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

            // Bo tròn bảng dữ liệu
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

        // [CẬP NHẬT] Thiết lập ô chọn ngày Đẹp - Hiện đại: Nền Trắng, Viền Xanh
        private Guna2DateTimePicker CreateDatePicker()
        {
            return new Guna2DateTimePicker
            {
                Size = new Size(160, 42),
                BorderRadius = 6,
                BorderThickness = 1,
                BorderColor = GREEN_LIGHT,
                FillColor = Color.White, // Ép nền trắng ban đầu
                ForeColor = GREEN_DARK,
                Format = DateTimePickerFormat.Short,
                Cursor = Cursors.Hand,
                HoverState = { BorderColor = GREEN_DARK },
                // Ép trạng thái Checked/Selected vẫn luôn giữ Nền Trắng - Viền Xanh Đậm
                CheckedState = {
                    FillColor = Color.White,
                    BorderColor = GREEN_DARK,
                    ForeColor = GREEN_DARK
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
                    FillColor = GREEN_HOVER,
                    BorderColor = GREEN_DARK,
                    ForeColor = GREEN_DARK
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
                MessageBox.Show("Từ ngày không được lớn hơn Đến ngày!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Lỗi lấy dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // [MỚI] Sự kiện click nút Cancel -> Khôi phục dữ liệu gốc toàn bộ hóa đơn
        private void BtnCancelLoc_Click(object sender, EventArgs e)
        {
            ResetDatePickers(); // Đặt lại ngày mặc định trên UI
            LoadData();         // Tải lại toàn bộ dữ liệu gốc từ database
        }

        // ════════════════════════════════════════════════════════
        //  CƠ CHẾ "BẤM LẦN NỮA ĐỂ HỦY CHỌN" (TOGGLE SELECTION)
        // ════════════════════════════════════════════════════════
        private void GridHoaDon_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Button == MouseButtons.Left)
            {
                if (gridHoaDon.Rows[e.RowIndex].Selected)
                {
                    _rowToDeselect = e.RowIndex;
                }
                else
                {
                    _rowToDeselect = -1;
                }
            }
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
        //  HIỆU ỨNG HOVER TRÊN BẢNG 
        // ════════════════════════════════════════════════════════
        private void GridHoaDon_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDon.Rows[e.RowIndex].Selected)
            {
                gridHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor = GREEN_HOVER;
            }
        }

        private void GridHoaDon_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && !gridHoaDon.Rows[e.RowIndex].Selected)
            {
                gridHoaDon.Rows[e.RowIndex].DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0) ? Color.White : CREAM;
            }
        }

        // ════════════════════════════════════════════════════════
        //  LOGIC XỬ LÝ (PAGINATION & DATA)
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
        //  XUẤT PDF & TỰ ĐỘNG BẬT PREVIEW
        // ════════════════════════════════════════════════════════
        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            if (_selectedHoaDon == null) return;

            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "PDF Documents (*.pdf)|*.pdf", FileName = $"HoaDon_{_selectedHoaDon.MaHDB}.pdf" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        ExportToPdf(sfd.FileName, _selectedHoaDon);
                        Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xuất PDF. File có thể đang được mở.\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng in đang kết nối tới máy in...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ExportToPdf(string filePath, HoaDonBanDTO hd)
        {
            Document document = new Document(PageSize.A5, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            string fontPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\arial.ttf";
            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            iTextSharp.text.Font fontTitle = new iTextSharp.text.Font(bf, 18, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font fontNormal = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font fontTotal = new iTextSharp.text.Font(bf, 15, iTextSharp.text.Font.BOLD, BaseColor.RED);

            Paragraph title = new Paragraph("DOUBLE2N BILLIARDS\nHÓA ĐƠN THANH TOÁN\n\n", fontTitle) { Alignment = Element.ALIGN_CENTER };
            document.Add(title);

            document.Add(new Paragraph($"Mã Hóa Đơn: {hd.MaHDB}", fontNormal));
            document.Add(new Paragraph($"Ngày Tạo: {hd.NgayBan:dd/MM/yyyy HH:mm}", fontNormal));
            document.Add(new Paragraph($"Nhân Viên: {hd.MaNV}", fontNormal));
            document.Add(new Paragraph($"Mã Phiên: {hd.MaPhien}", fontNormal));

            iTextSharp.text.pdf.draw.LineSeparator separator = new iTextSharp.text.pdf.draw.LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, 1);
            document.Add(new Chunk(separator));
            document.Add(new Paragraph("\n"));

            document.Add(new Paragraph($"Tiền Giờ Bida: {hd.TienBida:N0} đ", fontNormal));
            document.Add(new Paragraph($"Tiền Dịch Vụ: {hd.TienSanPham:N0} đ", fontNormal));

            document.Add(new Chunk(separator));
            document.Add(new Paragraph("\n"));

            Paragraph total = new Paragraph($"TỔNG TIỀN: {hd.TongTien:N0} đ", fontTotal) { Alignment = Element.ALIGN_RIGHT };
            document.Add(total);

            Paragraph footer = new Paragraph("\nCảm ơn quý khách và hẹn gặp lại!", fontNormal) { Alignment = Element.ALIGN_CENTER };
            document.Add(footer);

            document.Close();
        }
    }
}