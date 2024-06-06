using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// Represents a breadcrumb to an example.
	/// </summary>
	internal class NExampleBreadcrumb : NStackPanel
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleBreadcrumb()
		{
			Direction = ENHVDirection.LeftToRight;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleBreadcrumb()
		{
			NExampleBreadcrumbSchema = NSchema.Create(typeof(NExampleBreadcrumb), NStackPanelSchema);

			// Static Event Handlers
			NExampleBreadcrumbSchema.AddEventHandler(
				NButtonBase.ClickEvent,
				new NEventHandler<NEventArgs>(ButtonClickHandler));
		}

		#endregion

		#region Events

		public event Function<NEventArgs> ButtonClick;

		#endregion

		#region Public Methods

		/// <summary>
		/// Initializes the breadcrumb from the given XML element.
		/// </summary>
		/// <param name="xmlElement"></param>
		public void InitFromXmlElement(NXmlElement xmlElement)
		{
			Clear();

			NButton homeButton = NButton.CreateImageAndText(NResources.Image_ExamplesUI_Icons_HomeLight_png,
				NExamplesUi.ProcessHeaderText("Home"));
			NStylePropertyEx.SetFlatExtendedLook(homeButton);
			Add(homeButton);

			if (xmlElement == null)
				return;

			Add(new NCommandBarSeparator());

			Add(CreateCategoriesDropDown(xmlElement));
			Add(new NLabel(ItemSeparator));

			NDeque<NXmlElement> deque = NExamplesUi.GetExampleElementsPath(xmlElement);
			for (int i = 1; i < deque.Count; i++)
			{
				NXmlElement currentXmlElement = deque[i];
				string name = currentXmlElement.GetAttributeValue("name");
				name = NExamplesUi.ProcessHeaderText(name);

				NButton button = new NButton(name);
				NStylePropertyEx.SetFlatExtendedLook(button);
				button.Tag = currentXmlElement;
				Add(button);

				if (i < deque.Count - 1)
				{
					Add(new NLabel(ItemSeparator));
				}
			}
		}

		#endregion

		#region Implementation

		private NMenuDropDown CreateCategoriesDropDown(NXmlElement xmlElement)
		{
			if (CategoryElements == null)
			{
				CategoryElements = GetCategoryElements(xmlElement);
			}

			NXmlElement categoryElement = (NXmlElement)xmlElement.GetFirstAncestor(NExamplesXml.Element.Category);
			string categoryName = NExamplesXml.GetName(categoryElement);

			NMenuDropDown menuDropDown = new NMenuDropDown(categoryName);
			NStylePropertyEx.SetExtendedLook(menuDropDown, ENExtendedLook.Flat);

			for (int i = 0; i < CategoryElements.Length; i++)
			{
				categoryElement = CategoryElements[i];
				string curCategoryName = NExamplesXml.GetName(categoryElement);

				NMenuItem menuItem = new NMenuItem(curCategoryName);
				menuItem.Tag = categoryElement;
				menuItem.Click += OnButtonOrMenuItemClick;
				menuDropDown.Items.Add(menuItem);
			}

			return menuDropDown;
		}

		#endregion

		#region Event Handlers

		private void OnButtonOrMenuItemClick(NEventArgs arg)
		{
			if (ButtonClick != null)
			{
				ButtonClick(arg);
			}
		}

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleBreadcrumb.
		/// </summary>
		public static readonly NSchema NExampleBreadcrumbSchema;

		#endregion

		#region Static Methods

		private static NXmlElement[] GetCategoryElements(NXmlElement xmlElement)
		{
			// Load the categories
			NXmlElement categoriesElement = (NXmlElement)xmlElement.GetFirstAncestor(NExamplesXml.Element.Categories);
			NList<NXmlNode> categories = categoriesElement.GetChildren(NExamplesXml.Element.Category);
			return categories.ToArray<NXmlElement>();
		}

		#endregion

		#region Static Event Handlers

		/// <summary>
		/// Handler for button click events.
		/// </summary>
		/// <param name="args"></param>
		private static void ButtonClickHandler(NEventArgs arg)
		{
			NExampleBreadcrumb exampleBreadcrumb = (NExampleBreadcrumb)arg.CurrentTargetNode;
			exampleBreadcrumb.OnButtonOrMenuItemClick(arg);
		}

		#endregion

		#region Constants

		/// <summary>
		/// The breadcrumb item separator.
		/// </summary>
		private const string ItemSeparator = ">";
		//private const string ItemSeparator = "\u2022";

		private static NXmlElement[] CategoryElements = null;

		#endregion
	}
}