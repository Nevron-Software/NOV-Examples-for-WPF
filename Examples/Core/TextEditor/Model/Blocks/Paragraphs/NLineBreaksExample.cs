using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with line breaks
	/// </summary>
	public class NLineBreaksExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NLineBreaksExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NLineBreaksExample()
        {
            NLineBreaksExampleSchema = NSchema.Create(typeof(NLineBreaksExample), NExampleBaseSchema);
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
        protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to programmatically add line breaks to paragraphs.
</p>
";
		}

		private void PopulateRichText()
        {
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Paragraphs can contain explicit line breaks", "This example shows how to programmatically add line breaks to paragraphs", 2));

			NParagraph paragraph = new NParagraph();

			paragraph.Inlines.Add(new NTextInline("This text appears on the first line."));
			paragraph.Inlines.Add(new NLineBreakInline());
			paragraph.Inlines.Add(new NTextInline("This text appears on the second line (after the line break)."));

			section.Blocks.Add(paragraph);
        }

        #endregion

        #region Fields

        private NRichTextView m_RichText;

        #endregion

        #region Schema

        public static readonly NSchema NLineBreaksExampleSchema;

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
