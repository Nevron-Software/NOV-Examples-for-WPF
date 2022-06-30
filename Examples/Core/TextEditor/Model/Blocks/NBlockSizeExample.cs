using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with differnt inline formatting
	/// </summary>
	public class NBlockSizeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NBlockSizeExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NBlockSizeExample()
		{
			NBlockSizeExampleSchema = NSchema.Create(typeof(NBlockSizeExample), NExampleBaseSchema);
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
	This example demonstrates how to control the size of text blocks using the preferred, minimum and maximum width and height properties of each block element.
</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Block Size", "Every block in the document can have a specified Preferred, Minimum and Maximum size.", 1));

			section.Blocks.Add(GetDescriptionBlock("Preferred Width and Height", "The following paragraph has specified Preferred Width and Height.", 2));

			NParagraph paragraph1 = new NParagraph("Paragraph with Preferred Width 50% and Preferred Height of 200dips.");

			paragraph1.BackgroundFill = new NColorFill(NColor.LightGray);
			paragraph1.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 50);
			paragraph1.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 200);

			section.Blocks.Add(paragraph1);

			section.Blocks.Add(GetDescriptionBlock("Minimum and Maximum Width and Height", "The following paragraph has specified Minimum and Maximum Width.", 2));

			NParagraph paragraph2 = new NParagraph("Paragraph with Preferred Width 50% and Preferred Height of 200dips and Minimum Width of 200 dips and Maximum Width 300 dips.");

			paragraph2.BackgroundFill = new NColorFill(NColor.LightGray);
			paragraph2.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 50);
			paragraph2.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 200);
			paragraph2.MinWidth = new NMultiLength(ENMultiLengthUnit.Dip, 200);
			paragraph2.MaxWidth = new NMultiLength(ENMultiLengthUnit.Dip, 300);

			paragraph2.WrapDesiredWidth = false;
			paragraph2.WrapMinWidth = false;

			section.Blocks.Add(paragraph2);

			section.Blocks.Add(GetDescriptionBlock("Desired Width Wrapping", "The following paragraph has disabled desired width wrapping, resulting in a paragraph that does not introduce any hard line breaks.", 2));

			NParagraph paragraph3 = new NParagraph("Paragraph with WrapDesiredWidth set to false. Note that the paragraph will not introduce any hard breaks. The content of this paragraph is intentionally made long to illustrate this feature.");

			paragraph3.BackgroundFill = new NColorFill(NColor.LightGray);
			paragraph3.WrapDesiredWidth = false;
			paragraph3.WrapMinWidth = false;

			section.Blocks.Add(paragraph3);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NBlockSizeExampleSchema;

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