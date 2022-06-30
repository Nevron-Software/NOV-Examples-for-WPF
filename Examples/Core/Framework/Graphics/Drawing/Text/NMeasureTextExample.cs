

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;
using System.Text;

namespace Nevron.Nov.Examples.Framework
{
	/// <summary>
	/// The example demonstrates how to paint text at location
	/// </summary>
	public class NMeasureTextExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMeasureTextExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMeasureTextExample()
		{
			NMeasureTextExampleSchema = NSchema.Create(typeof(NMeasureTextExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a table panel to hold the canvases and the labels
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			m_Canvas = new NCanvas();
			stack.Add(m_Canvas);

			m_Canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);
			m_Canvas.BackgroundFill = new NColorFill(NColor.White);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			m_TextBox = new NTextBox();
			m_TextBox.Multiline = true;
			m_TextBox.AcceptsEnter = true;
			m_TextBox.MinHeight = 200;
			m_TextBox.Text = "Type some text to measure";
			m_TextBox.TextChanged += new Function<NValueChangeEventArgs>(OnTextBoxTextChanged);

			NStackPanel stack = new NStackPanel();
			stack.Add(new NLabel("Text:"));
			stack.Add(m_TextBox);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
The example demonstrates how to measure text. Type some text in the text box on the right. The blue rectangle shows the measured bounds. 
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

			// create the settings
			NPaintTextRectSettings settings = new NPaintTextRectSettings();
			settings.SingleLine = false;
			settings.WrapMode = ENTextWrapMode.WordWrap;
			settings.HorzAlign = ENTextHorzAlign.Left;
			settings.VertAlign = ENTextVertAlign.Top;

			// create the text
			string text = m_TextBox.Text;

			// calculate the text bounds the text bounds
			double resolution = canvas.GetResolution();
			NFont font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Regular);
			NSize textSize = font.MeasureString(text.ToCharArray(), resolution, contentEge.Width, ref settings);

			NPoint center = contentEge.Center;
			NRectangle textBounds = new NRectangle(
				center.X - textSize.Width / 2.0, 
				center.Y - textSize.Height / 2.0, 
				textSize.Width, 
				textSize.Height);

			// paint the bounding box
			paintVisitor.ClearStyles();
			paintVisitor.SetFill(NColor.LightBlue);
			paintVisitor.PaintRectangle(textBounds);

			// init font and fill
			paintVisitor.SetFill(NColor.Black);
			paintVisitor.SetFont(font);

			// paint the text
			paintVisitor.PaintString(textBounds, text.ToCharArray(), ref settings);

		}

		void OnTextBoxTextChanged(NValueChangeEventArgs arg)
		{
			m_Canvas.InvalidateDisplay();
		}

		#endregion

		#region Fields

		NCanvas m_Canvas;
		NTextBox m_TextBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMeasureTextExample.
		/// </summary>
		public static readonly NSchema NMeasureTextExampleSchema;

		#endregion
	}
}