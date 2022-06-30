using System;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NDocxImportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NDocxImportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NDocxImportExample()
		{
			NDocxImportExampleSchema = NSchema.Create(typeof(NDocxImportExample), NExampleBaseSchema);
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

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			NGroupBox predefinedDocumentGroupBox = CreatePredefinedDocumentGroupBox();
			predefinedDocumentGroupBox.VerticalPlacement = ENVerticalPlacement.Top;
			return predefinedDocumentGroupBox;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the DOCX import capabilities of Nevron Text control.</p>";
		}

		#endregion

		#region Implementation

		private NGroupBox CreatePredefinedDocumentGroupBox()
		{
			const string DocxSuffix = "_docx";

			NListBox testListBox = new NListBox();
			string[] resourceName = NResources.Instance.GetResourceNames();
			for (int i = 0, count = resourceName.Length; i < count; i++)
			{
				string resName = resourceName[i];
				if (resName.EndsWith(DocxSuffix, StringComparison.Ordinal))
				{
					// The current resource is a DOCX document, so add it to the list box
					string testName = resName.Substring(0, resName.Length - DocxSuffix.Length);
					testName = testName.Substring(testName.LastIndexOf('_') + 1);

                    NListBoxItem item = new NListBoxItem(NStringHelpers.InsertSpacesBeforeUppersAndDigits(testName));
					item.Tag = resName;
					testListBox.Items.Add(item);
				}
			}

			testListBox.Selection.Selected += OnListBoxItemSelected;
			testListBox.Selection.SingleSelect(testListBox.Items[1]);
			return new NGroupBox("Predefined DOCX documents", testListBox);
		}

		#endregion

		#region Event Handlers

		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> arg1)
		{
			if (arg1.TargetNode == null)
				return;

			// Determine the full name of the selected resource
			string resName = (string)arg1.Item.Tag;

			// Read the stream
			using (Stream stream = NResources.Instance.GetResourceStream(resName))
			{
				m_RichText.LoadFromStream(stream);
			}
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NDocxImportExample.
		/// </summary>
		public static readonly NSchema NDocxImportExampleSchema;

		#endregion
	}
}