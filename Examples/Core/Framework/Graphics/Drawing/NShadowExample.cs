using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NShadowExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NShadowExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NShadowExample()
		{
			NShadowExampleSchema = NSchema.Create(typeof(NShadowExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Shadow = new NShadow(new NColor(160, 160, 160), 20, 20);

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
				canvas.BackgroundFill = new NColorFill(NColor.White);
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
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
			// get editors for shadow properties
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Shadow).CreatePropertyEditors(
				m_Shadow,
				NShadow.ColorProperty,
                NShadow.AlignXFactorProperty,
                NShadow.AlignYFactorProperty,
				NShadow.OffsetXProperty,
				NShadow.OffsetYProperty,
				NShadow.ScaleXProperty,
				NShadow.ScaleYProperty,
				NShadow.SkewXProperty,
				NShadow.SkewYProperty,
				NShadow.UseFillAndStrokeAlphaProperty,
                NShadow.ApplyToFillingProperty,
				NShadow.ApplyToOutlineProperty);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_Shadow.Changed += new Function<NEventArgs>(OnEditShadowChanged);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the built-in shadows.
	Use the controls to the right to modify various properties of the shadow.
</p>
";
		}

		#endregion

		#region Event Handlers

		void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			NCanvas canvas = args.TargetNode as NCanvas;
			if (canvas == null)
				return;

			PaintPrimitiveDelegate paintDelegate = canvas.Tag as PaintPrimitiveDelegate;
			if (paintDelegate == null)
				throw new Exception("The canvas has no assigned paint delegate.");

			// Clear all styles and set the shadow
			args.PaintVisitor.ClearStyles();
			args.PaintVisitor.SetShadow(m_Shadow);

			// Paint the scene for the current canvas
			paintDelegate(args.PaintVisitor, canvas.Width, canvas.Height);

			// Paint a bounding rectangle for the canvas
			args.PaintVisitor.ClearStyles();
			args.PaintVisitor.SetStroke(NColor.Red, 1);
			args.PaintVisitor.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		void OnEditShadowChanged(NEventArgs args)
		{
			NValueChangeEventArgs localValueChangeArgs = args as NValueChangeEventArgs;

			if (localValueChangeArgs != null)
			{
				InvalidateCanvases();
			}
		}

		#endregion

		#region Implementation

		void InvalidateCanvases()
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

		void PaintLine(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(NColor.Black, 2);

			paintVisitor.PaintLine(0.2 * w, 0.8 * h, 0.7 * w, 0.2 * h);
		}
		void PaintPolyline(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(NColor.Black, 2);

			NPolyline polyline = new NPolyline(5);
			polyline.Add(0.2 * w, 0.2 * h);
			polyline.Add(0.4 * w, 0.3 * h);
			polyline.Add(0.3 * w, 0.5 * h);
			polyline.Add(0.4 * w, 0.7 * h);
			polyline.Add(0.8 * w, 0.8 * h);
			paintVisitor.PaintPolyline(polyline);
		}
		void PaintRectangle(NPaintVisitor paintVisitor, double w, double h)
		{
			NLinearGradientFill lgf = new NLinearGradientFill();
			lgf.GradientStops.Add(new NGradientStop(0, NColor.Indigo));
			lgf.GradientStops.Add(new NGradientStop(0.5f, NColor.SlateBlue));
			lgf.GradientStops.Add(new NGradientStop(1, new NColor(NColor.Crimson, 30)));

			paintVisitor.SetStroke(NColor.Black, 1);
			paintVisitor.SetFill(lgf);

			paintVisitor.PaintRectangle(0.2 * w, 0.3 * h, 0.6 * w, 0.4 * h);
		}
		void PaintEllipse(NPaintVisitor paintVisitor, double w, double h)
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0, NColor.Indigo));
			rgf.GradientStops.Add(new NGradientStop(0.6f, NColor.SlateBlue));
			rgf.GradientStops.Add(new NGradientStop(1, new NColor(NColor.Crimson, 30)));

			paintVisitor.SetStroke(NColor.Black, 1);
			paintVisitor.SetFill(rgf);

			paintVisitor.PaintEllipse(0.2 * w, 0.3 * h, 0.6 * w, 0.4 * h);
		}
		void PaintTriangle(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(NColor.Black, 2);
			paintVisitor.SetFill(NColor.Crimson);

			NPoint p1 = new NPoint(0.5 * w, 0.2 * h);
			NPoint p2 = new NPoint(0.8 * w, 0.8 * h);
			NPoint p3 = new NPoint(0.2 * w, 0.7 * h);

			paintVisitor.PaintTriangle(p1, p2, p3);
		}
		void PaintQuadrangle(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(NColor.Black, 2);
			paintVisitor.SetFill(new NColor(NColor.Crimson, 128));

			NPoint p1 = new NPoint(0.3 * w, 0.2 * h);
			NPoint p2 = new NPoint(0.6 * w, 0.2 * h);
			NPoint p3 = new NPoint(0.8 * w, 0.8 * h);
			NPoint p4 = new NPoint(0.2 * w, 0.6 * h);

			paintVisitor.PaintQuadrangle(p1, p2, p3, p4);
		}
		void PaintPolygon(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(new NColor(0, 0, 0, 160), 6);
			paintVisitor.SetFill(NColor.GreenYellow);

			NPolygon polygon = new NPolygon(6);
			polygon.Add(0.3 * w, 0.2 * h);
			polygon.Add(0.6 * w, 0.2 * h);
			polygon.Add(0.5 * w, 0.4 * h);
			polygon.Add(0.8 * w, 0.8 * h);
			polygon.Add(0.3 * w, 0.7 * h);
			polygon.Add(0.2 * w, 0.4 * h);

			paintVisitor.PaintPolygon(polygon, ENFillRule.EvenOdd);
		}
		void PaintPath(NPaintVisitor paintVisitor, double w, double h)
		{
			paintVisitor.SetStroke(new NColor(0, 0, 0, 160), 6);
			paintVisitor.SetFill(new NColor(NColor.GreenYellow, 128));

			NGraphicsPath path = new NGraphicsPath();
			path.StartFigure(0.2 * w, 0.2 * h);
			path.LineTo(0.7 * w, 0.3 * h);
			path.CubicBezierTo(0.8 * w, 0.8 * h, 1 * w, 0.4 * h, 0.5 * w, 0.7 * h);
			path.LineTo(0.3 * w, 0.7 * h);
			path.CubicBezierTo(0.2 * w, 0.2 * h, 0.3 * w, 0.7 * h, 0.4 * w, 0.6 * h);
			path.CloseFigure();

			paintVisitor.PaintPath(path);
		}

		#endregion

		#region Fields

		NShadow m_Shadow;
		NTableFlowPanel m_Table;

		#endregion

		#region Constants

		private const int DefaultCanvasWidth = 220;
		private const int DefaultCanvasHeight = 220;

		#endregion

		#region Schema

		public static readonly NSchema NShadowExampleSchema;

		#endregion

		#region Nested Types

		delegate void PaintPrimitiveDelegate(NPaintVisitor paintVisitor, double w, double h);

		#endregion
	}
}
