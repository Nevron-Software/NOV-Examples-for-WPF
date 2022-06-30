using System;

using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
    /// <summary>
    /// The example shows how to work with the NTextBox widget
    /// </summary>
    public class NTextBoxExample : NExampleBase
	{
		#region Constructors

		public NTextBoxExample()
		{
		}
		static NTextBoxExample()
		{
			NTextBoxExampleSchema = NSchema.Create(typeof(NTextBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{

			m_TextBox = new NTextBox();
			m_TextBox.Hint = "Enter some text here.";

			return m_TextBox;
		} 

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			
			// font families
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

			m_StrikeTroughCheckBox = new NCheckBox();
			m_StrikeTroughCheckBox.Content = new NLabel("Strikethrough");
			m_StrikeTroughCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnFontStyleChanged);
			stack.Add(m_StrikeTroughCheckBox); 
			
			// properties
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_TextBox).CreatePropertyEditors(
				m_TextBox,
				NTextBox.EnabledProperty,
				NTextBox.ReadOnlyProperty,
				NTextBox.MultilineProperty,
				NTextBox.WordWrapProperty,
				NTextBox.AlwaysShowSelectionProperty,
				NTextBox.AlwaysShowCaretProperty,
				NTextBox.AcceptsTabProperty,
				NTextBox.AcceptsEnterProperty,
				NTextBox.ShowCaretProperty,
				NTextBox.HorizontalPlacementProperty,
				NTextBox.VerticalPlacementProperty,
				NTextBox.TextAlignProperty,
				NTextBox.DirectionProperty,
				NTextBox.HScrollModeProperty,
				NTextBox.VScrollModeProperty,
				NTextBox.CharacterCasingProperty,
				NTextBox.PasswordCharProperty,
				NTextBox.HintProperty,
				NTextBox.HintFillProperty
			);
			
			NStackPanel propertiesStack = new NStackPanel();
			for (int i = 0; i < editors.Count; i++)
			{
				propertiesStack.Add(editors[i]);
			}

			stack.Add(new NGroupBox("Properties", new NUniSizeBoxGroup(propertiesStack)));

			// make sure font style is updated
			OnFontStyleChanged(null);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use text boxes. The controls on the right let you
	change the text box's font, alignment, placement, etc.
</p>
";
		}

		#endregion

		#region Event Handlers

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

			if (m_StrikeTroughCheckBox.Checked)
			{
                fontStyle |= ENFontStyle.Strikethrough;
			}

			m_TextBox.Font = new NFont(fontFamily, fontSize, fontStyle);
		}

		#endregion

		#region Fields

		private NTextBox m_TextBox;

		private NComboBox m_FontFamiliesComboBox;
		private NComboBox m_FontSizeComboBox;
		private NCheckBox m_BoldCheckBox;
		private NCheckBox m_ItalicCheckBox;
		private NCheckBox m_UnderlineCheckBox;
		private NCheckBox m_StrikeTroughCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NTextBoxExampleSchema;

		#endregion
	}
}
