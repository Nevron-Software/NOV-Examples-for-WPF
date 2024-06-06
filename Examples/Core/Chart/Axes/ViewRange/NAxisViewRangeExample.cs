using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Axis View Range example.
    /// </summary>
    public class NAxisViewRangeExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NAxisViewRangeExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NAxisViewRangeExample()
        {
            NAxisViewRangeExampleSchema = NSchema.Create(typeof(NAxisViewRangeExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Axis View Range";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            // configure axes
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            m_Chart.Axes[ENCartesianAxis.PrimaryY].ClipMode = ENAxisClipMode.Auto;

            // configure scale
            NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            yScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
            yScale.MinTickDistance = 30;

            // add an interlaced strip to the Y axis
            NScaleStrip interlacedStrip = new NScaleStrip();
            interlacedStrip.Interlaced = true;
            interlacedStrip.Fill = new NColorFill(NColor.Beige);
            yScale.Strips.Add(interlacedStrip);

            OnChangeDataButtonClick(null);

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            m_ViewRangeMode = new NComboBox();
            m_ViewRangeMode.FillFromEnum<ENAxisViewRangeMode>();
            m_ViewRangeMode.SelectedIndexChanged += OnViewRangeModeSelectedIndexChanged;
            m_ViewRangeMode.SelectedIndex = (int)ENAxisViewRangeMode.Auto;
            stack.Add(NPairBox.Create("View Range Mode:", m_ViewRangeMode));

            m_ViewRangeMinValue = new NNumericUpDown();
            m_ViewRangeMinValue.ValueChanged += OnViewRangeMinValueChanged;
            m_ViewRangeMinValue.Value = 20;
            stack.Add(NPairBox.Create("View Range Min:", m_ViewRangeMinValue));

            m_ViewRangeMaxValue = new NNumericUpDown();
            m_ViewRangeMaxValue.ValueChanged += OnViewRangMaxValueChanged;
            m_ViewRangeMaxValue.Value = 80;
            stack.Add(NPairBox.Create("View Range Max:", m_ViewRangeMaxValue));

            NButton changeDataButton = new NButton("Change Data");
            changeDataButton.Click += new Function<NEventArgs>(OnChangeDataButtonClick);
            stack.Add(changeDataButton);

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to explicitly set the axis view range.</p>";
        }

        #endregion

        #region Event Handlers

        private void OnViewRangeMinValueChanged(NValueChangeEventArgs arg)
        {
            m_Chart.Axes[ENCartesianAxis.PrimaryY].MinViewRangeValue = (double)m_ViewRangeMinValue.Value;
        }

        private void OnViewRangMaxValueChanged(NValueChangeEventArgs arg)
        {
            m_Chart.Axes[ENCartesianAxis.PrimaryY].MaxViewRangeValue = (double)m_ViewRangeMaxValue.Value;
        }

        private void OnViewRangeModeSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENAxisViewRangeMode viewRangeMode = (ENAxisViewRangeMode)m_ViewRangeMode.SelectedIndex;
            m_Chart.Axes[ENCartesianAxis.PrimaryY].ViewRangeMode = viewRangeMode;

            switch (viewRangeMode)
            {
                case ENAxisViewRangeMode.Auto:
                    m_ViewRangeMinValue.Enabled = false;
                    m_ViewRangeMaxValue.Enabled = false;
                    break;
                case ENAxisViewRangeMode.FixedMin:
                    m_ViewRangeMinValue.Enabled = true;
                    m_ViewRangeMaxValue.Enabled = false;
                    break;
                case ENAxisViewRangeMode.FixedMax:
                    m_ViewRangeMinValue.Enabled = false;
                    m_ViewRangeMaxValue.Enabled = true;
                    break;
                case ENAxisViewRangeMode.FixedRange:
                    m_ViewRangeMinValue.Enabled = true;
                    m_ViewRangeMaxValue.Enabled = true;
                    break;
            }
        }

        private void OnChangeDataButtonClick(NEventArgs arg)
        {
            m_Chart.Series.Clear();

            // setup bar series
            NBarSeries bar = new NBarSeries();
            m_Chart.Series.Add(bar);

            bar.DataLabelStyle = new NDataLabelStyle(false);
            bar.Stroke = new NStroke(1.5f, NColor.DarkGreen);

            // fill in some data so that it contains several peaks of data
            Random random = new Random();

            for (int i = 0; i < 25; i++)
            {
                double value = random.NextDouble() * 100;

                NBarDataPoint dataPoint = new NBarDataPoint(value);
                bar.DataPoints.Add(dataPoint);
            }
        }

        #endregion

        #region Implementation

        #endregion

        #region Fields

        NCartesianChart m_Chart;

        NComboBox m_ViewRangeMode;
        NNumericUpDown m_ViewRangeMinValue;
        NNumericUpDown m_ViewRangeMaxValue;

        #endregion

        #region Schema

        public static readonly NSchema NAxisViewRangeExampleSchema;

        #endregion
    }
}