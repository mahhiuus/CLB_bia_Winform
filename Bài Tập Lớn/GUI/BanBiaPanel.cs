using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;

namespace Bài_Tập_Lớn.GUI
{
    public partial class BanBiaPanel : Form
    {
        // ══════════════════════════════════════════════════════════
        //  Fields
        // ══════════════════════════════════════════════════════════
        private readonly BanBidaBLL _bll = new BanBidaBLL();

        /// <summary>ComboBox lọc theo Loại bàn — thêm động vào toolbar.</summary>
        private ComboBox cboLoaiBanFilter;

        // ══════════════════════════════════════════════════════════
        //  Khởi tạo
        // ══════════════════════════════════════════════════════════
        public BanBiaPanel()
        {
            InitializeComponent();
            ThemComboLocLoaiBan();
            CauHinhGrid();
            TaiDanhSach();
        }

        // ══════════════════════════════════════════════════════════
        //  Thiết lập ban đầu
        // ══════════════════════════════════════════════════════════

        /// <summary>Thêm ComboBox lọc loại bàn vào toolbar (guna2Panel2).</summary>
        private void ThemComboLocLoaiBan()
        {
            cboLoaiBanFilter = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                Width = 160,
                Dock = DockStyle.Left
            };
            cboLoaiBanFilter.Items.AddRange(new string[]
            {
                "-- Tất cả loại --",
                "Bàn Thường",
                "Bàn VIP",
                "Bàn Snooker"
            });
            cboLoaiBanFilter.SelectedIndex = 0;
            guna2Panel2.Controls.Add(cboLoaiBanFilter);
        }

        /// <summary>Cấu hình cột DataGridView map với DTO.</summary>
        private void CauHinhGrid()
        {
            guna2DataGridView1.Enabled = true;
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.AutoGenerateColumns = false;

            // Map cột với property DTO
            Column1.DataPropertyName = "MaBan";
            Column2.DataPropertyName = "TenBan";
            Column3.DataPropertyName = "LoaiBan";
            Column4.DataPropertyName = "GiaTheoGio";
            Column5.DataPropertyName = "TrangThai";

            // Độ rộng cột
            Column1.Width = 90;
            Column2.Width = 200;
            Column3.Width = 150;
            Column4.Width = 170;
            Column5.Width = 130;
            Column7.Width = 110;

            // Format giá tiền
            Column4.DefaultCellStyle.Format = "N0";
            Column4.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Cập nhật text nút Thêm
            btnThem.Text = "  ✚  Thêm Bàn Mới";

            // Đổi guna2Button1 thành nút Làm Mới
            guna2Button1.Text = "🔄  Làm Mới";
        }

        // ══════════════════════════════════════════════════════════
        //  Load & Hiển thị dữ liệu
        // ══════════════════════════════════════════════════════════
        private void TaiDanhSach()
        {
            try
            {
                var ds = _bll.LayTatCaBan();
                HienThiGrid(ds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiGrid(List<BanBidaDTO> ds)
        {
            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = ds;

            // Đặt text nút Xóa cho ButtonColumn
            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                if (row.Cells["Column7"] is DataGridViewButtonCell btn)
                    btn.Value = "🗑 Xóa";
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện — Thêm
        // ══════════════════════════════════════════════════════════
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (var popup = new BanBiaPopupUi())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                if (popup.ShowDialog(this) == DialogResult.OK)
                    TaiDanhSach();
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện — Tìm kiếm
        // ══════════════════════════════════════════════════════════
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            try
            {
                string loai = cboLoaiBanFilter.SelectedIndex == 0
                    ? "" : cboLoaiBanFilter.SelectedItem?.ToString();

                List<BanBidaDTO> ds = string.IsNullOrEmpty(loai)
                    ? _bll.LayTatCaBan()
                    : _bll.TimTheoLoaiBan(loai);

                // Lọc thêm theo từ khoá inputTimKiem
                string tuKhoa = inputTimKiem.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(tuKhoa))
                    ds = ds.Where(b =>
                        b.MaBan.ToLower().Contains(tuKhoa) ||
                        b.TenBan.ToLower().Contains(tuKhoa)).ToList();

                HienThiGrid(ds);

                if (ds.Count == 0)
                    MessageBox.Show("Không tìm thấy bàn nào phù hợp.", "Kết quả",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tìm kiếm",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện — Click dòng trong Grid
        // ══════════════════════════════════════════════════════════
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Nút Xóa (Column7)
            if (e.ColumnIndex == guna2DataGridView1.Columns["Column7"].Index)
            {
                XuLyXoa(e.RowIndex);
                return;
            }

            // Click dòng bất kỳ → mở popup Sửa
            MoPopupSua(e.RowIndex);
        }

        private void MoPopupSua(int rowIndex)
        {
            string maBan = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            if (string.IsNullOrEmpty(maBan)) return;

            BanBidaDTO ban = _bll.TimTheoMaBan(maBan);
            if (ban == null) return;

            using (var popup = new BanBiaPopupUi(ban))
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                if (popup.ShowDialog(this) == DialogResult.OK)
                    TaiDanhSach();
            }
        }

        private void XuLyXoa(int rowIndex)
        {
            string maBan = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            string tenBan = guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value?.ToString();
            if (string.IsNullOrEmpty(maBan)) return;

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa bàn \"{tenBan}\" ({maBan}) không?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (_bll.XoaBan(maBan))
                {
                    MessageBox.Show("Xóa bàn thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSach();
                }
                else
                {
                    MessageBox.Show("Xóa không thành công!", "Thất bại",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ══════════════════════════════════════════════════════════
        //  Sự kiện — Làm Mới (guna2Button1)
        // ══════════════════════════════════════════════════════════
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            inputTimKiem.Text = "";
            cboLoaiBanFilter.SelectedIndex = 0;
            TaiDanhSach();
        }

        // ── Event stubs giữ nguyên (Designer yêu cầu) ────────────
        private void inputTimKiem_Load(object sender, EventArgs e) { }
        private void MainHeader_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}