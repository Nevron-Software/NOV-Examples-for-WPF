using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to create a simple "WordPad" like editor.
	/// </summary>
	public class NWordPadEditorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NWordPadEditorExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NWordPadEditorExample()
		{
			NWordPadEditorExampleSchema = NSchema.Create(typeof(NWordPadEditorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			NSimpleRichTextViewWithRibbon richTextWithRibbon = new NSimpleRichTextViewWithRibbon();
			NRichTextView richTextView = richTextWithRibbon.View;

			// add some content
			NSection section = new NSection();

			for (int i = 0; i < 10; i++)
			{
				string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborumstring.";
				section.Blocks.Add(new NParagraph(text));
			}

			richTextView.Content.Sections.Add(section);
			richTextView.HRuler.Visibility = ENVisibility.Visible;
			richTextView.VRuler.Visibility = ENVisibility.Visible;

			return richTextWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return "<p>The example demonstrates how to use the <b>NSimpleRichTextViewWithRibbon</b> widget, which represents a simple \"WordPad\" like text editor.</p>";
		}

        #endregion

		#region Schema

		public static readonly NSchema NWordPadEditorExampleSchema;

		#endregion
	}
}