using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

// ── Cần NuGet: ClosedXML ──
using ClosedXML.Excel;

namespace Bài_Tập_Lớn.GUI
{
    public partial class ThongKeUi : Form
    {
        // ── BLL ──────────────────────────────────────────────────
        private readonly ThongKeBLL _thongKeBLL = new ThongKeBLL();

        // ══════════════════════════════════════════════════════════
        //  REAL-TIME: SMART DETECTION
        // ══════════════════════════════════════════════════════════
        private readonly Timer _pollTimer = new Timer();
        private const int POLL_INTERVAL_MS = 10_000;

        private int _lastSoHoaDon = -1;
        private DateTime _lastNgayMoiNhat = DateTime.MinValue;
        private int _lastSoBanHoatDong = -1;

        private bool pieChartLoaded = false;
        private bool barChartLoaded = false;       // biểu đồ THÁNG
        private bool dailyBarChartLoaded = false;  // biểu đồ NGÀY
        private bool yearChartLoaded = false;      // biểu đồ NĂM

        public ThongKeUi()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.Load += ThongKeUi_Load;
            this.FormClosing += ThongKeUi_FormClosing;
        }

        // ════════════════════════════════════════════════════════
        //  LOAD & TIMER
        // ════════════════════════════════════════════════════════
        private void ThongKeUi_Load(object sender, EventArgs e)
        {
            RefreshAll();

            _pollTimer.Interval = POLL_INTERVAL_MS;
            _pollTimer.Tick += PollTimer_Tick;
            _pollTimer.Start();
            btnExportExcel.Click += btnExportExcel_Click;
        }

        private void ThongKeUi_FormClosing(object sender, FormClosingEventArgs e)
        {
            _pollTimer.Stop();
            _pollTimer.Dispose();
        }

        // ════════════════════════════════════════════════════════
        //  SMART POLL
        // ════════════════════════════════════════════════════════
        private void PollTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var (soHD, ngayMoiNhat, soBan) = _thongKeBLL.GetSnapshotThayDoi();

                bool coThayDoi = soHD != _lastSoHoaDon
                              || ngayMoiNhat != _lastNgayMoiNhat
                              || soBan != _lastSoBanHoatDong;

