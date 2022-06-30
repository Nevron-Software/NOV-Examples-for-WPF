using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCurrentTimeIndicatorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCurrentTimeIndicatorExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCurrentTimeIndicatorExample()
		{
			NCurrentTimeIndicatorExampleSchema = NSchema.Create(typeof(NCurrentTimeIndicatorExample), NExampleBaseSchema);
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
			NTimeIndicator timeIndicator = m_ScheduleView.Content.CurrentTimeIndicator;
			NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(timeIndicator).CreatePropertyEditors(timeIndicator,
				NTimeIndicator.VisibleProperty,
				NTimeIndicator.FillProperty,
				NTimeIndicator.ThicknessProperty
			);

			NStackPanel stack = new NStackPanel();
			for (int i = 0; i < propertyEditors.Count; i++)
			{
				stack.Add(propertyEditors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to show and configure the current time indicator. This is done via the following properties
	of the schedule's <b>CurrentTimeIndicator</b>:
</p>
<ul>
	<li><b>Visible</b> - determines whether to show the time indicator.</li>
	<li><b>Fill</b> - determines the fill style of the time indicator. For best results set it to a semi-transparent color fill,
		for example - new NColorFill(new NColor(192, 0, 0, 160)).</li>
	<li><b>Thickness</b> - determines the thickness of the time indicator in DIPs. By default set to 2.</li>
</ul>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;
			schedule.ViewMode = ENScheduleViewMode.Day;

			// Add some appointments
			schedule.Appointments.Add(new NAppointment("Travel to Work", today.AddHours(6.5), today.AddHours(7.5)));
			schedule.Appointments.Add(new NAppointment("Meeting with John", today.AddHours(8), today.AddHours(10)));
			schedule.Appointments.Add(new NAppointment("Conference", today.AddHours(10.5), today.AddHours(11.5)));
			schedule.Appointments.Add(new NAppointment("Lunch", today.AddHours(12), today.AddHours(14)));
			schedule.Appointments.Add(new NAppointment("News Reading", today.AddHours(12.5), today.AddHours(13.5)));
			schedule.Appointments.Add(new NAppointment("Video Presentation", today.AddHours(14.5), today.AddHours(15.5)));
			schedule.Appointments.Add(new NAppointment("Web Meeting", today.AddHours(16), today.AddHours(17)));
			schedule.Appointments.Add(new NAppointment("Travel back home", today.AddHours(17.5), today.AddHours(19)));
			schedule.Appointments.Add(new NAppointment("Family Dinner", today.AddHours(20), today.AddHours(21)));

			// Show current time indicator
			schedule.CurrentTimeIndicator.Visible = true;
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCurrentTimeIndicatorExample.
		/// </summary>
		public static readonly NSchema NCurrentTimeIndicatorExampleSchema;

		#endregion
	}
}
