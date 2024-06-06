using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Base class for NOV examples home page panels like the home page header and the home page content.
	/// </summary>
	internal abstract class NHomePageRootWidget : NDocumentBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHomePageRootWidget()
		{
			// Create and apply an UI theme
			Document.InheritStyleSheets = false;
			Document.StyleSheets.ApplyTheme(CreateUiTheme());

			// Create the UI
			Surface = new NDocumentBoxSurface(CreateContent());
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHomePageRootWidget()
		{
			NHomePageRootWidgetSchema = NSchema.Create(typeof(NHomePageRootWidget), NDocumentBoxSchema);
		}

		#endregion

		#region Protected Overrides - Arrange

		/// <summary>
		/// Arranges the content. Overriden to set the "SmallSize" user class to the surface widget and the rich text view
		/// when the height of the display window is less than a given treshold to make the home page design responsive.
		/// </summary>
		/// <param name="ca"></param>
		protected override void ArrangeContent(NRectangle ca)
		{
			base.ArrangeContent(ca);

			if (DisplayWindow != null && DisplayWindow.Height < SmallSizeHeight)
			{
				// Set the "SmallSize" user class to the surface
				Surface.UserClass = SmallSizeClass;
			}
			else
			{
				// Clear the "SmallSize" user class of the surface
				Surface.ClearLocalValue(UserClassProperty);
			}
		}

		#endregion

		#region Protected Must Override - Styling

		protected abstract NUITheme CreateUiTheme();

		#endregion

		#region Protected Must Override - UI

		protected abstract NWidget CreateContent();

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHomePageRootWidget.
		/// </summary>
		public static readonly NSchema NHomePageRootWidgetSchema;

		#endregion

		#region Constants

		protected const string SmallSizeClass = "SmallSize";
		protected const double Spacing = 12;

		/// <summary>
		/// The height below which some elements of the examples UI (like the tab header product logos)
		/// will start getting smaller or collapsing.
		/// </summary>
		protected const double SmallSizeHeight = 750;

		#endregion
	}
}