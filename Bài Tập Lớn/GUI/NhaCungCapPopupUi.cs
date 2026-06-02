using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
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
        private static readonly HttpClient _http = new HttpClient();
        private static readonly string CACHE_FILE = "provinces_cache.json";

        public NhaCungCapDTO KetQua { get; private set; }
        public bool DaXoa { get; private set; } = false;

        // ── Helper: JSON model ────────────────────────────────────
        private class DvhcItem
        {
            [JsonProperty("code")] public int Code { get; set; }
            [JsonProperty("name")] public string Name { get; set; }

            [JsonProperty("districts")] public List<DvhcItem> Districts { get; set; }
            [JsonProperty("wards")] public List<DvhcItem> Wards { get; set; }

            public override string ToString() => Name ?? string.Empty;
        }

        // ── Constructor: Thêm mới ─────────────────────────────────
        public NhaCungCapPopupUi()
        {
            InitializeComponent();
            _laSua = false;
            inputMaNCC.Text = _bll.SinhMaMoi();
            inputMaNCC.ReadOnly = true;

            // Khoá Huyện & Phường cho đến khi user chọn cấp trên[cite: 4]
            cboHuyen.Enabled = false;
            cboPhuong.Enabled = false;

            LoadTinh();
        }

        // ── Constructor: Sửa ─────────────────────────────────────
        public NhaCungCapPopupUi(NhaCungCapDTO ncc) : this()
        {
            _laSua = true;
            inputMaNCC.Text = ncc.MaNCC;
            inputTenCongTy.Text = ncc.TenCongTy;
            inputSdt.Text = ncc.Sdt;
            inputEmail.Text = ncc.Email;
            inputNguoiLH.Text = ncc.NguoiLienHe;

            // Khi form Sửa, địa chỉ cũ hiển thị dạng placeholder[cite: 4]
            if (!string.IsNullOrWhiteSpace(ncc.DiaChi))
                cboTinh.Text = $"Cũ: {ncc.DiaChi}";
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

        // ══════════════════════════════════════════════════════════
        //  API — Load Tỉnh/Thành (Hỗ trợ Cache Offline)[cite: 4]
        // ══════════════════════════════════════════════════════════
        private async void LoadTinh()
        {
            try
            {
                cboTinh.Enabled = false;
                string json = string.Empty;

                try
                {
                    json = await _http.GetStringAsync("https://provinces.open-api.vn/api/?depth=1");
                    File.WriteAllText(CACHE_FILE, json); // Ghi đè vào bộ nhớ đệm khi có mạng
                }
                catch
                {
                    // Nếu mất mạng, kiểm tra xem có tệp cache cũ không
                    if (File.Exists(CACHE_FILE))
                    {
                        json = File.ReadAllText(CACHE_FILE);
                    }
                    else
                    {
                        throw new Exception("Không có kết nối mạng và không tìm thấy dữ liệu đệm.");
                    }
                }

                var list = JsonConvert.DeserializeObject<List<DvhcItem>>(json);

                cboTinh.Items.Clear();
                foreach (var t in list)
                    cboTinh.Items.Add(t);

                cboTinh.Enabled = true;
            }
            catch
            {
                MessageBox.Show(
                    "Không tải được danh sách Tỉnh/Thành.\nVui lòng kiểm tra lại kết nối internet.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTinh.Enabled = true;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Chọn Tỉnh → Load Quận/Huyện[cite: 4]
        // ══════════════════════════════════════════════════════════
        private async void cboTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboHuyen.Items.Clear();
            cboPhuong.Items.Clear();
            cboHuyen.Enabled = false;
            cboPhuong.Enabled = false;

            if (!(cboTinh.SelectedItem is DvhcItem tinh)) return;

            try
            {
                string json = await _http.GetStringAsync($"https://provinces.open-api.vn/api/p/{tinh.Code}?depth=2");
                var data = JsonConvert.DeserializeObject<DvhcItem>(json);

                foreach (var h in data.Districts)
                    cboHuyen.Items.Add(h);

                cboHuyen.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Không tải được danh sách Quận/Huyện.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Chọn Quận/Huyện → Load Phường/Xã[cite: 4]
        // ══════════════════════════════════════════════════════════
        private async void cboHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboPhuong.Items.Clear();
            cboPhuong.Enabled = false;

            if (!(cboHuyen.SelectedItem is DvhcItem huyen)) return;

            try
            {
                string json = await _http.GetStringAsync($"https://provinces.open-api.vn/api/d/{huyen.Code}?depth=2");
                var data = JsonConvert.DeserializeObject<DvhcItem>(json);

                foreach (var p in data.Wards)
                    cboPhuong.Items.Add(p);

                cboPhuong.Enabled = true;
            }
            catch
            {
                MessageBox.Show("Không tải được danh sách Phường/Xã.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Lấy chuỗi địa chỉ ghép từ 3 dropdown[cite: 4]
        // ══════════════════════════════════════════════════════════
        private string LayDiaChi()
        {
            var parts = new[]
            {
                (cboPhuong.SelectedItem as DvhcItem)?.Name,
                (cboHuyen.SelectedItem  as DvhcItem)?.Name,
                (cboTinh.SelectedItem   as DvhcItem)?.Name,
            };
            return string.Join(", ", parts.Where(s => !string.IsNullOrEmpty(s)));
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

                // Đồng bộ bắt buộc kiểm tra địa chỉ đầy đủ giống Khách Hàng[cite: 4]
                string diaChi = LayDiaChi();
                if (string.IsNullOrWhiteSpace(diaChi))
                    throw new Exception("Vui lòng chọn đầy đủ Tỉnh/Thành, Quận/Huyện, Phường/Xã!");

                var ncc = new NhaCungCapDTO
                {
                    MaNCC = inputMaNCC.Text.Trim(),
                    TenCongTy = inputTenCongTy.Text.Trim(),
                    Sdt = inputSdt.Text.Trim(),
                    Email = inputEmail.Text.Trim(),
                    DiaChi = diaChi,
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
        private void cboPhuong_SelectedIndexChanged(object sender, EventArgs e) { }
    }
}