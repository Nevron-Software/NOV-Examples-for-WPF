using System;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NRadialGradientFillExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRadialGradientFillExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRadialGradientFillExample()
		{
			NRadialGradientFillExampleSchema = NSchema.Create(typeof(NRadialGradientFillExample), NExampleBase.NExampleBaseSchema);
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
			m_Table.MaxOrdinal = 2;

			string[] texts = new string[]
			{
				"Two Gradient Stops (Stretch Mapping)",
				"Two Gradient Stops (ZoomToFill Mapping)",
				"Five Gradient Stops (Stretch Mapping)",
				"Five Gradient Stops (ZoomToFill Mapping)",
				"Shifted Gradient Center (Stretch Mapping)",
				"Shifted Gradient Center (ZoomToFill Mapping)",
				"Shifted Gradient Focus (Stretch Mapping)",
				"Shifted Gradient Focus (ZoomToFill Mapping)",
			};

			NRadialGradientFill[] fills = new NRadialGradientFill[]
			{
				TwoGradientStops_Stretch(),
				TwoGradientStops_Zoom(),
				FiveGradientStops_Stretch(),
				FiveGradientStops_Zoom(),
				ShiftedCenter_Stretch(),
				ShiftedCenter_Zoom(),
				ShiftedFocus_Stretch(),
				ShiftedFocus_Zoom()
			};

			// Add a canvas for each demonstrated gradient
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
			stack.Add(NPairBox.Create("Canvas Width:", m_CanvasWidthUpDown));
			stack.Add(NPairBox.Create("Canvas Height:", m_CanvasHeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates NOV's radial gradient fillings.
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

		NRadialGradientFill TwoGradientStops_Stretch()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0, NColor.AliceBlue));
			rgf.GradientStops.Add(new NGradientStop(1, NColor.DarkSlateBlue));
			return rgf;
		}
		NRadialGradientFill TwoGradientStops_Zoom()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0, NColor.AliceBlue));
			rgf.GradientStops.Add(new NGradientStop(1, NColor.DarkSlateBlue));
			// rgf.TextureMapping = new NFitAndAlignTextureMapping();
			return rgf;
		}
		NRadialGradientFill FiveGradientStops_Stretch()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.00f, NColor.Red));
			rgf.GradientStops.Add(new NGradientStop(0.25f, NColor.Yellow));
			rgf.GradientStops.Add(new NGradientStop(0.50f, NColor.LimeGreen));
			rgf.GradientStops.Add(new NGradientStop(0.75f, NColor.MediumBlue));
			rgf.GradientStops.Add(new NGradientStop(1.00f, NColor.DarkViolet));
			return rgf;
		}
		NRadialGradientFill FiveGradientStops_Zoom()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.00f, NColor.Red));
			rgf.GradientStops.Add(new NGradientStop(0.25f, NColor.Yellow));
			rgf.GradientStops.Add(new NGradientStop(0.50f, NColor.LimeGreen));
			rgf.GradientStops.Add(new NGradientStop(0.75f, NColor.MediumBlue));
			rgf.GradientStops.Add(new NGradientStop(1.00f, NColor.DarkViolet));
            // FIX: Gradient Transform
            // rgf.MappingMode = ENGradientMappingMode.ZoomToFill;
			return rgf;
		}
		NRadialGradientFill ShiftedCenter_Stretch()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.0f, NColor.Crimson));
			rgf.GradientStops.Add(new NGradientStop(0.5f, NColor.Goldenrod));
			rgf.GradientStops.Add(new NGradientStop(0.6f, NColor.Indigo));
			rgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Thistle));

			// The center coordinates are specified with values between 0 and 1
            // FIX: Radial Gradient
			// rgf.CenterFactorX = 0.0f;
            // rgf.CenterFactorY = 1.0f;

			return rgf;
		}
		NRadialGradientFill ShiftedCenter_Zoom()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.0f, NColor.Crimson));
			rgf.GradientStops.Add(new NGradientStop(0.5f, NColor.Goldenrod));
			rgf.GradientStops.Add(new NGradientStop(0.6f, NColor.Indigo));
			rgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Thistle));
            
			// FIX: Gradient Transform
            // rgf.MappingMode = ENGradientMappingMode.ZoomToFill;

			// The center coordinates are specified with values between 0 and 1
            // FIX: Radial Gradient
            // rgf.CenterFactorX = 0.0f;
            // rgf.CenterFactorY = 1.0f;

			return rgf;
		}
		NRadialGradientFill ShiftedFocus_Stretch()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.0f, NColor.White));
			rgf.GradientStops.Add(new NGradientStop(0.4f, NColor.Red));
			rgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Black));
			rgf.FocusFactorX = -0.6f;
			rgf.FocusFactorY = -0.6f;
			return rgf;
		}
		NRadialGradientFill ShiftedFocus_Zoom()
		{
			NRadialGradientFill rgf = new NRadialGradientFill();
			rgf.GradientStops.Add(new NGradientStop(0.0f, NColor.White));
			rgf.GradientStops.Add(new NGradientStop(0.4f, NColor.Red));
			rgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Black));
			rgf.FocusFactorX = -0.6f;
			rgf.FocusFactorY = -0.6f;
            // FIX: Radial Gradient
            // rgf.MappingMode = ENGradientMappingMode.ZoomToFill;
			return rgf;
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
		/// Schema associated with NRadialGradientFillExample.
		/// </summary>
		public static readonly NSchema NRadialGradientFillExampleSchema;

		#endregion
	}
}