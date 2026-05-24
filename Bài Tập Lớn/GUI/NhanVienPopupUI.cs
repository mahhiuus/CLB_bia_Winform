using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.UI
{
    public partial class NhanVienPopupUI : Form
    {
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color GREEN_LIGHT = ColorTranslator.FromHtml("#79ae6f");
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        static readonly Color BORDER_IDLE = Color.FromArgb(210, 220, 210);
        static readonly Color DANGER = Color.FromArgb(192, 57, 43);

        private readonly NhanVienBLL _bll;
        private readonly NhanVienDTO _editNV;
        private readonly bool _isEdit;

        private RoundedTextBox txtMaNV, txtHoTen, txtSdt, txtChucVu;
        private ComboBox cboGioiTinh;
        private DateTimePicker dtpNgaySinh;
        private RoundedButton btnLuu, btnHuy;
        private Label lblError;

        public NhanVienPopupUI(NhanVienDTO nv, NhanVienBLL bll)
        {
            _bll = bll;
            _editNV = nv;
            _isEdit = (nv != null);
            BuildUI();
            if (_isEdit) FillForm(nv); else PreFillMa();
        }

        private void BuildUI()
        {
            this.Size = new Size(500, 560);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = CREAM;
            this.Font = new Font("Segoe UI", 9.5f);

            // Bo tròn toàn bộ Form
            using (var path = GraphicsHelper.GetRoundedPath(new Rectangle(0, 0, Width, Height), 14))
                this.Region = new Region(path);

            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 62, BackColor = GREEN_DARK };
            pnlHeader.Controls.Add(new Label { Text = _isEdit ? "✏️" : "➕", Font = new Font("Segoe UI", 20f), ForeColor = Color.White, AutoSize = true, Location = new Point(20, 12) });
            pnlHeader.Controls.Add(new Label { Text = _isEdit ? "Sửa Nhân Viên" : "Thêm Nhân Viên", Font = new Font("Segoe UI Semibold", 13f), ForeColor = Color.White, AutoSize = true, Location = new Point(58, 20) });

            Button btnX = new Button { Text = "✕", Size = new Size(34, 34), Location = new Point(452, 14), FlatStyle = FlatStyle.Flat, ForeColor = Color.White, BackColor = GREEN_DARK, Font = new Font("Segoe UI", 10f) };
            btnX.FlatAppearance.BorderSize = 0;
            btnX.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            pnlHeader.Controls.Add(btnX);

            pnlHeader.MouseDown += DoDrag;

            Panel scroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(24, 16, 24, 0) };
            int y = 0;

            txtMaNV = new RoundedTextBox { ReadOnly = _isEdit, BackColor = _isEdit ? Color.FromArgb(238, 243, 238) : Color.White };
            AddCard(scroll, "🪪", "Mã nhân viên *", txtMaNV, ref y);

            txtHoTen = new RoundedTextBox();
            AddCard(scroll, "👤", "Họ và tên *", txtHoTen, ref y);

            txtSdt = new RoundedTextBox();
            AddCard(scroll, "📞", "Số điện thoại", txtSdt, ref y);

            cboGioiTinh = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11f), Width = 340 };
            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" }); cboGioiTinh.SelectedIndex = 0;
            AddCard(scroll, "⚥", "Giới tính", cboGioiTinh, ref y);

            txtChucVu = new RoundedTextBox();
            AddCard(scroll, "💼", "Chức vụ", txtChucVu, ref y);

            dtpNgaySinh = new DateTimePicker { Format = DateTimePickerFormat.Short, Font = new Font("Segoe UI", 11f), Width = 340, Value = DateTime.Now.AddYears(-22) };
            AddCard(scroll, "🎂", "Ngày sinh", dtpNgaySinh, ref y);

            lblError = new Label { ForeColor = DANGER, AutoSize = true, Location = new Point(4, y + 4) };
            scroll.Controls.Add(lblError);

            Panel pnlFooter = new Panel { Dock = DockStyle.Bottom, Height = 62, BackColor = Color.White };
            pnlFooter.Paint += (s, e) => e.Graphics.DrawLine(new Pen(BORDER_IDLE, 1), 0, 0, pnlFooter.Width, 0);

            btnHuy = new RoundedButton { Text = "Huỷ", Size = new Size(110, 36), Location = new Point(248, 13), BackColor = Color.White, ForeColor = Color.FromArgb(80, 80, 80) };
            btnHuy.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            btnLuu = new RoundedButton { Text = _isEdit ? "💾 Cập nhật" : "✅ Lưu mới", Size = new Size(128, 36), Location = new Point(364, 13), BackColor = GREEN_DARK, HoverColor = GREEN_LIGHT, ForeColor = Color.White };
            btnLuu.Click += BtnLuu_Click;

            pnlFooter.Controls.AddRange(new Control[] { btnHuy, btnLuu });

            this.Controls.Add(scroll);
            this.Controls.Add(pnlFooter);
            this.Controls.Add(pnlHeader);
        }

        private void AddCard(Panel parent, string icon, string labelText, Control ctrl, ref int y)
        {
            Panel card = new Panel { Size = new Size(428, 64), Location = new Point(0, y) };
            card.Controls.Add(new Label { Text = icon, Font = new Font("Segoe UI", 14f), AutoSize = true, Location = new Point(12, 18) });
            card.Controls.Add(new Label { Text = labelText, ForeColor = Color.Gray, Font = new Font("Segoe UI", 8f), AutoSize = true, Location = new Point(44, 4) });
            ctrl.Location = new Point(44, 22);
            ctrl.Width = 340;
            card.Controls.Add(ctrl);
            parent.Controls.Add(card);
            y += 70;
        }

        private void FillForm(NhanVienDTO nv)
        {
            txtMaNV.Text = nv.MaNV; txtHoTen.Text = nv.HoTen; txtSdt.Text = nv.Sdt;
            txtChucVu.Text = nv.ChucVu; cboGioiTinh.Text = nv.GioiTinh; dtpNgaySinh.Value = nv.NgaySinh ?? DateTime.Now;
        }

        private void PreFillMa() { try { txtMaNV.Text = _bll.SinhMaMoi(); } catch { txtMaNV.Text = "NV01"; } }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
            if (string.IsNullOrWhiteSpace(txtMaNV.Text)) { lblError.Text = "⚠ Mã nhân viên trống!"; return; }
            if (string.IsNullOrWhiteSpace(txtHoTen.Text)) { lblError.Text = "⚠ Họ tên trống!"; return; }

            var nv = new NhanVienDTO(txtMaNV.Text.Trim(), txtHoTen.Text.Trim(), txtSdt.Text.Trim(), cboGioiTinh.Text, txtChucVu.Text.Trim(), dtpNgaySinh.Value);
            try { if (_isEdit) _bll.CapNhatNhanVien(nv); else _bll.ThemNhanVien(nv); DialogResult = DialogResult.OK; Close(); }
            catch (Exception ex) { lblError.Text = "⚠ " + ex.Message; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(GREEN_DARK, 2)) e.Graphics.DrawPath(pen, GraphicsHelper.GetRoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), 14));
        }

        private void DoDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { NativeDrag.ReleaseCapture(); NativeDrag.SendMessage(Handle, 0xA1, 0x2, 0); }
        }
    }
}