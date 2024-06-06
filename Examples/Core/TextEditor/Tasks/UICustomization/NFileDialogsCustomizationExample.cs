using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Commands;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NFileDialogsCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NFileDialogsCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NFileDialogsCustomizationExample()
		{
			NFileDialogsCustomizationExampleSchema = NSchema.Create(typeof(NFileDialogsCustomizationExample), NExampleBaseSchema);
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

			// Replace the "Open", "Save As" and "Insert Image" command actions with the custom ones
			m_RichText.Commander.ReplaceCommandAction(new CustomOpenCommandAction());
			m_RichText.Commander.ReplaceCommandAction(new CustomSaveAsCommandAction());
			m_RichText.Commander.ReplaceCommandAction(new CustomInsertImageCommandAction());

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
			return @"<p>This example demonstrates how to customize the NOV Rich Text Open, Save As and Insert Image file dialogs.</p>";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Ribbon Customization",
				"This example demonstrates how to customize the NOV rich text Open, Save As and Insert Image file dialogs.", 1));
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NFileDialogsCustomizationExample.
		/// </summary>
		public static readonly NSchema NFileDialogsCustomizationExampleSchema;

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
			paragraph.BorderThickness = DefaultBorderThickness;

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
			groupBlock.BorderThickness = DefaultBorderThickness;

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
			groupBlock.BorderThickness = DefaultBorderThickness;

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

		private static readonly NMargins DefaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

		#endregion

		#region Nested Types

		private class CustomOpenCommandAction : NOpenCommandAction
		{
			public CustomOpenCommandAction()
			{
			}
			static CustomOpenCommandAction()
			{
				CustomOpenCommandActionSchema = NSchema.Create(typeof(CustomOpenCommandAction), NOpenCommandActionSchema);
			}

			protected override NTextFormat[] GetFormats()
			{
				return new NTextFormat[]
				{
					NTextFormat.Docx,
					NTextFormat.Rtf
				};
			}

			public static readonly NSchema CustomOpenCommandActionSchema;
		}

		private class CustomSaveAsCommandAction : NSaveAsCommandAction
		{
			public CustomSaveAsCommandAction()
			{
			}
			static CustomSaveAsCommandAction()
			{
				CustomSaveAsCommandActionSchema = NSchema.Create(typeof(CustomSaveAsCommandAction), NSaveAsCommandActionSchema);
			}

			protected override NTextFormat[] GetFormats()
			{
				return new NTextFormat[]
				{
					NTextFormat.Docx,
					NTextFormat.Rtf
				};
			}

			public static readonly NSchema CustomSaveAsCommandActionSchema;
		}

		private class CustomInsertImageCommandAction : NInsertImageCommandAction
		{
			public CustomInsertImageCommandAction()
			{
			}
			static CustomInsertImageCommandAction()
			{
				CustomInsertImageCommandActionSchema = NSchema.Create(typeof(CustomInsertImageCommandAction), NInsertImageCommandActionSchema);
			}

			protected override NImageFormat[] GetFormats()
			{
				return new NImageFormat[]
				{
					NImageFormat.Jpeg,
					NImageFormat.Png
				};
			}

			public static readonly NSchema CustomInsertImageCommandActionSchema;
		}

		#endregion
	}
}