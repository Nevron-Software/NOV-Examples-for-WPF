using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NGeometryArrowheadsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGeometryArrowheadsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGeometryArrowheadsExample()
        {
            NGeometryArrowheadsExampleSchema = NSchema.Create(typeof(NGeometryArrowheadsExample), NExampleBaseSchema);
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
            return @"<p>Demonstrates the arrowhead styles included in NOV Diagram.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;
			activePage.Layout.ContentPadding = new NMargins(20);

            // switch selected edit mode to geometry
            // this instructs the diagram to show geometry handles for the selected shapes.
            drawing.ScreenVisibility.ShowGrid = false;

            NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();
            ENArrowheadShape[] arrowheadShapes = NEnum.GetValues<ENArrowheadShape>();

			double x = 20;
			double y = 0;

            for (int i = 1; i < arrowheadShapes.Length; i++)
            {
                ENArrowheadShape arrowheadShape = arrowheadShapes[i];
                NShape shape = connectorShapes.CreateShape(ENConnectorShape.Line);
                drawing.ActivePage.Items.Add(shape);

                // create geometry arrowheads
                shape.Geometry.BeginArrowhead = new NArrowhead(arrowheadShape);
                shape.Geometry.EndArrowhead = new NArrowhead(arrowheadShape);

                shape.Text = NEnum.GetLocalizedString(arrowheadShape);

                shape.SetBeginPoint(new NPoint(x, y));
                shape.SetEndPoint(new NPoint(x + 350, y));

				y += 30;

				if (i == arrowheadShapes.Length / 2)
				{
					// Begin a second column of shapes
					x += 400;
					y = 0;
				}
            }

			activePage.SizeToContent();
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NGeometryArrowheadsExample.
        /// </summary>
        public static readonly NSchema NGeometryArrowheadsExampleSchema;

        #endregion
    }
}