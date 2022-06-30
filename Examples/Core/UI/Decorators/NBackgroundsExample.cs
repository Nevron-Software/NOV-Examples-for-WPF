using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NBackgroundsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBackgroundsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBackgroundsExample()
		{
			NBackgroundsExampleSchema = NSchema.Create(typeof(NBackgroundsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a table layout panel
			NTableFlowPanel table = new NTableFlowPanel();
			table.MaxOrdinal = 3;
			table.HorizontalSpacing = 10;
			table.VerticalSpacing = 10;
			table.ColFillMode = ENStackFillMode.Equal;
			table.ColFitMode = ENStackFitMode.Equal;
			table.RowFitMode = ENStackFitMode.Equal;
			table.RowFillMode = ENStackFillMode.Equal;
			table.UniformWidths = ENUniformSize.Max;
			table.UniformHeights = ENUniformSize.Max;

			// Create widgets to demonstrate the different background types
			table.Add(CreateWidget("Solid Background", new NColorFill(NColor.PapayaWhip)));
			table.Add(CreateWidget("Hatch Background", new NHatchFill(ENHatchStyle.Cross, NColor.Red, NColor.PapayaWhip)));
			table.Add(CreateWidget("Gradient Background", new NStockGradientFill(ENGradientStyle.FromCenter, ENGradientVariant.Variant1, NColor.Red, NColor.PapayaWhip)));
			table.Add(CreateWidget("Predefined Advanced Gradient", NAdvancedGradientFill.Create(ENAdvancedGradientColorScheme.Fire1, 5)));

			NAdvancedGradientFill agFill = new NAdvancedGradientFill();
			agFill.BackgroundColor = NColor.Black;
			agFill.Points.Add(new NAdvancedGradientPoint(NColor.Green, NAngle.Zero, 0.5f, 0.5f, 0.2f, ENAdvancedGradientPointShape.Line));
			agFill.Points.Add(new NAdvancedGradientPoint(NColor.Green, new NAngle(90, NUnit.Degree), 0.5f, 0.5f, 0.2f, ENAdvancedGradientPointShape.Line));
			agFill.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.5f, 0.5f, 0.5f, ENAdvancedGradientPointShape.Circle));
			table.Add(CreateWidget("Custom Advanced Gradient", agFill));

			NImageFill imageFill = new NImageFill(NResources.Image__24x24_Shortcuts_png);
			imageFill.TextureMapping = new NAlignTextureMapping();
			table.Add(CreateWidget("Normal Image Background", imageFill));

			imageFill = new NImageFill(NResources.Image__24x24_Shortcuts_png);
			imageFill.TextureMapping = new NAlignTextureMapping();
			table.Add(CreateWidget("Fit Image Background", imageFill));

			imageFill = new NImageFill(NResources.Image__24x24_Shortcuts_png);
			imageFill.TextureMapping = new NStretchTextureMapping();
			table.Add(CreateWidget("Stretched Image Background", imageFill));

			imageFill = new NImageFill(NResources.Image__24x24_Shortcuts_png);
			imageFill.TextureMapping = new NTileTextureMapping();
			table.Add(CreateWidget("Tiled Image Background", imageFill));

			return table;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create different types of backgrounds and apply them to widgets.
</p>
";
		}

		#endregion

		#region Implementation

		private NWidget CreateWidget(string text, NFill backgroundFill)
		{
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.First;
			stack.FillMode = ENStackFillMode.First;

			NWidget widget = new NWidget();
			widget.BackgroundFill = backgroundFill;
			stack.Add(widget);

			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			stack.Add(label);

			return stack;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBackgroundsExample.
		/// </summary>
		public static readonly NSchema NBackgroundsExampleSchema;

		#endregion
	}
}