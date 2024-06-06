using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
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
			return @"<p>This example shows the EPC shapes located in the ""Business\Event Driven Process Chain (EPC).nlb"" shape library.</p>";
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

			// Load the library and create all shapes from it
			NFile libraryFile = NApplication.ResourcesFolder.GetFile(NPath.Current.Combine(
					"ShapeLibraries", "Business", "Event Driven Process Chain (EPC).nlb"));
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
							group.TextBlock = new NTextBlock(shape.Name);
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
							shape.Text = shape.Name;
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
			);
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

		#region Nested Types

		/// <summary>
		/// Enumerates the EPC shapes.
		/// </summary>
		public enum ENEpcShape
		{
			/// <summary>
			/// And
			/// </summary>
			AND,
			/// <summary>
			/// Component
			/// </summary>
			Component,
			/// <summary>
			/// Enterprise Area
			/// </summary>
			EnterpriseArea,
			/// <summary>
			/// Event
			/// </summary>
			Event,
			/// <summary>
			/// Function
			/// </summary>
			Function,
			/// <summary>
			/// Information material
			/// </summary>
			InformationMaterial,
			/// <summary>
			/// Or
			/// </summary>
			OR,
			/// <summary>
			/// Organizational unit
			/// </summary>
			OrganizationalUnit,
			/// <summary>
			/// Main process
			/// </summary>
			MainProcess,
			/// <summary>
			/// Process group
			/// </summary>
			ProcessGroup,
			/// <summary>
			/// Process path
			/// </summary>
			ProcessPath,
			/// <summary>
			/// Routable connector
			/// </summary>
			RoutableConnector,
			/// <summary>
			/// Xor
			/// </summary>
			XOR
		}

		#endregion
	}
}