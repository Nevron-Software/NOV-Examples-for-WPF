using System;
using System.Globalization;
using System.IO;
using System.Text;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the home page of the examples.
	/// </summary>
	public class NHomePage : NDocumentBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHomePage()
		{
			// Apply a Windows 10 theme
			Document.InheritStyleSheets = false;
			NUITheme theme = new NWindows10Theme();
			Document.StyleSheets.ApplyTheme(theme);
			m_ExamplesMap = null;

			// Add some custom styles
			Document.StyleSheets.Add(CreateCustomStyleSheet(theme));

			// Load the metafile images
			Stream emfStream = NResources.Instance.GetResourceStream("RBIN_HomePageEmfs_zip");
			m_EmfDecompressor = new NEmfDecompressor();
			NCompression.DecompressZip(emfStream, m_EmfDecompressor);

			// Create the main dock panel
			m_MainPanel = new NDockPanel();
			m_MainPanel.HorizontalSpacing = GroupHorizontalSpacing;
			m_MainPanel.VerticalSpacing = GroupVerticalSpacing;
			m_MainPanel.Padding = new NMargins(GroupHorizontalSpacing, GroupVerticalSpacing);
			m_MainPanel.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_MainPanel.VerticalPlacement = ENVerticalPlacement.Center;

			// Create the contacts and the header labels
			m_MainPanel.Add(CreateSearchAndContacts(), ENDockArea.Top);
			m_HeaderLabel = CreateHeader();
			m_MainPanel.Add(m_HeaderLabel, ENDockArea.Top);

			// Create the page panel
			m_PagePanel = new NSingleVisiblePanel();
			m_PagePanel.Add(CreateWelcomePanel());
			m_MainPanel.Add(m_PagePanel, ENDockArea.Center);

			// Place the main panel in a scroll content
			NScrollContent scrollContent = new NScrollContent(m_MainPanel);
			scrollContent.WindowBackgroundFill = new NColorFill(BackgroundColor);
			scrollContent.NoScrollHAlign = ENNoScrollHAlign.Center;
			scrollContent.NoScrollVAlign = ENNoScrollVAlign.Center;
			scrollContent.HorizontalPlacement = ENHorizontalPlacement.Fit;
			scrollContent.VerticalPlacement = ENVerticalPlacement.Fit;
			scrollContent.Border = null;
			scrollContent.BorderThickness = NMargins.Zero;

			// Place the scroll content in a document box surface to prevent theme style inheritance
			NDocumentBoxSurface surface = new NDocumentBoxSurface(scrollContent);
			surface.TextFill = new NColorFill(TextColor);
			Document.Content = surface;

			// Subscribe to the ExampleOptions Changed event
			NExamplesOptions.Instance.Changed += OnExampleOptionsChanged;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHomePage()
		{
			NHomePageSchema = NSchema.Create(typeof(NHomePage), NDocumentBox.NDocumentBoxSchema);
		}

		#endregion

		#region Events

		public event Function<NXmlElement> TileSelected;

		#endregion

		#region Public Methods

		public void InitializeFromXml(NXmlDocument xmlDocument)
		{
			// Process the XML document
			if (xmlDocument == null || xmlDocument.ChildrenCount != 1)
				return;

			m_ExamplesMap = new NStringMap<NWidget>(false);

			// Get the root element (i.e. the <document> element)
			NXmlElement rootElement = (NXmlElement)xmlDocument.GetChildAt(0);

			// Process the head
			NXmlElement titleElement = (NXmlElement)rootElement.GetFirstChild("title");
			m_HeaderLabel.Text = ((NXmlTextNode)titleElement.GetChildAt(0)).Text;

			NXmlElement statusColorsElement = (NXmlElement)rootElement.GetFirstChild("statusColors");
			ParseStatusColors(statusColorsElement);

			// Process the categories
			NXmlElement categoriesElement = (NXmlElement)rootElement.GetFirstChild("categories");
			((NWelcomePanel)m_PagePanel[0]).Initialize(categoriesElement);

			for (int i = 0, count = categoriesElement.ChildrenCount; i < count; i++)
			{
				NXmlElement child = categoriesElement.GetChildAt(i) as NXmlElement;
				if (child == null)
					continue;

				if (child.Name != "category")
					throw new Exception("The body element can contain only category elements");

				// Create a widget and add it to the categories panel
				NWidget category = CreateCategory(child);
				m_PagePanel.Add(category);
			}

			// Init the search box
			m_SearchBox.InitAutoComplete(m_ExamplesMap, new NExampleTileFactory());
			m_SearchBox.ListBoxItemSelected += OnSearchBoxListBoxItemSelected;
		}
		public void NavigateToWelcomeScreen()
		{
			m_PagePanel.VisibleIndex = 0;
		}
		public void NavigateToCategory(string categoryName)
		{
			for (int i = 0; i < m_PagePanel.Count; i++)
			{
				if (categoryName.Equals(m_PagePanel[i].Tag))
				{
					m_PagePanel.VisibleIndex = i;
					return;
				}
			}

			// No category selected, so make the Welcome Screen visible
			NavigateToWelcomeScreen();
		}

		#endregion

		#region Protected Overrides - Mouse Up

		protected override void OnMouseUp(NMouseButtonEventArgs args)
		{
			base.OnMouseUp(args);

			if (args.Cancel || args.Button != ENMouseButtons.Left)
				return;

			if (args.TargetNode is NImageBox && args.TargetNode.Tag is string)
			{
				OnExampleCategoryMouseUp(args);
				return;
			}

			// Get the clicked example tile
			NExampleTile tile = args.TargetNode as NExampleTile;
			if (tile == null)
			{
				tile = (NExampleTile)args.TargetNode.GetFirstAncestor(NExampleTile.NExampleTileSchema);
			}

			if (tile == null)
				return;

			NExampleTileInfo itemInfo = (NExampleTileInfo)tile.Tag;
			if (TileSelected != null)
			{
				TileSelected(itemInfo.XmlElement);
			}

			// Mark the event as handled
			args.Cancel = true;
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Gets the background color for the given status.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		internal NColor GetStatusColor(string status)
		{
			NColor color;
			if (m_StatusColorMap.TryGet(status, out color))
				return color;

			return m_StatusColorMap[String.Empty];
		}

		#endregion

		#region Implementation - Header and Footer

		private NLabel CreateHeader()
		{
			NLabel label = new NLabel();
			label.TextAlignment = ENContentAlignment.MiddleCenter;
			label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 32);
			label.TextFill = new NColorFill(HeaderColor);

			return label;
		}
		private NStackPanel CreateSearchAndContacts()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			m_SearchBox = new NExamplesSearchBox();
			m_SearchBox.VerticalPlacement = ENVerticalPlacement.Center;
			stack.Add(m_SearchBox);

			m_FavoriteExamplesDropDown = new NMenuDropDown(NResources.Image_ExamplesUI_Icons_Favorites_png);
			m_FavoriteExamplesDropDown.VerticalPlacement = ENVerticalPlacement.Center;
			m_FavoriteExamplesDropDown.Tooltip = new NTooltip("Favorite Examples");
			NStylePropertyEx.SetFlatExtendedLook(m_FavoriteExamplesDropDown);
			stack.Add(m_FavoriteExamplesDropDown);

			m_RecentExamplesDropDown = new NMenuDropDown(Presentation.NResources.Image_File_RecentDocuments_png);
			m_RecentExamplesDropDown.VerticalPlacement = ENVerticalPlacement.Center;
			m_RecentExamplesDropDown.Tooltip = new NTooltip("Recent Examples");
			NStylePropertyEx.SetFlatExtendedLook(m_RecentExamplesDropDown);
			stack.Add(m_RecentExamplesDropDown);

			NWidget contactsPanel = CreateContactsPanel();
			contactsPanel.HorizontalPlacement = ENHorizontalPlacement.Right;
			stack.Add(contactsPanel);

			return stack;
		}
		private NPairBox CreateContactsPanel()
		{
			byte[] imageBytes = m_EmfDecompressor.GetMetaImage("MailIcon.emf");
			NImage emailIcon = NImage.FromBytes(imageBytes);

			NLabel emailLabel = new NLabel(NExamplesUiHelpers.NevronEmail);
			emailLabel.UserClass = "ContactLabel";
			emailLabel.MouseUp += OnEmailLabelMouseUp;

			return new NPairBox(emailIcon, emailLabel);
		}

		#endregion

		#region Implementation - Categories, Groups, Tiles and Labels

		/// <summary>
		/// Creates a widget for the given category element.
		/// Category elements can contain only row elements.
		/// </summary>
		/// <param name="categoryXmlElement"></param>
		/// <returns></returns>
		private NWidget CreateCategory(NXmlElement categoryXmlElement)
		{
			string categoryName = categoryXmlElement.GetAttributeValue("name");
			NColor color = NColor.ParseHex(categoryXmlElement.GetAttributeValue("color"));
			NColor lightColor = color.Lighten(0.6f);

			// Create the header label
			NExampleCategoryHeader categoryHeader = new NExampleCategoryHeader(categoryName);
			categoryHeader.BackgroundFill = new NColorFill(color);
			categoryHeader.Status = categoryXmlElement.GetAttributeValue("status");

			// Create a stack panel for the category widgets
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = ItemVerticalSpacing;
			stack.BackgroundFill = new NColorFill(lightColor);
			stack.Tag = categoryName;

			// Add the header label
			stack.Add(categoryHeader);

			// Loop through the rows
			for (int i = 0, count = categoryXmlElement.ChildrenCount; i < count; i++)
			{
				NXmlElement row = categoryXmlElement.GetChildAt(i) as NXmlElement;
				if (row == null)
					continue;

				if (row.Name != "row")
					throw new Exception("Category elements should contain only rows");

				NStackPanel rowStack = CreateRow(row, color);
				stack.Add(rowStack);
			}

			return stack;
		}
		/// <summary>
		/// Creates a group box for the given group element or a directly a table panel if the
		/// given group element does not have a name. Group elements can contain only tile
		/// elements, label elements and other group elements.
		/// </summary>
		/// <param name="group"></param>
		/// <param name="borderColor"></param>
		/// <returns></returns>
		private NWidget CreateGroup(NXmlElement group, NColor borderColor)
		{
			// Create a table panel
			NTableFlowPanel tablePanel = CreateTablePanel(group, borderColor);

			// Get the group title
			string groupTitle = group.GetAttributeValue("name");
			if (String.IsNullOrEmpty(groupTitle))
				return tablePanel;

			// Create a group box
			NLabel headerLabel = new NLabel(groupTitle);
			NGroupBox groupBox = new NGroupBox(headerLabel);
			groupBox.Header.HorizontalPlacement = ENHorizontalPlacement.Center;
			groupBox.Padding = new NMargins(ItemVerticalSpacing);
			groupBox.Border = NBorder.CreateFilledBorder(borderColor);

			// Create the table panel with the examples tiles
			groupBox.Content = CreateTablePanel(group, borderColor);

			return groupBox;
		}
		/// <summary>
		/// Creates a label. Label elements can contain only text.
		/// </summary>
		/// <param name="labelElement"></param>
		/// <returns></returns>
		private NLabel CreateLabel(NXmlNode labelElement)
		{
			// Process the label element's children to determine its text
			StringBuilder sb = new StringBuilder();
			for (int i = 0, count = labelElement.ChildrenCount; i < count; i++)
			{
				NXmlNode child = labelElement.GetChildAt(i);
				if (child.NodeType == ENXmlNodeType.Text)
				{
					sb.Append(((NXmlTextNode)child).Text);
				}
				else if (child.NodeType == ENXmlNodeType.Element && child.Name == "br")
				{
					sb.AppendLine();
				}
			}

			// Create a label
			NLabel label = new NLabel(sb.ToString());
			label.TextAlignment = ENContentAlignment.MiddleCenter;
			label.TextWrapMode = ENTextWrapMode.WordWrap;
			label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, LabelFontSize);

			return label;
		}
		/// <summary>
		/// Creates a tile. Tile elements can contain only examples.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="categoryNamespace"></param>
		/// <returns></returns>
		private NWidget CreateTile(NXmlElement element, string categoryNamespace)
		{
			string tileTitle = element.GetAttributeValue("name");
			string iconName = element.GetAttributeValue("icon");

			// Get the icon for the tile
			NImage icon = null;
			if (iconName != null)
			{
				if (NPath.Current.DirectorySeparatorChar != '\\')
				{
					iconName = iconName.Replace('\\', NPath.Current.DirectorySeparatorChar);
				}

				string imageFolder = NPath.Current.GetParentFolderPath(iconName);
				if (String.IsNullOrEmpty(imageFolder))
				{
					// The icon is in the folder for the current category
					imageFolder = categoryNamespace;
				}
				else
				{
					// The icon is in a folder of another category
					imageFolder = NPath.Current.Normalize(NPath.Current.Combine(categoryNamespace, imageFolder));
					if (imageFolder[imageFolder.Length - 1] == NPath.Current.DirectorySeparatorChar)
					{
						imageFolder = imageFolder.Remove(imageFolder.Length - 1);
					}

					// Update the icon name
					iconName = NPath.Current.GetFileName(iconName);
				}

				iconName = "RIMG_ExampleIcons_" + imageFolder + "_" + iconName.Replace('.', '_');
				icon = new NImage(new NEmbeddedResourceRef(NResources.Instance, iconName));
			}

			// Create and configure the tile
			NExampleTile tile = new NExampleTile(icon, tileTitle);
			tile.HorizontalPlacement = ENHorizontalPlacement.Left;
			tile.Status = element.GetAttributeValue("status");
			tile.Tag = new NExampleTileInfo(element);

			// Add the examples of the current tile to the examples map
			INIterator<NXmlNode> iter = element.GetChildNodesIterator();
			while (iter.MoveNext())
			{
				NXmlElement exampleElement = iter.Current as NXmlElement;
				if (exampleElement == null)
					continue;

				string examplePath = NExamplesUiHelpers.GetExamplePath(exampleElement);
				if (icon != null)
				{
					icon = new NImage(icon.ImageSource);
				}

				NExampleTile example = new NExampleTile(icon, examplePath);
				example.Status = exampleElement.GetAttributeValue("status");
				example.Tag = new NExampleTileInfo(exampleElement);

				if (m_ExamplesMap.Contains(examplePath) == false)
				{
					m_ExamplesMap.Add(examplePath, example);
				}
			}

			return tile;
		}

		#endregion

		#region Implementation - Layout Panels

		/// <summary>
		/// Creates the welcome panel.
		/// </summary>
		/// <returns></returns>
		private NWidget CreateWelcomePanel()
		{
			return new NWelcomePanel();
		}
		/// <summary>
		/// Creates a stack panel for the given row element. Row elements can contain only
		/// group elements and label elements.
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private NStackPanel CreateRow(NXmlElement row, NColor categoryColor)
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.FillMode = ENStackFillMode.Equal; // FIX: make this a setting
			stack.HorizontalSpacing = GroupHorizontalSpacing;
			stack.Padding = new NMargins(GroupHorizontalSpacing, 0);

			for (int i = 0, count = row.ChildrenCount; i < count; i++)
			{
				NXmlElement child = row.GetChildAt(i) as NXmlElement;
				if (child == null)
					continue;

				switch (child.Name)
				{
					case "group":
						stack.Add(CreateGroup(child, categoryColor));
						break;
					case "label":
						stack.Add(CreateLabel(child));
						break;
					default:
						throw new Exception("Unsuppoted row child element");
				}
			}

			return stack;
		}
		/// <summary>
		/// Creates a table panel, which is the content of a group.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="borderColor"></param>
		/// <returns></returns>
		private NTableFlowPanel CreateTablePanel(NXmlElement owner, NColor borderColor)
		{
			int maxRows;
			if (TryGetInt(owner, "maxRows", out maxRows) == false)
			{
				maxRows = defaultMaxRows;
			}

			// Create a table flow panel for the items
			NTableFlowPanel table = new NTableFlowPanel();
			table.UniformWidths = ENUniformSize.Max;
			table.UniformHeights = ENUniformSize.Max;
			table.ColFillMode = ENStackFillMode.Equal;
			table.Direction = ENHVDirection.TopToBottom;
			table.HorizontalSpacing = ItemHorizontalSpacing;
			table.VerticalSpacing = ItemVerticalSpacing;
			table.MaxOrdinal = maxRows;

			// Create the items and add them to the table panel
			string categoryNamespace = GetNamespace(owner);

			int childCount = owner.ChildrenCount;
			for (int i = 0; i < childCount; i++)
			{
				NXmlElement child = owner.GetChildAt(i) as NXmlElement;
				if (child == null)
				{
					continue;
				}

				switch (child.Name)
				{
					case "tile":
						table.Add(CreateTile(child, categoryNamespace));
						break;
					case "group":
						table.Add(CreateGroup(child, borderColor));
						break;
					case "label":
						table.Add(CreateLabel(child));
						break;
					default:
						throw new Exception("New examples XML tag?");
				}
			}

			return table;
		}

		#endregion

		#region Implementation - Styling

		/// <summary>
		/// Parses the background colors of the status labels defined in the examples XML file.
		/// </summary>
		/// <param name="styleElement"></param>
		private void ParseStatusColors(NXmlElement styleElement)
		{
			m_StatusColorMap = new NMap<string, NColor>();
			if (styleElement == null)
				return;

			for (int i = 0, count = styleElement.ChildrenCount; i < count; i++)
			{
				NXmlElement child = styleElement.GetChildAt(i) as NXmlElement;
				if (child == null || child.Name != "status")
					continue;

				// Get the status name
				string name = child.GetAttributeValue("name");
				if (name == null)
					continue;

				// Parse the status color
				string colorStr = child.GetAttributeValue("color");
				NColor color;
				if (NColor.TryParse(colorStr, out color) == false)
					continue;

				// Add the name/color pair to the status color map
				m_StatusColorMap.Set(name, color);
			}
		}

		private static NStyleSheet CreateCustomStyleSheet(NUITheme theme)
		{
			NStyleSheet styleSheet = new NStyleSheet();

			#region Category headers

			NRule rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NExampleCategoryHeader.NExampleCategoryHeaderSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NFont>(FontProperty, new NFont(NFontDescriptor.DefaultSansFamilyName,
				CategoryHeaderFontSize, ENFontStyle.Bold)));
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(NColor.White)));
			rule.Declarations.Add(new NValueDeclaration<NMargins>(PaddingProperty, CategoryHeaderPadding));

			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NLabel.NLabelSchema);
				sb.ValueEquals(NMouse.IsDirectlyOverPropertyEx, true);
				sb.ChildOf();
				sb.Type(NExampleCategoryHeader.NExampleCategoryHeaderSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(NColor.Wheat)));

			#endregion

			#region Component Image Box

			// Component image box
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NComponentImageBox.NComponentImageBoxSchema);
			});
			rule.Declarations.Add(new NValueDeclaration<NMargins>(BorderThicknessProperty, new NMargins(1)));
			rule.Declarations.Add(new NValueDeclaration<NFont>(FontProperty,
				new NFont(NFontDescriptor.DefaultSansFamilyName, 8.25, ENFontStyle.Bold)));
			rule.Declarations.Add(new NValueDeclaration<NCursor>(CursorProperty, new NCursor(ENPredefinedCursor.Hand)));

			// Component image box - mouse over
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NComponentImageBox.NComponentImageBoxSchema);
				sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			});
			rule.Declarations.Add(new NValueDeclaration<NBorder>(BorderProperty, NBorder.CreateFilledBorder(NColor.Gray)));

			#endregion

			#region Example tiles and contact labels

			// Example tiles and contact labels
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NExampleTile.NExampleTileSchema);
				sb.ChildOf();
				sb.Type(NTableFlowPanel.NTableFlowPanelSchema);
				sb.End();

				sb.Start();
				sb.Type(NLabel.NLabelSchema);
				sb.UserClass("ContactLabel");
			});
			rule.Declarations.Add(new NValueDeclaration<NCursor>(CursorProperty, new NCursor(ENPredefinedCursor.Hand)));

			// Example tiles and contact labels - mouse over
			rule = styleSheet.CreateRule(delegate (NSelectorBuilder sb)
			{
				sb.Type(NExampleTile.NExampleTileSchema);
				sb.ValueEquals(NMouse.IsOverPropertyEx, true);
				sb.ChildOf();
				sb.Type(NTableFlowPanel.NTableFlowPanelSchema);
				sb.End();

				sb.Start();
				sb.Type(NLabel.NLabelSchema);
				sb.UserClass("ContactLabel");
				sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			});
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(MouseOverColor)));

			#endregion

			#region Example in the search box drop down

			NExamplesSearchBox.AddStyles(styleSheet, theme);

			#endregion

			return styleSheet;
		}

		#endregion

		#region Event Handlers

		private void OnExampleCategoryMouseUp(NMouseButtonEventArgs arg)
		{
			string categoryName = (string)arg.TargetNode.Tag;
			NavigateToCategory(categoryName);
		}
		private void OnEmailLabelMouseUp(NMouseButtonEventArgs arg)
		{
			NApplication.OpenUrl("mailto:" + NExamplesUiHelpers.NevronEmail + "?subject=Nevron Open Vision Question");
		}
		private void OnSearchBoxListBoxItemSelected(NEventArgs arg)
		{
			if (arg.Cancel)
				return;

			INSearchableListBox listBox = (INSearchableListBox)arg.TargetNode;
			NWidget selectedItem = ((NKeyValuePair<string, NWidget>)listBox.GetSelectedItem()).Value;

			if (selectedItem != null && TileSelected != null)
			{
				NExampleTileInfo tileInfo = (NExampleTileInfo)selectedItem.Tag;
				TileSelected(tileInfo.XmlElement);
			}

			// Mark the event as handled
			arg.Cancel = true;
		}
		private void OnExampleOptionsChanged(NEventArgs arg)
		{
			NValueChangeEventArgs changeArg = arg as NValueChangeEventArgs;
			if (changeArg == null)
				return;

			if (changeArg.Property == NExamplesOptions.RecentExamplesProperty)
			{
				// Update the Recent Examples menu drop down
				NExamplesUiHelpers.PopulateExamplesDropDown(
					m_RecentExamplesDropDown,
					NExamplesOptions.Instance.RecentExamples.GetReverseIterator(), // Most recent examples should be first
					m_ExamplesMap,
					OnExampleMenuItemClick);
			}
			else if (changeArg.Property == NExamplesOptions.FavoriteExamplesProperty)
			{
				// Update Favorite Examples menu drop down
				NExamplesUiHelpers.PopulateExamplesDropDown(
					m_FavoriteExamplesDropDown,
					NExamplesOptions.Instance.FavoriteExamples.GetIterator(),
					m_ExamplesMap,
					OnExampleMenuItemClick);
			}
		}
		private void OnExampleMenuItemClick(NEventArgs arg)
		{
			if (TileSelected != null)
			{
				NXmlElement xmlElement = NExamplesUiHelpers.GetMenuItemExample((NMenuItem)arg.CurrentTargetNode);
				if (xmlElement != null)
				{
					TileSelected(xmlElement);
				}
			}
		}

		#endregion

		#region Fields

		internal NExamplesSearchBox m_SearchBox;
		internal NEmfDecompressor m_EmfDecompressor;
		internal NStringMap<NWidget> m_ExamplesMap;

		private NLabel m_HeaderLabel;
		private NSingleVisiblePanel m_PagePanel;
		private NDockPanel m_MainPanel;
		private NMenuDropDown m_FavoriteExamplesDropDown;
		private NMenuDropDown m_RecentExamplesDropDown;
		private NMap<string, NColor> m_StatusColorMap;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHomePage.
		/// </summary>
		public static readonly NSchema NHomePageSchema;

		#endregion

		#region Static Methods

		internal static string GetNamespace(NXmlElement element)
		{
			string result;
			do
			{
				result = element.GetAttributeValue("namespace");
				element = element.Parent as NXmlElement;
			}
			while ((result == null || result.Length == 0) && element != null);

			return result;
		}

		private static bool TryGetInt(NXmlElement element, string attribute, out int result)
		{
			string str = element.GetAttributeValue(attribute);
			if (str == null || str.Length == 0)
			{
				result = 0;
				return false;
			}

			return Int32.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
		}

		#endregion

		#region Constants

		internal static readonly NColor HeaderColor = new NColor(104, 52, 140);

		private static readonly NColor BackgroundColor = new NColor(234, 238, 242);
		private static readonly NColor TextColor = NColor.Black;
		private static readonly NColor MouseOverColor = NColor.Blue;

		private const double CategoryHeaderFontSize = 12;
		private const double LabelFontSize = 10;
		private static readonly NMargins CategoryHeaderPadding = new NMargins(5, 10);

		private const double GroupHorizontalSpacing = 20;
		private const double GroupVerticalSpacing = 20;
		private const double ItemHorizontalSpacing = 20;
		private const double ItemVerticalSpacing = 10;

		private const int defaultMaxRows = 15;

		#endregion
	}
}