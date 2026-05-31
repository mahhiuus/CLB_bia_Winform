using System.Drawing;
namespace Bài_Tập_Lớn.GUI
{
    partial class SanPhamPopupUi
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
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.panelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.panelBody = new Guna.UI2.WinForms.Guna2Panel();
            this.lblMaSP = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.inputMaSP = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.lblTenSP = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.inputTenSP = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.lblGiaBan = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.inputGiaBan = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.lblSoLuong = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.inputSoLuong = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.lblLoai = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cboLoai = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblNCC = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cboNCC = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblAnhSP = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panelAnh = new Guna.UI2.WinForms.Guna2Panel();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.lblTenAnh = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnChonAnh = new Guna.UI2.WinForms.Guna2Button();
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel2 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel3 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel5 = new Guna.UI2.WinForms.Guna2Panel();
            this.panelBody.SuspendLayout();
            this.panelAnh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this;
            // 
            // panelHeader
            // 
            this.panelHeader.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelHeader.BorderThickness = 5;
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(740, 69);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // panelBody
            // 
            this.panelBody.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelBody.BorderRadius = 10;
            this.panelBody.BorderThickness = 5;
            this.panelBody.Controls.Add(this.lblMaSP);
            this.panelBody.Controls.Add(this.inputMaSP);
            this.panelBody.Controls.Add(this.lblTenSP);
            this.panelBody.Controls.Add(this.inputTenSP);
            this.panelBody.Controls.Add(this.lblGiaBan);
            this.panelBody.Controls.Add(this.inputGiaBan);
            this.panelBody.Controls.Add(this.lblSoLuong);
            this.panelBody.Controls.Add(this.inputSoLuong);
            this.panelBody.Controls.Add(this.lblLoai);
            this.panelBody.Controls.Add(this.cboLoai);
            this.panelBody.Controls.Add(this.lblNCC);
            this.panelBody.Controls.Add(this.cboNCC);
            this.panelBody.Controls.Add(this.lblAnhSP);
            this.panelBody.Controls.Add(this.panelAnh);
            this.panelBody.Controls.Add(this.btnXacNhan);
            this.panelBody.Controls.Add(this.btnHuy);
            this.panelBody.Controls.Add(this.guna2Panel2);
            this.panelBody.Controls.Add(this.guna2Panel3);
            this.panelBody.Controls.Add(this.guna2Panel4);
            this.panelBody.Controls.Add(this.guna2Panel5);
            this.panelBody.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.panelBody.Location = new System.Drawing.Point(0, 69);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(740, 760);
            this.panelBody.TabIndex = 1;
            this.panelBody.Paint += new System.Windows.Forms.PaintEventHandler(this.panelBody_Paint);
            // 
            // lblMaSP
            // 
            this.lblMaSP.BackColor = System.Drawing.Color.Transparent;
            this.lblMaSP.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblMaSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblMaSP.Location = new System.Drawing.Point(30, 20);
            this.lblMaSP.Name = "lblMaSP";
            this.lblMaSP.Size = new System.Drawing.Size(112, 25);
            this.lblMaSP.TabIndex = 0;
            this.lblMaSP.Text = "Mã Sản Phẩm";
            this.lblMaSP.Click += new System.EventHandler(this.lblMaSP_Click);
            // 
            // inputMaSP
            // 
            this.inputMaSP.BackColor = System.Drawing.Color.White;
            this.inputMaSP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputMaSP.BorderRadius = 8;
            this.inputMaSP.FocusColor = System.Drawing.Color.Green;
            this.inputMaSP.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.inputMaSP.Location = new System.Drawing.Point(30, 48);
            this.inputMaSP.Name = "inputMaSP";
            this.inputMaSP.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputMaSP.PasswordChar = '\0';
            this.inputMaSP.ReadOnly = true;
            this.inputMaSP.Size = new System.Drawing.Size(320, 38);
            this.inputMaSP.TabIndex = 0;
            // 
            // lblTenSP
            // 
            this.lblTenSP.BackColor = System.Drawing.Color.Transparent;
            this.lblTenSP.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblTenSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblTenSP.Location = new System.Drawing.Point(30, 100);
            this.lblTenSP.Name = "lblTenSP";
            this.lblTenSP.Size = new System.Drawing.Size(129, 25);
            this.lblTenSP.TabIndex = 1;
            this.lblTenSP.Text = "Tên Sản Phẩm <span style=\"color:red\">*</span>";
            this.lblTenSP.Click += new System.EventHandler(this.lblTenSP_Click);
            // 
            // inputTenSP
            // 
            this.inputTenSP.BackColor = System.Drawing.Color.White;
            this.inputTenSP.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputTenSP.BorderRadius = 8;
            this.inputTenSP.FocusColor = System.Drawing.Color.Green;
            this.inputTenSP.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.inputTenSP.Location = new System.Drawing.Point(30, 128);
            this.inputTenSP.Name = "inputTenSP";
            this.inputTenSP.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputTenSP.PasswordChar = '\0';
            this.inputTenSP.ReadOnly = false;
            this.inputTenSP.Size = new System.Drawing.Size(680, 38);
            this.inputTenSP.TabIndex = 1;
            // 
            // lblGiaBan
            // 
            this.lblGiaBan.BackColor = System.Drawing.Color.Transparent;
            this.lblGiaBan.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblGiaBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblGiaBan.Location = new System.Drawing.Point(30, 180);
            this.lblGiaBan.Name = "lblGiaBan";
            this.lblGiaBan.Size = new System.Drawing.Size(105, 25);
            this.lblGiaBan.TabIndex = 2;
            this.lblGiaBan.Text = "Giá Bán (đ) <span style=\"color:red\">*</span>";
            this.lblGiaBan.Click += new System.EventHandler(this.lblGiaBan_Click);
            // 
            // inputGiaBan
            // 
            this.inputGiaBan.BackColor = System.Drawing.Color.White;
            this.inputGiaBan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputGiaBan.BorderRadius = 8;
            this.inputGiaBan.FocusColor = System.Drawing.Color.Green;
            this.inputGiaBan.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.inputGiaBan.Location = new System.Drawing.Point(30, 208);
            this.inputGiaBan.Name = "inputGiaBan";
            this.inputGiaBan.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputGiaBan.PasswordChar = '\0';
            this.inputGiaBan.ReadOnly = false;
            this.inputGiaBan.Size = new System.Drawing.Size(320, 38);
            this.inputGiaBan.TabIndex = 2;
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.BackColor = System.Drawing.Color.Transparent;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblSoLuong.Location = new System.Drawing.Point(390, 180);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(128, 25);
            this.lblSoLuong.TabIndex = 3;
            this.lblSoLuong.Text = "Số Lượng Tồn <span style=\"color:red\">*</span>";
            this.lblSoLuong.Click += new System.EventHandler(this.lblSoLuong_Click);
            // 
            // inputSoLuong
            // 
            this.inputSoLuong.BackColor = System.Drawing.Color.White;
            this.inputSoLuong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputSoLuong.BorderRadius = 8;
            this.inputSoLuong.FocusColor = System.Drawing.Color.Green;
            this.inputSoLuong.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.inputSoLuong.Location = new System.Drawing.Point(390, 208);
            this.inputSoLuong.Name = "inputSoLuong";
            this.inputSoLuong.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputSoLuong.PasswordChar = '\0';
            this.inputSoLuong.ReadOnly = false;
            this.inputSoLuong.Size = new System.Drawing.Size(320, 38);
            this.inputSoLuong.TabIndex = 3;
            // 
            // lblLoai
            // 
            this.lblLoai.BackColor = System.Drawing.Color.Transparent;
            this.lblLoai.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblLoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblLoai.Location = new System.Drawing.Point(30, 260);
            this.lblLoai.Name = "lblLoai";
            this.lblLoai.Size = new System.Drawing.Size(133, 25);
            this.lblLoai.TabIndex = 4;
            this.lblLoai.Text = "Loại Sản Phẩm <span style=\"color:red\">*</span>";
            this.lblLoai.Click += new System.EventHandler(this.lblLoai_Click);
            // 
            // cboLoai
            // 
            this.cboLoai.BackColor = System.Drawing.Color.Transparent;
            this.cboLoai.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboLoai.BorderRadius = 8;
            this.cboLoai.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboLoai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLoai.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboLoai.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboLoai.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cboLoai.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboLoai.ItemHeight = 36;
            this.cboLoai.Location = new System.Drawing.Point(30, 288);
            this.cboLoai.Name = "cboLoai";
            this.cboLoai.Size = new System.Drawing.Size(320, 42);
            this.cboLoai.TabIndex = 4;
            this.cboLoai.SelectedIndexChanged += new System.EventHandler(this.cboLoai_SelectedIndexChanged);
            // 
            // lblNCC
            // 
            this.lblNCC.BackColor = System.Drawing.Color.Transparent;
            this.lblNCC.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblNCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblNCC.Location = new System.Drawing.Point(390, 260);
            this.lblNCC.Name = "lblNCC";
            this.lblNCC.Size = new System.Drawing.Size(118, 25);
            this.lblNCC.TabIndex = 5;
            this.lblNCC.Text = "Nhà Cung Cấp";
            this.lblNCC.Click += new System.EventHandler(this.lblNCC_Click);
            // 
            // cboNCC
            // 
            this.cboNCC.BackColor = System.Drawing.Color.Transparent;
            this.cboNCC.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.BorderRadius = 8;
            this.cboNCC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboNCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNCC.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.Font = new System.Drawing.Font("Segoe UI", 10.2F);
            this.cboNCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.ItemHeight = 36;
            this.cboNCC.Location = new System.Drawing.Point(390, 288);
            this.cboNCC.Name = "cboNCC";
            this.cboNCC.Size = new System.Drawing.Size(320, 42);
            this.cboNCC.TabIndex = 5;
            this.cboNCC.SelectedIndexChanged += new System.EventHandler(this.cboNCC_SelectedIndexChanged);
            // 
            // lblAnhSP
            // 
            this.lblAnhSP.BackColor = System.Drawing.Color.Transparent;
            this.lblAnhSP.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold);
            this.lblAnhSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblAnhSP.Location = new System.Drawing.Point(30, 350);
            this.lblAnhSP.Name = "lblAnhSP";
            this.lblAnhSP.Size = new System.Drawing.Size(162, 25);
            this.lblAnhSP.TabIndex = 6;
            this.lblAnhSP.Text = "Hình Ảnh Sản Phẩm";
            // 
            // panelAnh
            // 
            this.panelAnh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.panelAnh.BorderRadius = 10;
            this.panelAnh.BorderThickness = 1;
            this.panelAnh.Controls.Add(this.picPreview);
            this.panelAnh.Controls.Add(this.lblTenAnh);
            this.panelAnh.Controls.Add(this.btnChonAnh);
            this.panelAnh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(252)))), ((int)(((byte)(245)))));
            this.panelAnh.Location = new System.Drawing.Point(30, 378);
            this.panelAnh.Name = "panelAnh";
            this.panelAnh.Size = new System.Drawing.Size(680, 220);
            this.panelAnh.TabIndex = 20;
            // 
            // picPreview
            // 
            this.picPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(240)))), ((int)(((byte)(230)))));
            this.picPreview.Location = new System.Drawing.Point(19, 31);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(200, 160);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 0;
            this.picPreview.TabStop = false;
            // 
            // lblTenAnh
            // 
            this.lblTenAnh.BackColor = System.Drawing.Color.Transparent;
            this.lblTenAnh.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTenAnh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblTenAnh.Location = new System.Drawing.Point(237, 31);
            this.lblTenAnh.Name = "lblTenAnh";
            this.lblTenAnh.Size = new System.Drawing.Size(96, 23);
            this.lblTenAnh.TabIndex = 1;
            this.lblTenAnh.Text = "(chưa có ảnh)";
            // 
            // btnChonAnh
            // 
            this.btnChonAnh.BackColor = System.Drawing.Color.Transparent;
            this.btnChonAnh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnChonAnh.BorderRadius = 8;
            this.btnChonAnh.BorderThickness = 1;
            this.btnChonAnh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnChonAnh.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnChonAnh.ForeColor = System.Drawing.Color.White;
            this.btnChonAnh.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(122)))), ((int)(((byte)(67)))));
            this.btnChonAnh.Location = new System.Drawing.Point(237, 68);
            this.btnChonAnh.Name = "btnChonAnh";
            this.btnChonAnh.Size = new System.Drawing.Size(180, 38);
            this.btnChonAnh.TabIndex = 1;
            this.btnChonAnh.Text = "📂  Chọn Ảnh";
            this.btnChonAnh.Click += new System.EventHandler(this.btnChonAnh_Click);
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BackColor = System.Drawing.Color.Transparent;
            this.btnXacNhan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.BorderRadius = 10;
            this.btnXacNhan.BorderThickness = 2;
            this.btnXacNhan.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXacNhan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.Location = new System.Drawing.Point(369, 658);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.ShadowDecoration.BorderRadius = 20;
            this.btnXacNhan.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.ShadowDecoration.Depth = 16;
            this.btnXacNhan.ShadowDecoration.Enabled = true;
            this.btnXacNhan.Size = new System.Drawing.Size(341, 57);
            this.btnXacNhan.TabIndex = 71;
            this.btnXacNhan.Text = "Xác Nhận";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.Transparent;
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.BorderRadius = 10;
            this.btnHuy.BorderThickness = 2;
            this.btnHuy.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.CheckedState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.btnHuy.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnHuy.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnHuy.FillColor = System.Drawing.Color.Transparent;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.btnHuy.Location = new System.Drawing.Point(30, 658);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.ShadowDecoration.BorderRadius = 10;
            this.btnHuy.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.ShadowDecoration.Depth = 16;
            this.btnHuy.Size = new System.Drawing.Size(333, 57);
            this.btnHuy.TabIndex = 70;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // guna2Panel2
            // 
            this.guna2Panel2.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel2.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel2.Name = "guna2Panel2";
            this.guna2Panel2.Size = new System.Drawing.Size(10, 10);
            this.guna2Panel2.TabIndex = 90;
            this.guna2Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel2_Paint);
            // 
            // guna2Panel3
            // 
            this.guna2Panel3.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel3.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel3.Name = "guna2Panel3";
            this.guna2Panel3.Size = new System.Drawing.Size(10, 10);
            this.guna2Panel3.TabIndex = 91;
            this.guna2Panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel3_Paint);
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel4.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.Size = new System.Drawing.Size(10, 10);
            this.guna2Panel4.TabIndex = 92;
            this.guna2Panel4.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel4_Paint);
            // 
            // guna2Panel5
            // 
            this.guna2Panel5.FillColor = System.Drawing.Color.Transparent;
            this.guna2Panel5.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel5.Name = "guna2Panel5";
            this.guna2Panel5.Size = new System.Drawing.Size(10, 10);
            this.guna2Panel5.TabIndex = 93;
            this.guna2Panel5.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel5_Paint);
            // 
            // SanPhamPopupUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(740, 830);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelBody);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SanPhamPopupUi";
            this.Text = "SanPhamPopupUi";
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            this.panelAnh.ResumeLayout(false);
            this.panelAnh.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // ── Field declarations ────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Panel panelHeader;
        private Guna.UI2.WinForms.Guna2Panel panelBody;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblMaSP;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTenSP;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGiaBan;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSoLuong;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLoai;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNCC;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblAnhSP;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputMaSP;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputTenSP;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputGiaBan;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputSoLuong;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoai;
        private Guna.UI2.WinForms.Guna2ComboBox cboNCC;
        private Guna.UI2.WinForms.Guna2Panel panelAnh;
        private System.Windows.Forms.PictureBox picPreview;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTenAnh;
        private Guna.UI2.WinForms.Guna2Button btnChonAnh;
        private Guna.UI2.WinForms.Guna2Button btnXacNhan;
        private Guna.UI2.WinForms.Guna2Button btnHuy;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel2;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel3;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel5;
    }
}