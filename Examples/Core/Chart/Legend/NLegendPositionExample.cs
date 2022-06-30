using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Legend Position Example
	/// </summary>
	public class NLegendPositionExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLegendPositionExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLegendPositionExample()
		{
			NLegendPositionExampleSchema = NSchema.Create(typeof(NLegendPositionExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
            m_ChartView = new NChartView();
            m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            m_ChartView.Surface.Titles[0].Text = "Legend Layout";

            // configure chart
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            // add interlace stripe
            NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            // add a bar series
            NBarSeries bar1 = new NBarSeries();
            bar1.Name = "Bar1";
            bar1.MultiBarMode = ENMultiBarMode.Series;
            bar1.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            bar1.DataLabelStyle = new NDataLabelStyle(false);
            bar1.ValueFormatter = new NNumericValueFormatter("0.###");
            chart.Series.Add(bar1);

            Random random = new Random();

            for (int i = 0; i < 5; i++)
            {
                bar1.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
            }

            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

            return m_ChartView;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NComboBox legendDockAreaComboBox = new NComboBox();
            legendDockAreaComboBox.FillFromEnum<ENDockArea>();
            legendDockAreaComboBox.SelectedIndexChanged += OnLegendDockAreaComboBoxSelectedIndexChanged;
            legendDockAreaComboBox.SelectedIndex = (int)ENDockArea.Right;
            stack.Add(NPairBox.Create("Dock Area: ", legendDockAreaComboBox));

            NCheckBox dockInsideChartPlotCheckBox = new NCheckBox("Dock in Chart Plot Area");
            dockInsideChartPlotCheckBox.CheckedChanged += OnDockInsideChartPlotCheckBoxCheckedChanged;
            stack.Add(dockInsideChartPlotCheckBox);

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to position the legend.</p>";
		}

		#endregion

		#region Event Handlers

        private void OnDockInsideChartPlotCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            NLegend legend = m_ChartView.Surface.Legends[0];
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            legend.ParentNode.RemoveChild(legend);

            if (((NCheckBox)arg.TargetNode).Checked)
            {
                // dock the legend inside the chart
                NDockPanel dockPanel = new NDockPanel();
                chart.Content = dockPanel;
                dockPanel.Add(legend);
            }
            else
            {
                // dock the legend inside the chart
                NDockPanel content = m_ChartView.Surface.Content as NDockPanel;
                content.Add(legend);
            }
        }

        private void OnLegendDockAreaComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENDockArea dockArea = (ENDockArea)((NComboBox)arg.TargetNode).SelectedIndex;
            NLegend legend = m_ChartView.Surface.Legends[0];

            // adjust the legend layout / position accordingly to the dock area
            switch (dockArea)
            {
                case ENDockArea.Left:
                    legend.ExpandMode = ENLegendExpandMode.RowsOnly;
                    legend.VerticalPlacement = ENVerticalPlacement.Center;
                    break;
                case ENDockArea.Top:
                    legend.ExpandMode = ENLegendExpandMode.ColsOnly;
                    legend.HorizontalPlacement = ENHorizontalPlacement.Center;
                    break;
                case ENDockArea.Right:
                    legend.ExpandMode = ENLegendExpandMode.RowsOnly;
                    legend.VerticalPlacement = ENVerticalPlacement.Center;
                    break;
                case ENDockArea.Bottom:
                    legend.ExpandMode = ENLegendExpandMode.ColsOnly;
                    legend.HorizontalPlacement = ENHorizontalPlacement.Center;
                    break;
                case ENDockArea.Center:
                    legend.ExpandMode = ENLegendExpandMode.RowsOnly;
                    legend.HorizontalPlacement = ENHorizontalPlacement.Center;
                    legend.VerticalPlacement = ENVerticalPlacement.Center;
                    break;
            }

            NDockLayout.SetDockArea(legend, dockArea);
        }

        #endregion

        #region Fields

        private NChartView m_ChartView;

        #endregion

        #region Schema

        public static readonly NSchema NLegendPositionExampleSchema;

        #endregion
    }
}