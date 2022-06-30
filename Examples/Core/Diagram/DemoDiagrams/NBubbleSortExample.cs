using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NBubbleSortExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NBubbleSortExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NBubbleSortExample()
        {
            NBubbleSortExampleSchema = NSchema.Create(typeof(NBubbleSortExample), NExampleBaseSchema);
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
	Demonstrates how to create a flowchart that describes the Bubble Sort algorithm.
</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NStyleSheet sheet = new NStyleSheet();
            drawingDocument.StyleSheets.Add(sheet);

            // create a rule that applies to the TextBlocks of all shapes with user class Connectors
			string connectorClass = NDR.StyleSheetNameConnectors;
            {
                NRule rule2 = new NRule();
                sheet.Add(rule2);

                NSelectorBuilder sb = rule2.GetSelectorBuilder();
                sb.Start();
                sb.Type(NTextBlock.NTextBlockSchema); sb.ChildOf(); sb.UserClass(connectorClass);
                sb.End();

                rule2.Declarations.Add(new NValueDeclaration<NFill>(NTextBlock.BackgroundFillProperty, new NColorFill(NColor.White)));
            }

            // get drawing and active page
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // hide ports and grid
            drawing.ScreenVisibility.ShowGrid = false;
            drawing.ScreenVisibility.ShowPorts = false;

            NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();
			NFlowchartShapeFactory flowChartingShapesFactory = new NFlowchartShapeFactory();
            NConnectorShapeFactory connectorShapesFactory = new NConnectorShapeFactory();

            // create title
            NShape titleShape = basicShapesFactory.CreateTextShape("Bubble Sort");
            titleShape.SetBounds(GetGridCell(0, 1, 2, 1));
            titleShape.TextBlock.FontName = "Arial";
            titleShape.TextBlock.FontSize = 40;
            titleShape.TextBlock.FontStyle = ENFontStyle.Bold;
            titleShape.TextBlock.Fill = new NColorFill(new NColor(68, 90, 108));
            titleShape.TextBlock.Shadow = new NShadow();
            activePage.Items.AddChild(titleShape);

            // begin shape
            NShape shapeBegin = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Termination);
            shapeBegin.SetBounds(GetGridCell(0, 0));
            shapeBegin.Text = "BEGIN";
            activePage.Items.Add(shapeBegin);

            // get array item shape
            NShape shapeGetItem = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Data);
            shapeGetItem.SetBounds(GetGridCell(1, 0));
            shapeGetItem.Text = "Get array item [1...n]";
            activePage.Items.Add(shapeGetItem);

            // i = 1 shape
            NShape shapeI1 = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Process);
            shapeI1.SetBounds(GetGridCell(2, 0));
            shapeI1.Text = "i = 1";
            activePage.Items.Add(shapeI1);

            // j = n shape
            NShape shapeJEN = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Process);
            shapeJEN.SetBounds(GetGridCell(3, 0));
            shapeJEN.Text = "j = n";
            activePage.Items.Add(shapeJEN);

            // less comparison shape
            NShape shapeLess = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Decision);
            shapeLess.SetBounds(GetGridCell(4, 0));
            shapeLess.Text = "item[i] < item[j - 1]?";
            activePage.Items.Add(shapeLess);

            // swap shape
            NShape shapeSwap = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Process);
            shapeSwap.SetBounds(GetGridCell(4, 1));
            shapeSwap.Text = "Swap item[i] and item[j-1]";
            activePage.Items.Add(shapeSwap);

            // j > i + 1? shape
            NShape shapeJQ = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Decision);
            shapeJQ.SetBounds(GetGridCell(5, 0));
            shapeJQ.Text = "j = (i + 1)?";
            activePage.Items.Add(shapeJQ);

            // dec j shape
            NShape shapeDecJ = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Process);
            shapeDecJ.SetBounds(GetGridCell(5, 1));
            shapeDecJ.Text = "j = j - 1";
            activePage.Items.Add(shapeDecJ);

            // i > n - 1? shape
            NShape shapeIQ = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Decision);
            shapeIQ.SetBounds(GetGridCell(6, 0));
            shapeIQ.Text = "i = (n - 1)?";
            activePage.Items.Add(shapeIQ);

            // inc i shape
            NShape shapeIncI = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Process);
            shapeIncI.SetBounds(GetGridCell(6, 1));
            shapeIncI.Text = "i = i + 1";
            activePage.Items.Add(shapeIncI);

            // end shape
            NShape shapeEnd = flowChartingShapesFactory.CreateShape(ENFlowchartingShape.Termination);
            shapeEnd.SetBounds(GetGridCell(7, 0));
            shapeEnd.Text = "END";
            activePage.Items.Add(shapeEnd);

            // connect begin with get array item
            NShape connector1 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector1.UserClass = connectorClass;
            activePage.Items.AddChild(connector1);
            connector1.GlueBeginToShape(shapeBegin);
            connector1.GlueEndToShape(shapeGetItem);

            // connect get array item with i = 1
            NShape connector2 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector2.UserClass = connectorClass;
            activePage.Items.AddChild(connector2);
            connector2.GlueBeginToShape(shapeGetItem);
            connector2.GlueEndToShape(shapeI1);

            // connect i = 1 and j = n
            NShape connector3 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector3.UserClass = connectorClass;
            activePage.Items.AddChild(connector3);
            connector3.GlueBeginToShape(shapeI1);
            connector3.GlueEndToShape(shapeJEN);

            // connect j = n and item[i] < item[j-1]?
            NShape connector4 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector4.UserClass = connectorClass;
            activePage.Items.AddChild(connector4);
            connector4.GlueBeginToShape(shapeJEN);
            connector4.GlueEndToShape(shapeLess);

            // connect item[i] < item[j-1]? and j = (i + 1)? 
            NShape connector5 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector5.UserClass = connectorClass;
            connector5.Text = "No";
            activePage.Items.AddChild(connector5);
            connector5.GlueBeginToShape(shapeLess);
            connector5.GlueEndToShape(shapeJQ);

            // connect j = (i + 1)? and i = (n - 1)?
            NShape connector6 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector6.UserClass = connectorClass;
            activePage.Items.AddChild(connector6);
            connector6.GlueBeginToShape(shapeJQ);
            connector6.GlueEndToShape(shapeIQ);

            // connect i = (n - 1) and END
            NShape connector7 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector7.UserClass = connectorClass;
            activePage.Items.AddChild(connector7);
            connector7.GlueBeginToShape(shapeIQ);
            connector7.GlueEndToShape(shapeEnd);

            // connect item[i] < item[j-1]? and Swap
            NShape connector8 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector8.UserClass = connectorClass;
            connector8.Text = "Yes";
            activePage.Items.AddChild(connector8);
            connector8.GlueBeginToShape(shapeLess);
            connector8.GlueEndToShape(shapeSwap);

            // connect j = (i + 1)? and j = (j - 1)
            NShape connector9 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector9.UserClass = connectorClass;
            activePage.Items.AddChild(connector9);
            connector9.GlueBeginToShape(shapeJQ);
            connector9.GlueEndToShape(shapeDecJ);

            // connect i = (n - 1)? and i = (i + 1)
            NShape connector10 = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
            connector10.UserClass = connectorClass;
            activePage.Items.AddChild(connector10);
            connector10.GlueBeginToShape(shapeIQ);
            connector10.GlueEndToShape(shapeIncI);

            // connect Swap to No connector
            NShape connector11 = connectorShapesFactory.CreateShape(ENConnectorShape.TopBottomToSide);
            connector11.UserClass = connectorClass;
            activePage.Items.AddChild(connector11);
            connector11.GlueBeginToShape(shapeSwap);
            connector11.GlueEndToShape(connector5);

            // connect i = i + 1 to connector3
            NShape connector12 = connectorShapesFactory.CreateSideToSide(m_GridSpacing.Width * 2);
            connector12.UserClass = connectorClass;
            activePage.Items.AddChild(connector12);
            connector12.GlueBeginToPort(shapeIncI.GetPortByName("Right"));
            connector12.GlueEndToGeometryContour(connector3, 0.5f);

            // connect j = j - 1 to connector4
            NShape connector13 = connectorShapesFactory.CreateSideToSide(m_GridSpacing.Width);
            connector13.UserClass = connectorClass;
            activePage.Items.AddChild(connector13);
            connector13.GlueBeginToPort(shapeDecJ.GetPortByName(("Right")));
            connector13.GlueEndToGeometryContour(connector4, 0.5f);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        protected NRectangle GetGridCell(int row, int col)
        {
            return GetGridCell(row, col, m_GridOrigin, m_GridCellSize, m_GridSpacing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        protected NRectangle GetGridCell(int row, int col, int rowSpan, int colSpan)
        {
            NRectangle cell1 = GetGridCell(row, col, m_GridOrigin, m_GridCellSize, m_GridSpacing);
            NRectangle cell2 = GetGridCell(row + rowSpan, col + colSpan, m_GridOrigin, m_GridCellSize, m_GridSpacing);
            return NRectangle.Union(cell1, cell2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <param name="spacing"></param>
        /// <returns></returns>
        protected NRectangle GetGridCell(int row, int col, NPoint origin, NSize size, NSize spacing)
        {
            return new NRectangle(origin.X + col * (size.Width + spacing.Width),
                                    origin.Y + row * (size.Height + spacing.Height),
                                    size.Width, size.Height);
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        private NPoint m_GridOrigin = new NPoint(30, 30);
        private NSize m_GridCellSize = new NSize(180, 70);
        private NSize m_GridSpacing = new NSize(50, 40);

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NBubbleSortExample.
        /// </summary>
        public static readonly NSchema NBubbleSortExampleSchema;

        #endregion
    }
}