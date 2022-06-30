using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// A dock panel that hosts a header at the top, an accordion on the left, an example on the right.
	/// </summary>
	internal class NExamplePage : NDockPanel
	{
		#region Constructors

		/// <summary>
		/// Initializing constructor.
		/// </summary>
		/// <param name="searchBox"></param>
		public NExamplePage(NStringMap<NWidget> searchBoxMap)
		{
			CreateContent(searchBoxMap);
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExamplePage()
		{
			NExamplePageSchema = NSchema.Create(typeof(NExamplePage), NDockPanel.NDockPanelSchema);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets/Sets the path to the examples.
		/// </summary>
		public string ExamplesPath
		{
			get
			{
				return m_ExamplesPath;
			}
			set
			{
				m_ExamplesPath = value;
			}
		}

		/// <summary>
		/// Gets the current example path.
		/// </summary>
		internal string CurrentExamplePath
		{
			get
			{
				return NExamplesUiHelpers.GetExamplePath(m_CurrentExampleXmlElement);
			}
		}

		#endregion

		#region Public Methods

		public void UpdateFromOptions()
		{
			ENUIThemeScheme uiThemeScheme = NExamplesOptions.Instance.ThemeScheme;
			if ((NApplication.IntegrationPlatform == ENIntegrationPlatform.XamarinMac && uiThemeScheme != ENUIThemeScheme.MacElCapitan) ||
				(NApplication.IntegrationPlatform != ENIntegrationPlatform.XamarinMac && uiThemeScheme != ENUIThemeScheme.Windows10))
			{
				// The example options require a non-default theme for the current intergation platform, so create and apply it
				NApplication.ApplyTheme(NUITheme.CreateForScheme(uiThemeScheme));
			}

			NApplication.DeveloperMode = NExamplesOptions.Instance.DeveloperMode;
		}
		public void InitializeFromXml(NXmlDocument xmlDocument)
		{
			// Initialize the accoridion
			m_Accordion.InitializeFromXml(xmlDocument);
		}
		public void NavigateToExample(NXmlElement xmlElement)
		{
			// Navigate to the element
			NXmlElement exampleXmlElement = m_Accordion.NavigateToExample(xmlElement);

			// Update the breadcrumb
			m_HeaderLane2.Breadcrumb.InitFromXmlElement(exampleXmlElement);

			if (m_CurrentExampleXmlElement != exampleXmlElement)
			{
				// The tree view selected path change event has not been fired, so load the example
				LoadExample(exampleXmlElement);
			}
		}

		#endregion

		#region Implementation - UI

		private void CreateContent(NStringMap<NWidget> searchBoxMap)
		{
			// Create the example header
			NWidget header = CreateHeader(searchBoxMap);
			Add(header, ENDockArea.Top);

			// Create the example footer
			m_Footer = new NExampleFooter();
			Add(m_Footer, ENDockArea.Bottom);

			// Create the example splitter and accordion with tree views
			m_Splitter = new NSplitter();
			m_Splitter.SplitMode = ENSplitterSplitMode.OffsetFromNearSide;
			m_Splitter.SplitOffset = AccordionPaneWidth;
			Add(m_Splitter, ENDockArea.Center);

			// Create and initialize the Examples accordion
			m_Accordion = new NExamplesAccordion();
			m_Accordion.TreeViewSelectedPathChanged += OnTreeViewSelectedPathChanged;

			m_Splitter.Pane1.Content = m_Accordion;

			// Bind search box width to accordion width
			m_HeaderLane2.SearchBox.SetFx(NWidget.PreferredWidthProperty, new NBindingFx(m_Accordion, NWidget.WidthProperty));
		}
		private NWidget CreateHeader(NStringMap<NWidget> searchBoxMap)
		{
			// Create the first header lane
			m_HeaderLane1 = new NExampleHeaderLane1(searchBoxMap);
			m_HeaderLane1.ExampleMenuItemClick += OnExampleMenuItemClick;

			// Create the second header lane
			m_HeaderLane2 = new NExampleHeaderLane2();
			m_HeaderLane2.SearchBox.InitAutoComplete(searchBoxMap, new NExampleTileFactory());
			m_HeaderLane2.SearchBoxItemSelected += OnSearchBoxItemSelected;
			m_HeaderLane2.FavoriteAddedOrRemoved += OnFavoriteAddedOrRemoved;
			m_HeaderLane2.BreadcrumbButtonClick += OnBreadcrumbButtonClick;
			m_HeaderLane2.PreviousExampleButtonClick += OnPreviousExampleButtonClick;
			m_HeaderLane2.NextExampleButtonClick += OnNextExampleButtonClick;

			NPairBox pairBox = new NPairBox(m_HeaderLane1, m_HeaderLane2, ENPairBoxRelation.Box1AboveBox2);
			pairBox.Spacing = 0;
			return pairBox;
		}

		#endregion

		#region Implementation - Example Loading

		private void LoadExample(NXmlElement xmlElement)
		{
			string groupNamespace = NHomePage.GetNamespace(xmlElement);
			string name = xmlElement.GetAttributeValue("name");
			string type = groupNamespace + "." + xmlElement.GetAttributeValue("type");
			string examplePath = NExamplesUiHelpers.GetExamplePath(xmlElement);

			// Set example title
			m_HeaderLane2.Title = NExamplesUiHelpers.ProcessHeaderText(name);
			m_HeaderLane2.UpdateFavoriteButton(NExamplesOptions.Instance.FavoriteExamples.Contains(examplePath));

			try
			{
				type = "Nevron.Nov.Examples." + type;
				Type exampleType = Type.GetType(type);
				if (exampleType != null)
				{
					NDomType domType = NDomType.FromType(exampleType);
					NDebug.Assert(domType != null, "The example type:" + type + " is not a valid type");

					// Create the example
					DateTime start = DateTime.Now;
					NExampleBase example = domType.CreateInstance() as NExampleBase;
					example.Title = name;
					example.Initialize();
					m_Splitter.Pane2.Content = example;

					// Evaluate the example
					string stats = "Example created in: " + (DateTime.Now - start).TotalSeconds + " seconds, ";
					start = DateTime.Now;
					OwnerDocument.Evaluate();
					stats += " evaluated in: " + (DateTime.Now - start).TotalSeconds + " seconds";
					NDebug.WriteLine(stats);

					m_CurrentExampleXmlElement = xmlElement;

					// Add the recent example
					NExamplesOptions.Instance.AddRecentExample(examplePath);
				}
			}
			catch (Exception ex)
			{
				NTrace.WriteException("Failed to load example", ex);
				m_Splitter.Pane2.Content = new NErrorPanel("Failed to load example. Exception was: " + ex.Message);
			}
		}
		private void CloseExample()
		{
			NExampleBase oldExample = m_Splitter.Pane2.Content as NExampleBase;
			if (oldExample != null)
			{
				// Notify the old example that it is about to be closed
				oldExample.OnClosing();
			}

			m_Splitter.Pane2.Content = null;
		}

		#endregion

		#region Event Handlers - Header

		private void OnExampleMenuItemClick(NXmlElement exampleXmlElement)
		{
			NavigateToExample(exampleXmlElement);
		}
		private void OnFavoriteAddedOrRemoved(bool favoriteAdded)
		{
			if (favoriteAdded)
			{
				NExamplesOptions.Instance.AddFavoriteExample(CurrentExamplePath);
			}
			else
			{
				NExamplesOptions.Instance.RemoveFavoriteExample(CurrentExamplePath);
			}
		}
		private void OnNextExampleButtonClick(NEventArgs arg)
		{
			NXmlElement current = m_CurrentExampleXmlElement;
			NXmlElement next = current.GetNextSibling(ENXmlNodeType.Element) as NXmlElement;

			while (next == null && current.Parent.Name != "categories")
			{
				NXmlElement parent = current.Parent as NXmlElement;
				NXmlElement nextParent = parent.GetNextSibling(ENXmlNodeType.Element) as NXmlElement;

				if (nextParent != null)
				{
					// Next parent is not null, so move to its first "example" descendant
					next = nextParent.GetFirstDescendant("example") as NXmlElement;
					if (next != null)
						break;
				}

				// Next parent is null or doesn't contain "example" elements, so move one level up
				current = parent;
			}

			if (next != null)
			{
				NavigateToExample(next);
			}
		}
		private void OnPreviousExampleButtonClick(NEventArgs arg)
		{
			NXmlElement current = m_CurrentExampleXmlElement;
			NXmlElement prev = current.GetPreviousSibling(ENXmlNodeType.Element) as NXmlElement;

			while (prev == null && current.Parent.Name != "categories")
			{
				NXmlElement parent = current.Parent as NXmlElement;
				NXmlElement prevParent = parent.GetPreviousSibling(ENXmlNodeType.Element) as NXmlElement;

				if (prevParent != null)
				{
					// Previous parent is not null, so move to its last "example" descendant
					prev = prevParent.GetLastDescendant("example") as NXmlElement;
					if (prev != null)
						break;
				}

				// Previous parent is null or doesn't contain "example" elements, so move one level up
				current = parent;
			}

			if (prev != null)
			{
				NavigateToExample(prev);
			}
		}

		#endregion

		#region Event Handlers - Navigation

		private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
		{
			// Close the old example
			CloseExample();

			// Load the new example
			NTreeViewItem selectedItem = ((NTreeView)arg.TargetNode).SelectedItem;
			if (selectedItem != null)
			{
				NXmlElement xmlElement = selectedItem.Tag as NXmlElement;
				if (xmlElement != null)
				{
					m_HeaderLane2.Breadcrumb.InitFromXmlElement(xmlElement);
					LoadExample(xmlElement);
				}
			}
		}
		private void OnBreadcrumbButtonClick(NEventArgs arg)
		{
			// Load the new example
			NXmlElement xmlElement = (NXmlElement)arg.TargetNode.Tag;
			if (xmlElement != null)
			{
				// An example path button was clicked
				NavigateToExample(xmlElement);
			}
			else
			{
				// The home button was clicked
				NExamplesContent examplesContent = (NExamplesContent)ParentNode;
				examplesContent.NavigateToHomePageWelcomeScreen();
			}
		}
		private void OnSearchBoxItemSelected(NEventArgs arg)
		{
			if (arg.Cancel)
				return;

			INSearchableListBox listBox = (INSearchableListBox)arg.TargetNode;
			NWidget selectedItem = ((NKeyValuePair<string, NWidget>)listBox.GetSelectedItem()).Value;

			if (selectedItem != null)
			{
				// Clear the search box text
				m_HeaderLane2.SearchBox.Text = null;

				// Navigate to the selected example
				NExampleTileInfo tileInfo = (NExampleTileInfo)selectedItem.Tag;
				NavigateToExample(tileInfo.XmlElement);
			}

			// Mark the event as handled
			arg.Cancel = true;
		}

		#endregion

		#region Fields

		private NExampleHeaderLane1 m_HeaderLane1;
		private NExampleHeaderLane2 m_HeaderLane2;
		private NSplitter m_Splitter;
		private NExamplesAccordion m_Accordion;
		private NExampleFooter m_Footer;

		private string m_ExamplesPath;
		private NXmlElement m_CurrentExampleXmlElement;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExamplePage.
		/// </summary>
		public static readonly NSchema NExamplePageSchema;

		#endregion

		#region Constants

		private const double AccordionPaneWidth = 260;

		#endregion
	}
}