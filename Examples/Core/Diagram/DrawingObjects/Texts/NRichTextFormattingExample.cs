using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NRichTextFormattingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NRichTextFormattingExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NRichTextFormattingExample()
        {
            NRichTextFormattingExampleSchema = NSchema.Create(typeof(NRichTextFormattingExample), NExampleBaseSchema);
        }

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

			m_DrawingView.Document.HistoryService.Pause();
			try
			{
				InitDiagram(m_DrawingView.Document);
			}
			finally
			{
				m_DrawingView.Document.HistoryService.Resume();
			}

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
        {
            return @"<p>Demonstrates how to apply rich text formatting to texts.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();

			NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape1.SetBounds(10, 10, 381, 600);

			NTextBlock textBlock1 = new NTextBlock();
			shape1.TextBlock = textBlock1;
			textBlock1.Content.Blocks.Clear();
			AddFormattedTextToContent(textBlock1.Content);
			drawing.ActivePage.Items.Add(shape1);

			NShape shape2 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape2.SetBounds(401, 10, 381, 600);
			NTextBlock textBlock2 = new NTextBlock();
			shape2.TextBlock = textBlock2;
			textBlock2.Content.Blocks.Clear();
			AddFormattedTextWithImagesToContent(textBlock2.Content);
			drawing.ActivePage.Items.Add(shape2);
		}

		#endregion

		#region Implementation 

		/// <summary>
		/// Adds rich formatted text to the specified text block content
		/// </summary>
		/// <param name="content"></param>
		private void AddFormattedTextToContent(NTextBlockContent content)
		{
			{
				// font style control
				NParagraph paragraph = new NParagraph();

				NTextInline textInline1 = new NTextInline("This paragraph contains text inlines with altered ");
				paragraph.Inlines.Add(textInline1);

				NTextInline textInline2 = new NTextInline("Font Name, ");
				textInline2.FontName = "Tahoma";
				paragraph.Inlines.Add(textInline2);

				NTextInline textInline3 = new NTextInline("Font Size, ");
				textInline3.FontSize = 14;
				paragraph.Inlines.Add(textInline3);

				NTextInline textInline4 = new NTextInline("Font Style (Bold), ");
				textInline4.FontStyle |= ENFontStyle.Bold;
				paragraph.Inlines.Add(textInline4);

				NTextInline textInline5 = new NTextInline("Font Style (Italic), ");
				textInline5.FontStyle |= ENFontStyle.Italic;
				paragraph.Inlines.Add(textInline5);

				NTextInline textInline6 = new NTextInline("Font Style (Underline), ");
				textInline6.FontStyle |= ENFontStyle.Underline;
				paragraph.Inlines.Add(textInline6);

				NTextInline textInline7 = new NTextInline("Font Style (StrikeTrough) ");
				textInline7.FontStyle |= ENFontStyle.Strikethrough;
				paragraph.Inlines.Add(textInline7);

				NTextInline textInline8 = new NTextInline(", and Font Style All.");
				textInline8.FontStyle = ENFontStyle.Bold | ENFontStyle.Italic | ENFontStyle.Underline | ENFontStyle.Strikethrough;
				paragraph.Inlines.Add(textInline8);

				content.Blocks.Add(paragraph);
			}

			{
				// appearance control
				NParagraph paragraph = new NParagraph();

				NTextInline textInline1 = new NTextInline("Each text inline element can contain text with different fill and background. ");
				paragraph.Inlines.Add(textInline1);

				NTextInline textInline2 = new NTextInline("Fill (Red), Background Fill Inherit. ");
				textInline2.Fill = new NColorFill(ENNamedColor.Red);
				paragraph.Inlines.Add(textInline2);

				NTextInline textInline3 = new NTextInline("Fill inherit, Background Fill (Green).");
				textInline3.BackgroundFill = new NColorFill(ENNamedColor.Green);
				paragraph.Inlines.Add(textInline3);

				content.Blocks.Add(paragraph);
			}

			{
				// line breaks
				// appearance control
				NParagraph paragraph = new NParagraph();

				NTextInline textInline1 = new NTextInline("Line breaks allow you to break...");
				paragraph.Inlines.Add(textInline1);

				NLineBreakInline lineBreak = new NLineBreakInline();
				paragraph.Inlines.Add(lineBreak);

				NTextInline textInline2 = new NTextInline("the current line in the paragraph.");
				paragraph.Inlines.Add(textInline2);

				content.Blocks.Add(paragraph);
			}

			{
				// tabs
				NParagraph paragraph = new NParagraph();

				NTabInline tabInline = new NTabInline();
				paragraph.Inlines.Add(tabInline);

				NTextInline textInline1 = new NTextInline("(Tabs) are not supported by HTML, however, they are essential when importing text documents.");
				paragraph.Inlines.Add(textInline1);

				content.Blocks.Add(paragraph);
			}
		}
		/// <summary>
		/// Adds formatted text with image elements to the specified text block content
		/// </summary>
		/// <param name="content"></param>
		private void AddFormattedTextWithImagesToContent(NTextBlockContent content)
		{
			// adding a raster image with automatic size
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Raster image in its original size (125x100):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_Artistic_FishBowl_jpg;
				paragraph.Inlines.Add(imageInline);

				content.Blocks.Add(paragraph);
			}


			// adding a raster image with specified preferred width and height
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Raster image with preferred width and height (80x60):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();

				imageInline.Image = NResources.Image_Artistic_FishBowl_jpg;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 80);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 60);

				paragraph.Inlines.Add(imageInline);

				content.Blocks.Add(paragraph);
			}

			// adding a metafile image with preferred width and height
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Metafile image with preferred width and height (125x100):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_FishBowl_wmf;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 125);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 100);

				paragraph.Inlines.Add(imageInline);

				content.Blocks.Add(paragraph);
			}


			// adding a metafile image with preferred width and height
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Metafile image with preferred width and height (80x60):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_FishBowl_wmf;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 80);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 60);

				paragraph.Inlines.Add(imageInline);

				content.Blocks.Add(paragraph);
			}
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRichTextFormattingExample.
		/// </summary>
		public static readonly NSchema NRichTextFormattingExampleSchema;

        #endregion
    }
}