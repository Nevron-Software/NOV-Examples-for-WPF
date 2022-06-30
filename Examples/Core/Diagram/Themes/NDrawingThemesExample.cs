using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDrawingThemesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDrawingThemesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDrawingThemesExample()
		{
			NDrawingThemesExampleSchema = NSchema.Create(typeof(NDrawingThemesExample), NExampleBaseSchema);
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
	This example shows how the different shape styles of a drawing theme look. Select a new page theme and
	page theme variant from the <b>Design</b> tab of the ribbon to see another theme.
</p>";
        }

		#endregion

		#region Implementation

		private void InitDiagram(NDrawingDocument drawingDocument)
        {
			const double ShapeWidth = 70;
			const double ShapeHeight = 50;
			const double Spacing = 20;
			const ENBasicShape ShapeType = ENBasicShape.Rectangle;

            NDrawing drawing = drawingDocument.Content;
            NPage page = drawing.ActivePage;
			NBasicShapeFactory factory = new NBasicShapeFactory();

			// Hide ports
			drawing.ScreenVisibility.ShowPorts = false;

			// Create the variant styled shapes
			double x = 0;
			double y = 0;

			for (int i = 0; i < 4; i++)
			{
				NShape shape = factory.CreateShape(ShapeType);
				shape.SetBounds(x, y, ShapeWidth, ShapeHeight);
				shape.Text = "Text";

				// Set the shape style
				int styleIndex = 100 + i;
				int colorIndex = 100 + i;
				shape.Style = new NShapeStyle(styleIndex, colorIndex);

				// Add the shape to the page
				page.Items.Add(shape);

				x += ShapeWidth + Spacing;
			}

			for (int i = 0; i < 6; i++)
			{
				int styleIndex = i;
				y += ShapeHeight + Spacing;
				x = 0;

				for (int j = 0; j < 7; j++)
				{
					NShape shape = factory.CreateShape(ShapeType);
					shape.SetBounds(x, y, ShapeWidth, ShapeHeight);
					shape.Text = "Text";

					// Set the shape style
					int colorIndex = 200 + j;
					shape.Style = new NShapeStyle(styleIndex, colorIndex);

					// Add the page to the shape
					page.Items.Add(shape);

					x += ShapeWidth + Spacing;
				}
			}

			// Connect 2 shapes with a connector
			NShape shape1 = (NShape)page.Items[0];
			NShape shape2 = (NShape)page.Items[11];

			NRoutableConnector connector = new NRoutableConnector();
			connector.Text = "Text";
			page.Items.Add(connector);
			connector.UserClass = NDR.StyleSheetNameConnectors;
			connector.RerouteMode = ENRoutableConnectorRerouteMode.Always;
			connector.GlueBeginToShape(shape1);
			connector.GlueEndToShape(shape2);

			shape1.AllowMoveX = false;
			shape1.AllowMoveY = false;

			page.SizeToContent();
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NDrawingThemesExample.
        /// </summary>
        public static readonly NSchema NDrawingThemesExampleSchema;

		#endregion
	}
}