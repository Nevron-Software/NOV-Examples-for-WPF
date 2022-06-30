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
    public class NHeaderAndFooterExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NHeaderAndFooterExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NHeaderAndFooterExample()
        {
            NHeaderAndFooterExampleSchema = NSchema.Create(typeof(NHeaderAndFooterExample), NExampleBaseSchema);
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
	This example demonstrates how create sections with different header and footer. Each section has three different properties for header and footer that allow you to specify distinct header and footer for first, left and right pages.
</p>
";
		}

        private void PopulateRichText()
        {
			m_RichText.Content.Layout = ENTextLayout.Print;

			for (int sectionIndex = 0; sectionIndex < 3; sectionIndex++)
			{
				NSection section = new NSection();

				NColor color = NColor.Empty;
				string sectionText = string.Empty;

				switch (sectionIndex)
				{
					case 0:
						{
							sectionText = "Red Section (uniform header and footer).";
							color = NColor.Red;

							section.Blocks.Add(GetDescriptionBlock("Section Headers and Footers", "This example shows how to create sections with different header and footer settings.", 1));

							section.DifferentFirstHeaderAndFooter = false;
							section.DifferentLeftRightHeadersAndFooters = false;

							NHeaderFooter header = CreateHeaderFooter("Uniform Header.");
							header.BackgroundFill = new NColorFill(color);
							section.Header = header;

							NHeaderFooter footer = CreateHeaderFooter("Uniform Footer.");
							footer.BackgroundFill = new NColorFill(color);
							section.Footer = footer;
						}
						break;
					case 1:
						{
							sectionText = "Green Section (different first header and footer).";
							color = NColor.Green;

							section.BreakType = ENSectionBreakType.NextPage;
							section.DifferentFirstHeaderAndFooter = true;
							section.DifferentLeftRightHeadersAndFooters = false;

							NHeaderFooter headerFirst = CreateHeaderFooter("First Page Header.");
							headerFirst.BackgroundFill = new NColorFill(color);
							section.HeaderFirst = headerFirst;

							NHeaderFooter footerFirst = CreateHeaderFooter("First Page Footer.");
							footerFirst.BackgroundFill = new NColorFill(color);
							section.FooterFirst = footerFirst;
						}
						break;
					case 2:
						{
							sectionText = "Blue Section (different left and right page header and footer).";
							color = NColor.Blue;

							section.BreakType = ENSectionBreakType.NextPage;
							section.DifferentFirstHeaderAndFooter = false;
							section.DifferentLeftRightHeadersAndFooters = true;

							NHeaderFooter headerLeft = CreateHeaderFooter("Left Page Header.");
							headerLeft.BackgroundFill = new NColorFill(color);
							section.HeaderLeft = headerLeft;

							NHeaderFooter headerRight = CreateHeaderFooter("Right Page Header.");
							headerRight.BackgroundFill = new NColorFill(color);
							section.HeaderRight = headerRight;

							NHeaderFooter footerLeft = CreateHeaderFooter("Left Page Footer.");
							footerLeft.BackgroundFill = new NColorFill(color);
							section.FooterLeft = footerLeft;

							NHeaderFooter footerRight = CreateHeaderFooter("Right Page Footer.");
							footerRight.BackgroundFill = new NColorFill(color);
							section.FooterRight = footerRight;
						}
						break;

				}

				m_RichText.Content.Sections.Add(section);

				// add some section contentparagraphs with some simple text
				for (int i = 0; i < 20; i++)
				{
					NParagraph paragraph = new NParagraph();

					NTextInline textInline = new NTextInline(GetRepeatingText(sectionText, 5));
					textInline.Fill = new NColorFill(color);
					paragraph.Inlines.Add(textInline);

					section.Blocks.Add(paragraph);
				}

			}
        }

		#endregion

		#region Implementation

		private NHeaderFooter CreateHeaderFooter(string text)
		{
			NHeaderFooter headerFooter = new NHeaderFooter();

			NParagraph paragraph = new NParagraph();

			paragraph.Inlines.Add(new NTextInline(text));
			paragraph.Inlines.Add(new NTextInline(". Page "));
			paragraph.Inlines.Add(new NFieldInline(ENNumericFieldName.PageNumber));
			paragraph.Inlines.Add(new NTextInline(" of "));
			paragraph.Inlines.Add(new NFieldInline(ENNumericFieldName.PageCount));

			headerFooter.Blocks.Add(paragraph);

			return headerFooter;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NHeaderAndFooterExampleSchema;

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
		private static NGroupBlock GetNoteBlock(string text, int level)
		{
			NColor color = NColor.Red;
			NParagraph paragraph = GetTitleParagraphNoBorder("Note", level);

			NGroupBlock groupBlock = new NGroupBlock();

			groupBlock.ClearMode = ENClearMode.All;
			groupBlock.Blocks.Add(paragraph);
			groupBlock.Blocks.Add(GetDescriptionParagraph(text));

			groupBlock.Border = CreateLeftTagBorder(color);
			groupBlock.BorderThickness = defaultBorderThickness;

			return groupBlock;
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
		private static NParagraph GetLoremIpsumParagraph()
		{
			return new NParagraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum placerat in tortor nec tincidunt. Sed sagittis in sem ac auctor. Donec scelerisque molestie eros, a dictum leo fringilla eu. Vivamus porta urna non ullamcorper commodo. Nulla posuere sodales pellentesque. Donec a erat et tortor viverra euismod non et erat. Donec dictum ante eu mauris porta, eget suscipit mi ultrices. Nunc convallis adipiscing ligula, non pharetra dolor egestas at. Etiam in condimentum sapien. Praesent sagittis pulvinar metus, a posuere mauris aliquam eget.");
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
