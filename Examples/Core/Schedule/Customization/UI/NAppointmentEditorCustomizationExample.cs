using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Schedule;
using Nevron.Nov.Schedule.Commands;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NAppointmentEditorCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAppointmentEditorCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAppointmentEditorCustomizationExample()
		{
			NAppointmentEditorCustomizationExampleSchema = NSchema.Create(typeof(NAppointmentEditorCustomizationExample), NExampleBaseSchema);
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
			return @"
<p>This example demonstrates how to replace the appointment edit dialog with a custom one.</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime start = DateTime.Now;

			// Replace the default Add Appointment command action with a custom one
			NCommandAction addAppointmentCommandAction = m_ScheduleView.Commander.GetCommandAction(NScheduleView.AddAppointmentCommand);
			m_ScheduleView.Commander.Remove(addAppointmentCommandAction);
			m_ScheduleView.Commander.Add(new CustomAddAppointmentCommandAction());

			// Replace the default Appointment Edit tool with a custom one
			NTool appointmentEditTool = m_ScheduleView.Interactor.GetTool(NAppointmentEditTool.NAppointmentEditToolSchema);
			int index = m_ScheduleView.Interactor.IndexOf(appointmentEditTool);
			m_ScheduleView.Interactor.RemoveAt(index);

			NTool customEditAppointmentTool = new CustomAppointmentEditTool();
			customEditAppointmentTool.Enabled = true;
			m_ScheduleView.Interactor.Insert(index, customEditAppointmentTool);

			// Create some custom appointments
			AppointmentWithLocation appointmentWithLocation = new AppointmentWithLocation(
				"Appointment with Location", start, start.AddHours(2));
			appointmentWithLocation.Location = "New York";
			schedule.Appointments.Add(appointmentWithLocation);

			AppointmentWithImage appointmentWithImage = new AppointmentWithImage(
				"Appointment with Image", start.AddHours(3), start.AddHours(5));
			appointmentWithImage.Image = NResources.Image_MobileComputers_UMPC_jpg;
			appointmentWithImage.Category = NLoc.Get("Orange");
			schedule.Appointments.Add(appointmentWithImage);

			schedule.ScrollToTime(start.TimeOfDay);
		}

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAppointmentEditorCustomizationExample.
		/// </summary>
		public static readonly NSchema NAppointmentEditorCustomizationExampleSchema;

		#endregion

		#region Nested Types

		/// <summary>
		/// A custom appointment class.
		/// </summary>
		public class AppointmentWithLocation : NAppointment
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public AppointmentWithLocation()
			{
			}
			/// <summary>
			/// Initializing constructor.
			/// </summary>
			/// <param name="subject"></param>
			/// <param name="start"></param>
			/// <param name="end"></param>
			public AppointmentWithLocation(string subject, DateTime start, DateTime end)
				: base(subject, start, end)
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static AppointmentWithLocation()
			{
				CustomAppointmentSchema = NSchema.Create(typeof(AppointmentWithLocation), NAppointmentSchema);

				// Properties
				LocationProperty = CustomAppointmentSchema.AddSlot("Location", NDomType.String, defaultLocation);

				// Designer
				CustomAppointmentSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(CustomAppointmentDesigner)));
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets/Sets the location of the appointment.
			/// </summary>
			public string Location
			{
				get
				{
					return (string)GetValue(LocationProperty);
				}
				set
				{
					SetValue(LocationProperty, value);
				}
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomAppointment.
			/// </summary>
			public static readonly NSchema CustomAppointmentSchema;
			/// <summary>
			/// Reference to the Location property.
			/// </summary>
			public static readonly NProperty LocationProperty;

			#endregion
			
			#region Default Values

			private const string defaultLocation = null;

			#endregion

			#region Nested Types - Designer

			public class CustomAppointmentDesigner : NAppointmentDesigner
			{
				protected override void ConfigureGeneralCategory()
				{
					base.ConfigureGeneralCategory();

					SetPropertyBrowsable(LocationProperty, true);
				}
			}

			#endregion
		}

		/// <summary>
		/// A custom appointment class.
		/// </summary>
		public class AppointmentWithImage : NAppointmentBase
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public AppointmentWithImage()
			{
			}
			/// <summary>
			/// Initializing constructor.
			/// </summary>
			/// <param name="subject"></param>
			/// <param name="start"></param>
			/// <param name="end"></param>
			public AppointmentWithImage(string subject, DateTime start, DateTime end)
				: base(subject, start, end)
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static AppointmentWithImage()
			{
				AppointmentWithImageSchema = NSchema.Create(typeof(AppointmentWithImage), NAppointmentBaseSchema);

				// Properties
				ImageProperty = AppointmentWithImageSchema.AddSlot("Image", typeof(NImage), defaultImage);
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets/Sets the image for this appointment.
			/// </summary>
			public NImage Image
			{
				get
				{
					return (NImage)GetValue(ImageProperty);
				}
				set
				{
					SetValue(ImageProperty, value);
				}
			}

			#endregion

			#region Public Overrides - Edit Dialog

			/// <summary>
			/// Creates a custom appointment edit dialog.
			/// </summary>
			/// <returns></returns>
			public override NTopLevelWindow CreateEditDialog()
			{
				NSchedule schedule = (NSchedule)GetFirstAncestor(NSchedule.NScheduleSchema);
				NWindow window = schedule != null ? schedule.DisplayWindow : null;

				// Create a dialog window
				NTopLevelWindow dialog = NApplication.CreateTopLevelWindow(NWindow.GetFocusedWindowIfNull(window));
				dialog.SetupDialogWindow("Appointment with Image Editor", true);

				NStackPanel stack = new NStackPanel();
				stack.FillMode = ENStackFillMode.Last;
				stack.FitMode = ENStackFitMode.Last;

				// Add an image box with the image
				NImageBox imageBox = new NImageBox((NImage)NSystem.SafeDeepClone(Image));
				stack.Add(imageBox);

				// Add property editors for some of the appointment properties
				NDesigner designer = NDesigner.GetDesigner(this);
				NList<NPropertyEditor> editors = designer.CreatePropertyEditors(this,
					SubjectProperty,
					StartProperty,
					EndProperty);

				for (int i = 0; i < editors.Count; i++)
				{
					stack.Add(editors[i]);
				}

				// Add a button strip with OK and Cancel buttons
				NButtonStrip buttonStrip = new NButtonStrip();
				buttonStrip.AddOKCancelButtons();
				stack.Add(buttonStrip);

				dialog.Content = new NUniSizeBoxGroup(stack);

				return dialog;
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with AppointmentWithImage.
			/// </summary>
			public static readonly NSchema AppointmentWithImageSchema;
			/// <summary>
			/// Reference to the Image property.
			/// </summary>
			public static readonly NProperty ImageProperty;

			#endregion

			#region Default Values

			private const NImage defaultImage = null;

			#endregion
		}

		public class CustomAddAppointmentCommandAction : NAddAppointmentCommandAction
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomAddAppointmentCommandAction()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomAddAppointmentCommandAction()
			{
				CustomAddAppointmentCommandActionSchema = NSchema.Create(typeof(CustomAddAppointmentCommandAction), NAddAppointmentCommandActionSchema);
			}

			#endregion

			#region Protected Overrides

			protected override NAppointmentBase CreateAppointmentForGridCell(NScheduleGridCell gridCell)
			{
				AppointmentWithImage appointment = new AppointmentWithImage(null, gridCell.StartTime, gridCell.EndTime);
				appointment.Image = NResources.Image_Artistic_Plane_png;

				return appointment;
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomAddAppointmentCommandAction.
			/// </summary>
			public static readonly NSchema CustomAddAppointmentCommandActionSchema;

			#endregion
		}

		public class CustomAppointmentEditTool : NAppointmentEditTool
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomAppointmentEditTool()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomAppointmentEditTool()
			{
				CustomAppointmentEditToolSchema = NSchema.Create(typeof(CustomAppointmentEditTool), NAppointmentEditToolSchema);
			}

			#endregion

			#region Protected Overrides

			protected override NAppointmentBase CreateAppointmentForGridCell(NScheduleGridCell gridCell)
			{
				AppointmentWithImage appointment = new AppointmentWithImage(null, gridCell.StartTime, gridCell.EndTime);
				appointment.Image = NResources.Image_Artistic_Plane_png;

				return appointment;
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomAppointmentEditTool.
			/// </summary>
			public static readonly NSchema CustomAppointmentEditToolSchema;

			#endregion
		}

		#endregion
	}
}