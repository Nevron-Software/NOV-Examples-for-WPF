using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Commands;
using Nevron.Nov.Text.UI;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NContextMenuCustomizationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NContextMenuCustomizationExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NContextMenuCustomizationExample()
		{
			NContextMenuCustomizationExampleSchema = NSchema.Create(typeof(NContextMenuCustomizationExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NRichTextViewWithRibbon richTextWithRibbon = new NRichTextViewWithRibbon();
			m_RichText = richTextWithRibbon.View;
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Populate the rich text
			PopulateRichText();

			// Add the custom command action to the rich text view's commander
			m_RichText.Commander.Add(new CustomCommandAction());

			// Get the context menu builder
			NRichTextContextMenuBuilder builder = m_RichText.ContextMenuBuilder;

			// Remove the common menu group, which contains commands such as Cut, Copy, Paste, etc.
			builder.Groups.RemoveByName(NRichTextMenuGroup.Common);

			// Add the custom context menu group
			builder.Groups.Add(new CustomMenuGroup());

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to customize the NOV rich text context menu. The common menu group,
which contains command such as Cut, Copy, Paste, etc. is removed and a custom menu group which contains
only the Copy command and a custom command is added to the context menu builder of the rich text view's
selection.</p>
";
		}		

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Context Menu Customization",
				"The example demonstrates how to customize the NOV rich text context menu.", 1));
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NContextMenuCustomizationExample.
		/// </summary>
		public static readonly NSchema NContextMenuCustomizationExampleSchema;

		#endregion

		#region Constants

		public static readonly NCommand CustomCommand = NCommand.Create(typeof(Nevron.Nov.Examples.Text.NContextMenuCustomizationExample),
			"CustomCommand", "Custom Command");

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

		#region Nested Types

		/// <summary>
		/// A custom menu group, which applies to every text element and adds only a Copy menu item
		/// and a custom command menu item.
		/// </summary>
		public class CustomMenuGroup : NRichTextMenuGroup
		{
			/// <summary>
			/// Default constructor.
			/// </summary>
			public CustomMenuGroup()
				: base("Custom")
			{
			}

			/// <summary>
			/// Gets whether this context menu group should be shown for the given
			/// text element. Overriden to return true for every text element.
			/// </summary>
			/// <param name="textElement"></param>
			/// <returns></returns>
			public override bool AppliesTo(NTextElement textElement)
			{
				return true;
			}
			/// <summary>
			/// Appends the context menu action items from this group to the given menu.
			/// </summary>
			/// <param name="menu">The menu to append to.</param>
			/// <param name="textElement">The text element this menu is built for.</param>
			public override void AppendActionsTo(NMenu menu, NTextElement textElement)
			{
				// Add the "Copy" command
				menu.Items.Add(CreateMenuItem(Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, NRichTextView.CopyCommand));

				// Add the custom command
				menu.Items.Add(CreateMenuItem(NResources.Image_Ribbon_16x16_smiley_png, CustomCommand));
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