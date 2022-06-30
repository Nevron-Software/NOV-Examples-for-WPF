using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
	public class NImageFillExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NImageFillExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NImageFillExample()
		{
			NImageFillExampleSchema = NSchema.Create(typeof(NImageFillExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a table layout panel
			m_Table = new NTableFlowPanel();
			m_Table.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_Table.VerticalPlacement = ENVerticalPlacement.Top;
			m_Table.Padding = new NMargins(30);
			m_Table.HorizontalSpacing = 30;
			m_Table.VerticalSpacing = 30;
			m_Table.MaxOrdinal = 4;

			string[] names = new string[]
			{
				"Align",
				"Fit and Align",
				"Fixed",
				"Stretch",
				"Stretch X, Align Y",
				"Stretch Y, Align X",
				"Tile",
				"Tile, FlipX",
				"Tile, FlipY",
				"Tile, FlipXY"
			};

			NTextureMapping[] mappings = new NTextureMapping[]
			{
				new NAlignTextureMapping(ENHorizontalAlignment.Left, ENVerticalAlignment.Top),
				new NFitAndAlignTextureMapping(ENHorizontalAlignment.Center, ENVerticalAlignment.Center),
				new NFixedTextureMapping(NMultiLength.NewPercentage(10), ENHorizontalAlignment.Left, NMultiLength.NewPercentage(10), ENVerticalAlignment.Top),
				new NStretchTextureMapping(),
				new NStretchXAlignYTextureMapping(ENVerticalAlignment.Bottom, ENTileMode.None),
				new NStretchYAlignXTextureMapping(ENHorizontalAlignment.Right, ENTileMode.None),
				new NTileTextureMapping(),
				new NTileTextureMapping(true, false),
				new NTileTextureMapping(false, true),
				new NTileTextureMapping(true, true),
			};

			// Add widgets with the proper filling and names to the panel
			for (int i = 0; i < mappings.Length; i++)
			{
				NStackPanel stack = new NStackPanel();
				m_Table.Add(stack);
				stack.Direction = ENHVDirection.TopToBottom;
				stack.FillMode = ENStackFillMode.First;
				stack.FitMode = ENStackFitMode.First;

				// Create a widget with the proper filling
				NCanvas canvas = new NCanvas();
				canvas.PreferredSize = new NSize(defaultCanvasWidth, defaultCanvasHeight);
				NImageFill fill = new NImageFill(NResources.Image_Artistic_Plane_png);
				fill.TextureMapping = mappings[i];
				canvas.Tag = fill;
				stack.Add(canvas);
				canvas.PrePaint += new Function<NCanvasPaintEventArgs>(OnCanvasPrePaint);

				// Create a label with the corresponding name
				NLabel label = new NLabel(names[i]);
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
			m_CanvasWidthUpDown.Minimum = 60;
			m_CanvasWidthUpDown.Maximum = 300;
			m_CanvasWidthUpDown.Value = defaultCanvasWidth;
			m_CanvasWidthUpDown.Step = 1;
			m_CanvasWidthUpDown.DecimalPlaces = 0;
			m_CanvasWidthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// Canvas height editor
			m_CanvasHeightUpDown = new NNumericUpDown();
			m_CanvasHeightUpDown.Minimum = 60;
			m_CanvasHeightUpDown.Maximum = 300;
			m_CanvasHeightUpDown.Value = defaultCanvasHeight;
			m_CanvasHeightUpDown.Step = 1;
			m_CanvasHeightUpDown.DecimalPlaces = 0;
			m_CanvasHeightUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnNumericUpDownValueChanged);

			// create a stack and put the controls in it
			NStackPanel stack = new NStackPanel();
			stack.Add(NPairBox.Create("Canvas Width:", m_CanvasWidthUpDown));
			stack.Add(NPairBox.Create("Canvas Height:", m_CanvasHeightUpDown));

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example shows the image fillings supported by NOV.
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
			args.PaintVisitor.SetStroke(NColor.Red, 1);
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
			//	((NHatchFill)canvas.Tag).SetValue(property, color);
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

		const int defaultCanvasWidth = 240;
		const int defaultCanvasHeight = 240;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NImageFillExample.
		/// </summary>
		public static readonly NSchema NImageFillExampleSchema;

		#endregion
	}
}