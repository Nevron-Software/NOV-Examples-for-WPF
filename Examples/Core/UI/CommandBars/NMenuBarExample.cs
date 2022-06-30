using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMenuBarExample : NExampleBase
	{
		#region Constructors

		public NMenuBarExample()
		{
		}
		static NMenuBarExample()
		{
			NMenuBarExampleSchema = NSchema.Create(typeof(NMenuBarExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_MenuBar = new NMenuBar();
			m_MenuBar.Text = "My Menu";
			m_MenuBar.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_MenuBar.VerticalPlacement = ENVerticalPlacement.Top;

			m_MenuBar.Items.Add(CreateFileMenuDropDown());
			m_MenuBar.Items.Add(CreateEditMenuDropDown());
			m_MenuBar.Items.Add(CreateViewMenuDropDown());

			m_MenuBar.AddEventHandler(NMenuPopupHost.ClickEvent, new NEventHandler<NEventArgs>(new Function<NEventArgs>(OnMenuItemClicked)));

			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.TopToBottom;
			stack.Add(m_MenuBar);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_MenuBar).CreatePropertyEditors(
				m_MenuBar,
				NMenuBar.OrientationProperty,
				NMenuBar.OpenPopupsOnMouseInProperty,
				NMenuBar.ClosePopupsOnMouseOutProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);
			NTrace.WriteLine("Create Menu Example Controls");
			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create drop down menus, nested menu items, menu separators and checkable menu items.
	The example also shows how to create a set of menu items, which behaves like a radio button group, i.e. when you click
	on one of the checkable menu items the previously checked one becomes unchecked.
</p>
";
		}

		#endregion

		#region Implementation

		private NMenuDropDown CreateFileMenuDropDown()
		{
			NMenuDropDown fileMenu = CreateMenuDropDown("File");

			NMenuItem newMenuItem = new NMenuItem("New");
			fileMenu.Items.Add(newMenuItem);
			newMenuItem.Items.Add(new NMenuItem("Project"));
			newMenuItem.Items.Add(new NMenuItem("Web Site"));
			newMenuItem.Items.Add(new NMenuItem("File"));

            NMenuItem openMenuItem = new NMenuItem(NResources.Image_ToolBar_16x16_Open_png, "Open");
            fileMenu.Items.Add(openMenuItem);
            openMenuItem.Items.Add(new NMenuItem("Project"));
            openMenuItem.Items.Add(new NMenuItem("Web Site"));
            openMenuItem.Items.Add(new NMenuItem("File"));

			fileMenu.Items.Add(new NMenuItem("Save"));
			fileMenu.Items.Add(new NMenuItem("Save As..."));

			fileMenu.Items.Add(new NMenuSeparator());
			fileMenu.Items.Add(new NMenuItem("Exit"));

			return fileMenu;
		}
		private NMenuDropDown CreateEditMenuDropDown()
		{
			NMenuDropDown editMenu = CreateMenuDropDown("Edit");

			editMenu.Items.Add(new NMenuItem("Undo"));
			editMenu.Items.Add(new NMenuItem("Redo"));
			editMenu.Items.Add(new NMenuSeparator());
			editMenu.Items.Add(new NMenuItem("Cut"));
			editMenu.Items.Add(new NMenuItem("Copy"));
			editMenu.Items.Add(new NMenuItem("Paste"));

			return editMenu;
		}
		private NMenuDropDown CreateViewMenuDropDown()
		{
			NMenuDropDown viewMenu = CreateMenuDropDown("View");

			m_ViewLayoutMenuItems = new NCheckableMenuItem[] {
				new NCheckableMenuItem(null, "Normal Layout", true),
				new NCheckableMenuItem("Web Layout"),
				new NCheckableMenuItem("Print Layout"),
				new NCheckableMenuItem("Reading Layout")
			};

			for (int i = 0, count = m_ViewLayoutMenuItems.Length; i < count; i++)
			{
				NCheckableMenuItem viewLayoutMenuItem = m_ViewLayoutMenuItems[i];
				viewLayoutMenuItem.CheckedChanging += new Function<NValueChangeEventArgs>(OnViewLayoutMenuItemCheckedChanging);
				viewLayoutMenuItem.CheckedChanged += new Function<NValueChangeEventArgs>(OnViewLayoutMenuItemCheckedChanged);
				viewMenu.Items.Add(viewLayoutMenuItem);
			}

			viewMenu.Items.Add(new NMenuSeparator());
			viewMenu.Items.Add(new NCheckableMenuItem(null, "Task Pane", true));
			viewMenu.Items.Add(new NCheckableMenuItem(null, "Toolbars", false));
			viewMenu.Items.Add(new NCheckableMenuItem(null, "Ruler", true));

			return viewMenu;
		}

		#endregion

		#region Protected - Event Handlers

		private void OnMenuItemClicked(NEventArgs args)
		{
			NMenuPopupHost itemBase = (NMenuPopupHost)args.TargetNode;
			if (itemBase.Content is NLabel)
			{
				m_EventsLog.LogEvent(((NLabel)itemBase.Content).Text + " Clicked");
			}
			else
			{
				m_EventsLog.LogEvent(itemBase.ToString() + " Clicked");
			}
		}
		private void OnViewLayoutMenuItemCheckedChanging(NValueChangeEventArgs args)
		{
			bool isChecked = (bool)args.NewValue;
			if (isChecked)
				return;

			// Make sure the user is not trying to uncheck the checked item
			NCheckableMenuItem item = (NCheckableMenuItem)args.TargetNode;
			for (int i = 0, count = m_ViewLayoutMenuItems.Length; i < count; i++)
			{
				NCheckableMenuItem currentItem = m_ViewLayoutMenuItems[i];
				if (currentItem != item && currentItem.Checked)
					return;
			}

			args.Cancel = true;
		}
		private void OnViewLayoutMenuItemCheckedChanged(NValueChangeEventArgs args)
		{
			bool isChecked = (bool)args.NewValue;
			if (isChecked == false)
				return;
	
			NCheckableMenuItem item = (NCheckableMenuItem)args.TargetNode;
			for (int i = 0, count = m_ViewLayoutMenuItems.Length; i < count; i++)
			{
				NCheckableMenuItem currentItem = m_ViewLayoutMenuItems[i];
				if (currentItem != item && currentItem.Checked)
				{
					// We've found the previously checked item, so uncheck it
					currentItem.Checked = false;
					break;
				}
			}
		}

		#endregion

		#region Fields

		private NMenuBar m_MenuBar;
		private NExampleEventsLog m_EventsLog;
		private NCheckableMenuItem[] m_ViewLayoutMenuItems;

		#endregion

		#region Schema

		public static readonly NSchema NMenuBarExampleSchema;

		#endregion

		#region Static Methods

		private static NMenuDropDown CreateMenuDropDown(string text)
		{
			NMenuDropDown menuDropDown = new NMenuDropDown(text);
			NCommandBar.SetText(menuDropDown, text);
			NCommandBar.SetImage(menuDropDown, null);
			return menuDropDown;
		}

		#endregion
	}
}