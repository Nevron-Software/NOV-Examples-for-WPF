using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NEpubImportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NEpubImportExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NEpubImportExample()
		{
			NEpubImportExampleSchema = NSchema.Create(typeof(NEpubImportExample), NExampleBaseSchema);
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
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to import Electronic Publications (EPUB files) in the Nevron Rich Text Editor.
</p>
";
		}

		private void PopulateRichText()
		{
			m_RichText.LoadFromResourceAsync(NResources.RBIN_EPUB_GeographyOfBliss_epub, NTextFormat.Epub);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NEpubImportExample.
		/// </summary>
		public static readonly NSchema NEpubImportExampleSchema;

		#endregion
	}
}