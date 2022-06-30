using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to programmatically create paragraphs with differnt formatting
    /// </summary>
    public class NParagraphFormattingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NParagraphFormattingExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NParagraphFormattingExample()
        {
            NParagraphFormattingExampleSchema = NSchema.Create(typeof(NParagraphFormattingExample), NExampleBaseSchema);
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
	This example demonstrates how to use different paragraph formatting properties.
</p>
";
		}

		private void PopulateRichText()
        {
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

            // paragraphs with different horizontal alignment
			section.Blocks.Add(GetDescriptionBlock("Paragraph Formatting Example", "The following examples show different paragraph formatting properties.", 1));
			section.Blocks.Add(GetTitleParagraph("Paragraphs with different horizontal alignment", 2));

			NParagraph paragraph;

            for (int i = 0; i < 4; i++)
            {
                paragraph = new NParagraph();

				switch (i)
                {
                    case 0:
                        paragraph.HorizontalAlignment = ENAlign.Left;
                        paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("left")));
                        break;
                    case 1:
                        paragraph.HorizontalAlignment = ENAlign.Center;
                        paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("center")));
                        break;
                    case 2:
                        paragraph.HorizontalAlignment = ENAlign.Right;
                        paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("right")));
                        break;
                    case 3:
                        paragraph.HorizontalAlignment = ENAlign.Justify;
                        paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("justify")));
                        break;
                }

                section.Blocks.Add(paragraph);
            }

			section.Blocks.Add(GetTitleParagraph("Paragraphs with Margins, Padding and Borders", 2));
            {
                // borders
                paragraph = new NParagraph();
                paragraph.BorderThickness = new NMargins(2, 2, 2, 2);
                paragraph.Border = NBorder.CreateFilledBorder(NColor.Red);
                paragraph.PreferredWidth = NMultiLength.NewPercentage(50);
                paragraph.Margins = new NMargins(5, 5, 5, 5);
                paragraph.Padding = new NMargins(5, 5, 5, 5);
                paragraph.PreferredWidth = NMultiLength.NewFixed(300);
				paragraph.PreferredHeight = NMultiLength.NewFixed(100);
                NTextInline textInline1 = new NTextInline("Paragraphs can have border, margins and padding as well as preffered size");
                paragraph.Inlines.Add(textInline1);

                section.Blocks.Add(paragraph);
            }

			// First line indent and hanging indent
			section.Blocks.Add(GetTitleParagraph("Paragraph with First Line Indent and Hanging Indent", 2));

			NParagraph paragraphWithIndents = new NParagraph(GetRepeatingText("First line indent -10dip, hanging indent 15dip.", 5));
			paragraphWithIndents.FirstLineIndent = -10;
			paragraphWithIndents.HangingIndent = 15;
			section.Blocks.Add(paragraphWithIndents);

			// First line indent and hanging indent
			section.Blocks.Add(GetTitleParagraph("Line Spacing", 2));

			NParagraph paragraphWithMultipleLineSpacing = new NParagraph(GetRepeatingText("Line space is two times bigger than normal", 10));
			paragraphWithMultipleLineSpacing.LineHeightMode = ENLineHeightMode.Multiple;
			paragraphWithMultipleLineSpacing.LineHeightFactor = 2.0;
			section.Blocks.Add(paragraphWithMultipleLineSpacing);

			NParagraph paragraphWithAtLeastLineSpacing = new NParagraph(GetRepeatingText("Line space is at least 20 dips.", 10));
			paragraphWithAtLeastLineSpacing.LineHeightMode = ENLineHeightMode.AtLeast;
			paragraphWithAtLeastLineSpacing.LineHeight = 20.0;
			section.Blocks.Add(paragraphWithAtLeastLineSpacing);

			NParagraph paragraphWithExactLineSpacing = new NParagraph(GetRepeatingText("Line space is exactly 20 dips.", 10));
			paragraphWithExactLineSpacing.LineHeightMode = ENLineHeightMode.Exactly;
			paragraphWithExactLineSpacing.LineHeight = 20.0;
			section.Blocks.Add(paragraphWithExactLineSpacing);

            // BIDI formatting
			section.Blocks.Add(GetTitleParagraph("Paragraphs with BIDI text", 2));

            paragraph = new NParagraph();
            NTextInline latinText1 = new NTextInline("This is some text in English. Followed by Arabic:");
            NTextInline arabicText = new NTextInline("أساسًا، تتعامل الحواسيب فقط مع الأرقام، وتقوم بتخزين الأحرف والمحارف الأخرى بعد أن تُعطي رقما معينا لكل واحد منها. وقبل اختراع ");
            NTextInline latinText2 = new NTextInline("This is some text in English.");


            paragraph.Inlines.Add(latinText1);
            paragraph.Inlines.Add(arabicText);
            paragraph.Inlines.Add(latinText2);

            section.Blocks.Add(paragraph);
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Gets dummy text for aligned paragraphs
        /// </summary>
        /// <param name="alignment"></param>
        /// <returns></returns>
        private static string GetAlignedParagraphText(string alignment)
        {
            string text = string.Empty;

            for (int i = 0; i < 10; i++)
            {
				if (text.Length > 0)
				{
					text += " ";
				}

                text += "This is a " + alignment + " aligned paragraph.";
            }

            return text;
        }

        #endregion

        #region Fields

        private NRichTextView m_RichText;

        #endregion

        #region Schema

        public static readonly NSchema NParagraphFormattingExampleSchema;

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
		/// <summary>
		/// Gets the specified text repeated
		/// </summary>
		/// <param name="text"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		private static string GetRepeatingText(string text, int count)
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < count; i++)
			{
				if (builder.Length > 0)
				{
					builder.Append(" ");
				}

				builder.Append(text);
			}

			return builder.ToString();
		}

		#endregion

		#region Constants

		private static readonly NMargins defaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

		#endregion
	}
}