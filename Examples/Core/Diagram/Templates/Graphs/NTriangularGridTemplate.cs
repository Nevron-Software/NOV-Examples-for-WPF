using System;


using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// The NTriangularGridTemplate class represents a triangular grid template
	/// </summary>
	public class NTriangularGridTemplate : NGraphTemplate
    {
        #region Constructors

        /// <summary>
		/// Default constructor
		/// </summary>
		public NTriangularGridTemplate()
			: base("Triangular Grid")
		{
			m_nLevels = 3;
			m_bConnectGrid = true;
        }

        #endregion

        #region Properties

        /// <summary>
		/// Gets or sets the levels count of the triangluar grid
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
		/// Specifies whether the grid vertices are connected
		/// </summary>
		/// <remarks>
		/// By default set to true
		/// </remarks>
		public bool ConnectGrid
		{
			get
			{
				return m_bConnectGrid;
			}
			set
			{
				if (value == m_bConnectGrid)
					return;

				m_bConnectGrid = value;
				OnTemplateChanged();
			}
		}


		#endregion

		#region Overrides

		/// <summary>
		/// Overriden to return the triangular grid description
		/// </summary>
		public override string GetDescription()
		{
			string description = String.Format("##Triangular grid graph with {0} levels.", m_nLevels);

			if (m_bConnectGrid)
				description += " " + "##Each cell has two child cells and is connected with them as well as with the adjacent cells from the same level.";

			return description;
		}

		#endregion

		#region Protected overrides

		/// <summary>
		/// Overriden to create the triangular grid template in the specified document
		/// </summary>
		/// <param name="document">document in which to create the template</param>
		protected override void CreateTemplate(NDrawingDocument document)
		{
			NPage page = document.Content.ActivePage;
            NRectangle templateBounds = new NRectangle( m_Origin.X, m_Origin.Y, 
														m_nLevels * m_VerticesSize.Width + (m_nLevels - 1) * m_fHorizontalSpacing,
														m_nLevels * m_VerticesSize.Height + (m_nLevels - 1) * m_fVerticalSpacing);
			NPoint location;
			NShape cur = null, prev = null;
			NShape edge = null;

			NList<NShape> curRowNodes = null;
            NList<NShape> prevRowNodes = null;

			for (int level = 1; level <= m_nLevels; level++)
			{
				// determine the location of the first node in the level
				location = new NPoint(	templateBounds.X + (templateBounds.Width - level * m_VerticesSize.Width - (level - 1) * m_fHorizontalSpacing) / 2,
										templateBounds.Y + (level - 1) * (m_VerticesSize.Height + m_fVerticalSpacing));

				curRowNodes = new NList<NShape>();
				for (int i = 0; i < level; i++)
				{
                    cur = CreateVertex(m_VerticesShape);
					cur.SetBounds(new NRectangle(location, m_VerticesSize));
					page.Items.AddChild(cur);

					location.X += m_VerticesSize.Width + m_fHorizontalSpacing;

					// connect the current node with its ancestors and prev node
					if (m_bConnectGrid == false)
						continue;
					
					// connect with prev
					if (i > 0)
					{
                        edge = CreateEdge(ENConnectorShape.Line);
						page.Items.AddChild(edge);

						edge.GlueBeginToGeometryIntersection(prev);
						edge.GlueEndToShape(cur);
					}

					// connect with ancestors
					if (level > 1)
					{
						if (i < prevRowNodes.Count)
						{
                            edge = CreateEdge(ENConnectorShape.Line);
							page.Items.AddChild(edge);

							edge.GlueBeginToGeometryIntersection((NShape)prevRowNodes[i]);
							edge.GlueEndToShape(cur);
						}

						if (i > 0)
						{
                            edge = CreateEdge(ENConnectorShape.Line);
							page.Items.AddChild(edge);

							edge.GlueBeginToGeometryIntersection((NShape)prevRowNodes[i - 1]);
							edge.GlueEndToShape(cur);
						}
					}

					curRowNodes.Add(cur);
					prev = cur;
				}

				prevRowNodes = curRowNodes;
			}
		}


		#endregion

		#region Fields

		internal int m_nLevels;
		internal bool m_bConnectGrid;

		#endregion
	}
}
