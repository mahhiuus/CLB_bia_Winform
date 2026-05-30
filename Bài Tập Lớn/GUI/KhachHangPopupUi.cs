using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // ═══════════════════════════════════════════════════════════════
    //  KHACH HANG POPUP UI
    // ═══════════════════════════════════════════════════════════════
    public partial class KhachHangPopupUi : Form
    {
        // ── Fields ────────────────────────────────────────────────
        private readonly KhachHangBLL _bll = new KhachHangBLL();
        private readonly bool _laSua;
        private OverlayForm _overlay;
        private static readonly HttpClient _http = new HttpClient();

        public KhachHangDTO KetQua { get; private set; }

        // ── Helper: JSON model ────────────────────────────────────
        private class DvhcItem
        {
            [JsonProperty("code")] public int Code { get; set; }
            [JsonProperty("name")] public string Name { get; set; }

            [JsonProperty("districts")] public List<DvhcItem> Districts { get; set; }
            [JsonProperty("wards")] public List<DvhcItem> Wards { get; set; }

            public override string ToString() => Name ?? string.Empty;
        }

        // ══════════════════════════════════════════════════════════
        //  Constructor: Thêm mới
        // ══════════════════════════════════════════════════════════
        public KhachHangPopupUi()
        {
            InitializeComponent();
            _laSua = false;

            inputMaKH.Text = _bll.SinhMaMoi();
            inputMaKH.ReadOnly = true;
            dtpNgayDangKy.Value = DateTime.Now;
            inputDiemTichLuy.Text = "0";


            // Khoá Huyện & Phường cho đến khi user chọn cấp trên
            cboHuyen.Enabled = false;
            cboPhuong.Enabled = false;

            LoadTinh();
        }

        // ══════════════════════════════════════════════════════════
        //  Constructor: Sửa
        // ══════════════════════════════════════════════════════════
        public KhachHangPopupUi(KhachHangDTO kh) : this()
        {
            _laSua = true;

            inputMaKH.Text = kh.MaKH;
            inputHoTen.Text = kh.HoTen;
            inputSdt.Text = kh.Sdt;
            inputDiemTichLuy.Text = kh.DiemTichLuy.ToString();
            dtpNgayDangKy.Value = kh.NgayDangKy;


            // Khi form Sửa, địa chỉ cũ (nếu có) hiển thị dạng placeholder
            // trên cboTinh để người dùng biết giá trị cũ
            if (!string.IsNullOrWhiteSpace(kh.DiaChi))
                cboTinh.Text = $"Cũ: {kh.DiaChi}";
        }

        // ══════════════════════════════════════════════════════════
        //  Overlay
        // ══════════════════════════════════════════════════════════
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
        //  Vẽ tiêu đề header
        // ══════════════════════════════════════════════════════════
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            string tieuDe = _laSua ? "Sửa Khách Hàng" : "Thêm Khách Hàng";

            using (Font font = new Font("Segoe UI", 15f, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                SizeF sz = e.Graphics.MeasureString(tieuDe, font);
                float x = (guna2Panel1.Width - sz.Width) / 2f;
                float y = (guna2Panel1.Height - sz.Height) / 2f;
                e.Graphics.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(tieuDe, font, brush, x, y);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  API — Load Tỉnh/Thành
        // ══════════════════════════════════════════════════════════
        private async void LoadTinh()
        {
            try
            {
                cboTinh.Enabled = false;
                string json = await _http.GetStringAsync(
                    "https://provinces.open-api.vn/api/?depth=1");
                var list = JsonConvert.DeserializeObject<List<DvhcItem>>(json);

                cboTinh.Items.Clear();
                foreach (var t in list)
                    cboTinh.Items.Add(t);

                cboTinh.Enabled = true;
            }
            catch
            {
                MessageBox.Show(
                    "Không tải được danh sách Tỉnh/Thành.\nKiểm tra kết nối internet.",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboTinh.Enabled = true;
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Chọn Tỉnh → Load Quận/Huyện
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
                string json = await _http.GetStringAsync(
                    $"https://provinces.open-api.vn/api/p/{tinh.Code}?depth=2");
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
        //  Chọn Quận/Huyện → Load Phường/Xã
        // ══════════════════════════════════════════════════════════
        private async void cboHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboPhuong.Items.Clear();
            cboPhuong.Enabled = false;

            if (!(cboHuyen.SelectedItem is DvhcItem huyen)) return;

            try
            {
                string json = await _http.GetStringAsync(
                    $"https://provinces.open-api.vn/api/d/{huyen.Code}?depth=2");
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
        //  Lấy chuỗi địa chỉ ghép từ 3 dropdown
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

        // ══════════════════════════════════════════════════════════
        //  Xác Nhận (Thêm / Cập nhật)
        // ══════════════════════════════════════════════════════════
        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputHoTen.Text))
                    throw new Exception("Vui lòng nhập Họ Tên khách hàng!");

                if (!int.TryParse(inputDiemTichLuy.Text.Trim(), out int diem) || diem < 0)
                    throw new Exception("Điểm tích lũy phải là số nguyên không âm!");

                string diaChi = LayDiaChi();
                if (string.IsNullOrWhiteSpace(diaChi))
                    throw new Exception("Vui lòng chọn đầy đủ Tỉnh/Thành, Quận/Huyện, Phường/Xã!");

                var kh = new KhachHangDTO
                {
                    MaKH = inputMaKH.Text.Trim(),
                    HoTen = inputHoTen.Text.Trim(),
                    Sdt = inputSdt.Text.Trim(),
                    DiaChi = diaChi,
                    DiemTichLuy = diem,
                    NgayDangKy = dtpNgayDangKy.Value.Date,
                };

                bool ok = _laSua ? _bll.CapNhatKhachHang(kh) : _bll.ThemKhachHang(kh);

                if (ok)
                {
                    KetQua = kh;
                    MessageBox.Show(
                        _laSua ? "Cập nhật khách hàng thành công!"
                               : "Thêm khách hàng thành công!",
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
        //  Xóa (chỉ hiện khi Sửa)
        // ══════════════════════════════════════════════════════════
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string hoTen = inputHoTen.Text.Trim();
            bool confirmed = false;

            using (var dlg = new ConfirmDeleteUI(hoTen, "khách hàng"))
                confirmed = dlg.ShowDialog(this) == DialogResult.OK;

            if (!confirmed) return;

            try
            {
                bool ok = _bll.XoaKhachHang(inputMaKH.Text.Trim());
                if (ok)
                {
                    MessageBox.Show(
                        $"Đã xóa khách hàng \"{hoTen}\" thành công!",
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Hủy ──────────────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ── Event stubs ───────────────────────────────────────────
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel7_Paint(object sender, PaintEventArgs e) { }
        private void lblMaKH_Click(object sender, EventArgs e) { }
        private void lblHoTen_Click(object sender, EventArgs e) { }
        private void lblSdt_Click(object sender, EventArgs e) { }
        private void lblDiaChi_Click(object sender, EventArgs e) { }
        private void lblDiemTichLuy_Click(object sender, EventArgs e) { }
        private void lblNgayDangKy_Click(object sender, EventArgs e) { }

        private void cboPhuong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}