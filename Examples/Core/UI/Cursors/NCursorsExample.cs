using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCursorsExample : NExampleBase
	{
		#region Constructors

		public NCursorsExample()
		{
		}
		static NCursorsExample()
		{
			NCursorsExampleSchema = NSchema.Create(typeof(NCursorsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			Array predefinedCursors = NEnum.GetValues(typeof(ENPredefinedCursor));

			NStackPanel stack = new NStackPanel();

			NGroupBox predefinedGroupBox = new NGroupBox("Predefined");
			stack.Add(predefinedGroupBox);

			NSplitter splitter = new NSplitter();
			splitter.SplitMode = ENSplitterSplitMode.Proportional;
			splitter.SplitFactor = 0.5d;
			predefinedGroupBox.Content = splitter;

			for (int i = 0; i < 2; i++)
			{
				NStackPanel pstack = new NStackPanel();
				pstack.VerticalSpacing = 1;
				switch (i)
				{
					case 0:
						splitter.Pane1.Content = new NGroupBox("Use Native If Possible", pstack);
						break;
					case 1:
						splitter.Pane2.Content = new NGroupBox("Use Built-In", pstack);
						break;
					default:
						throw new Exception("More cases?"); 
				}

				for (int j = 0; j < predefinedCursors.Length; j++)
				{
					ENPredefinedCursor predefinedCursor = (ENPredefinedCursor)predefinedCursors.GetValue(j);
                    NWidget element = CreateDemoElement(NStringHelpers.InsertSpacesBeforeUppersAndDigits(predefinedCursor.ToString()));
					element.Cursor = new NCursor(predefinedCursor, i == 0);
					pstack.Add(element);
				}
			}

            NWidget customElement = CreateDemoElement("Custom");
			customElement.Cursor = NResources.Cursor_CustomCursor_cur;

			NGroupBox customGroupBox = new NGroupBox("Custom", customElement);
			stack.Add(customGroupBox);

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
	This example shows how to use cursors. NOV supports 2 types of cursors - <b>predefined</b> and <b>custom</b>. Predefined cursors can
	be native or built-in. A custom cursor can be loaded from a stream or you can manually specify the values of its pixels.
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
			element.Padding = new NMargins(1);
			return element;
		}

		#endregion

		#region Schema

		public static readonly NSchema NCursorsExampleSchema;

		#endregion
	}
}