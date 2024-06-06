using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to programmatically create paragraphs with differnt inline formatting
    /// </summary>
    public class NSelectionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NSelectionExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NSelectionExample()
		{
			NSelectionExampleSchema = NSchema.Create(typeof(NSelectionExample), NExampleBaseSchema);
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
			NStackPanel groupsStack = new NStackPanel();

			// caret navigation
			{
				NGroupBox groupBox = new NGroupBox("Caret Navigation");
				groupsStack.Add(groupBox);

				NStackPanel stack = new NStackPanel();
				groupBox.Content = stack;

				NButton moveToNextWordButton = new NButton("Move Caret to Next Word");
				moveToNextWordButton.Click += new Function<NEventArgs>(OnMoveToNextWordButtonClick);
				stack.Add(moveToNextWordButton);

				NButton moveToPrevWordButton = new NButton("Move Caret to Prev Word");
				moveToPrevWordButton.Click += new Function<NEventArgs>(OnMoveToPrevWordButtonClick);
				stack.Add(moveToPrevWordButton);
			}

			{
				NGroupBox groupBox = new NGroupBox("Range Selection");
				groupsStack.Add(groupBox);

				NStackPanel stack = new NStackPanel();
				groupBox.Content = stack;

				NButton selectCurrentParagraphButton = new NButton("Select Current Paragraph");
				selectCurrentParagraphButton.Click += new Function<NEventArgs>(OnSelectCurrentParagraphButtonClick);
				stack.Add(selectCurrentParagraphButton);

				NButton deleteSelectedTextButton = new NButton("Delete Selected Text");
				deleteSelectedTextButton.Click += new Function<NEventArgs>(OnDeleteSelectedTextButtonClick);
				stack.Add(deleteSelectedTextButton);
			}

			{
				NGroupBox groupBox = new NGroupBox("Copy / Paste");
				groupsStack.Add(groupBox);

				NStackPanel pasteOptionsStack = new NStackPanel();
				groupBox.Content = pasteOptionsStack;

				{
					NButton copyButton = new NButton("Copy");
					copyButton.Click += new Function<NEventArgs>(OnCopyButtonClick);
					pasteOptionsStack.Add(copyButton);

					NButton pasteButton = new NButton("Paste");
					pasteButton.Click += new Function<NEventArgs>(OnPasteButtonClick);
					pasteOptionsStack.Add(pasteButton);

					NCheckBox allowImagesCheckBox = new NCheckBox("Allow Images");
					allowImagesCheckBox.CheckedChanged += OnAllowImagesCheckBoxCheckedChanged;
					allowImagesCheckBox.Checked = true;
					pasteOptionsStack.Add(allowImagesCheckBox);

					NCheckBox allowTablesCheckBox = new NCheckBox("Allow Tables");
					allowTablesCheckBox.CheckedChanged += OnAllowTablesCheckBoxCheckedChanged;
					allowTablesCheckBox.Checked = true;
					pasteOptionsStack.Add(allowTablesCheckBox);

					NCheckBox allowSectionsCheckBox = new NCheckBox("Allow Sections");
					allowSectionsCheckBox.CheckedChanged += OnAllowSectionsCheckBoxCheckedChanged;
					allowSectionsCheckBox.Checked = true;
					pasteOptionsStack.Add(allowSectionsCheckBox);
				}
			}

			{
				NGroupBox groupBox = new NGroupBox("Inline Formatting");
				groupsStack.Add(groupBox);

				NStackPanel stack = new NStackPanel();
				groupBox.Content = stack;

				NButton setBoldStyleButton = new NButton("Set Bold Style");
				setBoldStyleButton.Click += new Function<NEventArgs>(OnSetBoldStyleButtonClick);
				stack.Add(setBoldStyleButton);

				NButton setItalicStyleButton = new NButton("Set Italic Style");
				setItalicStyleButton.Click += new Function<NEventArgs>(OnSetItalicStyleButtonClick);
				stack.Add(setItalicStyleButton);

				NButton clearStyleButton = new NButton("Clear Style");
				clearStyleButton.Click += new Function<NEventArgs>(OnClearStyleButtonClick);
				stack.Add(clearStyleButton);
			}

			return groupsStack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to use the selection object in order to select text as well as how to modify selected text appearance</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Working With the Selection Object", "The example demonstrates how to work with the selection object.", 1));

			for (int i = 0; i < 10; i++)
			{
				section.Blocks.Add(new NParagraph("Now it so happened that on one occasion the princess's golden ball did not fall into the little hand which she was holding up for it, but on to the ground beyond, and rolled straight into the water.  The king's daughter followed it with her eyes, but it vanished, and the well was deep, so deep that the bottom could not be seen.  At this she began to cry, and cried louder and louder, and could not be comforted.  And as she thus lamented someone said to her, \"What ails you, king's daughter?  You weep so that even a stone would show pity.\""));
			}
		}

		#endregion

		#region Event Handlers


		void OnClearStyleButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.ClearFontStyleFromSelectedInlines(ENFontStyle.Bold | ENFontStyle.Italic | ENFontStyle.Strikethrough | ENFontStyle.Underline);
			m_RichText.Focus();
		}

		void OnSetItalicStyleButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.AddFontStyleToSelectedInlines(ENFontStyle.Italic);
			m_RichText.Focus();
		}

		void OnSetBoldStyleButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.AddFontStyleToSelectedInlines(ENFontStyle.Bold);
			m_RichText.Focus();
		}

		void OnPasteButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.Paste();
			m_RichText.Focus();
		}

		void OnCopyButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.SelectAll();
			m_RichText.Selection.Copy();
			m_RichText.Focus();
		}

		void OnDeleteSelectedTextButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.Delete();
			m_RichText.Focus();
		}

		void OnAllowTablesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichText.Selection.PasteOptions.AllowTables = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnAllowSectionsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichText.Selection.PasteOptions.AllowSections = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnAllowImagesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichText.Selection.PasteOptions.AllowImages = ((NCheckBox)arg.TargetNode).Checked;
		}


		void OnSelectCurrentParagraphButtonClick(NEventArgs arg)
		{
			NInline inline = m_RichText.Selection.CaretInline;

			if (inline == null)
				return;

			NParagraph currentParagraph = inline.ParentBlock as NParagraph;

			if (currentParagraph != null)
			{
				m_RichText.Selection.SelectRange(currentParagraph.Range);
			}
		}

		void OnMoveToNextWordButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.MoveCaret(ENCaretMoveDirection.NextWord, false);
			m_RichText.Focus();
		}

		void OnMoveToPrevWordButtonClick(NEventArgs arg)
		{
			m_RichText.Selection.MoveCaret(ENCaretMoveDirection.PrevWord, false);
			m_RichText.Focus();
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NSelectionExampleSchema;

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