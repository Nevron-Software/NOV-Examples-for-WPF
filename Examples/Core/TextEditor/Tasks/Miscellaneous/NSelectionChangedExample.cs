using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to programmatically a sample report.
    /// </summary>
    public class NSelectionChangedExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSelectionChangedExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSelectionChangedExample()
		{
			NSelectionChangedExampleSchema = NSchema.Create(typeof(NSelectionChangedExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			NSection section = new NSection();
			section.Blocks.Add(new NParagraph("The example demonstrates how to track the selection changed event."));
			section.Blocks.Add(new NParagraph("It also shows how to identify different text elements depending on the current selection."));
			section.Blocks.Add(new NParagraph("Move the selection and the control will highlight the currently selected words as well as blocks inside the text document."));


			NParagraph paragraph = new NParagraph();
			paragraph.Inlines.Add(new NTextInline("You can also detect the inline elements "));

			NTextInline inline1 = new NTextInline("that have different");
			paragraph.Inlines.Add(inline1);

			NTextInline inline2 = new NTextInline(" Font Style");
			inline2.FontStyle = ENFontStyle.Bold;
			paragraph.Inlines.Add(inline2);

			NTextInline inline3 = new NTextInline(" Font Size");
			inline3.FontSize = 14;
			paragraph.Inlines.Add(inline3);

			NTextInline inline4 = new NTextInline(" and / or other attributes");
			inline4.FontStyle = ENFontStyle.Italic | ENFontStyle.Underline;
			paragraph.Inlines.Add(inline4);

			section.Blocks.Add(paragraph);

			m_RichText.Content.Sections.Add(section);

			m_RichText.Content.Layout = ENTextLayout.Web;
            m_RichText.Selection.SelectionChanged += Selection_SelectionChanged;

			return m_RichText;
		}

        private void Selection_SelectionChanged(NEventArgs arg)
        {
			ClearHighlights();

			// highlight selected blocks
			NList<NBlock> selectedBlocks = m_RichText.Selection.GetSelectedBlocks();

			for (int i = 0; i < selectedBlocks.Count; i++)
            {
				selectedBlocks[i].BackgroundFill = new NColorFill(NColor.LightBlue);
            }

			NList<NInline> selectedInlines = m_RichText.Selection.GetSelectedInlines(false);

			for (int i = 0; i < selectedInlines.Count; i++)
            {
				selectedInlines[i].HighlightFill = new NColorFill(NColor.FromColor(NColor.Yellow, 125));
			}
        }

        protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to track the selection changed event as well as how to query the selection object.</p>";
		}

		#endregion

		#region Implementation

		private void ClearHighlights()
		{
			NList<NNode> blocks = m_RichText.Document.GetDescendants(NBlock.NBlockSchema);

			for (int i = 0; i < blocks.Count; i++)
			{
				((NBlock)blocks[i]).BackgroundFill = null;
			}

			NList<NNode> inlines = m_RichText.Document.GetDescendants(NInline.NInlineSchema);

			for (int i = 0; i < inlines.Count; i++)
			{
				((NInline)inlines[i]).HighlightFill = null;
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NSelectionChangedExampleSchema;

		#endregion
	}
}