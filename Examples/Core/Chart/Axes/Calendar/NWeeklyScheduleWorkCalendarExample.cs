using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Weekly Schdule Wrok Calendar Example
	/// </summary>
	public class NWeeklyScheduleWorkCalendarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NWeeklyScheduleWorkCalendarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NWeeklyScheduleWorkCalendarExample()
		{
			NWeeklyScheduleWorkCalendarExampleSchema = NSchema.Create(typeof(NWeeklyScheduleWorkCalendarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Weekly Schedule Work Calendar";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			NRangeSeries ranges = new NRangeSeries();
			m_Chart.Series.Add(ranges);

			ranges.DataLabelStyle = new NDataLabelStyle(false);
			ranges.UseXValues = true;

			DateTime dt = new DateTime(2014, 4, 14);
			Random rand = new Random();

			NRangeTimelineScale rangeTimeline = new NRangeTimelineScale();
			rangeTimeline.EnableCalendar = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = rangeTimeline;

			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.MajorGridLines.Visible = true;

			// add interlaced strip
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			yScale.Title.Text = "Weekly Workload in %";

			NWorkCalendar workCalendar = rangeTimeline.Calendar;

			// show only the working days on the scale
			NWeekDayRule weekDayRule = new NWeekDayRule(ENWeekDayBit.Monday | ENWeekDayBit.Tuesday | ENWeekDayBit.Wednesday | ENWeekDayBit.Thursday | ENWeekDayBit.Friday);
			workCalendar.Rules.Add(weekDayRule);

			// generate data for week working days
			for (int i = 0; i < 21; i++)
			{
				if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
				{
					ranges.DataPoints.Add(new NRangeDataPoint(NDateTimeHelpers.ToOADate(dt), 0, NDateTimeHelpers.ToOADate(dt + new TimeSpan(1, 0, 0, 0)), rand.NextDouble() * 70 + 30.0d));
				}

				dt += new TimeSpan(1, 0, 0, 0);
			}

			ConfigureInteractivity(m_Chart);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox enableWorkCalendarCheckBox = new NCheckBox("Enable Work Calendar");
			enableWorkCalendarCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableWorkCalendarCheckBoxCheckedChanged);
			enableWorkCalendarCheckBox.Checked = true;
			stack.Add(enableWorkCalendarCheckBox);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the ability of the timeline and date/time scales to skip date time ranges, when it is expected that there is no data for them. Common applications of this feature are financial charts that usually display only working week days as stock markets are closed on weekends.</p>";
		}

		#endregion

		#region Event Handlers

		void OnEnableWorkCalendarCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			((NRangeTimelineScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale).EnableCalendar = (arg.TargetNode as NCheckBox).Checked;
		}

		#endregion

		#region Implementation

		private void ConfigureInteractivity(NChart chart)
		{
			NInteractor interactor = new NInteractor();

			NRectangleZoomTool rectangleZoomTool = new NRectangleZoomTool();
			rectangleZoomTool.Enabled = true;
			rectangleZoomTool.VerticalValueSnapper = new NAxisRulerMinMaxSnapper();
			interactor.Add(rectangleZoomTool);

			NDataPanTool dataPanTool = new NDataPanTool();
			dataPanTool.StartMouseButtonEvent = ENMouseButtonEvent.RightButtonDown;
			dataPanTool.EndMouseButtonEvent = ENMouseButtonEvent.RightButtonUp;
			dataPanTool.Enabled = true;
			interactor.Add(dataPanTool);

			chart.Interactor = interactor;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NWeeklyScheduleWorkCalendarExampleSchema;

		#endregion
	}
}
