using System;
using System.IO;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples
{
	/// <summary>
	/// A dock panel that hosts a tree view on the left and an example on the right.
	/// </summary>
	public class NExampleHost : NDockPanel
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NExampleHost()
		{
			// Create the main menu
			NMenuBar mainMenu = CreateMainMenu();
			Add(mainMenu, ENDockArea.Top);

			// Create the example toolbar
			m_Toolbar = CreateToolbar();
			Add(m_Toolbar, ENDockArea.Top);

			// Create the status bar
			NStatusBar statusBar = new NStatusBar();
			m_StatusLabel = new NLabel();
			statusBar.Items.Add(m_StatusLabel);
			Add(statusBar, ENDockArea.Bottom);

			// Create the example splitter and tree view
			m_Splitter = new NSplitter();
			m_Splitter.SplitMode = ENSplitterSplitMode.OffsetFromNearSide;
			m_Splitter.SplitOffset = TreeViewPaneWidth;
			Add(m_Splitter, ENDockArea.Center);

			m_TreeView = new NTreeView();
			m_TreeView.SelectedPathChanged += OnTreeViewSelectedPathChanged;
			m_Splitter.Pane1.Content = m_TreeView;
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NExampleHost()
		{
			NExampleHostSchema = NSchema.Create(typeof(NExampleHost), NDockPanel.NDockPanelSchema);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the home button.
		/// </summary>
		public NButton HomeButton
		{
			get
			{
				return m_HomeButton;
			}
		}
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

		#endregion

		#region Public Methods

		public void InitForElement(NXmlElement element, bool loadDefaultExample)
		{
			// Populate the examples tree view
			if (m_TreeView.Items.Count > 0)
			{
				m_TreeView.SelectedItem = null;
				m_TreeView.Items.Clear();
			}

			int childCount = element.ChildrenCount;
			if (childCount > 0)
			{
				// This is a tile or an example folder
				AddItemsFor(m_TreeView.Items, element);

				// Expand all items
				m_TreeView.ExpandAll(true);
			}
			else
			{
				// This is a single example
				m_TreeView.Items.Add(CreateTreeViewItem(element));
			}

			if (loadDefaultExample)
			{
				// Find and select the default example
				m_TreeView.SelectedItem = GetDefaultOrFirstExampleItem();
			}
			else
			{
				CreateBreadcrumb(element);
			}
		}

		#endregion

		#region Protected Methods - Main Menu

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected NMenuBar CreateMainMenu()
		{
			NMenuBar menuBar = new NMenuBar();

			// collapse the gripper and the pendant for the main menu
			menuBar.Pendant.Visibility = ENVisibility.Collapsed;


			menuBar.Items.Add(CreateThemesMenuDropDown());
			menuBar.Items.Add(CreateOptionsMenuDropDown());


			return menuBar;
		}

		/// <summary>
		/// Creates the menu drop down that allows the user to select a theme.
		/// </summary>
		/// <returns></returns>
		protected NMenuDropDown CreateThemesMenuDropDown()
		{
			NMenuDropDown themesMenu = new NMenuDropDown("Theme");

			// Add the Windows Classic themes
			NMenuItem windowsClassicMenuItem = new NMenuItem("Windows Classic");
			themesMenu.Items.Add(windowsClassicMenuItem);

			ENUIThemeScheme[] windowsClassicSchemes = NEnum.GetValues<ENUIThemeScheme>();
			for (int i = 0, count = windowsClassicSchemes.Length; i < count; i++)
			{
				ENUIThemeScheme scheme = (ENUIThemeScheme)windowsClassicSchemes.GetValue(i);
				NWindowsClassicTheme theme = new NWindowsClassicTheme(scheme);
				NCheckableMenuItem themeItem = new NCheckableMenuItem(NStringHelpers.InsertSpacesBeforeUppersAndDigits(scheme.ToString()));
				themeItem.Tag = theme;
				themeItem.Click += OnThemeMenuItemClick;
				windowsClassicMenuItem.Items.Add(themeItem);
			}

			// Add the touch themes
			NMenuItem touchThemesMenuItem = new NMenuItem("Touch Themes");
			themesMenu.Items.Add(touchThemesMenuItem);

			// Add the Windows 8 touch theme 
			NCheckableMenuItem windows8ThemeMenuItemTouch = new NCheckableMenuItem("Windows 8 Touch");
			windows8ThemeMenuItemTouch.Tag = new NWindows8Theme(true);
			windows8ThemeMenuItemTouch.Click += OnThemeMenuItemClick;
			touchThemesMenuItem.Items.Add(windows8ThemeMenuItemTouch);

			// Add the dark theme
			NCheckableMenuItem darkThemeTouch = new NCheckableMenuItem("Nevron Dark Touch");
			darkThemeTouch.Tag = new NNevronDarkTheme(true);
			darkThemeTouch.Click += OnThemeMenuItemClick;
			touchThemesMenuItem.Items.Add(darkThemeTouch);

			// Add the light theme
			NCheckableMenuItem lightThemeTouch = new NCheckableMenuItem("Nevron Light Touch");
			lightThemeTouch.Tag = new NNevronLightTheme(true);
			lightThemeTouch.Click += OnThemeMenuItemClick;
			touchThemesMenuItem.Items.Add(lightThemeTouch);

			// Add the Windows XP Blue theme
			NCheckableMenuItem xpBlueMenuItem = new NCheckableMenuItem("Windows XP Blue");
			xpBlueMenuItem.Tag = new NWindowsXPBlueTheme();
			xpBlueMenuItem.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(xpBlueMenuItem);

			// Add the Windows Aero theme
			NCheckableMenuItem aeroThemeMenuItem = new NCheckableMenuItem("Windows 7 Aero");
			aeroThemeMenuItem.Tag = new NWindowsAeroTheme();
			aeroThemeMenuItem.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(aeroThemeMenuItem);

			// Add the Windows 8 theme (the default theme)
			NCheckableMenuItem windows8ThemeMenuItem = new NCheckableMenuItem("Windows 8");
			windows8ThemeMenuItem.Tag = new NWindows8Theme();
			windows8ThemeMenuItem.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(windows8ThemeMenuItem);
			windows8ThemeMenuItem.Checked = true;
			m_CurrentThemeMenuItem = windows8ThemeMenuItem;

			// Add the Mac Lion theme
			NCheckableMenuItem macLionThemeMenuItem = new NCheckableMenuItem("Mac OS X Lion");
			macLionThemeMenuItem.Tag = new NMacLionTheme();
			macLionThemeMenuItem.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(macLionThemeMenuItem);

			// Add the Mac El Capitan theme
			NCheckableMenuItem macElCapitanTheme = new NCheckableMenuItem("Mac OS X El Capitan");
			macElCapitanTheme.Tag = new NMacElCapitanTheme();
			macElCapitanTheme.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(macElCapitanTheme);

			// Add the dark theme
			NCheckableMenuItem darkTheme = new NCheckableMenuItem("Nevron Dark");
			darkTheme.Tag = new NNevronDarkTheme();
			darkTheme.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(darkTheme);

			// Add the light theme
			NCheckableMenuItem lightTheme = new NCheckableMenuItem("Nevron Light");
			lightTheme.Tag = new NNevronLightTheme();
			lightTheme.Click += OnThemeMenuItemClick;
			themesMenu.Items.Add(lightTheme);

			return themesMenu;
		}
		/// <summary>
		/// Creates the Options drop down menu.
		/// </summary>
		/// <returns></returns>
		protected virtual NMenuDropDown CreateOptionsMenuDropDown()
		{
			NMenuDropDown optionsMenu = new NMenuDropDown("Options");

			NCheckableMenuItem developerModeMenuItem = new NCheckableMenuItem("Developer Mode");
			developerModeMenuItem.Click += OnDeveloperModeMenuItemClick;
			optionsMenu.Items.Add(developerModeMenuItem);

			return optionsMenu;
		}


		#endregion

		#region Implementation - Tree View Items

		private void AddItemsFor(NTreeViewItemCollection items, NXmlElement element)
		{
			int childCount = element.ChildrenCount;
			for (int i = 0; i < childCount; i++)
			{
				NXmlElement child = element.GetChildAt(i) as NXmlElement;
				if (child == null)
					continue;

				NTreeViewItem item = CreateTreeViewItem(child);
				if (item != null)
				{
					items.Add(item);
					if (item.Tag is NXmlElement == false)
					{
						// This is a folder item, so add items for its children, too
						AddItemsFor(item.Items, child);
					}
				}
				else
				{
					AddItemsFor(items, child);
				}
			}
		}
		private NTreeViewItem CreateTreeViewItem(NXmlElement element)
		{
			if (IsSingleExampleTile(element))
			{
				// This is a tile with a single example, so create only the example tree view item
				return CreateTreeViewItem((NXmlElement)element.GetChildAt(0));
			}

			NImage icon;
			string name = element.GetAttributeValue("name");
			if (String.IsNullOrEmpty(name))
				return null;

			switch (element.Name)
			{
				case "tile":
				case "group":
				case "folder":
					icon = NResources.Image__16x16_Folders_png;
					break;
				case "example":
					icon = NResources.Image__16x16_Contacts_png;
					break;
				default:
					return null;
			}

			NExampleTile tile = new NExampleTile(icon, name);
			tile.Status = element.GetAttributeValue("status");
			tile.Box2.VerticalPlacement = ENVerticalPlacement.Center;
			tile.Spacing = NDesign.HorizontalSpacing;

			NTreeViewItem item = new NTreeViewItem(tile);
			if (element.Name == "example")
			{
				// This is an example element
				item.Tag = element;

				if (NApplication.Platform == ENPlatform.Silverlight)
				{
					// Handle the right click event in Silverlight to show a context menu
					// for copying a link to the example
					item.MouseDown += OnTreeViewItemMouseDown;
				}
			}

			return item;
		}

		#endregion

		#region Implementation

		private void LoadExample(NXmlElement element)
		{
			string groupNamespace = NExamplesHomePage.GetNamespace(element);
			string name = element.GetAttributeValue("name");
			string type = groupNamespace + "." + element.GetAttributeValue("type");

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

					string stats = "Example created in: " + (DateTime.Now - start).TotalSeconds + " seconds, ";

					// Evaluate the example
					start = DateTime.Now;
					OwnerDocument.Evaluate();
					stats += " evaluated in: " + (DateTime.Now - start).TotalSeconds + " seconds";

					m_StatusLabel.Text = stats;
				}

				// Set the breadcrumb
				CreateBreadcrumb(element);

			}
			catch (Exception ex)
			{
				NTrace.WriteException("Failed to load example", ex);
				m_Splitter.Pane2.Content = new NErrorPanel("Failed to load example. Exception was: " + ex.Message);
			}
		}
		private void CreateBreadcrumb(NXmlElement element)
		{
			NList<NXmlElement> path = GetBreadcrumbPath(element);
			for (int i = 0, count = path.Count; i < count; i++)
			{
				NXmlElement curElement = path[i];
				if (IsSingleExampleTile(curElement))
					continue;

				string name = curElement.GetAttributeValue("name");
				if (String.IsNullOrEmpty(name))
					continue;

				if (m_Toolbar.Items[m_Toolbar.Items.Count - 1] is NButton)
				{
                    NLabel label = new NLabel(" > ");
                    label.TextAlignment = ENContentAlignment.MiddleCenter;
                    label.VerticalPlacement = ENVerticalPlacement.Fit;
                    m_Toolbar.Items.Add(label);
				}

                if (i != (count - 1))
                {
                    NButton button = new NButton(name);
                    button.Content.VerticalPlacement = ENVerticalPlacement.Center;
                    button.Click += OnBreadcrumbButtonClick;
                    button.Tag = path[i];
                    m_Toolbar.Items.Add(button);
                }
                else
                {
                    NLabel label = new NLabel(name);
                    label.VerticalPlacement = ENVerticalPlacement.Center;
                    m_Toolbar.Items.Add(label);
                }
			}
		}
		private NToolBar CreateToolbar()
		{
			// Create the example toolbar
			NToolBar toolBar = new NToolBar();
			toolBar.Items.HorizontalSpacing = NDesign.HorizontalSpacing;
			toolBar.Gripper.Visibility = ENVisibility.Collapsed;
			toolBar.Pendant.Visibility = ENVisibility.Collapsed;

			// Create and add the "Home" button to the toolbar
			NPairBox pairBox = new NPairBox(NResources.Image_Home_png, "Home");
			pairBox.Box2.VerticalPlacement = ENVerticalPlacement.Center;
			pairBox.Spacing = NDesign.HorizontalSpacing;

			m_HomeButton = new NButton(pairBox);
			m_HomeButton.Click += OnHomeButtonClick;
			toolBar.Items.Add(m_HomeButton);

			toolBar.Items.Add(new NCommandBarSeparator());
			return toolBar;
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

			// Remove the current breadcrumb
			if (m_Toolbar != null)
			{
				NCommandBarItemCollection items = m_Toolbar.Items;
				for (int i = items.Count - 1; i >= 2; i--)
				{
					m_Toolbar.Items.RemoveAt(i);
				}
			}
		}
		private NTreeViewItem GetDefaultOrFirstExampleItem()
		{
			// Find the default tree view item
			NTreeViewItemCollection items = m_TreeView.Items;
			for (int i = 0, count = items.Count; i < count; i++)
			{
				NTreeViewItem defaultItem = GetDefaultExampleItem(items[i], 1, 0);
				if (defaultItem != null)
					return defaultItem;
			}

			// There isn't a default example, so return the first one
			return GetFirstExampleItem(items[0]);
		}

		#endregion

		#region Event Handles - Main Menu

        #region Theme Menu

        /// <summary>
		/// Called when a theme menu item is clicked -> applies the specified theme
		/// </summary>
		/// <param name="arg1"></param>
		private void OnThemeMenuItemClick(NEventArgs arg1)
		{
			NCheckableMenuItem item = (NCheckableMenuItem)arg1.CurrentTargetNode;
			if (item.Tag is NTheme)
			{
				NApplication.ApplyTheme((NTheme)item.Tag);

				// Update the current theme menu item and check it
				m_CurrentThemeMenuItem.Checked = false;
				item.Checked = true;
				m_CurrentThemeMenuItem = item;

				// Resize the right panel if the theme is in touch mode.
				NExampleBase exampleBase = m_Splitter.Pane2.Content as NExampleBase;
				if(exampleBase == null)
				{
					return;
				}
				
				NSplitter splitter = exampleBase.Content as NSplitter;
				if(splitter == null)
				{
					return;
				}
				
				NSplitter exampleSplitter = splitter.Pane1.Content as NSplitter;
				if (exampleSplitter == null)
				{ 
					return;
				}
				
				bool touchMode = NApplication.Desktop.TouchMode;
				exampleSplitter.SplitOffset = touchMode ? 450 : 300;
			}
			else
			{
				throw new Exception("Clicked menu item not a theme?");
			}
		}

		#endregion

		#region Options Menu

		private void OnDeveloperModeMenuItemClick(NEventArgs arg)
		{
			NApplication.DeveloperMode = !NApplication.DeveloperMode;
		}

		#endregion

		#region InternalCode

		#region File Menu

		protected void OnOpenFileMenuItemClick(NEventArgs args)
		{
			NOpenFileDialog ofd = new NOpenFileDialog();
			ofd.FileTypes = new NFileDialogFileType[]
			{
				new NFileDialogFileType("Text Files", "txt"),
				new NFileDialogFileType("All Files", "*")
			};
			ofd.SelectedFilter = 0;
			ofd.MultiSelect = false;

			ofd.Closed += new Function<NOpenFileDialogResult>(OnOpenFileDialogClosed);
			ofd.RequestShow();
		}
		protected void OnOpenFileDialogClosed(NOpenFileDialogResult result)
		{
			switch (result.Result)
			{
				case ENCommonDialogResult.OK:

					string safeFileName = result.Files[0].Name;
					string text;

					using (Stream fs = result.Files[0].OpenRead())
					{
						using (StreamReader sr = new StreamReader(fs))
						{
							text = sr.ReadToEnd();
						}
					}

					NMessageBox.Show(null, text, safeFileName, ENMessageBoxButtons.OK);
					break;

				case ENCommonDialogResult.Error:
					NMessageBox.Show(null, "Error: " + result.ErrorException.Message, "Dialog Closed", ENMessageBoxButtons.OK);
					break;
			}
		}

		protected void OnSaveFileMenuItemClick(NEventArgs args)
		{
			NSaveFileDialog sfd = new NSaveFileDialog();
			sfd.FileTypes = new NFileDialogFileType[]
			{
				new NFileDialogFileType("Text Files", "txt"),
				new NFileDialogFileType("All Files", "*")
			};
			sfd.SelectedFilter = 0;
			sfd.DefaultFileName = "NevronTest";
			sfd.DefaultExtension = "txt";

			sfd.Closed += new Function<NSaveFileDialogResult>(OnSaveFileDialogClosed);
			sfd.RequestShow();
		}
		protected void OnSaveFileDialogClosed(NSaveFileDialogResult result)
		{
			switch (result.Result)
			{
				case ENCommonDialogResult.OK:
					string safeFileName = result.SafeFileName;

					try
					{
						string fullFileName = result.File.Path;
					}
					catch
					{
					}

					using (Stream fs = result.File.OpenWrite())
					{
						using (StreamWriter sw = new StreamWriter(fs))
						{
							sw.Write("This text is generated by a dangerous VIRUS.");
						}
					}

					NMessageBox.Show(null, safeFileName, "File Saved", ENMessageBoxButtons.OK);
					break;

				case ENCommonDialogResult.Error:
					NMessageBox.Show(null, "Error: " + result.ErrorException.Message, "Dialog Closed", ENMessageBoxButtons.OK);
					break;
			}

		}

		protected void OnPrintMenuItemClick(NEventArgs args)
		{
			Random rand = new Random();

			NPrintDocument printDocument = new NPrintDocument();
			printDocument.DocumentName = "T:" + rand.Next().ToString();
			printDocument.BeginPrint += new Function<NPrintDocument, NBeginPrintEventArgs>(OnBeginPrint);
			printDocument.PrintPage += new Function<NPrintDocument, NPrintPageEventArgs>(OnPrintPage);
			printDocument.EndPrint += new Function<NPrintDocument, NEndPrintEventArgs>(OnEndPrint);

			NPrintDialog pd = new NPrintDialog();
			pd.EnableCustomPageRange = true;
			pd.EnableCurrentPage = true;
			pd.PrintRangeMode = ENPrintRangeMode.AllPages;
			pd.CustomPageRange = new NRangeI(1, 100);
			pd.NumberOfCopies = 1;
			pd.Collate = true;
			pd.PrintDocument = printDocument;

			pd.RequestShow();
		}
		protected void OnBeginPrint(NPrintDocument sender, NBeginPrintEventArgs e)
		{
		}
		protected void OnEndPrint(NPrintDocument sender, NEndPrintEventArgs e)
		{
		}
		protected void OnPrintPage(NPrintDocument sender, NPrintPageEventArgs e)
		{
			NSize pageSizeDIP = new NSize(this.Width, this.Height);

			try
			{
                double clipW = e.PrintableArea.Width;
                double clipH = e.PrintableArea.Height;

				NRegion clip = NRegion.FromRectangle(new NRectangle(0, 0, clipW, clipH));
                NMatrix transform = new NMatrix(e.PrintableArea.X, e.PrintableArea.Y);

				NPaintVisitor visitor = new NPaintVisitor(e.Graphics, 300, transform, clip);

				// forward traverse the display tree
				VisitDisplaySubtree(visitor);
				
				e.HasMorePages = false;
			}
			catch (Exception ex)
			{
				NMessageBox.Show(null, ex.Message, "Exception", ENMessageBoxButtons.OK, ENMessageBoxIcon.Error);
			}
		}
		protected void OnExportToPdfMenuItemClick(NEventArgs args)
		{
			/*
			#if DEBUG
						string fileName = "c:\\ExampleWindowExport.pdf";
						NSize pageSizeDIP = new NSize(this.Width, this.Height);

						try
						{
							NMargins pageMargins = NMargins.Zero;

							Nevron.Nov.Pdf.NPdfDocument pdfDoc  = new Nevron.Nov.Pdf.NPdfDocument();
							NGraphics2D pdfG = pdfDoc.AddDocumentPage(pageSizeDIP, pageMargins);

							NRegion clip = NRegion.FromRectangle(new NRectangle(0, 0, pageSizeDIP.Width, pageSizeDIP.Height));
							NPaintVisitor visitor = new NPaintVisitor(pdfG, 96, NMatrix.Identity, clip);

							// forward traverse the display tree
							visitor.BeginPainting();
							VisitDisplaySubtree(visitor);
							visitor.EndPainting();

			#if !SILVERLIGHT
							pdfDoc.SaveToFile(fileName, false);
							System.Diagnostics.Process.Start(fileName);
			//#endif
						}
						catch (Exception x)
						{
							NMessageBox.Show(null, x.Message, "Exception", ENMessageBoxButtons.OK);
						}
			//#endif*/

		}
		protected void OnExportWithDirect2DMenuItemClick(NEventArgs args)
		{
#if SUPPORT_DIRECT2D && DEBUG
			string fileName = "d:\\D2D_output.png";
			NSize imgSize = new NSize(this.Width, this.Height);

			try
			{
				Nevron.Windows.DirectX.ND2DGraphicsHelper gh = new Nevron.Windows.DirectX.ND2DGraphicsHelper();
				INGraphics2D pdfG = gh.CreateGraphics((int)imgSize.Width, (int)imgSize.Height);

				NRegion clip = NRegion.FromRectangle(new NRectangle(0, 0, imgSize.Width, imgSize.Height));

				NMatrix canvasTransform = NMatrix.Identity;
				NMatrix invertedCT = canvasTransform;
				invertedCT.Invert();

				NPaintVisitor visitor = new NPaintVisitor(pdfG, 96, invertedCT, clip);
				// assign media

				// forward traverse the display tree
				visitor.BeginPainting();
				VisitDisplaySubtree(visitor);
				visitor.EndPainting();

				gh.SaveToFileAndDispose(fileName);

				System.Diagnostics.Process.Start(fileName);
			}
			catch (Exception x)
			{
				NMessageBox.Show(null, x.Message, "Exception", ENMessageBoxButtons.OK);
			}
#endif
		}
		protected void OnExitMenuItemClick(NEventArgs args)
		{
			NMessageBox.Show(OwnerWindow, "This operation is not supported by the Nevron Open Vision framework", "Error", ENMessageBoxButtons.RetryCancel);
		}

        #endregion

		#region Diagnostics

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnClearBugLog(NEventArgs args)
		{
			//			NSystem.m_ErrorLog.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnDumpBugLog(NEventArgs args)
		{
			/*			NTopLevelWindow wnd = new NTopLevelWindow();
						NApplication.Desktop.Windows.Add(wnd);

						NListBox listBox = new NListBox();

						listBox.MaxWidth = 500;
						listBox.MaxHeight = 500;

						NList<string> errorLog = NSystem.m_ErrorLog;

						for (int i = 0; i < errorLog.Count; i++)
						{
							listBox.Items.Add(new NListBoxItem(errorLog[i]));
						}

						wnd.Content = listBox;
						wnd.Title = "Error";
						wnd.Modal = true;
						wnd.Open();*/
		}
		/// <summary>
		/// Called when the GC Collect button is clicked - collects the garbage
		/// </summary>
		/// <param name="args"></param>
		void OnGCCollectMenuItemClick(NEventArgs args)
		{
			GC.Collect();
		}
		/// <summary>
		/// Called when the Show Application StyleSheets is clicked - shows the NApplicaiton.
		/// </summary>
		/// <param name="arg1"></param>
		void OnShowApplicationStyleSheetsClick(NEventArgs arg1)
		{
			NDesigner designer = NDesigner.GetDesigner(NApplication.Desktop.OwnerDocument.StyleSheets);
			NEditor editor = designer.CreateInstanceEditor(NApplication.Desktop.OwnerDocument.StyleSheets);

			NEditorWindow window = new NEditorWindow();
			window.RemoveFromParentOnClose = true;
			window.Editor = editor;

			NApplication.Desktop.Windows.Add(window);
			window.Open();
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnShowRepaintAreasMenuCheckCheckedChanged(NValueChangeEventArgs args)
		{
			NUISettings.ShowRepaintAreas = ((NCheckableMenuItem)args.TargetNode).Checked;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnShowPaintCacheAreasMenuCheckCheckedChanged(NValueChangeEventArgs args)
		{
			NUISettings.ShowPaintCacheAreas = ((NCheckableMenuItem)args.TargetNode).Checked;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnEnablePaintCacheMenuCheckCheckedChanged(NValueChangeEventArgs args)
		{
			NUISettings.EnablePaintCache = ((NCheckableMenuItem)args.TargetNode).Checked;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		void OnEnableMultiThreadedPaintingCheckedChanged(NValueChangeEventArgs args)
		{
			NUISettings.EnableMultiThreadedPainting = ((NCheckableMenuItem)args.TargetNode).Checked;
		}

		#endregion

		#endregion

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
				NXmlElement element = selectedItem.Tag as NXmlElement;
				if (element != null)
				{
					LoadExample(element);
				}
			}
		}
		private void OnHomeButtonClick(NEventArgs arg)
		{
			CloseExample();
		}
		private void OnBreadcrumbButtonClick(NEventArgs arg)
		{
			// Close the old example
			CloseExample();

			// Load the new example
			NButton button = (NButton)arg.CurrentTargetNode;
			NXmlElement element = (NXmlElement)button.Tag;
			InitForElement(element, false);
		}
		private void OnTreeViewItemMouseDown(NMouseButtonEventArgs arg)
		{
			if (arg.Cancel || arg.Button != ENMouseButtons.Right)
				return;

			// Mark the event as handled
			arg.Cancel = true;

			// Get the right clicked tree view item
			NTreeViewItem item = (NTreeViewItem)arg.CurrentTargetNode;

			// Create the context menu
			NMenu contextMenu = new NMenu();
			NMenuItem copyLinkToClipboard = new NMenuItem("Copy link to clipboard");
			copyLinkToClipboard.Click += OnCopyLinkToClipboardClick;
			copyLinkToClipboard.Tag = item.Tag;
			contextMenu.Items.Add(copyLinkToClipboard);

			// Show the context menu
			NSplitter splitter = (NSplitter)m_TreeView.ParentNode.ParentNode;
			double x = splitter.X + m_TreeView.X + arg.CurrentTargetPosition.X;
			double y = splitter.Y + m_TreeView.Y + item.YInRootItems + arg.CurrentTargetPosition.Y;
			NPopupWindow.OpenInContext(new NPopupWindow(contextMenu), m_TreeView, new NPoint(x, y));
		}
		private void OnCopyLinkToClipboardClick(NEventArgs arg)
		{
			NDataObject dataObject = new NDataObject();
			NXmlElement element = (NXmlElement)arg.CurrentTargetNode.Tag;
			dataObject.SetData(NDataFormat.TextFormat, m_ExamplesPath + "?example=" + element.GetAttributeValue("type"));
			NClipboard.SetDataObject(dataObject);
		}

		#endregion

		#region Fields

		private NToolBar m_Toolbar;
		private NSplitter m_Splitter;
		private NTreeView m_TreeView;
		private NButton m_HomeButton;
		private NLabel m_StatusLabel;
		private NCheckableMenuItem m_CurrentThemeMenuItem;
		private string m_ExamplesPath;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NExampleHost.
		/// </summary>
		public static readonly NSchema NExampleHostSchema;

		#endregion

		#region Static Methods

		/// <summary>
		/// Checks whether the given XML element represents a tile, which contains only one example.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		internal static bool IsSingleExampleTile(NXmlElement element)
		{
			return element.Name == "tile" && element.ChildrenCount == 1 &&
				element.GetChildAt(0).Name == "example";
		}

		/// <summary>
		/// Gets a list of elements that represent the breadcrumb (path) to the given element
		/// from the root.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		private static NList<NXmlElement> GetBreadcrumbPath(NXmlElement element)
		{
			NList<NXmlElement> list = new NList<NXmlElement>();
			list.Add(element);
			element = (NXmlElement)element.Parent;

			while (element.Name != "document")
			{
				// A tile with only 1 child or a tile single child of a group should not be added to the breadcrumb
				if (IsPartOfBreadcrumb(element))
				{
					list.Add(element);
				}

				element = (NXmlElement)element.Parent;
			}

			list.Reverse();
			return list;
		}
		private static bool IsPartOfBreadcrumb(NXmlElement element)
		{
			if (element.Name == "group" && element.ChildrenCount == 1)
				return false;

			return true;
		}
		private static NTreeViewItem GetDefaultExampleItem(NTreeViewItem item, int maxDepth, int depth)
		{
			NXmlElement xmlElement = item.Tag as NXmlElement;
			if (xmlElement != null && xmlElement.GetAttributeValue("default") == "true")
			{
				return item;
			}

			if (depth < maxDepth)
			{
				depth++;
				NTreeViewItemCollection items = item.Items;
				for (int i = 0, count = items.Count; i < count; i++)
				{
					NTreeViewItem defaultItem = GetDefaultExampleItem(items[i], maxDepth, depth);
					if (defaultItem != null)
						return defaultItem;
				}
			}

			return null;
		}
		private static NTreeViewItem GetFirstExampleItem(NTreeViewItem item)
		{
			while (item.Tag is NXmlElement == false)
			{
				item = item.Items[0];
			}

			return item;
		}

		#endregion

		#region Constants

		private const double TreeViewPaneWidth = 230;

		#endregion
	}
}
