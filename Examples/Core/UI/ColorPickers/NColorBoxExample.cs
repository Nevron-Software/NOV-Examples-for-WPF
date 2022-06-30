using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NColorBoxExample : NExampleBase
	{
		#region Constructors

		public NColorBoxExample()
		{
		}
		static NColorBoxExample()
		{
			NColorBoxExampleSchema = NSchema.Create(typeof(NColorBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ColorBox = new NColorBox();
			m_ColorBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ColorBox.VerticalPlacement = ENVerticalPlacement.Top;
			m_ColorBox.SelectedColorChanged += OnColorBoxSelectedColorChanged;

			return m_ColorBox;
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
			paletteComboBox.SelectedIndex = 2;
			paletteComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPaletteComboBoxSelectedIndexChanged);
			stack.Add(new NPairBox("Palette:", paletteComboBox, true));

			// add come property editors
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ColorBox).CreatePropertyEditors(
				m_ColorBox,
				NColorBox.EnabledProperty,
				NColorBox.ShowMoreColorsButtonProperty,
				NColorBox.ShowOpacitySliderInDialogProperty,
				NColorBox.SelectedColorProperty
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
	This example demonstrates how to create and use a NOV color box (i.e. a drop down palette color picker). 
	The controls on the right let you change the palette of the picker, whether the drop down should have a
	""More Colors..."" button and which is the currently selected color.
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
					m_ColorBox.Palette = new NColorPalette(ENColorPaletteType.MicrosoftPaint);
					break;

				case 1: // "Office 2003":
					m_ColorBox.Palette = new NColorPalette(ENColorPaletteType.MicrosoftOffice2003);
					break;

				case 2: // "Office 2007":
					m_ColorBox.Palette = new NColorPalette(ENColorPaletteType.MicrosoftOffice2007);
					break;

				case 3: // "Web Safe":
					m_ColorBox.Palette = new NColorPalette(ENColorPaletteType.WebSafe);
					break;
			}
		}
		private void OnColorBoxSelectedColorChanged(NValueChangeEventArgs args)
		{
			NColorBox colorBox = (NColorBox)args.TargetNode;
			m_EventsLog.LogEvent(NColor.GetNameOrHex(colorBox.SelectedColor));
		}

		#endregion

		#region Fields

		private NColorBox m_ColorBox;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NColorBoxExampleSchema;

		#endregion
	}
}