using System;

using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NToolBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NToolBarExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NToolBarExample()
		{
			NToolBarExampleSchema = NSchema.Create(typeof(NToolBarExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ToolBar = new NToolBar();
			m_ToolBar.Text = "My Toolbar";
			m_ToolBar.VerticalPlacement = ENVerticalPlacement.Top;

			return m_ToolBar;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;
			
			// Create the tool bar button type radio group
			NStackPanel buttonTypeStack = new NStackPanel();

			ENToolBarButtonType[] buttonTypes = NEnum.GetValues<ENToolBarButtonType>();
			for (int i = 0, count = buttonTypes.Length; i < count; i++)
			{
				// Get the current button type and its string representation
				ENToolBarButtonType buttonType = buttonTypes[i];
				string text = NStringHelpers.InsertSpacesBeforeUppersAndDigits(buttonType.ToString());

				// Create a radio button for the current button type
				NRadioButton radioButton = new NRadioButton(text);
				buttonTypeStack.Add(radioButton);
			}

			NRadioButtonGroup buttonTypeGroup = new NRadioButtonGroup(buttonTypeStack);
			buttonTypeGroup.SelectedIndexChanged += OnButtonTypeGroupSelectedIndexChanged;
			buttonTypeGroup.SelectedIndex = 2;
			stack.Add(new NGroupBox("Button Type", buttonTypeGroup));

			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a toolbar and populate it with buttons, which consist of image and text.
	Using the controls to the right you can change the size and the visibility of the images, which will result in
	recreation of the toolbar buttons.
</p>
";
		}

		#endregion

		#region Implementation

		private NImage GetSmallImage(string text)
		{
			string imageName = "RIMG_ToolBar_16x16_" + text.Replace(" ", String.Empty) + "_png";
			NEmbeddedResourceRef resourceRef = new NEmbeddedResourceRef(NResources.Instance, imageName);
			return new NImage(resourceRef);
		}
		private NImage GetLargeImage(string text)
		{
			string imageName = "RIMG_ToolBar_32x32_" + text.Replace(" ", String.Empty) + "_png";
			NEmbeddedResourceRef resourceRef = new NEmbeddedResourceRef(NResources.Instance, imageName);
			return new NImage(resourceRef);
		}
		private NPairBox CreatePairBox(NImage image, string text)
		{
			NPairBox pairBox = new NPairBox(image, text);
			pairBox.Box2.VerticalPlacement = ENVerticalPlacement.Center;
			pairBox.Spacing = 3;
			return pairBox;
		}
		private NButton CreateButton(ENToolBarButtonType buttonType, string text)
		{
			NButton button = null;
			NImage image = null;

			switch (buttonType)
			{
				case ENToolBarButtonType.Text:
					button = new NButton(text);
					break;
				case ENToolBarButtonType.SmallIcon:
					image = GetSmallImage(text);
					button = new NButton(image);
					break;
				case ENToolBarButtonType.SmallIconAndText:
					image = GetSmallImage(text);
					button = new NButton(CreatePairBox(image, text));
					break;
				case ENToolBarButtonType.LargeIcon:
					image = GetLargeImage(text);
					button = new NButton(image);
					break;
				case ENToolBarButtonType.LargeIconAndText:
					image = GetLargeImage(text);
					button = new NButton(CreatePairBox(image, text));
					break;
				default:
					throw new Exception("New ENToolBarButtonType?");
			}

			NCommandBar.SetText(button, text);
			NCommandBar.SetImage(button, image);

			button.Click += new Function<NEventArgs>(OnToolBarButtonClick);
			return button;
		}
		private void RecreateToolBarButtons(ENToolBarButtonType buttonType)
		{
			m_ToolBar.Items.Clear();

			for (int i = 0, buttonCount = ButtonTexts.Length; i < buttonCount; i++)
			{
				string buttonText = ButtonTexts[i];
				if (buttonText == null || buttonText.Length == 0)
				{
					m_ToolBar.Items.Add(new NCommandBarSeparator());
				}
				else
				{
					m_ToolBar.Items.Add(CreateButton(buttonType, buttonText));
				}
			}

			m_ToolBar.Items.Add(new NMenuDropDown("Test"));
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Called when the user has checked a new button type radio button.
		/// </summary>
		/// <param name="arg"></param>
		private void OnButtonTypeGroupSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			RecreateToolBarButtons((ENToolBarButtonType)(int)arg.NewValue);
		}
		/// <summary>
		/// Occurs when the user clicks on a tool bar button.
		/// </summary>
		/// <param name="args"></param>
		private void OnToolBarButtonClick(NEventArgs args)
		{
			string buttonText = NCommandBar.GetText((NButton)args.TargetNode);
			m_EventsLog.LogEvent("<" + buttonText + "> button clicked");
		}

		#endregion

		#region Fields

		private NToolBar m_ToolBar;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NToolBarExample.
		/// </summary>
		public static readonly NSchema NToolBarExampleSchema;

		#endregion

		#region Constants

		private static readonly string[] ButtonTexts = new string[] { "Open", "Save", "Save As", null, "Print", "Options", null, "Help" };

		#endregion

		#region Nested Types

		public enum ENToolBarButtonType
		{
			Text,
			SmallIcon,
			SmallIconAndText,
			LargeIcon,
			LargeIconAndText
		}

		#endregion
	}
}