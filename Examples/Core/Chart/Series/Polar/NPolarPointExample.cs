using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Polar point example
	/// </summary>
	public class NPolarPointExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarPointExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarPointExample()
		{
			NPolarPointExampleSchema = NSchema.Create(typeof(NPolarPointExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Point";

			// configure chart
			m_Chart = (NPolarChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedPolarAxes(ENPredefinedPolarAxes.AngleValue);

			// setup polar axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENPolarAxis.PrimaryValue].Scale;
			linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			linearScale.InflateViewRangeBegin = true;
			linearScale.InflateViewRangeEnd = true;

			linearScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.FromColor(NColor.Cyan, 0.4f));
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);
            linearScale.MajorGridLines.Visible = true;

			// setup polar angle axis
			NAngularScale angularScale = (NAngularScale)m_Chart.Axes[ENPolarAxis.PrimaryAngle].Scale;
			strip = new NScaleStrip();
            strip.Fill = new NColorFill(NColor.FromRGBA(192, 192, 192, 125));
			strip.Interlaced = true;

			angularScale.Strips.Add(strip);
            angularScale.MajorGridLines.Visible = true;

			// create three polar point series
            Random random = new Random();
            NSeries s1 = CreatePolarPointSeries("Sample 1", ENPointShape.Ellipse, random);
			NSeries s2 = CreatePolarPointSeries("Sample 2", ENPointShape.Rectangle, random);
            NSeries s3 = CreatePolarPointSeries("Sample 3", ENPointShape.Triangle, random);

			// add the series to the chart
			m_Chart.Series.Add(s1);
			m_Chart.Series.Add(s2);
			m_Chart.Series.Add(s3);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			


			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a polar point chart.</p>";
		}

		#endregion

		#region Event Handlers

	

		#endregion

		#region Implementation

        private NSeries CreatePolarPointSeries(string name, ENPointShape shape, Random random)
		{
			NPolarPointSeries series = new NPolarPointSeries();
			series.Name = name;
            series.InflateMargins = false;

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = false;
			dataLabelStyle.Format = "<value> - <angle_in_degrees>";
			series.DataLabelStyle = dataLabelStyle;
			series.Shape = shape;
			series.Size = 3.0;

			// add data
			for (int i = 0; i < 1000; i++)
			{
				series.DataPoints.Add(new NPolarPointDataPoint(random.Next(360), random.Next(100)));
			}

			return series;
		}

		#endregion

		#region Fields

		NPolarChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NPolarPointExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreatePolarChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Polar);
			return chartView;
		}

		#endregion
	}
}