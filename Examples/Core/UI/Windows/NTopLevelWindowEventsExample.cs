using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTopLevelWindowEventsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTopLevelWindowEventsExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTopLevelWindowEventsExample()
		{
			NTopLevelWindowEventsExampleSchema = NSchema.Create(typeof(NTopLevelWindowEventsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChildWindowIndex = 1;

			// Create the example's content
			NButton openWindowButton = new NButton("Open Window...");
			openWindowButton.HorizontalPlacement = ENHorizontalPlacement.Left;
			openWindowButton.VerticalPlacement = ENVerticalPlacement.Top;
			openWindowButton.Click += new Function<NEventArgs>(OnOpenChildWindowButtonClicked);

			return openWindowButton;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.First;
			stack.FillMode = ENStackFillMode.First;

			// Create the opened windows tree view
			m_TreeView = new NTreeView();
			m_TreeView.SelectedPathChanged += new Function<NValueChangeEventArgs>(OnTreeViewSelectedPathChanged);
			stack.Add(m_TreeView);

			// create some command buttons
			m_ButtonsStack = new NStackPanel();
			m_ButtonsStack.HorizontalSpacing = 3;
			m_ButtonsStack.Direction = ENHVDirection.LeftToRight;
			m_ButtonsStack.Add(new NButton(ActivateButtonText));
			m_ButtonsStack.Add(new NButton(FocusButtonText));
			m_ButtonsStack.Add(new NButton(CloseButtonText));

			// capture the button click for the buttons at stack panel level
			m_ButtonsStack.AddEventHandler(NButton.ClickEvent, new NEventHandler<NEventArgs>(new Function<NEventArgs>(OnWindowActionButtonClicked)));

			m_ButtonsStack.Enabled = false;
			stack.Add(m_ButtonsStack);

			NGroupBox openedWindowsGroupBox = new NGroupBox("Opened Windows", stack);

			// Add the events log
			m_EventsLog = new NExampleEventsLog();

			// Add the opened windows group box and the events log to a splitter
			NSplitter splitter = new NSplitter(openedWindowsGroupBox, m_EventsLog, ENSplitterSplitMode.OffsetFromNearSide, 250);
			splitter.Orientation = ENHVOrientation.Vertical;
			return splitter;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and open and manage top level windows. The events that
	occur during the lifetime of each window are logged and displayed in a list box on the right.
</p>
";
		}
		protected internal override void OnClosing()
		{
			base.OnClosing();

			// Loop through the tree view items to close all opened windows
			NTreeViewItemCollection items = m_TreeView.Items;
			for (int i = items.Count - 1; i >= 0; i--)
			{
				((NTopLevelWindow)items[i].Tag).Close();
			}
		}

		#endregion

		#region Implementation

		private NButton CreateOpenChildWindowButton()
		{
			NButton button = new NButton("Open Child Window...");
			button.HorizontalPlacement = ENHorizontalPlacement.Center;
			button.VerticalPlacement = ENVerticalPlacement.Bottom;
			button.Click += new Function<NEventArgs>(OnOpenChildWindowButtonClicked);

			return button;
		}
		/// <summary>
		/// Creates and opens a child window of the specified owner window.
		/// </summary>
		/// <param name="ownerWindow"></param>
		private void OpenChildWindow(NWindow ownerWindow)
		{
			// Create the window
			NTopLevelWindow window = NApplication.CreateTopLevelWindow(ownerWindow);
			window.Title = "Window " + m_ChildWindowIndex++;
			window.PreferredSize = WindowSize;

			// subscribe for window state events
			window.Opened += new Function<NEventArgs>(OnWindowStateEvent);
			window.Activated += new Function<NEventArgs>(OnWindowStateEvent);
			window.Deactivated += new Function<NEventArgs>(OnWindowStateEvent);
			window.Closing += new Function<NEventArgs>(OnWindowStateEvent);
			window.Closed += new Function<NEventArgs>(OnWindowStateEvent);

			// subscribe for window UI events
			window.GotFocus += new Function<NFocusChangeEventArgs>(OnWindowUIEvent);
			window.LostFocus += new Function<NFocusChangeEventArgs>(OnWindowUIEvent);

			// Create its content
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.First;
			stack.FitMode = ENStackFitMode.First;

			string ownerName = ownerWindow is NTopLevelWindow ? ((NTopLevelWindow)ownerWindow).Title : "Examples Window";
			NLabel label = new NLabel("Child Of \"" + ownerName + "\"");
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;
			stack.Add(label);

			stack.Add(CreateOpenChildWindowButton());
			window.Content = stack;

			// Open the window
			AddTreeViewItemForWindow(window);
			window.Open();

			if (ownerWindow is NTopLevelWindow)
			{
				window.X = ownerWindow.X + 25;
				window.Y = ownerWindow.Y + 25;
			}
		}
		private void AddTreeViewItemForWindow(NTopLevelWindow window)
		{
			NTreeViewItem item = new NTreeViewItem(window.Title);
			item.Tag = window;
			window.Tag = item;

			NTopLevelWindow parentWindow = window.ParentWindow as NTopLevelWindow;
			if (parentWindow == null)
			{
				m_TreeView.Items.Add(item);
			}
			else
			{
				NTreeViewItem parentItem = (NTreeViewItem)parentWindow.Tag;
				parentItem.Items.Add(item);
				parentItem.Expanded = true;
			}
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnOpenChildWindowButtonClicked(NEventArgs args)
		{
			NButton button = (NButton)args.TargetNode;
			OpenChildWindow(button.DisplayWindow);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnTreeViewSelectedPathChanged(NValueChangeEventArgs args)
		{
			NTreeView treeView = (NTreeView)args.TargetNode;
			m_ButtonsStack.Enabled = treeView.SelectedItem != null;
		}
		/// <summary>
		/// Event handler for window state events (Opened, Activated, Deactivated, Closing, Closed)
		/// </summary>
		/// <param name="args"></param>
		private void OnWindowStateEvent(NEventArgs args)
		{
			if (args.EventPhase != ENEventPhase.AtTarget)
				return;

			NTopLevelWindow window = (NTopLevelWindow)args.CurrentTargetNode;
			string eventName = args.Event.Name;
			m_EventsLog.LogEvent(window.Title + " " + eventName.Substring(eventName.LastIndexOf('.') + 1));

			if (args.Event == NTopLevelWindow.ActivatedEvent)
			{
				// Select the corresponding item from the tree view
				NTreeViewItem item = (NTreeViewItem)window.Tag;
				m_TreeView.SelectedItem = item;
			}
			else if (args.Event == NTopLevelWindow.ClosedEvent)
			{
				// Remove the corresponding item from the tree view
				NTreeViewItem item = (NTreeViewItem)window.Tag;
				NTreeViewItemCollection items = (NTreeViewItemCollection)item.ParentNode;
				items.Remove(item);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnWindowUIEvent(NEventArgs args)
		{
			NTopLevelWindow window = (NTopLevelWindow)args.CurrentTargetNode;

			string eventName = args.Event.Name;
			eventName = eventName.Substring(eventName.LastIndexOf('.') + 1);

			m_EventsLog.LogEvent(window.Title + " " + eventName + " from target: " + args.TargetNode.GetType().Name);
		}
		/// <summary>
		/// Called when some of the window action buttons has been clicked.
		/// </summary>
		/// <param name="args"></param>
		private void OnWindowActionButtonClicked(NEventArgs args)
		{
			if (m_TreeView.SelectedItem == null)
				return;

			NTopLevelWindow window = (m_TreeView.SelectedItem.Tag as NTopLevelWindow);
			if (window == null)
				return;

			NButton button = (NButton)args.TargetNode;
			NLabel label = (NLabel)button.Content;

			switch (label.Text)
			{
				case ActivateButtonText:
					window.Activate();
					break;
				case FocusButtonText:
					window.Focus();
					break;
				case CloseButtonText:
					window.Close();
					break;
			}
		}

		#endregion

		#region Fields

		private int m_ChildWindowIndex;
		private NTreeView m_TreeView;
		private NStackPanel m_ButtonsStack;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTopLevelWindowEventsExample.
		/// </summary>
		public static readonly NSchema NTopLevelWindowEventsExampleSchema;

		#endregion

		#region Constants

		private static readonly NSize WindowSize = new NSize(300, 300);

		private const string ActivateButtonText = "Activate";
		private const string FocusButtonText = "Focus";
		private const string CloseButtonText = "Close";

		#endregion
	}
}