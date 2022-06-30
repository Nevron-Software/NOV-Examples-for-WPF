using System.Globalization;

using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.Text;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NRibbonExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRibbonExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRibbonExample()
		{
			NRibbonExampleSchema = NSchema.Create(typeof(NRibbonExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NRibbon ribbon = new NRibbon();
			ribbon.VerticalPlacement = ENVerticalPlacement.Top;

			// The application menu
			ribbon.Tab.ApplicationMenu = CreateMenu();

			// The "Home" page
			ribbon.Tab.TabPages.Add(CreateHomePage());

			// The "Insert" page
			ribbon.Tab.TabPages.Add(CreateInsertPage());

			// The "View" page
			NRibbonTabPage viewPage = new NRibbonTabPage("View");
			ribbon.Tab.TabPages.Add(viewPage);

			// The help button
			ribbon.Tab.AdditionalContent = new NRibbonHelpButton();

			// The ribbon search box
			ribbon.Tab.SearchBox = new NRibbonSearchBox();
			ribbon.Tab.SearchBox.InitFromOwnerRibbon();

			// Subscribe to ribbon button Click events
			ribbon.AddEventHandler(NButtonBase.ClickEvent, new NEventHandler<NEventArgs>(OnRibbonButtonClicked));			

			return ribbon;
		}				
		protected override NWidget CreateExampleControls()
		{
			m_EventsLog = new NExampleEventsLog();
			return m_EventsLog;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and populate a ribbon. It also demonstrates how to create and initialize a ribbon search box.
</p>
";
		}

		#endregion

		#region Implementation

		private NApplicationMenu CreateMenu()
		{
			NApplicationMenu appMenu = new NApplicationMenu("File");
			NMenu menu = appMenu.MenuPane;

			// Create the "Open" and "Save" menu items
			menu.Items.Add(new NMenuItem(NResources.Image_Ribbon_32x32_folder_action_open_png, "Open"));
			menu.Items.Add(new NMenuItem(NResources.Image_Ribbon_32x32_save_png, "Save"));

			// Create the "Save As" menu item and its sub items
			NMenuItem saveAsMenuItem = new NMenuItem(NResources.Image_Ribbon_32x32_save_as_png, "Save As");
			saveAsMenuItem.Items.Add(new NMenuItem("PNG Image"));
			saveAsMenuItem.Items.Add(new NMenuItem("JPEG Image"));
			saveAsMenuItem.Items.Add(new NMenuItem("BMP Image"));
			saveAsMenuItem.Items.Add(new NMenuItem("GIF Image"));
			menu.Items.Add(saveAsMenuItem);

			// Create the rest of the menu items
			menu.Items.Add(new NMenuSeparator());
			menu.Items.Add(new NMenuItem(NResources.Image_Ribbon_32x32_print_png, "Print"));
			menu.Items.Add(new NMenuItem(NResources.Image_Ribbon_32x32_settings_png, "Options"));
			menu.Items.Add(new NMenuSeparator());
			menu.Items.Add(new NMenuItem(NResources.Image_Ribbon_32x32_exit_png, "Exit"));

			// Create a label for the content pane
			appMenu.ContentPane = new NLabel("This is the content pane");

			// Create 2 buttons for the footer pane
			appMenu.FooterPane = new NApplicationMenuFooterPanel();
			appMenu.FooterPane.Add(new NButton("Options..."));
			appMenu.FooterPane.Add(new NButton("Exit"));

			return appMenu;
		}
		private NRibbonTabPage CreateHomePage()
		{
			NRibbonTabPage page = new NRibbonTabPage("Home");

			#region Clipboard Group

			NRibbonGroup group = new NRibbonGroup("Clipboard");
			group.Icon = NResources.Image_Ribbon_16x16_clipboard_copy_png;
			page.Groups.Add(group);

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

			#endregion

			#region Font Group

			group = new NRibbonGroup("Font");
			group.Icon = NResources.Image_Ribbon_16x16_character_change_case_png;
			page.Groups.Add(group);

			NRibbonWrapFlowPanel wrapPanel = new NRibbonWrapFlowPanel();
			wrapPanel.HorizontalSpacing = NDesign.HorizontalSpacing;
			group.Items.Add(wrapPanel);

			NRibbonStackPanel stackPanel = CreateStackPanel();
			wrapPanel.Add(stackPanel);
			NFontNameComboBox fontNameComboBox = new NFontNameComboBox();
			fontNameComboBox.SelectedIndex = 5;
			stackPanel.Add(fontNameComboBox);
			NComboBox fontSizeComboBox = new NComboBox();
			FillFontSizeCombo(fontSizeComboBox);
			fontSizeComboBox.SelectedIndex = 2;
			stackPanel.Add(fontSizeComboBox);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonButton.CreateSmall("Grow Font", NResources.Image_Ribbon_16x16_font_grow_png));
			stackPanel.Add(NRibbonButton.CreateSmall("Shrink Font", NResources.Image_Ribbon_16x16_font_shrink_png));
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(stackPanel);

			NRibbonMenuDropDown changeCaseMenu = NRibbonMenuDropDown.CreateSmall("Change Case", NResources.Image_Ribbon_16x16_character_change_case_png);
			changeCaseMenu.Menu.Items.Add(new NMenuItem("lowercase"));
			changeCaseMenu.Menu.Items.Add(new NMenuItem("UPPERCASE"));
			changeCaseMenu.Menu.Items.Add(new NMenuItem("Capitalize Each Word"));			
			wrapPanel.Add(changeCaseMenu);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Bold", NResources.Image_Ribbon_16x16_character_bold_small_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Italic", NResources.Image_Ribbon_16x16_character_italic_small_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Underline", NResources.Image_Ribbon_16x16_character_underline_small_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Strikethrough", NResources.Image_Ribbon_16x16_character_strikethrough_small_png));
			NRibbonStackPanel panel2 = CreateStackPanel();
			panel2.Add(NRibbonToggleButton.CreateSmall("Subscript", NResources.Image_Ribbon_16x16_character_subscript_small_png));
			panel2.Add(NRibbonToggleButton.CreateSmall("Superscript", NResources.Image_Ribbon_16x16_character_superscript_small_png));
			stackPanel.Add(new NToggleButtonGroup(panel2));
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(stackPanel);

			NFillSplitButton fillSplitButton = new NFillSplitButton();
			fillSplitButton.Image = NResources.Image_Ribbon_16x16_text_fill_png;
			wrapPanel.Add(fillSplitButton);

			#endregion

			#region Paragraph Group

			group = new NRibbonGroup("Paragraph");
			group.Icon = NResources.Image_Ribbon_16x16_paragraph_align_center_png;
			page.Groups.Add(group);

			wrapPanel = new NRibbonWrapFlowPanel();
			wrapPanel.HorizontalSpacing = NDesign.HorizontalSpacing;
			group.Items.Add(wrapPanel);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonSplitButton.CreateSmall("Bullets", NResources.Image_Ribbon_16x16_list_bullets_png));
			stackPanel.Add(NRibbonSplitButton.CreateSmall("Numbering", NResources.Image_Ribbon_16x16_list_numbers_png));
			NRibbonMenuDropDown multilevelListMenu = NRibbonMenuDropDown.CreateSmall("Multilevel List", NResources.Image_Ribbon_16x16_list_multilevel_png);
			multilevelListMenu.Menu.Items.Add(new NMenuItem("Alpha and Numeric"));
			multilevelListMenu.Menu.Items.Add(new NMenuItem("Alpha and Roman"));
			multilevelListMenu.Menu.Items.Add(new NMenuItem("Numeric and Roman"));
			stackPanel.Add(multilevelListMenu);
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(stackPanel);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonButton.CreateSmall("Decrease Indent", NResources.Image_Ribbon_16x16_paragraph_indent_left_png));
			stackPanel.Add(NRibbonButton.CreateSmall("Increase Indent", NResources.Image_Ribbon_16x16_paragraph_indent_right_png));
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(stackPanel);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonButton.CreateSmall("Sort", NResources.Image_Ribbon_16x16_sort_az_png));
			stackPanel.Add(new NRibbonSeparator());
			stackPanel.Add(NRibbonButton.CreateSmall("Marks", NResources.Image_Ribbon_16x16_paragraph_marker_small_png));
			wrapPanel.Add(stackPanel);

			stackPanel = CreateStackPanel();
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Align Left", NResources.Image_Ribbon_16x16_paragraph_align_left_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Align Center", NResources.Image_Ribbon_16x16_paragraph_align_center_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Align Right", NResources.Image_Ribbon_16x16_paragraph_align_right_png));
			stackPanel.Add(NRibbonToggleButton.CreateSmall("Justify", NResources.Image_Ribbon_16x16_paragraph_align_justified_png));
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(new NToggleButtonGroup(stackPanel));

			stackPanel = CreateStackPanel();
			NRibbonMenuDropDown lineSpacingMenu = NRibbonMenuDropDown.CreateSmall("Line Spacing", NResources.Image_Ribbon_16x16_paragraph_spacing_before_png);
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("1.0"));
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("1.15"));
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("1.5"));
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("2.0"));
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("3.0"));
			lineSpacingMenu.Menu.Items.Add(new NMenuSeparator());
			lineSpacingMenu.Menu.Items.Add(new NMenuItem("Line Spacing Options..."));
			stackPanel.Add(lineSpacingMenu);
			stackPanel.Add(new NRibbonSeparator());
			wrapPanel.Add(stackPanel);

			fillSplitButton = new NFillSplitButton();
			wrapPanel.Add(fillSplitButton);

			#endregion

			#region Styles Group

			group = new NRibbonGroup("Styles");
			group.Icon = NResources.Image_Ribbon_16x16_table_design_png;
			page.Groups.Add(group);

			NRibbonGallery gallery = new NRibbonGallery("Table Style", NResources.Image_Ribbon_32x32_table_design_png, new NTableStylePicker());
			gallery.MinimumPopupColumnCount = 7;
			gallery.PopupMenu = new NMenu();
			gallery.PopupMenu.Items.Add(new NMenuSeparator());
			gallery.PopupMenu.Items.Add(new NMenuItem("Modify Table Style..."));
			gallery.PopupMenu.Items.Add(new NMenuItem("New Table Style..."));
			group.Items.Add(gallery);

			#endregion

			return page;
		}
		private NRibbonTabPage CreateInsertPage()
		{
			NRibbonTabPage page = new NRibbonTabPage("Insert");

			// The "Page" group
			NRibbonGroup group = new NRibbonGroup("Page");
			group.Icon = NResources.Image_Ribbon_16x16_cover_page_png;

			NRibbonCollapsiblePanel panel = new NRibbonCollapsiblePanel();
			group.Items.Add(panel);

			NRibbonButton button = new NRibbonButton("Cover Page");
			button.LargeImage = NResources.Image_Ribbon_32x32_cover_page_png;
			button.SmallImage = NResources.Image_Ribbon_16x16_cover_page_png;
			panel.Add(button);

			NImageBox imageBox = new NImageBox(NResources.Image_Ribbon_16x16_character_bold_small_png);

			button = new NRibbonButton("Blank Page");
			button.LargeImage = NResources.Image_Ribbon_32x32_page_png;
			button.SmallImage = NResources.Image_Ribbon_16x16_page_png;
			panel.Add(button);

			button = new NRibbonButton("Page Break");
			button.LargeImage = NResources.Image_Ribbon_32x32_page_break_png;
			button.SmallImage = NResources.Image_Ribbon_16x16_page_break_png;
			panel.Add(button);

			page.Groups.Add(group);

			return page;
		}

		#endregion

		#region Event Handlers

		private void OnRibbonButtonClicked(NEventArgs arg)
		{
			INRibbonButton button = arg.TargetNode as INRibbonButton;
			if (button != null)
			{
				m_EventsLog.LogEvent(button.Text + " clicked");
			}
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRibbonExample.
		/// </summary>
		public static readonly NSchema NRibbonExampleSchema;

		#endregion

		#region Static Methods

		private static NRibbonStackPanel CreateStackPanel()
		{
			NRibbonStackPanel stackPanel = new NRibbonStackPanel();
			stackPanel.HorizontalSpacing = NDesign.HorizontalSpacing;
			return stackPanel;
		}
		/// <summary>
		/// Fills the given combo box with the commonly used font sizes.
		/// </summary>
		/// <returns></returns>
		private static void FillFontSizeCombo(NComboBox comboBox)
		{
			NComboBoxItemCollection items = comboBox.Items;
			items.Clear();

			int i = 8;
			for (; i < 12; i++)
			{
				items.Add(CreateFontSizeComboBoxItem(i));
			}

			for (; i <= 28; i += 2)
			{
				items.Add(CreateFontSizeComboBoxItem(i));
			}

			items.Add(CreateFontSizeComboBoxItem(36));
			items.Add(CreateFontSizeComboBoxItem(48));
			items.Add(CreateFontSizeComboBoxItem(72));
		}
		private static NComboBoxItem CreateFontSizeComboBoxItem(int fontSize)
		{
			NComboBoxItem item = new NComboBoxItem(fontSize.ToString(CultureInfo.InvariantCulture));
			return item;
		}

		#endregion
	}
}