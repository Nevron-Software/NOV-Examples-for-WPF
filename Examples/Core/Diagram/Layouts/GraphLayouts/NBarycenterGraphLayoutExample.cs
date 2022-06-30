using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Batches;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NBarycenterGraphLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NBarycenterGraphLayoutExample()
        {
            m_Layout.FixedVertexPlacement.Mode = ENFixedVertexPlacementMode.AutomaticEllipseRim;
            m_Layout.FixedVertexPlacement.PredefinedEllipse = new NRectangle(0, 0, 500, 500);
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NBarycenterGraphLayoutExample()
        {
            NBarycenterGraphLayoutExampleSchema = NSchema.Create(typeof(NBarycenterGraphLayoutExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override string GetExampleDescription()
        {
            return @"
<p>     
    The barycenter layout method splits the input graph into a set of fixed and free vertices. 
    Fixed vertices are nailed to the corners of a strictly convex polygon,           
    while free vertices are placed in the barycenter of their neighbors. 
    The barycenter force accessible from the <b>BarycenterForce</b> property of the layout is 
    responsible for attracting the vertices to their barycenter.
</p>
<p>
	In case there are no fixed vertices this will place all vertices at a single point, 
	which is obviously not a good graph drawing. That is why the barycenter layout needs 
	at least three fixed vertices.
</p>
<p>
	The minimal amount of fixed vertices is specified by the <b>MinFixedVerticesCount</b> property. 
	If the input graph does not have that many fixed vertices, the layout will automatically 
	fulfill this requirement. This is done by fixing the vertices with the smallest degree.
</p>
<p>
	In this example the fixed vertices are highlighted in orange.
</p>
";
        }
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
            NButton triangularGrid6 = new NButton("Create Triangular Grid (levels 6)");
            triangularGrid6.Click += delegate(NEventArgs args) { CreateTriangularGridDiagram(6); };
            stack.AddChild(triangularGrid6);

            NButton triangularGrid8 = new NButton("Create Triangular Grid (levels 8)");
            triangularGrid8.Click += delegate(NEventArgs args) { CreateTriangularGridDiagram(8); };
            stack.AddChild(triangularGrid8);

            NButton random10 = new NButton("Random (fixed 10, free 10)");
            random10.Click += delegate(NEventArgs args) { CreateRandomBarycenterDiagram(10, 10); };
            stack.AddChild(random10);

            NButton random15 = new NButton("Random (fixed 15, free 15)");
            random15.Click += delegate(NEventArgs args) { CreateRandomBarycenterDiagram(15, 15); };
            stack.AddChild(random15);

            stack.Add(new NGroupBox("Items", itemsStack));

            return stack;
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;

            // Create a random diagram 
            CreateRandomBarycenterDiagram(8, 10);

            // Arrange the diagram
            ArrangeDiagram(drawingDocument);

            // Fit active page
            drawingDocument.Content.ActivePage.ZoomMode = ENZoomMode.Fit;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a random barycenter diagram with the specified settings
        /// </summary>
        /// <param name="fixedCount">number of fixed vertices (must be larger than 3)</param>
        /// <param name="freeCount">number of free vertices</param>
        private void CreateRandomBarycenterDiagram(int fixedCount, int freeCount)
        {
            if (fixedCount < 3)
                throw new ArgumentException("Needs at least three fixed vertices");

            // clean up the active page
            NPage activePage = m_DrawingView.ActivePage;
            activePage.Items.Clear();

            // we will be using basic circle shapes with default size of (30, 30)
            NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();
            basicShapesFactory.DefaultSize = new NSize(30, 30);

			// create the fixed vertices
			NShape[] fixedShapes = new NShape[fixedCount];

            for (int i = 0; i < fixedCount; i++)
            {
                fixedShapes[i] = basicShapesFactory.CreateShape(ENBasicShape.Circle);

                //((NDynamicPort)fixedShapes[i].Ports.GetChildByName("Center", -1)).GlueMode = DynamicPortGlueMode.GlueToLocation;
                fixedShapes[i].Geometry.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant3, new NColor(251, 203, 156), new NColor(247, 150, 56));
                fixedShapes[i].Geometry.Stroke = new NStroke(1, new NColor(68, 90, 108));

                // setting the ForceXMoveable and ForceYMoveable properties to false
                // specifies that the layout cannot move the shape in both X and Y directions
                NForceDirectedGraphLayout.SetXMoveable(fixedShapes[i], false);
                NForceDirectedGraphLayout.SetYMoveable(fixedShapes[i], false);

                activePage.Items.AddChild(fixedShapes[i]);
            }

            // create the free vertices
            NShape[] freeShapes = new NShape[freeCount];
            for (int i = 0; i < freeCount; i++)
            {
                freeShapes[i] = basicShapesFactory.CreateShape(ENBasicShape.Circle);
                freeShapes[i].Geometry.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant3, new NColor(192, 194, 194), new NColor(129, 133, 133));
                freeShapes[i].Geometry.Stroke = new NStroke(1, new NColor(68, 90, 108));
                activePage.Items.AddChild(freeShapes[i]);
            }

            // link the fixed shapes in a circle
            for (int i = 0; i < fixedCount; i++)
            {
                NRoutableConnector connector = new NRoutableConnector();
                connector.MakeLine();
                activePage.Items.AddChild(connector);

                if (i == 0)
                {
                    connector.GlueBeginToShape(fixedShapes[fixedCount - 1]);
                    connector.GlueEndToShape(fixedShapes[0]);
                }
                else
                {
                    connector.GlueBeginToShape(fixedShapes[i - 1]);
                    connector.GlueEndToShape(fixedShapes[i]);
                }
            }

            // link each free shape with two different random fixed shapes
            Random rnd = new Random();
            for (int i = 0; i < freeCount; i++)
            {
                int firstFixed = rnd.Next(fixedCount);
                int secondFixed = (firstFixed + rnd.Next(fixedCount / 3) + 1) % fixedCount;

                // link with first fixed
                NRoutableConnector lineShape = new NRoutableConnector();
                lineShape.MakeLine();
                activePage.Items.AddChild(lineShape);

                lineShape.GlueBeginToShape(freeShapes[i]);
                lineShape.GlueEndToShape(fixedShapes[firstFixed]);

                // link with second fixed
                lineShape = new NRoutableConnector();
                lineShape.MakeLine();
                activePage.Items.AddChild(lineShape);

                lineShape.GlueBeginToShape(freeShapes[i]);
                lineShape.GlueEndToShape(fixedShapes[secondFixed]);
            }

            // link each free shape with another free shape
            for (int i = 1; i < freeCount; i++)
            {
                NRoutableConnector connector = new NRoutableConnector();
                connector.MakeLine();
                activePage.Items.AddChild(connector);

                connector.GlueBeginToShape(freeShapes[i - 1]);
                connector.GlueEndToShape(freeShapes[i]);
            }

            // send all edges to back
            NBatchReorder batchReorder = new NBatchReorder(m_DrawingView.Document);
            batchReorder.Build(activePage.GetShapes(false, NDiagramFilters.ShapeType1D).CastAll<NDiagramItem>());
            batchReorder.SendToBack(activePage);

            // arrange the elements
            ArrangeDiagram(m_DrawingView.Document);
        }
        /// <summary>
        /// Creates a triangular grid diagram with the specified count of levels
        /// </summary>
        /// <param name="levels"></param>
        private void CreateTriangularGridDiagram(int levels)
        {
            // clean up the active page
            NPage activePage = m_DrawingView.ActivePage;
            activePage.Items.Clear();

            // we will be using basic circle shapes with default size of (30, 30)
            NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();
            basicShapesFactory.DefaultSize = new NSize(30, 30);

			NShape prev = null;
			NList<NShape> prevRowShapes = null;

            for (int level = 1; level < levels; level++)
            {
				NList<NShape> curRowShapes = new NList<NShape>();

				for (int i = 0; i < level; i++)
                {
					NShape cur = basicShapesFactory.CreateShape(ENBasicShape.Circle);
					cur.Geometry.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant3, new NColor(192, 194, 194), new NColor(129, 133, 133));
                    cur.Geometry.Stroke = new NStroke(1, new NColor(68, 90, 108));
                    activePage.Items.Add(cur);

					NRoutableConnector edge;
					// connect with prev
					if (i > 0)
					{
						edge = new NRoutableConnector();
						edge.MakeLine();
						activePage.Items.Add(edge);

						edge.GlueBeginToShape(prev);
						edge.GlueEndToShape(cur);
					}

					// connect with ancestors
					if (level > 1)
                    {
                        if (i < prevRowShapes.Count)
                        {
                            edge = new NRoutableConnector();
                            edge.MakeLine();
                            activePage.Items.Add(edge);

                            edge.GlueBeginToShape(prevRowShapes[i]);
                            edge.GlueEndToShape(cur);
                        }

                        if (i > 0)
                        {
                            edge = new NRoutableConnector();
                            edge.MakeLine();
                            activePage.Items.Add(edge);

                            edge.GlueBeginToShape(prevRowShapes[i - 1]);
                            edge.GlueEndToShape(cur);
                        }
                    }

                    // fix the three corner vertices
                    if (level == 1 || (level == levels - 1 && (i == 0 || i == level - 1)))
                    {
                        NForceDirectedGraphLayout.SetXMoveable(cur, false);
                        NForceDirectedGraphLayout.SetYMoveable(cur, false);

                        cur.Geometry.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant3, new NColor(251, 203, 156), new NColor(247, 150, 56));
                        cur.Geometry.Stroke = new NStroke(1, new NColor(68, 90, 108));
                    }

                    curRowShapes.Add(cur);
                    prev = cur;
                }

                prevRowShapes = curRowShapes;
            }

            // send all edges to back
            NBatchReorder batchReorder = new NBatchReorder(m_DrawingView.Document);
            batchReorder.Build(activePage.GetShapes(false, NDiagramFilters.ShapeType1D).CastAll<NDiagramItem>());
            batchReorder.SendToBack(activePage);

            // arrange the elements
            ArrangeDiagram(m_DrawingView.Document);
        }
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
        private NBarycenterGraphLayout m_Layout = new NBarycenterGraphLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NBarycenterGraphLayoutExample.
        /// </summary>
        public static readonly NSchema NBarycenterGraphLayoutExampleSchema;

        #endregion
    }
}