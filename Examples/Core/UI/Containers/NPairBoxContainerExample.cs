using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NUniSizeBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NUniSizeBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NUniSizeBoxExample()
		{
			NUniSizeBoxExampleSchema = NSchema.Create(typeof(NUniSizeBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;

			m_PairBoxes = new NPairBox[Texts.Length / 2];
			for (int i = 0; i < m_PairBoxes.Length; i++)
			{
				NPairBox pairBox = new NPairBox(new NLabel(Texts[i * 2]), new NLabel(Texts[i * 2 + 1]), true);
				
				pairBox.Box1.Border = NBorder.CreateFilledBorder(NColor.Blue);
				pairBox.Box1.BorderThickness = new NMargins(1);
				pairBox.Box2.Border = NBorder.CreateFilledBorder(NColor.Red);
				pairBox.Box2.BorderThickness = new NMargins(1);

				m_PairBoxes[i] = pairBox;
				stack.Add(pairBox);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			for (int i = 0, count = m_PairBoxes.Length; i < count; i++)
			{
				// Create the pair box property editors
				NPairBox pairBox = m_PairBoxes[i];
				NList<NPropertyEditor> editors = NDesigner.GetDesigner(pairBox).CreatePropertyEditors(pairBox,
					NPairBox.FillModeProperty,
					NPairBox.FitModeProperty
				);

				NUniSizeBox box1 = (NUniSizeBox)pairBox.Box1;
				editors.Add(NDesigner.GetDesigner(box1).CreatePropertyEditor(
					box1,
					NUniSizeBox.UniSizeModeProperty
				));

				NUniSizeBox box2 = (NUniSizeBox)pairBox.Box2;
				editors.Add(NDesigner.GetDesigner(box2).CreatePropertyEditor(
					box2,
					NUniSizeBox.UniSizeModeProperty
				));

				// Create the properties stack panel
				NStackPanel propertyStack = new NStackPanel();
				for (int j = 0, editorCount = editors.Count; j < editorCount; j++)
				{
					propertyStack.Add(editors[j]);
				}

				// Add the box 1 preferred height editor
				NPropertyEditor editor = NDesigner.GetDesigner(box1.Content).CreatePropertyEditor(box1.Content, NWidget.PreferredHeightProperty);
				NLabel label = editor.GetFirstDescendant<NLabel>();
				label.Text = "Box 1 Preferred Height:";
				propertyStack.Add(editor);

				// Add the box 2 preferred height editor
				editor = NDesigner.GetDesigner(box2.Content).CreatePropertyEditor(box2.Content, NWidget.PreferredHeightProperty);
				label = editor.GetFirstDescendant<NLabel>();
				label.Text = "Box 2 Preferred Height:";
				propertyStack.Add(editor);

				// Create a group box for the properties
				NGroupBox groupBox = new NGroupBox("Pair Box " + (i + 1).ToString());
				groupBox.Content = propertyStack;
				stack.Add(groupBox);
			}

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create several pair boxes and place them in an alignable element container.
	Alignable element containers let the user specify how to size the alignable elements in the container.
</p>
";
		}

		#endregion

		#region Fields

		private NPairBox[] m_PairBoxes;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NUniSizeBoxExample.
		/// </summary>
		public static readonly NSchema NUniSizeBoxExampleSchema;

		#endregion

		#region Constants

		private static readonly string[] Texts = new string[] {
			"This is box 1.1", "I'm box 1.2 and I'm wider",
			"Box 2.1", "Box 2.2",
			"I am box 3.1 and I am the widest one", "The last box - 3.2"
		};

		#endregion
	}
}