using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// The NFamilyTreeTemplate class represents a family tree template
	/// </summary>
	public class NFamilyTreeTemplate : NGraphTemplate
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NFamilyTreeTemplate()
			: base("Family Tree")
		{
			m_nChildrenCount = 2;
		}


		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the children count
		/// </summary>
		/// <remarks>
		/// By default set to 2
		/// </remarks>
		public int ChildrenCount
		{
			get
			{
				return m_nChildrenCount;
			}
			set
			{
				if (value < 0)
					throw new ArgumentOutOfRangeException("The value must be >= 0");

				m_nChildrenCount = value;
				OnTemplateChanged();
			}
		}


		#endregion

		#region Overrides

		/// <summary>
		/// Overriden to return the family tree description
		/// </summary>
		public override string GetDescription()
		{
            string description = "##Family tree with two parents and " + m_nChildrenCount.ToString() + " " + (m_nChildrenCount == 1 ? " child" : " children") + ".";
			return  description;
		}

		#endregion

		#region Protected overrides

		/// <summary>
		/// Overriden to create the family tree template
		/// </summary>
		/// <param name="document">document in which to create the template</param>
		protected override void CreateTemplate(NDrawingDocument document)
		{
			NPoint pt;
			NShape node;
			NShape edge = null;
			NPage page = document.Content.ActivePage;
			
			// determine the elements dimensions
			double childrenWidth = m_nChildrenCount * m_VerticesSize.Width + (m_nChildrenCount - 1) * m_fHorizontalSpacing;
            double parentsWidth = m_VerticesSize.Width * 2 + m_fHorizontalSpacing;
			
			// determine the template dimensions
			double templateWidth = Math.Max(childrenWidth, parentsWidth);
			NRectangle templateBounds = new NRectangle(m_Origin.X, m_Origin.Y, templateWidth, m_VerticesSize.Height * 2 + m_fVerticalSpacing); 
			NPoint center = templateBounds.Center;

			// create the parent nodes
			NShape father = CreateVertex(m_VerticesShape);
			pt = new NPoint(center.X - (m_VerticesSize.Width + m_fHorizontalSpacing / 2), templateBounds.Y);  
			father.SetBounds(new NRectangle(pt, m_VerticesSize));
			page.Items.AddChild(father);

			NShape mother = CreateVertex(m_VerticesShape);
			pt = new NPoint(center.X + m_fHorizontalSpacing / 2, templateBounds.Y);  
			mother.SetBounds(new NRectangle(pt, m_VerticesSize));
			page.Items.AddChild(mother);

			// create the children
			if (m_nChildrenCount > 0)
			{
				double childrenY = templateBounds.Y + m_VerticesSize.Height + m_fVerticalSpacing;
				for (int i = 0; i < m_nChildrenCount; i++)
				{
					// create the child
					node = CreateVertex(m_VerticesShape);
					pt = new NPoint(i * (m_VerticesSize.Width + m_fHorizontalSpacing), childrenY);
					node.SetBounds(new NRectangle(pt, m_VerticesSize));
					page.Items.AddChild(node);

					// attach it to the parents
					edge = CreateEdge(ENConnectorShape.BottomToTop1);
					page.Items.AddChild(edge);

					edge.GlueBeginToGeometryIntersection(father);
					edge.GlueEndToShape(node);

                    edge = CreateEdge(ENConnectorShape.BottomToTop1);
					page.Items.AddChild(edge);

					edge.GlueBeginToGeometryIntersection(mother);
					edge.GlueEndToShape(node);
				}
			}
		}


		#endregion

		#region Fields

		internal int m_nChildrenCount;

		#endregion
	}
}
#region AUTO_CODE_BLOCK [NFamilyTreeTemplate]
#endregion
