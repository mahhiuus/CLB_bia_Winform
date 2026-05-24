using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.UI
{
    public partial class ConfirmDeleteUI : Form
    {
        static readonly Color GREEN_DARK = ColorTranslator.FromHtml("#2b4e23");
        static readonly Color CREAM = Color.FromArgb(255, 255, 251);
        static readonly Color DANGER = Color.FromArgb(192, 57, 43);

        public ConfirmDeleteUI(string tenNV)
        {
            BuildUI(tenNV);
        }

        private void BuildUI(string tenNV)
        {
            this.Size = new Size(400, 210);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = CREAM;

            using (var path = GraphicsHelper.GetRoundedPath(new Rectangle(0, 0, Width, Height), 14))
                this.Region = new Region(path);

            this.MouseDown += DoDrag;

            this.Controls.Add(new Label { Text = "🗑️", Font = new Font("Segoe UI", 26f), AutoSize = true, Location = new Point(172, 18) });
            this.Controls.Add(new Label { Text = $"Bạn có chắc muốn xóa\n\"{tenNV}\" không?", Font = new Font("Segoe UI", 10f), TextAlign = ContentAlignment.MiddleCenter, Size = new Size(350, 50), Location = new Point(24, 90) });

            RoundedButton btnNo = new RoundedButton { Text = "Huỷ", Size = new Size(106, 36), Location = new Point(168, 154), BackColor = Color.White, ForeColor = Color.FromArgb(80, 80, 80), Font = new Font("Segoe UI Semibold", 9.5f) };
            btnNo.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            RoundedButton btnYes = new RoundedButton { Text = "🗑 Xóa", Size = new Size(100, 36), Location = new Point(280, 154), BackColor = DANGER, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 9.5f) };
            btnYes.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };

            this.Controls.AddRange(new Control[] { btnNo, btnYes });
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(DANGER, 2)) e.Graphics.DrawPath(pen, GraphicsHelper.GetRoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), 14));
        }

        private void DoDrag(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { NativeDrag.ReleaseCapture(); NativeDrag.SendMessage(Handle, 0xA1, 0x2, 0); }
        }
    }

    internal static class NativeDrag
    {
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}