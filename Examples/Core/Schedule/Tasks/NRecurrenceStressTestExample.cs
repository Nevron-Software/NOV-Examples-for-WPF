using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NRecurrenceStressTestExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRecurrenceStressTestExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRecurrenceStressTestExample()
		{
			NRecurrenceStressTestExampleSchema = NSchema.Create(typeof(NRecurrenceStressTestExample), NExampleBaseSchema);
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
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how NOV Schedule deals with a large number of occurrences of recurring appointments.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;
			DateTime startDate = new DateTime(today.Year, 1, 1);

			// Create an appointment which occurs per 3 hours
			NAppointment appointment = new NAppointment("Meeting", today, today.AddHours(2));
			NRecurrenceHourlyRule rule = new NRecurrenceHourlyRule();
			rule.StartDate = startDate;
			rule.Interval = 3;
			appointment.RecurrenceRule = rule;
			schedule.Appointments.Add(appointment);

			// Create an appointment which occurs every hour and categorize it
			appointment = new NAppointment("Talking", today, today.AddHours(0.5));
			rule = new NRecurrenceHourlyRule();
			rule.StartDate = startDate;
			rule.Interval = 1;
			appointment.RecurrenceRule = rule;
			appointment.Category = "Red";
			schedule.Appointments.Add(appointment);

			// Swicth schedule to week view mode
			schedule.ViewMode = ENScheduleViewMode.Week;
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRecurrenceStressTestExample.
		/// </summary>
		public static readonly NSchema NRecurrenceStressTestExampleSchema;

		#endregion
	}
}