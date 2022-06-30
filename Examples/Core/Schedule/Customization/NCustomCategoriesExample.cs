using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCustomCategoriesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomCategoriesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomCategoriesExample()
		{
			NCustomCategoriesExampleSchema = NSchema.Create(typeof(NCustomCategoriesExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates how to create and use custom categories.</p>";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			// Rename the first predefined category
			schedule.Categories[0].Name = NLoc.Get("Renamed Category");

			// Create and add some custom categories
			NCategory category1 = new NCategory(NLoc.Get("Custom Category 1"), NColor.Khaki);
			schedule.Categories.Add(category1);

			NCategory category2 = new NCategory(NLoc.Get("Custom Category 2"), new NHatchFill(ENHatchStyle.DiagonalBrick, NColor.Red, NColor.Orange));
			schedule.Categories.Add(category2);

			// Remove the second category
			schedule.Categories.RemoveAt(1);

			// Remove the category called "Green"
			NCategory categoryToRemove = schedule.Categories.FindByName(NLoc.Get("Green"));
			if (categoryToRemove != null)
			{
				schedule.Categories.Remove(categoryToRemove);
			}

			// Create and add some appointments
			DateTime start = DateTime.Now;

			NAppointment appointment1 = new NAppointment("Meeting 1", start, start.AddHours(2));
			appointment1.Category = category1.Name;
			schedule.Appointments.Add(appointment1);

			NAppointment appointment2 = new NAppointment("Meeting 2", appointment1.End.AddHours(0.5), appointment1.End.AddHours(2.5));
			appointment2.Category = category2.Name;
			schedule.Appointments.Add(appointment2);

			// Scroll the schedule to the start of the first appointment
			schedule.ScrollToTime(start.TimeOfDay);
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCustomCategoriesExample.
		/// </summary>
		public static readonly NSchema NCustomCategoriesExampleSchema;


		#endregion
	}
}