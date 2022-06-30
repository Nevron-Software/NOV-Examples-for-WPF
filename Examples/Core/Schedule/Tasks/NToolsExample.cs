using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NToolsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NToolsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NToolsExample()
		{
			NToolsExampleSchema = NSchema.Create(typeof(NToolsExample), NExampleBaseSchema);
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

			// Add a check box for each tool of the schedule view
			NStackPanel toolsStack = new NStackPanel();
			for (int i = 0; i < m_ScheduleView.Interactor.Count; i++)
			{
				NTool tool = m_ScheduleView.Interactor[i];
				NCheckBox checkBox = new NCheckBox(tool.ToString(), tool.Enabled);
				checkBox.Tag = tool;
				checkBox.CheckedChanged += OnCheckBoxCheckedChanged;
				toolsStack.Add(checkBox);
			}

			stack.Add(new NGroupBox("Tools", toolsStack));

			// Add a setting for the mouse button event of the Click Select tool
			NStackPanel clickSelectStack = new NStackPanel();

			NScheduleClickSelectTool clickSelectTool = (NScheduleClickSelectTool)m_ScheduleView.Interactor.GetTool(
				NScheduleClickSelectTool.NScheduleClickSelectToolSchema);
			clickSelectStack.Add(NDesigner.GetDesigner(clickSelectTool).CreatePropertyEditor(
				clickSelectTool, NScheduleClickSelectTool.MouseButtonEventProperty));

			stack.Add(new NGroupBox("Click Select Tool", clickSelectStack));

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    This example demonstrates how to configure, enable and disable schedule view tools.
	Use the widgets on the right to control the tools of the schedule view.
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

		private void OnCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			// Enable/Disable the tool based on the new value of the check box's Check property
			NTool tool = (NTool)arg.CurrentTargetNode.Tag;
			tool.Enabled = (bool)arg.NewValue;
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NToolsExample.
		/// </summary>
		public static readonly NSchema NToolsExampleSchema;

		#endregion
	}
}