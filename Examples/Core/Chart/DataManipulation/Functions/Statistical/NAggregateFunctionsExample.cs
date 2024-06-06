using System.Diagnostics;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Aggregate Functions (Average, Median, Min, Max, Sum, StdDev, Variance)
	/// </summary>
	public class NAggregateFunctionsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NAggregateFunctionsExample()
        {
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static NAggregateFunctionsExample()
        {
            NAggregateFunctionsExampleSchema = NSchema.Create(typeof(NAggregateFunctionsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Aggregate Functions";

            // configure chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            // add interlace stripe
            NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            // setup a bar series
            m_Bar = new NBarSeries();
            m_Bar.Name = "Bar Series";
            m_Bar.InflateMargins = true;
            m_Bar.UseXValues = false;
            m_Bar.Shadow = new NShadow(NColor.LightGray, 2, 2);

            // add some data to the bar series
            m_Bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            m_Bar.DataPoints.Add(new NBarDataPoint(18, "C++"));
            m_Bar.DataPoints.Add(new NBarDataPoint(15, "Ruby"));
            m_Bar.DataPoints.Add(new NBarDataPoint(21, "Python"));
            m_Bar.DataPoints.Add(new NBarDataPoint(23, "Java"));
            m_Bar.DataPoints.Add(new NBarDataPoint(27, "Javascript"));
            m_Bar.DataPoints.Add(new NBarDataPoint(29, "C#"));
            m_Bar.DataPoints.Add(new NBarDataPoint(26, "PHP"));
            chart.Series.Add(m_Bar);

            // create const line to show the result of the function
            m_ConstLine = new NAxisReferenceLine();
            m_ConstLine.Stroke = new NStroke(1, NColor.Red);
            m_ConstLine.PaintAfterChartContent = true;
            m_ConstLine.IncludeValueInAxisRange = true;
            chart.Axes[ENCartesianAxis.PrimaryY].ReferenceLines.Add(m_ConstLine);

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            // function combo
            m_FunctionComboBox = new NComboBox();
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Average"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Median"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Min"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Max"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Sum"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Standard Deviation"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Variance"));

            m_FunctionComboBox.SelectedIndex = 0;
            m_FunctionComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFunctionComboBoxSelectedIndexChanged);
            stack.Add(NPairBox.Create("Function: ", m_FunctionComboBox));

            // new positive data button
            NButton newPositiveDataButton = new NButton("New Positive Data");
            newPositiveDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData(false);

                // calculate function
                CalculateFunction();
            };
            stack.Add(newPositiveDataButton);

            // new positive & negative data button
            NButton newPositiveAndNegativeDataButton = new NButton("New Positive & Negative Data");
            newPositiveAndNegativeDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData(true);

                // calculate function
                CalculateFunction();
            };
            stack.Add(newPositiveAndNegativeDataButton);

            CalculateFunction();

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>This example demonstrates the most commonly used aggregate functions (SUM, MIN, MAX etc.).</p>
<p>Aggregate functions take as input an array of values, and produce a single resulting value.</p>
<p>In this example the Green bar series represent the values, and the Red constant line represents the aggregate value for the currently selected function</p>";
        }

        #endregion

        #region Event Handlers

        void OnFunctionComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            CalculateFunction();
        }

        #endregion

        #region Implementation

        private void RandomData(bool positiveAndNegative)
        {
            double[] randomData;
            if (positiveAndNegative)
            {
                randomData = CreateRandomData(m_Bar.DataPoints.Count, -100, 100);
            }
            else 
            {
                randomData = CreateRandomData(m_Bar.DataPoints.Count, 0, 100);
            }

            for (int i = 0; i < m_Bar.DataPoints.Count; i++)
            {
                m_Bar.DataPoints[i].Value = randomData[i];
            }
        }
        void CalculateFunction()
        {
            string formula;
            switch (m_FunctionComboBox.SelectedIndex)
            {
                case 0: // average
                    formula = "AVG(BarValues)";
                    break;

                case 1: // median
                    formula = "MEDIAN(BarValues)";
                    break;

                case 2: // min
                    formula = "MIN(BarValues)";
                    break;

                case 3: // max
                    formula = "MAX(BarValues)";
                    break;

                case 4: // sum
                    formula = "SUM(BarValues)";
                    break;

                case 5: // Standard Deviation
                    formula = "STDDEV(BarValues, true)";
                    break;

                case 6: // Variance
                    formula = "VARIANCE(BarValues, true)";
                    break;

                default:
                    return;
            }

            // create a chart formula calculator in which:
            // BarValues variable represents m_Bar Values 
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = formula;
            calculator.Variables["BarValues"] = m_Bar.DataPoints.GetDataSeriesVariant("Value");

            // calculate the formula. Note that the result of an aggregate function is always a single value variant
            NVariant result = calculator.Evaluate();
            Debug.Assert(!result.IsArray);

            // display single values as a const line
            m_ConstLine.Value = result.ToDouble(null);
        }

        #endregion

        #region Fields

        NBarSeries m_Bar;
        NAxisReferenceLine m_ConstLine;
        NComboBox m_FunctionComboBox;

        #endregion

        #region Schema

        public static readonly NSchema NAggregateFunctionsExampleSchema;

        #endregion
    }
}