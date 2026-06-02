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
    public partial class NhapHangPanel : Form
    {
        // ── Dependencies ──────────────────────────────────────────
        private readonly NhapHangBLL _bll = new NhapHangBLL();

        // ── Pagination ────────────────────────────────────────────
        private List<HoaDonNhapDTO> _dsDayDu = new List<HoaDonNhapDTO>();
        private int _trangHienTai = 1;
        private const int _soDoiMoiTrang = 12;
        private int _tongSoTrang => (int)Math.Ceiling((double)_dsDayDu.Count / _soDoiMoiTrang);

        // ── Pager controls ────────────────────────────────────────
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        // ══════════════════════════════════════════════════════════
        //  Constructor
        // ══════════════════════════════════════════════════════════
        public NhapHangPanel()
        {
            InitializeComponent();
            CauHinhGrid();
            TaoPhanTrang();
            TaiDanhSach();

            this.Load += (s, e) => ApDungBoTronGrid();
            dgvPhieuNhap.Resize += (s, e) => ApDungBoTronGrid();
        }

        // ══════════════════════════════════════════════════════════
        //  Tải danh sách
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                var ds = _bll.LayTatCa();
                HienThiGrid(ds ?? new List<HoaDonNhapDTO>());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách phiếu nhập: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Cấu hình DataGridView
        // ══════════════════════════════════════════════════════════
        private void CauHinhGrid()
        {
            dgvPhieuNhap.Enabled = true;
            dgvPhieuNhap.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuNhap.ReadOnly = true;
            dgvPhieuNhap.AutoGenerateColumns = false;

            // Cấu hình giao diện cho các cột nút bấm
            if (dgvPhieuNhap.Columns.Contains("ColXemCT") && dgvPhieuNhap.Columns["ColXemCT"] is DataGridViewButtonColumn btnXem)
            {
                btnXem.UseColumnTextForButtonValue = true;
                btnXem.FlatStyle = FlatStyle.Flat;
            }
            if (dgvPhieuNhap.Columns.Contains("ColXoa") && dgvPhieuNhap.Columns["ColXoa"] is DataGridViewButtonColumn btnXoa)
            {
                btnXoa.UseColumnTextForButtonValue = true;
                btnXoa.FlatStyle = FlatStyle.Flat;
            }

            // Đăng ký các sự kiện chuột và màu sắc
            dgvPhieuNhap.CellFormatting += dgvPhieuNhap_CellFormatting;
            dgvPhieuNhap.CellMouseEnter += dgvPhieuNhap_CellMouseEnter;
            dgvPhieuNhap.CellMouseLeave += dgvPhieuNhap_CellMouseLeave;
            dgvPhieuNhap.CellMouseDown += dgvPhieuNhap_CellMouseDown;
        }

        // ══════════════════════════════════════════════════════════
        //  Hiển thị grid (phân trang)
        // ══════════════════════════════════════════════════════════
        private void HienThiGrid(List<HoaDonNhapDTO> ds)
        {
            _dsDayDu = ds;
            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        private void HienThiTrangHienTai()
        {
            var ds = _dsDayDu
                .Skip((_trangHienTai - 1) * _soDoiMoiTrang)
                .Take(_soDoiMoiTrang)
                .ToList();

            dgvPhieuNhap.Rows.Clear();

            foreach (var hdn in ds)
            {
                dgvPhieuNhap.Rows.Add(
                    hdn.MaHDN,
                    hdn.MaNCC,
                    hdn.MaNV,
                    hdn.NgayNhap.ToString("dd/MM/yyyy"),
                    hdn.TongTien.ToString("N0") + " đ",
                    hdn.GhiChu,
                    "Xem CT",
                    "Xóa"
                );
            }

            dgvPhieuNhap.ClearSelection();

            // ── Cập nhật pager ────────────────────────────────────
            int tongTrang = Math.Max(1, _tongSoTrang);
            _lblTrangInfo.Text = $"Trang {_trangHienTai} / {tongTrang}";

            _btnPrev.Enabled = _trangHienTai > 1;
            _btnNext.Enabled = _trangHienTai < tongTrang;

            // Enabled → xanh lá đậm  |  Disabled → xám nhạt
            _btnPrev.FillColor = _btnPrev.Enabled
                ? Color.FromArgb(43, 78, 35) : Color.FromArgb(210, 210, 210);
            _btnNext.FillColor = _btnNext.Enabled
                ? Color.FromArgb(43, 78, 35) : Color.FromArgb(210, 210, 210);

            _btnPrev.ForeColor = _btnPrev.Enabled
                ? Color.White : Color.FromArgb(170, 170, 170);
            _btnNext.ForeColor = _btnNext.Enabled
                ? Color.White : Color.FromArgb(170, 170, 170);

            _btnPrev.BorderColor = _btnPrev.Enabled
                ? Color.FromArgb(38, 68, 20) : Color.FromArgb(190, 190, 190);
            _btnNext.BorderColor = _btnNext.Enabled
                ? Color.FromArgb(38, 68, 20) : Color.FromArgb(190, 190, 190);
        }

        // ══════════════════════════════════════════════════════════
        //  Phân trang
        // ══════════════════════════════════════════════════════════
        private void TaoPhanTrang()
        {
            // Màu khởi tạo — sẽ được ghi đè trong HienThiTrangHienTai()
            Color clrActive = Color.FromArgb(43, 78, 35);
            Color clrDisabled = Color.FromArgb(210, 210, 210);

            _btnPrev = new Guna2Button
            {
                Text = "<",
                Size = new Size(32, 28),
                Location = new Point(6, 10),
                BorderRadius = 6,
                FillColor = clrDisabled,          // trang 1 → Prev bị disabled
                ForeColor = Color.FromArgb(170, 170, 170),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(190, 190, 190),
                BorderThickness = 1
            };
            // Hover chỉ hiện khi nút enabled (màu xanh sáng hơn)
            _btnPrev.HoverState.FillColor = Color.FromArgb(60, 110, 48);
            _btnPrev.HoverState.ForeColor = Color.White;
            _btnPrev.Click += (s, e) => ChuyenTrang(_trangHienTai - 1);

            _lblTrangInfo = new Label
            {
                Text = "Trang 1 / 1",
                Size = new Size(120, 28),
                Location = new Point(44, 10),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(43, 78, 35),
                BackColor = Color.Transparent
            };

            _btnNext = new Guna2Button
            {
                Text = ">",
                Size = new Size(32, 28),
                Location = new Point(170, 10),
                BorderRadius = 6,
                FillColor = clrActive,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(38, 68, 20),
                BorderThickness = 1
            };
            _btnNext.HoverState.FillColor = Color.FromArgb(60, 110, 48);
            _btnNext.HoverState.ForeColor = Color.White;
            _btnNext.Click += (s, e) => ChuyenTrang(_trangHienTai + 1);

            pnlPager.Controls.Add(_btnPrev);
            pnlPager.Controls.Add(_lblTrangInfo);
            pnlPager.Controls.Add(_btnNext);
        }

        private void ChuyenTrang(int trang)
        {
            if (trang < 1 || trang > _tongSoTrang) return;
            _trangHienTai = trang;
            HienThiTrangHienTai();
        }

        // ══════════════════════════════════════════════════════════
        //  Bo tròn DataGridView  (giống TaiKhoanPanel)
        // ══════════════════════════════════════════════════════════
        private void ApDungBoTronGrid()
        {
            const int r = 16;
            var b = dgvPhieuNhap.ClientRectangle;
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            dgvPhieuNhap.Region = new Region(path);
        }

        // ══════════════════════════════════════════════════════════
        //  CellFormatting — Set màu gốc
        // ══════════════════════════════════════════════════════════
        private void dgvPhieuNhap_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int colXemCT = dgvPhieuNhap.Columns.Contains("ColXemCT") ? dgvPhieuNhap.Columns["ColXemCT"].Index : -1;
            int colXoa = dgvPhieuNhap.Columns.Contains("ColXoa") ? dgvPhieuNhap.Columns["ColXoa"].Index : -1;

            if (e.ColumnIndex == colXemCT)
            {
                e.CellStyle.BackColor = Color.FromArgb(43, 78, 35);       // Xanh lá gốc
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.FromArgb(34, 62, 28); // Xanh lá đậm khi select
                e.CellStyle.SelectionForeColor = Color.White;
            }
            else if (e.ColumnIndex == colXoa)
            {
                e.CellStyle.BackColor = Color.FromArgb(220, 53, 53);      // Đỏ gốc
                e.CellStyle.ForeColor = Color.White;
                e.CellStyle.SelectionBackColor = Color.FromArgb(185, 28, 28); // Đỏ đậm khi select
                e.CellStyle.SelectionForeColor = Color.White;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Hover — Đậm màu lên khi chuột đi vào
        // ══════════════════════════════════════════════════════════
        private void dgvPhieuNhap_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int colXemCT = dgvPhieuNhap.Columns.Contains("ColXemCT") ? dgvPhieuNhap.Columns["ColXemCT"].Index : -1;
            int colXoa = dgvPhieuNhap.Columns.Contains("ColXoa") ? dgvPhieuNhap.Columns["ColXoa"].Index : -1;

            if (e.ColumnIndex == colXemCT)
            {
                // Xanh lá đậm hơn
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.BackColor = Color.FromArgb(34, 62, 28);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.ForeColor = Color.White;
            }
            else if (e.ColumnIndex == colXoa)
            {
                // Đỏ đậm hơn
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(185, 28, 28);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Leave — Trả lại màu gốc (Không dùng Color.Empty để tránh bị xám)
        // ══════════════════════════════════════════════════════════
        private void dgvPhieuNhap_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int colXemCT = dgvPhieuNhap.Columns.Contains("ColXemCT") ? dgvPhieuNhap.Columns["ColXemCT"].Index : -1;
            int colXoa = dgvPhieuNhap.Columns.Contains("ColXoa") ? dgvPhieuNhap.Columns["ColXoa"].Index : -1;

            if (e.ColumnIndex == colXemCT)
            {
                // Trả về xanh lá gốc
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.BackColor = Color.FromArgb(43, 78, 35);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.ForeColor = Color.White;
            }
            else if (e.ColumnIndex == colXoa)
            {
                // Trả về đỏ gốc
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(220, 53, 53);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
            }
        }

        // ── Press (CellMouseDown) ─────────────
        private void dgvPhieuNhap_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int colXemCT = dgvPhieuNhap.Columns.Contains("ColXemCT") ? dgvPhieuNhap.Columns["ColXemCT"].Index : -1;
            int colXoa = dgvPhieuNhap.Columns.Contains("ColXoa") ? dgvPhieuNhap.Columns["ColXoa"].Index : -1;

            if (e.ColumnIndex == colXemCT)
            {
                // Xanh lá tối nhất
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.BackColor = Color.FromArgb(25, 45, 20);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXemCT"].Style.ForeColor = Color.White;
            }
            else if (e.ColumnIndex == colXoa)
            {
                // Đỏ tối nhất
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(150, 20, 20);
                dgvPhieuNhap.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Click cell
        // ══════════════════════════════════════════════════════════
        private void dgvPhieuNhap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string maHDN = dgvPhieuNhap.Rows[e.RowIndex].Cells["ColMaHDN"].Value?.ToString();

            if (e.ColumnIndex == dgvPhieuNhap.Columns["ColXoa"].Index)
                XuLyXoa(e.RowIndex, maHDN);
            else if (e.ColumnIndex == dgvPhieuNhap.Columns["ColXemCT"].Index)
                XemChiTiet(maHDN);
        }

        // ══════════════════════════════════════════════════════════
        //  Xem chi tiết phiếu nhập
        // ══════════════════════════════════════════════════════════
        private void XemChiTiet(string maHDN)
        {
            if (string.IsNullOrEmpty(maHDN)) return;

            try
            {
                var dsChiTiet = _bll.LayChiTietTheoMaHDN(maHDN);
                if (dsChiTiet == null || dsChiTiet.Count == 0)
                {
                    MessageBox.Show("Không có chi tiết cho phiếu nhập này.",
                        "Thông cấu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new Form())
                {
                    dlg.Text = $"Chi Tiết Phiếu Nhập — {maHDN}";
                    dlg.Size = new Size(720, 440);
                    dlg.StartPosition = FormStartPosition.CenterParent;
                    dlg.FormBorderStyle = FormBorderStyle.FixedDialog;
                    dlg.MaximizeBox = false;
                    dlg.BackColor = Color.FromArgb(255, 255, 251);

                    var lbl = new Label
                    {
                        Text = $"Chi tiết phiếu nhập hàng: {maHDN}",
                        Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                        ForeColor = Color.FromArgb(43, 78, 35),
                        Dock = DockStyle.Top,
                        Height = 40,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    var dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        AllowUserToAddRows = false,
                        ReadOnly = true,
                        RowHeadersVisible = false,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                        BackgroundColor = Color.White,
                        GridColor = Color.FromArgb(210, 220, 210),
                        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                        MultiSelect = false,
                        ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                        ColumnHeadersHeight = 38
                    };
                    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(43, 78, 35);
                    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold);
                    dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    dgv.Columns.Add("MaCT", "Mã CT");
                    dgv.Columns.Add("MaSP", "Mã SP");
                    dgv.Columns.Add("SoLuong", "Số Lượng");
                    dgv.Columns.Add("DonGia", "Đơn Giá Nhập");
                    dgv.Columns.Add("ThanhTien", "Thành Tiền");

                    foreach (var ct in dsChiTiet)
                        dgv.Rows.Add(ct.MaCTHDN, ct.MaSP, ct.SoLuong,
                            ct.DonGiaNhap.ToString("N0") + " đ",
                            (ct.SoLuong * ct.DonGiaNhap).ToString("N0") + " đ");

                    var btnDong = new Button
                    {
                        Text = "Đóng",
                        Dock = DockStyle.Bottom,
                        Height = 40,
                        BackColor = Color.FromArgb(43, 78, 35),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                        Cursor = Cursors.Hand
                    };
                    btnDong.Click += (s, ev) => dlg.Close();

                    dlg.Controls.Add(dgv);
                    dlg.Controls.Add(lbl);
                    dlg.Controls.Add(btnDong);
                    dlg.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Xóa phiếu nhập
        // ══════════════════════════════════════════════════════════
        private void XuLyXoa(int rowIndex, string maHDN)
        {
            if (string.IsNullOrEmpty(maHDN)) return;

            using (var dlg = new ConfirmDeleteUI(maHDN, "phiếu nhập"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
            }

            try
            {
                if (_bll.XoaPhieuNhap(maHDN))
                {
                    MessageBox.Show($"Đã xóa phiếu nhập {maHDN} thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSach();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Toolbar events
        // ══════════════════════════════════════════════════════════
        private void btnTaoPhieu_Click(object sender, EventArgs e)
        {
            using (var popup = new NhapHangPopup())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowOverlay(this);
                if (popup.ShowDialog(this) == DialogResult.OK)
                    TaiDanhSach();
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                btnReload.Enabled = false;
                btnReload.Text = "Đang tải...";
                txtTimKiem.Text = "";
                TaiDanhSach();
            }
            finally
            {
                btnReload.Enabled = true;
                btnReload.Text = "↺  Tải Lại";
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e) => ThucHienTimKiem();

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) ThucHienTimKiem();
        }

        private void ThucHienTimKiem()
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim();
                var ds = _bll.TimKiem(keyword);
                HienThiGrid(ds ?? new List<HoaDonNhapDTO>());

                if (ds == null || ds.Count == 0)
                    MessageBox.Show("Không tìm thấy phiếu nhập nào phù hợp.",
                        "Kết quả", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}