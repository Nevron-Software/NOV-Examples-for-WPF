using Nevron.Nov.Data;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Import.Map;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;


namespace Nevron.Nov.Examples.Diagram
{
	public class NWorldPopulationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NWorldPopulationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NWorldPopulationExample()
		{
			NWorldPopulationExampleSchema = NSchema.Create(typeof(NWorldPopulationExample), NExampleBaseSchema);
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
			NStackPanel stack = new NStackPanel();
			
			// Create radio buttons for the different data groupings
			stack.Add(new NRadioButton("Optimal"));
			stack.Add(new NRadioButton("Equal Interval"));
			stack.Add(new NRadioButton("Equal Distribution"));

			// Create a radio button group to hold the radio buttons
			NRadioButtonGroup group = new NRadioButtonGroup(stack);
			group.SelectedIndex = 0;
			group.SelectedIndexChanged += OnDataGroupingSelectedIndexChanged;

			// Create the data grouping group box
			NGroupBox dataGroupingGroupBox = new NGroupBox("Data Grouping", group);

			return dataGroupingGroupBox;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example shows a world population map by countries (darker countries have larger population).
    A fill rule is used to automatically color the countries on the map based on their population attribute.
</p>
<p>
	Upon import of a shape additional information from the DBF file that accompanies the shapefile
	is provided (e.g. Country Name, Population, Currency, GDP, etc.). You can use these values to
	customze the name, the text and the fill of the shape. You can also provide an INShapeCreatedListener
	implementation to the shape importer of the map in order to get notified when a shape is imported
	and use the values from the DBF file for this shape to customize it even further. This example
	demonstrates how to create and use such interface implementation to add tooltips for the shapes.
</p>
";
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
			page.ZoomMode = ENZoomMode.Fit;
			page.BackgroundFill = new NColorFill(NColor.LightBlue);

			// Create a map importer
			m_MapImporter = new NEsriMapImporter();
			m_MapImporter.MapBounds = NMapBounds.World;

			// Add a shapefile
			NEsriShapefile countries = new NEsriShapefile(Nevron.Nov.Diagram.NResources.RBIN_countries_zip);
			countries.NameColumn = "name_long";
			countries.TextColumn = "name_long";
			countries.MinTextZoomPercent = 50;
			countries.FillRule = new NMapFillRuleRange("pop_est", NColor.White, new NColor(0, 80, 0), 12);
			m_MapImporter.AddShapefile(countries);

			// Set the shape created listener
			m_MapImporter.ShapeCreatedListener = new NCustomShapeCreatedListener();

			// Read the map data
			m_MapImporter.Read();

			// Import the map to the drawing document
			ImportMap();
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

			rule.Declarations.Add(new NValueDeclaration<NStroke>(NGeometry.StrokeProperty, new NStroke(new NColor(80, 80, 80))));
			styleSheet.Add(rule);
		}
		private void ImportMap()
		{
			NPage page = m_DrawingView.ActivePage;
			page.Items.Clear();
			page.Bounds = new NRectangle(0, 0, 10000, 10000);

			// Import the map to the drawing document
			m_MapImporter.Import(m_DrawingView.Document, page.Bounds);

			// Size page to content
			page.SizeToContent();
		}

		#endregion

		#region Event Handlers

		private void OnDataGroupingSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NMapFillRuleRange fillRule = (NMapFillRuleRange)m_MapImporter.GetShapefileAt(0).FillRule;

			switch ((int)arg.NewValue)
			{
				case 0:
					fillRule.DataGrouping = new NDataGroupingOptimal();
					break;
				case 1:
					fillRule.DataGrouping = new NDataGroupingEqualInterval();
					break;
				case 2:
					fillRule.DataGrouping = new NDataGroupingEqualDistribution();
					break;
			}

			ImportMap();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
		private NEsriMapImporter m_MapImporter;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NWorldPopulationExample.
		/// </summary>
		public static readonly NSchema NWorldPopulationExampleSchema;

		#endregion

		#region Nested Types

		private class NCustomShapeCreatedListener : NShapeCreatedListener
		{
			public override bool OnMultiPolygonCreated(NShape shape, NGisFeature feature)
			{
				return OnPolygonCreated(shape, feature);
			}
			public override bool OnPolygonCreated(NShape shape, NGisFeature feature)
			{
				NDataTableRow row = feature.Attributes;
				string countryName = (string)row["name_long"];
				decimal population = (decimal)row["pop_est"];

				string tooltip = countryName + "'s population: " + population.ToString("N0");		
				shape.Tooltip = new NTooltip(tooltip);

				return true;
			}
		}

		#endregion
	}
}
