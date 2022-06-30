using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NEPCShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NEPCShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NEPCShapesExample()
		{
			NEPCShapesExampleSchema = NSchema.Create(typeof(NEPCShapesExample), NExampleBaseSchema);
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
<p>
    This example demonstrates the EPC shapes, which are created by the NEpcShapeFactory.
</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// Create all shapes
			NEPCDiagramShapesFactory factory = new NEPCDiagramShapesFactory();
			factory.DefaultSize = new NSize(80, 60);

			int row = 0, col = 0;
			double cellWidth = 180;
			double cellHeight = 120;

			for (int i = 0; i < factory.ShapeCount; i++, col++)
			{
				NShape shape = factory.CreateShape(i);
				NShape tempShape;
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				if (i == (int)ENEpcShape.AND ||
					i == (int)ENEpcShape.OR ||
					i == (int)ENEpcShape.XOR)
				{
					NGroup group = new NGroup();
					group.Width = shape.Width;
					group.Height = shape.Height;
					group.Shapes.Add(shape);
					group.TextBlock = new NTextBlock(factory.GetShapeInfo(i).Name);
					shape.SetFx(NShape.PinXProperty, "Width / 2");
					shape.SetFx(NShape.PinYProperty, "Height / 2");
					shape.SetFx(NShape.WidthProperty, "$ParentSheet.Width");
					shape.SetFx(NShape.HeightProperty, "$ParentSheet.Height");
					group.MoveTextBlockBelowShape();

					activePage.Items.Add(group);
					tempShape = group;
				}
				else
				{
					shape.Text = factory.GetShapeInfo(i).Name;
					if (i == (int)ENEpcShape.InformationMaterial)
					{
						shape.TextBlock.Fill = new NColorFill(NColor.Black);
					}

					shape.MoveTextBlockBelowShape();
					activePage.Items.Add(shape);
					tempShape = shape;
				}

				if (col >= 4)
				{
					row++;
					col = 0;
				}

				NPoint beginPoint = new NPoint(50 + col * cellWidth, 50 + row * cellHeight);
				if (shape.ShapeType == ENShapeType.Shape1D)
				{
					NPoint endPoint = beginPoint + new NPoint(cellWidth - 100, cellHeight - 100);
					tempShape.SetBeginPoint(beginPoint);
					tempShape.SetEndPoint(endPoint);
				}
				else
				{
					tempShape.SetBounds(beginPoint.X, beginPoint.Y, shape.Width, shape.Height);
				}
			}

			// size page to content
			activePage.Layout.ContentPadding = new NMargins(40);
			activePage.SizeToContent();
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NEPCShapesExample.
		/// </summary>
		public static readonly NSchema NEPCShapesExampleSchema;

		#endregion
	}
}
