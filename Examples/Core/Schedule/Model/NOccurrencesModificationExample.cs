using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NOccurrencesModificationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NOccurrencesModificationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NOccurrencesModificationExample()
		{
			NOccurrencesModificationExampleSchema = NSchema.Create(typeof(NOccurrencesModificationExample), NExampleBaseSchema);
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
    This example demonstrates how to create recurring appointments, i.e. appointments, which occur multiple
	times. Recurring appointments are created in the same way as ordinary appointments with the only difference
	that they have a recurrence rule assigned to their <b>RecurrenceRule</b> property and can be easily
	recognized by the circular arrows symbol at their top left corner.
</p>
<p>
	This examples also shows how to change the properties or delete an occurrence of a recurring appointment.
	By default occurrences of recurring appointments inherit the property values of the recurring appointment,
	but you can easily change any of these properties for a specific occurrence. All changes to such occurrences
	are remembered, so even if you move to the previous or the next range of the schedule, when you get back
	to the current range, you'll be able to see all these changes again.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;

			// Create a recurring appointment, which occurs every day
			NAppointment appointment = new NAppointment("Appointment", today.AddHours(12), today.AddHours(14));
			NRecurrenceDailyRule rule = new NRecurrenceDailyRule();
			rule.StartDate = new DateTime(2015, 1, 1);
			appointment.RecurrenceRule = rule;

			// Add the recurring appointment to the schedule
			schedule.Appointments.Add(appointment);

			// Change the time of the first appointment in the current week
			NAppointmentBase appChild = appointment.Occurrences[0];
			appChild.Start = appChild.Start.AddHours(-2);
			appChild.End = appChild.End.AddHours(-2);

			// Change the subject of the second appointment
			appChild = appointment.Occurrences[1];
			appChild.Subject = "Custom Subject";

			// Change the category of the third appointment
			appChild = appointment.Occurrences[2];
			appChild.Category = "Red";

			// Delete the fourth appointment
			appointment.Occurrences.RemoveAt(3);
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NOccurrencesModificationExample.
		/// </summary>
		public static readonly NSchema NOccurrencesModificationExampleSchema;

		#endregion
	}
}