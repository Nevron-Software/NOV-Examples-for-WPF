using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NSliderExample : NExampleBase
	{
		#region Constructors

		public NSliderExample()
		{
		}
		static NSliderExample()
		{
			NSliderExampleSchema = NSchema.Create(typeof(NSliderExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();

			NGroupBox gbh = new NGroupBox("Horizontal");
			stack.Add(gbh);
			m_HSlider = new NSlider();
			gbh.Content = m_HSlider;
			m_HSlider.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_HSlider.PreferredWidth = 300;
			m_HSlider.ValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);

			NGroupBox gbv = new NGroupBox("Vertical");
			stack.Add(gbv);
			m_VSlider = new NSlider();
			gbv.Content = m_VSlider;
			m_VSlider.Orientation = ENHVOrientation.Vertical;
			m_VSlider.PreferredHeight = 300;
			m_VSlider.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_VSlider.ValueChanged += new Function<NValueChangeEventArgs>(OnSliderValueChanged);

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
				NSlider.EnabledProperty,
				NSlider.ValueProperty,
				NSlider.LargeChangeProperty,
				NSlider.SnappingStepProperty,
				NSlider.MinimumProperty,
				NSlider.MaximumProperty,
				NSlider.TicksPlacementProperty,
				NSlider.TicksIntervalProperty,
				NSlider.TicksLengthProperty
			};

			// Create the Horizontal slider properties
			NStackPanel hsbStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HSlider).CreatePropertyEditors(
				m_HSlider, properties);

			for (int i = 0; i < editors.Count; i++)
			{
				hsbStack.Add(editors[i]);
			}

			tab.TabPages.Add(new NTabPage("Horizontal", hsbStack));

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
	This example demonstrates how to create and use sliders. The slider is a widget that
	lets the user select a distinct value in a given range by dragging a thumb. You can
	specify whether the slider is horizontally or vertically oriented through its
	<b>Orientation</b> property. To control the tick visibility and placement use the
	<b>TicksLength</b>, <b>TicksInterval</b> and <b>TicksPlacement</b> properties.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnSliderValueChanged(NValueChangeEventArgs args)
		{
			string text = args.TargetNode == m_HSlider ? "Horizontal: " : "Vertical: ";
			text += args.NewValue.ToString();
			m_EventsLog.LogEvent(text);
		}

		#endregion

		#region Fields

		private NSlider m_HSlider;
		private NSlider m_VSlider;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		public static readonly NSchema NSliderExampleSchema;

		#endregion
	}
}