using System;
using System.Globalization;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Editors;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;


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
        /// Default constructor
        /// </summary>
        public NGraphTemplate()
        {
            Initialize();
        }
        /// <summary>
        /// Initializer constructor
        /// </summary>
        /// <param name="name">template name</param>
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
        /// Gets or sets the size of the vertices constructed by this template
        /// </summary>
        public NSize VerticesSize
        {
            get
            {
                return m_VerticesSize;
            }
            set
            {
                if (value == m_VerticesSize)
                    return;

                if (value.Width <= 0 || value.Height <= 0)
                    throw new ArgumentOutOfRangeException();

                m_VerticesSize = value;
                OnTemplateChanged();
            }
        }

        /// <summary>
        /// Gets or sets the shape of the vertices constructed by this template
        /// </summary>
        /// <remarks>
        /// By default set to Rectangle
        /// </remarks>
        public ENBasicShape VerticesShape
        {
            get
            {
                return m_VerticesShape;
            }
            set
            {
                if (m_VerticesShape == value)
                    return;

                m_VerticesShape = value;
                OnTemplateChanged();
            }
        }
        /// <summary>
        /// Gets or sets the horizontal spacing between vertices
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
        /// Gets or sets the vertical spacing between vertices
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
        /// Specifies the default style applied to vertices
        /// </summary>
        public string VerticesUserClass
        {
            get
            {
                return m_VerticesUserClass;
            }
            set
            {
                if (m_VerticesUserClass == null)
                    throw new ArgumentNullException();

                m_VerticesUserClass = value;
                OnTemplateChanged();
            }
        }

        /// <summary>
        /// Specifies the default style applied to edges
        /// </summary>
        public string EdgesUserClass
        {
            get
            {
                return m_EdgesUserClass;
            }
            set
            {
                if (m_EdgesUserClass == null)
                    throw new ArgumentNullException();

                m_EdgesUserClass = value;
                OnTemplateChanged();
            }
        }

        #endregion

        #region Protected Overridable

        /// <summary>
        /// Creates a new connector from the specified type
        /// </summary>
        /// <remarks>
        /// The new connector style uses a copy of EdgesUserClass style
        /// </remarks>
        /// <param name="type"></param>
        /// <returns>new connector</returns> 
        protected virtual NShape CreateEdge(ENConnectorShape type)
        {
            NConnectorShapeFactory factory = new NConnectorShapeFactory();
            NShape connector = factory.CreateShape(type);
            connector.Name = m_sName + " Edge " + CurrentEdgeIndex.ToString(CultureInfo.InvariantCulture);
            connector.UserClass = m_EdgesUserClass;
            CurrentEdgeIndex++;
            return connector;
        }
        /// <summary>
        /// Creates a new graph vertex with the specified predefined shape
        /// </summary>
        /// <remarks>
        /// The new graph vertex style is a copy of the VerticesUserClass style
        /// </remarks>
        /// <param name="shape">predefined shape</param>
        /// <returns>new graph vertex</returns>
        protected virtual NShape CreateVertex(ENBasicShape shape)
        {
            NShape vertex = m_ShapeFactory.CreateShape(shape);
            vertex.Name = m_sName + " Vertex " + CurrentVertexIndex.ToString(CultureInfo.InvariantCulture);
            vertex.UserClass = m_VerticesUserClass;
            CurrentVertexIndex++;

            return vertex;
        }

        #endregion

        #region Implementation

        private void Initialize()
        {
            m_fHorizontalSpacing = 30;
            m_fVerticalSpacing = 30;

            m_VerticesSize = new NSize(40, 40);
            m_VerticesShape = ENBasicShape.Rectangle;

            m_VerticesUserClass = "";
            m_EdgesUserClass = "";

            m_ShapeFactory = new NBasicShapeFactory();
        }

        #endregion

        #region Fields

        internal double m_fHorizontalSpacing;
        internal double m_fVerticalSpacing;

        internal NSize m_VerticesSize;
        internal ENBasicShape m_VerticesShape;

        internal string m_VerticesUserClass;
        internal string m_EdgesUserClass;

        private NBasicShapeFactory m_ShapeFactory;

        #endregion

        #region Static Fields

        public static long CurrentEdgeIndex;
        public static long CurrentVertexIndex;

        #endregion
    }
}