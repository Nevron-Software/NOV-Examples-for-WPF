using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NPdfExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NPdfExportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NPdfExportExample()
		{
			NPdfExportExampleSchema = NSchema.Create(typeof(NPdfExportExample), NExampleBaseSchema);
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

			NButton exportToPdfButton = new NButton("Export to PDF...");
			exportToPdfButton.Click += OnExportToPdfButtonClick;
			stack.Add(exportToPdfButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to export a Nevron Rich Text document to PDF.
</p>
";
		}

		private void PopulateRichText()
		{
			// Load a document from resource
			m_RichText.LoadFromResourceAsync(NResources.RBIN_DOCX_ComplexDocument_docx);
		}

		#endregion

		#region Event Handlers

		private void OnExportToPdfButtonClick(NEventArgs arg)
		{
			// Create and show a save file dialog
			NSaveFileDialog saveDialog = new NSaveFileDialog();
			saveDialog.Title = "Export to PDF";
			saveDialog.DefaultFileName = "Document1.pdf";
			saveDialog.FileTypes = new NFileDialogFileType[] {
				new NFileDialogFileType(NTextFormat.Pdf)
			};

			saveDialog.Closed += OnSaveDialogClosed;
			saveDialog.RequestShow();
		}
		private void OnSaveDialogClosed(NSaveFileDialogResult arg)
		{
			if (arg.Result == ENCommonDialogResult.OK)
			{
				m_RichText.SaveToFileAsync(arg.File);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NPdfExportExample.
		/// </summary>
		public static readonly NSchema NPdfExportExampleSchema;

		#endregion
	}
}