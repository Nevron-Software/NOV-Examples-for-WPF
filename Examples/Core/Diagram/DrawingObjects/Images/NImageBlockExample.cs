using Nevron.Nov.Diagram;
using Nevron.Nov.Diagram.Shapes;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Diagram
{
	public class NImageBlockExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NImageBlockExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NImageBlockExample()
        {
            NImageBlockExampleSchema = NSchema.Create(typeof(NImageBlockExample), NExampleBaseSchema);
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
            return @"<p>Demonstrates image blocks.</p>";
        }

        private void InitDiagram(NDrawingDocument drawingDocument)
        {
            NDrawing drawing = drawingDocument.Content;

            // hide the grid
            drawing.ScreenVisibility.ShowGrid = false;

			NBasicShapeFactory basicShapesFactory = new NBasicShapeFactory();

			NShape shape1 = basicShapesFactory.CreateShape(ENBasicShape.Rectangle);
			shape1.SetBounds(10, 10, 600, 1000);
            NImageBlock imageBlock = shape1.ImageBlock;

			imageBlock.Image = NResources.Image_FishBowl_wmf;
            imageBlock.Padding = new NMargins(20);

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
			content.Blocks.Add(GetTitleParagraph("Bullet lists allow you to apply automatic numbering on paragraphs or groups of blocks.", 1));

			// setting bullet list template type
			{
				content.Blocks.Add(GetTitleParagraph("Following are bullet lists with different formatting", 2));

				ENBulletListTemplateType[] values = NEnum.GetValues<ENBulletListTemplateType>();
				string[] names = NEnum.GetNames<ENBulletListTemplateType>();

				string itemText = "Bullet List Item";

				for (int i = 0; i < values.Length - 1; i++)
				{
					NGroupBlock group = new NGroupBlock();
					content.Blocks.Add(group);
					group.MarginTop = 10;
					group.MarginBottom = 10;

					NBulletList bulletList = new NBulletList(values[i]);
					content.BulletLists.Add(bulletList);

					for (int j = 0; j < 3; j++)
					{
						NParagraph paragraph = new NParagraph(itemText + j.ToString());
						NBulletInline bullet = new NBulletInline();
						bullet.List = bulletList;
						paragraph.Bullet = bullet;

						group.Blocks.Add(paragraph);
					}
				}
			}

			// nested bullet lists
			{
				content.Blocks.Add(GetTitleParagraph("Following is an example of bullets with different bullet level", 2));

				NBulletList bulletList = new NBulletList(ENBulletListTemplateType.Decimal);
				content.BulletLists.Add(bulletList);

				NGroupBlock group = new NGroupBlock();
				content.Blocks.Add(group);

				for (int i = 0; i < 3; i++)
				{
					NParagraph par = new NParagraph("Bullet List Item" + i.ToString(), bulletList, 0);
					group.Blocks.Add(par);

					for (int j = 0; j < 2; j++)
					{
						NParagraph nestedPar = new NParagraph("Nested Bullet List Item" + i.ToString(), bulletList, 1);
						nestedPar.MarginLeft = 10;

						group.Blocks.Add(nestedPar);
					}
				}
			}
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
		/// Schema associated with NImageBlockExample.
		/// </summary>
		public static readonly NSchema NImageBlockExampleSchema;

        #endregion
    }
}