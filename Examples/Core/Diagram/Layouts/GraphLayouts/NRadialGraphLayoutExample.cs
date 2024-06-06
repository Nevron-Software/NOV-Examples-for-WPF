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
    public class NRadialGraphLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NRadialGraphLayoutExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NRadialGraphLayoutExample()
        {
            NRadialGraphLayoutExampleSchema = NSchema.Create(typeof(NRadialGraphLayoutExample), NExampleBaseSchema);
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

			// NOTE: For Graph layouts we provide the user with the ability to generate random graph diagrams so that he/she can test the layouts
			NButton randomGraph1Button = new NButton("Random Graph (10 vertices, 15 edges)");
			randomGraph1Button.Click += OnRandomGraph1ButtonClick;
			itemsStack.Add(randomGraph1Button);

			NButton randomGraph2Button = new NButton("Random Graph (20 vertices, 30 edges)");
			randomGraph2Button.Click += OnRandomGraph2ButtonClick;
			itemsStack.Add(randomGraph2Button);

			stack.Add(new NGroupBox("Items", itemsStack));

			return stack;
		}
        protected override string GetExampleDescription()
        {
            return @"
<p>
    The radial graph layout layouts the graphs in concentric circles. The vertices with no
    predecessors are placed in the center and their descendants are placed on the next circle
    and so on. It produces a straight line graph drawing. The most important properties are:
</p>
<ul>
	<li>
		<b>Aspect Ratio</b> - determines the aspect (width/height) ratio of the layout.
		By default set to 1 which layouts the nodes in a circle. A value different from 1
		will make the layout order the nodes in an ellipse.
	</li>
	<li>
		<b>AutoSizeRings</b> - if set to true the RingRadius property is automatically
		calculated to have such value that the total area of the drawing is minimized and there
		is no node overlapping.
	</li>
	<li>
		<b>RingRadius</b> - determines the size of the radius of the first imaginary circle where
		nodes are placed. The radius of each other circle is a sum of the RingRadius value and
		the radius of the previous circle. This value is automatically determined if the 
		AutoSizeRings property is set to true.
	</li>
</ul>
<p>
	To experiment with their behavior just change the properties of the layout in the property
	grid and click the <b>Layout</b> button.
</p>
            ";
        }

		private void InitDiagram(NDrawingDocument drawingDocument)
        {
			// Hide ports
			drawingDocument.Content.ScreenVisibility.ShowPorts = false;

			// Create a random tree
			NGenericTreeTemplate tree = new NGenericTreeTemplate();
            tree.Levels = 4;
            tree.BranchNodes = 4;
            tree.HorizontalSpacing = 10;
            tree.VerticalSpacing = 10;
            tree.ConnectorType = ENConnectorShape.RoutableConnector;
            tree.VertexShape = ENBasicShape.Circle;
            tree.VertexSize = new NSize(40, 40);
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

		private void OnRandomGraph1ButtonClick(NEventArgs arg)
		{
			NDrawingDocument drawingDocument = m_DrawingView.Document;

			drawingDocument.StartHistoryTransaction("Create Random Graph 1");
			try
			{
				m_DrawingView.ActivePage.Items.Clear();

				// create a test tree
				NRandomGraphTemplate graph = new NRandomGraphTemplate();
				graph.EdgeUserClass = NDR.StyleSheetNameConnectors;
				graph.VertexCount = 10;
				graph.EdgeCount = 15;
				graph.VertexShape = VertexShape;
				graph.VertexSize = VertexSize;
				graph.Create(drawingDocument);

				// layout the tree
				ArrangeDiagram(drawingDocument);
			}
			finally
			{
				drawingDocument.CommitHistoryTransaction();
			}
		}
		private void OnRandomGraph2ButtonClick(NEventArgs arg)
		{
			NDrawingDocument drawingDocument = m_DrawingView.Document;

			drawingDocument.StartHistoryTransaction("Create Random Graph 2");
			try
			{
				m_DrawingView.ActivePage.Items.Clear();

				// create a test tree
				NRandomGraphTemplate graph = new NRandomGraphTemplate();
				graph.EdgeUserClass = NDR.StyleSheetNameConnectors;
				graph.VertexCount = 20;
				graph.EdgeCount = 30;
				graph.VertexShape = VertexShape;
				graph.VertexSize = VertexSize;
				graph.Create(drawingDocument);

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
		protected virtual void OnArrangeButtonClick(NEventArgs arg)
		{
			ArrangeDiagram(m_DrawingView.Document);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;
        private NRadialGraphLayout m_Layout = new NRadialGraphLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NRadialGraphLayoutExample.
        /// </summary>
        public static readonly NSchema NRadialGraphLayoutExampleSchema;

		#endregion

		#region Constants

		private const ENBasicShape VertexShape = ENBasicShape.Circle;
		private static readonly NSize VertexSize = new NSize(50, 50);

		#endregion
	}
}