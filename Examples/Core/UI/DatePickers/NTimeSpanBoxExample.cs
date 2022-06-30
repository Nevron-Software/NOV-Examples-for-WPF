using System;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.UI
{
	public class NTimeSpanBoxExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NTimeSpanBoxExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NTimeSpanBoxExample()
		{
			NTimeSpanBoxExampleSchema = NSchema.Create(typeof(NTimeSpanBoxExample), NExampleBase.NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NTimeSpanBox timeSpanBox = new NTimeSpanBox();
			timeSpanBox.HorizontalPlacement = ENHorizontalPlacement.Left;
			timeSpanBox.VerticalPlacement = ENVerticalPlacement.Top;

			// Add some time spans to the box
			timeSpanBox.AddItem(TimeSpan.MinValue);
			timeSpanBox.AddItem(TimeSpan.Zero);

			timeSpanBox.AddItem(TimeSpan.FromMinutes(10));
			timeSpanBox.AddItem(TimeSpan.FromMinutes(15));
			timeSpanBox.AddItem(TimeSpan.FromMinutes(30));

			timeSpanBox.AddItem(TimeSpan.FromHours(1));
			timeSpanBox.AddItem(TimeSpan.FromHours(1.5));
			timeSpanBox.AddItem(TimeSpan.FromHours(2));
			timeSpanBox.AddItem(TimeSpan.FromHours(3));
			timeSpanBox.AddItem(TimeSpan.FromHours(6));
			timeSpanBox.AddItem(TimeSpan.FromHours(8));
			timeSpanBox.AddItem(TimeSpan.FromHours(12));

			timeSpanBox.AddItem(TimeSpan.FromDays(1));
			timeSpanBox.AddItem(TimeSpan.FromDays(1.5));
			timeSpanBox.AddItem(TimeSpan.FromDays(2));

			// Add the "Custom..." option
			timeSpanBox.AddCustomItem();

			// Select the first item
			timeSpanBox.SelectedIndex = 0;

			// Subscribe to the SelectedIndex changed event
			timeSpanBox.SelectedIndexChanged += OnTimeSpanBoxSelectedIndexChanged;

			return timeSpanBox;
		}
		protected override NWidget CreateExampleControls()
		{
			m_EventsLog = new NExampleEventsLog();
			return m_EventsLog;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to create and configure a time span box. Using the controls on the right you can
	modify various aspects of the time span box.
</p>
";
		}

		#endregion

		#region Event Handlers

		private void OnTimeSpanBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			NTimeSpanBox timeSpanBox = (NTimeSpanBox)arg.TargetNode;

			// Log the selected time span
			TimeSpan timeSpan = timeSpanBox.SelectedTimeSpan;
			if (timeSpan != TimeSpan.MinValue)
			{
				m_EventsLog.LogEvent("Selected time span: " + timeSpan.ToString());
			}
			else
			{
				m_EventsLog.LogEvent("Selected time span: none");
			}
		}

		#endregion

		#region Fields

		private NExampleEventsLog m_EventsLog;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NTimeSpanBoxExample.
		/// </summary>
		public static readonly NSchema NTimeSpanBoxExampleSchema;

		#endregion
	}
}