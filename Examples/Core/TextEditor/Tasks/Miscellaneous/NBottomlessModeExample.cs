using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically a sample report.
	/// </summary>
	public class NBottomlessModeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// 
		/// </summary>
		public NBottomlessModeExample()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		static NBottomlessModeExample()
		{
			NBottomlessModeExampleSchema = NSchema.Create(typeof(NBottomlessModeExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the rich text
			m_RichText = new NRichTextView();
			m_RichText.AcceptsTab = true;
			m_RichText.Content.Sections.Clear();

			NSection section = new NSection();
			section.Blocks.Add(new NParagraph("Type some content here"));
			m_RichText.Content.Sections.Add(section);

			m_RichText.Content.Layout = ENTextLayout.Web;

			m_RichText.ViewSettings.ExtendLineBreakWithSpaces = false;

			m_RichText.VScrollMode = ENScrollMode.Never;
			m_RichText.HScrollMode = ENScrollMode.Never;
			m_RichText.HRuler.Visibility = ENVisibility.Hidden;
			m_RichText.VRuler.Visibility = ENVisibility.Hidden;
			m_RichText.PreferredWidth = double.NaN;
			m_RichText.PreferredHeight = double.NaN;
			m_RichText.Border = NBorder.CreateFilledBorder(NColor.Black);
			m_RichText.BorderThickness = new NMargins(1);

			m_RichText.HorizontalPlacement = ENHorizontalPlacement.Fit;
			m_RichText.VerticalPlacement = ENVerticalPlacement.Top;

			return m_RichText;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create a bottomless text control.</p>";
		}

		#endregion

		#region Fields

		private NRichTextView m_RichText;

		#endregion

		#region Schema

		public static readonly NSchema NBottomlessModeExampleSchema;

		#endregion
	}
}