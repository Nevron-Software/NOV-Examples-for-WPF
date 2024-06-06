using System;
using System.IO;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Xml;

namespace Nevron.Nov.Examples.UI
{
	public class NAutoCompleteBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NAutoCompleteBoxExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NAutoCompleteBoxExample()
		{
			NAutoCompleteBoxExampleSchema = NSchema.Create(typeof(NAutoCompleteBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.HorizontalPlacement = ENHorizontalPlacement.Left;
			stack.VerticalPlacement = ENVerticalPlacement.Top;
			stack.VerticalSpacing = 10;

			// Load the contry data
			m_Countries = LoadCountryData();

			// Create the simple auto complete text box
			m_TextBox = new NAutoCompleteBox();
			m_TextBox.PreferredWidth = 300;
			m_TextBox.InitAutoComplete(m_Countries);
			m_TextBox.TextChanged += OnTextBoxTextChanged;
			NPairBox pairBox = CreatePairBox("Enter country name:", m_TextBox);
			stack.Add(new NGroupBox("Auto complete items -> Labels", pairBox));

			// Create the advanced auto complete text box
			m_AdvancedTextBox = new NAutoCompleteBox();
			m_AdvancedTextBox.PreferredWidth = 300;
			m_AdvancedTextBox.Image = NResources.Image_ExamplesUI_Icons_Search_png;
			m_AdvancedTextBox.InitAutoComplete(m_Countries, new NCountryFactory());
			m_AdvancedTextBox.TextChanged += OnTextBoxTextChanged;
			pairBox = CreatePairBox("Enter country name:", m_AdvancedTextBox);
			stack.Add(new NGroupBox("Auto complete items -> Custom widgets", pairBox));

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Create the property editors
			NCheckBox enabledCheckBox = new NCheckBox("Enabled", true);
			enabledCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnabledCheckBoxCheckedChanged);
			stack.Add(enabledCheckBox);

			m_CaseSensitiveCheckBox = new NCheckBox("Case Sensitive", false);
			m_CaseSensitiveCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnCaseSensitiveCheckBoxCheckedChanged);
			stack.Add(m_CaseSensitiveCheckBox);

			NComboBox stringMacthModeComboBox = new NComboBox();
			stringMacthModeComboBox.FillFromEnum<ENStringMatchMode>();
			stringMacthModeComboBox.SelectedIndexChanged += OnStringMacthModeComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("String Match Mode:", stringMacthModeComboBox));

			// Add the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use auto complete text boxes. The auto complete text box
	is an UI element that hosts a text box and also provides an auto complete functionality. Using the 
	controls on the right you can specify whether the auto complete should be case sensitive (default)
	or not and the string match mode.
</p>
";
		}

		#endregion

		#region Implementation

		private NPairBox CreatePairBox(string labelText, NAutoCompleteBox textBox)
		{
			NLabel label = new NLabel(labelText);
			label.VerticalPlacement = ENVerticalPlacement.Center;

			NPairBox pairBox = new NPairBox(label, textBox, ENPairBoxRelation.Box1AboveBox2, false);
			pairBox.Spacing = 3;
			return pairBox;
		}
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
				NEmbeddedResource flagResource = Presentation.NResources.Instance.GetResource(flagResourceName);
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
		private NCountry GetCountryByName(string str, bool caseSensitive)
		{
			StringComparison comparison = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
			for (int i = 0, count = m_Countries.Count; i < count; i++)
			{
				NCountry country = m_Countries[i];
				if (String.Equals(str, country.Name, comparison))
					return country;
			}

			return null;
		}

		#endregion

		#region Event Handlers

		private void OnEnabledCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool enabled = (bool)arg.NewValue;
			m_TextBox.Enabled = enabled;
			m_AdvancedTextBox.Enabled = enabled;
		}
		private void OnCaseSensitiveCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool autocompleteCaseSensitive = (bool)arg.NewValue;
			m_TextBox.CaseSensitive = autocompleteCaseSensitive;
			m_AdvancedTextBox.CaseSensitive = autocompleteCaseSensitive;
		}
		private void OnStringMacthModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NComboBox comboBox = (NComboBox)arg.CurrentTargetNode;
			ENStringMatchMode stringMatchMode = (ENStringMatchMode)comboBox.SelectedItem.Tag;
			m_TextBox.StringMatchMode = stringMatchMode;
			m_AdvancedTextBox.StringMatchMode = stringMatchMode;
		}
		private void OnTextBoxTextChanged(NValueChangeEventArgs arg)
		{
			string text = (string)arg.NewValue;
			NCountry country = GetCountryByName(text, m_CaseSensitiveCheckBox.Checked);
			if (country != null)
			{
				m_EventsLog.LogEvent("Selected country: " + country.Name);
			}
		}

		#endregion

		#region Fields

		private NAutoCompleteBox m_TextBox;
		private NAutoCompleteBox m_AdvancedTextBox;

		private NCheckBox m_CaseSensitiveCheckBox;
		private NExampleEventsLog m_EventsLog;
		private NList<NCountry> m_Countries;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NAutoCompleteBoxExample.
		/// </summary>
		public static readonly NSchema NAutoCompleteBoxExampleSchema;

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

		private class NCountry : IComparable<NCountry>, INDeeplyCloneable
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

		private class NCountryFactory : NWidgetFactory<NCountry>
		{
			public override NWidget CreateWidget(NCountry country)
			{
				// Create a dock panel
				NStackPanel stack = new NStackPanel();
				stack.Padding = new NMargins(3);
				stack.Tag = country;

				// Create the flag image box and the country name label
				NLabel countryLabel = new NLabel(country.Name);
				countryLabel.VerticalPlacement = ENVerticalPlacement.Center;
				countryLabel.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Bold);

				NImageBox imageBox = new NImageBox((NImage)NSystem.SafeDeepClone(country.Flag));
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

				return stack;
			}
		}

		#endregion
	}
}