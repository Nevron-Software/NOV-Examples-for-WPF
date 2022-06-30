using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Import.Map;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;


namespace Nevron.Nov.Examples.Diagram
{
	public class NWorldMapExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NWorldMapExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NWorldMapExample()
		{
			NWorldMapExampleSchema = NSchema.Create(typeof(NWorldMapExample), NExampleBaseSchema);
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
    <b>NOV Diagram</b> makes it easy to import geographical data from ESRI shapefiles. You
    can control the way the shapes are rendered by applying various fill rules to them. You can
	also specify a zoom range in which the shapes and/or texts of a shapefile should be visible.
	For example when you zoom this map to 50% you will notice that labels appear for the countries.
</p>
<p>
	Upon import of a shape additional information from the DBF file that accompanies the shapefile
	is provided (e.g. Country Name, Population, Currency, GDP, etc.). You can use these values to
	customize the name, the text and the fill of the shape. You can also provide an INShapeCreatedListener
	implementation to the shape importer of the map in order to get notified when a shape is imported
	and use the values from the DBF file for this shape to customize it even further.
</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			// Configure the document
			NDrawing drawing = drawingDocument.Content;
			drawing.ScreenVisibility.ShowGrid = false;

			// Add styles
			AddStyles(drawingDocument);

			// Configure the active page
			NPage page = drawing.ActivePage;
			page.Bounds = new NRectangle(0, 0, 10000, 10000);
			page.ZoomMode = ENZoomMode.Fit;
			page.BackgroundFill = new NColorFill(NColor.LightBlue);

			// Create a map importer
			NEsriMapImporter mapImporter = new NEsriMapImporter();
			mapImporter.MapBounds = NMapBounds.World;

			// Add an ESRI shapefile
			NEsriShapefile countries = new NEsriShapefile(Nevron.Nov.Diagram.NResources.RBIN_countries_zip);
			countries.NameColumn = "name_long";
			countries.TextColumn = "name_long";
			countries.MinTextZoomPercent = 50;
			countries.FillRule = new NMapFillRuleValue("mapcolor8", Colors);
			mapImporter.AddShapefile(countries);

			// Read the map data
			mapImporter.Read();

			// Import the map to the drawing document
			mapImporter.Import(drawingDocument, page.Bounds);

			// Size page to content
			page.SizeToContent();
		}

		#endregion

		#region Implementation

		private void AddStyles(NDrawingDocument document)
		{
			// Create a style sheet
			NStyleSheet styleSheet = new NStyleSheet();
			document.StyleSheets.Add(styleSheet);

			// Add some styling for the shapes
			NRule rule = new NRule();
			NSelectorBuilder sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NGeometry.NGeometrySchema);
			sb.ChildOf();
			sb.Type(NShape.NShapeSchema);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NStroke>(NGeometry.StrokeProperty, new NStroke(new NColor(68, 90, 108))));
			styleSheet.Add(rule);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NWorldMapExample.
		/// </summary>
		public static readonly NSchema NWorldMapExampleSchema;

		#endregion

		#region Constants

		/// <summary>
		/// The colors used to fill the countries.
		/// </summary>
		private static readonly NColor[] Colors = new NColor[]{
            NColor.OldLace,
            NColor.PaleGreen,
            NColor.Gold,
            NColor.Khaki,
            NColor.Tan,
            NColor.Orange,
            NColor.Salmon,
            NColor.PaleGoldenrod
        };

		#endregion
	}
}
