using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Gauge
{
	/// <summary>
	/// This example demonstrates the functionality of the NNumericDisplayPanel.
	/// </summary>
	public class NNumericLedDisplayExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public NNumericLedDisplayExample()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        static NNumericLedDisplayExample()
        {
			NNumericLedDisplayExampleSchema = NSchema.Create(typeof(NNumericLedDisplayExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
		{
			NStackPanel stack = new NStackPanel();
			stack.Unregistered += OnStackUnregistered;

			stack.HorizontalPlacement = ENHorizontalPlacement.Left;

			m_NumericDisplay1 = CreateNumericLedDisplay();
			stack.Add(m_NumericDisplay1);

			m_NumericDisplay2 = CreateNumericLedDisplay();
			stack.Add(m_NumericDisplay2);

			m_NumericDisplay3 = CreateNumericLedDisplay();
			stack.Add(m_NumericDisplay3);

			m_DataFeedTimer = new NTimer();
			m_DataFeedTimer.Tick += new Function(OnDataFeedTimerTick);
			m_DataFeedTimer.Start();
			
			return stack;
		}
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();

			NStackPanel propertyStack = new NStackPanel();
			stack.Add(new NUniSizeBoxGroup(propertyStack));

			// init form controls
			m_CellSizeComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Cell Size:", m_CellSizeComboBox, true));

			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Small"));
			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Normal"));
			m_CellSizeComboBox.Items.Add(new NComboBoxItem("Large"));
			m_CellSizeComboBox.SelectedIndex = 1;
			m_CellSizeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnCellSizeComboBoxSelectedIndexChanged);

			m_DisplayStyleComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Display Style:", m_DisplayStyleComboBox, true));
			m_DisplayStyleComboBox.FillFromEnum<ENDisplayStyle>();
			m_DisplayStyleComboBox.SelectedIndex = (int)m_NumericDisplay1.DisplayStyle;
			m_DisplayStyleComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnDisplayStyleComboBoxSelectedIndexChanged);
			
			m_ContentAlignmentComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Content Alignment:", m_ContentAlignmentComboBox, true));

			m_ContentAlignmentComboBox.FillFromEnum<ENContentAlignment>();
			m_ContentAlignmentComboBox.SelectedIndex = (int)m_NumericDisplay1.ContentAlignment;
			m_ContentAlignmentComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnContentAlignmentComboBoxSelectedIndexChanged);

			m_SignModeComboBox = new NComboBox();
			propertyStack.Add(new NPairBox("Sign Mode", m_SignModeComboBox, true));

			m_SignModeComboBox.FillFromEnum<ENDisplaySignMode>();
			m_SignModeComboBox.SelectedIndex = (int)m_NumericDisplay1.SignMode;
			m_SignModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnSignModeComboBoxSelectedIndexChanged);

			m_ShowLeadingZerosCheckBox = new NCheckBox("Show Leading Zeroes");
			propertyStack.Add(m_ShowLeadingZerosCheckBox);
			m_ShowLeadingZerosCheckBox.CheckedChanged +=new Function<NValueChangeEventArgs>(OnShowLeadingZerosCheckBoxCheckedChanged);

			m_AttachSignToNumberCheckBox = new NCheckBox("Attach Sign to Number");
			propertyStack.Add(m_AttachSignToNumberCheckBox);
			m_AttachSignToNumberCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnAttachSignToNumberCheckBoxCheckedChanged);

			NButton litFillButton = new NButton("Lit Fill");
			litFillButton.Click += new Function<NEventArgs>(OnLitFillButtonClick);
			propertyStack.Add(litFillButton);

			NButton dimFillButton = new NButton("Dim Fill");
			dimFillButton.Click += new Function<NEventArgs>(OnDimFillButtonClick);
			propertyStack.Add(dimFillButton);

			m_StopStartTimerButton = new NButton("Stop Timer");
            m_StopStartTimerButton.Click += OnStopStartTimerButtonClick;
			propertyStack.Add(m_StopStartTimerButton);

			return stack;
        }
		protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the properties of the numeric led display.</p>";
		}

		#endregion 

		#region Implementation

		private NNumericLedDisplay CreateNumericLedDisplay()
		{
			NNumericLedDisplay numericLedDisplay = new NNumericLedDisplay();

			numericLedDisplay.Value = 0.0;
			numericLedDisplay.CellCountMode = ENDisplayCellCountMode.Fixed;
			numericLedDisplay.CellCount = 7;
			numericLedDisplay.BackgroundFill = new NColorFill(NColor.Black);

			numericLedDisplay.Border = NBorder.CreateSunken3DBorder(new NUIThemeColorMap(ENUIThemeScheme.WindowsClassic));
			numericLedDisplay.BorderThickness = new NMargins(6);
			numericLedDisplay.Margins = new NMargins(5);
			numericLedDisplay.Padding = new NMargins(5);
			numericLedDisplay.CapEffect = new NGelCapEffect();

			return numericLedDisplay;
		}

		#endregion

		#region Event Handlers

        void OnStopStartTimerButtonClick(NEventArgs arg)
        {
            NLabel label = ((NLabel)((NButton)arg.TargetNode).Content);
            if (label.Text.StartsWith("Stop"))
            {
                label.Text = "Start Timer";
                m_DataFeedTimer.Stop();
            }
            else
            {
                label.Text = "Stop Timer";
                m_DataFeedTimer.Start();
            }
        }

		void OnDataFeedTimerTick()
		{
			double value1 = -50 + m_Random.Next(10000) / 100.0;
			m_NumericDisplay1.Value = value1;

			double value2;

			if (m_Counter % 4 == 0)
			{
				value2 = -50 + m_Random.Next(10000) / 100.0;
				m_NumericDisplay2.Value = value2;
			}

			double value3;
			if (m_Counter % 8 == 0)
			{
				value3 = 200 + m_Random.Next(10000) / 100.0;
				m_NumericDisplay3.Value = value3;
			}

			m_Counter++;
		}

		void OnLitFillButtonClick(NEventArgs arg)
		{
			NEditorWindow.CreateForType(
                (NFill)m_NumericDisplay1.LitFill.DeepClone(), 
                null,
                m_NumericDisplay1.DisplayWindow, 
                false, 
                OnLitFillEdited).Open();
		}

		void OnLitFillEdited(NFill fill)
		{
			m_NumericDisplay1.LitFill = (NFill)(fill.DeepClone());
			m_NumericDisplay2.LitFill = (NFill)(fill.DeepClone());
			m_NumericDisplay3.LitFill = (NFill)(fill.DeepClone());
		}

		void OnDimFillButtonClick(NEventArgs arg)
		{
			NEditorWindow.CreateForType(
                (NFill)m_NumericDisplay1.DimFill.DeepClone(), 
                null,
				m_NumericDisplay1.DisplayWindow, 
                false, 
                OnDimFillEdited).Open();
		}

		void OnDimFillEdited(NFill fill)
		{
			m_NumericDisplay1.DimFill = (NFill)(fill.DeepClone());
			m_NumericDisplay2.DimFill = (NFill)(fill.DeepClone());
			m_NumericDisplay3.DimFill = (NFill)(fill.DeepClone());
		}

		void OnCellSizeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			double segmentWidth = 0.0;
			double segmentGap = 0.0;
			NSize cellSize = new NSize(0.0, 0.0);

			switch (m_CellSizeComboBox.SelectedIndex)
			{
				case 0: // small
					segmentWidth = 2.0;
					segmentGap = 1.0;
					cellSize = new NSize(15, 30);
					break;
				case 1: // normal
					segmentWidth = 3;
					segmentGap = 1;
					cellSize = new NSize(20, 40);
					break;
				case 2: // large
					segmentWidth = 4;
					segmentGap = 2;
					cellSize = new NSize(26, 52);
					break;
			}

			NNumericLedDisplay[] displays = new NNumericLedDisplay[] { m_NumericDisplay1, m_NumericDisplay2, m_NumericDisplay3 };

			for (int i = 0; i < displays.Length; i++)
			{
				NNumericLedDisplay display = displays[i];

				display.CellSize = cellSize;
				display.SegmentGap = segmentGap;
				display.SegmentWidth = segmentWidth;

				display.DecimalCellSize = cellSize;
				display.DecimalSegmentGap = segmentGap;
				display.DecimalSegmentWidth = segmentWidth;
			}
		}

		void OnDisplayStyleComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_NumericDisplay1.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
			m_NumericDisplay2.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
			m_NumericDisplay3.DisplayStyle = (ENDisplayStyle)m_DisplayStyleComboBox.SelectedIndex;
		}

		void OnContentAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_NumericDisplay1.ContentAlignment = (ENContentAlignment)m_ContentAlignmentComboBox.SelectedIndex;
			m_NumericDisplay2.ContentAlignment = (ENContentAlignment)m_ContentAlignmentComboBox.SelectedIndex;
			m_NumericDisplay3.ContentAlignment = (ENContentAlignment)m_ContentAlignmentComboBox.SelectedIndex;			
		}

		private void OnStopStartTimerButtonClick(object sender, System.EventArgs e)
		{
			if (this.m_DataFeedTimer.IsStarted)
			{
				this.m_DataFeedTimer.Stop();
				m_StopStartTimerButton.Content = new NLabel("Start Timer");
			}
			else
			{
				this.m_DataFeedTimer.Start();
				m_StopStartTimerButton.Content = new NLabel("Stop Timer");
			}
		}

		void OnSignModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_NumericDisplay1.SignMode = (ENDisplaySignMode)m_SignModeComboBox.SelectedIndex;
			m_NumericDisplay2.SignMode = (ENDisplaySignMode)m_SignModeComboBox.SelectedIndex;
			m_NumericDisplay3.SignMode = (ENDisplaySignMode)m_SignModeComboBox.SelectedIndex;
		}

		void OnShowLeadingZerosCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_NumericDisplay1.ShowLeadingZeros = m_ShowLeadingZerosCheckBox.Checked;
			m_NumericDisplay2.ShowLeadingZeros = m_ShowLeadingZerosCheckBox.Checked;
			m_NumericDisplay3.ShowLeadingZeros = m_ShowLeadingZerosCheckBox.Checked;
		}

		void OnAttachSignToNumberCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_NumericDisplay1.AttachSignToNumber = m_AttachSignToNumberCheckBox.Checked;
			m_NumericDisplay2.AttachSignToNumber = m_AttachSignToNumberCheckBox.Checked;
			m_NumericDisplay3.AttachSignToNumber = m_AttachSignToNumberCheckBox.Checked;
		}

		private void OnStackUnregistered(NEventArgs arg)
		{
			m_DataFeedTimer.Stop();
		}

		#endregion

		#region Fields

		private NButton m_StopStartTimerButton;
		private NCheckBox m_AttachSignToNumberCheckBox;
		private NCheckBox m_ShowLeadingZerosCheckBox;
		private NComboBox m_SignModeComboBox;
		private NComboBox m_ContentAlignmentComboBox;
		private NComboBox m_DisplayStyleComboBox;

		private NNumericLedDisplay m_NumericDisplay1;
		private NNumericLedDisplay m_NumericDisplay2;
		private NNumericLedDisplay m_NumericDisplay3;
		private NTimer m_DataFeedTimer;
		private NComboBox m_CellSizeComboBox;
		private int m_Counter = 0;
		private Random m_Random = new Random();

		#endregion

		#region Schema

		public static readonly NSchema NNumericLedDisplayExampleSchema;

        #endregion
	}
}