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
	public class NSectionPagesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSectionPagesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSectionPagesExample()
		{
			NSectionPagesExampleSchema = NSchema.Create(typeof(NSectionPagesExample), NExampleBaseSchema);
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
	This example demonstrates how to set different page properties, like page size, page orientation, and page border.
</p>
";
		}

		private void PopulateRichText()
		{
			m_RichText.Content.Layout = ENTextLayout.Print;
			m_RichText.Content.ZoomFactor = 0.5;

			for (int sectionIndex = 0; sectionIndex < 4; sectionIndex++)
			{
				NSection section = new NSection();

				section.Margins = NMargins.Zero;
				section.Padding = NMargins.Zero;

				string sectionText = string.Empty;

				switch (sectionIndex)
				{
					case 0:
						sectionText = "Paper size A4.";
						section.PageSize = new NPaperSize(ENPaperKind.A4);
						section.BreakType = ENSectionBreakType.NextPage;
						section.Blocks.Add(GetDescriptionBlock("Section Pages", "This example shows how to set different page properties, like page size, page orientation, and page border", 1));
						break;
					case 1:
						sectionText = "Paper size A5.";
						section.PageSize = new NPaperSize(ENPaperKind.A5);
						section.BreakType = ENSectionBreakType.NextPage;
						break;
					case 2:
						sectionText = "Paper size A4, paper orientation portrait.";
						section.PageOrientation = ENPageOrientation.Landscape;
						section.PageSize = new NPaperSize(ENPaperKind.A4);
						section.BreakType = ENSectionBreakType.NextPage;
						break;
					case 3:
						sectionText = "Paper size A4, page border solid 10dip.";
						section.PageBorder = NBorder.CreateFilledBorder(NColor.Black);
						section.PageBorderThickness = new NMargins(10);
						section.PageSize = new NPaperSize(ENPaperKind.A4);
						section.BreakType = ENSectionBreakType.NextPage;
						break;

				}

				m_RichText.Content.Sections.Add(section);

				// add some content
				NParagraph paragraph = new NParagraph(sectionText);
				section.Blocks.Add(paragraph);
			}
		}

		#endregion

		#region Implementation

		private NHeaderFooter CreateHeaderFooter(string text)
		{
			NHeaderFooter headerFooter = new NHeaderFooter();

			NParagraph paragraph = new NParagraph();

			paragraph.Inlines.Add(new NTextInline(text));
			paragraph.Inlines.Add(new NTextInline("Page "));
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

		public static readonly NSchema NSectionPagesExampleSchema;

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