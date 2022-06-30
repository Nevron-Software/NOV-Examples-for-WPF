using System;
using System.Collections.Generic;

using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.Diagram
{
    /// <summary>
    /// Generates a connnected random graph.
    /// </summary>
    public class NRandomGraphTemplate : NGraphTemplate
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NRandomGraphTemplate()
            : base("Random Graph")
        {
            m_nVertexCount = 10;
            m_nEdgeCount = 15;
            m_MinVerticesSize = m_VerticesSize;
            m_MaxVerticesSize = m_VerticesSize;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The number of edges to generate.
        /// </summary>
        public int EdgeCount
        {
            get
            {
                return m_nEdgeCount;
            }
            set
            {
                if (m_nEdgeCount != value)
                {
                    m_nEdgeCount = value;
                    OnTemplateChanged();
                }
            }
        }

        /// <summary>
        /// The number of vertices to generate.
        /// </summary>
        public int VertexCount
        {
            get
            {
                return m_nVertexCount;
            }
            set
            {
                if (m_nVertexCount != value)
                {
                    m_nVertexCount = value;
                    OnTemplateChanged();
                }
            }
        }

        /// <summary>
        /// The minimal size of a vertex in the graph.
        /// </summary>
        public NSize MinVerticesSize
        {
            get
            {
                return m_MinVerticesSize;
            }
            set
            {
                if (m_MinVerticesSize != value)
                {
                    if (value.Width > m_MaxVerticesSize.Width || value.Height > m_MaxVerticesSize.Height)
                    {
                        throw new ArgumentException("MinVerticesSize must be smaller than or equal to MaxVertexSize");
                    }

                    m_MinVerticesSize = value;
                    OnTemplateChanged();
                }
            }
        }

        /// <summary>
        /// The maximal size of a vertex in the graph.
        /// </summary>
        public NSize MaxVerticesSize
        {
            get
            {
                return m_MaxVerticesSize;
            }
            set
            {
                if (m_MaxVerticesSize != value)
                {
                    if (value.Width < m_MinVerticesSize.Width || value.Height < m_MinVerticesSize.Height)
                    {
                        throw new ArgumentException("MexVerticesSize must be greater than or equal to MinVerticesSize");
                    }

                    m_MaxVerticesSize = value;
                    OnTemplateChanged();
                }
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Overriden to provide a description of the template.
        /// </summary>
        /// <returns></returns>
        public override string GetDescription()
        {
            return string.Format("##Generates a random graph with {0} vertices and {1} edges.", m_nVertexCount, m_nEdgeCount);
        }
        /// <summary>
        /// Overriden to create a random graph template in the specified document.
        /// </summary>
        /// <param name="document">The document to create a graph in.</param>
        protected override void CreateTemplate(NDrawingDocument document)
        {
            if (m_nEdgeCount < m_nVertexCount - 1)
                throw new Exception("##The number of edges must be greater than or equal to the (number of vertices - 1) in order to generate a connected graph");

            if (m_nEdgeCount > MaxEdgeCount(m_nVertexCount))
                throw new Exception("##Too many edges wanted for the graph");

            int i;
            Random random = new Random();
            NPage activePage = document.Content.ActivePage;

            NShape[] vertices = new NShape[m_nVertexCount];
            NList<NPointI> edges = GetRandomMST(m_nVertexCount);
            NPointI edgeInfo;

            NSizeI minSize = m_MinVerticesSize.Round();
            NSizeI maxSize = m_MaxVerticesSize.Round();
            maxSize.Width++;
            maxSize.Height++;

            // Create the vertices
            for (i = 0; i < m_nVertexCount; i++)
            {
                vertices[i] = CreateVertex(m_VerticesShape);
                double width = random.Next(minSize.Width, maxSize.Width);
                double height = random.Next(minSize.Height, maxSize.Height);
                vertices[i].SetBounds(new NRectangle(0, 0, width, height));
                activePage.Items.AddChild(vertices[i]);
            }

            // Generate the edges
            for (i = m_nVertexCount - 1; i < m_nEdgeCount; i++)
            {
                do
                {   // Generate a new edge
                    edgeInfo = new NPointI(random.Next(m_nVertexCount), random.Next(m_nVertexCount));
                }
                while (edgeInfo.X == edgeInfo.Y || edges.Contains(edgeInfo) || edges.Contains(new NPointI(edgeInfo.Y, edgeInfo.X)));
                edges.Add(edgeInfo);
            }

            // Create the edges
            for (i = 0; i < m_nEdgeCount; i++)
            {
                edgeInfo = edges[i];
                NShape edge = CreateEdge(ENConnectorShape.RoutableConnector);
                activePage.Items.AddChild(edge);
                edge.GlueBeginToGeometryIntersection(vertices[edgeInfo.X]);
                edge.GlueEndToShape(vertices[edgeInfo.Y]);
            }

            // Apply a table layout to the generated graph
            NTableFlowLayout tableLayout = new NTableFlowLayout();
            tableLayout.MaxOrdinal = (int)Math.Sqrt(m_nVertexCount) + 1;
            tableLayout.HorizontalSpacing = m_VerticesSize.Width / 5;
            tableLayout.VerticalSpacing = m_VerticesSize.Width / 5;

            NDrawingLayoutContext context = new NDrawingLayoutContext(document, activePage);
            tableLayout.Arrange(new NList<object>(NArrayHelpers<NShape>.CastAll<object>(vertices)), context);
        }

        #endregion

        #region Fields

        private int m_nEdgeCount;
        private int m_nVertexCount;
        private NSize m_MinVerticesSize;
        private NSize m_MaxVerticesSize;

        #endregion

        #region Static

        /// <summary>
        /// Returns the maximum number of edges for the specified number of vertices.
        /// </summary>
        /// <param name="vertexCount"></param>
        /// <returns></returns>
        private static int MaxEdgeCount(int vertexCount)
        {
            return (vertexCount * (vertexCount - 1)) / 2;
        }
        /// <summary>
        /// Creates a random minimum spannig tree (this ensures that the graph will be connected).
        /// </summary>
        /// <returns></returns>
        private static NList<NPointI> GetRandomMST(int vertexCount)
        {
            int i, v1, v2;
            Random random = new Random();
            NList<NPointI> edges = new NList<NPointI>();
            NList<int> usedVertices = new NList<int>();
            NList<int> unusedVertices = new NList<int>(vertexCount);

            for (i = 0; i < vertexCount; i++)
            {
                unusedVertices.Add(i);
            }

            // Determine the root
            v1 = random.Next(vertexCount);
            usedVertices.Add(v1);
            unusedVertices.RemoveAt(v1);

            for (i = 1; i < vertexCount; i++)
            {
                v1 = random.Next(usedVertices.Count);
                v2 = random.Next(unusedVertices.Count);
                edges.Add(new NPointI(usedVertices[v1], unusedVertices[v2]));

                usedVertices.Add(unusedVertices[v2]);
                unusedVertices.RemoveAt(v2);
            }

            return edges;
        }

        #endregion
    }
}
