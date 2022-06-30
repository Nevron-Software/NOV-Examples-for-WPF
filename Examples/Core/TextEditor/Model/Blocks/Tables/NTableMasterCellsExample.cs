using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with differnt inline formatting
	/// </summary>
	public class NTableMasterCellsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NTableMasterCellsExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NTableMasterCellsExample()
		{
            NTableMasterCellsExampleSchema = NSchema.Create(typeof(NTableMasterCellsExample), NExampleBaseSchema);
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
	This example demonstrates how to programmatically create row and column master cells.
</p>
";
		}

		private void PopulateRichText()
		{
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Table Master Cells Example", "This example shows how to programmatically create and add master cells.", 1));

            // first create the table
            NTable table = new NTable(5, 5);
			table.AllowSpacingBetweenCells = false;

            for (int row = 0; row < table.Rows.Count; row++)
            {
                for (int col = 0; col < table.Columns.Count; col++)
                {
                    NParagraph paragraph = new NParagraph("Normal Cell");

					NTableCell tableCell = table.Rows[row].Cells[col];
					tableCell.BorderThickness = new Nov.Graphics.NMargins(1);
					tableCell.Border = NBorder.CreateFilledBorder(NColor.Black);

                    // by default cells contain a single paragraph
					tableCell.Blocks.Clear();
					tableCell.Blocks.Add(paragraph);
                }
            }

            // set cell [0, 2] to be column master
            NTableCell colMasterCell = table.Rows[0].Cells[2];
            colMasterCell.ColSpan = 2;
            colMasterCell.BackgroundFill = new NColorFill(ENNamedColor.LightSkyBlue);
			colMasterCell.Blocks.Clear();
            colMasterCell.Blocks.Add(new NParagraph("Column Master Cell"));

            // set cell [1, 0] to be row master
            NTableCell rowMasterCell = table.Rows[1].Cells[0];
            rowMasterCell.RowSpan = 2;
            rowMasterCell.BackgroundFill = new NColorFill(ENNamedColor.LightSteelBlue);
			rowMasterCell.Blocks.Clear();
            rowMasterCell.Blocks.Add(new NParagraph("Row Master Cell"));

            // set cell [2, 2] to be column and row master
            NTableCell rowColMasterCell = table.Rows[2].Cells[2];
            rowColMasterCell.ColSpan = 2;
            rowColMasterCell.RowSpan = 2;
            rowColMasterCell.BackgroundFill = new NColorFill(ENNamedColor.MediumTurquoise);
			rowColMasterCell.Blocks.Clear();
            rowColMasterCell.Blocks.Add(new NParagraph("Row\\Col Master Cell"));

            section.Blocks.Add(table);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NTableMasterCellsExampleSchema;

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