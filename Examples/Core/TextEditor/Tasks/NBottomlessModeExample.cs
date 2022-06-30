using Nevron.Nov.Dom;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Graphics;
using Nevron.Nov.Editors;
using Nevron.Nov.Chart;

namespace Nevron.Nov.Examples.Text
{
	/// <summary>
	/// The example demonstrates how to programmatically a sample report.
	/// </summary>
	public class NBottomlessModeExample : NTextExampleBase
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
			NBottomlessModeExampleSchema = NSchema.Create(typeof(NBottomlessModeExample), NTextExampleBase.NTextExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides

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
			m_RichText.VScrollMode = ENScrollMode.Never;
			m_RichText.HScrollMode = ENScrollMode.Never;
			m_RichText.HRuler.Visibility = ENVisibility.Hidden;
			m_RichText.VRuler.Visibility = ENVisibility.Hidden;
			m_RichText.PreferredWidth = double.NaN;
			m_RichText.PreferredHeight = double.NaN;
			m_RichText.Border = NBorder.CreateFilledBorder(NColor.Black);
			m_RichText.BorderThickness = new NMargins(1);

			m_RichText.HorizontalPlacement = Layout.ENHorizontalPlacement.Fit;
			m_RichText.VerticalPlacement = Layout.ENVerticalPlacement.Top;

			return m_RichText;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			
			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create a bottomless text control.</p>s";
		}

		#endregion

		#region Fields

		#endregion

		#region Schema

		public static readonly NSchema NBottomlessModeExampleSchema;

		#endregion
	}
}