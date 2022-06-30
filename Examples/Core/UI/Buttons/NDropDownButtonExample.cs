using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NDropDownButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDropDownButtonExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDropDownButtonExample()
		{
			NDropDownButtonExampleSchema = NSchema.Create(typeof(NDropDownButtonExample), NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Example

		protected override NWidget CreateExampleContent()
		{
			m_DropDownButton = new NDropDownButton(NPairBox.Create(NResources.Image__16x16_Calendar_png, "Event"));
			m_DropDownButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_DropDownButton.VerticalPlacement = ENVerticalPlacement.Top;
			m_DropDownButton.Margins = new NMargins(0, NDesign.VerticalSpacing, 0, 0);

			// Create the drop down button Popup content
			NStackPanel popupStack = new NStackPanel();
			popupStack.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			popupStack.Add(new NLabel("Event Date:"));
			popupStack.Add(new NCalendar());
			popupStack.Add(new NLabel("Event Color:"));
			popupStack.Add(new NPaletteColorPicker());

			NButtonStrip buttonStrip = new NButtonStrip();
			buttonStrip.AddOKCancelButtons();
			popupStack.Add(buttonStrip);

			m_DropDownButton.Popup.Content = popupStack;

			return m_DropDownButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_DropDownButton).CreatePropertyEditors(m_DropDownButton,
				NWidget.EnabledProperty,
				NStylePropertyEx.ExtendedLookPropertyEx
			);

			// Change the text of the extended look property editor
			NLabel label = (NLabel)editors[0].GetFirstDescendant(new NInstanceOfSchemaFilter(NLabel.NLabelSchema));
			label.Text = "Extended Look:";

			NStackPanel stack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to create a drop down button and set the content of its popup.</p>";
		}

		#endregion

		#region Fields

		private NDropDownButton m_DropDownButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDropDownButtonExample.
		/// </summary>
		public static readonly NSchema NDropDownButtonExampleSchema;

		#endregion
	}
}