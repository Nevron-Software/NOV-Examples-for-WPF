using System;
using System.IO;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.Text.Formats;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	internal class NHomePageContent : NHomePageRootWidget
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHomePageContent()
		{
			m_RichTextViews = new NList<NRichTextView>();
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHomePageContent()
		{
			NHomePageContentSchema = NSchema.Create(typeof(NHomePageContent), NHomePageRootWidgetSchema);
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Adds a category to the tab of this widget.
		/// </summary>
		/// <param name="categoryElement"></param>
		/// <param name="examplesCount"></param>
		public void AddCategory(NXmlElement categoryElement, int examplesCount)
		{
			bool selected = m_Tab.TabPages.Count == 0; // The first tab page should be selected
			NTabPage tabPage = new NTabPage(
				CreateTabPageHeader(categoryElement, selected),
				CreateTabPageContent(categoryElement, examplesCount)
			);
			tabPage.Tag = categoryElement;

			m_Tab.TabPages.Add(tabPage);
		}
		/// <summary>
		/// Navigates to the category with the given name.
		/// </summary>
		/// <param name="categoryName"></param>
		public void NavigateToCategory(string categoryName)
		{
			for (int i = 0; i < m_Tab.TabPages.Count; i++)
			{
				NXmlElement xmlElement = (NXmlElement)m_Tab.TabPages[i].Tag;
				string pageName = xmlElement.GetAttributeValue(NExamplesXml.Attribute.Name);
				if (pageName == categoryName)
				{
					m_Tab.SelectedIndex = i;
					break;
				}
			}
		}

		#endregion

		#region Protected Overrides - Styling

		protected override NUITheme CreateUiTheme()
		{
			return new NHomePageLightTheme();
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
				// Set the "SmallSize" to all rich text views
				for (int i = 0; i < m_RichTextViews.Count; i++)
				{
					m_RichTextViews[i].Content.UserClass = SmallSizeClass;
				}
			}
			else
			{
				// Clear the "SmallSize" user class of all rich text views
				for (int i = 0; i < m_RichTextViews.Count; i++)
				{
					m_RichTextViews[i].Content.ClearLocalValue(UserClassProperty);
				}
			}
		}

		#endregion

		#region Protected Overrides - UI

		protected override NWidget CreateContent()
		{
			m_Tab = new NTab();
			m_Tab.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Tab.VerticalPlacement = ENVerticalPlacement.Center;
			m_Tab.SelectedIndexChanged += OnTabSelectedIndexChanged;

			NPairBox pairBox = new NPairBox(m_Tab, CreatePlatformsPanel(), ENPairBoxRelation.Box1AboveBox2);
			pairBox.FillMode = ENStackFillMode.First;
			pairBox.FitMode = ENStackFitMode.First;
			pairBox.Spacing = Spacing * 2;

			return pairBox;
		}

		#endregion

		#region Implementation - UI

		/// <summary>
		/// Creates a tab page header.
		/// </summary>
		/// <param name="categoryElement"></param>
		/// <param name="selected"></param>
		/// <returns></returns>
		private NPairBox CreateTabPageHeader(NXmlElement categoryElement, bool selected)
		{
			// Get the product logo icon
			NImage productLogoIcon = GetProductLogoIcon(categoryElement, selected);
			NImageBox imageBox = new NImageBox(productLogoIcon);
			imageBox.UserClass = ProductLogoImageBoxClass;

			// Create the tab page header label
			string categoryName = categoryElement.GetAttributeValue(NExamplesXml.Attribute.Name);
			NLabel headerLabel = new NLabel(categoryName);

			// Create a pair box with the icon and the label
			return new NPairBox(imageBox, headerLabel, ENPairBoxRelation.Box1AboveBox2);
		}
        /// <summary>
        /// Creates the content of a tab page.
        /// </summary>
        /// <param name="categoryElement"></param>
		/// <param name="examplesCount"></param>
        /// <returns></returns>
        private NWidget CreateTabPageContent(NXmlElement categoryElement, int examplesCount)
		{
			NFlexBoxPanel infoFlexPanel = new NFlexBoxPanel();

			// Get XML data
			string categoryName = categoryElement.GetAttributeValue(NExamplesXml.Attribute.Name);
			string categoryNamespace = categoryElement.GetAttributeValue(NExamplesXml.Attribute.Namespace);
			NXmlElement descriptionElement = (NXmlElement)categoryElement.GetFirstDescendant(NExamplesXml.Element.Description);

			// Create the header label
			NLabel headerLabel = new NLabel($"NOV {categoryName} for .NET");
			headerLabel.UserClass = HeaderLabelClass;
			infoFlexPanel.Add(headerLabel);

			// Create the description rich text view
			NRichTextView richTextView = CreateDescriptionRichTextView(descriptionElement);
			infoFlexPanel.Add(richTextView, 1, 1);
			m_RichTextViews.Add(richTextView);

			// Create the buttons at the bottom
			NTableFlowPanel buttonsTable = new NTableFlowPanel();
			buttonsTable.VerticalPlacement = ENVerticalPlacement.Bottom;
			buttonsTable.UniformWidths = ENUniformSize.Max;
			buttonsTable.HorizontalSpacing = Spacing * 2;

			NButton learnMoreButton = new NButton("Learn More");
			learnMoreButton.UserId = LearnMoreButtonId;
			learnMoreButton.Click += OnLearnMoreButtonClick;
			buttonsTable.Add(learnMoreButton);

			NButton examplesButton = new NButton("Examples");
			examplesButton.UserId = ExamplesButtonId;
			examplesButton.Tooltip = new NTooltip($"Show {examplesCount} {categoryName} examples");
			examplesButton.Click += OnExamplesButtonClick;
			buttonsTable.Add(examplesButton);

			infoFlexPanel.Add(buttonsTable);

			// Create the collage image box
			NImage collageImage = NImage.FromResource(NResources.Instance.GetResource($"RIMG_ExamplesUI_Collages_{categoryNamespace}_png"));
			NImageBox collageImageBox = new NImageBox(collageImage);
			collageImageBox.UserClass = CollageImageBoxClass;

			if (categoryName == "Framework")
			{
				// Set white background fill to the "Framework" collage image box
				collageImageBox.BackgroundFill = new NColorFill(NColor.White);
			}

			// Add the info flex panel and the collage image box to the main flex panel
			NFlexBoxPanel mainFlexPanel = new NFlexBoxPanel();
			mainFlexPanel.Direction = ENHVDirection.LeftToRight;
			mainFlexPanel.UniformWidths = ENUniformSize.Max;

			mainFlexPanel.Add(infoFlexPanel, 1, 1);
			mainFlexPanel.Add(collageImageBox, 1, 1);

			return mainFlexPanel;
		}
		/// <summary>
		/// Creates the component description rich text view.
		/// </summary>
		/// <param name="descriptionElement"></param>
		/// <returns></returns>
		private NRichTextView CreateDescriptionRichTextView(NXmlElement descriptionElement)
		{
			NRichTextView richTextView = NExampleBase.CreateDescriptionRichTextView();
			richTextView.UserClass = DescriptionLabelClass;

			// Hide the context menu and the mini toolbar
			richTextView.ContextMenuBuilder.ShowMiniToolbar = false;
			richTextView.ContextMenuBuilder.ClearAllGroups();

			// Load the description
			using (MemoryStream memoryStream = new MemoryStream(NEncoding.UTF8.GetBytes(descriptionElement.InnerXml)))
			{
				// Load the stream in the rich text view as HTML
				richTextView.LoadFromStream(memoryStream, NTextFormat.Html);
			}

			return richTextView;
		}

		/// <summary>
		/// Creates the platforms panel.
		/// </summary>
		/// <returns></returns>
		private NTableFlowPanel CreatePlatformsPanel()
		{
			NTableFlowPanel table = new NTableFlowPanel();
			table.HorizontalPlacement = ENHorizontalPlacement.Center;

			//table.Add(new NLabel("Hello"));
			table.Add(CreatePlatformImageBox(NResources.Image_ExamplesUI_Logos_Platforms_Blazor_svg));
			table.Add(CreatePlatformImageBox(NResources.Image_ExamplesUI_Logos_Platforms_Wpf_svg));
			table.Add(CreatePlatformImageBox(NResources.Image_ExamplesUI_Logos_Platforms_WinForms_svg));
			table.Add(CreatePlatformImageBox(NResources.Image_ExamplesUI_Logos_Platforms_Xamarin_svg));

			return table;
		}
		private NImageBox CreatePlatformImageBox(NImage image)
		{
			NImageBox imageBox = new NImageBox(image);
			imageBox.UserClass = PlatformImageBoxClass;
			return imageBox;
		}

		#endregion

		#region Event Handlers

		private void OnTabSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NTab tab = (NTab)arg.CurrentTargetNode;
			int oldIndex = (int)arg.OldValue;
			int newIndex = (int)arg.NewValue;

			// Update tab page icons
			if (oldIndex != -1)
			{
				UpdatePageIcon(tab.TabPages[oldIndex], false);
			}

			UpdatePageIcon(tab.TabPages[newIndex], true);
		}
		private void OnLearnMoreButtonClick(NEventArgs arg)
		{
			// Open the link to the component's web page in the default web browser
			NXmlElement xmlElement = (NXmlElement)m_Tab.SelectedPage.Tag;
			string link = xmlElement.GetAttributeValue(NExamplesXml.Attribute.Link);
			NApplication.OpenUrl(link);
		}
		private void OnExamplesButtonClick(NEventArgs arg)
		{
			// Open the first example tile for the selected component
			NXmlElement categoryElement = (NXmlElement)m_Tab.SelectedPage.Tag;
			NXmlElement firstExample = (NXmlElement)categoryElement.GetFirstDescendant(NExamplesXml.Element.Example);
			GetFirstAncestor<NHomePage>().RaiseItemSelected(firstExample);
		}

		#endregion

		#region Fields

		private NTab m_Tab;
		private NList<NRichTextView> m_RichTextViews;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHomePageContent.
		/// </summary>
		public static readonly NSchema NHomePageContentSchema;

		#endregion

		#region Static Methods - Icons

		private void UpdatePageIcon(NTabPage tabPage, bool selected)
		{
			NXmlElement categoryElement = (NXmlElement)tabPage.Tag;
			NImageBox imageBox = tabPage.Header.GetFirstDescendant<NImageBox>();
			imageBox.Image = GetProductLogoIcon(categoryElement, selected);
		}
		private static NImage GetProductLogoIcon(NXmlElement categoryElement, bool selected)
		{
			string imageName = categoryElement.GetAttributeValue(NExamplesXml.Attribute.Namespace);
			string selectedText = selected ? "Selected" : String.Empty;
			return NImage.FromResource(NResources.Instance.GetResource($"RIMG_ExamplesUI_Icons_Products_{imageName}{selectedText}_svg"));
		}

		#endregion

		#region Constants

		private const string HeaderLabelClass = "HeaderLabel";
		private const string DescriptionLabelClass = "DescriptionLabel";

		private const string ProductLogoImageBoxClass = "ProductLogoImageBox";
		private const string CollageImageBoxClass = "CollageImageBox";
		private const string PlatformImageBoxClass = "PlatformImageBox";

		private const string LearnMoreButtonId = "LearnMoreButton";
		private const string ExamplesButtonId = "ExamplesButton";

		#endregion

		#region Nested Types - UI Theme

		private class NHomePageLightTheme : NNevronLightTheme
		{
			#region Constructors

			public NHomePageLightTheme()
			{
				// Update the text color
				Colors.ControlText = TextColor;

				InSmallSizeContext = CreateContext(sb =>
				{
					sb.DescendantOf();
					sb.UserClass(SmallSizeClass);
				});
			}

			static NHomePageLightTheme()
			{
				NHomePageLightThemeSchema = NSchema.Create(typeof(NHomePageLightTheme), NNevronLightThemeSchema);
			}

			#endregion

			#region Protected Overrides - Styles

			protected override void CreateDocumentBoxStyles()
			{
				base.CreateDocumentBoxStyles();

				CreateRichTextStyles();

				// Surface
				NThemeRule rule = GetRule(NDocumentBoxSurface.NDocumentBoxSurfaceSchema);
				Background(rule, new NImageFill(NResources.Image_ExamplesUI_Backgrounds_HomeContentBackground_svg));
				Padding(rule, new NMargins(Spacing));
			}

			protected override void CreateTabStyles()
			{
				// Contexts
				NThemingContext inTabPageHeaderContext = CreateChildOfTypeContext(NTabPageHeader.NTabPageHeaderSchema);

				#region Tab Pane and Tab Page Headers Skin

				Skins.TabPane.Padding = new NMargins(Spacing * 3);

				NColorSkinState normalState = (NColorSkinState)Skins.TabPane.GetState(NSkinState.Normal);
				normalState.InnerCornerRadius = TabInnerCornerRadius;
				normalState.OuterCornerRadius = TabOuterCornerRadius;

				// Modify tab page headers skin styles
				NSkin[] tabPageHeaderSkins = TabSkins.Top.GetSkins();
				for (int i = 0; i < tabPageHeaderSkins.Length; i++)
				{
					NColorSkin skin = (NColorSkin)tabPageHeaderSkins[i];
					skin.Padding = new NMargins(Spacing, 0, Spacing, Spacing / 2);
					skin.BorderThickness = new NMargins(0, 6, 0, 0);
					skin.RoundedCorners = ENUIThemeBorderBrickType.LeftTopCorner | ENUIThemeBorderBrickType.TopRightCorner;

					// Set default background fill to null to make the tab page headers transparent
					normalState = (NColorSkinState)skin.GetState(NSkinState.Normal);
					normalState.BackgroundFill = null;

					// Set top border fill of the selected state
					NColorSkinState selectedState = (NColorSkinState)skin.GetState(NSkinState.TabSelected);
					selectedState.BorderTopFill = new NColorFill(HighlightColor);

					// Set corner rounding to all states
					for (int j = 0; j < skin.States.Count; j++)
					{
						NColorSkinState curState = (NColorSkinState)skin.States[j];
						curState.InnerCornerRadius = TabInnerCornerRadius;
						curState.OuterCornerRadius = TabOuterCornerRadius;
					}
				}

				#endregion

				base.CreateTabStyles();

				#region Tab

				NThemeRule rule = GetRule(NTab.NTabSchema);
				Margins(rule, TabMargins);
				rule.Set(NTab.HeadersAlignmentProperty, ENRelativeAlignment.Center);

				#endregion

				#region Tab Page Header

				// Tab page header - selected
				rule = GetRule(NTabPageHeader.NTabPageHeaderSchema, InTabPageContext, IsSelectedState);
				TextFill(rule, HighlightColor);

				// Tab page header label
				rule = GetRule(NLabel.NLabelSchema, InParentContext, inTabPageHeaderContext);
				HorizontalPlacementCenter(rule);
				DefaultFont(rule, ENRelativeFontSize.Large);

				#endregion
			}
			protected override void CreateLabelStyles()
			{
				base.CreateLabelStyles();

				NThemingState headerLabelState = CreateUserClassState(HeaderLabelClass);

				// Content header label
				NThemeRule rule = GetRule(NLabel.NLabelSchema, headerLabelState);
				DefaultFont(rule, ENRelativeFontSize.XXXLarge, ENFontStyle.Bold);
				Padding(rule, new NMargins(0, 0, 0, Spacing * 2));

				// Content header label - in small size
				rule = GetRule(NLabel.NLabelSchema, headerLabelState, InSmallSizeContext);
				DefaultFont(rule, ENRelativeFontSize.XXLarge, ENFontStyle.Bold);
				Padding(rule, new NMargins(0, 0, 0, Spacing));
			}
			protected override void CreateButtonStyles(NSchema buttonSchema)
			{
				base.CreateButtonStyles(buttonSchema);

				if (buttonSchema == NButtonBase.NButtonBaseSchema)
				{
					// "Learn More" button
					NColorSkinState buttonMouseOverState = (NColorSkinState)Skins.Button.GetState(NSkinState.MouseOver);
					NThemeRule rule = StyleButton(LearnMoreButtonId, null, (NFill)buttonMouseOverState.BackgroundFill.DeepClone());
					Border(rule, NBorder.CreateFilledBorder(TextColor, 4, 6));

					// "Examples" button
					rule = StyleButton(ExamplesButtonId, new NColorFill(HighlightColor), new NColorFill(HighlightColor.Darken()));
					Border(rule, NBorder.CreateFilledBorder(HighlightColor, 4, 6));
					TextFill(rule, NColor.White);
				}
			}
			protected override void CreateImageBoxStyles()
			{
				base.CreateImageBoxStyles();

				// Collage image box
				NThemingState collageImageBoxState = CreateUserClassState(CollageImageBoxClass);
				NThemeRule rule = GetRule(NImageBox.NImageBoxSchema, collageImageBoxState);
				Border(rule, NBorder.CreateFilledBorder(HighlightColor, 6, 9));
				BorderThickness(rule, new NMargins(16));
				Background(rule, new NRadialGradientFill(0.5d, 0.5d, new NColor(254, 254, 254), new NColor(227, 227, 227)));

				// Collage image box - in small size
				rule = GetRule(NImageBox.NImageBoxSchema, collageImageBoxState, InSmallSizeContext);
				BorderThickness(rule, new NMargins(8));

				// Product logo image box - in small size
				rule = GetRule(NImageBox.NImageBoxSchema, CreateUserClassState(ProductLogoImageBoxClass), InSmallSizeContext);
				PreferredSize(rule, 24, 24);

				// Platform image box - in small size
				rule = GetRule(NImageBox.NImageBoxSchema, CreateUserClassState(PlatformImageBoxClass), InSmallSizeContext);
				PreferredHeight(rule, 24);
			}

			#endregion

			#region Implementation - Styles

			private void CreateRichTextStyles()
			{
				NThemeRule rule = GetRule(NParagraph.NParagraphSchema, InSmallSizeContext);
				rule.Set(NParagraph.FontSizeProperty, 10.0, true);
			}

			private NThemeRule StyleButton(string buttonId, NFill backgroundFill, NFill mouseOverFill)
			{
				NThemingState state = CreateUserIdState(buttonId);

				// Button label
				NThemeRule labelRule = GetRule(NLabel.NLabelSchema, InParentContext, state);
				HorizontalPlacementCenter(labelRule);

				// Button
				NThemeRule buttonRule = GetRule(NButton.NButtonSchema, state);
				DefaultFont(buttonRule, ENRelativeFontSize.XXLarge, ENFontStyle.Bold);
				Background(buttonRule, backgroundFill);
				Padding(buttonRule, new NMargins(Spacing * 4, Spacing));
				ThickBorderThickness(buttonRule);

				// Button Mouse Over
				NThemeRule buttonMouseOverRule = GetRule(NButton.NButtonSchema, state, IsMouseOverState);
				Background(buttonMouseOverRule, mouseOverFill);

				// Button - in small size
				NThemeRule smallButtonRule = GetRule(NButton.NButtonSchema, state, InSmallSizeContext);
				DefaultFont(smallButtonRule, ENRelativeFontSize.Large, ENFontStyle.Bold);
				Padding(smallButtonRule, new NMargins(Spacing * 2, Spacing / 2));
				ThinBorderThickness(smallButtonRule);

				return buttonRule;
			}

			#endregion

			#region Fields

			private readonly NThemingContext InSmallSizeContext;

			#endregion

			#region Schema

			public static readonly NSchema NHomePageLightThemeSchema;

			#endregion

			#region Constants

			private static readonly NColor TextColor = new NColor(0xff282826);
			private static readonly NColor GrayTextColor = new NColor(0xff858585);
			private static readonly NColor HighlightColor = new NColor(0xff612d87);

			private static readonly NMargins TabMargins = new NMargins(100, 0);
			private const double TabOuterCornerRadius = 8;
			private const double TabInnerCornerRadius = 0;

			#endregion
		}

		#endregion
	}
}