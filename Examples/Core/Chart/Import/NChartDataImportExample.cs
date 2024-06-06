using System;
using System.Collections;

using Nevron.Nov.Chart;
using Nevron.Nov.Data;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Formulas;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Data Import Example.
	/// </summary>
	public class NChartDataImportExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NChartDataImportExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NChartDataImportExample()
        {
            NChartDataImportExampleSchema = NSchema.Create(typeof(NChartDataImportExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            m_ChartView.Surface.Titles[0].Text = "Chart Data Import";

            // use bright theme
            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

            // import single data series
            OnImportSingleDataSeriesClick(null);

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NButton importSingleDataSeries = new NButton("Import Single Data Series");
            importSingleDataSeries.Click += OnImportSingleDataSeriesClick;
            stack.Add(importSingleDataSeries);

            NButton importMultipleDataSeriesFromEnumrables = new NButton("Import Multiple Data Series\r\n(from IEnumerables)");
            importMultipleDataSeriesFromEnumrables.Click += OnImportMultipleDataSeriesFromEnumerablesClick;
            stack.Add(importMultipleDataSeriesFromEnumrables);

            NButton importMultipleDataSeriesFromTable = new NButton("Import Multiple Data Series\r\n(from NDataTable)");
            importMultipleDataSeriesFromTable.Click += OnImportMultipleDataSeriesFromTableClick;
            stack.Add(importMultipleDataSeriesFromTable);

            if (NDbConnection.IsTypeSupported(ENDbConnectionType.Odbc))
            {
                NButton importMultipleDataSeriesFromDBReader = new NButton("Import Multiple Data Series\r\n(from NDbReader)");
                importMultipleDataSeriesFromDBReader.Click += OnImportMultipleDataSeriesFromDBReaderClick;
                stack.Add(importMultipleDataSeriesFromDBReader);
            }

            return group;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how import data from various data sources.</p>";
        }

        #endregion

        #region Implementation

        private void OnImportMultipleDataSeriesFromEnumerablesClick(NEventArgs arg)
        {
            // clear chart
            ClearChart();

            // get chart
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            // switch X axis in linear scale
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();

            // create a point series to show the imported values
            NPointSeries pointSeries = new NPointSeries();
            chart.Series.Add(pointSeries);
            pointSeries.ValueFormatter = new NNumericValueFormatter("0.00");
            pointSeries.DataLabelStyle = new NDataLabelStyle();
            pointSeries.DataLabelStyle.Visible = true;
            pointSeries.DataLabelStyle.Format = "<label>";
            pointSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Bottom;
            pointSeries.DataLabelStyle.ArrowLength = 30;

            // create random data series
            const int dataPointCount = 10;

            double[] values = CreateRandomData(dataPointCount);
            double[] xvalues = CreateRandomData(dataPointCount);
            string[] labels = new string[values.Length];
            for (int i = 0; i < dataPointCount; i++)
            {
                labels[i] = "Label: " + (i + 1).ToString();
            }

            // import multiple data series
            pointSeries.DataPoints.SetDataSeries(new NKeyValuePair<IEnumerable, string>[] {
                new NKeyValuePair<IEnumerable, string>(values, "Value"),
                new NKeyValuePair<IEnumerable, string>(xvalues, "X"),
                new NKeyValuePair<IEnumerable, string>(labels, "Label")
            });
        }

        private void OnImportMultipleDataSeriesFromTableClick(NEventArgs arg)
        {
            // clear chart
            ClearChart();

            // get chart
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            // switch X axis in linear scale
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();

            // create a line series to show the imported values
            NLineSeries lineSeries = new NLineSeries();
            chart.Series.Add(lineSeries);
            lineSeries.ValueFormatter = new NNumericValueFormatter("0.00");
            lineSeries.DataLabelStyle = new NDataLabelStyle();
            lineSeries.DataLabelStyle.Visible = true;
            lineSeries.DataLabelStyle.Format = "<label>";
            lineSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Bottom;
            lineSeries.DataLabelStyle.ArrowLength = 30;

            // create a table with random data
            NMemoryDataTable table = new NMemoryDataTable(
                new NFieldInfo("Y", typeof(double)),
                new NFieldInfo("X", typeof(double)),
                new NFieldInfo("Label", typeof(string))
            );

            const int dataPointCount = 10;
            Random rnd = new Random();
            for (int i = 0; i < dataPointCount; i++)
            {
                table.AddRow();
                table.SetValue(i, "Y", rnd.Next(0, 100));
                table.SetValue(i, "X", rnd.Next(0, 100));
                table.SetValue(i, "Label", "Label: " + i.ToString());
            }

            // import multiple data series
            lineSeries.DataPoints.SetDataSeries(table,
                    new NKeyValuePair<string, string>[] {
                        new NKeyValuePair<string, string>("Y", "Value"),
                        new NKeyValuePair<string, string>("X", "X"),
                        new NKeyValuePair<string, string>("Label", "Label")
            });
        }

        private void OnImportMultipleDataSeriesFromDBReaderClick(NEventArgs arg)
        {
            NTopLevelWindow topLevelWindow = NApplication.CreateTopLevelWindow();
            topLevelWindow.Title = "Select DB Source";

            NStackPanel stack = new NStackPanel ();
            
            // connection string
            NTextBox connectionString = new NTextBox ();
            connectionString.Text = "DSN=Xtreme";
            stack.Add(new NPairBox("Connection String:", connectionString));

            // connection string
            NTextBox sqlQuery = new NTextBox();
            sqlQuery.Text = "SELECT * FROM SALES";
            stack.Add(new NPairBox("SQL Query:", sqlQuery));

            // value column
            NTextBox valueColumn = new NTextBox();
            valueColumn.Text = "<value_field>";
            stack.Add(new NPairBox("Value Column:", valueColumn));

            // label column
            NTextBox labelColumn = new NTextBox();
            labelColumn.Text = "<label_field>";
            stack.Add(new NPairBox("Label Column:", labelColumn));

            NButtonStrip buttonStrip = new NButtonStrip ();
            buttonStrip.AddOKCancelButtons();
            stack.Add(buttonStrip);

            topLevelWindow.Content = stack;
            topLevelWindow.Open();

            topLevelWindow.Closed += (e) =>
            {
                // clear chart
                ClearChart();

                // get chart
                NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

                // switch X axis in linear scale
                chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NOrdinalScale();

                // create a point series to show the imported values
                NPointSeries pointSeries = new NPointSeries();
                chart.Series.Add(pointSeries);
                pointSeries.ValueFormatter = new NNumericValueFormatter("0.00");
                pointSeries.DataLabelStyle = new NDataLabelStyle();
                pointSeries.DataLabelStyle.Visible = true;
                pointSeries.DataLabelStyle.Format = "<label>";
                pointSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Bottom;
                pointSeries.DataLabelStyle.ArrowLength = 30;

                // created random data series
                NDbConnection connection = null;
                try
                {
                    // try open the connection
                    connection = NDbConnection.Open(ENDbConnectionType.Odbc, connectionString.Text);
                    NDbDataReader dataReader = connection.ExecuteReader("SELECT * FROM SALES");

                    // TODO - execute queries and commands
                    // import multiple data series
                    NList<NKeyValuePair<string, string>> dataMappings = new NList<NKeyValuePair<string, string>>();
                    if (!string.IsNullOrEmpty(valueColumn.Text))
                    {
                        dataMappings.Add(new NKeyValuePair<string, string>(valueColumn.Text, "Value"));
                    }
                    if (!string.IsNullOrEmpty(labelColumn.Text))
                    {
                        dataMappings.Add(new NKeyValuePair<string, string>(labelColumn.Text, "Label"));
                    }

                    pointSeries.DataPoints.SetDataSeries(dataReader, dataMappings.ToArray());
                }
                catch (Exception ex)
                {
                    // TODO: report connection or execution error
                    NMessageBox.Show(ex.Message, "Data Import Error");
                }
                finally
                {
                    // close the connection
                    if (connection != null)
                    {
                        connection.Close();
                    }
                }
            };
        }
       
        private void OnImportSingleDataSeriesClick(NEventArgs arg)
        {
            // clear chart
            ClearChart();

            // get chart
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            // switch X axis in ordinal scale
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NOrdinalScale();

            // create a bar series to show the imported values
            NBarSeries barSeries = new NBarSeries();
            chart.Series.Add(barSeries);
            barSeries.ValueFormatter = new NNumericValueFormatter("0.00");

            // import randomly created double[] 
            double[] values = CreateRandomData(10);
            barSeries.DataPoints.SetDataSeries(values, "Value");

            
        }

        private void ClearChart()
        {
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
            chart.Series.Clear();
        }

        #endregion

        #region Fields

        private NChartView m_ChartView;

        #endregion

        #region Schema

        public static readonly NSchema NChartDataImportExampleSchema;

        #endregion
    }
}