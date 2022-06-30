using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Schedule;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	/// <summary>
	/// Base class for all schedule examples.
	/// </summary>
	public abstract class NScheduleExampleBase : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NScheduleExampleBase()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NScheduleExampleBase()
		{
			NScheduleExampleBaseSchema = NSchema.Create(typeof(NScheduleExampleBase), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Example

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

		#endregion

		#region Protected Must Override

		protected abstract void InitSchedule(NSchedule schedule);

		#endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the rich text view from its parent
			m_ScheduleView.ParentNode.RemoveChild(m_ScheduleView);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				m_ExampleTabPage.Content = m_RibbonBuilder.CreateUI(m_ScheduleView);
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

				m_ExampleTabPage.Content = m_CommandBarBuilder.CreateUI(m_ScheduleView);
			}
		}

		#endregion

		#region Fields

		protected NScheduleView m_ScheduleView;
		protected NScheduleRibbonBuilder m_RibbonBuilder;
		protected NScheduleCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NScheduleExampleBase.
		/// </summary>
		public static readonly NSchema NScheduleExampleBaseSchema;

		#endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}