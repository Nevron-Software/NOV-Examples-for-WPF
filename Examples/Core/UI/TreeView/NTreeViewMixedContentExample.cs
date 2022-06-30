using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTreeViewMixedContentExample : NExampleBase
	{
		#region Constructors

		public NTreeViewMixedContentExample()
		{
		}
		static NTreeViewMixedContentExample()
		{
			NTreeViewMixedContentExampleSchema = NSchema.Create(typeof(NTreeViewMixedContentExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Random = new Random();

			// Create the tree view
			m_TreeView = new NTreeView();
			m_TreeView.HorizontalPlacement = ENHorizontalPlacement.Left;
			// Check whether the application is in touch mode and set the width of the tree view.
			bool touchMode = NApplication.Desktop.TouchMode;
			m_TreeView.PreferredWidth = touchMode? 300: 200;

			// Add some items
			for (int i = 0; i < 32; i++)
			{
				bool checkBox = (i >= 8 && i < 16) || i >= 24;
				bool image = i >= 16;

				NTreeViewItem l1Item = CreateTreeViewItem(String.Format("Item {0}", i), checkBox, image);
				m_TreeView.Items.Add(l1Item);

				for (int j = 0; j < 8; j++)
				{
					NTreeViewItem l2Item = CreateTreeViewItem(String.Format("Item {0}.{1}", i, j), checkBox, image);
					l1Item.Items.Add(l2Item);

					for (int k = 0; k < 2; k++)
					{
						NTreeViewItem l3Item = CreateTreeViewItem(String.Format("Item {0}.{1}.{2}", i, j, k), checkBox, image);
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
	This example demonstrates how to create a tree view and add items with various content to it - text only items, items with image and text,
	checkable items and so on. Using the controls to the right you can modify the appearance and the behavior of the tree view.
</p>
";
		}

		#endregion

		#region Implementation

		private NTreeViewItem CreateTreeViewItem(string text, bool hasCheckBox, bool image)
		{
			if (hasCheckBox == false && image == false)
			{
				NTreeViewItem item = new NTreeViewItem(text);
				item.Tag = text;
				return item;
			}

			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalSpacing = 3;

			if (hasCheckBox)
			{
				NCheckBox checkBox = new NCheckBox();
				checkBox.VerticalPlacement = ENVerticalPlacement.Center;
				stack.Add(checkBox);
			}

			if (image)
			{
				string[] imageNames = new string[] { "Calendar", "Contacts", "Folders", "Journal", "Mail", "Notes", "Shortcuts", "Tasks" };
				int index = m_Random.Next(imageNames.Length);
				string imageName = imageNames[index];

				NImage icon = new NImage(new NEmbeddedResourceRef(NResources.Instance, "RIMG__16x16_" + imageName + "_png"));

				NImageBox imageBox = new NImageBox(icon);
				imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
				imageBox.VerticalPlacement = ENVerticalPlacement.Center;

				stack.Add(imageBox);
			}

			NLabel label = new NLabel(text);
			label.VerticalPlacement = ENVerticalPlacement.Center;
			stack.Add(label);

			NTreeViewItem treeViewItem = new NTreeViewItem(stack);
			treeViewItem.Tag = text;
			treeViewItem.ExpandedChanged += new Function<NValueChangeEventArgs>(OnTreeViewItemExpandedChanged); 

			return treeViewItem;
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
		private Random m_Random;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTreeViewMixedContentExample.
		/// </summary>
		public static readonly NSchema NTreeViewMixedContentExampleSchema;

		#endregion
	}
}