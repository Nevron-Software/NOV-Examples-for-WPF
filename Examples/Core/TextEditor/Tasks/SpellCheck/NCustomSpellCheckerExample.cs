using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.SpellCheck;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	public class NCustomSpellCheckerExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCustomSpellCheckerExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomSpellCheckerExample()
		{
			NCustomSpellCheckerExampleSchema = NSchema.Create(typeof(NCustomSpellCheckerExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

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

			m_RichTextView.SpellChecker = new CustomSpellchecker();

			// the following code shows how to disable the mini toolbar and leave only the text proofing context menu builder
			m_RichTextView.ContextMenuBuilder.Groups.Clear();
			m_RichTextView.ContextMenuBuilder.ShowMiniToolbar = false;
			m_RichTextView.ContextMenuBuilder.Groups.Add(new Nevron.Nov.Text.UI.NProofingMenuGroup());

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			NToggleButton spellCheckButton = new NToggleButton("Enable Spell Checking");
			spellCheckButton.CheckedChanged += OnSpellCheckButtonCheckedChanged;
			stack.Add(spellCheckButton);

			spellCheckButton.Checked = true;

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to implement custom spell check functionality. In this example the custom spellchecker searched for the word ""Nevron"" and when it finds it will underline it. This functionality also works when you type text in the editor.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnSpellCheckButtonCheckedChanged(NValueChangeEventArgs arg)
		{
			m_RichTextView.SpellChecker.Enabled = (bool)arg.NewValue;
		}

		#endregion

		#region Fields

		private NRichTextView m_RichTextView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCustomSpellCheckerExample.
		/// </summary>
		public static readonly NSchema NCustomSpellCheckerExampleSchema;

		#endregion

		#region Constants

		private const string Text1 = "Nevron Softuare is a globl leader in component based data vizualization technology " +
			"for a divrese range of Microsoft centric platforms. Built with perfectin, usability and enterprize level featurs " +
			"in mind, our components deliverr advanced digital dachboards and diagrams that are not to be matched.";		
		private const string Text2 = "Tuday Nevron components are used by many Fortune 500 companis and thousands of developers " +
			"and IT profesionals worldwide.";
		private const long BytesInMB = 1024 * 1024;

		#endregion

		#region Nested Types

		/// <summary>
		/// Sample class that shows how to implement a custom spell checker
		/// </summary>
		private class CustomSpellchecker : NSpellChecker
		{
			/// <summary>
			/// Default constructor (mandatory for all NOV DOM objects)
			/// </summary>
			public CustomSpellchecker()
			{

			}
			/// <summary>
			/// Static constructor (mandatory for all NOV DOM objects)
			/// </summary>
			static CustomSpellchecker()
			{
				CustomSpellcheckerSchema = NSchema.Create(typeof(CustomSpellchecker), NSpellChecker.NSpellCheckerSchema);
			}
			/// <summary>
			/// Gets a list of misspelled word ranges
			/// </summary>
			/// <param name="chars"></param>
			/// <param name="protectWordRange">when set to true the spellchecker must check the words that intersect the specified range as the user may be currently typing there.</param>
			/// <param name="protectedWordRange">the protected word range</param>
			/// <returns></returns>
			public override NList<NRangeI> GetMisspelledWordRanges(char[] chars, bool protectWordRange, NRangeI protectedWordRange)
			{
				string text = new string(chars);
				string invalidWord = "Nevron";

				int index = 0;
				NList<NRangeI> misspelledWordRanges = new NList<NRangeI>();

				while ((index = text.IndexOf(invalidWord, index)) != -1)
				{
					NRangeI currentRange = new NRangeI(index, index + invalidWord.Length - 1);
					// skip the currently protected spellcheck word, because the user is currently typing in there
					if (!protectWordRange || !protectedWordRange.Equals(currentRange))
					{
						misspelledWordRanges.Add(currentRange);
					}

					index += invalidWord.Length;
				}

				return misspelledWordRanges;
			}
			public override INIterator<char[]> GetSuggestions(char[] chars, int beginIndex, int endIndex)
			{
				return base.GetSuggestions(chars, beginIndex, endIndex);
			}

			private static readonly NSchema CustomSpellcheckerSchema;
		}
	}

	#endregion
}