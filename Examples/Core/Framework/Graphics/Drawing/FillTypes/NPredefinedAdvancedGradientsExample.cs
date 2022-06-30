using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.Framework
{
	public class NPredefinedAdvancedGradientsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPredefinedAdvancedGradientsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPredefinedAdvancedGradientsExample()
		{
			NPredefinedAdvancedGradientsExampleSchema = NSchema.Create(typeof(NPredefinedAdvancedGradientsExample), NExampleBase.NExampleBaseSchema);
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

			ENAdvancedGradientColorScheme[] predefinedGradientSchemes = NEnum.GetValues<ENAdvancedGradientColorScheme>();

			for (int i = 0; i < predefinedGradientSchemes.Length; i++)
			{
				NStackPanel stack = new NStackPanel();
				m_Table.Add(stack);
				stack.Direction = ENHVDirection.TopToBottom;
				stack.FillMode = ENStackFillMode.First;
				stack.FitMode = ENStackFitMode.First;

				// Create a widget with the proper filling
				NCanvas canvas = new NCanvas();
				canvas.PreferredSize = new NSize(defaultCanvasWidth, defaultCanvasHeight);
				canvas.Tag = NAdvancedGradientFill.Create(predefinedGradientSchemes[i], 0);
				stack.Add(canvas);
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

				// Create a label with the corresponding name
				NLabel label = new NLabel(predefinedGradientSchemes[i].ToString());
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
			NGroupBox groupBox = new NGroupBox("Gradients Variant:");
			NRadioButtonGroup radioGroup = new NRadioButtonGroup();
			groupBox.Content = radioGroup;

			NStackPanel radioButtonsStack = new NStackPanel();
			radioGroup.Content = radioButtonsStack;

			for (int i = 0; i < 16; i++)
			{
				NRadioButton radioButton = new NRadioButton("Variant " + i.ToString());
				radioButtonsStack.Add(radioButton);
			}

			radioGroup.SelectedIndex = 0;
			radioGroup.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnRadioGroupSelectedIndexChanged);

			// Canvas width editor
			m_CanvasWidthUpDown = new NNumericUpDown();
			m_CanvasWidthUpDown.Minimum = 100;
			m_CanvasWidthUpDown.Maximum = 350;
			m_CanvasWidthUpDown.Value = defaultCanvasWidth;
			m_CanvasWidthUpDown.Step = 1;
			m_CanvasWidthUpDown.DecimalPlaces = 0;
			m_CanvasWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// Canvas height editor
			m_CanvasHeightUpDown = new NNumericUpDown();
			m_CanvasHeightUpDown.Minimum = 100;
			m_CanvasHeightUpDown.Maximum = 350;
			m_CanvasHeightUpDown.Value = defaultCanvasHeight;
			m_CanvasHeightUpDown.Step = 1;
			m_CanvasHeightUpDown.DecimalPlaces = 0;
			m_CanvasHeightUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.None;
			stack.FitMode = ENStackFitMode.None;
			stack.Add(groupBox);
			stack.Add(NPairBox.Create("Canvas Width:", m_CanvasWidthUpDown));
			stack.Add(NPairBox.Create("Canvas Height:", m_CanvasHeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the predefined advanced gradient fills provided by NOV. Use the radio buttons on the right to select
	the gradient variant.
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
		private void OnRadioGroupSelectedIndexChanged(NValueChangeEventArgs args)
		{
			if (m_Table == null)
				return;

			ENAdvancedGradientColorScheme[] predefinedGradientSchemes = NEnum.GetValues<ENAdvancedGradientColorScheme>();
			INIterator<NNode> iterator = m_Table.GetSubtreeIterator(ENTreeTraversalOrder.DepthFirstPreOrder, new NInstanceOfSchemaFilter(NCanvas.NCanvasSchema));

			int gradientVariant = (int)args.NewValue;
			int schemeIndex = 0;

			while (iterator.MoveNext())
			{
				NCanvas canvas = (NCanvas)iterator.Current;
				canvas.Tag = NAdvancedGradientFill.Create(predefinedGradientSchemes[schemeIndex++], gradientVariant);
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

		#region Fields

		NTableFlowPanel m_Table;
		NNumericUpDown m_CanvasWidthUpDown;
		NNumericUpDown m_CanvasHeightUpDown;

		#endregion

		#region Constants

		const int defaultCanvasWidth = 220;
		const int defaultCanvasHeight = 136;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPredefinedAdvancedGradientsExample.
		/// </summary>
		public static readonly NSchema NPredefinedAdvancedGradientsExampleSchema;

		#endregion
	}
}