using System.Drawing;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    partial class MenuSanPham
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutOuter = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutMain = new System.Windows.Forms.TableLayoutPanel();
            this.panelHeader = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblTitle = new Guna.UI2.WinForms.Guna2Button();
            this.panelToolbar = new Guna.UI2.WinForms.Guna2Panel();
            this.btnReload = new Guna.UI2.WinForms.Guna2Button();
            this.btnTimKiem = new Guna.UI2.WinForms.Guna2Button();
            this.spacer1 = new Guna.UI2.WinForms.Guna2Panel();
            this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
            this.panelTabLoc = new Guna.UI2.WinForms.Guna2Panel();
            this.panelCardWrap = new Guna.UI2.WinForms.Guna2Panel();
            this.flowCards = new System.Windows.Forms.FlowLayoutPanel();
            this.panelPager = new Guna.UI2.WinForms.Guna2Panel();
            this.panelRightBar = new Guna.UI2.WinForms.Guna2Panel();
            this.tlRight = new System.Windows.Forms.TableLayoutPanel();
            this.panelRightHeader = new Guna.UI2.WinForms.Guna2CustomGradientPanel();
            this.lblRightTitle = new Guna.UI2.WinForms.Guna2Button();
            this.panelSelectBan = new Guna.UI2.WinForms.Guna2Panel();
            this.cboBan = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblChonBan = new System.Windows.Forms.Label();
            this.panelDonHang = new Guna.UI2.WinForms.Guna2Panel();
            this.panelOrderList = new Guna.UI2.WinForms.Guna2Panel();
            this.flowOrderItems = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDonHangTitle = new System.Windows.Forms.Label();
            this.panelRightFooter = new Guna.UI2.WinForms.Guna2Panel();
            this.btnThanhToan = new Guna.UI2.WinForms.Guna2Button();
            this.lblTongTienVal = new System.Windows.Forms.Label();
            this.lblTongTien = new System.Windows.Forms.Label();
            this.separatorFooter = new System.Windows.Forms.Panel();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this.tableLayoutOuter.SuspendLayout();
            this.tableLayoutMain.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelToolbar.SuspendLayout();
            this.panelCardWrap.SuspendLayout();
            this.panelRightBar.SuspendLayout();
            this.tlRight.SuspendLayout();
            this.panelRightHeader.SuspendLayout();
            this.panelSelectBan.SuspendLayout();
            this.panelDonHang.SuspendLayout();
            this.panelOrderList.SuspendLayout();
            this.panelRightFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutOuter
            // 
            this.tableLayoutOuter.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutOuter.ColumnCount = 2;
            this.tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutOuter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.tableLayoutOuter.Controls.Add(this.tableLayoutMain, 0, 0);
            this.tableLayoutOuter.Controls.Add(this.panelRightBar, 1, 0);
            this.tableLayoutOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutOuter.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutOuter.Name = "tableLayoutOuter";
            this.tableLayoutOuter.RowCount = 1;
            this.tableLayoutOuter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutOuter.Size = new System.Drawing.Size(1260, 885);
            this.tableLayoutOuter.TabIndex = 0;
            this.tableLayoutOuter.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutOuter_Paint);
            // 
            // tableLayoutMain
            // 
            this.tableLayoutMain.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutMain.ColumnCount = 1;
            this.tableLayoutMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.Controls.Add(this.panelHeader, 0, 0);
            this.tableLayoutMain.Controls.Add(this.panelToolbar, 0, 1);
            this.tableLayoutMain.Controls.Add(this.panelTabLoc, 0, 2);
            this.tableLayoutMain.Controls.Add(this.panelCardWrap, 0, 3);
            this.tableLayoutMain.Controls.Add(this.panelPager, 0, 4);
            this.tableLayoutMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutMain.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutMain.Name = "tableLayoutMain";
            this.tableLayoutMain.RowCount = 5;
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutMain.Size = new System.Drawing.Size(994, 879);
            this.tableLayoutMain.TabIndex = 0;
            this.tableLayoutMain.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutMain_Paint);
            // 
            // panelHeader
            // 
            this.panelHeader.BackgroundImage = global::Bài_Tập_Lớn.Properties.Resources.Screenshot_2026_05_27_170816;
            this.panelHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(81)))), ((int)(((byte)(30)))));
            this.panelHeader.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(122)))), ((int)(((byte)(67)))));
            this.panelHeader.FillColor3 = System.Drawing.Color.Transparent;
            this.panelHeader.FillColor4 = System.Drawing.Color.Transparent;
            this.panelHeader.Location = new System.Drawing.Point(3, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(988, 194);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.FillColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(340, 194);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = " Menu Sản Phẩm";
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.btnReload);
            this.panelToolbar.Controls.Add(this.btnTimKiem);
            this.panelToolbar.Controls.Add(this.spacer1);
            this.panelToolbar.Controls.Add(this.txtTimKiem);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelToolbar.Location = new System.Drawing.Point(3, 203);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Padding = new System.Windows.Forms.Padding(0, 20, 12, 9);
            this.panelToolbar.Size = new System.Drawing.Size(988, 66);
            this.panelToolbar.TabIndex = 1;
            this.panelToolbar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelToolbar_Paint);
            // 
            // btnReload
            // 
            this.btnReload.BorderRadius = 10;
            this.btnReload.BorderThickness = 1;
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnReload.FillColor = System.Drawing.Color.Transparent;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnReload.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnReload.Image = global::Bài_Tập_Lớn.Properties.Resources.reload;
            this.btnReload.Location = new System.Drawing.Point(835, 20);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(141, 37);
            this.btnReload.TabIndex = 0;
            this.btnReload.Text = "Tải Lại";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BorderRadius = 10;
            this.btnTimKiem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTimKiem.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnTimKiem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.btnTimKiem.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(421, 20);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(133, 37);
            this.btnTimKiem.TabIndex = 1;
            this.btnTimKiem.Text = "Tìm Kiếm";
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // spacer1
            // 
            this.spacer1.BackColor = System.Drawing.Color.Transparent;
            this.spacer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.spacer1.FillColor = System.Drawing.Color.Transparent;
            this.spacer1.Location = new System.Drawing.Point(408, 20);
            this.spacer1.Name = "spacer1";
            this.spacer1.Size = new System.Drawing.Size(13, 37);
            this.spacer1.TabIndex = 2;
            this.spacer1.Paint += new System.Windows.Forms.PaintEventHandler(this.spacer1_Paint);
            // 
            // txtTimKiem
            // 
            this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtTimKiem.BorderRadius = 10;
            this.txtTimKiem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTimKiem.DefaultText = "";
            this.txtTimKiem.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtTimKiem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtTimKiem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.txtTimKiem.Location = new System.Drawing.Point(0, 20);
            this.txtTimKiem.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtTimKiem.Name = "txtTimKiem";
            this.txtTimKiem.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTimKiem.PlaceholderText = " Tìm tên sản phẩm...";
            this.txtTimKiem.SelectedText = "";
            this.txtTimKiem.Size = new System.Drawing.Size(408, 37);
            this.txtTimKiem.TabIndex = 3;
            this.txtTimKiem.TextChanged += new System.EventHandler(this.txtTimKiem_TextChanged);
            this.txtTimKiem.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTimKiem_KeyDown);
            // 
            // panelTabLoc
            // 
            this.panelTabLoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTabLoc.Location = new System.Drawing.Point(3, 275);
            this.panelTabLoc.Name = "panelTabLoc";
            this.panelTabLoc.Padding = new System.Windows.Forms.Padding(12, 10, 12, 4);
            this.panelTabLoc.Size = new System.Drawing.Size(988, 68);
            this.panelTabLoc.TabIndex = 2;
            this.panelTabLoc.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTabLoc_Paint);
            // 
            // panelCardWrap
            // 
            this.panelCardWrap.Controls.Add(this.flowCards);
            this.panelCardWrap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCardWrap.Location = new System.Drawing.Point(3, 349);
            this.panelCardWrap.Name = "panelCardWrap";
            this.panelCardWrap.Size = new System.Drawing.Size(988, 481);
            this.panelCardWrap.TabIndex = 3;
            this.panelCardWrap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCardWrap_Paint);
            // 
            // flowCards
            // 
            this.flowCards.AutoScroll = true;
            this.flowCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCards.Location = new System.Drawing.Point(0, 0);
            this.flowCards.Name = "flowCards";
            this.flowCards.Padding = new System.Windows.Forms.Padding(10, 8, 10, 8);
            this.flowCards.Size = new System.Drawing.Size(988, 481);
            this.flowCards.TabIndex = 0;
            this.flowCards.Paint += new System.Windows.Forms.PaintEventHandler(this.flowCards_Paint);
            // 
            // panelPager
            // 
            this.panelPager.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPager.Location = new System.Drawing.Point(3, 836);
            this.panelPager.Name = "panelPager";
            this.panelPager.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this.panelPager.Size = new System.Drawing.Size(988, 40);
            this.panelPager.TabIndex = 4;
            this.panelPager.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPager_Paint);
            // 
            // panelRightBar
            // 
            this.panelRightBar.Controls.Add(this.tlRight);
            this.panelRightBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightBar.Location = new System.Drawing.Point(1003, 3);
            this.panelRightBar.Name = "panelRightBar";
            this.panelRightBar.Size = new System.Drawing.Size(254, 879);
            this.panelRightBar.TabIndex = 1;
            this.panelRightBar.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRightBar_Paint);
            // 
            // tlRight
            // 
            this.tlRight.BackColor = System.Drawing.Color.Transparent;
            this.tlRight.ColumnCount = 1;
            this.tlRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlRight.Controls.Add(this.panelRightHeader, 0, 0);
            this.tlRight.Controls.Add(this.panelSelectBan, 0, 1);
            this.tlRight.Controls.Add(this.panelDonHang, 0, 2);
            this.tlRight.Controls.Add(this.panelRightFooter, 0, 3);
            this.tlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlRight.Location = new System.Drawing.Point(0, 0);
            this.tlRight.Name = "tlRight";
            this.tlRight.RowCount = 4;
            this.tlRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 71F));
            this.tlRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 88F));
            this.tlRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlRight.Size = new System.Drawing.Size(254, 879);
            this.tlRight.TabIndex = 0;
            this.tlRight.Paint += new System.Windows.Forms.PaintEventHandler(this.tlRight_Paint);
            // 
            // panelRightHeader
            // 
            this.panelRightHeader.BorderRadius = 10;
            this.panelRightHeader.Controls.Add(this.lblRightTitle);
            this.panelRightHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightHeader.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelRightHeader.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this.panelRightHeader.FillColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(98)))), ((int)(((byte)(46)))));
            this.panelRightHeader.FillColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(174)))), ((int)(((byte)(111)))));
            this.panelRightHeader.Location = new System.Drawing.Point(3, 3);
            this.panelRightHeader.Name = "panelRightHeader";
            this.panelRightHeader.Size = new System.Drawing.Size(248, 65);
            this.panelRightHeader.TabIndex = 0;
            this.panelRightHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRightHeader_Paint);
            // 
            // lblRightTitle
            // 
            this.lblRightTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRightTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblRightTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRightTitle.FillColor = System.Drawing.Color.Transparent;
            this.lblRightTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRightTitle.ForeColor = System.Drawing.Color.White;
            this.lblRightTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRightTitle.Name = "lblRightTitle";
            this.lblRightTitle.Size = new System.Drawing.Size(248, 65);
            this.lblRightTitle.TabIndex = 0;
            this.lblRightTitle.Text = "🧾  Đơn Hàng";
            this.lblRightTitle.Click += new System.EventHandler(this.lblRightTitle_Click);
            // 
            // panelSelectBan
            // 
            this.panelSelectBan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.panelSelectBan.Controls.Add(this.cboBan);
            this.panelSelectBan.Controls.Add(this.lblChonBan);
            this.panelSelectBan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectBan.FillColor = System.Drawing.Color.White;
            this.panelSelectBan.Location = new System.Drawing.Point(3, 74);
            this.panelSelectBan.Name = "panelSelectBan";
            this.panelSelectBan.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelSelectBan.Size = new System.Drawing.Size(248, 82);
            this.panelSelectBan.TabIndex = 1;
            this.panelSelectBan.Paint += new System.Windows.Forms.PaintEventHandler(this.panelSelectBan_Paint);
            // 
            // cboBan
            // 
            this.cboBan.BackColor = System.Drawing.Color.Transparent;
            this.cboBan.BorderRadius = 10;
            this.cboBan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cboBan.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboBan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBan.FocusedColor = System.Drawing.Color.Empty;
            this.cboBan.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.cboBan.ItemHeight = 28;
            this.cboBan.Location = new System.Drawing.Point(4, 35);
            this.cboBan.Name = "cboBan";
            this.cboBan.Size = new System.Drawing.Size(232, 34);
            this.cboBan.TabIndex = 0;
            this.cboBan.SelectedIndexChanged += new System.EventHandler(this.cboBan_SelectedIndexChanged);
            // 
            // lblChonBan
            // 
            this.lblChonBan.AutoSize = true;
            this.lblChonBan.BackColor = System.Drawing.Color.Transparent;
            this.lblChonBan.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblChonBan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblChonBan.Location = new System.Drawing.Point(12, 12);
            this.lblChonBan.Name = "lblChonBan";
            this.lblChonBan.Size = new System.Drawing.Size(79, 20);
            this.lblChonBan.TabIndex = 1;
            this.lblChonBan.Text = "Chọn bàn:";
            this.lblChonBan.Click += new System.EventHandler(this.lblChonBan_Click);
            // 
            // panelDonHang
            // 
            this.panelDonHang.BackColor = System.Drawing.Color.White;
            this.panelDonHang.Controls.Add(this.panelOrderList);
            this.panelDonHang.Controls.Add(this.lblDonHangTitle);
            this.panelDonHang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDonHang.FillColor = System.Drawing.Color.White;
            this.panelDonHang.Location = new System.Drawing.Point(3, 162);
            this.panelDonHang.Name = "panelDonHang";
            this.panelDonHang.Size = new System.Drawing.Size(248, 594);
            this.panelDonHang.TabIndex = 2;
            this.panelDonHang.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDonHang_Paint);
            // 
            // panelOrderList
            // 
            this.panelOrderList.BackColor = System.Drawing.Color.White;
            this.panelOrderList.Controls.Add(this.flowOrderItems);
            this.panelOrderList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOrderList.FillColor = System.Drawing.Color.White;
            this.panelOrderList.Location = new System.Drawing.Point(0, 28);
            this.panelOrderList.Name = "panelOrderList";
            this.panelOrderList.Size = new System.Drawing.Size(248, 566);
            this.panelOrderList.TabIndex = 0;
            this.panelOrderList.Paint += new System.Windows.Forms.PaintEventHandler(this.panelOrderList_Paint);
            // 
            // flowOrderItems
            // 
            this.flowOrderItems.AutoScroll = true;
            this.flowOrderItems.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.flowOrderItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowOrderItems.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowOrderItems.Location = new System.Drawing.Point(0, 0);
            this.flowOrderItems.Name = "flowOrderItems";
            this.flowOrderItems.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.flowOrderItems.Size = new System.Drawing.Size(248, 566);
            this.flowOrderItems.TabIndex = 0;
            this.flowOrderItems.WrapContents = false;
            this.flowOrderItems.Paint += new System.Windows.Forms.PaintEventHandler(this.flowOrderItems_Paint);
            // 
            // lblDonHangTitle
            // 
            this.lblDonHangTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDonHangTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblDonHangTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblDonHangTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDonHangTitle.Name = "lblDonHangTitle";
            this.lblDonHangTitle.Size = new System.Drawing.Size(248, 28);
            this.lblDonHangTitle.TabIndex = 1;
            this.lblDonHangTitle.Text = "  Món đã gọi";
            this.lblDonHangTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDonHangTitle.Click += new System.EventHandler(this.lblDonHangTitle_Click);
            // 
            // panelRightFooter
            // 
            this.panelRightFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.panelRightFooter.Controls.Add(this.btnThanhToan);
            this.panelRightFooter.Controls.Add(this.lblTongTienVal);
            this.panelRightFooter.Controls.Add(this.lblTongTien);
            this.panelRightFooter.Controls.Add(this.separatorFooter);
            this.panelRightFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRightFooter.FillColor = System.Drawing.Color.White;
            this.panelRightFooter.Location = new System.Drawing.Point(3, 762);
            this.panelRightFooter.Name = "panelRightFooter";
            this.panelRightFooter.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelRightFooter.Size = new System.Drawing.Size(248, 114);
            this.panelRightFooter.TabIndex = 3;
            this.panelRightFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelRightFooter_Paint);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BorderRadius = 12;
            this.btnThanhToan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThanhToan.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnThanhToan.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(50)))), ((int)(((byte)(0)))));
            this.btnThanhToan.HoverState.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(12, 48);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(232, 42);
            this.btnThanhToan.TabIndex = 0;
            this.btnThanhToan.Text = "Tổng Tiền";
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // lblTongTienVal
            // 
            this.lblTongTienVal.BackColor = System.Drawing.Color.Transparent;
            this.lblTongTienVal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTongTienVal.ForeColor = System.Drawing.Color.Green;
            this.lblTongTienVal.Location = new System.Drawing.Point(122, 14);
            this.lblTongTienVal.Name = "lblTongTienVal";
            this.lblTongTienVal.Size = new System.Drawing.Size(114, 24);
            this.lblTongTienVal.TabIndex = 1;
            this.lblTongTienVal.Text = "0 ₫";
            this.lblTongTienVal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTongTienVal.Click += new System.EventHandler(this.lblTongTienVal_Click);
            // 
            // lblTongTien
            // 
            this.lblTongTien.BackColor = System.Drawing.Color.Transparent;
            this.lblTongTien.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.lblTongTien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblTongTien.Location = new System.Drawing.Point(12, 14);
            this.lblTongTien.Name = "lblTongTien";
            this.lblTongTien.Size = new System.Drawing.Size(120, 24);
            this.lblTongTien.TabIndex = 2;
            this.lblTongTien.Text = "Tổng tiền SP:";
            this.lblTongTien.Click += new System.EventHandler(this.lblTongTien_Click);
            // 
            // separatorFooter
            // 
            this.separatorFooter.Dock = System.Windows.Forms.DockStyle.Top;
            this.separatorFooter.Location = new System.Drawing.Point(12, 10);
            this.separatorFooter.Name = "separatorFooter";
            this.separatorFooter.Size = new System.Drawing.Size(224, 1);
            this.separatorFooter.TabIndex = 3;
            this.separatorFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.separatorFooter_Paint);
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.BorderRadius = 20;
            this.guna2Elipse1.TargetControl = this;
            // 
            // MenuSanPham
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1280, 905);
            this.Controls.Add(this.tableLayoutOuter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MenuSanPham";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Menu Sản Phẩm";
            this.tableLayoutOuter.ResumeLayout(false);
            this.tableLayoutMain.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelToolbar.ResumeLayout(false);
            this.panelCardWrap.ResumeLayout(false);
            this.panelRightBar.ResumeLayout(false);
            this.tlRight.ResumeLayout(false);
            this.panelRightHeader.ResumeLayout(false);
            this.panelSelectBan.ResumeLayout(false);
            this.panelSelectBan.PerformLayout();
            this.panelDonHang.ResumeLayout(false);
            this.panelOrderList.ResumeLayout(false);
            this.panelRightFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // ── Field declarations ────────────────────────────────────
        private System.Windows.Forms.TableLayoutPanel tableLayoutOuter;
        private System.Windows.Forms.TableLayoutPanel tableLayoutMain;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelHeader;
        private Guna.UI2.WinForms.Guna2Button lblTitle;
        private Guna.UI2.WinForms.Guna2Panel panelToolbar;
        private Guna.UI2.WinForms.Guna2TextBox txtTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnTimKiem;
        private Guna.UI2.WinForms.Guna2Button btnReload;
        private Guna.UI2.WinForms.Guna2Panel spacer1;
        private Guna.UI2.WinForms.Guna2Panel panelTabLoc;
        private Guna.UI2.WinForms.Guna2Panel panelCardWrap;
        private System.Windows.Forms.FlowLayoutPanel flowCards;
        private Guna.UI2.WinForms.Guna2Panel panelPager;
        // Right bar
        private Guna.UI2.WinForms.Guna2Panel panelRightBar;
        private System.Windows.Forms.TableLayoutPanel tlRight;
        private Guna.UI2.WinForms.Guna2CustomGradientPanel panelRightHeader;
        private Guna.UI2.WinForms.Guna2Button lblRightTitle;
        private Guna.UI2.WinForms.Guna2Panel panelSelectBan;
        private System.Windows.Forms.Label lblChonBan;
        private Guna.UI2.WinForms.Guna2ComboBox cboBan;
        private Guna.UI2.WinForms.Guna2Panel panelDonHang;
        private System.Windows.Forms.Label lblDonHangTitle;
        private Guna.UI2.WinForms.Guna2Panel panelOrderList;
        private System.Windows.Forms.FlowLayoutPanel flowOrderItems;
        private Guna.UI2.WinForms.Guna2Panel panelRightFooter;
        private System.Windows.Forms.Panel separatorFooter;
        private System.Windows.Forms.Label lblTongTien;
        private System.Windows.Forms.Label lblTongTienVal;
        private Guna.UI2.WinForms.Guna2Button btnThanhToan;
        private Guna.UI2.WinForms.Guna2Elipse guna2Elipse1;
    }
}