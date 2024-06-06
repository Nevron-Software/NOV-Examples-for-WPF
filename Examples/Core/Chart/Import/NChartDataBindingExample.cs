using Nevron.Nov.Chart;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Grid;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Chart Data Binding Example
    /// </summary>
    public class NChartDataBindingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NChartDataBindingExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NChartDataBindingExample()
        {
            NChartDataBindingExampleSchema = NSchema.Create(typeof(NChartDataBindingExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            // create a dummy data source, which to share between the grid and the chart
            m_MemoryDataTable = new NMemoryDataTable(
                new NFieldInfo("Value", typeof(double), false),
                new NFieldInfo("XValue", typeof(double), false),
                new NFieldInfo("Label", typeof(string), false));

            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                m_MemoryDataTable.AddRow(
                    rnd.Next(0, 100), 
                    rnd.Next(0, 100), 
                    "Label: " + i.ToString());
            }

			// create cartesian chart view 
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // get chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            // switch X axis in linear scale
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();

            // create a point series to show the data source values
            NPointSeries pointSeries = new NPointSeries();
            chart.Series.Add(pointSeries);
            pointSeries.UseXValues = true;
            pointSeries.ValueFormatter = new NNumericValueFormatter("0.00");
            pointSeries.DataLabelStyle = new NDataLabelStyle();
            pointSeries.DataLabelStyle.Visible = true;
            pointSeries.DataLabelStyle.Format = "<label>";
            pointSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Bottom;
            pointSeries.DataLabelStyle.ArrowLength = 30;

            // bind point series
            pointSeries.DataPoints.Bind(new NDataSource(m_MemoryDataTable),
                new NKeyValuePair<string, string>("Value", "Value"),
                new NKeyValuePair<string, string>("XValue", "X"),
                new NKeyValuePair<string, string>("Label", "Label"));

            // configure title
            chartView.Surface.Titles[0].Text = "Chart Data Binding";

            // use bright theme
            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));
            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            stack.FillMode = ENStackFillMode.First;
            stack.FitMode = ENStackFitMode.First;

            // table to edit the data
            NTableGridView tableGridView = new NTableGridView();
            tableGridView.GroupingPanel.Visibility = ENVisibility.Collapsed;

            tableGridView.Content.RowHeaders.ShowRowNumbers = true;
            tableGridView.Content.AutoCreateColumns = true;
            tableGridView.Content.AllowEdit = true;
            tableGridView.Content.DataSource = new NDataSource(m_MemoryDataTable);
            stack.Add(tableGridView);

            NStackPanel hstack = new NStackPanel();
            stack.Add(hstack);

            return stack;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how bind the chart data points to a data source. 
                </br>
                Once the data points are bound, they will be automatically updated when the data source changes.</p>";
        }

        #endregion

        #region Fields

        private NMemoryDataTable m_MemoryDataTable;

        #endregion

        #region Schema

        public static readonly NSchema NChartDataBindingExampleSchema;

        #endregion
    }
}
