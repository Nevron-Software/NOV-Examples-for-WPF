using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Interactive Legend Example
	/// </summary>
	public class NInteractiveLegendExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NInteractiveLegendExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NInteractiveLegendExample()
		{
			NInteractiveLegendExampleSchema = NSchema.Create(typeof(NInteractiveLegendExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Interactive Legend";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			linearScale.Strips.Add(stripStyle);

			// add the first bar
            NBarSeries bar1 = new NBarSeries();
			bar1.Name = "Bar1";
			bar1.MultiBarMode = ENMultiBarMode.Series;
            bar1.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			chart.Series.Add(bar1);

			// add the second bar
            NBarSeries bar2 = new NBarSeries();
			bar2.Name = "Bar2";
			bar2.MultiBarMode = ENMultiBarMode.Stacked;
            bar2.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			chart.Series.Add(bar2);

			// add the third bar
            NBarSeries bar3 = new NBarSeries();
			bar3.Name = "Bar3";
			bar3.MultiBarMode = ENMultiBarMode.Stacked;
            bar3.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			chart.Series.Add(bar3);

			// setup value formatting
			bar1.ValueFormatter = new NNumericValueFormatter("0.###");
			bar2.ValueFormatter = new NNumericValueFormatter("0.###");
			bar3.ValueFormatter = new NNumericValueFormatter("0.###");

			// position data labels in the center of the bars
			bar1.DataLabelStyle = CreateDataLabelStyle();
			bar2.DataLabelStyle = CreateDataLabelStyle();
			bar3.DataLabelStyle = CreateDataLabelStyle();

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			// pass some random data
            Random random = new Random();
            for (int i = 0; i < 12; i++)
            {
                bar1.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
                bar2.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
                bar3.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
            }


			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);
			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create an interactive legend.</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates a new data label style object
		/// </summary>
		/// <returns></returns>
		private NDataLabelStyle CreateDataLabelStyle()
		{
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.VertAlign = ENVerticalAlignment.Center;
			dataLabelStyle.ArrowLength = 0;

			return dataLabelStyle;
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Fields


		#endregion

		#region Schema

		public static readonly NSchema NInteractiveLegendExampleSchema;

		#endregion
	}
}