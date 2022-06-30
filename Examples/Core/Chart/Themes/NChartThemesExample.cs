using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Themes Example
	/// </summary>
	public class NChartThemesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NChartThemesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NChartThemesExample()
		{
			NChartThemesExampleSchema = NSchema.Create(typeof(NChartThemesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChartView = new NChartView();
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Chart Themes";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// add a bar series
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                NBarSeries bar = new NBarSeries();
			    bar.Name = "Bar" + i.ToString();
			    bar.MultiBarMode = ENMultiBarMode.Clustered;
                bar.DataLabelStyle = new NDataLabelStyle(false);
			    bar.ValueFormatter = new NNumericValueFormatter("0.###");
			    chart.Series.Add(bar);

                for (int j = 0; j < 6; j++)
                {
                    bar.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
                }
            }

			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return m_ChartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            m_ChartThemesComboBox = new NComboBox();
            m_ChartThemesComboBox.FillFromEnum<ENChartPalette>();
            m_ChartThemesComboBox.SelectedIndexChanged += OnChartThemesComboBoxSelectedIndexChanged;
            stack.Add(m_ChartThemesComboBox);

            m_ColorDataPointsCheckBox = new NCheckBox("Color Data Points");
            m_ColorDataPointsCheckBox.CheckedChanged += OnColorDataPointsCheckBoxCheckedChanged;
            stack.Add(m_ColorDataPointsCheckBox);

            m_ChartThemesComboBox.SelectedIndex = (int)ENChartPalette.Autumn;
            m_ColorDataPointsCheckBox.Checked = true;

            return boxGroup;
		}		
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to apply different chart color themes.</p>";
		}

		#endregion

		#region Event Handlers

        void OnColorDataPointsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme((ENChartPalette)m_ChartThemesComboBox.SelectedIndex, m_ColorDataPointsCheckBox.Checked));
        }

        void OnChartThemesComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme((ENChartPalette)m_ChartThemesComboBox.SelectedIndex, m_ColorDataPointsCheckBox.Checked));
        }

		#endregion

		#region Fields

		private NChartView m_ChartView;
		private NComboBox m_ChartThemesComboBox;
        private NCheckBox m_ColorDataPointsCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NChartThemesExampleSchema;

		#endregion
	}
}