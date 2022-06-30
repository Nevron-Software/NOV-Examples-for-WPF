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
	public class NBulletListsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NBulletListsExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NBulletListsExample()
		{
			NBulletListsExampleSchema = NSchema.Create(typeof(NBulletListsExample), NExampleBaseSchema);
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
	This example demonstrates how to set different bullet list templates to bullet lists as well as how to create nested (multilevel) bullet lists.
</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Bullet Lists", "Bullet lists allow you to apply automatic numbering on paragraphs or groups of blocks.", 1));

			section.Blocks.Add(GetDescriptionBlock("Simple bullet list", "Following is a bullet list with default formatting.", 2));

			CreateSampleBulletList(section, ENBulletListTemplateType.Bullet, 3, "Bullet List Item");

			// setting bullet list template type
			{
				section.Blocks.Add(GetDescriptionBlock("Bullet Lists with Different Template", "Following are bullet lists with different formatting", 2));

				ENBulletListTemplateType[] values = NEnum.GetValues<ENBulletListTemplateType>();
				string[] names = NEnum.GetNames<ENBulletListTemplateType>();

				for (int i = 0; i < values.Length - 1; i++)
				{
					CreateSampleBulletList(section, values[i], 3, names[i] + " bullet list item ");
				}
			}

			// nested bullet lists
			{
				section.Blocks.Add(GetDescriptionBlock("Bullet List Levels", "Following is an example of bullet list levels", 2));

				NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Decimal);
				m_RichText.Content.BulletLists.Add(bulletList);

				for (int i = 0; i < 3; i++)
				{
					NParagraph par1 = new NParagraph("Bullet List Item" + i.ToString());
					par1.SetBulletList(bulletList, 0);
					section.Blocks.Add(par1);

					for (int j = 0; j < 2; j++)
					{
						NParagraph par2 = new NParagraph("Nested Bullet List Item" + i.ToString());
						par2.SetBulletList(bulletList, 1);
						par2.MarginLeft = 20;
						section.Blocks.Add(par2);
					}
				}
			}
		}

		#endregion

		#region Implementation

		private void CreateSampleBulletList(NSection section, ENBulletListTemplateType bulletListType, int items, string itemText)
		{
			NBulletList bulletList = new NBulletList(bulletListType);
			m_RichText.Content.BulletLists.Add(bulletList);

			for (int i = 0; i < items; i++)
			{
				NParagraph par = new NParagraph(itemText + i.ToString());
				par.SetBulletList(bulletList, 0);
				section.Blocks.Add(par);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NBulletListsExampleSchema;

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