using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    // SAN PHAM POPUP UI
    // ═══════════════════════════════════════════════════════════════
    public partial class SanPhamPopupUi : Form
    {
        private readonly SanPhamBLL _bll = new SanPhamBLL();
        private readonly NhaCungCapBLL _bllNCC = new NhaCungCapBLL();
        private readonly bool _laSua;
        private OverlayForm _overlay;

        // Đường dẫn ảnh đã chọn (chưa copy)
        private string _duongDanAnhTam = "";

        public SanPhamDTO KetQua { get; private set; }
        public bool DaXoa { get; private set; } = false;

        // ── Danh sách loại cố định ────────────────────────────────
        // [SỬA Ở ĐÂY]: Thay thế dynamic bằng KeyValuePair chuẩn và an toàn
        private static readonly List<KeyValuePair<string, string>> _dsLoai = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Đồ uống", "DO_UONG"),
            new KeyValuePair<string, string>("Đồ ăn", "DO_AN"),
            new KeyValuePair<string, string>("Dụng cụ", "DUNG_CU")
        };

        // ── Constructor: Thêm mới ─────────────────────────────────
        public SanPhamPopupUi()
        {
            InitializeComponent();
            _laSua = false;

            inputMaSP.Text = _bll.SinhMaMoi();
            inputMaSP.ReadOnly = true;

            NapComboLoai();
            NapComboNCC();

            // Ẩn nút Xóa khi thêm mới
        }

        // ── Constructor: Sửa ─────────────────────────────────────
        public SanPhamPopupUi(SanPhamDTO sp) : this()
        {
            _laSua = true;

            inputMaSP.Text = sp.MaSP;
            inputTenSP.Text = sp.TenSP;
            inputGiaBan.Text = sp.GiaBan.ToString("0");
            inputSoLuong.Text = sp.SoLuongTon.ToString();

            // Chọn Loại
            // [SỬA Ở ĐÂY]: Dùng SelectedValue để gán lại giá trị khi Sửa
            cboLoai.SelectedValue = sp.Loai;
            if (cboLoai.SelectedIndex < 0 && cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;

            // Chọn NCC
            if (!string.IsNullOrWhiteSpace(sp.MaNCC))
            {
                foreach (NhaCungCapDTO ncc in cboNCC.Items)
                {
                    if (ncc.MaNCC == sp.MaNCC)
                    {
                        cboNCC.SelectedItem = ncc;
                        break;
                    }
                }
            }
            else
            {
                if (cboNCC.Items.Count > 0) cboNCC.SelectedIndex = 0;
            }

            // Hiển thị ảnh hiện tại
            if (!string.IsNullOrWhiteSpace(sp.HinhAnh))
            {
                lblTenAnh.Text = sp.HinhAnh;
                HienThiAnhTuTenFile(sp.HinhAnh);
            }

            // Hiện nút Xóa khi sửa
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
        private void NapComboLoai()
        {
            // [SỬA Ở ĐÂY]: Dùng tính năng DataSource Binding thay vì vòng lặp Add
            cboLoai.DataSource = new List<KeyValuePair<string, string>>(_dsLoai);
            cboLoai.DisplayMember = "Key";    // Hiện tiếng Việt
            cboLoai.ValueMember = "Value";    // Nhận giá trị không dấu

            if (cboLoai.Items.Count > 0)
                cboLoai.SelectedIndex = 0;
        }

        private void NapComboNCC()
        {
            try
            {
                cboNCC.Items.Clear();
                cboNCC.DisplayMember = "TenCongTy";
                cboNCC.ValueMember = "MaNCC";

                // Mục "(Không gán)" đứng đầu
                cboNCC.Items.Add(new NhaCungCapDTO { MaNCC = "", TenCongTy = "(Không gán)" });

                var dsNCC = _bllNCC.LayTatCaNhaCungCap();
                if (dsNCC != null)
                    foreach (var ncc in dsNCC)
                        cboNCC.Items.Add(ncc);

                if (cboNCC.Items.Count > 0)
                    cboNCC.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách nhà cung cấp: " + ex.Message,
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Vẽ tiêu đề header
        // ══════════════════════════════════════════════════════════
        private void panelHeader_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            string tieuDe = _laSua ? "Sửa Sản Phẩm" : "Thêm Sản Phẩm";

            using (Font font = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                SizeF size = e.Graphics.MeasureString(tieuDe, font);
                float x = (panelHeader.Width - size.Width) / 2f;
                float y = (panelHeader.Height - size.Height) / 2f;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(tieuDe, font, brush, x, y);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Upload ảnh
        // ══════════════════════════════════════════════════════════
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = "Chọn ảnh sản phẩm";
                dlg.Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.bmp;*.gif)|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                if (dlg.ShowDialog() != DialogResult.OK) return;

                _duongDanAnhTam = dlg.FileName;
                lblTenAnh.Text = Path.GetFileName(dlg.FileName);

                // Preview ảnh
                try
                {
                    picPreview.Image?.Dispose();
                    picPreview.Image = Image.FromFile(dlg.FileName);
                }
                catch
                {
                    picPreview.Image = null;
                    lblTenAnh.Text = "⚠ Không đọc được ảnh";
                }
            }
        }

        // ── Hiển thị ảnh từ tên file (khi sửa) ──────────────────
        private void HienThiAnhTuTenFile(string tenFile)
        {
            try
            {
                // Thư mục Images nằm cùng cấp với thư mục chạy exe
                string folder = Path.Combine(Application.StartupPath, "Images");
                string fullPath = Path.Combine(folder, tenFile);

                if (File.Exists(fullPath))
                {
                    picPreview.Image?.Dispose();
                    picPreview.Image = Image.FromFile(fullPath);
                }
            }
            catch { /* bỏ qua nếu không load được */ }
        }

        // ── Copy ảnh vào thư mục /Images ─────────────────────────
        private string LuuAnh(string maSP)
        {
            if (string.IsNullOrWhiteSpace(_duongDanAnhTam))
            {
                // Giữ nguyên tên file cũ khi sửa mà không chọn ảnh mới
                return lblTenAnh.Text.Trim() == "(chưa có ảnh)" ? "" : lblTenAnh.Text.Trim();
            }

            string folder = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string ext = Path.GetExtension(_duongDanAnhTam);
            string tenFile = $"{maSP}_{DateTime.Now:yyyyMMddHHmmss}{ext}";
            string dich = Path.Combine(folder, tenFile);

            File.Copy(_duongDanAnhTam, dich, overwrite: true);
            return tenFile;
        }

        // ══════════════════════════════════════════════════════════
        //  Xác Nhận
        // ══════════════════════════════════════════════════════════
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputTenSP.Text))
                    throw new Exception("Vui lòng nhập Tên Sản Phẩm!");

                if (cboLoai.SelectedItem == null)
                    throw new Exception("Vui lòng chọn Loại Sản Phẩm!");

                if (!double.TryParse(inputGiaBan.Text.Trim(), out double giaBan) || giaBan < 0)
                    throw new Exception("Giá bán không hợp lệ (phải là số >= 0)!");

                if (!int.TryParse(inputSoLuong.Text.Trim(), out int soLuong) || soLuong < 0)
                    throw new Exception("Số lượng tồn kho không hợp lệ (phải là số nguyên >= 0)!");

                // Lấy MaNCC
                string maNCC = null;
                if (cboNCC.SelectedItem is NhaCungCapDTO selectedNCC && !string.IsNullOrWhiteSpace(selectedNCC.MaNCC))
                    maNCC = selectedNCC.MaNCC;

                string maSP = inputMaSP.Text.Trim();

                // Lưu ảnh → lấy tên file
                string tenFileAnh = LuuAnh(maSP);

                var sp = new SanPhamDTO
                {
                    MaSP = maSP,
                    TenSP = inputTenSP.Text.Trim(),
                    // [SỬA Ở ĐÂY]: Lấy SelectedValue thay vì SelectedItem.ToString()
                    Loai = cboLoai.SelectedValue?.ToString(),
                    GiaBan = giaBan,
                    SoLuongTon = soLuong,
                    MaNCC = maNCC,
                    HinhAnh = tenFileAnh,
                };

                bool ok = _laSua ? _bll.CapNhatSanPham(sp) : _bll.ThemSanPham(sp);

                if (ok)
                {
                    KetQua = sp;
                    MessageBox.Show(
                        _laSua ? "Cập nhật sản phẩm thành công!" : "Thêm sản phẩm thành công!",
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
        //  Xóa
        // ══════════════════════════════════════════════════════════
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string tenSP = inputTenSP.Text.Trim();

            using (var popup = new ConfirmDeleteUI(tenSP, "sản phẩm"))
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    bool ok = _bll.XoaSanPham(inputMaSP.Text.Trim());
                    if (ok)
                    {
                        DaXoa = true;
                        MessageBox.Show($"Đã xóa sản phẩm \"{tenSP}\" thành công!",
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

        // ── Event stubs ───────────────────────────────────────────
        private void inputMaSP_Load(object sender, EventArgs e) { }
        private void inputTenSP_Load(object sender, EventArgs e) { }
        private void inputGiaBan_Load(object sender, EventArgs e) { }
        private void inputSoLuong_Load(object sender, EventArgs e) { }
        private void guna2Panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void guna2Panel4_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, System.Windows.Forms.PaintEventArgs e) { }
        private void cboLoai_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboNCC_SelectedIndexChanged(object sender, EventArgs e) { }
        private void lblMaSP_Click(object sender, EventArgs e) { }
        private void lblTenSP_Click(object sender, EventArgs e) { }
        private void lblGiaBan_Click(object sender, EventArgs e) { }
        private void lblSoLuong_Click(object sender, EventArgs e) { }
        private void lblLoai_Click(object sender, EventArgs e) { }
        private void lblNCC_Click(object sender, EventArgs e) { }
    }
}