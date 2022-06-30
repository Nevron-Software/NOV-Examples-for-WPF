using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMultiSplitterExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMultiSplitterExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMultiSplitterExample()
		{
			NMultiSplitterExampleSchema = NSchema.Create(typeof(NMultiSplitterExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a multi splitter
			m_MultiSplitter = new NMultiSplitter();

			NSplitterPane pane1 = new NSplitterPane();
			pane1.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			pane1.BackgroundFill = new NColorFill(NColor.LightGreen);
			pane1.Border = NBorder.CreateFilledBorder(NColor.Black);
			pane1.BorderThickness = new NMargins(1);
			pane1.Content = new NLabel("Pane 1");
			m_MultiSplitter.Widgets.Add(pane1);

			NSplitterThumb thumb1 = new NSplitterThumb();
			thumb1.CollapseMode = ENSplitterCollapseMode.BothPanes;
			m_MultiSplitter.Widgets.Add(thumb1);

			NSplitterPane pane2 = new NSplitterPane();
			pane2.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			pane2.BackgroundFill = new NColorFill(NColor.LightBlue);
			pane2.Border = NBorder.CreateFilledBorder(NColor.Black);
			pane2.BorderThickness = new NMargins(1);
			pane2.Content = new NLabel("Pane 2");
			m_MultiSplitter.Widgets.Add(pane2);

			NSplitterThumb thumb2 = new NSplitterThumb();
			thumb2.CollapseMode = ENSplitterCollapseMode.BothPanes;
			m_MultiSplitter.Widgets.Add(thumb2);

			NSplitterPane pane3 = new NSplitterPane();
			pane3.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			pane3.BackgroundFill = new NColorFill(NColor.LightYellow);
			pane3.Border = NBorder.CreateFilledBorder(NColor.Black);
			pane3.BorderThickness = new NMargins(1);
			pane3.Content = new NLabel("Pane 3");
			m_MultiSplitter.Widgets.Add(pane3);

			return m_MultiSplitter;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			// Create multi splitter property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_MultiSplitter).CreatePropertyEditors(
				m_MultiSplitter,
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
			NList<NSplitterThumb> thumbs = m_MultiSplitter.Widgets.GetChildren<NSplitterThumb>();
			for (int i = 0; i < thumbs.Count; i++)
			{
				editors = NDesigner.GetDesigner(thumbs[i]).CreatePropertyEditors(
					thumbs[i],
					NSplitterThumb.SplitModeProperty,
					NSplitterThumb.SplitOffsetProperty,
					NSplitterThumb.SplitFactorProperty,
					NSplitterThumb.CollapseModeProperty
				);

				propertyStack = new NStackPanel();
				for (int j = 0; j < editors.Count; j++)
				{
					propertyStack.Add(editors[j]);
				}

				stack.Add(new NGroupBox("Splitter Thumb " + (i + 1).ToString() + " Properties", propertyStack));
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and configure a multi splitter. The multi splitter is a widget that
	splits its content area into multiple resizable panes, which can be interactively resized with help of thumbs.
	Using the <b>Orientation</b> property you can specify whether the splitter is horizontal or vertical.
	To control how the splitter splits its content area you can use the <b>SplitMode</b> property.
</p>
";
		}

		#endregion

		#region Fields

		private NMultiSplitter m_MultiSplitter;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMultiSplitterExample.
		/// </summary>
		public static readonly NSchema NMultiSplitterExampleSchema;

		#endregion
	}
}