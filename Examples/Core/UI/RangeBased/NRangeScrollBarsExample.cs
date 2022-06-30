using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NRangeScrollBarsExample : NExampleBase
	{
		#region Constructors

		public NRangeScrollBarsExample()
		{
		}

		static NRangeScrollBarsExample()
		{
			NRangeScrollBarsExampleSchema = NSchema.Create(typeof(NRangeScrollBarsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a stack for the scroll bars
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;
			
			// Create the HScrollBar
			m_HScrollBar = new NHRangeScrollBar();
			m_HScrollBar.BeginValue = 20;
			m_HScrollBar.EndValue = 40;
			m_HScrollBar.VerticalPlacement = ENVerticalPlacement.Top;
			m_HScrollBar.BeginValueChanged += OnScrollBarValueChanged;
			m_HScrollBar.EndValueChanged += OnScrollBarValueChanged;
			stack.Add(new NGroupBox("Horizontal", m_HScrollBar));

			// Create the VScrollBar
			m_VScrollBar = new NVRangeScrollBar();
			m_VScrollBar.BeginValue = 20;
			m_VScrollBar.EndValue = 40;
			m_VScrollBar.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_VScrollBar.BeginValueChanged += OnScrollBarValueChanged;
			m_VScrollBar.EndValueChanged += OnScrollBarValueChanged;
			stack.Add(new NGroupBox("Vertical", m_VScrollBar));

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

			// Create the Horizontal scrollbar properties
			NStackPanel hsbStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HScrollBar).CreatePropertyEditors(
				m_HScrollBar,
				NHRangeScrollBar.EnabledProperty,
				NHRangeScrollBar.BeginValueProperty,
				NHRangeScrollBar.EndValueProperty,
				NHRangeScrollBar.SmallChangeProperty,
				NHRangeScrollBar.SnappingStepProperty,
				NHRangeScrollBar.MinimumProperty,
				NHRangeScrollBar.MaximumProperty
			);
			
			for (int i = 0; i < editors.Count; i++)
			{
				hsbStack.Add(editors[i]);
			}

			tab.TabPages.Add(new NTabPage("Horizontal", hsbStack));

			// Create the Vertical scrollbar properties
			NStackPanel vsbStack = new NStackPanel();
			editors = NDesigner.GetDesigner(m_VScrollBar).CreatePropertyEditors(
				m_VScrollBar,
				NVRangeScrollBar.EnabledProperty,
				NVRangeScrollBar.BeginValueProperty,
				NVRangeScrollBar.EndValueProperty,
				NVRangeScrollBar.SmallChangeProperty,
				NVRangeScrollBar.SnappingStepProperty,
				NVRangeScrollBar.MinimumProperty,
				NVRangeScrollBar.MaximumProperty
			);

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
	This example demonstrates how to create and use range scroll bars. Range scroll bars are range based widgets, which
	are used for selecting a range defined by a begin and an end value. They can be horizontal and vertical and expose
	a set of properties, which you can use to control their appearance and behavior.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnScrollBarValueChanged(NValueChangeEventArgs args)
		{
			string text;
			if (args.TargetNode == m_HScrollBar)
			{
				text = "Horizontal Range: " + m_HScrollBar.BeginValue.ToString("0.###") + " - " +
					m_HScrollBar.EndValue.ToString("0.###");
			}
			else
			{
				text = "Vertical Range: " + m_VScrollBar.BeginValue.ToString("0.###") + " - " +
					m_VScrollBar.EndValue.ToString("0.###"); ;
			}

			m_EventsLog.LogEvent(text);
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NHRangeScrollBar m_HScrollBar;
		private NVRangeScrollBar m_VScrollBar;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NRangeScrollBarsExample.
		/// </summary>
		public static readonly NSchema NRangeScrollBarsExampleSchema;

		#endregion
	}
}