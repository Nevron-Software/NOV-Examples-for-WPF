using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDimensioningEngineeringShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDimensioningEngineeringShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDimensioningEngineeringShapesExample()
		{
			NDimensioningEngineeringShapesExampleSchema = NSchema.Create(typeof(NDimensioningEngineeringShapesExample), NExampleBaseSchema);
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
    This example demonstrates the dimensioning engineering shapes, which are created by the NDimensioningEngineeringShapeFactory.
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
			NDimensioningEngineeringShapeFactory factory = new NDimensioningEngineeringShapeFactory();
			factory.DefaultSize = new NSize(90, 90);

			double x = 0;
			double y = 0;

			for (int i = 0; i < factory.ShapeCount; i++)
			{
				NShape shape = factory.CreateShape(i);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				shape.Tooltip = new NTooltip(factory.GetShapeInfo(i).Name);
				activePage.Items.Add(shape);

				if (shape.ShapeType == ENShapeType.Shape1D)
				{
					ENDimensioningEngineeringShapes shapeType = (ENDimensioningEngineeringShapes)i;
					switch (shapeType)
					{
						case ENDimensioningEngineeringShapes.VerticalBaseline:
						case ENDimensioningEngineeringShapes.Vertical:
						case ENDimensioningEngineeringShapes.VerticalOutside:
						case ENDimensioningEngineeringShapes.OrdinateVertical:
						case ENDimensioningEngineeringShapes.OrdinateVerticalMultiple:
							shape.SetBeginPoint(new NPoint(x + shape.Width, y + shape.Height));
							shape.SetEndPoint(new NPoint(x + shape.Width, y));
							break;
						case ENDimensioningEngineeringShapes.OrdinateHorizontalMultiple:
						case ENDimensioningEngineeringShapes.OrdinateHorizontal:
							shape.SetBeginPoint(new NPoint(x, y));
							shape.SetEndPoint(new NPoint(x + shape.Width, y));
							break;
						case ENDimensioningEngineeringShapes.Radius:
						case ENDimensioningEngineeringShapes.RadiusOutside:
						case ENDimensioningEngineeringShapes.ArcRadius:
						case ENDimensioningEngineeringShapes.Diameter:
						case ENDimensioningEngineeringShapes.DiameterOutside:
							shape.SetBeginPoint(new NPoint(x, y + shape.Height / 2));
							shape.SetEndPoint(new NPoint(x + shape.Width, y - shape.Height / 2));
							break;
						case ENDimensioningEngineeringShapes.AngleCenter:
						case ENDimensioningEngineeringShapes.AngleEven:
						case ENDimensioningEngineeringShapes.AngleOutside:
						case ENDimensioningEngineeringShapes.AngleUneven:
							shape.SetBeginPoint(new NPoint(x, y + shape.Width / 2));
							shape.SetEndPoint(new NPoint(x + shape.Width, y + shape.Width / 2));
							break;
						default:
							shape.SetBeginPoint(new NPoint(x, y));
							shape.SetEndPoint(new NPoint(x + shape.Width, y + shape.Height));
							break;
					}
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

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDimensioningEngineeringShapesExample.
		/// </summary>
		public static readonly NSchema NDimensioningEngineeringShapesExampleSchema;

		#endregion
	}
}