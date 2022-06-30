using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NListBoxMixedContentExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NListBoxMixedContentExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NListBoxMixedContentExample()
		{
			NListBoxMixedContentExampleSchema = NSchema.Create(typeof(NListBoxMixedContentExample), NExampleBase.NExampleBaseSchema);

			// Properties
			ContentTypeProperty = NListBoxMixedContentExampleSchema.AddSlot("ContentType", typeof(ENListBoxContentType), defaultContentType);
			ContentTypeProperty.AddValueChangedCallback(delegate(NNode t, NValueChangeData d) { ((NListBoxMixedContentExample)t).OnContentTypeChanged(d); });

			// Designer
			NListBoxMixedContentExampleSchema.SetMetaUnit(new NDesignerMetaUnit(typeof(NListBoxMixedContentDesigner)));
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the value of the ContentType property.
		/// </summary>
		public ENListBoxContentType ContentType
		{
			get
			{
				return (ENListBoxContentType)GetValue(ContentTypeProperty);
			}
			set
			{
				SetValue(ContentTypeProperty, value);
			}
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a list box
			m_ListBox = new NListBox();
			m_ListBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ListBox.PreferredSize = new NSize(200, 400);

			// Fill the image Box
			FillWithImageCheckBoxAndTitle();

			// Hook to list box selection events
			m_ListBox.Selection.Selected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemSelected);
			m_ListBox.Selection.Deselected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemDeselected);

			return m_ListBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Create the content type group box
			NPropertyEditor contentTypePropertyEditor = NDesigner.GetDesigner(this).CreatePropertyEditor(this, ContentTypeProperty);
			stack.Add(contentTypePropertyEditor);

			// Create the properties group box
			NStackPanel propertiesStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ListBox).CreatePropertyEditors(m_ListBox,
				NWidget.EnabledProperty,
				NWidget.HorizontalPlacementProperty,
				NWidget.VerticalPlacementProperty,
				NScrollContentBase.HScrollModeProperty,
				NScrollContentBase.VScrollModeProperty,
				NScrollContentBase.NoScrollHAlignProperty,
				NScrollContentBase.NoScrollVAlignProperty,
				NListBox.IntegralVScrollProperty
			);

			for (int i = 0, count = editors.Count; i < count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			NGroupBox propertiesGroupBox = new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack));
			stack.Add(propertiesGroupBox);

			// Create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a list box and add items with various content to it - text only items, items with image and text,
	checkable items and so on. Using the controls to the right you can modify the appearance and the behavior of the list box.
