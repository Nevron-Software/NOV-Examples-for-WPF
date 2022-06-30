using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NHSBWheelColorPickerExample : NExampleBase
	{
		#region Constructors

		public NHSBWheelColorPickerExample()
		{
		}
		static NHSBWheelColorPickerExample()
		{
			NHSBWheelColorPickerExampleSchema = NSchema.Create(typeof(NHSBWheelColorPickerExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the HSB color picker
			m_ColorPicker = new NHsbWheelColorPicker();
			m_ColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ColorPicker.VerticalPlacement = ENVerticalPlacement.Top;
			m_ColorPicker.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnSelectedColorChanged);

			return m_ColorPicker;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ColorPicker).CreatePropertyEditors(
				m_ColorPicker,
				NHsbWheelColorPicker.UpdateWhileDraggingProperty,
				NHsbWheelColorPicker.SelectedColorProperty,
				NHsbWheelColorPicker.HueSelectorSectorAngleProperty,
				NHsbWheelColorPicker.HueSelectorExtendPercentProperty,
				NHsbWheelColorPicker.HueWheelWidthPercentProperty,
				NHsbWheelColorPicker.SBSelectorRadiusPercentProperty,
				NHsbWheelColorPicker.SBTriangleMarginsPercentProperty
			);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

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
	This example demonstrates how to create, configure and use an HSB Wheel Color Picker. The HSB Wheel Color Picker
	is a color picker that consists of a hue color wheel and a Saturation-Brightness triangle. The controls on the right
	let you control the appearance and the behavior of the picker.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnSelectedColorChanged(NValueChangeEventArgs args)
		{
			NColor selectedColor = (NColor)args.NewValue;
			m_EventsLog.LogEvent(NColor.GetNameOrHex(selectedColor));
		}

		#endregion

		#region Fields

		private NHsbWheelColorPicker m_ColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NHSBWheelColorPickerExampleSchema;		

		#endregion
	}
}