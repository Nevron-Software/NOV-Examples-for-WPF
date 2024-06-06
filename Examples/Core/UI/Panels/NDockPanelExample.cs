using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NDockPanelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDockPanelExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDockPanelExample()
		{
			NDockPanelExampleSchema = NSchema.Create(typeof(NDockPanelExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a dock panel with red border
			m_DockPanel = new NDockPanel();
			m_DockPanel.SetBorder(1, NColor.Red);

            // Create and dock several widgets
            NWidget widget = CreateDockedWidget(ENDockArea.Left);
			widget.PreferredSize = new NSize(100, 100);
			m_DockPanel.Add(widget);

			widget = CreateDockedWidget(ENDockArea.Top);
			widget.PreferredSize = new NSize(100, 100);
			m_DockPanel.Add(widget);

			widget = CreateDockedWidget(ENDockArea.Right);
			widget.PreferredSize = new NSize(100, 100);
			m_DockPanel.Add(widget);

			widget = CreateDockedWidget(ENDockArea.Bottom);
			widget.PreferredSize = new NSize(100, 100);
			m_DockPanel.Add(widget);

			widget = CreateDockedWidget(ENDockArea.Center);
			widget.PreferredSize = new NSize(300, 300);
			m_DockPanel.Add(widget);

			return m_DockPanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// properties stack
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_DockPanel).CreatePropertyEditors(
				m_DockPanel,
				NDockPanel.EnabledProperty,
				NDockPanel.HorizontalPlacementProperty,
				NDockPanel.VerticalPlacementProperty,
				NDockPanel.VerticalSpacingProperty,
				NDockPanel.HorizontalSpacingProperty,
				NDockPanel.UniformWidthsProperty,
				NDockPanel.UniformHeightsProperty);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// items stack
			NStackPanel itemsStack = new NStackPanel();
			m_DockAreaCombo = new NComboBox();
			m_DockAreaCombo.Items.Add(new NComboBoxItem("Left"));
			m_DockAreaCombo.Items.Add(new NComboBoxItem("Top"));
			m_DockAreaCombo.Items.Add(new NComboBoxItem("Right"));
			m_DockAreaCombo.Items.Add(new NComboBoxItem("Bottom"));
			m_DockAreaCombo.Items.Add(new NComboBoxItem("Center"));
			m_DockAreaCombo.SelectedIndex = 1;
			NLabel dockAreaLabel = new NLabel("Dock Area:");
			dockAreaLabel.VerticalPlacement = ENVerticalPlacement.Center;
			itemsStack.Add(new NPairBox(dockAreaLabel, m_DockAreaCombo, true));

			NButton addSmallItemButton = new NButton("Add Small Item");
			addSmallItemButton.Click += new Function<NEventArgs>(OnAddSmallItemButtonClick);
			itemsStack.Add(addSmallItemButton);

			NButton addLargeItemButton = new NButton("Add Large Item");
			addLargeItemButton.Click += new Function<NEventArgs>(OnAddLargeItemButtonClick);
			itemsStack.Add(addLargeItemButton);

			NButton addRandomItemButton = new NButton("Add Random Item");
			addRandomItemButton.Click += new Function<NEventArgs>(OnAddRandomItemButtonClick);
			itemsStack.Add(addRandomItemButton);

			NButton removeAllItemsButton = new NButton("Remove All Items");
			removeAllItemsButton.Click += new Function<NEventArgs>(OnRemoveAllItemsButtonClick);
			itemsStack.Add(removeAllItemsButton);

			stack.Add(new NGroupBox("Items", itemsStack));
			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a dock layout panel and add
	widgets to it specifying the dock area each widget should be placed in.
</p>
";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Gets the currently selected dock area from the DockAreaCombo.
		/// </summary>
		/// <returns></returns>
		private ENDockArea GetCurrentDockArea()
		{
			switch (m_DockAreaCombo.SelectedIndex)
			{
				case 0:
					return ENDockArea.Left;
				case 1:
					return ENDockArea.Top;
				case 2:
					return ENDockArea.Right;
				case 3:
					return ENDockArea.Bottom;
				case 4:
					return ENDockArea.Center;
				default:
					return ENDockArea.Top;
			}
		}
		/// <summary>
		/// Sets the currently selected dock area to the specified widget.
		/// </summary>
		/// <param name="widget"></param>
		private void SetCurrentDockArea(NWidget widget)
		{
			SetDockArea(widget, GetCurrentDockArea());
		}
		/// <summary>
		/// Sets a custom dock area to the specified widget and colors its background accordingly
		/// </summary>
		/// <param name="widget"></param>
		/// <param name="dockArea"></param>
		private void SetDockArea(NWidget widget, ENDockArea dockArea)
		{
			NDockLayout.SetDockArea(widget, dockArea);
			switch (dockArea)
			{
				case ENDockArea.Bottom:
					widget.BackgroundFill = new NColorFill(new NColor(0, 162, 232));
					break;
				case ENDockArea.Center:
					widget.BackgroundFill = new NColorFill(new NColor(239, 228, 176));
					break;
				case ENDockArea.Left:
					widget.BackgroundFill = new NColorFill(new NColor(34, 177, 76));
					break;
				case ENDockArea.Right:
					widget.BackgroundFill = new NColorFill(new NColor(163, 73, 164));
					break;
				case ENDockArea.Top:
					widget.BackgroundFill = new NColorFill(new NColor(237, 28, 36));
					break;
				default:
					throw new Exception("New ENDockArea?");
			}
		}

		/// <summary>
		/// Creates the default docked widget for this example, that is docked to the current dock area.
		/// </summary>
		/// <returns></returns>
		private NWidget CreateDockedWidget()
		{
			return CreateDockedWidget(GetCurrentDockArea());
		}
		/// <summary>
		/// Creates the default docked widget for this example, that is docked to the specified area.
		/// </summary>
		/// <param name="dockArea"></param>
		/// <returns></returns>
		private NWidget CreateDockedWidget(ENDockArea dockArea)
		{
			NLabel label = new NLabel(dockArea.ToString() + "(" + m_DockPanel.Count.ToString() + ")");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;

			NWidget widget = new NContentHolder(label);
			widget.Border = NBorder.CreateFilledBorder(NColor.Black);
			widget.BorderThickness = new NMargins(1);
			SetDockArea(widget, dockArea);
			return widget;
		}

		#endregion

		#region Event Handlers

		private void OnAddSmallItemButtonClick(NEventArgs args)
		{
			NWidget item = CreateDockedWidget();
			item.MinSize = new NSize(20, 20);
			item.PreferredSize = new NSize(60, 60);
			m_DockPanel.Add(item);
		}
		private void OnAddLargeItemButtonClick(NEventArgs args)
		{
			NWidget item = CreateDockedWidget();
			item.MinSize = new NSize(40, 40);
			item.PreferredSize = new NSize(100, 100);
			m_DockPanel.Add(item);
		}
		private void OnAddRandomItemButtonClick(NEventArgs args)
		{
			int range = 50;
			Random rnd = new Random();

			NWidget item = CreateDockedWidget();
			item.MinSize = new NSize(rnd.Next(range), rnd.Next(range));
			item.PreferredSize = new NSize(rnd.Next(range) + range, rnd.Next(range) + range);
			m_DockPanel.Add(item);
		}
		private void OnRemoveAllItemsButtonClick(NEventArgs args)
		{
			m_DockPanel.Clear();
		}

		#endregion

		#region Fields

		private NComboBox m_DockAreaCombo;
		private NDockPanel m_DockPanel;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDockPanelExample.
		/// </summary>
		public static readonly NSchema NDockPanelExampleSchema;

		#endregion
	}
}