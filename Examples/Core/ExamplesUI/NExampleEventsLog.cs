using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples
{
    /// <summary>
    /// A group box that contains a list box, which is typically populated with the events handled by a specific example.
    /// </summary>
	public class NExampleEventsLog : NGroupBox
    {
        #region Constructors

        public NExampleEventsLog()
		{
			Header.Content = NWidget.FromObject("Events");

			NStackPanel stack = new NStackPanel();
			stack.FillMode = ENStackFillMode.Last;
			stack.FitMode = ENStackFitMode.Last;

			// log events
			m_LogEventCheck = new NCheckBox("Log Events");
			m_LogEventCheck.Checked = true;
			stack.Add(m_LogEventCheck);

			// clear button
			NButton button = new NButton("Clear Events Log");
			button.Content.HorizontalPlacement = ENHorizontalPlacement.Center;
			button.Click += new Function<NEventArgs>(OnClearEventsLogButtonClicked);
			stack.Add(button);

			// list
			m_EventsListBox = new NListBox();
			stack.Add(m_EventsListBox);

			Content = stack;
		}
		static NExampleEventsLog()
		{
			NExampleEventsLogSchema = NSchema.Create(typeof(NExampleEventsLog), NGroupBox.NGroupBoxSchema);
		}

        #endregion

        #region Public Methods

        /// <summary>
        /// Appends (logs) the specified event description.
        /// </summary>
        /// <param name="eventStr"></param>
        public void LogEvent(string eventStr)
		{
            if (m_LogEventCheck.Checked)
            {
                NListBoxItem item = new NListBoxItem(m_nEventCounter + ". " + eventStr);
                m_EventsListBox.Items.Add(item);
                m_EventsListBox.EnsureVisible(item);
                m_nEventCounter++;
            }
		}

        #endregion

        #region Event Handlers

        /// <summary>
        /// Called when the Clear Events button was clicked
        /// </summary>
        /// <param name="args"></param>
        private void OnClearEventsLogButtonClicked(NEventArgs args)
		{
			m_EventsListBox.Items.Clear();
			m_nEventCounter = 0;
		}

        #endregion

        #region Fields

        NListBox m_EventsListBox;
		NCheckBox m_LogEventCheck;
		int m_nEventCounter = 0;

        #endregion

        #region Schema

        public static readonly NSchema NExampleEventsLogSchema;

        #endregion
    }
}