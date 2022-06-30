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
    /// <summary>
    /// Serves as base class for all examples that demonstrates NOV Diagram Layouts
    /// </summary>
    /// <typeparam name="TLayout"></typeparam>
    public abstract class NLayoutExampleBase<TLayout> : NDiagramExampleBase
        where TLayout : NLayout, new()
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NLayoutExampleBase()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NLayoutExampleBase()
        {
            NLayoutExampleBaseSchema = NSchema.Create(typeof(NLayoutExampleBase<TLayout>), NDiagramExampleBase.NDiagramExampleBaseSchema);
        }

        #endregion

        #region Protected Overrides - CreateDiagram, CreateExampleControls

        protected override void InitDiagram()
        {
            base.InitDiagram();

            // hide ports and grid
            NDrawing drawing = m_DrawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            drawing.ScreenVisibility.ShowPorts = false;
            drawing.ScreenVisibility.ShowGrid = false;
        }
        protected override NWidget CreateExampleControls()
        {
            m_Layout.Changed += OnLayoutChanged;

			NStackPanel stack = (NStackPanel)base.CreateExampleControls();

            // property editor
            NEditor editor = NDesigner.GetDesigner(m_Layout).CreateInstanceEditor(m_Layout);
            stack.Add(new NGroupBox("Properties", editor));

            NButton arrangeButton = new NButton("Arrange Diagram");
            arrangeButton.Click += OnArrangeButtonClick;
            stack.Add(arrangeButton);

            // items stack
            NStackPanel itemsStack = new NStackPanel();
            if ((m_Layout is NBoxLayout) && !(m_Layout is NDockLayout))
            {
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
            }
            else if (m_Layout is NTreeLayout)
            {
                // NOTE: For Tree layouts we provide the user with the ability to generate random tree diagrams so that he/she can test the layouts
                NButton randomTree1Button = new NButton("Random Tree (max 6 levels, max 3 branch nodes)");
                randomTree1Button.Click += OnRandomTree1ButtonClick;
                itemsStack.Add(randomTree1Button);

                NButton randomTree2Button = new NButton("Random Tree (max 8 levels, max 2 branch nodes)");
                randomTree2Button.Click += OnRandomTree2ButtonClick;
                itemsStack.Add(randomTree2Button);
            }
            else if ((m_Layout is NGraphLayout) && !(m_Layout is NBarycenterGraphLayout)) 
            {
                // NOTE: For Graph layouts we provide the user with the ability to generate random graph diagrams so that he/she can test the layouts
                NButton randomGraph1Button = new NButton("Random Graph (10 vertices, 15 edges)");
                randomGraph1Button.Click += OnRandomGraph1ButtonClick;
                itemsStack.Add(randomGraph1Button);

                NButton randomGraph2Button = new NButton("Random Graph (20 vertices, 30 edges)");
                randomGraph2Button.Click += OnRandomGraph2ButtonClick;
                itemsStack.Add(randomGraph2Button);
            }
            else
            {
                // NOTE: Not any of the predefined cases -> create custom controls
                CreateItemsControls(itemsStack);
            }

            stack.Add(new NGroupBox("Items", itemsStack));

            return stack;
        }

        #endregion

        #region Protected Overridable - PerformLayout

        /// <summary>
        /// Arranges the shapes in the active page.
        /// </summary>
        protected virtual void ArrangeDiagram()
        {
            // get all top-level shapes that reside in the active page
            NPage activePage = m_DrawingDocument.Content.ActivePage;
            NList<NShape> shapes = activePage.GetShapes(false);
            
            // create a layout context and use it to arrange the shapes using the current layout
            NDrawingLayoutContext layoutContext = new NDrawingLayoutContext(m_DrawingDocument, activePage);
            m_Layout.Arrange(shapes.CastAll<object>(), layoutContext);

            // size the page to the content size
            activePage.SizeToContent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stack"></param>
        protected virtual void CreateItemsControls(NStackPanel stack)
        {
        }

        #endregion

        #region Event Handlers

        protected virtual NShape CreateShape()
        {
            NBasicShapeFactory factory = new NBasicShapeFactory();
            return factory.CreateShape(ENBasicShape.Rectangle);
        }
        protected virtual void OnLayoutChanged(NEventArgs arg)
        {
            ArrangeDiagram();
        }

        protected virtual void OnAddSmallItemButtonClick(NEventArgs args)
        {
            NShape item = CreateShape();
            item.Width = 25;
            item.Height = 25;
            m_DrawingDocument.Content.ActivePage.Items.Add(item);
            ArrangeDiagram();
        }
        protected virtual void OnAddLargeItemButtonClick(NEventArgs args)
        {
            NShape item = CreateShape();
            item.Width = 60;
            item.Height = 60;
            m_DrawingDocument.Content.ActivePage.Items.Add(item);
            ArrangeDiagram();
        }
        protected virtual void OnAddRandomItemButtonClick(NEventArgs args)
        {
            int range = 30;
            Random rnd = new Random();
            
            NShape item = CreateShape();
            item.Width = rnd.Next(range) + range;
            item.Height = rnd.Next(range) + range;
            m_DrawingDocument.Content.ActivePage.Items.Add(item);

            ArrangeDiagram();
        }
        protected virtual void OnRemoveAllItemsButtonClick(NEventArgs args)
        {
            m_DrawingDocument.StartHistoryTransaction("Remove All Items");
            try
            {
                m_DrawingDocument.Content.ActivePage.Items.Clear();
            }
            finally
            {
                m_DrawingDocument.CommitHistoryTransaction();
            }
        }

        protected virtual void OnRandomTree1ButtonClick(NEventArgs arg)
        {
            m_DrawingDocument.StartHistoryTransaction("Create Random Tree 1");
            try
            {
                m_DrawingDocument.Content.ActivePage.Items.Clear();

                // create a random tree
                NGenericTreeTemplate tree = new NGenericTreeTemplate();
                tree.EdgesUserClass = "Connector";
                tree.Levels = 6;
                tree.BranchNodes = 3;
                tree.HorizontalSpacing = 10;
                tree.VerticalSpacing = 10;
                tree.VerticesShape = GetDefaultShapeType();
                tree.VerticesSize = GetDefaultShapeSize();

				if (m_Layout is NBalloonTreeLayout)
				{
					tree.Balanced = true;
					tree.VertexSizeDeviation = 0;
				}
				else
				{
					tree.Balanced = false;
					tree.VertexSizeDeviation = 1;
				}

                tree.Create(m_DrawingDocument);

                // layout the tree
                ArrangeDiagram();
            }
            finally
            {
                m_DrawingDocument.CommitHistoryTransaction();
            }
        }
        protected virtual void OnRandomTree2ButtonClick(NEventArgs arg)
        {
            m_DrawingDocument.StartHistoryTransaction("Create Random Tree 2");
            try
            {
                m_DrawingDocument.Content.ActivePage.Items.Clear();

                // create a random tree
                NGenericTreeTemplate tree = new NGenericTreeTemplate();
                tree.EdgesUserClass = "Connector";
                tree.Levels = 8;
                tree.BranchNodes = 2;
                tree.HorizontalSpacing = 10;
                tree.VerticalSpacing = 10;
                tree.VerticesShape = GetDefaultShapeType();
                tree.VerticesSize = GetDefaultShapeSize();

				if (m_Layout is NBalloonTreeLayout)
				{
					tree.Balanced = true;
					tree.VertexSizeDeviation = 0;
				}
				else
				{
					tree.Balanced = false;
					tree.VertexSizeDeviation = 1;
				}
				
				tree.Create(m_DrawingDocument);

                // layout the tree
                ArrangeDiagram();
            }
            finally
            {
                m_DrawingDocument.CommitHistoryTransaction();
            }
        }

        protected virtual void OnRandomGraph1ButtonClick(NEventArgs arg)
        {
            m_DrawingDocument.StartHistoryTransaction("Create Random Graph 1");
            try
            {
                m_DrawingDocument.Content.ActivePage.Items.Clear();

                // create a test tree
                NRandomGraphTemplate graph = new NRandomGraphTemplate();
                graph.EdgesUserClass = "Connector";
                graph.VertexCount = 10;
                graph.EdgeCount = 15;
                graph.VerticesShape = GetDefaultShapeType();
                graph.VerticesSize = GetDefaultShapeSize();
                graph.Create(m_DrawingDocument);

                // layout the tree
                ArrangeDiagram();
            }
            finally
            {
                m_DrawingDocument.CommitHistoryTransaction();
            }
        }
        protected virtual void OnRandomGraph2ButtonClick(NEventArgs arg)
        {
            m_DrawingDocument.StartHistoryTransaction("Create Random Graph 2");
            try
            {
                m_DrawingDocument.Content.ActivePage.Items.Clear();

                // create a test tree
                NRandomGraphTemplate graph = new NRandomGraphTemplate();
                graph.EdgesUserClass = "Connector";
                graph.VertexCount = 20;
                graph.EdgeCount = 30;
                graph.VerticesShape = GetDefaultShapeType();
                graph.VerticesSize = GetDefaultShapeSize();
                graph.Create(m_DrawingDocument);

                // layout the tree
                ArrangeDiagram();
            }
            finally
            {
                m_DrawingDocument.CommitHistoryTransaction();
            }
        }

        protected virtual void OnArrangeButtonClick(NEventArgs arg)
        {
            ArrangeDiagram();
        }

        protected virtual NSize GetDefaultShapeSize()
        {
            return new NSize(50, 50);
        }
        protected virtual ENBasicShape GetDefaultShapeType()
        {
            return ENBasicShape.Rectangle;
        }

        #endregion

        #region Fields

        protected TLayout m_Layout = new TLayout();

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NLayoutExampleBase.
        /// </summary>
        public static readonly NSchema NLayoutExampleBaseSchema;

        #endregion
    }
}