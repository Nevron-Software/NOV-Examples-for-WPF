using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NRangeSliderExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NRangeSliderExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NRangeSliderExample()
		{
			NRangeSliderExampleSchema = NSchema.Create(typeof(NRangeSliderExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			
			m_HSlider = new NRangeSlider();
			m_HSlider.BeginValue = 20;
			m_HSlider.EndValue = 40;
			m_HSlider.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_HSlider.PreferredWidth = 300;
			m_HSlider.BeginValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);
			m_HSlider.EndValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);
			stack.Add(new NGroupBox("Horizontal", m_HSlider));

			m_VSlider = new NRangeSlider();
			m_VSlider.BeginValue = 20;
			m_VSlider.EndValue = 40;
			m_VSlider.Orientation = ENHVOrientation.Vertical;
			m_VSlider.PreferredHeight = 300;
			m_VSlider.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_VSlider.BeginValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);
			m_VSlider.EndValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);
			stack.Add(new NGroupBox("Vertical", m_VSlider));

			return stack;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// Create a tab
			NTab tab = new NTab();
			stack.Add(tab);

			NProperty[] properties = new NProperty[] {
				NRangeSlider.EnabledProperty,
				NRangeSlider.BeginValueProperty,
				NRangeSlider.EndValueProperty,
				NRangeSlider.LargeChangeProperty,
				NRangeSlider.SnappingStepProperty,
				NRangeSlider.MinimumProperty,
				NRangeSlider.MaximumProperty,
				NRangeSlider.TicksPlacementProperty,
				NRangeSlider.TicksIntervalProperty,
				NRangeSlider.TicksLengthProperty
			};

			// Create the Horizontal slider properties
			NStackPanel hsbStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HSlider).CreatePropertyEditors(
				m_HSlider, properties);

			for (int i = 0; i < editors.Count; i++)
			{
				hsbStack.Add(editors[i]);
			}

			tab.TabPages.Add(new NTabPage("Horzontal", hsbStack));

			// Create the Vertical slider properties
			NStackPanel vsbStack = new NStackPanel();
			editors = NDesigner.GetDesigner(m_VSlider).CreatePropertyEditors(
				m_VSlider, properties);

			for (int i = 0; i < editors.Count; i++)
			{
				vsbStack.Add(editors[i]);
			}

			tab.TabPages.Add(new NTabPage("Vertical", vsbStack));

			// Add events log
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and use range sliders. The range slider is a widget that
	lets the user select a range defined by a begin and end value in a given range by dragging thumbs.
	You can specify whether the slider is horizontally or vertically oriented through its
	<b>Orientation</b> property. To control the tick visibility and placement use the
	<b>TicksLength</b>, <b>TicksInterval</b> and <b>TicksPlacement</b> properties.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnSliderValueChanged(NValueChangeEventArgs args)
		{
			NRangeSlider slider = (NRangeSlider)args.TargetNode;
			string text = slider == m_HSlider ? "Horizontal Range: " : "Vertical Range: ";
			text += slider.BeginValue.ToString("0.###") + " - " + slider.EndValue.ToString("0.###");
			m_EventsLog.LogEvent(text);
		}

		#endregion

		#region Fields

		private NRangeSlider m_HSlider;
		private NRangeSlider m_VSlider;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRangeSliderExample.
		/// </summary>
		public static readonly NSchema NRangeSliderExampleSchema;

		#endregion
	}
}