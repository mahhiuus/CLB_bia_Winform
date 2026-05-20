using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        private void DesignUI()
        {
           
        }
        // ===== HÀM BO GÓC =====
        private void BoGoc(Control ctrl, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();

            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddArc(new Rectangle(ctrl.Width - radius, 0, radius, radius), 270, 90);
            path.AddArc(new Rectangle(ctrl.Width - radius, ctrl.Height - radius, radius, radius), 0, 90);
            path.AddArc(new Rectangle(0, ctrl.Height - radius, radius, radius), 90, 90);

            path.CloseFigure();

            ctrl.Region = new Region(path);
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
