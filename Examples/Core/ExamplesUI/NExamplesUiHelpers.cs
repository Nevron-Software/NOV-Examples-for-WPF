using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	internal static class NExamplesUiHelpers
	{
		#region Methods - Example Path

		/// <summary>
		/// Gets the breadcrumb string for the given XML element.
		/// </summary>
		/// <param name="xmlElement"></param>
		/// <returns></returns>
		public static string GetExamplePath(NXmlElement xmlElement)
		{
			NExamplesStringPathBuilder pathBuilder = new NExamplesStringPathBuilder();
			BuildExamplePath(xmlElement, pathBuilder);
			return pathBuilder.Path;
		}
		/// <summary>
		/// Gets a deque of elements that represent the path to the given XML element
		/// from the root.
		/// </summary>
		/// <param name="xmlElement"></param>
		/// <returns></returns>
		public static NDeque<NXmlElement> GetExampleElementsPath(NXmlElement xmlElement)
		{
			NExamplesElementsPathBuilder pathBuilder = new NExamplesElementsPathBuilder();
			BuildExamplePath(xmlElement, pathBuilder);
			return pathBuilder.Elements;
		}
		/// <summary>
		/// Checks whether the given XML element represents a tile, which contains only one example.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		public static bool IsSingleExampleTile(NXmlElement element)
		{
			return element.Name == "tile" && element.ChildrenCount == 1 &&
				element.GetChildAt(0).Name == "example";
		}
		/// <summary>
		/// Escapes the invalid URL characters from the given path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static string UrlEscapeExamplePath(string path)
		{
			if (String.IsNullOrEmpty(path))
				return path;

			return path
				.Replace("&", "&amp;")
				.Replace(">", "&gt;")
				.Replace("<", "&lt;");

		}

		/// <summary>
		/// Builds example path for the given XML element using the specified path builder.
		/// </summary>
		/// <param name="xmlElement"></param>
		/// <param name="pathBuilder"></param>
		/// <returns></returns>
		private static void BuildExamplePath(NXmlElement xmlElement, NExamplesPathBuilder pathBuilder)
		{
			while (xmlElement.Name != "document")
			{
				if (IsSingleExampleTile(xmlElement) == false)
				{
					// The current XML element is not a tile with a single example
					string name = xmlElement.GetAttributeValue("name");
					if (!String.IsNullOrEmpty(name))
					{
						// The current XML element has a "name" attribute value, so process the element
						pathBuilder.ProcessElement(xmlElement);
					}
				}

				xmlElement = (NXmlElement)xmlElement.Parent;
			}
		}

		#endregion

		#region Methods - Favorite and Recent Menus

		/// <summary>
		/// Populates the given examples menu drop down.
		/// </summary>
		/// <param name="dropDown">The drop down to populate.</param>
		/// <param name="examplePathsIter">An iterator that iterates through the example paths that should be added to the drop down menu.</param>
		/// <param name="examplesMap">A map of example path to example widget.</param>
		/// <param name="menuItemClickHandler">The handler that should process menu item "Click" events.</param>
		public static void PopulateExamplesDropDown(NContentPopupHost dropDown, INIterator<string> examplePathsIter,
			NStringMap<NWidget> examplesMap, Function<NEventArgs> menuItemClickHandler)
		{
			dropDown.ClosePopup();

			NMenuItemCollection menuItems = ((NMenu)dropDown.Popup.Content).Items;
			menuItems.Clear();

			if (examplesMap == null)
				return;

			while (examplePathsIter.MoveNext())
			{
				string examplePath = examplePathsIter.Current;

				NWidget widget;
				if (examplesMap.TryGet(examplePath, out widget))
				{
					NExampleTile tile = (NExampleTile)widget;
					NMenuItem menuItem = new NMenuItem(NSystem.SafeDeepClone(tile.Box1), NSystem.SafeDeepClone(tile.Box2));
					menuItem.Tag = tile.Tag;
					menuItem.Click += menuItemClickHandler;
					menuItems.Add(menuItem);
				}
			}
		}
		/// <summary>
		/// Gets the example XML element for the given menu item.
		/// </summary>
		/// <param name="menuItem"></param>
		/// <returns></returns>
		public static NXmlElement GetMenuItemExample(NMenuItem menuItem)
		{
			NExampleTileInfo tileInfo = (NExampleTileInfo)menuItem.Tag;
			return tileInfo.XmlElement;
		}

		#endregion

		#region Methods - Text Processing

		/// <summary>
		/// Processes the given header text.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ProcessHeaderText(string text)
		{
			return text;

			/*if (text == null)
				return text;

			return text.ToUpper();*/
		}

		#endregion

		#region Nested Types - Example Path

		private abstract class NExamplesPathBuilder
		{
			public abstract void ProcessElement(NXmlElement element);
		}

		private class NExamplesStringPathBuilder : NExamplesPathBuilder
		{
			public string Path
			{
				get
				{
					return m_Path;
				}
			}

			public override void ProcessElement(NXmlElement element)
			{
				string name = element.GetAttributeValue("name");
				m_Path = String.IsNullOrEmpty(m_Path) ? name : name + " > " + m_Path;
			}

			private string m_Path;
		}

		private class NExamplesElementsPathBuilder : NExamplesPathBuilder
		{
			public NExamplesElementsPathBuilder()
			{
				m_Elements = new NDeque<NXmlElement>();
			}

			public NDeque<NXmlElement> Elements
			{
				get
				{
					return m_Elements;
				}
			}

			public override void ProcessElement(NXmlElement element)
			{
				m_Elements.PushFront(element);
			}

			private NDeque<NXmlElement> m_Elements;
		}

		#endregion

		#region Constants

		/// <summary>
		/// The Nevron email address.
		/// </summary>
		public const string NevronEmail = "support@nevron.com";

		#endregion
	}
}