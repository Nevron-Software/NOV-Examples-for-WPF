using System;
using System.IO;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// An accordion that has an item for each NOV component and shows a tree view
	/// with the examples for the selected component.
	/// </summary>
	internal class NExamplesAccordion : NAccordion
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExamplesAccordion()
		{
			m_ExamplesMap = new NMap<string, NTreeViewItem>();

			// Create a stack panel for the example categories (NOV components)
			m_FlexBox = new NAccordionFlexBoxPanel();
			m_FlexBox.Direction = ENHVDirection.TopToBottom;
			m_FlexBox.VerticalSpacing = 0;

			// Set the stack as content of the accordion
			Content = m_FlexBox;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExamplesAccordion()
		{
			NExamplesAccordionSchema = NSchema.Create(typeof(NExamplesAccordion), NAccordionSchema);

			// Create the product icon map
			ExampleIconMap = new NMap<string, NImage>();
		}

		#endregion

		#region Events

		public event Function<NValueChangeEventArgs> TreeViewSelectedPathChanged;

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes the accordion from the given XML document.
		/// </summary>
		/// <param name="xmlDocument"></param>
		public void InitializeFromXml(NXmlDocument xmlDocument)
		{
			m_FlexBox.Clear();
			m_ExamplesMap.Clear();

			NXmlNode categoriesNode = xmlDocument.GetFirstDescendant("categories");
			NList<NXmlNode> categories = categoriesNode.GetChildren(CategoryElement);

			for (int i = 0; i < categories.Count; i++)
			{
				NXmlElement category = (NXmlElement)categories[i];
				NWidget sectionHeader = CreateSectionHeader(category);
				NWidget sectionContent = CreateSectionContent(category);

				// Create an expandable section for the current category
				NExpandableSection section = new NExpandableSection(sectionHeader, sectionContent);
				section.ContentPadding = NMargins.Zero;
				m_FlexBox.Add(section);
			}
		}

		/// <summary>
		/// Finds, navigates to and returns the default example element of the given XML element.
		/// </summary>
		/// <param name="xmlElement"></param>
		/// <returns></returns>
		public NXmlElement NavigateToExample(NXmlElement xmlElement)
		{
			switch (xmlElement.Name)
			{
				case ExampleElement:
					break;

				case TileElement:
					xmlElement = GetExampleXmlElement(xmlElement);
					break;

				case CategoryElement:
					// Navigate to an example's category on the home screen
					NExamplesContent examplesContent = GetFirstAncestor<NExamplesContent>();
					examplesContent.NavigateToHomePageCategory(xmlElement.GetAttributeValue(NameAttribute));
					return null;
			}

			string examplePath = NExamplesUiHelpers.GetExamplePath(xmlElement);

			NTreeViewItem treeViewItem;
			if (m_ExamplesMap.TryGet(examplePath, out treeViewItem))
			{
				// Navigate to an example or an example folder
				NTreeView treeView = treeViewItem.OwnerTreeView;
				treeView.ExpandPathToItem(treeViewItem);
				treeView.SelectedItem = treeViewItem;
				treeView.EnsureVisible(treeViewItem);

				NExpandableSection section = treeView.GetFirstAncestor<NExpandableSection>();
				section.OwnerAccordion.ExpandedSection = section;
			}
			else
			{
				NDebug.Assert(false);
			}

			return xmlElement;
		}

		#endregion

		#region Implementation - Accordion

		private NWidget CreateSectionHeader(NXmlElement category)
		{
			string name = category.GetAttributeValue(NameAttribute);
			NImage icon = GetCategoryIcon(category.GetAttributeValue(NamespaceAttribute));

			NPairBox sectionHeader = NPairBox.Create(icon, name);
			return sectionHeader;
		}
		private NWidget CreateSectionContent(NXmlElement category)
		{
			NTreeView treeView = new NTreeView();
			treeView.HScrollMode = ENScrollMode.Never;
			treeView.BorderThickness = NMargins.Zero;
			treeView.Items.Padding = new NMargins(NDesign.HorizontalSpacing, NDesign.VerticalSpacing);
			treeView.SelectedPathChanged += OnTreeViewSelectedPathChanged;

			AddItemsFor(treeView.Items, category);

			return treeView;
		}

		#endregion

		#region Implementation - Tree View

		private void AddItemsFor(NTreeViewItemCollection items, NXmlElement xmlElement)
		{
			int childrenCount = xmlElement.ChildrenCount;
			for (int i = 0; i < childrenCount; i++)
			{
				NXmlElement child = xmlElement.GetChildAt(i) as NXmlElement;
				if (child == null)
					continue;

				NTreeViewItem item = CreateTreeViewItem(child);
				if (item != null)
				{
					items.Add(item);
					if (item.Tag is NXmlElement == false)
					{
						// This is a folder item, so add items for its children, too
						AddItemsFor(item.Items, child);
					}
				}
				else
				{
					AddItemsFor(items, child);
				}
			}
		}
		private NTreeViewItem CreateTreeViewItem(NXmlElement xmlElement)
		{
			if (NExamplesUiHelpers.IsSingleExampleTile(xmlElement))
			{
				// This is a tile with a single example, so create only the example tree view item
				return CreateTreeViewItem((NXmlElement)xmlElement.GetChildAt(0));
			}

			NImage icon;
			string name = xmlElement.GetAttributeValue(NameAttribute);
			if (String.IsNullOrEmpty(name))
				return null;

			switch (xmlElement.Name)
			{
				case TileElement:
				case GroupElement:
					icon = NResources.Image__16x16_Folders_png;
					break;

				case ExampleElement:
					// Get an icon for the example, which is grayscale version of the example's category icon
					NXmlElement categoryXmlElement = (NXmlElement)xmlElement.GetFirstAncestor(CategoryElement);
					string categoryNamespace = categoryXmlElement.GetAttributeValue(NamespaceAttribute);
					icon = GetExampleIcon(categoryNamespace);
					break;

				default:
					return null;
			}

			NExampleTile tile = new NExampleTile(icon, name);
			tile.Box1.Padding = new NMargins(0, 1, 0, 1);
			tile.Status = xmlElement.GetAttributeValue(StatusAttribute);
			tile.Box2.VerticalPlacement = ENVerticalPlacement.Center;
			tile.Spacing = NDesign.HorizontalSpacing;

			NTreeViewItem treeViewItem = new NTreeViewItem(tile);
			string examplePath = NExamplesUiHelpers.GetExamplePath(xmlElement);
			m_ExamplesMap.Add(examplePath, treeViewItem);

			if (xmlElement.Name == ExampleElement)
			{
				// This is an example element
				treeViewItem.Tag = xmlElement;

				// Handle the right click event to show a context menu for copying a link to the example
				treeViewItem.MouseDown += OnTreeViewItemMouseDown;
			}

			return treeViewItem;
		}

		#endregion

		#region Event Handlers - Tree View

		/// <summary>
		/// Called when a new tree view item has been selected.
		/// </summary>
		/// <param name="arg"></param>
		private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
		{
			NTreeView treeView = (NTreeView)arg.TargetNode;
			NTreeViewItem oldSelectedItem = GetTreeViewItem(treeView, (NDomPath)arg.OldValue);
			NTreeViewItem newSelectedItem = GetTreeViewItem(treeView, (NDomPath)arg.NewValue);

			if (oldSelectedItem != null &&
				oldSelectedItem.Tag is NXmlElement oldXmlElement && oldXmlElement.Name == ExampleElement)
			{
				// Convert the image of the old selected tree view item to grayscale
				NImageBox imageBox = oldSelectedItem.GetFirstDescendant<NImageBox>();
				NImageSource imageSource = imageBox.Image.ImageSource;
				imageBox.Image = new NImage(new NGrayscaleImageSource(imageSource));
			}

			if (newSelectedItem != null &&
				newSelectedItem.Tag is NXmlElement newXmlElement && newXmlElement.Name == ExampleElement)
			{
				// Set a colored image to the new selected tree view image
				NImageBox imageBox = newSelectedItem.GetFirstDescendant<NImageBox>();
				NImageSource imageSource = imageBox.Image.ImageSource;
				imageBox.Image = new NImage(((NGrayscaleImageSource)imageSource).OriginalImageSource);
			}

			if (TreeViewSelectedPathChanged != null)
			{
				TreeViewSelectedPathChanged(arg);
			}
		}

		/// <summary>
		/// Called to show a context menu with a copy link to an example option.
		/// </summary>
		/// <param name="arg"></param>
		private void OnTreeViewItemMouseDown(NMouseButtonEventArgs arg)
		{
			if (arg.Cancel || arg.Button != ENMouseButtons.Right)
				return;

			// Mark the event as handled
			arg.Cancel = true;

			// Get the right clicked tree view item
			NTreeViewItem item = (NTreeViewItem)arg.CurrentTargetNode;
			NTreeView treeView = item.OwnerTreeView;

			// Create the context menu
			NMenu contextMenu = new NMenu();
			NMenuItem copyLinkToClipboard = new NMenuItem(Presentation.NResources.Image_Edit_Copy_png, "Copy link to clipboard");
			copyLinkToClipboard.Click += OnCopyLinkToClipboardClick;
			copyLinkToClipboard.Tag = item.Tag;
			contextMenu.Items.Add(copyLinkToClipboard);

			// Show the context menu
			NPopupWindow.OpenInContext(new NPopupWindow(contextMenu), treeView, arg.ScreenPosition);
		}
		/// <summary>
		/// Called to copy a link to an example to the clipboard.
		/// </summary>
		/// <param name="arg"></param>
		private void OnCopyLinkToClipboardClick(NEventArgs arg)
		{
			NXmlElement xmlElement = (NXmlElement)arg.CurrentTargetNode.Tag;

			string exampleLink;
			if (NApplication.IntegrationPlatform == ENIntegrationPlatform.WebAssembly)
			{
				NExamplePage exampleHost = GetFirstAncestor<NExamplePage>();
				exampleLink = exampleHost.ExamplesPath + "?example=" + xmlElement.GetAttributeValue("type");
			}
			else
			{
				exampleLink = NExamplesUiHelpers.GetExamplePath(xmlElement);
			}

			// Copy the example link to clipboard
			NDataObject dataObject = new NDataObject();
			dataObject.SetData(NDataFormat.TextFormatString, exampleLink);
			NClipboard.SetDataObject(dataObject);
		}

		#endregion

		#region Fields

		private NAccordionFlexBoxPanel m_FlexBox;
		private NMap<string, NTreeViewItem> m_ExamplesMap;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExamplesAccordion.
		/// </summary>
		public static readonly NSchema NExamplesAccordionSchema;

		#endregion

		#region Static Methods

		/// <summary>
		/// Gets a category icon. Category icons are used in the accordion items.
		/// </summary>
		/// <param name="productName"></param>
		/// <returns></returns>
		private static NImage GetCategoryIcon(string productName)
		{
			// Get product icon from resources
			byte[] imageData = NResources.Instance.GetResourceBytes($"RIMG_ExamplesUI_ProductIcons_{productName}_png");
			NDebug.Assert(imageData != null, $"Image for product {productName} not found");
			return NImage.FromBytes(imageData);
		}
		/// <summary>
		/// Gets a grayscale icon for an example. Example icons are used in the tree views hosted in accordion items.
		/// </summary>
		/// <param name="productName"></param>
		/// <returns></returns>
		private static NImage GetExampleIcon(string productName)
		{
			NImage image;
			if (ExampleIconMap.TryGet(productName, out image))
				return (NImage)image.DeepClone();

			image = GetCategoryIcon(productName);
			image = new NImage(new NGrayscaleImageSource(image.ImageSource));
			ExampleIconMap.Add(productName, image);

			return image;
		}
		private static NTreeViewItem GetTreeViewItem(NTreeView treeView, NDomPath domPath)
		{
			return domPath != null ? (NTreeViewItem)domPath.Select(treeView) : null;
		}

		/// <summary>
		/// Gets the default "example" XML element for the given "tile" XML element.
		/// </summary>
		/// <param name="tileXmlElement"></param>
		/// <returns></returns>
		private static NXmlElement GetExampleXmlElement(NXmlElement tileXmlElement)
		{
			NList<NXmlNode> exampleXmlNodes = tileXmlElement.GetChildren(ExampleElement);
			if (exampleXmlNodes.Count == 0)
			{
				NDebug.Assert(false, "Empty XML tile element found");
				return null;
			}

			for (int i = 0; i < exampleXmlNodes.Count; i++)
			{
				NXmlElement exampleXmlElement = (NXmlElement)exampleXmlNodes[i];
				if (exampleXmlElement.GetAttributeValue("default") == "true")
					return exampleXmlElement;
			}

			// A default XML element not found, so return the first one
			return (NXmlElement)exampleXmlNodes[0];
		}

		#endregion

		#region Constants

		private static readonly NMap<string, NImage> ExampleIconMap;

		// XML Elements
		private const string CategoryElement = "category";
		private const string GroupElement = "group";
		private const string TileElement = "tile";
		private const string ExampleElement = "example";

		// XML Attributes
		private const string NameAttribute = "name";
		private const string NamespaceAttribute = "namespace";
		private const string StatusAttribute = "status";

		#endregion
	}
}