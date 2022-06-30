using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NNotificationsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NNotificationsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NNotificationsExample()
		{
			NNotificationsExampleSchema = NSchema.Create(typeof(NNotificationsExample), NExampleBaseSchema);
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
    This example demonstrates how to assign notifications to appointments and how to configure NOV Schedule to
	show notification messages.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			schedule.ViewMode = ENScheduleViewMode.Day;

			// Create an old appointment
			DateTime oldStart = DateTime.Now.AddHours(-3);
			NAppointment oldAppointment = new NAppointment("Old Meeting", oldStart, oldStart.AddHours(2));
			oldAppointment.Notification = TimeSpan.Zero;
			schedule.Appointments.Add(oldAppointment);

			// Create an appointment and assign a notification 10 minutes before its start
			DateTime newStart = DateTime.Now.AddMinutes(10);
			NAppointment newAppointment = new NAppointment("New Meeting", newStart, newStart.AddHours(2));
			newAppointment.Notification = TimeSpan.FromMinutes(10);
			schedule.Appointments.Add(newAppointment);

			// Scroll the schedule to the current hour
			schedule.ScrollToTime(TimeSpan.FromHours(Math.Floor((double)oldStart.Hour)));

			// Configure the schedule view to check for pending notifications every 60 seconds
			m_ScheduleView.NotificationCheckInterval = 60;
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NNotificationsExample.
		/// </summary>
		public static readonly NSchema NNotificationsExampleSchema;

		#endregion
	}
}