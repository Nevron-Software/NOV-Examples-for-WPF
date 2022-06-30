using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Stick Stock Example
	/// </summary>
	public class NStickStockExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStickStockExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStickStockExample()
		{
			NStickStockExampleSchema = NSchema.Create(typeof(NStickStockExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Stick Stock";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NCartesianAxis axis = m_Chart.Axes[ENCartesianAxis.PrimaryX];

			axis.Scale = new NValueTimelineScale();

			// setup primary Y axis
			axis = m_Chart.Axes[ENCartesianAxis.PrimaryY];
			NLinearScale linearScale = (NLinearScale)axis.Scale;

			// configure ticks and grid lines
			linearScale.MajorGridLines.Stroke = new NStroke(NColor.LightGray);
			linearScale.InnerMajorTicks.Visible = false;

			// add interlaced stripe 
			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.Beige);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// Setup the stock series
			m_Stock = new NStockSeries();
			m_Chart.Series.Add(m_Stock);

			m_Stock.DataLabelStyle = new NDataLabelStyle(false);
			m_Stock.CandleShape = ENCandleShape.Stick;
			m_Stock.CandleWidth = 4;
			m_Stock.UseXValues = true;

			GenerateData();

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
			return @"<p>This example demonstrates how to create a stick stock chart.</p>";
		}

		#endregion

		#region Implementation

		void GenerateData()
		{
			// generate data for 30 weeks
			DateTime dtNow = DateTime.Now;
			DateTime dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 7, 0, 0, 0);
			DateTime dtStart = NDateTimeUnit.Week.Add(dtEnd, -30);
			NDateTimeSpan span = new NDateTimeSpan(1, NDateTimeUnit.Day);

			long count = span.GetSpanCountInRange(new NDateTimeRange(dtStart, dtEnd));

			double open, high, low, close;

			m_Stock.DataPoints.Clear();
			Random random = new Random();

			double prevClose = 100;

			for (int nIndex = 0; nIndex < count; nIndex++)
			{
				open = prevClose;

				if (prevClose < 25 || random.NextDouble() > 0.5)
				{
					// upward price change
					close = open + (2 + (random.NextDouble() * 20));
					high = close + (random.NextDouble() * 10);
					low = open - (random.NextDouble() * 10);
				}
				else
				{
					// downward price change
					close = open - (2 + (random.NextDouble() * 20));
					high = open + (random.NextDouble() * 10);
					low = close - (random.NextDouble() * 10);
				}

				if (low < 1)
				{
					low = 1;
				}

				prevClose = close;

				m_Stock.DataPoints.Add(new NStockDataPoint(NDateTimeHelpers.ToOADate(dtNow), open, close, high, low));
				dtNow = span.Add(dtNow);
			}
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Fields

		NStockSeries m_Stock;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NStickStockExampleSchema;

		#endregion
	}
}
