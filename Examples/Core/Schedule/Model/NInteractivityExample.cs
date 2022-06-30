using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NInteractivityExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NInteractivityExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NInteractivityExample()
		{
			NReadOnlyOrDisabledScheduleViewExampleSchema = NSchema.Create(typeof(NInteractivityExample), NExampleBaseSchema);
		}

		#endregion

		#region Examples

		protected override NWidget CreateExampleContent()
		{
			// Create a simple schedule
			NScheduleViewWithRibbon scheduleViewWithRibbon = new NScheduleViewWithRibbon();
			m_ScheduleView = scheduleViewWithRibbon.View;
			m_ScheduleView.EnabledChanged += OnScheduleViewEnabledOrReadOnlyChanged;
			m_ScheduleView.ReadOnlyChanged += OnScheduleViewEnabledOrReadOnlyChanged;

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

			// Add schedule view property editors
			NList<NPropertyEditor> propertyEditors = NDesigner.GetDesigner(m_ScheduleView).CreatePropertyEditors(m_ScheduleView,
				NScheduleView.EnabledProperty,
				NScheduleView.ReadOnlyProperty);

			NStackPanel scheduleViewStack = new NStackPanel();
			for (int i = 0; i < propertyEditors.Count; i++)
			{
				scheduleViewStack.Add(propertyEditors[i]);
			}

			stack.Add(new NGroupBox("Schedule View", new NUniSizeBoxGroup(scheduleViewStack)));

			// Add schedule property editors
			NLabel infoLabel = new NLabel("Taken into account only if the schedule view is enabled and not read only");
			infoLabel.TextWrapMode = ENTextWrapMode.WordWrap;

			NSchedule schedule = m_ScheduleView.Content;
			m_InteractivityEditor = NDesigner.GetDesigner(schedule).CreatePropertyEditor(schedule, NSchedule.InteractivityProperty);
			stack.Add(new NGroupBox(
				"Schedule - Interactivity",
				new NPairBox(infoLabel, m_InteractivityEditor, ENPairBoxRelation.Box1AboveBox2)));

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how a read only or disabled schedule view behaves. Use the check boxes on the right
	to enable/disable or make the schedule view (NScheduleView) read only.
</p>
<p>
	If the schedule view is enabled and not read only, you can use the check boxes in the ""Interactivity"" group box
	on the right to enable/disable individual schedule interactivity features. These check boxes modify the <b>Interactivity</b>
	property of the schedule (NSchedule). The schedule interactivity can also be controlled from the ""Interactivity"" group of the
	schedule's ""Settings"" ribbon tab.
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

		private void OnScheduleViewEnabledOrReadOnlyChanged(NValueChangeEventArgs arg)
		{
			if (m_InteractivityEditor != null)
			{
				NScheduleView scheduleView = (NScheduleView)arg.CurrentTargetNode;
				m_InteractivityEditor.Enabled = scheduleView.Enabled && !scheduleView.ReadOnly;
			}
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;
		private NPropertyEditor m_InteractivityEditor;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NReadOnlyOrDisabledScheduleViewExample.
		/// </summary>
		public static readonly NSchema NReadOnlyOrDisabledScheduleViewExampleSchema;

		#endregion
	}
}