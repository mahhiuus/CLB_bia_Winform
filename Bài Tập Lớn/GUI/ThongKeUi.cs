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
        private bool pieChartLoaded = false;

        public ThongKeUi()
        {
            InitializeComponent();
            this.Load += ThongKeUi_Load;
        }

        private void ThongKeUi_Load(object sender, EventArgs e)
        {
            LoadFakePieChart();
            LoadFakeBarChart();
            LoadDailyBarChart();
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

        private void LoadFakePieChart()
        {
            if (pieChartLoaded) return;
            pieChartLoaded = true;

            guna2Panel6.Controls.Clear();
            guna2Panel6.Padding = new Padding(0);

            var data = new Dictionary<string, double>
{
    { "Thức Ăn",  26.3 },
    { "Bàn Bìa",  57.9 },
    { "Thuê Gậy", 15.8 }
};

            Color[] colors =
            {
                ColorTranslator.FromHtml("#79ae6f"),
                ColorTranslator.FromHtml("#f0f0e8"),
                ColorTranslator.FromHtml("#2b4e23"),
            };

            // ── Layout chính: chia 2 hàng (chart trên, legend dưới) ──
            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 85f)); // chart chiếm 75%
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 15f)); // legend chiếm 25%
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ── Chart (không có legend mặc định) ──
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

            // Ẩn legend mặc định của MS Chart
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
                point.Color = colors[idx];
                point.LegendText = item.Key;
                series.Points.Add(point);
                idx++;
            }

            chart.Series.Add(series);
            mainLayout.Controls.Add(chart, 0, 0);

            // ── Legend tự vẽ: nằm ngang bên dưới ──
            var legendPanel = new FlowLayoutPanel();
            legendPanel.Dock = DockStyle.Fill;
            legendPanel.BackColor = Color.Transparent;
            legendPanel.FlowDirection = FlowDirection.LeftToRight;
            legendPanel.WrapContents = false;
            legendPanel.Padding = new Padding(8, 4, 8, 4);

            // Căn giữa theo chiều dọc
            legendPanel.AutoSize = false;

            for (int i = 0; i < data.Count; i++)
            {
                string label = data.Keys.ElementAt(i);
                double value = data.Values.ElementAt(i);
                Color color = colors[i];

                // Mỗi item legend = Panel ngang nhỏ
                var item = new Panel();
                item.BackColor = Color.Transparent;
                item.AutoSize = true;
                item.Margin = new Padding(10, 6, 10, 6);

                // Ô màu vuông
                var colorBox = new Panel();
                colorBox.Size = new Size(14, 14);
                colorBox.BackColor = color;
                colorBox.BorderStyle = BorderStyle.FixedSingle;
                colorBox.Location = new Point(0, 3);

                // Chữ label
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

            // Căn giữa legendPanel
            legendPanel.Padding = new Padding(
                (guna2Panel6.Width - (data.Count * 140)) / 2, 6, 0, 6
            );

            mainLayout.Controls.Add(legendPanel, 0, 1);
            guna2Panel6.Controls.Add(mainLayout);
        }

        private bool barChartLoaded = false;

        private void guna2Panel7_Paint(object sender, PaintEventArgs e) { }
        private void LoadFakeBarChart()
        {
            if (barChartLoaded) return;
            barChartLoaded = true;

            guna2Panel7.Controls.Clear();
            guna2Panel7.Padding = new Padding(0);

            var doanhThu = new Dictionary<string, double>
    {
        { "T1", 85 }, { "T2", 72 }, { "T3", 95 },
        { "T4", 110 }, { "T5", 88 }, { "T6", 120 }
    };

            var loiNhuan = new Dictionary<string, double>
    {
        { "T1", 32 }, { "T2", 28 }, { "T3", 41 },
        { "T4", 50 }, { "T5", 35 }, { "T6", 55 }
    };

            Color colorDoanhThu = ColorTranslator.FromHtml("#2b4e23");
            Color colorLoiNhuan = ColorTranslator.FromHtml("#79ae6f");

            // ── Layout: chart trên, legend dưới ──
            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 1;
            mainLayout.ColumnCount = 2;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 80f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ── Chart ──
            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var chartArea = new ChartArea("main");
            chartArea.BackColor = Color.Transparent;
            chartArea.BorderColor = Color.Transparent;
            chartArea.BorderDashStyle = ChartDashStyle.NotSet;

            // Trục X (giá trị - nằm dưới)
            chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(120, 120, 120);
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisX.LineColor = Color.FromArgb(220, 220, 220);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(235, 235, 235);
            chartArea.AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            // Trục Y (nhãn tháng - bên trái)
            chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(120, 120, 120);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9f);
            chartArea.AxisY.LineColor = Color.Transparent;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            chart.ChartAreas.Add(chartArea);

            // Ẩn legend mặc định
            var legend = new Legend("legend");
            legend.Enabled = false;
            chart.Legends.Add(legend);

            // Series Doanh Thu
            var seriesDoanhThu = new Series("Doanh Thu");
            seriesDoanhThu.ChartType = SeriesChartType.Bar; // Bar = nằm ngang
            seriesDoanhThu.ChartArea = "main";
            seriesDoanhThu.IsVisibleInLegend = false;
            seriesDoanhThu.Color = colorDoanhThu;
            seriesDoanhThu.BorderColor = Color.Transparent;
            seriesDoanhThu["DrawingStyle"] = "Default";
            seriesDoanhThu.ToolTip = "Doanh Thu: #VAL (Triệu VNĐ)";
            foreach (var item in doanhThu)
                seriesDoanhThu.Points.AddXY(item.Key, item.Value);
            chart.Series.Add(seriesDoanhThu);

            // Series Lợi Nhuận
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

            // ── Legend tự vẽ nằm ngang bên dưới ──
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


        private bool dailyBarChartLoaded = false;

        private void guna2Panel5_Paint_2(object sender, PaintEventArgs e)
        {
            LoadDailyBarChart();
        }

        private void LoadDailyBarChart()
        {
            if (dailyBarChartLoaded) return;
            dailyBarChartLoaded = true;

            guna2Panel5.Controls.Clear();
            guna2Panel5.Padding = new Padding(0);

            // ── Dữ liệu ──
            string[] labels = { "T2", "T3", "T4", "T5", "T6", "T7", "CN" };
            double[] doanhThu = { 85, 72, 110, 95, 130, 148, 60 };
            double[] loiNhuan = { 32, 28, 50, 41, 58, 65, 22 };
            int count = labels.Length;

            Color colorDoanhThu = ColorTranslator.FromHtml("#2b4e23");
            Color colorLoiNhuan = ColorTranslator.FromHtml("#79ae6f");

            // ── Layout: chart 82% | legend 18% ──
            var mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.RowCount = 2;
            mainLayout.ColumnCount = 1;
            mainLayout.BackColor = Color.Transparent;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 88f));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 12f));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ── Chart ──
            var chart = new Chart();
            chart.Dock = DockStyle.Fill;
            chart.BackColor = Color.Transparent;
            chart.BorderlineColor = Color.Transparent;
            chart.BorderlineDashStyle = ChartDashStyle.NotSet;

            var ca = new ChartArea("daily");
            ca.BackColor = Color.Transparent;
            ca.BorderColor = Color.Transparent;
            ca.BorderDashStyle = ChartDashStyle.NotSet;

            // Trục X — nhãn ngày
            ca.AxisX.LabelStyle.ForeColor = Color.FromArgb(130, 130, 130);
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 8.5f);
            ca.AxisX.LineColor = Color.FromArgb(210, 210, 210);
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisX.MajorTickMark.Enabled = false;
            ca.AxisX.Interval = 1;

            // Trục Y
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

            // ── Series — Transparent để ẩn cột gốc ──
            // MS Chart cần ít nhất 1 series có data để vẽ trục X đúng
            var sDT = new Series("Doanh Thu");
            sDT.ChartType = SeriesChartType.Column;
            sDT.ChartArea = "daily";
            sDT.IsVisibleInLegend = false;
            sDT.Color = Color.Transparent;
            sDT.BorderColor = Color.Transparent;
            sDT["PointWidth"] = "0.85"; // rộng để chiếm không gian, PostPaint vẽ đè
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

                // Khoảng cách pixel giữa 2 điểm liên tiếp trên trục X
                // Category axis: điểm đầu = 1.0, điểm cuối = count
                float x0 = (float)ca.AxisX.ValueToPixelPosition(1.0);
                float x1 = (float)ca.AxisX.ValueToPixelPosition(2.0);
                float unitPx = Math.Abs(x1 - x0);

                float innerGap = 6f;                              // gap giữa 2 cột cùng nhóm
                float totalBarW = unitPx * 0.80f;                  // 2 cột chiếm 80% slot
                float barW = (totalBarW - innerGap) / 2f;     // mỗi cột

                float yBot = (float)ca.AxisY.ValueToPixelPosition(0);
                int radius = 3;

                for (int i = 0; i < count; i++)
                {
                    // XValue của category axis = 1-based index
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

            // ── Legend ──
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

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel11_Click(object sender, EventArgs e)
        {

        }
    }
}