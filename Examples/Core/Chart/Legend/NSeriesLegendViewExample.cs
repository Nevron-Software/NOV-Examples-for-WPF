using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Series Legend Modes Example
	/// </summary>
	public class NSeriesLegendViewExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NSeriesLegendViewExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NSeriesLegendViewExample()
		{
			NSeriesLegendViewExampleSchema = NSchema.Create(typeof(NSeriesLegendViewExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
            NChartView chartView = new NChartView();
            chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Series Legend View";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            // add interlace stripe
            NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            linearScale.Strips.Add(strip);

            // add a bar series
            NBarSeries bar1 = new NBarSeries();
            bar1.Name = "Bar1";
            bar1.MultiBarMode = ENMultiBarMode.Series;
            bar1.DataLabelStyle = new NDataLabelStyle(false);
            bar1.ValueFormatter = new NNumericValueFormatter("0.###");
            m_Chart.Series.Add(bar1);

            // add another bar series
            NBarSeries bar2 = new NBarSeries();
            bar2.Name = "Bar2";
            bar2.MultiBarMode = ENMultiBarMode.Clustered;
            bar2.DataLabelStyle = new NDataLabelStyle(false); 
            bar2.ValueFormatter = new NNumericValueFormatter("0.###");
            m_Chart.Series.Add(bar2);

            // add another bar series
            NBarSeries bar3 = new NBarSeries();
            bar3.Name = "Bar2";
            bar3.MultiBarMode = ENMultiBarMode.Clustered;
            bar3.DataLabelStyle = new NDataLabelStyle(false);
            bar3.ValueFormatter = new NNumericValueFormatter("0.###");
            m_Chart.Series.Add(bar3);

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                bar1.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
                bar2.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
                bar3.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
            }

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

            return chartView;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NComboBox seriesLegendModeComboBox = new NComboBox();
            seriesLegendModeComboBox.FillFromEnum<ENSeriesLegendMode>();
            seriesLegendModeComboBox.SelectedIndexChanged += OnSeriesLegendModeComboBoxSelectedIndexChanged;
            seriesLegendModeComboBox.SelectedIndex = (int)ENSeriesLegendMode.Series;
            stack.Add(NPairBox.Create("Legend Mode: ", seriesLegendModeComboBox));

            NComboBox seriesLegendOrderComboBox = new NComboBox();
            seriesLegendOrderComboBox.FillFromEnum<ENSeriesLegendOrder>();
            seriesLegendOrderComboBox.SelectedIndexChanged += OnSeriesLegendOrderComboBoxSelectedIndexChanged;
            seriesLegendOrderComboBox.SelectedIndex = (int)ENSeriesLegendOrder.Append;
            stack.Add(NPairBox.Create("Legend Order: ", seriesLegendOrderComboBox));

            NNumericUpDown markSizeUpDown = new NNumericUpDown();
            markSizeUpDown.ValueChanged += OnMarkSizeUpDownValueChanged;
            markSizeUpDown.Value = 10;
            stack.Add(NPairBox.Create("Mark Size: ", markSizeUpDown));

            NNumericUpDown fontSizeUpDown = new NNumericUpDown();
            fontSizeUpDown.ValueChanged += OnFontSizeUpDownValueChanged;
            fontSizeUpDown.Value = 10;
            stack.Add(NPairBox.Create("Font Size: ", fontSizeUpDown));

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the effect of different series legend view settings.</p>";
		}

		#endregion

		#region Event Handlers

        void OnSeriesLegendModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENSeriesLegendMode seriesLegendMode = (ENSeriesLegendMode)((NComboBox)arg.TargetNode).SelectedIndex;
            for (int i = 0; i < m_Chart.Series.Count; i++)
            {
                m_Chart.Series[i].LegendView.Mode = seriesLegendMode;
            }
        }

        void OnSeriesLegendOrderComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENSeriesLegendOrder seriesLegendOrder = (ENSeriesLegendOrder)((NComboBox)arg.TargetNode).SelectedIndex;
            for (int i = 0; i < m_Chart.Series.Count; i++)
            {
                m_Chart.Series[i].LegendView.Order = seriesLegendOrder;
            }
        }

        void OnFontSizeUpDownValueChanged(NValueChangeEventArgs arg)
        {
            double fontSize = ((NNumericUpDown)arg.TargetNode).Value;
            for (int i = 0; i < m_Chart.Series.Count; i++)
            {
                m_Chart.Series[i].LegendView.TextStyle.Font.Size = fontSize;
            }
        }

        void OnMarkSizeUpDownValueChanged(NValueChangeEventArgs arg)
        {
            double markSize = ((NNumericUpDown)arg.TargetNode).Value;
            for (int i = 0; i < m_Chart.Series.Count; i++)
            {
                m_Chart.Series[i].LegendView.MarkSize = new NSize(markSize, markSize);
            }
        }

		#endregion

        #region Fields

        NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NSeriesLegendViewExampleSchema;

        #endregion
    }
}