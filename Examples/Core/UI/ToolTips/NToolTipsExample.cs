using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTooltipsExample : NExampleBase
	{
		#region Constructors

		public NTooltipsExample()
		{
		}
		static NTooltipsExample()
		{
			NTooltipsExampleSchema = NSchema.Create(typeof(NTooltipsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// create the host
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = 2;

			NGroupBox contentGroupBox = new NGroupBox("Content");
			stack.Add(contentGroupBox);

			NStackPanel contentStack = new NStackPanel();
			contentStack.VerticalSpacing = 2;
			contentGroupBox.Content = contentStack;

			NWidget textTooltip = CreateDemoElement("Text tooltip");
			textTooltip.Tooltip = new NTooltip("Tooltip text");
			contentStack.Add(textTooltip);

			NWidget imageTooltip = CreateDemoElement("Image tooltip");
			imageTooltip.Tooltip = new NTooltip(NResources.Image__48x48_Book_png);
			contentStack.Add(imageTooltip);

			NWidget richTooltip = CreateDemoElement("Rich tooltip");
			NStackPanel richTooltipContent = new NStackPanel();
			richTooltipContent.Add(new NLabel("The tooltip can contain any type of Nevron Open Vision Content"));
			richTooltipContent.Add(new NImageBox(NResources.Image__48x48_Book_png));
			richTooltip.Tooltip = new NTooltip(richTooltipContent);
			contentStack.Add(richTooltip);

			NWidget dynamicContentTooltip = CreateDemoElement("Dynamic content");
			dynamicContentTooltip.Tooltip = new NDynamicContentTooltip();
			contentStack.Add(dynamicContentTooltip);

			NGroupBox behaviorGroupBox = new NGroupBox("Behavior");
			stack.Add(behaviorGroupBox);

			NStackPanel behaviorStack = new NStackPanel();
			behaviorStack.VerticalSpacing = 2;
			behaviorGroupBox.Content = behaviorStack;

			NWidget followMouse = CreateDemoElement("Follow mouse");
			followMouse.Tooltip = new NTooltip("I am following the mouse");
			followMouse.Tooltip.FollowMouse = true;
			behaviorStack.Add(followMouse);

			NWidget instantShowTooltip = CreateDemoElement("Shown instantly");
			instantShowTooltip.Tooltip = new NTooltip("I was shown instantly");
			instantShowTooltip.Tooltip.FirstShowDelay = 0;
			instantShowTooltip.Tooltip.NextShowDelay = 0;
			behaviorStack.Add(instantShowTooltip);

			NWidget doNotCloseOnClick = CreateDemoElement("Do not close on click");
			doNotCloseOnClick.Tooltip = new NTooltip("I am not closed on click");
			doNotCloseOnClick.Tooltip.CloseOnMouseDown = false;
			behaviorStack.Add(doNotCloseOnClick);

			NGroupBox positionGroupBox = new NGroupBox("Position");
			stack.Add(positionGroupBox);

			NTableFlowPanel positionTable = new NTableFlowPanel();
			positionGroupBox.Content = positionTable;

			positionTable.HorizontalSpacing = 2;
			positionTable.VerticalSpacing = 2;
			positionTable.Direction = ENHVDirection.LeftToRight;
			positionTable.MaxOrdinal = 3;

			foreach (ENTooltipPosition pos in NEnum.GetValues(typeof(ENTooltipPosition)))
			{
				NWidget posElement = CreateDemoElement(pos.ToString());
				posElement.Tooltip = new NTooltip(pos.ToString());

				posElement.Tooltip.FirstShowDelay = 0;
				posElement.Tooltip.NextShowDelay = 0;
				posElement.Tooltip.Position = pos;
				posElement.Tooltip.FollowMouse = true;
				posElement.Tooltip.ShowDuration = -1;
				positionTable.Add(posElement);
			}

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use tooltips. The tooltip is a container for the information
	that can be displayed when the mouse moves over a certain element or area. The content of the tooltip can
	be set through its constructor or through its <b>Content</b> property. The content can be any object and is
	converted to an <b>NWidget</b> by the <b>GetContent()</b> method of the <b>NTooltip</b> class. The <b>NTooltip</b>
	class provides several properties that allows you to control the tooltip position, show delay and duration,
	whether it should follow the mouse cursor or not and so on.
</p>
";
		}

		#endregion

		#region Implementation

		private NWidget CreateDemoElement(string text)
		{
			NContentHolder element = new NContentHolder(text);
			element.Border = NBorder.CreateFilledBorder(NColor.Black, 2, 5);
			element.BorderThickness = new NMargins(1);
			element.BackgroundFill = new NColorFill(NColor.PapayaWhip);
			element.TextFill = new NColorFill(NColor.Black);
			element.Padding = new NMargins(10);
			return element;
		}

		#endregion

		#region Schema

		public static readonly NSchema NTooltipsExampleSchema;

		#endregion

		#region Nested Types - NDynamicContentTooltip

		/// <summary>
		/// A tooltip that shows as content the current date and time
		/// </summary>
		public class NDynamicContentTooltip : NTooltip
		{
			#region Constructors

			public NDynamicContentTooltip()
			{

			}
			static NDynamicContentTooltip()
			{
				NDynamicContentTooltipSchema = NSchema.Create(typeof(NDynamicContentTooltip), NTooltip.NTooltipSchema);
			}

			#endregion

			#region Overrides - GetContent()

			public override NWidget GetContent()
			{
				DateTime now = DateTime.Now;
				return new NLabel("I was shown at: " + now.ToString("T"));
			}

			#endregion

			#region Schema

			public static readonly NSchema NDynamicContentTooltipSchema;

			#endregion
		}

		#endregion
	}
}