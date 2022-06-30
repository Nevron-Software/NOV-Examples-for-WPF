using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDiagramDesignerExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NDiagramDesignerExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NDiagramDesignerExample()
        {
            NDiagramDesignerExampleSchema = NSchema.Create(typeof(NDiagramDesignerExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a library browser that displays all predefined shape factories
			m_LibraryBrowser = new NLibraryBrowser();
            m_LibraryBrowser.LibraryViewType = ENLibraryViewType.Thumbnails;

            // create pan and zoom
            m_PanAndZoom = new NPanAndZoomView();
            m_PanAndZoom.PreferredSize = new NSize(150, 150);
            
            // create side bar
            m_SideBar = new NSideBar();

            // create a drawing view
			m_DrawingView = new NDrawingView();
			m_DrawingView.HorizontalPlacement = ENHorizontalPlacement.Fit;
			m_DrawingView.VerticalPlacement = ENVerticalPlacement.Fit;

            // bind components to drawing view
			m_LibraryBrowser.DrawingView = m_DrawingView;
            m_LibraryBrowser.ResetLibraries();
            m_PanAndZoom.DrawingView = m_DrawingView;
            m_SideBar.DrawingView = m_DrawingView;            

            // create splitters
            NSplitter libraryPanSplitter = new NSplitter();
            libraryPanSplitter.Orientation = ENHVOrientation.Vertical;
            libraryPanSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
            libraryPanSplitter.Pane1.Content = m_LibraryBrowser;
            libraryPanSplitter.Pane2.Content = m_PanAndZoom;

            NSplitter leftSplitter = new NSplitter();
            leftSplitter.Orientation = ENHVOrientation.Horizontal;
            leftSplitter.SplitMode = ENSplitterSplitMode.OffsetFromNearSide;
            leftSplitter.SplitOffset = 370;
            leftSplitter.Pane1.Content = libraryPanSplitter;
            leftSplitter.Pane2.Content = m_DrawingView;
            
            NSplitter rightSplitter = new NSplitter();
            rightSplitter.Orientation = ENHVOrientation.Horizontal;
            rightSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
            rightSplitter.SplitOffset = 370;
            rightSplitter.Pane1.Content = leftSplitter;
            rightSplitter.Pane2.Content = m_SideBar;

			// Create the ribbon UI
			NDiagramRibbonBuilder builder = new NDiagramRibbonBuilder();
            return builder.CreateUI(rightSplitter, m_DrawingView);
		}
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return "<p>Demonstrates how to create a Diagram Designer Application</p>";
        }

        #endregion

        #region Fields

        private NLibraryBrowser m_LibraryBrowser;
        private NDrawingView m_DrawingView;
        private NPanAndZoomView m_PanAndZoom;
        private NSideBar m_SideBar;

		#endregion

		#region Schema

		/// <summary>
        /// Schema associated with NDiagramDesignerExample.
        /// </summary>
        public static readonly NSchema NDiagramDesignerExampleSchema;

        #endregion
	}
}