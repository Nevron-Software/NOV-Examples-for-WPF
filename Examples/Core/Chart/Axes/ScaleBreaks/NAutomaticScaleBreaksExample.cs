using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Automatic Scale Breaks Example
	/// </summary>
	public class NAutomaticScaleBreaksExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAutomaticScaleBreaksExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAutomaticScaleBreaksExample()
		{
			NAutomaticScaleBreaksExampleSchema = NSchema.Create(typeof(NAutomaticScaleBreaksExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Automatic Scale Breaks";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// configure scale
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
			yScale.MinTickDistance = 30;

			// add an interlaced strip to the Y axis
			NScaleStrip interlacedStrip = new NScaleStrip();
			interlacedStrip.Interlaced = true;
			interlacedStrip.Fill= new NColorFill(NColor.Beige);
			yScale.Strips.Add(interlacedStrip);

			OnChangeDataButtonClick(null);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_EnableScaleBreaksCheckBox = new NCheckBox("Enable Scale Breaks");
			m_EnableScaleBreaksCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableScaleBreaksCheckBoxCheckedChanged);
			stack.Add(m_EnableScaleBreaksCheckBox);

			m_ThresholdUpDown = new NNumericUpDown ();
			m_ThresholdUpDown.Step = 0.05;
			m_ThresholdUpDown.Maximum = 1;
			m_ThresholdUpDown.DecimalPlaces = 2;
			m_ThresholdUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnThresholdUpDownValueChanged);
			stack.Add(NPairBox.Create("Threshold", m_ThresholdUpDown));

			m_LengthUpDown = new NNumericUpDown();
			m_LengthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnLengthUpDownValueChanged);
			stack.Add(NPairBox.Create("Length", m_LengthUpDown));

			m_MaxBreaksUpDown = new NNumericUpDown();
			m_MaxBreaksUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMaxBreaksUpDownValueChanged);
			stack.Add(NPairBox.Create("Max Breaks", m_MaxBreaksUpDown));

			m_PositionModeComboBox = new NComboBox();
			m_PositionModeComboBox.FillFromEnum<ENScaleBreakPositionMode>();
			m_PositionModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPositionModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Position Mode:", m_PositionModeComboBox));

			m_PositionPercentUpDown = new NNumericUpDown();
			m_PositionPercentUpDown.Minimum = 0;
			m_PositionPercentUpDown.Maximum = 100;
			m_PositionPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnPositionPercentUpDownValueChanged);
			stack.Add(NPairBox.Create("Position Percent:", m_PositionPercentUpDown));

			NButton changeDataButton = new NButton("Change Data");
			changeDataButton.Click += new Function<NEventArgs>(OnChangeDataButtonClick);
			stack.Add(changeDataButton);

			// update initial state
			m_EnableScaleBreaksCheckBox.Checked = true;
			m_LengthUpDown.Value = 5;
			m_ThresholdUpDown.Value = 0.25; // this is relatively low factor
			m_MaxBreaksUpDown.Value = 1;
			m_PositionPercentUpDown.Value = 50;
			m_PositionModeComboBox.SelectedIndex = (int)ENScaleBreakPositionMode.Content; // use content mode by default

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to add automatic scale breaks.</p>";
		}

		#endregion

		#region Event Handlers

		void OnChangeDataButtonClick(NEventArgs arg)
		{
			m_Chart.Series.Clear();

            // setup bar series
            NBarSeries bar = new NBarSeries();
			m_Chart.Series.Add(bar);

            bar.DataLabelStyle = new NDataLabelStyle(false);
			bar.Stroke = new NStroke(1.5f, NColor.DarkGreen);

            // fill in some data so that it contains several peaks of data
            Random random = new Random();
            double value = 0;

            for (int i = 0; i < 25; i++)
            {
                if (i < 6)
                {
                    value = 600 + random.Next(30);
                }
                else if (i < 11)
                {
                    value = 200 + random.Next(50);
                }
                else if (i < 16)
                {
                    value = 400 + random.Next(50);
                }
                else if (i < 21)
                {
                    value = random.Next(30);
                }
                else
                {
                    value = random.Next(50);
                }

				NBarDataPoint dataPoint = new NBarDataPoint(value);
				dataPoint.Fill = new NStockGradientFill(ColorFromValue(value), NColor.DarkSlateBlue);
                bar.DataPoints.Add(dataPoint);
            }
		}

		void OnPositionPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();
		}

		void OnPositionModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();
			m_PositionPercentUpDown.Enabled = m_PositionModeComboBox.SelectedIndex == (int)ENScaleBreakPositionMode.Percent;
		}

		void OnMaxBreaksUpDownValueChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();
		}

		void OnLengthUpDownValueChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();
		}

		void OnThresholdUpDownValueChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();
		}

		void OnEnableScaleBreaksCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			UpdateScaleBreak();

			bool enableControls = m_EnableScaleBreaksCheckBox.Checked;

			m_ThresholdUpDown.Enabled = enableControls;
			m_LengthUpDown.Enabled = enableControls;
			m_MaxBreaksUpDown.Enabled = enableControls;
			m_PositionModeComboBox.Enabled = enableControls;
			m_PositionPercentUpDown.Enabled = enableControls && m_PositionModeComboBox.SelectedIndex == (int)ENScaleBreakPositionMode.Percent;
		}

		#endregion

		#region Implementation

		private void UpdateScaleBreak()
		{
			NLinearScale scale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			scale.ScaleBreaks.Clear();

			if (m_EnableScaleBreaksCheckBox.Checked)
			{
				NAutoScaleBreak scaleBreak = new NAutoScaleBreak((float)m_ThresholdUpDown.Value);

				scaleBreak.Style = ENScaleBreakStyle.Line;
				scaleBreak.Length = m_LengthUpDown.Value;
				scaleBreak.MaxScaleBreakCount = (int)m_MaxBreaksUpDown.Value;

				scaleBreak.PositionMode = (ENScaleBreakPositionMode)m_PositionModeComboBox.SelectedIndex;
				scaleBreak.Percent = (float)m_PositionPercentUpDown.Value;

				scale.ScaleBreaks.Add(scaleBreak);
			}
		}

        private static NColor ColorFromValue(double value)
        {
            NColor beginColor = NColor.LightBlue;
            NColor endColor = NColor.DarkSlateBlue;

            float factor = (float)(value / 650.0f);
            float oneMinusFactor = 1.0f - factor;

			return NColor.FromRGB((byte)((float)beginColor.R * factor + (float)endColor.R * oneMinusFactor),
                                    (byte)((float)beginColor.G * factor + (float)endColor.G * oneMinusFactor),
                                    (byte)((float)beginColor.B * factor + (float)endColor.B * oneMinusFactor));
        }

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NCheckBox m_EnableScaleBreaksCheckBox;
		NNumericUpDown m_ThresholdUpDown;
		NNumericUpDown m_LengthUpDown;
		NNumericUpDown m_MaxBreaksUpDown;
		NComboBox m_PositionModeComboBox;
		NNumericUpDown m_PositionPercentUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NAutomaticScaleBreaksExampleSchema;

		#endregion
	}
}