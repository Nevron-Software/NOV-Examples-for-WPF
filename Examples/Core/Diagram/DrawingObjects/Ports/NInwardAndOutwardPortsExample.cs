using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NInwardAndOutwardPortsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NInwardAndOutwardPortsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NInwardAndOutwardPortsExample()
        {
            NInwardAndOutwardPortsExampleSchema = NSchema.Create(typeof(NInwardAndOutwardPortsExample), NExampleBaseSchema);
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
    Demonstrates the behavior of inward and outward ports. Move the rectangle close to the Star to see the rectangle rotate to match the snapped ports direction.
</p>
<p> 
    In NOV Diagram each port GlueMode can be set to Inward, Outward or InwardAndOutward. 
</p>
<p>
    <b>Inward</b> ports can accept connections with Begin and End points of 1D shapes as well as other shapes Outward ports.
    Most of the ports are only Inward ports.
</p>
<p>
    <b>Outward</b> ports can be connected only to other shapes Inward ports. When a shape with outward ports
	is moved closed a shape with inward ports. The two shapes are glued in master-slave relation. The shape
	to which the outward port belongs is rotated and translated so that its outward port location matches
	the inward port location and so that the ports directions form a line.
</p>
<p>
    <b>InwardAndOutward</b> ports behave as both inward and outward.
</p>
";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            // plotter commands
            NBasicShapeFactory basicShapes = new NBasicShapeFactory();
           
            // create a rectangle with an outward port
            NShape rectShape = basicShapes.CreateShape(ENBasicShape.Rectangle);
            rectShape.SetBounds(50, 50, 100, 100);
            rectShape.Text = "Move me close to the star";
            rectShape.GetPortByName("Top").GlueMode = ENPortGlueMode.Outward;
            activePage.Items.Add(rectShape);

            // create a pentagram
            NShape pentagramShape = basicShapes.CreateShape(ENBasicShape.Pentagram);
            pentagramShape.SetBounds(310, 310, 100, 100);
            activePage.Items.Add(pentagramShape);
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NInwardAndOutwardPortsExample.
        /// </summary>
        public static readonly NSchema NInwardAndOutwardPortsExampleSchema;

        #endregion
    }
}