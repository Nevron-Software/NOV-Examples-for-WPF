using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Formats;
using Nevron.Nov.Dom;
using Nevron.Nov.IO;
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
<p>
	Demonstrates how to import a family tree in GEDCOM format (""*.ged"") in Nevron Diagram.
	Before importing any GEDCOM files, you should set the <b>FamilyTreeLibrary</b> property of
	<b>NDrawingFormat.Gedcom</b> or use its <b>LoadFamilyTreeLibraryFromFile</b> or
	<b>LoadFamilyTreeLibraryFromStream</b> properties. This will also enable the ribbon's
	<b>File -> Import -> GEDCOM</b> menu item and add the GEDCOM drawing format to the
	Open file dialog of the drawing view.
</p>
<p>
	This example imports the family tree of the US president Abraham Lincoln.
</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
			// Import a GEDCOM file from a resource
			m_DrawingView.LoadFromResourceAsync(NResources.RSTR_LincolnFamily_ged, NDrawingFormat.Gedcom);

			// Hide the ports
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