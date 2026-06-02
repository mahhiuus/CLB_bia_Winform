using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DTO;
using Bài_Tập_Lớn.UI;
using Guna.UI2.WinForms;

namespace Bài_Tập_Lớn.GUI
{
    public partial class BanBiaPanel : Form
    {
        private readonly BanBidaBLL _bll = new BanBidaBLL();

        // Event bắn ra mỗi khi thêm / sửa / xóa bàn thành công
        // Maindashboard subscribe để gọi SoDoBanUi.RefreshMap()
        public event EventHandler BanDuocThemHoacXoa;

        private bool _dangKhoiTao = true;
        private List<BanBidaDTO> _dsDayDu = new List<BanBidaDTO>();
        private int _trangHienTai = 1;
        private const int _soDoiMoiTrang = 10;
        private int _tongSoTrang => (int)Math.Ceiling((double)_dsDayDu.Count / _soDoiMoiTrang);
        private Guna2Button _btnPrev;
        private Guna2Button _btnNext;
        private Label _lblTrangInfo;

        public BanBiaPanel()
        {
            InitializeComponent();
            _dangKhoiTao = true;
            CauHinhGrid();
            TaoPhanTrang();
            _dangKhoiTao = false;
            TaiDanhSach();

            // Hủy đăng ký sự kiện trước khi đăng ký để tránh double fire
            guna2DataGridView1.CellFormatting -= guna2DataGridView1_CellFormatting;
            guna2DataGridView1.CellFormatting += guna2DataGridView1_CellFormatting;

            guna2DataGridView1.CellMouseEnter -= guna2DataGridView1_CellMouseEnter;
            guna2DataGridView1.CellMouseEnter += guna2DataGridView1_CellMouseEnter;

            guna2DataGridView1.CellMouseLeave -= guna2DataGridView1_CellMouseLeave;
            guna2DataGridView1.CellMouseLeave += guna2DataGridView1_CellMouseLeave;

            guna2DataGridView1.CellMouseDown -= guna2DataGridView1_CellMouseDown;
            guna2DataGridView1.CellMouseDown += guna2DataGridView1_CellMouseDown;

            guna2DataGridView1.CellContentClick -= guna2DataGridView1_CellContentClick;
            guna2DataGridView1.CellContentClick += guna2DataGridView1_CellContentClick;

            this.Load += (s, e) => ApDungBoTron();
            guna2DataGridView1.Resize += (s, e) => ApDungBoTron();
        }

        private void TaoPhanTrang()
        {
            Color clrBtnNormal = Color.FromArgb(200, 200, 200);
            Color clrBtnHover = Color.FromArgb(170, 170, 170);
            Color clrText = Color.FromArgb(43, 78, 35);

            _btnPrev = new Guna2Button
            {
                Text = "<",
                Size = new Size(32, 28),
                Location = new Point(6, 3),
                BorderRadius = 6,
                FillColor = clrBtnNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnPrev.HoverState.FillColor = clrBtnHover;
            _btnPrev.HoverState.ForeColor = Color.FromArgb(50, 50, 50);
            _btnPrev.Click += (s, e) => ChuyenTrang(_trangHienTai - 1);

            _lblTrangInfo = new Label
            {
                Text = "Trang 1 / 1",
                Size = new Size(110, 28),
                Location = new Point(44, 3),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                ForeColor = clrText,
                BackColor = Color.Transparent,
            };

            _btnNext = new Guna2Button
            {
                Text = ">",
                Size = new Size(32, 28),
                Location = new Point(160, 3),
                BorderRadius = 6,
                FillColor = clrBtnNormal,
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                BorderColor = Color.FromArgb(180, 180, 180),
                BorderThickness = 1,
            };
            _btnNext.HoverState.FillColor = clrBtnHover;
            _btnNext.HoverState.ForeColor = Color.FromArgb(50, 50, 50);
            _btnNext.Click += (s, e) => ChuyenTrang(_trangHienTai + 1);

            guna2Panel3.Controls.Add(_btnPrev);
            guna2Panel3.Controls.Add(_lblTrangInfo);
            guna2Panel3.Controls.Add(_btnNext);
        }

        private void ChuyenTrang(int trang)
        {
            if (trang < 1 || trang > _tongSoTrang) return;
            _trangHienTai = trang;
            HienThiTrangHienTai();
        }

        private void HienThiTrangHienTai()
        {
            var ds = _dsDayDu
              .Skip((_trangHienTai - 1) * _soDoiMoiTrang)
              .Take(_soDoiMoiTrang)
              .ToList();

            guna2DataGridView1.DataSource = null;
            guna2DataGridView1.DataSource = ds;
            guna2DataGridView1.ClearSelection();

            int tongTrang = Math.Max(1, _tongSoTrang);
            _lblTrangInfo.Text = $"Trang {_trangHienTai} / {tongTrang}";
            _btnPrev.Enabled = _trangHienTai > 1;
            _btnNext.Enabled = _trangHienTai < tongTrang;
            _btnPrev.FillColor = _btnPrev.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnNext.FillColor = _btnNext.Enabled ? Color.FromArgb(200, 200, 200) : Color.FromArgb(225, 225, 225);
            _btnPrev.ForeColor = _btnPrev.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
            _btnNext.ForeColor = _btnNext.Enabled ? Color.FromArgb(80, 80, 80) : Color.FromArgb(180, 180, 180);
        }

        private void ApDungBoTron()
        {
            const int r = 16;
            var b = guna2DataGridView1.ClientRectangle;
            if (b.Width <= 0 || b.Height <= 0) return;
            var path = new GraphicsPath();
            path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
            path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
            path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            guna2DataGridView1.Region = new Region(path);
        }

        private void CauHinhGrid()
        {
            guna2DataGridView1.Enabled = true;
            guna2DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            guna2DataGridView1.ReadOnly = true;
            guna2DataGridView1.AutoGenerateColumns = false;
            guna2DataGridView1.DataError += (s, e) => { e.Cancel = true; };

            Column1.DataPropertyName = "MaBan";
            Column2.DataPropertyName = "TenBan";
            Column3.DataPropertyName = "LoaiBan";
            Column4.DataPropertyName = "GiaTheoGio";
            Column5.DataPropertyName = "TrangThai";

            Column1.Width = 90;
            Column2.Width = 200;
            Column3.Width = 150;
            Column4.Width = 170;
            Column5.Width = 130;
            Column7.Width = 110;

            guna2DataGridView1.RowTemplate.Height = 38;
            guna2DataGridView1.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            Column7.UseColumnTextForButtonValue = true;
            Column7.Text = "🗑  Xóa";
            Column7.FlatStyle = FlatStyle.Flat;
            Column7.DefaultCellStyle.Padding = new Padding(20, 6, 20, 6);
            Column7.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Column7.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

            foreach (DataGridViewColumn col in guna2DataGridView1.Columns)
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            selectTimKiem.DisplayMember = "Value";
            selectTimKiem.ValueMember = "Key";
            selectTimKiem.DataSource = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("",       "-- Tất cả loại --"),
                new KeyValuePair<string, string>("THUONG", "Bàn Thường"),
                new KeyValuePair<string, string>("VIP",    "Bàn VIP"),
            };
            selectTimKiem.SelectedIndex = 0;
        }

