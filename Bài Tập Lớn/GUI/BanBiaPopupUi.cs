using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Guna.UI2.WinForms;
using System;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class BanBiaPopupUi : Form
    {
        private readonly BanBidaBLL _bll = new BanBidaBLL();
        private readonly bool _laSua;

        public BanBidaDTO KetQua { get; private set; }

        // ── Constructor: Thêm mới ──────────────────────────────────
        public BanBiaPopupUi()
        {
            InitializeComponent();
            _laSua = false;
            KhởiTaoCombo();
            inputMaNv.Text = _bll.SinhMaMoi();
            inputMaNv.ReadOnly = true;
        }

        // ── Constructor: Sửa ──────────────────────────────────────
        public BanBiaPopupUi(BanBidaDTO ban) : this()
        {
            _laSua = true;
            inputMaNv.Text = ban.MaBan;
            inputHoTen.Text = ban.TenBan;
            inputSdt.Text = ban.GiaTheoGio.ToString("N0");

            // SelectedItem so sánh theo giá trị thật (THUONG, VIP,...)
            selectChucVu.SelectedItem = ban.LoaiBan;
            selectGioiTinh.SelectedItem = ban.TrangThai;
        }

        // ── Khởi tạo ComboBox ────────────────────────────────────
        private void KhởiTaoCombo()
        {
            // selectChucVu → Loại Bàn (khớp đúng giá trị DB)
            selectChucVu.Items.Clear();
            selectChucVu.Items.AddRange(new object[] { "THUONG", "VIP", "SNOOKER" });
            selectChucVu.SelectedIndex = 0;

            // selectGioiTinh → Trạng Thái (khớp đúng giá trị DB)
            selectGioiTinh.Items.Clear();
            selectGioiTinh.Items.AddRange(new object[] { "TRONG", "DANG_CHOI", "BAO_TRI" });
            selectGioiTinh.SelectedIndex = 0;

            // Ẩn controls không dùng
            btnIncluded.Visible = false;

            // Đổi label cho đúng ngữ cảnh
            guna2HtmlLabel2.Text = "Tên Bàn";
            guna2HtmlLabel8.Text = "Giá Theo Giờ (VNĐ)";
            guna2HtmlLabel7.Text = "Trạng Thái";
            guna2HtmlLabel4.Text = "Loại Bàn";
        }

        // ── Xác Nhận ─────────────────────────────────────────────
        private void btnXacNhan_Click_1(object sender, EventArgs e)
        {
            try
            {
                string giaText = inputSdt.Text.Trim().Replace(",", "").Replace(".", "");
                if (!double.TryParse(giaText, out double gia) || gia <= 0)
                    throw new Exception("Giá theo giờ không hợp lệ! Vui lòng nhập số lớn hơn 0.");

                var ban = new BanBidaDTO
                {
                    MaBan = inputMaNv.Text.Trim(),
                    TenBan = inputHoTen.Text.Trim(),
                    LoaiBan = selectChucVu.SelectedItem?.ToString() ?? "",
                    GiaTheoGio = gia,
                    TrangThai = selectGioiTinh.SelectedItem?.ToString() ?? ""
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

        // ── Hủy ──────────────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}