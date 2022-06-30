using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NScrollBarsExample : NExampleBase
	{
		#region Constructors

		public NScrollBarsExample()
		{
		}
		static NScrollBarsExample()
		{
			NScrollBarsExampleSchema = NSchema.Create(typeof(NScrollBarsExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// create the root tab
			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;
			
			// create the HScrollBar
			m_HScrollBar = new NHScrollBar();
			m_HScrollBar.VerticalPlacement = ENVerticalPlacement.Top;
			m_HScrollBar.StartScrolling += new Function<NEventArgs>(OnScrollBarStartScrolling);
			m_HScrollBar.ValueChanged += new Function<NValueChangeEventArgs>(OnScrollBarValueChanged);
			m_HScrollBar.EndScrolling += new Function<NEventArgs>(OnScrollBarEndScrolling);
			stack.Add(new NGroupBox("Horizontal", m_HScrollBar));

			// create the VScrollBar
			m_VScrollBar = new NVScrollBar();
			m_VScrollBar.HorizontalPlacement = ENHorizontalPlacement.Left;
			m_VScrollBar.StartScrolling += new Function<NEventArgs>(OnScrollBarStartScrolling);
			m_VScrollBar.ValueChanged += new Function<NValueChangeEventArgs>(OnScrollBarValueChanged);
			m_VScrollBar.EndScrolling += new Function<NEventArgs>(OnScrollBarEndScrolling);
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

			NProperty[] properties = new NProperty[] {
				NScrollBar.EnabledProperty,
				NScrollBar.ValueProperty,
				NScrollBar.SmallChangeProperty,
				NScrollBar.LargeChangeProperty,
				NScrollBar.SnappingStepProperty,
				NScrollBar.MinimumProperty,
				NScrollBar.MaximumProperty
			};

			// Create the Horizontal scrollbar properties
			NStackPanel hsbStack = new NStackPanel();
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_HScrollBar).CreatePropertyEditors(
				m_HScrollBar, properties);
			
			for (int i = 0; i < editors.Count; i++)
			{
				hsbStack.Add(editors[i]);
			}

			tab.TabPages.Add(new NTabPage("Horizontal", hsbStack));

			// Create the Vertical scrollbar properties
			NStackPanel vsbStack = new NStackPanel();
			editors = NDesigner.GetDesigner(m_VScrollBar).CreatePropertyEditors(
				m_VScrollBar, properties);

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
	This example demonstrates how to create and use scrollbars. Scrollbars are range based widgets, which come in handy
	when you need to display large content in a limited area on screen. The scrollbars let the user change the currently
	visible part of this large content by dragging a thumb or clicking on an arrow button. The scrollbars can be horizontal
	and vertical and expose a set of properties, which you can use to control their appearance and behavior.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnScrollBarStartScrolling(NEventArgs arg1)
		{
			m_EventsLog.LogEvent("Start Scrolling");
		}
		private void OnScrollBarEndScrolling(NEventArgs arg1)
		{
			m_EventsLog.LogEvent("End Scrolling");
		}
		private void OnScrollBarValueChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Value: " + args.NewValue.ToString());
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;
		private NHScrollBar m_HScrollBar;
		private NVScrollBar m_VScrollBar;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NScrollBarsExample.
		/// </summary>
		public static readonly NSchema NScrollBarsExampleSchema;

		#endregion
	}
}