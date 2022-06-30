using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	#region Nested Types

	/// <summary>
	/// A custom NOV theme based on the Windows 10 theme.
	/// </summary>
	public class NCustomTheme : NWindows10Theme 
	{
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCustomTheme()
		{
			NCustomThemeSchema = NSchema.Create(typeof(NCustomTheme), NWindows10ThemeSchema);
		}

		/// <summary>
		/// Creates button styles.
		/// </summary>
		protected override void CreateButtonStyles(NSchema buttonSchema)
		{
			// Change mouse over background fill to orange
			NColorSkinState mouseOverState = (NColorSkinState)Skins.Button.GetState(NSkinState.MouseOver);
			mouseOverState.BackgroundFill = new NColorFill(NColor.Orange);

			// Change pressed button border to 3px purple border, background fill to dark red and text fill to white
			NColorSkinState pressedState = (NColorSkinState)Skins.Button.GetState(NSkinState.Pressed);
			pressedState.SetBorderFill(new NColorFill(NColor.Green));
			pressedState.BorderThickness = new NMargins(3);
			pressedState.BackgroundFill = new NColorFill(NColor.DarkRed);
			pressedState.TextFill = new NColorFill(NColor.White);

			// Call base to skin the buttons
			base.CreateButtonStyles(buttonSchema);
		}
		/// <summary>
		/// Creates flat button styles, which are buttons commonly in ribbon and toolbars.
		/// </summary>
		protected override void CreateFlatButtonStyles()
		{
			NColorSkinState mouseOverState = (NColorSkinState)Skins.FlatButton.GetState(NSkinState.MouseOver);
			mouseOverState.BackgroundFill = new NColorFill(NColor.Orange);

			// Call base to skin flat buttons
			base.CreateFlatButtonStyles();
		}
		/// <summary>
		/// Creates the tab styles. Overriden to make the mouse over state orange.
		/// </summary>
		protected override void CreateTabStyles()
		{
			// Modify the tab skins
			NColor backgroundColor = NColor.Orange;

			NSkin[] tabSkins = new NSkin[] {
			  TabSkins.Top.FarTabPageHeaderSkin,
			  TabSkins.Top.InnerTabPageHeaderSkin,
			  TabSkins.Top.NearAndFarTabPageHeaderSkin,
			  TabSkins.Top.NearTabPageHeaderSkin };

			for (int i = 0; i < tabSkins.Length; i++)
			{
				NColorSkinState state = (NColorSkinState)tabSkins[i].GetState(NSkinState.MouseOver);
				state.BackgroundFill = new NColorFill(backgroundColor);
			}

			// Call base to skin the tab widget
			base.CreateTabStyles();
		}

		/// <summary>
		/// Schema associated with NCustomTheme.
		/// </summary>
		public static readonly NSchema NCustomThemeSchema;
	}

	#endregion

	public class NDocumentBoxAndThemesExample : NExampleBase
	{
		#region Constructors

		public NDocumentBoxAndThemesExample()
		{
		}
		static NDocumentBoxAndThemesExample()
		{
			NDocumentBoxAndThemesExampleSchema = NSchema.Create(typeof(NDocumentBoxAndThemesExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

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

			table.Add(CreateGroupBox("Ribbon", CreateRibbon()));
			table.Add(CreateGroupBox("Command Bars", CreateCommandBars()));
			table.Add(CreateGroupBox("Windows", CreateWindows()));

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
			NStackPanel stack = new NStackPanel();

			NCheckBox enabledCheckBox = new NCheckBox("Enabled", true);
			enabledCheckBox.CheckedChanged += OnEnabledCheckBoxCheckedChanged;
			stack.Add(enabledCheckBox);


			// Create the theme tree view
			NTheme theme;
			NTreeViewItem rootItem, themeItem;

			NTreeView themeTreeView = new NTreeView();
			stack.Add(themeTreeView);

			//
			// Add the "Inherit Styles" root tree view item
			//
			rootItem = new NTreeViewItem("Inherit Styles");
			rootItem.Tag = "inherit";
			themeTreeView.Items.Add(rootItem);
			themeTreeView.SelectedItem = rootItem;

			//
			// Add the part based UI themes to the tree view
			//
			rootItem = new NTreeViewItem("Part Based");
			rootItem.Expanded = true;
			themeTreeView.Items.Add(rootItem);

            themeItem = new NTreeViewItem("Windows XP Blue");
            themeItem.Tag = new NWindowsXPBlueTheme();
            rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Windows 7 Aero");
			themeItem.Tag = new NWindowsAeroTheme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Windows 8");
			themeItem.Tag = new NWindows8Theme();
			rootItem.Items.Add(themeItem);

			themeItem = new NTreeViewItem("Windows 10");
			themeItem.Tag = new NWindows10Theme();
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

			//
			// Add the windows classic UI themes to the tree view
			//
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

			//
			// Add the custom themes to the tree view
			//
			rootItem = new NTreeViewItem("Custom");
			rootItem.Expanded = true;
			themeTreeView.Items.Add(rootItem);

			themeItem = new NTreeViewItem("Custom Theme");
			themeItem.Tag = new NCustomTheme();
			rootItem.Items.Add(themeItem);

			// Subscribe to the selected path changed event
			themeTreeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnThemeTreeViewSelectedPathChanged);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to use document boxes. The document box is a widget that has a document.
	If you set the <b>InheritStyleSheetsProperty</b> of the document box's document to false, then the widgets hosted
	in it will not inherit the styling of the document the document box is placed in.
</p>
<p>
	This example also demonstrates the UI themes incuded in Nevron Open Vision. Select a theme from the tree view on
	the right to apply it to the document box and see how the widgets look when that theme is applied.
</p>
<p>
	The last item in the tree view is a custom theme that changes the mouse over state of buttons, flat buttons (i.e.
	buttons in ribbon and toolbars) and tab page headers. The custom theme is defined in the beginning of the example's
	source code.
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
			for (int i = 1; i <= 7; i++)
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

			stack.Add(CreateComboBox());

			NColorBox colorBox = new NColorBox();
			stack.Add(colorBox);

			NDateTimeBox dateTimeBox = new NDateTimeBox();
			stack.Add(dateTimeBox);

			NFillSplitButton splitButton = new NFillSplitButton();
			stack.Add(splitButton);

			return stack;
		}
		private NComboBox CreateComboBox()
		{
			NComboBox comboBox = new NComboBox();
			comboBox.Items.Add(new NComboBoxItem("Item 1"));
			comboBox.Items.Add(new NComboBoxItem("Item 2"));
			comboBox.Items.Add(new NComboBoxItem("Item 3"));
			comboBox.Items.Add(new NComboBoxItem("Item 4"));
			comboBox.SelectedIndex = 0;

			return comboBox;
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

		private NRibbon CreateRibbon()
		{
			NRibbon ribbon = new NRibbon();
			ribbon.VerticalPlacement = ENVerticalPlacement.Top;

			//
			// Create the "Home" ribbon tab page
			//
			NRibbonTabPage pageHome = new NRibbonTabPage("Home");
			ribbon.Tab.TabPages.Add(pageHome);

			// Create the "Clipboard" group of the "Home" tab page
			NRibbonGroup group = new NRibbonGroup("Clipboard");
			group.Icon = NResources.Image_Ribbon_16x16_clipboard_copy_png;
			pageHome.Groups.Add(group);

			NRibbonSplitButton pasteSplitButton = NRibbonSplitButton.CreateLarge("Paste", NResources.Image_Ribbon_32x32_clipboard_paste_png);
			pasteSplitButton.CollapseToMedium = ENCollapseCondition.Never;
			pasteSplitButton.CollapseToSmall = ENCollapseCondition.Never;

			NMenu pasteMenu = new NMenu();
			pasteMenu.Items.Add(new NMenuItem("Paste"));
			pasteMenu.Items.Add(new NMenuItem("Paste Special..."));
			pasteMenu.Items.Add(new NMenuItem("Paste as Link"));
			pasteSplitButton.Popup.Content = pasteMenu;

			group.Items.Add(pasteSplitButton);

			NRibbonCollapsiblePanel collapsiblePanel = new NRibbonCollapsiblePanel();
			collapsiblePanel.InitialState = (int)ENRibbonWidgetState.Medium;
			group.Items.Add(collapsiblePanel);

			collapsiblePanel.Add(new NRibbonButton("Cut", null, NResources.Image_Ribbon_16x16_clipboard_cut_png));
			collapsiblePanel.Add(new NRibbonButton("Copy", null, NResources.Image_Ribbon_16x16_clipboard_copy_png));
			collapsiblePanel.Add(new NRibbonButton("Format Painter", null, NResources.Image_Ribbon_16x16_copy_format_png));

			// Create the "Format" group of the "Home" tab page
			group = new NRibbonGroup("Format");
			pageHome.Groups.Add(group);

			collapsiblePanel = new NRibbonCollapsiblePanel();
			collapsiblePanel.InitialState = (int)ENRibbonWidgetState.Medium;
			group.Items.Add(collapsiblePanel);

			NFillSplitButton fillSplitButton = new NFillSplitButton();
			collapsiblePanel.Add(fillSplitButton);

			NStrokeSplitButton strokeSplitButton = new NStrokeSplitButton();
			collapsiblePanel.Add(strokeSplitButton);

			//
			// Add an "Insert" ribbon tab page
			//
			ribbon.Tab.TabPages.Add(new NRibbonTabPage("Insert"));

			return ribbon;
		}
		private NCommandBarManager CreateCommandBars()
		{
			NCommandBarManager manager = new NCommandBarManager();

			// Create a menu bar in the first lane
			NCommandBarLane lane1 = new NCommandBarLane();
			manager.TopDock.Add(lane1);

			NMenuBar menuBar = new NMenuBar();
			menuBar.Pendant.Visibility = ENVisibility.Collapsed;
			lane1.Add(menuBar);

			NMenuDropDown fileMenu = new NMenuDropDown("File");
			menuBar.Items.Add(fileMenu);

			NMenuItem newMenuItem = new NMenuItem(Nevron.Nov.Presentation.NResources.Image_File_New_png, "New");
			fileMenu.Items.Add(newMenuItem);
			newMenuItem.Items.Add(new NMenuItem("Project"));
			newMenuItem.Items.Add(new NMenuItem("Web Site"));
			newMenuItem.Items.Add(new NMenuItem("File"));

			fileMenu.Items.Add(new NMenuItem(Nevron.Nov.Presentation.NResources.Image_File_Open_png, "Open"));
			fileMenu.Items.Add(new NMenuItem(Nevron.Nov.Presentation.NResources.Image_File_Save_png, "Save"));
			fileMenu.Items.Add(new NMenuSeparator());
			fileMenu.Items.Add(new NMenuItem(Nevron.Nov.Presentation.NResources.Image_File_Print_png, "Print"));

			NMenuDropDown editMenu = new NMenuDropDown("Edit");
			menuBar.Items.Add(editMenu);

			editMenu.Items.Add(new NMenuItem(Nevron.Nov.Presentation.NResources.Image_Edit_Undo_png, "Undo"));
			editMenu.Items.Add(new NMenuItem(Nevron.Nov.Presentation.NResources.Image_Edit_Redo_png, "Redo"));

			// Add a toolbar in the second lane
			NCommandBarLane lane2 = new NCommandBarLane();
			manager.TopDock.Add(lane2);

			NToolBar toolbar = new NToolBar();
			toolbar.Pendant.Visibility = ENVisibility.Collapsed;
			lane2.Add(toolbar);

			toolbar.Items.Add(NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_New_png, "New"));
			toolbar.Items.Add(NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_Open_png, "Open"));
			toolbar.Items.Add(NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_Save_png, "Save"));
			toolbar.Items.Add(new NCommandBarSeparator());
			toolbar.Items.Add(NButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_File_Print_png, "Print"));

			// Add a toolbar in the third lane
			NCommandBarLane lane3 = new NCommandBarLane();
			manager.TopDock.Add(lane3);

			toolbar = new NToolBar();
			toolbar.Pendant.Visibility = ENVisibility.Collapsed;
			lane3.Add(toolbar);

			toolbar.Items.Add(CreateComboBox());
			toolbar.Items.Add(new NCommandBarSeparator());
			toolbar.Items.Add(NToggleButton.CreateImageAndText(Nevron.Nov.Presentation.NResources.Image_FontStyle_Bold_png, "Bold"));
			toolbar.Items.Add(new NCheckBox("Plain Text"));

			return manager;
		}
		private NStackPanel CreateWindows()
		{
			NStackPanel stack = new NStackPanel();
			stack.VerticalSpacing = 10;

			// create a button which shows a top-level window, added to the NDocumentBoxSurface.Windows collection.
			// such top-level windows will use the style sheets applied to the NDocumentBox.
			NButton windowButton = new NButton("Show Window...");
			windowButton.Click += delegate (NEventArgs arg)
			{
				NTopLevelWindow window = NApplication.CreateTopLevelWindow(m_DocumentBox.Surface);
				window.Title = "Top-Level Window";

				NStackPanel windowStack = new NStackPanel();

				NLabel label = new NLabel("Top-level windows that are added to the NDocumentBoxSurface Windows collection,\r\n will use the NDocumentBox styling.");
				windowStack.Add(label);

				NButtonStrip buttonStrip = new NButtonStrip();
				buttonStrip.AddCloseButton();
				windowStack.Add(buttonStrip);

				window.Content = windowStack;
				window.Open();
			};
			stack.Add(windowButton);

			// message box
			NButton showMessageBoxButton = new NButton("Show MessageBox...");
			showMessageBoxButton.Click += delegate (NEventArgs arg)
			{
				NMessageBoxSettings msgBoxSettings = new NMessageBoxSettings("Message boxes that are added to the NDocumentBoxSurface Windows collection,\r\n will use the NDocumentBox styling.", "Message box title");
				msgBoxSettings.WindowsContainer = m_DocumentBox.Surface;
				NMessageBox.Show(msgBoxSettings);
			};
			stack.Add(showMessageBoxButton);

			return stack;
		}

		private NGroupBox CreateGroupBox(object header, object content)
		{
			NGroupBox groupBox = new NGroupBox(header, content);

			// Check whether the application is in touch mode and set the size of the group box accordingly
			groupBox.PreferredSize = NApplication.Desktop.TouchMode ?
				new NSize(360, 250) : // touch mode size
				new NSize(260, 180);  // regular mode size

			return groupBox;
		}

		#endregion

		#region Event Handlers

		private void OnEnabledCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_DocumentBox.Surface.Content.Enabled = (bool)arg.NewValue;
		}
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

		public static readonly NSchema NDocumentBoxAndThemesExampleSchema;

		#endregion
	}
}