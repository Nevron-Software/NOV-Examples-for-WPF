using Nevron.Nov.Diagram;
using Nevron.Nov.Graphics;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Diagram.Batches;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.Diagram
{
    /// <summary>
    /// 
    /// </summary>
    public class NGroupsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGroupsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGroupsExample()
        {
            NGroupsExampleSchema = NSchema.Create(typeof(NGroupsExample), NExampleBaseSchema);
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
            return "Demonstrates how to create and use groups.";
        }
        
        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // create networks
            NGroup network1 = CreateNetwork(new NPoint(200, 20), "Network 1");
            NGroup network2 = CreateNetwork(new NPoint(400, 250), "Network 2");
            NGroup network3 = CreateNetwork(new NPoint(20, 250), "Network 3");
            NGroup network4 = CreateNetwork(new NPoint(200, 475), "Network 4");

            // connect networks
            ConnectNetworks(network1, network2);
            ConnectNetworks(network1, network3);
            ConnectNetworks(network1, network4);

            // hide some elements
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;
            drawingDocument.Content.ScreenVisibility.ShowGrid = false;
        }

        #endregion

        #region Implementation

        protected NGroup CreateNetwork(NPoint location, string labelText)
        {
            bool rectValid = false;
            NRectangle rect = NRectangle.Zero;
            NPage activePage = m_DrawingView.ActivePage;

            NList<NShape> shapes = activePage.GetShapes(false);
            for (int i = 0; i < shapes.Count; i++)
            {
                NRectangle bounds = shapes[i].GetAlignBoxInPage();
                if (rectValid)
                {
                    rect = NRectangle.Union(rect, bounds);
                }
                else
                {
                    rect = bounds;
                    rectValid = true;
                }
            }

            if (rectValid)
            {
                // determine how much is out of layout area
            }

            // create computer1
            NShape computer1 = CreateComputer();
            computer1.SetBounds(0, 0, computer1.Width, computer1.Height);

            // create computer2
            NShape computer2 = CreateComputer();
            computer2.SetBounds(150, 0, computer2.Width, computer2.Height);

            // create computer3
            NShape computer3 = CreateComputer();
            computer3.SetBounds(75, 120, computer3.Width, computer3.Height);

            // create the group that contains the comptures
            NGroup group = new NGroup();
            NBatchGroup batchGroup = new NBatchGroup(m_DrawingView.Document);
            batchGroup.Build(computer1, computer2, computer3);
            batchGroup.Group(m_DrawingView.ActivePage, out group);

            // connect the computers in the network
            ConnectComputers(computer1, computer2, group);
            ConnectComputers(computer2, computer3, group);
            ConnectComputers(computer3, computer1, group);

            // insert a frame
            NDrawRectangle drawRect = new NDrawRectangle(0, 0, 1, 1);
            drawRect.Relative = true;
            group.Geometry.Add(drawRect);

            // change group fill style
            group.Geometry.Fill = new NStockGradientFill(ENGradientStyle.FromCenter, ENGradientVariant.Variant2, NColor.Gainsboro, NColor.White);

            // reposition and resize the group
            group.SetBounds(location.X, location.Y, group.Width, group.Height);

            // set label text
            group.TextBlock.Text = labelText;



            return group;
        }

        protected NShape CreateComputer()
        {
            NNetworkShapeFactory networkShapes = new NNetworkShapeFactory();
            NShape computerShape = networkShapes.CreateShape((int)ENNetworkShape.Computer);
            return computerShape;
        }

        private void ConnectNetworks(NGroup fromNetwork, NGroup toNetwork)
        {
            NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();
            NShape lineShape = connectorShapes.CreateShape((int)ENConnectorShape.Line);
            lineShape.GlueBeginToShape(fromNetwork);
            lineShape.GlueEndToShape(toNetwork);
            m_DrawingView.ActivePage.Items.Add(lineShape);
        }

        private void ConnectComputers(NShape fromComputer, NShape toComputer, NGroup group)
        {
            NConnectorShapeFactory connectorShapes = new NConnectorShapeFactory();
            NShape lineShape = connectorShapes.CreateShape((int)ENConnectorShape.Line);
            lineShape.GlueBeginToShapeBoxIntersection(fromComputer);
            lineShape.GlueEndToShapeBoxIntersection(toComputer);
            group.Shapes.Add(lineShape);
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NGroupsExample.
        /// </summary>
        public static readonly NSchema NGroupsExampleSchema;

        #endregion
    }
}