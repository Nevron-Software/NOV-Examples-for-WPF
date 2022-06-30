using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCategorizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCategorizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCategorizationExample()
		{
			NCategorizationExampleSchema = NSchema.Create(typeof(NCategorizationExample), NExampleBaseSchema);
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
    This example demonstrates how to create and add appointments to a schedule and how to categorize them through code.
	To categorize an appointment means to assign a category and/or a time marker to it. The schedule contains a collection
	of default categories and time markers, but you can easily modify them when needed.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;
			schedule.ViewMode = ENScheduleViewMode.Day;

			// The categories of the schedule are stored in its Categories collections and by default are:
			// "Orange", "Red", "Blue", "Green", "Purple" and "Yellow"
			// You can either use these names (case sensitive), or obtain them from the Categories collection of the schedule

			// Create an appointment and associate it with the "Red" category
			NAppointment appointment = new NAppointment("Travel to Work", today.AddHours(6.5), today.AddHours(7.5));
			appointment.Category = "Red";
			schedule.Appointments.Add(appointment);

			// Create an appointment and associate it with the first category of the schedule
			appointment = new NAppointment("Meeting with John", today.AddHours(8), today.AddHours(10));
			appointment.Category = schedule.Categories[0].Name;
			schedule.Appointments.Add(appointment);

			// Time markers are similar to categories with the diference that they only color the header of an appointment.
			// The time markers of a schedule are stored in its TimeMarkers collection and by default are:
			// "Free", "Tentative", "Busy" and "Out of Office"
			// You can either use these names (case sensitive), or obtain them from the TmieMarkers collection of the schedule

			// Create an appointment and assign the "Busy" time marker to it
			appointment = new NAppointment("Conference", today.AddHours(10.5), today.AddHours(11.5));
			appointment.TimeMarker = "Busy";
			schedule.Appointments.Add(appointment);

			// Create an appointment and assign the first time marker of the schedule to it
			appointment = new NAppointment("Lunch", today.AddHours(12), today.AddHours(14));
			appointment.TimeMarker = schedule.TimeMarkers[0].Name;
			schedule.Appointments.Add(appointment);

			// Create an appointment and assign both a category and a time marker to it
			appointment = new NAppointment("News Reading", today.AddHours(12.5), today.AddHours(13.5));
			appointment.Category = "Yellow";
			appointment.TimeMarker = "Tentative";
			schedule.Appointments.Add(appointment);

			// Add some more appointments
			schedule.Appointments.Add(new NAppointment("Video Presentation", today.AddHours(14.5), today.AddHours(15.5)));
			schedule.Appointments.Add(new NAppointment("Web Meeting", today.AddHours(16), today.AddHours(17)));
			schedule.Appointments.Add(new NAppointment("Travel back home", today.AddHours(17.5), today.AddHours(19)));
			schedule.Appointments.Add(new NAppointment("Family Dinner", today.AddHours(20), today.AddHours(21)));
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCategorizationExample.
		/// </summary>
		public static readonly NSchema NCategorizationExampleSchema;

		#endregion
	}
}