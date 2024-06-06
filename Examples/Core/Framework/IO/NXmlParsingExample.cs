using System;
using System.IO;
using System.Text;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.IO;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples.Framework
{
	public class NXmlParsingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NXmlParsingExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NXmlParsingExample()
		{
			NXmlParsingExampleSchema = NSchema.Create(typeof(NXmlParsingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NSplitter splitter = new NSplitter();
			splitter.SplitMode = ENSplitterSplitMode.Proportional;
			splitter.SplitFactor = 0.5;

			// Create the "XML content" group box
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.First;
			stack.FillMode = ENStackFillMode.First;
			stack.VerticalSpacing = NDesign.VerticalSpacing;

			m_XmlTextBox = new NTextBox();
			m_XmlTextBox.AcceptsEnter = true;
			m_XmlTextBox.AcceptsTab = true;
			m_XmlTextBox.Multiline = true;
			m_XmlTextBox.WordWrap = false;
			m_XmlTextBox.VScrollMode = ENScrollMode.WhenNeeded;
			m_XmlTextBox.HScrollMode = ENScrollMode.WhenNeeded;
			m_XmlTextBox.Text = SampleXml;
			stack.Add(m_XmlTextBox);

			NButton parseButton = new NButton("Parse");
			parseButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			parseButton.Click += OnParseButtonClick;
			stack.Add(parseButton);

			splitter.Pane1.Content = new NGroupBox("XML Content", stack);

			// Create the "DOM tree" group box
			m_DomTree = new NTreeView();
			splitter.Pane2.Content = new NGroupBox("DOM Tree",  m_DomTree);

			return splitter;
		}
		protected override NWidget CreateExampleControls()
		{
			NButton openFileButton = new NButton("Open File");
			openFileButton.Click += OnOpenFileButtonClick;
			openFileButton.VerticalPlacement = ENVerticalPlacement.Top;

			return openFileButton;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates the XML parsing engine provided by Nevron Open Vision. Edit the XML content and when ready click
	the <b>Parse</b> button to trigger the parsing of the XML content. You will see the resulting DOM tree on the right. You
	can also load a XML file for parsing by clicking the <b>Open File</b> button on the right.
</p>
";
		}

		#endregion

		#region Implementation - Parsing

		private void Parse(object data)
		{
			// Parse the content of the text box
			NXmlDocument document = null;
			if (data is string)
			{
				// Method 1 - use a parser and a listener
				NXmlDocumentParserListener listener = new NXmlDocumentParserListener();
				NXmlParser parser = new NXmlParser(listener);
				parser.Parse(((string)data).ToCharArray());
				document = listener.Document;
			}
			else if (data is Stream)
			{
				// Method 2 - call the static Load method of the XML document class
				document = NXmlDocument.LoadFromStream((Stream)data);
			}
			else
			{
				throw new ArgumentException("Unsupported data type", "data");
			}

			// Populate the DOM tree view
			m_DomTree.SelectedItem = null;
			m_DomTree.Items.Clear();
			m_DomTree.Items.Add(CreateTreeViewItem(document));
			
			// Expand all items up to the second level
			ExpandTreeViewItems(m_DomTree.Items[0], 2, 1);
		}

		#endregion

		#region Event Handlers

		private void OnParseButtonClick(NEventArgs arg)
		{
			Parse(m_XmlTextBox.Text);
		}
		private void OnOpenFileButtonClick(NEventArgs arg)
		{
			NOpenFileDialog openFileDialog = new NOpenFileDialog();
			openFileDialog.FileTypes = new NFileDialogFileType[]{ new NFileDialogFileType("Xml Files (*.xml)", "xml") };
			openFileDialog.Closed += OnOpenFileDialogClosed;
			openFileDialog.RequestShow();
		}
		private void OnOpenFileDialogClosed(NOpenFileDialogResult arg)
		{
			if (arg.Result != ENCommonDialogResult.OK)
				return;

			arg.Files[0].OpenReadAsync().Then(
				delegate (Stream stream)
				{
					using (stream)
					{
						m_XmlTextBox.Text = NStreamHelpers.ReadToEndAsString(stream);
						stream.Position = 0;
						Parse(stream);
					}
				},
				delegate (Exception ex)
				{
					NMessageBox.ShowError(ex.Message, "Error");
				}
			);
		}

		#endregion

		#region Fields

		private NTextBox m_XmlTextBox;
		private NTreeView m_DomTree;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NXmlParsingExample.
		/// </summary>
		public static readonly NSchema NXmlParsingExampleSchema;

		#endregion

		#region Static Methods

		private static NTreeViewItem CreateTreeViewItem(NXmlNode node)
		{
			// Create a tree view item for the current XML node
			NTreeViewItem item;
			switch (node.NodeType)
			{
				case ENXmlNodeType.CDATA:
				case ENXmlNodeType.Comment:
				case ENXmlNodeType.Document:
					item = new NTreeViewItem(node.Name);
					break;
				case ENXmlNodeType.Declaration:
				case ENXmlNodeType.Element:
					string text = node.Name;
					NXmlElement element = (NXmlElement)node;
					INIterator<NKeyValuePair<string, string>> attributesIter = element.GetAttributesIterator();
					if (attributesIter != null)
					{
						// Append the element attributes
						StringBuilder sb = new StringBuilder(text);
						while (attributesIter.MoveNext())
						{
							sb.Append(" ");
							sb.Append(attributesIter.Current.Key);
							sb.Append("=\"");
							sb.Append(attributesIter.Current.Value);
							sb.Append("\"");
						}

						text = sb.ToString();
					}

					item = new NTreeViewItem(text);
					break;
				case ENXmlNodeType.Text:
					item = new NTreeViewItem("Text: \"" + ((NXmlTextNode)node).Text + "\"");
					break;
				default:
					throw new Exception("New ENXmlNodeType?");
			}

			// Traverse the node's children and create a child item for each of them
			INIterator<NXmlNode> iter = node.GetChildNodesIterator();
			if (iter != null)
			{
				while (iter.MoveNext())
				{
					NTreeViewItem childItem = CreateTreeViewItem(iter.Current);
					item.Items.Add(childItem);
				}
			}

			// Return the created tree view item
			return item;
		}
		private static void ExpandTreeViewItems(NTreeViewItem item, int levelsToExand, int currentLevel)
		{
			// Expand the current item
			item.Expanded = true;

			// If the desired number of levels has been expanded, quit
			if (currentLevel == levelsToExand)
				return;

			// Expand the child items of the current item
			currentLevel++;
			for (int i = 0, count = item.Items.Count; i < count; i++)
			{
				ExpandTreeViewItems(item.Items[i], levelsToExand, currentLevel);
			}
		}

		#endregion

		#region Constants

		private const string SampleXml = @"
<?xml version=""1.0""?>
<catalog>
   <book id=""bk101"">
      <author>Gambardella, Matthew</author>
      <title>XML Developer's Guide</title>
      <genre>Computer</genre>
      <price>44.95</price>
      <publish_date>2000-10-01</publish_date>
      <description>An in-depth look at creating applications 
      with XML.</description>
   </book>
   <book id=""bk102"">
      <author>Ralls, Kim</author>
      <title>Midnight Rain</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2000-12-16</publish_date>
      <description>A former architect battles corporate zombies, 
      an evil sorceress, and her own childhood to become queen 
      of the world.</description>
   </book>
   <book id=""bk103"">
      <author>Corets, Eva</author>
      <title>Maeve Ascendant</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2000-11-17</publish_date>
      <description>After the collapse of a nanotechnology 
      society in England, the young survivors lay the 
      foundation for a new society.</description>
   </book>
   <book id=""bk104"">
      <author>Corets, Eva</author>
      <title>Oberon's Legacy</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2001-03-10</publish_date>
      <description>In post-apocalypse England, the mysterious 
      agent known only as Oberon helps to create a new life 
      for the inhabitants of London. Sequel to Maeve 
      Ascendant.</description>
   </book>
   <book id=""bk105"">
      <author>Corets, Eva</author>
      <title>The Sundered Grail</title>
      <genre>Fantasy</genre>
      <price>5.95</price>
      <publish_date>2001-09-10</publish_date>
      <description>The two daughters of Maeve, half-sisters, 
      battle one another for control of England. Sequel to 
      Oberon's Legacy.</description>
   </book>
   <book id=""bk106"">
      <author>Randall, Cynthia</author>
      <title>Lover Birds</title>
      <genre>Romance</genre>
      <price>4.95</price>
      <publish_date>2000-09-02</publish_date>
      <description>When Carla meets Paul at an ornithology 
      conference, tempers fly as feathers get ruffled.</description>
   </book>
   <book id=""bk107"">
      <author>Thurman, Paula</author>
      <title>Splish Splash</title>
      <genre>Romance</genre>
      <price>4.95</price>
      <publish_date>2000-11-02</publish_date>
      <description>A deep sea diver finds true love twenty 
      thousand leagues beneath the sea.</description>
   </book>
   <book id=""bk108"">
      <author>Knorr, Stefan</author>
      <title>Creepy Crawlies</title>
      <genre>Horror</genre>
      <price>4.95</price>
      <publish_date>2000-12-06</publish_date>
      <description>An anthology of horror stories about roaches,
      centipedes, scorpions  and other insects.</description>
   </book>
   <book id=""bk109"">
      <author>Kress, Peter</author>
      <title>Paradox Lost</title>
      <genre>Science Fiction</genre>
      <price>6.95</price>
      <publish_date>2000-11-02</publish_date>
      <description>After an inadvertant trip through a Heisenberg
      Uncertainty Device, James Salway discovers the problems 
      of being quantum.</description>
   </book>
   <book id=""bk110"">
      <author>O'Brien, Tim</author>
      <title>Microsoft .NET: The Programming Bible</title>
      <genre>Computer</genre>
      <price>36.95</price>
      <publish_date>2000-12-09</publish_date>
      <description>Microsoft's .NET initiative is explored in 
      detail in this deep programmer's reference.</description>
   </book>
   <book id=""bk111"">
      <author>O'Brien, Tim</author>
      <title>MSXML3: A Comprehensive Guide</title>
      <genre>Computer</genre>
      <price>36.95</price>
      <publish_date>2000-12-01</publish_date>
      <description>The Microsoft MSXML3 parser is covered in 
      detail, with attention to XML DOM interfaces, XSLT processing, 
      SAX and more.</description>
   </book>
   <book id=""bk112"">
      <author>Galos, Mike</author>
      <title>Visual Studio 7: A Comprehensive Guide</title>
      <genre>Computer</genre>
      <price>49.95</price>
      <publish_date>2001-04-16</publish_date>
      <description>Microsoft Visual Studio 7 is explored in depth,
      looking at how Visual Basic, Visual C++, C#, and ASP+ are 
      integrated into a comprehensive development 
      environment.</description>
   </book>
</catalog>
";

		#endregion
	}
}