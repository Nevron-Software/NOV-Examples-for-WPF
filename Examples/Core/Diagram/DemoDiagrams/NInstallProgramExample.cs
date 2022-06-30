using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NInstallProgramExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NInstallProgramExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NInstallProgramExample()
        {
            NInstallProgramExampleSchema = NSchema.Create(typeof(NInstallProgramExample), NExampleBaseSchema);
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
            return @"<p>Demonstrates how to create a flowchart that describes a Software Installation process.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NStyleSheet sheet = new NStyleSheet();
            drawingDocument.StyleSheets.Add(sheet);

            // create a rule that applies to the geometries of all shapes with user class Connectors
            const string connectorsClass = "Connector";
            {
                NRule rule = sheet.CreateRule(delegate(NSelectorBuilder sb)
                {
                    sb.Type(NGeometry.NGeometrySchema); sb.ChildOf(); sb.UserClass(connectorsClass);
                });
                rule.AddValueDeclaration<NArrowhead>(NGeometry.EndArrowheadProperty, new NArrowhead(ENArrowheadShape.TriangleNoFill), true);
            }

            // create a rule that applies to the TextBlocks of all shapes with user class Connectors
            {
                NRule rule = sheet.CreateRule(delegate(NSelectorBuilder sb)
                {
                    sb.Type(NTextBlock.NTextBlockSchema); sb.ChildOf(); sb.UserClass(connectorsClass);
                });
                rule.AddValueDeclaration<NFill>(NTextBlock.BackgroundFillProperty, new NColorFill(NColor.White));
            }

            // create a rule that applies to shapes with user class  "STARTEND"
            {
                NRule rule = sheet.CreateRule(delegate(NSelectorBuilder sb)
                {
                    sb.Type(NGeometry.NGeometrySchema); sb.ChildOf(); sb.UserClass("STARTEND");
                });
                rule.AddValueDeclaration<NFill>(NGeometry.FillProperty, new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, new NColor(247, 150, 56), new NColor(251, 203, 156)));
            }

            // create a rule that applies to shapes with user class  "QUESTION"
            {
                NRule rule = sheet.CreateRule(delegate(NSelectorBuilder sb)
                {
                    sb.Type(NGeometry.NGeometrySchema); sb.ChildOf(); sb.UserClass("QUESTION");
                });
                rule.AddValueDeclaration<NFill>(NGeometry.FillProperty, new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, new NColor(129, 133, 133), new NColor(192, 194, 194)));
            }

            // create a rule that applies to shapes with user class  "ACTION"
            {
                NRule rule = sheet.CreateRule(delegate(NSelectorBuilder sb)
                {
                    sb.Type(NGeometry.NGeometrySchema); sb.ChildOf(); sb.UserClass("ACTION");
                });
                rule.AddValueDeclaration<NFill>(NGeometry.FillProperty, new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant1, new NColor(68, 90, 108), new NColor(162, 173, 182)));
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

            NRectangle bounds;

            int vSpacing = 35;
            int hSpacing = 45;
            int topMargin = 10;
            int leftMargin = 10;

            int shapeWidth = 90;
            int shapeHeight = 55;

            int col1 = leftMargin;
            int col2 = col1 + shapeWidth + hSpacing;
            int col3 = col2 + shapeWidth + hSpacing;
            int col4 = col3 + shapeWidth + hSpacing;

            int row1 = topMargin;
            int row2 = row1 + shapeHeight + vSpacing;
            int row3 = row2 + shapeHeight + vSpacing;
            int row4 = row3 + shapeHeight + vSpacing;
            int row5 = row4 + shapeHeight + vSpacing;
            int row6 = row5 + shapeHeight + vSpacing;

            bounds = new NRectangle(col2, row1, shapeWidth, shapeHeight);
            NShape start = CreateFlowChartingShape(ENFlowchartingShape.Termination, bounds, "START", "STARTEND");

            // row 2
            bounds = new NRectangle(col2, row2, shapeWidth, shapeHeight);
            NShape haveSerialNumber = CreateFlowChartingShape(ENFlowchartingShape.Decision, bounds, "Have a serial number?", "QUESTION");

            bounds = new NRectangle(col3, row2, shapeWidth, shapeHeight);
            NShape getSerialNumber = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Get serial number", "ACTION");

            // row 3
            bounds = new NRectangle(col1, row3, shapeWidth, shapeHeight);
            NShape enterSerialNumber = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Enter serial number", "ACTION");

            bounds = new NRectangle(col2, row3, shapeWidth, shapeHeight);
            NShape haveDiskSpace = CreateFlowChartingShape(ENFlowchartingShape.Decision, bounds, "Have disk space?", "QUESTION");

            bounds = new NRectangle(col3, row3, shapeWidth, shapeHeight);
            NShape freeUpSpace = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Free up space", "ACTION");

            // row 4
            bounds = new NRectangle(col1, row4, shapeWidth, shapeHeight);
            NShape runInstallRect = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Run install file", "ACTION");

            bounds = new NRectangle(col2, row4, shapeWidth, shapeHeight);
            NShape registerNow = CreateFlowChartingShape(ENFlowchartingShape.Decision, bounds, "Register now?", "QUESTION");

            bounds = new NRectangle(col3, row4, shapeWidth, shapeHeight);
            NShape fillForm = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Fill out form", "ACTION");

            bounds = new NRectangle(col4, row4, shapeWidth, shapeHeight);
            NShape submitForm = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Submit form", "ACTION");

            // row 5
            bounds = new NRectangle(col1, row5, shapeWidth, shapeHeight);
            NShape finishInstall = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Finish installation", "ACTION");

            bounds = new NRectangle(col2, row5, shapeWidth, shapeHeight);
            NShape restartNeeded = CreateFlowChartingShape(ENFlowchartingShape.Decision, bounds, "Restart needed?", "QUESTION");

            bounds = new NRectangle(col3, row5, shapeWidth, shapeHeight);
            NShape restart = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "Restart", "ACTION");

            // row 6
            bounds = new NRectangle(col2, row6, shapeWidth, shapeHeight);
            NShape run = CreateFlowChartingShape(ENFlowchartingShape.Process, bounds, "RUN", "STARTEND");

            // create connectors
            CreateConnector(start, "Bottom", haveSerialNumber, "Top", ENConnectorShape.Line, "");
            CreateConnector(getSerialNumber, "Top", haveSerialNumber, "Top", ENConnectorShape.RoutableConnector, "");
            CreateConnector(haveSerialNumber, "Right", getSerialNumber, "Left", ENConnectorShape.Line, "No");
            CreateConnector(haveSerialNumber, "Bottom", enterSerialNumber, "Top", ENConnectorShape.BottomToTop1, "Yes");
            CreateConnector(enterSerialNumber, "Right", haveDiskSpace, "Left", ENConnectorShape.Line, "");
            CreateConnector(freeUpSpace, "Top", haveDiskSpace, "Top", ENConnectorShape.RoutableConnector, "");
            CreateConnector(haveDiskSpace, "Right", freeUpSpace, "Left", ENConnectorShape.Line, "No");
            CreateConnector(haveDiskSpace, "Bottom", runInstallRect, "Top", ENConnectorShape.BottomToTop1, "Yes");
            CreateConnector(registerNow, "Right", fillForm, "Left", ENConnectorShape.Line, "Yes");
            CreateConnector(registerNow, "Bottom", finishInstall, "Top", ENConnectorShape.BottomToTop1, "No");
            CreateConnector(fillForm, "Right", submitForm, "Left", ENConnectorShape.Line, "");
            CreateConnector(submitForm, "Bottom", finishInstall, "Top", ENConnectorShape.BottomToTop1, "");
            CreateConnector(finishInstall, "Right", restartNeeded, "Left", ENConnectorShape.Line, "");
            CreateConnector(restart, "Bottom", run, "Top", ENConnectorShape.BottomToTop1, "");
            CreateConnector(restartNeeded, "Right", restart, "Left", ENConnectorShape.Line, "Yes");
            CreateConnector(restartNeeded, "Bottom", run, "Top", ENConnectorShape.Line, "No");
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a predefined basic shape
        /// </summary>
        /// <param name="basicShape">basic shape</param>
        /// <param name="bounds">bounds</param>
        /// <param name="text">default label text</param>
        /// <param name="userClass">name of the stylesheet from which to inherit styles</param>
        /// <returns>new basic shape</returns>
        private NShape CreateBasicShape(ENBasicShape basicShape, NRectangle bounds, string text, string userClass)
        {
            // create shape
            NShape shape = new NBasicShapeFactory().CreateShape(basicShape);

            // set bounds, text and user class
            shape.SetBounds(bounds);
            shape.Text = text;
            shape.UserClass = userClass;

            // add to active page
            m_DrawingView.ActivePage.Items.Add(shape);
            return shape;
        }
        /// <summary>
        /// Creates a predefined flow charting shape
        /// </summary>
        /// <param name="flowChartShape">flow charting shape</param>
        /// <param name="bounds">bounds</param>
        /// <param name="text">default label text</param>
        /// <param name="userClass">name of the stylesheet from which to inherit styles</param>
        /// <returns>new basic shape</returns>
        private NShape CreateFlowChartingShape(ENFlowchartingShape flowChartShape, NRectangle bounds, string text, string userClass)
        {
            // create shape
            NShape shape = new NFlowchartShapeFactory().CreateShape(flowChartShape);

            // set bounds, text and user class
            shape.SetBounds(bounds);
            shape.Text = text;
            shape.UserClass = userClass;

            // add to active page
            m_DrawingView.ActivePage.Items.Add(shape);
            return shape;
        }
        /// <summary>
        /// Creates a new connector, which connects the specified shapes
        /// </summary>
        /// <param name="fromShape"></param>
        /// <param name="fromPortName"></param>
        /// <param name="toShape"></param>
        /// <param name="toPortName"></param>
        /// <param name="connectorType"></param>
        /// <param name="text"></param>
        /// <returns>new 1D shapes</returns>
        private NShape CreateConnector(NShape fromShape, string fromPortName, NShape toShape, string toPortName, ENConnectorShape connectorType, string text)
        {
            // check arguments
            if (fromShape == null)
                throw new ArgumentNullException("fromShape");

            if (toShape == null)
                throw new ArgumentNullException("toShape");

            // create the connector
            NShape connector = new NConnectorShapeFactory().CreateShape(connectorType);

            // set text and user class
            connector.Text = text;
            connector.UserClass = NDR.StyleSheetNameConnectors;

            // connect begin
            NPort fromPort = fromShape.Ports.GetPortByName(fromPortName);
            if (fromPort != null)
            {
                connector.GlueBeginToPort(fromPort);
            }
            else
            {
                connector.GlueBeginToShape(fromShape);
            }

            // connect end
            NPort toPort = toShape.Ports.GetPortByName(toPortName);
            if (toPort != null)
            {
                connector.GlueEndToPort(toPort);
            }
            else
            {
                connector.GlueEndToShape(toShape);
            }

            // add to active page
            m_DrawingView.ActivePage.Items.Add(connector);

            return connector;
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
        /// Schema associated with NInstallProgramExample.
        /// </summary>
        public static readonly NSchema NInstallProgramExampleSchema;

        #endregion
    }
}