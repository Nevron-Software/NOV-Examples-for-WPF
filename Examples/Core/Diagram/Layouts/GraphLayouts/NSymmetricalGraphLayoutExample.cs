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
    public class NSymmetricalGraphLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NSymmetricalGraphLayoutExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NSymmetricalGraphLayoutExample()
        {
            NSymmetricalGraphLayoutExampleSchema = NSchema.Create(typeof(NSymmetricalGraphLayoutExample), NExampleBaseSchema);
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
    The symmetrical layout represents an implementation of the Fruchertman and Reingold force directed layout (with some modifications).
</p>
<p>
    It uses attractive and repulsive forces, which aim to produce a drawing with uniform distance between each set of connected vertices. Because of that the drawing tends to be symmetrical.
</p>
<p>
	The attractive and repulsive forces are coupled in an instance of the <b>NDesiredDistanceForce</b> class, accessible from the <b>DesiredDistanceForce</b> property. 
</p>
<p>
	To experiment with the layout behavior just change the properties of the layout in the property grid and click the <b>Layout</b> button.
</p>
            ";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
			// Hide ports
			drawingDocument.Content.ScreenVisibility.ShowPorts = false;

			// Create random diagram
			NGenericTreeTemplate template = new NGenericTreeTemplate();
            template.Balanced = false;
            template.Levels = 6;
            template.BranchNodes = 3;
            template.HorizontalSpacing = 10;
            template.VerticalSpacing = 10;
            template.VerticesShape = ENBasicShape.Circle;
            template.VerticesSize = new NSize(50, 50);
            template.VertexSizeDeviation = 0;
            template.Create(drawingDocument);

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
				graph.EdgesUserClass = "Connector";
				graph.VertexCount = 10;
				graph.EdgeCount = 15;
				graph.VerticesShape = VertexShape;
				graph.VerticesSize = VertexSize;
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
				graph.EdgesUserClass = "Connector";
				graph.VertexCount = 20;
				graph.EdgeCount = 30;
				graph.VerticesShape = VertexShape;
				graph.VerticesSize = VertexSize;
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
        private NSymmetricalGraphLayout m_Layout = new NSymmetricalGraphLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NSymmetricalGraphLayoutExample.
        /// </summary>
        public static readonly NSchema NSymmetricalGraphLayoutExampleSchema;

		#endregion

		#region Constants

		private const ENBasicShape VertexShape = ENBasicShape.Circle;
		private static readonly NSize VertexSize = new NSize(50, 50);

		#endregion
	}
}