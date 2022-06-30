using System;
using System.Globalization;
using System.IO;
using System.Text;

using Nevron.Nov.Compression;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Wmf;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents the home page of the examples.
	/// </summary>
	public class NExamplesHomePage : NDocumentBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExamplesHomePage()
		{
			// Apply a Windows 8 theme
			Document.InheritStyleSheets = false;
			NUITheme theme = new NWindows8Theme();
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
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExamplesHomePage()
		{
			NExamplesHomePageSchema = NSchema.Create(typeof(NExamplesHomePage), NDocumentBox.NDocumentBoxSchema);
		}

		#endregion

		#region Events

		public event Function<NXmlElement> TileSelected;

		#endregion

		#region Public Methods - Load from Stream

		public void LoadFromStream(Stream stream)
		{
			// Load an xml document from the stream
			NXmlDocument xmlDocument = NXmlDocument.LoadFromStream(stream);

			// Process it
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
			m_SearchBox.InitAutoComplete(m_ExamplesMap, new NExampleFactory());
			m_SearchBox.ListBoxItemSelected += OnSearchBoxListBoxItemSelected;
		}

		#endregion

		#region Protected Overrides - Mouse Up

		protected override void OnMouseUp(NMouseButtonEventArgs args)
		{
			base.OnMouseUp(args);

			if (args.Cancel || args.Button != ENMouseButtons.Left)
				return;

			if (args.TargetNode is NImageBox && args.TargetNode.Tag is NXmlElement)
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

			NItemInfo itemInfo = (NItemInfo)tile.Tag;
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
		private NPairBox CreateSearchAndContacts()
		{
			m_SearchBox = CreateSearchBox();
			m_SearchBox.VerticalPlacement = ENVerticalPlacement.Center;

			NTableFlowPanel contactsPanel = CreateContactsPanel();
			contactsPanel.HorizontalPlacement = ENHorizontalPlacement.Right;

			NPairBox pairBox = new NPairBox(m_SearchBox, contactsPanel);
			pairBox.Spacing = NDesign.HorizontalSpacing;
			return pairBox;
		}
		private NAutoCompleteBox CreateSearchBox()
		{
			NAutoCompleteBox searchBox = new NAutoCompleteBox();
			searchBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			searchBox.VerticalPlacement = ENVerticalPlacement.Center;
			searchBox.StringMatchMode = ENStringMatchMode.WordStartsWith;
			searchBox.PreferredWidth = 200;
			searchBox.Image = NResources.Image_Search_png;
			searchBox.Padding = new NMargins(2, 5, 2, 5);

			return searchBox;
		}
		private NTableFlowPanel CreateContactsPanel()
		{
			NTableFlowPanel table = new NTableFlowPanel();
			table.Direction = ENHVDirection.LeftToRight;
			table.MaxOrdinal = 2;

			byte[] metaImage = m_EmfDecompressor.GetMetaImage("PhoneIcon.emf");
			table.Add(new NImageBox(new NBytesImageSource(metaImage)));

			NLabel phoneLabel = new NLabel("+1-888-201-6088");
			phoneLabel.UserClass = "ContactLabel";
			phoneLabel.MouseUp += OnPhoneLabelMouseUp;
			table.Add(phoneLabel);

			metaImage = m_EmfDecompressor.GetMetaImage("MailIcon.emf");
			table.Add(new NImageBox(new NBytesImageSource(metaImage)));

			NLabel emailLabel = new NLabel("support@nevron.com");
			emailLabel.UserClass = "ContactLabel";
			emailLabel.MouseUp += OnEmailLabelMouseUp;
			table.Add(emailLabel);

			return table;
		}

		#endregion

		#region Implementation - Categories, Groups, Tiles and Labels

		/// <summary>
		/// Creates a widget for the given category element.
		/// Category elements can contain only row elements.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		private NWidget CreateCategory(NXmlElement category)
		{
			string categoryTitle = category.GetAttributeValue("name");
			NColor color = NColor.ParseHex(category.GetAttributeValue("color"));
			NColor lightColor = color.Lighten(0.6f);

			// Create the header label
			NExampleCategoryHeader categoryHeader = new NExampleCategoryHeader(categoryTitle);
			categoryHeader.BackgroundFill = new NColorFill(color);
			categoryHeader.Status = category.GetAttributeValue("status");

			// Create a stack panel for the category widgets
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = ItemVerticalSpacing;
			stack.BackgroundFill = new NColorFill(lightColor);
			stack.Tag = category;

			// Add the header label
			stack.Add(categoryHeader);

			// Loop through the rows
			for (int i = 0, count = category.ChildrenCount; i < count; i++)
			{
				NXmlElement row = category.GetChildAt(i) as NXmlElement;
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
				if (NApplication.IOService.DirectorySeparatorChar != '\\')
				{
					iconName = iconName.Replace('\\', NApplication.IOService.DirectorySeparatorChar);
				}

				string imageFolder = NPath.GetFullDirectoryName(iconName);
				if (String.IsNullOrEmpty(imageFolder))
				{
					// The icon is in the folder for the current category
					imageFolder = categoryNamespace;
				}
				else
				{
					// The icon is in a folder of another category
					imageFolder = NPath.Normalize(NPath.Combine(categoryNamespace, imageFolder));
					if (imageFolder[imageFolder.Length - 1] == NApplication.IOService.DirectorySeparatorChar)
					{
						imageFolder = imageFolder.Remove(imageFolder.Length - 1);
					}

					// Update the icon name
					iconName = NPath.GetFileName(iconName);
				}

				iconName = "RIMG_ExampleIcons_" + imageFolder + "_" + iconName.Replace('.', '_');
				icon = new NImage(new NEmbeddedResourceRef(NResources.Instance, iconName));
			}

			// Create and configure the tile
			NExampleTile tile = new NExampleTile(icon, tileTitle);
			tile.HorizontalPlacement = ENHorizontalPlacement.Left;
			tile.Status = element.GetAttributeValue("status");
			tile.Tag = new NItemInfo(element);

			// Add the examples of the current tile to the examples map
			INIterator<NXmlNode> iter = element.GetChildNodesIterator();
			while (iter.MoveNext())
			{
				NXmlElement exampleElement = iter.Current as NXmlElement;
				if (exampleElement == null)
					continue;

				string examplePath = GetExamplePath(exampleElement);
				if (icon != null)
				{
					icon = new NImage(icon.ImageSource);
				}

				NExampleTile example = new NExampleTile(icon, examplePath);
				example.Status = exampleElement.GetAttributeValue("status");
				example.Tag = new NItemInfo(exampleElement);

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

		private NStyleSheet CreateCustomStyleSheet(NUITheme theme)
		{
			NStyleSheet styleSheet = new NStyleSheet();

			#region Category headers

			NRule rule = new NRule();
			NSelectorBuilder sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NExampleCategoryHeader.NExampleCategoryHeaderSchema);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NFont>(FontProperty, new NFont(NFontDescriptor.DefaultSansFamilyName,
				CategoryHeaderFontSize, ENFontStyle.Bold)));
			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(NColor.White)));
			rule.Declarations.Add(new NValueDeclaration<NMargins>(PaddingProperty, CategoryHeaderPadding));
			styleSheet.Add(rule);

			rule = new NRule();
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NLabel.NLabelSchema);
			sb.ValueEquals(NMouse.IsDirectlyOverPropertyEx, true);
			sb.ChildOf();
			sb.Type(NExampleCategoryHeader.NExampleCategoryHeaderSchema);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(NColor.Wheat)));
			styleSheet.Add(rule);

			#endregion

			#region Component Image Box

			// Component image box
			rule = new NRule();
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NComponentImageBox.NComponentImageBoxSchema);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NMargins>(BorderThicknessProperty, new NMargins(1)));
			rule.Declarations.Add(new NValueDeclaration<NFont>(FontProperty,
				new NFont(NFontDescriptor.DefaultSansFamilyName, 8.25, ENFontStyle.Bold)));
			rule.Declarations.Add(new NValueDeclaration<NCursor>(CursorProperty, new NCursor(ENPredefinedCursor.Hand)));
			styleSheet.Add(rule);

			// Component image box - mouse over
			rule = new NRule();
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NComponentImageBox.NComponentImageBoxSchema);
			sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NBorder>(BorderProperty, NBorder.CreateFilledBorder(NColor.Gray)));
			styleSheet.Add(rule);

			#endregion

			#region Example tiles and contact labels

			// Example tiles and contact labels
			rule = new NRule();
			
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NExampleTile.NExampleTileSchema);
			sb.ChildOf();
			sb.Type(NTableFlowPanel.NTableFlowPanelSchema);
			sb.End();

			sb.Start();
			sb.Type(NLabel.NLabelSchema);
			sb.UserClass("ContactLabel");
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NCursor>(CursorProperty, new NCursor(ENPredefinedCursor.Hand)));
			styleSheet.Add(rule);

			// Example tiles and contact labels - mouse over
			rule = new NRule();
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NExampleTile.NExampleTileSchema);
			sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			sb.ChildOf();
			sb.Type(NTableFlowPanel.NTableFlowPanelSchema);
			sb.End();

			sb.Start();
			sb.Type(NLabel.NLabelSchema);
			sb.UserClass("ContactLabel");
			sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NFill>(TextFillProperty, new NColorFill(MouseOverColor)));
			styleSheet.Add(rule);

			#endregion

			#region Example in the autocomplete list box

			rule = new NRule();
			sb = rule.GetSelectorBuilder();
			sb.Start();
			sb.Type(NListBoxItem.NListBoxItemSchema);
			sb.ValueEquals(NMouse.IsOverPropertyEx, true);
			sb.ChildOf();
			sb.ChildOf();
			sb.Type(NListBox.NListBoxSchema);
			sb.ChildOf();
			sb.Type(NPopupWindow.NPopupWindowSchema);
			sb.End();

			rule.Declarations.Add(new NValueDeclaration<NFill>(BackgroundFillProperty,
				new NColorFill(theme.Colors.ButtonSelectedHighlight)));
			styleSheet.Add(rule);

			#endregion

			return styleSheet;
		}
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

		#endregion

		#region Event Handlers

		private void OnExampleCategoryMouseUp(NMouseButtonEventArgs arg)
		{
			NXmlElement element = (NXmlElement)arg.TargetNode.Tag;

			// Find the category correpsonding to the XML element of the clicked header
			int count = m_PagePanel.Count;
			for (int i = 0; i < count; i++)
			{
				if (m_PagePanel[i].Tag == element)
				{
					m_PagePanel.VisibleIndex = i;
					return;
				}
			}

			// No category selected, so make the Welcome Screen visible
			m_PagePanel.VisibleIndex = 0;
		}
		private void OnPhoneLabelMouseUp(NMouseButtonEventArgs arg)
		{
			string phone = ((NLabel)arg.CurrentTargetNode).Text;
			phone = phone.Replace("-", String.Empty);
			NApplication.OpenUrl("tel:" + phone);
		}
		private void OnEmailLabelMouseUp(NMouseButtonEventArgs arg)
		{
			string email = ((NLabel)arg.CurrentTargetNode).Text;
			NApplication.OpenUrl("mailto:" + email + "?subject=Nevron Open Vision Question");
		}
		private void OnSearchBoxListBoxItemSelected(NEventArgs arg)
		{
			if (arg.Cancel)
				return;

			INSearchableListBox listBox = (INSearchableListBox)arg.TargetNode;
			NWidget selectedItem = ((NKeyValuePair<string, NWidget>)listBox.GetSelectedItem()).Value;

			if (selectedItem != null && TileSelected != null)
			{
				NItemInfo tileInfo = (NItemInfo)selectedItem.Tag;
				TileSelected(tileInfo.XmlElement);
			}

			// Mark the event as handled
			arg.Cancel = true;
		}

		#endregion

		#region Fields

		internal NAutoCompleteBox m_SearchBox;
		internal NEmfDecompressor m_EmfDecompressor;

		private NLabel m_HeaderLabel;
		private NSingleVisiblePanel m_PagePanel;
		private NDockPanel m_MainPanel;
		private NStringMap<NWidget> m_ExamplesMap;
		private NMap<string, NColor> m_StatusColorMap;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExamplesHomePage.
		/// </summary>
		public static readonly NSchema NExamplesHomePageSchema;

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

		private static int GetDistanceFromRoot(NXmlElement element)
		{
			int distanceFromRoot = 0;

			NXmlNode node = element.Parent;
			while (node != null)
			{
				node = node.Parent;
				distanceFromRoot++;
			}

			return distanceFromRoot - 1;
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
		private static bool TryGetFactor(NXmlElement element, string attribute, out double result)
		{
			string str = NStringHelpers.SafeTrim(element.GetAttributeValue(attribute));
			if (str == null || str.Length == 0)
			{
				result = 0;
				return false;
			}

			bool isPercent = str[str.Length - 1] == '%';
			if (isPercent)
			{
				str = str.Substring(0, str.Length - 1);
			}

			if (Double.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out result) == false)
				return false;

			if (isPercent)
			{
				result = result / 100;
			}

			return true;
		}
		private static string ToString(double value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}
		/// <summary>
		/// Gets the full path to the given example by prepending the names of its parent XML elements.
		/// </summary>
		/// <param name="exampleElement"></param>
		/// <returns></returns>
		private static string GetExamplePath(NXmlElement exampleElement)
		{
			string path = null;

			NXmlElement element = exampleElement;
			while (element.Name != "document")
			{
				if (NExampleHost.IsSingleExampleTile(element) == false)
				{
					// The current XML element is not a tile with a single example
					string name = element.GetAttributeValue("name");
					if (String.IsNullOrEmpty(name) == false)
					{
						// The current element has a "name" attribute value, so prepend it to the path
						path = String.IsNullOrEmpty(path) ? name : name + " > " + path;
					}
				}

				element = (NXmlElement)element.Parent;
			}

			return path;
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

		#region Nested Types

		private class NExampleFactory : NWidgetFactory<NKeyValuePair<string, NWidget>>
		{
			public override string GetString(NKeyValuePair<string, NWidget> item)
			{
				return item.Key;
			}
			public override NWidget CreateWidget(NKeyValuePair<string, NWidget> item)
			{
				return (NWidget)item.Value.DeepClone();
			}
		}
		private class NItemInfo : INDeeplyCloneable
		{
			public NItemInfo(NXmlElement xmlElement)
			{
				XmlElement = xmlElement;
			}

			public object DeepClone()
			{
				return new NItemInfo(XmlElement);
			}

			public NXmlElement XmlElement;
		}

		#endregion
	}
}