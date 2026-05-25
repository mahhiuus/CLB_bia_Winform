using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    // Tắt cảnh báo bắt buộc viết hoa chữ cái đầu
#pragma warning disable IDE1006 

    public partial class NhanVienPopupUI : Form
    {
        private NhanVienBLL _bll;
        private NhanVienDTO _nv;
        private bool _isEdit;

        // ==================== TẠO BÓNG (SHADOW THUẦN WINFORMS) ====================
        // Không dùng API ngoài (dwmapi.dll) nên an toàn tuyệt đối, không gây văng app
        private const int CS_DROPSHADOW = 0x00020000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        // ==================== FADE + SCALE ====================
        private Timer _fadeTimer;
        private double _fadeStep = 0;
        private const double _totalSteps = 8;

        public NhanVienPopupUI()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Opacity = 0;
            this.Scale(new SizeF(0.92f, 0.92f));

            _fadeTimer = new Timer { Interval = 10 };
            _fadeTimer.Tick += FadeScaleTick;

            this.Shown += (s, e) => _fadeTimer.Start();
        }

        private void FadeScaleTick(object sender, EventArgs e)
        {
            _fadeStep++;
            double t = _fadeStep / _totalSteps;
            double ease = 1 - (1 - t) * (1 - t);
            this.Opacity = ease;

            if (_fadeStep >= _totalSteps)
            {
                this.Opacity = 1;
                _fadeTimer.Stop();
            }
        }

        public NhanVienPopupUI(NhanVienDTO nv, NhanVienBLL bll) : this()
        {
            _bll = bll;
            _nv = nv;
            _isEdit = (nv != null);
            this.Load += NhanVienPopupUI_FormLoad;
        }

        // ==================== FORM LOAD ====================
        private void NhanVienPopupUI_FormLoad(object sender, EventArgs e)
        {
            if (selectChucVu != null)
            {
                selectChucVu.Items.Clear();
                selectChucVu.Items.AddRange(new string[]
                    { "Quản lý", "Nhân viên bán hàng", "Kế toán", "Kho vận", "Bảo vệ" });
            }

            if (selectGioiTinh != null)
            {
                selectGioiTinh.Items.Clear();
                selectGioiTinh.Items.AddRange(new string[] { "Nam", "Nữ", "Khác" });
            }

            if (btnHuy != null) btnHuy.Visible = true;

            if (_isEdit)
            {
                btnIncluded.Text = "Cập nhật";
                inputMaNv.Text = _nv.MaNV;
                inputMaNv.Enabled = false;
                inputHoTen.Text = _nv.HoTen;
                inputSdt.Text = _nv.Sdt;

                // Xử lý Guna2DateTimePicker an toàn
                try
                {
                    if (_nv.NgaySinh.HasValue && chonNgaySinh is Guna.UI2.WinForms.Guna2DateTimePicker dtp)
                    {
                        dtp.Value = _nv.NgaySinh.Value;
                    }
                    else
                    {
                        chonNgaySinh.Text = _nv.NgaySinh.HasValue ? _nv.NgaySinh.Value.ToString("dd/MM/yyyy") : "";
                    }
                }
                catch { }

                if (selectGioiTinh != null)
                {
                    int idxGT = selectGioiTinh.Items.IndexOf(_nv.GioiTinh);
                    selectGioiTinh.SelectedIndex = idxGT >= 0 ? idxGT : -1;
                }
                selectGioiTinh.Text = _nv.GioiTinh;

                if (selectChucVu != null)
                {
                    int idxCV = selectChucVu.Items.IndexOf(_nv.ChucVu);
                    selectChucVu.SelectedIndex = idxCV >= 0 ? idxCV : -1;
                }
                selectChucVu.Text = _nv.ChucVu;
            }
            else
            {
                btnIncluded.Text = "Thêm mới";
                try { inputMaNv.Text = _bll.SinhMaMoi(); inputMaNv.Enabled = false; }
                catch { inputMaNv.Text = "NV01"; }
            }
        }

        // ==================== LƯU / CẬP NHẬT ====================
        private void btnThem_Click(object sender, EventArgs e)
        {
            string maNV = inputMaNv.Text.Trim();
            string hoTen = inputHoTen.Text.Trim();
            string sdt = inputSdt.Text.Trim();
            string gioiTinh = selectGioiTinh.Text.Trim();
            string chucVu = selectChucVu.Text.Trim();

            if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Mã nhân viên và Họ tên không được để trống!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(gioiTinh))
            {
                MessageBox.Show("Vui lòng chọn giới tính!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(chucVu))
            {
                MessageBox.Show("Vui lòng chọn chức vụ!",
                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DateTime ngaySinh = DateTime.Now;
            try
            {
                // Sử dụng đúng kiểu Guna2DateTimePicker
                if (chonNgaySinh is Guna.UI2.WinForms.Guna2DateTimePicker dtp)
                {
                    ngaySinh = dtp.Value.Date;
                }
                else
                {
                    DateTime.TryParseExact(chonNgaySinh.Text.Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out ngaySinh);
                }
            }
            catch { }

            var nvMoi = new NhanVienDTO(maNV, hoTen, sdt, gioiTinh, chucVu, ngaySinh);

            try
            {
                if (_isEdit)
                    _bll.CapNhatNhanVien(nvMoi);
                else
                    _bll.ThemNhanVien(nvMoi);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Alias cho nút Included
        private void btnIncluded_Click(object sender, EventArgs e) => btnThem_Click(sender, e);

        // ==================== HỦY (Chỉ đóng Popup) ====================
        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // ==================== EVENT CHỌN NGÀY / COMBOBOX ====================
        private void chonNgaySinh_ValueChanged(object sender, EventArgs e)
        {
            // Sử dụng đúng kiểu Guna2DateTimePicker
            if (sender is Guna.UI2.WinForms.Guna2DateTimePicker dtp && !(chonNgaySinh is Guna.UI2.WinForms.Guna2DateTimePicker))
            {
                chonNgaySinh.Text = dtp.Value.ToString("dd/MM/yyyy");
            }
        }

        private void selectChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectChucVu?.SelectedItem != null)
                selectChucVu.Text = selectChucVu.SelectedItem.ToString();
        }

        private void selectGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectGioiTinh?.SelectedItem != null)
                selectGioiTinh.Text = selectGioiTinh.SelectedItem.ToString();
        }

        // ==================== DỌN DẸP BỘ NHỚ KHI ĐÓNG ====================
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_fadeTimer != null)
            {
                _fadeTimer.Stop();
                _fadeTimer.Dispose();
            }
            base.OnFormClosed(e);
        }

        // ==================== EVENT TRỐNG ====================
        private void inputMaNv_Load(object sender, EventArgs e) { }
        private void inputHoTen_Load(object sender, EventArgs e) { }
        private void inputSdt_Load(object sender, EventArgs e) { }
        private void selectGioiTinh_Load(object sender, EventArgs e) { }
        private void selectChucVu_Load(object sender, EventArgs e) { }
        private void chonNgaySinh_Load(object sender, EventArgs e) { }
        private void NhanVienPopupUI_Load(object sender, EventArgs e) { }
    }
}