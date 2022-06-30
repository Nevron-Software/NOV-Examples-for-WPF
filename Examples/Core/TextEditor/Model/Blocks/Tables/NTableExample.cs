using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// This example demonstrates how to programmatically create tables.
	/// </summary>
	public class NTableExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NTableExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NTableExample()
		{
            NTableExampleSchema = NSchema.Create(typeof(NTableExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates how to programmatically create tables.</p>";
		}

		private void PopulateRichText()
		{
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Table example", "This example shows how to programmatically create and add a table to the text control.", 1));

			NParagraph p = GetTitleParagraph("Table with cell spacing.", 2);
			section.Blocks.Add(p);

			NTable tableWithCellSpacing = CreateTable();
			tableWithCellSpacing.AllowSpacingBetweenCells = true;
			section.Blocks.Add(tableWithCellSpacing);

			section.Blocks.Add(GetTitleParagraph("Table without cell spacing.", 2));

			NTable tableWithoutCellSpacing = CreateTable();
			tableWithoutCellSpacing.AllowSpacingBetweenCells = false;
			section.Blocks.Add(tableWithoutCellSpacing);
		}

		#endregion

		#region Implementation

		private NTable CreateTable()
		{
			NTable table = new NTable();

			int rowCount = 3;
			int colCount = 3;

			// first create the columns
			for (int i = 0; i < colCount; i++)
			{
				table.Columns.Add(new NTableColumn());
			}

			// then add rows with cells count matching the number of columns
			for (int row = 0; row < rowCount; row++)
			{
				NTableRow tableRow = new NTableRow();
				table.Rows.Add(tableRow);

				for (int col = 0; col < colCount; col++)
				{
					NTableCell tableCell = new NTableCell();
					tableRow.Cells.Add(tableCell);
					tableCell.Margins = new NMargins(4);

					tableCell.Border = NBorder.CreateFilledBorder(NColor.Black);
					tableCell.BorderThickness = new NMargins(1);

					NParagraph paragraph = new NParagraph("This is a table cell [" + row.ToString() + ", " + col.ToString() + "]");
					tableCell.Blocks.Add(paragraph);
				}
			}

			return table;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NTableExampleSchema;

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