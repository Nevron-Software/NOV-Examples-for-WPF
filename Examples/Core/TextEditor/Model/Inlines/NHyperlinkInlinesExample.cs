using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create inline elements with different formatting
	/// </summary>
	public class NHyperlinkInlinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NHyperlinkInlinesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NHyperlinkInlinesExample()
		{
			NHyperlinkInlinesExampleSchema = NSchema.Create(typeof(NHyperlinkInlinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Examples

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
			NStackPanel stack = new NStackPanel();

			NButton jumpToBookmarkButton = new NButton("Jump to Bookmark");
			jumpToBookmarkButton.Click += new Function<NEventArgs>(OnJumpToBookmarkButtonClick);

			stack.Add(jumpToBookmarkButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to add hyperlinks and bookmarks and how to create image hyperlinks.</p>
<p>Press the ""Jump to Bookmark"" button to position the caret to ""MyBookmark"" bookmark.</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Hyperlink Inlines", "The example shows how to use hyperlinks inlines.", 1));

			// Hyperlink inline with a hyperlink to an URL
			{
				NHyperlinkInline hyperlinkInline = new NHyperlinkInline();
				hyperlinkInline.Hyperlink = new NUrlHyperlink("http://www.nevron.com", ENUrlHyperlinkTarget.SameWindowSameFrame);
				hyperlinkInline.Text = "Jump to www.nevron.com";

				NParagraph paragraph = new NParagraph();
				paragraph.Inlines.Add(hyperlinkInline);
				section.Blocks.Add(paragraph);
			}

			// Image inline with a hyperlink to an URL
			{
				NImageInline imageInline = new NImageInline();
				imageInline.Image = Nov.Diagram.NResources.Image_MyDraw_Logos_MyDrawLogo_png;
				imageInline.Hyperlink = new NUrlHyperlink("http://www.mydraw.com", ENUrlHyperlinkTarget.SameWindowSameFrame);

				NParagraph paragraph = new NParagraph();
				paragraph.Inlines.Add(imageInline);
				section.Blocks.Add(paragraph);
			}

			for (int i = 0; i < 10; i++)
			{
				section.Blocks.Add(new NParagraph("Some paragraph"));
			}

			// Bookmark inline
			{
				NParagraph paragraph = new NParagraph();

				NBookmarkInline bookmark = new NBookmarkInline();
				bookmark.Name = "MyBookmark";
				bookmark.Text = "This is a bookmark";
				bookmark.Fill = new NColorFill(NColor.Red);
				paragraph.Inlines.Add(bookmark);

				section.Blocks.Add(paragraph);
			}
		}

		#endregion

		#region Event Handlers

		void OnJumpToBookmarkButtonClick(NEventArgs arg)
		{
			m_RichText.Content.Goto(ENTextDocumentPart.Bookmark, "MyBookmark", true);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NHyperlinkInlinesExampleSchema;

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