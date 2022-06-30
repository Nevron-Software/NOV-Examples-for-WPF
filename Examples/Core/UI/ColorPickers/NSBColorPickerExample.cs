using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NSBColorPickerExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NSBColorPickerExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NSBColorPickerExample()
		{
			NSBColorPickerExampleSchema = NSchema.Create(typeof(NSBColorPickerExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create the SB color box
			m_ColorPicker = new NSBColorPicker();
			m_ColorPicker.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_ColorPicker.VerticalPlacement = ENVerticalPlacement.Top;
			m_ColorPicker.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnSelectedColorChanged);

			return m_ColorPicker;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_ColorPicker).CreatePropertyEditors(
				m_ColorPicker,
				NSBColorPicker.UpdateWhileDraggingProperty,
				NSBColorPicker.SelectedColorProperty,
				NSBColorPicker.SBSelectorRadiusPercentProperty
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
	This example demonstrates how to create a Saturation-Brightness color box. The SB color box lets the user modify
	the saturation and brightness of a color with a specified hue component. The hue component can be controlled through
	the <b>Hue</b> property and the currently selected color is stored in the <b>SelectedColor</b> property.
	The <b>SBSelectorRadiusPercent</b> property determines the size of the circular color selector and the 
	<b>UpdateWhileDragging</b> property specifies whether the selected color should be updated while the user drags
	the color selector or only when the user drops it. You can modify the values of all these properties using the controls
	on the rights.
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

		private NSBColorPicker m_ColorPicker;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NSBColorPickerExample.
		/// </summary>
		public static readonly NSchema NSBColorPickerExampleSchema;

		#endregion
	}
}