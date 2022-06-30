using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Polar vector example
	/// </summary>
	public class NPolarVectorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPolarVectorExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPolarVectorExample()
		{
			NPolarVectorExampleSchema = NSchema.Create(typeof(NPolarVectorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePolarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Polar Vector";

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

			// create a polar line series
			NPolarVectorSeries vectorSeries = new NPolarVectorSeries();
			m_Chart.Series.Add(vectorSeries);
			vectorSeries.Name = "Series 1";
			vectorSeries.DataLabelStyle = new NDataLabelStyle(false);

			for (int i = 0; i < 360; i += 30)
			{
				for (int j = 10; j <= 100; j += 10)
				{
					vectorSeries.DataPoints.Add(new NPolarVectorDataPoint(i, j, i + j / 10, j, null, new NStroke(1, ColorFromValue(j)))); 
				}
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
			return @"<p>This example demonstrates how to create a polar line chart.</p>";
		}

		#endregion

		#region Implementation


		#endregion

		#region Fields

		NPolarChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NPolarVectorExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreatePolarChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Polar);
			return chartView;
		}
		private static NColor ColorFromValue(double value)
		{
			return NColor.InterpolateColors(NColor.Red, NColor.Blue, value / 100.0);
		}

		#endregion
	}
}