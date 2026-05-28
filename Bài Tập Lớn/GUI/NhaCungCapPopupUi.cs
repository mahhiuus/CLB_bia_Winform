using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    // NHA CUNG CAP POPUP UI
    // ═══════════════════════════════════════════════════════════════
    public partial class NhaCungCapPopupUi : Form
    {
        private readonly NhaCungCapBLL _bll = new NhaCungCapBLL();
        private readonly bool _laSua;
        private OverlayForm _overlay;

        public NhaCungCapDTO KetQua { get; private set; }
        public bool DaXoa { get; private set; } = false;

        // ── Constructor: Thêm mới ─────────────────────────────────
        public NhaCungCapPopupUi()
        {
            InitializeComponent();
            _laSua = false;
            inputMaNCC.Text = _bll.SinhMaMoi();
            inputMaNCC.ReadOnly = true;
        }

        // ── Constructor: Sửa ─────────────────────────────────────
        public NhaCungCapPopupUi(NhaCungCapDTO ncc) : this()
        {
            _laSua = true;
            inputMaNCC.Text = ncc.MaNCC;
            inputTenCongTy.Text = ncc.TenCongTy;
            inputSdt.Text = ncc.Sdt;
            inputEmail.Text = ncc.Email;
            inputDiaChi.Text = ncc.DiaChi;
            inputNguoiLH.Text = ncc.NguoiLienHe;

            // Khi sửa hiện nút Xóa
        }

        // ── Hiện Overlay ─────────────────────────────────────────
        public void ShowOverlay(Form parent)
        {
            _overlay = new OverlayForm();
            _overlay.Show(parent);
            _overlay.StartFade();
        }

        // ── Tự đóng overlay khi popup đóng ───────────────────────
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _overlay?.Close();
            _overlay = null;
        }

        // ── Vẽ tiêu đề header panel ──────────────────────────────
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            string tieuDe = _laSua ? "Sửa Nhà Cung Cấp" : "Thêm Nhà Cung Cấp";

            using (Font font = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                SizeF size = e.Graphics.MeasureString(tieuDe, font);
                float x = (guna2Panel1.Width - size.Width) / 2f;
                float y = (guna2Panel1.Height - size.Height) / 2f;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(tieuDe, font, brush, x, y);
            }
        }

        // ── Xác Nhận ─────────────────────────────────────────────
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputTenCongTy.Text))
                    throw new Exception("Vui lòng nhập Tên Công Ty!");

                if (string.IsNullOrWhiteSpace(inputSdt.Text))
                    throw new Exception("Vui lòng nhập Số Điện Thoại!");

                var ncc = new NhaCungCapDTO
                {
                    MaNCC = inputMaNCC.Text.Trim(),
                    TenCongTy = inputTenCongTy.Text.Trim(),
                    Sdt = inputSdt.Text.Trim(),
                    Email = inputEmail.Text.Trim(),
                    DiaChi = inputDiaChi.Text.Trim(),
                    NguoiLienHe = inputNguoiLH.Text.Trim(),
                };

                bool ok = _laSua
                    ? _bll.CapNhatNhaCungCap(ncc)
                    : _bll.ThemNhaCungCap(ncc);

                if (ok)
                {
                    KetQua = ncc;
                    MessageBox.Show(
                        _laSua ? "Cập nhật nhà cung cấp thành công!" : "Thêm nhà cung cấp thành công!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thao tác không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Xóa ──────────────────────────────────────────────────
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string tenCT = inputTenCongTy.Text.Trim();

            using (var popup = new ConfirmDeleteUI(tenCT, "nhà cung cấp"))
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    bool ok = _bll.XoaNhaCungCap(inputMaNCC.Text.Trim());
                    if (ok)
                    {
                        DaXoa = true;
                        MessageBox.Show($"Đã xóa \"{tenCT}\" thành công!",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Xóa không thành công!", "Thất bại",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // ── Hủy ──────────────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ── Event stubs giữ Designer không lỗi ───────────────────
        private void inputMaNCC_Load(object sender, EventArgs e) { }
        private void inputTenCongTy_Load(object sender, EventArgs e) { }
        private void inputSdt_Load(object sender, EventArgs e) { }
        private void inputEmail_Load(object sender, EventArgs e) { }
        private void inputDiaChi_Load(object sender, EventArgs e) { }
        private void inputNguoiLH_Load(object sender, EventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel7_Paint(object sender, PaintEventArgs e) { }
        private void lblMaNCC_Click(object sender, EventArgs e) { }
        private void lblTenCongTy_Click(object sender, EventArgs e) { }
        private void lblSdt_Click(object sender, EventArgs e) { }
        private void lblEmail_Click(object sender, EventArgs e) { }
        private void lblDiaChi_Click(object sender, EventArgs e) { }
        private void lblNguoiLH_Click(object sender, EventArgs e) { }
    }
}