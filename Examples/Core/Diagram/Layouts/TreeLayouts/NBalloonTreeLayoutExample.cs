using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NBalloonTreeLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NBalloonTreeLayoutExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NBalloonTreeLayoutExample()
        {
            NBalloonTreeLayoutExampleSchema = NSchema.Create(typeof(NBalloonTreeLayoutExample), NExampleBaseSchema);
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
            m_Layout.Changed += OnLayoutChanged;

            NStackPanel stack = new NStackPanel();

            // property editor
            NEditor editor = NDesigner.GetDesigner(m_Layout).CreateInstanceEditor(m_Layout);
            stack.Add(new NGroupBox("Properties", editor));

            NButton arrangeButton = new NButton("Arrange Diagram");
            arrangeButton.Click += OnArrangeButtonClick;
            stack.Add(arrangeButton);

            // items stack
            NStackPanel itemsStack = new NStackPanel();

            // NOTE: For Tree layouts we provide the user with the ability to generate random tree diagrams so that he/she can test the layouts
            NButton randomTree1Button = new NButton("Random Tree (max 6 levels, max 3 branch nodes)");
            randomTree1Button.Click += OnRandomTree1ButtonClick;
            itemsStack.Add(randomTree1Button);

            NButton randomTree2Button = new NButton("Random Tree (max 8 levels, max 2 branch nodes)");
            randomTree2Button.Click += OnRandomTree2ButtonClick;
            itemsStack.Add(randomTree2Button);

            stack.Add(new NGroupBox("Items", itemsStack));

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    The balloon tree layout tries to compact the drawing area of the tree 
    by placing the vertices in balloons around the tree root.
    It produces straight line tree drawings. 
</p>
<p>        
    Following is a brief description of its properties:
</p>
<ul>
	<li>
		<b>ParentChildSpacing</b> - the preferred spacing between a parent and a child
		vertex in the layout direction. The real spacing may be different for some nodes,
		because the layout does not allow overlapping.
	</li>
	<li>
		<b>VertexSpacing</b> - the minimal spacing between 2 nodes in the layout.
		If set to 0, the nodes may touch each other.
	</li>
	<li>
		<b>ChildWedge</b> - the sector angle (measured in degrees) for the children
		of each vertex.
	</li>
	<li>
		<b>RootWedge</b> - the sector angle (measured in degrees) for the children
		of the root vertex.
	</li>
	<li>
		<b>StartAngle</b> - the start angle for the children of the root vertex, measured in
		degrees anticlockwise from the x-axis.
	</li>
</ul>
<p>
	To experiment with the layout just change its properties from the property grid and click the <b>Layout</b> button.
</p>            
            ";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;

            // Create a template graph
            NGenericTreeTemplate tree = new NGenericTreeTemplate();
            tree.EdgesUserClass = "Connector";
            tree.Levels = 4;
            tree.BranchNodes = 4;
            tree.HorizontalSpacing = 10;
            tree.VerticalSpacing = 10;
            tree.ConnectorType = ENConnectorShape.RoutableConnector;
            tree.VerticesShape = VertexShape;
            tree.VerticesSize = VertexSize;
            tree.Create(drawingDocument);

            // Arrange diagram
            ArrangeDiagram(drawingDocument);

            // Fit active page
            drawingDocument.Content.ActivePage.ZoomMode = ENZoomMode.Fit;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Arranges the shapes in the active page.
        /// </summary>
        /// <param name="drawingDocument"></param>
        private void ArrangeDiagram(NDrawingDocument drawingDocument)
        {
            // get all top-level shapes that reside in the active page
            NPage activePage = drawingDocument.Content.ActivePage;
            NList<NShape> shapes = activePage.GetShapes(false);

            // create a layout context and use it to arrange the shapes using the current layout
            NDrawingLayoutContext layoutContext = new NDrawingLayoutContext(drawingDocument, activePage);
            m_Layout.Arrange(shapes.CastAll<object>(), layoutContext);

            // size the page to the content size
            activePage.SizeToContent();
        }

        #endregion

        #region Event Handlers

        private void OnRandomTree1ButtonClick(NEventArgs arg)
        {
            NDrawingDocument drawingDocument = m_DrawingView.Document;

            drawingDocument.StartHistoryTransaction("Create Random Tree 1");
            try
            {
                drawingDocument.Content.ActivePage.Items.Clear();

                // create a random tree
                NGenericTreeTemplate tree = new NGenericTreeTemplate();
                tree.EdgesUserClass = "Connector";
                tree.Levels = 6;
                tree.BranchNodes = 3;
                tree.HorizontalSpacing = 10;
                tree.VerticalSpacing = 10;
                tree.VerticesShape = VertexShape;
                tree.VerticesSize = VertexSize;
                tree.Balanced = true;
                tree.VertexSizeDeviation = 0;

                tree.Create(drawingDocument);

                // layout the tree
                ArrangeDiagram(drawingDocument);
            }
            finally
            {
                drawingDocument.CommitHistoryTransaction();
            }
        }
        private void OnRandomTree2ButtonClick(NEventArgs arg)
        {
            NDrawingDocument drawingDocument = m_DrawingView.Document;

            drawingDocument.StartHistoryTransaction("Create Random Tree 2");
            try
            {
                drawingDocument.Content.ActivePage.Items.Clear();

                // create a random tree
                NGenericTreeTemplate tree = new NGenericTreeTemplate();
                tree.EdgesUserClass = "Connector";
                tree.Levels = 8;
                tree.BranchNodes = 2;
                tree.HorizontalSpacing = 10;
                tree.VerticalSpacing = 10;
                tree.VerticesShape = VertexShape;
                tree.VerticesSize = VertexSize;
                tree.Balanced = true;
                tree.VertexSizeDeviation = 0;

                tree.Create(drawingDocument);

                // layout the tree
                ArrangeDiagram(drawingDocument);
            }
            finally
            {
                drawingDocument.CommitHistoryTransaction();
            }
        }
        private void OnLayoutChanged(NEventArgs arg)
        {
            ArrangeDiagram(m_DrawingView.Document);
        }
        private void OnArrangeButtonClick(NEventArgs arg)
        {
            ArrangeDiagram(m_DrawingView.Document);
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;
        private NBalloonTreeLayout m_Layout = new NBalloonTreeLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NBalloonTreeLayoutExample.
        /// </summary>
        public static readonly NSchema NBalloonTreeLayoutExampleSchema;

        #endregion

        #region Constants

        private const ENBasicShape VertexShape = ENBasicShape.Circle;
        private static readonly NSize VertexSize = new NSize(60, 60);

        #endregion
    }
}