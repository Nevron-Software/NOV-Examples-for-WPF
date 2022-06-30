using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NPaintingGraphicsPathsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPaintingGraphicsPathsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPaintingGraphicsPathsExample()
		{
			NPaintingGraphicsPathsExampleSchema = NSchema.Create(typeof(NPaintingGraphicsPathsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a table panel to hold the canvases and the labels
			m_Table = new NTableFlowPanel();
			m_Table.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_Table.VerticalPlacement = ENVerticalPlacement.Top;
			m_Table.Padding = new NMargins(30);
			m_Table.HorizontalSpacing = 30;
			m_Table.VerticalSpacing = 30;
			m_Table.MaxOrdinal = 4;

			string[] names = new string[]
			{
				"Rectangle",
				"Rounded Rectangle",
				"Ellipse",
				"Ellipse Segment",
				"Elliptical Arc",
				"Pie",
				"Circle",
				"Circle Segment", 
				"Triangle",
				"Quad",
				"Polygon",
				"Line Segment",
				"Polyline",
				"Cubic Bezier",
				"Nurbs Curve",
				"Path with Multiple Figures"
			};

			NGraphicsPath[] paths = CreatePaths(DefaultCanvasWidth, DefaultCanvasHeight);

			int count = paths.Length;

			for (int i = 0; i < count; i++)
			{
				NStackPanel stack = new NStackPanel();
				m_Table.Add(stack);
				stack.VerticalSpacing = 5;

				// Create a canvas to draw the graphics path in
				NCanvas canvas = new NCanvas();
				canvas.PreferredSize = new NSize(DefaultCanvasWidth, DefaultCanvasHeight);
				canvas.Tag = paths[i];
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
				canvas.BackgroundFill = new NColorFill(NColor.LightGreen);
				stack.Add(canvas);

				// Create a label for the geometry primitive's name
				NLabel label = new NLabel(names[i]);
				label.HorizontalPlacement = ENHorizontalPlacement.Center;
				stack.Add(label);
			}

			// The table must be scrollable
			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_Table;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			// Fill
			m_FillSplitButton = new NFillSplitButton();
			m_FillSplitButton.SelectedValueChanged += OnFillSplitButtonSelectedValueChanged;

			// Stroke color
			m_StrokeColorBox = new NColorBox();
			m_StrokeColorBox.SelectedColor = m_Stroke.Color;
			m_StrokeColorBox.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnStrokeColorBoxSelectedColorChanged);

			// Stroke width
			m_StrokeWidthCombo = new NComboBox();
			for (int i = 0; i < 6; i++)
			{
				m_StrokeWidthCombo.Items.Add(new NComboBoxItem(i.ToString()));
			}
			m_StrokeWidthCombo.SelectedIndex = 1;
			m_StrokeWidthCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnStrokeWidthComboSelectedIndexChanged);

			// Canvas width editor
			m_CanvasWidthUpDown = new NNumericUpDown();
			m_CanvasWidthUpDown.Minimum = 100;
			m_CanvasWidthUpDown.Maximum = 350;
			m_CanvasWidthUpDown.Value = DefaultCanvasWidth;
			m_CanvasWidthUpDown.Step = 1;
			m_CanvasWidthUpDown.DecimalPlaces = 0;
			m_CanvasWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// Canvas height editor
			m_CanvasHeightUpDown = new NNumericUpDown();
			m_CanvasHeightUpDown.Minimum = 100;
			m_CanvasHeightUpDown.Maximum = 350;
			m_CanvasHeightUpDown.Value = DefaultCanvasHeight;
			m_CanvasHeightUpDown.Step = 1;
			m_CanvasHeightUpDown.DecimalPlaces = 0;
			m_CanvasHeightUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;
			stack.Add(NPairBox.Create("Fill:", m_FillSplitButton));
			stack.Add(NPairBox.Create("Stroke Color:", m_StrokeColorBox));
			stack.Add(NPairBox.Create("Stroke Width:", m_StrokeWidthCombo));
			stack.Add(NPairBox.Create("Canvas Width:", m_CanvasWidthUpDown));
			stack.Add(NPairBox.Create("Canvas Height:", m_CanvasHeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the graphics path's capabilities for painting of geometric primitives.
	You can use the controls in the right-side panel to change the fill and stroke of the graphics paths.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			NCanvas canvas = args.TargetNode as NCanvas;
			if (canvas == null)
				return;

			NGraphicsPath path = (NGraphicsPath)canvas.Tag;

			args.PaintVisitor.ClearStyles();
			args.PaintVisitor.SetStroke(m_Stroke);
			args.PaintVisitor.SetFill(m_Fill);
			args.PaintVisitor.PaintPath(path, ENFillRule.EvenOdd);
		}
		private void OnNumericUpDownValueChanged(NValueChangeEventArgs args)
		{
			if (m_Table == null)
				return;

			double width = m_CanvasWidthUpDown.Value;
			double height = m_CanvasHeightUpDown.Value;

			// Recreate all graphics paths
			NGraphicsPath[] paths = CreatePaths(width, height);
			int index = 0;

			// Resize the canvases
			INIterator<NNode> iterator = m_Table.GetSubtreeIterator(ENTreeTraversalOrder.DepthFirstPreOrder, new NInstanceOfSchemaFilter(NCanvas.NCanvasSchema));

			while (iterator.MoveNext())
			{
				NCanvas canvas = (NCanvas)iterator.Current;

				((NWidget)canvas.ParentNode).PreferredWidth = width;
				canvas.PreferredHeight = height;

				canvas.Tag = paths[index++];
			}
		}
		private void OnFillSplitButtonSelectedValueChanged(NValueChangeEventArgs arg)
		{
			NAutomaticValue<NFill> selectedFill = (NAutomaticValue<NFill>)arg.NewValue;
			m_Fill = selectedFill.Automatic ? null : selectedFill.Value;

			InvalidateCanvases();
		}
		private void OnStrokeColorBoxSelectedColorChanged(NValueChangeEventArgs arg)
		{
			m_Stroke.Color = m_StrokeColorBox.SelectedColor;

			InvalidateCanvases();
		}
		private void OnStrokeWidthComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Stroke.Width = m_StrokeWidthCombo.SelectedIndex;

			InvalidateCanvases();
		}

		#endregion

		#region Implementation

		private void InvalidateCanvases()
		{
			if (m_Table == null)
				return;

			INIterator<NNode> iterator = m_Table.GetSubtreeIterator(ENTreeTraversalOrder.DepthFirstPreOrder, new NInstanceOfSchemaFilter(NCanvas.NCanvasSchema));

			while (iterator.MoveNext())
			{
				NCanvas canvas = (NCanvas)iterator.Current;
				canvas.InvalidateDisplay();
			}
		}

		private NGraphicsPath[] CreatePaths(double w, double h)
		{
			NGraphicsPath[] paths = new NGraphicsPath[]
			{
				CreateRectangle(w, h),
				CreateRoundedRectangle(w, h),
				CreateEllipse(w, h),
				CreateEllipseSegment(w, h),
				CreateEllipticalArc(w, h),
				CreatePie(w, h),
				CreateCircle(w, h),
				CreateCircleSegment(w, h),
				CreateTriangle(w, h),
				CreateQuad(w, h),
				CreatePolygon(w, h),
				CreateLineSegment(w, h),
				CreatePolyline(w, h),
				CreateCubicBezier(w, h),
				CreateNurbsCurve(w, h),
				CreatePathWithMultipleFigures(w, h)
			};

			return paths;
		}

		#endregion

		#region Fields

		private NTableFlowPanel m_Table;
		private NNumericUpDown m_CanvasWidthUpDown;
		private NNumericUpDown m_CanvasHeightUpDown;
		private NFillSplitButton m_FillSplitButton;
		private NColorBox m_StrokeColorBox;
		private NComboBox m_StrokeWidthCombo;

		private NStroke m_Stroke = new NStroke();
		private NFill m_Fill;

		#endregion

		#region Static Methods

		private static NGraphicsPath CreateRectangle(double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.AddRectangle(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
			return path;
		}
		private static NGraphicsPath CreateRoundedRectangle(double w, double h)
		{
			NRectangle rect = new NRectangle(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
			NGraphicsPath path = new NGraphicsPath();
			path.AddRoundedRectangle(rect, h * 0.05);
			return path;
		}
		private static NGraphicsPath CreateEllipse(double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.AddEllipse(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
			return path;
		}
		private static NGraphicsPath CreateEllipseSegment(double w, double h)
		{
			NRectangle rect = new NRectangle(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
			NGraphicsPath path = new NGraphicsPath();
			path.AddEllipseSegment(rect, NMath.PI * 0.1, NMath.PI * 1.2);
			return path;
		}
		private static NGraphicsPath CreateEllipticalArc(double w, double h)
		{
			NPoint start = new NPoint(w * 0.3, h * 0.85);
			NPoint control = new NPoint(w * 0.5, h * 0.15);
			NPoint end = new NPoint(w * 0.8, h * 0.85);
			double angle = 1;
			double ratio = 1.4;

			NGraphicsPath path = new NGraphicsPath();
			path.AddEllipticalArc(start, control, end, angle, ratio);
			return path;
		}
		private static NGraphicsPath CreatePie(double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.AddPie(0.1 * w, 0.1 * h, 0.8 * w, 0.8 * h, 0.25 * NMath.PI, 1.5 * NMath.PI);
			return path;
		}
		private static NGraphicsPath CreateCircle(double w, double h)
		{
			double radius = 0.4 * NMath.Min(w, h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddCircle(0.5 * w, 0.5 * h, radius);
			return path;
		}
		private static NGraphicsPath CreateCircleSegment(double w, double h)
		{
			double radius = 0.4 * NMath.Min(w, h);
			NPoint center = new NPoint(0.5 * w, 0.5 * h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddCircleSegment(center, radius, 0.25 * NMath.PI, 1.5 * NMath.PI);
			return path;
		}
		private static NGraphicsPath CreateTriangle(double w, double h)
		{
			NTriangle triangle = new NTriangle();
			triangle.A = new NPoint(0.5 * w, 0.1 * h);
			triangle.B = new NPoint(0.9 * w, 0.9 * h);
			triangle.C = new NPoint(0.1 * w, 0.8 * h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddTriangle(triangle);
			return path;
		}
		private static NGraphicsPath CreateQuad(double w, double h)
		{
			NQuadrangle quad = new NQuadrangle();
			quad.A = new NPoint(0.2 * w, 0.1 * h);
			quad.B = new NPoint(0.6 * w, 0.1 * h);
			quad.C = new NPoint(0.9 * w, 0.9 * h);
			quad.D = new NPoint(0.1 * w, 0.6 * h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddQuad(quad);
			return path;
		}
		private static NGraphicsPath CreatePolygon(double w, double h)
		{
			NPolygon polygon = new NPolygon(6);
			polygon.Add(0.3 * w, 0.1 * h);
			polygon.Add(0.7 * w, 0.1 * h);
			polygon.Add(0.5 * w, 0.4 * h);
			polygon.Add(0.9 * w, 0.9 * h);
			polygon.Add(0.2 * w, 0.8 * h);
			polygon.Add(0.1 * w, 0.4 * h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddPolygon(polygon);
			return path;
		}
		private static NGraphicsPath CreateLineSegment(double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.AddLineSegment(0.2 * w, 0.1 * h, 0.9 * w, 0.9 * h);
			return path;
		}
		private static NGraphicsPath CreatePolyline(double w, double h)
		{
			NPoint[] points = new NPoint[]
			{
				new NPoint(0.1 * w, 0.1 * h),
				new NPoint(0.9 * w, 0.3 * h),
				new NPoint(0.2 * w, 0.6 * h),
				new NPoint(0.8 * w, 0.7 * h),
				new NPoint(0.6 * w, 0.9 * h)
			};

			NGraphicsPath path = new NGraphicsPath();
			path.AddPolyline(points);
			return path;
		}
		private static NGraphicsPath CreateCubicBezier(double w, double h)
		{
			NPoint start = new NPoint(0.1 * w, 0.1 * h);
			NPoint c1 = new NPoint(0.8 * w, 0.0 * h);
			NPoint c2 = new NPoint(0.5 * w, 1.0 * h);
			NPoint end = new NPoint(0.9 * w, 0.9 * h);

			NGraphicsPath path = new NGraphicsPath();
			path.AddCubicBezier(start, c1, c2, end);
			return path;
		}
		private static NGraphicsPath CreateNurbsCurve(double w, double h)
		{
			NNurbsCurve nurbs = new NNurbsCurve(3);
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.05 * w, 0.50 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.20 * w, 0.20 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.40 * w, 0.00 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.60 * w, 0.50 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.60 * w, 0.70 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.30 * w, 0.95 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.40 * w, 0.50 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.90 * w, 0.00 * h));
			nurbs.ControlPoints.Add(new NNurbsControlPoint(0.95 * w, 0.50 * h));

			nurbs.Knots.AddRange(new double[] { 0, 0, 0, 0, 1, 2, 3, 4, 5, 6, 6, 6, 6 });

			NGraphicsPath path = new NGraphicsPath();
			path.AddNurbsCurve(nurbs);
			return path;
		}
		private static NGraphicsPath CreatePathWithMultipleFigures(double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.StartFigure(0.05 * w, 0.05 * h);
			path.LineTo(0.4 * w, 0.2 * h);
			path.LineTo(0.54 * w, 0.5 * h);
			path.CircularArcTo(new NPoint(0.05 * w, 0.5 * h), 12);
			path.CloseFigure();

			path.StartFigure(0.05 * w, 0.95 * h);
			path.LineTo(0.05 * w, 0.6 * h);
			path.CubicBezierTo(0.6 * w, 0.7 * h, 0.2 * w, 0.1 * h, 0.5 * w, 0.7 * h);
			path.LineTo(0.6 * w, 0.95 * h);
			path.CloseFigure();

			NPoint[] points = new NPoint[]
			{
				new NPoint(0.95 * w, 0.05 * h),
				new NPoint(0.95 * w, 0.95 * h),
				new NPoint(0.65 * w, 0.95 * h),
				new NPoint(0.65 * w, 0.60 * h),
			};

			path.StartFigure(0.4 * w, 0.05 * h);
			path.LineTos(points, 0, points.Length);
			path.CloseFigure();

			path.StartFigure(0.7 * w, 0.1 * h);
			path.EllipticalArcTo(new NPoint(0.7 * w, 0.3 * h), new NPoint(0.9 * w, 0.2 * h), 0, 2);
			path.CloseFigure();

			path.StartFigure(0.7 * w, 0.6 * h);
			path.CircularArcTo(new NPoint(0.7 * w, 0.8 * h), 0.2 * w);
			path.CloseFigure();

			return path;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPaintingGraphicsPathsExample.
		/// </summary>
		public static readonly NSchema NPaintingGraphicsPathsExampleSchema;

		#endregion

		#region Constants

		private const int DefaultCanvasWidth = 220;
		private const int DefaultCanvasHeight = 220;

		#endregion
	}
}