using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NMeasureUpDownExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NMeasureUpDownExample()
		{
		}
		/// <summary>
		/// Static constructor.
		/// </summary>
		static NMeasureUpDownExample()
		{
			NMeasureUpDownExampleSchema = NSchema.Create(typeof(NMeasureUpDownExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_StackPanel = new NStackPanel();
			m_StackPanel.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_StackPanel.VerticalSpacing = 5;

			m_StackPanel.Add(CreatePairBox(
				"Angle:",
				NUnit.Radian, NUnit.Degree, NUnit.Grad)
			);

			m_StackPanel.Add(CreatePairBox(
				"SI Length:",
				NUnit.Millimeter, NUnit.Centimeter, NUnit.Meter, NUnit.Kilometer)
			);

			m_StackPanel.Add(CreatePairBox(
				"Imperial Length:",
				NUnit.Inch, NUnit.Foot, NUnit.Yard, NUnit.Mile)
			);

			m_StackPanel.Add(CreatePairBox(
				"Mixed Length:",
				NUnit.Millimeter, NUnit.Centimeter, NUnit.Meter, NUnit.Kilometer, NUnit.Inch, NUnit.Foot, NUnit.Yard, NUnit.Mile)
			);

			m_StackPanel.Add(CreatePairBox(
				"Graphics Length:",
				NUnit.Millimeter, NUnit.Inch, NUnit.DIP, NUnit.Point)
			);

			return new NUniSizeBoxGroup(m_StackPanel);
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			NCheckBox enabledCheckBox = new NCheckBox("Enabled", true);
			enabledCheckBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			enabledCheckBox.VerticalPlacement = ENVerticalPlacement.Top;
			enabledCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnabledCheckBoxCheckedChanged);
			stack.Add(enabledCheckBox);

			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return stack;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use measure up downs. The measure up down extends the functionality
	of the numeric up down by adding a combo box for selecting a unit. If the units can be converted to each other,
	when the user selects a new unit from the unit combo box, the value will be automatically converted. The <b>Unit</b>
	property determines the currently selected measurement unit.
</p>
";
		}

		#endregion

		#region Implementation

		private NPairBox CreatePairBox(string text, params NUnit[] units)
		{
			NLabel label = new NLabel(text);
			label.HorizontalPlacement = ENHorizontalPlacement.Right;
			label.VerticalPlacement = ENVerticalPlacement.Center;

			NMeasureUpDown measureUpDown = new NMeasureUpDown(units);
			measureUpDown.Minimum = Double.MinValue;
			measureUpDown.Maximum = Double.MaxValue;
			measureUpDown.Value = 1;
			measureUpDown.DecimalPlaces = 3;
			measureUpDown.UnitComboBox.PreferredWidth = 40;
			measureUpDown.ValueChanged += OnMeasureUpDownValueChanged;

			NPairBox pairBox = new NPairBox(label, measureUpDown, true);
			pairBox.Spacing = 3;
			return pairBox;
		}

		#endregion

		#region Event Handlers

		private void OnMeasureUpDownValueChanged(NValueChangeEventArgs args)
		{
			NMeasureUpDown measureUpDown = (NMeasureUpDown)args.TargetNode;
			double value = (double)args.NewValue;
			NUnit unit = measureUpDown.SelectedUnit;

			string unitString = unit.ToString();
			if (value != 1)
			{
				// Make the unit string plural
				if (unit == NUnit.Inch)
				{
					unitString = "inches";
				}
				else if (unit == NUnit.Foot)
				{
					unitString = "feet";
				}
				else
				{
					unitString += "s";
				}
			}

			// Log the event
			m_EventsLog.LogEvent("New value: " + value.ToString() + " " + unitString);
		}
		private void OnEnabledCheckBoxCheckedChanged(NValueChangeEventArgs args)
		{
			m_StackPanel.Enabled = (bool)args.NewValue;
		}

		#endregion

		#region Fields

		private NStackPanel m_StackPanel;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NMeasureUpDownExample.
		/// </summary>
		public static readonly NSchema NMeasureUpDownExampleSchema;

		#endregion

	}
}