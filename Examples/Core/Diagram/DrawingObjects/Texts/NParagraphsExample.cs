using System.Text;

using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NParagraphsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NParagraphsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NParagraphsExample()
        {
            NParagraphsExampleSchema = NSchema.Create(typeof(NParagraphsExample), NExampleBaseSchema);
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
            return @"<p>Demonstrates how to modify different aspects of the rich text paragraphs.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();

			NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape1.SetBounds(10, 10, 600, 900);

            NTextBlock textBlock = new NTextBlock();
			shape1.TextBlock = textBlock;
			textBlock.Padding = new NMargins(20);
			textBlock.Content.Blocks.Clear();
			AddFormattedTextToContent(textBlock.Content);

			drawing.ActivePage.Items.Add(shape1);
		}

		#endregion

		#region Implementation 

		/// <summary>
		/// Adds rich formatted text to the specified text block content
		/// </summary>
		/// <param name="content"></param>
		private void AddFormattedTextToContent(NTextBlockContent content)
		{
			// paragraphs with different horizontal alignment

			NParagraph paragraph;

			for (int i = 0; i < 4; i++)
			{
				paragraph = new NParagraph();

				switch (i)
				{
					case 0:
						paragraph.HorizontalAlignment = ENAlign.Left;
						paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("left")));
						break;
					case 1:
						paragraph.HorizontalAlignment = ENAlign.Center;
						paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("center")));
						break;
					case 2:
						paragraph.HorizontalAlignment = ENAlign.Right;
						paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("right")));
						break;
					case 3:
						paragraph.HorizontalAlignment = ENAlign.Justify;
						paragraph.Inlines.Add(new NTextInline(GetAlignedParagraphText("justify")));
						break;
				}

				content.Blocks.Add(paragraph);
			}

			{
				// borders
				paragraph = new NParagraph();
				paragraph.BorderThickness = new NMargins(2, 2, 2, 2);
				paragraph.Border = NBorder.CreateFilledBorder(NColor.Red);
				paragraph.PreferredWidth = NMultiLength.NewPercentage(50);
				paragraph.Margins = new NMargins(5, 5, 5, 5);
				paragraph.Padding = new NMargins(5, 5, 5, 5);
				paragraph.PreferredWidth = NMultiLength.NewFixed(300);
				paragraph.PreferredHeight = NMultiLength.NewFixed(100);
				NTextInline textInline1 = new NTextInline("Paragraphs can have border, margins and padding as well as preffered size");
				paragraph.Inlines.Add(textInline1);

				content.Blocks.Add(paragraph);
			}

			// First line indent and hanging indent
			content.Blocks.Add(GetTitleParagraph("Paragraph with First Line Indent and Hanging Indent", 2));

			NParagraph paragraphWithIndents = new NParagraph(GetRepeatingText("First line indent -10dip, hanging indent 15dip.", 5));
			paragraphWithIndents.HorizontalAlignment = ENAlign.Left;
			paragraphWithIndents.FirstLineIndent = -10;
			paragraphWithIndents.HangingIndent = 15;
			content.Blocks.Add(paragraphWithIndents);

			// First line indent and hanging indent
			content.Blocks.Add(GetTitleParagraph("Line Spacing", 2));

			NParagraph paragraphWithMultipleLineSpacing = new NParagraph(GetRepeatingText("Line space is two times bigger than normal", 10));
			paragraphWithMultipleLineSpacing.HorizontalAlignment = ENAlign.Left;
			paragraphWithMultipleLineSpacing.LineHeightMode = ENLineHeightMode.Multiple;
			paragraphWithMultipleLineSpacing.LineHeightFactor = 2.0;
			content.Blocks.Add(paragraphWithMultipleLineSpacing);

			NParagraph paragraphWithAtLeastLineSpacing = new NParagraph(GetRepeatingText("Line space is at least 20 dips.", 10));
			paragraphWithAtLeastLineSpacing.HorizontalAlignment = ENAlign.Left;
			paragraphWithAtLeastLineSpacing.LineHeightMode = ENLineHeightMode.AtLeast;
			paragraphWithAtLeastLineSpacing.LineHeight = 20.0;
			content.Blocks.Add(paragraphWithAtLeastLineSpacing);

			NParagraph paragraphWithExactLineSpacing = new NParagraph(GetRepeatingText("Line space is exactly 20 dips.", 10));
			paragraphWithExactLineSpacing.HorizontalAlignment = ENAlign.Left;
			paragraphWithExactLineSpacing.LineHeightMode = ENLineHeightMode.Exactly;
			paragraphWithExactLineSpacing.LineHeight = 20.0;
			content.Blocks.Add(paragraphWithExactLineSpacing);

			// BIDI formatting
			content.Blocks.Add(GetTitleParagraph("Paragraphs with BIDI text", 2));

			paragraph = new NParagraph();
			paragraph.HorizontalAlignment = ENAlign.Left;
			NTextInline latinText1 = new NTextInline("This is some text in English. Followed by Arabic:");
			NTextInline arabicText = new NTextInline("أساسًا، تتعامل الحواسيب فقط مع الأرقام، وتقوم بتخزين الأحرف والمحارف الأخرى بعد أن تُعطي رقما معينا لكل واحد منها. وقبل اختراع ");
			NTextInline latinText2 = new NTextInline("This is some text in English.");


			paragraph.Inlines.Add(latinText1);
			paragraph.Inlines.Add(arabicText);
			paragraph.Inlines.Add(latinText2);

			content.Blocks.Add(paragraph);
		}
		/// <summary>
		/// Gets dummy text for aligned paragraphs
		/// </summary>
		/// <param name="alignment"></param>
		/// <returns></returns>
		private static string GetAlignedParagraphText(string alignment)
		{
			string text = string.Empty;

			for (int i = 0; i < 10; i++)
			{
				if (text.Length > 0)
				{
					text += " ";
				}

				text += "This is " + alignment + " aligned paragraph.";
			}

			return text;
		}
		/// <summary>
		/// Gets the specified text repeated
		/// </summary>
		/// <param name="text"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		internal static string GetRepeatingText(string text, int count)
		{
			StringBuilder builder = new StringBuilder();

			for (int i = 0; i < count; i++)
			{
				if (builder.Length > 0)
				{
					builder.Append(" ");
				}

				builder.Append(text);
			}

			return builder.ToString();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		internal static NParagraph GetTitleParagraphNoBorder(string text, int level)
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
		internal static NParagraph GetTitleParagraph(string text, int level)
		{
			NColor color = NColor.Black;

			NParagraph paragraph = GetTitleParagraphNoBorder(text, level);
			paragraph.HorizontalAlignment = ENAlign.Left;

			paragraph.Border = CreateLeftTagBorder(color);
			paragraph.BorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

			return paragraph;
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

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NParagraphsExample.
		/// </summary>
		public static readonly NSchema NParagraphsExampleSchema;

        #endregion
    }
}