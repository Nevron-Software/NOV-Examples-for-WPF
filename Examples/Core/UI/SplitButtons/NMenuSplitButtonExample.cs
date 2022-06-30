using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMenuSplitButtonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMenuSplitButtonExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMenuSplitButtonExample()
		{
			NMenuSplitButtonExampleSchema = NSchema.Create(typeof(NMenuSplitButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create an array of images to use for the headers of the menu items
			NImage[] BorderSideImages = new NImage[] {
				NResources.Image_TableBorders_AllBorders_png,
				NResources.Image_TableBorders_NoBorder_png,
				NResources.Image_TableBorders_OutsideBorders_png,
				NResources.Image_TableBorders_InsideBorders_png,
				NResources.Image_TableBorders_TopBorder_png,
				NResources.Image_TableBorders_BottomBorder_png,
				NResources.Image_TableBorders_LeftBorder_png,
				NResources.Image_TableBorders_RightBorder_png,
				NResources.Image_TableBorders_InsideHorizontalBorder_png,
				NResources.Image_TableBorders_InsideVerticalBorder_png
			};

			// Create a menu split button
			m_MenuSplitButton = new NMenuSplitButton();
			m_MenuSplitButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_MenuSplitButton.VerticalPlacement = ENVerticalPlacement.Top;

			// Fill the menu split button drop down menu from an enum and with
			// the images created above for headers
			m_MenuSplitButton.FillFromEnum<ENTableBorders, NImage>(BorderSideImages);

			// Subscribe to the SelectedIndexChanged and the Click events
			m_MenuSplitButton.SelectedIndexChanged += OnMenuSplitButtonSelectedIndexChanged;
			m_MenuSplitButton.Click += OnMenuSplitButtonClick;
			return m_MenuSplitButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_MenuSplitButton).CreatePropertyEditors(
				m_MenuSplitButton,
				NMenuSplitButton.EnabledProperty,
				NMenuSplitButton.HorizontalPlacementProperty,
				NMenuSplitButton.VerticalPlacementProperty,
				NMenuSplitButton.DropDownButtonPositionProperty,
				NMenuSplitButton.SelectedIndexProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use menu split buttons. Split buttons are drop down edits,
	whose item slot is filled with an action button, which generates a <b>Click</b> event on behalf of the
	split button. The menu split button's drop down content is a menu, which allows the user to quickly
	select an option. This option is then assigned to the action button of the split button and can be
	activated by clicking the action button.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnMenuSplitButtonSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			// Get the selected index
			int selectedIndex = (int)arg.NewValue;
			if (selectedIndex == -1)
			{
				m_EventsLog.LogEvent("No item selected");
				return;
			}

			// Obtain and show the selected enum value
			NMenuSplitButton splitButton = (NMenuSplitButton)arg.CurrentTargetNode;
			ENTableBorders selectedSide = (ENTableBorders)splitButton.SelectedValue;

			m_EventsLog.LogEvent("Selected Index: " + selectedIndex.ToString() + " (" +
				selectedSide.ToString() + ")");
		}
		private void OnMenuSplitButtonClick(NEventArgs arg)
		{
			// Get the selected index
			NMenuSplitButton splitButton = (NMenuSplitButton)arg.CurrentTargetNode;
			int selectedIndex = splitButton.SelectedIndex;

			if (selectedIndex == -1)
			{
				m_EventsLog.LogEvent("No item selected");
				return;
			}

			// Obtain and show the selected enum value
			ENTableBorders selectedSide = (ENTableBorders)splitButton.SelectedValue;

			m_EventsLog.LogEvent("Action button clicked, index: " + selectedIndex.ToString() + " (" +
				selectedSide.ToString() + ")");
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NMenuSplitButton m_MenuSplitButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMenuSplitButtonExample.
		/// </summary>
		public static readonly NSchema NMenuSplitButtonExampleSchema;

		#endregion
	}
}