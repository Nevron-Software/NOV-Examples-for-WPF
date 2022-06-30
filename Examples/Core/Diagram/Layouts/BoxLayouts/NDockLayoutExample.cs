using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
    public class NDockLayoutExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NDockLayoutExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NDockLayoutExample()
        {
            NDockLayoutExampleSchema = NSchema.Create(typeof(NDockLayoutExample), NExampleBaseSchema);
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
    The dock layout is a space eating cells layout, which places vertices at per-vertex specified docking areas of the currently available layout area.
</p>
<p>
	The most important properties of this layout are:
	<ul>
		<li>
		    <b>HorizontalContentPlacement and VerticalContentPlacement</b> - determine the default placement
		        of the cell content in regards to the X or the Y dimension of the cell bounds.
		</li>
		<li>
		    <b>HorizontalSpacing and VerticalSpacing</b> - determine the minimal spacing between 2 cells in
		        horizontal and vertical direction respectively.
		</li>
		<li>
			<b>FillMode and FitMode</b> - when the size of the content is smaller than the container size 
			the FillMode property is taken into account. If the content size is greater than the container,
			then the layout takes the value of the FitMode into account. Possible values are:
			<ul>
			    <li>None - the dock layout does not attempt to resolve the available/insufficient area problem</li>
			    <li>Equal - the dock inflates/deflates the size of each object with equal amount of space in order
			        to resolve the available/insufficient area problem</li>
			    <li>CenterFirst - the dock inflates/deflates the size of the center object in the dock, then the size of the
			        pair formed by the previous and the next one and so on until the available/insufficient area
			        problem is resolved</li>
			    <li>SidesFirst - the dock inflates/deflates the size of the pair formed by the first and the last
			        object in the dock, then the size of the pair formed by the next and the previous one and so
			        on until the available/insufficient area problem is resolved</li>
			    <li>ForwardOrder - the bodies are resized in the order they were added</li>
			    <li>ReverseOrder - the bodies are resized in reverse order to the order they were added</li>
			</ul>
			In all cases the minimal and maximal size constraints of each shape are not broken, so it is possible
			the dock cannot resolve the available/insufficient area problem completely.
		</li>
	</ul>
</p>
<p>
	To experiment with the layout just change the properties of the layout in the property grid and click the <b>Layout</b> button.
</p>
            ";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Hide ports
            drawingDocument.Content.ScreenVisibility.ShowPorts = false;

            NPage activePage = drawingDocument.Content.ActivePage;
            NBasicShapeFactory basicShapes = new NBasicShapeFactory();

            int min = 100;
            int max = 200;

            NShape shape;
            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                shape = basicShapes.CreateShape(ENBasicShape.Rectangle);

                NColor[] shapeLightColors = new NColor[] {
                                                    new NColor(236, 97, 49),
                                                    new NColor(247, 150, 56),
                                                    new NColor(68, 90, 108),
                                                    new NColor(129, 133, 133),
                                                    new NColor(255, 165, 109)};

                NColor[] shapeDarkColors = new NColor[] {
                                                    new NColor(246, 176, 152),
                                                    new NColor(251, 203, 156),
                                                    new NColor(162, 173, 182),
                                                    new NColor(192, 194, 194),
                                                    new NColor(255, 210, 182)};

                shape.Geometry.Fill = new NStockGradientFill(ENGradientStyle.Horizontal, ENGradientVariant.Variant3, shapeLightColors[i], shapeDarkColors[i]);
                shape.Geometry.Stroke = new NStroke(1, new NColor(68, 90, 108));


                // Generate random width and height
                float width = random.Next(min, max);
                float height = random.Next(min, max);

                switch (i)
                {
                    case 0:
                        shape.LayoutData.DockArea = ENDockArea.Top;
                        shape.Text = "Top (" + i.ToString() + ")";
                        break;

                    case 1:
                        shape.LayoutData.DockArea = ENDockArea.Bottom;
                        shape.Text = "Bottom (" + i.ToString() + ")";
                        break;

                    case 2:
                        shape.LayoutData.DockArea = ENDockArea.Left;
                        shape.Text = "Left (" + i.ToString() + ")";
                        break;

                    case 3:
                        shape.LayoutData.DockArea = ENDockArea.Right;
                        shape.Text = "Right (" + i.ToString() + ")";
                        break;

                    case 4:
                        shape.LayoutData.DockArea = ENDockArea.Center;
                        shape.Text = "Center (" + i.ToString() + ")";
                        break;
                }

                shape.SetBounds(new NRectangle(0, 0, width, height));
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
        private NDockLayout m_Layout = new NDockLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NDockLayoutExample.
        /// </summary>
        public static readonly NSchema NDockLayoutExampleSchema;

        #endregion
    }
}