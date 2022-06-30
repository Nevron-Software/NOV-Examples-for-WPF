using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Vector Example
	/// </summary>
	public class NStandardVectorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardVectorExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardVectorExample()
		{
			NStandardVectorExampleSchema = NSchema.Create(typeof(NStandardVectorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Vector";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// setup X axis
			NLinearScale linearScale = new NLinearScale();
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = linearScale;

			// setup Y axis
			linearScale = new NLinearScale();
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = linearScale;

			// setup shape series
			NVectorSeries vectorSeries = new NVectorSeries();
			m_Chart.Series.Add(vectorSeries);
			vectorSeries.DataLabelStyle = new NDataLabelStyle(false);
			vectorSeries.InflateMargins = false;
			vectorSeries.UseXValues = true;
			vectorSeries.MinArrowheadSize = new NSize(2, 3);
			vectorSeries.MaxArrowheadSize = new NSize(4, 6);

			// fill data
			FillData(vectorSeries);
		
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
			return @"<p>This example demonstrates how to create a standard 2D vector chart.</p>";
		}

		#endregion

		#region Implementation

		private void FillData(NVectorSeries vectorSeries)
		{
			double x = 0, y = 0;
			int k = 0;

			for (int i = 0; i < 10; i++)
			{
				x = 1;
				y += 1;

				for (int j = 0; j < 10; j++)
				{
					x += 1;

					double dx = Math.Sin(x / 3.0) * Math.Cos((x - y) / 4.0);
					double dy = Math.Cos(y / 8.0) * Math.Cos(y / 4.0);

					NColor color = ColorFromVector(dx, dy);
					vectorSeries.DataPoints.Add(new NVectorDataPoint(x, y, x + dx, y + dy, new NColorFill(color), new NStroke(1, color)));
					k++;
				}
			}		
		}
		private NColor ColorFromVector(double dx, double dy)
		{
			double length = Math.Sqrt(dx * dx + dy * dy);

			double sq2 = Math.Sqrt(2);

			int r = (int)((255 / sq2) * length);
			int g = 20;
			int b = (int)((255 / sq2) * (sq2 - length));

			return NColor.FromRGB((byte)r, (byte)g, (byte)b);
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardVectorExampleSchema;

		#endregion
	}
}
