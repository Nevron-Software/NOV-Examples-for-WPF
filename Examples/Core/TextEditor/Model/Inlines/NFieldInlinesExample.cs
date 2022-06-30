using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically create inline elements with different formatting
	/// </summary>
	public class NFieldInlinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NFieldInlinesExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NFieldInlinesExample()
		{
			NFieldInlinesExampleSchema = NSchema.Create(typeof(NFieldInlinesExample), NExampleBaseSchema);
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
			
			NButton updateAllFieldsButton = new NButton("Update All Fields");
			updateAllFieldsButton.Click += new Function<NEventArgs>(OnUpdateAllFieldsButtonClick);

			stack.Add(updateAllFieldsButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>This example demonstrates how to add different field inlines as well as how to use a range visitor delegate to update all fields in a block tree.</p>
<p>Press the ""Update All Fields"" button to update all fields.</p>
";
		}

		private void PopulateRichText()
		{
			NSection section = new NSection();
			m_RichText.Content.Sections.Add(section);

			section.Blocks.Add(GetDescriptionBlock("Field Inlines", "The example shows how to use field inlines.", 1));

			section.Blocks.Add(GetNoteBlock("Not all field values are always available. Please check the documentation for more information.", 1));

			// add numeric fields
			section.Blocks.Add(GetTitleParagraph("Numeric Fields", 2));

			ENNumericFieldName[] numericFields = NEnum.GetValues<ENNumericFieldName>();
			string[] numericFieldNames = NEnum.GetNames<ENNumericFieldName>();

			for (int i = 0; i < numericFieldNames.Length; i++)
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline(numericFieldNames[i] + " ["));

				NFieldInline fieldInline = new NFieldInline();
				fieldInline.Value = new NNumericFieldValue(numericFields[i]);
				fieldInline.Text = "Not Updated";
				paragraph.Inlines.Add(fieldInline);

				paragraph.Inlines.Add(new NTextInline("]"));

				section.Blocks.Add(paragraph);
			}

			// add date time fields
			section.Blocks.Add(GetTitleParagraph("Date/Time Fields", 2));

			ENDateTimeFieldName[] dateTimeFields = NEnum.GetValues<ENDateTimeFieldName>();
			string[] dateTimecFieldNames = NEnum.GetNames<ENDateTimeFieldName>();

			for (int i = 0; i < dateTimecFieldNames.Length; i++)
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline(dateTimecFieldNames[i] + " ["));

				NFieldInline fieldInline = new NFieldInline();
				fieldInline.Value = new NDateTimeFieldValue(dateTimeFields[i]);
				fieldInline.Text = "Not Updated";
				paragraph.Inlines.Add(fieldInline);

				paragraph.Inlines.Add(new NTextInline("]"));

				section.Blocks.Add(paragraph);
			}

			// add string fields
			section.Blocks.Add(GetTitleParagraph("String Fields", 2));

			ENStringFieldName[] stringFields = NEnum.GetValues<ENStringFieldName>();
			string[] stringcFieldNames = NEnum.GetNames<ENStringFieldName>();

			for (int i = 0; i < stringcFieldNames.Length; i++)
			{
				NParagraph paragraph = new NParagraph();

				paragraph.Inlines.Add(new NTextInline(stringcFieldNames[i] + " ["));

				NFieldInline fieldInline = new NFieldInline();
				fieldInline.Value = new NStringFieldValue(stringFields[i]);
				fieldInline.Text = "Not Updated";
				paragraph.Inlines.Add(fieldInline);

				paragraph.Inlines.Add(new NTextInline("]"));

				section.Blocks.Add(paragraph);
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		void OnUpdateAllFieldsButtonClick(NEventArgs arg)
		{
			m_RichText.Content.VisitRanges(delegate(NRangeTextElement range)
			{
				NFieldInline field = range as NFieldInline;

				if (field != null)
				{
					field.Update();
				}
			});
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NFieldInlinesExampleSchema;

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

		#endregion

		#region Constants

		private static readonly NMargins defaultBorderThickness = new NMargins(5.0, 0.0, 0.0, 0.0);

		#endregion
	}
}
