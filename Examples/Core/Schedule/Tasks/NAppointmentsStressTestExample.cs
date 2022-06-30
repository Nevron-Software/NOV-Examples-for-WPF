using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Globalization;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NAppointmentsStressTestExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAppointmentsStressTestExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAppointmentsStressTestExample()
		{
			NAppointmentsStressTestExampleSchema = NSchema.Create(typeof(NAppointmentsStressTestExample), NExampleBaseSchema);
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
			return String.Format(NCultureInfo.EnglishUS, @"
<p>
    This example demonstrates how NOV Schedule handles {0:N0} appointments.
</p>
", TotalAppointments);
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			m_Random = new Random();

			int totalDays = TotalAppointments / AppointmentsPerDay;
			DateTime date = DateTime.Today.AddDays(-totalDays / 2);

			// Generate the random appointments
			NAppointmentCollection appointments = schedule.Appointments;
			for (int i = 0; i < totalDays; i++)
			{
				AddRandomAppointments(appointments, date);
				date = date.AddDays(1);
			}

			// Switch the schedule to week view
			schedule.ViewMode = ENScheduleViewMode.Week;
		}
		/// <summary>
		/// Adds some random appointments for the given date.
		/// </summary>
		/// <param name="appointments"></param>
		/// <param name="date"></param>
		private void AddRandomAppointments(NAppointmentCollection appointments, DateTime date)
		{
			for (int i = 0; i < AppointmentsPerDay; i++)
			{
				// Generate random subject
				string subject = AppointmentSubjects[m_Random.Next(0, AppointmentSubjects.Length)];

				// Generate random start hour from 0 to 24
				double startHour = m_Random.NextDouble() * 24;

				// Generate random duration from 0.5 to 2.5 hours
				double duration = 0.5 + m_Random.NextDouble() * 2; 

				// Create and add the appointment
				NAppointment appointment = new NAppointment(subject, date.AddHours(startHour), 
					date.AddHours(startHour + duration));
				appointments.Add(appointment);
			}
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;
		private Random m_Random;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAppointmentsStressTestExample.
		/// </summary>
		public static readonly NSchema NAppointmentsStressTestExampleSchema;

		#endregion

		#region Constants

		private const int AppointmentsPerDay = 20;
		private const int TotalAppointments = 100000;
		private static readonly string[] AppointmentSubjects = new string[] {
			"Travel to Work",
			"Meeting with John",
			"Conference",
			"Lunch",
			"News Reading",
			"Video Presentation",
			"Web Meeting",
			"Travel back home", 
			"Family Dinner"
		};

		#endregion
	}
}