using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Editors;
using Nevron.Nov.DataStructures;

namespace Nevron.Nov.Examples.Framework
{
	public class NPaintingPrimitivesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPaintingPrimitivesExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPaintingPrimitivesExample()
		{
			NPaintingPrimitivesExampleSchema = NSchema.Create(typeof(NPaintingPrimitivesExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			string[] names = new string[]
			{
				"Line", "Polyline", "Rectangle", "Ellipse", "Triangle", "Quad", "Polygon", "Graphics Path"
			};

			PaintPrimitiveDelegate[] delegates = new PaintPrimitiveDelegate[]
			{
				new PaintPrimitiveDelegate(PaintLine),
				new PaintPrimitiveDelegate(PaintPolyline),
				new PaintPrimitiveDelegate(PaintRectangle),
				new PaintPrimitiveDelegate(PaintEllipse),
				new PaintPrimitiveDelegate(PaintTriangle),
				new PaintPrimitiveDelegate(PaintQuadrangle),
				new PaintPrimitiveDelegate(PaintPolygon),
				new PaintPrimitiveDelegate(PaintPath),
			};

			int count = delegates.Length;

			// Create a table panel to hold the canvases and the labels
			m_Table = new NTableFlowPanel();
			m_Table.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_Table.VerticalPlacement = ENVerticalPlacement.Top;
			m_Table.Padding = new NMargins(30);
			m_Table.HorizontalSpacing = 30;
			m_Table.VerticalSpacing = 30;
			m_Table.MaxOrdinal = 4;

			for (int i = 0; i < count; i++)
			{
				NStackPanel stack = new NStackPanel();
				m_Table.Add(stack);
				stack.VerticalSpacing = 5;

				// Create a canvas to draw in
				NCanvas canvas = new NCanvas();
				canvas.PreferredSize = new NSize(DefaultCanvasWidth, DefaultCanvasHeight);
				canvas.Tag = delegates[i];
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
				canvas.BackgroundFill = new NColorFill(NColor.LightBlue);
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
			m_StrokeColorBox.SelectedColorChanged += OnStrokeColorBoxSelectedColorChanged;

			// Stroke width
			m_StrokeWidthCombo = new NComboBox();
			for (int i = 0; i < 6; i++)
			{
				m_StrokeWidthCombo.Items.Add(new NComboBoxItem(i.ToString()));
			}
			m_StrokeWidthCombo.SelectedIndex = 1;
			m_StrokeWidthCombo.SelectedIndexChanged += OnStrokeWidthComboSelectedIndexChanged;

			// Canvas width editor
			m_CanvasWidthUpDown = new NNumericUpDown();
			m_CanvasWidthUpDown.Minimum = 100;
			m_CanvasWidthUpDown.Maximum = 350;
			m_CanvasWidthUpDown.Value = DefaultCanvasWidth;
			m_CanvasWidthUpDown.Step = 1;
			m_CanvasWidthUpDown.DecimalPlaces = 0;
			m_CanvasWidthUpDown.ValueChanged += OnNumericUpDownValueChanged;

			// Canvas height editor
			m_CanvasHeightUpDown = new NNumericUpDown();
			m_CanvasHeightUpDown.Minimum = 100;
			m_CanvasHeightUpDown.Maximum = 350;
			m_CanvasHeightUpDown.Value = DefaultCanvasHeight;
			m_CanvasHeightUpDown.Step = 1;
			m_CanvasHeightUpDown.DecimalPlaces = 0;
			m_CanvasHeightUpDown.ValueChanged += OnNumericUpDownValueChanged;

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
	This example demonstrates the primitive painting capabilities of the NOV graphics.
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

			PaintPrimitiveDelegate paintDelegate = canvas.Tag as PaintPrimitiveDelegate;
			if (paintDelegate == null)
				throw new Exception("The canvas has no assigned paint delegate.");

			args.PaintVisitor.ClearStyles();
			args.PaintVisitor.SetStroke(m_Stroke);
			args.PaintVisitor.SetFill(m_Fill);

			paintDelegate(args.PaintVisitor, canvas.Width, canvas.Height);
		}
		private void OnNumericUpDownValueChanged(NValueChangeEventArgs args)
		{
			if (m_Table == null)
				return;

			double width = m_CanvasWidthUpDown.Value;
			double height = m_CanvasHeightUpDown.Value;

			// Resize the canvases
			INIterator<NNode> iterator = m_Table.GetSubtreeIterator(ENTreeTraversalOrder.DepthFirstPreOrder, new NInstanceOfSchemaFilter(NCanvas.NCanvasSchema));

			while (iterator.MoveNext())
			{
				NCanvas canvas = (NCanvas)iterator.Current;

				((NWidget)canvas.ParentNode).PreferredWidth = width;
				canvas.PreferredHeight = height;
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

		#region Schema

		/// <summary>
		/// Schema associated with NPaintingPrimitivesExample.
		/// </summary>
		public static readonly NSchema NPaintingPrimitivesExampleSchema;

		#endregion

		#region Static Methods

		private static void PaintLine(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.PaintLine(0.2 * w, 0.1 * h, 0.9 * w, 0.8 * h);
			paintVisitor.PaintLine(0.1 * w, 0.4 * h, 0.8 * w, 0.9 * h);
		}
		private static void PaintPolyline(NPaintVisitor paintVisitor, double w, double h)
		{
			NPolyline polyline = new NPolyline(5);
			polyline.Add(0.1 * w, 0.1 * h);
			polyline.Add(0.4 * w, 0.2 * h);
			polyline.Add(0.2 * w, 0.5 * h);
			polyline.Add(0.3 * w, 0.8 * h);
			polyline.Add(0.9 * w, 0.9 * h);
			paintVisitor.PaintPolyline(polyline);
		}
		private static void PaintRectangle(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.PaintRectangle(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
		}
		private static void PaintEllipse(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.PaintEllipse(0.05 * w, 0.25 * h, 0.9 * w, 0.5 * h);
		}
		private static void PaintTriangle(NPaintVisitor paintVisitor, double w, double h)
		{
			NPoint p1 = new NPoint(0.5 * w, 0.1 * h);
			NPoint p2 = new NPoint(0.9 * w, 0.9 * h);
			NPoint p3 = new NPoint(0.1 * w, 0.8 * h);

			paintVisitor.PaintTriangle(p1, p2, p3);
		}
		private static void PaintQuadrangle(NPaintVisitor paintVisitor, double w, double h)
		{
			NPoint p1 = new NPoint(0.2 * w, 0.1 * h);
			NPoint p2 = new NPoint(0.6 * w, 0.1 * h);
			NPoint p3 = new NPoint(0.9 * w, 0.9 * h);
			NPoint p4 = new NPoint(0.1 * w, 0.6 * h);

			paintVisitor.PaintQuadrangle(p1, p2, p3, p4);
		}
		private static void PaintPolygon(NPaintVisitor paintVisitor, double w, double h)
		{
			NPolygon polygon = new NPolygon(6);
			polygon.Add(0.3 * w, 0.1 * h);
			polygon.Add(0.7 * w, 0.1 * h);
			polygon.Add(0.5 * w, 0.4 * h);
			polygon.Add(0.9 * w, 0.9 * h);
			polygon.Add(0.2 * w, 0.8 * h);
			polygon.Add(0.1 * w, 0.4 * h);

			paintVisitor.PaintPolygon(polygon, ENFillRule.EvenOdd);
		}
		private static void PaintPath(NPaintVisitor paintVisitor, double w, double h)
		{
			NGraphicsPath path = new NGraphicsPath();
			path.StartFigure(0.1 * w, 0.1 * h);
			path.LineTo(0.7 * w, 0.2 * h);
			path.CubicBezierTo(0.9 * w, 0.9 * h, 1 * w, 0.4 * h, 0.5 * w, 0.7 * h);
			path.LineTo(0.2 * w, 0.8 * h);
			path.CubicBezierTo(0.1 * w, 0.1 * h, 0.3 * w, 0.7 * h, 0.4 * w, 0.6 * h);
			path.CloseFigure();

			paintVisitor.PaintPath(path);
		}

		#endregion

		#region Constants

		private const int DefaultCanvasWidth = 220;
		private const int DefaultCanvasHeight = 220;

		#endregion

		#region Nested Types

		delegate void PaintPrimitiveDelegate(NPaintVisitor paintVisitor, double w, double h);

		#endregion
	}
}