using Nevron.Nov.DataStructures;
using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	/// <summary>
	/// 
	/// </summary>
	public class NFindAndReplaceExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NFindAndReplaceExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NFindAndReplaceExample()
        {
            NFindAndReplaceExampleSchema = NSchema.Create(typeof(NFindAndReplaceExample), NExampleBaseSchema);
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
			
			m_FindTextBox = new NTextBox();
			m_FindTextBox.Text = "quick";
			stack.Add(new NPairBox(new NLabel("Find:"), m_FindTextBox, ENPairBoxRelation.Box1AboveBox2));

			m_ReplaceTextBox = new NTextBox();
			m_ReplaceTextBox.Text = "slow";
			stack.Add(new NPairBox(new NLabel("Replace:"), m_ReplaceTextBox, ENPairBoxRelation.Box1AboveBox2));

			NButton findAllButton = new NButton("Find All");
			findAllButton.Click += new Function<NEventArgs>(OnFindAllButtonClick);
			stack.Add(findAllButton);

			NButton replaceAllButton = new NButton("Replace All");
			replaceAllButton.Click += new Function<NEventArgs>(OnReplaceAllButtonClick);
			stack.Add(replaceAllButton);

			NButton clearHighlightButton = new NButton("Clear Highlight");
			clearHighlightButton.Click += new Function<NEventArgs>(OnClearHighlightButtonClick);
			stack.Add(clearHighlightButton);

			return stack;
		}
        protected override string GetExampleDescription()
        {
			return @"
<p>This example demonstrates how to find and replace text.</p>
<p>Press the ""Find All"" button to highlight all occurrences of ""Find"".</p>
<p>Press the ""Replace All"" button to replace and highlight all occurrences of ""Find"" with ""Replace""</p>
<p>Press the ""Clear Highlight"" button to clear all highlighting</p>
";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();

			double padding = 10;
			double sizeX = 160;
			double sizeY = 160;

			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
					shape1.SetBounds(padding + x * (padding + sizeX), padding + y * (padding + sizeY), sizeX, sizeY);
					shape1.TextBlock = new NTextBlock();
					shape1.TextBlock.Padding = new NMargins(20);
					shape1.TextBlock.Text = "The quick brown fox jumps over the lazy dog";
					drawing.ActivePage.Items.Add(shape1);
				}
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when the user presses the find all button
		/// </summary>
		/// <param name="arg"></param>
		void OnFindAllButtonClick(NEventArgs arg)
		{
			// init find settings
			NFindTextSettings settings = new NFindTextSettings();
			settings.FindWhat = m_FindTextBox.Text;
			settings.SearchDirection = ENDiagramTextSearchDirection.ForwardReading;

			// loop through all occurrences
			NTextSearcher searcher = new NTextSearcher(m_DrawingView, settings);
			searcher.ActivateEditor = false;
			NShapeTextSearchState state;

			while (searcher.FindNext(out state))
			{
				state.Shape.GetTextSelection().SetHighlightFillToSelectedInlines(new NColorFill(ENNamedColor.Red));
			}
		}
		/// <summary>
		/// Called when the user presses the replace all button
		/// </summary>
		/// <param name="arg"></param>
		void OnReplaceAllButtonClick(NEventArgs arg)
		{
			// init find settings
			NFindTextSettings settings = new NFindTextSettings();
			settings.FindWhat = m_FindTextBox.Text;
			settings.SearchDirection = ENDiagramTextSearchDirection.ForwardReading;

			// find all occurrences 
			NTextSearcher searcher = new NTextSearcher(m_DrawingView, settings);
			searcher.ActivateEditor = false;
			NShapeTextSearchState state;

			while (searcher.FindNext(out state))
			{
				NSelection selection = state.Shape.GetTextSelection();

				// replace 
				NRangeI selectedRange = selection.SelectedRange;
				selection.InsertText(m_ReplaceTextBox.Text);

				if (m_ReplaceTextBox.Text.Length > 0)
				{
					selection.SelectRange(new NRangeI(selectedRange.Begin, selectedRange.Begin + m_ReplaceTextBox.Text.Length - 1));
					selection.SetHighlightFillToSelectedInlines(new NColorFill(ENNamedColor.LimeGreen));
				}
			}
		}
		/// <summary>
		/// Called when the user presses clear highlight button
		/// </summary>
		/// <param name="arg"></param>
		void OnClearHighlightButtonClick(NEventArgs arg)
		{
			NList<NNode> shapes = m_DrawingView.Drawing.GetDescendants(NShape.NShapeSchema);

			for (int i = 0; i < shapes.Count; i++)
			{
				NShape shape = (NShape)shapes[i];

				NRangeTextElement rootTextElement = (NRangeTextElement)shape.GetTextBlockContentNoCreate();

				if (rootTextElement == null)
					continue;

				rootTextElement.VisitRanges(delegate (NRangeTextElement range)
				{
					NInline inline = range as NInline;

					if (inline != null)
					{
						inline.ClearLocalValue(NInline.HighlightFillProperty);
					}
				});
			}
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		private NTextBox m_FindTextBox;
		private NTextBox m_ReplaceTextBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFindAndReplaceExample.
		/// </summary>
		public static readonly NSchema NFindAndReplaceExampleSchema;

        #endregion
    }
}