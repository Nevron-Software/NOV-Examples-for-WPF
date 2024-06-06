using System;
using System.Globalization;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Diagram
{
    /// <summary>
    /// The NGraphTemplate class is a template, which serves as base class for all templates which create graph
    /// </summary>
    /// <remarks>
    /// It enhances its base with the following features:
    /// <list type="bullet">
    /// <item>
    ///		<term>Vertex style and Edge style attributes</term>
    ///		<description>Exposed by the VerticesUserClass and EdgesUserClass properties
    ///		</description>
    ///	</item>
    /// <item>
    ///		<term>Control over the vertices shape and size</term>
    ///		<description>Exposed by the VerticesSize and VerticesShape properties
    ///		</description>
    ///	</item>
    /// <item>
    ///		<term>Generic spacing control</term>
    ///		<description>Exposed by the HorizontalSpacing and VerticalSpacing properties
    ///		</description>
    ///	</item>
    /// <item>
    ///		<term>Ability to create new vertices and edges which conform to the template settings</term>
    ///		<description>
    ///		Achieved with the help of the CreateLineGraphEdge and CreateGraphVertex methods.
    ///		</description>
    ///	</item>
    ///	</list>
    /// </remarks>
    public abstract class NGraphTemplate : NTemplate
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NGraphTemplate()
        {
            Initialize();
        }
        /// <summary>
        /// Initializing constructor.
        /// </summary>
        /// <param name="name">Template name.</param>
        public NGraphTemplate(string name)
            : base(name)
        {
            Initialize();
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NGraphTemplate()
        {
            CurrentEdgeIndex = 1;
            CurrentVertexIndex = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the size of the vertices constructed by this template.
        /// </summary>
        public NSize VertexSize
        {
            get
            {
                return m_VertexSize;
            }
            set
            {
                if (value == m_VertexSize)
                    return;

                if (value.Width <= 0 || value.Height <= 0)
                    throw new ArgumentOutOfRangeException();

                m_VertexSize = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets the shape of the vertices constructed by this template. By default set to
		/// <see cref="ENBasicShape.Rectangle"/>.
        /// </summary>
        public ENBasicShape VertexShape
        {
            get
            {
                return m_VertexShape;
            }
            set
            {
                if (m_VertexShape == value)
                    return;

                m_VertexShape = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets the horizontal spacing between vertices.
        /// </summary>
        public double HorizontalSpacing
        {
            get
            {
                return m_fHorizontalSpacing;
            }
            set
            {
                if (value == m_fHorizontalSpacing)
                    return;

                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                m_fHorizontalSpacing = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets the vertical spacing between vertices.
        /// </summary>
        public double VerticalSpacing
        {
            get
            {
                return m_fVerticalSpacing;
            }
            set
            {
                if (value == m_fVerticalSpacing)
                    return;

                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                m_fVerticalSpacing = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Specifies the user class for vertices.
        /// </summary>
        public string VerticesUserClass
        {
            get
            {
                return m_VertexUserClass;
            }
            set
            {
                if (m_VertexUserClass == null)
                    throw new ArgumentNullException();

                m_VertexUserClass = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Specifies the user class for edges.
        /// </summary>
        public string EdgeUserClass
        {
            get
            {
                return m_EdgeUserClass;
            }
            set
            {
                if (m_EdgeUserClass == null)
                    throw new ArgumentNullException();

                m_EdgeUserClass = value;
                OnTemplateChanged();
            }
        }

        #endregion

        #region Protected Overridable

        /// <summary>
        /// Creates a new edge shape of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>A new edge shape.</returns> 
        protected virtual NShape CreateEdge(ENConnectorShape type)
        {
			NShape connector = m_ConnectorShapes.CreateShape(type);
            connector.Name = m_sName + " Edge " + CurrentEdgeIndex.ToString(CultureInfo.InvariantCulture);
            connector.UserClass = m_EdgeUserClass;
            CurrentEdgeIndex++;

            return connector;
        }
        /// <summary>
        /// Creates a new vertex shape of the specified type.
        /// </summary>
        /// <param name="shape">A predefined basic shape.</param>
        /// <returns>A new vertex shape.</returns>
        protected virtual NShape CreateVertex(ENBasicShape shape)
        {
            NShape vertex = m_BasicShapes.CreateShape(shape);
            vertex.Name = m_sName + " Vertex " + CurrentVertexIndex.ToString(CultureInfo.InvariantCulture);
            vertex.UserClass = m_VertexUserClass;
            CurrentVertexIndex++;

            return vertex;
        }

        #endregion

        #region Implementation

        private void Initialize()
        {
            m_fHorizontalSpacing = 30;
            m_fVerticalSpacing = 30;

            m_VertexSize = new NSize(40, 40);
            m_VertexShape = ENBasicShape.Rectangle;

            m_VertexUserClass = String.Empty;
            m_EdgeUserClass = String.Empty;

			m_BasicShapes = new NBasicShapeFactory();
			m_ConnectorShapes = new NConnectorShapeFactory();
        }

        #endregion

        #region Fields

        internal double m_fHorizontalSpacing;
        internal double m_fVerticalSpacing;

        internal NSize m_VertexSize;
        internal ENBasicShape m_VertexShape;

        internal string m_VertexUserClass;
        internal string m_EdgeUserClass;

		private NBasicShapeFactory m_BasicShapes;
		private NConnectorShapeFactory m_ConnectorShapes;

        #endregion

        #region Static Fields

        public static long CurrentEdgeIndex;
        public static long CurrentVertexIndex;

        #endregion
    }
}