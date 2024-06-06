using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NSingleVisiblePanelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSingleVisiblePanelExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSingleVisiblePanelExample()
		{
			NSingleVisiblePanelExampleSchema = NSchema.Create(typeof(NSingleVisiblePanelExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_SingleVisiblePanel = new NSingleVisiblePanel();
			m_SingleVisiblePanel.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_SingleVisiblePanel.VerticalPlacement = ENVerticalPlacement.Top;
			m_SingleVisiblePanel.PreferredWidth = 400;
			m_SingleVisiblePanel.SetBorder(1, NColor.Red);

            NStackPanel mainStack = new NStackPanel();
			m_SingleVisiblePanel.Add(mainStack);

			mainStack.Add(CreateHeaderLabel("Mobile Computers"));

			for (int i = 0, count = MobileComputers.Length; i < count; i++)
			{
				NMobileCopmuterInfo info = MobileComputers[i];

				// Create the topic's button
				NButton button = new NButton(info.Name);
				button.Tag = i + 1;
				mainStack.Add(button);

				// Create and add the topic's content
				m_SingleVisiblePanel.Add(CreateComputerInfoWidget(info));
			}

			m_SingleVisiblePanel.VisibleIndexChanged += new Function<NValueChangeEventArgs>(OnVisibleIndexValueChanged);
			m_SingleVisiblePanel.AddEventHandler(NButtonBase.ClickEvent, new NEventHandler<NEventArgs>(new Function<NEventArgs>(OnButtonClicked)));

			return m_SingleVisiblePanel;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Add the properties group box
			NList<NPropertyEditor> editors = new NList<NPropertyEditor>(NDesigner.GetDesigner(m_SingleVisiblePanel).CreatePropertyEditors(
				m_SingleVisiblePanel,
				NSingleVisiblePanel.EnabledProperty,
				NSingleVisiblePanel.SizeToVisibleProperty,
				NSingleVisiblePanel.VisibleIndexProperty
			));

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0, count = editors.Count; i < count; i++)
			{
				stack.Add(editors[i]);
			}

			NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
			stack.Add(propertiesGroupBox);

			// Add an events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a single visible panel and add some widgets to it.
	The single visible panel is a panel in which only a single child element can be visible.
	The currently visible child element can be controlled through the <b>VisibleIndex</b> or the
	<b>VisibleElement</b> property. The <b>SizeToVisible</b> property determines whether the panel
	should be sized to the visible element or to all contained elements.  
</p>
";
		}

		#endregion

		#region Implementation

		private NLabel CreateHeaderLabel(string text)
		{
			NLabel label = new NLabel(text);
			label.TextFill = new NColorFill(NColor.Blue);
			label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 16);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			return label;
		}
		private NWidget CreateComputerInfoWidget(NMobileCopmuterInfo info)
		{
			NStackPanel stack = new NStackPanel();
			
			stack.Add(CreateHeaderLabel(info.Name));

			// Create a pair box with the image and the description
			NLabel descriptionLabel = new NLabel(info.Description);
			descriptionLabel.TextWrapMode = ENTextWrapMode.WordWrap;

			NPairBox pairBox = new NPairBox(info.Image, descriptionLabel);
			pairBox.Box1.Border = NBorder.CreateFilledBorder(NColor.Black);
			pairBox.Box1.BorderThickness = new NMargins(1);
			pairBox.Spacing = 5;
			stack.Add(pairBox);

			NButton backButton = new NButton("Back");
			backButton.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			backButton.Tag = 0;
			stack.Add(backButton);

			return stack;
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		private void OnVisibleIndexValueChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Visible child index: " + args.NewValue.ToString());
		}
		/// <summary>
		/// Handler for NButtonBase.Click event.
		/// </summary>
		/// <param name="args"></param>
		private void OnButtonClicked(NEventArgs args)
		{
			if (args.Cancel || !(args.TargetNode is NButton) || !(args.TargetNode.Tag is int))
				return;

			NSingleVisiblePanel singleVisiblePanel = ((NSingleVisiblePanel)args.CurrentTargetNode);
			NButton button = (NButton)args.TargetNode;
			singleVisiblePanel.VisibleIndex = (int)button.Tag;
		}

		#endregion

		#region Fields

		private NSingleVisiblePanel m_SingleVisiblePanel;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSingleVisiblePanelExample.
		/// </summary>
		public static readonly NSchema NSingleVisiblePanelExampleSchema;

		#endregion

		#region Constants

		private static readonly NMobileCopmuterInfo[] MobileComputers = new NMobileCopmuterInfo[] {
			new NMobileCopmuterInfo("Laptop", NResources.Image_MobileComputers_Laptop_jpg, "A laptop, also called a notebook, is a personal computer for mobile use. A laptop integrates most of the typical components of a desktop computer, including a display, a keyboard, a pointing device (a touchpad, also known as a trackpad, and/or a pointing stick) and speakers into a single unit."),
			new NMobileCopmuterInfo("Netbook", NResources.Image_MobileComputers_Netbook_jpg, "Netbooks are a category of small, lightweight, legacy-free, and inexpensive laptop computers. At their inception in late 2007 as smaller notebooks optimized for low weight and low cost — netbooks omitted certain features (e.g., the optical drive), featured smaller screens and keyboards, and offered reduced computing power when compared to a full-sized laptop."),
			new NMobileCopmuterInfo("Smartbook", NResources.Image_MobileComputers_Smartbook_jpg, "A smartbook is a class of mobile device that combine certain features of both a smartphone and netbook computer. Smartbooks feature always on, all-day battery life, 3G, or Wi-Fi connectivity and GPS (all typically found in smartphones) in a laptop or tablet-style body with a screen size of 5 to 10 inches and a physical or soft touchscreen keyboard."),
			new NMobileCopmuterInfo("Tablet", NResources.Image_MobileComputers_Tablet_jpg, "A tablet computer, or simply tablet, is a complete mobile computer, larger than a mobile phone or personal digital assistant, integrated into a flat touch screen and primarily operated by touching the screen. It often uses an onscreen virtual keyboard, a passive stylus pen, or a digital pen, rather than a physical keyboard."),
			new NMobileCopmuterInfo("Ultra-mobile PC", NResources.Image_MobileComputers_UMPC_jpg, "An ultra-mobile PC (ultra-mobile personal computer or UMPC) is a small form factor version of a pen computer which is smaller than a netbook, has a TFT display measuring about 12.7 to 17.8 cm and is operated using a touch screen or a stylus. Lately ultra-mobile PCs have largely been supplanted by tablets."),
			new NMobileCopmuterInfo("Ultrabook", NResources.Image_MobileComputers_Ultrabook_jpg, "An Ultrabook is a computer in a category of thin and lightweight ultraportable laptops, defined by a specification from Intel corporation. Ultrabooks combine the power of ordinary laptops and the portability and battery life of netbooks but this comes at a higher price.")
		};

		#endregion

		#region Nested Types

		private struct NMobileCopmuterInfo
		{
			public NMobileCopmuterInfo(string name, NImage image, string description)
			{
				Name = name;
				Image = image;
				Description = description;
			}

			public string Name;
			public NImage Image;
			public string Description;
		}

		#endregion
	}
}