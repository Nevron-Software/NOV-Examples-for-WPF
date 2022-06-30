using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Import.Map;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;


namespace Nevron.Nov.Examples.Diagram
{
	public class NMapProjectionsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMapProjectionsExample()
		{
			m_Projections = new NMapProjection[] {
				new NAitoffProjection(),
				new NBonneProjection(),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.Lambert),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.Behrmann),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.TristanEdwards),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.Peters),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.Gall),
				new NCylindricalEqualAreaProjection(ENCylindricalEqualAreaProjectionType.Balthasart),

				new NEckertIVProjection(),
				new NEckertVIProjection(),
				new NEquirectangularProjection(),
				new NHammerProjection(),
				new NKavrayskiyVIIProjection(),
				new NMercatorProjection(),
				new NMillerCylindricalProjection(),
				new NMollweideProjection(),
				new NOrthographicProjection(),
				new NRobinsonProjection(),
				new NStereographicProjection(),
				new NVanDerGrintenProjection(),
				new NWagnerVIProjection(),
				new NWinkelTripelProjection(),
			};
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMapProjectionsExample()
		{
			NMapProjectionsExampleSchema = NSchema.Create(typeof(NMapProjectionsExample), NExampleBaseSchema);
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
			
			// Create the projection combo box
			m_ProjectionComboBox = new NComboBox();
			m_ProjectionComboBox.FillFromArray(m_Projections);
			m_ProjectionComboBox.SelectedIndex = DefaultProjectionIndex;
			m_ProjectionComboBox.SelectedIndexChanged += OnProjectionComboSelectedIndexChanged;

			NPairBox pairBox = NPairBox.Create("Projection:", m_ProjectionComboBox);
			stack.Add(pairBox);

			// Create the label arcs check box
			NCheckBox labelArcsCheckBox = new NCheckBox();
			labelArcsCheckBox.CheckedChanged += OnLabelArcsCheckBoxCheckedChanged;
			labelArcsCheckBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			labelArcsCheckBox.Padding = NMargins.Zero;

			pairBox = NPairBox.Create("Label arcs:", labelArcsCheckBox);
			stack.Add(pairBox);

			// Create the center parallel numeric up down
			m_CenterParalelNumericUpDown = new NNumericUpDown();
			m_CenterParalelNumericUpDown.Minimum = -90;
			m_CenterParalelNumericUpDown.Maximum = 90;
			m_CenterParalelNumericUpDown.Step = 15;
			m_CenterParalelNumericUpDown.ValueChanged += OnCenterParallelNumericUpDownValueChanged;
			stack.Add(NPairBox.Create("Central parallel:", m_CenterParalelNumericUpDown));

			// Create the center meridian numeric up down
			m_CenterMeridianNumericUpDown = new NNumericUpDown();
			m_CenterMeridianNumericUpDown.Minimum = -180;
			m_CenterMeridianNumericUpDown.Maximum = 180;
			m_CenterMeridianNumericUpDown.Step = 15;
			m_CenterMeridianNumericUpDown.ValueChanged += OnCenterMeridianNumericUpDownValueChanged;
			stack.Add(NPairBox.Create("Central meridian:", m_CenterMeridianNumericUpDown));

			NGroupBox settingsGroupBox = new NGroupBox("Settings", new NUniSizeBoxGroup(stack));
			return settingsGroupBox;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
    <b>NOV Diagram</b> makes it easy to import geographical data from ESRI shapefiles. You
    can control the way the shapes are rendered by applying various fill rules to them. You can
	also specify a map projection to be used for transforming the 3D geographical data to 2D
	screen coordinates as this example demonstrates. Using the controls on the right you can
	change the map projection and turn on or off the arc labelling. Note that some projections
	also lets you specify a central parallel and/or meridian.
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
			page.BackgroundFill = new NColorFill(NColor.LightBlue);
			page.Bounds = new NRectangle(0, 0, 10000, 10000);
			page.ZoomMode = ENZoomMode.Fit;

			// Create a map importer
			m_MapImporter = new NEsriMapImporter();
			m_MapImporter.MapBounds = NMapBounds.World;

			m_MapImporter.MeridianSettings.RenderMode = ENArcRenderMode.BelowObjects;
			m_MapImporter.ParallelSettings.RenderMode = ENArcRenderMode.BelowObjects;
			m_MapImporter.Projection = m_Projections[DefaultProjectionIndex];

			// Add an ESRI shapefile
			NEsriShapefile countries = new NEsriShapefile(Nevron.Nov.Diagram.NResources.RBIN_countries_zip);
			countries.NameColumn = "name_long";
			countries.FillRule = new NMapFillRuleValue("mapcolor8", Colors);
			m_MapImporter.AddShapefile(countries);

			// Read the map data
			m_MapImporter.Read();

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

			rule.Declarations.Add(new NValueDeclaration<NStroke>(NGeometry.StrokeProperty, new NStroke(new NColor(68, 90, 108))));
			styleSheet.Add(rule);
		}
		private void ImportMap()
		{
			NPage page = m_DrawingView.ActivePage;
			page.Items.Clear();

			// Import the map to the drawing document
			m_MapImporter.Import(m_DrawingView.Document, page.Bounds);
		}

		#endregion

		#region Event Handlers

		private void OnProjectionComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NComboBox projectionCombo = (NComboBox)arg.TargetNode;
			NMapProjection projection = (NMapProjection)projectionCombo.SelectedItem.Tag;
			m_MapImporter.Projection = projection;

			// Reimport the map applying the newly selected projection
			if (m_MapImporter.Projection is NOrthographicProjection)
			{
				GetOwnerPairBox(m_CenterParalelNumericUpDown).Visibility = ENVisibility.Visible;
				GetOwnerPairBox(m_CenterMeridianNumericUpDown).Visibility = ENVisibility.Visible;

				NOrthographicProjection ortographicProjection = (NOrthographicProjection)m_MapImporter.Projection;
				ortographicProjection.CenterPoint = new NPoint(m_CenterMeridianNumericUpDown.Value,
					m_CenterParalelNumericUpDown.Value);
			}
			else if (m_MapImporter.Projection is NBonneProjection)
			{
				GetOwnerPairBox(m_CenterParalelNumericUpDown).Visibility = ENVisibility.Visible;
				GetOwnerPairBox(m_CenterMeridianNumericUpDown).Visibility = ENVisibility.Hidden;

				((NBonneProjection)m_MapImporter.Projection).StandardParallel = m_CenterParalelNumericUpDown.Value;
			}
			else
			{
				GetOwnerPairBox(m_CenterParalelNumericUpDown).Visibility = ENVisibility.Hidden;
				GetOwnerPairBox(m_CenterMeridianNumericUpDown).Visibility = ENVisibility.Hidden;
			}

			ImportMap();
		}
		private void OnLabelArcsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool showLabels = (bool)arg.NewValue;
			m_MapImporter.ParallelSettings.ShowLabels = showLabels;
			m_MapImporter.MeridianSettings.ShowLabels = showLabels;
			ImportMap();
		}
		private void OnCenterParallelNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			double value = (double)arg.NewValue;
			if (m_MapImporter.Projection is NBonneProjection)
			{
				((NBonneProjection)m_MapImporter.Projection).StandardParallel = value;
				ImportMap();
			}
			else if (m_MapImporter.Projection is NOrthographicProjection)
			{
				NOrthographicProjection ortographicProjection = (NOrthographicProjection)m_MapImporter.Projection;
				ortographicProjection.CenterPoint = new NPoint(ortographicProjection.CenterPoint.X, value);
				ImportMap();
			}
		}
		private void OnCenterMeridianNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			double value = (double)arg.NewValue;
			if (m_MapImporter.Projection is NOrthographicProjection)
			{
				NOrthographicProjection ortographicProjection = (NOrthographicProjection)m_MapImporter.Projection;
				ortographicProjection.CenterPoint = new NPoint(value, ortographicProjection.CenterPoint.Y);
				ImportMap();
			}
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		private NMapProjection[] m_Projections;
		private NEsriMapImporter m_MapImporter;
		private NComboBox m_ProjectionComboBox;
		private NNumericUpDown m_CenterParalelNumericUpDown;
		private NNumericUpDown m_CenterMeridianNumericUpDown;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMapProjectionsExample.
		/// </summary>
		public static readonly NSchema NMapProjectionsExampleSchema;

		#endregion

		#region Static Methods

		private static NPairBox GetOwnerPairBox(NWidget widget)
		{
			return (NPairBox)widget.GetFirstAncestor(NPairBox.NPairBoxSchema);
		}

		#endregion

		#region Constants

		private const int DefaultProjectionIndex = 16;

		/// <summary>
		/// The colors used to fill the countries.
		/// </summary>
		private static readonly NColor[] Colors = new NColor[] {
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
