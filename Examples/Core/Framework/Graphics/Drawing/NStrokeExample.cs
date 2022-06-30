using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NStrokeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default Constructor.
		/// </summary>
		public NStrokeExample()
		{
		}
		/// <summary>
		/// Static Constructor.
		/// </summary>
		static NStrokeExample()
		{
			NStrokeExampleSchema = NSchema.Create(typeof(NStrokeExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			double width = 1;
			NColor color = NColor.Black;

			m_arrStrokes = new NStroke[]
			{
				new NStroke(width, color, ENDashStyle.Solid),
				new NStroke(width, color, ENDashStyle.Dot),
				new NStroke(width, color, ENDashStyle.Dash),
				new NStroke(width, color, ENDashStyle.DashDot),
				new NStroke(width, color, ENDashStyle.DashDotDot),
				new NStroke(width, color, new NDashPattern(2, 2, 2, 2, 0, 2))
			};

			m_EditStroke = new NStroke();
			m_EditStroke.Width = width;
			m_EditStroke.Color = color;
			m_EditStroke.DashCap = ENLineCap.Square;
			m_EditStroke.StartCap = ENLineCap.Square;
			m_EditStroke.EndCap = ENLineCap.Square;

			for (int i = 0; i < m_arrStrokes.Length; i++)
			{
				NStroke stroke = m_arrStrokes[i];
				stroke.DashCap = m_EditStroke.DashCap;
				stroke.StartCap = m_EditStroke.StartCap;
				stroke.EndCap = m_EditStroke.EndCap;
			}

			m_LabelFont = new NFont(NFontDescriptor.DefaultSansFamilyName, 12, ENFontStyle.Bold);
			m_LabelFill = new NColorFill(ENNamedColor.Black);

			m_CanvasStack = new NStackPanel();
			m_CanvasStack.FillMode = ENStackFillMode.None;
			m_CanvasStack.FitMode = ENStackFitMode.None;

			NSize preferredSize = GetCanvasPreferredSize(m_EditStroke.Width);

			for (int i = 0; i < m_arrStrokes.Length; i++)
			{
				NCanvas canvas = new NCanvas();
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
				canvas.PreferredSize = preferredSize;
				canvas.Tag = m_arrStrokes[i];
				canvas.BackgroundFill = new NColorFill(NColor.White);
				m_CanvasStack.Add(canvas);
			}

			// The stack must be scrollable
			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_CanvasStack;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;

			// get editors for stroke properties
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_EditStroke).CreatePropertyEditors(
				m_EditStroke,
				NStroke.WidthProperty,
				NStroke.ColorProperty,
				NStroke.DashCapProperty,
				NStroke.StartCapProperty,
				NStroke.EndCapProperty,
				NStroke.LineJoinProperty);

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_EditStroke.Changed += new Function<NEventArgs>(OnEditStrokeChanged);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to use strokes. You can specify the width and the color of a stroke,
	as well as its dash properties. NOV supports the following dash styles:
	<ul>
		<li>Solid - specifies a solid line.</li>
		<li>Dash - specifies a line consisting of dashes.</li>
		<li>Dot - specifies a line consisting of dots.</li>
		<li>DashDot - specifies a line consisting of a repeating pattern of dash-dot.</li>
		<li>DashDotDot - specifies a line consisting of a repeating pattern of dash-dot-dot.</li>
		<li>Custom - specifies a user-defined custom dash style.</li>
	</ul>

	Use the controls to the right to modify various properties of the stroke.
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

			NStroke stroke = (NStroke)canvas.Tag;

			NPaintVisitor visitor = args.PaintVisitor;
			visitor.SetStroke(stroke);
			visitor.SetFill(null);

			double strokeWidth = stroke.Width;
			double rectWidth = 300;
			double ellipseWidth = 150;
			double polylineWidth = 180;
			double dist = 20;

			double x1 = 10 + strokeWidth / 2;
			double x2 = x1 + rectWidth + dist + strokeWidth;
			double x3 = x2 + ellipseWidth;
			double x4 = x3 + dist + strokeWidth;
			double x5 = x4 + polylineWidth + dist + strokeWidth / 2;
			double y1 = 10 + strokeWidth / 2;
			double y2 = y1 + strokeWidth + 10;
			double y3 = y1 + 50;

			// draw a horizontal line
			visitor.PaintLine(x1, y1, x3, y1);

			// draw a rectangle
			visitor.PaintRectangle(x1, y2, rectWidth, 100);

			// draw an ellipse
			visitor.PaintEllipse(x2, y2, ellipseWidth, 100);

			// draw a polyline
			NPolyline polyLine = new NPolyline(4);
			polyLine.Add(new NPoint(x4, y2 + 90));
			polyLine.Add(new NPoint(x4 + 60, y2));
			polyLine.Add(new NPoint(x4 + 120, y2 + 90));
			polyLine.Add(new NPoint(x4 + 180, y2));
			visitor.PaintPolyline(polyLine);

			// draw text
			string dashStyleName = stroke.DashStyle.ToString();

			visitor.ClearStroke();
			visitor.SetFont(m_LabelFont);
			visitor.SetFill(m_LabelFill);

			NPaintTextRectSettings settings = new NPaintTextRectSettings();
			visitor.PaintString(new NRectangle(x5, y3, 200, 50), dashStyleName, ref settings);
		}
		void OnEditStrokeChanged(NEventArgs args)
		{
			NValueChangeEventArgs localValueChangeArgs = args as NValueChangeEventArgs;

			if (localValueChangeArgs != null)
			{
				for (int i = 0; i < m_arrStrokes.Length; i++)
				{
					NStroke stroke = m_arrStrokes[i];
					stroke.SetValue(localValueChangeArgs.Property, localValueChangeArgs.NewValue);

					NCanvas canvas = m_CanvasStack[i] as NCanvas;
					double strokeWidth = stroke.Width;

					if (strokeWidth < 0)
						strokeWidth = 0;

					canvas.PreferredSize = GetCanvasPreferredSize(strokeWidth);

					if (canvas != null)
					{
						canvas.InvalidateDisplay();
					}
				}
			}
		}

		#endregion

		#region Implementation

		NSize GetCanvasPreferredSize(double strokeWidth)
		{
			return new NSize(850 + 3 * strokeWidth, 150 + 2 * strokeWidth);
		}

		#endregion

		#region Fields

		NFont m_LabelFont;
		NFill m_LabelFill;
		NStroke m_EditStroke;
		NStroke[] m_arrStrokes;
		NStackPanel m_CanvasStack;

		#endregion

		#region Schema

		public static readonly NSchema NStrokeExampleSchema;

		#endregion
	}
}
