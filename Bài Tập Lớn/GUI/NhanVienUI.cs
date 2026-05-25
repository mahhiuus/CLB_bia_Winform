using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class NhanVienUI : Form
    {
        // ═══════════════════════════════════════════════════════
        //  PALETTE — dùng xuyên suốt toàn form
        // ═══════════════════════════════════════════════════════
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_ACTIVE_BG = Color.FromArgb(232, 245, 232);
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        static readonly Color BORDER_IDLE = Color.FromArgb(210, 220, 210);

        // ═══════════════════════════════════════════════════════
        //  ICON NỔI (floating action labels)
        // ═══════════════════════════════════════════════════════
        private Label _lblSua;
        private Label _lblXoa;
        private int _hoveredRowIndex = -1;

        private readonly NhanVienBLL _bll = new NhanVienBLL();

        public NhanVienUI()
        {
            InitializeComponent();
            ApplyTheme();
            this.BackColor = Color.White;
        }

        // ==================== ÁP DỤNG MÀU GIAO DIỆN ====================
        private void ApplyTheme()
        {
            this.BackColor = CREAM;
            this.tableLayoutPanel1.BackColor = CREAM;
            this.MainHeader.FillColor = CREAM;
            this.guna2Panel2.FillColor = CREAM;

            this.guna2DataGridView1.BackgroundColor = CREAM;
            this.guna2DataGridView1.ThemeStyle.BackColor = CREAM;
            this.guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = GREEN_DARK;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = CREAM;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = GREEN_ACTIVE_BG;
            this.guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = GREEN_DARK;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = GREEN_ACTIVE_BG;
            this.guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = GREEN_DARK;

            this.btnTimKiem.BorderColor = GREEN_DARK;
            this.btnTimKiem.FillColor = GREEN_LIGHT;
            this.btnTimKiem.HoverState.FillColor = GREEN_DARK;

            this.btnThem.BorderColor = GREEN_DARK;
            this.btnThem.FillColor = GREEN_DARK;
            this.btnThem.HoverState.FillColor = GREEN_LIGHT;

            this.inputTimKiem.BorderColor = BORDER_IDLE;
            this.inputTimKiem.FocusColor = GREEN_LIGHT;
        }

        // ==================== LOAD FORM ====================
        private void NhanVienUI_Load(object sender, EventArgs e)
        {
            CauHinhDataGridView();
            TaoIconNoi();
            TaiDanhSach();
        }

        // ==================== CẤU HÌNH DATAGRIDVIEW ====================
        private void CauHinhDataGridView()
        {
            guna2DataGridView1.AutoGenerateColumns = false;
            guna2DataGridView1.Columns.Clear();
            guna2DataGridView1.ReadOnly = true;

            var headerFont = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
            var dataFont = new Font("Segoe UI", 10f);

            void AddText(string name, string header, string prop, int w,
                DataGridViewContentAlignment align = DataGridViewContentAlignment.MiddleLeft)
            {
                guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    HeaderText = header,
                    DataPropertyName = prop,
                    Width = w,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    DefaultCellStyle = new DataGridViewCellStyle { Font = dataFont, Alignment = align }
                });
            }

            AddText("colMaNV", "Mã NV", "MaNV", 80, DataGridViewContentAlignment.MiddleCenter);
            AddText("colHoTen", "Họ Tên", "HoTen", 205);
            AddText("colSdt", "SĐT", "Sdt", 135, DataGridViewContentAlignment.MiddleCenter);
            AddText("colGioiTinh", "Giới Tính", "GioiTinh", 95, DataGridViewContentAlignment.MiddleCenter);
            AddText("colChucVu", "Chức Vụ", "ChucVu", 145, DataGridViewContentAlignment.MiddleCenter);

            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colNgaySinh",
                HeaderText = "Ngày Sinh",
                DataPropertyName = "NgaySinh",
                Width = 115,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = dataFont,
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy"
                }
            });

            // Cột trống — chỗ đậu cho 2 icon nổi
            guna2DataGridView1.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colActions",
                HeaderText = "",
                Width = 90,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                ReadOnly = true
            });

            // ── Header style ────────────────────────────────────────────────────
            guna2DataGridView1.EnableHeadersVisualStyles = false;
            guna2DataGridView1.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = headerFont,
                BackColor = GREEN_DARK,
                ForeColor = Color.White,
                SelectionBackColor = GREEN_DARK,
                SelectionForeColor = Color.White,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                WrapMode = DataGridViewTriState.False
            };
            guna2DataGridView1.ColumnHeadersHeight = 44;
            guna2DataGridView1.ColumnHeadersHeightSizeMode =
                DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            guna2DataGridView1.ColumnHeaderMouseClick += (s, ev) =>
            {
                guna2DataGridView1.ColumnHeadersDefaultCellStyle.BackColor = GREEN_DARK;
                guna2DataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = GREEN_DARK;
            };

            // ── Row / Cell styles ───────────────────────────────────────────────
            guna2DataGridView1.RowTemplate.Height = 40;
            guna2DataGridView1.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = dataFont,
                ForeColor = Color.FromArgb(35, 35, 35),
                BackColor = CREAM,
                SelectionBackColor = GREEN_ACTIVE_BG,
                SelectionForeColor = GREEN_DARK
            };
            guna2DataGridView1.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                Font = dataFont,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(35, 35, 35),
                SelectionBackColor = GREEN_ACTIVE_BG,
                SelectionForeColor = GREEN_DARK
            };
        }

        // ==================== TẠO ICON NỔI ====================
        private void TaoIconNoi()
        {
            var iconFont = new Font("Segoe MDL2 Assets", 13f);

            _lblSua = new Label
            {
                Text = "\uE70F",
                Font = iconFont,
                Size = new Size(38, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = GREEN_LIGHT,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Visible = false,
                BorderStyle = BorderStyle.None
            };

            _lblXoa = new Label
            {
                Text = "\uE74D",
                Font = iconFont,
                Size = new Size(38, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = GREEN_DARK,
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Visible = false,
                BorderStyle = BorderStyle.None
            };

            _lblSua.MouseEnter += (s, e) => _lblSua.BackColor = ColorTranslator.FromHtml("#5a9a50");
            _lblSua.MouseLeave += IconLabel_MouseLeave;
            _lblXoa.MouseEnter += (s, e) => _lblXoa.BackColor = ColorTranslator.FromHtml("#1e3a17");
            _lblXoa.MouseLeave += IconLabel_MouseLeave;

            _lblSua.Click += (s, e) => HandleSua();
            _lblXoa.Click += (s, e) => HandleXoa();

            guna2DataGridView1.Controls.Add(_lblXoa);
            guna2DataGridView1.Controls.Add(_lblSua);
            _lblSua.BringToFront();
            _lblXoa.BringToFront();

            guna2DataGridView1.CellMouseEnter += (s, ev) =>
            {
                if (ev.RowIndex < 0) return;
                _hoveredRowIndex = ev.RowIndex;
                CapNhatViTriIcon();
            };

            guna2DataGridView1.CellMouseLeave += (s, ev) =>
            {
                Point pos = guna2DataGridView1.PointToClient(Cursor.Position);
                if (_lblSua.Bounds.Contains(pos) || _lblXoa.Bounds.Contains(pos)) return;
                _lblSua.Visible = false;
                _lblXoa.Visible = false;
                _hoveredRowIndex = -1;
            };

            guna2DataGridView1.Scroll += (s, ev) => CapNhatViTriIcon();
        }

        private void IconLabel_MouseLeave(object sender, EventArgs e)
        {
            _lblSua.BackColor = GREEN_LIGHT;
            _lblXoa.BackColor = GREEN_DARK;

            Point pos = guna2DataGridView1.PointToClient(Cursor.Position);
            bool overDgv = guna2DataGridView1.ClientRectangle.Contains(pos);
            if (!overDgv)
            {
                _lblSua.Visible = false;
                _lblXoa.Visible = false;
                _hoveredRowIndex = -1;
            }
        }

        private void CapNhatViTriIcon()
        {
            if (_hoveredRowIndex < 0 || _hoveredRowIndex >= guna2DataGridView1.Rows.Count)
            {
                _lblSua.Visible = _lblXoa.Visible = false;
                return;
            }

            Rectangle rowRect = guna2DataGridView1.GetRowDisplayRectangle(_hoveredRowIndex, false);
            if (rowRect.IsEmpty) { _lblSua.Visible = _lblXoa.Visible = false; return; }

            const int iconW = 38, iconH = 30, gap = 6, margin = 10;
            int y = rowRect.Top + (rowRect.Height - iconH) / 2;
            int x2 = guna2DataGridView1.ClientSize.Width - margin - iconW;
            int x1 = x2 - gap - iconW;

            _lblSua.SetBounds(x1, y, iconW, iconH);
            _lblXoa.SetBounds(x2, y, iconW, iconH);
            _lblSua.Visible = _lblXoa.Visible = true;
            _lblSua.BringToFront();
            _lblXoa.BringToFront();
        }

        private void HandleSua()
        {
            if (_hoveredRowIndex < 0) return;
            string maNV = guna2DataGridView1.Rows[_hoveredRowIndex]
                              .Cells["colMaNV"].Value?.ToString();
            if (string.IsNullOrEmpty(maNV)) return;

            NhanVienDTO nv = _bll.TimTheoMa(maNV);
            if (nv == null) return;

            var popup = new NhanVienPopupUI(nv, _bll);
            popup.FormClosed += (s, args) => TaiDanhSach();
            popup.ShowDialog();
        }

        private void HandleXoa()
        {
            if (_hoveredRowIndex < 0) return;
            string maNV = guna2DataGridView1.Rows[_hoveredRowIndex]
                              .Cells["colMaNV"].Value?.ToString();
            if (string.IsNullOrEmpty(maNV)) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa nhân viên [{maNV}]?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                try
                {
                    _bll.XoaNhanVien(maNV);
                    MessageBox.Show("Xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSach();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa: " + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void TaiDanhSach(string keyword = "")
        {
            try
            {
                List<NhanVienDTO> ds = string.IsNullOrWhiteSpace(keyword)
                    ? _bll.LayTatCaNhanVien()
                    : _bll.TimKiem(keyword);

                guna2DataGridView1.DataSource = null;
                guna2DataGridView1.DataSource = ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTimKiem_Click_1(object sender, EventArgs e)
            => TaiDanhSach(inputTimKiem.Text.Trim());

        private void btnThem_Click(object sender, EventArgs e)
        {
            var popup = new NhanVienPopupUI(null, _bll);
            popup.FormClosed += (s, args) => TaiDanhSach();
            popup.ShowDialog();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void MainHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void inputTimKiem_Load(object sender, EventArgs e)
        {

        }
    }
}