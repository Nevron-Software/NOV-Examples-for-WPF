using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NDfdShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDfdShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDfdShapesExample()
		{
			NDfdShapesExampleSchema = NSchema.Create(typeof(NDfdShapesExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates the data flow shapes, which are created by the NDfdShapeFactory.</p>";
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

			// Create all shapes
			NDataFlowDiagramShapesFactory factory = new NDataFlowDiagramShapesFactory();
			factory.DefaultSize = new NSize(80, 60);

			double x = 0;
			double y = 0;

			for (int i = 0; i < factory.ShapeCount; i++)
			{
				NShape shape = factory.CreateShape(i);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				string name = factory.GetShapeInfo(i).Name;
				shape.Tooltip = new NTooltip(name);
				shape.Text = factory.GetShapeInfo(i).Name;
				shape.MoveTextBlockBelowShape();

				activePage.Items.Add(shape);

				if (shape.ShapeType == ENShapeType.Shape1D)
				{
					shape.SetBeginPoint(new NPoint(x, y));
					shape.SetEndPoint(new NPoint(x + shape.Width, y));
				}
				else
				{
					shape.SetBounds(x, y, shape.Width, shape.Height);
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
		/// Schema associated with NDfdShapesExample.
		/// </summary>
		public static readonly NSchema NDfdShapesExampleSchema;

		#endregion		
	}
}
