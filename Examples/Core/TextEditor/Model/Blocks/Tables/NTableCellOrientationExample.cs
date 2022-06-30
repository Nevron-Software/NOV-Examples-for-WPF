using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to control the table cell orientation and alignment
	/// </summary>
	public class NTableCellOrientationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NTableCellOrientationExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NTableCellOrientationExample()
		{
            NTableCellOrientationExampleSchema = NSchema.Create(typeof(NTableCellOrientationExample), NExampleBaseSchema);
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
	This example shows how to programmatically modify the orientation of cells.
</p>
";
		}

		private void PopulateRichText()
		{
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Table Cell Orientation Example", "This example shows how to programmatically modify the orientation of cells.", 1));

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

            // set cell [1, 0] to be row master
            NTableCell leftToRightCell = table.Rows[1].Cells[0];
            leftToRightCell.RowSpan = int.MaxValue;
            leftToRightCell.BackgroundFill = new NColorFill(ENNamedColor.LightSteelBlue);
			leftToRightCell.Blocks.Clear();
            leftToRightCell.Blocks.Add(new NParagraph("Cell With Left to Right Orientation"));
            leftToRightCell.TextDirection = ENTableCellTextDirection.LeftToRight;
            leftToRightCell.HorizontalAlignment = ENAlign.Center;
            leftToRightCell.VerticalAlignment = ENVAlign.Center;

            // set cell [1, 0] to be row master
            NTableCell rightToLeftCell = table.Rows[1].Cells[table.Columns.Count - 1];
            rightToLeftCell.RowSpan = int.MaxValue;
            rightToLeftCell.BackgroundFill = new NColorFill(ENNamedColor.LightGreen);
            rightToLeftCell.Blocks.Clear();
            rightToLeftCell.Blocks.Add(new NParagraph("Cell With Right to Left Orientation"));
            rightToLeftCell.TextDirection = ENTableCellTextDirection.RightToLeft;
            rightToLeftCell.HorizontalAlignment = ENAlign.Center;
            rightToLeftCell.VerticalAlignment = ENVAlign.Center;

            section.Blocks.Add(table);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NTableCellOrientationExampleSchema;

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
