using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.UI
{
    public static class GraphicsHelper
    {
        public static GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int curve = radius * 2;
            path.AddArc(rect.X, rect.Y, curve, curve, 180, 90);
            path.AddArc(rect.Right - curve, rect.Y, curve, curve, 270, 90);
            path.AddArc(rect.Right - curve, rect.Bottom - curve, curve, curve, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curve, curve, curve, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    public class RoundedButton : Button
    {
        public int BorderRadius { get; set; } = 8;
        public Color HoverColor { get; set; } = Color.Empty;
        private bool _isHovering = false;

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.White;
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            using (GraphicsPath path = GraphicsHelper.GetRoundedPath(new Rectangle(0, 0, Width - 1, Height - 1), BorderRadius))
            {
                this.Region = new Region(path);
                Color fill = (_isHovering && HoverColor != Color.Empty) ? HoverColor : BackColor;
                using (SolidBrush brush = new SolidBrush(fill))
                {
                    pevent.Graphics.FillPath(brush, path);
                }
                TextRenderer.DrawText(pevent.Graphics, Text, Font, rect, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
    }

    public class RoundedTextBox : UserControl
    {
        private readonly TextBox _txt;

        public int BorderRadius { get; set; } = 8;
        public Color BorderColor { get; set; } = Color.LightGray;
        public Color FocusColor { get; set; } = Color.Green;
        private bool _isFocused = false;

        public override string Text
        {
            get => _txt.Text;
            set => _txt.Text = value;
        }

        public bool ReadOnly
        {
            get => _txt.ReadOnly;
            set => _txt.ReadOnly = value;
        }

        public char PasswordChar
        {
            get => _txt.PasswordChar;
            set => _txt.PasswordChar = value;
        }

        public event KeyEventHandler TextBoxKeyDown;

        public RoundedTextBox()
        {
            _txt = new TextBox();
            Padding = new Padding(10, 8, 10, 8);
            BackColor = Color.White;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            _txt.BorderStyle = BorderStyle.None;
            _txt.Dock = DockStyle.Fill;
            _txt.Font = new Font("Segoe UI", 9.5f);
            _txt.Enter += (s, e) => { _isFocused = true; Invalidate(); };
            _txt.Leave += (s, e) => { _isFocused = false; Invalidate(); };
            _txt.KeyDown += (s, e) => TextBoxKeyDown?.Invoke(s, e);

            Controls.Add(_txt);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath path = GraphicsHelper.GetRoundedPath(
                new Rectangle(0, 0, Width - 1, Height - 1), BorderRadius))
            {
                Color color = _isFocused ? FocusColor : BorderColor;
                using (Pen pen = new Pen(color, 1.5f))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _txt?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
