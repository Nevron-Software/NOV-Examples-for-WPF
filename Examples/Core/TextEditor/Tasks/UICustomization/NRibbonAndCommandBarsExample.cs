using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.UI;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NRibbonAndCommandBarsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonAndCommandBarsExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonAndCommandBarsExample()
		{
			NRibbonAndCommandBarsExampleSchema = NSchema.Create(typeof(NRibbonAndCommandBarsExample), NExampleBaseSchema);
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

			// Create and execute a ribbon UI builder
			m_RibbonBuilder = new NRichTextRibbonBuilder();
			return m_RibbonBuilder.CreateUI(m_RichText);
		}
		protected override NWidget CreateExampleControls()
		{
			// Switch UI button
			NButton switchUIButton = new NButton(SwitchToCommandBars);
			switchUIButton.VerticalPlacement = ENVerticalPlacement.Top;
			switchUIButton.Click += OnSwitchUIButtonClick;

			return switchUIButton;
		}
		protected override string GetExampleDescription()
		{
			return "<p>This example demonstrates how to switch the NOV Rich Text commanding interface between ribbon and command bars.</p>";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Rich Text Ribbon and Command Bars",
				"This example demonstrates how to customize the NOV rich text ribbon.", 1));
		}

		#endregion

		#region Implementation

		private void SetUI(NCommandUIHolder oldUiHolder, NWidget widget)
		{
			if (oldUiHolder.ParentNode is NTabPage)
			{
				((NTabPage)oldUiHolder.ParentNode).Content = widget;
			}
			else if (oldUiHolder.ParentNode is NPairBox)
			{
				((NPairBox)oldUiHolder.ParentNode).Box1 = widget;
			}
		}

		#endregion

		#region Event Handlers

		private void OnSwitchUIButtonClick(NEventArgs arg)
		{
			NButton switchUIButton = (NButton)arg.TargetNode;
			NLabel label = (NLabel)switchUIButton.Content;

			// Remove the rich text view from its parent
			NCommandUIHolder uiHolder = m_RichText.GetFirstAncestor<NCommandUIHolder>();
			m_RichText.ParentNode.RemoveChild(m_RichText);

			if (label.Text == SwitchToRibbon)
			{
				// We are in "Command Bars" mode, so switch to "Ribbon"
				label.Text = SwitchToCommandBars;

				// Create the ribbon
				SetUI(uiHolder, m_RibbonBuilder.CreateUI(m_RichText));
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

				SetUI(uiHolder, m_CommandBarBuilder.CreateUI(m_RichText));
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NRichTextRibbonBuilder m_RibbonBuilder;
		private NRichTextCommandBarBuilder m_CommandBarBuilder;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonAndCommandBarsExample.
		/// </summary>
		public static readonly NSchema NRibbonAndCommandBarsExampleSchema;

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

		#endregion

		#region Constants

		private const string SwitchToCommandBars = "Switch to Command Bars";
		private const string SwitchToRibbon = "Switch to Ribbon";

		private static readonly NMargins DefaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

		#endregion
	}
}