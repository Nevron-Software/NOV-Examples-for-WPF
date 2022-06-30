using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Graphics;
using System.Text;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to modify the table borders, spacing etc.
    /// </summary>
    public class NTableBordersExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NTableBordersExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NTableBordersExample()
        {
            NTableBordersExampleSchema = NSchema.Create(typeof(NTableBordersExample), NExampleBaseSchema);
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
	This example demonstrates how table borders behave when the table allows or not table cells spacing.
</p>
";
		}

		private void PopulateRichText()
        {
            NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

            {
				section.Blocks.Add(GetDescriptionBlock("Table Borders Example", "This example shows how table borders behave when the table allows or does not table cell spacing.", 1));

                // first create the table
                NTable table = CreateTable(2, 3);

				table.AllowSpacingBetweenCells = true;
                table.TableCellHorizontalSpacing = 3;
                table.TableCellVerticalSpacing = 3;

                for (int col = 0; col < table.Columns.Count; col++)
                {
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        NTableCell cell = table.Rows[row].Cells[col];

                        switch (col % 3)
                        {
                            case 0:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Red);
                                cell.Margins = new NMargins(3);
                                cell.Padding  = new NMargins(3);
                                cell.BorderThickness = new NMargins(1);
                                break;
                            case 1:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Green);
                                cell.Margins = new NMargins(5);
                                cell.Padding  = new NMargins(5);
                                cell.BorderThickness = new NMargins(3);
                                break;
                            case 2:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Blue);
                                cell.Margins = new NMargins(7);
                                cell.Padding  = new NMargins(7);
                                cell.BorderThickness = new NMargins(2);
                                break;
                        }
                    }
                }

                table.Border = NBorder.CreateFilledBorder(NColor.Red);
                table.BorderThickness = new NMargins(2, 2, 2, 2);

                section.Blocks.Add(table);

                section.Blocks.Add(new NParagraph("This is a 2x3 table, with borders in collapsed table border mode:"));

                // first create the table
                table = CreateTable(2, 3);

                table.AllowSpacingBetweenCells = false;

                for (int col = 0; col < table.Columns.Count; col++)
                {
                    for (int row = 0; row < table.Rows.Count; row++)
                    {
                        NTableCell cell = table.Rows[row].Cells[col];

                        switch (col % 3)
                        {
                            case 0:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Red);
                                cell.Margins = new NMargins(3);
                                cell.Padding  = new NMargins(3);
                                cell.BorderThickness = new NMargins(1);
                                break;
                            case 1:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Green);
                                cell.Margins = new NMargins(5);
                                cell.Padding  = new NMargins(5);
                                cell.BorderThickness = new NMargins(3);
                                break;
                            case 2:
                                cell.Border = NBorder.CreateFilledBorder(NColor.Blue);
                                cell.Margins = new NMargins(7);
                                cell.Padding  = new NMargins(7);
                                cell.BorderThickness = new NMargins(2);
                                break;
                        }
                    }
                }

                table.Border = NBorder.CreateFilledBorder(NColor.Red);

                section.Blocks.Add(table);
            }
        }

		#endregion

        #region Fields

        private NRichTextView m_RichText;

        #endregion

        #region Schema

        public static readonly NSchema NTableBordersExampleSchema;

        #endregion

		#region Static Methods

		private static NTable CreateTable(int rowCount, int colCount)
        {
            // first create the table
            NTable table = new NTable(rowCount, colCount);

			table.AllowSpacingBetweenCells = true;
            table.TableCellHorizontalSpacing = 3;
            table.TableCellVerticalSpacing = 3;

            for (int col = 0; col < table.Columns.Count; col++)
            {
                // set table column preferred width
                string headerText = string.Empty;
                switch (col)
                {
                    case 0: // Fixed column
						table.Columns[col].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 80);
                        headerText = "Fixed [80dips]";
                        break;
                    case 1: // Auto
                        headerText = "Automatic";
                        break;
                    case 2: // Percentage
						table.Columns[col].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 20);
                        headerText = "Percentage [20%]";
                        break;
                    case 3: // Fixed
						table.Columns[col].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 160);
                        headerText = "Fixed [160dips]";
                        break;
                    case 4: // Percentage
						table.Columns[col].PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 30);
                        headerText = "Percentage [30%]";
                        break;
                }

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    NParagraph paragraph;
                    if (row == 0)
                    {
                        paragraph = new NParagraph(headerText);
                    }
                    else
                    {
                        paragraph = new NParagraph("Cell");
                    }

                    NTableCell cell = table.Rows[row].Cells[col];

					cell.Blocks.Clear();
                    cell.Blocks.Add(paragraph);
                }

                
            }

            return table;
        }

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