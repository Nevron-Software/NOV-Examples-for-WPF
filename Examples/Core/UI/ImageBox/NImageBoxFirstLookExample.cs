using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NImageBoxFirstLookExample : NExampleBase
	{		
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NImageBoxFirstLookExample()
		{
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NImageBoxFirstLookExample()
		{
			NImageBoxFirstLookExampleSchema = NSchema.Create(typeof(NImageBoxFirstLookExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create an image box
			m_ImageBox = new NImageBox(NResources.Image_SampleImage_png);
			m_ImageBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ImageBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_ImageBox.SetBorder(1, NColor.Red);

			return m_ImageBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ImageBox).CreatePropertyEditors(m_ImageBox,
				NImageBox.HorizontalPlacementProperty,
				NImageBox.VerticalPlacementProperty,
				NImageBox.BackgroundFillProperty,
				NImageBox.ImageMappingProperty,
				NImageBox.ImageRenderModeProperty,
				NImageBox.ImageProperty
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
	This example demonstrates the features of the Nevron image box widget. Use the controls on the right to load an image
	and change the image box settings.
</p>
";
		}

		#endregion

		#region Fields

		private NImageBox m_ImageBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NImageBoxFirstLookExample
		/// </summary>
		public static readonly NSchema NImageBoxFirstLookExampleSchema;

		#endregion
	}
}
