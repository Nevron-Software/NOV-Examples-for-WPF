using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NSplitterExample : NExampleBase
	{
		#region Constructors

		public NSplitterExample()
		{
		}
		static NSplitterExample()
		{
			NSplitterExampleSchema = NSchema.Create(typeof(NSplitterExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a splitter
			m_Splitter = new NSplitter();

			m_Splitter.Pane1.Content = new NLabel("Pane 1");
			m_Splitter.Pane1.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			m_Splitter.Pane1.BackgroundFill = new NColorFill(NColor.LightGreen);
			m_Splitter.Pane1.Border = NBorder.CreateFilledBorder(NColor.Black);
			m_Splitter.Pane1.BorderThickness = new NMargins(1);

			m_Splitter.Pane2.Content = new NLabel("Pane 2");
			m_Splitter.Pane2.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			m_Splitter.Pane2.BackgroundFill = new NColorFill(NColor.LightBlue);
			m_Splitter.Pane2.Border = NBorder.CreateFilledBorder(NColor.Black);
			m_Splitter.Pane2.BorderThickness = new NMargins(1);

			m_Splitter.ResizeStep = 20;

			// Host it
			return m_Splitter;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Create some splitter property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Splitter).CreatePropertyEditors(
				m_Splitter,
				NSplitterBase.OrientationProperty,
				NSplitterBase.ResizeWhileDraggingProperty,
				NSplitterBase.ResizeStepProperty
			);

			NStackPanel propertyStack = new NStackPanel();			
			for (int i = 0; i < editors.Count; i++)
			{
				propertyStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Splitter Properties", propertyStack));

			// Create splitter thumb property editors
			editors = NDesigner.GetDesigner(m_Splitter.Thumb).CreatePropertyEditors(
				m_Splitter.Thumb,
				NSplitterThumb.SplitModeProperty,
				NSplitterThumb.SplitOffsetProperty,
				NSplitterThumb.SplitFactorProperty,
				NSplitterThumb.CollapseModeProperty
			);

			propertyStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertyStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Splitter Thumb Properties", propertyStack));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and configure a splitter. The splitter is a widget that splits
	its content area into two resizable panes, which can be interactively resized with help of a thumb located
	in the middle. Using the <b>Orientation</b> property you can specify whether the splitter is horizontal or
	vertical. To control how the splitter splits its content area you can use the <b>SplitMode</b> property.
</p>
";
		}

		#endregion

		#region Fields

		private NSplitter m_Splitter;

		#endregion

		#region Schema

		public static readonly NSchema NSplitterExampleSchema;

		#endregion
	}
}