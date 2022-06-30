using System;
using System.IO;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples.UI
{
	public class NListBoxDataBindingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NListBoxDataBindingExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NListBoxDataBindingExample()
		{
			NListBoxDataBindingExampleSchema = NSchema.Create(typeof(NListBoxDataBindingExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.Direction = ENHVDirection.LeftToRight;
			stack.HorizontalSpacing = 10;

			// Load the contry data
			NList<NCountry> countries = LoadCountryData();

			// Create the simple list box
			m_ListBox = new NListBox();
			m_ListBox.Selection.Selected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemSelected);
			stack.Add(new NGroupBox("Data source items -> Labels", m_ListBox));

			// Create the simple data binding
			NNodeCollectionDataBinding<NListBoxItemCollection, NListBoxItem, NCountry> countryNameDataBinding =
				new NNodeCollectionDataBinding<NListBoxItemCollection, NListBoxItem, NCountry>();
			NDataBinding.SetDataBinding(m_ListBox.Items, countryNameDataBinding);
			countryNameDataBinding.DataSource = countries;
			countryNameDataBinding.CreateItemNode += new Function<NCreateItemNodeEventArgs<NListBoxItem, NCountry>>(OnCountryNameDataBindingCreateItemNode);
			countryNameDataBinding.RebuildTarget();

			// Create the advanced list box
			m_AdvancedListBox = new NListBox();
			m_AdvancedListBox.Selection.Selected += new Function<NSelectEventArgs<NListBoxItem>>(OnListBoxItemSelected);
			stack.Add(new NGroupBox("Data source items -> Custom widgets", m_AdvancedListBox));

			// Create the advanced data binding
			NNodeCollectionDataBinding<NListBoxItemCollection, NListBoxItem, NCountry> countryDataBinding =
				new NNodeCollectionDataBinding<NListBoxItemCollection, NListBoxItem, NCountry>();
			NDataBinding.SetDataBinding(m_AdvancedListBox.Items, countryDataBinding);
			countryDataBinding.DataSource = countries;
			countryDataBinding.CreateItemNode += new Function<NCreateItemNodeEventArgs<NListBoxItem, NCountry>>(OnCountryDataBindingCreateItemNode);
			countryDataBinding.RebuildTarget();

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FitMode = ENStackFitMode.Last;
			stack.FillMode = ENStackFillMode.Last;

			// Create the property editors
			NCheckBox enabledCheckBox = new NCheckBox("Enabled", true);
			enabledCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnabledCheckBoxCheckedChanged);
			stack.Add(new NGroupBox("Properties", enabledCheckBox));

			// Add an event log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a list box and bind data to it. This is done by creating a <b>NNodeCollectionDataBinding</b>
	and then attaching it to the list box's collection of items. When the data binding needs a new list box item to be created, it
	calls the <b>CreateItemNode event</b> where you should create the list box item using the <b>Item</b> property of the event argument
	and then you should assign the newly created list box item to the event argument's <b>Node</b> property.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnEnabledCheckBoxCheckedChanged(NValueChangeEventArgs args)
		{
			bool enabled = (bool)args.NewValue;
			m_ListBox.Enabled = enabled;
			m_AdvancedListBox.Enabled = enabled;
		}
		private void OnListBoxItemSelected(NSelectEventArgs<NListBoxItem> args)
		{
			if (m_bSelectionUpdating)
			{
				m_bSelectionUpdating = false;
				return;
			}

			// Get the list box and the other list box
			NListBox listBox = (NListBox)args.Item.OwnerListBox;
			NListBox otherListBox = listBox == m_ListBox ? m_AdvancedListBox : m_ListBox;

			// Log the selection
			NListBoxItem selectedItem = listBox.Selection.FirstSelected;
			NCountry country = NNodeCollectionDataBinding<NListBoxItemCollection, NListBoxItem, NCountry>.GetDataBoundItem(selectedItem);
			m_EventsLog.LogEvent("'" + country.Name + "' selected");

			// Synchronize the selection between the two list boxes
			m_bSelectionUpdating = true;
			int selectedIndex = listBox.Items.IndexOf(selectedItem);
			otherListBox.Selection.SingleSelect(otherListBox.Items[selectedIndex]);
		}
		private void OnCountryNameDataBindingCreateItemNode(NCreateItemNodeEventArgs<NListBoxItem, NCountry> args)
		{
			// Create a list box item for the current country
			NCountry country = args.Item;
			args.Node = new NListBoxItem(country.Name);
		}
		private void OnCountryDataBindingCreateItemNode(NCreateItemNodeEventArgs<NListBoxItem, NCountry> args)
		{
			NCountry country = args.Item;

			// Create a stack panel
			NStackPanel stack = new NStackPanel();
			stack.Padding = new NMargins(3);
			stack.Tag = country;

			// Create the flag image box and the country name label
			NLabel countryLabel = new NLabel(country.Name);
			countryLabel.VerticalPlacement = ENVerticalPlacement.Center;
			countryLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Bold);

			NImageBox imageBox = new NImageBox(country.Flag);
			imageBox.VerticalPlacement = ENVerticalPlacement.Center;
			imageBox.HorizontalPlacement = ENHorizontalPlacement.Left;

			NPairBox pairBox = new NPairBox(imageBox, countryLabel);
			pairBox.Spacing = 3;
			stack.Add(pairBox);

			// Create the capital label
			NLabel capitalLabel = new NLabel("Capital: " + country.Capital);
			stack.Add(capitalLabel);

			// Create the currency label
			NLabel currencyLabel = new NLabel("Currency: " + country.CurrencyName + ", " +
				country.CurrencyCode);

			stack.Add(currencyLabel);

			// Create a list box item to host the created widget
			NListBoxItem listBoxItem = new NListBoxItem(stack);
			listBoxItem.Text = country.Name;
			args.Node = listBoxItem;
		}

		#endregion

		#region Implementation

		private NList<NCountry> LoadCountryData()
		{
			// Get the country list XML stream
			Stream stream = NResources.Instance.GetResourceStream("RSTR_CountryList_xml");

			// Load an xml document from the stream
			NXmlDocument xmlDocument = NXmlDocument.LoadFromStream(stream);

			// Process it
			NXmlNode rows = xmlDocument.GetChildAt(0).GetChildAt(1);
			NList<NCountry> countries = new NList<NCountry>();

			for (int i = 0, countryCount = rows.ChildrenCount; i < countryCount; i++)
			{
				NXmlNode row = rows.GetChildAt(i);

				// Get the country name
				NCountry country = new NCountry(GetValue(row.GetChildAt(1)));
				if (String.IsNullOrEmpty(country.Name))
					continue;

				// Get the country's capital
				country.Capital = GetValue(row.GetChildAt(6));
				if (String.IsNullOrEmpty(country.Capital))
					continue;

				// Get the country's currency
				country.CurrencyCode = GetValue(row.GetChildAt(7));
				country.CurrencyName = GetValue(row.GetChildAt(8));
				if (String.IsNullOrEmpty(country.CurrencyCode) || String.IsNullOrEmpty(country.CurrencyName))
					continue;

				// Get the country code (ISO 3166-1 2 Letter Code)
				country.Code = GetValue(row.GetChildAt(10));
				if (String.IsNullOrEmpty(country.Code))
					continue;

				// Get the country flag
				string flagResourceName = "RIMG_CountryFlags_" + country.Code.ToLower() + "_png";
				NEmbeddedResource flagResource = NResources.Instance.GetResource(flagResourceName);
				if (flagResource == null)
					continue;

				country.Flag = new NImage(new NEmbeddedResourceRef(flagResource));

				// Add the country to the list
				countries.Add(country);
			}

			// Sort the countries by name and return them
			countries.Sort();
			return countries;
		}

		#endregion

		#region Fields

		private bool m_bSelectionUpdating = false;

		private NListBox m_ListBox;
		private NListBox m_AdvancedListBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NListBoxDataBindingExample.
		/// </summary>
		public static readonly NSchema NListBoxDataBindingExampleSchema;

		#endregion

		#region Static Methods

		private static string GetValue(NXmlNode node)
		{
			if (node.ChildrenCount != 1)
				return null;

			NXmlTextNode textNode = node.GetChildAt(0) as NXmlTextNode;
			return textNode != null ? textNode.Text : null;
		}

		#endregion

		#region Nested Types

		protected class NCountry : IComparable<NCountry>, INDeeplyCloneable
		{
			public NCountry(string name)
			{
				Name = name;
				Code = null;
				CurrencyCode = null;
				CurrencyName = null;
				Capital = null;
				Flag = null;
			}

			public override string ToString()
			{
				return Name;
			}
			public int CompareTo(NCountry other)
			{
				return Name.CompareTo(other.Name);
			}
			/// <summary>
			/// Creates an identical copy of this object.
			/// </summary>
			/// <returns>A copy of this instance.</returns>
			public object DeepClone()
			{
				NCountry country = new NCountry(Name);
				return country;
			}

			public string Name;
			public string Code;
			public string CurrencyCode;
			public string CurrencyName;
			public string Capital;
			public NImage Flag;
		}

		#endregion
	}
}