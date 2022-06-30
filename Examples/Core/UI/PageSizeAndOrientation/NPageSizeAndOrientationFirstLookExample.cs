using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Editors;

namespace Nevron.Nov.Examples.UI
{
    public class NPageSizeAndOrientationFirstLookExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NPageSizeAndOrientationFirstLookExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NPageSizeAndOrientationFirstLookExample()
        {
            NPageSizeAndOrientationFirstLookExampleSchema = NSchema.Create(typeof(NPageSizeAndOrientationFirstLookExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create the host
            NStackPanel stack = new NStackPanel();
            stack.HorizontalPlacement = ENHorizontalPlacement.Left;
            stack.VerticalPlacement = ENVerticalPlacement.Top;
            stack.VerticalSpacing = 5;

            // Page size button
            m_PageSizeDD = new NPageSizeDropDown();
            m_PageSizeDD.HorizontalPlacement = ENHorizontalPlacement.Left;
            m_PageSizeDD.SelectedPageSizeChanged += OnPageSizeDDSelectedPageSizeChanged;
            stack.Add(m_PageSizeDD);

            // Page orientation button
            m_PageOrientationDD = new NPageOrientationDropDown();
            m_PageOrientationDD.SelectedPageOrientationChanged += OnPageOrientationDDSelectedPageOrientationChanged;
            stack.Add(m_PageOrientationDD);

            return stack;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FillMode = ENStackFillMode.Last;
            stack.FitMode = ENStackFitMode.Last;

            NComboBox lookComboBox = new NComboBox();
            lookComboBox.FillFromEnum<ENExtendedLook>();
			lookComboBox.SelectedIndexChanged += OnLookComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Look:", lookComboBox));

            // Add the events log
            m_EventsLog = new NExampleEventsLog();
            stack.Add(m_EventsLog);

            return stack;
        }
		protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates the Page Size selection drop down.</p>";
        }

		#endregion

		#region Event Handlers

		private void OnPageOrientationDDSelectedPageOrientationChanged(NValueChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Selected page orientation changed to: " + m_PageOrientationDD.SelectedPageOrientation.ToString());
        }
        private void OnPageSizeDDSelectedPageSizeChanged(NValueChangeEventArgs arg)
        {
            m_EventsLog.LogEvent("Selected page size changed to: " + m_PageSizeDD.SelectedPageSize.ToString());
        }
		private void OnLookComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
            ENExtendedLook look = (ENExtendedLook)arg.NewValue;
            NStylePropertyEx.SetExtendedLook(m_PageSizeDD, look);
            NStylePropertyEx.SetExtendedLook(m_PageOrientationDD, look);
        }

		#endregion

		#region Fields

		private NPageSizeDropDown m_PageSizeDD;
        private NPageOrientationDropDown m_PageOrientationDD;
        private NExampleEventsLog m_EventsLog;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NPageSizeAndOrientationFirstLookExample.
        /// </summary>
        public static readonly NSchema NPageSizeAndOrientationFirstLookExampleSchema;

        #endregion
    }
}