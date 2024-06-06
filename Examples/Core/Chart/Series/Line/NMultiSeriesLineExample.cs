using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Multi Series Line Example
    /// </summary>
    public class NMultiSeriesLineExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NMultiSeriesLineExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NMultiSeriesLineExample()
        {
            NMultiSeriesLineExampleSchema = NSchema.Create(typeof(NMultiSeriesLineExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Multi Series Line Chart";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup chart
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 60;
            m_Chart.ModelHeight = 25;
            m_Chart.ModelDepth = 45;
            m_Chart.Projection.ProjectionType = ENProjectionType.Perspective;
            m_Chart.Projection.Elevation = 28;
            m_Chart.Projection.Rotation = -17;
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);

            // add interlaced stripe to the Y axis
            NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            strip.SetShowAtWall(ENChartWall.Back, true);
            strip.SetShowAtWall(ENChartWall.Left, true);
            strip.Interlaced = true;
            ((NStandardScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).Strips.Add(strip);

            // show the X axis gridlines
            NOrdinalScale ordinalScale = m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale as NOrdinalScale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);

            // add the first line
            m_Line1 = new NLineSeries();
            m_Chart.Series.Add(m_Line1);
            m_Line1.MultiLineMode = ENMultiLineMode.Series;
            m_Line1.LineSegmentShape = ENLineSegmentShape.Tape;
            m_Line1.DepthPercent = 50;
            m_Line1.Name = "Line 1";

            // add the second line
            m_Line2 = new NLineSeries();
            m_Chart.Series.Add(m_Line2);
            m_Line2.MultiLineMode = ENMultiLineMode.Series;
            m_Line2.LineSegmentShape = ENLineSegmentShape.Tape;
            m_Line2.DepthPercent = 50;
            m_Line2.Name = "Line 2";

            // add the third line
            m_Line3 = new NLineSeries();
            m_Chart.Series.Add(m_Line3);
            m_Line3.MultiLineMode = ENMultiLineMode.Series;
            m_Line3.LineSegmentShape = ENLineSegmentShape.Tape;
            m_Line3.DepthPercent = 50;
            m_Line3.Name = "Line 3";

            GenerateData();

            // apply style sheet
            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Fresh, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
        }

        private void GenerateData()
        {
            Random random = new Random();

            for (int i = 0; i < 7; i++)
            {
                m_Line1.DataPoints.Add(new NLineDataPoint(random.Next(100)));
                m_Line2.DataPoints.Add(new NLineDataPoint(random.Next(100)));
                m_Line3.DataPoints.Add(new NLineDataPoint(random.Next(100)));
            }
        }

        private void NewDataButton_Click(object sender, System.EventArgs e)
        {
            GenerateData();
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NHScrollBar chartDepthScrollBar = new NHScrollBar();
            chartDepthScrollBar.Value = m_Chart.ModelDepth;
            chartDepthScrollBar.ValueChanged += ChartDepthScrollBar_ValueChanged;
            stack.Add(NPairBox.Create("Chart Depth:", chartDepthScrollBar));

            NHScrollBar lineDepthScrollBar = new NHScrollBar();
            lineDepthScrollBar.Value = m_Line1.DepthPercent;
            lineDepthScrollBar.ValueChanged += LineDepthScrollBar_ValueChanged;
            stack.Add(NPairBox.Create("Line Depth:", lineDepthScrollBar));

            return group;
        }

        private void LineDepthScrollBar_ValueChanged(NValueChangeEventArgs arg)
        {
            float depthPercent = System.Convert.ToSingle(arg.NewValue);

            m_Line1.DepthPercent = depthPercent;
            m_Line2.DepthPercent = depthPercent;
            m_Line3.DepthPercent = depthPercent;
        }

        private void ChartDepthScrollBar_ValueChanged(NValueChangeEventArgs arg)
        {
            m_Chart.ModelDepth = System.Convert.ToSingle(arg.NewValue);
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to create a multi-series line chart where lines occupy a specified percentage of their category.</p>";
        }

        #endregion

        #region Fields

        NCartesianChart m_Chart;
        NLineSeries m_Line1;
        NLineSeries m_Line2;
        NLineSeries m_Line3;

        #endregion

        #region Schema

        public static readonly NSchema NMultiSeriesLineExampleSchema;

        #endregion
    }
}
