using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with differnt formatting
	/// </summary>
	public class NPageBreaksExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NPageBreaksExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NPageBreaksExample()
        {
            NPageBreaksExampleSchema = NSchema.Create(typeof(NPageBreaksExample), NExampleBaseSchema);
        }

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
			m_RichText = richTextWithRibbon.View;
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to programmatically add page breaks inlines to paragraphs.
</p>
";
		}

		private void PopulateRichText()
        {
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);
			m_RichText.Content.Layout = ENTextLayout.Print;

			///////////////
/*			NParagraph par1 = new NParagraph("Paragraph 1");
			par1.Inlines.Add(new NPageBreakInline());
			section.Blocks.Add(par1);

			NParagraph par2 = new NParagraph("Paragraph 2");
			section.Blocks.Add(par2);

			///////////////
			return;*/

			section.Blocks.Add(GetDescriptionBlock("Page Breaks", "The example shows how to add page break inlines.", 1));

            // paragraphs with different horizontal alignment
            section.Blocks.Add(GetTitleParagraph("Paragraphs can contain explicit page breaks", 2));

            for (int i = 0; i < 23; i++)
            {
                if (i % 10 == 0)
                {
                    section.Blocks.Add(GetParagraphWithPageBreak());
                }
                else
                {
                    section.Blocks.Add(GetParagraphWithoutPageBreak());                    
                }                
            }

			section.Blocks.Add(GetTitleParagraph("Tables can also contain page breaks", 2));

            NTable table = new NTable(3, 3);

            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    // by default cells contain a single paragraph
					table.Rows[row].Cells[col].Blocks.Clear();
                    table.Rows[row].Cells[col].Blocks.Add(GetParagraphWithoutPageBreak());
                }
            }

			table.Rows[1].Cells[1].Blocks.Clear();
            table.Rows[1].Cells[1].Blocks.Add(GetParagraphWithPageBreak());

            section.Blocks.Add(table);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets a paragraph without an explicit page break
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private static NParagraph GetParagraphWithoutPageBreak()
        {
            string text = string.Empty;

            for (int i = 0; i < 10; i++)
            {
				if (text.Length > 0)
				{
					text += " ";
				}

				text += "This is a paragraph without an explicit page break.";
            }

            return new NParagraph(text);
        }
        /// <summary>
        /// Gets a paragraph with an explicit page break
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private static NParagraph GetParagraphWithPageBreak()
        {
            string text = string.Empty;
            NParagraph paragraph = new NParagraph();

            for (int i = 0; i < 5; i++)
            {
				if (text.Length > 0)
				{
					text += " ";
				}

                text += "This is a paragraph with an explicit page break.";
            }

            paragraph.Inlines.Add(new NTextInline(text));
            
            NTextInline inline = new NTextInline("Page break appears here!");
            inline.FontStyle |= ENFontStyle.Bold;
            paragraph.Inlines.Add(inline);

            paragraph.Inlines.Add(new NPageBreakInline());
            paragraph.Inlines.Add(new NTextInline(text));

            return paragraph;
        }

        #endregion

        #region Fields

        private NRichTextView m_RichText;

        #endregion

        #region Schema

        public static readonly NSchema NPageBreaksExampleSchema;

		#endregion

		#region Static Methods

		private static NParagraph GetDescriptionParagraph(string text)
		{
			return new NParagraph(text);
		}
		private static NParagraph GetTitleParagraphNoBorder(string text, int level)
		{
			double fontSize = 10;
			ENFontStyle fontStyle = ENFontStyle.Regular;

			switch (level)
			{
				case 1:
					fontSize = 16;
					fontStyle = ENFontStyle.Bold;
					break;
				case 2:
					fontSize = 10;
					fontStyle = ENFontStyle.Bold;
					break;
			}

			NParagraph paragraph = new NParagraph();

			paragraph.HorizontalAlignment = ENAlign.Left;
			paragraph.FontSize = fontSize;
			paragraph.FontStyle = fontStyle;

			NTextInline textInline = new NTextInline(text);

			textInline.FontStyle = fontStyle;
			textInline.FontSize = fontSize;

			paragraph.Inlines.Add(textInline);

			return paragraph;

		}
		/// <summary>
		/// Gets a paragraph with title formatting
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private static NParagraph GetTitleParagraph(string text, int level)
		{
			NColor color = NColor.Black;

			NParagraph paragraph = GetTitleParagraphNoBorder(text, level);
			paragraph.HorizontalAlignment = ENAlign.Left;

			paragraph.Border = CreateLeftTagBorder(color);
			paragraph.BorderThickness = defaultBorderThickness;

			return paragraph;
		}
		private static NGroupBlock GetDescriptionBlock(string title, string description, int level)
		{
			NColor color = NColor.Black;

			NParagraph paragraph = GetTitleParagraphNoBorder(title, level);

			NGroupBlock groupBlock = new NGroupBlock();

			groupBlock.ClearMode = ENClearMode.All;
			groupBlock.Blocks.Add(paragraph);
			groupBlock.Blocks.Add(GetDescriptionParagraph(description));

			groupBlock.Border = CreateLeftTagBorder(color);
			groupBlock.BorderThickness = defaultBorderThickness;

			return groupBlock;
		}
		/// <summary>
		/// Creates a left tag border with the specified border
		/// </summary>
		/// <param name="color"></param>
		/// <returns></returns>
		private static NBorder CreateLeftTagBorder(NColor color)
		{
			NBorder border = new NBorder();

			border.LeftSide = new NBorderSide();
			border.LeftSide.Fill = new NColorFill(color);

			return border;
		}

		#endregion

		#region Constants

		private static readonly NMargins defaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

        #endregion
    }
}