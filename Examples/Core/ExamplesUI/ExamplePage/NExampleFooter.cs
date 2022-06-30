using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the Example page footer.
	/// </summary>
	internal class NExampleFooter : NExampleDarkLane
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleFooter()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleFooter()
		{
			NExampleFooterSchema = NSchema.Create(typeof(NExampleFooter), NExampleDarkLaneSchema);
		}

		#endregion

		#region Protected Overrides - UI

		/// <summary>
		/// Creates the content of this lane.
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalPlacement = ENHorizontalPlacement.Center;
			NStylePropertyEx.SetRelativeFontSize(stack, ENRelativeFontSize.Large);

			NLinkLabel contactUsLabel = new NLinkLabel("Contact Us", String.Format(NevronMailToLink, "Nevron Open Vision Question"));
			contactUsLabel.MouseDown += OnContactUsLabelMouseDown;
			stack.Add(contactUsLabel);

			stack.Add(new NCommandBarSeparator());

			NLinkLabel purchaseLabel = new NLinkLabel("Purchase", "https://www.nevron.com/orders-purchase-nov.aspx");
			stack.Add(purchaseLabel);

			return stack;
		}

		private void OnContactUsLabelMouseDown(NMouseButtonEventArgs arg)
		{
			if (arg.Button == ENMouseButtons.Left)
			{
				// Update the mail to subject to point to the current example
				NExamplePage ownerPage = GetFirstAncestor<NExamplePage>();
				NLinkLabel contactUsLabel = (NLinkLabel)arg.CurrentTargetNode;
				contactUsLabel.LinkUrl = String.Format(NevronMailToLink, "Nevron Open Vision: " + ownerPage.CurrentExamplePath);
			}
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleFooter.
		/// </summary>
		public static readonly NSchema NExampleFooterSchema;

		#endregion

		#region Constants

		private const string NevronMailToLink = "mailto:" + NExamplesUiHelpers.NevronEmail + "?subject={0}";

		#endregion
	}
}