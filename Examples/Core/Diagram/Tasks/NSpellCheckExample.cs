using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NSpellCheckExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NSpellCheckExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NSpellCheckExample()
        {
            NSpellCheckExampleSchema = NSchema.Create(typeof(NSpellCheckExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // Create a simple drawing
            NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
            m_DrawingView = drawingViewWithRibbon.View;

            m_DrawingView.Document.HistoryService.Pause();
            try
            {
                InitDiagram(m_DrawingView.Document);
            }
            finally
            {
                m_DrawingView.Document.HistoryService.Resume();
            }

            return drawingViewWithRibbon;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			NCheckBox enableSpellCheck = new NCheckBox("Enable Spell Check");
			enableSpellCheck.Click += new Function<NEventArgs>(OnEnableSpellCheckButtonClick);
			stack.Add(enableSpellCheck);

			return stack;
		}
        protected override string GetExampleDescription()
        {
            return @"<p>
						Demonstrates how to enable the build in spell check.
					</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            // Hide the grid
            NDrawing drawing = drawingDocument.Content;
            drawing.ScreenVisibility.ShowGrid = false;

			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();

			NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape1.SetBounds(10, 10, 200, 200);
			shape1.TextBlock = new NTextBlock();
			shape1.TextBlock.Padding = new NMargins(20);
			shape1.TextBlock.Text = "This text cantains many typpos. This text contuins manyy typos.";
			drawing.ActivePage.Items.Add(shape1);
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when the user presses the find all button
		/// </summary>
		/// <param name="arg"></param>
		void OnEnableSpellCheckButtonClick(NEventArgs arg)
		{
			m_DrawingView.SpellChecker.Enabled = ((NCheckBox)arg.TargetNode).Checked;
		}

        #endregion

        #region Fields

        private NDrawingView m_DrawingView;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NSpellCheckExample.
        /// </summary>
        public static readonly NSchema NSpellCheckExampleSchema;

        #endregion
    }
}