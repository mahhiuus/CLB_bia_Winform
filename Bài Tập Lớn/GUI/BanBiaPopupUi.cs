using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    internal class OverlayForm : Form
    {
        private System.Windows.Forms.Timer _fadeTimer;

        public OverlayForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.White;
            this.Opacity = 0;
            this.StartPosition = FormStartPosition.Manual;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            this.ShowInTaskbar = false;
            this.TopMost = false;

            _fadeTimer = new System.Windows.Forms.Timer { Interval = 15 };
            _fadeTimer.Tick += (s, e) =>
            {
                if (this.Opacity >= 0.55) { this.Opacity = 0.55; _fadeTimer.Stop(); }
                else this.Opacity += 0.05;
            };
        }

        public void StartFade() => _fadeTimer.Start();

        protected override void Dispose(bool disposing)
        {
            if (disposing) _fadeTimer?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "OverlayForm";
            this.Load += new System.EventHandler(this.OverlayForm_Load);
            this.ResumeLayout(false);

        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {

        }
    }
    public partial class BanBiaPopupUi : Form
    {
        private readonly BanBidaBLL _bll = new BanBidaBLL();
        private readonly bool _laSua;
        private OverlayForm _overlay;

        public BanBidaDTO KetQua { get; private set; }
        public bool DaXoa { get; private set; } = false;
        public BanBiaPopupUi()
        {
            InitializeComponent();
            _laSua = false;
            KhởiTaoCombo();
            inputMaNv.Text = _bll.SinhMaMoi();
            inputMaNv.ReadOnly = true;
        }
        public BanBiaPopupUi(BanBidaDTO ban) : this()
        {
            _laSua = true;
            inputMaNv.Text = ban.MaBan;
            inputHoTen.Text = ban.TenBan;
            txtGiaTheoGio.Text = ban.GiaTheoGio.ToString("N0");

            selectBan.SelectedValue = ban.LoaiBan;
            selectTrangThai.SelectedValue = ban.TrangThai;
        }
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
        private void KhởiTaoCombo()
        {
            selectBan.DataSource = new[]
            {
                new { Text = "Bàn Thường",  Value = "THUONG"  },
                new { Text = "Bàn VIP",     Value = "VIP"     },
                new { Text = "Bàn Snooker", Value = "SNOOKER" },
            };
            selectBan.DisplayMember = "Text";
            selectBan.ValueMember = "Value";
            selectBan.SelectedIndex = 0;

            selectTrangThai.DataSource = new[]
            {
                new { Text = "Trống",     Value = "TRONG"     },
                new { Text = "Đang Chơi", Value = "DANG_CHOI" },
                new { Text = "Bảo Trì",   Value = "BAO_TRI"   },
            };
            selectTrangThai.DisplayMember = "Text";
            selectTrangThai.ValueMember = "Value";
            selectTrangThai.SelectedIndex = 0;

            btnIncluded.Visible = false;

            guna2HtmlLabel2.Text = "Tên Bàn";
            guna2HtmlLabel8.Text = "Loại Bàn";
            guna2HtmlLabel7.Text = "Giá Theo Giờ (VNĐ)";
            guna2HtmlLabel4.Text = "Trạng Thái";
        }
        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {
            string tieuDe = _laSua ? "Sửa Dữ Liệu" : "Thêm Bàn Bida";

            using (Font font = new Font("Segoe UI", 16f, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.White))
            {
                SizeF size = e.Graphics.MeasureString(tieuDe, font);
                float x = (guna2Panel1.Width - size.Width) / 2f;
                float y = (guna2Panel1.Height - size.Height) / 2f;
                e.Graphics.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(tieuDe, font, brush, x, y);
            }
        }
        private void btnXacNhan_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputHoTen.Text))
                    throw new Exception("Vui lòng nhập Tên Bàn!");

                string giaText = txtGiaTheoGio.Text.Trim().Replace(",", "").Replace(".", "");
                if (!double.TryParse(giaText, out double gia) || gia <= 0)
                    throw new Exception("Giá theo giờ không hợp lệ! Vui lòng nhập số lớn hơn 0.");

                var ban = new BanBidaDTO
                {
                    MaBan = inputMaNv.Text.Trim(),
                    TenBan = inputHoTen.Text.Trim(),
                    LoaiBan = selectBan.SelectedValue?.ToString() ?? "",
                    GiaTheoGio = gia,
                    TrangThai = selectTrangThai.SelectedValue?.ToString() ?? ""
                };

                bool ok = _laSua ? _bll.CapNhatBan(ban) : _bll.ThemBan(ban);

                if (ok)
                {
                    KetQua = ban;
                    MessageBox.Show(
                        _laSua ? "Cập nhật bàn thành công!" : "Thêm bàn mới thành công!",
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
        private void btnXoa_Click(object sender, EventArgs e)
        {
            string tenBan = inputHoTen.Text.Trim();

            using (var popup = new ConfirmDeleteUI(tenBan, "bàn bida"))
            {
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    bool ok = _bll.XoaBan(inputMaNv.Text.Trim());
                    if (ok)
                    {
                        DaXoa = true;
                        MessageBox.Show($"Đã xóa \"{tenBan}\" thành công!",
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
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void inputHoTen_Load(object sender, EventArgs e) { }
        private void inputMaNv_Load(object sender, EventArgs e) { }
        private void txtGiaTheoGio_Load(object sender, EventArgs e) { }
        private void selectTrangThai_SelectedIndexChanged(object sender, EventArgs e) { }
        private void selectBan_SelectedIndexChanged(object sender, EventArgs e) { }
        private void btnIncluded_Click(object sender, EventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel4_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel9_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel7_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel8_Click(object sender, EventArgs e) { }
    }
}