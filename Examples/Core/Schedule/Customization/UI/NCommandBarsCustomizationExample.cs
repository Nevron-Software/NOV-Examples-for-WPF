using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Schedule;
using Nevron.Nov.Schedule.Commands;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCommandBarsCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCommandBarsCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCommandBarsCustomizationExample()
		{
			NCommandBarsCustomizationExampleSchema = NSchema.Create(typeof(NCommandBarsCustomizationExample), NExampleBaseSchema);
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

			// Create and configure command bars UI builder
			// Remove the "Edit" menu and insert a custom one
			NScheduleCommandBarBuilder commandBarBuilder = new NScheduleCommandBarBuilder();
			commandBarBuilder.MenuDropDownBuilders.Remove(NScheduleCommandBarBuilder.MenuEditName);
			commandBarBuilder.MenuDropDownBuilders.Insert(1, new CustomMenuBuilder());

			// Remove the "Standard" toolbar and insert a custom one
			commandBarBuilder.ToolBarBuilders.Remove(NScheduleCommandBarBuilder.ToolbarStandardName);
			commandBarBuilder.ToolBarBuilders.Insert(0, new CustomToolBarBuilder());

			return commandBarBuilder.CreateUI(m_ScheduleView);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to customize the NOV schedule command bars (menus and toolbars).</p>
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
		/// Schema associated with NCommandBarsCustomizationExample.
		/// </summary>
		public static readonly NSchema NCommandBarsCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Schedule.NCommandBarsCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomMenuBuilder : NMenuDropDownBuilder
		{
			public CustomMenuBuilder()
				: base("Custom Menu")
			{
			}

			protected override void AddItems(NMenuItemCollection items)
			{
				// Add the "Add Appointment" menu item
				items.Add(CreateMenuItem(Nevron.Nov.Schedule.NResources.Image_Edit_AddAppointment_png, NScheduleView.AddAppointmentCommand));

				// Add the custom command menu item
				items.Add(CreateMenuItem(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
		}

		public class CustomToolBarBuilder : NToolBarBuilder
		{
			public CustomToolBarBuilder()
				: base("Custom Toolbar")
			{
			}

			protected override void AddItems(NCommandBarItemCollection items)
			{
				// Add the "Add Appointment" button
				items.Add(CreateButton(Nevron.Nov.Schedule.NResources.Image_Edit_AddAppointment_png, NScheduleView.AddAppointmentCommand));

				// Add the custom command button
				items.Add(CreateButton(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
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