</p>
";
		}

		#endregion

		#region Implementation

		private NListBoxItem CreateListBoxItem(int index, bool hasCheckBox, bool hasImage)
		{
			string text = "Item " + index.ToString();
			if (hasCheckBox == false && hasImage == false)
				return new NListBoxItem(text);

			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalSpacing = 3;

			if (hasCheckBox)
			{
				NCheckBox checkBox = new NCheckBox();
				checkBox.VerticalPlacement = ENVerticalPlacement.Center;
				stack.Add(checkBox);
			}

			if (hasImage)
			{
				string imageName = ImageNames[index % ImageNames.Length];
				NImage icon = new NImage(new NEmbeddedResourceRef(NResources.Instance, "RIMG__16x16_" + imageName + "_png"));

				NImageBox imageBox = new NImageBox(icon);
				imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
				imageBox.VerticalPlacement = ENVerticalPlacement.Center;

				stack.Add(imageBox);
			}

			NLabel label = new NLabel(text);
			label.VerticalPlacement = ENVerticalPlacement.Center;
			stack.Add(label);

			return new NListBoxItem(stack);
		}
		private void FillWithImageCheckBoxAndTitle()
		{
			m_ListBox.Items.Clear();

			for (int i = 0; i < 100; i++)
			{
				int index = (i % 32) / 8;
				bool checkBox = index == 1 || index == 3;
				bool image = index == 2 || index == 3;

				m_ListBox.Items.Add(CreateListBoxItem(i, checkBox, image));
			}
		}

		private NListBoxItem CreateDetailedListBoxItem(int index)
		{
			NDockPanel dock = new NDockPanel();
			dock.HorizontalSpacing = 3;
			dock.Padding = new NMargins(0, 2, 0, 2);

			// Add the image
			string imageName = ImageNames[index % ImageNames.Length];
			NImage icon = new NImage(new NEmbeddedResourceRef(NResources.Instance, "RIMG__24x24_" + imageName + "_png"));

			NImageBox imageBox = new NImageBox(icon);
			imageBox.HorizontalPlacement = ENHorizontalPlacement.Center;
			imageBox.VerticalPlacement = ENVerticalPlacement.Center;
			NDockLayout.SetDockArea(imageBox, ENDockArea.Left);

			dock.Add(imageBox);

			// Add the title
			NLabel titleLabel = new NLabel("Item " + index.ToString());
			titleLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Bold);
			NDockLayout.SetDockArea(titleLabel, ENDockArea.Top);
			dock.AddChild(titleLabel);

			// Add the description
			NLabel descriptionLabel = new NLabel("This is item " + index.ToString() + "'s description.");
			NDockLayout.SetDockArea(descriptionLabel, ENDockArea.Center);
			dock.AddChild(descriptionLabel);	

			return new NListBoxItem(dock);
		}
		private void FillWithImageTitleAndDetails()
		{
			m_ListBox.Items.Clear();

			for (int i = 0; i < 100; i++)
			{
				m_ListBox.Items.Add(CreateDetailedListBoxItem(i));
			}
		}

		#endregion

		#region Event Handlers

		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> args)
		{
			NListBoxItem item = args.Item;
			int index = item.GetAggregationInfo().Index;
			m_EventsLog.LogEvent("Selected Item: " + index.ToString());
		}
		private void OnListBoxItemDeselected(NSelectEventArgs<NListBoxItem> args)
		{
			NListBoxItem item = args.Item;
			int index = item.GetAggregationInfo().Index;
			m_EventsLog.LogEvent("Deselected Item: " + index.ToString());
		}
		/// <summary>
		/// Called when the ContentType property has changed.
		/// </summary>
		/// <param name="data"></param>
		private void OnContentTypeChanged(NValueChangeData data)
		{
			switch ((ENListBoxContentType)data.NewValue)
			{
				case ENListBoxContentType.ImageCheckBoxAndTitle:
					FillWithImageCheckBoxAndTitle();
					break;
				case ENListBoxContentType.ImageTitleAndDetails:
					FillWithImageTitleAndDetails();
					break;
				default:
					throw new Exception("New ENListBoxContentType?");
			}
		}

		#endregion

		#region Fields

		private NListBox m_ListBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NListBoxMixedContentExample.
		/// </summary>
		public static readonly NSchema NListBoxMixedContentExampleSchema;
		/// <summary>
		/// Reference to the ContentType property.
		/// </summary>
		public static readonly NProperty ContentTypeProperty;

		#endregion

		#region Constants

		private const ENListBoxContentType defaultContentType = ENListBoxContentType.ImageCheckBoxAndTitle;

		private static readonly string[] ImageNames = new string[] { 
			"Calendar", "Contacts", "Folders", "Journal",
			"Mail", "Notes", "Shortcuts", "Tasks"
		};

		#endregion

		#region Nested Types

		public enum ENListBoxContentType
		{
			ImageCheckBoxAndTitle,
			ImageTitleAndDetails
		}

		/// <summary>
		/// Designer for NListBoxMixedContent.
		/// </summary>
		public class NListBoxMixedContentDesigner : NDesigner
		{
			/// <summary>
			/// Default constructor.
			/// </summary>
			public NListBoxMixedContentDesigner()
			{
				SetPropertyEditor(ContentTypeProperty, NEnumPropertyEditor.VerticalRadioGroupTemplate);
			}
		}

		#endregion
	}
}