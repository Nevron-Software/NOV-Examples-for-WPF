using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Wafer Chart Example
	/// </summary>
	public class NWaferChartExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NWaferChartExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NWaferChartExample()
		{
			NWaferChartExampleSchema = NSchema.Create(typeof(NWaferChartExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Heat Maps";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			NHeatMapSeries heatMap = new NHeatMapSeries();
			chart.Series.Add(heatMap);

			NGridData data = heatMap.Data;

			heatMap.Palette = new NTwoColorPalette(NColor.Green, NColor.Red);

			int gridSizeX = 100;
			int gridSizeY = 100;
			data.Size = new NSizeI(gridSizeX, gridSizeY);

			int centerX = gridSizeX / 2;
			int centerY = gridSizeY / 2;

			int radius = gridSizeX / 2;
			Random rand = new Random();

			for (int y = 0; y < gridSizeY; y++)
			{
				for (int x = 0; x < gridSizeX; x++)
				{
					int dx = x - centerX;
					int dy = y - centerY;

					double pointDistance = Math.Sqrt(dx * dx + dy * dy);

					if (pointDistance < radius)
					{
						// assign value
						data.SetValue(x, y, pointDistance + rand.Next(20));
					}
					else
					{
						data.SetValue(x, y, double.NaN);
					}
				}
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
			return @"<p>This example demonstrates how to create a standard bar chart.</p>";
		}

		#endregion

		#region Event Handlers

		#endregion

		#region Fields


		#endregion

		#region Schema

		public static readonly NSchema NWaferChartExampleSchema;

		#endregion
	}
}