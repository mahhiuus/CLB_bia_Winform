using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Bài_Tập_Lớn.GUI
{
    partial class MenuSanPhamCard
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._panelAnh = new System.Windows.Forms.Panel();
            this._picAnh = new System.Windows.Forms.PictureBox();
            this._lblLoai = new System.Windows.Forms.Label();
            this._lblTen = new System.Windows.Forms.Label();
            this._lblGia = new System.Windows.Forms.Label();
            this._lblSoLuong = new System.Windows.Forms.Label();
            this._btnThem = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Elipse1 = new Guna.UI2.WinForms.Guna2Elipse(this.components);
            this._panelAnh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._picAnh)).BeginInit();
            this.SuspendLayout();
            // 
            // _panelAnh
            // 
            this._panelAnh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(228)))), ((int)(((byte)(225)))));
            this._panelAnh.Controls.Add(this._picAnh);
            this._panelAnh.Location = new System.Drawing.Point(12, 12);
            this._panelAnh.Name = "_panelAnh";
            this._panelAnh.Size = new System.Drawing.Size(221, 128);
            this._panelAnh.TabIndex = 0;
            // 
            // _picAnh
            // 
            this._picAnh.BackColor = System.Drawing.Color.Transparent;
            this._picAnh.Dock = System.Windows.Forms.DockStyle.Fill;
            this._picAnh.Location = new System.Drawing.Point(0, 0);
            this._picAnh.Name = "_picAnh";
            this._picAnh.Size = new System.Drawing.Size(221, 128);
            this._picAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._picAnh.TabIndex = 0;
            this._picAnh.TabStop = false;
            // 
            // _lblLoai
            // 
            this._lblLoai.BackColor = System.Drawing.Color.Transparent;
            this._lblLoai.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold);
            this._lblLoai.ForeColor = System.Drawing.Color.White;
            this._lblLoai.Location = new System.Drawing.Point(12, 148);
            this._lblLoai.Name = "_lblLoai";
            this._lblLoai.Size = new System.Drawing.Size(88, 20);
            this._lblLoai.TabIndex = 1;
            this._lblLoai.Text = "Đồ ăn";
            this._lblLoai.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lblTen
            // 
            this._lblTen.AutoEllipsis = true;
            this._lblTen.BackColor = System.Drawing.Color.Transparent;
            this._lblTen.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this._lblTen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this._lblTen.Location = new System.Drawing.Point(12, 174);
            this._lblTen.Name = "_lblTen";
            this._lblTen.Size = new System.Drawing.Size(191, 36);
            this._lblTen.TabIndex = 2;
            this._lblTen.Text = "Tên sản phẩm";
            // 
            // _lblGia
            // 
            this._lblGia.BackColor = System.Drawing.Color.Transparent;
            this._lblGia.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Bold);
            this._lblGia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this._lblGia.Location = new System.Drawing.Point(11, 206);
            this._lblGia.Name = "_lblGia";
            this._lblGia.Size = new System.Drawing.Size(102, 22);
            this._lblGia.TabIndex = 3;
            this._lblGia.Text = "0 ₫";
            this._lblGia.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lblGia.Click += new System.EventHandler(this._lblGia_Click);
            // 
            // _lblSoLuong
            // 
            this._lblSoLuong.BackColor = System.Drawing.Color.Transparent;
            this._lblSoLuong.Font = new System.Drawing.Font("Segoe UI", 8F);
            this._lblSoLuong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(130)))), ((int)(((byte)(55)))));
            this._lblSoLuong.Location = new System.Drawing.Point(12, 241);
            this._lblSoLuong.Name = "_lblSoLuong";
            this._lblSoLuong.Size = new System.Drawing.Size(140, 18);
            this._lblSoLuong.TabIndex = 4;
            this._lblSoLuong.Text = "Còn: 0";
            this._lblSoLuong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _btnThem
            // 
            this._btnThem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(65)))), ((int)(((byte)(28)))));
            this._btnThem.BorderRadius = 12;
            this._btnThem.BorderThickness = 1;
            this._btnThem.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btnThem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(78)))), ((int)(((byte)(35)))));
            this._btnThem.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this._btnThem.ForeColor = System.Drawing.Color.White;
            this._btnThem.Image = global::Bài_Tập_Lớn.Properties.Resources.plus;
            this._btnThem.Location = new System.Drawing.Point(185, 208);
            this._btnThem.Name = "_btnThem";
            this._btnThem.Size = new System.Drawing.Size(48, 48);
            this._btnThem.TabIndex = 5;
            this._btnThem.Click += new System.EventHandler(this.BtnThem_Click);
            // 
            // guna2Elipse1
            // 
            this.guna2Elipse1.TargetControl = this;
            // 
            // MenuSanPhamCard
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(251)))));
            this.Controls.Add(this._btnThem);
            this.Controls.Add(this._lblSoLuong);
            this.Controls.Add(this._lblGia);
            this.Controls.Add(this._lblTen);
            this.Controls.Add(this._lblLoai);
            this.Controls.Add(this._panelAnh);
            this.Margin = new System.Windows.Forms.Padding(8);
            this.Name = "MenuSanPhamCard";
            this.Size = new System.Drawing.Size(249, 301);
            this._panelAnh.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._picAnh)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna2Elipse guna2Elipse1;
    }
}