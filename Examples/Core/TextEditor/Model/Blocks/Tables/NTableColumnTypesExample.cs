using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Graphics;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with differnt inline formatting
	/// </summary>
	public class NTableColumnTypesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NTableColumnTypesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NTableColumnTypesExample()
		{
			NTableColumnTypesExampleSchema = NSchema.Create(typeof(NTableColumnTypesExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates how to set the table column preferred width.</p>";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Table Column Types Example", "This example shows how to set the table column's preferred width.", 1));

			{
				// create the table
				NTable table = new NTable();

				table.AllowSpacingBetweenCells = false;

				int columnCount = 5;
				int rowCount = 5;
				
				for (int row = 0; row < rowCount; row++)
				{
					NTableRow tableRow = new NTableRow();
					table.Rows.Add(tableRow);

					for (int col = 0; col < columnCount; col++)
					{
						NParagraph paragraph;

						if (row == 0)
						{
							// set table column preferred width
							string headerText = string.Empty;
							NTableColumn tableColumn = new NTableColumn();

							if (col % 2 == 0)
							{
								tableColumn.BackgroundFill = new NColorFill(NColor.LightGray);
							}
							else
							{
								tableColumn.BackgroundFill = new NColorFill(NColor.Beige);
							}

							switch (col)
							{
								case 0: // Fixed column
									tableColumn.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 80);
									headerText = "Fixed [80dips]";
									break;
								case 1: // Auto
									headerText = "Automatic";
									break;
								case 2: // Percentage
									tableColumn.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 20);
									headerText = "Percentage [20%]";
									break;
								case 3: // Fixed
									tableColumn.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 160);
									headerText = "Fixed [160dips]";
									break;
								case 4: // Percentage
									tableColumn.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Percentage, 30);
									headerText = "Percentage [30%]";
									break;
							}

							table.Columns.Add(tableColumn);
							paragraph = new NParagraph(headerText);
						}
						else
						{
							paragraph = new NParagraph("Cell");
						}

						// by default cells contain a single paragraph
						NTableCell tableCell = new NTableCell();
						tableCell.Border = NBorder.CreateFilledBorder(NColor.Black);
						tableCell.BorderThickness = new NMargins(1);
						tableCell.Blocks.Add(paragraph);

						tableRow.Cells.Add(tableCell);
					}
				}

				section.Blocks.Add(table);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NTableColumnTypesExampleSchema;

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