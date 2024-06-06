using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Editors;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Diagram.UI;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NLayersExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NLayersExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NLayersExample()
        {
            NLayersExampleSchema = NSchema.Create(typeof(NLayersExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a library browser that displays all predefined shape factories
            NLibraryBrowser libraryBrowser = new NLibraryBrowser();
            libraryBrowser.LibraryViewType = ENLibraryViewType.Thumbnails;

            //NLibrary library = new NBasicShapeFactory().CreateLibrary();
            //libraryBrowser.AddLibrarySection(library);

            // create pan and zoom
            NPanAndZoomView m_PanAndZoom = new NPanAndZoomView();
            m_PanAndZoom.PreferredSize = new NSize(150, 150);

            // create a drawing view
            m_DrawingView = new NDrawingView();
            m_DrawingView.HorizontalPlacement = ENHorizontalPlacement.Fit;
            m_DrawingView.VerticalPlacement = ENVerticalPlacement.Fit;

            // bind components to drawing view
            libraryBrowser.DrawingView = m_DrawingView;
            libraryBrowser.ResetLibraries();
            m_PanAndZoom.DrawingView = m_DrawingView;

            // create splitters
            NSplitter libraryPanSplitter = new NSplitter();
            libraryPanSplitter.Orientation = ENHVOrientation.Vertical;
            libraryPanSplitter.SplitMode = ENSplitterSplitMode.OffsetFromFarSide;
            libraryPanSplitter.Pane1.Content = libraryBrowser;
            libraryPanSplitter.Pane2.Content = m_PanAndZoom;

            NSplitter mainSplitter = new NSplitter();
            mainSplitter.Orientation = ENHVOrientation.Horizontal;
            mainSplitter.SplitMode = ENSplitterSplitMode.OffsetFromNearSide;
            mainSplitter.SplitOffset = 370;
            mainSplitter.Pane1.Content = libraryPanSplitter;
            mainSplitter.Pane2.Content = m_DrawingView;

            // Create the ribbon UI
            NDiagramRibbonBuilder builder = new NDiagramRibbonBuilder();

            m_DrawingView.Document.HistoryService.Pause();
            try
            {
                InitDiagram(m_DrawingView.Document);
            }
            finally
            {
                m_DrawingView.Document.HistoryService.Resume();
            }

            return builder.CreateUI(mainSplitter, m_DrawingView);
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stackPanel = new NStackPanel();

            NDesigner designer = NDesigner.GetDesigner(m_DrawingView.Content.Pages[0].Layers);
            NGeneralEditor editor = designer.CreateInstanceEditor(m_DrawingView.Content.Pages[0].Layers);
            stackPanel.Add(editor);

            return stackPanel;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>Demonstrates page layers.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

            // create several layers
            NLayer redLayer = new NLayer();
            redLayer.Color = NColor.Red;
            redLayer.Name = "Red";
            drawing.ActivePage.Layers.Add(redLayer);

            NLayer greenLayer = new NLayer();
            greenLayer.Color = NColor.Green;
            greenLayer.Name = "Green";
            drawing.ActivePage.Layers.Add(greenLayer);

            NLayer blueLayer = new NLayer();
            blueLayer.Name = "Blue";
            blueLayer.Color = NColor.Blue;
            drawing.ActivePage.Layers.Add(blueLayer);

            // create several shapes, each assigned to different layer(s)
            NBasicShapeFactory shapeFactory = new NBasicShapeFactory();

            NShape redShape = shapeFactory.CreateShape(ENBasicShape.Rectangle);
            redShape.Text = "Red";
            redShape.Geometry.Fill = new NColorFill(NColor.Red);
            redShape.SetBounds(10, 10, 100, 100);
            redShape.AssignToLayers(redLayer);
            drawing.ActivePage.Items.Add(redShape);

            NShape greenShape = shapeFactory.CreateShape(ENBasicShape.Rectangle);
            greenShape.Text = "Green";
            greenShape.Geometry.Fill = new NColorFill(NColor.Green);
            greenShape.SetBounds(120, 10, 100, 100);
            greenShape.AssignToLayers(greenLayer);
            drawing.ActivePage.Items.Add(greenShape);

            NShape blueShape = shapeFactory.CreateShape(ENBasicShape.Rectangle);
            blueShape.Text = "Blue";
            blueShape.Geometry.Fill = new NColorFill(NColor.Blue);
            blueShape.SetBounds(230, 10, 100, 100);
            blueShape.AssignToLayers(blueLayer);
            drawing.ActivePage.Items.Add(blueShape);

            NShape grayShape = shapeFactory.CreateShape(ENBasicShape.Rectangle);
            grayShape.Text = "Red, Green and Blue";
            grayShape.Geometry.Fill = new NColorFill(NColor.Gray);
            grayShape.SetBounds(10, 120, 320, 100);
            grayShape.AssignToLayers(
                redLayer,
                greenLayer,
                blueLayer);
            drawing.ActivePage.Items.Add(grayShape);
        }

        #endregion

      
        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NLayersExample.
        /// </summary>
        public static readonly NSchema NLayersExampleSchema;

        #endregion
    }
}