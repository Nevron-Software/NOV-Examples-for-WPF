using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.UI
{
	public class NListBoxFirstLookExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NListBoxFirstLookExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NListBoxFirstLookExample()
		{
			NListBoxFirstLookExampleSchema = NSchema.Create(typeof(NListBoxFirstLookExample), NExampleBase.NExampleBaseSchema);
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
			stack.Add(CreatePropertiesGroupBox());

			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a simple list box with text only items. You can use the controls
	on the right to modify various properties of the list box.
</p>
";
		}

		#endregion

		#region Implementation

		private NGroupBox CreatePropertiesGroupBox()
		{
			NStackPanel propertiesStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ListBox).CreatePropertyEditors(m_ListBox,
				NListBox.EnabledProperty,
				NListBox.HorizontalPlacementProperty,
				NListBox.VerticalPlacementProperty,
				NListBox.HScrollModeProperty,
				NListBox.VScrollModeProperty,
				NListBox.NoScrollHAlignProperty,
				NListBox.NoScrollVAlignProperty,
				NListBox.IntegralVScrollProperty
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
			return propertiesGroupBox;
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
		/// Schema associated with NListBoxFirstLookExample.
		/// </summary>
		public static readonly NSchema NListBoxFirstLookExampleSchema;

		#endregion
	}
}