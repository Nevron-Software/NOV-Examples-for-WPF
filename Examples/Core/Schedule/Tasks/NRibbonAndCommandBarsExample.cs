using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NRibbonAndCommandBarsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonAndCommandBarsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonAndCommandBarsExample()
		{
			NRibbonAndCommandBarsExampleSchema = NSchema.Create(typeof(NRibbonAndCommandBarsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple schedule
			m_ScheduleView = new NScheduleView();

			m_ScheduleView.Document.PauseHistoryService();
			try
			{
				InitSchedule(m_ScheduleView.Content);
			}
			finally
			{
				m_ScheduleView.Document.ResumeHistoryService();
			}

			// Create and execute a ribbon UI builder
			m_RibbonBuilder = new NScheduleRibbonBuilder();
			return m_RibbonBuilder.CreateUI(m_ScheduleView);
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.Click += OnSwitchUIButtonClick;
			stack.Add(switchUIButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to switch the NOV Schedule commanding interface between ribbon and command bars.</p>";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime start = DateTime.Now;

			// Create an appointment
			NAppointment appointment = new NAppointment("Meeting", start, start.AddHours(2));
			schedule.Appointments.Add(appointment);
			schedule.ScrollToTime(start.TimeOfDay);
		}
		private void SetUI(NCommandUIHolder oldUiHolder, NWidget widget)
		{
			if (oldUiHolder.ParentNode is NTabPage)
			{
				((NTabPage)oldUiHolder.ParentNode).Content = widget;
			}
			else if (oldUiHolder.ParentNode is NPairBox)
			{
				((NPairBox)oldUiHolder.ParentNode).Box1 = widget;
			}
		}

		#endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the schedule view from its parent
			NCommandUIHolder uiHolder = m_ScheduleView.GetFirstAncestor<NCommandUIHolder>();
			m_ScheduleView.ParentNode.RemoveChild(m_ScheduleView);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				SetUI(uiHolder, m_RibbonBuilder.CreateUI(m_ScheduleView));
			}
			else
			{
				// We are in "Ribbon" mode, so switch to "Command Bars"
				label.Text = SwitchToRibbon;

				// Create the command bars
				if (m_CommandBarBuilder == null)
				{
					m_CommandBarBuilder = new NScheduleCommandBarBuilder();
				}

				SetUI(uiHolder, m_CommandBarBuilder.CreateUI(m_ScheduleView));
			}
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;
		private NScheduleRibbonBuilder m_RibbonBuilder;
		private NScheduleCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonAndCommandBarsSwitchingExample.
		/// </summary>
		public static readonly NSchema NRibbonAndCommandBarsExampleSchema;

		#endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}