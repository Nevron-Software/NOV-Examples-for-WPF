using System.IO;
using System.Text;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples.Framework
{
	public class NXmlSerializingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NXmlSerializingExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NXmlSerializingExample()
		{
			NXmlSerializingExampleSchema = NSchema.Create(typeof(NXmlSerializingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Properties

		private NTreeViewItem DocumentItem
		{
			get
			{
				return m_TreeView.Items[0];
			}
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NSplitter splitter = new NSplitter();
			splitter.SplitMode = ENSplitterSplitMode.Proportional;
			splitter.SplitFactor = 0.5;

			// Create the "Dom Tree" group box
			m_TreeView = CreateTreeView();
			m_TreeView.SelectedPathChanged += OnTreeViewSelectedPathChanged;

			NToolBar toolBar = new NToolBar();
			m_AddChildItemButton = CreateButton(NResources.Image_Add_png, "Add Child Item");
			m_AddChildItemButton.Click += OnAddChildItemButtonClick;
			toolBar.Items.Add(m_AddChildItemButton);

			m_RemoveSelectedItemButton = CreateButton(NResources.Image_Delete_png, "Remove Selected Item");
			m_RemoveSelectedItemButton.Click += OnRemoveSelectedItemButtonClick;
			toolBar.Items.Add(m_RemoveSelectedItemButton);

			toolBar.Items.Add(new NCommandBarSeparator());

			m_AddAttributeButton = CreateButton(NResources.Image_Add_png, "Add Attribute");
			m_AddAttributeButton.Click += OnAddAttributeButtonClick;
			toolBar.Items.Add(m_AddAttributeButton);

			m_RemoveAttributeButton = CreateButton(NResources.Image_Delete_png, "Remove Attribute");
			m_RemoveAttributeButton.Click += OnRemoveAttributeButtonClick;
			toolBar.Items.Add(m_RemoveAttributeButton);

			toolBar.Items.Add(new NCommandBarSeparator());

			m_SerializeButton = CreateButton(NResources.Image__16x16_Contacts_png, "Serialize");
			m_SerializeButton.Enabled = true;
			m_SerializeButton.Click += OnSerializeButtonClick;
			toolBar.Items.Add(m_SerializeButton);

			NPairBox pairBox = new NPairBox(m_TreeView, toolBar, ENPairBoxRelation.Box1AboveBox2);
			pairBox.FillMode = ENStackFillMode.First;
			pairBox.FitMode = ENStackFitMode.First;
			pairBox.Spacing = NDesign.VerticalSpacing;
			splitter.Pane1.Content = pairBox;

			// Create the "XML output" group box
			m_XmlTextBox = new NTextBox();
			m_XmlTextBox.AcceptsEnter = true;
			m_XmlTextBox.AcceptsTab = true;
			m_XmlTextBox.Multiline = true;
			m_XmlTextBox.WordWrap = false;
			m_XmlTextBox.VScrollMode = ENScrollMode.WhenNeeded;
			m_XmlTextBox.HScrollMode = ENScrollMode.WhenNeeded;
			splitter.Pane2.Content = m_XmlTextBox;

			// Select the "Document" tree view item
			m_TreeView.SelectedItem = DocumentItem;

			return splitter;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and serialize XML documents with Nevron Open Vision. Use the buttons below the tree
	view to create a DOM tree and when ready click the <b>Serialize</b> button to construct a XML document from it and serialize
	it to the text box on the right.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnAddChildItemButtonClick(NEventArgs arg)
		{
			NTopLevelWindow dialog = NApplication.CreateTopLevelWindow(NWindow.GetFocusedWindowIfNull(DisplayWindow));
			dialog.SetupDialogWindow("Enter element's name", false);

			NTextBox textBox = new NTextBox();
			NButtonStrip buttonStrip = new NButtonStrip();
			buttonStrip.AddOKCancelButtons();

			NPairBox pairBox = new NPairBox(textBox, buttonStrip, ENPairBoxRelation.Box1AboveBox2);
			pairBox.Spacing = NDesign.VerticalSpacing;
			dialog.Content = pairBox;

			dialog.Opened += delegate(NEventArgs args) {
				textBox.Focus();
			};

			dialog.Closed += delegate(NEventArgs args) {
				if (dialog.Result == ENWindowResult.OK)
				{
					// Add an item with the specified name
					m_TreeView.SelectedItem.Items.Add(CreateTreeViewItem(textBox.Text));
					m_TreeView.SelectedItem.Expanded = true;

					if (m_SerializeButton.Enabled == false)
					{
						m_SerializeButton.Enabled = true;
					}
				}
			};

			dialog.Open();
		}
		private void OnRemoveSelectedItemButtonClick(NEventArgs arg)
		{
			NMessageBox.Show(
				NLoc.Get("Remove the selected tree view item"), 
				NLoc.Get("Question"), 
				ENMessageBoxButtons.YesNo, 
				ENMessageBoxIcon.Question).Then(delegate(ENWindowResult result) {
					if (result == ENWindowResult.Yes)
					{
						NTreeViewItem item = m_TreeView.SelectedItem;
						NTreeViewItem parentItem = item.ParentItem;
						m_TreeView.SelectedItem = null;
						parentItem.Items.Remove(item);
						m_TreeView.SelectedItem = parentItem;

						if (DocumentItem.Items.Count == 0)
						{
							m_SerializeButton.Enabled = false;
						}
					}
				}
			);
		}
		private void OnAddAttributeButtonClick(NEventArgs arg)
		{
			NTopLevelWindow dialog = NApplication.CreateTopLevelWindow();
			dialog.SetupDialogWindow("Enter attribute's name and value", false);

			NTableFlowPanel table = new NTableFlowPanel();
			table.Direction = ENHVDirection.LeftToRight;
			table.ColFillMode = ENStackFillMode.Last;
			table.ColFitMode = ENStackFitMode.Last;
			table.MaxOrdinal = 2;

			NLabel nameLabel = new NLabel("Name:");
			table.Add(nameLabel);

			NTextBox nameTextBox = new NTextBox();
			table.Add(nameTextBox);

			NLabel valueLabel = new NLabel("Value:");
			table.Add(valueLabel);

			NTextBox valueTextBox = new NTextBox();
			table.Add(valueTextBox);

			table.Add(new NWidget());

			NButtonStrip buttonStrip = new NButtonStrip();
			buttonStrip.AddOKCancelButtons();
			table.Add(buttonStrip);

			dialog.Content = table;

			dialog.Opened += delegate(NEventArgs args) {
				nameTextBox.Focus();
			};

			dialog.Closed += delegate(NEventArgs args) {
				if (dialog.Result == ENWindowResult.OK)
				{
					NElementInfo elementInfo = (NElementInfo)m_TreeView.SelectedItem.Tag;
					elementInfo.Attributes.Set(nameTextBox.Text, valueTextBox.Text);
					UpdateTreeViewItemText(m_TreeView.SelectedItem);

					if (m_RemoveAttributeButton.Enabled == false)
					{
						m_RemoveAttributeButton.Enabled = true;
					}
				}
			};

			dialog.Open();

		}
		private void OnRemoveAttributeButtonClick(NEventArgs arg)
		{
			NTopLevelWindow dialog = NApplication.CreateTopLevelWindow();
			dialog.SetupDialogWindow("Select an Attribute to Remove", false);

			NListBox listBox = new NListBox();
			NElementInfo elementInfo = (NElementInfo)m_TreeView.SelectedItem.Tag;
			INIterator<NKeyValuePair<string, string>> iter = elementInfo.Attributes.GetIterator();

			while (iter.MoveNext())
			{
				listBox.Items.Add(new NListBoxItem(iter.Current.Key));
			}

			NButtonStrip buttonStrip = new NButtonStrip();
			buttonStrip.AddOKCancelButtons();

			NPairBox pairBox = new NPairBox(listBox, buttonStrip, ENPairBoxRelation.Box1AboveBox2);
			pairBox.Spacing = NDesign.VerticalSpacing;
			dialog.Content = pairBox;

			dialog.Opened += delegate(NEventArgs args) {
				listBox.Focus();
			};

			dialog.Closed += delegate(NEventArgs args) {
				if (dialog.Result == ENWindowResult.OK)
				{
					// Remove the selected attribute
					NListBoxItem selectedItem = listBox.Selection.FirstSelected;
					if (selectedItem != null)
					{
						string name = ((NLabel)selectedItem.Content).Text;
						elementInfo.Attributes.Remove(name);
						UpdateTreeViewItemText(m_TreeView.SelectedItem);

						if (elementInfo.Attributes.Count == 0)
						{
							m_RemoveAttributeButton.Enabled = false;
						}
					}
				}
			};

			dialog.Open();
		}
		private void OnSerializeButtonClick(NEventArgs arg)
		{
			// Create the XML document
			NXmlDocument document = new NXmlDocument();
			NTreeViewItem documentItem = DocumentItem;
			for (int i = 0, childCount = documentItem.Items.Count; i < childCount; i++)
			{
				document.AddChild(SerializeTreeViewItem(documentItem.Items[i]));
			}

			// Serialize the document to the XML text box
			using (MemoryStream stream = new MemoryStream())
			{
				// Serialize the document to a memory stream
				document.SaveToStream(stream, NEncoding.UTF8);

				// Populate the XML text box from the memory stream
				byte[] data = stream.ToArray();
				m_XmlTextBox.Text = NEncoding.UTF8.GetString(data);
			}
		}
		private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs arg)
		{
			NTreeViewItem selectItem = m_TreeView.SelectedItem;
			if (selectItem == null)
			{
				m_AddChildItemButton.Enabled = false;
				m_RemoveSelectedItemButton.Enabled = false;
				m_AddAttributeButton.Enabled = false;
				m_RemoveAttributeButton.Enabled = false;
				return;
			}

			m_AddChildItemButton.Enabled = true;
			m_RemoveSelectedItemButton.Enabled = selectItem != DocumentItem;

			NElementInfo elementInfo = selectItem.Tag as NElementInfo;
			m_AddAttributeButton.Enabled = elementInfo != null;
			m_RemoveAttributeButton.Enabled = elementInfo != null && elementInfo.Attributes.Count > 0;
		}

		#endregion

		#region Fields

		private NTreeView m_TreeView;
		private NTextBox m_XmlTextBox;

		private NButton m_AddChildItemButton;
		private NButton m_RemoveSelectedItemButton;
		private NButton m_AddAttributeButton;
		private NButton m_RemoveAttributeButton;
		private NButton m_SerializeButton;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NXmlSerializingExample.
		/// </summary>
		public static readonly NSchema NXmlSerializingExampleSchema;

		#endregion

		#region Static Methods

		private static NButton CreateButton(NImage image, string text)
		{
			NPairBox pairBox = new NPairBox(image, text);
			pairBox.Box1.VerticalPlacement = ENVerticalPlacement.Center;
			pairBox.Box2.VerticalPlacement = ENVerticalPlacement.Center;
			pairBox.Spacing = NDesign.VerticalSpacing;
			
			NButton button = new NButton(pairBox);
			button.Enabled = false;
			return button;
		}
		private static NTreeViewItem CreateTreeViewItem(string name)
		{
			return CreateTreeViewItem(name, null);
		}
		private static NTreeViewItem CreateTreeViewItem(string name, string value)
		{
			NTreeViewItem item = new NTreeViewItem(name);
			item.Tag = new NElementInfo(name);
			if (value != null)
			{
				item.Items.Add(new NTreeViewItem(value));
			}

			return item;
		}
		private static NXmlNode SerializeTreeViewItem(NTreeViewItem item)
		{
			NElementInfo elementInfo = (NElementInfo)item.Tag;
			if (elementInfo == null)
			{
				string text = ((NLabel)item.Header.Content).Text;
				return new NXmlTextNode(ENXmlNodeType.Text, text);
			}

			// Create an XML element for the current tree view item
			NXmlElement element = new NXmlElement(elementInfo.Name);
			if (elementInfo.Attributes.Count > 0)
			{
				// Set the element's attributes
				INIterator<NKeyValuePair<string, string>> iter = elementInfo.Attributes.GetIterator();
				while (iter.MoveNext())
				{
					element.SetAttribute(iter.Current.Key, iter.Current.Value);
				}
			}

			// Loop through the item's children
			for (int i = 0, childCount = item.Items.Count; i < childCount; i++)
			{
				element.AddChild(SerializeTreeViewItem(item.Items[i]));
			}

			return element;
		}
		private static void UpdateTreeViewItemText(NTreeViewItem item)
		{
			NElementInfo elementInfo = item.Tag as NElementInfo;
			if (elementInfo == null)
				return;

			string text = elementInfo.Name;
			if (elementInfo.Attributes.Count > 0)
			{
				// Iterate through the attributes and append them to the text
				StringBuilder sb = new StringBuilder(text);
				INIterator<NKeyValuePair<string, string>> iter = elementInfo.Attributes.GetIterator();
				while (iter.MoveNext())
				{
					sb.Append(" ");
					sb.Append(iter.Current.Key);
					sb.Append("=\"");
					sb.Append(iter.Current.Value);
					sb.Append("\"");
				}

				text = sb.ToString();
			}

			// Update the text of the given tree view item
			((NLabel)item.Header.Content).Text = text;
		}
		private static NTreeView CreateTreeView()
		{
			NTreeView treeView = new NTreeView();

			NTreeViewItem root = CreateTreeViewItem("Document");
			root.Expanded = true;
			treeView.Items.Add(root);

			NTreeViewItem book1 = CreateTreeViewItem("book");
			book1.Expanded = true;
			book1.Items.Add(CreateTreeViewItem("Author", "Gambardella, Matthew"));
			book1.Items.Add(CreateTreeViewItem("Title", "XML Developer's Guide"));
			root.Items.Add(book1);

			NTreeViewItem book2 = CreateTreeViewItem("book");
			book2.Expanded = true;
			book2.Items.Add(CreateTreeViewItem("Author", "O'Brien, Tim"));
			book2.Items.Add(CreateTreeViewItem("Title", "MSXML3: A Comprehensive Guide"));
			root.Items.Add(book2);

			return treeView;
		}

		#endregion

		#region Nested Types

		private class NElementInfo
		{
			public NElementInfo(string name)
			{
				Name = name;
				Attributes = new NMap<string, string>();
			}

			public string Name;
			public NMap<string, string> Attributes;
		}

		#endregion
	}
}