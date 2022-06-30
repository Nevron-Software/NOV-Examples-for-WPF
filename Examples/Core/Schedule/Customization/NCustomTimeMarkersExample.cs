using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCustomTimeMarkersExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomTimeMarkersExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomTimeMarkersExample()
		{
			NCustomTimeMarkersExampleSchema = NSchema.Create(typeof(NCustomTimeMarkersExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_DefaultAppointmentHeaderThickness = NScheduleTheme.AppointmentHeaderThickness;

			// Change the thickness of time markers from the default 5 DIPs to 20 DIPs
			NScheduleTheme.AppointmentHeaderThickness = 20;

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
<p>This example demonstrates how to create and use custom time markers. It also shows how to use the
<b>NScheduleTheme.AppointmentHeaderThickness</b> static property to change the thickness of time markers.</p>";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			// Rename the first predefined time marker
			schedule.TimeMarkers[0].Name = NLoc.Get("Renamed Time Marker");

			// Create and add some custom time markers
			NTimeMarker timeMarker1 = new NTimeMarker(NLoc.Get("Custom Time Marker 1"), NColor.Khaki);
			schedule.TimeMarkers.Add(timeMarker1);

			NTimeMarker timeMarker2 = new NTimeMarker(NLoc.Get("Custom Time Marker 2"), new NHatchFill(ENHatchStyle.DiagonalBrick, NColor.Red, NColor.Orange));
			schedule.TimeMarkers.Add(timeMarker2);

			// Remove the second time marker
			schedule.TimeMarkers.RemoveAt(1);

			// Remove the time marker called "Busy"
			NTimeMarker timeMarkerToRemove = schedule.TimeMarkers.FindByName(NLoc.Get("Busy"));
			if (timeMarkerToRemove != null)
			{
				schedule.TimeMarkers.Remove(timeMarkerToRemove);
			}

			// Create and add some appointments
			DateTime start = DateTime.Now;

			NAppointment appointment1 = new NAppointment("Meeting 1", start, start.AddHours(2));
			appointment1.TimeMarker = timeMarker1.Name;
			schedule.Appointments.Add(appointment1);

			NAppointment appointment2 = new NAppointment("Meeting 2", appointment1.End.AddHours(0.5), appointment1.End.AddHours(2.5));
			appointment2.TimeMarker = timeMarker2.Name;
			schedule.Appointments.Add(appointment2);

			// Scroll the schedule to the start of the first appointment
			schedule.ScrollToTime(start.TimeOfDay);
		}

		#endregion

		#region Example Lifetime

		protected internal override void OnClosing()
		{
			base.OnClosing();

			// Restore appointment header thickness to its default value
			NScheduleTheme.AppointmentHeaderThickness = m_DefaultAppointmentHeaderThickness;
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;
		private double m_DefaultAppointmentHeaderThickness;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCustomTimeMarkersExample.
		/// </summary>
		public static readonly NSchema NCustomTimeMarkersExampleSchema;


		#endregion
	}
}