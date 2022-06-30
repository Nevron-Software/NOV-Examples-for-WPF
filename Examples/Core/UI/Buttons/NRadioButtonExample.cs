using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Graphics;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.UI
{
	public class NRadioButtonExample : NExampleBase
	{
		#region Constructors

		public NRadioButtonExample()
		{
		}
		static NRadioButtonExample()
		{
			NRadioButtonExampleSchema = NSchema.Create(typeof(NRadioButtonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NTab tab = new NTab();

			// create a tab page with vertically arranged radio buttons
			NTabPage verticalTabPage = new NTabPage("Vertical Radio Group");
			tab.TabPages.Add(verticalTabPage);

			NRadioButtonGroup verticalRadioGroup = new NRadioButtonGroup();
			verticalRadioGroup.HorizontalPlacement = ENHorizontalPlacement.Left;
			verticalRadioGroup.VerticalPlacement = ENVerticalPlacement.Top;
			verticalTabPage.Content = verticalRadioGroup;
			
			NStackPanel verticalStack = new NStackPanel();
			verticalRadioGroup.Content = verticalStack;

			for (int i = 0; i < 5; i++)
			{
				NRadioButton radioButton = new NRadioButton("Item " + i.ToString());
				verticalStack.Add(radioButton);
			}

			NRadioButton disabledRadioButton1 = new NRadioButton("Disabled");
			disabledRadioButton1.Enabled = false;
			verticalStack.Add(disabledRadioButton1);
			verticalRadioGroup.SelectedIndexChanged += OnVerticalRadioGroupSelectedIndexChanged;

			// create a tab page with horizontally arranged radio buttons
			NTabPage horizontalTabPage = new NTabPage("Horizontal Radio Group");
			tab.TabPages.Add(horizontalTabPage);

			NRadioButtonGroup horizontalRadioGroup = new NRadioButtonGroup();
			horizontalRadioGroup.VerticalPlacement = ENVerticalPlacement.Top;
			horizontalRadioGroup.HorizontalPlacement = ENHorizontalPlacement.Left;
			horizontalTabPage.Content = horizontalRadioGroup;

			NStackPanel horizontalStack = new NStackPanel();
			horizontalStack.Direction = ENHVDirection.LeftToRight;
			horizontalRadioGroup.Content = horizontalStack;

            for (int i = 0; i < 5; i++)
			{
				NRadioButton radioButton = new NRadioButton("Item " + i.ToString());
				horizontalStack.Add(radioButton);
			}

			NRadioButton disabledRadioButton2 = new NRadioButton("Disabled");
			disabledRadioButton2.Enabled = false;
			horizontalStack.Add(disabledRadioButton2);
			horizontalRadioGroup.SelectedIndexChanged += OnHorizontalRadioGroupSelectedIndexChanged;
			return tab;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// create the events list box
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use radio buttons and radio button groups. 
</p>
<p>
    A radio button (NRadioButton) is a kind of a toggle button, which is intended to be placed inside the sub-hierarchy of a radio button group (NRadioButtonGroup).
    When there are multiple radio buttons inside the radio button group sub-hierarchy, only one can be checked at a time. 
    Checking a different radio button will automatically uncheck the previously checked one.
</p>
<p>
    The radio button group (NRadioButtonGroup) is a content container, thus it is up to the developer to choose the style in which the radio buttons are arranged.
    In this example we have created two radio groups, each of which holding a stack panel (horizontal or vertical) that contain radio buttons. 
    It is however possible to arrange the radio buttons in any way that you see fit.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnHorizontalRadioGroupSelectedIndexChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Horizontal Radio Selected: " + (int)args.NewValue);
		}
		private void OnVerticalRadioGroupSelectedIndexChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Vertical Radio Selected: " + (int)args.NewValue);
		}
		
		#endregion

		#region Fields

		NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NRadioButtonExampleSchema;

		#endregion
	}
}