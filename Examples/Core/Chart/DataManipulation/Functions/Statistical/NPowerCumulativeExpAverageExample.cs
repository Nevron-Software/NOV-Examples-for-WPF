using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Power, Cumulative, ExpAverage functions
	/// </summary>
	public class NPowerCumulativeExpAverageExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NPowerCumulativeExpAverageExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NPowerCumulativeExpAverageExample()
        {
            NPowerCumulativeExpAverageExampleSchema = NSchema.Create(typeof(NPowerCumulativeExpAverageExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Basic Functions";

            // configure chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            // add the first bar
            m_Bar = new NBarSeries();
            chart.Series.Add(m_Bar);
            m_Bar.Name = "Green";
            m_Bar.ValueFormatter = new NNumericValueFormatter("0.00");
            m_Bar.MultiBarMode = ENMultiBarMode.Series;

            m_Bar.DataLabelStyle = new NDataLabelStyle();
            m_Bar.DataLabelStyle.Visible = false;

            m_Bar.LegendView = new NSeriesLegendView();
            m_Bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;

            m_Bar.Fill = new NColorFill(NColor.SeaGreen);
            m_Bar.Stroke = new NStroke(NColor.Black);
            m_Bar.Shape = ENBarShape.Rectangle;           

            // add a line series for the function
            m_LineSeries = new NLineSeries();
            chart.Series.Add(m_LineSeries);
            m_LineSeries.InflateMargins = false;
            m_LineSeries.Name = "Function";
            m_LineSeries.DataLabelStyle = new NDataLabelStyle();
            m_LineSeries.DataLabelStyle.Format = "<value>";

            m_LineSeries.MarkerStyle = new NMarkerStyle();
            m_LineSeries.MarkerStyle.Shape = ENPointShape3D.Ellipse;
            m_LineSeries.MarkerStyle.Visible = true;
            m_LineSeries.MarkerStyle.Border = new NStroke(1, NColor.Crimson);
            m_LineSeries.MarkerStyle.Size = new NSize(10, 10);

            m_LineSeries.LegendView.Mode = ENSeriesLegendMode.None;
            m_LineSeries.ValueFormatter = new NNumericValueFormatter("0.00");

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            // function combo
            m_FunctionComboBox = new NComboBox();
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Power"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Cumulative"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Exponential Average"));
            m_FunctionComboBox.SelectedIndexChanged += (e) =>
            {
                CalculateFunction();
                m_PowerNUD.Enabled = m_FunctionComboBox.SelectedIndex == 0;
                m_ExponentialWeightNUD.Enabled = m_FunctionComboBox.SelectedIndex == 2;
            };            
            stack.Add(NPairBox.Create("Function: ", m_FunctionComboBox));

            // power nud
            m_PowerNUD = new NNumericUpDown();
            m_PowerNUD.Minimum = -10;
            m_PowerNUD.Maximum = 10;
            m_PowerNUD.Value = 1.1;
            m_PowerNUD.Step = 0.01;
            m_PowerNUD.DecimalPlaces = 2;
            m_PowerNUD.ValueChanged += (e) =>
            {
                CalculateFunction();
            };
            stack.Add(NPairBox.Create("Power: ", m_PowerNUD));

            // exponential weight nud
            m_ExponentialWeightNUD = new NNumericUpDown();
            m_ExponentialWeightNUD.Minimum = 0;
            m_ExponentialWeightNUD.Maximum = 1;
            m_ExponentialWeightNUD.Value = 0.5;
            m_ExponentialWeightNUD.Step = 0.01;
            m_ExponentialWeightNUD.DecimalPlaces = 2;
            m_ExponentialWeightNUD.ValueChanged += (e) =>
            {
                CalculateFunction();
            };
            stack.Add(NPairBox.Create("Exponential Weight: ", m_ExponentialWeightNUD));

            // new positive data button
            NButton newPositiveDataButton = new NButton("New Positive Data");
            newPositiveDataButton.Click += (e) =>
            {
                // fill with random data
                RandomData();
            };
            stack.Add(newPositiveDataButton);


            // fill with random data
            RandomData();

            // select first function
            m_FunctionComboBox.SelectedIndex = 0;

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to create a standard bar chart.</p>";
        }

        #endregion

        #region Implementation

        void RandomData()
        {
            double[] randomData = CreateRandomData(10, 0, 100);

            m_Bar.DataPoints.Clear();
            for (int i = 0; i < randomData.Length; i++)
            {
                NBarDataPoint barDataPoint = new NBarDataPoint();
                barDataPoint.Value = randomData[i];
                m_Bar.DataPoints.Add(barDataPoint);
            }

            CalculateFunction();
        }
        void CalculateFunction()
        {
            string formula;
            switch (m_FunctionComboBox.SelectedIndex)
            {
                case 0: // power
                    formula = string.Format("POW(BarValues, {0})", m_PowerNUD.Value.ToString("0.00"));
                    break;

                case 1: // cumulative
                    formula = "CUMSUM(BarValues)";
                    break;

                case 2: // exponential average
                    formula = string.Format("EXPAVG(BarValues, {0})", m_ExponentialWeightNUD.Value.ToString("0.00"));
                    break;

                default:
                    return;
            }

            // create a chart formula calculator in which:
            // BarValues variable represents m_Bar Values 
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = formula;
            calculator.Variables["BarValues"] = m_Bar.DataPoints.GetDataSeriesVariant("Value");

            // calculate
            NVariant result = calculator.Evaluate();

            // display result arrays as line series
            m_LineSeries.DataPoints.SetDataSeriesVariant(result, "Value");
        }

        #endregion

        #region Fields

        NBarSeries m_Bar;
        NLineSeries m_LineSeries;
        NComboBox m_FunctionComboBox;
        NNumericUpDown m_PowerNUD;
        NNumericUpDown m_ExponentialWeightNUD;

        #endregion

        #region Schema

        public static readonly NSchema NPowerCumulativeExpAverageExampleSchema;

        #endregion
    }
}