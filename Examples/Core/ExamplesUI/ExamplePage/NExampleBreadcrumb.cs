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
				NExamplesUiHelpers.ProcessHeaderText("Home"));
			NStylePropertyEx.SetFlatExtendedLook(homeButton);
			Add(homeButton);

			if (xmlElement == null)
				return;

			Add(new NCommandBarSeparator());

			NDeque<NXmlElement> deque = NExamplesUiHelpers.GetExampleElementsPath(xmlElement);
			for (int i = 0; i < deque.Count; i++)
			{
				NXmlElement currentXmlElement = deque[i];
				string name = currentXmlElement.GetAttributeValue("name");
				name = NExamplesUiHelpers.ProcessHeaderText(name);

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

		#region Event Handlers

		private void OnButtonClick(NEventArgs arg)
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

		#region Static Event Handlers

		/// <summary>
		/// Handler for button click events.
		/// </summary>
		/// <param name="args"></param>
		private static void ButtonClickHandler(NEventArgs arg)
		{
			NExampleBreadcrumb exampleBreadcrumb = (NExampleBreadcrumb)arg.CurrentTargetNode;
			exampleBreadcrumb.OnButtonClick(arg);
		}

		#endregion

		#region Constants

		/// <summary>
		/// The breadcrumb item separator.
		/// </summary>
		private const string ItemSeparator = ">";
		//private const string ItemSeparator = "\u2022";

		#endregion
	}
}