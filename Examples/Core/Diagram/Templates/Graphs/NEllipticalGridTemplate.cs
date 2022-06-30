using System;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// The NEllipticalGridTemplate class represents an elliptical grid template
	/// </summary>
	public class NEllipticalGridTemplate : NGraphTemplate
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NEllipticalGridTemplate()
			: base("Elliptical Grid")
		{
			m_nRimNodesCount = 6;
			m_dRadiusY = 100;
			m_dRadiusX = 100;
			m_bHasCenter = true;
			m_bConnectGrid = true;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the count of the nodes on the ellipse rim
		/// </summary>
		/// <remarks>
		/// By default set to 6
		/// </remarks>
		public int RimNodesCount
		{
			get
			{
				return m_nRimNodesCount;
			}
			set
			{
				if (value == m_nRimNodesCount)
					return;

				if (value < 3)
					throw new ArgumentOutOfRangeException("The value must be > 2.");

				m_nRimNodesCount = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Controls the X radius of the ellipse
		/// </summary>		
		public double RadiusX
		{
			get
			{
				return m_dRadiusX;
			}
			set
			{
				if (value == m_dRadiusX)
					return;

				if (value <= 0)
					throw new ArgumentOutOfRangeException("The value must be > 0.");

				m_dRadiusX = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Controls the Y radius of the ellipse
		/// </summary>		
		public double RadiusY
		{
			get
			{
				return m_dRadiusY;
			}
			set
			{
				if (value == m_dRadiusY)
					return;

				if (value <= 0)
					throw new ArgumentOutOfRangeException("The value must be > 0.");

				m_dRadiusY = value;
				OnTemplateChanged();
			}
		}

		/// <summary>
		/// Specifies whether the grid has a center node
		/// </summary>
		/// <remarks>
		/// By default set to true
		/// </remarks>
		public bool HasCenter
		{
			get
			{
				return m_bHasCenter;
			}
			set
			{
				if (value == m_bHasCenter)
					return;

				m_bHasCenter = value;
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
		/// Overriden to return the elliptical grid description
		/// </summary>
		public override string GetDescription()
		{
			string description = String.Format("##Elliptical grid graph with {0} nodes on the rim.", m_nRimNodesCount);

            if (m_bHasCenter)
            {
                description += " " + "##Has center node.";
            }


			if (m_bConnectGrid)
			{
				description += " " + "##Each node is connected with the next node on the rim.";
				if (m_bHasCenter)
					description += " " + "##Each node is also connected with the center node.";
			}

			return  description;
		}

		#endregion

		#region Protected overrides

		/// <summary>
		/// Overriden to create the elliptical grid template in the specified document
		/// </summary>
		/// <param name="document">document in which to create the template</param>
		protected override void CreateTemplate(NDrawingDocument document)
		{
			int i;
			NShape node;
			NShape edge = null;

			NPoint pt;
			NList<NShape> nodes = new NList<NShape>();
			NPage page = document.Content.ActivePage;

			// create the ellipse nodes
			double curAngle = 0;
			double stepAngle = NMath.PI2 / m_nRimNodesCount;

			NPoint center = new NPoint(	m_Origin.X + m_dRadiusX + m_VerticesSize.Width / 2, 
										m_Origin.Y + m_dRadiusY + m_VerticesSize.Height / 2);

			for (i = 0; i < m_nRimNodesCount; i++)
			{
				pt = new NPoint(	center.X + m_dRadiusX * (double)Math.Cos(curAngle) - m_VerticesSize.Width / 2,
									center.Y + m_dRadiusY * (double)Math.Sin(curAngle) - m_VerticesSize.Height / 2);

				node = CreateVertex(m_VerticesShape);

				node.SetBounds(new NRectangle(pt, m_VerticesSize));
                page.Items.AddChild(node);

				nodes.Add(node);
                curAngle += stepAngle;
			}

			// connect the ellipse nodes
			if (m_bConnectGrid)
			{
				for (i = 0; i < m_nRimNodesCount; i++)
				{
					edge = CreateEdge(ENConnectorShape.Line);
					page.Items.AddChild(edge);

					edge.GlueBeginToGeometryIntersection(nodes[i]);
                    edge.GlueEndToGeometryIntersection(nodes[(i + 1) % m_nRimNodesCount]);
				}
			}

			if (m_bHasCenter == false) 
				return;

			// create the center
			node = CreateVertex(m_VerticesShape);
			pt = new NPoint(	center.X - m_VerticesSize.Width / 2,
								center.Y - m_VerticesSize.Height / 2);

			node.SetBounds(new NRectangle(pt, m_VerticesSize));
            page.Items.AddChild(node);

			// connect the ellipse nodes with the center
			if (m_bConnectGrid)
			{
				for (i = 0; i < m_nRimNodesCount; i++)
				{
					edge = CreateEdge(ENConnectorShape.Line);
                    page.Items.AddChild(edge);

					edge.GlueBeginToGeometryIntersection(node);
					edge.GlueEndToGeometryIntersection(nodes[i]);
				}
			}
		}

		#endregion

		#region Fields

		internal int m_nRimNodesCount;
		internal double m_dRadiusY;
		internal double m_dRadiusX;
		internal bool m_bHasCenter;
		internal bool m_bConnectGrid;

		#endregion
	}
}
