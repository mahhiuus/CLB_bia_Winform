namespace Bài_Tập_Lớn.GUI
{
    partial class ThanhToanDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gunaElipse = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.panelHeader = new System.Windows.Forms.Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.tableBody = new System.Windows.Forms.TableLayoutPanel();
            this.lblLBan = new System.Windows.Forms.Label();
            this.lblVBan = new System.Windows.Forms.Label();
            this.lblLNhanVien = new System.Windows.Forms.Label();
            this.lblVNhanVien = new System.Windows.Forms.Label();
            this.lblLMaPhien = new System.Windows.Forms.Label();
            this.lblVMaPhien = new System.Windows.Forms.Label();
            this.lblLBatDau = new System.Windows.Forms.Label();
            this.lblVBatDau = new System.Windows.Forms.Label();
            this.lblLThoiGian = new System.Windows.Forms.Label();
            this.lblThoiGian = new System.Windows.Forms.Label();
            this.lblLGiaGio = new System.Windows.Forms.Label();
            this.lblVGiaGio = new System.Windows.Forms.Label();
            this.sep1 = new System.Windows.Forms.Panel();
            this.lblLTienGio = new System.Windows.Forms.Label();
            this.lblTienGio = new System.Windows.Forms.Label();
            this.lblLTienSP = new System.Windows.Forms.Label();
            this.lblTienSP = new System.Windows.Forms.Label();
            this.sep2 = new System.Windows.Forms.Panel();
            this.lblLKhachHang = new System.Windows.Forms.Label();
            this.cmbKhachHang = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblThongTinKH = new System.Windows.Forms.Label();
            this.lblLGiamSuKien = new System.Windows.Forms.Label();
            this.cmbGiamGiaSuKien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.sep3 = new System.Windows.Forms.Panel();
            this.lblLGiamGia = new System.Windows.Forms.Label();
            this.lblGiamGia = new System.Windows.Forms.Label();
            this.lblLTongTien = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnPay = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.panelHeader.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            this.tableBody.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gunaElipse
            // 
            this.gunaElipse.BorderRadius = 20;
            this.gunaElipse.TargetControl = this;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.Transparent;
            this.panelHeader.Controls.Add(this.guna2Panel1);
            this.panelHeader.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(480, 72);
            this.panelHeader.TabIndex = 10;
            this.panelHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelHeader_MouseDown);
            this.panelHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PanelHeader_MouseMove);
            this.panelHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PanelHeader_MouseUp);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BorderRadius = 10;
            this.guna2Panel1.Controls.Add(this.guna2Button1);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(480, 72);
            this.guna2Panel1.TabIndex = 0;
            // 
            // guna2Button1
            // 
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.Transparent;
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Image = global::Bài_Tập_Lớn.Properties.Resources.wallet;
            this.guna2Button1.ImageSize = new System.Drawing.Size(30, 30);
            this.guna2Button1.Location = new System.Drawing.Point(33, 8);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(394, 54);
            this.guna2Button1.TabIndex = 0;
            this.guna2Button1.Text = "THANH TOÁN";
            // 
            // tableBody
            // 
            this.tableBody.BackColor = System.Drawing.Color.Transparent;
            this.tableBody.ColumnCount = 2;
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48F));
            this.tableBody.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52F));
            this.tableBody.Controls.Add(this.lblLBan, 0, 0);
            this.tableBody.Controls.Add(this.lblVBan, 1, 0);
            this.tableBody.Controls.Add(this.lblLNhanVien, 0, 1);
            this.tableBody.Controls.Add(this.lblVNhanVien, 1, 1);
            this.tableBody.Controls.Add(this.lblLMaPhien, 0, 2);
            this.tableBody.Controls.Add(this.lblVMaPhien, 1, 2);
            this.tableBody.Controls.Add(this.lblLBatDau, 0, 3);
            this.tableBody.Controls.Add(this.lblVBatDau, 1, 3);
            this.tableBody.Controls.Add(this.lblLThoiGian, 0, 4);
            this.tableBody.Controls.Add(this.lblThoiGian, 1, 4);
            this.tableBody.Controls.Add(this.lblLGiaGio, 0, 5);
            this.tableBody.Controls.Add(this.lblVGiaGio, 1, 5);
            this.tableBody.Controls.Add(this.sep1, 0, 6);
            this.tableBody.Controls.Add(this.lblLTienGio, 0, 7);
            this.tableBody.Controls.Add(this.lblTienGio, 1, 7);
            this.tableBody.Controls.Add(this.lblLTienSP, 0, 8);
            this.tableBody.Controls.Add(this.lblTienSP, 1, 8);
            this.tableBody.Controls.Add(this.sep2, 0, 9);
            this.tableBody.Controls.Add(this.lblLKhachHang, 0, 10);
            this.tableBody.Controls.Add(this.cmbKhachHang, 0, 11);
            this.tableBody.Controls.Add(this.lblThongTinKH, 0, 12);
            this.tableBody.Controls.Add(this.lblLGiamSuKien, 0, 13);
            this.tableBody.Controls.Add(this.cmbGiamGiaSuKien, 0, 14);
            this.tableBody.Controls.Add(this.sep3, 0, 15);
            this.tableBody.Controls.Add(this.lblLGiamGia, 0, 16);
            this.tableBody.Controls.Add(this.lblGiamGia, 1, 16);
            this.tableBody.Controls.Add(this.lblLTongTien, 0, 17);
            this.tableBody.Controls.Add(this.lblTongTien, 1, 17);
            this.tableBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableBody.Location = new System.Drawing.Point(0, 72);
            this.tableBody.Name = "tableBody";
            this.tableBody.Padding = new System.Windows.Forms.Padding(28, 14, 28, 8);
            this.tableBody.RowCount = 19;
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableBody.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableBody.Size = new System.Drawing.Size(480, 693);
            this.tableBody.TabIndex = 5;
            this.tableBody.Paint += new System.Windows.Forms.PaintEventHandler(this.tableBody_Paint);
            // 
            // lblLBan
            // 
            this.lblLBan.AutoSize = true;
            this.lblLBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLBan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLBan.Location = new System.Drawing.Point(28, 21);
            this.lblLBan.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLBan.Name = "lblLBan";
            this.lblLBan.Size = new System.Drawing.Size(203, 23);
            this.lblLBan.TabIndex = 0;
            this.lblLBan.Text = "🎱  Bàn";
            this.lblLBan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVBan
            // 
            this.lblVBan.AutoSize = true;
            this.lblVBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVBan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVBan.Location = new System.Drawing.Point(231, 21);
            this.lblVBan.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblVBan.Name = "lblVBan";
            this.lblVBan.Size = new System.Drawing.Size(221, 23);
            this.lblVBan.TabIndex = 1;
            this.lblVBan.Text = "—";
            this.lblVBan.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLNhanVien
            // 
            this.lblLNhanVien.AutoSize = true;
            this.lblLNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLNhanVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLNhanVien.Location = new System.Drawing.Point(28, 58);
            this.lblLNhanVien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLNhanVien.Name = "lblLNhanVien";
            this.lblLNhanVien.Size = new System.Drawing.Size(203, 23);
            this.lblLNhanVien.TabIndex = 2;
            this.lblLNhanVien.Text = "👤  Nhân viên";
            this.lblLNhanVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVNhanVien
            // 
            this.lblVNhanVien.AutoSize = true;
            this.lblVNhanVien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVNhanVien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVNhanVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVNhanVien.Location = new System.Drawing.Point(231, 58);
            this.lblVNhanVien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblVNhanVien.Name = "lblVNhanVien";
            this.lblVNhanVien.Size = new System.Drawing.Size(221, 23);
            this.lblVNhanVien.TabIndex = 3;
            this.lblVNhanVien.Text = "—";
            this.lblVNhanVien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLMaPhien
            // 
            this.lblLMaPhien.AutoSize = true;
            this.lblLMaPhien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLMaPhien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLMaPhien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLMaPhien.Location = new System.Drawing.Point(28, 95);
            this.lblLMaPhien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLMaPhien.Name = "lblLMaPhien";
            this.lblLMaPhien.Size = new System.Drawing.Size(203, 23);
            this.lblLMaPhien.TabIndex = 4;
            this.lblLMaPhien.Text = "🔖  Mã phiên";
            this.lblLMaPhien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVMaPhien
            // 
            this.lblVMaPhien.AutoSize = true;
            this.lblVMaPhien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVMaPhien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVMaPhien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVMaPhien.Location = new System.Drawing.Point(231, 95);
            this.lblVMaPhien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblVMaPhien.Name = "lblVMaPhien";
            this.lblVMaPhien.Size = new System.Drawing.Size(221, 23);
            this.lblVMaPhien.TabIndex = 5;
            this.lblVMaPhien.Text = "—";
            this.lblVMaPhien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLBatDau
            // 
            this.lblLBatDau.AutoSize = true;
            this.lblLBatDau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLBatDau.Location = new System.Drawing.Point(28, 132);
            this.lblLBatDau.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLBatDau.Name = "lblLBatDau";
            this.lblLBatDau.Size = new System.Drawing.Size(203, 23);
            this.lblLBatDau.TabIndex = 6;
            this.lblLBatDau.Text = "⏱  Bắt đầu";
            this.lblLBatDau.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVBatDau
            // 
            this.lblVBatDau.AutoSize = true;
            this.lblVBatDau.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVBatDau.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVBatDau.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVBatDau.Location = new System.Drawing.Point(231, 132);
            this.lblVBatDau.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblVBatDau.Name = "lblVBatDau";
            this.lblVBatDau.Size = new System.Drawing.Size(221, 23);
            this.lblVBatDau.TabIndex = 7;
            this.lblVBatDau.Text = "—";
            this.lblVBatDau.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLThoiGian
            // 
            this.lblLThoiGian.AutoSize = true;
            this.lblLThoiGian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLThoiGian.Location = new System.Drawing.Point(28, 169);
            this.lblLThoiGian.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLThoiGian.Name = "lblLThoiGian";
            this.lblLThoiGian.Size = new System.Drawing.Size(203, 23);
            this.lblLThoiGian.TabIndex = 8;
            this.lblLThoiGian.Text = "🕐  Thời gian";
            this.lblLThoiGian.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblThoiGian
            // 
            this.lblThoiGian.AutoSize = true;
            this.lblThoiGian.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThoiGian.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblThoiGian.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblThoiGian.Location = new System.Drawing.Point(231, 169);
            this.lblThoiGian.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblThoiGian.Name = "lblThoiGian";
            this.lblThoiGian.Size = new System.Drawing.Size(221, 23);
            this.lblThoiGian.TabIndex = 9;
            this.lblThoiGian.Text = "00:00:00";
            this.lblThoiGian.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLGiaGio
            // 
            this.lblLGiaGio.AutoSize = true;
            this.lblLGiaGio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLGiaGio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLGiaGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLGiaGio.Location = new System.Drawing.Point(28, 206);
            this.lblLGiaGio.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLGiaGio.Name = "lblLGiaGio";
            this.lblLGiaGio.Size = new System.Drawing.Size(203, 23);
            this.lblLGiaGio.TabIndex = 10;
            this.lblLGiaGio.Text = "💰  Giá/giờ";
            this.lblLGiaGio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblVGiaGio
            // 
            this.lblVGiaGio.AutoSize = true;
            this.lblVGiaGio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblVGiaGio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblVGiaGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVGiaGio.Location = new System.Drawing.Point(231, 206);
            this.lblVGiaGio.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblVGiaGio.Name = "lblVGiaGio";
            this.lblVGiaGio.Size = new System.Drawing.Size(221, 23);
            this.lblVGiaGio.TabIndex = 11;
            this.lblVGiaGio.Text = "—";
            this.lblVGiaGio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sep1
            // 
            this.sep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(220)))));
            this.tableBody.SetColumnSpan(this.sep1, 2);
            this.sep1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sep1.Location = new System.Drawing.Point(28, 242);
            this.sep1.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(424, 6);
            this.sep1.TabIndex = 20;
            // 
            // lblLTienGio
            // 
            this.lblLTienGio.AutoSize = true;
            this.lblLTienGio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLTienGio.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLTienGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLTienGio.Location = new System.Drawing.Point(28, 261);
            this.lblLTienGio.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLTienGio.Name = "lblLTienGio";
            this.lblLTienGio.Size = new System.Drawing.Size(203, 28);
            this.lblLTienGio.TabIndex = 12;
            this.lblLTienGio.Text = "⏳  Tiền giờ chơi";
            this.lblLTienGio.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTienGio
            // 
            this.lblTienGio.AutoSize = true;
            this.lblTienGio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTienGio.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTienGio.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblTienGio.Location = new System.Drawing.Point(231, 261);
            this.lblTienGio.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblTienGio.Name = "lblTienGio";
            this.lblTienGio.Size = new System.Drawing.Size(221, 28);
            this.lblTienGio.TabIndex = 13;
            this.lblTienGio.Text = "0 đ";
            this.lblTienGio.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLTienSP
            // 
            this.lblLTienSP.AutoSize = true;
            this.lblLTienSP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLTienSP.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLTienSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.lblLTienSP.Location = new System.Drawing.Point(28, 303);
            this.lblLTienSP.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLTienSP.Name = "lblLTienSP";
            this.lblLTienSP.Size = new System.Drawing.Size(203, 28);
            this.lblLTienSP.TabIndex = 14;
            this.lblLTienSP.Text = "🛒  Tiền sản phẩm";
            this.lblLTienSP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTienSP
            // 
            this.lblTienSP.AutoSize = true;
            this.lblTienSP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTienSP.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTienSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblTienSP.Location = new System.Drawing.Point(231, 303);
            this.lblTienSP.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblTienSP.Name = "lblTienSP";
            this.lblTienSP.Size = new System.Drawing.Size(221, 28);
            this.lblTienSP.TabIndex = 15;
            this.lblTienSP.Text = "0 đ";
            this.lblTienSP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sep2
            // 
            this.sep2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(225)))), ((int)(((byte)(225)))), ((int)(((byte)(220)))));
            this.tableBody.SetColumnSpan(this.sep2, 2);
            this.sep2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sep2.Location = new System.Drawing.Point(28, 344);
            this.sep2.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(424, 6);
            this.sep2.TabIndex = 21;
            // 
            // lblLKhachHang
            // 
            this.lblLKhachHang.AutoSize = true;
            this.tableBody.SetColumnSpan(this.lblLKhachHang, 2);
            this.lblLKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLKhachHang.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblLKhachHang.Location = new System.Drawing.Point(28, 368);
            this.lblLKhachHang.Margin = new System.Windows.Forms.Padding(0, 12, 0, 4);
            this.lblLKhachHang.Name = "lblLKhachHang";
            this.lblLKhachHang.Size = new System.Drawing.Size(424, 21);
            this.lblLKhachHang.TabIndex = 31;
            this.lblLKhachHang.Text = "⭐  Khách hàng thân thiết";
            this.lblLKhachHang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbKhachHang
            // 
            this.cmbKhachHang.BackColor = System.Drawing.Color.Transparent;
            this.cmbKhachHang.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cmbKhachHang.BorderRadius = 10;
            this.tableBody.SetColumnSpan(this.cmbKhachHang, 2);
            this.cmbKhachHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbKhachHang.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbKhachHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbKhachHang.FocusedColor = System.Drawing.Color.Green;
            this.cmbKhachHang.FocusedState.BorderColor = System.Drawing.Color.Green;
            this.cmbKhachHang.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbKhachHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbKhachHang.ItemHeight = 30;
            this.cmbKhachHang.Location = new System.Drawing.Point(28, 393);
            this.cmbKhachHang.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.cmbKhachHang.Name = "cmbKhachHang";
            this.cmbKhachHang.Size = new System.Drawing.Size(424, 36);
            this.cmbKhachHang.TabIndex = 32;
            this.cmbKhachHang.SelectedIndexChanged += new System.EventHandler(this.CmbKhachHang_SelectedIndexChanged);
            // 
            // lblThongTinKH
            // 
            this.tableBody.SetColumnSpan(this.lblThongTinKH, 2);
            this.lblThongTinKH.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblThongTinKH.Font = new System.Drawing.Font("Segoe UI", 8.5F, System.Drawing.FontStyle.Italic);
            this.lblThongTinKH.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(120)))), ((int)(((byte)(43)))));
            this.lblThongTinKH.Location = new System.Drawing.Point(28, 437);
            this.lblThongTinKH.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblThongTinKH.Name = "lblThongTinKH";
            this.lblThongTinKH.Size = new System.Drawing.Size(424, 20);
            this.lblThongTinKH.TabIndex = 33;
            this.lblThongTinKH.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblThongTinKH.Visible = false;
            // 
            // lblLGiamSuKien
            // 
            this.lblLGiamSuKien.AutoSize = true;
            this.tableBody.SetColumnSpan(this.lblLGiamSuKien, 2);
            this.lblLGiamSuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLGiamSuKien.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblLGiamSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblLGiamSuKien.Location = new System.Drawing.Point(28, 477);
            this.lblLGiamSuKien.Margin = new System.Windows.Forms.Padding(0, 12, 0, 4);
            this.lblLGiamSuKien.Name = "lblLGiamSuKien";
            this.lblLGiamSuKien.Size = new System.Drawing.Size(424, 21);
            this.lblLGiamSuKien.TabIndex = 34;
            this.lblLGiamSuKien.Text = "🎉  Giảm giá sự kiện";
            this.lblLGiamSuKien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLGiamSuKien.Click += new System.EventHandler(this.lblLGiamSuKien_Click);
            // 
            // cmbGiamGiaSuKien
            // 
            this.cmbGiamGiaSuKien.BackColor = System.Drawing.Color.Transparent;
            this.cmbGiamGiaSuKien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cmbGiamGiaSuKien.BorderRadius = 10;
            this.tableBody.SetColumnSpan(this.cmbGiamGiaSuKien, 2);
            this.cmbGiamGiaSuKien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbGiamGiaSuKien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbGiamGiaSuKien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGiamGiaSuKien.FocusedColor = System.Drawing.Color.Green;
            this.cmbGiamGiaSuKien.FocusedState.BorderColor = System.Drawing.Color.Green;
            this.cmbGiamGiaSuKien.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbGiamGiaSuKien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.cmbGiamGiaSuKien.ItemHeight = 30;
            this.cmbGiamGiaSuKien.Items.AddRange(new object[] {
            "Không giảm",
            "5%",
            "10%",
            "15%",
            "20%"});
            this.cmbGiamGiaSuKien.Location = new System.Drawing.Point(28, 502);
            this.cmbGiamGiaSuKien.Margin = new System.Windows.Forms.Padding(0, 0, 0, 15);
            this.cmbGiamGiaSuKien.Name = "cmbGiamGiaSuKien";
            this.cmbGiamGiaSuKien.Size = new System.Drawing.Size(424, 36);
            this.cmbGiamGiaSuKien.TabIndex = 35;
            this.cmbGiamGiaSuKien.SelectedIndexChanged += new System.EventHandler(this.CmbGiamGiaSuKien_SelectedIndexChanged);
            // 
            // sep3
            // 
            this.sep3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(230)))), ((int)(((byte)(200)))));
            this.tableBody.SetColumnSpan(this.sep3, 2);
            this.sep3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sep3.Location = new System.Drawing.Point(28, 559);
            this.sep3.Margin = new System.Windows.Forms.Padding(0, 6, 0, 6);
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(424, 6);
            this.sep3.TabIndex = 30;
            // 
            // lblLGiamGia
            // 
            this.lblLGiamGia.AutoSize = true;
            this.lblLGiamGia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLGiamGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblLGiamGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(120)))), ((int)(((byte)(43)))));
            this.lblLGiamGia.Location = new System.Drawing.Point(28, 578);
            this.lblLGiamGia.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLGiamGia.Name = "lblLGiamGia";
            this.lblLGiamGia.Size = new System.Drawing.Size(203, 23);
            this.lblLGiamGia.TabIndex = 36;
            this.lblLGiamGia.Text = "🏷️  Chiết khấu";
            this.lblLGiamGia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLGiamGia.Visible = false;
            // 
            // lblGiamGia
            // 
            this.lblGiamGia.AutoSize = true;
            this.lblGiamGia.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblGiamGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGiamGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(120)))), ((int)(((byte)(43)))));
            this.lblGiamGia.Location = new System.Drawing.Point(231, 578);
            this.lblGiamGia.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblGiamGia.Name = "lblGiamGia";
            this.lblGiamGia.Size = new System.Drawing.Size(221, 23);
            this.lblGiamGia.TabIndex = 37;
            this.lblGiamGia.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblGiamGia.Visible = false;
            this.lblGiamGia.Click += new System.EventHandler(this.lblGiamGia_Click);
            // 
            // lblLTongTien
            // 
            this.lblLTongTien.AutoSize = true;
            this.lblLTongTien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblLTongTien.Font = new System.Drawing.Font("Segoe UI", 11.5F, System.Drawing.FontStyle.Bold);
            this.lblLTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblLTongTien.Location = new System.Drawing.Point(28, 615);
            this.lblLTongTien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblLTongTien.Name = "lblLTongTien";
            this.lblLTongTien.Size = new System.Drawing.Size(203, 35);
            this.lblLTongTien.TabIndex = 16;
            this.lblLTongTien.Text = "💵  TỔNG TIỀN";
            this.lblLTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTongTien
            // 
            this.lblTongTien.AutoSize = true;
            this.lblTongTien.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.lblTongTien.Location = new System.Drawing.Point(231, 615);
            this.lblTongTien.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(221, 35);
            this.lblTongTien.TabIndex = 17;
            this.lblTongTien.Text = "0 đ";
            this.lblTongTien.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.Transparent;
            this.panelFooter.Controls.Add(this.btnPay);
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 765);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Padding = new System.Windows.Forms.Padding(28, 10, 28, 14);
            this.panelFooter.Size = new System.Drawing.Size(480, 118);
            this.panelFooter.TabIndex = 0;
            // 
            // btnPay
            // 
            this.btnPay.BackColor = System.Drawing.Color.Transparent;
            this.btnPay.BorderRadius = 12;
            this.btnPay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPay.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPay.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPay.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPay.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPay.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnPay.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnPay.ForeColor = System.Drawing.Color.White;
            this.btnPay.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.btnPay.Location = new System.Drawing.Point(28, 10);
            this.btnPay.Name = "btnPay";
            this.btnPay.PressedColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(60)))), ((int)(((byte)(22)))));
            this.btnPay.ShadowDecoration.BorderRadius = 12;
            this.btnPay.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnPay.ShadowDecoration.Depth = 8;
            this.btnPay.ShadowDecoration.Enabled = true;
            this.btnPay.ShadowDecoration.Shadow = new System.Windows.Forms.Padding(2);
            this.btnPay.Size = new System.Drawing.Size(424, 44);
            this.btnPay.TabIndex = 0;
            this.btnPay.Text = "✔   XÁC NHẬN THANH TOÁN";
            this.btnPay.Click += new System.EventHandler(this.BtnPay_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btnCancel.BorderRadius = 12;
            this.btnCancel.BorderThickness = 1;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FillColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btnCancel.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(55)))), ((int)(((byte)(55)))));
            this.btnCancel.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(28, 63);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(424, 40);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "✖   HỦY BỎ";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // ThanhToanDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(480, 883);
            this.Controls.Add(this.tableBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ThanhToanDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Thanh Toán";
            this.Load += new System.EventHandler(this.ThanhToanDialog_Load);
            this.panelHeader.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            this.tableBody.ResumeLayout(false);
            this.tableBody.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // ── Guna ──────────────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Elipse gunaElipse;
        private Guna.UI2.WinForms.Guna2Button btnPay;
        private Guna.UI2.WinForms.Guna2Button btnCancel;

        // ── Panels ────────────────────────────────────────────────
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.TableLayoutPanel tableBody;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel sep1;
        private System.Windows.Forms.Panel sep2;
        private System.Windows.Forms.Panel sep3;                   // [MỚI]

        // ── Key labels (trái) ─────────────────────────────────────
        private System.Windows.Forms.Label lblLBan;
        private System.Windows.Forms.Label lblLNhanVien;
        private System.Windows.Forms.Label lblLMaPhien;
        private System.Windows.Forms.Label lblLBatDau;
        private System.Windows.Forms.Label lblLThoiGian;
        private System.Windows.Forms.Label lblLGiaGio;
        private System.Windows.Forms.Label lblLTienGio;
        private System.Windows.Forms.Label lblLTienSP;
        private System.Windows.Forms.Label lblLTongTien;
        private System.Windows.Forms.Label lblLGiamGia;             // [MỚI]

        // ── Value labels tĩnh (phải) ──────────────────────────────
        private System.Windows.Forms.Label lblVBan;
        private System.Windows.Forms.Label lblVNhanVien;
        private System.Windows.Forms.Label lblVMaPhien;
        private System.Windows.Forms.Label lblVBatDau;
        private System.Windows.Forms.Label lblVGiaGio;

        // ── Value labels live (phải) ──────────────────────────────
        private System.Windows.Forms.Label lblThoiGian;
        private System.Windows.Forms.Label lblTienGio;
        private System.Windows.Forms.Label lblTienSP;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblGiamGia;              // [MỚI]

        // ── [MỚI] Khách hàng thân thiết ──────────────────────────
        private System.Windows.Forms.Label lblLKhachHang;
        private Guna.UI2.WinForms.Guna2ComboBox cmbKhachHang;
        private System.Windows.Forms.Label lblThongTinKH;

        // ── [MỚI] Giảm giá sự kiện ───────────────────────────────
        private System.Windows.Forms.Label lblLGiamSuKien;
        private Guna.UI2.WinForms.Guna2ComboBox cmbGiamGiaSuKien;

        // ── Guna header ───────────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}