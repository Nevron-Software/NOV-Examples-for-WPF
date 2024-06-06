using Nevron.Nov.Barcode;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NBarcodesInRichTextExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NBarcodesInRichTextExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NBarcodesInRichTextExample()
		{
			NBarcodesInRichTextExampleSchema = NSchema.Create(typeof(NBarcodesInRichTextExample), NExampleBaseSchema);
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
	This example demonstrates how to embed barcodes in rich text documents as widget inlines.
	If you right click the barcode widget you will be able to edit its content and appearance
	through the ""Edit Barcode..."" option.
</p>
";
		}

		private void PopulateRichText()
		{
			NDocumentBlock documentBlock = m_RichText.Content;
			documentBlock.Layout = ENTextLayout.Print;

			NSection section = new NSection();
			documentBlock.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Barcode Widget Inlines",
				"Nevron Open Vision lets you easily insert barcodes in text documents as widget inlines.", 1));

			// Create a table
			NTable table = new NTable(2, 2);
			section.Blocks.Add(table);

			// Create a linear barcode
			NLinearBarcode linearBarcode = new NLinearBarcode(ENLinearBarcodeSymbology.EAN13, "0123456789012");
			NWidgetInline widgetInline = new NWidgetInline(linearBarcode);

			// Create a paragraph to host the linear barcode widget inline
			NTableCell cell = table.Rows[0].Cells[0];
			cell.HorizontalAlignment = ENAlign.Center;
			NParagraph paragraph = (NParagraph)cell.Blocks[0];
			paragraph.Inlines.Add(widgetInline);

			// Create a paragraph to the right with some text
			cell = table.Rows[0].Cells[1];
			paragraph = (NParagraph)cell.Blocks[0];
			paragraph.Inlines.Add(new NTextInline("The linear barcode to the left uses the EAN13 symbology."));

			// Create a QR code widget inline
			NMatrixBarcode qrCode = new NMatrixBarcode(ENMatrixBarcodeSymbology.QrCode, "https://www.nevron.com");
			widgetInline = new NWidgetInline(qrCode);

			// Create a paragraph to host the QR code widget inline
			cell = table.Rows[1].Cells[0];
			cell.HorizontalAlignment = ENAlign.Center;
			paragraph = (NParagraph)cell.Blocks[0];
			paragraph.Inlines.Add(widgetInline);

			// Create a paragraph to the right with some text
			cell = table.Rows[1].Cells[1];
			paragraph = (NParagraph)cell.Blocks[0];
			paragraph.Inlines.Add(new NTextInline("The QR code to the left contains a link to "));
			paragraph.Inlines.Add(new NFieldInline("https://www.nevron.com", new NUrlHyperlink("https://www.nevron.com")));
			paragraph.Inlines.Add(new NTextInline("."));
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NBarcodesInRichTextExample.
		/// </summary>
		public static readonly NSchema NBarcodesInRichTextExampleSchema;

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
		private static NGroupBlock GetNoteBlock(string text, int level)
		{
			NColor color = NColor.Red;
			NParagraph paragraph = GetTitleParagraphNoBorder("Note", level);

			NGroupBlock groupBlock = new NGroupBlock();

			groupBlock.ClearMode = ENClearMode.All;
			groupBlock.Blocks.Add(paragraph);
			groupBlock.Blocks.Add(GetDescriptionParagraph(text));

			groupBlock.Border = CreateLeftTagBorder(color);
			groupBlock.BorderThickness = defaultBorderThickness;

			return groupBlock;
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
		private static NParagraph GetLoremIpsumParagraph()
		{
			return new NParagraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum placerat in tortor nec tincidunt. Sed sagittis in sem ac auctor. Donec scelerisque molestie eros, a dictum leo fringilla eu. Vivamus porta urna non ullamcorper commodo. Nulla posuere sodales pellentesque. Donec a erat et tortor viverra euismod non et erat. Donec dictum ante eu mauris porta, eget suscipit mi ultrices. Nunc convallis adipiscing ligula, non pharetra dolor egestas at. Etiam in condimentum sapien. Praesent sagittis pulvinar metus, a posuere mauris aliquam eget.");
		}

		#endregion

		#region Constants

		private static readonly NMargins defaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

		#endregion
	}
}