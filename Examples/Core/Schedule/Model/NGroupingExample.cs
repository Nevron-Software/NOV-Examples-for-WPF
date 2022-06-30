using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NGroupingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NGroupingExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NGroupingExample()
		{
			NGroupingExampleSchema = NSchema.Create(typeof(NGroupingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple schedule
			NScheduleViewWithRibbon scheduleViewWithRibbon = new NScheduleViewWithRibbon();
			m_ScheduleView = scheduleViewWithRibbon.View;

			m_ScheduleView.Document.PauseHistoryService();
			try
			{
				InitSchedule(m_ScheduleView.Content);
			}
			finally
			{
				m_ScheduleView.Document.ResumeHistoryService();
			}

			// Return the commanding widget
			return scheduleViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			stack.Add(new NRadioButton("Time grouping first"));
			stack.Add(new NRadioButton("Group grouping first"));

			NRadioButtonGroup groupingOrderGroup = new NRadioButtonGroup(stack);
			groupingOrderGroup.SelectedIndexChanged += OnRadioGroupSelectedIndexChanged;
			groupingOrderGroup.SelectedIndex = 0;

			return groupingOrderGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to create schedule groups, how to associate a group to appointments and
	how to apply grouping to schedule view modes. Each view mode (except for ""Timeline"") has a default
	date related grouping, so you can either add a new grouping or insert it before the default one. This
	will affect the order in which the groupings are applied. Use the radio button on the right to control
	the grouping order.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			const string ActivityGroup = "Activity";
			const string Work = "Work";
			const string Rest = "Rest";
			const string Travel = "Travel";

			schedule.ViewMode = ENScheduleViewMode.Day;

			// Add a schedule group
			schedule.Groups.Add(new NScheduleGroup(ActivityGroup, new string[] { Work, Rest, Travel }));

			// Add a grouping to each of the view modes
			schedule.DayViewMode.Groupings.Add(new NGroupGrouping(ActivityGroup));
			schedule.WeekViewMode.Groupings.Add(new NGroupGrouping(ActivityGroup));
			schedule.MonthViewMode.Groupings.Add(new NGroupGrouping(ActivityGroup));
			schedule.TimelineViewMode.Groupings.Add(new NGroupGrouping(ActivityGroup));

			// Create the appointments
			DateTime today = DateTime.Today;
			schedule.Appointments.Add(CreateAppointment("Travel to Work", today.AddHours(6.5), today.AddHours(7.5), Travel));
			schedule.Appointments.Add(CreateAppointment("Meeting with John", today.AddHours(8), today.AddHours(10), Work));
			schedule.Appointments.Add(CreateAppointment("Conference", today.AddHours(10.5), today.AddHours(11.5), Work));
			schedule.Appointments.Add(CreateAppointment("Lunch", today.AddHours(12), today.AddHours(14), Rest));
			schedule.Appointments.Add(CreateAppointment("News Reading", today.AddHours(12.5), today.AddHours(13.5), Rest));
			schedule.Appointments.Add(CreateAppointment("Video Presentation", today.AddHours(14.5), today.AddHours(15.5), Work));
			schedule.Appointments.Add(CreateAppointment("Web Meeting", today.AddHours(16), today.AddHours(17), Work));
			schedule.Appointments.Add(CreateAppointment("Travel back home", today.AddHours(17.5), today.AddHours(19), Travel));
			schedule.Appointments.Add(CreateAppointment("Family Dinner", today.AddHours(20), today.AddHours(21), Rest));
		}
		private NAppointment CreateAppointment(string subject, DateTime start, DateTime end, string groupItem)
		{
			NAppointment appointment = new NAppointment(subject, start, end);
			appointment.Groups = new NAppointmentGroupCollection();
			appointment.Groups.Add(new NAppointmentGroup("Activity", groupItem));
			return appointment;
		}
		private void SwapGroupings(NViewMode viewMode)
		{
			NGrouping grouping1 = viewMode.Groupings[0];
			NGrouping grouping2 = viewMode.Groupings[1];

			viewMode.Groupings.Clear();
			viewMode.Groupings.Add(grouping2);
			viewMode.Groupings.Add(grouping1);
		}

		#endregion

		#region Event Handlers

		private void OnRadioGroupSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			int selectedIndex = (int)arg.NewValue;
			NSchedule schedule = m_ScheduleView.Content;

			if (selectedIndex == 0)
			{
				// Time grouping should be first
				if (schedule.DayViewMode.Groupings[0] is NGroupGrouping)
				{
					SwapGroupings(schedule.DayViewMode);
					SwapGroupings(schedule.WeekViewMode);
					SwapGroupings(schedule.MonthViewMode);
				}
			}
			else
			{
				// Schedule group grouping should be first
				if (schedule.DayViewMode.Groupings[0] is NDateRangeGrouping)
				{
					SwapGroupings(schedule.DayViewMode);
					SwapGroupings(schedule.WeekViewMode);
					SwapGroupings(schedule.MonthViewMode);
				}
			}
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NGroupingExample.
		/// </summary>
		public static readonly NSchema NGroupingExampleSchema;

		#endregion
	}
}