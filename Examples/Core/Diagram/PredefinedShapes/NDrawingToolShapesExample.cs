using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Expressions;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDrawingToolShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDrawingToolShapesExample()
		{

		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDrawingToolShapesExample()
		{
			NDrawingToolShapesExampleSchema = NSchema.Create(typeof(NDrawingToolShapesExample), NExampleBaseSchema);
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
    This example demonstrates the drawing tool shapes, which are created by the NDrawingToolShapeFactory.
</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			const double XStep = 150;
			const double YStep = 200;

			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// create all shapes
			NDrawingToolShapeFactory factory = new NDrawingToolShapeFactory();
			factory.DefaultSize = new NSize(90, 90);

			double x = 0;
			double y = 0;

			for (int i = 0; i < factory.ShapeCount; i++)
			{
				NShape shape = factory.CreateShape(i);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				shape.Tooltip = new NTooltip(factory.GetShapeInfo(i).Name);

				if (i != (int)ENDrawingToolShapes.SectorNumeric &&
					i != (int)ENDrawingToolShapes.ArcNumeric &&
					i != (int)ENDrawingToolShapes.RightTriangle)
				{
					shape.Text = factory.GetShapeInfo(i).Name;
					MoveTextBelowShape(shape);
				}
				activePage.Items.Add(shape);

				if (shape.ShapeType == ENShapeType.Shape1D)
				{
					if (i == (int)ENDrawingToolShapes.CircleRadius)
					{
						shape.SetBeginPoint(new NPoint(x + shape.Width / 2, y));
					}
					else
					{
						shape.SetBeginPoint(new NPoint(x, y));
					}

					double width = shape.Width;

					if (i == (int)ENDrawingToolShapes.MultigonEdge)
					{
						width = 90;
					}
					else if (i == (int)ENDrawingToolShapes.MultigonCenter)
					{
						width = 30;
					}

					shape.SetEndPoint(new NPoint(x + width, y + shape.Height));
				}
				else
				{
					shape.SetBounds(x, y, shape.Width, shape.Height);
					shape.LocPinY = 1;
				}

				x += XStep;
				if (x > activePage.Width)
				{
					x = 0;
					y += YStep;
				}
			}

			// size page to content
			activePage.Layout.ContentPadding = new NMargins(50);
			activePage.SizeToContent();
		}
		private void MoveTextBelowShape(NShape shape)
		{
			if (shape.ShapeType == ENShapeType.Shape1D)
			{
				NTextBlock textBlock = (NTextBlock)shape.TextBlock;
				textBlock.Padding = new NMargins(0, 5, 0, 0);
				textBlock.ResizeMode = ENTextBlockResizeMode.TextSize;
				textBlock.SetFx(NTextBlock.PinYProperty, new NShapeHeightFactorFx(1.0));
				textBlock.LocPinY = -2;
			}
			else
			{
				shape.MoveTextBlockBelowShape();
			}
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBasicShapesExample.
		/// </summary>
		public static readonly NSchema NDrawingToolShapesExampleSchema;

		#endregion
	}
}