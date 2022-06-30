using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Expressions;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NAnnotationShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAnnotationShapesExample()
		{ 
			
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAnnotationShapesExample()
		{
			NAnnotationShapesExampleSchema = NSchema.Create(typeof(NAnnotationShapesExample), NExampleBaseSchema);
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
    This example demonstrates the annotation shapes, which are created by the NAnnotationShapeFactory.
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
			NAnnotationShapeFactory factory = new NAnnotationShapeFactory();
			factory.DefaultSize = new NSize(60, 60);

			int row = 0, col = 0;
			double cellWidth = 0;// 180;
			double cellHeight = 150;
			bool is1D = false;

			for (int i = 0; i < factory.ShapeCount; i++, col++)
			{
				NShape shape = factory.CreateShape(i);
				NShape tempShape;
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;

				if (i == (int)ENAnnotationShape.Text ||
					i == (int)ENAnnotationShape.FiveRuledColumn ||
					i == (int)ENAnnotationShape.InfoLine ||

					i == (int)ENAnnotationShape.NorthArrow5 ||
					i == (int)ENAnnotationShape.NoteSymbol ||
					i == (int)ENAnnotationShape.ReferenceTriangle ||
					i == (int)ENAnnotationShape.ReferenceRectangle ||
					i == (int)ENAnnotationShape.ReferenceHexagon ||
					i == (int)ENAnnotationShape.ReferenceCircle ||
					i == (int)ENAnnotationShape.ReferenceOval)
				{
					NGroup group = new NGroup();

					group.Width = shape.Width;
					group.Height = shape.Height;
					if (i == (int)ENAnnotationShape.Text)
					{
						shape.PinX = 0;
						shape.SetFx(NShape.PinYProperty, "Height");
					}
					else
					{
						shape.SetFx(NShape.PinXProperty, "Width / 2");
						shape.SetFx(NShape.PinYProperty, "Height / 2");
					}

					group.TextBlock = new NTextBlock(factory.GetShapeInfo(i).Name);

					shape.SetFx(NShape.WidthProperty, "$ParentSheet.Width");
					shape.SetFx(NShape.HeightProperty, "$ParentSheet.Height");
					MoveTextBelowShape(group);

					group.Shapes.Add(shape);
					activePage.Items.Add(group);
					tempShape = group;
				}
				else
				{
					if (i != (int)ENAnnotationShape.Benchmark)
					{
						shape.Text = factory.GetShapeInfo(i).Name;
						MoveTextBelowShape(shape);
						if (i == (int)ENAnnotationShape.ReferenceCallout1)
						{
							shape.TextBlock.PinX = 40;
							shape.TextBlock.PinY = 10;
						}
						if (i == (int)ENAnnotationShape.ReferenceCallout2)
						{
							shape.TextBlock.Angle = new NAngle(0);
							shape.TextBlock.PinY = 100;
						}
					}

					activePage.Items.Add(shape);
					tempShape = shape;
				}

				if (col >= 5)
				{
					row++;
					col = 0;
					cellWidth = 0;
					is1D = false;
				}

				int widthGap = is1D ? 150 : 100;
				is1D = shape.ShapeType == ENShapeType.Shape1D;
				NPoint beginPoint = new NPoint(widthGap + cellWidth, 50 + row * cellHeight);
				if (is1D)
				{
					NPoint endPoint = beginPoint + new NPoint(0, cellHeight - 60);
					if (i == (int)ENAnnotationShape.ReferenceCallout1 || i == (int)ENAnnotationShape.ReferenceCallout2)
					{
						tempShape.SetBeginPoint(beginPoint);
						tempShape.SetEndPoint(endPoint);
					}
					else
					{
						tempShape.SetBeginPoint(endPoint);
						tempShape.SetEndPoint(beginPoint);
					}
				}
				else
				{
					tempShape.SetBounds(beginPoint.X, beginPoint.Y, shape.Width, shape.Height);
				}

				cellWidth += widthGap + tempShape.Width;
			}

			// size page to content
			activePage.Layout.ContentPadding = new NMargins(40);
			activePage.SizeToContent();
		}
		private void MoveTextBelowShape(NShape shape)
		{
			if (shape.ShapeType == ENShapeType.Shape2D)
			{
				shape.MoveTextBlockBelowShape();
				return;
			}

			// if the shape is 1D put the text block on the left part of the shape and rotate it on 90 degrees.
			NTextBlock textBlock = shape.GetTextBlock();
			textBlock.Padding = new NMargins(5, 0, 0, 0);
			textBlock.ResizeMode = ENTextBlockResizeMode.TextSize;
			textBlock.PinX = shape.BeginX;
			textBlock.SetFx(NTextBlock.PinYProperty, new NShapeHeightFactorFx(0));
			textBlock.LocPinY = 0;
			textBlock.Angle = new NAngle(90);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAnnotationShapesExample.
		/// </summary>
		public static readonly NSchema NAnnotationShapesExampleSchema;

		#endregion
	}
}
