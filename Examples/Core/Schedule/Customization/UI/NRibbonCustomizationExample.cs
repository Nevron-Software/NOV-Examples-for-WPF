using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.Schedule.Commands;
using Nevron.Nov.Schedule.UI;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NRibbonCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonCustomizationExample()
		{
			NRibbonCustomizationExampleSchema = NSchema.Create(typeof(NRibbonCustomizationExample), NExampleBaseSchema);
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

			// Add the custom command action to the schedule view's commander
			m_ScheduleView.Commander.Add(new CustomCommandAction());

			// Create and configure a ribbon UI builder
			NScheduleRibbonBuilder ribbonBuilder = new NScheduleRibbonBuilder();

			// Rename the "Home" ribbon tab page
			NRibbonTabPageBuilder homeTabBuilder = ribbonBuilder.TabPageBuilders[NScheduleRibbonBuilder.TabPageHomeName];
			homeTabBuilder.Name = "Start";

			// Rename the "Font" ribbon group of the "Home" tab page
			NRibbonGroupBuilder fontGroupBuilder = homeTabBuilder.RibbonGroupBuilders[NHomeTabPageBuilder.GroupViewName];
			fontGroupBuilder.Name = "Look";

			// Remove the "Editing" ribbon group from the "Home" tab page
			homeTabBuilder.RibbonGroupBuilders.Remove(NHomeTabPageBuilder.GroupEditingName);

			// Insert the custom ribbon group at the beginning of the home tab page
			homeTabBuilder.RibbonGroupBuilders.Insert(0, new CustomRibbonGroupBuilder());

			return ribbonBuilder.CreateUI(m_ScheduleView);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to customize the NOV schedule ribbon.</p>
";
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

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonCustomizationExample.
		/// </summary>
		public static readonly NSchema NRibbonCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Schedule.NRibbonCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomRibbonGroupBuilder : NRibbonGroupBuilder
		{
			public CustomRibbonGroupBuilder()
				: base("Custom Group", NResources.Image_Ribbon_16x16_smiley_png)
			{
			}

			protected override void AddRibbonGroupItems(NRibbonGroupItemCollection items)
			{
				// Add the "Add Appointment" command
				items.Add(CreateRibbonButton(Nevron.Nov.Schedule.NResources.Image_Ribbon_32x32_AddAppointment_png,
					Nevron.Nov.Schedule.NResources.Image_Edit_AddAppointment_png, NScheduleView.AddAppointmentCommand));

				// Add the custom command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_smiley_png,
					NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
		}

		public class CustomCommandAction : NScheduleCommandAction
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomCommandAction()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomCommandAction()
			{
				CustomCommandActionSchema = NSchema.Create(typeof(CustomCommandAction), NScheduleCommandAction.NScheduleCommandActionSchema);
			}

			#endregion

			#region Public Overrides

			/// <summary>
			/// Gets the command associated with this command action.
			/// </summary>
			/// <returns></returns>
			public override NCommand GetCommand()
			{
				return CustomCommand;
			}
			/// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				NScheduleView scheduleView = GetView(target);

				NMessageBox.Show("Schedule Custom Command executed!", "Custom Command", ENMessageBoxButtons.OK,
					ENMessageBoxIcon.Information);
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomCommandAction.
			/// </summary>
			public static readonly NSchema CustomCommandActionSchema;

			#endregion
		}

		#endregion
	}
}