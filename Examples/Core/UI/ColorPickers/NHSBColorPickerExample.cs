using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NHSBColorPickerExample : NExampleBase
	{
		#region Constructors

		public NHSBColorPickerExample()
		{
		}
		static NHSBColorPickerExample()
		{
			NHSBColorPickerExampleSchema = NSchema.Create(typeof(NHSBColorPickerExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the HSB color picker
			m_ColorPicker = new NHsbColorPicker();
			m_ColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ColorPicker.VerticalPlacement = ENVerticalPlacement.Top;
			m_ColorPicker.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnSelectedColorChanged);
			
			return m_ColorPicker;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ColorPicker).CreatePropertyEditors(
				m_ColorPicker,
				NHsbColorPicker.SelectedColorProperty,
				NHsbColorPicker.HuePositionProperty,
				NHsbColorPicker.SpacingProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NCheckBox updateWhileDraggingCheckBox = new NCheckBox("Update While Dragging", true);
			updateWhileDraggingCheckBox.CheckedChanged += OnUpdateWhileDraggingCheckBoxCheckedChanged;
			stack.Add(updateWhileDraggingCheckBox);

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create, configure and use an HSB Box Color Picker. The HSB Box Color Picker consists
	of a hue color bar and a Saturation-Brightness color box. The controls on the right let you change the currently selected
	color, the hue position and the spacing between the SB color box and the hue color bar.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnUpdateWhileDraggingCheckBoxCheckedChanged(NValueChangeEventArgs arg1)
		{
			m_ColorPicker.UpdateWhileDragging = (bool)arg1.NewValue;
		}
		private void OnSelectedColorChanged(NValueChangeEventArgs arg1)
		{
			NColor selectedColor = (NColor)arg1.NewValue;
			m_EventsLog.LogEvent(NColor.GetNameOrHex(selectedColor));
		}

		#endregion

		#region Fields

		private NHsbColorPicker m_ColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NHSBColorPickerExampleSchema;		

		#endregion
	}
}