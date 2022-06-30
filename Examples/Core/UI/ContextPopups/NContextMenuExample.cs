using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NContextMenuExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NContextMenuExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NContextMenuExample()
		{
			NContextMenuExampleSchema = NSchema.Create(typeof(NContextMenuExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_TextChecked = new bool[] { true, true, false };
			m_ImageAndTextChecked = new bool[] { true, false, true };

			NStackPanel leftStack = new NStackPanel();
			leftStack.Add(CreateWidget("Text Only", CreateTextContextMenu));
			leftStack.Add(CreateWidget("Image and Text", CreateImageAndTextContextMenu));
			leftStack.Add(CreateWidget("Checkable Text Only", CreateCheckableTextContextMenu));
			leftStack.Add(CreateWidget("Checkable Image And Text", CreateCheckableImageAndTextContextMenu));

			NStackPanel rightStack = new NStackPanel();
			rightStack.Add(CreateWidget("Text Only", CreateTextContextMenu));
			rightStack.Add(CreateWidget("Image and Text", CreateImageAndTextContextMenu));
			rightStack.Add(CreateWidget("Checkable Text Only", CreateCheckableTextContextMenu));
			rightStack.Add(CreateWidget("Checkable Image And Text", CreateCheckableImageAndTextContextMenu));

			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;
			stack.HorizontalSpacing = 10;

			stack.Add(new NGroupBox("Left Button Context Menu", leftStack));
			stack.Add(new NGroupBox("Right Button Context Menu", rightStack));

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create context menus. It creates several widgets and a context menu
	for each of them. The example shows how to create different types of menu items such as text only
	menu items, menu items with image and text, checkable text only menu items and checkable image and
	text menu items.
</p>";
		}

		#endregion

		#region Implementation

		private NWidget CreateWidget(string text, CreateMenuDelegate createMenuDelegate)
		{
			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;
			label.TextFill = new NColorFill(NColor.Black);

			NContentHolder widget = new NContentHolder(label);
			widget.HorizontalPlacement = ENHorizontalPlacement.Left;
			widget.VerticalPlacement = ENVerticalPlacement.Top;
			widget.BackgroundFill = new NColorFill(NColor.PapayaWhip);
			widget.Border = NBorder.CreateFilledBorder(NColor.Black);
			widget.BorderThickness = new NMargins(1);
			widget.PreferredSize = new NSize(200, 100);
			widget.Tag = createMenuDelegate;
			widget.MouseDown += new Function<NMouseButtonEventArgs>(OnTargetWidgetMouseDown);

			return widget;
		}

		private NMenu CreateTextContextMenu()
		{
			NMenu contextMenu = new NMenu();
			for (int i = 0; i < 3; i++)
			{
				contextMenu.Items.Add(new NMenuItem("Option " + (i + 1).ToString()));
			}

			return contextMenu;
		}
		private NMenu CreateImageAndTextContextMenu()
		{
			NMenu contextMenu = new NMenu();
			for (int i = 0; i < 3; i++)
			{
				contextMenu.Items.Add(new NMenuItem(MenuItemImages[i], "Option " + (i + 1).ToString()));
			}

			return contextMenu;
		}
		private NMenu CreateCheckableTextContextMenu()
		{
			NMenu contextMenu = new NMenu();
			for (int i = 0; i < 3; i++)
			{
				NCheckableMenuItem menuItem = new NCheckableMenuItem(null, "Option " + (i + 1).ToString(), m_TextChecked[i]);
				menuItem.Tag = i;
				menuItem.CheckedChanged += new Function<NValueChangeEventArgs>(OnTextCheckableMenuItemCheckedChanged);
				contextMenu.Items.Add(menuItem);
			}

			return contextMenu;
		}
		private NMenu CreateCheckableImageAndTextContextMenu()
		{
			NMenu contextMenu = new NMenu();

			for (int i = 0; i < 3; i++)
			{
				NCheckableMenuItem menuItem = new NCheckableMenuItem(MenuItemImages[i], "Option " + (i + 1).ToString(), m_ImageAndTextChecked[i]);
				menuItem.Tag = i;
				menuItem.CheckedChanged += new Function<NValueChangeEventArgs>(OnImageAndTextCheckableMenuItemCheckedChanged);
				contextMenu.Items.Add(menuItem);
			}

			return contextMenu;
		}

		#endregion

		#region Event Handlers

		private void OnTextCheckableMenuItemCheckedChanged(NValueChangeEventArgs args)
		{
			int index = (int)args.CurrentTargetNode.Tag;
			m_TextChecked[index] = (bool)args.NewValue;
		}
		private void OnImageAndTextCheckableMenuItemCheckedChanged(NValueChangeEventArgs args)
		{
			int index = (int)args.CurrentTargetNode.Tag;
			m_ImageAndTextChecked[index] = (bool)args.NewValue;
		}
		private void OnTargetWidgetMouseDown(NMouseButtonEventArgs args)
		{
			NGroupBox ownerGroupBox = (NGroupBox)args.CurrentTargetNode.GetFirstAncestor(NGroupBox.NGroupBoxSchema);
			string groupBoxTitle = ((NLabel)ownerGroupBox.Header.Content).Text;

			if ((groupBoxTitle.StartsWith("Left") && args.Button != ENMouseButtons.Left) ||
				(groupBoxTitle.StartsWith("Right") && args.Button != ENMouseButtons.Right))
				return;

			// Mark the event as handled
			args.Cancel = true;

			// Create and show the popup
			CreateMenuDelegate createMenuDelegate = (CreateMenuDelegate)args.CurrentTargetNode.Tag;
			NMenu contextMenu = createMenuDelegate();
			NPopupWindow.OpenInContext(new NPopupWindow(contextMenu), args.CurrentTargetNode, args.ScreenPosition);
		}

		#endregion

		#region Fields

		private bool[] m_TextChecked;
		private bool[] m_ImageAndTextChecked;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NContextMenuExample.
		/// </summary>
		public static readonly NSchema NContextMenuExampleSchema;

		#endregion

		#region Constants

		private static readonly NImage[] MenuItemImages = new NImage[]{
			NResources.Image__16x16_Calendar_png,
			NResources.Image__16x16_Contacts_png,
			NResources.Image__16x16_Mail_png
		};

		#endregion

		#region Nested Types

		private delegate NMenu CreateMenuDelegate();

		#endregion
	}
}