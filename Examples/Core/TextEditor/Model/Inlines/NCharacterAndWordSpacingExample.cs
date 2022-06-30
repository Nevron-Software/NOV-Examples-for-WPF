using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create inline elements with different formatting
	/// </summary>
	public class NCharacterAndWordSpacingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NCharacterAndWordSpacingExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NCharacterAndWordSpacingExample()
		{
			NCharacterAndWordSpacingExampleSchema = NSchema.Create(typeof(NCharacterAndWordSpacingExample), NExampleBaseSchema);
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

			m_CharacterSpacing = new NNumericUpDown();
			m_CharacterSpacing.Value = 0;
			m_CharacterSpacing.ValueChanged += OnCharacterSpacingFactorValueChanged;

			m_WordSpacing = new NNumericUpDown();
			m_WordSpacing.Value = 0;
			m_WordSpacing.ValueChanged += OnWordSpacingFactorValueChanged;

			stack.Add(new NPairBox(new NLabel("Character Spacing:"), m_CharacterSpacing, ENPairBoxRelation.Box1BeforeBox2));
			stack.Add(new NPairBox(new NLabel("Word Spacing:"), m_WordSpacing, ENPairBoxRelation.Box1BeforeBox2));

			return new NUniSizeBoxGroup(stack);
		}

        protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to apply character and word spacing.
</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			for (int i = 0; i < 3; i++)
			{
				NParagraph paragraph = new NParagraph();
				section.Blocks.Add(paragraph);

				paragraph.Inlines.Add(new NTextInline("This example demonstrates the ability to apply ", ENFontStyle.Underline));

				if (i == 1)
				{
					paragraph.Inlines.Add(new NTextInline("Character and Word (This is inline with modified character and word spacing) ", ENFontStyle.BoldItalic));
				}

                paragraph.Inlines.Add(new NTextInline("spacing to inlines and individual blocks."));

				if (i == 0)
                {
					paragraph.Inlines.Add(new NTextInline("This paragraph has modified character and word spacing. Use the controls on the right to set different word and character spacing"));
                }
			}
		}

		#endregion

		#region Event Handlers

		private void OnWordSpacingFactorValueChanged(NValueChangeEventArgs arg)
		{
			NList<NTextElement> textElements = CollectTextElements();

			for (int i = 0; i < textElements.Count; i++)
			{
				textElements[i].FontWordSpacing = m_WordSpacing.Value;
			}
		}

		private void OnCharacterSpacingFactorValueChanged(NValueChangeEventArgs arg)
		{
			NList<NTextElement> textElements = CollectTextElements();

			for (int i = 0; i < textElements.Count; i++)
			{
				textElements[i].FontCharacterSpacing = m_CharacterSpacing.Value;
			}
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Collects some text elements to apply character and word spacing to. You can apply 
		/// </summary>
		/// <returns></returns>
		private NList<NTextElement> CollectTextElements()
        {
			NList<NTextElement> targetElements = new NList<NTextElement>();
			NList<NNode> paragraphs = m_RichText.Content.GetDescendants(NParagraph.NParagraphSchema);

			if (paragraphs.Count > 0)
            {
				targetElements.Add((NTextElement)paragraphs[0]);
			}

			if (paragraphs.Count > 1)
			{
				if (((NParagraph)paragraphs[1]).Inlines.Count > 1)
                {
					targetElements.Add(((NParagraph)paragraphs[1]).Inlines[1]);
				}
			}

			return targetElements;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		private NNumericUpDown m_CharacterSpacing;
		private NNumericUpDown m_WordSpacing;

		#endregion

		#region Schema

		public static readonly NSchema NCharacterAndWordSpacingExampleSchema;

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