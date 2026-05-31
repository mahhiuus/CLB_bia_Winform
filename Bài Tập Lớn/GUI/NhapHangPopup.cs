using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    //  NHAP HANG POPUP  (Fixed)
    //  FIX 1: SinhMaCTHDNMoi() không còn gọi DB ngay lúc thêm vào giỏ
    //         → mã sẽ được sinh đúng trong TaoPhieuNhap (NhapHangBLL)
    //  FIX 2: Khi xác nhận, NhapHangBLL.TaoPhieuNhap tự động:
    //         INSERT hoa_don_nhap → INSERT chi_tiet → UPDATE tồn kho
    // ═══════════════════════════════════════════════════════════════
    public partial class NhapHangPopup : Form
    {
        // ── Dependencies ──────────────────────────────────────────
        private readonly NhapHangBLL _bll = new NhapHangBLL();
        private readonly SanPhamBLL _bllSP = new SanPhamBLL();
        private readonly NhaCungCapBLL _bllNCC = new NhaCungCapBLL();

        // ── Overlay ───────────────────────────────────────────────
        private OverlayForm _overlay;

        // ── Giỏ hàng tạm (danh sách chi tiết đang nhập) ──────────
        private readonly List<(ChiTietHoaDonNhapDTO CT, string TenSP)> _gio
            = new List<(ChiTietHoaDonNhapDTO, string)>();

        // ── Kết quả trả về cho panel cha ──────────────────────────
        public HoaDonNhapDTO KetQua { get; private set; }

        // ══════════════════════════════════════════════════════════
        //  Constructor
        // ══════════════════════════════════════════════════════════
        public NhapHangPopup()
        {
            InitializeComponent();

            inputMaHDN.Text = _bll.SinhMaHDNMoi();
            dtpNgayNhap.Value = DateTime.Today;

            NapComboNCC();
            NapComboSanPham();
            CapNhatGrid();
            CapNhatTongTien();

            this.Load += (s, e) => ApDungBoTronGrid();
            dgvChiTiet.Resize += (s, e) => ApDungBoTronGrid();
        }

        // ── Hiện Overlay ─────────────────────────────────────────
        public void ShowOverlay(Form parent)
        {
            _overlay = new OverlayForm();
            _overlay.Show(parent);
            _overlay.StartFade();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _overlay?.Close();
            _overlay = null;
        }

        // ══════════════════════════════════════════════════════════
        //  Nạp ComboBox
        // ══════════════════════════════════════════════════════════
        private void NapComboNCC()
        {
            try
            {
                cboNCC.Items.Clear();
                cboNCC.DisplayMember = "TenCongTy";
                cboNCC.ValueMember = "MaNCC";
                cboNCC.Items.Add(new NhaCungCapDTO { MaNCC = "", TenCongTy = "(Không gán)" });

                var dsNCC = _bllNCC.LayTatCaNhaCungCap();
                if (dsNCC != null)
                    foreach (var ncc in dsNCC)
                        cboNCC.Items.Add(ncc);

                if (cboNCC.Items.Count > 0) cboNCC.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách NCC: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void NapComboSanPham()
        {
            try
            {
                cboSanPham.Items.Clear();
                cboSanPham.DisplayMember = "TenSP";
                cboSanPham.ValueMember = "MaSP";

                var dsSP = _bllSP.TimKiem("");
                if (dsSP != null)
                    foreach (var sp in dsSP)
                        cboSanPham.Items.Add(sp);

                if (cboSanPham.Items.Count > 0) cboSanPham.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách sản phẩm: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện ComboBox sản phẩm thay đổi
        // ══════════════════════════════════════════════════════════
        private void cboSanPham_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblGiaDeXuatVal.Text = "—";
        }

        // ══════════════════════════════════════════════════════════
        //  Tính giá bán đề xuất
        // ══════════════════════════════════════════════════════════
        private void btnTinhGia_Click(object sender, EventArgs e)
        {
            try
            {
                if (!double.TryParse(inputDonGia.Text.Trim(), out double donGia) || donGia < 0)
                    throw new Exception("Đơn giá nhập không hợp lệ!");
                if (!double.TryParse(inputLoiNhuan.Text.Trim(), out double pct) || pct < 0)
                    throw new Exception("Phần trăm lợi nhuận không hợp lệ!");

                double giaDX = _bll.TinhGiaBanDeXuat(donGia, pct);
                lblGiaDeXuatVal.Text = giaDX.ToString("N0") + " đ";
                lblGiaDeXuatVal.ForeColor = Color.FromArgb(200, 90, 20);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void inputDonGia_TextChanged(object sender, EventArgs e)
        {
            lblGiaDeXuatVal.Text = "—";
        }

        // ══════════════════════════════════════════════════════════
        //  Thêm sản phẩm vào giỏ
        //  FIX: KHÔNG sinh mã CT ở đây — để NhapHangBLL.TaoPhieuNhap sinh
        // ══════════════════════════════════════════════════════════
        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboSanPham.SelectedItem == null)
                    throw new Exception("Vui lòng chọn sản phẩm!");
                if (!int.TryParse(inputSoLuong.Text.Trim(), out int soLuong) || soLuong <= 0)
                    throw new Exception("Số lượng phải là số nguyên > 0!");
                if (!double.TryParse(inputDonGia.Text.Trim(), out double donGia) || donGia < 0)
                    throw new Exception("Đơn giá nhập không hợp lệ (phải >= 0)!");

                var sp = cboSanPham.SelectedItem as SanPhamDTO;
                if (sp == null) throw new Exception("Sản phẩm không hợp lệ!");

                // Nếu SP đã có trong giỏ → cộng dồn
                int idx = _gio.FindIndex(g => g.CT.MaSP == sp.MaSP);
                if (idx >= 0)
                {
                    var cu = _gio[idx];
                    var ctMoi = new ChiTietHoaDonNhapDTO
                    {
                        // FIX: mã CT để trống, sẽ sinh trong TaoPhieuNhap
                        MaCTHDN = "",
                        MaHDN = "",
                        MaSP = cu.CT.MaSP,
                        SoLuong = cu.CT.SoLuong + soLuong,
                        DonGiaNhap = donGia
                    };
                    _gio[idx] = (ctMoi, cu.TenSP);
                }
                else
                {
                    var ct = new ChiTietHoaDonNhapDTO
                    {
                        // FIX: không sinh mã ở đây
                        MaCTHDN = "",
                        MaHDN = "",
                        MaSP = sp.MaSP,
                        SoLuong = soLuong,
                        DonGiaNhap = donGia
                    };
                    _gio.Add((ct, sp.TenSP));
                }

                CapNhatGrid();
                CapNhatTongTien();

                // Reset input
                inputSoLuong.Text = "";
                inputDonGia.Text = "";
                lblGiaDeXuatVal.Text = "—";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Cập nhật DataGridView chi tiết
        // ══════════════════════════════════════════════════════════
        private void CapNhatGrid()
        {
            dgvChiTiet.Rows.Clear();
            for (int i = 0; i < _gio.Count; i++)
            {
                var (ct, tenSP) = _gio[i];
                double thanhTien = ct.SoLuong * ct.DonGiaNhap;
                dgvChiTiet.Rows.Add(
                    i + 1,
                    ct.MaSP,
                    tenSP,
                    ct.SoLuong,
                    ct.DonGiaNhap.ToString("N0") + " đ",
                    thanhTien.ToString("N0") + " đ",
                    "Xóa"
                );
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Tính và hiển thị tổng tiền
        // ══════════════════════════════════════════════════════════
        private void CapNhatTongTien()
        {
            double tong = 0;
            foreach (var (ct, _) in _gio)
                tong += ct.SoLuong * ct.DonGiaNhap;
            lblTongTienVal.Text = tong.ToString("N0") + " đ";
        }

        // ══════════════════════════════════════════════════════════
        //  DataGridView events
        // ══════════════════════════════════════════════════════════
        private void dgvChiTiet_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvChiTiet.Columns["ColXoa"].Index) return;
            e.CellStyle.BackColor = Color.FromArgb(220, 53, 53);
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.SelectionBackColor = Color.FromArgb(180, 20, 20);
        }

        private void dgvChiTiet_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvChiTiet.Columns["ColXoa"].Index) return;
            dgvChiTiet.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.FromArgb(185, 28, 28);
            dgvChiTiet.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.White;
        }

        private void dgvChiTiet_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvChiTiet.Columns["ColXoa"].Index) return;
            dgvChiTiet.Rows[e.RowIndex].Cells["ColXoa"].Style.BackColor = Color.Empty;
            dgvChiTiet.Rows[e.RowIndex].Cells["ColXoa"].Style.ForeColor = Color.Empty;
        }

        private void dgvChiTiet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvChiTiet.Columns["ColXoa"].Index) return;
            _gio.RemoveAt(e.RowIndex);
            CapNhatGrid();
            CapNhatTongTien();
        }

        // ══════════════════════════════════════════════════════════
        //  Vẽ header
        // ══════════════════════════════════════════════════════════
        private void panelHeader_Paint(object sender, PaintEventArgs e)
        {
            const string tieuDe = "Tạo Phiếu Nhập Hàng";
            using (var font = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                SizeF size = e.Graphics.MeasureString(tieuDe, font);
                float x = (panelHeader.Width - size.Width) / 2f;
                float y = (panelHeader.Height - size.Height) / 2f;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(tieuDe, font, brush, x, y);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Bo tròn DataGridView
        // ══════════════════════════════════════════════════════════
        private void ApDungBoTronGrid()
        {
            const int r = 12;
            var b = dgvChiTiet.ClientRectangle;
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            dgvChiTiet.Region = new Region(path);
        }

        // ══════════════════════════════════════════════════════════
        //  XÁC NHẬN — Lưu phiếu nhập + cập nhật tồn kho
        //  FIX: Gọi NhapHangBLL.TaoPhieuNhap() thay vì tự INSERT
        // ══════════════════════════════════════════════════════════
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (_gio.Count == 0)
                    throw new Exception("Vui lòng thêm ít nhất 1 sản phẩm vào phiếu nhập!");

                // Lấy mã NCC
                string maNCC = "";
                if (cboNCC.SelectedItem is NhaCungCapDTO selNCC)
                    maNCC = selNCC.MaNCC ?? "";

                if (string.IsNullOrWhiteSpace(maNCC))
                    throw new Exception("Vui lòng chọn nhà cung cấp!");

                // Tính tổng tiền
                double tongTien = 0;
                foreach (var (ct, _) in _gio)
                    tongTien += ct.SoLuong * ct.DonGiaNhap;

                // Tạo header HoaDonNhap
                var hdn = new HoaDonNhapDTO
                {
                    MaHDN = inputMaHDN.Text.Trim(),
                    MaNCC = maNCC,
                    MaNV = "",   // TODO: lấy từ session nếu có
                    NgayNhap = dtpNgayNhap.Value.Date,
                    TongTien = tongTien,
                    GhiChu = inputGhiChu.Text.Trim()
                };

                // Chuẩn hóa list chi tiết (mã CT để BLL sinh)
                var dsChiTiet = new List<ChiTietHoaDonNhapDTO>();
                foreach (var (ct, _) in _gio)
                {
                    dsChiTiet.Add(new ChiTietHoaDonNhapDTO
                    {
                        MaCTHDN = "",        // BLL sẽ sinh: HDNxxx_01, HDNxxx_02...
                        MaHDN = hdn.MaHDN,
                        MaSP = ct.MaSP,
                        SoLuong = ct.SoLuong,
                        DonGiaNhap = ct.DonGiaNhap
                    });
                }

                // Gọi BLL — sẽ INSERT hóa đơn + chi tiết + CẬP NHẬT TỒN KHO
                bool ok = _bll.TaoPhieuNhap(hdn, dsChiTiet);

                if (ok)
                {
                    KetQua = hdn;
                    MessageBox.Show(
                        $"Tạo phiếu nhập {hdn.MaHDN} thành công!\n" +
                        $"Tổng tiền: {tongTien:N0} đ\n" +
                        $"Tồn kho đã được cập nhật.",
                        "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu phiếu không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Hủy
        // ══════════════════════════════════════════════════════════
        private void btnHuy_Click(object sender, EventArgs e)
        {
            if (_gio.Count > 0)
            {
                var confirm = MessageBox.Show(
                    "Bạn đã thêm sản phẩm vào phiếu. Bỏ qua và thoát?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm != DialogResult.Yes) return;
            }
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}