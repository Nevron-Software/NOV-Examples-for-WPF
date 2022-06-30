

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
	/// The example demonstrates how to paint text at location
	/// </summary>
	public class NPaintingTextAtLocationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPaintingTextAtLocationExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPaintingTextAtLocationExample()
		{
			NPaintingTextAtLocationExampleSchema = NSchema.Create(typeof(NPaintingTextAtLocationExample), NExampleBase.NExampleBaseSchema);
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
			m_HorizontalAlignmentCombo = new NComboBox();
			m_HorizontalAlignmentCombo.FillFromEnum<ENTextHorzAlign>();
			m_HorizontalAlignmentCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnHorizontalAlignmentComboSelectedIndexChanged);

			m_VerticalAlignmentCombo = new NComboBox();
			m_VerticalAlignmentCombo.FillFromEnum<ENTextVertAlign>();
			m_VerticalAlignmentCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnVerticalAlignmentComboSelectedIndexChanged);
			
			m_SingleLineCheckBox = new NCheckBox("Single Line");
			m_SingleLineCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnSingleLineCheckBoxCheckedChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;

			stack.Add(NPairBox.Create("Horizontal Alignment", m_HorizontalAlignmentCombo));
			stack.Add(NPairBox.Create("Vertical Alignment", m_VerticalAlignmentCombo));
			stack.Add(m_SingleLineCheckBox);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
The example demonstrates how to paint text at location. Use the controls to the right to modify different parameters of the point paint text settings.
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

			NPaintTextPointSettings settings = new NPaintTextPointSettings();

			settings.SingleLine = m_SingleLineCheckBox.Checked;
			settings.VertAlign = (ENTextVertAlign)m_VerticalAlignmentCombo.SelectedItem.Tag;
			settings.HorzAlign = (ENTextHorzAlign)m_HorizontalAlignmentCombo.SelectedItem.Tag;

			NPoint location = canvas.GetContentEdge().Center;

			// set styles
			paintVisitor.ClearStyles();
			paintVisitor.SetFont(new NFont(NFontDescriptor.DefaultSansFamilyName, 10));
			paintVisitor.SetFill(NColor.Black);

			// create text to paint
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("Paint text at location [" + location.X.ToString("0.") + ", " + location.Y.ToString("0.") + "]");
			builder.AppendLine("Horizontal Alignment [" + settings.HorzAlign.ToString() + "]");
			builder.AppendLine("Vertical Alignment [" + settings.VertAlign.ToString() + "]");

			// paint string
			paintVisitor.PaintString(location, builder.ToString(), ref settings);

			// paint location
			double inflate = 5.0;
			paintVisitor.SetFill(NColor.Red);
			paintVisitor.PaintRectangle(new NRectangle(location.X - inflate, location.Y - inflate, inflate * 2.0, inflate * 2.0));
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

		NComboBox m_HorizontalAlignmentCombo;
		NComboBox m_VerticalAlignmentCombo;
		NCheckBox m_SingleLineCheckBox;
		NCanvas m_Canvas;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPaintingTextAtLocationExample.
		/// </summary>
		public static readonly NSchema NPaintingTextAtLocationExampleSchema;

		#endregion
	}
}