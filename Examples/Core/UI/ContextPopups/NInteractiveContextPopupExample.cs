using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NInteractiveContextPopupExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NInteractiveContextPopupExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NInteractiveContextPopupExample()
		{
			NInteractiveContextPopupExampleSchema = NSchema.Create(typeof(NInteractiveContextPopupExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_Label = new NLabel("Click me with the right mouse button");
			m_Label.HorizontalPlacement = ENHorizontalPlacement.Center;
			m_Label.VerticalPlacement = ENVerticalPlacement.Center;
			m_Label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 10, ENFontStyle.Regular);
			m_Label.TextFill = new NColorFill(NColor.Black);

			NContentHolder widget = new NContentHolder(m_Label);
			widget.HorizontalPlacement = ENHorizontalPlacement.Left;
			widget.VerticalPlacement = ENVerticalPlacement.Top;
			widget.BackgroundFill = new NColorFill(NColor.PapayaWhip);
			widget.Border = NBorder.CreateFilledBorder(NColor.Black);
			widget.BorderThickness = new NMargins(1);
			widget.PreferredSize = new NSize(300, 100);
			widget.MouseDown += new Function<NMouseButtonEventArgs>(OnTargetWidgetMouseDown);

			return widget;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create an interactive context popup. All you have to do is to create
	a popup window and show it using the <b>NPopupWindow.OpenInContext(...)</b> method when the user right
	clicks the widget the context is designed for.
</p>
";
		}

		#endregion

		#region Implementation

		private NToggleButton CreateToggleButton(string text, bool isChecked)
		{
			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Center;
			label.VerticalPlacement = ENVerticalPlacement.Center;

			NToggleButton button = new NToggleButton(label);
			button.Checked = isChecked;
			button.PreferredWidth = 20;
			button.CheckedChanged += new Function<NValueChangeEventArgs>(OnToggleButtonCheckedChanged);
			return button;
		}
		private NWidget CreatePopupContent()
		{
			// Create the first tool bar
			NToolBar toolBar1 = new NToolBar();
			toolBar1.Gripper.Visibility = ENVisibility.Collapsed;
			toolBar1.Pendant.Visibility = ENVisibility.Collapsed;
			toolBar1.Items.HorizontalSpacing = 3;

			NComboBox fontComboBox = new NComboBox();
			for (int i = 0, fontCount = Fonts.Length; i < fontCount; i++)
			{
				string fontName = Fonts[i];
				NLabel label = new NLabel(fontName);
				label.Font = new NFont(fontName, 8, ENFontStyle.Regular);
				fontComboBox.Items.Add(new NComboBoxItem(label));

				if (fontName == m_Label.Font.Name)
				{
					// Update the selected index
					fontComboBox.SelectedIndex = i;
				}
			}

			fontComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFontComboBoxSelectedIndexChanged);
			toolBar1.Items.Add(fontComboBox);

			NNumericUpDown fontSizeNumericUpDown = new NNumericUpDown();
			fontSizeNumericUpDown.Minimum = 6;
			fontSizeNumericUpDown.Maximum = 32;
			fontSizeNumericUpDown.Value = m_Label.Font.Size;
			fontSizeNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnFontSizeNumericUpDownValueChanged);
			toolBar1.Items.Add(fontSizeNumericUpDown);

			// Create the second tool bar
			NToolBar toolBar2 = new NToolBar();
			toolBar2.Gripper.Visibility = ENVisibility.Collapsed;
			toolBar2.Pendant.Visibility = ENVisibility.Collapsed;

			NToggleButton boldButton = CreateToggleButton("B", (m_Label.Font.Style & ENFontStyle.Bold) == ENFontStyle.Bold);
			toolBar2.Items.Add(boldButton);

			NToggleButton italicButton = CreateToggleButton("I", (m_Label.Font.Style & ENFontStyle.Italic) == ENFontStyle.Italic);
			toolBar2.Items.Add(italicButton);

			NToggleButton underlineButton = CreateToggleButton("U", (m_Label.Font.Style & ENFontStyle.Underline) == ENFontStyle.Underline);
			toolBar2.Items.Add(underlineButton);

			NFillSplitButton fillButton = new NFillSplitButton();
            fillButton.SelectedValue = new NAutomaticValue<NFill>(false, (NFill)m_Label.TextFill.DeepClone());
			fillButton.SelectedValueChanged += new Function<NValueChangeEventArgs>(OnFillButtonSelectedValueChanged);
			toolBar2.Items.Add(fillButton);

			// Add the tool bars in a stack
			NStackPanel stack = new NStackPanel();
			stack.Add(toolBar1);
			stack.Add(toolBar2);

			return stack;
		}

		#endregion

		#region Event Handlers

		private void OnTargetWidgetMouseDown(NMouseButtonEventArgs args)
		{
			if (args.Button != ENMouseButtons.Right)
				return;

			// Mark the event as handled
			args.Cancel = true;

			// Create and show the popup
			NWidget popupContent = CreatePopupContent();
			NPopupWindow popupWindow = new NPopupWindow(popupContent);
			NPopupWindow.OpenInContext(popupWindow, args.CurrentTargetNode, args.ScreenPosition);
		}

		private void OnFontComboBoxSelectedIndexChanged(NValueChangeEventArgs args)
		{
			int index = (int)args.NewValue;
			m_Label.Font.Name = Fonts[index];
		}
		private void OnToggleButtonCheckedChanged(NValueChangeEventArgs args)
		{
			NToggleButton toggleButton = (NToggleButton)args.CurrentTargetNode;
			NLabel label = (NLabel)toggleButton.Content;
			switch (label.Text)
			{
				case "B":
					m_Label.Font.Style ^= ENFontStyle.Bold;
					break;
				case "I":
					m_Label.Font.Style ^= ENFontStyle.Italic;
					break;
				case "U":
					m_Label.Font.Style ^= ENFontStyle.Underline;
					break;
			}
		}
		private void OnFontSizeNumericUpDownValueChanged(NValueChangeEventArgs args)
		{
			m_Label.Font.Size = (double)args.NewValue;
		}
		private void OnFillButtonSelectedValueChanged(NValueChangeEventArgs args)
		{
            NAutomaticValue<NFill> newValue = ((NAutomaticValue<NFill>)args.NewValue);
			NFill fill = newValue.Value;
			m_Label.TextFill = (NFill)fill.DeepClone();
		}

		#endregion

		#region Fields

		private NLabel m_Label;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NInteractiveContextPopupExample.
		/// </summary>
		public static readonly NSchema NInteractiveContextPopupExampleSchema;

		#endregion

		#region Constants

		private static readonly string[] Fonts = new string[] {
			NFontDescriptor.DefaultSansFamilyName,
			NFontDescriptor.DefaultSerifFamilyName,
			NFontDescriptor.DefaultMonoFamilyName
		};

		#endregion
	}
}