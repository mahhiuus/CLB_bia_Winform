using System.Drawing;

namespace Bài_Tập_Lớn.GUI
{
    partial class NhapHangPopup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.panelHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.panelBody = new Guna.UI2.WinForms.Guna2Panel();
            this.lblMaHDN = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblNgayNhap = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dtpNgayNhap = new System.Windows.Forms.DateTimePicker();
            this.lblMaNCC = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cboNCC = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblGhiChu = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.panelDivider = new Guna.UI2.WinForms.Guna2Panel();
            this.lblChonSP = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.cboSanPham = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblSoLuong = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblDonGia = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblLoiNhuan = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblGiaDeXuat = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblGiaDeXuatVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnTinhGia = new Guna.UI2.WinForms.Guna2Button();
            this.btnThemVaoGio = new Guna.UI2.WinForms.Guna2Button();
            this.lblChiTiet = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.dgvChiTiet = new Guna.UI2.WinForms.Guna2DataGridView();
            this.ColSTT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMaSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTenSP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDonGiaNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColXoa = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panelFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTongTienText = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.lblTongTienVal = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnXacNhan = new Guna.UI2.WinForms.Guna2Button();
            this.btnHuy = new Guna.UI2.WinForms.Guna2Button();
            this.inputMaHDN = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.inputGhiChu = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.inputSoLuong = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.inputDonGia = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.inputLoiNhuan = new Bài_Tập_Lớn.UI.RoundedTextBox();
            this.panelBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).BeginInit();
            this.panelFooter.SuspendLayout();
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
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(900, 64);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // panelBody
            // 
            this.panelBody.BorderThickness = 2;
            this.panelBody.Controls.Add(this.lblMaHDN);
            this.panelBody.Controls.Add(this.inputMaHDN);
            this.panelBody.Controls.Add(this.lblNgayNhap);
            this.panelBody.Controls.Add(this.dtpNgayNhap);
            this.panelBody.Controls.Add(this.lblMaNCC);
            this.panelBody.Controls.Add(this.cboNCC);
            this.panelBody.Controls.Add(this.lblGhiChu);
            this.panelBody.Controls.Add(this.inputGhiChu);
            this.panelBody.Controls.Add(this.panelDivider);
            this.panelBody.Controls.Add(this.lblChonSP);
            this.panelBody.Controls.Add(this.cboSanPham);
            this.panelBody.Controls.Add(this.lblSoLuong);
            this.panelBody.Controls.Add(this.inputSoLuong);
            this.panelBody.Controls.Add(this.lblDonGia);
            this.panelBody.Controls.Add(this.inputDonGia);
            this.panelBody.Controls.Add(this.lblLoiNhuan);
            this.panelBody.Controls.Add(this.inputLoiNhuan);
            this.panelBody.Controls.Add(this.lblGiaDeXuat);
            this.panelBody.Controls.Add(this.lblGiaDeXuatVal);
            this.panelBody.Controls.Add(this.btnTinhGia);
            this.panelBody.Controls.Add(this.btnThemVaoGio);
            this.panelBody.Controls.Add(this.lblChiTiet);
            this.panelBody.Controls.Add(this.dgvChiTiet);
            this.panelBody.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panelBody.CustomBorderThickness = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.panelBody.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.panelBody.Location = new System.Drawing.Point(0, 64);
            this.panelBody.Name = "panelBody";
            this.panelBody.Size = new System.Drawing.Size(900, 700);
            this.panelBody.TabIndex = 1;
            // 
            // lblMaHDN
            // 
            this.lblMaHDN.BackColor = System.Drawing.Color.Transparent;
            this.lblMaHDN.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaHDN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblMaHDN.Location = new System.Drawing.Point(24, 18);
            this.lblMaHDN.Name = "lblMaHDN";
            this.lblMaHDN.Size = new System.Drawing.Size(125, 25);
            this.lblMaHDN.TabIndex = 0;
            this.lblMaHDN.Text = "Mã Phiếu Nhập";
            // 
            // lblNgayNhap
            // 
            this.lblNgayNhap.BackColor = System.Drawing.Color.Transparent;
            this.lblNgayNhap.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNgayNhap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblNgayNhap.Location = new System.Drawing.Point(230, 18);
            this.lblNgayNhap.Name = "lblNgayNhap";
            this.lblNgayNhap.Size = new System.Drawing.Size(93, 25);
            this.lblNgayNhap.TabIndex = 1;
            this.lblNgayNhap.Text = "Ngày Nhập";
            // 
            // dtpNgayNhap
            // 
            this.dtpNgayNhap.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpNgayNhap.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgayNhap.Location = new System.Drawing.Point(230, 44);
            this.dtpNgayNhap.Name = "dtpNgayNhap";
            this.dtpNgayNhap.Size = new System.Drawing.Size(180, 30);
            this.dtpNgayNhap.TabIndex = 1;
            // 
            // lblMaNCC
            // 
            this.lblMaNCC.BackColor = System.Drawing.Color.Transparent;
            this.lblMaNCC.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMaNCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblMaNCC.Location = new System.Drawing.Point(428, 18);
            this.lblMaNCC.Name = "lblMaNCC";
            this.lblMaNCC.Size = new System.Drawing.Size(118, 25);
            this.lblMaNCC.TabIndex = 2;
            this.lblMaNCC.Text = "Nhà Cung Cấp";
            // 
            // cboNCC
            // 
            this.cboNCC.BackColor = System.Drawing.Color.Transparent;
            this.cboNCC.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.BorderRadius = 8;
            this.cboNCC.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboNCC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNCC.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.cboNCC.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.cboNCC.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboNCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboNCC.ItemHeight = 34;
            this.cboNCC.Location = new System.Drawing.Point(428, 44);
            this.cboNCC.Name = "cboNCC";
            this.cboNCC.Size = new System.Drawing.Size(220, 40);
            this.cboNCC.TabIndex = 2;
            // 
            // lblGhiChu
            // 
            this.lblGhiChu.BackColor = System.Drawing.Color.Transparent;
            this.lblGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGhiChu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblGhiChu.Location = new System.Drawing.Point(664, 18);
            this.lblGhiChu.Name = "lblGhiChu";
            this.lblGhiChu.Size = new System.Drawing.Size(66, 25);
            this.lblGhiChu.TabIndex = 3;
            this.lblGhiChu.Text = "Ghi Chú";
            // 
            // panelDivider
            // 
            this.panelDivider.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.panelDivider.Location = new System.Drawing.Point(24, 96);
            this.panelDivider.Name = "panelDivider";
            this.panelDivider.Size = new System.Drawing.Size(852, 2);
            this.panelDivider.TabIndex = 99;
            // 
            // lblChonSP
            // 
            this.lblChonSP.BackColor = System.Drawing.Color.Transparent;
            this.lblChonSP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChonSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblChonSP.Location = new System.Drawing.Point(24, 108);
            this.lblChonSP.Name = "lblChonSP";
            this.lblChonSP.Size = new System.Drawing.Size(128, 25);
            this.lblChonSP.TabIndex = 100;
            this.lblChonSP.Text = "Chọn Sản Phẩm";
            // 
            // cboSanPham
            // 
            this.cboSanPham.BackColor = System.Drawing.Color.Transparent;
            this.cboSanPham.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboSanPham.BorderRadius = 8;
            this.cboSanPham.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboSanPham.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSanPham.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.cboSanPham.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.cboSanPham.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboSanPham.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.cboSanPham.ItemHeight = 34;
            this.cboSanPham.Location = new System.Drawing.Point(24, 134);
            this.cboSanPham.Name = "cboSanPham";
            this.cboSanPham.Size = new System.Drawing.Size(260, 40);
            this.cboSanPham.TabIndex = 4;
            this.cboSanPham.SelectedIndexChanged += new System.EventHandler(this.cboSanPham_SelectedIndexChanged);
            // 
            // lblSoLuong
            // 
            this.lblSoLuong.BackColor = System.Drawing.Color.Transparent;
            this.lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblSoLuong.Location = new System.Drawing.Point(300, 108);
            this.lblSoLuong.Name = "lblSoLuong";
            this.lblSoLuong.Size = new System.Drawing.Size(80, 25);
            this.lblSoLuong.TabIndex = 101;
            this.lblSoLuong.Text = "Số Lượng";
            // 
            // lblDonGia
            // 
            this.lblDonGia.BackColor = System.Drawing.Color.Transparent;
            this.lblDonGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDonGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblDonGia.Location = new System.Drawing.Point(448, 108);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(144, 25);
            this.lblDonGia.TabIndex = 102;
            this.lblDonGia.Text = "Đơn Giá Nhập (đ)";
            // 
            // lblLoiNhuan
            // 
            this.lblLoiNhuan.BackColor = System.Drawing.Color.Transparent;
            this.lblLoiNhuan.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblLoiNhuan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblLoiNhuan.Location = new System.Drawing.Point(24, 186);
            this.lblLoiNhuan.Name = "lblLoiNhuan";
            this.lblLoiNhuan.Size = new System.Drawing.Size(174, 25);
            this.lblLoiNhuan.TabIndex = 103;
            this.lblLoiNhuan.Text = "% Lợi Nhuận Đề Xuất";
            // 
            // lblGiaDeXuat
            // 
            this.lblGiaDeXuat.BackColor = System.Drawing.Color.Transparent;
            this.lblGiaDeXuat.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGiaDeXuat.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblGiaDeXuat.Location = new System.Drawing.Point(255, 186);
            this.lblGiaDeXuat.Name = "lblGiaDeXuat";
            this.lblGiaDeXuat.Size = new System.Drawing.Size(138, 25);
            this.lblGiaDeXuat.TabIndex = 104;
            this.lblGiaDeXuat.Text = "Giá Bán Đề Xuất:";
            // 
            // lblGiaDeXuatVal
            // 
            this.lblGiaDeXuatVal.BackColor = System.Drawing.Color.Transparent;
            this.lblGiaDeXuatVal.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblGiaDeXuatVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(90)))), ((int)(((byte)(20)))));
            this.lblGiaDeXuatVal.Location = new System.Drawing.Point(255, 217);
            this.lblGiaDeXuatVal.Name = "lblGiaDeXuatVal";
            this.lblGiaDeXuatVal.Size = new System.Drawing.Size(23, 30);
            this.lblGiaDeXuatVal.TabIndex = 105;
            this.lblGiaDeXuatVal.Text = "—";
            // 
            // btnTinhGia
            // 
            this.btnTinhGia.BackColor = System.Drawing.Color.Transparent;
            this.btnTinhGia.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTinhGia.BorderRadius = 8;
            this.btnTinhGia.BorderThickness = 1;
            this.btnTinhGia.FillColor = System.Drawing.Color.White;
            this.btnTinhGia.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTinhGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTinhGia.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.btnTinhGia.Location = new System.Drawing.Point(526, 212);
            this.btnTinhGia.Name = "btnTinhGia";
            this.btnTinhGia.Size = new System.Drawing.Size(130, 38);
            this.btnTinhGia.TabIndex = 8;
            this.btnTinhGia.Text = "⟳ Tính Giá";
            this.btnTinhGia.Click += new System.EventHandler(this.btnTinhGia_Click);
            // 
            // btnThemVaoGio
            // 
            this.btnThemVaoGio.BackColor = System.Drawing.Color.Transparent;
            this.btnThemVaoGio.BorderRadius = 8;
            this.btnThemVaoGio.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.btnThemVaoGio.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnThemVaoGio.ForeColor = System.Drawing.Color.White;
            this.btnThemVaoGio.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnThemVaoGio.Location = new System.Drawing.Point(674, 212);
            this.btnThemVaoGio.Name = "btnThemVaoGio";
            this.btnThemVaoGio.Size = new System.Drawing.Size(170, 38);
            this.btnThemVaoGio.TabIndex = 9;
            this.btnThemVaoGio.Text = "+ Thêm vào phiếu";
            this.btnThemVaoGio.Click += new System.EventHandler(this.btnThemVaoGio_Click);
            // 
            // lblChiTiet
            // 
            this.lblChiTiet.BackColor = System.Drawing.Color.Transparent;
            this.lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblChiTiet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblChiTiet.Location = new System.Drawing.Point(24, 262);
            this.lblChiTiet.Name = "lblChiTiet";
            this.lblChiTiet.Size = new System.Drawing.Size(197, 25);
            this.lblChiTiet.TabIndex = 106;
            this.lblChiTiet.Text = "Chi Tiết Sản Phẩm Nhập";
            // 
            // dgvChiTiet
            // 
            this.dgvChiTiet.AllowUserToAddRows = false;
            this.dgvChiTiet.AllowUserToDeleteRows = false;
            this.dgvChiTiet.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            this.dgvChiTiet.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChiTiet.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvChiTiet.ColumnHeadersHeight = 40;
            this.dgvChiTiet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColSTT,
            this.ColMaSP,
            this.ColTenSP,
            this.ColSoLuong,
            this.ColDonGiaNhap,
            this.ColThanhTien,
            this.ColXoa});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChiTiet.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvChiTiet.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.dgvChiTiet.Location = new System.Drawing.Point(3, 293);
            this.dgvChiTiet.MultiSelect = false;
            this.dgvChiTiet.Name = "dgvChiTiet";
            this.dgvChiTiet.ReadOnly = true;
            this.dgvChiTiet.RowHeadersVisible = false;
            this.dgvChiTiet.RowHeadersWidth = 51;
            this.dgvChiTiet.RowTemplate.Height = 38;
            this.dgvChiTiet.Size = new System.Drawing.Size(894, 330);
            this.dgvChiTiet.TabIndex = 10;
            this.dgvChiTiet.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiTiet.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvChiTiet.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvChiTiet.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvChiTiet.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvChiTiet.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiTiet.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.dgvChiTiet.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvChiTiet.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvChiTiet.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.dgvChiTiet.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvChiTiet.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvChiTiet.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvChiTiet.ThemeStyle.ReadOnly = true;
            this.dgvChiTiet.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvChiTiet.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvChiTiet.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvChiTiet.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvChiTiet.ThemeStyle.RowsStyle.Height = 38;
            this.dgvChiTiet.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.dgvChiTiet.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvChiTiet.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTiet_CellContentClick);
            this.dgvChiTiet.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvChiTiet_CellFormatting);
            this.dgvChiTiet.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTiet_CellMouseEnter);
            this.dgvChiTiet.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChiTiet_CellMouseLeave);
            // 
            // ColSTT
            // 
            this.ColSTT.HeaderText = "STT";
            this.ColSTT.MinimumWidth = 6;
            this.ColSTT.Name = "ColSTT";
            this.ColSTT.ReadOnly = true;
            // 
            // ColMaSP
            // 
            this.ColMaSP.HeaderText = "Mã SP";
            this.ColMaSP.MinimumWidth = 6;
            this.ColMaSP.Name = "ColMaSP";
            this.ColMaSP.ReadOnly = true;
            // 
            // ColTenSP
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColTenSP.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColTenSP.HeaderText = "Tên Sản Phẩm";
            this.ColTenSP.MinimumWidth = 6;
            this.ColTenSP.Name = "ColTenSP";
            this.ColTenSP.ReadOnly = true;
            // 
            // ColSoLuong
            // 
            this.ColSoLuong.HeaderText = "Số Lượng";
            this.ColSoLuong.MinimumWidth = 6;
            this.ColSoLuong.Name = "ColSoLuong";
            this.ColSoLuong.ReadOnly = true;
            // 
            // ColDonGiaNhap
            // 
            this.ColDonGiaNhap.HeaderText = "Đơn Giá Nhập";
            this.ColDonGiaNhap.MinimumWidth = 6;
            this.ColDonGiaNhap.Name = "ColDonGiaNhap";
            this.ColDonGiaNhap.ReadOnly = true;
            // 
            // ColThanhTien
            // 
            this.ColThanhTien.HeaderText = "Thành Tiền";
            this.ColThanhTien.MinimumWidth = 6;
            this.ColThanhTien.Name = "ColThanhTien";
            this.ColThanhTien.ReadOnly = true;
            // 
            // ColXoa
            // 
            this.ColXoa.HeaderText = "";
            this.ColXoa.MinimumWidth = 6;
            this.ColXoa.Name = "ColXoa";
            this.ColXoa.ReadOnly = true;
            this.ColXoa.Text = "Xóa";
            this.ColXoa.UseColumnTextForButtonValue = true;
            // 
            // panelFooter
            // 
            this.panelFooter.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.panelFooter.BorderThickness = 1;
            this.panelFooter.Controls.Add(this.lblTongTienText);
            this.panelFooter.Controls.Add(this.lblTongTienVal);
            this.panelFooter.Controls.Add(this.btnXacNhan);
            this.panelFooter.Controls.Add(this.btnHuy);
            this.panelFooter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(245)))));
            this.panelFooter.Location = new System.Drawing.Point(0, 764);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(900, 70);
            this.panelFooter.TabIndex = 2;
            // 
            // lblTongTienText
            // 
            this.lblTongTienText.BackColor = System.Drawing.Color.Transparent;
            this.lblTongTienText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTienText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.lblTongTienText.Location = new System.Drawing.Point(24, 20);
            this.lblTongTienText.Name = "lblTongTienText";
            this.lblTongTienText.Size = new System.Drawing.Size(94, 27);
            this.lblTongTienText.TabIndex = 0;
            this.lblTongTienText.Text = "Tổng Tiền:";
            // 
            // lblTongTienVal
            // 
            this.lblTongTienVal.BackColor = System.Drawing.Color.Transparent;
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTongTienVal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(90)))), ((int)(((byte)(20)))));
            this.lblTongTienVal.Location = new System.Drawing.Point(150, 16);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(37, 33);
            this.lblTongTienVal.TabIndex = 1;
            this.lblTongTienVal.Text = "0 đ";
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.BackColor = System.Drawing.Color.Transparent;
            this.btnXacNhan.BorderRadius = 10;
            this.btnXacNhan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.btnXacNhan.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnXacNhan.ForeColor = System.Drawing.Color.White;
            this.btnXacNhan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.Location = new System.Drawing.Point(664, 12);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.ShadowDecoration.BorderRadius = 10;
            this.btnXacNhan.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnXacNhan.ShadowDecoration.Depth = 12;
            this.btnXacNhan.ShadowDecoration.Enabled = true;
            this.btnXacNhan.Size = new System.Drawing.Size(220, 46);
            this.btnXacNhan.TabIndex = 21;
            this.btnXacNhan.Text = "✔ Lưu Phiếu Nhập";
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.Transparent;
            this.btnHuy.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.BorderRadius = 10;
            this.btnHuy.BorderThickness = 2;
            this.btnHuy.FillColor = System.Drawing.Color.Transparent;
            this.btnHuy.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this.btnHuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(37)))), ((int)(((byte)(16)))));
            this.btnHuy.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnHuy.Location = new System.Drawing.Point(488, 14);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(160, 46);
            this.btnHuy.TabIndex = 20;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // inputMaHDN
            // 
            this.inputMaHDN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(245)))));
            this.inputMaHDN.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputMaHDN.BorderRadius = 8;
            this.inputMaHDN.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.inputMaHDN.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputMaHDN.Location = new System.Drawing.Point(24, 44);
            this.inputMaHDN.Name = "inputMaHDN";
            this.inputMaHDN.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputMaHDN.PasswordChar = '\0';
            this.inputMaHDN.ReadOnly = true;
            this.inputMaHDN.Size = new System.Drawing.Size(190, 38);
            this.inputMaHDN.TabIndex = 0;
            // 
            // inputGhiChu
            // 
            this.inputGhiChu.BackColor = System.Drawing.Color.White;
            this.inputGhiChu.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputGhiChu.BorderRadius = 8;
            this.inputGhiChu.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.inputGhiChu.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputGhiChu.Location = new System.Drawing.Point(664, 44);
            this.inputGhiChu.Name = "inputGhiChu";
            this.inputGhiChu.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputGhiChu.PasswordChar = '\0';
            this.inputGhiChu.ReadOnly = false;
            this.inputGhiChu.Size = new System.Drawing.Size(210, 38);
            this.inputGhiChu.TabIndex = 3;
            // 
            // inputSoLuong
            // 
            this.inputSoLuong.BackColor = System.Drawing.Color.White;
            this.inputSoLuong.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputSoLuong.BorderRadius = 8;
            this.inputSoLuong.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.inputSoLuong.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputSoLuong.Location = new System.Drawing.Point(300, 134);
            this.inputSoLuong.Name = "inputSoLuong";
            this.inputSoLuong.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputSoLuong.PasswordChar = '\0';
            this.inputSoLuong.ReadOnly = false;
            this.inputSoLuong.Size = new System.Drawing.Size(130, 38);
            this.inputSoLuong.TabIndex = 5;
            this.inputSoLuong.TextChanged += new System.EventHandler(this.inputDonGia_TextChanged);
            // 
            // inputDonGia
            // 
            this.inputDonGia.BackColor = System.Drawing.Color.White;
            this.inputDonGia.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputDonGia.BorderRadius = 8;
            this.inputDonGia.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.inputDonGia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputDonGia.Location = new System.Drawing.Point(448, 134);
            this.inputDonGia.Name = "inputDonGia";
            this.inputDonGia.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputDonGia.PasswordChar = '\0';
            this.inputDonGia.ReadOnly = false;
            this.inputDonGia.Size = new System.Drawing.Size(160, 38);
            this.inputDonGia.TabIndex = 6;
            this.inputDonGia.TextChanged += new System.EventHandler(this.inputDonGia_TextChanged);
            // 
            // inputLoiNhuan
            // 
            this.inputLoiNhuan.BackColor = System.Drawing.Color.White;
            this.inputLoiNhuan.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.inputLoiNhuan.BorderRadius = 8;
            this.inputLoiNhuan.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.inputLoiNhuan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.inputLoiNhuan.Location = new System.Drawing.Point(24, 218);
            this.inputLoiNhuan.Name = "inputLoiNhuan";
            this.inputLoiNhuan.Padding = new System.Windows.Forms.Padding(10, 6, 10, 6);
            this.inputLoiNhuan.PasswordChar = '\0';
            this.inputLoiNhuan.ReadOnly = false;
            this.inputLoiNhuan.Size = new System.Drawing.Size(120, 38);
            this.inputLoiNhuan.TabIndex = 7;
            // 
            // NhapHangPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(900, 834);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NhapHangPopup";
            this.Text = "NhapHangPopup";
            this.panelBody.ResumeLayout(false);
            this.panelBody.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChiTiet)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        // ── Field declarations ────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private Guna.UI2.WinForms.Guna2Panel panelHeader;
        private Guna.UI2.WinForms.Guna2Panel panelBody;
        private Guna.UI2.WinForms.Guna2Panel panelDivider;
        private Guna.UI2.WinForms.Guna2Panel panelFooter;

        private Guna.UI2.WinForms.Guna2HtmlLabel lblMaHDN;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblNgayNhap;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblMaNCC;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGhiChu;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblChonSP;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblSoLuong;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblDonGia;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblLoiNhuan;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGiaDeXuat;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblGiaDeXuatVal;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblChiTiet;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTongTienText;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblTongTienVal;

        private Bài_Tập_Lớn.UI.RoundedTextBox inputMaHDN;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputGhiChu;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputSoLuong;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputDonGia;
        private Bài_Tập_Lớn.UI.RoundedTextBox inputLoiNhuan;

        private System.Windows.Forms.DateTimePicker dtpNgayNhap;

        private Guna.UI2.WinForms.Guna2ComboBox cboNCC;
        private Guna.UI2.WinForms.Guna2ComboBox cboSanPham;

        private Guna.UI2.WinForms.Guna2Button btnTinhGia;
        private Guna.UI2.WinForms.Guna2Button btnThemVaoGio;
        private Guna.UI2.WinForms.Guna2Button btnXacNhan;
        private Guna.UI2.WinForms.Guna2Button btnHuy;

        private Guna.UI2.WinForms.Guna2DataGridView dgvChiTiet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSTT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTenSP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDonGiaNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColThanhTien;
        private System.Windows.Forms.DataGridViewButtonColumn ColXoa;
    }
}