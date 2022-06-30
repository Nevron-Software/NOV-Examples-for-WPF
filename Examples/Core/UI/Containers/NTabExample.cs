using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTabExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTabExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTabExample()
		{
			NTabExampleSchema = NSchema.Create(typeof(NTabExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// create a tab
			m_Tab = new NTab();

			m_Tab.TabPages.Add(CreatePage("Page 1", "This is the first tab page."));
			m_Tab.TabPages.Add(CreatePage("Page 2", "This is the second tab page."));
			m_Tab.TabPages.Add(CreatePage("Page 3", "This is the third tab page.\nIt is the largest both horizontally and vertically."));
			m_Tab.TabPages.Add(CreatePage("Page 4", "This is the fourth tab page."));
			m_Tab.TabPages.Add(CreatePage("Page 5", "This is the fifth tab page."));

			m_Tab.SelectedIndex = 0;
			m_Tab.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnTabSelectedIndexChanged);

			// host it
			return m_Tab;
		}
		protected override NWidget CreateExampleControls()
		{
			NList<NPropertyEditor> editors = NDesigner.GetDesigner(m_Tab).CreatePropertyEditors(
				m_Tab,
				NTab.EnabledProperty,
				NTab.SizeToSelectedPageProperty,
				NTab.CycleTabPagesProperty,
				NTab.HeadersPositionProperty,
				NTab.HeadersModeProperty,
				NTab.HeadersAlignmentProperty,
				NTab.HeadersSpacingProperty,
				NTab.HorizontalPlacementProperty,
				NTab.VerticalPlacementProperty);

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			for (int i = 0; i < editors.Count; i++)
			{
				stack.Add(editors[i]);
			}

			// create the events list box
			m_EventsLog = new NExampleEventsLog();
			stack.Add(m_EventsLog);

			return new NUniSizeBoxGroup(stack);
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create tab widgets. The tab is a widget that contains tab pages. Their order in the
	<b>TabPages</b> collection of the tab widget reflects the order they appear in the widget. The <b>SelectedIndex</b>
	property stores the index of the currently selected tab page. The <b>HeadersPosition</b> property determines the
	way the tab page headers are position in respect to the tab widget. The possible values are: <b>Left</b>, <b>Top</b>
	(default), <b>Right</b> and <b>Bottom</b>. You can also specify the spacing between the headers. To do so, use the
	<b>HeadersSpacing</b> property. The <b>HeadersAlignment</b> property determines how the tab page headers are aligned
	on the tab widget side they are placed on. The supported values are <b>Near</b> (default), <b>Center</b> and <b>Far</b>. 
	The <b>CycleTablePages</b> instructs the control to cycle pages when the tab has focus and the user presseses the left 
	or right arrow keys.
</p>
";
		}
		#endregion

		#region Implementation

		private NTabPage CreatePage(string name, string content)
		{
			NTabPage tabPage = new NTabPage(name, content);

			tabPage.Header.Content = NPairBox.Create("Text With Image:", new NImageBox(NResources.Image__16x16_folderDeleted_png));


			return tabPage;
		}

		#endregion

		#region Event Handlers

		private void OnHeadersPositionComboSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox combo = (NComboBox)args.TargetNode;
			ENTabHeadersPosition headersPosition = (ENTabHeadersPosition)args.NewValue;
			m_Tab.HeadersPosition = headersPosition;
		}
		private void OnHeadersSpacingComboSelectedIndexChanged(NValueChangeEventArgs args)
		{
			NComboBox combo = (NComboBox)args.TargetNode;
			m_Tab.HeadersSpacing = combo.SelectedIndex;
		}
		private void OnTabSelectedIndexChanged(NValueChangeEventArgs args)
		{
			m_EventsLog.LogEvent("Selected Index: " + args.NewValue);
		}

		#endregion

		#region Fields

		private NTab m_Tab;
		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTabExample.
		/// </summary>
		public static readonly NSchema NTabExampleSchema;

		#endregion
	}
}