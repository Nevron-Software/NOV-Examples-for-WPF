using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NStackLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NStackLayoutExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NStackLayoutExample()
        {
            NStackLayoutExampleSchema = NSchema.Create(typeof(NStackLayoutExample), NExampleBaseSchema);
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
            m_Layout.Changed += OnLayoutChanged;

            NStackPanel stack = new NStackPanel();

            // property editor
            NEditor editor = NDesigner.GetDesigner(m_Layout).CreateInstanceEditor(m_Layout);
            stack.Add(new NGroupBox("Properties", editor));

            NButton arrangeButton = new NButton("Arrange Diagram");
            arrangeButton.Click += OnArrangeButtonClick;
            stack.Add(arrangeButton);

            // items stack
            NStackPanel itemsStack = new NStackPanel();

            // NOTE: For Cells layout we provide the user with the ability to add shapes with different sizes so that he/she can test the layouts
            NButton addSmallItemButton = new NButton("Add Small Shape");
            addSmallItemButton.Click += new Function<NEventArgs>(OnAddSmallItemButtonClick);
            itemsStack.Add(addSmallItemButton);

            NButton addLargeItemButton = new NButton("Add Large Shape");
            addLargeItemButton.Click += new Function<NEventArgs>(OnAddLargeItemButtonClick);
            itemsStack.Add(addLargeItemButton);

            NButton addRandomItemButton = new NButton("Add Random Shape");
            addRandomItemButton.Click += new Function<NEventArgs>(OnAddRandomItemButtonClick);
            itemsStack.Add(addRandomItemButton);

            NButton removeAllItemsButton = new NButton("Remove All Shapes");
            removeAllItemsButton.Click += new Function<NEventArgs>(OnRemoveAllItemsButtonClick);
            itemsStack.Add(removeAllItemsButton);

            stack.Add(new NGroupBox("Items", itemsStack));

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    The stack layout is a directed constrained cells layout, which stacks the cells in horizontal or vertical order.
    Depending on the layout direction the layout is constrained by either width or height.
</p>
<p>
	The most important properties of this layout are:
	<ul>
		<li>
			<b>Direction</b> - determines the direction in which the layout arranges adjacent cells.
		</li>
		<li>
		    <b>HorizontalContentPlacement and VerticalContentPlacement</b> - determine the default placement
		        of the cell content in regards to the X or the Y dimension of the cell bounds.
		</li>
		<li>
		    <b>HorizontalSpacing and VerticalSpacing</b> - determine the minimal spacing between 2 cells in
		        horizontal and vertical direction respectively.
		</li>
		<li>
			<b>FillMode</b> - when the size of the content is smaller than the container size 
			the FillMode property is taken into account. Possible values are:
			<ul>
			    <li>None - no filling is performed</li>
			    <li>Equal - all shapes are equally inflated</li>
			    <li>Proportional - shapes are inflated proportionally to their size</li>
			    <li>First - shapes are inflated in forward order until content fills the area</li>
			    <li>Last - shapes are inflated in reverse order until content fills the area</li>
			</ul>
			In all cases the maximal size constraints of each shape are not broken.
		</li>
		<li>
			<b>FitMode</b> - when the size of the content is larger than the container size 
			the FitMode property is taken into account. Possible values are:
			<ul>
			    <li>None - no fitting is performed</li>
			    <li>Equal - all shapes are equally deflated</li>
			    <li>Proportional - shapes are deflated proportionally to their size</li>
			    <li>First - shapes are deflated in forward order until content fits the area</li>
			    <li>Last - shapes are deflated in reverse order until content fits the area</li>
			</ul>
			In all cases the minimal size constraints of each shape are not broken.
		</li>
	</ul>
</p>
<p>
	To experiment with their behavior just change the properties of the layout in the property
	grid and click the button <b>Layout</b> button. 
</p>		
<p>	
	Change the drawing width and height to see how the layout behaves with a different layout area.
</p>
            ";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;

            // Create some shapes
            NPage activePage = drawingDocument.Content.ActivePage;
            NBasicShapeFactory basicShapes = new NBasicShapeFactory();

            for (int i = 0; i < 20; i++)
            {
                NShape shape = basicShapes.CreateShape(ENBasicShape.Rectangle);
                activePage.Items.Add(shape);
            }

            // Arrange diagram
            ArrangeDiagram(drawingDocument);

            // Fit page
            drawingDocument.Content.ActivePage.ZoomMode = ENZoomMode.Fit;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Arranges the shapes in the active page.
        /// </summary>
        /// <param name="drawingDocument"></param>
        private void ArrangeDiagram(NDrawingDocument drawingDocument)
        {
            // get all top-level shapes that reside in the active page
            NPage activePage = drawingDocument.Content.ActivePage;
            NList<NShape> shapes = activePage.GetShapes(false);

            // create a layout context and use it to arrange the shapes using the current layout
            NDrawingLayoutContext layoutContext = new NDrawingLayoutContext(drawingDocument, activePage);
            m_Layout.Arrange(shapes.CastAll<object>(), layoutContext);

            // size the page to the content size
            activePage.SizeToContent();
        }
        private NShape CreateShape()
        {
            NBasicShapeFactory factory = new NBasicShapeFactory();
            return factory.CreateShape(ENBasicShape.Rectangle);
        }
        private void OnAddSmallItemButtonClick(NEventArgs args)
        {
            NShape shape = CreateShape();
            shape.Width = 25;
            shape.Height = 25;

            m_DrawingView.ActivePage.Items.Add(shape);
            ArrangeDiagram(m_DrawingView.Document);
        }
        private void OnAddLargeItemButtonClick(NEventArgs args)
        {
            NShape shape = CreateShape();
            shape.Width = 60;
            shape.Height = 60;

            m_DrawingView.ActivePage.Items.Add(shape);
            ArrangeDiagram(m_DrawingView.Document);
        }
        private void OnAddRandomItemButtonClick(NEventArgs args)
        {
            int range = 30;
            Random rnd = new Random();

            NShape shape = CreateShape();
            shape.Width = rnd.Next(range) + range;
            shape.Height = rnd.Next(range) + range;

            m_DrawingView.ActivePage.Items.Add(shape);
            ArrangeDiagram(m_DrawingView.Document);
        }
        private void OnRemoveAllItemsButtonClick(NEventArgs args)
        {
            m_DrawingView.Document.StartHistoryTransaction("Remove All Items");
            try
            {
                m_DrawingView.Document.Content.ActivePage.Items.Clear();
            }
            finally
            {
                m_DrawingView.Document.CommitHistoryTransaction();
            }
        }

        #endregion

        #region Event Handlers

        private void OnLayoutChanged(NEventArgs arg)
        {
            ArrangeDiagram(m_DrawingView.Document);
        }
        protected virtual void OnArrangeButtonClick(NEventArgs arg)
        {
            ArrangeDiagram(m_DrawingView.Document);
        }

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;
        private NStackLayout m_Layout = new NStackLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NStackLayoutExample.
        /// </summary>
        public static readonly NSchema NStackLayoutExampleSchema;

        #endregion
    }
}