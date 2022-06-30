using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically assign a bullet list template to a bullet list
	/// </summary>
	public class NBulletListTemplateExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NBulletListTemplateExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NBulletListTemplateExample()
		{
			NBulletListTemplateExampleSchema = NSchema.Create(typeof(NBulletListTemplateExample), NExampleBaseSchema);
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
	This example demonstrates how to programmatically create custom bullet list templates.
</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Bullet List Templates", "Bullet lists templates control the appearance of bullet items for a particular level.", 1));
			section.Blocks.Add(GetDescriptionBlock("Setting bullet list template", "Following is a nested bullet list that has a custom defined bullet list template.", 2));

			// create a custom bullet list template
			NBulletList bulletList = new NBulletList();

			NBulletListLevel bulletListLevel1 = new NBulletListLevel();
			bulletListLevel1.BulletType = ENBulletType.UpperRoman;
			bulletListLevel1.Format = "\\0.";
			bulletList.Levels.Add(bulletListLevel1);

			NBulletListLevel bulletListLevel2 = new NBulletListLevel();
			bulletListLevel2.BulletType = ENBulletType.Text;
			bulletListLevel2.BulletText = new string((char)ENBulletChar.BlackCircle, 1);
			bulletList.Levels.Add(bulletListLevel2);

			// Create the first bullet list
			for (int i = 0; i < 3; i++)
			{
				section.Blocks.Add(new NParagraph("Bullet List Item" + i.ToString(), bulletList, 0));

				for (int j = 0; j < 2; j++)
				{
					section.Blocks.Add(new NParagraph("Nested Bullet List Item" + i.ToString(), bulletList, 1));
				}
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NBulletListTemplateExampleSchema;

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