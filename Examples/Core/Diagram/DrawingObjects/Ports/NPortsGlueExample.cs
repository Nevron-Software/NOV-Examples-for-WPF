using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NPortsGlueExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NPortsGlueExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NPortsGlueExample()
        {
            NPortsGlueExampleSchema = NSchema.Create(typeof(NPortsGlueExample), NExampleBaseSchema);
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
    Demonstrates the ports glue. Move the rectangle close to the Connector or the Line to test the port direction that is syncrhonized with the geometry tangenta at this point.
</p>
<p> 
    In NOV Diagram each port GlueMode can be set to Inward, Outward or InwardAndOutward. 
</p>
<p>
    <b>Inward</b> ports can accept connections with Begin and End points of 1D shapes as well as other shapes Outward ports.
    Most of the ports are only Inward ports.
</p>
<p>
    <b>Outward</b> ports can be connected only to other shapes Inward ports. When a shape with outward ports
	is moved closed a shape with inward ports. The two shapes are glued in master-slave relation. The shape
	to which the outward port belongs is rotated and translated so that its outward port location matches
	the inward port location and so that the ports directions form a line.
</p>
<p>
    <b>InwardAndOutward</b> ports behave as both inward and outward.
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            activePage.Interaction.Enable1DShapeSplitting = false;
            activePage.Interaction.AutoConnectToBeginPoints = false;
            activePage.Interaction.AutoConnectToEndPoints = false;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            NBasicShapeFactory basicShapes = new NBasicShapeFactory();

            // Port Glue To Geometry Contour
            {
                // create a connector with begin, end and middle ports
                // start and end ports are offset with absolute values
                NRoutableConnector connector = new NRoutableConnector();
                activePage.Items.Add(connector);
                connector.SetBeginPoint(new NPoint(50, 50));
                connector.SetEndPoint(new NPoint(250, 250));

                NPort beginPort = new NPort();
                connector.Ports.Add(beginPort);
                beginPort.GlueToGeometryContour(0, 10, true, NAngle.Zero);

                NPort endPort = new NPort();
                connector.Ports.Add(endPort);
                endPort.GlueToGeometryContour(1, -10, true, NAngle.Zero);

                NPort middlePort = new NPort();
                connector.Ports.Add(middlePort);
                middlePort.GlueToGeometryContour(0.5, 0, true, NAngle.Zero);
            }

            // Port Glue To Shape Line
            {
                NShape lineShape = NShape.CreateLineShape();
                activePage.Items.Add(lineShape);
                lineShape.SetBeginPoint(new NPoint(350, 50));
                lineShape.SetEndPoint(new NPoint(550, 250));

                NPort middlePort = new NPort();
                lineShape.Ports.Add(middlePort);
                middlePort.GlueToGeometryContour(0.5, 0, true, NAngle.Zero);
            }

            // create a rectangle shape with 
            {
                NShape rectShape = NShape.CreateRectangle();
                rectShape.SetBounds(300, 300, 100, 100);
                rectShape.Text = "Test Port Direction with Me";

                NPort port = new NPort(0.5, 0, true);
                port.DirectionMode = ENPortDirectionMode.AutoCenter;
                port.GlueMode = ENPortGlueMode.Outward;
                rectShape.Ports.Add(port);

                activePage.Items.Add(rectShape);
            }
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NPortsGlueExample.
        /// </summary>
        public static readonly NSchema NPortsGlueExampleSchema;

        #endregion
    }
}