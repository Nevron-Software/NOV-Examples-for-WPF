using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Commands;
using Nevron.Nov.Text.UI;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NRibbonCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonCustomizationExample()
		{
			NRibbonCustomizationExampleSchema = NSchema.Create(typeof(NRibbonCustomizationExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			// Add the custom command action to the rich text view's commander
			m_RichText.Commander.Add(new CustomCommandAction());

			m_RibbonBuilder = new NRichTextRibbonBuilder();

			// Rename the "Home" ribbon tab page
			NRibbonTabPageBuilder homeTabBuilder = m_RibbonBuilder.TabPageBuilders[NRichTextRibbonBuilder.TabPageHomeName];
			homeTabBuilder.Name = "Start";

			// Rename the "Font" ribbon group of the "Home" tab page
			NRibbonGroupBuilder fontGroupBuilder = homeTabBuilder.RibbonGroupBuilders[NHomeTabPageBuilder.GroupFontName];
			fontGroupBuilder.Name = "Text";

			// Remove the "Clipboard" ribbon group of the "Home" tab page
			homeTabBuilder.RibbonGroupBuilders.Remove(NHomeTabPageBuilder.GroupClipboardName);

			// Insert the custom ribbon group at the beginning of the home tab page
			homeTabBuilder.RibbonGroupBuilders.Insert(0, new CustomRibbonGroup());

			// Create the ribbon commanding UI
			return m_RibbonBuilder.CreateUI(m_RichText);
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to customize the NOV rich text ribbon.</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Ribbon Customization",
				"This example demonstrates how to customize the NOV rich text ribbon.", 1));
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NRichTextRibbonBuilder m_RibbonBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonCustomizationExample.
		/// </summary>
		public static readonly NSchema NRibbonCustomizationExampleSchema;

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
		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Text.NRibbonCustomizationExample),
			"CustomCommand", "Custom Command");

		#endregion

		#region Nested Types

		public class CustomRibbonGroup : NRibbonGroupBuilder
		{
			public CustomRibbonGroup()
				: base("Custom Group", NResources.Image_Ribbon_16x16_smiley_png)
			{
			}

			protected override void AddRibbonGroupItems(NRibbonGroupItemCollection items)
			{
				// Add the copy command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_clipboard_copy_png,
					Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, NRichTextView.CopyCommand));

				// Add the custom command
				items.Add(CreateRibbonButton(NResources.Image_Ribbon_32x32_smiley_png,
					NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
			}
		}

		public class CustomCommandAction : NTextCommandAction
		{
			#region Constructors

			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomCommandAction()
			{
			}

			/// <summary>
			/// Static constructor.
			/// </summary>
			static CustomCommandAction()
			{
				CustomCommandActionSchema = NSchema.Create(typeof(CustomCommandAction), NTextCommandAction.NTextCommandActionSchema);
			}

			#endregion

			#region Public Overrides

			/// <summary>
			/// Gets the command associated with this command action.
			/// </summary>
			/// <returns></returns>
			public override NCommand GetCommand()
			{
				return CustomCommand;
			}
			/// <summary>
			/// Executes the command action.
			/// </summary>
			/// <param name="target"></param>
			/// <param name="parameter"></param>
			public override void Execute(NNode target, object parameter)
			{
				INRichTextView richTextView = GetRichTextView(target);

				NMessageBox.Show("Rich Text Custom Command executed!", "Custom Command", ENMessageBoxButtons.OK,
					ENMessageBoxIcon.Information);
			}

			#endregion

			#region Schema

			/// <summary>
			/// Schema associated with CustomCommandAction.
			/// </summary>
			public static readonly NSchema CustomCommandActionSchema;

			#endregion
		}

		#endregion
	}
}