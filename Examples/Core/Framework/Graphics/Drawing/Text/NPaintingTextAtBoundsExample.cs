

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;
using System.Text;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The example demonstrates how to paint text at given bounds
	/// </summary>
	public class NPaintingTextAtBoundsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPaintingTextAtBoundsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPaintingTextAtBoundsExample()
		{
			NPaintingTextAtBoundsExampleSchema = NSchema.Create(typeof(NPaintingTextAtBoundsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Canvas = new NCanvas();
			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
			m_Canvas.BackgroundFill = new NColorFill(NColor.White);

			return m_Canvas;
		}
		protected override NWidget CreateExampleControls()
		{
			m_WrapModeCombo = new NComboBox();
			m_WrapModeCombo.FillFromEnum<ENTextWrapMode>();
			m_WrapModeCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnWrapModeComboSelectedIndexChanged);

			m_HorizontalAlignmentCombo = new NComboBox();
			m_HorizontalAlignmentCombo.FillFromEnum<ENTextHorzAlign>();
			m_HorizontalAlignmentCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnHorizontalAlignmentComboSelectedIndexChanged);

			m_VerticalAlignmentCombo = new NComboBox();
			m_VerticalAlignmentCombo.FillFromEnum<ENTextVertAlign>();
			m_VerticalAlignmentCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnVerticalAlignmentComboSelectedIndexChanged);

			m_SingleLineCheckBox = new NCheckBox("Single Line");
			m_SingleLineCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnSingleLineCheckBoxCheckedChanged);

			m_WidthPercentUpDown = new NNumericUpDown();
			m_WidthPercentUpDown.Value = 50;
			m_WidthPercentUpDown.Minimum = 0;
			m_WidthPercentUpDown.Maximum = 100.0;
			m_WidthPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnWidthPercentValueChanged);

			m_HeightPercentUpDown = new NNumericUpDown();
			m_HeightPercentUpDown.Value = 50;
			m_HeightPercentUpDown.Minimum = 0;
			m_HeightPercentUpDown.Maximum = 100.0;
			m_HeightPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnHeightPercentValueChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;
			stack.Add(NPairBox.Create("Wrap Mode", m_WrapModeCombo));
			stack.Add(NPairBox.Create("Horizontal Alignment", m_HorizontalAlignmentCombo));
			stack.Add(NPairBox.Create("Vertical Alignment", m_VerticalAlignmentCombo));
			stack.Add(m_SingleLineCheckBox);
			stack.Add(NPairBox.Create("Width Percent:", m_WidthPercentUpDown));
			stack.Add(NPairBox.Create("Height Percent:", m_HeightPercentUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
The example demonstrates how to paint text path. Use the controls to the right to modify different parameters of the rectangular paint text settings.
</p>
";
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnCanvasPrePaint(NCanvasPaintEventArgs args)
		{
			NCanvas canvas = args.TargetNode as NCanvas;
			if (canvas == null)
				return;

			NPaintVisitor paintVisitor = args.PaintVisitor;

			NRectangle contentEge = canvas.GetContentEdge();

			// create the text bounds
			double width = contentEge.Width * m_WidthPercentUpDown.Value / 100.0;
			double height = contentEge.Height * m_HeightPercentUpDown.Value / 100.0;
			NPoint center = contentEge.Center;

			NRectangle textBounds = new NRectangle(center.X - width / 2.0, center.Y - height / 2.0, width, height);

			// create the settings
			NPaintTextRectSettings settings = new NPaintTextRectSettings();
			settings.SingleLine = m_SingleLineCheckBox.Checked;
			settings.WrapMode = (ENTextWrapMode)m_WrapModeCombo.SelectedIndex;
			settings.HorzAlign = (ENTextHorzAlign)m_HorizontalAlignmentCombo.SelectedIndex;
			settings.VertAlign = (ENTextVertAlign)m_VerticalAlignmentCombo.SelectedIndex;

			// create the text
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("Paint text at bounds [" + textBounds.X.ToString("0.") + ", " + textBounds.Y.ToString("0.") + "]");
			builder.AppendLine("Horizontal Alignment [" + settings.HorzAlign.ToString() + "]");
			builder.AppendLine("Vertical Alignment [" + settings.VertAlign.ToString() + "]");

			// paint the bounding box
			paintVisitor.ClearStyles();
			paintVisitor.SetFill(NColor.LightBlue);
			paintVisitor.PaintRectangle(textBounds);

			// init font and fill
			paintVisitor.SetFill(NColor.Black);
			paintVisitor.SetFont(new NFont(NFontDescriptor.DefaultSansFamilyName, 10));

			// paint the text
			paintVisitor.PaintString(textBounds, builder.ToString(), ref settings);
		}

		void OnWidthPercentValueChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();	
		}

		void OnHeightPercentValueChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();	
		}

		void OnWrapModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();
		}

		void OnSingleLineCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();
		}

		void OnVerticalAlignmentComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();
		}

		void OnHorizontalAlignmentComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Fields

		NNumericUpDown m_WidthPercentUpDown;
		NNumericUpDown m_HeightPercentUpDown;

		NComboBox m_WrapModeCombo;
		NComboBox m_HorizontalAlignmentCombo;
		NComboBox m_VerticalAlignmentCombo;
		NCheckBox m_SingleLineCheckBox;
		NCanvas m_Canvas;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPaintingTextAtBoundsExample.
		/// </summary>
		public static readonly NSchema NPaintingTextAtBoundsExampleSchema;

		#endregion
	}
}