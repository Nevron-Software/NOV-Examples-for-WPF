using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NInputAndOutputPortsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NInputAndOutputPortsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NInputAndOutputPortsExample()
        {
            NInputAndOutputPortsExampleSchema = NSchema.Create(typeof(NInputAndOutputPortsExample), NExampleBaseSchema);
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
    Demonstrates the behavior of input and output ports. Try to connect the AND shape ports with lines.
</p>
<p> 
    In NOV Diagram each port FlowMode can be set to Input, Output or InputAndOutput (the default). 
</p>
<p>
    <b>Input</b> ports can accept connections with End points of 1D shapes and output ports of 2D shapes.
    Input ports are painted in green color.
</p>
<p>
    <b>Output</b> ports can accept connections with Begin points of 1D shapes and input ports of 2D shapes.
    Output ports are painted in red color.
</p>
<p>
    <b>InputAndOutput</b> ports behave as both input and output ports (the default).
    InputAndOutput ports are painted in blue color.
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            // create a AND shape
            NShape andShape = CreateAndShape();
            andShape.SetBounds(300, 100, 150, 100);
            activePage.Items.Add(andShape);
        }

        #endregion

        #region Implementation 

        private NShape CreateAndShape()
        {
            NShape shape = new NShape();
            shape.Init2DShape();

            NSize normalSize = new NSize(1, 1);
            NGraphicsPath path = new NGraphicsPath();

            // create input lines
            double x1 = 0;
            double y1 = normalSize.Height / 3;
            path.StartFigure(x1, y1);
            path.LineTo(normalSize.Width / 4, y1);

            double y2 = normalSize.Height * 2 / 3;
            double x2 = 0;
            path.StartFigure(x2, y2);
            path.LineTo(normalSize.Width / 4, y2);

            // create body
            path.StartFigure(normalSize.Width / 4, 0);
            path.LineTo(normalSize.Width / 4, 1);
            NPoint ellipseCenter = new  NPoint(normalSize.Width / 4, 0.5);
            path.AddEllipseSegment(NRectangle.FromCenterAndSize(ellipseCenter, normalSize.Width, normalSize.Height), NMath.PIHalf, -NMath.PI);
            path.CloseFigure();

            // create output
            double y3 = normalSize.Height / 2;
            double x3 = normalSize.Width;
            path.StartFigure(normalSize.Width * 3 / 4, y3);
            path.LineTo(x3, y3);

            shape.Geometry.AddRelative(new NDrawPath(new NRectangle(0, 0, 1, 1), path));

            // create ports
            NPort input1 = new NPort();
            input1.X = x1;
            input1.Y = y1;
            input1.Relative = true;
            input1.SetDirection(ENBoxDirection.Left);
            input1.FlowMode = ENPortFlowMode.Input;
            shape.Ports.Add(input1);

            NPort input2 = new NPort();
            input2.X = x2;
            input2.Y = y2;
            input2.Relative = true;
            input2.SetDirection(ENBoxDirection.Left);
            input2.FlowMode = ENPortFlowMode.Input;
            shape.Ports.Add(input2);

            NPort output1 = new NPort();
            output1.X = x3;
            output1.Y = y3;
            output1.Relative = true;
            output1.SetDirection(ENBoxDirection.Right);
            output1.FlowMode = ENPortFlowMode.Output;
            shape.Ports.Add(output1);

            // by default this shape does not accept shape-to-shape connections
            shape.DefaultShapeGlue = ENDefaultShapeGlue.None;

            // set text
            shape.Text = "AND";

            return shape;
 
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NInputAndOutputPortsExample.
        /// </summary>
        public static readonly NSchema NInputAndOutputPortsExampleSchema;

        #endregion
    }
}