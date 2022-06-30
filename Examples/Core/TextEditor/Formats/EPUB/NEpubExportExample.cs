using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NEpubExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NEpubExportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NEpubExportExample()
		{
			NEpubExportExampleSchema = NSchema.Create(typeof(NEpubExportExample), NExampleBaseSchema);
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

			NButton exportToEpubButton = new NButton("Export to EPUB...");
			exportToEpubButton.Click += OnExportToEpubButtonClick;
			stack.Add(exportToEpubButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to import Electronic Publications (EPUB files) in Nevron Rich Text Editor.
</p>
";
		}

		private void PopulateRichText()
		{
			NDocumentBlock documentBlock = m_RichText.Content;
			NRichTextStyle heading1Style = documentBlock.Styles.FindStyleByTypeAndId(
				ENRichTextStyleType.Paragraph, "Heading1");

			// Add chapter 1
			NSection section = new NSection();
			documentBlock.Sections.Add(section);

			NParagraph paragraph = new NParagraph("Chapter 1: EPUB Import");
			section.Blocks.Add(paragraph);
			heading1Style.Apply(paragraph);

			paragraph = new NParagraph("NOV Rich Text Editor lets you import Electronic Publications. " +
				"Thus you can use it to read e-books on your PC or MAC.");
			section.Blocks.Add(paragraph);

			paragraph = new NParagraph("You can also use it to import and edit existing Electronic Publications and books.");
			section.Blocks.Add(paragraph);

			// Add chapter 2
			section = new NSection();
			section.BreakType = ENSectionBreakType.NextPage;
			documentBlock.Sections.Add(section);

			paragraph = new NParagraph("Chapter 2: EPUB Export");
			section.Blocks.Add(paragraph);
			heading1Style.Apply(paragraph);

			paragraph = new NParagraph("NOV Rich Text Editor lets you export a rich text document to an Electronic Publication. " +
				"Thus you can use it to create e-books on your PC or MAC.");
			section.Blocks.Add(paragraph);

			paragraph = new NParagraph("You can also use it to import and edit existing Electronic Publications and books.");
			section.Blocks.Add(paragraph);
		}

		#endregion

		#region Event Handlers

		private void OnExportToEpubButtonClick(NEventArgs arg)
		{
			// Create and show a save file dialog
			NSaveFileDialog saveDialog = new NSaveFileDialog();
			saveDialog.Title = "Export to EPUB";
			saveDialog.DefaultFileName = "MyBook.epub";
			saveDialog.FileTypes = new NFileDialogFileType[] {
				new NFileDialogFileType(NTextFormat.Epub)
			};

			saveDialog.Closed += OnSaveDialogClosed;
			saveDialog.RequestShow();
		}
		private void OnSaveDialogClosed(NSaveFileDialogResult arg)
		{
			if (arg.Result == ENCommonDialogResult.OK)
			{
				m_RichText.SaveToFile(arg.File);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NEpubExportExample.
		/// </summary>
		public static readonly NSchema NEpubExportExampleSchema;

		#endregion
	}
}