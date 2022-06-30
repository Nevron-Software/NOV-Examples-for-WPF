using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to create a simple "Notepad" like editor.
	/// </summary>
	public class NNotepadEditorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NNotepadEditorExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NNotepadEditorExample()
		{
			NNotepadEditorExampleSchema = NSchema.Create(typeof(NNotepadEditorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;

			// make sure line breaks behave like a normal text box
			m_RichText.ViewSettings.ExtendLineBreakWithSpaces = false;
			m_RichText.Content.Sections.Clear();

			// set the content to web and allow only text copy paste
			m_RichText.Content.Layout = ENTextLayout.Web;
			// m_RichText.Content.Selection.ClipboardTextFormats = new NClipboardTextFormat[] { NTextClipboardTextFormat.Instance };

			// add some content
			NSection section = new NSection();

			for (int i = 0; i < 10; i++)
			{
				string text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborumstring.";
				section.Blocks.Add(new NParagraph(text));
			}

			m_RichText.Content.Sections.Add(section);

			m_RichText.HRuler.Visibility = ENVisibility.Visible;
			m_RichText.VRuler.Visibility = ENVisibility.Visible;

			return m_RichText;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			m_WordWrapCheckBox = new NCheckBox("Word Wrap");
			m_WordWrapCheckBox.CheckedChanged += OnWordWrapCheckBoxCheckedChanged;
			stack.Add(m_WordWrapCheckBox);
			m_WordWrapCheckBox.Checked = false;

			OnWordWrapCheckBoxCheckedChanged(null);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return "<p>The example demonstrates how to create a simple \"Notepad\" like editor.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnWordWrapCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichText.Content.WrapMinWidth = m_WordWrapCheckBox.Checked;
			m_RichText.Content.WrapDesiredWidth = m_WordWrapCheckBox.Checked;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NCheckBox m_WordWrapCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NNotepadEditorExampleSchema;

		#endregion
	}
}