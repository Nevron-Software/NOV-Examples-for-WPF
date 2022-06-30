using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NLayoutGroupExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLayoutGroupExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLayoutGroupExample()
		{
			NLayoutGroupExampleSchema = NSchema.Create(typeof(NLayoutGroupExample), NExampleBaseSchema);
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
			return "Demonstrates how to layout the shapes in a group.";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			// Create a group
			NGroup group = new NGroup();
			drawingDocument.Content.ActivePage.Items.Add(group);
			group.PinX = 300;
			group.PinY = 300;

			// Create some shapes and add them to the group
			NBasicShapeFactory factory = new NBasicShapeFactory();

			NShape root = factory.CreateShape(ENBasicShape.Rectangle);
			root.Text = "Root";
			group.Shapes.Add(root);

			NShape shape1 = factory.CreateShape(ENBasicShape.Circle);
			shape1.Text = "Circle 1";
			group.Shapes.Add(shape1);

			NShape shape2 = factory.CreateShape(ENBasicShape.Circle);
			shape2.Text = "Circle 2";
			group.Shapes.Add(shape2);

			NShape shape3 = factory.CreateShape(ENBasicShape.Circle);
			shape3.Text = "Circle 3";
			group.Shapes.Add(shape3);

			// Connect the shapes
			ConnectShapes(root, shape1);
			ConnectShapes(root, shape2);
			ConnectShapes(root, shape3);

			// Update group bounds
			group.UpdateBounds();

			// Create a layout context and configure the area you want the group to be arranged in.
			// The layout area is in page coordinates
			NDrawingLayoutContext layoutContext = new NDrawingLayoutContext(drawingDocument, group);
			layoutContext.LayoutArea = new NRectangle(100, 100, 200, 200);

			// Layout the shapes in the group
			NRadialGraphLayout layout = new NRadialGraphLayout();
			layout.Arrange(group.Shapes.ToArray(), layoutContext);

			// Update the group bounds
			group.UpdateBounds();
		}

		#endregion

		#region Implementation

		private void ConnectShapes(NShape shape1, NShape shape2)
		{
			NGroup group = shape1.OwnerGroup;

			NRoutableConnector connector = new NRoutableConnector();
			connector.UserClass = NDR.StyleSheetNameConnectors;
			group.Shapes.Add(connector);
			connector.GlueBeginToShape(shape1);
			connector.GlueEndToShape(shape2);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLayoutGroupExample.
		/// </summary>
		public static readonly NSchema NLayoutGroupExampleSchema;

		#endregion
	}
}