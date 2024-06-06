using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NFormatsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFormatsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFormatsExample()
		{
			NFormatsExampleSchema = NSchema.Create(typeof(NFormatsExample), NExampleBaseSchema);
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
			NButton loadButton = new NButton("Load...");
			loadButton.Click += OnLoadButtonClick;
			stack.Add(loadButton);

			NButton saveButton = new NButton("Save...");
			saveButton.Click += OnSaveButtonClick;
			stack.Add(saveButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to load and save schedule documents from and to all currently
	supported formats using the default open and save dialogs of the schedule view.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;
			schedule.ViewMode = ENScheduleViewMode.Day;

			schedule.Appointments.Add(new NAppointment("Travel to Work", today.AddHours(6.5), today.AddHours(7.5)));
			schedule.Appointments.Add(new NAppointment("Meeting with John", today.AddHours(8), today.AddHours(10)));
			schedule.Appointments.Add(new NAppointment("Conference", today.AddHours(10.5), today.AddHours(11.5)));
			schedule.Appointments.Add(new NAppointment("Lunch", today.AddHours(12), today.AddHours(14)));
			schedule.Appointments.Add(new NAppointment("News Reading", today.AddHours(12.5), today.AddHours(13.5)));
			schedule.Appointments.Add(new NAppointment("Video Presentation", today.AddHours(14.5), today.AddHours(15.5)));
			schedule.Appointments.Add(new NAppointment("Web Meeting", today.AddHours(16), today.AddHours(17)));
			schedule.Appointments.Add(new NAppointment("Travel back home", today.AddHours(17.5), today.AddHours(19)));
			schedule.Appointments.Add(new NAppointment("Family Dinner", today.AddHours(20), today.AddHours(21)));
		}

		#endregion

		#region Event Handlers

		private void OnLoadButtonClick(NEventArgs arg)
		{
			m_ScheduleView.OpenFileAsync();
		}
		private void OnSaveButtonClick(NEventArgs arg)
		{
			m_ScheduleView.SaveAsAsync();
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFormatsExample.
		/// </summary>
		public static readonly NSchema NFormatsExampleSchema;

		#endregion
	}
}