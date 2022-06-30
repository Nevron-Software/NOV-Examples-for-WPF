using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NUmlClassDiagramExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NUmlClassDiagramExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NUmlClassDiagramExample()
        {
            NUmlClassDiagramExampleSchema = NSchema.Create(typeof(NUmlClassDiagramExample), NExampleBaseSchema);
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
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to create an UML class diagrams.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Get drawing and active page
            NDrawing drawing = drawingDocument.Content;
            NPage activePage = drawing.ActivePage;

            // Hide ports and grid
            drawing.ScreenVisibility.ShowGrid = false;

            // Create styles
            NRule rule = CreateConnectorOneToManyRule();

            NStyleSheet styleSheet = new NStyleSheet();
            styleSheet.Add(rule);
            drawingDocument.StyleSheets.Add(styleSheet);

            // Create some UML shapes
            NShape shapeDevice = CreateUmlShape("Device", new NMemberInfo[] {
				new NMemberInfo(ENMemberVisibility.Public, "DeviceID", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Protected, "DeviceCategory", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Private, "name", "varchar(50)"),
				new NMemberInfo(ENMemberVisibility.Private, "description", "blob"),
				new NMemberInfo(ENMemberVisibility.Private, "detail", "blob")
			});
            activePage.Items.Add(shapeDevice);

            NShape shapeDeviceCategory = CreateUmlShape("DeviceCategory", new NMemberInfo[] {
				new NMemberInfo(ENMemberVisibility.Public, "CategoryID", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Private, "description", "blob")
			});
            activePage.Items.Add(shapeDeviceCategory);

            NShape shapeSupportRequest = CreateUmlShape("SupportRequest", new NMemberInfo[] {
				new NMemberInfo(ENMemberVisibility.Protected, "Device", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Public, "RequestID", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Protected, "User", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Private, "reportDate", "date"),
				new NMemberInfo(ENMemberVisibility.Private, "description", "blob")
			});
            activePage.Items.Add(shapeSupportRequest);

            NShape shapeUser = CreateUmlShape("User", new NMemberInfo[] {
				new NMemberInfo(ENMemberVisibility.Public, "ID", "integer(10)"),
				new NMemberInfo(ENMemberVisibility.Public, "firstName", "varchar(50)"),
				new NMemberInfo(ENMemberVisibility.Protected, "lastName", "varchar(50)"),
				new NMemberInfo(ENMemberVisibility.Private, "phone", "varchar(12)"),
				new NMemberInfo(ENMemberVisibility.Private, "email", "varchar(50)"),
				new NMemberInfo(ENMemberVisibility.Private, "address", "blob"),
				new NMemberInfo(ENMemberVisibility.Private, "remarks", "blob")
			});
            activePage.Items.Add(shapeUser);

            // Connect the shapes
            Connect(GetShapeByName("DeviceCategory"), "CategoryID", GetShapeByName("Device"), "DeviceCategory");
            Connect(GetShapeByName("Device"), "DeviceID", GetShapeByName("SupportRequest"), "Device");
            Connect(GetShapeByName("User"), "ID", GetShapeByName("SupportRequest"), "User");

            // Subscribe to the drawing view's Registered event to layout the shapes
            // when the drawing view is registered in its owner document
            m_DrawingView.Registered += OnDrawingViewRegistered;
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates the rule for one to many connectors.
        /// </summary>
        /// <returns></returns>
        private NRule CreateConnectorOneToManyRule()
        {
            NRule rule = new NRule();

            NSelectorBuilder sb = rule.GetSelectorBuilder();
            sb.Start();
            sb.Type(NGeometry.NGeometrySchema);
            sb.ChildOf();
            sb.UserClass(ConnectorOneToManyClassName);
            sb.End();

            rule.Declarations.Add(new NValueDeclaration<NStroke>(NGeometry.StrokeProperty,
                new NStroke(1, NColor.Black, ENDashStyle.Dash)));
            rule.Declarations.Add(new NValueDeclaration<NArrowhead>(NGeometry.BeginArrowheadProperty,
                new NArrowhead(ENArrowheadShape.VerticalLine, 8, 8, null, new NStroke(1, NColor.Black))));
            rule.Declarations.Add(new NValueDeclaration<NArrowhead>(NGeometry.EndArrowheadProperty,
                new NArrowhead(ENArrowheadShape.InvertedLineArrowWithCircleNoFill, 8, 8, null, new NStroke(1, NColor.Black))));

            return rule;
        }
        /// <summary>
        /// Creates a one to many connector from the member1 of shape1 to
        /// member2 of shape2.
        /// </summary>
        /// <param name="shape1"></param>
        /// <param name="member1"></param>
        /// <param name="shape2"></param>
        /// <param name="member2"></param>
        private void Connect(NShape shape1, string member1, NShape shape2, string member2)
        {
            NRoutableConnector connector = new NRoutableConnector();
            connector.UserClass = ConnectorOneToManyClassName;
            m_DrawingView.ActivePage.Items.Add(connector);

            // Get or create the ports
            NPort port1 = GetOrCreatePort(shape1, member1);
            NPort port2 = GetOrCreatePort(shape2, member2);

            if (port1 == null)
                throw new ArgumentException("A member with name '" + member1 + "' not found in shape '" + shape1.Name + "'", "member");

            if (port1 == null)
                throw new ArgumentException("A member with name '" + member2 + "' not found in shape '" + shape2.Name + "'", "member");

            // Connect the ports
            connector.GlueBeginToPort(port1);
            connector.GlueEndToPort(port2);
        }
        /// <summary>
        /// Gets the port with the given name or creates one if a port with the given name
        /// does not exist in the specified shape.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private NPort GetOrCreatePort(NShape shape, string member)
        {
            NPort port = shape.GetPortByName(member);
            if (port != null)
                return port;

            // The port does not exist, so create it
            NLabel label = (NLabel)shape.Widget.GetFirstDescendant(new NLabelByTextFilter(member));
            if (label == null)
                return null;

            NPairBox pairBox = (NPairBox)label.GetFirstAncestor(NPairBox.NPairBoxSchema);
            NStackPanel stack = (NStackPanel)pairBox.ParentNode;
            double yRelative = (pairBox.GetAggregationInfo().Index + 0.5) / stack.Count;

            port = new NPort(0.5, yRelative, true);
            port.SetDirection(ENBoxDirection.Right);
            shape.Ports.Add(port);

            return port;
        }

        /// <summary>
        /// Creates an UML class diagram shape.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="memberInfos"></param>
        /// <returns></returns>
        private NShape CreateUmlShape(string name, NMemberInfo[] memberInfos)
        {
            // Create the shape
            NShape shape = new NShape();
            shape.Name = name;

            // Set a rounded rectangle geometry
            NDrawRectangle drawRectangle = new NDrawRectangle(0, 0, 1, 1);
            drawRectangle.Relative = true;
            shape.Geometry.Add(drawRectangle);
            shape.Geometry.CornerRounding = 15;

            // Create a stack panel
            NStackPanel stack = new NStackPanel();
            stack.Margins = new NMargins(5);
            stack.Direction = ENHVDirection.TopToBottom;
            
			// Create the title label
			NLabel titleLabel = new NLabel(name);
			titleLabel.TextAlignment = ENContentAlignment.MiddleCenter;
			titleLabel.Border = NBorder.CreateFilledBorder(NColor.Black);
			titleLabel.BorderThickness = new NMargins(0, 0, 0, 1);
			stack.Add(titleLabel);
			
			// Create the member info pair boxes
			for (int i = 0; i < memberInfos.Length; i++)
			{
				stack.Add(CreatePairBox(memberInfos[i]));
			}

            shape.Widget = new NUniSizeBoxGroup(stack);
            BindSizeToDesiredSize(shape);

            return shape;
        }
        /// <summary>
        /// Creates a pair box from the given member information.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        private NPairBox CreatePairBox(NMemberInfo memberInfo)
        {
            NPairBox pairBox = new NPairBox((char)memberInfo.Visibility + memberInfo.Name, memberInfo.Type, true);
            pairBox.Spacing = NDesign.HorizontalSpacing * 2;
            pairBox.FillMode = ENStackFillMode.Equal;
            pairBox.FitMode = ENStackFitMode.Last;

            switch (memberInfo.Visibility)
            {
                case ENMemberVisibility.Public:
                    // Set bold font style
                    pairBox.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 9, ENFontStyle.Bold);
                    break;
                case ENMemberVisibility.Protected:
                    // Set italic font style
                    pairBox.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 9, ENFontStyle.Italic);
                    break;
                case ENMemberVisibility.Private:
                    // Do not change the font style
                    break;
            }

            return pairBox;
        }
        /// <summary>
        /// Binds the size of the shape to the embedded widget desired size.
        /// </summary>
        /// <param name="shape"></param>
        private void BindSizeToDesiredSize(NShape shape)
        {
            NWidget widget = shape.Widget;

            // bind shape width to button desired width
            NBindingFx bx = new NBindingFx(widget, NButton.DesiredWidthProperty);
            bx.Guard = true;
            shape.SetFx(NShape.WidthProperty, bx);

            // bind shape height to button desired height
            NBindingFx by = new NBindingFx(widget, NButton.DesiredHeightProperty);
            by.Guard = true;
            shape.SetFx(NShape.HeightProperty, by);

            shape.AllowResizeX = false;
            shape.AllowRotate = false;
            shape.AllowResizeY = false;
        }
        /// <summary>
        /// Gets the first shape with the given name in the drawing document's active page.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private NShape GetShapeByName(string name)
        {
            NPage activePage = m_DrawingView.ActivePage;
            return (NShape)activePage.GetFirstDescendant(new NShapeByNameFilter(name));
        }

        /// <summary>
        /// Arranges the shapes in the active page.
        /// </summary>
        private void ArrangeDiagram()
        {
            // Create and configure a layout
            NLayeredGraphLayout layout = new NLayeredGraphLayout();

            // Get all top-level shapes that reside in the active page
            NPage activePage = m_DrawingView.ActivePage;
            NList<NShape> shapes = activePage.GetShapes(false);

            // Create a layout context and use it to arrange the shapes using the current layout
            NDrawingLayoutContext layoutContext = new NDrawingLayoutContext(m_DrawingView.Document, activePage);
            layout.Arrange(shapes.CastAll<object>(), layoutContext);

            // Size the page to the content size
            activePage.SizeToContent();
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when the drawing view is registered to its owner document.
        /// </summary>
        /// <param name="arg"></param>
        private void OnDrawingViewRegistered(NEventArgs arg)
        {
            // Evaluate the drawing document
            m_DrawingView.Document.Evaluate();

            // Layout the shapes
            ArrangeDiagram();
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NUmlClassDiagramExample.
        /// </summary>
        public static readonly NSchema NUmlClassDiagramExampleSchema;

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Constants

        private const string ConnectorOneToManyClassName = "ConnectorOneToMany";

        #endregion

        #region Nested Types

        private enum ENMemberVisibility
        {
            Public = (int)'+',
            Protected = (int)'#',
            Private = (int)'-'
        }

        private struct NMemberInfo
        {
            public NMemberInfo(ENMemberVisibility visibility, string name, string type)
            {
                Visibility = visibility;
                Name = name;
                Type = type;
            }

            public ENMemberVisibility Visibility;
            public string Name;
            public string Type;
        }

        private class NShapeByNameFilter : INFilter<NNode>
        {
            public NShapeByNameFilter(string name)
            {
                Name = name;
            }

            public bool Filter(NNode item)
            {
                NShape shape = item as NShape;
                return shape != null && shape.Name == Name;
            }

            public string Name;
        }

        private class NLabelByTextFilter : INFilter<NNode>
        {
            public NLabelByTextFilter(string text)
            {
                Text = text;
            }

            public bool Filter(NNode item)
            {
                NLabel label = item as NLabel;
                return label != null && label.Text.Substring(1) == Text;
            }

            public string Text;
        }

        #endregion
    }
}