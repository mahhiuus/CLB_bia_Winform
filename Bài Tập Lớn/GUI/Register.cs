using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices; // [THÊM MỚI] Để gọi thư viện Windows API
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class RegisterUI : Form
    {
        // [THÊM MỚI] Gọi hàm API của Windows để kiểm tra mức độ DPI
        [DllImport("shcore.dll")]
        private static extern int GetProcessDpiAwareness(IntPtr hprocess, out int awareness);

        public RegisterUI()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Kiểm tra trạng thái Manifest khi Form vừa load

        }

        // Các hàm ẩn phía dưới giữ nguyên không thay đổi...
        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2CustomGradientPanel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2HtmlLabel1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }

        private void guna2CustomGradientPanel1_Paint(object sender, PaintEventArgs e)
        {
            // Bật thuật toán khử răng cưa và làm mịn ảnh tối đa
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }
    }
}