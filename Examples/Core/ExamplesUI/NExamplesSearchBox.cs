using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the examples' search box, that searches through all examples.
	/// </summary>
	internal class NExamplesSearchBox : NAutoCompleteBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExamplesSearchBox()
		{
			HorizontalPlacement = ENHorizontalPlacement.Left;
			VerticalPlacement = ENVerticalPlacement.Center;
			StringMatchMode = ENStringMatchMode.WordStartsWith;
			PreferredWidth = 200;
			Image = NResources.Image_ExamplesUI_Icons_Search_png;
			Padding = new NMargins(2, 5, 2, 5);
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExamplesSearchBox()
		{
			NExamplesSearchBoxSchema = NSchema.Create(typeof(NExamplesSearchBox), NAutoCompleteBoxSchema);
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExamplesSearchBox.
		/// </summary>
		public static readonly NSchema NExamplesSearchBoxSchema;

		#endregion

		#region Static Methods

		internal static void AddStyles(NStyleSheet styleSheet, NUITheme theme)
		{
			NRule rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NListBoxItem.NListBoxItemSchema);
				sb.ValueEquals(NMouse.IsOverPropertyEx, true);
				sb.ChildOf();
				sb.ChildOf();
				sb.Type(NListBox.NListBoxSchema);
				sb.ChildOf();
				sb.Type(NPopupWindow.NPopupWindowSchema);
			});

			rule.Declarations.Add(new NValueDeclaration<NFill>(BackgroundFillProperty, new NColorFill(theme.Colors.ButtonSelectedHighlight)));
		}

		#endregion
	}
}