using System;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Themes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NPipelineEditorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPipelineEditorExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPipelineEditorExample()
		{
			NPipelineEditorExampleSchema = NSchema.Create(typeof(NPipelineEditorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_DrawingView = new NDrawingView();

			NPage page = m_DrawingView.ActivePage;
			page.Items.Add(CreateTitleShape("Pipeline Editor"));

			return m_DrawingView;
		}
		protected override NWidget CreateExampleControls()
		{
			NLibraryView libraryView = new NLibraryView();
			libraryView.Content = CreateLibrary();
			return libraryView;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to take advantage of inward/outward ports in order to build a pipeline editor application.
	Drag and drop pipes from the pipes panel into the drawing area. When a pipe is near other pipe it sticks to its end if possible
	or automatically rotates if needed and then sticks to the other pipe. Use the end pipe symbols (triangles) to mark pipe ends.
</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates the Title shape.
		/// </summary>
		/// <param name="title"></param>
		/// <returns></returns>
		private NShape CreateTitleShape(string title)
		{
			NDrawingTheme theme = NDrawingTheme.MyDrawNature;

			NShape titleShape = new NShape();
			titleShape.SetBounds(0, 0, 500, 50);
			titleShape.Text = title;
			titleShape.SetProtectionMask(ENDiagramItemOperationMask.All);

			NTextBlock titleTextBlock = (NTextBlock)titleShape.TextBlock;
			titleTextBlock.ResizeMode = ENTextBlockResizeMode.ShapeSize;
			titleTextBlock.FontSize = 28;
			titleTextBlock.FontStyleBold = true;

			// Set theme-based colors to the title text, so that it changes when the user changes the theme
			NColor strokeColor = theme.ColorPalette.Variants[0][0];
			strokeColor.Tag = new NThemeVariantColorInfo(0);
			titleTextBlock.Stroke = new NStroke(strokeColor);

			NColor fillColor = theme.ColorPalette.Variants[0][4];
			fillColor.Tag = new NThemeVariantColorInfo(4);
			titleTextBlock.Fill = new NColorFill(fillColor);

			// Alternatively, you can also use fixed colors (uncomment the 2 lines below)
			//titleTextBlock.Stroke = new NStroke(NColor.DarkBlue);
			//titleTextBlock.Fill = new NColorFill(NColor.LightBlue);

			// Set an expression to center the title horizontally in the page
			titleShape.SetFx(NShape.PinXProperty, new NFormulaFx("$ParentSheet.X + $ParentSheet.Width / 2", true));

			return titleShape;
		}

		private NLibrary CreateLibrary()
		{
			NLibrary library = new NLibrary();

			library.Items.Add(new NLibraryItem(CreateHorizontalPipe(), "Horizontal Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateVerticalPipe(), "Vertical Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateCrossPipe(), "Cross Pipe", "Drag me on the drawing"));

			library.Items.Add(new NLibraryItem(CreateElbowPipe("NW"), "North-West Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateElbowPipe("NE"), "North-East Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateElbowPipe("SW"), "South-West Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateElbowPipe("SE"), "South-East Pipe", "Drag me on the drawing"));

			library.Items.Add(new NLibraryItem(CreateTPipe("NEW"), "North-East-West Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateTPipe("NES"), "North-East-South Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateTPipe("NWS"), "North-West-South Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateTPipe("SEW"), "South-East-West Pipe", "Drag me on the drawing"));

			library.Items.Add(new NLibraryItem(CreateEndPipe("W"), "West-End Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateEndPipe("N"), "North-End Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateEndPipe("E"), "East-End Pipe", "Drag me on the drawing"));
			library.Items.Add(new NLibraryItem(CreateEndPipe("S"), "South-End Pipe", "Drag me on the drawing"));

			return library;
		}

		private NShape CreateHorizontalPipe()
		{
			NShape shape = new NShape();
			shape.SetBounds(0, BoxSize, 3 * BoxSize, BoxSize);

			NDrawRectangle drawRectangle = new NDrawRectangle(0, 0, 1, 1);
			drawRectangle.ShowStroke = false;
			shape.Geometry.AddRelative(drawRectangle);

			shape.Geometry.AddRelative(new NMoveTo(0, 0));
			shape.Geometry.AddRelative(new NLineTo(1, 0));
			shape.Geometry.AddRelative(new NMoveTo(0, 1));
			shape.Geometry.AddRelative(new NLineTo(1, 1));

			shape.Ports.Add(CreatePort(Side.Left));
			shape.Ports.Add(CreatePort(Side.Right));

			SetProtections(shape);
			return shape;
		}
		private NShape CreateVerticalPipe()
		{
			NShape shape = new NShape();
			shape.SetBounds(BoxSize, 0, BoxSize, 3 * BoxSize);

			NDrawRectangle drawRectangle = new NDrawRectangle(0, 0, 1, 1);
			drawRectangle.ShowStroke = false;
			shape.Geometry.AddRelative(drawRectangle);

			shape.Geometry.AddRelative(new NMoveTo(0, 0));
			shape.Geometry.AddRelative(new NLineTo(0, 1));
			shape.Geometry.AddRelative(new NMoveTo(1, 0));
			shape.Geometry.AddRelative(new NLineTo(1, 1));

			shape.Ports.Add(CreatePort(Side.Top));
			shape.Ports.Add(CreatePort(Side.Bottom));

			SetProtections(shape);
			return shape;
		}
		private NShape CreateCrossPipe()
		{
			NShape shape = new NShape();
			shape.SetBounds(0, 0, 3 * BoxSize, 3 * BoxSize);

			NDrawPolygon drawPolygon = new NDrawPolygon(0, 0, 1, 1,
				new NPoint[] {
					new NPoint(0, OneThird),
					new NPoint(OneThird, OneThird),
					new NPoint(OneThird, 0),
					new NPoint(TwoThirds, 0),
					new NPoint(TwoThirds, OneThird),
					new NPoint(1, OneThird),
					new NPoint(1, TwoThirds),
					new NPoint(TwoThirds, TwoThirds),
					new NPoint(TwoThirds, 1),
					new NPoint(OneThird, 1),
					new NPoint(OneThird, TwoThirds),
					new NPoint(0, TwoThirds)
			});

			drawPolygon.ShowStroke = false;
			shape.Geometry.AddRelative(drawPolygon);

			shape.Geometry.AddRelative(new NMoveTo(0, OneThird));
			shape.Geometry.AddRelative(new NLineTo(OneThird, OneThird));
			shape.Geometry.AddRelative(new NMoveTo(OneThird, OneThird));
			shape.Geometry.AddRelative(new NLineTo(OneThird, 0));

			shape.Geometry.AddRelative(new NMoveTo(TwoThirds, 0));
			shape.Geometry.AddRelative(new NLineTo(TwoThirds, OneThird));
			shape.Geometry.AddRelative(new NMoveTo(TwoThirds, OneThird));
			shape.Geometry.AddRelative(new NLineTo(1, OneThird));

			shape.Geometry.AddRelative(new NMoveTo(1, TwoThirds));
			shape.Geometry.AddRelative(new NLineTo(TwoThirds, TwoThirds));
			shape.Geometry.AddRelative(new NMoveTo(TwoThirds, TwoThirds));
			shape.Geometry.AddRelative(new NLineTo(TwoThirds, 1));

			shape.Geometry.AddRelative(new NMoveTo(OneThird, 1));
			shape.Geometry.AddRelative(new NLineTo(OneThird, TwoThirds));
			shape.Geometry.AddRelative(new NMoveTo(OneThird, TwoThirds));
			shape.Geometry.AddRelative(new NLineTo(0, TwoThirds));

			shape.Ports.Add(CreatePort(Side.Left));
			shape.Ports.Add(CreatePort(Side.Top));
			shape.Ports.Add(CreatePort(Side.Right));
			shape.Ports.Add(CreatePort(Side.Bottom));

			SetProtections(shape);
			return shape;
		}
		private NShape CreateElbowPipe(string type)
		{
			NDrawPolygon drawPolygon;
			Side ca1, ca2;

			switch (type)
			{
				case "NW":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[] {
						new NPoint(0, OneThird),
						new NPoint(OneThird, OneThird),
						new NPoint(OneThird, 0),
						new NPoint(TwoThirds, 0),
						new NPoint(TwoThirds, TwoThirds),
						new NPoint(0, TwoThirds)
					});

					ca1 = Side.Top;
					ca2 = Side.Left;
					break;

				case "NE":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[] {
						new NPoint(1, OneThird),
						new NPoint(TwoThirds, OneThird),
						new NPoint(TwoThirds, 0),
						new NPoint(OneThird, 0),
						new NPoint(OneThird, TwoThirds),
						new NPoint(1, TwoThirds)
					});

					ca1 = Side.Top;
					ca2 = Side.Right;
					break;

				case "SW":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[] {
						new NPoint(0, TwoThirds),
						new NPoint(OneThird, TwoThirds),
						new NPoint(OneThird, 1),
						new NPoint(TwoThirds, 1),
						new NPoint(TwoThirds, OneThird),
						new NPoint(0, OneThird)
					});

					ca1 = Side.Bottom;
					ca2 = Side.Left;
					break;

				case "SE":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[] {
						new NPoint(1, TwoThirds),
						new NPoint(TwoThirds, TwoThirds),
						new NPoint(TwoThirds, 1),
						new NPoint(OneThird, 1),
						new NPoint(OneThird, OneThird),
						new NPoint(1, OneThird)
					});

					ca1 = Side.Bottom;
					ca2 = Side.Right;
					break;

				default:
					throw new ArgumentException("Unsupported elbow pipe type");
			}

			NShape shape = new NShape();
			shape.SetBounds(0, 0, 3 * BoxSize, 3 * BoxSize);

			drawPolygon.ShowStroke = false;
			shape.Geometry.AddRelative(drawPolygon);

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[0]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[1]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[1]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[2]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[3]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[4]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[4]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[5]));

			shape.Ports.Add(CreatePort(ca1));
			shape.Ports.Add(CreatePort(ca2));

			SetProtections(shape);
			return shape;
		}
		private NShape CreateTPipe(string type)
		{
			NDrawPolygon drawPolygon;
			Side ca1, ca2, ca3;

			switch (type)
			{
				case "NEW":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(0, OneThird),
						new NPoint(OneThird, OneThird),
						new NPoint(OneThird, 0),
						new NPoint(TwoThirds, 0),
						new NPoint(TwoThirds, OneThird),
						new NPoint(1, OneThird),
						new NPoint(1, TwoThirds),
						new NPoint(0, TwoThirds)
					});

					ca1 = Side.Top;
					ca2 = Side.Left;
					ca3 = Side.Right;
					break;

				case "NES":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(1, OneThird),
						new NPoint(TwoThirds, OneThird),
						new NPoint(TwoThirds, 0),
						new NPoint(OneThird, 0),
						new NPoint(OneThird, 1),
						new NPoint(TwoThirds, 1),
						new NPoint(TwoThirds, TwoThirds),
						new NPoint(1, TwoThirds)
					});

					ca1 = Side.Top;
					ca2 = Side.Right;
					ca3 = Side.Bottom;
					break;

				case "NWS":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(0, OneThird),
						new NPoint(OneThird, OneThird),
						new NPoint(OneThird, 0),
						new NPoint(TwoThirds, 0),
						new NPoint(TwoThirds, 1),
						new NPoint(OneThird, 1),
						new NPoint(OneThird, TwoThirds),
						new NPoint(0, TwoThirds)
					});

					ca1 = Side.Top;
					ca2 = Side.Left;
					ca3 = Side.Bottom;
					break;

				case "SEW":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(1, TwoThirds),
						new NPoint(TwoThirds, TwoThirds),
						new NPoint(TwoThirds, 1),
						new NPoint(OneThird, 1),
						new NPoint(OneThird, TwoThirds),
						new NPoint(0, TwoThirds),
						new NPoint(0, OneThird),
						new NPoint(1, OneThird)
					});

					ca1 = Side.Bottom;
					ca2 = Side.Right;
					ca3 = Side.Left;
					break;

				default:
					throw new ArgumentException("Unsupported elbow pipe type");
			}

			NShape shape = new NShape();
			shape.SetBounds(0, 0, 3 * BoxSize, 3 * BoxSize);

			drawPolygon.ShowStroke = false;
			shape.Geometry.AddRelative(drawPolygon);

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[0]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[1]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[1]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[2]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[3]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[4]));

			if (type.Contains("S") && type.Contains("N"))
			{
				shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[5]));
				shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[6]));
			}
			else
			{
				shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[4]));
				shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[5]));
			}

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[6]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[7]));

			shape.Ports.Add(CreatePort(ca1));
			shape.Ports.Add(CreatePort(ca2));
			shape.Ports.Add(CreatePort(ca3));

			SetProtections(shape);
			return shape;
		}
		private NShape CreateEndPipe(string type)
		{
			NDrawPolygon drawPolygon;
			Side ca;

			switch (type)
			{
				case "W":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(1, OneThird),
						new NPoint(TwoThirds, Half),
						new NPoint(1, TwoThirds)
					});

					ca = Side.Right;
					break;

				case "N":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(OneThird, 1),
						new NPoint(Half, TwoThirds),
						new NPoint(TwoThirds, 1)
					});

					ca = Side.Bottom;
					break;

				case "E":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(0, OneThird),
						new NPoint(OneThird, Half),
						new NPoint(0, TwoThirds)
					});

					ca = Side.Left;
					break;

				case "S":
					drawPolygon = new NDrawPolygon(0, 0, 1, 1, new NPoint[]{
						new NPoint(OneThird, 0),
						new NPoint(Half, OneThird),
						new NPoint(TwoThirds, 0)
					});

					ca = Side.Top;
					break;

				default:
					throw new ArgumentException("Unsupported elbow pipe type");
			}

			NShape shape = new NShape();
			shape.SetBounds(0, 0, 3 * BoxSize, 3 * BoxSize);

			drawPolygon.ShowStroke = false;
			shape.Geometry.AddRelative(drawPolygon);

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[0]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[1]));

			shape.Geometry.AddRelative(new NMoveTo(drawPolygon.Points[1]));
			shape.Geometry.AddRelative(new NLineTo(drawPolygon.Points[2]));

			shape.Ports.Add(CreatePort(ca));

			SetProtections(shape);
			return shape;
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPipelineEditorExample.
		/// </summary>
		public static readonly NSchema NPipelineEditorExampleSchema;

		#endregion

		#region Static Methods

		private static void SetProtections(NShape shape)
		{
			shape.AllowResizeX = false;
			shape.AllowResizeY = false;
			shape.AllowRotate = false;
			shape.AllowInplaceEdit = false;
		}

		private static NPort CreatePort(Side side)
		{
			NPort port;
			switch (side)
			{
				case Side.Left:
					port = new NPort(0, 0.5, true);
					port.SetDirection(ENBoxDirection.Left);
					break;
				case Side.Top:
					port = new NPort(0.5, 0, true);
					port.SetDirection(ENBoxDirection.Up);
					break;
				case Side.Right:
					port = new NPort(1, 0.5, true);
					port.SetDirection(ENBoxDirection.Right);
					break;
				case Side.Bottom:
					port = new NPort(0.5, 1, true);
					port.SetDirection(ENBoxDirection.Down);
					break;
				default:
					return null;
			}

			port.GlueMode = ENPortGlueMode.InwardAndOutward;
			return port;
		}

		#endregion

		#region Constants

		private const int BoxSize = 25;

		private const double OneThird = 1.0 / 3.0;
		private const double TwoThirds = 2.0 / 3.0;
		private const double Half = 0.5;

		#endregion

		#region Nested Types

		private enum Side
		{
			Left,
			Top,
			Right,
			Bottom
		}

		#endregion
	}
}