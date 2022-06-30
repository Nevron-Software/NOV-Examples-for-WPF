using System.Text;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NSpellCheckExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSpellCheckExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSpellCheckExample()
		{
			NSpellCheckExampleSchema = NSchema.Create(typeof(NSpellCheckExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;
			stack.VerticalSpacing = 10;

			m_RichTextView = new NRichTextView();
			m_RichTextView.Content.Sections.Clear();

			NSection section = new NSection();
			m_RichTextView.Content.Sections.Add(section);
			stack.Add(m_RichTextView);

            section.Blocks.Add(new NParagraph(Text1));
			section.Blocks.Add(new NParagraph(Text2));

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NToggleButton spellCheckButton = new NToggleButton("Enable Spell Checking");
			spellCheckButton.CheckedChanged += OnSpellCheckButtonCheckedChanged;
			stack.Add(spellCheckButton);

			NButton suggetsionsButton = new NButton("Get Suggestions");
			suggetsionsButton.Click += OnSuggetsionsButtonClick;
			stack.Add(suggetsionsButton);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the built in spell checker functionality.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnSpellCheckButtonCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichTextView.SpellChecker.Enabled = (bool)arg.NewValue;
		}
		private void OnSuggetsionsButtonClick(NEventArgs args)
		{
			NList<NParagraph> paragraphs = m_RichTextView.Selection.GetSelectedParagraphs();
			if (paragraphs.Count == 0)
				return;

			NParagraph paragraph = paragraphs[0];

			// Determine the current word
			NList<NRangeI> words = paragraph.GetWordRanges();

			int index = m_RichTextView.EditingRoot.Selection.Position.InsertIndex - paragraph.Range.Begin;
			bool hasWord = false;
			NRangeI wordRange = NRangeI.Zero;

			for (int i = 0; i < words.Count; i++)
			{
				if (words[i].Contains(index))
				{
					hasWord = true;
					wordRange = words[i];
				}
			}

			if (!hasWord)
			{
				NMessageBox.Show(this.OwnerWindow, "You should click in a word first", "Warning", ENMessageBoxButtons.OK, ENMessageBoxIcon.Warning);
				return;
			}

			char[] word = paragraph.Text.Substring(wordRange.Begin, wordRange.GetLength() + 1).ToCharArray();
			string title = "Suggestions for '" + new string(word) + "'";
			string content;

			if (m_RichTextView.SpellChecker.IsCorrect(word, 0, word.Length - 1) == false)
			{
				INIterator<char[]> suggestions = m_RichTextView.SpellChecker.GetSuggestions(word, 0, word.Length - 1);
				StringBuilder sb = new StringBuilder();

				while (suggestions.MoveNext())
				{
					if (sb.Length > 0)
					{
						sb.Append("\n");
					}

					sb.Append(suggestions.Current);
				}

				content = sb.ToString();
			}
			else
			{
				content = "The word is correct.";
			}

			NMessageBox.Show(this.OwnerWindow, content, title, ENMessageBoxButtons.OK);
		}

		#endregion

		#region Fields

		private NRichTextView m_RichTextView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSpellCheckExample.
		/// </summary>
		public static readonly NSchema NSpellCheckExampleSchema;

		#endregion

		#region Constants

		private const string Text1 = "Nevron Softuare is a globl leader in component based data vizualization technology " +
			"for a divrese range of Microsoft centric platforms. Built with perfectin, usability and enterprize level featurs " +
			"in mind, our components deliverr advanced digital dachboards and diagrams that are not to be matched.";		
		private const string Text2 = "Tuday Nevron components are used by many Fortune 500 companis and thousands of developers " +
			"and IT profesionals worldwide.";
		private const long BytesInMB = 1024 * 1024;

		#endregion
	}
}