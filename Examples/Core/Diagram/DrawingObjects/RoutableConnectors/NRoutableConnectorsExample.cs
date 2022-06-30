using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// Summary description for NRoutableConnectors.
	/// </summary>
	public class NRoutableConnectorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NRoutableConnectorsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NRoutableConnectorsExample()
        {
            NRoutableConnectorsExampleSchema = NSchema.Create(typeof(NRoutableConnectorsExample), NExampleBaseSchema);
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
	This example demonstrates routable connectors and routing.
</p>
<p>
	Routing is the process of finding a path between two points, which strives not 
	to cross any obstacles and also tries to obey certain aesthetic criteria (such 
	as minimal number of turns, port orientation etc.).
</p>
<p>
    Routing works with three corner stone objects: routable connector, obstacle shapes and router. 
    A routable connector tries to avoid the current set of obstacle shapes (residing in the page) by obtaining routing points from the router. 
    The router is responsible for creating and maintaining a routing graph for the current set of obstacle shapes existing in the page.    
</p>
<p>
	A routable connector can be automatically rerouted in three modes:
	<ul>
		<li>
			<b>Never</b> - the connector is never automatically rerouted. You can still reroute the 
			route by executing the Reroute command (from the context menu or from code).
		</li>
		<li>
			<b>Always</b> - the connector is automatically rerouted when any of the obstacles have 
			changed (i.e. there is a possibility for the route to be rerouted in a better way).
		</li>
		<li>
			<b>When Needed</b> - the connector is automatically rerouted when an obstacle is placed on it 
			(i.e. the route needs to be rerouted cause it crosses an obstacle).
		</li>
	</ul>
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // hide grid and ports
            drawing.ScreenVisibility.ShowGrid = false;
            drawing.ScreenVisibility.ShowPorts = false;

            // create a stylesheet for styling the different bricks
            NStyleSheet styleSheet = new NStyleSheet();
            drawingDocument.StyleSheets.AddChild(styleSheet);

            // the first rule fills brichs with UserClass BRICK1
            NRule ruleBrick1 = new NRule();
            styleSheet.Add(ruleBrick1);

            NSelectorBuilder sb = ruleBrick1.GetSelectorBuilder();
            sb.Start();
            sb.Type(NGeometry.NGeometrySchema);
            sb.ChildOf();
            sb.UserClass("BRICK1");
            sb.End();

            ruleBrick1.Declarations.Add(new NValueDeclaration<NFill>(NGeometry.FillProperty, new NHatchFill(ENHatchStyle.HorizontalBrick, NColor.DarkOrange, NColor.Gold)));

            // the second rule fills brichs with UserClass BRICK2
            NRule ruleBrick2 = new NRule();
            styleSheet.Add(ruleBrick2);

            sb = ruleBrick2.GetSelectorBuilder();
            sb.Start();
            sb.Type(NGeometry.NGeometrySchema);
            sb.ChildOf();
            sb.UserClass("BRICK2");
            sb.End();

            ruleBrick2.Declarations.Add(new NValueDeclaration<NFill>(NGeometry.FillProperty, new NHatchFill(ENHatchStyle.HorizontalBrick, NColor.DarkRed, NColor.Gold)));

            // create all shapes
            // create the maze frame
            CreateBrick(new NRectangle(50, 0, 700, 50), "BRICK1");
            CreateBrick(new NRectangle(750, 0, 50, 800), "BRICK1");
            CreateBrick(new NRectangle(50, 750, 700, 50), "BRICK1");
            CreateBrick(new NRectangle(0, 0, 50, 800), "BRICK1");

            // create the maze obstacles
            CreateBrick(new NRectangle(100, 200, 200, 50), "BRICK2");
            CreateBrick(new NRectangle(300, 50, 50, 200), "BRICK2");
            CreateBrick(new NRectangle(450, 50, 50, 200), "BRICK2");
            CreateBrick(new NRectangle(500, 200, 200, 50), "BRICK2");
            CreateBrick(new NRectangle(50, 300, 250, 50), "BRICK2");
            CreateBrick(new NRectangle(500, 300, 250, 50), "BRICK2");
            CreateBrick(new NRectangle(350, 350, 100, 100), "BRICK2");
            CreateBrick(new NRectangle(50, 450, 250, 50), "BRICK2");
            CreateBrick(new NRectangle(500, 450, 250, 50), "BRICK2");
            CreateBrick(new NRectangle(100, 550, 200, 50), "BRICK2");
            CreateBrick(new NRectangle(300, 550, 50, 200), "BRICK2");
            CreateBrick(new NRectangle(450, 550, 50, 200), "BRICK2");
            CreateBrick(new NRectangle(500, 550, 200, 50), "BRICK2");

            // create the first set of start/end shapes
            NShape start = CreateEllipse(new NRectangle(100, 100, 50, 50), "START");
            NShape end = CreateEllipse(new NRectangle(650, 650, 50, 50), "END");

            // connect them with a dynamic HV routable connector, 
            // which is rerouted whenever the obstacles have changed
            NRoutableConnector routableConnector = new NRoutableConnector();
            routableConnector.RerouteMode = ENRoutableConnectorRerouteMode.Always;
            routableConnector.Geometry.Stroke = new NStroke(3, NColor.Black);
            activePage.Items.Add(routableConnector);

            // connect the start and end shapes
            routableConnector.GlueBeginToShape(start);
            routableConnector.GlueEndToShape(end);

            // reroute the connector
            routableConnector.RequestReroute();

            // size document to fit the maze
            activePage.SizeToContent();
        }

        #endregion
        
        #region Implementation

        /// <summary>
        /// Creates a brick shape (Rectangle) and applies the specified class to ut 
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="userClass"></param>
        /// <returns></returns>
        NShape CreateBrick(NRectangle bounds, string userClass)
        {
            NBasicShapeFactory factory = new NBasicShapeFactory();
            NShape shape = factory.CreateShape(ENBasicShape.Rectangle);
            shape.SetBounds(bounds);
            shape.UserClass = userClass;
            m_DrawingView.ActivePage.Items.Add(shape);
            return shape;
        }
        /// <summary>
        /// Creates an ellipse shapeand applies the specified class to ut (used for Start and End shapes)
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="userClass"></param>
        /// <returns></returns>
        NShape CreateEllipse(NRectangle bounds, string userClass)
        {
            NBasicShapeFactory factory = new NBasicShapeFactory();
            NShape shape = factory.CreateShape(ENBasicShape.Ellipse);
            shape.SetBounds(bounds);
            shape.UserClass = userClass;
            m_DrawingView.ActivePage.Items.Add(shape);
            return shape;
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NRoutableConnectorsExample.
        /// </summary>
        public static readonly NSchema NRoutableConnectorsExampleSchema;

        #endregion
    }
}