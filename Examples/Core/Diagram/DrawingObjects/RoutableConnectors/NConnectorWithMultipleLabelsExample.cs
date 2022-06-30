using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NConnectorWithMultipleLabelsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NConnectorWithMultipleLabelsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NConnectorWithMultipleLabelsExample()
		{
			NConnectorWithMultipleLabelsExampleSchema = NSchema.Create(typeof(NConnectorWithMultipleLabelsExample), NExampleBaseSchema);
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
			return @"
<p>This example demonstrates how to create a connector with multiple labels. This can be done by adding
outward ports to a connector and then gluing shapes that only have text to these outward ports.</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// 1. Create some shape factories
			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();
			NConnectorShapeFactory connectorShapesFactory = new NConnectorShapeFactory();

			// 2. Create and add some shapes
			NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape1.SetBounds(new NRectangle(50, 50, 100, 100));
			activePage.Items.Add(shape1);

			NShape shape2 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape2.SetBounds(new NRectangle(400, 50, 100, 100));
			activePage.Items.Add(shape2);

			// 3. Connect the shapes
			NShape connector = connectorShapesFactory.CreateShape(ENConnectorShape.Line);
			activePage.Items.Add(connector);
			connector.GlueBeginToShape(shape1);
			connector.GlueEndToShape(shape2);

			// Add 2 outward ports to the connector
			NPort port1 = new NPort(0.3, 0.3, true);
			port1.GlueMode = ENPortGlueMode.Outward;
			connector.Ports.Add(port1);

			NPort port2 = new NPort(0.7, 0.7, true);
			port2.GlueMode = ENPortGlueMode.Outward;
			connector.Ports.Add(port2);

			// Attach label shapes to the outward ports of the connector
			NShape labelShape1 = CreateLabelShape("Label 1");
			activePage.Items.Add(labelShape1);
			labelShape1.GlueMasterPortToPort(labelShape1.Ports[0], port1);

			NShape labelShape2 = CreateLabelShape("Label 2");
			activePage.Items.Add(labelShape2);
			labelShape2.GlueMasterPortToPort(labelShape2.Ports[0], port2);
		}

		#endregion

		#region Implementation

		private NShape CreateLabelShape(string text)
		{
			NShape labelShape = new NShape();

			// Configure shape
			labelShape.SetBounds(0, 0, 100, 30);
			labelShape.SetProtectionMask(ENDiagramItemOperationMask.All);
			labelShape.CanSplit = false;
			labelShape.GraphPart = false;
			labelShape.RouteThroughHorizontally = true;
			labelShape.RouteThroughVertically = true;

			// Set text and make text block resize to text
			labelShape.Text = text;
			((NTextBlock)labelShape.TextBlock).ResizeMode = ENTextBlockResizeMode.TextSize;

			// Set text background and border
			labelShape.TextBlock.BackgroundFill = new NColorFill(NColor.White);
			labelShape.TextBlock.Border = NBorder.CreateFilledBorder(NColor.Black);
			labelShape.TextBlock.BorderThickness = new NMargins(1);
			labelShape.TextBlock.Padding = new NMargins(2);

			// Add a port to the shape
			labelShape.Ports.Add(new NPort(0.5, 0.5, true));

			return labelShape;
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NConnectorWithMultipleLabelsExample.
		/// </summary>
		public static readonly NSchema NConnectorWithMultipleLabelsExampleSchema;

		#endregion
	}
}
