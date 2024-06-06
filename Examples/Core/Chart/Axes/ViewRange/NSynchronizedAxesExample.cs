using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis View Range Example
	/// </summary>
	public class NSynchronizedAxesExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NSynchronizedAxesExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NSynchronizedAxesExample()
        {
            NSynchronizedAxesExampleSchema = NSchema.Create(typeof(NSynchronizedAxesExample), NExampleBaseSchema);
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
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.PrimaryAndSecondaryLinear);

            // configure axes

            NCartesianAxis primaryX = m_Chart.Axes[ENCartesianAxis.PrimaryX];
            NCartesianAxis secondaryX = m_Chart.Axes[ENCartesianAxis.SecondaryX];
            NCartesianAxis primaryY = m_Chart.Axes[ENCartesianAxis.PrimaryY];
            NCartesianAxis secondaryY = m_Chart.Axes[ENCartesianAxis.SecondaryY];

            primaryX.Scale.Title.Text = "Primary X";
            primaryY.Scale.Title.Text = "Primary Y";
            secondaryX.Scale.Title.Text = "Secondary X";
            secondaryY.Scale.Title.Text = "Secondary Y";

            primaryX.SynchronizedAxes = new NDomArray<NNodeRef>(new NNodeRef(secondaryX));
            primaryY.SynchronizedAxes = new NDomArray<NNodeRef>(new NNodeRef(secondaryY));

            secondaryX.VisibilityMode = ENAxisVisibilityMode.Visible;
            secondaryY.VisibilityMode = ENAxisVisibilityMode.Visible;

            OnChangeDataButtonClick(null);

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NButton changeDataButton = new NButton("Change Data");
            changeDataButton.Click += new Function<NEventArgs>(OnChangeDataButtonClick);
            stack.Add(changeDataButton);

            return boxGroup;
        }
        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to synchronize the ranges of two or more axes.</p>";
        }

        #endregion

        #region Event Handlers

        void OnChangeDataButtonClick(NEventArgs arg)
        {
            m_Chart.Series.Clear();

            // setup bar series
            NPointSeries point = new NPointSeries();
            point.UseXValues = true;
            m_Chart.Series.Add(point);

            point.DataLabelStyle = new NDataLabelStyle(false);

            // fill in some data so that it contains several peaks of data
            Random random = new Random();

            for (int i = 0; i < 25; i++)
            {
                double value = random.NextDouble() * 100;
                double xvalue = random.NextDouble() * 100;

                NPointDataPoint dataPoint = new NPointDataPoint(value, xvalue);
                point.DataPoints.Add(dataPoint);
            }
        }

        #endregion

        #region Implementation

        #endregion

        #region Fields

        NCartesianChart m_Chart;

        #endregion

        #region Schema

        public static readonly NSchema NSynchronizedAxesExampleSchema;

        #endregion
    }
}