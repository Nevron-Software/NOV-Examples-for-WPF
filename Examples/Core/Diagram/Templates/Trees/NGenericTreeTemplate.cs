using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;

namespace Nevron.Nov.Examples.Diagram
{
    /// <summary>
    /// The NGenericTreeTemplate class represents a generic tree template
    /// </summary>
    public class NGenericTreeTemplate : NGraphTemplate
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NGenericTreeTemplate()
            : base("Generic Tree")
        {
            m_nLevels = 3;
            m_nBranchNodes = 2;
            m_nVertexSizeDeviation = 0.0f;
            m_bBalanced = true;
            m_LayoutDirection = ENHVDirection.TopToBottom;
            m_LayoutType = ENTreeLayoutType.Layered;
            m_ConnectorShape = ENConnectorShape.RoutableConnector;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the count of the child nodes for each branch
        /// </summary>
        /// <remarks>
        /// By default set to 2
        /// </remarks>
        public int BranchNodes
        {
            get
            {
                return m_nBranchNodes;
            }
            set
            {
                if (value == m_nBranchNodes)
                    return;

                if (value < 1)
                    throw new ArgumentOutOfRangeException("The value must be > 0.");

                m_nBranchNodes = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets the tree levels count
        /// </summary>
        /// <remarks>
        /// By default set to 3 
        /// </remarks>
        public int Levels
        {
            get
            {
                return m_nLevels;
            }
            set
            {
                if (value == m_nLevels)
                    return;

                if (value < 1)
                    throw new ArgumentOutOfRangeException("The value must be > 0.");

                m_nLevels = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets if the tree is balanced or not
        /// </summary>
        /// <remarks>
        /// By default set to true 
        /// </remarks>
        public bool Balanced
        {
            get
            {
                return m_bBalanced;
            }
            set
            {
                if (m_bBalanced != value)
                {
                    m_bBalanced = value;
                    OnTemplateChanged();
                }
            }
        }
        /// <summary>
        /// Specifies the tree layout expand
        /// </summary>
        /// <remarks>
        /// By default set to TopToBottom
        /// </remarks>
        public ENHVDirection LayoutDirection
        {
            get
            {
                return m_LayoutDirection;
            }
            set
            {
                if (value == m_LayoutDirection)
                    return;

                m_LayoutDirection = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Specifies the tree layout scheme
        /// </summary>
        /// <remarks>
        /// By default set to NormalCompressed
        /// </remarks>
        public ENTreeLayoutType LayoutType
        {
            get
            {
                return m_LayoutType;
            }
            set
            {
                if (value == m_LayoutType)
                    return;

                m_LayoutType = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Specifies the edge connector type
        /// </summary>
        /// <remarks>
        /// By default set to DynamicPolyline
        /// </remarks>
        public ENConnectorShape ConnectorType
        {
            get
            {
                return m_ConnectorShape;
            }
            set
            {
                if (m_ConnectorShape != value)
                {
                    m_ConnectorShape = value;
                    OnTemplateChanged();
                }
            }
        }
        /// <summary>
        /// Specifies the deviation of the vertex size according to the VerticesSize (0 for none)
        /// </summary>
        /// <remarks>
        /// By default set to 0
        /// </remarks>
        public double VertexSizeDeviation
        {
            get
            {
                return m_nVertexSizeDeviation;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Negative values are not allowed.");

                if (m_nVertexSizeDeviation != value)
                {
                    m_nVertexSizeDeviation = value;
                    OnTemplateChanged();
                }
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overriden to return the tree description
        /// </summary>
        public override string GetDescription()
        {
            string description = String.Format("##Tree with {0} levels and {1} branch elements.", m_nLevels, m_nBranchNodes);
            return description;
        }

        #endregion

        #region Protected overrides

        /// <summary>
        /// Overriden to create the tree template
        /// </summary>
        /// <param name="document">document in which to create the template</param>
        protected override void CreateTemplate(NDrawingDocument document)
        {
            // create the tree
            NList<NShape> elements = CreateTree(document);
            if (m_LayoutType == ENTreeLayoutType.None)
                return;

            // layout the tree
            NLayeredTreeLayout layout = new NLayeredTreeLayout();

            // sync measurement unit 
            layout.OrthogonalEdgeRouting = false;

            switch (m_LayoutType)
            {
                case ENTreeLayoutType.Layered:
                    layout.VertexSpacing = m_fHorizontalSpacing;
                    layout.LayerSpacing = m_fVerticalSpacing;
                    break;

                case ENTreeLayoutType.LayeredLeftAligned:
                    layout.VertexSpacing = m_fHorizontalSpacing;
                    layout.LayerSpacing = m_fVerticalSpacing;
                    layout.ParentPlacement.Anchor = ENParentAnchor.SubtreeNear;
                    layout.ParentPlacement.Alignment = ENRelativeAlignment.Near;
                    break;

                case ENTreeLayoutType.LayeredRightAligned:
                    layout.VertexSpacing = m_fHorizontalSpacing;
                    layout.LayerSpacing = m_fVerticalSpacing;
                    layout.ParentPlacement.Anchor = ENParentAnchor.SubtreeFar;
                    layout.ParentPlacement.Alignment = ENRelativeAlignment.Far;
                    break;
            }

            // apply layout
            NDrawingLayoutContext context = new NDrawingLayoutContext(document, new NRectangle(Origin, NSize.Zero));
            layout.Arrange(elements.CastAll<object>(), context);
        }

        #endregion

        #region Protected overridable

        /// <summary>
        /// Gets the size for a new vertex taking into account the VertexSizeDeviation property.
        /// </summary>
        /// <returns></returns>
        protected virtual NSize GetSize(Random rnd)
        {
            if (m_nVertexSizeDeviation == 0)
                return m_VertexSize;

            double factor = m_nVertexSizeDeviation + 1;

            double width = rnd.Next((int)(m_VertexSize.Width / factor), (int)(m_VertexSize.Width * factor));
            double height = rnd.Next((int)(m_VertexSize.Height / factor), (int)(m_VertexSize.Height * factor));

            return new NSize(width, height);
        }
        /// <summary>
        /// Creates a tree in the specified document
        /// </summary>
        /// <param name="document">document in which to create a tree</param>
        /// <returns>tree elements</returns>
        protected virtual NList<NShape> CreateTree(NDrawingDocument document)
        {
            NPage page = document.Content.ActivePage;
            NList<NShape> elements = new NList<NShape>();

            NShape cur = null;
            NShape edge = null;

            NList<NShape> curRowVertices = null;
            NList<NShape> prevRowVertices = null;

            int i, j, level;
            int childrenCount, levelNodesCount;
            Random rnd = new Random();

            for (level = 1; level <= m_nLevels; level++)
            {
                curRowVertices = new NList<NShape>();

                if (m_bBalanced)
                {
                    //Create a balanced tree
                    levelNodesCount = (int)Math.Pow(m_nBranchNodes, level - 1);
                    for (i = 0; i < levelNodesCount; i++)
                    {
                        // create the cur node
                        cur = CreateVertex(m_VertexShape);
                        cur.SetBounds(new NRectangle(m_Origin, GetSize(rnd)));

                        page.Items.AddChild(cur);
                        elements.Add(cur);

                        // connect with ancestor
                        if (level > 1)
                        {
                            edge = CreateEdge(m_ConnectorShape);
                            page.Items.AddChild(edge);

                            int parentIndex = (int)Math.Floor((double)(i / m_nBranchNodes));
                            edge.GlueBeginToGeometryIntersection(prevRowVertices[parentIndex]);
                            edge.GlueEndToShape(cur);
                        }

                        curRowVertices.Add(cur);
                    }
                }
                else
                {
                    //Create an unbalanced tree
                    if (level == 1)
                    {
                        // Create the current node
                        cur = CreateVertex(m_VertexShape);
                        cur.SetBounds(new NRectangle(m_Origin, GetSize(rnd)));

                        page.Items.AddChild(cur);
                        elements.Add(cur);

                        curRowVertices.Add(cur);
                    }
                    else
                    {
                        levelNodesCount = prevRowVertices.Count;
                        do
                        {
                            // Ensure that the desired level depth is reached
                            for (i = 0; i < levelNodesCount; i++)
                            {
                                childrenCount = rnd.Next(0, m_nBranchNodes + 1);
                                for (j = 0; j < childrenCount; j++)
                                {
                                    // Create the current node
                                    cur = CreateVertex(m_VertexShape);
                                    cur.SetBounds(new NRectangle(m_Origin, GetSize(rnd)));

                                    page.Items.AddChild(cur);
                                    elements.Add(cur);
                                    curRowVertices.Add(cur);

                                    // Connect with ancestor
                                    edge = CreateEdge(m_ConnectorShape);
                                    page.Items.AddChild(edge);

                                    edge.GlueBeginToGeometryIntersection(prevRowVertices[i]);
                                    edge.GlueEndToShape(cur);
                                }
                            }
                        }
                        while (level < m_nLevels && curRowVertices.Count == 0);
                    }
                }

                prevRowVertices = curRowVertices;
            }

            return elements;
        }

        #endregion

        #region Fields

        internal int m_nLevels;
        internal int m_nBranchNodes;
        internal bool m_bBalanced;
        internal ENHVDirection m_LayoutDirection;
        internal ENTreeLayoutType m_LayoutType;
        internal ENConnectorShape m_ConnectorShape;
        internal double m_nVertexSizeDeviation;

        #endregion
    }
}