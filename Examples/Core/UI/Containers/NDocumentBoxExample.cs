using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NDocumentBoxExample : NExampleBase
	{
		#region Constructors

		public NDocumentBoxExample()
		{
		}
		static NDocumentBoxExample()
		{
			NDocumentBoxExampleSchema = NSchema.Create(typeof(NDocumentBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Example

		protected override NWidget CreateExampleContent()
		{
			NTableFlowPanel table = new NTableFlowPanel();
			table.HorizontalSpacing = 10;
			table.VerticalSpacing = 10;
			table.MaxOrdinal = 3;

			table.Add(CreateGroupBox("Buttons", CreateButtons()));
			table.Add(CreateGroupBox("List Box", CreateListBox()));
			table.Add(CreateGroupBox("Tree View", CreateTreeView()));
			table.Add(CreateGroupBox("Drop Down Edits", CreateDropDownEdits()));
			table.Add(CreateGroupBox("Tab", CreateTab()));
			table.Add(CreateGroupBox("Range Based", CreateRangeBased()));

			// Create the document box
			m_DocumentBox = new NDocumentBox();
			m_DocumentBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_DocumentBox.Border = NBorder.CreateFilledBorder(NColor.Red);
			m_DocumentBox.BorderThickness = new NMargins(1);
			m_DocumentBox.Surface = new NDocumentBoxSurface(table);

			return m_DocumentBox;
		}
		protected override NWidget CreateExampleControls()
		{
			// Create the theme tree view
			NTheme theme;
			NTreeViewItem rootItem, themeItem;
			NTreeView themeTreeView = new NTreeView();

			// Add the "Inherit Styles" root tree view item
			rootItem = new NTreeViewItem("Inherit Styles");
			rootItem.Tag = "inherit";
			themeTreeView.Items.Add(rootItem);
			themeTreeView.SelectedItem = rootItem;

			// Add the image based UI themes to the tree view
			rootItem = new NTreeViewItem("Part Based");
			rootItem.Expanded = true;
			themeTreeView.Items.Add(rootItem);

            themeItem = new NTreeViewItem("Windows XP Blue");
            themeItem.Tag = new NWindowsXPBlueTheme();
            rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Windows Aero");
			themeItem.Tag = new NWindowsAeroTheme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Windows 8");
			themeItem.Tag = new NWindows8Theme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Mac OS X 10.7 Lion");
			themeItem.Tag = new NMacLionTheme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Mac OS X 10.11 El Capitan");
			themeItem.Tag = new NMacElCapitanTheme();
			rootItem.Items.Add(themeItem);
						
			themeItem = new NTreeViewItem("Nevron Dark");
			themeItem.Tag = new NNevronDarkTheme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Nevron Light");
			themeItem.Tag = new NNevronLightTheme();
			rootItem.Items.Add(themeItem);

			// Add the windows classic UI themes to the tree view
			rootItem = new NTreeViewItem("Windows Classic");
			rootItem.Expanded = true;
			themeTreeView.Items.Add(rootItem);			

			ENUIThemeScheme[] windowsClassicSchemes = NEnum.GetValues<ENUIThemeScheme>();
			for(int i = 0, count = windowsClassicSchemes.Length; i < count; i++)
			{
				ENUIThemeScheme scheme = windowsClassicSchemes[i];
				theme = new NWindowsClassicTheme(scheme);
				themeItem = new NTreeViewItem(NStringHelpers.InsertSpacesBeforeUppersAndDigits(scheme.ToString()));
				themeItem.Tag = theme;
				rootItem.Items.Add(themeItem);
			}

			// Subscribe to the selected path changed event
			themeTreeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnThemeTreeViewSelectedPathChanged);

			return themeTreeView;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to use document boxes. The document box is a widget that has a document.
	If you set the <b>InheritStyleSheetsProperty</b> of the document box's document to false, then the widgets hosted
	in it will not inherit the styling of the document the document box is placed in.
</p>
";
		}

		#endregion

		#region Implementation

		private NStackPanel CreateButtons()
		{
			// Create buttons
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = 10;

			NCheckBox checkBox = new NCheckBox("Check Box");
			stack.Add(checkBox);

			NRadioButton radioButton = new NRadioButton("Radio Button");
			stack.Add(radioButton);

			NButton button = new NButton("Button");
			stack.Add(button);

			return stack;
		}
		private NListBox CreateListBox()
		{
			NListBox listBox = new NListBox();
			for (int i = 1; i <= 20; i++)
			{
				listBox.Items.Add(new NListBoxItem("Item " + i));
			}

			return listBox;
		}
		private NTreeView CreateTreeView()
		{
			NTreeView treeView = new NTreeView();
			for (int i = 1; i <= 8; i++)
			{
				string itemName = "Item " + i;
				NTreeViewItem item = new NTreeViewItem(itemName);
				treeView.Items.Add(item);

				itemName += ".";
				for (int j = 1; j <= 3; j++)
				{
					NTreeViewItem childItem = new NTreeViewItem(itemName + j);
					item.Items.Add(childItem);
				}
			}

			return treeView;
		}
		private NStackPanel CreateDropDownEdits()
		{
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = 10;

			NComboBox comboBox = new NComboBox();
			comboBox.Items.Add(new NComboBoxItem("Item 1"));
			comboBox.Items.Add(new NComboBoxItem("Item 2"));
			comboBox.Items.Add(new NComboBoxItem("Item 3"));
			comboBox.Items.Add(new NComboBoxItem("Item 4"));
			comboBox.SelectedIndex = 0;
			stack.Add(comboBox);

			NColorBox colorBox = new NColorBox();
			stack.Add(colorBox);

			NDateTimeBox dateTimeBox = new NDateTimeBox();
			stack.Add(dateTimeBox);

			NFillSplitButton splitButton = new NFillSplitButton();
			stack.Add(splitButton);

			return stack;
		}
		private NTab CreateTab()
		{
			NTab tab = new NTab();
			tab.TabPages.Add(new NTabPage("Page 1", "This is tab page 1"));
			tab.TabPages.Add(new NTabPage("Page 2", "This is tab page 2"));
			tab.TabPages.Add(new NTabPage("Page 3", "This is tab page 3"));
			return tab;
		}
		private NStackPanel CreateRangeBased()
		{
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = 10;

			NNumericUpDown numericUpDown = new NNumericUpDown();
			stack.Add(numericUpDown);

			NSlider slider = new NSlider();
			stack.Add(slider);

			NProgressBar progressBar = new NProgressBar();
			progressBar.PreferredHeight = 20;
			progressBar.Value = 40;
			stack.Add(progressBar);

			return stack;
		}

		private NGroupBox CreateGroupBox(object header, object content)
		{
			NGroupBox groupBox = new NGroupBox(header, content);
			// Check whether the application is in touch mode and set the Size of the group box.
			bool touchMode = NApplication.Desktop.TouchMode;
			NSize size = new NSize(150, 150);
			if (touchMode)
			{
				size = new NSize(250, 250);
			}
			groupBox.PreferredSize = size;
			return groupBox;
		}

		#endregion

		#region Event Handlers

		private void OnThemeTreeViewSelectedPathChanged(NValueChangeEventArgs arg1)
		{
			NTreeView treeView = (NTreeView)arg1.CurrentTargetNode;
			NTreeViewItem selectedItem = treeView.SelectedItem;
			if (selectedItem == null)
				return;

			if (selectedItem.Tag is NTheme)
			{
				// Apply the selected theme to the document box's document
				NTheme theme = (NTheme)selectedItem.Tag;
				m_DocumentBox.Document.InheritStyleSheets = false;
				m_DocumentBox.Document.StyleSheets.ApplyTheme(theme);
			}
			else if (NStringHelpers.Equals(selectedItem.Tag, "inherit"))
			{
				// Make the document inherit its style sheets and clear all current ones
				m_DocumentBox.Document.InheritStyleSheets = true;
				m_DocumentBox.Document.StyleSheets.Clear();
			}
		}
		
		#endregion

		#region Fields

		private NDocumentBox m_DocumentBox;

		#endregion

		#region Schema

		public static readonly NSchema NDocumentBoxExampleSchema;

		#endregion
	}
}