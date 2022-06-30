using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NPaletteColorPickerExample : NExampleBase
	{
		#region Constructors

		public NPaletteColorPickerExample()
		{
		}
		static NPaletteColorPickerExample()
		{
			NPaletteColorPickerExampleSchema = NSchema.Create(typeof(NPaletteColorPickerExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Protected Overrides - Examples Content

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();

			m_PaletteColorPicker = new NPaletteColorPicker();
			m_PaletteColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_PaletteColorPicker.VerticalPlacement = ENVerticalPlacement.Top;
			m_PaletteColorPicker.SelectedIndexChanged += OnSelectedIndexChanged;
			stack.Add(m_PaletteColorPicker);

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// create the palette select combo box
			NComboBox paletteComboBox = new NComboBox();
			paletteComboBox.Items.Add(new NComboBoxItem(ENColorPaletteType.MicrosoftPaint));
			paletteComboBox.Items.Add(new NComboBoxItem(ENColorPaletteType.MicrosoftOffice2003));
			paletteComboBox.Items.Add(new NComboBoxItem(ENColorPaletteType.MicrosoftOffice2007));
			paletteComboBox.Items.Add(new NComboBoxItem(ENColorPaletteType.WebSafe));
			paletteComboBox.SelectedIndex = 0;
			paletteComboBox.SelectedIndexChanged += OnPaletteComboBoxSelectedIndexChanged;
			stack.Add(new NPairBox("Palette:", paletteComboBox, true));

			// add some property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_PaletteColorPicker).CreatePropertyEditors(
				m_PaletteColorPicker,
				NPaletteColorPicker.EnabledProperty,
				NPaletteColorPicker.CyclicKeyboardNavigationProperty,
				NPaletteColorPicker.SelectedIndexProperty
			);

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			// create the events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create a palette color picker. The palette color picker is table picker
	that lets the user pick a color from a palette of colors. The palette to use may be passed as a parameter
	to the palette color picker constructor or may be assigned to its <b>Palette</b> property. You can use the
	controls on the right to change the currently used color palette.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnPaletteComboBoxSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox cbPalette = (NComboBox)args.TargetNode;
			switch (cbPalette.SelectedIndex)
			{
				case 0: // "MS Paint"
					m_PaletteColorPicker.Palette = new NColorPalette(ENColorPaletteType.MicrosoftPaint);
					break;

				case 1: // "Office 2003":
					m_PaletteColorPicker.Palette = new NColorPalette(ENColorPaletteType.MicrosoftOffice2003);
					break;

				case 2: // "Office 2007":
					m_PaletteColorPicker.Palette = new NColorPalette(ENColorPaletteType.MicrosoftOffice2007);
					break;

				case 3: // "Web Safe":
					m_PaletteColorPicker.Palette = new NColorPalette(ENColorPaletteType.WebSafe);
					break;
			}
		}
		private void OnSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NPaletteColorPicker colorPicker = (NPaletteColorPicker)args.TargetNode;
			NColor selectedColor = colorPicker.SelectedColor;
			m_EventsLog.LogEvent(selectedColor.GetHEX().ToUpper());
		}

		#endregion

		#region Fields

		private NPaletteColorPicker m_PaletteColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NPaletteColorPickerExampleSchema;

		#endregion
	}
}