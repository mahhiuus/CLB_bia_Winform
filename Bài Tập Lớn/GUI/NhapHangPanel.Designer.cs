using System.Drawing;

namespace Bài_Tập_Lớn.GUI
{
    partial class NhapHangPanel
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlGradientHeader = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.btnTieuDe = new Guna.UI2.WinForms.Guna2Button();
            this.pnlToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnTimKiem = new Guna.UI2.WinForms.Guna2Button();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnTaoPhieu = new Guna.UI2.WinForms.Guna2Button();
            this.dgvPhieuNhap = new Guna.UI2.WinForms.Guna2DataGridView();
            this.ColMaHDN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMaNCC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMaNV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNgayNhap = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTongTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColXemCT = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ColXoa = new System.Windows.Forms.DataGridViewButtonColumn();
            this.pnlPager = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2Panel4 = new Guna.UI2.WinForms.Guna2Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlGradientHeader.SuspendLayout();
            this.pnlToolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).BeginInit();
            this.SuspendLayout();
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.pnlGradientHeader, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlToolbar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvPhieuNhap, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.pnlPager, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 4);
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1280, 905);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlGradientHeader
            // 
            this.pnlGradientHeader.BorderRadius = 10;
            this.pnlGradientHeader.Controls.Add(this.btnTieuDe);
            this.pnlGradientHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGradientHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.pnlGradientHeader.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(140)))), ((int)(((byte)(68)))));
            this.pnlGradientHeader.FillColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.pnlGradientHeader.FillColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(140)))), ((int)(((byte)(68)))));
            this.pnlGradientHeader.Location = new System.Drawing.Point(13, 10);
            this.pnlGradientHeader.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
            this.pnlGradientHeader.Name = "pnlGradientHeader";
            this.pnlGradientHeader.Size = new System.Drawing.Size(1254, 60);
            this.pnlGradientHeader.TabIndex = 0;
            // 
            // btnTieuDe
            // 
            this.btnTieuDe.BackColor = System.Drawing.Color.Transparent;
            this.btnTieuDe.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTieuDe.FillColor = System.Drawing.Color.Transparent;
            this.btnTieuDe.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnTieuDe.ForeColor = System.Drawing.Color.White;
            this.btnTieuDe.Location = new System.Drawing.Point(0, 0);
            this.btnTieuDe.Name = "btnTieuDe";
            this.btnTieuDe.Size = new System.Drawing.Size(340, 60);
            this.btnTieuDe.TabIndex = 0;
            this.btnTieuDe.Text = "📦  Quản Lý Nhập Hàng";
            // 
            // pnlToolbar
            // 
            this.pnlToolbar.BackColor = System.Drawing.Color.Transparent;
            this.pnlToolbar.Controls.Add(this.btnTimKiem);
            this.pnlToolbar.Controls.Add(this.guna2Panel1);
            this.pnlToolbar.Controls.Add(this.btnReload);
            this.pnlToolbar.Controls.Add(this.btnTaoPhieu);
            this.pnlToolbar.Controls.Add(this.guna2Panel4);
            this.pnlToolbar.Controls.Add(this.txtTimKiem);
            this.pnlToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToolbar.Location = new System.Drawing.Point(13, 83);
            this.pnlToolbar.Name = "pnlToolbar";
            this.pnlToolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlToolbar.Size = new System.Drawing.Size(1254, 48);
            this.pnlToolbar.TabIndex = 1;
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.txtTimKiem.BorderRadius = 8;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtTimKiem.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 8);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderText = "Tìm mã phiếu, mã NCC, mã NV...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(320, 32);
            this.txtTimKiem.TabIndex = 0;
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.Transparent;
            this.btnTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTimKiem.BorderRadius = 8;
            this.btnTimKiem.BorderThickness = 1;
            this.btnTimKiem.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTimKiem.FillColor = System.Drawing.Color.White;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTimKiem.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.btnTimKiem.Image = global::Bài_Tập_Lớn.Properties.Resources.search;
            this.btnTimKiem.ImageSize = new System.Drawing.Size(15, 15);
            this.btnTimKiem.Location = new System.Drawing.Point(450, 8);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(110, 32);
            this.btnTimKiem.TabIndex = 1;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnReload
            // 
            this.btnReload.BackColor = System.Drawing.Color.Transparent;
            this.btnReload.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnReload.BorderRadius = 8;
            this.btnReload.BorderThickness = 1;
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnReload.FillColor = System.Drawing.Color.White;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnReload.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnReload.Image = global::Bài_Tập_Lớn.Properties.Resources.reload;
            this.btnReload.ImageSize = new System.Drawing.Size(15, 15);
            this.btnReload.Location = new System.Drawing.Point(330, 8);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(110, 32);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = " Tải Lại";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnTaoPhieu
            // 
            this.btnTaoPhieu.BackColor = System.Drawing.Color.Transparent;
            this.btnTaoPhieu.BorderRadius = 10;
            this.btnTaoPhieu.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTaoPhieu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTaoPhieu.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTaoPhieu.ForeColor = System.Drawing.Color.White;
            this.btnTaoPhieu.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(140)))), ((int)(((byte)(68)))));
            this.btnTaoPhieu.Image = global::Bài_Tập_Lớn.Properties.Resources.plus1;
            this.btnTaoPhieu.Location = new System.Drawing.Point(1004, 8);
            this.btnTaoPhieu.Name = "btnTaoPhieu";
            this.btnTaoPhieu.ShadowDecoration.Color = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.btnTaoPhieu.ShadowDecoration.Depth = 10;
            this.btnTaoPhieu.ShadowDecoration.Enabled = true;
            this.btnTaoPhieu.Size = new System.Drawing.Size(250, 32);
            this.btnTaoPhieu.TabIndex = 3;
            this.btnTaoPhieu.Text = "Tạo Phiếu Nhập Hàng";
            this.btnTaoPhieu.Click += new System.EventHandler(this.btnTaoPhieu_Click);
            // 
            // dgvPhieuNhap
            // 
            this.dgvPhieuNhap.AllowUserToAddRows = false;
            this.dgvPhieuNhap.AllowUserToDeleteRows = false;
            this.dgvPhieuNhap.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.dgvPhieuNhap.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle11;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPhieuNhap.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvPhieuNhap.ColumnHeadersHeight = 44;
            this.dgvPhieuNhap.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColMaHDN,
            this.ColMaNCC,
            this.ColMaNV,
            this.ColNgayNhap,
            this.ColTongTien,
            this.ColGhiChu,
            this.ColXemCT,
            this.ColXoa});
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Segoe UI", 10F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhieuNhap.DefaultCellStyle = dataGridViewCellStyle14;
            this.dgvPhieuNhap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPhieuNhap.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.dgvPhieuNhap.Location = new System.Drawing.Point(20, 144);
            this.dgvPhieuNhap.Margin = new System.Windows.Forms.Padding(10);
            this.dgvPhieuNhap.MultiSelect = false;
            this.dgvPhieuNhap.Name = "dgvPhieuNhap";
            this.dgvPhieuNhap.ReadOnly = true;
            this.dgvPhieuNhap.RowHeadersVisible = false;
            this.dgvPhieuNhap.RowHeadersWidth = 51;
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.dgvPhieuNhap.RowsDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvPhieuNhap.RowTemplate.Height = 42;
            this.dgvPhieuNhap.Size = new System.Drawing.Size(1240, 693);
            this.dgvPhieuNhap.TabIndex = 2;
            this.dgvPhieuNhap.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(252)))), ((int)(((byte)(247)))));
            this.dgvPhieuNhap.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvPhieuNhap.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.dgvPhieuNhap.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvPhieuNhap.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvPhieuNhap.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvPhieuNhap.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(220)))), ((int)(((byte)(210)))));
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPhieuNhap.ThemeStyle.HeaderStyle.Height = 44;
            this.dgvPhieuNhap.ThemeStyle.ReadOnly = true;
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.Height = 42;
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(223)))), ((int)(((byte)(194)))));
            this.dgvPhieuNhap.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.dgvPhieuNhap.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhieuNhap_CellContentClick);
            this.dgvPhieuNhap.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPhieuNhap_CellFormatting);
            this.dgvPhieuNhap.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhieuNhap_CellMouseEnter);
            this.dgvPhieuNhap.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhieuNhap_CellMouseLeave);
            // 
            // ColMaHDN
            // 
            this.ColMaHDN.DataPropertyName = "MaHDN";
            this.ColMaHDN.HeaderText = "Mã Phiếu";
            this.ColMaHDN.MinimumWidth = 6;
            this.ColMaHDN.Name = "ColMaHDN";
            this.ColMaHDN.ReadOnly = true;
            // 
            // ColMaNCC
            // 
            this.ColMaNCC.DataPropertyName = "MaNCC";
            this.ColMaNCC.HeaderText = "Mã NCC";
            this.ColMaNCC.MinimumWidth = 6;
            this.ColMaNCC.Name = "ColMaNCC";
            this.ColMaNCC.ReadOnly = true;
            // 
            // ColMaNV
            // 
            this.ColMaNV.DataPropertyName = "MaNV";
            this.ColMaNV.HeaderText = "Mã NV";
            this.ColMaNV.MinimumWidth = 6;
            this.ColMaNV.Name = "ColMaNV";
            this.ColMaNV.ReadOnly = true;
            // 
            // ColNgayNhap
            // 
            this.ColNgayNhap.DataPropertyName = "NgayNhap";
            this.ColNgayNhap.HeaderText = "Ngày Nhập";
            this.ColNgayNhap.MinimumWidth = 6;
            this.ColNgayNhap.Name = "ColNgayNhap";
            this.ColNgayNhap.ReadOnly = true;
            // 
            // ColTongTien
            // 
            this.ColTongTien.DataPropertyName = "TongTien";
            this.ColTongTien.HeaderText = "Tổng Tiền";
            this.ColTongTien.MinimumWidth = 6;
            this.ColTongTien.Name = "ColTongTien";
            this.ColTongTien.ReadOnly = true;
            // 
            // ColGhiChu
            // 
            this.ColGhiChu.DataPropertyName = "GhiChu";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.ColGhiChu.DefaultCellStyle = dataGridViewCellStyle13;
            this.ColGhiChu.HeaderText = "Ghi Chú";
            this.ColGhiChu.MinimumWidth = 6;
            this.ColGhiChu.Name = "ColGhiChu";
            this.ColGhiChu.ReadOnly = true;
            // 
            // ColXemCT
            // 
            this.ColXemCT.HeaderText = "";
            this.ColXemCT.MinimumWidth = 6;
            this.ColXemCT.Name = "ColXemCT";
            this.ColXemCT.ReadOnly = true;
            this.ColXemCT.Text = "Xem CT";
            this.ColXemCT.UseColumnTextForButtonValue = true;
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
            // pnlPager
            // 
            this.pnlPager.BackColor = System.Drawing.Color.Transparent;
            this.pnlPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPager.Location = new System.Drawing.Point(13, 850);
            this.pnlPager.Name = "pnlPager";
            this.pnlPager.Size = new System.Drawing.Size(1254, 48);
            this.pnlPager.TabIndex = 3;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel1.Location = new System.Drawing.Point(440, 8);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(10, 32);
            this.guna2Panel1.TabIndex = 7;
            // 
            // guna2Panel4
            // 
            this.guna2Panel4.BackColor = System.Drawing.Color.Transparent;
            this.guna2Panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.guna2Panel4.Location = new System.Drawing.Point(320, 8);
            this.guna2Panel4.Name = "guna2Panel4";
            this.guna2Panel4.Size = new System.Drawing.Size(10, 32);
            this.guna2Panel4.TabIndex = 8;
            // 
            // NhapHangPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1280, 905);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "NhapHangPanel";
            this.Text = "NhapHangPanel";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlGradientHeader.ResumeLayout(false);
            this.pnlToolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhieuNhap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // ── Field declarations ────────────────────────────────────
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel pnlGradientHeader;
        private Guna.UI2.WinForms.Guna2Button btnTieuDe;
        private Guna.UI2.WinForms.Guna2Panel pnlToolbar;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Button btnTaoPhieu;
        private Guna.UI2.WinForms.Guna2DataGridView dgvPhieuNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaHDN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaNCC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMaNV;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNgayNhap;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTongTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColGhiChu;
        private System.Windows.Forms.DataGridViewButtonColumn ColXemCT;
        private System.Windows.Forms.DataGridViewButtonColumn ColXoa;
        private Guna.UI2.WinForms.Guna2Panel pnlPager;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel4;
    }
}