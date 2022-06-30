using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.Framework
{
	public class NStockGradientFillExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NStockGradientFillExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NStockGradientFillExample()
		{
			NStockGradientFillExampleSchema = NSchema.Create(typeof(NStockGradientFillExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the gradient fills
			NFill[] fills;
			string[] texts;

			int columnCount = CreateFillsAndDescriptions(out fills, out texts);

			// Create a table panel to hold the canvases and the labels
			m_Table = new NTableFlowPanel();
			m_Table.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_Table.VerticalPlacement = ENVerticalPlacement.Top;
			m_Table.Padding = new NMargins(30);
			m_Table.HorizontalSpacing = 30;
			m_Table.VerticalSpacing = 30;
			m_Table.MaxOrdinal = columnCount;

			for (int i = 0; i < fills.Length; i++)
			{
				NStackPanel stack = new NStackPanel();
				m_Table.Add(stack);
				stack.Direction = ENHVDirection.TopToBottom;
				stack.FillMode = ENStackFillMode.First;
				stack.FitMode = ENStackFitMode.First;

				// Create a widget with the proper filling
				NCanvas canvas = new NCanvas();
				canvas.PreferredSize = new NSize(defaultCanvasWidth, defaultCanvasHeight);
				canvas.Tag = fills[i];
				stack.Add(canvas);
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

				// Create a label with the corresponding name
				NLabel label = new NLabel(texts[i]);
				stack.Add(label);
				label.HorizontalPlacement = ENHorizontalPlacement.Center;
			}

			// The table must be scrollable
			NScrollContent scroll = new NScrollContent();
			scroll.Content = m_Table;
			return scroll;
		}
		protected override NWidget CreateExampleControls()
		{
			NColorBox colorBox1 = new NColorBox();
			colorBox1.SelectedColor = defaultBeginColor;
			colorBox1.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnColorBoxSelectedColorChanged);
			colorBox1.Tag = NStockGradientFill.BeginColorProperty;

			NColorBox colorBox2 = new NColorBox();
			colorBox2.SelectedColor = defaultEndColor;
			colorBox2.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnColorBoxSelectedColorChanged);
			colorBox2.Tag = NStockGradientFill.EndColorProperty;

			// Canvas width editor
			m_CanvasWidthUpDown = new NNumericUpDown();
			m_CanvasWidthUpDown.Minimum = 100;
			m_CanvasWidthUpDown.Maximum = 300;
			m_CanvasWidthUpDown.Value = defaultCanvasWidth;
			m_CanvasWidthUpDown.Step = 1;
			m_CanvasWidthUpDown.DecimalPlaces = 0;
			m_CanvasWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// Canvas height editor
			m_CanvasHeightUpDown = new NNumericUpDown();
			m_CanvasHeightUpDown.Minimum = 100;
			m_CanvasHeightUpDown.Maximum = 300;
			m_CanvasHeightUpDown.Value = defaultCanvasHeight;
			m_CanvasHeightUpDown.Step = 1;
			m_CanvasHeightUpDown.DecimalPlaces = 0;
			m_CanvasHeightUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// create a stack and put the controls in it
			NStackPanel stack = new NStackPanel();
			stack.Add(NPairBox.Create("Begin Color:", colorBox1));
			stack.Add(NPairBox.Create("End Color:", colorBox2));
			stack.Add(NPairBox.Create("Canvas Width:", m_CanvasWidthUpDown));
			stack.Add(NPairBox.Create("Canvas Height:", m_CanvasHeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example shows the gradient fillings supported by NOV. Use the controls to the right to specify the begin and end colors of the gradients.
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

			NFill fill = (NFill)canvas.Tag;

			args.PaintVisitor.ClearStyles();
			args.PaintVisitor.SetFill(fill);
			args.PaintVisitor.PaintRectangle(0, 0, canvas.Width, canvas.Height);
		}
		private void OnColorBoxSelectedColorChanged(NValueChangeEventArgs args)
		{
			if (m_Table == null)
				return;

			NColor color = (NColor)args.NewValue;
			NColorBox colorBox = (NColorBox)args.TargetNode;
			NProperty property = (NProperty)colorBox.Tag;

			INIterator<NNode> iterator = m_Table.GetSubtreeIterator(ENTreeTraversalOrder.DepthFirstPreOrder, new NInstanceOfSchemaFilter(NCanvas.NCanvasSchema));

			while (iterator.MoveNext())
			{
				NCanvas canvas = (NCanvas)iterator.Current;

				// update the gradient color that corresponds to the changed color box
				((NStockGradientFill)canvas.Tag).SetValue(property, color);

				// Invalidate the canvas
				canvas.InvalidateDisplay();
			}
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

		#endregion

		#region Implementation

		int CreateFillsAndDescriptions(out NFill[] fills, out string[] texts)
		{
			Array gradientStyles = NEnum.GetValues(typeof(ENGradientStyle));
			Array gradientVariants = NEnum.GetValues(typeof(ENGradientVariant));

			int styleCount = gradientStyles.Length;
			int variantCount = gradientVariants.Length;
			int count = styleCount * variantCount;
			int index = 0;

			fills = new NFill[count];
			texts = new string[count];

			// Create the gradient fills
			for (int i = 0; i < variantCount; i++)
			{
				ENGradientVariant variant = (ENGradientVariant)gradientVariants.GetValue(i);

				for (int j = 0; j < styleCount; j++)
				{
					ENGradientStyle style = (ENGradientStyle)gradientStyles.GetValue(j);

					fills[index] = new NStockGradientFill(style, variant, defaultBeginColor, defaultEndColor);
					texts[index] = style.ToString() + " " + variant.ToString();

					index++;
				}
			}

			return styleCount;
		}

		#endregion

		#region Fields

		NTableFlowPanel m_Table;
		NNumericUpDown m_CanvasWidthUpDown;
		NNumericUpDown m_CanvasHeightUpDown;

		#endregion

		#region Constants

		const int defaultCanvasWidth = 160;
		const int defaultCanvasHeight = 100;

		static readonly NColor defaultBeginColor = NColor.Lavender;
		static readonly NColor defaultEndColor = NColor.Indigo;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NStockGradientFillExample.
		/// </summary>
		public static readonly NSchema NStockGradientFillExampleSchema;

		#endregion
	}
}