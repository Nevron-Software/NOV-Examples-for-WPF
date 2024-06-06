using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// The second lane of the Example header.
	/// </summary>
	internal class NExampleHeaderLane2 : NExampleDarkLane
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleHeaderLane2()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleHeaderLane2()
		{
			NExampleHeaderLane2Schema = NSchema.Create(typeof(NExampleHeaderLane2), NExampleDarkLaneSchema);
		}

		#endregion

		#region Events

		public event Function<NEventArgs> SearchBoxItemSelected
		{
			add
			{
				m_SearchBox.ListBoxItemSelected += value;
			}
			remove
			{
				m_SearchBox.ListBoxItemSelected -= value;
			}
		}
		public event Function<NEventArgs> BreadcrumbButtonClick
		{
			add
			{
				m_Breadcrumb.ButtonClick += value;
			}
			remove
			{
				m_Breadcrumb.ButtonClick -= value;
			}
		}
		public event Function<NEventArgs> PreviousExampleButtonClick
		{
			add
			{
				m_PreviousExampleButton.Click += value;
			}
			remove
			{
				m_PreviousExampleButton.Click -= value;
			}
		}
		public event Function<NEventArgs> NextExampleButtonClick
		{
			add
			{
				m_NextExampleButton.Click += value;
			}
			remove
			{
				m_NextExampleButton.Click -= value;
			}
		}
		public event Function<bool> FavoriteAddedOrRemoved;

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
		/// <summary>
		/// Gets the example breadcrumb.
		/// </summary>
		public NExampleBreadcrumb Breadcrumb
		{
			get
			{
				return m_Breadcrumb;
			}
		}
		/// <summary>
		/// Gets/Sets the example's header title.
		/// </summary>
		public string Title
		{
			get
			{
				return m_TitleLabel.Text;
			}
			set
			{
				m_TitleLabel.Text = value;
			}
		}

		#endregion

		#region Public Methods

		public void Update(NXmlElement xmlElement)
		{
            string name = xmlElement.GetAttributeValue("name");
            string examplePath = NExamplesUi.GetExamplePath(xmlElement);

            Title = NExamplesUi.ProcessHeaderText(name);
            UpdateFavoriteButton(NExamplesOptions.Instance.FavoriteExamples.Contains(examplePath));

			m_ExampleElement = xmlElement;
        }

		#endregion

		#region Protected Overrides - UI

		/// <summary>
		/// Creates the content of this lane.
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateContent()
		{
			NDockPanel dock = new NDockPanel();

			// Create the search box
			{
				m_SearchBox = new NExamplesSearchBox();
				m_SearchBox.Margins = new NMargins(0, 0, NDesign.HorizontalSpacing * 2, 0);
				dock.Add(m_SearchBox, ENDockArea.Left);
			}

			// Create the title label and the Favorite and Copy Link buttons
			{
				m_TitleLabel = new NLabel();
				NStylePropertyEx.SetRelativeFontSize(m_TitleLabel, ENRelativeFontSize.XLarge);
				NStylePropertyEx.SetFontStyleBold(m_TitleLabel, true);

				m_FavoriteButton = new NButton(NResources.Image_ExamplesUI_Icons_FavoritesEmpty_png);
				NStylePropertyEx.SetExtendedLook(m_FavoriteButton, ENExtendedLook.Flat);
				UpdateFavoriteButton(false);
				m_FavoriteButton.Click += OnFavoriteButtonClick;

				NButton copyLinkButton = new NButton(NResources.Image_ExamplesUI_Icons_Link_png);
                NStylePropertyEx.SetExtendedLook(copyLinkButton, ENExtendedLook.Flat);
				copyLinkButton.Tooltip = new NTooltip("Copy link to clipboard");
                copyLinkButton.Click += OnCopyLinkButtonClick;

				NStackPanel stack = new NStackPanel();
				stack.Direction = ENHVDirection.LeftToRight;
				stack.Add(m_TitleLabel);
				stack.Add(m_FavoriteButton);
				stack.Add(copyLinkButton);

				dock.Add(stack, ENDockArea.Center);
			}

			// Create the prev/next example buttons
			{
				NSize symbolSize = new NSize(12, 12);
				NColor symbolColor = NColor.White;

				m_NextExampleButton = new NButton(NSymbol.Create(ENSymbolShape.TriangleRight, symbolSize, symbolColor));
				NStylePropertyEx.SetExtendedLook(m_NextExampleButton, ENExtendedLook.Flat);
				m_NextExampleButton.Tooltip = new NTooltip("Next Example");
				dock.Add(m_NextExampleButton, ENDockArea.Right);

				m_PreviousExampleButton = new NButton(NSymbol.Create(ENSymbolShape.TriangleLeft, symbolSize, symbolColor));
				NStylePropertyEx.SetExtendedLook(m_PreviousExampleButton, ENExtendedLook.Flat);
				m_PreviousExampleButton.Tooltip = new NTooltip("Previous Example");
				dock.Add(m_PreviousExampleButton, ENDockArea.Right);

				dock.Add(new NCommandBarSeparator(), ENDockArea.Right);
			}

			// Create the breadcrumb
			{
				m_Breadcrumb = new NExampleBreadcrumb();
				dock.Add(m_Breadcrumb, ENDockArea.Right);
			}

			return dock;
		}

        #endregion

        #region Implementation - UI

        private void UpdateFavoriteButton(bool addedToFavorites)
		{
			NImageBox imageBox = (NImageBox)m_FavoriteButton.Content;
			if (addedToFavorites)
			{
				// The example is added to Favorites
				imageBox.Image = NResources.Image_ExamplesUI_Icons_Favorites_png;
				m_FavoriteButton.Tooltip = new NTooltip("Remove from Favorites");
			}
			else
			{
				// The example is not added to Favorites
				imageBox.Image = NResources.Image_ExamplesUI_Icons_FavoritesEmpty_png;
				m_FavoriteButton.Tooltip = new NTooltip("Add to Favorites");
			}
		}

        #endregion

        #region Event Handlers

        private void OnFavoriteButtonClick(NEventArgs arg)
		{
			NButton favoriteButton = (NButton)arg.TargetNode;
			
			// Toggle the image of the favorite button
			NImageBox imageBox = (NImageBox)favoriteButton.Content;
			bool addedToFavorites = imageBox.Image.ImageSource.Equals(NResources.Image_ExamplesUI_Icons_Favorites_png.ImageSource);

			addedToFavorites = !addedToFavorites;
			UpdateFavoriteButton(addedToFavorites);

			if (FavoriteAddedOrRemoved != null)
			{
				FavoriteAddedOrRemoved(addedToFavorites);
			}
		}
        private void OnCopyLinkButtonClick(NEventArgs arg)
        {
            NExamplesContent examplesContent = GetFirstAncestor<NExamplesContent>();
            NExamplesUi.CopyExampleLinkToClipboard(examplesContent.LinkProcessor, m_ExampleElement);
        }

        #endregion

        #region Fields

        private NExamplesSearchBox m_SearchBox;
		private NLabel m_TitleLabel;
		private NButton m_FavoriteButton;
		private NExampleBreadcrumb m_Breadcrumb;
		private NButton m_PreviousExampleButton;
		private NButton m_NextExampleButton;

		private NXmlElement m_ExampleElement;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleHeaderLane2.
		/// </summary>
		public static readonly NSchema NExampleHeaderLane2Schema;

		#endregion
	}
}