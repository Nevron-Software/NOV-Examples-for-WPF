using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NBrainstormingShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBrainstormingShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBrainstormingShapesExample()
		{
			NBrainstormingShapesExampleSchema = NSchema.Create(typeof(NBrainstormingShapesExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates the brainstorming shapes, which are created by the NBrainstormingShapesFactory.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// Create all shapes
			NBrainstormingShapeFactory factory = new NBrainstormingShapeFactory();
			factory.DefaultSize = new NSize(60, 60);

			for (int i = 0; i < factory.ShapeCount; i++)
			{
				NShape shape = factory.CreateShape(i);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				shape.Text = factory.GetShapeInfo(i).Name;
				shape.MoveTextBlockBelowShape();
				activePage.Items.Add(shape);
			}

			// Arrange them
			NList<NShape> shapes = activePage.GetShapes(false);
			NLayoutContext layoutContext = new NLayoutContext();
			layoutContext.BodyAdapter = new NShapeBodyAdapter(drawingDocument);
			layoutContext.GraphAdapter = new NShapeGraphAdapter();
			layoutContext.LayoutArea = activePage.GetContentEdge();

			NTableFlowLayout tableLayout = new NTableFlowLayout();
			tableLayout.HorizontalSpacing = 30;
			tableLayout.VerticalSpacing = 50;
			tableLayout.Direction = ENHVDirection.LeftToRight;
			tableLayout.MaxOrdinal = 5;

			tableLayout.Arrange(shapes.CastAll<object>(), layoutContext);

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
		/// Schema associated with NBrainstormingShapesExample.
		/// </summary>
		public static readonly NSchema NBrainstormingShapesExampleSchema;

		#endregion
	}
}