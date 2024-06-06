using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Layout;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NFlowchartShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFlowchartShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFlowchartShapesExample()
		{
			NFlowchartShapesExampleSchema = NSchema.Create(typeof(NFlowchartShapesExample), NExampleBaseSchema);
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
    This example demonstrates the flowchart shapes accessible through the ""NLibrary.FlowchartShapes"" library.
    You should distribute this library with your application if you plan to use the flowchart import or any flowchart shapes.
    The library should be placed in the resources folder of the application in ""ShapeLibraries\Flowchart\Flowchart Shapes.nlb"".
</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			const double ShapeWidth = 60;
			const double ShapeHeight = 60;

			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// create all shapes
			NLibrary library = NLibrary.FlowchartShapes;

			for (int i = 0; i < library.Items.Count; i++)
			{
				NShape shape = library.CreateShape(i, ShapeWidth, ShapeHeight);
				shape.HorizontalPlacement = ENHorizontalPlacement.Center;
				shape.VerticalPlacement = ENVerticalPlacement.Center;
				shape.Text = shape.Name;
				shape.MoveTextBlockBelowShape();
				activePage.Items.Add(shape);
			}

			// arrange them
			NList<NShape> shapes = activePage.GetShapes(false);
			NLayoutContext layoutContext = new NLayoutContext();
			layoutContext.BodyAdapter = new NShapeBodyAdapter(drawingDocument);
			layoutContext.GraphAdapter = new NShapeGraphAdapter();
			layoutContext.LayoutArea = activePage.Bounds;

			NTableFlowLayout flowLayout = new NTableFlowLayout();
			flowLayout.HorizontalSpacing = 30;
			flowLayout.VerticalSpacing = 50;
			flowLayout.Direction = ENHVDirection.LeftToRight;
			flowLayout.MaxOrdinal = 5;
			flowLayout.Arrange(shapes.CastAll<object>(), layoutContext);

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
		/// Schema associated with NFlowchartShapesExample.
		/// </summary>
		public static readonly NSchema NFlowchartShapesExampleSchema;

		#endregion
	}
}