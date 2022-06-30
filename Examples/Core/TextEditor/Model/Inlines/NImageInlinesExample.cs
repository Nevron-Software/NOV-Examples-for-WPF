using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create inline elements with different formatting
	/// </summary>
	public class NImageInlinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NImageInlinesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NImageInlinesExample()
		{
			NImageInlinesExampleSchema = NSchema.Create(typeof(NImageInlinesExample), NExampleBaseSchema);
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
			NStackPanel stack = new NStackPanel();

			NCheckBox maintainImageAspect = new NCheckBox("Maintain Image Aspect On Resize");
            maintainImageAspect.CheckedChanged += OnMaintainImageAspectCheckedChanged;
			maintainImageAspect.Checked = true;

			stack.Add(maintainImageAspect);

			return stack;
		}

        private void OnMaintainImageAspectCheckedChanged(NValueChangeEventArgs arg)
        {
			bool maintainImageAspect = (bool)arg.NewValue;
			NList<NNode> imageInlines = m_RichText.Content.GetDescendants(NImageInline.NImageInlineSchema);

			for (int i = 0; i < imageInlines.Count; i++)
            {
				((NImageInline)imageInlines[i]).MaintainAspect = maintainImageAspect;
            }
        }

        protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to add raster and metafile image inlines to text documents. Note that metafile images scale without loss of quality. On the right side, you can control whether images will maintain their aspect ratio when resized with the mouse.
</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Image Inlines", "The example shows how to add image inlines with altered preferred width and height.", 1));

			// adding a raster image with automatic size
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Raster image in its original size (125x100):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_Artistic_FishBowl_jpg;
				paragraph.Inlines.Add(imageInline);

				section.Blocks.Add(paragraph);
			}

			// adding a raster image with specified preferred width and height
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Raster image with preferred width and height (250x200):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();

				imageInline.Image = NResources.Image_Artistic_FishBowl_jpg;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 250);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 200);

				paragraph.Inlines.Add(imageInline);

				section.Blocks.Add(paragraph);
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

				section.Blocks.Add(paragraph);
			}

			// adding a metafile image with preferred width and height
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline("Metafile image with preferred width and height (250x200):"));
				paragraph.Inlines.Add(new NLineBreakInline());

				NImageInline imageInline = new NImageInline();
				imageInline.Image = NResources.Image_FishBowl_wmf;
				imageInline.PreferredWidth = new NMultiLength(ENMultiLengthUnit.Dip, 250);
				imageInline.PreferredHeight = new NMultiLength(ENMultiLengthUnit.Dip, 200);				

				paragraph.Inlines.Add(imageInline);

				section.Blocks.Add(paragraph);
			}

		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NImageInlinesExampleSchema;

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
