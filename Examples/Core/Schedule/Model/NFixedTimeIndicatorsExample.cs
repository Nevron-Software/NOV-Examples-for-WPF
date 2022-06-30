using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NFixedTimeIndicatorsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFixedTimeIndicatorsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFixedTimeIndicatorsExample()
		{
			NFixedTimeIndicatorsExampleSchema = NSchema.Create(typeof(NFixedTimeIndicatorsExample), NExampleBaseSchema);
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

			NFixedTimeIndicatorCollection timeIndicators = m_ScheduleView.Content.TimeIndicators;
			for (int i = 0; i < timeIndicators.Count; i++)
			{
				NTimeIndicator timeIndicator = timeIndicators[i];
				NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(timeIndicator).CreatePropertyEditors(timeIndicator,
					NTimeIndicator.VisibleProperty,
					NTimeIndicator.FillProperty,
					NTimeIndicator.ThicknessProperty,
					NFixedTimeIndicator.TimeProperty
				);

				NStackPanel currentStack = new NStackPanel();
				for (int j = 0; j < propertyEditors.Count; j++)
				{
					currentStack.Add(propertyEditors[j]);
				}

				stack.Add(new NGroupBox($"Time Indicator {i + 1}", new NUniSizeBoxGroup(currentStack)));
			}

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to show fixed time indicator in the schedule. Fixed schedule time indicators mark specific points
	in time on the schedule. The following properties control the appearance and position of the fixed time indicators:
</p>
<ul>
	<li><b>Time</b> - the point in time to paint the fixed time indicator at.</li>
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

			// Add two fixed time indicators
			schedule.TimeIndicators.Add(new NFixedTimeIndicator(DateTime.Now.AddHours(-1), new NColor(NColor.MediumBlue, 160)));
			schedule.TimeIndicators.Add(new NFixedTimeIndicator(DateTime.Now.AddHours(1), new NColor(NColor.DarkGreen, 160)));
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFixedTimeIndicatorsExample.
		/// </summary>
		public static readonly NSchema NFixedTimeIndicatorsExampleSchema;

		#endregion
	}
}