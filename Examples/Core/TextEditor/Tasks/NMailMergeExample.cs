using System.Globalization;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Data;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// Demonstrates how to use the mail merge functionality of the Nevron Rich Text control.
	/// </summary>
	public class NMailMergeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMailMergeExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMailMergeExample()
		{
			NMailMergeExampleSchema = NSchema.Create(typeof(NMailMergeExample), NExampleBaseSchema);
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

			if (NApplication.IntegrationPlatform != ENIntegrationPlatform.Silverlight)
			{
				NButton mergeAndSaveToFolderButton = new NButton("Merge & Save to Folder");
				mergeAndSaveToFolderButton.Click += OnMergeAndSaveToFolderButtonClick;
				stack.Add(mergeAndSaveToFolderButton);
			}

			return stack;
		}
		protected override string GetExampleDescription()
		{
			NDataUri previewMailMergeUri = NDataUri.FromImage(NResources.Image_Documentation_PreviewResults_png);
			return @"
<p>
	This example demonstrates how to use the mail merge functionality of the Nevron Rich Text control.
</p>
<p>
	Click the <b>Preview Mail Merge</b> button (&nbsp;<img src=""" + previewMailMergeUri.ToString() +
	@""" />&nbsp;) from the <b>Mailings</b> ribbon tab to see the values for the currently selected
    mail merge record. When ready click the <b>Merge & Save</b> button to save all merged documents to a file.
</p>
<p>
	The <b>Merge & Save</b> button saves each of the individual documents result of the mail
	merge operation to a folder.	
</p>
";
		}

		private void PopulateRichText()
		{
			// Create some text
			NDocumentBlock documentBlock = m_RichText.Content;
			documentBlock.Layout = ENTextLayout.Print;

			NSection section = new NSection();
			documentBlock.Sections.Add(section);

			NParagraph paragraph = new NParagraph();
			paragraph.Inlines.Add(CreateMailMergeField(new NGreetingLineFieldValue()));
			section.Blocks.Add(paragraph);

			paragraph = new NParagraph();
			paragraph.Inlines.Add(new NTextInline("We would like to offer you a unique software component that will help you leverage multiple platforms with single code base. We believe that as a "));
			paragraph.Inlines.Add(CreateMailMergeField(new NMailMergeSourceFieldValue("Title")));
			paragraph.Inlines.Add(new NTextInline(" in your company you will be very interested in this solution. If that's the case do not hesitate to contact us in order to arrange a meeting in "));
			paragraph.Inlines.Add(CreateMailMergeField(new NMailMergePredefinedFieldValue(ENMailMergeDataField.City)));
			paragraph.Inlines.Add(new NTextInline("."));
			section.Blocks.Add(paragraph);

			paragraph = new NParagraph();
			paragraph.Inlines.Add(new NTextInline("Best Regards,"));
			paragraph.Inlines.Add(new NLineBreakInline());
			paragraph.Inlines.Add(new NTextInline("Nevron Software"));
			paragraph.Inlines.Add(new NLineBreakInline());
			paragraph.Inlines.Add(new NFieldInline("www.nevron.com", new NUrlHyperlink("https://www.nevron.com")));
			section.Blocks.Add(paragraph);

			// Load a mail merge data source from resource
			Stream stream = NResources.Instance.GetResourceStream("RSTR_Employees_csv");
			NMailMergeDataSource dataSource = NDataSourceFormat.Csv.LoadFromStream(stream, new NDataSourceLoadSettings(null, null, true));

			// Create the field mappings
            NMailMergeFieldMap fieldMap = new NMailMergeFieldMap();

			fieldMap.Set(ENMailMergeDataField.CourtesyTitle, "TitleOfCourtesy");
			fieldMap.Set(ENMailMergeDataField.FirstName, "FirstName");
			fieldMap.Set(ENMailMergeDataField.LastName, "LastName");
			fieldMap.Set(ENMailMergeDataField.City, "City");

            dataSource.FieldMap = fieldMap;
            documentBlock.MailMerge.DataSource = dataSource;
        }

		#endregion

		#region Implementation

		private void MergeAndSaveToFolder(string targetPath)
		{
			NFolder folder = NFileSystem.Current.GetFolder(targetPath);
			
			if (folder == null)
			{
				NMessageBox.Show("The entered target path does not exist", "Error",
					ENMessageBoxButtons.OK, ENMessageBoxIcon.Error);
				return;
			}

			// Clone the rich text view
			NRichTextView clonedRichTextView = (NRichTextView)m_RichText.DeepClone();

			// Switch the mail merge of the cloned rich text view to preview mode
			NMailMerge mailMerge = clonedRichTextView.Content.MailMerge;
			mailMerge.PreviewMailMerge = true;

			// Loop through all mail merge records to save individual documents to file
			for (int i = 0; i < mailMerge.DataRecordCount; i++)
			{
				// Move to the next data source record
				mailMerge.CurrentDataRecordIndex = i;

				// Save the merged document to file
				string fileName = "Document" + i.ToString(CultureInfo.InvariantCulture) + ".docx";
				clonedRichTextView.SaveToFileAsync(NPath.Current.Combine(targetPath, fileName));
			}

			NMessageBox.Show("Merged documents saved to \"" + targetPath + "\".", "Mail Merge Complete",
				ENMessageBoxButtons.OK, ENMessageBoxIcon.Information);
			
		}

		#endregion

		#region Event Handlers

		private void OnMergeAndSaveToFolderButtonClick(NEventArgs arg)
		{
			NTextBox textBox = new NTextBox();
			NButtonStrip buttonStrip = new NButtonStrip();
			buttonStrip.AddOKCancelButtons();
			NPairBox pairBox = new NPairBox(textBox, buttonStrip, ENPairBoxRelation.Box1AboveBox2);

			NTopLevelWindow dialog = NApplication.CreateTopLevelWindow();
			dialog.SetupDialogWindow("Enter Folder Path", false);
			dialog.Content = pairBox;
			dialog.Closed += OnEnterFolderDialogClosed;
			dialog.Open();
		}
		private void OnEnterFolderDialogClosed(NEventArgs arg)
		{
			NTopLevelWindow dialog = (NTopLevelWindow)arg.TargetNode;
			if (dialog.Result == ENWindowResult.OK)
			{
				NTextBox textBox = (NTextBox)dialog.Content.GetFirstDescendant(NTextBox.NTextBoxSchema);
				MergeAndSaveToFolder(textBox.Text);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMailMergeExample.
		/// </summary>
		public static readonly NSchema NMailMergeExampleSchema;

		#endregion

		#region Static Methods

		private static NFieldInline CreateMailMergeField(NMailMergeFieldValue value)
		{
			NFieldInline fieldInline = new NFieldInline(value);
			fieldInline.FontStyle = ENFontStyle.Bold;
			return fieldInline;
		}

		#endregion
	}
}