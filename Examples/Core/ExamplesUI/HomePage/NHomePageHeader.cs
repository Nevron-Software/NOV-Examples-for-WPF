using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
	internal class NHomePageHeader : NHomePageRootWidget
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHomePageHeader()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHomePageHeader()
		{
			NHomePageHeaderSchema = NSchema.Create(typeof(NHomePageHeader), NHomePageRootWidgetSchema);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the examples search box.
		/// </summary>
		public NExamplesSearchBox SearchBox
		{
			get
			{
				return m_SearchBox;
			}
		}
		public NMenuDropDown FavoriteExamplesDropDown
		{
			get
			{
				return m_FavoriteExamplesDropDown;
			}
		}
		public NMenuDropDown RecentExamplesDropDown
		{
			get
			{
				return m_RecentExamplesDropDown;
			}
		}

		#endregion

		#region Protected Overrides - Styling

		protected override NUITheme CreateUiTheme()
		{
			return new NHomePageDarkTheme();
		}

		#endregion

		#region Protected Overrides - UI

		protected override NWidget CreateContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = Spacing;

			stack.Add(CreateToolbar());

			NImageBox novLogoImageBox = new NImageBox(NResources.Image_ExamplesUI_Logos_NOV_svg);
			novLogoImageBox.UserId = NovLogoImageBoxId;
			stack.Add(novLogoImageBox);

			NLabel label = new NLabel("Leading User Interface Components for Blazor, WinForms, WPF and Xamarin.Mac");
			label.UserId = DescriptionLabelId;
			stack.Add(label);

			return stack;
		}

		#endregion

		#region Implementation - UI

		private NStackPanel CreateToolbar()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.FillMode = ENStackFillMode.First;
			stack.FitMode = ENStackFitMode.First;

			NImageBox nevronImageBox = new NImageBox(NResources.Image_ExamplesUI_Logos_Nevron_svg);
			nevronImageBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			nevronImageBox.UserId = NevronLogoImageBoxId;
			stack.Add(nevronImageBox);

			m_SearchBox = CreateSearchBox();
			stack.Add(m_SearchBox);

			m_FavoriteExamplesDropDown = new NMenuDropDown(NResources.Image_ExamplesUI_Icons_Favorites_svg);
			m_FavoriteExamplesDropDown.VerticalPlacement = ENVerticalPlacement.Center;
			m_FavoriteExamplesDropDown.Tooltip = new NTooltip("Favorite Examples");
			NStylePropertyEx.SetFlatExtendedLook(m_FavoriteExamplesDropDown);
			stack.Add(m_FavoriteExamplesDropDown);

			m_RecentExamplesDropDown = new NMenuDropDown(NResources.Image_ExamplesUI_Icons_Recent_svg);
			m_RecentExamplesDropDown.VerticalPlacement = ENVerticalPlacement.Center;
			m_RecentExamplesDropDown.Tooltip = new NTooltip("Recent Examples");
			NStylePropertyEx.SetFlatExtendedLook(m_RecentExamplesDropDown);
			stack.Add(m_RecentExamplesDropDown);

			NButton mailButton = new NButton(NResources.Image_ExamplesUI_Icons_Mail_svg);
			mailButton.Click += OnMailButtonClick;
			NStylePropertyEx.SetExtendedLook(mailButton, ENExtendedLook.Flat);
			stack.Add(mailButton);

			return stack;
		}
		private NExamplesSearchBox CreateSearchBox()
		{
			NExamplesSearchBox searchBox = new NExamplesSearchBox();
			return searchBox;
		}

		#endregion

		#region Event Handlers

		private void OnMailButtonClick(NEventArgs arg)
		{
			NApplication.OpenUrl("mailto:" + NExamplesUi.NevronEmail + "?subject=Nevron%20Open%20Vision%20Question");
		}

		#endregion

		#region Fields

		internal NExamplesSearchBox m_SearchBox;
		private NMenuDropDown m_FavoriteExamplesDropDown;
		private NMenuDropDown m_RecentExamplesDropDown;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHomePageHeader.
		/// </summary>
		public static readonly NSchema NHomePageHeaderSchema;

		#endregion

		#region Constants

		private const string NevronLogoImageBoxId = "NevronLogoImageBox";
		private const string NovLogoImageBoxId = "NovLogoImageBox";
		private const string DescriptionLabelId = "DescriptionLabel";

		#endregion

		#region Nested Types - Theme

		private class NHomePageDarkTheme : NNevronDarkTheme
		{
			#region Constructors

			public NHomePageDarkTheme()
			{
				InSmallSizeContext = CreateContext(sb =>
				{
					sb.DescendantOf();
					sb.UserClass(SmallSizeClass);
				});
			}

			static NHomePageDarkTheme()
			{
				NHomePageDarkThemeSchema = NSchema.Create(typeof(NHomePageDarkTheme), NNevronDarkThemeSchema);
			}

			#endregion

			#region Protected Overrides - Styles

			protected override void CreateDocumentBoxStyles()
			{
				base.CreateDocumentBoxStyles();

				// Surface
				NThemeRule rule = GetRule(NDocumentBoxSurface.NDocumentBoxSurfaceSchema);
				Background(rule, new NImageFill(NResources.Image_ExamplesUI_Backgrounds_HomeHeaderBackground_svg));
				Padding(rule, new NMargins(Spacing, Spacing, Spacing, Spacing * 2));
			}
			protected override void CreateLabelStyles()
			{
				base.CreateLabelStyles();

				NThemingState descriptionLabelState = CreateUserIdState(DescriptionLabelId);

				// Description label
				NThemeRule rule = GetRule(NLabel.NLabelSchema, descriptionLabelState);
				HorizontalPlacementCenter(rule);
				TextFill(rule, NColor.White);
				DefaultFont(rule, ENRelativeFontSize.XXLarge);

				// Description label - in small size
				rule = GetRule(NLabel.NLabelSchema, descriptionLabelState, InSmallSizeContext);
				DefaultFont(rule, ENRelativeFontSize.XLarge);
			}
			protected override void CreateImageBoxStyles()
			{
				base.CreateImageBoxStyles();

				// Nevron logo image box - in small size
				NThemeRule rule = GetRule(NImageBox.NImageBoxSchema, CreateUserIdState(NevronLogoImageBoxId), InSmallSizeContext);
				PreferredHeight(rule, 16);
				Margins(rule, new NMargins(0, 8));

				// NOV logo image box - in small size
				rule = GetRule(NImageBox.NImageBoxSchema, CreateUserIdState(NovLogoImageBoxId), InSmallSizeContext);
				PreferredHeight(rule, 34);
			}
			protected override void CreateListBoxStyles()
			{
				base.CreateListBoxStyles();

				// List box item
				NThemeRule rule = GetRule(NListBoxItem.NListBoxItemSchema);
				DefaultFont(rule, ENRelativeFontSize.Large);

				// List box item mouse over
				rule = GetRule(NListBoxItem.NListBoxItemSchema, IsMouseOverState);
				Background(rule, Colors.ButtonSelectedHighlight);
			}
			protected override void CreateMenuStyles()
			{
				base.CreateMenuStyles();

				// Menu item
				NThemeRule rule = GetRule(NMenuItem.NMenuItemSchema);
				DefaultFont(rule, ENRelativeFontSize.Large);
			}
			protected override void CreateTextBoxStyles()
			{
				base.CreateTextBoxStyles();

				// Examples search box
				NThemeRule rule = GetRule(NExamplesSearchBox.NExamplesSearchBoxSchema);
				Background(rule, TextBoxBackgroundColor);
				Padding(rule, new NMargins(5, -1));
				Border(rule, NBorder.CreateFilledBorder(NColor.Transparent, 8, 12));

				// Text box in autocomplete box
				rule = GetRule(NTextBox.NTextBoxSchema, NormalState, InParentContext, InAutoCompleteBoxContext);
				Background(rule, TextBoxBackgroundColor);
				HighlightTextFill(rule);
				DefaultFont(rule, ENRelativeFontSize.XLarge);
				Margins(rule, NMargins.Zero);
			}

			#endregion

			#region Fields

			private readonly NThemingContext InSmallSizeContext;

			#endregion

			#region Schema

			public static readonly NSchema NHomePageDarkThemeSchema;

			#endregion

			#region Constants

			private static readonly NColor TextBoxBackgroundColor = new NColor(0xffc7c7c7);

			#endregion
		}

		#endregion
	}
}