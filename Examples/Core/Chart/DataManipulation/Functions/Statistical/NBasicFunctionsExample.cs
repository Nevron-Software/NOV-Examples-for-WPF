using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Basic Statistical Functions (+, -, /, *, HIGH, LOW)
    /// </summary>
    public class NBasicFunctionsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NBasicFunctionsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NBasicFunctionsExample()
        {
            NBasicFunctionsExampleSchema = NSchema.Create(typeof(NBasicFunctionsExample), NExampleBaseSchema);
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
            m_GreenBars = new NBarSeries();
            chart.Series.Add(m_GreenBars);
            m_GreenBars.Name = "Green";
            m_GreenBars.ValueFormatter = new NNumericValueFormatter("0.00");
            m_GreenBars.MultiBarMode = ENMultiBarMode.Series;

            m_GreenBars.DataLabelStyle = new NDataLabelStyle();
            m_GreenBars.DataLabelStyle.Visible = false;

            m_GreenBars.LegendView = new NSeriesLegendView();
            m_GreenBars.LegendView.Mode = ENSeriesLegendMode.DataPoints;

            m_GreenBars.Fill = new NColorFill(NColor.SeaGreen);
            m_GreenBars.Stroke = new NStroke(NColor.Black);
            m_GreenBars.Shape = ENBarShape.Rectangle;

            // add the second bar
            m_BlueBars = new NBarSeries();
            chart.Series.Add(m_BlueBars);
            m_BlueBars.Name = "Blue";
            m_BlueBars.ValueFormatter = new NNumericValueFormatter("0.00");
            m_BlueBars.MultiBarMode = ENMultiBarMode.Clustered;

            m_GreenBars.DataLabelStyle = new NDataLabelStyle();
            m_GreenBars.DataLabelStyle.Visible = false;

            m_BlueBars.LegendView = new NSeriesLegendView();
            m_BlueBars.LegendView.Mode = ENSeriesLegendMode.DataPoints;

            m_BlueBars.Fill  = new NColorFill(NColor.CornflowerBlue);
            m_BlueBars.Stroke = new NStroke(NColor.Black);
            m_BlueBars.Shape = ENBarShape.Rectangle;

            // add a line series for the function
            m_LineSeries = new NLineSeries();
            chart.Series.Add(m_LineSeries);
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

            // fill with random data
            FillDataSeries(m_GreenBars , 5);
            FillDataSeries(m_BlueBars, 5);
            
            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            // function combo
            m_FunctionComboBox = new NComboBox();
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Green + Blue"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Green - Blue"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Green * Blue"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("Green / Blue"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("HIGH(Green, Blue)"));
            m_FunctionComboBox.Items.Add(new NComboBoxItem("LOW(Green, Blue)"));
            m_FunctionComboBox.SelectedIndex = 0;
            m_FunctionComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnFunctionComboBoxSelectedIndexChanged);
            stack.Add(NPairBox.Create("Function: ", m_FunctionComboBox));

            // new data button
            NButton newDataButton = new NButton("New Data");
            newDataButton.Click += OnNewDataButtonClick;
            stack.Add(newDataButton);

            CalculateFunction();

            return boxGroup;
        }
        

        protected override string GetExampleDescription()
        {
            return @"
<p>This example demonstrates the basic arithmethic functions (+, -, *, /)  and the HIGH and LOW functions between data series.</p>
<p>The Green and Blue bar series serve as input parameters for the currently selected function, while the Red line series is used to display the result.</p>";
        }

        #endregion

        #region Event Handlers

        private void OnNewDataButtonClick(NEventArgs arg)
        {
            // fill with random data
            FillDataSeries(m_GreenBars, 5);
            FillDataSeries(m_BlueBars, 5);

            // calculate function
            CalculateFunction();
        }
        void OnFunctionComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            CalculateFunction();
        }

        #endregion

        #region Implementation

        void CalculateFunction()
        {
            // determine the formula to use
            string formula;
            switch (m_FunctionComboBox.SelectedIndex)
            {
                case 0: // add
                    formula = "Green + Blue";
                    break;

                case 1: // sub
                    formula = "Green - Blue";
                    break;

                case 2: // mul
                    formula = "Green * Blue";
                    break;

                case 3: // div
                    formula = "Green / Blue";
                    break;

                case 4: // HIGH
                    formula = "HIGH(Green, Blue)";
                    break;

                case 5: // LOW
                    formula = "LOW(Green, Blue)";
                    break;

                default:
                    return;
            }

            // create a chart formula calculator in which:
            // Green variable represents m_GreenBars Values  and 
            // Blue variable represents m_BlueBars Values
            NFormulaCalculator calculator = new NFormulaCalculator();
            calculator.Formula = formula;
            calculator.Variables["Green"] = m_GreenBars.DataPoints.GetDataSeriesVariant("Value");
            calculator.Variables["Blue"] = m_BlueBars.DataPoints.GetDataSeriesVariant("Value");

            // calculate
            NVariant result = calculator.Evaluate();

            // update result line series data points
            m_LineSeries.DataPoints.SetDataSeriesVariant(result, "Value");

            NLineSeries lineSeries = new NLineSeries();
            lineSeries.DataPoints.Add(new NLineDataPoint() {
                Value = 10,
                X = 50
            });
            lineSeries.DataPoints.Add(new NLineDataPoint() {
                Value = 20,
                X = 70
            });
            lineSeries.DataPoints.Add(new NLineDataPoint() {
                Value = 30,
                X = 90
            });
        }

        private void FillDataSeries(NBarSeries ds, int count)
        {
            double[] randomData = CreateRandomData(count, -100, 100);

            ds.DataPoints.Clear();
            for (int i = 0; i < randomData.Length; i++)
            {
                NBarDataPoint dp = new NBarDataPoint();
                dp.Value = randomData[i];
                ds.DataPoints.Add(dp);
            }
        }

        #endregion

        #region Fields

        NBarSeries m_GreenBars;
        NBarSeries m_BlueBars;
        NLineSeries m_LineSeries;
        NComboBox m_FunctionComboBox;

        #endregion

        #region Schema

        public static readonly NSchema NBasicFunctionsExampleSchema;

        #endregion
    }
}