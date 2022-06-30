using System.Text;

using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to get document statistics
	/// </summary>
	public class NDocumentStatisticsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NDocumentStatisticsExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NDocumentStatisticsExample()
		{
			NDocumentStatisticsExampleSchema = NSchema.Create(typeof(NDocumentStatisticsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			// set to print to have page count statistics
			m_RichText.Content.Layout = ENTextLayout.Print;

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

			NButton statisticsButton = new NButton("Get Statistics");
			statisticsButton.Click += StatisticsButton_Click;
			stack.Add(statisticsButton);

			m_StatisticsTextBox = new NTextBox();
			m_StatisticsTextBox.Multiline = true;
			m_StatisticsTextBox.ReadOnly = true;
			stack.Add(m_StatisticsTextBox);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return "<p>The example demonstrates how to get document statistics about character count, word count etc.</p>";
		}

		#endregion

		#region Event Handlers

		private void StatisticsButton_Click(NEventArgs arg)
		{
			StringBuilder builder = new StringBuilder();

			builder.AppendLine("Character Count: " + m_RichText.Content.Statistics.CharactersWithSpacesCount.ToString());
			builder.AppendLine("Word Count: " + m_RichText.Content.Statistics.WordCount.ToString());
			builder.AppendLine("Line Count: " + m_RichText.Content.Statistics.LineCount.ToString());
			builder.AppendLine("Paragraph Count: " + m_RichText.Content.Statistics.ParagraphCount.ToString());
			builder.AppendLine("Page Count: " + m_RichText.Content.Statistics.PageCount.ToString());

			m_StatisticsTextBox.Text = builder.ToString();
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;
		private NTextBox m_StatisticsTextBox;

		#endregion

		#region Schema

		public static readonly NSchema NDocumentStatisticsExampleSchema;

		#endregion
	}
}