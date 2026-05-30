using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class SanPhamPanel : Form
    {
        // ══════════════════════════════════════════════════════════
        //  Fields
        // ══════════════════════════════════════════════════════════
        private readonly SanPhamBLL _bll = new SanPhamBLL();
        private bool _dangKhoiTao = true;

        // ── Pagination ────────────────────────────────────────────
        private List<SanPhamDTO> _dsDayDu = new List<SanPhamDTO>();
        private int _trangHienTai = 1;
        private const int _soDoiMoiTrang = 10;
        private int _tongSoTrang => (int)Math.Ceiling((double)_dsDayDu.Count / _soDoiMoiTrang);

        // ── Pager controls ────────────────────────────────────────
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        // ── FlowLayoutPanel (ẩn, giữ cấu trúc giống mẫu) ─────────
        private FlowLayoutPanel _flowCards;

        // ══════════════════════════════════════════════════════════
        //  Khởi tạo
        // ══════════════════════════════════════════════════════════
        public SanPhamPanel()
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
        //  Tạo FlowLayoutPanel (ẩn)
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
            tableLayoutPanel1.Controls.Add(_flowCards, 0, 2);
        }

        // ══════════════════════════════════════════════════════════
        //  Phân trang
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

            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = ds;
            guna2DataGridView1.ClearSelection();

            int tongTrang = Math.Max(1, _tongSoTrang);
            _lblTrangInfo.Text = $"Trang {_trangHienTai} / {tongTrang}";

            _btnPrev.Enabled = _trangHienTai > 1;
            _btnNext.Enabled = _trangHienTai < tongTrang;
            _btnPrev.FillColor = _btnPrev.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnNext.FillColor = _btnNext.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnPrev.ForeColor = _btnPrev.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            _btnNext.ForeColor = _btnNext.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
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

            // Bind property name → column
            ColMaSP.DataPropertyName = "MaSP";
            ColTenSP.DataPropertyName = "TenSP";
            ColLoai.DataPropertyName = "Loai";
            ColGiaBan.DataPropertyName = "GiaBan";
            ColSoLuong.DataPropertyName = "SoLuongTon";
            ColMaNCC.DataPropertyName = "MaNCC";
            ColHinhAnh.DataPropertyName = "HinhAnh";

            // Độ rộng
            ColMaSP.Width = 80;
            ColTenSP.Width = 220;
            ColLoai.Width = 130;
            ColGiaBan.Width = 120;
            ColSoLuong.Width = 110;
            ColMaNCC.Width = 110;
            ColHinhAnh.Width = 160;
            ColXoa.Width = 90;

            // Format cột giá
            ColGiaBan.DefaultCellStyle.Format = "N0";
            ColGiaBan.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ColXoa.FlatStyle = FlatStyle.Flat;

        }

        // ══════════════════════════════════════════════════════════
        //  Tải dữ liệu
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                var ds = _bll.TimKiem("");
                HienThiGrid(ds ?? new List<SanPhamDTO>());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiGrid(List<SanPhamDTO> ds)
        {
            _dsDayDu = ds;
            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  CellFormatting — nút Xóa đỏ
        // ══════════════════════════════════════════════════════════
        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["ColXoa"].Index) return;

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
            if (e.ColumnIndex != guna2DataGridView1.Columns["ColXoa"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
        }

        private void guna2DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["ColXoa"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.Empty;
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.Empty;
        }

        private void guna2DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["ColXoa"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
        }

        // ══════════════════════════════════════════════════════════
        //  Tìm kiếm
        // ══════════════════════════════════════════════════════════
        private void ThucHienTimKiem()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                var ds = _bll.TimKiem(keyword);

                HienThiGrid(ds ?? new List<SanPhamDTO>());

                if (ds == null || ds.Count == 0)
                    MessageBox.Show("Không tìm thấy sản phẩm nào phù hợp.", "Kết quả",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện toolbar
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

        private void btnLabelTitle_Click(object sender, EventArgs e)
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
            using (var popup = new SanPhamPopupUi())
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

            // Nút Xóa
            if (e.ColumnIndex == guna2DataGridView1.Columns["ColXoa"].Index)
            {
                XuLyXoa(e.RowIndex);
                return;
            }

            // Click ô khác → mở popup sửa
            string maSP = guna2DataGridView1.Rows[e.RowIndex].Cells["ColMaSP"].Value?.ToString();
            MoPopupSua(maSP);
        }

        // ── Mở popup Sửa ─────────────────────────────────────────
        private void MoPopupSua(string maSP)
        {
            if (string.IsNullOrEmpty(maSP)) return;

            var dsSP = _bll.TimTheoMaSanPham(maSP);
            if (dsSP == null || dsSP.Count == 0) return;

            var sp = dsSP[0];

            using (var popup = new SanPhamPopupUi(sp))
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
            string maSP = guna2DataGridView1.Rows[rowIndex].Cells["ColMaSP"].Value?.ToString();
            string tenSP = guna2DataGridView1.Rows[rowIndex].Cells["ColTenSP"].Value?.ToString();
            if (string.IsNullOrEmpty(maSP)) return;

            using (var dlg = new ConfirmDeleteUI(tenSP, "sản phẩm"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
            }

            try
            {
                if (_bll.XoaSanPham(maSP))
                {
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thành công",
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
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }

        private void guna2Panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}