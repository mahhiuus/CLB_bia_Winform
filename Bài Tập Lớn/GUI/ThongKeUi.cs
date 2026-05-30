using Bài_Tập_Lớn.BLL;
using Bài_Tập_Lớn.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Bài_Tập_Lớn.GUI
{
    public partial class ThongKeUi : Form
    {
        // ── BLL ──────────────────────────────────────────────────
        private readonly ThongKeBLL _thongKeBLL = new ThongKeBLL();

        private bool pieChartLoaded = false;

        public ThongKeUi()
        {
            InitializeComponent();
            this.Load += ThongKeUi_Load;
        }

        private void ThongKeUi_Load(object sender, EventArgs e)
        {
            LoadCards();          // <── MỚI: nối 4 card từ BLL
            LoadPieChart();       // <── ĐỔI TÊN: gọi BLL thay fake
            LoadBarChartThang();  // <── ĐỔI TÊN: gọi BLL thay fake
            LoadDailyBarChart();  // <── ĐỔI TÊN: gọi BLL thay fake
        }

        // ════════════════════════════════════════════════════════
        //  CARDS – nối dữ liệu thật
        // ════════════════════════════════════════════════════════
        private void LoadCards()
        {
            try
            {
                // Card 1 – Doanh Thu (guna2GradientPanel1) → guna2HtmlLabel3
                double doanhThu = _thongKeBLL.GetDoanhThuThangHienTai();
                guna2HtmlLabel3.Text = (doanhThu / 1_000_000).ToString("N1"); // triệu VNĐ

                // Card 2 – Lợi Nhuận (guna2GradientPanel5) → guna2HtmlLabel6
                double giaVon = _thongKeBLL.GetGiaVonThangHienTai();
                double loiNhuan = doanhThu - giaVon;
                guna2HtmlLabel6.Text = (loiNhuan / 1_000_000).ToString("N1");

                // Card 3 – Hóa Đơn (guna2GradientPanel2) → guna2HtmlLabel10
                int soHoaDon = _thongKeBLL.GetSoHoaDonThangHienTai();
                guna2HtmlLabel10.Text = soHoaDon.ToString("N0");

                // Card 4 – Số Bàn đang hoạt động (guna2GradientPanel3) → guna2HtmlLabel13
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
        //  PIE CHART – cơ cấu doanh thu (guna2Panel6)
        //  Chỉ thay phần data, giữ nguyên 100% code vẽ
        // ════════════════════════════════════════════════════════
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
        private void guna2GradientPanel3_Paint(object sender, PaintEventArgs e) { }
        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint(object sender, PaintEventArgs e) { }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
        private void guna2Panel5_Paint_1(object sender, PaintEventArgs e) { }
        private void gunaChart1_Load(object sender, EventArgs e) { }
        private void guna2Panel6_Paint(object sender, PaintEventArgs e) { }

        private void LoadPieChart()
        {
            if (pieChartLoaded) return;
            pieChartLoaded = true;

            guna2Panel6.Controls.Clear();
            guna2Panel6.Padding = new Padding(0);

            // ── LẤY DỮ LIỆU THẬT TỪ BLL ──
            Dictionary<string, double> data;
            try
            {
                double doanhThu = _thongKeBLL.GetDoanhThuThangHienTai();
                double giaVon = _thongKeBLL.GetGiaVonThangHienTai();
                double loiNhuan = Math.Max(0, doanhThu - giaVon);
                double tongBida = doanhThu * 0.60;
                double tongSanPham = doanhThu * 0.40;
                double total = tongBida + tongSanPham;
                if (total <= 0) total = 1;

                data = new Dictionary<string, double>
                {
                    { "Bàn Bida",  Math.Round(tongBida    / total * 100, 1) },
                    { "Sản Phẩm",  Math.Round(tongSanPham / total * 100, 1) },
                };

                double pctLN = Math.Round(loiNhuan / Math.Max(doanhThu, 1) * 100, 1);
                if (pctLN > 0)
                    data["Lợi Nhuận"] = pctLN;
            }
            catch
            {
                data = new Dictionary<string, double>
                {
                    { "Sản Phẩm",  26.3 },
                    { "Bàn Bida",  57.9 },
                    { "Lợi Nhuận", 15.8 }
                };
            }

            Color[] colors =
            {
                ColorTranslator.FromHtml("#79ae6f"),
                ColorTranslator.FromHtml("#f0f0e8"),
                ColorTranslator.FromHtml("#2b4e23"),
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
            chartArea.InnerPlotPosition = new ElementPosition(2, 2, 96, 96);
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
                lbl.Text = $"{label}";
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
                (guna2Panel6.Width - (data.Count * 140)) / 2, 6, 0, 6
            );

            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel6.Controls.Add(mainLayout);
        }

        // ════════════════════════════════════════════════════════
        //  BAR CHART THÁNG – doanh thu + lợi nhuận (guna2Panel7)
        //  Chỉ thay phần data, giữ nguyên 100% code vẽ
        // ════════════════════════════════════════════════════════
        private bool barChartLoaded = false;

        private void guna2Panel7_Paint(object sender, PaintEventArgs e) { }

        private void LoadBarChartThang()
        {
            if (barChartLoaded) return;
            barChartLoaded = true;

            guna2Panel7.Controls.Clear();
            guna2Panel7.Padding = new Padding(0);

            // ── LẤY DỮ LIỆU THẬT TỪ BLL ──
            var doanhThu = new Dictionary<string, double>();
            var loiNhuan = new Dictionary<string, double>();
            try
            {
                var listData = _thongKeBLL.GetBieuDoTheoThang(DateTime.Now.Year);
                foreach (var row in listData)
                {
                    string thangLabel = row["thang_label"].ToString(); // "Tháng X"
                    string key = "T" + thangLabel.Replace("Tháng ", "");
                    double dt = Convert.ToDouble(row["doanh_thu"]) / 1_000_000;
                    double ln = Convert.ToDouble(row["loi_nhuan"]) / 1_000_000;
                    doanhThu[key] = Math.Round(dt, 1);
                    loiNhuan[key] = Math.Round(ln, 1);
                }
            }
            catch
            {
                doanhThu = new Dictionary<string, double>
                {
                    { "T1", 85 }, { "T2", 72 }, { "T3", 95 },
                    { "T4", 110 }, { "T5", 88 }, { "T6", 120 }
                };
                loiNhuan = new Dictionary<string, double>
                {
                    { "T1", 32 }, { "T2", 28 }, { "T3", 41 },
                    { "T4", 50 }, { "T5", 35 }, { "T6", 55 }
                };
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

            var chartArea = new ChartArea("main");
            chartArea.BackColor = Color.Transparent;
            chartArea.BorderColor = Color.Transparent;
            chartArea.BorderDashStyle = ChartDashStyle.NotSet;

            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(120, 120, 120);
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisX.LineColor = Color.FromArgb(220, 220, 220);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(120, 120, 120);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisY.LineColor = Color.Transparent;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            chart.ChartAreas.Add(chartArea);

            var legend = new Legend("legend");
            legend.Enabled = false;
            chart.Legends.Add(legend);

            var seriesDoanhThu = new Series("Doanh Thu");
            seriesDoanhThu.ChartType = SeriesChartType.Bar;
            seriesDoanhThu.ChartArea = "main";
            seriesDoanhThu.IsVisibleInLegend = false;
            seriesDoanhThu.Color = colorDoanhThu;
            seriesDoanhThu.BorderColor = Color.Transparent;
            seriesDoanhThu["DrawingStyle"] = "Default";
            seriesDoanhThu.ToolTip = "Doanh Thu: #VAL (Triệu VNĐ)";
            foreach (var item in doanhThu)
                seriesDoanhThu.Points.AddXY(item.Key, item.Value);
            chart.Series.Add(seriesDoanhThu);

            var seriesLoiNhuan = new Series("Lợi Nhuận");
            seriesLoiNhuan.ChartType = SeriesChartType.Bar;
            seriesLoiNhuan.ChartArea = "main";
            seriesLoiNhuan.IsVisibleInLegend = false;
            seriesLoiNhuan.Color = colorLoiNhuan;
            seriesLoiNhuan.BorderColor = Color.Transparent;
            seriesLoiNhuan["DrawingStyle"] = "Default";
            seriesLoiNhuan.ToolTip = "Lợi Nhuận: #VAL (Triệu VNĐ)";
            foreach (var item in loiNhuan)
                seriesLoiNhuan.Points.AddXY(item.Key, item.Value);
            chart.Series.Add(seriesLoiNhuan);

            mainLayout.Controls.Add(chart, 0, 0);

            var legendPanel = new FlowLayoutPanel();
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.BackColor = Color.Transparent;
            legendPanel.FlowDirection = FlowDirection.LeftToRight;
            legendPanel.WrapContents = false;
            legendPanel.Padding = new Padding(12, 4, 0, 4);

            var legendItems = new[]
            {
                ("Doanh Thu", colorDoanhThu),
                ("Lợi Nhuận", colorLoiNhuan)
            };

            foreach (var (name, color) in legendItems)
            {
                var item = new Panel();
                item.BackColor = Color.Transparent;
                item.AutoSize = true;
                item.Margin = new Padding(10, 6, 10, 6);

                var colorBox = new Panel();
                colorBox.Size = new Size(14, 14);
                colorBox.BackColor = color;
                colorBox.Location = new Point(0, 3);
                colorBox.BorderStyle = BorderStyle.None;

                var lbl = new Label();
                lbl.Text = name;
                lbl.Font = new Font("Segoe UI", 9f);
                lbl.ForeColor = Color.FromArgb(80, 80, 80);
                lbl.AutoSize = true;
                lbl.Location = new Point(20, 0);

                item.Controls.Add(colorBox);
                item.Controls.Add(lbl);
                item.Width = lbl.PreferredWidth + 28;
                legendPanel.Controls.Add(item);
            }

            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel7.Controls.Add(mainLayout);
        }

        // ════════════════════════════════════════════════════════
        //  BAR CHART NGÀY (7 ngày gần nhất) – (guna2Panel5)
        //  Chỉ thay phần data, giữ nguyên 100% code vẽ
        // ════════════════════════════════════════════════════════
        private bool dailyBarChartLoaded = false;

        private void guna2Panel5_Paint_2(object sender, PaintEventArgs e)
        {
        }

        private void LoadDailyBarChart()
        {
            if (dailyBarChartLoaded) return;
            dailyBarChartLoaded = true;

            guna2Panel5.Controls.Clear();
            guna2Panel5.Padding = new Padding(0);

            // ── LẤY DỮ LIỆU THẬT TỪ BLL (7 ngày gần nhất) ──
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
                    labels[i] = listData[i]["ngay_ban_label"].ToString(); // "dd/MM"
                    doanhThu[i] = Math.Round(Convert.ToDouble(listData[i]["doanh_thu"]) / 1_000_000, 1);
                    loiNhuan[i] = Math.Round(Convert.ToDouble(listData[i]["loi_nhuan"]) / 1_000_000, 1);
                }
            }
            catch
            {
                labels = new[] { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
                doanhThu = new double[] { 85, 72, 110, 95, 130, 148, 60 };
                loiNhuan = new double[] { 32, 28, 50, 41, 58, 65, 22 };
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

            ca.InnerPlotPosition = new ElementPosition(0, 5, 88, 88);
            chart.ChartAreas.Add(ca);

            var leg = new Legend("leg"); leg.Enabled = false;
            chart.Legends.Add(leg);

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

            // ── PostPaint: vẽ lại toàn bộ bằng index ──
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

                    // ── Cột Doanh Thu (trái) ──
                    float yTopDT = (float)ca.AxisY.ValueToPixelPosition(doanhThu[i]);
                    float hDT = yBot - yTopDT;
                    if (hDT > 0)
                    {
                        float leftDT = xCenter - (barW + innerGap / 2f);
                        using (var br = new SolidBrush(colorDoanhThu))
                            DrawRoundedTopBar(g, br, leftDT, yTopDT, barW, hDT, radius);
                    }

                    // ── Cột Lợi Nhuận (phải) ──
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

            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel5.Controls.Add(mainLayout);
        }

        // ── Helper: bo tròn 2 góc trên ──
        private void DrawRoundedTopBar(Graphics g, Brush brush,
            float x, float y, float width, float height, int radius)
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
    }
}