        private void TaiDanhSach()
        {
            try
            {
                _dsDayDu = _bll.LayTatCaBan();
                _trangHienTai = 1;
                HienThiTrangHienTai();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải dữ liệu",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiGrid(List<BanBidaDTO> ds)
        {
            _dsDayDu = ds;
            _trangHienTai = 1;
            HienThiTrangHienTai();
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;

            e.CellStyle.BackColor = Color.FromArgb(220, 53, 53);
            e.CellStyle.ForeColor = Color.White;
            e.CellStyle.SelectionBackColor = Color.FromArgb(180, 20, 20);
            e.CellStyle.SelectionForeColor = Color.White;
        }

        private void guna2DataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.White;
        }

        private void guna2DataGridView1_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.Empty;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.Empty;
        }

        private void guna2DataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != guna2DataGridView1.Columns["Column7"].Index) return;
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.BackColor = Color.FromArgb(185, 28, 28);
            guna2DataGridView1.Rows[e.RowIndex].Cells["Column7"].Style.ForeColor = Color.White;
        }

        private void ThucHienTimKiem()
        {
            try
            {
                string loai = selectTimKiem.SelectedValue?.ToString();
                var ds = string.IsNullOrEmpty(loai)
                    ? _bll.LayTatCaBan()
                    : _bll.TimTheoLoaiBan(loai);

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

        private void btnReload_Click(object sender, EventArgs e)
        {
            try
            {
                btnReload.Enabled = false;
                btnReload.Text = "Đang tải...";
                _dangKhoiTao = true;
                selectTimKiem.SelectedIndex = 0;
                _dangKhoiTao = false;
                TaiDanhSach();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi tải lại dữ liệu",
                  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnReload.Enabled = true;
                btnReload.Text = "Tải Lại";
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            _dangKhoiTao = true;
            selectTimKiem.SelectedIndex = 0;
            _dangKhoiTao = false;
            TaiDanhSach();
        }

        private void selectTimKiem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_dangKhoiTao) return;
            ThucHienTimKiem();
        }

        // FIX REALTIME: Bắn event sau khi thêm thành công
        private void btnThem_Click(object sender, EventArgs e)
        {
            using (var popup = new BanBiaPopupUi())
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowOverlay(this);
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    TaiDanhSach();
                    BanDuocThemHoacXoa?.Invoke(this, EventArgs.Empty); // 👈 thêm realtime
                }
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == guna2DataGridView1.Columns["Column7"].Index)
            {
                XuLyXoa(e.RowIndex);
                return;
            }

            MoPopupSua(e.RowIndex);
        }

        // FIX REALTIME: Bắn event sau khi sửa thành công
        private void MoPopupSua(int rowIndex)
        {
            string maBan = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            if (string.IsNullOrEmpty(maBan)) return;

            var ban = _bll.TimTheoMaBan(maBan);
            if (ban == null) return;

            using (var popup = new BanBiaPopupUi(ban))
            {
                popup.StartPosition = FormStartPosition.CenterParent;
                popup.ShowOverlay(this);
                if (popup.ShowDialog(this) == DialogResult.OK)
                {
                    TaiDanhSach();
                    BanDuocThemHoacXoa?.Invoke(this, EventArgs.Empty); // 👈 thêm realtime
                }
            }
        }

        // FIX REALTIME: Bắn event sau khi xóa thành công
        private void XuLyXoa(int rowIndex)
        {
            string maBan = guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString();
            string tenBan = guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value?.ToString();
            if (string.IsNullOrEmpty(maBan)) return;

            using (var dlg = new ConfirmDeleteUI(tenBan, "bàn bida"))
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
            }

            try
            {
                if (_bll.XoaBan(maBan))
                {
                    MessageBox.Show("Xóa bàn thành công!", "Thành công",
                      MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TaiDanhSach();
                    BanDuocThemHoacXoa?.Invoke(this, EventArgs.Empty); // 👈 thêm realtime
                }
                else
                    MessageBox.Show("Xóa không thành công!", "Thất bại",
                      MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainHeader_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
    }
}