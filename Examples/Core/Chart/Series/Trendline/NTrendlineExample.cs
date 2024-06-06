using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Trendline example
    /// </summary>
    public class NTrendlineExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NTrendlineExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NTrendlineExample()
        {
            NCovarianceAndCorrelationExampleSchema = NSchema.Create(typeof(NTrendlineExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
            NChartView chartView = chartViewWithCommandBars.View;
            chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Trendline Series";
            chartView.Surface.Legends[0].Visibility = ENVisibility.Collapsed;

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // switch X axis in linear scale
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();

            // add interlace stripe
            NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            // setup line series to show trenline
            m_Trendline = new NTrendlineSeries();
            m_Trendline.Name = "Trendline Series";
            m_Trendline.Stroke = new NStroke(3, LineColor);
            m_Chart.Series.Add(m_Trendline);

            m_Trendline.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;

            // setup point series to show XY scatter
            m_PointSeries = new NPointSeries();
            m_PointSeries.Name = "Point Series";
            m_PointSeries.UseXValues = true;
            m_PointSeries.Fill = new NColorFill(BarColor);
            m_Chart.Series.Add(m_PointSeries);

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NNumericUpDown movingAveragePeriodNumericUpDown = new NNumericUpDown();
            NNumericUpDown polynomialOrderNumericUpDown = new NNumericUpDown();
            m_PolynomialOrderNumericUpDown = polynomialOrderNumericUpDown;
            m_MovingAveragePeriodNumericUpDown = movingAveragePeriodNumericUpDown;

            NComboBox trendlineTypeComboBox = new NComboBox();
            trendlineTypeComboBox.FillFromEnum<ENTrendlineType>();
            trendlineTypeComboBox.SelectedIndexChanged += OnTrendlineTypeComboBoxSelectedIndexChanged;
            trendlineTypeComboBox.SelectedIndex = (int)m_Trendline.TrendlineType;
            stack.Add(NPairBox.Create("Trendline Type:", trendlineTypeComboBox));
            
            movingAveragePeriodNumericUpDown.Minimum = 1;
            movingAveragePeriodNumericUpDown.Maximum = 30;
            movingAveragePeriodNumericUpDown.Value = m_Trendline.MovingAveragePeriod;
            movingAveragePeriodNumericUpDown.ValueChanged += OnMovingAveragePeriodNumericUpDownValueChanged;
            stack.Add(NPairBox.Create("Moving Average Period:", movingAveragePeriodNumericUpDown));
            
            polynomialOrderNumericUpDown.Minimum = 1;
            polynomialOrderNumericUpDown.Maximum = 100;
            polynomialOrderNumericUpDown.Value = m_Trendline.PolynomialOrder;
            polynomialOrderNumericUpDown.ValueChanged += OnPolynomialOrderNumericUpDownValueChanged;
            stack.Add(NPairBox.Create("Polynomial Order:", polynomialOrderNumericUpDown));

            // new positive data button
            NButton newDataButton = new NButton("New Data");
            newDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData();
            };
            stack.Add(newDataButton);

            // fill with random data
            RandomData();

            return boxGroup;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>Curve fitting is the process of constructing a curve, or mathematical function, that has the best fit to a series of data points. In this example, the trendline series is used to 
display different fittings like linear, exponential, logarithmic, etc. of a source data points set.</p>";
        }

        #endregion

        #region Events

        private void OnTrendlineTypeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Trendline.TrendlineType = (ENTrendlineType)arg.NewValue;

            m_PolynomialOrderNumericUpDown.Enabled = false;
            m_MovingAveragePeriodNumericUpDown.Enabled = false;

            switch (m_Trendline.TrendlineType)
            {
                case ENTrendlineType.Polynomial:
                    m_PolynomialOrderNumericUpDown.Enabled = true;
                    break;
                case ENTrendlineType.MovingAverage:
                    m_MovingAveragePeriodNumericUpDown.Enabled = true;
                    break;
            }
        }


        private void OnPolynomialOrderNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Trendline.PolynomialOrder = (int)(double)arg.NewValue;
        }

        private void OnMovingAveragePeriodNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Trendline.MovingAveragePeriod = (int)(double)arg.NewValue;
        }

        #endregion

        #region Implementation

        private void RandomData()
        {
            Random rand = new Random();

            int count = 100;
            double[] randomDataX =  new double[count];
            double[] randomDataY = new double[count];

            double x;
            for (int i = 0; i < count; i++)
            {
                x = i + rand.NextDouble();

                randomDataX[i] = x * 10;
                randomDataY[i] = x * 5.0 * rand.NextDouble() + rand.NextDouble();
            }

            m_PointSeries.DataPoints.Clear();
            for (int i = 0; i < randomDataX.Length; i++)
            {
                m_PointSeries.DataPoints.Add(new NPointDataPoint(randomDataX[i], randomDataY[i]));
            }

            CalculateStatistics();
        }
        void CalculateStatistics()
        {

            // show trendline
            m_Trendline.SourceSeries = m_PointSeries;
        }

        private NColor BarColor
        {
            get
            {
                return NChartPalette.FreshPalette[0];
            }
        }

        private NColor LineColor
        {
            get
            {
                return NChartPalette.FreshPalette[1];
            }
        }

        #endregion

        #region Fields

        NPointSeries m_PointSeries;
        NCartesianChart m_Chart;
        NTrendlineSeries m_Trendline;
        NNumericUpDown m_PolynomialOrderNumericUpDown;
        NNumericUpDown m_MovingAveragePeriodNumericUpDown;

        #endregion

        #region Schema

        public static readonly NSchema NCovarianceAndCorrelationExampleSchema;

        #endregion
    }
}