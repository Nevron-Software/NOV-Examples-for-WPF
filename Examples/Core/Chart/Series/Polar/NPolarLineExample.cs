using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Polar area example
	/// </summary>
	public class NPolarLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarLineExample()
		{
			NPolarLineExampleSchema = NSchema.Create(typeof(NPolarLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Line";

			// configure chart
			m_Chart = (NPolarChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedPolarAxes(ENPredefinedPolarAxes.AngleValue);

			// setup polar axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENPolarAxis.PrimaryValue].Scale;
			linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			linearScale.InflateViewRangeBegin = true;
			linearScale.InflateViewRangeEnd = true;
			linearScale.MajorGridLines.Visible = true;

			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(new NColor(NColor.Beige, 125));
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// setup polar angle axis
			NAngularScale angularScale = (NAngularScale)m_Chart.Axes[ENPolarAxis.PrimaryAngle].Scale;
			strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.FromRGBA(192, 192, 192, 125));
			strip.Interlaced = true;
			angularScale.Strips.Add(strip);

			// add a const line
			NAxisReferenceLine line = new NAxisReferenceLine();
			m_Chart.Axes[ENPolarAxis.PrimaryValue].ReferenceLines.Add(line);
			line.Value = 50;
			line.Stroke = new NStroke(1, NColor.SlateBlue);

			// create a polar line series
			NPolarLineSeries series1 = new NPolarLineSeries();
			m_Chart.Series.Add(series1);
			series1.Name = "Series 1";
			series1.CloseContour = true;
			series1.DataLabelStyle = new NDataLabelStyle(false);
			Curve1(series1, 50);

			// create a polar line series
			NPolarLineSeries series2 = new NPolarLineSeries();
			m_Chart.Series.Add(series2);
			series2.Name = "Series 2";
			series2.DataLabelStyle = new NDataLabelStyle(false);
			series2.CloseContour = true;

			Curve2(series2, 100);

			// create a polar line series
			NPolarLineSeries series3 = new NPolarLineSeries();
			m_Chart.Series.Add(series3);
			series3.Name = "Series 3";
			series3.CloseContour = false;
			series3.DataLabelStyle = new NDataLabelStyle(false);
			Curve3(series3, 100);
			
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
			return @"<p>This example demonstrates how to create a polar line chart.</p>";
		}

		#endregion

		#region Event Handlers



		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="series"></param>
		/// <param name="count"></param>
		internal static void Curve1(NPolarLineSeries series, int count)
		{
			series.DataPoints.Clear();

			double angleStep = 2 * Math.PI / count;

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double radius = 1 + Math.Cos(angle);

				series.DataPoints.Add(new NPolarLineDataPoint(angle * 180 / Math.PI, radius));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="series"></param>
		/// <param name="count"></param>
		internal static void Curve2(NPolarLineSeries series, int count)
		{
			series.DataPoints.Clear();

			double angleStep = 2 * Math.PI / count;

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double radius = 0.2 + 1.7 * Math.Sin(2 * angle) + 1.7 * Math.Cos(2 * angle);

				radius = Math.Abs(radius);

				series.DataPoints.Add(new NPolarLineDataPoint(angle * 180 / Math.PI, radius));
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="series"></param>
		/// <param name="count"></param>
		internal static void Curve3(NPolarLineSeries series, int count)
		{
			series.DataPoints.Clear();

			double angleStep = 4 * Math.PI / count;

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double radius = 0.2 + angle / 5.0;

				series.DataPoints.Add(new NPolarLineDataPoint(angle * 180 / Math.PI, radius));
			}
		}

		#endregion

		#region Fields

		NPolarChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NPolarLineExampleSchema;

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