using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NPairBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPairBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPairBoxExample()
		{
			NPairBoxExampleSchema = NSchema.Create(typeof(NPairBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_PairBox = new NPairBox(CreateBoxContent("Box 1", NColor.Blue), CreateBoxContent("Box 2", NColor.Red));
			m_PairBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_PairBox.VerticalPlacement = ENVerticalPlacement.Top;

			// The Spacing property is automatically set from the UI theme to NDesign.HorizontalSpacing,
			// so you don't need to set it. It is set here only for the purposes of the example.
			m_PairBox.Spacing = NDesign.HorizontalSpacing;

			return m_PairBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_PairBox).CreatePropertyEditors(m_PairBox,
				NPairBox.EnabledProperty,
				NPairBox.HorizontalPlacementProperty,
				NPairBox.VerticalPlacementProperty,
				NPairBox.BoxesRelationProperty,
				NPairBox.FitModeProperty,
				NPairBox.FillModeProperty,
				NPairBox.SpacingProperty
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a pair box. The pair box is a widget, which consists of 2 other widgets - <b>Box1</b> and <b>Box2</b>.
	You can change the relative alignment, the spacing and the size mode of this widgets using the controls to the right.
</p>
";
		}

		#endregion

		#region Implementation

		private NWidget CreateBoxContent(string text, NColor borderColor)
		{
			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;

			NContentHolder contentElement = new NContentHolder(label);
			contentElement.Border = NBorder.CreateFilledBorder(borderColor);
			contentElement.BorderThickness = new NMargins(1);
			contentElement.Padding = new NMargins(2);

			return contentElement;
		}

		#endregion

		#region Fields

		private NPairBox m_PairBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPairBoxExample.
		/// </summary>
		public static readonly NSchema NPairBoxExampleSchema;

		#endregion
	}
}