using System;
using System.IO;

using Nevron.Nov.Dom;
using Nevron.Nov.Examples.ExamplesUI;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// The examples content.
	/// </summary>
	public class NExamplesContent : NContentHolder
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NExamplesContent()
		{
			// Load the examples XML
			NXmlDocument xmlDocument;
			using (MemoryStream memoryStream = new MemoryStream(NResources.RSTR_Examples_xml.Data))
			{
				xmlDocument = NXmlDocument.LoadFromStream(memoryStream);
			}

			// Create the Examples' home page
			m_HomePage = new NHomePage();
			m_HomePage.InitializeFromXml(xmlDocument);
			m_HomePage.ItemSelected += OnItemSelected;

			// Host it
			Content = m_HomePage;

			// Create the Example host page
			m_ExamplePage = new NExamplePage(m_HomePage.m_ExamplesMap);
			m_ExamplePage.InitializeFromXml(xmlDocument);
		}

		/// <summary>
		/// Static constructor
		/// </summary>
		static NExamplesContent()
		{
			NExamplesContentSchema = NSchema.Create(typeof(NExamplesContent), NContentHolderSchema);
		}

		#endregion

		#region Properties

        /// <summary>
        /// Gets/Sets the examples link processor.
        /// </summary>
        public INExampleLinkProcessor LinkProcessor
        {
            get;
            set;
        }

		#endregion

		#region Public Methods - Navigation

		/// <summary>
		/// Navigates to the welcome screen of examples home page.
		/// </summary>
		public void NavigateToHomePageWelcomeScreen()
		{
			// Show the home page
			Content = m_HomePage;

			// Clear the text of the search box
			m_HomePage.Header.m_SearchBox.Text = null;
		}
		/// <summary>
		/// Navigates to the given category of the examples home page.
		/// </summary>
		/// <param name="categoryName"></param>
		public void NavigateToHomePageCategory(string categoryName)
		{
			// Show the home page
			m_HomePage.Content.NavigateToCategory(categoryName);
			Content = m_HomePage;

			// Clear the text of the search box
			m_HomePage.Header.m_SearchBox.Text = null;
		}

        /// <summary>
        /// Navigates to the given example URI string.
        /// </summary>
        /// <param name="uri"></param>
        /// <remarks>
        /// Used by the WinForms, WPF and WebAssembly examples to navigate to a specific example from a custom URL like:
        /// <para>nov-winforms://NDiagramDesignerExample</para>
        /// <para>nov-wpf://NDiagramDesignerExample</para>
        /// </remarks>
        public void NavigateToExampleUri(string uriString)
        {
            string exampleTypeName = LinkProcessor.GetExampleType(uriString);
            if (exampleTypeName != null)
            {
                NavigateToExample(exampleTypeName);
            }
        }
        /// <summary>
        /// Navigates to the given example.
        /// </summary>
        /// <param name="exampleTypeName">Example type name, for example: "NDiagramDesignerExample".</param>
        public void NavigateToExample(string exampleTypeName)
		{
			NXmlDocument document;
			using (Stream stream = NResources.Instance.GetResourceStream("RSTR_Examples_xml"))
			{
				document = NXmlDocument.LoadFromStream(stream);				
			}

			// Find the XML element with the given example type
			// FIX: use map like in NExamplesAccordion
			NXmlElement xmlElement = GetExampleElement(document, exampleTypeName);
			if (xmlElement != null)
			{
				NavigateToExample(xmlElement);
			}
		}
		/// <summary>
		/// Navigates to the given example XML element.
		/// </summary>
		/// <param name="xmlElement"></param>
		public void NavigateToExample(NXmlElement xmlElement)
		{
			if (Content != m_ExamplePage)
			{
				Content = m_ExamplePage;
				m_ExamplePage.NavigateToExample(xmlElement);
			}
		}

		#endregion

		#region Protected Overrides - Examples Lifetime

		/// <summary>
		/// Called when the example content has been created, which is on Examples' application launch.
		/// </summary>
		protected override void OnRegistered()
		{
			base.OnRegistered();

			// Load examples options
			NExamplesOptions.Instance.LoadAsync().Then(
				delegate (NUndefined ud)
				{
					// Example options has been loaded
					m_ExamplePage.UpdateFromOptions();
				}
			);
		}

		#endregion

		#region Event Handles - Navigation

		private void OnItemSelected(NXmlElement element)
		{
			NavigateToExample(element);
		}

		#endregion

		#region Fields

		internal NHomePage m_HomePage;
		private NExamplePage m_ExamplePage;

		#endregion

		#region Schema

		public static readonly NSchema NExamplesContentSchema;

		#endregion

		#region Static Methods

		private static NXmlElement GetExampleElement(NXmlNode node, string exampleTypeName)
		{
			NXmlElement element = node as NXmlElement;
			if (element != null && element.Name == "example" &&
                String.Equals(element.GetAttributeValue("type"), exampleTypeName, StringComparison.OrdinalIgnoreCase))
				return element;

			for (int i = 0, count = node.ChildrenCount; i < count; i++)
			{
				element = GetExampleElement(node.GetChildAt(i), exampleTypeName);
				if (element != null)
					return element;
			}

			return null;
		}

		#endregion
	}
}