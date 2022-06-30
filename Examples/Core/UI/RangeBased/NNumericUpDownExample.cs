using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NNumericUpDownExample : NExampleBase
	{
		#region Constructors

		public NNumericUpDownExample()
		{
		}
		static NNumericUpDownExample()
		{
			NNumericUpDownExampleSchema = NSchema.Create(typeof(NNumericUpDownExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();

			m_NumericUpDown = new NNumericUpDown();
			m_NumericUpDown.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_NumericUpDown.Minimum = 0;
			m_NumericUpDown.Maximum = 100;
			m_NumericUpDown.Value = 50;
			m_NumericUpDown.Step = 1;
			m_NumericUpDown.DecimalPlaces = 2;
			m_NumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnValueChanged);

			stack.Add(m_NumericUpDown);
			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_NumericUpDown).CreatePropertyEditors(
				m_NumericUpDown,
				NNumericUpDown.EnabledProperty,
				NNumericUpDown.ValueProperty,
				NNumericUpDown.MinimumProperty,
				NNumericUpDown.MaximumProperty,
				NNumericUpDown.StepProperty,
				NNumericUpDown.DecimalPlacesProperty,
                NNumericUpDown.WheelIncDecModeProperty,
				NNumericUpDown.TextSelectionModeProperty
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
	This example demonstrates how to create and use numeric up downs. The numeric up down widget
	consists of a text box and a spinner with up and down buttons. The text box shows and also
	can be used to edit the current numeric value and accepts only digits and the decimal separator.
	The current value is stored in the <b>Value</b> property. The <b>Step</b> property defines how
	much to increase/decrease the current value when the user clicks on the increase/decrease spinner
	button or presses the up/down key from the keyboard while the numeric up down text box is focused.
	The <b>TextSelectionMode</b> controls how the text selection mode is changed when the user presses 
	the numeric up-down spin buttons.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnValueChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("New value: " + args.NewValue.ToString());
		}

		#endregion

		#region Fields

		private NNumericUpDown m_NumericUpDown;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NNumericUpDownExampleSchema;

		#endregion
	}
}