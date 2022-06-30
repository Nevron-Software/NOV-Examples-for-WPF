using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NComboBoxItemsManipulationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NComboBoxItemsManipulationExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NComboBoxItemsManipulationExample()
		{
			NComboBoxItemsManipulationExampleSchema = NSchema.Create(typeof(NComboBoxItemsManipulationExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Overrides - Example

		protected override NWidget CreateExampleContent()
		{
			// create the combo box
			m_ComboBox = new NComboBox();
			m_ComboBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ComboBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_ComboBox.DropDownStyle = ENComboBoxStyle.DropDownList;

			// add a few items
			for (int i = 0; i < 10; i++)
			{
				m_ComboBox.Items.Add(new NComboBoxItem("Item " + i.ToString()));
			}

			// select the first item
			m_ComboBox.SelectedIndex = 0;

			// hook combo box selection events
			m_ComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnComboBoxSelectedIndexChanged);

			return m_ComboBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Create the commands
			stack.Add(CreateCommandsGroupBox());

			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a simple combo box with text only items and how to add/remove items.
	You can use the buttons on the right to add/remove items from the combo box's <b>Items</b> collection.
</p>
";
		}

		#endregion

		#region Implementation

		private NGroupBox CreateCommandsGroupBox()
		{
			NStackPanel commandsStack = new NStackPanel();

			NButton addButton = new NButton("Add Item");
			addButton.Click += new Function<NEventArgs>(OnAddButtonClick);
			commandsStack.Add(addButton);

			NButton removeSelectedButton = new NButton("Remove Selected");
			removeSelectedButton.Click += new Function<NEventArgs>(OnRemoveSelectedButtonClick);
			commandsStack.Add(removeSelectedButton);

			NButton removeAllButton = new NButton("Remove All");
			removeAllButton.Click += new Function<NEventArgs>(OnRemoveAllButtonClick);
			commandsStack.Add(removeAllButton);

			return new NGroupBox("Commands", commandsStack);
		}

		#endregion

		#region Event Handlers

		private void OnComboBoxSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox comboBox = (NComboBox)args.TargetNode;
			m_EventsLog.LogEvent("Selected Index: " + comboBox.SelectedIndex.ToString());
		}
		private void OnAddButtonClick(NEventArgs args)
		{
			m_ComboBox.Items.Add(new NComboBoxItem("Item " + m_ComboBox.Items.Count));
		}
		private void OnRemoveSelectedButtonClick(NEventArgs args)
		{
			if (m_ComboBox.SelectedIndex != -1)
			{
				m_ComboBox.Items.RemoveAt(m_ComboBox.SelectedIndex);
				m_ComboBox.SelectedIndex = -1;
			}
		}
		private void OnRemoveAllButtonClick(NEventArgs args)
		{
			m_ComboBox.Items.Clear();
		}

		#endregion

		#region Fields

		private NComboBox m_ComboBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NComboBoxItemsManipulationExample.
		/// </summary>
		public static readonly NSchema NComboBoxItemsManipulationExampleSchema;

		#endregion
	}
}