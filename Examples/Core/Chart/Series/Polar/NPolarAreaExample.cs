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
	public class NPolarAreaExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarAreaExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarAreaExample()
		{
			NPolarAreaExampleSchema = NSchema.Create(typeof(NPolarAreaExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Area";

			// configure chart
			m_Chart = (NPolarChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedPolarAxes(ENPredefinedPolarAxes.AngleValue);

			// setup polar axis
			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENPolarAxis.PrimaryValue].Scale;
			linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			linearScale.InflateViewRangeBegin = true;
			linearScale.InflateViewRangeEnd = true;
			linearScale.MajorGridLines.Stroke = new NStroke(1, NColor.Black);

			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.FromColor(NColor.Beige, 0.5f));
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// setup polar angle axis
			NAngularScale angularScale = (NAngularScale)m_Chart.Axes[ENPolarAxis.PrimaryAngle].Scale;

			strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.FromRGBA(192, 192, 192, 125));
			strip.Interlaced = true;
			angularScale.Strips.Add(strip);

			// polar area series 1
			NPolarAreaSeries series1 = new NPolarAreaSeries();
			m_Chart.Series.Add(series1);
			series1.Name = "Theoretical";
			series1.DataLabelStyle = new NDataLabelStyle(false);
			GenerateData(series1, 100, 15.0);

			// polar area series 2
			NPolarAreaSeries series2 = new NPolarAreaSeries();
			m_Chart.Series.Add(series2);
			series2.Name = "Experimental";
			series2.DataLabelStyle = new NDataLabelStyle(false);
			GenerateData(series2, 100, 10.0);

			// apply style sheet
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
			return @"<p>This example demonstrates how to create a polar area chart.</p>";
		}

		#endregion

		#region Event Handlers

	

		#endregion

		#region Implementation

		private void GenerateData(NPolarAreaSeries series, int count, double scale)
		{
			series.DataPoints.Clear();

			double angleStep = 2 * Math.PI / count;
			Random random = new Random();

			for (int i = 0; i < count; i++)
			{
				double angle = i * angleStep;
				double c1 = 1.0 * Math.Sin(angle * 3);
				double c2 = 0.3 * Math.Sin(angle * 1.5);
				double c3 = 0.05 * Math.Cos(angle * 26);
				double c4 = 0.05 * (0.5 - random.NextDouble());
				double value = scale * (Math.Abs(c1 + c2 + c3) + c4);

				if (value < 0)
					value = 0;

				series.DataPoints.Add(new NPolarAreaDataPoint(angle * 180 / Math.PI, value));
			}
		}

		#endregion

		#region Fields

		NPolarChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NPolarAreaExampleSchema;

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