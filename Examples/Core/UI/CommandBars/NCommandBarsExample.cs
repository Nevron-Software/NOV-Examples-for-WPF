using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NCommandBarsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NCommandBarsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NCommandBarsExample()
		{
			NCommandBarsExampleSchema = NSchema.Create(typeof(NCommandBarsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example
		
		protected override NWidget CreateExampleContent()
		{
			NCommandBarManager manager = new NCommandBarManager();

			// create two lanes
			NCommandBarLane lane0 = new NCommandBarLane();
			manager.TopDock.Add(lane0);

			NCommandBarLane lane1 = new NCommandBarLane();
			manager.TopDock.Add(lane1);

			NCommandBarLane lane2 = new NCommandBarLane();
			manager.TopDock.Add(lane2);
			NCommandBarLane lane3 = new NCommandBarLane();
			manager.TopDock.Add(lane3);

			// create a menu bar in the first lane
			NMenuBar menuBar = new NMenuBar();
			lane0.Add(menuBar);

			menuBar.Items.Add(CreateFileMenu());
			menuBar.Items.Add(CreateEditMenu());
			menuBar.Items.Add(CreateViewMenu());
			menuBar.Text = "Main Menu";

			//Create File toolbar.
			NToolBar fileToolBar = new NToolBar();
			lane1.Add(fileToolBar);
			fileToolBar.Text = "File";

			AddToolBarItem(fileToolBar, Nevron.Nov.Presentation.NResources.Image_File_New_png, null, "New");
			AddToolBarItem(fileToolBar, Nevron.Nov.Presentation.NResources.Image_File_Open_png, null, "Open");
			fileToolBar.Items.Add(new NCommandBarSeparator());
			AddToolBarItem(fileToolBar, Nevron.Nov.Presentation.NResources.Image_File_Save_png, null, "Save...");
			AddToolBarItem(fileToolBar, Nevron.Nov.Presentation.NResources.Image_File_SaveAs_png, null, "Save As...");
			
			//Create Edit toolbar.
			NToolBar editToolBar = new NToolBar();
			lane1.Add(editToolBar);
			editToolBar.Text = "Edit";

			AddToolBarItem(editToolBar, Nevron.Nov.Presentation.NResources.Image_Edit_Undo_png, "Undo");
			AddToolBarItem(editToolBar, Nevron.Nov.Presentation.NResources.Image_Edit_Redo_png, "Redo");
			editToolBar.Items.Add(new NCommandBarSeparator());
			AddToolBarItem(editToolBar, Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png, "Copy");
			AddToolBarItem(editToolBar, Nevron.Nov.Presentation.NResources.Image_Edit_Cut_png, "Cut");
			AddToolBarItem(editToolBar, Nevron.Nov.Presentation.NResources.Image_Edit_Paste_png, "Paste");

			//Create View toolbar.
			NToolBar viewToolBar = new NToolBar();
			lane1.Add(viewToolBar);
			viewToolBar.Text = "View";

			//Add toggle buttons in a toggle button group which acts like radio buttons.
			AddToggleToolBarItem(viewToolBar, Nevron.Nov.Text.NResources.Image_Layout_Normal_png, "Normal Layout");
			AddToggleToolBarItem(viewToolBar, Nevron.Nov.Text.NResources.Image_Layout_Web_png, "Web Layout");
			AddToggleToolBarItem(viewToolBar, Nevron.Nov.Text.NResources.Image_Layout_Print_png, "Print Layout");

			viewToolBar.Items.Add(new NCommandBarSeparator());
			AddToolBarItem(viewToolBar, null, "Task Pane");
			AddToolBarItem(viewToolBar, null, "Toolbars");
			AddToolBarItem(viewToolBar, null, "Ruller");

			NToolBar toolbar = new NToolBar();
			lane2.Add(toolbar);
			toolbar.Text = "Toolbar";
			toolbar.Wrappable = true;

			NColorBox colorBoxItem = new NColorBox();
			colorBoxItem.Tooltip = new NTooltip("Select Color");
			NCommandBar.SetText(colorBoxItem, "Select Color");
			toolbar.Items.Add(colorBoxItem);

			NMenuSplitButton splitButton = new NMenuSplitButton();			
			splitButton.ActionButton.Content = NWidget.FromObject("Send/Receive");			
			splitButton.Menu.Items.Add(new NMenuItem("Send Receive All"));
			splitButton.SelectedIndexChanged += OnSplitButtonSelectedIndexChanged;
			splitButton.Menu.Items.Add(new NMenuItem("Send All"));
			splitButton.Menu.Items.Add(new NMenuItem("Receive All"));

			toolbar.Items.Add(splitButton);

			//Add toggle button which enable/disables the next fill split button.
			NToggleButton toggleButton = new NToggleButton("Enable");			
			toggleButton.CheckedChanged += OnToggleButtonCheckedChanged;
			toolbar.Items.Add(toggleButton);

			// Add fill split button
			NFillSplitButton fillButton = new NFillSplitButton();
			fillButton.Tooltip = new NTooltip("Select Fill");
			fillButton.Enabled = false;
			toolbar.Items.Add(fillButton);

			// Add shadow split button
			NShadowSplitButton shadowButton = new NShadowSplitButton();
			shadowButton.Tooltip = new NTooltip("Select Shadow");
			toolbar.Items.Add(shadowButton);

			// Add stroke split button
			NStrokeSplitButton strokeButton = new NStrokeSplitButton();
			strokeButton.Tooltip = new NTooltip("Select Stroke");
			toolbar.Items.Add(strokeButton);			
			
			manager.Content = new NLabel("Content Goes Here");
			manager.Content.AllowFocus = true;
			manager.Content.MouseDown += new Function<NMouseButtonEventArgs>(OnContentMouseDown);
			manager.Content.Border = NBorder.CreateFilledBorder(NColor.Black);
			manager.Content.BackgroundFill = new NColorFill(NColor.White);
			manager.Content.BorderThickness = new NMargins(1);
			manager.Content.GotFocus += new Function<NFocusChangeEventArgs>(OnContentGotFocus);
			manager.Content.LostFocus += new Function<NFocusChangeEventArgs>(OnContentLostFocus);

			return manager;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and populate various types of command bars such as menu bars and toolbars.
</p>
";
		}

		#endregion

		#region Implementation

		private NMenuDropDown CreateFileMenu()
		{
			NMenuDropDown file = new NMenuDropDown("File");

			NMenuItem newMenuItem = CreateMenuItem("New", Nevron.Nov.Presentation.NResources.Image_File_New_png);
			file.Items.Add(newMenuItem);

			newMenuItem.Items.Add(new NMenuItem("Project"));
			newMenuItem.Items.Add(new NMenuItem("Web Site"));
			newMenuItem.Items.Add(new NMenuItem("File"));

			NMenuItem openMenuItem = CreateMenuItem("Open", Nevron.Nov.Presentation.NResources.Image_File_Open_png);
			file.Items.Add(openMenuItem);

			openMenuItem.Items.Add(new NMenuItem("Project"));
			openMenuItem.Items.Add(new NMenuItem("Web Site"));
			openMenuItem.Items.Add(new NMenuItem("File"));

			file.Items.Add(new NMenuSeparator());

			file.Items.Add(CreateMenuItem("Save", Nevron.Nov.Presentation.NResources.Image_File_Save_png));
			file.Items.Add(CreateMenuItem("Save As...", Nevron.Nov.Presentation.NResources.Image_File_SaveAs_png));

			return file;
		}
		private NMenuDropDown CreateEditMenu()
		{
			NMenuDropDown edit = new NMenuDropDown("Edit");

			edit.Items.Add(CreateMenuItem("Undo", Nevron.Nov.Presentation.NResources.Image_Edit_Undo_png));
			edit.Items.Add(CreateMenuItem("Redo", Nevron.Nov.Presentation.NResources.Image_Edit_Redo_png));
			edit.Items.Add(new NMenuSeparator());
			edit.Items.Add(CreateMenuItem("Cut", Nevron.Nov.Presentation.NResources.Image_Edit_Cut_png));
			edit.Items.Add(CreateMenuItem("Copy", Nevron.Nov.Presentation.NResources.Image_Edit_Copy_png));
			edit.Items.Add(CreateMenuItem("Paste", Nevron.Nov.Presentation.NResources.Image_Edit_Paste_png));

			return edit;
		}
		private NMenuDropDown CreateViewMenu()
		{
			NMenuDropDown view = new NMenuDropDown("View");
			view.Items.Add(CreateCheckableMenuItem("Normal", Nevron.Nov.Text.NResources.Image_Layout_Normal_png));
			view.Items.Add(CreateCheckableMenuItem("Web Layout", Nevron.Nov.Text.NResources.Image_Layout_Web_png));
			view.Items.Add(CreateCheckableMenuItem("Print Layout", Nevron.Nov.Text.NResources.Image_Layout_Print_png));
			view.Items.Add(new NMenuSeparator());
			view.Items.Add(new NMenuItem("Task Pane"));
			view.Items.Add(new NMenuItem("Toolbars"));
			view.Items.Add(new NMenuItem("Ruler"));

			return view;
		}
		private void AddToggleToolBarItem(NToolBar toolBar, NImage image, string tooltip)
		{
			NToggleButton item = new NToggleButton(image);
			item.Tooltip = new NTooltip(tooltip);
			NCommandBar.SetText(item, tooltip);
			NCommandBar.SetImage(item, image);
			toolBar.Items.Add(item);			
		}
		private void AddToolBarItem(NToolBar toolBar, NImage image)
		{
			AddToolBarItem(toolBar, image, null);
		}
		private void AddToolBarItem(NToolBar toolBar, NImage image, string text)
		{
			AddToolBarItem(toolBar, image, text, text);
		}
		private void AddToolBarItem(NToolBar toolBar, NImage image, string text, string tooltip)
		{
			NWidget item;
			if (text == null)
			{
				text = string.Empty;
			}

			if (image == null)
			{
				item = new NButton(text);
			}
			else
			{
				item = new NButton(NPairBox.Create(image, text));

			}

			if (!string.IsNullOrEmpty(tooltip))
			{
				item.Tooltip = new NTooltip(tooltip);
			}

			toolBar.Items.Add(item);

			NCommandBar.SetText(item, text);

			if (image != null)
			{
				NCommandBar.SetImage(item, (NImage)image.DeepClone());
			}
		}
		private NMenuItem CreateMenuItem(string text, NImage image)
		{
			if (image == null)
			{
				return new NMenuItem(text);
			}
			else
			{
				NImageBox imageBox = new NImageBox(image);
				imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
				imageBox.VerticalPlacement = ENVerticalPlacement.Center;
				return new NMenuItem(imageBox, text);
			}
		}
		private NCheckableMenuItem CreateCheckableMenuItem(string text, NImage image)
		{
			NCheckableMenuItem item = new NCheckableMenuItem(text);
			item.CheckedChanging += item_CheckedChanging;
			item.CheckedChanged += item_CheckedChanged;

			if (image != null)
			{				
				NImageBox imageBox = new NImageBox(image);
				imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
				imageBox.VerticalPlacement = ENVerticalPlacement.Center;
				item.Header = imageBox;
				item.Content = new NLabel(text);
			}

			return item;
		}
				
		#endregion

		#region Event Handlers

		private void OnContentLostFocus(NFocusChangeEventArgs args)
		{
			(args.TargetNode as NLabel).Border = NBorder.CreateFilledBorder(NColor.Black);
		}
		private void OnContentGotFocus(NFocusChangeEventArgs args)
		{
			(args.TargetNode as NLabel).Border = NBorder.CreateFilledBorder(NColor.Red);
		}
		private void OnContentMouseDown(NMouseButtonEventArgs args)
		{
			(args.TargetNode as NLabel).Focus();
		}
		private void OnSplitButtonSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NMenuSplitButton menuSplitButton = (NMenuSplitButton)arg.CurrentTargetNode;
			NMenuItem menuItem = (NMenuItem)menuSplitButton.Items[menuSplitButton.SelectedIndex];
			NLabel label = (NLabel)menuItem.Content;
			menuSplitButton.ActionButton.Content = NWidget.FromObject(label.Text);
		}
		private void OnToggleButtonCheckedChanged(NValueChangeEventArgs arg)
		{
			NToggleButton button = arg.CurrentTargetNode as NToggleButton;
			if (button == null)
			{
				return;
			}

			//Get the toolbar items collection and enable or disable the next button in the collection
			NCommandBarItemCollection toolbarItems = button.ParentNode as NCommandBarItemCollection;
			if (toolbarItems == null)
			{
				return;
			}

			int buttonIndex = toolbarItems.IndexOfChild(button);
			NFillSplitButton fillSplitButton = toolbarItems[buttonIndex + 1] as NFillSplitButton;
			if (fillSplitButton == null)
			{
				return;
			}

			if (button.Checked)
			{
				fillSplitButton.Enabled = true;
				((NLabel)button.Content).Text = "Disable";
			}
			else
			{
				fillSplitButton.Enabled = false;
				((NLabel)button.Content).Text = "Enable";
			}
		}
		void item_CheckedChanging(NValueChangeEventArgs arg)
		{
			bool isChecked = (bool)arg.NewValue;
			if (isChecked)
				return;

			// Make sure the user is not trying to uncheck the checked item
			NCheckableMenuItem item = (NCheckableMenuItem)arg.TargetNode;
			NMenuItemCollection items = item.ParentNode as NMenuItemCollection;
			
			for (int i = 0, count = items.Count; i < count; i++)
			{
				NCheckableMenuItem currentItem = items[i] as NCheckableMenuItem;
				if (currentItem == null)
				{
					continue;
				}

				if (currentItem != item && currentItem.Checked)
					return;
			}

			arg.Cancel = true;
		}

		void item_CheckedChanged(NValueChangeEventArgs arg)
		{
			bool isChecked = (bool)arg.NewValue;
			if (isChecked == false)
				return;

			NCheckableMenuItem item = (NCheckableMenuItem)arg.TargetNode;
			NMenuItemCollection items = item.ParentNode as NMenuItemCollection;

			for (int i = 0, count = items.Count; i < count; i++)
			{
				NCheckableMenuItem currentItem = items[i] as NCheckableMenuItem;
				if (currentItem == null)
				{
					continue;
				}

				if (currentItem != item && currentItem.Checked)
				{
					// We've found the previously checked item, so uncheck it
					currentItem.Checked = false;
					break;
				}
			}
		}
		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NCommandBarsExample.
		/// </summary>
		public static readonly NSchema NCommandBarsExampleSchema;

		#endregion
	}
}