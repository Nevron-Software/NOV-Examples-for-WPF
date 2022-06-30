using System;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NListBoxItemsManipulationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NListBoxItemsManipulationExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NListBoxItemsManipulationExample()
		{
			NListBoxItemsManipulationExampleSchema = NSchema.Create(typeof(NListBoxItemsManipulationExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a list box
			m_ListBox = new NListBox();
			m_ListBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ListBox.PreferredSize = new NSize(200, 400);

			// Add some items
			for (int i = 0; i < 10; i++)
			{
				NListBoxItem item = new NListBoxItem("Item " + i.ToString());
				item.Tag = i;
				m_ListBox.Items.Add(item);
			}

			// Hook to list box selection events
			m_ListBox.Selection.Selected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemSelected);
			m_ListBox.Selection.Deselected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemDeselected);

			return m_ListBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Create the commands group box
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
	This example demonstrates how to create a simple list box with text only list box items and how to add/remove items.
	You can use the buttons on the right to add/remove items from the list box's <b>Items</b> collection.
</p>
";
		}

		#endregion

		#region Implementation

		private NGroupBox CreateCommandsGroupBox()
		{
			NStackPanel commandsStack = new NStackPanel();

			// Create the command buttons
			m_AddButton = new NButton("Add Item");
			m_AddButton.Click += new Function<NEventArgs>(OnAddButtonClick);
			commandsStack.Add(m_AddButton);

			m_RemoveSelectedButton = new NButton("Remove Selected");
			m_RemoveSelectedButton.Enabled = false;
			m_RemoveSelectedButton.Click += new Function<NEventArgs>(OnRemoveSelectedButtonClick);
			commandsStack.Add(m_RemoveSelectedButton);

			m_RemoveAllButton = new NButton("Remove All");
			m_RemoveAllButton.Click += new Function<NEventArgs>(OnRemoveAllButtonClick);
			commandsStack.Add(m_RemoveAllButton);

			// Create the commands group box
			NGroupBox commmandsGroupBox = new NGroupBox("Commands");
			commmandsGroupBox.Content = commandsStack;
			return commmandsGroupBox;
		}

		#endregion

		#region Event Handlers

		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> args)
		{
			m_RemoveSelectedButton.Enabled = true;

			NListBoxItem item = args.Item;
			m_EventsLog.LogEvent("Selected Item: " + item.Tag.ToString());
		}
		private void OnListBoxItemDeselected(NSelectEventArgs<NListBoxItem> args)
		{
			NListBoxItem item = args.Item;
			m_EventsLog.LogEvent("Deselected Item: " + item.Tag.ToString());
		}

		private void OnAddButtonClick(NEventArgs args)
		{
			int index;
			string value = "0";
			if (m_ListBox.Items.Count > 0)
			{
				NListBoxItem lastItem = m_ListBox.Items[m_ListBox.Items.Count - 1];
				NLabel label = (NLabel)lastItem.GetDescendants(new NInstanceOfSchemaFilter(NLabel.NLabelSchema))[0];
				value = label.Text;
				value = value.Remove(0, value.LastIndexOf(' ') + 1);
			}

			// Add an item with the calculated index
			index = Int32.Parse(value) + 1;
			NListBoxItem item = new NListBoxItem("Item " + index);
			item.Tag = index;
			m_ListBox.Items.Add(item);

			if (m_ListBox.Items.Count == 1)
			{
				m_RemoveAllButton.Enabled = true;
			}
		}
		private void OnRemoveSelectedButtonClick(NEventArgs args)
		{
			NList<NListBoxItem> selected = m_ListBox.Selection.SelectedItems;
			for (int i = 0; i < selected.Count; i++)
			{
				m_ListBox.Items.Remove(selected[i]);
			}

			if (m_ListBox.Items.Count == 0)
			{
				m_RemoveAllButton.Enabled = false;
				m_RemoveSelectedButton.Enabled = false;
			}
		}
		private void OnRemoveAllButtonClick(NEventArgs args)
		{
			m_ListBox.Items.Clear();
			m_RemoveAllButton.Enabled = false;
			m_RemoveSelectedButton.Enabled = false;
		}

		#endregion

		#region Fields

		private NListBox m_ListBox;
		private NExampleEventsLog m_EventsLog;

		private NButton m_AddButton;
		private NButton m_RemoveSelectedButton;
		private NButton m_RemoveAllButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NListBoxItemsManipulationExample.
		/// </summary>
		public static readonly NSchema NListBoxItemsManipulationExampleSchema;

		#endregion
	}
}