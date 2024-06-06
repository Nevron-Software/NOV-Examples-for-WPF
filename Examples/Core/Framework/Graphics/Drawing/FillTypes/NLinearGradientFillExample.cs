using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Framework
{
    public class NLinearGradientFillExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NLinearGradientFillExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NLinearGradientFillExample()
        {
            NLinearGradientFillExampleSchema = NSchema.Create(typeof(NLinearGradientFillExample), NExampleBase.NExampleBaseSchema);
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
                "Two Gradient Stops, Horizontal Gradient Axis",
                "Five Gradient Stops, Vertical Gradient Axis",
                "Gradient Axis Angle = 45deg, Mapping Mode = ZoomToFill",
                "Gradient Axis Angle = 45deg, Mapping Mode = Stretch"
            };

            NLinearGradientFill[] fills = new NLinearGradientFill[]
            {
                GradientWithTwoStops(),
                GradientWithFiveStops(),
                GradientInZoomToFillMode(),
                GradientInStretchMode(),
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
	This example demonstrates NOV's linear gradient fills. The first row contains a horizontal linear gradient with two gradient stops and a vertical gradient with five gradient stops.
	The second row contains two gradients that have the same gradient stops and axis angles, but are mapped differently.
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

        NLinearGradientFill GradientWithTwoStops()
        {
            NLinearGradientFill lgf = new NLinearGradientFill();
            lgf.GradientStops.Add(new NGradientStop(0, NColor.Red));
            lgf.GradientStops.Add(new NGradientStop(1, NColor.DarkBlue));
            return lgf;
        }
        NLinearGradientFill GradientWithFiveStops()
        {
            NLinearGradientFill lgf = new NLinearGradientFill();
            lgf.GradientStops.Add(new NGradientStop(0.00f, NColor.Red));
            lgf.GradientStops.Add(new NGradientStop(0.25f, NColor.Yellow));
            lgf.GradientStops.Add(new NGradientStop(0.50f, NColor.LimeGreen));
            lgf.GradientStops.Add(new NGradientStop(0.75f, NColor.RoyalBlue));
            lgf.GradientStops.Add(new NGradientStop(1.00f, NColor.BlueViolet));
            lgf.SetAngle(new NAngle(90, NUnit.Degree));
            return lgf;
        }
        NLinearGradientFill GradientInZoomToFillMode()
        {
            NLinearGradientFill lgf = new NLinearGradientFill();
            lgf.GradientStops.Add(new NGradientStop(0.0f, NColor.Red));
            lgf.GradientStops.Add(new NGradientStop(0.4f, NColor.BlueViolet));
            lgf.GradientStops.Add(new NGradientStop(0.5f, NColor.LavenderBlush));
            lgf.GradientStops.Add(new NGradientStop(0.6f, NColor.BlueViolet));
            lgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Red));
            lgf.SetAngle(new NAngle(45, NUnit.Degree));
            // FIX: Gradient Transform
            // lgf.MappingMode = ENGradientMappingMode.ZoomToFill;
            return lgf;
        }
        NLinearGradientFill GradientInStretchMode()
        {
            NLinearGradientFill lgf = new NLinearGradientFill();
            lgf.GradientStops.Add(new NGradientStop(0.0f, NColor.Red));
            lgf.GradientStops.Add(new NGradientStop(0.4f, NColor.BlueViolet));
            lgf.GradientStops.Add(new NGradientStop(0.5f, NColor.LavenderBlush));
            lgf.GradientStops.Add(new NGradientStop(0.6f, NColor.BlueViolet));
            lgf.GradientStops.Add(new NGradientStop(1.0f, NColor.Red));
            lgf.SetAngle(new NAngle(45, NUnit.Degree));
            // FIX: Gradient Transform
            // lgf.MappingMode = ENGradientMappingMode.Stretch;
            return lgf;
        }

        #endregion

        #region Fields

        NTableFlowPanel m_Table;
        NNumericUpDown m_CanvasWidthUpDown;
        NNumericUpDown m_CanvasHeightUpDown;

        #endregion

        #region Constants

        const int defaultCanvasWidth = 320;
        const int defaultCanvasHeight = 200;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NLinearGradientFillExample.
        /// </summary>
        public static readonly NSchema NLinearGradientFillExampleSchema;

        #endregion
    }
}