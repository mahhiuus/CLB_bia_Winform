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
    public partial class KhachHangPanel : Form
    {
        // ══════════════════════════════════════════════════════════
        //  Fields
        // ══════════════════════════════════════════════════════════
        private readonly KhachHangBLL _bll = new KhachHangBLL();
        private bool _dangKhoiTao = true;

        // ── Pagination ────────────────────────────────────────────
        private List<KhachHangDTO> _dsDayDu = new List<KhachHangDTO>();
        private int _trangHienTai = 1;
        private const int _soDoiMoiTrang = 10;
        private int _tongSoTrang => (int)Math.Ceiling((double)_dsDayDu.Count / _soDoiMoiTrang);

        // ── Pager controls ────────────────────────────────────────
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        // ══════════════════════════════════════════════════════════
        //  Khởi tạo
        // ══════════════════════════════════════════════════════════
        public KhachHangPanel()
        {
            InitializeComponent();
            _dangKhoiTao = true;
            CauHinhGrid();
            TaoPhanTrang();
            _dangKhoiTao = false;
            TaiDanhSach();

            this.Load += (s, e) => ApDungBoTron();
            guna2DataGridView1.Resize += (s, e) => ApDungBoTron();
        }

        // ══════════════════════════════════════════════════════════
        //  Phân trang
        // ══════════════════════════════════════════════════════════
        private void TaoPhanTrang()
        {
            Color clrNormal = Color.FromArgb(200, 200, 200);
            Color clrHover = Color.FromArgb(170, 170, 170);
            Color clrText = Color.FromArgb(43, 78, 35);

            _btnPrev = new Guna2Button
            {
                Text = "<",
                Size = new Size(32, 28),
                Location = new Point(6, 3),
                BorderRadius = 6,
                FillColor = clrNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnPrev.HoverState.FillColor = clrHover;
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
                FillColor = clrNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnNext.HoverState.FillColor = clrHover;
            _btnNext.HoverState.ForeColor = Color.FromArgb(50, 50, 50);
            _btnNext.Click += (s, e) => ChuyenTrang(_trangHienTai + 1);

            guna2Panel3.Controls.Add(_btnPrev);
            guna2Panel3.Controls.Add(_lblTrangInfo);
            guna2Panel3.Controls.Add(_btnNext);
        }

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

            // [SỬA Ở ĐÂY]: Đã XÓA vòng lặp foreach đổi Value thành chuỗi.
            // Việc format ngày đã được thực hiện chuẩn chỉ ở hàm CauHinhGrid() bên dưới.

            int tongTrang = Math.Max(1, _tongSoTrang);
            _lblTrangInfo.Text = string.Format("Trang {0} / {1}", _trangHienTai, tongTrang);

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
        //  Cấu hình Grid
        // ══════════════════════════════════════════════════════════
        private void CauHinhGrid()
        {
            guna2DataGridView1.Enabled = true;
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.AutoGenerateColumns = false;

            // [SỬA Ở ĐÂY]: Xử lý gọn sự kiện DataError mặc định để chặn popup báo lỗi
            guna2DataGridView1.DataError += (s, e) => { e.Cancel = true; };

            Column1.DataPropertyName = "MaKH";
            Column2.DataPropertyName = "HoTen";
            Column3.DataPropertyName = "Sdt";
            Column4.DataPropertyName = "DiaChi";
            Column5.DataPropertyName = "DiemTichLuy";
            Column6.DataPropertyName = "NgayDangKy";

            // [SỬA Ở ĐÂY]: Định dạng ngày trực tiếp trên Cột thay vì đổi value thành chuỗi
            Column6.DefaultCellStyle.Format = "dd/MM/yyyy";

            // Column7 = nút Xóa (không bind property)

            Column1.Width = 80;
            Column2.Width = 200;
            Column3.Width = 130;
            Column4.Width = 210;
            Column5.Width = 120;
            Column6.Width = 120;
            Column7.Width = 90;
            Column7.FlatStyle = FlatStyle.Flat;
        }

        // ══════════════════════════════════════════════════════════
        //  Tải danh sách
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                var ds = _bll.LayTatCaKhachHang();
                HienThiGrid(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiGrid(List<KhachHangDTO> ds)
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
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;

            e.CellStyle.BackColor = Color.FromArgb(220, 53, 53);
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.SelectionBackColor = Color.FromArgb(180, 20, 20);
            e.CellStyle.SelectionForeColor = Color.White;
        }

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
                       ? _bll.LayTatCaKhachHang()
                       : _bll.TimKiem(keyword);

                HienThiGrid(ds);

                if (ds.Count == 0)
                    MessageBox.Show("Không tìm thấy khách hàng nào phù hợp.", "Kết quả",
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
                MessageBox.Show(ex.Message, "Lỗi tải lại",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnReload.Enabled = true;
                btnReload.Text = "🔄  Tải Lại";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            txtTimKiem.Text = "";
            TaiDanhSach();
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) ThucHienTimKiem();
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => ThucHienTimKiem();

        // ── Thêm mới ─────────────────────────────────────────────
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (var popup = new KhachHangPopupUi())
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

            string maKH = guna2DataGridView1.Rows[e.RowIndex].Cells["Column1"].Value?.ToString();
            MoPopupSua(maKH);
        }

        // ── Mở popup Sửa ─────────────────────────────────────────
        private void MoPopupSua(string maKH)
        {
            if (string.IsNullOrEmpty(maKH)) return;

            var kh = _bll.TimTheoMaKhachHang(maKH);
            if (kh == null) return;

            using (var popup = new KhachHangPopupUi(kh))
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
            string maKH = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            string hoTen = guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value?.ToString();
            if (string.IsNullOrEmpty(maKH)) return;

            bool confirmed = false;
            using (var dlg = new ConfirmDeleteUI(hoTen, "khách hàng"))
            {
                confirmed = dlg.ShowDialog(this) == DialogResult.OK;
            }
            if (!confirmed) return;

            try
            {
                if (_bll.XoaKhachHang(maKH))
                {
                    MessageBox.Show("Xóa khách hàng thành công!", "Thành công",
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

        // ── Vẽ tiêu đề banner ────────────────────────────────────


        // ── Event stubs ───────────────────────────────────────────
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}