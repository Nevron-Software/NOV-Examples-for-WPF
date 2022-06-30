using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NTableStylesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTableStylesExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTableStylesExample()
		{
			NTableStylesExampleSchema = NSchema.Create(typeof(NTableStylesExample), NExampleBaseSchema);
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
			return @"<p>This example demonstrates how to create and apply table styles.</p>";
		}

		private void PopulateRichText()
		{
			NDocumentBlock documentBlock = m_RichText.Content;
			NSection section = new NSection();
			documentBlock.Sections.Add(section);

			// Create the first table
			section.Blocks.Add(new NParagraph("Table with predefined table style:"));
			NTable table1 = CreateTable();
			section.Blocks.Add(table1);

			// Apply a predefined table style
			NRichTextStyle predefinedStyle = documentBlock.Styles.FindStyleByTypeAndId(ENRichTextStyleType.Table, "GridTable2Blue");
			predefinedStyle.Apply(table1);

			// Create the second table
			NParagraph paragraph = new NParagraph("Table with custom table style:");
			paragraph.MarginTop = 30;
			section.Blocks.Add(paragraph);
			NTable table2 = CreateTable();
			section.Blocks.Add(table2);

			// Create a custom table style and apply it
			NTableStyle customStyle = new NTableStyle("CustomTableStyle");
			customStyle.WholeTable = new NTablePartStyle();
			customStyle.WholeTable.BorderRule = new NBorderRule(ENPredefinedBorderStyle.Solid, NColor.DarkRed, new NMargins(1));
			customStyle.WholeTable.BorderRule.InsideHSides = new NBorderSideRule(ENPredefinedBorderStyle.Solid, NColor.DarkRed, 1);
			customStyle.WholeTable.BorderRule.InsideVSides = new NBorderSideRule(ENPredefinedBorderStyle.Solid, NColor.DarkRed, 1);

			customStyle.FirstRow = new NTablePartStyle();
			customStyle.FirstRow.BackgroundFill = new NColorFill(NColor.DarkRed);
			customStyle.FirstRow.InlineRule = new NInlineRule(NColor.White);
			customStyle.FirstRow.InlineRule.FontStyle = ENFontStyle.Bold;

			customStyle.Apply(table2);
		}

		#endregion

		#region Implementation

		private NTable CreateTable()
		{
			NTable table = new NTable();
			table.AllowSpacingBetweenCells = false;

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

		/// <summary>
		/// Schema associated with NTableStylesExample.
		/// </summary>
		public static readonly NSchema NTableStylesExampleSchema;

		#endregion
	}
}