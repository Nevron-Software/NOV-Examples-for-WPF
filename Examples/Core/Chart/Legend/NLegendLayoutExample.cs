using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Legend Layout Example
	/// </summary>
	public class NLegendLayoutExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLegendLayoutExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLegendLayoutExample()
		{
			NLegendLayoutExampleSchema = NSchema.Create(typeof(NLegendLayoutExample), NExampleBaseSchema);
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

            // add another bar series
            NBarSeries bar2 = new NBarSeries();
            bar2.Name = "Bar2";
            bar2.MultiBarMode = ENMultiBarMode.Clustered;
            bar2.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            bar2.DataLabelStyle = new NDataLabelStyle(false); 
            bar2.ValueFormatter = new NNumericValueFormatter("0.###");
            chart.Series.Add(bar2);

            // add another bar series
            NBarSeries bar3 = new NBarSeries();
            bar3.Name = "Bar2";
            bar3.MultiBarMode = ENMultiBarMode.Clustered;
            bar3.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            bar3.DataLabelStyle = new NDataLabelStyle(false);
            bar3.ValueFormatter = new NNumericValueFormatter("0.###");
            chart.Series.Add(bar3);

            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                bar1.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
                bar2.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
                bar3.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
            }

            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

            return m_ChartView;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NComboBox legendExpandModeComboBox = new NComboBox();
            legendExpandModeComboBox.FillFromEnum<ENLegendExpandMode>();
            legendExpandModeComboBox.SelectedIndexChanged += OnLegendExpandModeComboBoxSelectedIndexChanged;
            legendExpandModeComboBox.SelectedIndex = (int)ENLegendExpandMode.RowsOnly;
            stack.Add(NPairBox.Create("Expand Mode: ", legendExpandModeComboBox));

            m_RowCountUpDown = new NNumericUpDown();
            m_RowCountUpDown.Enabled = false;
            m_RowCountUpDown.Minimum = 1;
            m_RowCountUpDown.Value = 1;
            m_RowCountUpDown.ValueChanged += OnRowCountUpDownValueChanged;
            stack.Add(NPairBox.Create("Row Count: ", m_RowCountUpDown));

            m_ColCountUpDown = new NNumericUpDown();
            m_ColCountUpDown.Enabled = false;
            m_ColCountUpDown.Minimum = 1;
            m_ColCountUpDown.Value = 1;
            m_ColCountUpDown.ValueChanged += OnColCountUpDownValueChanged;
            stack.Add(NPairBox.Create("Col Count: ", m_ColCountUpDown));

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates different settings related to legend layout.</p>";
		}

		#endregion

		#region Event Handlers

        void OnRowCountUpDownValueChanged(NValueChangeEventArgs arg)
        {
 	        m_ChartView.Surface.Legends[0].RowCount = (int)((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnColCountUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_ChartView.Surface.Legends[0].ColCount = (int)((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnLegendExpandModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_ChartView.Surface.Legends[0].ExpandMode = (ENLegendExpandMode)(((NComboBox)arg.TargetNode).SelectedIndex);

            switch (m_ChartView.Surface.Legends[0].ExpandMode)
            {
                case ENLegendExpandMode.ColsFixed:
                    m_ColCountUpDown.Enabled = true;
                    m_RowCountUpDown.Enabled = false;
                    break;
                case ENLegendExpandMode.RowsFixed:
                    m_ColCountUpDown.Enabled = false;
                    m_RowCountUpDown.Enabled = true;
                    break;
                default:
                    m_ColCountUpDown.Enabled = false;
                    m_RowCountUpDown.Enabled = false;
                    break;

            }
        }

        #endregion

        #region Fields

        private NChartView m_ChartView;
        private NNumericUpDown m_ColCountUpDown;
        private NNumericUpDown m_RowCountUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NLegendLayoutExampleSchema;

        #endregion
    }
}