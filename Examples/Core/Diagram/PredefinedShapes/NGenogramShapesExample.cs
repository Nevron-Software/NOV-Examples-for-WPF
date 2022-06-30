using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NGenogramShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NGenogramShapesExample()
		{ 
			
		}

		/// <summary>
		/// Static constructor
		/// </summary>
		static NGenogramShapesExample()
		{
			NGenogramShapesExampleSchema = NSchema.Create(typeof(NGenogramShapesExample), NExampleBaseSchema);
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
    This example demonstrates the genogram shapes, which are created by the NGenogramShapeFactory.
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
			NGenogramShapeFactory factory = new NGenogramShapeFactory();

			int row = 0, col = 0;
			double cellWidth = 240;
			double cellHeight = 150;

			for (int i = 0; i < factory.ShapeCount; i++, col++)
			{
				NShape shape = factory.CreateShape(i);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;

				NTextBlock textBlock = shape.GetFirstDescendant<NTextBlock>();

				if (textBlock == null ||
					i == (int)ENGenogramShape.Male ||
					i == (int)ENGenogramShape.Female ||
					i == (int)ENGenogramShape.Pet ||
					i == (int)ENGenogramShape.UnknownGender)
				{
					textBlock = (NTextBlock)shape.TextBlock;
				}

				textBlock.Text = factory.GetShapeInfo(i).Name;

				activePage.Items.Add(shape);

				if (col >= 4)
				{
					row++;
					col = 0;
				}

				NPoint beginPoint = new NPoint(50 + col * cellWidth, 50 + row * cellHeight);
				if (shape.ShapeType == ENShapeType.Shape1D)
				{
					NPoint endPoint = beginPoint + new NPoint(cellWidth - 50, cellHeight - 50);

					shape.SetBeginPoint(beginPoint);
					shape.SetEndPoint(endPoint);
				}
				else
				{
					textBlock.SetFx(NTextBlock.PinYProperty, "$Parent.Height + Height + 10");
					textBlock.ResizeMode = ENTextBlockResizeMode.TextSize;
					shape.SetBounds(beginPoint.X, beginPoint.Y, shape.Width, shape.Height);
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
		/// Schema associated with NGenogramShapesExample.
		/// </summary>
		public static readonly NSchema NGenogramShapesExampleSchema;

		#endregion
	}
}
