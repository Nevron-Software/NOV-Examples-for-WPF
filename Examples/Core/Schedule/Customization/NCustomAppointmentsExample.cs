using System;

using Nevron.Nov.Barcode;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Schedule;
using Nevron.Nov.Schedule.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Schedule
{
	public class NCustomAppointmentsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomAppointmentsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomAppointmentsExample()
		{
			NCustomAppointmentsExampleSchema = NSchema.Create(typeof(NCustomAppointmentsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple schedule
			NScheduleViewWithRibbon scheduleViewWithRibbon = new NScheduleViewWithRibbon();
			scheduleViewWithRibbon.Registered += OnScheduleViewWithRibbonRegistered;
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
<p>
    This example demonstrates how to create a custom appointments class, which inherits from NAppointment and customizes
	the content of the generated appointment widget. If you scan the generated QR code with your smartphone you will be
	asked whether to add it to your smartphone's calendar.
</p>
<p>
	This example also shows how to add a custom property to the appointment and add it to the appointment designer, which
	opens when you double click the appointment.
</p>
";
		}

		#endregion

		#region Implementation

		private void InitSchedule(NSchedule schedule)
		{
			DateTime today = DateTime.Today;
			schedule.ViewMode = ENScheduleViewMode.Day;

			schedule.Appointments.Add(new CustomAppointment("Travel to Work", today.AddHours(6.5), today.AddHours(7.5)));
			schedule.Appointments.Add(new CustomAppointment("Meeting with John", today.AddHours(8), today.AddHours(10)));
			schedule.Appointments.Add(new CustomAppointment("Conference", today.AddHours(10.5), today.AddHours(11.5)));
			schedule.Appointments.Add(new CustomAppointment("Lunch", today.AddHours(12), today.AddHours(14)));
			schedule.Appointments.Add(new CustomAppointment("News Reading", today.AddHours(12.5), today.AddHours(13.5)));
			schedule.Appointments.Add(new CustomAppointment("Video Presentation", today.AddHours(14.5), today.AddHours(15.5)));
			schedule.Appointments.Add(new CustomAppointment("Web Meeting", today.AddHours(16), today.AddHours(17)));
			schedule.Appointments.Add(new CustomAppointment("Travel back home", today.AddHours(17.5), today.AddHours(19)));
			schedule.Appointments.Add(new CustomAppointment("Family Dinner", today.AddHours(20), today.AddHours(21)));

			// Increase the height of the day view mode
			schedule.DayViewMode.Height = 2000;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when the schedule view with ribbon is added to a document.
		/// </summary>
		/// <param name="arg"></param>
		private void OnScheduleViewWithRibbonRegistered(NEventArgs arg)
		{
			// Evaluate the document
			((NDocumentNode)arg.TargetNode).OwnerDocument.Evaluate();

			// Scroll the schedule to 6 AM
			m_ScheduleView.Content.ScrollToTime(TimeSpan.FromHours(6));
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCustomAppointmentsExample.
		/// </summary>
		public static readonly NSchema NCustomAppointmentsExampleSchema;

		#endregion

		#region Fields

		private NScheduleView m_ScheduleView;

		#endregion

		#region Nested Types

		public class CustomAppointment : NAppointment
		{
			#region Constructors

			public CustomAppointment()
			{
			}
			public CustomAppointment(string subject, DateTime start, DateTime end)
				: base(subject, start, end)
			{
			}

			static CustomAppointment()
			{
				CustomAppointmentSchema = NSchema.Create(typeof(CustomAppointment), NAppointment.NAppointmentSchema);

				// Properties
				CustomTextProperty = CustomAppointmentSchema.AddSlot("CustomText", NDomType.String, null);

				// Set designer
				CustomAppointmentSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(CustomAppointmentDesigner)));
			}

			#endregion

			#region Properties

			/// <summary>
			/// Gets/Sets the value of the CustomText property.
			/// </summary>
			public string CustomText
			{
				get
				{
					return (string)GetValue(CustomTextProperty);
				}
				set
				{
					SetValue(CustomTextProperty, value);
				}
			}

			#endregion

			#region Protected Overrides

			protected override NAppointmentWidget CreateWidget()
			{
				return new CustomAppointmentWidget();
			}

			#endregion

			#region Schema

			public static readonly NSchema CustomAppointmentSchema;
			/// <summary>
			/// Reference to the CustomText property.
			/// </summary>
			public static readonly NProperty CustomTextProperty;

			#endregion

			#region Nested Types - Designer

			public class CustomAppointmentDesigner : NAppointmentDesigner
			{
				public CustomAppointmentDesigner()
				{
					SetPropertyBrowsable(CustomTextProperty, true);
				}
			}

			#endregion
		}

		public class CustomAppointmentWidget : NAppointmentWidget
		{
			#region Constructors

			public CustomAppointmentWidget()
			{
			}
			static CustomAppointmentWidget()
			{
				CustomAppointmentWidgetSchema = NSchema.Create(typeof(CustomAppointmentWidget), NAppointmentWidget.NAppointmentWidgetSchema);
			}

			#endregion

			#region Protected Overrides

			protected override NWidget CreateContent()
			{
				// Create a label
				NLabel label = new NLabel();
				label.TextWrapMode = ENTextWrapMode.WordWrap;
				label.TextAlignment = ENContentAlignment.MiddleCenter;
				label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 12, ENFontStyle.Underline);

				// Create a barcode
				NMatrixBarcode barcode = new NMatrixBarcode();
				barcode.Scale = 2;
				barcode.HorizontalPlacement = ENHorizontalPlacement.Center;
				barcode.VerticalPlacement = ENVerticalPlacement.Center;

				// Place the label and the barcode in a pair box
				NPairBox pairBox = new NPairBox(label, barcode, ENPairBoxRelation.Box1BeforeBox2);
				pairBox.Spacing = NDesign.HorizontalSpacing;
				pairBox.FillMode = ENStackFillMode.First;
				pairBox.FitMode = ENStackFitMode.Last;
				pairBox.Padding = new NMargins(NDesign.HorizontalSpacing);
				return pairBox;
			}
			protected override void OnAppointmentSubjectChanged(string oldSubject, string newSubject)
			{
				NLabel label = (NLabel)GetFirstDescendant(NLabel.NLabelSchema);
				label.Text = newSubject;

				UpdateBarcode();
			}
			protected override void OnRegistered()
			{
				base.OnRegistered();

				UpdateBarcode();
			}

			#endregion

			#region Implementation

			private void OnAppointmentChanged(NEventArgs args)
			{
				NValueChangeEventArgs valueChangeArgs = args as NValueChangeEventArgs;
				if (valueChangeArgs == null)
					return;

				// If the start or the end time of the appointment has changed, update the barcode
				NProperty property = valueChangeArgs.Property;
				if (property == NAppointmentBase.StartProperty || property == NAppointmentBase.EndProperty)
				{
					UpdateBarcode();
					NSchedule schedule = (NSchedule)this.GetFirstAncestor(NSchedule.NScheduleSchema);
					schedule.ScrollToTime(TimeSpan.FromHours(6));
				}
			}
			private void UpdateBarcode()
			{
				NAppointmentBase appointment = this.Appointment;
				if (appointment == null)
					return;

				// Serialize the appointment to a string
				string text = NScheduleFormat.iCalendar.SerializeAppointment(appointment, false);

				// Update the text of the matrix barcode widget
				NMatrixBarcode barcode = (NMatrixBarcode)GetFirstDescendant(NMatrixBarcode.NMatrixBarcodeSchema);
				barcode.Text = text;
			}

			#endregion

			#region Schema

			public static readonly NSchema CustomAppointmentWidgetSchema;

			#endregion
		}

		#endregion
	}
}