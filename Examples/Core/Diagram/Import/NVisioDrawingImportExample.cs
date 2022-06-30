using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NVisioDrawingImportExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NVisioDrawingImportExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NVisioDrawingImportExample()
        {
            NVisioDrawingImportExampleSchema = NSchema.Create(typeof(NVisioDrawingImportExample), NExampleBaseSchema);
        }

        #endregion

        #region Protected Overrides

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
            return @"<p>Demonstrates how to import a Visio drawing (in VSDX format) to NOV Diagram.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Import a Visio diagram
            m_DrawingView.LoadFromResource(NResources.RBIN_VSDX_CorporateDiagramShapes_vsdx);

            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NVisioDrawingImportExample.
        /// </summary>
        public static readonly NSchema NVisioDrawingImportExampleSchema;

        #endregion
    }
}