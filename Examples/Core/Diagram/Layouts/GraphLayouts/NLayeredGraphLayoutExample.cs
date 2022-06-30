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
	public class NLayeredGraphLayoutExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLayeredGraphLayoutExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLayeredGraphLayoutExample()
		{
			NLayeredGraphLayoutExampleSchema = NSchema.Create(typeof(NLayeredGraphLayoutExample), NExampleBaseSchema);
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
    The layered graph layout is used to layout a graph in layers. 
    The layout strives to minimize the number of crossings between edges 
    and produces a polyline graph drawing. This type of layout is very useful
    for the arrangement of flow diagrams, since it works well on all types of graphs
    (including those with self-loops and duplicate edges).
</p>
<p>
	The most important properties are:
	<ul>
		<li>
			<b>Direction</b> - determines the flow direction of the layout. By default set to <i>TopToBottom</i>.
		</li>
		<li>
			<b>EdgeRouting</b> - determines what edge routing is applied. Possible values are:
			<ul>
			    <li><i>Polyline</i> - the edges are drawn using a polyline with few bends</li>
			    <li><i>Orthogonal</i> - the edges are drawn using only horizontal and vertical line segments</li>
			</ul>  
		</li>
		<li>
			<b>NodeRank</b> - specifies the node ranking policy used by the layout. It can be:
			<ul>
			    <li><i>TopMost</i> - all nodes without incoming edges are assigned to the topmost layer</li>
			    <li><i>Gravity</i> - layer distribution is done in such a way that the total length of all edges is minimized</li>
			    <li><i>Optimal</i> - similar to the topmost, but after the initial assignment all nodes fall downwards as much as possible</li>
			</ul>
		</li>
		<li>
			<b>PlugSpacing</b> - determines the spacing between the plugs of a node.
			You can set a fixed amount of spacing or a proportional spacing, which means that the plugs
			are distributed along the whole side of the node.
		</li>
		<li>
			<b>LayerAlignment</b> - determines the vertical alignment of the nodes in the layers.
		</li>
		<li>
			<b>NodeAlignment</b> - determines the horizontal alignment of the nodes in the layers.
		</li>
		<li>
			<b>SelfLoopSpacingFactor</b> - determines the self-loop spacing factor. It spaces the self-loops as a ratio of the body height.
		</li>
		<li>
			<b>VertexSpacing and LayerSpacing</b> - determine the spacing between nodes and layers
			respectively.
		</li>
		<li>
			<b>StraightenLines</b> - if turned on an additional step is performed that tries to
			straighten the lines as much as possible in the case of orthogonal edge routing.
		</li>
		<li>
			<b>UseSingleBus</b> - if true and the EdgeRouting is orthogonal, all edges will be
			placed on a single bus between each pair of layers.
		</li>
		<li>
			<b>Compact</b> - determines whether the layout should try to minimize the width of the drawing or not.
		</li>
	</ul>		
	You can set the index of a given node in its layer explicitly using the <b>IndexInLayer</b> property
	from the <b>LayoutData</b> collection of the node. By default the index is set to -1 which
	means that the layout will automatically calculate it. If you set a value greater than or equal
	to 0 the node will be placed at that index. If the index is greater than the total number
	of nodes in the layer, it will be set equal to the number of vertices - 1.
</p>
<p>
	To experiment with the layout just change its properties from the property grid and click the <b>Layout</b> button. 
    To see the layout in action on a different graph, just click the <b>Random Graph</b> button. 
</p>
            ";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			const double width = 40;
			const double height = 40;
			const double distance = 80;

			// Hide ports
			drawingDocument.Content.ScreenVisibility.ShowPorts = false;

			NBasicShapeFactory basicShapes = new NBasicShapeFactory();
			NPage activePage = drawingDocument.Content.ActivePage;

			int[] from = new int[] { 1, 1, 1, 2, 2, 3, 3, 4, 4, 5, 6 };
			int[] to = new int[] { 2, 3, 4, 4, 5, 6, 7, 5, 9, 8, 9 };
			NShape[] shapes = new NShape[9];
			int vertexCount = shapes.Length, edgeCount = from.Length;
			int i, j, count = vertexCount + edgeCount;

			for (i = 0; i < count; i++)
			{
				if (i < vertexCount)
				{
					j = vertexCount % 2 == 0 ? i : i + 1;
					shapes[i] = basicShapes.CreateShape(ENBasicShape.Rectangle);

					if (vertexCount % 2 != 0 && i == 0)
					{
						shapes[i].SetBounds(new NRectangle(
							(width + (distance * 1.5)) / 2,
							distance + (j / 2) * (distance * 1.5),
							width,
							height));
					}
					else
					{
						shapes[i].SetBounds(new NRectangle(
							width / 2 + (j % 2) * (distance * 1.5),
							height + (j / 2) * (distance * 1.5),
							width,
							height));
					}

					activePage.Items.Add(shapes[i]);
				}
				else
				{
					NRoutableConnector edge = new NRoutableConnector();
					edge.UserClass = "Connector";
					activePage.Items.Add(edge);
					edge.GlueBeginToShape(shapes[from[i - vertexCount] - 1]);
					edge.GlueEndToShape(shapes[to[i - vertexCount] - 1]);
				}
			}

			// arrange diagram
			ArrangeDiagram(drawingDocument);

			// fit active page
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

				// create a test graph
				NRandomGraphTemplate graph = new NRandomGraphTemplate();
				graph.EdgesUserClass = "Connector";
				graph.VertexCount = 20;
				graph.EdgeCount = 30;
				graph.VerticesShape = VertexShape;
				graph.VerticesSize = VertexSize;
				graph.Create(drawingDocument);

				// layout the graph
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
		private NLayeredGraphLayout m_Layout = new NLayeredGraphLayout();

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLayeredGraphLayoutExample.
		/// </summary>
		public static readonly NSchema NLayeredGraphLayoutExampleSchema;

		#endregion

		#region Constants

		private const ENBasicShape VertexShape = ENBasicShape.Rectangle;
		private static readonly NSize VertexSize = new NSize(50, 50);

		#endregion
	}
}