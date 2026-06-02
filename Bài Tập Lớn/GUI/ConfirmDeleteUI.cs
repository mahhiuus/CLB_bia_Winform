using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Bài_Tập_Lớn.UI
{
    /// <summary>
    /// Dialog xác nhận xóa — phong cách nhất quán với NhapHangPopup (Guna UI2).
    /// Trả về DialogResult.OK nếu người dùng xác nhận xóa,
    /// DialogResult.Cancel nếu hủy.
    /// </summary>
    public partial class ConfirmDeleteUI : Form
    {
        // ── Constructors ────────────────────────────────────────────
        /// <summary>Chỉ truyền tên đối tượng cần xóa.</summary>
        public ConfirmDeleteUI(string tenDoiTuong)
            : this(tenDoiTuong, string.Empty) { }

        /// <summary>Truyền tên và loại đối tượng cần xóa.</summary>
        public ConfirmDeleteUI(string tenDoiTuong, string loaiDoiTuong)
        {
            InitializeComponent();

            string dongMsg = string.IsNullOrWhiteSpace(loaiDoiTuong)
                ? $"Bạn có chắc muốn xóa\n\"{tenDoiTuong}\" không?"
                : $"Bạn có chắc muốn xóa {loaiDoiTuong}\n\"{tenDoiTuong}\" không?";

            lblMsg.Text = dongMsg;
        }

        // ── Event handlers ──────────────────────────────────────────
        private void btnHuy_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Drag form (không có title bar) ──────────────────────────
        private void panelHeader_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                NativeDrag.ReleaseCapture();
                NativeDrag.SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }
    }

    // ── P/Invoke helper ─────────────────────────────────────────────
    internal static class NativeDrag
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}