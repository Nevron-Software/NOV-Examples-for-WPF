using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Line Example
	/// </summary>
	public class NStandardLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardLineExample()
		{
			NStandardLineExampleSchema = NSchema.Create(typeof(NStandardLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Line";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlaced stripe to the Y axis
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			m_Line = new NLineSeries();
			m_Line.Name = "Line Series";
			m_Line.InflateMargins = true;
			m_Line.DataLabelStyle = new NDataLabelStyle("<value>");
			m_Line.MarkerStyle = new NMarkerStyle(new NSize(4, 4));

			Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_Line.DataPoints.Add(new NLineDataPoint(random.Next(80) + 20));
			}

			m_Chart.Series.Add(m_Line);

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
			return @"<p>This example demonstrates how to create a standard line chart.</p>";
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NStandardLineExampleSchema;

		#endregion
	}
}
