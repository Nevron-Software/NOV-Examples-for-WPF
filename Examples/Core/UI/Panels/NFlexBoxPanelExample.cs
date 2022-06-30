using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using Nevron.Nov.Layout;

namespace Nevron.Nov.Examples.UI
{
	public class NFlexBoxPanelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFlexBoxPanelExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFlexBoxPanelExample()
		{
			NFlexBoxPanelExampleSchema = NSchema.Create(typeof(NFlexBoxPanelExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NMargins labelPadding = new NMargins(2);

			m_FlexBoxPanel = new NFlexBoxPanel();
			m_FlexBoxPanel.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_FlexBoxPanel.VerticalPlacement = ENVerticalPlacement.Top;
			m_FlexBoxPanel.PreferredHeight = 200;

			// Set the Grow and Shrink extended properties of the first label explicitly
			NLabel label1 = new NLabel("Label 1 - Grow: 1, Shrink: 1");
			label1.BackgroundFill = new NColorFill(NColor.Gold);
			label1.Padding = labelPadding;
			NFlexBoxLayout.SetGrow(label1, 1);
			NFlexBoxLayout.SetShrink(label1, 1);
			m_FlexBoxPanel.Add(label1);

			// Pass the values of the Grow and Shrink extended properties of the second label
			// to the Add method of the panel
			NLabel label2 = new NLabel("Label 2 - Grow: 3, Shrink: 3");
			label2.BackgroundFill = new NColorFill(NColor.Orange);
			label2.Padding = labelPadding;
			m_FlexBoxPanel.Add(label2, 3, 3);

			// The third label will have the default values for Grow and Shrink - 0
			NLabel label3 = new NLabel("Label 3 - Grow: 0, Shrink: 0");
			label3.BackgroundFill = new NColorFill(NColor.Red);
			label3.Padding = labelPadding;
			m_FlexBoxPanel.Add(label3);

			return m_FlexBoxPanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// properties stack
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_FlexBoxPanel).CreatePropertyEditors(
				m_FlexBoxPanel,
				NFlexBoxPanel.EnabledProperty,
				NFlexBoxPanel.HorizontalPlacementProperty,
				NFlexBoxPanel.VerticalPlacementProperty,
				NFlexBoxPanel.DirectionProperty,
				NFlexBoxPanel.VerticalSpacingProperty,
				NFlexBoxPanel.HorizontalSpacingProperty,
				NFlexBoxPanel.UniformWidthsProperty,
				NFlexBoxPanel.UniformHeightsProperty
			);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// items stack
			NStackPanel itemsStack = new NStackPanel();

			m_GrowUpDown = new NNumericUpDown(0, 100, 0);
			itemsStack.Add(NPairBox.Create("Grow Factor: ", m_GrowUpDown));

			m_ShrinkUpDown = new NNumericUpDown(0, 100, 0);
			itemsStack.Add(NPairBox.Create("Shrink Factor: ", m_ShrinkUpDown));

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

			stack.Add(new NGroupBox("Items", new NUniSizeBoxGroup(itemsStack)));

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a flexbox layout panel and add widgets to it.
	You can control the parameters of the layout algorithm using the controls on the right.
	The Grow and Shrink factors of the widgets determine the portion of area to use for growing/shrinking.
	They are by default set to 0, which means that the widget will not grow or shrink.
</p>
<p>
	To test how the Grow and Shrink factors affect the layout, click the <b>Remove All Items</b> button
	on the right, then set Grow and Shrink factor and click any of the <b>Add ... Item</b> buttons.
	Then update the Grow and Shrink factor if you want, click any of the <b>Add ... Item</b> buttons again and so on.
</p>
";
		}

		#endregion

		#region Implementation

		private void SetGrowAndShrink(NWidget widget)
		{
			double grow = m_GrowUpDown.Value;
			if (grow != 0)
			{
				NFlexBoxLayout.SetGrow(widget, grow);
			}

			double shrink = m_ShrinkUpDown.Value;
			if (shrink != 0)
			{
				NFlexBoxLayout.SetShrink(widget, shrink);
			}
		}

		#endregion

		#region Event Handlers

		private void OnAddSmallItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Small " + m_FlexBoxPanel.Count.ToString());
			item.MinSize = new NSize(5, 5);
			item.PreferredSize = new NSize(25, 25);
			SetGrowAndShrink(item);
			m_FlexBoxPanel.Add(item);
		}
		private void OnAddLargeItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Large " + m_FlexBoxPanel.Count.ToString());
			item.MinSize = new NSize(20, 20);
			item.PreferredSize = new NSize(60, 60);
			SetGrowAndShrink(item);
			m_FlexBoxPanel.Add(item);
		}
		private void OnAddRandomItemButtonClick(NEventArgs args)
		{
			int range = 30;
			Random rnd = new Random();

			NButton item = new NButton("Random " + m_FlexBoxPanel.Count.ToString());
			item.MinSize = new NSize(rnd.Next(range), rnd.Next(range));
			item.PreferredSize = new NSize(rnd.Next(range) + range, rnd.Next(range) + range);
			SetGrowAndShrink(item);
			m_FlexBoxPanel.Add(item);
		}
		private void OnRemoveAllItemsButtonClick(NEventArgs args)
		{
			m_FlexBoxPanel.Clear();
		}

		#endregion

		#region Fields

		private NFlexBoxPanel m_FlexBoxPanel;
		private NNumericUpDown m_GrowUpDown;
		private NNumericUpDown m_ShrinkUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NFlexBoxPanelExampleSchema;

		#endregion
	}
}