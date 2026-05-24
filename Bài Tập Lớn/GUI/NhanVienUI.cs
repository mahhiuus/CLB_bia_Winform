using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.UI
{
    public partial class NhanVienUI : Form
    {
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color GREEN_ACTIVE_BG = Color.FromArgb(232, 245, 232);
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        static readonly Color BORDER_IDLE = Color.FromArgb(210, 220, 210);
        static readonly Color DANGER = Color.FromArgb(192, 57, 43);

        private readonly NhanVienBLL _bll = new NhanVienBLL();

        private RoundedTextBox txtSearch;
        private RoundedButton btnSearch, btnThem;
        private DataGridView dgv;
        private Panel pnlTop, pnlBar, pnlGrid;
        private Label lblTitle;

        public NhanVienUI()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            BuildUI();
            LoadData();
        }

        private void BuildUI()
        {
            this.Text = "Quản lý Nhân viên";
            this.Size = new Size(1050, 640);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = CREAM;
            this.Font = new Font("Segoe UI", 9.5f);
            this.MinimumSize = new Size(900, 520);

            // Header
            pnlTop = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = GREEN_DARK };
            lblTitle = new Label { Text = "👥  Quản lý Nhân viên", ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 14f), AutoSize = true, Location = new Point(24, 16) };
            pnlTop.Controls.Add(lblTitle);

            // Toolbar
            pnlBar = new Panel { Dock = DockStyle.Top, Height = 58, BackColor = Color.White };
            pnlBar.Paint += (s, e) => e.Graphics.DrawLine(new Pen(BORDER_IDLE, 1), 0, pnlBar.Height - 1, pnlBar.Width, pnlBar.Height - 1);

            txtSearch = new RoundedTextBox { Width = 280, Height = 36, Location = new Point(16, 11), BorderColor = BORDER_IDLE, FocusColor = GREEN_LIGHT };
            txtSearch.TextBoxKeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) DoSearch(); };

            btnSearch = new RoundedButton { Text = "Tìm kiếm", BackColor = GREEN_LIGHT, ForeColor = Color.White, HoverColor = GREEN_DARK, Location = new Point(304, 11), Size = new Size(100, 36), Font = new Font("Segoe UI Semibold", 9f) };
            btnSearch.Click += (s, e) => DoSearch();

            btnThem = new RoundedButton { Text = "＋  Thêm NV", BackColor = GREEN_DARK, ForeColor = Color.White, HoverColor = GREEN_LIGHT, Size = new Size(120, 36), Font = new Font("Segoe UI Semibold", 9f), Anchor = AnchorStyles.Top | AnchorStyles.Right, Location = new Point(pnlBar.Width - 136, 11) };
            btnThem.Click += (s, e) => OpenPopup(null);

            pnlBar.Controls.AddRange(new Control[] { txtSearch, btnSearch, btnThem });

            // Grid
            pnlGrid = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(14, 10, 14, 14) };

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                Font = new Font("Segoe UI", 9.5f),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ColumnHeadersHeight = 40,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = BORDER_IDLE,
                EnableHeadersVisualStyles = false
            };

            dgv.DefaultCellStyle.SelectionBackColor = GREEN_ACTIVE_BG;
            dgv.DefaultCellStyle.SelectionForeColor = GREEN_DARK;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 251, 247);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = GREEN_DARK;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5f);
            dgv.RowTemplate.Height = 42;

            dgv.Columns.AddRange(
                TxtCol("MaNV", "Mã NV", 80),
                TxtCol("HoTen", "Họ Tên", 190),
                TxtCol("Sdt", "Số ĐT", 120),
                TxtCol("GioiTinh", "Giới tính", 90),
                TxtCol("ChucVu", "Chức vụ", 140),
                TxtCol("NgaySinh", "Ngày sinh", 110)
            );

            var colAction = new DataGridViewTextBoxColumn { HeaderText = "Thao tác", Name = "colAction", Width = 90, ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable };
            dgv.Columns.Add(colAction);

            dgv.CellPainting += Dgv_CellPainting;
            dgv.CellClick += Dgv_CellClick;
            dgv.CellMouseEnter += (s, e) => { if (e.ColumnIndex == dgv.Columns["colAction"].Index && e.RowIndex >= 0) dgv.Cursor = Cursors.Hand; };
            dgv.CellMouseLeave += (s, e) => dgv.Cursor = Cursors.Default;

            pnlGrid.Controls.Add(dgv);

            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlBar);
            this.Controls.Add(pnlTop);
        }

        private void Dgv_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != dgv.Columns["colAction"].Index || e.RowIndex < 0) return;
            e.PaintBackground(e.ClipBounds, true);
            bool selected = (e.State & DataGridViewElementStates.Selected) != 0;
            Color bg = selected ? GREEN_ACTIVE_BG : (e.RowIndex % 2 == 1 ? Color.FromArgb(247, 251, 247) : Color.White);
            using (var brush = new SolidBrush(bg)) e.Graphics.FillRectangle(brush, e.CellBounds);

            var font = new Font("Segoe UI", 13f);
            int cx = e.CellBounds.X + 8;
            int cy = e.CellBounds.Y + (e.CellBounds.Height - 20) / 2;

            TextRenderer.DrawText(e.Graphics, "✏", font, new Point(cx, cy), Color.FromArgb(41, 128, 185));
            TextRenderer.DrawText(e.Graphics, "🗑", font, new Point(cx + 36, cy), DANGER);
            e.Handled = true;
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgv.Columns["colAction"].Index) return;
            var nv = dgv.Rows[e.RowIndex].DataBoundItem as NhanVienDTO;
            if (nv == null) return;

            var mousePos = dgv.PointToClient(Control.MousePosition);
            var cellRect = dgv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
            if (mousePos.X - cellRect.X < 32)
            {
                OpenPopup(nv);
            }
            else
            {
                bool confirmed = false;
                using (var dlg = new ConfirmDeleteUI(nv.HoTen)) confirmed = dlg.ShowDialog() == DialogResult.OK;
                if (!confirmed) return;
                try { _bll.XoaNhanVien(nv.MaNV); LoadData(); }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private void LoadData(List<NhanVienDTO> data = null)
        {
            try
            {
                dgv.DataSource = null;
                dgv.DataSource = data ?? _bll.LayTatCaNhanVien();
                if (dgv.Columns.Contains("NgaySinh"))
                    foreach (DataGridViewRow row in dgv.Rows)
                        if (row.Cells["NgaySinh"].Value is DateTime dt) row.Cells["NgaySinh"].Value = dt.ToString("dd/MM/yyyy");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // NhanVienUI
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "NhanVienUI";
            this.Load += new System.EventHandler(this.NhanVienUI_Load);
            this.ResumeLayout(false);

        }

        private void NhanVienUI_Load(object sender, EventArgs e)
        {

        }

        private void DoSearch()
        {
            try { string kw = txtSearch.Text.Trim(); LoadData(string.IsNullOrEmpty(kw) ? _bll.LayTatCaNhanVien() : _bll.TimKiem(kw)); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void OpenPopup(NhanVienDTO nv)
        {
            using (var popup = new NhanVienPopupUI(nv, _bll))
                if (popup.ShowDialog() == DialogResult.OK) LoadData();
        }

        private DataGridViewTextBoxColumn TxtCol(string prop, string header, int w) => new DataGridViewTextBoxColumn { DataPropertyName = prop, HeaderText = header, MinimumWidth = w, FillWeight = w, SortMode = DataGridViewColumnSortMode.Automatic };
    }
}