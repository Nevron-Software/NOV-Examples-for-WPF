using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NPaintingImagesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPaintingImagesExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPaintingImagesExample()
		{
			NPaintingImagesExampleSchema = NSchema.Create(typeof(NPaintingImagesExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_PaintImageInRectangle = false;
			m_PosX = 10;
			m_PosY = 10;
			m_Width = 200;
			m_Height = 200;

			m_Canvas = new NCanvas();
			m_Canvas.PreferredSize = new NSize(800, 600);
			m_Canvas.BackgroundFill = new NColorFill(new NColor(220, 220, 200));
			m_Canvas.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Canvas.VerticalPlacement = ENVerticalPlacement.Center;
			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_Canvas;
			scroll.NoScrollHAlign = ENNoScrollHAlign.Center;
			scroll.NoScrollVAlign = ENNoScrollVAlign.Center;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			NRadioButton drawAtPointButton = new NRadioButton("Draw Image at Point");
			NRadioButton drawInRectButton = new NRadioButton("Draw Image in Rectangle");

			NStackPanel radioStack = new NStackPanel();
			radioStack.Add(drawAtPointButton);
			radioStack.Add(drawInRectButton);

			m_RadioGroup = new NRadioButtonGroup();
			m_RadioGroup.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_RadioGroup.VerticalPlacement = ENVerticalPlacement.Top;
			m_RadioGroup.Content = radioStack;
			m_RadioGroup.SelectedIndex = m_PaintImageInRectangle ? 1 : 0;
			m_RadioGroup.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			// Image X position editor
			m_PositionXUpDown = new NNumericUpDown();
			m_PositionXUpDown.Minimum = 0;
			m_PositionXUpDown.Maximum = 800;
			m_PositionXUpDown.Value = m_PosX;
			m_PositionXUpDown.Step = 1;
			m_PositionXUpDown.DecimalPlaces = 0;
			m_PositionXUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			// Image Y position editor
			m_PositionYUpDown = new NNumericUpDown();
			m_PositionYUpDown.Minimum = 0;
			m_PositionYUpDown.Maximum = 600;
			m_PositionYUpDown.Value = m_PosY;
			m_PositionYUpDown.Step = 1;
			m_PositionYUpDown.DecimalPlaces = 0;
			m_PositionYUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			// Image height editor
			m_WidthUpDown = new NNumericUpDown();
			m_WidthUpDown.Enabled = m_PaintImageInRectangle;
			m_WidthUpDown.Minimum = 0;
			m_WidthUpDown.Maximum = 400;
			m_WidthUpDown.Value = m_Width;
			m_WidthUpDown.Step = 1;
			m_WidthUpDown.DecimalPlaces = 0;
			m_WidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			// Image height editor
			m_HeightUpDown = new NNumericUpDown();
			m_HeightUpDown.Enabled = m_PaintImageInRectangle;
			m_HeightUpDown.Minimum = 0;
			m_HeightUpDown.Maximum = 400;
			m_HeightUpDown.Value = m_Height;
			m_HeightUpDown.Step = 1;
			m_HeightUpDown.DecimalPlaces = 0;
			m_HeightUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;
			stack.Add(m_RadioGroup);
			stack.Add(NPairBox.Create("X Position:", m_PositionXUpDown));
			stack.Add(NPairBox.Create("Y Position:", m_PositionYUpDown));
			stack.Add(NPairBox.Create("Width:", m_WidthUpDown));
			stack.Add(NPairBox.Create("Height:", m_HeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the image painting capabilities of the NOV graphics.
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

			pv.ClearStyles();

			if (m_PaintImageInRectangle)
			{
				pv.PaintImage(NResources.Image_JpegSuite_q080_jpg.ImageSource, new NRectangle(m_PosX, m_PosY, m_Width, m_Height));
			}
			else
			{
				pv.PaintImage(NResources.Image_JpegSuite_q080_jpg.ImageSource, new NPoint(m_PosX, m_PosY));
			}

			// paint a border around the canvas
			pv.SetStroke(NColor.Black, 1);
			pv.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		private void OnValueChanged(NValueChangeEventArgs args)
		{
			m_PaintImageInRectangle = (m_RadioGroup.SelectedIndex == 1);
			m_PosX = m_PositionXUpDown.Value;
			m_PosY = m_PositionYUpDown.Value;
			m_Width = m_WidthUpDown.Value;
			m_Height = m_HeightUpDown.Value;

			m_WidthUpDown.Enabled = m_PaintImageInRectangle;
			m_HeightUpDown.Enabled = m_PaintImageInRectangle;

			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Fields

		NCanvas m_Canvas;
		NRadioButtonGroup m_RadioGroup;
		NNumericUpDown m_PositionXUpDown;
		NNumericUpDown m_PositionYUpDown;
		NNumericUpDown m_WidthUpDown;
		NNumericUpDown m_HeightUpDown;

		bool m_PaintImageInRectangle;
		double m_PosX;
		double m_PosY;
		double m_Width;
		double m_Height;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPaintingImagesExample.
		/// </summary>
		public static readonly NSchema NPaintingImagesExampleSchema;

		#endregion
	}
}