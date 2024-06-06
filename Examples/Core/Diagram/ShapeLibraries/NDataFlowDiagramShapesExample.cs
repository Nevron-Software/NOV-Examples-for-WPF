using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
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
			return @"<p>This example shows the data flow shapes located in the ""Software\Data Flow Diagram Shapes.nlb"" shape library.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			const double ShapeWidth = 80;
			const double ShapeHeight = 60;
			const double XStep = 150;
			const double YStep = 200;

			NDrawing drawing = drawingDocument.Content;
			NPage activePage = drawing.ActivePage;

			// Hide grid and ports
			drawing.ScreenVisibility.ShowGrid = false;
			drawing.ScreenVisibility.ShowPorts = false;

			// Load the library and create all shapes from it
			NFile libraryFile = NApplication.ResourcesFolder.GetFile(NPath.Current.Combine(
					"ShapeLibraries", "Software", "Data Flow Diagram Shapes.nlb"));
			NLibraryDocument.FromFileAsync(libraryFile).Then(
				libraryDocument =>
				{
					NLibrary library = libraryDocument.Content;
					double x = 0;
					double y = 0;

					for (int i = 0; i < library.Items.Count; i++)
					{
						NShape shape = library.CreateShape(i, ShapeWidth, ShapeHeight);
						shape.HorizontalPlacement = ENHorizontalPlacement.Center;
						shape.VerticalPlacement = ENVerticalPlacement.Center;

						shape.Tooltip = new NTooltip(shape.Name);
						shape.Text = shape.Name;
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
			);
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