using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.Framework
{
	public class NAdvancedGradientFillExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAdvancedGradientFillExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAdvancedGradientFillExample()
		{
			NAdvancedGradientFillExampleSchema = NSchema.Create(typeof(NAdvancedGradientFillExample), NExampleBase.NExampleBaseSchema);
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
			m_Table.MaxOrdinal = 3;

			string[] texts = new string[]
			{
				"Azure",
				"Flow",
				"Bulb",
				"Eclipse",
				"The Eye",
				"Medusa",
				"Kaleidoscope",
				"Reactor",
				"Green"
			};

			// Create the advanced gradients
			NAdvancedGradientFill[] fills = new NAdvancedGradientFill[]
			{
				AzureLight,
				Flow,
				Bulb,
				Eclipse,
				TheEye,
				Medusa,
				Kaleidoscope,
				Reactor,
				Green
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
	This example demonstrates how to create and configure advanced gradient fills.
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

		public NAdvancedGradientFill Eclipse
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.MidnightBlue;
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Crimson, NAngle.Zero, 0.5f, 0.5f, 0.5f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.1f, 0.2f, 0.7f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.DarkOrchid, NAngle.Zero, 0.9f, 0.9f, 1.0f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill Green
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.Black;
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Green, NAngle.Zero, 0.5f, 0.5f, 0.2f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Green, new NAngle(90, NUnit.Degree), 0.5f, 0.5f, 0.2f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.5f, 0.5f, 0.5f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill Bulb
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.Purple;
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Khaki, NAngle.Zero, 0.65f, 0.35f, 0.4f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, new NAngle(135, NUnit.Degree), 0.5f, 0.5f, 0.7f, ENAdvancedGradientPointShape.Line));
				return ag;
			}
		}
		public NAdvancedGradientFill Kaleidoscope
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.DarkSlateBlue;
				ag.Points.Add(new NAdvancedGradientPoint(NColor.DarkSlateBlue, NAngle.Zero, 0.5f, 0.5f, 0.3f, ENAdvancedGradientPointShape.Rectangle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Cornsilk, new NAngle(45, NUnit.Degree), 0.5f, 0.5f, 0.4f, ENAdvancedGradientPointShape.Rectangle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Thistle, NAngle.Zero, 0.1f, 0.1f, 0.3f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Thistle, NAngle.Zero, 0.9f, 0.1f, 0.3f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Thistle, NAngle.Zero, 0.9f, 0.9f, 0.3f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Thistle, NAngle.Zero, 0.1f, 0.9f, 0.3f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill TheEye
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = new NColor(64, 0, 128);
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(128, 128, 255), NAngle.Zero, 0.5f, 0.5f, 0.51f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.5f, 0.5f, 0.49f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(0, 0, 64), NAngle.Zero, 0.5f, 0.5f, 0.23f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.Black, NAngle.Zero, 0.5f, 0.5f, 0.13f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill Medusa
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = new NColor(0, 0, 64);
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(0, 128, 255), NAngle.Zero, 0.12f, 0.57f, 0.60f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.29f, 0.29f, 0.35f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(0, 128, 255), NAngle.Zero, 0.57f, 0.12f, 0.60f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(128, 0, 255), NAngle.Zero, 0.60f, 0.60f, 0.37f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.84f, 0.84f, 0.50f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill Reactor
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.Black;
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(255, 128, 255), NAngle.Zero, 0.50f, 0.78f, 0.35f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(128, 128, 255), NAngle.Zero, 0.50f, 0.22f, 0.35f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.50f, 0.50f, 0.52f, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}
		public NAdvancedGradientFill Flow
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = new NColor(64, 0, 128);
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(255, 255, 128), new NAngle(50, NUnit.Degree), 0.38f, 0.17f, 0.48f, ENAdvancedGradientPointShape.Line));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(255, 0, 128), NAngle.Zero, 0.58f, 0.74f, 1, ENAdvancedGradientPointShape.Line));
				return ag;
			}
		}
		public NAdvancedGradientFill AzureLight
		{
			get
			{
				NAdvancedGradientFill ag = new NAdvancedGradientFill();
				ag.BackgroundColor = NColor.White;
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(235, 168, 255), NAngle.Zero, 0.87f, 0.29f, 0.92f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(new NColor(64, 199, 255), NAngle.Zero, 0.53f, 0.82f, 0.81f, ENAdvancedGradientPointShape.Circle));
				ag.Points.Add(new NAdvancedGradientPoint(NColor.White, NAngle.Zero, 0.16f, 0.17f, 1, ENAdvancedGradientPointShape.Circle));
				return ag;
			}
		}

		#endregion

		#region Fields

		NTableFlowPanel m_Table;
		NNumericUpDown m_CanvasWidthUpDown;
		NNumericUpDown m_CanvasHeightUpDown;

		#endregion

		#region Constants

		const int defaultCanvasWidth = 180;
		const int defaultCanvasHeight = 180;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAdvancedGradientFillExample.
		/// </summary>
		public static readonly NSchema NAdvancedGradientFillExampleSchema;

		#endregion
	}
}