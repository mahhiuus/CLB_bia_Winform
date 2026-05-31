using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    // TAI KHOAN POPUP UI
    // ═══════════════════════════════════════════════════════════════
    public partial class TaiKhoanPopupUi : Form
    {
        private readonly TaiKhoanBLL _bll = new TaiKhoanBLL();
        private readonly NhanVienBLL _bllNV = new NhanVienBLL();   // load Guna2ComboBox NV
        private readonly bool _laSua;
        private OverlayForm _overlay;

        // Toggle ẩn/hiện mật khẩu
        private bool _hienMatKhau = false;
        private bool _hienMatKhauMoi = false;   // dùng khi sửa (đổi mật khẩu)

        public TaiKhoanDTO KetQua { get; private set; }
        public bool DaXoa { get; private set; } = false;

        // ── Danh sách vai trò cố định ─────────────────────────────
        // Hiển thị tiếng Việt nhưng lưu xuống DB dạng mã
        private static readonly List<KeyValuePair<string, string>> _dsVaiTro = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Quản Lý", "admin"),
            new KeyValuePair<string, string>("Nhân Viên", "Nhân viên")
        };

        // ── Constructor: Thêm mới ─────────────────────────────────
        public TaiKhoanPopupUi()
        {
            InitializeComponent();
            _laSua = false;

            inputMaTK.Text = _bll.SinhMaMoi();
            inputMaTK.ReadOnly = true;

            NapComboVaiTro();
            NapComboNhanVien();

            // Khi thêm mới: ẩn khu vực Đổi Mật Khẩu
            panelDoiMatKhau.Visible = false;
        }

        // ── Constructor: Sửa ─────────────────────────────────────
        public TaiKhoanPopupUi(TaiKhoanDTO tk) : this()
        {
            _laSua = true;

            inputMaTK.Text = tk.MaTK;
            inputTenDangNhap.Text = tk.TenDangNhap;
            inputMatKhau.Text = tk.MatKhau;        // mật khẩu hiện tại (readonly khi sửa)
            inputMatKhau.ReadOnly = true;          // không cho sửa trực tiếp — dùng khu vực Đổi MK

            // Chọn đúng vai trò bằng SelectedValue
            cboVaiTros.SelectedValue = tk.VaiTro;
            if (cboVaiTros.SelectedIndex < 0 && cboVaiTros.Items.Count > 0)
                cboVaiTros.SelectedIndex = 1; // Mặc định Nhân viên nếu không tìm thấy

            // Chọn đúng nhân viên
            if (!string.IsNullOrWhiteSpace(tk.MaNV))
            {
                foreach (NhanVienDTO nv in cbNhanViens.Items)
                {
                    if (nv.MaNV == tk.MaNV)
                    {
                        cbNhanViens.SelectedItem = nv;
                        break;
                    }
                }
            }
            else
            {
                if (cbNhanViens.Items.Count > 0)
                    cbNhanViens.SelectedIndex = 0; // "(Không gán)"
            }

            // Khi sửa: hiện khu vực Đổi Mật Khẩu
            panelDoiMatKhau.Visible = true;
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
        //  Nạp dữ liệu ComboBox
        // ══════════════════════════════════════════════════════════
        private void NapComboVaiTro()
        {
            // Dùng tính năng DataSource Binding thay vì vòng lặp Add
            cboVaiTros.DataSource = new List<KeyValuePair<string, string>>(_dsVaiTro);
            cboVaiTros.DisplayMember = "Key";    // Hiện "Quản Lý", "Nhân Viên"
            cboVaiTros.ValueMember = "Value";    // Lưu "admin", "Nhân viên"

            if (cboVaiTros.Items.Count > 0)
                cboVaiTros.SelectedIndex = 1; // Mặc định chọn Nhân viên
        }

        private void NapComboNhanVien()
        {
            try
            {
                cbNhanViens.Items.Clear();
                cbNhanViens.DisplayMember = "HoTen";   // property hiển thị
                cbNhanViens.ValueMember = "MaNV";

                // Thêm mục "(Không gán)" đứng đầu
                cbNhanViens.Items.Add(new NhanVienDTO { MaNV = "", HoTen = "(Không gán)" });

                var dsNV = _bllNV.LayTatCaNhanVien();
                if (dsNV != null)
                {
                    foreach (var nv in dsNV)
                    {
                        cbNhanViens.Items.Add(nv);
                    }
                }

                if (cbNhanViens.Items.Count > 0)
                    cbNhanViens.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách nhân viên: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Vẽ tiêu đề header
        // ══════════════════════════════════════════════════════════
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            string tieuDe = _laSua ? "Sửa Tài Khoản" : "Thêm Tài Khoản";

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

        // ══════════════════════════════════════════════════════════
        //  Toggle ẩn/hiện mật khẩu
        // ══════════════════════════════════════════════════════════
        private void btnToggleMatKhau_Click(object sender, EventArgs e)
        {
            _hienMatKhau = !_hienMatKhau;
            inputMatKhau.PasswordChar = _hienMatKhau ? '\0' : '*';
            btnToggleMatKhau.Text = _hienMatKhau ? "🙈" : "👁";
        }

        private void btnToggleMatKhauMoi_Click(object sender, EventArgs e)
        {
            _hienMatKhauMoi = !_hienMatKhauMoi;
            inputMatKhauMoi.PasswordChar = _hienMatKhauMoi ? '\0' : '*';
            btnToggleMatKhauMoi.Text = _hienMatKhauMoi ? "🙈" : "👁";
        }

        // ══════════════════════════════════════════════════════════
        //  Xác Nhận
        // ══════════════════════════════════════════════════════════
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputTenDangNhap.Text))
                    throw new Exception("Vui lòng nhập Tên Đăng Nhập!");

                if (!_laSua && string.IsNullOrWhiteSpace(inputMatKhau.Text))
                    throw new Exception("Vui lòng nhập Mật Khẩu!");

                if (cboVaiTros.SelectedItem == null)
                    throw new Exception("Vui lòng chọn Vai Trò!");

                // Lấy MaNV từ Guna2ComboBox (có thể null nếu chọn "(Không gán)")
                string maNV = null;
                if (cbNhanViens.SelectedItem is NhanVienDTO selectedNV && !string.IsNullOrWhiteSpace(selectedNV.MaNV))
                {
                    maNV = selectedNV.MaNV;
                }

                var tk = new TaiKhoanDTO
                {
                    MaTK = inputMaTK.Text.Trim(),
                    TenDangNhap = inputTenDangNhap.Text.Trim(),
                    MatKhau = inputMatKhau.Text.Trim(),
                    // Lấy SelectedValue thay vì SelectedItem.ToString()
                    VaiTro = cboVaiTros.SelectedValue?.ToString(),
                    MaNV = maNV,
                };

                bool ok = _laSua ? _bll.CapNhatTaiKhoan(tk) : _bll.ThemTaiKhoan(tk);

                if (ok)
                {
                    KetQua = tk;
                    MessageBox.Show(
                        _laSua ? "Cập nhật tài khoản thành công!" : "Thêm tài khoản thành công!",
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

        // ══════════════════════════════════════════════════════════
        //  Đổi Mật Khẩu (chỉ dùng khi Sửa)
        // ══════════════════════════════════════════════════════════
        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {
            try
            {
                string matKhauMoi = inputMatKhauMoi.Text.Trim();

                if (string.IsNullOrWhiteSpace(matKhauMoi))
                    throw new Exception("Vui lòng nhập Mật Khẩu Mới!");

                if (matKhauMoi.Length < 6)
                    throw new Exception("Mật khẩu mới phải có ít nhất 6 ký tự!");

                bool ok = _bll.DoiMatKhau(inputMaTK.Text.Trim(), matKhauMoi);

                if (ok)
                {
                    inputMatKhau.Text = matKhauMoi;
                    inputMatKhauMoi.Text = "";
                    MessageBox.Show("Đổi mật khẩu thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Đổi mật khẩu không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Xóa
        // ══════════════════════════════════════════════════════════
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string tenDN = inputTenDangNhap.Text.Trim();

            using (var popup = new ConfirmDeleteUI(tenDN, "tài khoản"))
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    bool ok = _bll.XoaTaiKhoan(inputMaTK.Text.Trim());
                    if (ok)
                    {
                        DaXoa = true;
                        MessageBox.Show($"Đã xóa tài khoản \"{tenDN}\" thành công!",
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
        private void inputMaTK_Load(object sender, EventArgs e) { }
        private void inputTenDangNhap_Load(object sender, EventArgs e) { }
        private void inputMatKhau_Load(object sender, EventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }
        private void lblMaTK_Click(object sender, EventArgs e) { }
        private void lblTenDangNhap_Click(object sender, EventArgs e) { }
        private void lblMatKhau_Click(object sender, EventArgs e) { }
        private void lblVaiTro_Click(object sender, EventArgs e) { }
        private void lblNhanVien_Click(object sender, EventArgs e) { }
        private void cboVaiTros_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}