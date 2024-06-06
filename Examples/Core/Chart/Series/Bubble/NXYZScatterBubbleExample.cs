using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Bubble Example
	/// </summary>
	public class NXYZScatterBubbleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZScatterBubbleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZScatterBubbleExample()
		{
			NXYZScatterBubbleExampleSchema = NSchema.Create(typeof(NXYZScatterBubbleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Scatter Bubble";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);

            // setup chart
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 50;
            m_Chart.ModelDepth = 50;
            m_Chart.ModelHeight = 50;
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            NLinearScale depthScale = new NLinearScale();
            depthScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
            depthScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            depthScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            m_Chart.Axes[ENCartesianAxis.Depth].Scale = depthScale;

            NLinearScale yScale = new NLinearScale();
            yScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
            yScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            yScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

            // add interlace style
            NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            strip.SetShowAtWall(ENChartWall.Back, true);
            strip.SetShowAtWall(ENChartWall.Left, true);
            strip.Interlaced = true;
            yScale.Strips.Add(strip);
            m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = yScale;

            // switch the x axis in linear mode
            NLinearScale xScale = new NLinearScale();
			xScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
            xScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            xScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = xScale;

			m_Bubble = new NBubbleSeries();
			m_Chart.Series.Add(m_Bubble);
            m_Bubble.InflateMargins = true;
            m_Bubble.Shape = ENPointShape3D.Sphere;
            m_Bubble.LegendView.Format = "x:<xvalue> y:<value> z:<zvalue> sz:<size>";
            m_Bubble.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            m_Bubble.MinSize = 20;
            m_Bubble.MaxSize = 100;
            m_Bubble.UseXValues = true;
            m_Bubble.UseZValues = true;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

            OnChangeDataButtonClick(null);

            return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NButton changeDataButton = new NButton("Change Data");
			changeDataButton.Click += new Function<NEventArgs>(OnChangeDataButtonClick);
			stack.Add(changeDataButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter bubble chart.</p>";
		}

        #endregion

        #region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
        void OnChangeDataButtonClick(NEventArgs arg)
        {
            Random random = new Random();
			m_Bubble.DataPoints.Clear();

            for (int i = 0; i < 6; i++)
            {
				NBubbleDataPoint dataPoint = new NBubbleDataPoint();

                dataPoint.Value = random.Next(-100, 100);
                dataPoint.X = random.Next(-100, 100);
                dataPoint.Z = random.Next(-100, 100);
				dataPoint.Size = random.NextDouble();

				m_Bubble.DataPoints.Add(dataPoint);
            }
        }

        #endregion

        #region Fields

        NBubbleSeries m_Bubble;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NXYZScatterBubbleExampleSchema;

		#endregion
	}
}