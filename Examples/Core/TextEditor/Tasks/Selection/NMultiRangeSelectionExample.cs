using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create paragraphs with different inline formatting.
	/// </summary>
	public class NMultiRangeSelectionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NMultiRangeSelectionExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NMultiRangeSelectionExample()
		{
			NMultiRangeSelectionExampleSchema = NSchema.Create(typeof(NMultiRangeSelectionExample), NExampleBaseSchema);
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
			
			m_FindTextBox = new NTextBox();
			m_FindTextBox.Text = "princess";
			stack.Add(new NPairBox(new NLabel("Find:"), m_FindTextBox, ENPairBoxRelation.Box1AboveBox2));

			NButton selectAll = new NButton("Select All");
			selectAll.Click += new Function<NEventArgs>(OnSelectAllButtonClick);
			stack.Add(selectAll);

			NButton deleteAllButton = new NButton("Delete All");
			deleteAllButton.Click += new Function<NEventArgs>(OnDeleteAllButtonClick);
			stack.Add(deleteAllButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates the ability of the control to select multiple ranges of text.</p>
<p>Press the ""Mark All"" button to select all occurrences of ""Find"".</p>
<p>Press the ""Delete All"" button to delete all occurrences of ""Find""</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Multiple Range Selection", "The example demonstrates how to use multiple range selection.", 1));

			for (int i = 0; i < 10; i++)
			{
				section.Blocks.Add(new NParagraph("Now it so happened that on one occasion the princess's golden ball did not fall into the little hand which she was holding up for it, but on to the ground beyond, and rolled straight into the water.  She followed it with her eyes, but it vanished, and the well was deep, so deep that the bottom could not be seen.  At this she began to cry, and cried louder and louder, and could not be comforted.  And as she thus lamented someone said to her, \"What ails you?  You weep so that even a stone would show pity.\""));
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when the user presses the find all button
		/// </summary>
		/// <param name="arg"></param>
		void OnSelectAllButtonClick(NEventArgs arg)
		{
			// init find settings
			NFindSettings settings = new NFindSettings();
			settings.FindWhat = m_FindTextBox.Text;
			settings.SearchDirection = ENSearchDirection.Forward;

			// loop through all occurances
			NRangeI textRange = NRangeI.Zero;

			// move caret to beginning of document
			m_RichText.Selection.MoveCaret(ENCaretMoveDirection.DocumentBegin, false);

			while (m_RichText.EditingRoot.FindNext(settings, ref textRange))
			{
				m_RichText.Selection.SelectRange(textRange, true);
			}
		}
		/// <summary>
		/// Called when the user presses the replace all button
		/// </summary>
		/// <param name="arg"></param>
		void OnDeleteAllButtonClick(NEventArgs arg)
		{
			// delete the selection
			m_RichText.EditingRoot.Selection.Delete();
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NTextBox m_FindTextBox;

		#endregion

		#region Schema

		public static readonly NSchema NMultiRangeSelectionExampleSchema;

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