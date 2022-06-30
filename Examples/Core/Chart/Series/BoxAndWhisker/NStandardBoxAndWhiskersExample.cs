using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Box and Whiskers Example
	/// </summary>
	public class NStandardBoxAndWhiskersExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NStandardBoxAndWhiskersExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NStandardBoxAndWhiskersExample()
        {
            NStandardBoxAndWhiskersExampleSchema = NSchema.Create(typeof(NStandardBoxAndWhiskersExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleContent()
        {
            NChartView chartView = new NChartView();
            chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Standard Box and Whiskers";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            // add interlace stripe
            NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            m_BoxAndWhiskerSeries = new NBoxAndWhiskerSeries();
            m_BoxAndWhiskerSeries.WidthMode = ENBarWidthMode.FixedWidth;
            m_Chart.Series.Add(m_BoxAndWhiskerSeries);

            m_BoxAndWhiskerSeries.Fill = new NStockGradientFill(ENGradientStyle.Vertical, ENGradientVariant.Variant4, NColor.LightYellow, NColor.DarkOrange);
            m_BoxAndWhiskerSeries.DataLabelStyle = new NDataLabelStyle(false);
            m_BoxAndWhiskerSeries.MedianStroke = new NStroke(NColor.Indigo);
            m_BoxAndWhiskerSeries.AverageStroke = new NStroke(1, NColor.DarkRed, ENDashStyle.Dot);
            m_BoxAndWhiskerSeries.OutlierStroke = new NStroke(NColor.DarkCyan);
            m_BoxAndWhiskerSeries.OutlierFill = new NColorFill(NColor.Red);

            GenerateData(m_BoxAndWhiskerSeries, 7);

            return chartView;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NNumericUpDown boxWidthUpDown = new NNumericUpDown();
            boxWidthUpDown.Value = m_BoxAndWhiskerSeries.Width;
            boxWidthUpDown.ValueChanged += OnBoxWidthUpDownValueChanged;
            stack.Add(NPairBox.Create("Box Width:", boxWidthUpDown));

            NNumericUpDown whiskerWidthPercentUpDown = new NNumericUpDown();
            whiskerWidthPercentUpDown.Value = m_BoxAndWhiskerSeries.WhiskerWidthPercent;
            whiskerWidthPercentUpDown.ValueChanged += OnWhiskerWidthPercentUpDownValueChanged;
            stack.Add(NPairBox.Create("Whisker Width %:", whiskerWidthPercentUpDown));

            NCheckBox inflateMarginsCheckBox = new NCheckBox("Inflate Margins");
            inflateMarginsCheckBox.CheckedChanged += OnInflateMarginsCheckBoxCheckedChanged;
            inflateMarginsCheckBox.Checked = true;
            stack.Add(inflateMarginsCheckBox);

            NCheckBox yAxisRoundToTickCheckBox = new NCheckBox("Y Axis Round To Tick");
            yAxisRoundToTickCheckBox.CheckedChanged += OnYAxisRoundToTickCheckBoxCheckedChanged;
            yAxisRoundToTickCheckBox.Checked = true;
            stack.Add(yAxisRoundToTickCheckBox);

            NButton generateDataButton = new NButton("Generate Data");
            generateDataButton.Click += OnGenerateDataButtonClick;
            stack.Add(generateDataButton);

            return boxGroup;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to create a standard box and whisker chart.</p>";
        }

        #endregion

        #region Event Handlers

        void OnBoxWidthUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_BoxAndWhiskerSeries.Width = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnGenerateDataButtonClick(NEventArgs arg)
        {
            GenerateData(m_BoxAndWhiskerSeries, 7);
        }

        void OnWhiskerWidthPercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_BoxAndWhiskerSeries.WhiskerWidthPercent = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_BoxAndWhiskerSeries.InflateMargins = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnYAxisRoundToTickCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

            if (((NCheckBox)arg.TargetNode).Checked)
            {
                yScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
                yScale.InflateViewRangeBegin = true;
                yScale.InflateViewRangeEnd = true;
            }
            else
            {
                yScale.InflateViewRangeBegin = false;
                yScale.InflateViewRangeEnd = false;
            }
        }

        #endregion

        #region Implementation

        private void GenerateData(NBoxAndWhiskerSeries series, int nCount)
        {
            series.DataPoints.Clear();
            Random random = new Random();

            for (int i = 0; i < nCount; i++)
            {
                double boxLower = 1000 + random.NextDouble() * 200;
                double boxUpper = boxLower + 200 + random.NextDouble() * 200;
                double whiskersLower = boxLower - (20 + random.NextDouble() * 300);
                double whiskersUpper = boxUpper + (20 + random.NextDouble() * 300);

                double IQR = (boxUpper - boxLower);
                double median = boxLower + IQR * 0.25 + random.NextDouble() * IQR * 0.5;
                double average = boxLower + IQR * 0.25 + random.NextDouble() * IQR * 0.5;

                int outliersCount = random.Next(5);

                double[] outliers = new double[outliersCount];

                for (int k = 0; k < outliersCount; k++)
                {
                    double outlier = 0;

                    if (random.NextDouble() > 0.5)
                    {
                        outlier = boxUpper + IQR * 1.5 + random.NextDouble() * 100;
                    }
                    else
                    {
                        outlier = boxLower - IQR * 1.5 - random.NextDouble() * 100;
                    }

                    outliers[k] = outlier;
                }

                series.DataPoints.Add(new NBoxAndWhiskerDataPoint(i, boxUpper, boxLower, median, average, whiskersUpper, whiskersLower, outliers, string.Empty));
            }
        }

        #endregion

        #region Fields

        NCartesianChart m_Chart;
        NBoxAndWhiskerSeries m_BoxAndWhiskerSeries;

        #endregion

        #region Schema

        public static readonly NSchema NStandardBoxAndWhiskersExampleSchema;

        #endregion
    }
}
