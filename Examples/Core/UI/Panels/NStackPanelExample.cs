using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NStackPanelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStackPanelExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStackPanelExample()
		{
			NStackPanelExampleSchema = NSchema.Create(typeof(NStackPanelExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_StackPanel = new NStackPanel();			
			m_StackPanel.SetBorder(1, NColor.Red);

            for (int i = 1; i <= 16; i++)
			{
				m_StackPanel.Add(new NButton("Button " + i.ToString()));
			}

			return m_StackPanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// properties stack
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_StackPanel).CreatePropertyEditors(
				m_StackPanel,
				NStackPanel.EnabledProperty,
				NStackPanel.HorizontalPlacementProperty,
				NStackPanel.VerticalPlacementProperty,
				NStackPanel.DirectionProperty,
				NStackPanel.VerticalSpacingProperty,
				NStackPanel.HorizontalSpacingProperty,
				NStackPanel.FitModeProperty,
				NStackPanel.FillModeProperty,
				NStackPanel.UniformWidthsProperty,
				NStackPanel.UniformHeightsProperty
			);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				NPropertyEditor editor = editors[i];
				if (editor.EditedProperty == NStackPanel.HorizontalSpacingProperty)
				{
					editor.SetEditedLocalValue(NDesign.HorizontalSpacing);
				}
				else if (editor.EditedProperty == NStackPanel.VerticalSpacingProperty)
				{
					editor.SetEditedLocalValue(NDesign.VerticalSpacing);
				}

				propertiesStack.Add(editor);
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
	This example demonstrates how to create a stack layout panel and add
	widgets to it. You can control the parameters of the layout algorithm
	using the controls to the right.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnAddSmallItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Small." + m_StackPanel.Count.ToString());
			item.MinSize = new NSize(5, 5);
			item.PreferredSize = new NSize(25, 25);
			m_StackPanel.Add(item);
		}
		private void OnAddLargeItemButtonClick(NEventArgs args)
		{
			NButton item = new NButton("Large." + m_StackPanel.Count.ToString());
			item.MinSize = new NSize(20, 20);
			item.PreferredSize = new NSize(60, 60);
			m_StackPanel.Add(item);
		}
		private void OnAddRandomItemButtonClick(NEventArgs args)
		{
			int range = 30;
			Random rnd = new Random();
			NButton item = new NButton("Random." + m_StackPanel.Count.ToString());
			item.MinSize = new NSize(rnd.Next(range), rnd.Next(range));
			item.PreferredSize = new NSize(rnd.Next(range) + range, rnd.Next(range) + range);
			m_StackPanel.Add(item);
		}
		private void OnRemoveAllItemsButtonClick(NEventArgs args)
		{
			m_StackPanel.Clear();
		}

		#endregion

		#region Fields

		private NStackPanel m_StackPanel;

		#endregion

		#region Schema

		public static readonly NSchema NStackPanelExampleSchema;

		#endregion
	}
}