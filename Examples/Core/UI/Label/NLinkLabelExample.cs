using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NLinkLabelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NLinkLabelExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NLinkLabelExample()
		{
			NLinkLabelExampleSchema = NSchema.Create(typeof(NLinkLabelExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);

			NLinkLabel webLinkLabel = new NLinkLabel("Nevron Website", "https://www.nevron.com/");
			stack.Add(webLinkLabel);

			NLinkLabel emailLinkLabel = new NLinkLabel("Nevron Support Email", "mailto:support@nevron.com");
			stack.Add(emailLinkLabel);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}

		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use link labels. The first label leads to Nevron's website and the second one
	should open your default email client when clicked. When a link label is clicked, it is automatically marked as visited by
	setting its <b>IsVisited</b> property to true.
</p>
";
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLinkLabelExample.
		/// </summary>
		public static readonly NSchema NLinkLabelExampleSchema;

		#endregion
	}
}