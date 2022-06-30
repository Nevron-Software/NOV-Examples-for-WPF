using System;
using Nevron.Nov.Grid;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Data;
using Nevron.Nov.Dom;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;
using Nevron.Nov.Chart;

namespace Nevron.Nov.Examples.Grid
{
    public class NCustomColumnFormatsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public NCustomColumnFormatsExample()
        {
        }
        /// <summary>
        /// Static constructor.
        /// </summary>
        static NCustomColumnFormatsExample()
        {
            NCustomColumnFormatsExampleSchema = NSchema.Create(typeof(NCustomColumnFormatsExample), NExampleBase.NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
            NMemoryDataTable dataTable = new NMemoryDataTable(
                new NFieldInfo("Company", typeof(String)),
                new NFieldInfo("RegionSales", typeof(Double[])));

            Random rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                Double[] arr = new Double[10];
                for (int j = 0; j < 10; j++)
                {
                    arr[j] = rnd.Next(100);
                }

                dataTable.AddRow(NDummyDataSource.RandomCompanyName(), arr);
            }

            // create a view and get its grid
            NTableGridView view = new NTableGridView();
            NTableGrid grid = view.Grid;

            grid.AutoCreateColumn += delegate(NAutoCreateColumnEventArgs arg)
            {
                if (arg.DataColumn.FieldName == "RegionSales")
                {
                    NCustomColumnFormat pieColumnFormat = new NCustomColumnFormat();
                    pieColumnFormat.FormatDefaultDataCellDelegate = delegate(NDataCell theDataCell)
                    {
                        NWidget widget = new NWidget();
                        widget.PreferredSize = new NSize(400, 300);
                    };

                    pieColumnFormat.CreateValueDataCellViewDelegate = delegate(NDataCell theDataCell, object value)
                    {
                        double[] values = (double[])value;

                        NChartView chartView = new NChartView();
                        chartView.PreferredSize = new NSize(300, 60);

						NCartesianChart cartesianChart = new NCartesianChart();

						NDockLayout.SetDockArea(cartesianChart, ENDockArea.Center);
						chartView.Surface.Content = cartesianChart;

						cartesianChart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
						cartesianChart.Legend = null;

						cartesianChart.Axes[ENCartesianAxis.PrimaryX].Visible = false;
						NCartesianAxis yAxis = cartesianChart.Axes[ENCartesianAxis.PrimaryY];

						NValueScaleLabelStyle labelStyle = new NValueScaleLabelStyle();
						labelStyle.TextStyle.Font = new NFont("Arimo", 8);
						((NLinearScale)yAxis.Scale).Labels.Style = labelStyle;

                        NBarSeries barSeries = new NBarSeries();
						barSeries.DataLabelStyle = new NDataLabelStyle(false);
						barSeries.InflateMargins = false;
                        cartesianChart.Series.Add(barSeries);

                        int count = values.Length;
                        for (int i = 0; i < count; i++)
                        {
                            barSeries.DataPoints.Add(new NBarDataPoint(values[i]));
                        }

                        return chartView;
                    };

                    arg.DataColumn.Format = pieColumnFormat;
                }
            };

            grid.DataSource = new NDataSource(dataTable);
            return view;
        }
        protected override NWidget CreateExampleControls()
        {
            return null;
        }
        protected override string GetExampleDescription()
        {
            return @"
<p>
    Demonstrates custom column formatting.
</p>
<p>
    The custom column format is represented by the <b>NCustomColumnFormat</b> class. It exposes a set of delegates that you can hook to create your own widgets that represent the row values.
</p>
<p>
    In this example we are using the <b>NOV Chart for .NET</b> to display <b>Double[]</b> values.
<p>
";
        }

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NCustomColumnFormatsExample.
        /// </summary>
        public static readonly NSchema NCustomColumnFormatsExampleSchema;

        #endregion
    }
}