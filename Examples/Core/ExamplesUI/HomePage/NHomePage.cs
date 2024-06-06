using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// The home page of the NOV examples.
	/// </summary>
	public class NHomePage : NPairBox
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NHomePage()
			: base(new NHomePageHeader(), new NHomePageContent(), ENPairBoxRelation.Box1AboveBox2)
		{
			Spacing = 0;

			// Subscribe to the ExampleOptions Changed event
			NExamplesOptions.Instance.Changed += OnExampleOptionsChanged;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NHomePage()
		{
			NHomePageSchema = NSchema.Create(typeof(NHomePage), NPairBoxSchema);
		}

		#endregion

		#region Events

		public event Function<NXmlElement> ItemSelected;

		#endregion

		#region Properties

		internal NHomePageHeader Header
		{
			get
			{
				return (NHomePageHeader)Box1;
			}
		}
		internal NHomePageContent Content
		{
			get
			{
				return (NHomePageContent)Box2;
			}
		}

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
			NXmlElement titleElement = (NXmlElement)rootElement.GetFirstChild(NExamplesXml.Attribute.Title);
			string title = ((NXmlTextNode)titleElement.GetChildAt(0)).Text;

			NXmlElement statusColorsElement = (NXmlElement)rootElement.GetFirstChild("statusColors");
			m_StatusColorMap = ParseStatusColors(statusColorsElement);

			// Process the categories
			NXmlElement categoriesElement = (NXmlElement)rootElement.GetFirstChild("categories");

			for (int i = 0, count = categoriesElement.ChildrenCount; i < count; i++)
			{
				if (categoriesElement.GetChildAt(i) is NXmlElement childElement)
				{
					if (childElement.Name != NExamplesXml.Element.Category)
						throw new Exception("The body element can contain only category elements");

					// Parse the category
					int oldExamplesCount = m_ExamplesMap.Count;
					ParseCategory(childElement);

					// Add a tab page for the category
					int examplesCount = m_ExamplesMap.Count - oldExamplesCount;
					Content.AddCategory(childElement, examplesCount);
				}
			}

			// Init the search box
			Header.SearchBox.InitAutoComplete(m_ExamplesMap, new NExampleTileFactory());
			Header.SearchBox.ListBoxItemSelected += OnSearchBoxListBoxItemSelected;
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
		/// <summary>
		/// Raises the <see cref="ItemSelected"/> event.
		/// </summary>
		/// <param name="xmlElement"></param>
		internal void RaiseItemSelected(NXmlElement xmlElement)
		{
			if (ItemSelected != null)
			{
				ItemSelected(xmlElement);
			}
		}

		#endregion

		#region Implementation - XML Parsing

		private void ParseCategory(NXmlElement categoryElement)
		{
			string categoryNamespace = categoryElement.GetAttributeValue(NExamplesXml.Attribute.Namespace);
			NXmlNode contentElement = categoryElement.GetFirstChild(NExamplesXml.Element.Content);
			if (contentElement == null)
				return;

			// Get the icon for the category
			NImage icon = NExamplesUi.GetComponentIcon(categoryElement, ENImageStyle.Colored);

			// Parse the category child elements
			for (int i = 0; i < contentElement.ChildrenCount; i++)
			{
				if (contentElement.GetChildAt(i) is NXmlElement childElement)
				{
					ParseXmlElement(childElement, categoryNamespace, icon);
				}
            }
		}
		private void ParseXmlElement(NXmlElement xmlElement, string categoryNamespace, NImage icon)
		{
			switch (xmlElement.Name)
			{
				case NExamplesXml.Element.Folder:
					ParseFolder(xmlElement, categoryNamespace, icon);
					break;
				case NExamplesXml.Element.Example:
					ParseExample(xmlElement, icon);
					break;
				default:
					NDebug.Assert(false, "New examples content XML child element?");
					break;
			}
		}
		private void ParseFolder(NXmlElement folderElement, string categoryNamespace, NImage icon)
		{
			// Add the examples of the current tile to the examples map
			for (int i = 0; i < folderElement.ChildrenCount; i++)
			{
				if (folderElement.GetChildAt(i) is NXmlElement xmlElement)
				{
					ParseXmlElement(xmlElement, categoryNamespace, icon);
				}
			}
		}
		private void ParseExample(NXmlElement exampleElement, NImage icon)
		{
			if (!NExamplesXml.IsSupportedOnTheCurrentPlatform(exampleElement))
				return;

			string examplePath = NExamplesUi.GetExamplePath(exampleElement);
			if (icon != null)
			{
				icon = new NImage(icon.ImageSource);
			}

			NExampleTile example = new NExampleTile(icon, examplePath);
			example.Status = exampleElement.GetAttributeValue(NExamplesXml.Attribute.Status);
			example.Tag = new NExampleTileInfo(exampleElement);

			if (!m_ExamplesMap.Contains(examplePath))
			{
				m_ExamplesMap.Add(examplePath, example);
			}
		}

		#endregion

		#region Event Handlers

		private void OnExampleOptionsChanged(NEventArgs arg)
		{
			NValueChangeEventArgs changeArg = arg as NValueChangeEventArgs;
			if (changeArg == null)
				return;

			if (changeArg.Property == NExamplesOptions.RecentExamplesProperty)
			{
				// Update the Recent Examples menu drop down
				NExamplesUi.PopulateExamplesDropDown(
					Header.RecentExamplesDropDown,
					NExamplesOptions.Instance.RecentExamples.GetReverseIterator(), // Most recent examples should be first
					m_ExamplesMap,
					OnExampleMenuItemClick);
			}
			else if (changeArg.Property == NExamplesOptions.FavoriteExamplesProperty)
			{
				// Update Favorite Examples menu drop down
				NExamplesUi.PopulateExamplesDropDown(
					Header.FavoriteExamplesDropDown,
					NExamplesOptions.Instance.FavoriteExamples.GetIterator(),
					m_ExamplesMap,
					OnExampleMenuItemClick);
			}
		}
		private void OnExampleMenuItemClick(NEventArgs arg)
		{
			NXmlElement xmlElement = NExamplesUi.GetMenuItemExample((NMenuItem)arg.CurrentTargetNode);
			if (xmlElement != null)
			{
				RaiseItemSelected(xmlElement);
			}
		}
		private void OnSearchBoxListBoxItemSelected(NEventArgs arg)
		{
			if (arg.Cancel)
				return;

			INSearchableListBox listBox = (INSearchableListBox)arg.TargetNode;
			NWidget selectedItem = ((NKeyValuePair<string, NWidget>)listBox.GetSelectedItem()).Value;

			if (selectedItem != null)
			{
				NExampleTileInfo tileInfo = (NExampleTileInfo)selectedItem.Tag;
				RaiseItemSelected(tileInfo.XmlElement);
			}

			// Mark the event as handled
			arg.Cancel = true;
		}

		#endregion

		#region Fields

		// Examples data structures
		internal NStringMap<NWidget> m_ExamplesMap;
		private NMap<string, NColor> m_StatusColorMap;

		#endregion

		#region Static Methods - Styling

		/// <summary>
		/// Parses the background colors of the status labels defined in the examples XML file.
		/// </summary>
		/// <param name="styleElement"></param>
		private static NMap<string, NColor> ParseStatusColors(NXmlElement styleElement)
		{
			NMap<string, NColor> statusColorMap = new NMap<string, NColor>();
			if (styleElement != null)
			{
				for (int i = 0, count = styleElement.ChildrenCount; i < count; i++)
				{
					NXmlElement child = styleElement.GetChildAt(i) as NXmlElement;
					if (child == null || child.Name != NExamplesXml.Attribute.Status)
						continue;

					// Get the status name
					string name = child.GetAttributeValue(NExamplesXml.Attribute.Name);
					if (name == null)
						continue;

					// Parse the status color
					string colorStr = child.GetAttributeValue(NExamplesXml.Attribute.Color);
					NColor color;
					if (NColor.TryParse(colorStr, out color) == false)
						continue;

					// Add the name/color pair to the status color map
					statusColorMap.Set(name, color);
				}
			}

			return statusColorMap;
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NHomePage.
		/// </summary>
		public static readonly NSchema NHomePageSchema;

		#endregion
	}
}