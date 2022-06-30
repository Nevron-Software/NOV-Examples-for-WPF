using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NFlowChartingShapesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFlowChartingShapesExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFlowChartingShapesExample()
        {
            NFlowChartingShapesExampleSchema = NSchema.Create(typeof(NFlowChartingShapesExample), NExampleBaseSchema);
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
    This example demonstrates the flow charting shapes, which are created by the NFlowChartingShapesFactory.
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // Hide grid and ports
            drawing.ScreenVisibility.ShowGrid = false;
            drawing.ScreenVisibility.ShowPorts = false;

            // create all shapes
            NFlowchartShapeFactory factory = new NFlowchartShapeFactory();
            factory.DefaultSize = new NSize(120, 90);

            for (int i = 0; i < factory.ShapeCount; i++)
            {
                NShape shape = factory.CreateShape(i);
                shape.HorizontalPlacement = ENHorizontalPlacement.Center;
                shape.VerticalPlacement = ENVerticalPlacement.Center;
                shape.Text = factory.GetShapeInfo(i).Name;
                shape.MoveTextBlockBelowShape();
                activePage.Items.Add(shape);
            }

            // arrange them
            NList<NShape> shapes = activePage.GetShapes(false);
            NLayoutContext layoutContext = new NLayoutContext();
            layoutContext.BodyAdapter = new NShapeBodyAdapter(drawingDocument);
            layoutContext.GraphAdapter = new NShapeGraphAdapter();
            layoutContext.LayoutArea = activePage.Bounds;

            NTableFlowLayout flowLayout = new NTableFlowLayout();
            flowLayout.HorizontalSpacing = 30;
            flowLayout.VerticalSpacing = 50;
            flowLayout.Direction = ENHVDirection.LeftToRight;
            flowLayout.MaxOrdinal = 5;
            flowLayout.Arrange(shapes.CastAll<object>(), layoutContext);

            // size page to content
            activePage.Layout.ContentPadding = new NMargins(40);
            activePage.SizeToContent();
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NFlowChartingShapesExample.
        /// </summary>
        public static readonly NSchema NFlowChartingShapesExampleSchema;

        #endregion
    }
}