using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NSDLShapesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSDLShapesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSDLShapesExample()
		{
			NSDLShapesExampleSchema = NSchema.Create(typeof(NSDLShapesExample), NExampleBaseSchema);
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
			return @"<p>This example shows the SDL shapes located in the ""Flowchart\SDL Diagram Shapes.nlb"" shape library.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			const double ShapeWidth = 80;
			const double ShapeHeight = 60;
			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// Load the library and create all shapes from it
			NFile libraryFile = NApplication.ResourcesFolder.GetFile(NPath.Current.Combine(
					"ShapeLibraries", "Flowchart", "SDL Diagram Shapes.nlb"));
			NLibraryDocument.FromFileAsync(libraryFile).Then(
				libraryDocument =>
				{
					NLibrary library = libraryDocument.Content;
					int row = 0, col = 0;
					double cellWidth = 180;
					double cellHeight = 120;

					for (int i = 0; i < library.Items.Count; i++, col++)
					{
						NShape shape = library.CreateShape(i, ShapeWidth, ShapeHeight);
						shape.HorizontalPlacement = ENHorizontalPlacement.Center;
						shape.VerticalPlacement = ENVerticalPlacement.Center;
						shape.Text = shape.Name;
						shape.MoveTextBlockBelowShape();
						activePage.Items.Add(shape);

						if (col >= 5)
						{
							row++;
							col = 0;
						}

						NPoint beginPoint = new NPoint(50 + col * cellWidth, 50 + row * cellHeight);
						if (shape.ShapeType == ENShapeType.Shape1D)
						{
							NPoint endPoint = beginPoint + new NPoint(cellWidth - 100, cellHeight - 100);
							shape.SetBeginPoint(beginPoint);
							shape.SetEndPoint(endPoint);
						}
						else
						{
							shape.SetBounds(beginPoint.X, beginPoint.Y, shape.Width, shape.Height);
						}
					}

					// size page to content
					activePage.Layout.ContentPadding = new NMargins(40);
					activePage.SizeToContent();
				}
			);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSDLShapesExample.
		/// </summary>
		public static readonly NSchema NSDLShapesExampleSchema;

		#endregion
	}
}