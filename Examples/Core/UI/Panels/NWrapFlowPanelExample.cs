using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NWrapFlowPanelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NWrapFlowPanelExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NWrapFlowPanelExample()
		{
			NWrapFlowPanelExampleSchema = NSchema.Create(typeof(NWrapFlowPanelExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_WrapFlowPanel = new NWrapFlowPanel();
			m_WrapFlowPanel.SetBorder(1, NColor.Red);

            for (int i = 1; i <= 16; i++)
			{
				m_WrapFlowPanel.Add(new NButton("Button " + i.ToString()));
			}

			return m_WrapFlowPanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_WrapFlowPanel).CreatePropertyEditors(
				m_WrapFlowPanel,
				NWrapFlowPanel.EnabledProperty,
				NWrapFlowPanel.HorizontalPlacementProperty,
				NWrapFlowPanel.VerticalPlacementProperty,
				NWrapFlowPanel.DirectionProperty,
				NWrapFlowPanel.VerticalSpacingProperty,
				NWrapFlowPanel.HorizontalSpacingProperty,
				NWrapFlowPanel.LaneFillModeProperty,
				NWrapFlowPanel.FillModeProperty,
				NWrapFlowPanel.FitModeProperty,
				NWrapFlowPanel.InvertedProperty,
				NWrapFlowPanel.UniformWidthsProperty,
				NWrapFlowPanel.UniformHeightsProperty);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// items stack
			NStackPanel itemsStack = new NStackPanel();
			
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

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a wrap flow layout panel and add
	widgets to it. You can control the parameters of the layout algorithm
	using the controls to the right.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnAddSmallItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Small." + m_WrapFlowPanel.Count.ToString());
			item.MinSize = new NSize(5, 5);
			item.PreferredSize = new NSize(25, 25);
			m_WrapFlowPanel.Add(item);
		}
		private void OnAddLargeItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Large." + m_WrapFlowPanel.Count.ToString());
			item.MinSize = new NSize(20, 20);
			item.PreferredSize = new NSize(60, 60);
			m_WrapFlowPanel.Add(item);
		}
		private void OnAddRandomItemButtonClick(NEventArgs args)
		{
			int range = 30;
			Random rnd = new Random();
			NButton item = new NButton("Random." + m_WrapFlowPanel.Count.ToString());
			item.MinSize = new NSize(rnd.Next(range), rnd.Next(range));
			item.PreferredSize = new NSize(rnd.Next(range) + range, rnd.Next(range) + range);
			m_WrapFlowPanel.Add(item);
		}
		private void OnRemoveAllItemsButtonClick(NEventArgs args)
		{
			m_WrapFlowPanel.Clear();
		}

		#endregion

		#region Fields

		NWrapFlowPanel m_WrapFlowPanel;

		#endregion

		#region Schema

		public static readonly NSchema NWrapFlowPanelExampleSchema;

		#endregion
	}
}