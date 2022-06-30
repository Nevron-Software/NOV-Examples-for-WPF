using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NGedcomImportExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGedcomImportExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGedcomImportExample()
        {
            NGedcomImportExampleSchema = NSchema.Create(typeof(NGedcomImportExample), NExampleBaseSchema);
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
<p>Demonstrates how to import a family tree in GEDCOM format (""*.ged"") to Nevron Diagram.
This example imports the family tree of the US president Abraham Lincoln.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Import a Visio diagram
            m_DrawingView.LoadFromResource(NResources.RSTR_LincolnFamily_ged);

            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NGedcomImportExample.
        /// </summary>
        public static readonly NSchema NGedcomImportExampleSchema;

        #endregion
    }
}