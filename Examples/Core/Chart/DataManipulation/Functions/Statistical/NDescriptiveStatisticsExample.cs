using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Descriptive Statistics Functions (Average, Standard Deviation, Median)
	/// </summary>
	public class NDescriptiveStatisticsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NDescriptiveStatisticsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NDescriptiveStatisticsExample()
        {
            NDescriptiveStatisticsExampleSchema = NSchema.Create(typeof(NDescriptiveStatisticsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Descriptive Statistics";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            // configure legend
            m_Chart.Legend.Mode = ENLegendMode.Custom;

            // add interlace stripe
            NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            // setup a bar series
            m_LineSeries = new NLineSeries();
            m_LineSeries.Name = "Line Series";
            m_LineSeries.InflateMargins = false;
            m_LineSeries.UseXValues = false;
            m_LineSeries.Stroke = new NStroke(3, LineColor);

            // add some data to the bar series
            m_Chart.Series.Add(m_LineSeries);

            // create median const line
            m_MedianConstLine = new NAxisReferenceLine();
            m_MedianConstLine.Stroke = new NStroke(2, MedianColor);
            m_Chart.Axes[ENCartesianAxis.PrimaryY].ReferenceLines.Add(m_MedianConstLine);

            // create mean const line
            m_MeanConstLine = new NAxisReferenceLine();
            m_MeanConstLine.Stroke = new NStroke(2, MeanColor);
            m_Chart.Axes[ENCartesianAxis.PrimaryY].ReferenceLines.Add(m_MeanConstLine);

            // create standard deviation stripe
            m_StandardDeviationStripe = new NAxisStripe();
            m_StandardDeviationStripe.Fill = new NColorFill(StdDevColor);
            m_Chart.Axes[ENCartesianAxis.PrimaryY].Stripes.Add(m_StandardDeviationStripe);

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            // new positive data button
            NButton newPositiveDataButton = new NButton("New Positive Data");
            newPositiveDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData(false);
            };
            stack.Add(newPositiveDataButton);

            // new positive & negative data button
            NButton newPositiveAndNegativeDataButton = new NButton("New Positive & Negative Data");
            newPositiveAndNegativeDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData(true);
            };
            stack.Add(newPositiveAndNegativeDataButton);

            // fill with random data
            RandomData(true);

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>Descriptive statistics is a set of brief descriptive coefficients that summarize a given data set representative of an entire or sample population.</p>
<p>In this example we demonstrate how to calculate the Mean, Median and Standard Deviation of a set of values, represented by the Line Series.</p>";
        }

        #endregion

        #region Implementation

        private void RandomData(bool positiveAndNegative)
        {
            double[] randomData;
            if (positiveAndNegative)
            {
                randomData = CreateRandomData(20, -100, 100);
            }
            else
            {
                randomData = CreateRandomData(20, 0, 100);
            }

            m_LineSeries.DataPoints.Clear();
            for (int i = 0; i < randomData.Length; i++)
            {
                m_LineSeries.DataPoints.Add(new NLineDataPoint(randomData[i]));
            }

            CalculateStatistics();
        }
        void CalculateStatistics()
        {
            // create a chart formula calculator in which:
            // LineValues variable represents m_LineSeries Values 
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Variables["LineValues"] = m_LineSeries.DataPoints.GetDataSeriesVariant("Value");

            // calculate the mean (average)
            calculator.Formula = "AVG(LineValues)";
            double mean = (double)calculator.Evaluate();
            m_MeanConstLine.Value = mean;

            // calculate median
            calculator.Formula = "MEDIAN(LineValues)";
            double median = (double)calculator.Evaluate();
            m_MedianConstLine.Value = median;

            // calculate standard deviation
            calculator.Formula = "STDDEV(LineValues, false)";
            double stdDev = (double)calculator.Evaluate();
            m_StandardDeviationStripe.Range = new NRange(mean - stdDev, mean + stdDev);

            // update legend
            m_Chart.Legend.Items.Clear();

            CreateLegendItem(ENLegendMarkShape.Rectangle, "Mean: " + mean.ToString("G2"), MeanColor);
            CreateLegendItem(ENLegendMarkShape.Rectangle, "Median: " + median.ToString("G2"), MedianColor);
            CreateLegendItem(ENLegendMarkShape.Rectangle, "Standard Deviation: " + stdDev.ToString("G2"), StdDevColor);
        }

        private void CreateLegendItem(ENLegendMarkShape shape, string text, NColor color)
        {
            NWidget symbol;
            if (shape == ENLegendMarkShape.Line)
            {
                symbol = NLegend.CreateLegendSymbol(
                    shape,
                    new NSize(20, 20),
                    new NMargins(2),
                    null,
                    new NStroke(NColor.Black),
                    new NStroke(color));
            }
            else
            {
                symbol = NLegend.CreateLegendSymbol(
                    shape,
                    new NSize(20, 20),
                    new NMargins(2),
                    new NColorFill(color),
                    new NStroke(NColor.Black),
                    new NStroke(NColor.Black));
            }
            

            NLabel label = new NLabel(text);
            label.Margins = new NMargins(2);
            m_Chart.Legend.Items.Add(new NPairBox(symbol, label));
        }

        private NColor MeanColor
        {
            get
            {
                return NChartPalette.FreshPalette[0];
            }
        }

        private NColor MedianColor
        {
            get
            {
                return NChartPalette.FreshPalette[1];
            }
        }

        private NColor StdDevColor
        {
            get
            {
                return new NColor(NChartPalette.FreshPalette[2], 128);
            }
        }

        private NColor LineColor
        {
            get
            {
                return NChartPalette.FreshPalette[3];
            }
        }

        #endregion

        #region Fields

        NLineSeries m_LineSeries;
        NAxisReferenceLine m_MedianConstLine;
        NAxisReferenceLine m_MeanConstLine;
        NAxisStripe m_StandardDeviationStripe;
        NCartesianChart m_Chart;

        #endregion

        #region Schema

        public static readonly NSchema NDescriptiveStatisticsExampleSchema;

        #endregion
    }
}