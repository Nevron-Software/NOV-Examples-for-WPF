using System.Text;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.Text.UI;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
    /// <summary>
    /// The example demonstrates how to programmatically create paragraphs with differnt inline formatting.
    /// </summary>
    public abstract class NTextExampleBase : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NTextExampleBase()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NTextExampleBase()
        {
            NTextExampleBaseSchema = NSchema.Create(typeof(NTextExampleBase), NExampleBase.NExampleBaseSchema);
        }

        #endregion

		#region Protected Overrides - Example

		public override void Initialize()
		{
			base.Initialize();

			// Populate the rich text
			PopulateRichText();
		}
		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// Create the ribbon
			m_RibbonBuilder = new NRichTextRibbonBuilder();
			return m_RibbonBuilder.CreateUI(m_RichText);
		}
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
			
			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.Click += OnSwitchUIButtonClick;
			stack.Add(switchUIButton);

            return stack;
        }

        #endregion

		#region Protected Overridable

		protected virtual void PopulateRichText() 
		{ 
		}

		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		internal static NParagraph GetDescriptionParagraph(string text)
		{
			return new NParagraph(text);
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
			paragraph.BorderThickness = defaultBorderThickness;

            return paragraph;
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		internal static NGroupBlock GetNoteBlock(string text, int level)
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
		/// <summary>
		/// 
		/// </summary>
		/// <param name="title"></param>
		/// <param name="description"></param>
		/// <param name="level"></param>
		/// <returns></returns>
		internal static NGroupBlock GetDescriptionBlock(string title, string description, int level)
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
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		internal static NParagraph GetLoremIpsumParagraph()
		{
			return new NParagraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum placerat in tortor nec tincidunt. Sed sagittis in sem ac auctor. Donec scelerisque molestie eros, a dictum leo fringilla eu. Vivamus porta urna non ullamcorper commodo. Nulla posuere sodales pellentesque. Donec a erat et tortor viverra euismod non et erat. Donec dictum ante eu mauris porta, eget suscipit mi ultrices. Nunc convallis adipiscing ligula, non pharetra dolor egestas at. Etiam in condimentum sapien. Praesent sagittis pulvinar metus, a posuere mauris aliquam eget.");
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

        #endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the rich text view from its parent
			m_RichText.ParentNode.RemoveChild(m_RichText);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				m_ExampleTabPage.Content = m_RibbonBuilder.CreateUI(m_RichText);
			}
			else
			{
				// We are in "Ribbon" mode, so switch to "Command Bars"
				label.Text = SwitchToRibbon;

				// Create the command bars
				if (m_CommandBarBuilder == null)
				{
					m_CommandBarBuilder = new NRichTextCommandBarBuilder();
				}

				m_ExampleTabPage.Content = m_CommandBarBuilder.CreateUI(m_RichText);
			}
		}

		#endregion

		#region Fields

		protected NRichTextView m_RichText;
		protected NRichTextRibbonBuilder m_RibbonBuilder;
		protected NRichTextCommandBarBuilder m_CommandBarBuilder;

        #endregion

        #region Schema

        public static readonly NSchema NTextExampleBaseSchema;

        #endregion

		#region Constants

		internal static readonly NMargins defaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);
		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		#endregion
	}
}