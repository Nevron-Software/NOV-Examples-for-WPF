using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents a lane that is dark regardless of the active UI theme.
	/// </summary>
	internal abstract class NExampleDarkLane : NDocumentBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleDarkLane()
		{
			// Apply a Windows 10 theme
			Document.InheritStyleSheets = false;
			NUIPartSkinsTheme theme = new NWindows10Theme();
			Document.StyleSheets.ApplyTheme(theme);

			// Add some custom styles
			Document.StyleSheets.Add(CreateCustomStyleSheet(theme));

			// Create the lane's content
			Document.Content = new NDocumentBoxSurface(CreateContent());
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleDarkLane()
		{
			NExampleDarkLaneSchema = NSchema.Create(typeof(NExampleDarkLane), NDocumentBoxSchema);
		}

		#endregion

		#region Protected Must Override

		/// <summary>
		/// Creates the content of this lane.
		/// </summary>
		/// <returns></returns>
		protected abstract NWidget CreateContent();

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleDarkLane.
		/// </summary>
		public static readonly NSchema NExampleDarkLaneSchema;

		#endregion

		#region Static Methods - Styling

		private static NStyleSheet CreateCustomStyleSheet(NUIPartSkinsTheme theme)
		{
			NStyleSheet styleSheet = new NStyleSheet();

			NColor grayColor = theme.Colors.GrayText;
			NColor lightGrayColor = grayColor.Lighten(0.2f);
			NColor lighterGrayColor = grayColor.Lighten(0.4f);
			NColor lightestGrayColor = grayColor.Lighten(0.8f);
			NColor textColor = theme.Colors.HighlightText;

			// Dock panel
			NRule rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NDocumentBoxSurface.NDocumentBoxSurfaceSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(BackgroundFillProperty, new NColorFill(grayColor)));
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(textColor)));
			rule.Declarations.Add(new NValueDeclaration<NMargins>(PaddingProperty, new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing)));

			// Search box
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NExamplesSearchBox.NExamplesSearchBoxSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(theme.Colors.ControlText)));

			// Button - mouse over 
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NButton.NButtonSchema);
				sb.ValueEquals(NStylePropertyEx.ExtendedLookPropertyEx, ENExtendedLook.Flat);
				sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(BackgroundFillProperty, new NColorFill(lightGrayColor)));
			rule.Declarations.Add(new NValueDeclaration<NBorder>(BorderProperty, NBorder.CreateFilledBorder(lightestGrayColor)));

			// Button - pressed 
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NButton.NButtonSchema);
				sb.ValueEquals(NStylePropertyEx.ExtendedLookPropertyEx, ENExtendedLook.Flat);
				sb.ValueEquals(NButtonBase.IsPressedProperty, true);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(BackgroundFillProperty, new NColorFill(lighterGrayColor)));
			rule.Declarations.Add(new NValueDeclaration<NBorder>(BorderProperty, NBorder.CreateFilledBorder(lightestGrayColor)));

			// Link labels
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NLinkLabel.NLinkLabelSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(textColor)));

			// Example in the search box drop down
			NExamplesSearchBox.AddStyles(styleSheet, theme);

			return styleSheet;
		}

		#endregion
	}
}