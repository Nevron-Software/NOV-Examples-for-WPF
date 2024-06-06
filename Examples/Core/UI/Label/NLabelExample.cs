using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NLabelExample : NExampleBase
	{
		#region Constructors

		public NLabelExample()
		{
		}
		static NLabelExample()
		{
			NLabelExampleSchema = NSchema.Create(typeof(NLabelExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Label = new NLabel();
			m_Label.SetBorder(1, NColor.Red);

            return m_Label;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			m_SampleTextComboBox = new NComboBox();

			for (int i = 0; i < 2; i++)
			{
				m_SampleTextComboBox.Items.Add(new NComboBoxItem("Sample " + i.ToString()));
			}

			m_SampleTextComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnSampleTextChanged);
			m_SampleTextComboBox.SelectedIndex = 0;

			stack.Add(new NLabel("Sample Text:"));
			stack.Add(m_SampleTextComboBox);

			// font families
			stack.Add(new NLabel("Font Family:"));
			m_FontFamiliesComboBox = new NComboBox();

			string[] fontFamilies = NApplication.FontService.InstalledFontsMap.FontFamilies;
			int selectedIndex = 0;

			for (int i = 0; i < fontFamilies.Length; i++)
			{
				m_FontFamiliesComboBox.Items.Add(new NComboBoxItem(fontFamilies[i]));

				if (fontFamilies[i] == NFontDescriptor.DefaultSansFamilyName)
				{
					selectedIndex = i;
				}
			}

			m_FontFamiliesComboBox.SelectedIndex = selectedIndex;
			m_FontFamiliesComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_FontFamiliesComboBox);

			// font sizes
			stack.Add(new NLabel("Font Size:"));
			m_FontSizeComboBox = new NComboBox();
			for (int i = 5; i < 72; i++)
			{
				m_FontSizeComboBox.Items.Add(new NComboBoxItem(i.ToString()));
			}

			m_FontSizeComboBox.SelectedIndex = 4;
			m_FontSizeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_FontSizeComboBox);

			// add style controls
			m_BoldCheckBox = new NCheckBox();
			m_BoldCheckBox.Content = new NLabel("Bold");
			m_BoldCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_BoldCheckBox);

			m_ItalicCheckBox = new NCheckBox();
			m_ItalicCheckBox.Content = new NLabel("Italic");
			m_ItalicCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_ItalicCheckBox);

			m_UnderlineCheckBox = new NCheckBox();
			m_UnderlineCheckBox.Content = new NLabel("Underline");
			m_UnderlineCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_UnderlineCheckBox);

			m_StrikeThroughCheckBox = new NCheckBox();
			m_StrikeThroughCheckBox.Content = new NLabel("Strikethrough");
			m_StrikeThroughCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_StrikeThroughCheckBox); 

			// properties
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Label).CreatePropertyEditors(
				m_Label,
				NLabel.EnabledProperty,
				NLabel.HorizontalPlacementProperty,
				NLabel.VerticalPlacementProperty,
				NLabel.TextAlignmentProperty,
				NLabel.TextWrapModeProperty
			);

			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			// make sure font style is updated
			OnFontStyleChanged(null);

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));
			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use labels. The controls on the right let you
	change the label's font, alignment, placement, etc.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnSampleTextChanged(NValueChangeEventArgs args)
		{
			switch (m_SampleTextComboBox.SelectedIndex)
			{ 
				case 0:
                    m_Label.Text = "The quick brown fox jumps over the lazy dog.";
					break;
				case 1:
					m_Label.Text = "This is the first line of a multi line label.\nThis is the second line.";
					break;
			}
		}
		private void OnFontStyleChanged(NValueChangeEventArgs args)
		{
			string fontFamily = (m_FontFamiliesComboBox.SelectedItem.Content as NLabel).Text;
			int fontSize = Int32.Parse((m_FontSizeComboBox.SelectedItem.Content as NLabel).Text);

			ENFontStyle fontStyle = ENFontStyle.Regular;

			if (m_BoldCheckBox.Checked)
			{
				fontStyle |= ENFontStyle.Bold;
			}

			if (m_ItalicCheckBox.Checked)
			{
				fontStyle |= ENFontStyle.Italic;
			}

			if (m_UnderlineCheckBox.Checked)
			{
				fontStyle |= ENFontStyle.Underline;
			}

			if (m_StrikeThroughCheckBox.Checked)
			{
                fontStyle |= ENFontStyle.Strikethrough;
			}

            m_Label.Font = new NFont(fontFamily, fontSize, fontStyle, ENFontRasterizationMode.Antialiased);
		}

		#endregion

		#region Fields

		private NLabel m_Label;

		private NComboBox m_SampleTextComboBox;
		private NComboBox m_FontFamiliesComboBox;
		private NComboBox m_FontSizeComboBox;
		private NCheckBox m_BoldCheckBox;
		private NCheckBox m_ItalicCheckBox;
		private NCheckBox m_UnderlineCheckBox;
		private NCheckBox m_StrikeThroughCheckBox;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NLabelExample.
		/// </summary>
		public static readonly NSchema NLabelExampleSchema;

		#endregion
	}
}