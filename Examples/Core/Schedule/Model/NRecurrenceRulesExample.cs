using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NRecurrenceRulesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRecurrenceRulesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRecurrenceRulesExample()
		{
			NRecurrenceRulesExampleSchema = NSchema.Create(typeof(NRecurrenceRulesExample), NExampleBaseSchema);
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

			stack.Add(new NRadioButton("No recurrence"));
			stack.Add(new NRadioButton("Every 3 hours"));
			stack.Add(new NRadioButton("Every day"));
			stack.Add(new NRadioButton("Every week on Monday and Friday"));
			stack.Add(new NRadioButton("The third day of every other month"));
			stack.Add(new NRadioButton("The last Sunday of every month"));
			stack.Add(new NRadioButton("May 7 and July 7 of every year"));
			stack.Add(new NRadioButton("The first Monday and Tuesday of\nMay, June and August"));
			stack.Add(new NRadioButton("Every second day for 1 month from today"));
			stack.Add(new NRadioButton("Every day for 5 occurrences from today"));

			NRadioButtonGroup group = new NRadioButtonGroup(stack);
			group.SelectedIndex = 2;
			group.SelectedIndexChanged += OnRadioGroupSelectedIndexChanged;
			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to create recurring appointments, i.e. appointments, which occur multiple
	times. Recurring appointments are created in the same way as ordinary appointments with the only difference
	that they have a recurrence rule assigned to their <b>RecurrenceRule</b> property. This recurring rule
	defines when the appointment occurs. Recurring appointments can be easily recognized by the circular arrows
	symbol at their top left corner.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;

			NAppointment appointment = new NAppointment("Appoinment", today.AddHours(12), today.AddHours(14));
			appointment.RecurrenceRule = CreateDailyRule();
			schedule.Appointments.Add(appointment);
		}

		private NRecurrenceHourlyRule CreateHourlyRule()
		{
			// Create a rule, which occurs every 3 hours
			NRecurrenceHourlyRule rule = new NRecurrenceHourlyRule();
			rule.StartDate = RuleStart;
			rule.Interval = 3;
			return rule;
		}
		private NRecurrenceDailyRule CreateDailyRule()
		{
			// Create a rule, which occurs every day
			NRecurrenceDailyRule rule = new NRecurrenceDailyRule();
			rule.StartDate = RuleStart;
			return rule;
		}
		private NRecurrenceWeeklyRule CreateWeeklyRule()
		{
			// Create a rule, which occurs every week on Monday and Friday
			NRecurrenceWeeklyRule rule = new NRecurrenceWeeklyRule(ENDayOfWeek.Monday | ENDayOfWeek.Friday);
			rule.StartDate = RuleStart;
			return rule;
		}
		private NRecurrenceMonthlyRule CreateAbsoluteMonthlyRule()
		{
			// Create a rule, which occurs on the third day of every other month
			// Use negative values for the last days of the month, for example -1 refers to the last day of the month
			NRecurrenceMonthlyRule rule = new NRecurrenceMonthlyRule(3);
			rule.StartDate = RuleStart;
			rule.Interval = 2;
			return rule;
		}
		private NRecurrenceMonthlyRule CreateRelativeMonthlyRule()
		{
			// Create a rule, which occurs on the last Sunday of every month
			NRecurrenceMonthlyRule rule = new NRecurrenceMonthlyRule(ENDayOrdinal.Last, ENDayOfWeek.Sunday);
			rule.StartDate = RuleStart;
			return rule;
		}
		private NRecurrenceYearlyRule CreateAbsoluteYearlyRule()
		{
			// Create a rule, which occurs on every May 7 and July 7, every year
			NRecurrenceYearlyRule rule = new NRecurrenceYearlyRule(7, ENMonth.May | ENMonth.July);
			rule.StartDate = RuleStart;
			return rule;
		}
		private NRecurrenceYearlyRule CreateRelativeYearlyRule()
		{
			// Create a rule, which occurs on the first Monday and Tuesday of May, June and August
			NRecurrenceYearlyRule rule = new NRecurrenceYearlyRule(
				ENDayOrdinal.First,
				ENDayOfWeek.Monday | ENDayOfWeek.Tuesday,
				ENMonth.May | ENMonth.June | ENMonth.August);
			rule.StartDate = RuleStart;
			return rule;
		}
		private NRecurrenceDailyRule CreateDailyRuleForOneMonth()
		{
			NRecurrenceDailyRule rule = new NRecurrenceDailyRule();
			rule.EndMode = ENRecurrenceEndMode.ByDate;
			rule.EndDate = DateTime.Today.AddMonths(1);
			rule.Interval = 2;

			return rule;
		}
		private NRecurrenceDailyRule CreateDailyRuleForFiveOccurrences()
		{
			NRecurrenceDailyRule rule = new NRecurrenceDailyRule();
			rule.EndMode = ENRecurrenceEndMode.AfterOccurrences;
			rule.MaxOccurrences = 5;

			return rule;
		}

		#endregion

		#region Event Handlers

		private void OnRadioGroupSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NSchedule schedule = m_ScheduleView.Content;
			NAppointment appointment = (NAppointment)schedule.Appointments[0];

			int selectedIndex = (int)arg.NewValue;
			switch (selectedIndex)
			{
				case 0:
					appointment.RecurrenceRule = null;
					schedule.ViewMode = ENScheduleViewMode.Week;
					break;
				case 1:
					appointment.RecurrenceRule = CreateHourlyRule();
					schedule.ViewMode = ENScheduleViewMode.Week;
					break;
				case 2:
					appointment.RecurrenceRule = CreateDailyRule();
					schedule.ViewMode = ENScheduleViewMode.Week;
					break;
				case 3:
					appointment.RecurrenceRule = CreateWeeklyRule();
					schedule.ViewMode = ENScheduleViewMode.Week;
					break;
				case 4:
					appointment.RecurrenceRule = CreateAbsoluteMonthlyRule();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
				case 5:
					appointment.RecurrenceRule = CreateRelativeMonthlyRule();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
				case 6:
					appointment.RecurrenceRule = CreateAbsoluteYearlyRule();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
				case 7:
					appointment.RecurrenceRule = CreateRelativeYearlyRule();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
				case 8:
					appointment.RecurrenceRule = CreateDailyRuleForOneMonth();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
				case 9:
					appointment.RecurrenceRule = CreateDailyRuleForFiveOccurrences();
					schedule.ViewMode = ENScheduleViewMode.Month;
					break;
			}
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRecurrenceRulesExample.
		/// </summary>
		public static readonly NSchema NRecurrenceRulesExampleSchema;

		#endregion

		#region Constants

		// The start date of the recurrence rules. By default rules start from the current day (today),
		// so if you want to change that, you should set their Start property to another date.
		private static readonly DateTime RuleStart = new DateTime(2015, 1, 1);

		#endregion
	}
}