                if (coThayDoi)
                {
                    _lastSoHoaDon = soHD;
                    _lastNgayMoiNhat = ngayMoiNhat;
                    _lastSoBanHoatDong = soBan;
                    RefreshAll();
                }
            }
            catch { }
        }

        // ════════════════════════════════════════════════════════
        //  REFRESH ALL
        // ════════════════════════════════════════════════════════
        private void RefreshAll()
        {
            pieChartLoaded = false;
            barChartLoaded = false;
            dailyBarChartLoaded = false;
            yearChartLoaded = false;

            LoadCards();
            LoadTopMayCard();        // card top máy
            LoadPieChart();
            LoadBarChartThang();
            LoadDailyBarChart();
            LoadYearBarChart();      // biểu đồ năm
        }

        // ════════════════════════════════════════════════════════
        //  CARDS
        // ════════════════════════════════════════════════════════
        private void LoadCards()
        {
            try
            {
                double doanhThu = _thongKeBLL.GetDoanhThuThangHienTai();
                guna2HtmlLabel3.Text = (doanhThu / 1000).ToString("N1");

                double loiNhuan = _thongKeBLL.GetLoiNhuanThangHienTai();
                guna2HtmlLabel6.Text = (loiNhuan / 1000).ToString("N1");

                // ── Số Hóa Đơn ──
                int soHoaDon = _thongKeBLL.GetSoHoaDonThangHienTai();
                guna2HtmlLabel10.Text = soHoaDon.ToString("N0");

                // ── Số Bàn đang hoạt động ──
                int soBan = _thongKeBLL.GetSoBanDangHoatDong();
                guna2HtmlLabel13.Text = soBan.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thống kê: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ════════════════════════════════════════════════════════
        //  CARD TOP MÁY DOANH THU CAO NHẤT
        // ════════════════════════════════════════════════════════
        private Panel _topMayHost;   // host panel tạo runtime

        private void LoadTopMayCard()
        {
            try
            {
                if (_topMayHost == null)
                {
                    _topMayHost = new Panel();
                    _topMayHost.Size = new Size(280, 120);
                    _topMayHost.BackColor = Color.FromArgb(255, 255, 251);
                    guna2Panel2.Controls.Add(_topMayHost);
                    _topMayHost.Location = new Point(10, 10);
                }

                _topMayHost.Controls.Clear();

                var topList = _thongKeBLL.GetTopMayDoanhThu(top: 3);

                int y = 8;
                var title = new Label();
                title.Text = "🏆 Top Máy Doanh Thu";
                title.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                title.ForeColor = Color.FromArgb(43, 78, 35);
                title.AutoSize = true;
                title.Location = new Point(8, y);
                _topMayHost.Controls.Add(title);
                y += 24;

                int rank = 1;
                foreach (var (tenMay, dt) in topList)
                {
                    string icon = rank == 1 ? "🥇" : rank == 2 ? "🥈" : "🥉";
                    var lbl = new Label();
                    lbl.Text = $"{icon} {tenMay}  –  {(dt / 1_000_000):N1}M";
                    lbl.Font = new Font("Segoe UI", 8.5f);
                    lbl.ForeColor = Color.FromArgb(60, 60, 60);
                    lbl.AutoSize = true;
                    lbl.Location = new Point(8, y);
                    _topMayHost.Controls.Add(lbl);
                    y += 20;
                    rank++;
                }
            }
            catch
            {
                // BLL chưa có method → bỏ qua, không crash UI
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void guna2GradientPanel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint_1(object sender, PaintEventArgs e) { }
        private void gunaChart1_Load(object sender, EventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }

        // ════════════════════════════════════════════════════════
        //  PIE CHART – cơ cấu doanh thu
        // ════════════════════════════════════════════════════════
        private void LoadPieChart()
        {
            if (pieChartLoaded) return;
            pieChartLoaded = true;

            guna2Panel6.Controls.Clear();
            guna2Panel6.Padding = new Padding(0);

            Dictionary<string, double> data;
            try
            {
                var (tienBida, tienSanPham) = _thongKeBLL.GetTienBidaVaTienSanPhamThangHienTai();
                double total = tienBida + tienSanPham;
                if (total <= 0) total = 1;

                double pctBida = Math.Round(tienBida / total * 100, 1);
                double pctSanPham = Math.Round(tienSanPham / total * 100, 1);

                data = new Dictionary<string, double>
                {
                    { "Bàn Bida",   pctBida    },
                    { "Sản Phẩm",   pctSanPham },
                };
            }
            catch
            {
                data = new Dictionary<string, double>
                {
                    { "Bàn Bida",   60.0 },
                    { "Sản Phẩm",   40.0 },
                };
            }

            Color[] colors =
            {
                ColorTranslator.FromHtml("#2b4e23"),
                ColorTranslator.FromHtml("#79ae6f"),
            };

            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var chartArea = new ChartArea("main");
            chartArea.BackColor = Color.Transparent;
            chartArea.BorderColor = Color.Transparent;
            chartArea.BorderDashStyle = ChartDashStyle.NotSet;
            chartArea.InnerPlotPosition = new ElementPosition(2, 2, 88, 88);
            chart.ChartAreas.Add(chartArea);

            var legend = new Legend("legend");
            legend.Enabled = false;
            chart.Legends.Add(legend);

            var series = new Series("DoanhThu");
            series.ChartType = SeriesChartType.Pie;
            series.ChartArea = "main";
            series.Legend = "legend";
            series.IsVisibleInLegend = false;
            series.Label = "#VALX\n#PERCENT{P1}";
            series.LabelForeColor = Color.FromArgb(60, 60, 60);
            series["PieLabelStyle"] = "Outside";
            series["PieLineColor"] = "Gray";
            series["PieDrawingStyle"] = "Default";
            series.ToolTip = "#VALX: #VAL (#PERCENT{P1})";

            int idx = 0;
            foreach (var item in data)
            {
                var point = new DataPoint();
                point.SetValueXY(item.Key, item.Value);
                point.Color = colors[idx % colors.Length];
                point.LegendText = item.Key;
                series.Points.Add(point);
                idx++;
            }

            chart.Series.Add(series);
            mainLayout.Controls.Add(chart, 0, 0);

            var legendPanel = new FlowLayoutPanel();
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.BackColor = Color.Transparent;
            legendPanel.FlowDirection = FlowDirection.LeftToRight;
            legendPanel.WrapContents = false;
            legendPanel.Padding = new Padding(8, 4, 8, 4);
            legendPanel.AutoSize = false;

            for (int i = 0; i < data.Count; i++)
            {
                string label = data.Keys.ElementAt(i);
                Color color = colors[i % colors.Length];

                var item = new Panel();
                item.BackColor = Color.Transparent;
                item.AutoSize = true;
                item.Margin = new Padding(10, 6, 10, 6);

                var colorBox = new Panel();
                colorBox.Size = new Size(14, 14);
                colorBox.BackColor = color;
                colorBox.BorderStyle = BorderStyle.FixedSingle;
                colorBox.Location = new Point(0, 3);

                var lbl = new Label();
                lbl.Text = label;
                lbl.Font = new Font("Segoe UI", 9f);
                lbl.ForeColor = Color.FromArgb(60, 60, 60);
                lbl.AutoSize = true;
                lbl.Location = new Point(40, 0);

                item.Controls.Add(colorBox);
                item.Controls.Add(lbl);
                item.Width = lbl.PreferredWidth + 28;
                legendPanel.Controls.Add(item);
            }

            legendPanel.Padding = new Padding(
                Math.Max(0, (guna2Panel6.Width - (data.Count * 140)) / 2), 6, 0, 6
            );

            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel6.Controls.Add(mainLayout);
        }

        // ════════════════════════════════════════════════════════
        //  BAR CHART THÁNG → Cột nằm ngang bo tròn (ĐÃ SỬA CÔNG THỨC)
        // ════════════════════════════════════════════════════════
        private void guna2Panel7_Paint(object sender, PaintEventArgs e) { }

        private void LoadBarChartThang()
        {
            if (barChartLoaded) return;
            barChartLoaded = true;

            guna2Panel7.Controls.Clear();
            guna2Panel7.Padding = new Padding(0);

            string[] labels;
            double[] doanhThuArr;
            double[] loiNhuanArr;
            int count;

            try
            {
                var listData = _thongKeBLL.GetBieuDoTheoThang(DateTime.Now.Year);
                count = listData.Count;
                labels = new string[count];
                doanhThuArr = new double[count];
                loiNhuanArr = new double[count];

                for (int i = 0; i < count; i++)
                {
                    string thangLabel = listData[i]["thang_label"].ToString();
                    labels[i] = "T" + thangLabel.Replace("Tháng ", "");
                    doanhThuArr[i] = Math.Round(Convert.ToDouble(listData[i]["doanh_thu"]) / 1_000_000, 1);

                    double tb = Convert.ToDouble(listData[i]["tien_bida"]);
                    double ts = Convert.ToDouble(listData[i]["tien_san_pham"]);

                    // Cập nhật công thức đồng đều doanh thu và lợi nhuận công bằng
                    double loiNhuanThang = tb + (ts * 0.4);
                    loiNhuanArr[i] = Math.Round(loiNhuanThang / 1_000_000, 1);
                }
            }
            catch
            {
                labels = new[] { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };
                doanhThuArr = new double[] { 85, 72, 95, 110, 88, 120, 95, 130, 100, 115, 90, 140 };
                loiNhuanArr = new double[] { 65, 55, 72, 85, 68, 92, 73, 99, 78, 88, 69, 108 };
                count = labels.Length;
            }

            Color colorDoanhThu = ColorTranslator.FromHtml("#2b4e23");
            Color colorLoiNhuan = ColorTranslator.FromHtml("#79ae6f");

            // ── Layout ──
            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 88f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var ca = new ChartArea("thang");
            ca.BackColor = Color.Transparent;
            ca.BorderColor = Color.Transparent;
            ca.BorderDashStyle = ChartDashStyle.NotSet;

            // Trục X (trục đứng, hiển thị Tháng)
            ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 8f);
            ca.AxisX.LineColor = Color.FromArgb(210, 210, 210);
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.MajorTickMark.Enabled = false;
            ca.AxisX.Interval = 1;
            ca.AxisX.IsReversed = true;

            // Trục Y (trục ngang, hiển thị Giá trị)
            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 8f);
            ca.AxisY.LabelStyle.Format = "# 'tr'";
            ca.AxisY.LineColor = Color.Transparent;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            ca.AxisY.MajorTickMark.Enabled = false;
            ca.AxisY.Minimum = 0;

            ca.InnerPlotPosition = new ElementPosition(5, 0, 88, 88);
            chart.ChartAreas.Add(ca);

            chart.Legends.Add(new Legend("leg") { Enabled = false });

            // ── Series (Sử dụng Bar chart, Color Transparent) ──
            var sDT = new Series("DoanhThu");
            sDT.ChartType = SeriesChartType.Bar;
            sDT.ChartArea = "thang";
            sDT.IsVisibleInLegend = false;
            sDT.Color = Color.Transparent;
            sDT.BorderColor = Color.Transparent;
            sDT["PointWidth"] = "0.85";
            sDT.ToolTip = "Doanh Thu: #VAL tr";
            for (int i = 0; i < count; i++)
                sDT.Points.AddXY(labels[i], doanhThuArr[i]);
            chart.Series.Add(sDT);

            var sLN = new Series("LoiNhuan");
            sLN.ChartType = SeriesChartType.Bar;
            sLN.ChartArea = "thang";
            sLN.IsVisibleInLegend = false;
            sLN.Color = Color.Transparent;
            sLN.BorderColor = Color.Transparent;
            sLN["PointWidth"] = "0.85";
            sLN.ToolTip = "Lợi Nhuận: #VAL tr";
            for (int i = 0; i < count; i++)
                sLN.Points.AddXY(labels[i], loiNhuanArr[i]);
            chart.Series.Add(sLN);

            // ── PostPaint: Vẽ cột ngang bo tròn ──
            chart.PostPaint += (s, pe) =>
            {
                var g = pe.ChartGraphics.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float y0 = (float)ca.AxisX.ValueToPixelPosition(1.0);
                float y1 = (float)ca.AxisX.ValueToPixelPosition(2.0);
                float unitPx = Math.Abs(y1 - y0);

                float innerGap = 4f;
                float totalBarH = unitPx * 0.75f;
                float barH = (totalBarH - innerGap) / 2f;
                float xLeft = (float)ca.AxisY.ValueToPixelPosition(0);
                int radius = 3;

                for (int i = 0; i < count; i++)
                {
                    float yCenter = (float)ca.AxisX.ValueToPixelPosition(i + 1.0);

                    // Vẽ Doanh Thu (cột bên trên)
                    float xRightDT = (float)ca.AxisY.ValueToPixelPosition(doanhThuArr[i]);
                    float wDT = xRightDT - xLeft;
                    if (wDT > 0)
                    {
                        float topDT = yCenter - (barH + innerGap / 2f);
                        using (var br = new SolidBrush(colorDoanhThu))
                            DrawRoundedRightBar(g, br, xLeft, topDT, wDT, barH, radius);
                    }

                    // Vẽ Lợi Nhuận (cột bên dưới)
                    float xRightLN = (float)ca.AxisY.ValueToPixelPosition(loiNhuanArr[i]);
                    float wLN = xRightLN - xLeft;
                    if (wLN > 0)
                    {
                        float topLN = yCenter + innerGap / 2f;
                        using (var br = new SolidBrush(colorLoiNhuan))
                            DrawRoundedRightBar(g, br, xLeft, topLN, wLN, barH, radius);
                    }
                }
            };

            mainLayout.Controls.Add(chart, 0, 0);

            var legendPanel = BuildLegendPanel(colorDoanhThu, colorLoiNhuan);
            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel7.Controls.Add(mainLayout);
        }

        // Hàm vẽ cột nằm ngang bo tròn 2 góc bên phải
        private void DrawRoundedRightBar(Graphics g, Brush br, float x, float y, float w, float h, int r)
        {
            if (w <= 0 || h <= 0) return;

            if (r * 2 > h) r = (int)(h / 2);
            if (r * 2 > w) r = (int)(w / 2);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddLine(x, y, x + w - r, y);
                path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
                path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
                path.AddLine(x + w - r, y + h, x, y + h);

                path.CloseFigure();
                g.FillPath(br, path);
            }
        }

        // ════════════════════════════════════════════════════════
        //  BAR CHART NGÀY (7 ngày gần nhất) – Đmax bo tròn (ĐA SỬA CÔNG THỨC)
        // ════════════════════════════════════════════════════════
        private void guna2Panel5_Paint_2(object sender, PaintEventArgs e) { }

        private void LoadDailyBarChart()
        {
            if (dailyBarChartLoaded) return;
            dailyBarChartLoaded = true;

            guna2Panel5.Controls.Clear();
            guna2Panel5.Padding = new Padding(0);

            string[] labels;
            double[] doanhThu;
            double[] loiNhuan;
            int count;

            try
            {
                DateTime denNgay = DateTime.Today;
                DateTime tuNgay = denNgay.AddDays(-6);

                var listData = _thongKeBLL.GetBieuDoTheoNgay(tuNgay, denNgay);

                count = listData.Count;
                labels = new string[count];
                doanhThu = new double[count];
                loiNhuan = new double[count];

                for (int i = 0; i < count; i++)
                {
                    labels[i] = listData[i]["ngay_ban_label"].ToString();
                    doanhThu[i] = Math.Round(Convert.ToDouble(listData[i]["doanh_thu"]) / 1_000_000, 1);

                    double tb = Convert.ToDouble(listData[i]["tien_bida"]);
                    double ts = Convert.ToDouble(listData[i]["tien_san_pham"]);

                    // Sửa công thức lợi nhuận ngày đồng đều với doanh thu
                    double loiNhuanNgay = tb + (ts * 0.4);
                    loiNhuan[i] = Math.Round(loiNhuanNgay / 1_000_000, 1);
                }
            }
            catch
            {
                labels = new[] { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
                doanhThu = new double[] { 85, 72, 110, 95, 130, 148, 60 };
                loiNhuan = new double[] { 62, 54, 85, 71, 98, 112, 46 };
                count = labels.Length;
            }

            Color colorDoanhThu = ColorTranslator.FromHtml("#2b4e23");
            Color colorLoiNhuan = ColorTranslator.FromHtml("#79ae6f");

            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 88f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var ca = new ChartArea("daily");
            ca.BackColor = Color.Transparent;
            ca.BorderColor = Color.Transparent;
            ca.BorderDashStyle = ChartDashStyle.NotSet;

            ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5f);
            ca.AxisX.LineColor = Color.FromArgb(210, 210, 210);
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.MajorTickMark.Enabled = false;
            ca.AxisX.Interval = 1;

            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 8.5f);
            ca.AxisY.LabelStyle.Format = "# 'tr'";
            ca.AxisY.LineColor = Color.Transparent;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            ca.AxisY.MajorTickMark.Enabled = false;
            ca.AxisY.Minimum = 0;

            ca.InnerPlotPosition = new ElementPosition(0, 5, 90, 90);
            chart.ChartAreas.Add(ca);

            chart.Legends.Add(new Legend("leg") { Enabled = false });

            var sDT = new Series("Doanh Thu");
            sDT.ChartType = SeriesChartType.Column;
            sDT.ChartArea = "daily";
            sDT.IsVisibleInLegend = false;
            sDT.Color = Color.Transparent;
            sDT.BorderColor = Color.Transparent;
            sDT["PointWidth"] = "0.85";
            sDT.ToolTip = "Doanh Thu: #VAL tr";
            for (int i = 0; i < count; i++)
                sDT.Points.AddXY(labels[i], doanhThu[i]);
            chart.Series.Add(sDT);

            var sLN = new Series("Lợi Nhuận");
            sLN.ChartType = SeriesChartType.Column;
            sLN.ChartArea = "daily";
            sLN.IsVisibleInLegend = false;
            sLN.Color = Color.Transparent;
            sLN.BorderColor = Color.Transparent;
            sLN["PointWidth"] = "0.85";
            sLN.ToolTip = "Lợi Nhuận: #VAL tr";
            for (int i = 0; i < count; i++)
                sLN.Points.AddXY(labels[i], loiNhuan[i]);
            chart.Series.Add(sLN);

            chart.PostPaint += (s, pe) =>
            {
                var g = pe.ChartGraphics.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float x0 = (float)ca.AxisX.ValueToPixelPosition(1.0);
                float x1 = (float)ca.AxisX.ValueToPixelPosition(2.0);
                float unitPx = Math.Abs(x1 - x0);

                float innerGap = 6f;
                float totalBarW = unitPx * 0.80f;
                float barW = (totalBarW - innerGap) / 2f;
                float yBot = (float)ca.AxisY.ValueToPixelPosition(0);
                int radius = 3;

                for (int i = 0; i < count; i++)
                {
                    float xCenter = (float)ca.AxisX.ValueToPixelPosition(i + 1.0);

                    float yTopDT = (float)ca.AxisY.ValueToPixelPosition(doanhThu[i]);
                    float hDT = yBot - yTopDT;
                    if (hDT > 0)
                    {
                        float leftDT = xCenter - (barW + innerGap / 2f);
                        using (var br = new SolidBrush(colorDoanhThu))
                            DrawRoundedTopBar(g, br, leftDT, yTopDT, barW, hDT, radius);
                    }

                    float yTopLN = (float)ca.AxisY.ValueToPixelPosition(loiNhuan[i]);
                    float hLN = yBot - yTopLN;
                    if (hLN > 0)
                    {
                        float leftLN = xCenter + innerGap / 2f;
                        using (var br = new SolidBrush(colorLoiNhuan))
                            DrawRoundedTopBar(g, br, leftLN, yTopLN, barW, hLN, radius);
                    }
                }
            };

            mainLayout.Controls.Add(chart, 0, 0);
            mainLayout.Controls.Add(BuildLegendPanel(colorDoanhThu, colorLoiNhuan), 0, 1);
            guna2Panel5.Controls.Add(mainLayout);
        }

        // ════════════════════════════════════════════════════════
        //  BIỂU ĐỒ NĂM – doanh thu các năm gần nhất (ĐÃ SỬA CÔNG THỨC)
        // ════════════════════════════════════════════════════════
        private Panel _yearChartHost;

        private void LoadYearBarChart()
        {
            if (yearChartLoaded) return;
            yearChartLoaded = true;

            if (_yearChartHost == null)
            {
                _yearChartHost = new Panel();
                _yearChartHost.Size = new Size(460, 200);
                _yearChartHost.BackColor = Color.FromArgb(255, 255, 251);
                guna2Panel2.Controls.Add(_yearChartHost);
                _yearChartHost.Location = new Point(10, 140);
            }

            _yearChartHost.Controls.Clear();

            string[] labels;
            double[] doanhThuArr;
            double[] loiNhuanArr;
            int count;

            try
            {
                var listData = _thongKeBLL.GetBieuDoTheoNam();
                count = listData.Count;
                labels = new string[count];
                doanhThuArr = new double[count];
                loiNhuanArr = new double[count];

                for (int i = 0; i < count; i++)
                {
                    labels[i] = listData[i]["nam_label"].ToString();
                    doanhThuArr[i] = Math.Round(Convert.ToDouble(listData[i]["doanh_thu"]) / 1_000_000, 1);
                    double tb = Convert.ToDouble(listData[i]["tien_bida"]);
                    double ts = Convert.ToDouble(listData[i]["tien_san_pham"]);

                    // Sửa công thức lợi nhuận năm đồng đều
                    double loiNhuanNam = tb + (ts * 0.4);
                    loiNhuanArr[i] = Math.Round(loiNhuanNam / 1_000_000, 1);
                }
            }
            catch
            {
                int yr = DateTime.Now.Year;
                labels = new[] { (yr - 2).ToString(), (yr - 1).ToString(), yr.ToString() };
                doanhThuArr = new double[] { 980, 1150, 640 };
                loiNhuanArr = new double[] { 740, 890, 480 };
                count = 3;
            }

            Color colorDoanhThu = ColorTranslator.FromHtml("#2b4e23");
            Color colorLoiNhuan = ColorTranslator.FromHtml("#79ae6f");

            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var ca = new ChartArea("year");
            ca.BackColor = Color.Transparent;
            ca.BorderColor = Color.Transparent;
            ca.BorderDashStyle = ChartDashStyle.NotSet;

            ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            ca.AxisX.LineColor = Color.FromArgb(210, 210, 210);
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.MajorTickMark.Enabled = false;
            ca.AxisX.Interval = 1;

            ca.AxisY.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            ca.AxisY.LabelStyle.Format = "# 'tr'";
            ca.AxisY.LineColor = Color.Transparent;
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 230);
            ca.AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            ca.AxisY.MajorTickMark.Enabled = false;
            ca.AxisY.Minimum = 0;

            ca.InnerPlotPosition = new ElementPosition(0, 5, 88, 88);
            chart.ChartAreas.Add(ca);
            chart.Legends.Add(new Legend("leg") { Enabled = false });

            var sDT = new Series("DoanhThu");
            sDT.ChartType = SeriesChartType.Column;
            sDT.ChartArea = "year";
            sDT.Color = Color.Transparent;
            sDT.BorderColor = Color.Transparent;
            sDT["PointWidth"] = "0.85";
            sDT.ToolTip = "Doanh Thu: #VAL tr";
            for (int i = 0; i < count; i++)
                sDT.Points.AddXY(labels[i], doanhThuArr[i]);
            chart.Series.Add(sDT);

            var sLN = new Series("LoiNhuan");
            sLN.ChartType = SeriesChartType.Column;
            sLN.ChartArea = "year";
            sLN.Color = Color.Transparent;
            sLN.BorderColor = Color.Transparent;
            sLN["PointWidth"] = "0.85";
            sLN.ToolTip = "Lợi Nhuận: #VAL tr";
            for (int i = 0; i < count; i++)
                sLN.Points.AddXY(labels[i], loiNhuanArr[i]);
            chart.Series.Add(sLN);

            chart.PostPaint += (s, pe) =>
            {
                var g = pe.ChartGraphics.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                float x0 = (float)ca.AxisX.ValueToPixelPosition(1.0);
                float x1 = (float)ca.AxisX.ValueToPixelPosition(2.0);
                float unitPx = Math.Abs(x1 - x0);

                float innerGap = 8f;
                float totalBarW = unitPx * 0.70f;
                float barW = (totalBarW - innerGap) / 2f;
                float yBot = (float)ca.AxisY.ValueToPixelPosition(0);
                int radius = 4;

                for (int i = 0; i < count; i++)
                {
                    float xCenter = (float)ca.AxisX.ValueToPixelPosition(i + 1.0);

                    float yTopDT = (float)ca.AxisY.ValueToPixelPosition(doanhThuArr[i]);
                    float hDT = yBot - yTopDT;
                    if (hDT > 0)
                    {
                        float leftDT = xCenter - (barW + innerGap / 2f);
                        using (var br = new SolidBrush(colorDoanhThu))
                            DrawRoundedTopBar(g, br, leftDT, yTopDT, barW, hDT, radius);
                    }

                    float yTopLN = (float)ca.AxisY.ValueToPixelPosition(loiNhuanArr[i]);
                    float hLN = yBot - yTopLN;
                    if (hLN > 0)
                    {
                        float leftLN = xCenter + innerGap / 2f;
                        using (var br = new SolidBrush(colorLoiNhuan))
                            DrawRoundedTopBar(g, br, leftLN, yTopLN, barW, hLN, radius);
                    }
                }
            };

            mainLayout.Controls.Add(chart, 0, 0);
            mainLayout.Controls.Add(BuildLegendPanel(colorDoanhThu, colorLoiNhuan), 0, 1);
            _yearChartHost.Controls.Add(mainLayout);
        }

        // ════════════════════════════════════════════════════════
        //  EXPORT EXCEL (ĐA SỬA CÔNG THỨC EXCEL)
        // ════════════════════════════════════════════════════════
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExportExcel();
        }

        private void ExportExcel()
        {
            try
            {
                using (var wb = new XLWorkbook())
                {
                    string fontName = "Times New Roman";

                    // ===== HEADER STYLE =====
                    void ApplyHeaderStyle(IXLWorksheet ws, string mainTitle, string subTitle, int maxColumn)
                    {
                        var titleRange = ws.Range(2, 1, 2, maxColumn);
                        titleRange.Merge();

                        var cellTitle = ws.Cell(2, 1);
                        cellTitle.Value = mainTitle;
                        cellTitle.Style.Font.FontName = fontName;
                        cellTitle.Style.Font.FontSize = 16;
                        cellTitle.Style.Font.Bold = true;
                        cellTitle.Style.Font.FontColor = XLColor.White;
                        cellTitle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellTitle.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        titleRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2b4e23");
                        titleRange.Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                        titleRange.Style.Border.OutsideBorderColor = XLColor.FromHtml("#1d3617");

                        ws.Row(2).Height = 30;

                        var subTitleRange = ws.Range(3, 1, 3, maxColumn);
                        subTitleRange.Merge();

                        var cellSub = ws.Cell(3, 1);
                        cellSub.Value = subTitle;
                        cellSub.Style.Font.FontName = fontName;
                        cellSub.Style.Font.FontSize = 11;
                        cellSub.Style.Font.Italic = true;
                        cellSub.Style.Font.FontColor = XLColor.FromHtml("#555555");
                        cellSub.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cellSub.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                        ws.Row(3).Height = 22;
                        ws.Row(4).Height = 10;
                    }

                    // ===== FOOTER SIGNATURE =====
                    void ApplyFooterSignature(IXLWorksheet ws, int startRow, int maxColumn)
                    {
                        int rDate = startRow + 2;
                        int rSign = startRow + 3;

                        var cellDate = ws.Cell(rDate, maxColumn - 1);
                        cellDate.Value = $"Hà Nội, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}";
                        cellDate.Style.Font.FontName = fontName;
                        cellDate.Style.Font.FontSize = 11;
                        cellDate.Style.Font.Italic = true;

                        ws.Range(rDate, maxColumn - 1, rDate, maxColumn).Merge()
                            .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        var cellSign = ws.Cell(rSign, maxColumn - 1);
                        cellSign.Value = "Người lập báo cáo";
                        cellSign.Style.Font.FontName = fontName;
                        cellSign.Style.Font.FontSize = 11;
                        cellSign.Style.Font.Bold = true;

                        ws.Range(rSign, maxColumn - 1, rSign, maxColumn).Merge()
                            .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    }

                    // ── SHEET 1 ──
                    var ws1 = wb.Worksheets.Add("Tháng Hiện Tại");
                    ApplyHeaderStyle(ws1, "CÂU LẠC BỘ BILLIARD DOUBLE2N - TỔNG QUAN THÁNG", $"Tháng hiện tại: {DateTime.Now:MM/yyyy}", 3);

                    string[] headers1 = { "Chỉ số", "Giá trị", "Đơn vị" };
                    for (int i = 0; i < headers1.Length; i++)
                    {
                        var cell = ws1.Cell(5, i + 1);
                        cell.Value = headers1[i];
                        cell.Style.Font.FontName = fontName;
                        cell.Style.Font.FontSize = 11;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontColor = XLColor.White;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2b4e23");
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    }
                    ws1.Row(5).Height = 22;

                    double doanhThu = _thongKeBLL.GetDoanhThuThangHienTai();
                    var (tienBida, tienSanPham) = _thongKeBLL.GetTienBidaVaTienSanPhamThangHienTai();

                    // Cập nhật công thức lợi nhuận đồng bộ Excel Sheet 1
                    double loiNhuan = tienBida + (tienSanPham * 0.4);

                    int soHoaDon = _thongKeBLL.GetSoHoaDonThangHienTai();
                    int soBan = _thongKeBLL.GetSoBanDangHoatDong();

                    object[,] data1 =
                    {
                        { "Doanh Thu", doanhThu, "VNĐ" },
                        { "Lợi Nhuận Thống Kê", loiNhuan, "VNĐ" },
                        { "Tiền Bida", tienBida, "VNĐ" },
                        { "Tiền Sản Phẩm", tienSanPham, "VNĐ" },
                        { "Số Hóa Đơn", soHoaDon, "Hóa đơn" },
                        { "Số Bàn Hoạt Động", soBan, "Bàn" }
                    };

                    int row1 = 6;
                    for (int i = 0; i < data1.GetLength(0); i++)
                    {
                        ws1.Cell(row1, 1).Value = data1[i, 0].ToString();
                        ws1.Cell(row1, 2).Value = Convert.ToDouble(data1[i, 1]);
                        ws1.Cell(row1, 3).Value = data1[i, 2].ToString();

                        ws1.Range(row1, 1, row1, 3).Style.Font.FontName = fontName;
                        ws1.Range(row1, 1, row1, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        ws1.Range(row1, 1, row1, 3).Style.Border.BottomBorderColor = XLColor.FromHtml("#E0E0E0");

                        ws1.Cell(row1, 2).Style.NumberFormat.Format = "#,##0";
                        ws1.Cell(row1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                        ws1.Cell(row1, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        ws1.Cell(row1, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        ws1.Row(row1).Height = 20;
                        row1++;
                    }
                    ApplyFooterSignature(ws1, row1, 3);

                    // ── SHEET 2 ──
                    var ws2 = wb.Worksheets.Add("Theo Tháng");
                    ApplyHeaderStyle(ws2, "CÂU LẠC BỘ BILLIARD DOUBLE2N - DOANH THU THEO THÁNG", $"Năm báo cáo: {DateTime.Now.Year}", 3);

                    ws2.Cell(5, 1).Value = "Tháng";
                    ws2.Cell(5, 2).Value = "Doanh Thu (VNĐ)";
                    ws2.Cell(5, 3).Value = "Lợi Nhuận (VNĐ)";

                    for (int col = 1; col <= 3; col++)
                    {
                        var cell = ws2.Cell(5, col);
                        cell.Style.Font.FontName = fontName;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontColor = XLColor.White;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2b4e23");
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        cell.Style.Alignment.Horizontal = col == 1 ? XLAlignmentHorizontalValues.Center : XLAlignmentHorizontalValues.Right;
                    }

                    int row2 = 6;
                    try
                    {
                        var listThang = _thongKeBLL.GetBieuDoTheoThang(DateTime.Now.Year);
                        foreach (var item in listThang)
                        {
                            ws2.Cell(row2, 1).Value = item["thang_label"].ToString();
                            ws2.Cell(row2, 2).Value = Convert.ToDouble(item["doanh_thu"]);

                            double tb = Convert.ToDouble(item["tien_bida"]);
                            double ts = Convert.ToDouble(item["tien_san_pham"]);

                            // Sửa công thức lợi nhuận đồng bộ Excel Sheet 2
                            ws2.Cell(row2, 3).Value = tb + (ts * 0.4);

                            ws2.Range(row2, 1, row2, 3).Style.Font.FontName = fontName;
                            ws2.Range(row2, 1, row2, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            ws2.Range(row2, 1, row2, 3).Style.Border.BottomBorderColor = XLColor.FromHtml("#E0E0E0");
                            ws2.Row(row2).Height = 20;
                            row2++;
                        }
                    }
                    catch { }

                    ws2.Cell(row2, 1).Value = "Tổng cộng:";
                    ws2.Cell(row2, 1).Style.Font.Bold = true;
                    ws2.Cell(row2, 2).FormulaA1 = $"=SUM(B6:B{row2 - 1})";
                    ws2.Cell(row2, 3).FormulaA1 = $"=SUM(C6:C{row2 - 1})";

                    ws2.Range(row2, 1, row2, 3).Style.Font.Bold = true;
                    ws2.Range(row2, 1, row2, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws2.Range(row2, 1, row2, 3).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws2.Columns(2, 3).Style.NumberFormat.Format = "#,##0";
                    ApplyFooterSignature(ws2, row2, 3);

                    // ── SHEET 3 ──
                    var ws3 = wb.Worksheets.Add("7 Ngày Gần Nhất");
                    string tuNgay = DateTime.Today.AddDays(-6).ToString("dd/MM/yyyy");
                    string denNgay = DateTime.Today.ToString("dd/MM/yyyy");
                    ApplyHeaderStyle(ws3, "CÂU LẠC BỘ BILLIARD DOUBLE2N - DOANH THU THEO NGÀY", $"Từ ngày: {tuNgay} đến ngày: {denNgay}", 3);

                    ws3.Cell(5, 1).Value = "Ngày";
                    ws3.Cell(5, 2).Value = "Doanh Thu (VNĐ)";
                    ws3.Cell(5, 3).Value = "Lợi Nhuận (VNĐ)";

                    for (int col = 1; col <= 3; col++)
                    {
                        var cell = ws3.Cell(5, col);
                        cell.Style.Font.FontName = fontName;
                        cell.Style.Font.Bold = true;
                        cell.Style.Font.FontColor = XLColor.White;
                        cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#2b4e23");
                        cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;
                        cell.Style.Alignment.Horizontal = col == 1 ? XLAlignmentHorizontalValues.Center : XLAlignmentHorizontalValues.Right;
                    }

                    int row3 = 6;
                    try
                    {
                        var listNgay = _thongKeBLL.GetBieuDoTheoNgay(DateTime.Today.AddDays(-6), DateTime.Today);
                        foreach (var item in listNgay)
                        {
                            ws3.Cell(row3, 1).Value = item["ngay_ban_label"].ToString();
                            ws3.Cell(row3, 2).Value = Convert.ToDouble(item["doanh_thu"]);

                            double tb = Convert.ToDouble(item["tien_bida"]);
                            double ts = Convert.ToDouble(item["tien_san_pham"]);

                            // Sửa công thức lợi nhuận đồng bộ Excel Sheet 3
                            ws3.Cell(row3, 3).Value = tb + (ts * 0.4);

                            ws3.Range(row3, 1, row3, 3).Style.Font.FontName = fontName;
                            ws3.Range(row3, 1, row3, 3).Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                            ws3.Range(row3, 1, row3, 3).Style.Border.BottomBorderColor = XLColor.FromHtml("#E0E0E0");
                            ws3.Row(row3).Height = 20;
                            row3++;
                        }
                    }
                    catch { }

                    ws3.Cell(row3, 1).Value = "Tổng cộng:";
                    ws3.Cell(row3, 1).Style.Font.Bold = true;
                    ws3.Cell(row3, 2).FormulaA1 = $"=SUM(B6:B{row3 - 1})";
                    ws3.Cell(row3, 3).FormulaA1 = $"=SUM(C6:C{row3 - 1})";

                    ws3.Range(row3, 1, row3, 3).Style.Font.Bold = true;
                    ws3.Range(row3, 1, row3, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws3.Range(row3, 1, row3, 3).Style.Border.BottomBorder = XLBorderStyleValues.Double;
                    ws3.Columns(2, 3).Style.NumberFormat.Format = "#,##0";
                    ApplyFooterSignature(ws3, row3, 3);

                    // ===== STYLE CHUNG =====
                    foreach (var ws in wb.Worksheets)
                    {
                        var used = ws.RangeUsed();
                        if (used != null)
                        {
                            used.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                            used.Style.Border.InsideBorderColor = XLColor.FromHtml("#EAEAEA");
                            used.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                        }
                        ws.Columns().AdjustToContents();
                        foreach (var col in ws.Columns(1, 3))
                        {
                            col.Width += 4;
                        }
                    }

                    // ===== SAVE FILE =====
                    using (var dlg = new SaveFileDialog())
                    {
                        dlg.Filter = "Excel Files|*.xlsx";
                        dlg.FileName = $"BaoCaoDoanhThu_Double2N_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
                        dlg.Title = "Lưu báo cáo Excel";

                        if (dlg.ShowDialog() == DialogResult.OK)
                        {
                            wb.SaveAs(dlg.FileName);
                            MessageBox.Show("Xuất Excel thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PreviewExcel(dlg.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PreviewExcel(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở file preview: " + ex.Message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // ════════════════════════════════════════════════════════
        //  HELPERS dùng chung
        // ════════════════════════════════════════════════════════
        private FlowLayoutPanel BuildLegendPanel(Color colorDoanhThu, Color colorLoiNhuan)
        {
            var legendPanel = new FlowLayoutPanel();
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.BackColor = Color.Transparent;
            legendPanel.FlowDirection = FlowDirection.LeftToRight;
            legendPanel.WrapContents = false;
            legendPanel.Padding = new Padding(12, 4, 0, 4);

            foreach (var (name, color) in new[] { ("Doanh Thu", colorDoanhThu), ("Lợi Nhuận", colorLoiNhuan) })
            {
                var item = new Panel { BackColor = Color.Transparent, AutoSize = true, Margin = new Padding(10, 6, 10, 6) };
                var colorBox = new Panel { Size = new Size(12, 12), BackColor = color, Location = new Point(0, 3), BorderStyle = BorderStyle.None };
                var lbl = new Label { Text = name, Font = new Font("Segoe UI", 8.5f), ForeColor = Color.FromArgb(80, 80, 80), AutoSize = true, Location = new Point(18, 0) };

                item.Controls.Add(colorBox);
                item.Controls.Add(lbl);
                item.Width = lbl.PreferredWidth + 26;
                legendPanel.Controls.Add(item);
            }

            return legendPanel;
        }

        private void DrawRoundedTopBar(Graphics g, Brush brush, float x, float y, float width, float height, int radius)
        {
            if (radius <= 0 || height < radius * 2)
            {
                g.FillRectangle(brush, x, y, width, height);
                return;
            }
            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(x, y, radius * 2, radius * 2, 180, 90);
                path.AddArc(x + width - radius * 2, y, radius * 2, radius * 2, 270, 90);
                path.AddLine(x + width, y + height, x, y + height);
                path.CloseFigure();
                g.FillPath(brush, path);
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e) { }
        private void guna2HtmlLabel11_Click(object sender, EventArgs e) { }
        private void guna2Panel12_Paint(object sender, PaintEventArgs e) { }
        private void btnExportExcel_Click_1(object sender, EventArgs e) { }
        private void guna2HtmlLabel4_Click(object sender, EventArgs e) { }
    }
}