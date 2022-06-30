using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NThemeBasedShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NThemeBasedShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NThemeBasedShapesExample()
		{
			NThemeBasedShapesExampleSchema = NSchema.Create(typeof(NThemeBasedShapesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

			m_DrawingView.Document.HistoryService.Pause();
			try
			{
				InitDiagram(m_DrawingView.Document);
			}
			finally
			{
				m_DrawingView.Document.HistoryService.Resume();
			}

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create shapes whose appearance depends on
	the currently selected page theme and page theme variant. To do that, set
	the <b>Tag</b> property of the shapes to a theme based color info.
</p>
<p>
	Select a new page theme or page theme variant from the <b>Design</b> tab of
	the ribbon to see how the style of the shape will change.
</p>";
		}

		#endregion

		#region Implementation

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			// Create a rectangle shape
			NShape shape = new NShape();
			shape.Text = "Shape";
			shape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
			shape.SetBounds(100, 100, 200, 150);

			// Make color1 a theme variant color
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;
			NColor color1 = theme.ColorPalette.Variants[0][0];
			color1.Tag = new NThemeVariantColorInfo(0);

			// Make color2 a theme palette color
			NColor color2 = theme.ColorPalette.Light1;
			color2.Tag = new NThemePaletteColorInfo(ENThemeColorName.Light1, 0);

			// Set the fill of the geometry to a hatch that depends on the theme
			shape.Geometry.Fill = new NHatchFill(ENHatchStyle.DiagonalCross, color1, color2);

			// Add the theme based shape to the active page of the drawing
			NPage activePage = drawingDocument.Content.ActivePage;
			activePage.Items.Add(shape);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NThemeBasedShapesExample.
		/// </summary>
		public static readonly NSchema NThemeBasedShapesExampleSchema;

		#endregion
	}
}
