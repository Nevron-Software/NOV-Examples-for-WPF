using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Polar range example
	/// </summary>
	public class NPolarRangeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarRangeExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarRangeExample()
		{
			NPolarRangeExampleSchema = NSchema.Create(typeof(NPolarRangeExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Range";

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
			strip.Fill = new NColorFill(new NColor(NColor.Gray, 100));
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);
			linearScale.MajorGridLines.Visible = true;

			// setup polar angle axis			
			NPolarAxis angleAxis = m_Chart.Axes[ENPolarAxis.PrimaryAngle];

			NOrdinalScale ordinalScale = new NOrdinalScale();

			strip = new NScaleStrip();
			strip.Fill = new NColorFill(new NColor(NColor.DarkGray, 100));
			strip.Interlaced = true;
			ordinalScale.Strips.Add(strip);

			ordinalScale.InflateContentRange = false;
			ordinalScale.MajorTickMode = ENMajorTickMode.CustomTicks;
			ordinalScale.DisplayDataPointsBetweenTicks = false;

			ordinalScale.MajorTickMode = ENMajorTickMode.CustomStep;
			ordinalScale.CustomStep = 1;
			string[] labels = new string[] { "E", "NE", "N", "NW", "W", "SW", "S", "SE" };

			ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(labels);
			ordinalScale.Labels.DisplayLast = false;

			angleAxis.Scale = ordinalScale;
			angleAxis.ViewRangeMode = ENAxisViewRangeMode.FixedRange;
			angleAxis.MinViewRangeValue = 0;
			angleAxis.MaxViewRangeValue = 8;

			NPolarRangeSeries polarRange = new NPolarRangeSeries();
			polarRange.DataLabelStyle = new NDataLabelStyle(false);
			m_Chart.Series.Add(polarRange);

			Random rand = new Random();

			for (int i = 0; i < 8; i++)
			{
				polarRange.DataPoints.Add(new NPolarRangeDataPoint(i - 0.4, 0.0, i + 0.4, rand.Next(80) + 20.0));
			}
			
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
			return @"<p>This example demonstrates how to create a polar range chart.</p>";
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

		public static readonly NSchema NPolarRangeExampleSchema;

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