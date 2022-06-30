using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NLibrariesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLibrariesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLibrariesExample()
		{
			NLibrariesExampleSchema = NSchema.Create(typeof(NLibrariesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a drawing view
			NDrawingView drawingView = new NDrawingView();

			// Create a library view
			m_LibraryView = new NLibraryView();
			m_LibraryView.Document.HistoryService.Pause();

			try
			{
				InitLibrary(m_LibraryView.Document);
			}
			finally
			{
				m_LibraryView.Document.HistoryService.Resume();
			}

			// Associate the drawing view with the library view to make
			// the drawing theme update the appearance of shapes in the library
			m_LibraryView.DrawingView = drawingView;

			// Place the library view and the drawing view in a splitter
			NSplitter splitter = new NSplitter(m_LibraryView, drawingView, ENSplitterSplitMode.OffsetFromNearSide, 275);

			// Create a diagram ribbon
			NDiagramRibbonBuilder builder = new NDiagramRibbonBuilder();
			return builder.CreateUI(splitter, drawingView);
		}
		protected override NWidget CreateExampleControls()
		{
			NButton openLibraryButton = NButton.CreateImageAndText(Nov.Diagram.NResources.Image_Library_LibraryOpen_png,  "Open Library...");
			openLibraryButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			openLibraryButton.VerticalPlacement = ENVerticalPlacement.Top;
			openLibraryButton.Click += OnOpenLibraryButtonClick;

			return openLibraryButton;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example shows how to create various NOV Diagram library items and place them in a library.</p>
<p>If you want to open a NOV Diagram library file or a Visio Stencil, use the <b>Open Library</b> button on the right.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnOpenLibraryButtonClick(NEventArgs arg)
		{
			m_LibraryView.OpenFile();
		}

		#endregion

		#region Implementation

		private void InitLibrary(NLibraryDocument libraryDocument)
		{
			NRectangle shapeBounds = new NRectangle(0, 0, 150, 100);
			NLibrary library = libraryDocument.Content;

			// 1. Rectangle shape library item
			NShape rectangleShape = new NShape();
			rectangleShape.SetBounds(shapeBounds);
			rectangleShape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
			library.Items.Add(new NLibraryItem(rectangleShape, "Rectangle Shape", "This is a rectangle shape"));

			// 2. Image shape library item
			NShape imageShape = new NShape();
			imageShape.SetBounds(0, 0, 128, 128);
			imageShape.ImageBlock.Image = NResources.Image__256x256_FemaleIcon_jpg;
			library.Items.Add(new NLibraryItem(imageShape, "Image Shape", "This is a shape with an image"));

			// 3. Shape filled with a hatch
			NShape hatchShape = new NShape();
			hatchShape.SetBounds(shapeBounds);
			hatchShape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));
			hatchShape.Geometry.Fill = new NHatchFill(ENHatchStyle.DiagonalCross, NColor.Black, NColor.White);
			hatchShape.Geometry.Stroke = new NStroke(NColor.Black);
			library.Items.Add(new NLibraryItem(hatchShape, "Hatch Shape", "Shape filled with a theme independent hatch"));

			// 4. Shape filled with a theme based hatch
			NShape themeShape = new NShape();
			themeShape.SetBounds(shapeBounds);
			themeShape.Geometry.AddRelative(new NDrawRectangle(0, 0, 1, 1));

			// Make color1 a theme variant color
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;
			NColor color1 = theme.ColorPalette.Variants[0][0];
			color1.Tag = new NThemeVariantColorInfo(0);

			// Make color2 a theme palette color
			NColor color2 = theme.ColorPalette.Light1;
			color2.Tag = new NThemePaletteColorInfo(ENThemeColorName.Light1, 0);

			themeShape.Geometry.Fill = new NHatchFill(ENHatchStyle.DiagonalCross, color1, color2);
			library.Items.Add(new NLibraryItem(themeShape, "Theme Based Shape", "Shape filled with a theme dependent hatch"));
		}

		#endregion

		#region Fields

		private NLibraryView m_LibraryView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLibrariesExample.
		/// </summary>
		public static readonly NSchema NLibrariesExampleSchema;

		#endregion
	}
}