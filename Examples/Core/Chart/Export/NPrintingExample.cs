using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Export;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Pdf Export Example
	/// </summary>
	public class NPrintingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPrintingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPrintingExample()
		{
			NPrintingExampleSchema = NSchema.Create(typeof(NPrintingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChartView = new NChartView();
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Printing Example";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NLinearScale xScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			xScale.MajorGridLines.Visible = true;

			// setup Y axis
			NLinearScale yScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.MajorGridLines.Visible = true;
			
			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			// setup shape series
			NRangeSeries range = new NRangeSeries();
			chart.Series.Add(range);

			range.DataLabelStyle = new NDataLabelStyle(false);
			range.UseXValues = true;
			range.Fill = new NColorFill(NColor.DarkOrange);
			range.Stroke = new NStroke(NColor.DarkRed);

			// fill data
			double[] intervals = new double[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 30, 60 };
			double[] values = new double[] { 4180, 13687, 18618, 19634, 17981, 7190, 16369, 3212, 4122, 9200, 6461, 3435 };

			int count = Math.Min(intervals.Length, values.Length);
			double x = 0;

			for (int i = 0; i < count; i++)
			{
				double interval = intervals[i];
				double value = values[i];

				double x1 = x;
				double y1 = 0;

				x += interval;
				double x2 = x;
				double y2 = value / interval;

				range.DataPoints.Add(new NRangeDataPoint(x1, y1, x2, y2));
			}

			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return m_ChartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

			NButton printButton = new NButton("Print...");
			printButton.Click += OnPrintButtonClick;
			stack.Add(printButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to print the chart.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnPrintButtonClick(NEventArgs arg)
		{
			NChartPrintExporter printExporter = new NChartPrintExporter(m_ChartView.Document);
			printExporter.Print(new NRectangle(0, 0, m_ChartView.Size.Width, m_ChartView.Size.Height));
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;

		#endregion

		#region Schema

		public static readonly NSchema NPrintingExampleSchema;

		#endregion
	}
}
