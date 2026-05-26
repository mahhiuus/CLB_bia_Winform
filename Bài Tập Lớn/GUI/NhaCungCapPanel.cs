using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class NhaCungCapPanel : Form
    {
        // ══════════════════════════════════════════════════════════
        //  Fields
        // ══════════════════════════════════════════════════════════
        private readonly NhaCungCapBLL _bll = new NhaCungCapBLL();
        private bool _dangKhoiTao = true;

        // ── Pagination ────────────────────────────────────────────
        private List<NhaCungCapDTO> _dsDayDu = new List<NhaCungCapDTO>();
        private int _trangHienTai = 1;
        private const int _soDoiMoiTrang = 10;
        private int _tongSoTrang => (int)Math.Ceiling((double)_dsDayDu.Count / _soDoiMoiTrang);

        // ── Pager controls (tạo code, không dùng Designer) ───────
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        // ── Card panel (hiển thị dạng thẻ) ───────────────────────
        private FlowLayoutPanel _flowCards;

        // ══════════════════════════════════════════════════════════
        //  Khởi tạo
        // ══════════════════════════════════════════════════════════
        public NhaCungCapPanel()
        {
            InitializeComponent();
            _dangKhoiTao = true;
            CauHinhGrid();
            TaoPhanTrang();
            TaoFlowCards();
            _dangKhoiTao = false;
            TaiDanhSach();

            this.Load += (s, e) => ApDungBoTron();
            guna2DataGridView1.Resize += (s, e) => ApDungBoTron();
        }

        // ══════════════════════════════════════════════════════════
        //  Tạo nút phân trang vào guna2Panel3
        // ══════════════════════════════════════════════════════════
        private void TaoPhanTrang()
        {
            Color clrBtnNormal = Color.FromArgb(200, 200, 200);
            Color clrBtnHover = Color.FromArgb(170, 170, 170);
            Color clrText = Color.FromArgb(43, 78, 35);

            _btnPrev = new Guna2Button
            {
                Text = "<",
                Size = new Size(32, 28),
                Location = new Point(6, 3),
                BorderRadius = 6,
                FillColor = clrBtnNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnPrev.HoverState.FillColor = clrBtnHover;
            _btnPrev.HoverState.ForeColor = Color.FromArgb(50, 50, 50);
            _btnPrev.Click += (s, e) => ChuyenTrang(_trangHienTai - 1);

            _lblTrangInfo = new Label
            {
                Text = "Trang 1 / 1",
                Size = new Size(110, 28),
                Location = new Point(44, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = clrText,
                BackColor = Color.Transparent,
            };

            _btnNext = new Guna2Button
            {
                Text = ">",
                Size = new Size(32, 28),
                Location = new Point(160, 3),
                BorderRadius = 6,
                FillColor = clrBtnNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnNext.HoverState.FillColor = clrBtnHover;
            _btnNext.HoverState.ForeColor = Color.FromArgb(50, 50, 50);
            _btnNext.Click += (s, e) => ChuyenTrang(_trangHienTai + 1);

            guna2Panel3.Controls.Add(_btnPrev);
            guna2Panel3.Controls.Add(_lblTrangInfo);
            guna2Panel3.Controls.Add(_btnNext);
        }

        // ══════════════════════════════════════════════════════════
        //  Tạo FlowLayoutPanel chứa cards
        // ══════════════════════════════════════════════════════════
        private void TaoFlowCards()
        {
            _flowCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.FromArgb(255, 255, 251),
                Padding = new Padding(10),
                Visible = false,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
            };
            // Thêm vào vị trí của DataGridView (cùng cell)
            tableLayoutPanel1.Controls.Add(_flowCards, 0, 2);
        }

        // ══════════════════════════════════════════════════════════
        //  Chuyển trang
        // ══════════════════════════════════════════════════════════
        private void ChuyenTrang(int trang)
        {
            if (trang < 1 || trang > _tongSoTrang) return;
            _trangHienTai = trang;
            HienThiTrangHienTai();
        }

        private void HienThiTrangHienTai()
        {
            var ds = _dsDayDu
                .Skip((_trangHienTai - 1) * _soDoiMoiTrang)
                .Take(_soDoiMoiTrang)
                .ToList();

            // ── Grid ─────────────────────────────────────────────
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = ds;
            guna2DataGridView1.ClearSelection();

            // ── Cards ────────────────────────────────────────────

            int tongTrang = Math.Max(1, _tongSoTrang);
            _lblTrangInfo.Text = $"Trang {_trangHienTai} / {tongTrang}";

            _btnPrev.Enabled = _trangHienTai > 1;
            _btnNext.Enabled = _trangHienTai < tongTrang;
            _btnPrev.FillColor = _btnPrev.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnNext.FillColor = _btnNext.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnPrev.ForeColor = _btnPrev.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            _btnNext.ForeColor = _btnNext.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
        }

   
        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

 

        // ══════════════════════════════════════════════════════════
        //  Bo tròn DataGridView
        // ══════════════════════════════════════════════════════════
        private void ApDungBoTron()
        {
            const int r = 16;
            var b = guna2DataGridView1.ClientRectangle;
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            guna2DataGridView1.Region = new Region(path);
        }

        // ══════════════════════════════════════════════════════════
        //  Thiết lập Grid
        // ══════════════════════════════════════════════════════════
        private void CauHinhGrid()
        {
            guna2DataGridView1.Enabled = true;
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.AutoGenerateColumns = false;

            Column1.DataPropertyName = "MaNCC";
            Column2.DataPropertyName = "TenCongTy";
            Column3.DataPropertyName = "Sdt";
            Column4.DataPropertyName = "Email";
            Column5.DataPropertyName = "DiaChi";
            Column6.DataPropertyName = "NguoiLienHe";

            Column1.Width = 80;
            Column2.Width = 220;
            Column3.Width = 120;
            Column4.Width = 200;
            Column5.Width = 220;
            Column6.Width = 150;
            Column7.Width = 110;

            guna2DataGridView1.RowTemplate.Height = 38;
            guna2DataGridView1.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            Column7.UseColumnTextForButtonValue = true;
            Column7.Text = "🗑  Xóa";
            Column7.FlatStyle = FlatStyle.Flat;
            Column7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column7.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            // ComboBox tìm kiếm text
            txtTimKiem.PlaceholderText = "Tìm theo mã hoặc tên công ty...";
        }

        // ══════════════════════════════════════════════════════════
        //  Load & Hiển thị dữ liệu
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                _dsDayDu = _bll.LayTatCaNhaCungCap();
                _trangHienTai = 1;
                HienThiTrangHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiGrid(List<NhaCungCapDTO> ds)
        {
            _dsDayDu = ds;
            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  CellFormatting — nút Xóa đỏ đồng đều
        // ══════════════════════════════════════════════════════════
        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;

            e.CellStyle.BackColor = Color.FromArgb(220, 53, 53);
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.SelectionBackColor = Color.FromArgb(180, 20, 20);
            e.CellStyle.SelectionForeColor = Color.White;
        }

        // ══════════════════════════════════════════════════════════
        //  Hover / Press nút Xóa
        // ══════════════════════════════════════════════════════════
        private void guna2DataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.White;
        }

        private void guna2DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.Empty;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.Empty;
        }

        private void guna2DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.White;
        }

        // ══════════════════════════════════════════════════════════
        //  Tìm kiếm
        // ══════════════════════════════════════════════════════════
        private void ThucHienTimKiem()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                var ds = string.IsNullOrWhiteSpace(keyword)
                         ? _bll.LayTatCaNhaCungCap()
                         : _bll.TimKiem(keyword);

                HienThiGrid(ds);

                if (ds.Count == 0)
                    MessageBox.Show("Không tìm thấy nhà cung cấp nào phù hợp.", "Kết quả",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Toggle chế độ xem: Grid / Card
        // ══════════════════════════════════════════════════════════
        private bool _dangXemCard = false;

        private void btnToggleView_Click(object sender, EventArgs e)
        {
            _dangXemCard = !_dangXemCard;

            guna2DataGridView1.Visible = !_dangXemCard;
            _flowCards.Visible = _dangXemCard;

        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện nút
        // ══════════════════════════════════════════════════════════
        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                btnReload.Enabled = false;
                btnReload.Text = "Đang tải...";
                txtTimKiem.Text = "";
                TaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải lại dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnReload.Enabled = true;
                btnReload.Text = "Tải Lại";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            TaiDanhSach();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ThucHienTimKiem();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            ThucHienTimKiem();
        }

        // ── Thêm mới ─────────────────────────────────────────────
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (var popup = new NhaCungCapPopupUi())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowOverlay(this);
                if (popup.ShowDialog(this) == DialogResult.OK)
                    TaiDanhSach();
            }
        }

        // ── Click vào cell ────────────────────────────────────────
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == guna2DataGridView1.Columns["Column7"].Index)
            {
                XuLyXoa(e.RowIndex);
                return;
            }

            string maNCC = guna2DataGridView1.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();
            MoPopupSua(maNCC);
        }

        // ── Mở popup Sửa ─────────────────────────────────────────
        private void MoPopupSua(string maNCC)
        {
            if (string.IsNullOrEmpty(maNCC)) return;

            var ncc = _bll.TimTheoMaNhaCungCap(maNCC);
            if (ncc == null) return;

            using (var popup = new NhaCungCapPopupUi(ncc))
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowOverlay(this);
                if (popup.ShowDialog(this) == DialogResult.OK)
                    TaiDanhSach();
            }
        }

        // ── Xóa ──────────────────────────────────────────────────
        private void XuLyXoa(int rowIndex)
        {
            string maNCC = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            string tenCT = guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value?.ToString();
            if (string.IsNullOrEmpty(maNCC)) return;

            using (var dlg = new ConfirmDeleteUI(tenCT, "nhà cung cấp"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
            }

            try
            {
                if (_bll.XoaNhaCungCap(maNCC))
                {
                    MessageBox.Show("Xóa nhà cung cấp thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSach();
                }
                else
                    MessageBox.Show("Xóa không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Event stubs ───────────────────────────────────────────
        private void MainHeader_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
    }
}