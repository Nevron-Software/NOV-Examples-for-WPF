using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Range Timeline Scale Example
	/// </summary>
	public class NRangeTimelineScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRangeTimelineScaleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRangeTimelineScaleExample()
		{
			NRangeTimelineScaleExampleSchema = NSchema.Create(typeof(NRangeTimelineScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Range Timeline Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// setup X axis
			NCartesianAxis axis = m_Chart.Axes[ENCartesianAxis.PrimaryX];

			m_RangeTimelineScale = new NRangeTimelineScale();
			axis.Scale = m_RangeTimelineScale;

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

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			OnWeeklyDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox firstRowVisibleCheckBox = new NCheckBox("First Row Visible");
			firstRowVisibleCheckBox.Click += new Function<NEventArgs>(OnFirstRowVisibleCheckBoxClick);
			stack.Add(firstRowVisibleCheckBox);

			NCheckBox secondRowVisibleCheckBox = new NCheckBox("Second Row Visible");
			secondRowVisibleCheckBox.Click += new Function<NEventArgs>(OnSecondRowVisibleCheckBoxClick);
			stack.Add(secondRowVisibleCheckBox);

			NCheckBox thirdRowVisibleCheckBox = new NCheckBox("Third Row Visible");
			thirdRowVisibleCheckBox.Click += new Function<NEventArgs>(OnThirdRowVisibleCheckBoxClick);
			stack.Add(thirdRowVisibleCheckBox);

			NButton dailyDataButton = new NButton("Daily Data");
			dailyDataButton.Click += new Function<NEventArgs>(OnDailyDataButtonClick);
			stack.Add(dailyDataButton);

			NButton weeklyDataButton = new NButton("Weekly Data");
			weeklyDataButton.Click += new Function<NEventArgs>(OnWeeklyDataButtonClick);
			stack.Add(weeklyDataButton);

			NButton monthlyDataButton = new NButton("Monthly Data");
			monthlyDataButton.Click += new Function<NEventArgs>(OnMonthlyDataButtonClick);
			stack.Add(monthlyDataButton);

			NButton yearlyDataButton = new NButton("Yearly Data");
			yearlyDataButton.Click += new Function<NEventArgs>(OnYearlyDataButtonClick);
			stack.Add(yearlyDataButton);

			firstRowVisibleCheckBox.Checked = true;
			secondRowVisibleCheckBox.Checked = true;
			thirdRowVisibleCheckBox.Checked = true;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a range timeline scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnThirdRowVisibleCheckBoxClick(NEventArgs arg)
		{
			m_RangeTimelineScale.FirstRow.Visible = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnSecondRowVisibleCheckBoxClick(NEventArgs arg)
		{
			m_RangeTimelineScale.SecondRow.Visible = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnFirstRowVisibleCheckBoxClick(NEventArgs arg)
		{
			m_RangeTimelineScale.ThirdRow.Visible = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnYearlyDataButtonClick(NEventArgs arg)
		{
			// generate data for 30 years
			DateTime dtNow = DateTime.Now;
			DateTime dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 7, 0, 0, 0);
			DateTime dtStart = NDateTimeUnit.Year.Add(dtEnd, -30);

			GenerateData(dtStart, dtEnd, new NDateTimeSpan(1, NDateTimeUnit.Month));
		}

		void OnMonthlyDataButtonClick(NEventArgs arg)
		{
			// generate data for 30 months 
			DateTime dtNow = DateTime.Now;
			DateTime dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 7, 0, 0, 0);
			DateTime dtStart = NDateTimeUnit.Month.Add(dtEnd, -30);

			GenerateData(dtStart, dtEnd, new NDateTimeSpan(1, NDateTimeUnit.Week));
		}

		void OnWeeklyDataButtonClick(NEventArgs arg)
		{
			// generate data for 30 weeks
			DateTime dtNow = DateTime.Now;
			DateTime dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 7, 0, 0, 0);
			DateTime dtStart = NDateTimeUnit.Week.Add(dtEnd, -30);

			GenerateData(dtStart, dtEnd, new NDateTimeSpan(1, NDateTimeUnit.Day));
		}

		void OnDailyDataButtonClick(NEventArgs arg)
		{
			// generate data for 30 days
			DateTime dtNow = DateTime.Now;
			DateTime dtEnd = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 17, 0, 0, 0);
			DateTime dtStart = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 7, 0, 0, 0);

			GenerateData(dtStart, dtEnd, new NDateTimeSpan(5, NDateTimeUnit.Minute));
		}

		#endregion

		#region Implementation

		private void GenerateData(DateTime dtStart, DateTime dtEnd, NDateTimeSpan span)
		{
			long count = span.GetSpanCountInRange(new NDateTimeRange(dtStart, dtEnd));

			double open, high, low, close;

			m_Stock.DataPoints.Clear();
			Random random = new Random();
			DateTime dtNow = dtStart;

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

		#region Fields

		NCartesianChart m_Chart;
		NStockSeries m_Stock;
		NRangeTimelineScale m_RangeTimelineScale;

		#endregion

		#region Schema

		public static readonly NSchema NRangeTimelineScaleExampleSchema;

		#endregion
	}
}