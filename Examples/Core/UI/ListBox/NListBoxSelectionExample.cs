using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NListBoxSelectionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NListBoxSelectionExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NListBoxSelectionExample()
		{
			NListBoxSelectionExampleSchema = NSchema.Create(typeof(NListBoxSelectionExample), NExampleBase.NExampleBaseSchema);
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
			for (int i = 0; i < 100; i++)
			{
				m_ListBox.Items.Add(new NListBoxItem("Item " + i.ToString()));
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

			// Create the properties group box
			NStackPanel propertiesStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ListBox).CreatePropertyEditors(m_ListBox,
				NListBox.EnabledProperty,
				NListBox.HorizontalPlacementProperty,
				NListBox.VerticalPlacementProperty
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			NPropertyEditor editor = NDesigner.GetDesigner(m_ListBox.Selection).CreatePropertyEditor(m_ListBox.Selection,
				NListBoxSelection.ModeProperty);
			NLabel label = (NLabel)editor.GetDescendants(new NInstanceOfSchemaFilter(NLabel.NLabelSchema))[0];
			label.Text = "Selection Mode:";
			propertiesStack.Add(editor);

			NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
			stack.Add(propertiesGroupBox);

			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;			
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to work with list box selection. You can change the selection mode
	using the ""Selection Mode"" combo box on the right. When in <b>Multiple</b> selection mode you
	can hold &lt;Shift&gt; or &lt;Ctrl&gt; to select multiple items from the list box.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> args)
		{
			NListBoxItem item = args.Item;
			int index = item.GetAggregationInfo().Index;
			m_EventsLog.LogEvent("Selected Item: " + index.ToString());
		}
		private void OnListBoxItemDeselected(NSelectEventArgs<NListBoxItem> args)
		{
			NListBoxItem item = args.Item;
			int index = item.GetAggregationInfo().Index;
			m_EventsLog.LogEvent("Deselected Item: " + index.ToString());
		}

		#endregion

		#region Fields

		private NListBox m_ListBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NListBoxSelectionExample.
		/// </summary>
		public static readonly NSchema NListBoxSelectionExampleSchema;

		#endregion
	}
}