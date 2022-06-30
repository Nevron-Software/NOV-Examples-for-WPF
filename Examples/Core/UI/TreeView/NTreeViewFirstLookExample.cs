using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTreeViewFirstLookExample : NExampleBase
	{
		#region Constructors

		public NTreeViewFirstLookExample()
		{
		}
		static NTreeViewFirstLookExample()
		{
			NTreeViewFirstLookExampleSchema = NSchema.Create(typeof(NTreeViewFirstLookExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the tree view
			m_TreeView = new NTreeView();
			m_TreeView.HorizontalPlacement = ENHorizontalPlacement.Left;
			// Check whether the application is in touch mode and set the width of the tree view.
			bool touchMode = NApplication.Desktop.TouchMode;
			m_TreeView.PreferredWidth = touchMode ? 300 : 200;

			// Add some items
			for (int i = 0; i < 32; i++)
			{
				NTreeViewItem l1Item = CreateTreeViewItem(String.Format("Item {0}", i));
				m_TreeView.Items.Add(l1Item);

				for (int j = 0; j < 8; j++)
				{
					NTreeViewItem l2Item = CreateTreeViewItem(String.Format("Item {0}.{1}", i, j));
					l1Item.Items.Add(l2Item);

					for (int k = 0; k < 2; k++)
					{
						NTreeViewItem l3Item = CreateTreeViewItem(String.Format("Item {0}.{1}.{2}", i, j, k));
						l2Item.Items.Add(l3Item);
					}
				}
			}

			// Hook to tree view events
			m_TreeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnTreeViewSelectedPathChanged);
			return m_TreeView;
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
	This example demonstrates how to create a tree view with text only items. You can use the controls
	on the right to modify various properties of the tree box.
</p>
";
		}

		#endregion

		#region Implementation

		private NGroupBox CreatePropertiesGroupBox()
		{
			NStackPanel propertiesStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TreeView).CreatePropertyEditors(m_TreeView,
				NTreeView.EnabledProperty,
				NTreeView.HorizontalPlacementProperty,
				NTreeView.VerticalPlacementProperty,
				NTreeView.NoScrollHAlignProperty,
				NTreeView.NoScrollVAlignProperty,
				NTreeView.HScrollModeProperty,
				NTreeView.VScrollModeProperty,
				NTreeView.IntegralVScrollProperty				
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
			return propertiesGroupBox;
		}
		private NTreeViewItem CreateTreeViewItem(string text)
		{
			NTreeViewItem item = new NTreeViewItem(text);
			item.Tag = text;
			item.ExpandedChanged += new Function<NValueChangeEventArgs>(OnTreeViewItemExpandedChanged);
			return item;
		}

		#endregion

		#region Event Handlers

		private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Selected: " + m_TreeView.SelectedItem.Tag.ToString());
		}
		private void OnTreeViewItemExpandedChanged(NValueChangeEventArgs args)
		{
			NTreeViewItem item = (NTreeViewItem)args.TargetNode;
			if (item.Expanded)
			{
				m_EventsLog.LogEvent("Expanded: " + item.Tag.ToString());
			}
			else
			{
				m_EventsLog.LogEvent("Collapsed: " + item.Tag.ToString());
			}
		}

		#endregion

		#region Fields

		private NTreeView m_TreeView;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTreeViewFirstLookExample.
		/// </summary>
		public static readonly NSchema NTreeViewFirstLookExampleSchema;

		#endregion
	}
}