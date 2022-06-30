using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NSolidColorFillExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSolidColorFillExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSolidColorFillExample()
		{
			NSolidColorFillExampleSchema = NSchema.Create(typeof(NSolidColorFillExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Canvas = new NCanvas();
			m_Canvas.PreferredSize = new NSize(W, H);
			m_Canvas.BackgroundFill = new NHatchFill(ENHatchStyle.LargeCheckerBoard, NColor.LightGray, NColor.White);
			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

			m_Canvas.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Canvas.VerticalPlacement = ENVerticalPlacement.Center;

			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_Canvas;
			scroll.NoScrollHAlign = ENNoScrollHAlign.Center;
			scroll.NoScrollVAlign = ENNoScrollVAlign.Center;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			// Create background color editor
			NColorBox colorBox = new NColorBox();
			colorBox.SelectedColor = m_ColorFills[0].Color;
			colorBox.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnColorBoxSelectedColorChanged);

			NStackPanel stack = new NStackPanel();
			stack.Add(NPairBox.Create("Detached Slice's Color:", colorBox));
			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the most common fill type - the solid color fill. A solid color fill paints the interior of a shape or graphics path with a single solid color (opaque or semi-transparent).
	In this example each pie slice is filled with a different solid color fill. You can change the color of the detached pie slice using the color combo box in the upper-right corner.
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

			NPaintVisitor pv = args.PaintVisitor;
			
			int count = m_Values.Length;

			// calculate total value
			double total = 0;
			for (int i = 0; i < count; i++)
			{
				total += m_Values[i];
			}

			// paint the pie slices
			double beginAngle = 0;

			pv.ClearStyles();

			for (int i = 0; i < count; i++)
			{
				double sweepAngle = NMath.PI2 * (m_Values[i] / total);

				NGraphicsPath path = new NGraphicsPath();
				path.AddPie(0.1 * W, 0.1 * H, 0.8 * W, 0.8 * H, beginAngle, sweepAngle);

				if (i == 0)
				{
					const double detachment = 20;
					double midAngle = beginAngle + sweepAngle / 2;
					double dx = Math.Cos(midAngle) * detachment;
					double dy = Math.Sin(midAngle) * detachment;
					path.Translate(dx, dy);
				}

				pv.SetFill(m_ColorFills[i]);
				pv.PaintPath(path);

				beginAngle += sweepAngle;
			}

			// paint a border around the canvas
			pv.ClearFill();
			pv.SetStroke(NColor.Black, 1);
			pv.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		private void OnColorBoxSelectedColorChanged(NValueChangeEventArgs args)
		{
			m_ColorFills[0] = new NColorFill((NColor)args.NewValue);

			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Fields

		NCanvas m_Canvas;

		NColorFill[] m_ColorFills = new NColorFill[]
		{
			new NColorFill(NColor.FromColor(NColor.IndianRed, 0.5f)), // semi-transparent
			new NColorFill(NColor.Peru),
			new NColorFill(NColor.DarkKhaki),
			new NColorFill(NColor.OliveDrab),
			new NColorFill(NColor.DarkSeaGreen),
			new NColorFill(NColor.MediumSeaGreen),
			new NColorFill(NColor.SteelBlue),
			new NColorFill(NColor.SlateBlue),
			new NColorFill(NColor.MediumOrchid),
			new NColorFill(NColor.HotPink)
		};

		double[] m_Values = new double[] { 40, 20, 15, 19, 27, 29, 21, 32, 19, 14 };

		#endregion

		#region Constants

		private const int W = 400;
		private const int H = 400;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSolidColorFillExample.
		/// </summary>
		public static readonly NSchema NSolidColorFillExampleSchema;

		#endregion
	}
}