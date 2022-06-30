using System;

using System.Collections;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// The NEllipticalGridTemplate class represents a rectangular grid template
	/// </summary>
	public class NRectangularGridTemplate : NGraphTemplate
    {
        #region Constructors

        /// <summary>
		/// Default constructor
		/// </summary>
		public NRectangularGridTemplate()
			: base("Rectangular Grid")
		{
			m_nRows = 3;
			m_nColumns = 3;
			m_bConnectGrid = true;
        }

        #endregion

        #region Properties

        /// <summary>
		/// Gets or sets the columns count
		/// </summary>
		/// <remarks>
		/// By default set to 3
		/// </remarks>
		public int ColumnCount
		{
			get
			{
				return m_nColumns;
			}
			set
			{
				if (value == m_nColumns)
					return;

				if (value < 1)
					throw new ArgumentOutOfRangeException("The value must be > 0.");

				m_nColumns = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Gets or sets the rows count
		/// </summary>
		/// <remarks>
		/// By default set to 3
		/// </remarks>
		public int RowCount
		{
			get
			{
				return m_nRows;
			}
			set
			{
				if (value == m_nRows)
					return;

				if (value < 1)
					throw new ArgumentOutOfRangeException("The value must be > 0.");

				m_nRows = value;
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
		/// Overriden to return the rectangular grid description
		/// </summary>
		public override string GetDescription()
		{
			string description = String.Format("##Rectangular grid graph with {0} columns and {1} rows.", m_nColumns, m_nRows);

			if (m_bConnectGrid)
				description += " " + "##Each cell is connected with the horizontally and vertically adjacent cells.";

			return  description;
		}

		/// <summary>
		/// Overriden to create the rectangular grid template in the specified document
		/// </summary>
		/// <param name="document">document in which to create the template</param>
		protected override void CreateTemplate(NDrawingDocument document)
		{
            NPage page = document.Content.ActivePage;
			NShape edge = null;
			NShape vertex;
			NShape[,] vertexGrid = new NShape[m_nRows, m_nColumns];

			for (int row = 0; row < m_nRows; row++)			
			{
				for (int col = 0; col < m_nColumns; col++)
				{	
					// create the vertex
					vertex = CreateVertex(m_VerticesShape);
					vertex.SetBounds(new NRectangle(	m_Origin.X + col * (m_VerticesSize.Width + m_fHorizontalSpacing), 
														m_Origin.Y + row * (m_VerticesSize.Height + m_fVerticalSpacing),
														m_VerticesSize.Width, m_VerticesSize.Height));
					page.Items.AddChild(vertex);

					// connect it with its X and Y predecessors
					if (m_bConnectGrid == false)
						continue;

					vertexGrid[row, col] = vertex;

					// connect X 
					if (col > 0)
					{
                        edge = CreateEdge(ENConnectorShape.Line);
                        page.Items.AddChild(edge);
						edge.GlueBeginToGeometryIntersection(vertexGrid[row, col - 1]);
						edge.GlueEndToShape(vertex);
					}

					// connect Y
					if (row > 0)
					{
                        edge = CreateEdge(ENConnectorShape.Line);
                        page.Items.AddChild(edge);
                        edge.GlueBeginToGeometryIntersection(vertexGrid[row - 1, col]);
						edge.GlueEndToShape(vertex);
					}
				}
			}
		}


		#endregion

		#region Fields

		internal int m_nRows;
		internal int m_nColumns;
		internal bool m_bConnectGrid;

		#endregion
	}
}
