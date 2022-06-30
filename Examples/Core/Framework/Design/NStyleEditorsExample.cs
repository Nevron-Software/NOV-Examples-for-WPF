using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NStyleEditorsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStyleEditorsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStyleEditorsExample()
		{
			NStyleEditorsExampleSchema = NSchema.Create(typeof(NStyleEditorsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NTab tab = new NTab();
			tab.TabPages.Add(CreateTabPage("Fill Styles",
				NStyleNode.FillProperty,
				NStyleNode.ColorFillProperty,
				NStyleNode.StockGradientFillProperty,
				NStyleNode.LinearGradientFillProperty,
				NStyleNode.RadialGradientFillProperty,
				NStyleNode.AdvancedGradientFillProperty,
				NStyleNode.HatchFillProperty,
				NStyleNode.ImageFillProperty
			));

			tab.TabPages.Add(CreateTabPage("Stroke Styles", NStyleNode.StrokeProperty));
			tab.TabPages.Add(CreateTabPage("Borders", NStyleNode.BorderProperty));
			tab.TabPages.Add(CreateTabPage("Text Styles", NStyleNode.FontProperty));

			return tab;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how the designers of the style related nodes look. Select the tab page of
    the style category you are interested in and click the button next to a style node to see its designer.    
</p>
";
		}

		#endregion

		#region Implementation

		private NTabPage CreateTabPage(string title, params NProperty[] properties)
		{
			NTabPage page = new NTabPage(title);
			NStackPanel stack = new NStackPanel();
			page.Content = new NUniSizeBoxGroup(stack);
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			
			NList<NPropertyEditor> editors = Designer.CreatePropertyEditors(new NStyleNode(), properties);
			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			return page;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStyleEditorsExample.
		/// </summary>
		public static readonly NSchema NStyleEditorsExampleSchema;

		#endregion

		#region Constants

		private static readonly NDesigner Designer = NDesigner.GetDesigner(NStyleNode.NStyleNodeSchema);

		#endregion
	}
}