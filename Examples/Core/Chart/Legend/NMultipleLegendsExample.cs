using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Legend Layout Example
	/// </summary>
	public class NMultipleLegendsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NMultipleLegendsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
        static NMultipleLegendsExample()
		{
			NMultipleLegendsExampleSchema = NSchema.Create(typeof(NMultipleLegendsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
            m_ChartView = new NChartView();

            NDockPanel dockPanel = new NDockPanel();
            m_ChartView.Surface.Content = dockPanel;

            NLabel label = new NLabel();
            label.Margins = new NMargins(10);
            label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 12);
            label.TextFill = new NColorFill(NColor.Black);
            label.TextAlignment = ENContentAlignment.MiddleCenter;
            label.Text = "Multiple Legends";
            NDockLayout.SetDockArea(label, ENDockArea.Top);
            dockPanel.AddChild(label);

            // stack panel holding content
            NStackPanel stackPanel = new NStackPanel();
            stackPanel.UniformHeights = ENUniformSize.Max;
            stackPanel.FillMode = ENStackFillMode.Equal;
            stackPanel.FitMode = ENStackFitMode.Equal;
            NDockLayout.SetDockArea(stackPanel, ENDockArea.Center);
            dockPanel.AddChild(stackPanel);

            // first group of pie + legend
            NDockPanel firstGroupPanel = new NDockPanel();
            stackPanel.AddChild(firstGroupPanel);

            m_PieChart1 = CreatePieChart();
            m_Legend1 = CreateLegend();

            m_PieChart1.Legend = m_Legend1;

            firstGroupPanel.AddChild(m_Legend1);
            firstGroupPanel.AddChild(m_PieChart1);

            // second group of pie + legend
            NDockPanel secondGroupPanel = new NDockPanel();
            stackPanel.AddChild(secondGroupPanel);

            // setup the volume chart
            m_PieChart2 = CreatePieChart();
            m_Legend2 = CreateLegend();

            m_PieChart2.Legend = m_Legend2;

            secondGroupPanel.AddChild(m_Legend2);
            secondGroupPanel.AddChild(m_PieChart2);

            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

            return m_ChartView;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NCheckBox useTwoLegendsCheckBox = new NCheckBox("Use Two Legends");
            useTwoLegendsCheckBox.Checked = true;
            useTwoLegendsCheckBox.CheckedChanged += OnUseTwoLegendsCheckBoxCheckedChanged;
            stack.Add(useTwoLegendsCheckBox);

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to display series data on different legends.</p>";
		}

		#endregion

        #region Implementation

        private NLegend CreateLegend()
        {
            NLegend legend = new NLegend();

            NDockLayout.SetDockArea(legend, ENDockArea.Center);

            legend.HorizontalPlacement = ENHorizontalPlacement.Right;
            legend.VerticalPlacement = ENVerticalPlacement.Top;

            return legend;
        }

        private NPieChart CreatePieChart()
        {
            NPieChart pieChart = new NPieChart();
            NDockLayout.SetDockArea(pieChart, ENDockArea.Center);
            pieChart.Margins = new NMargins(10, 0, 10, 10);

            NPieSeries pieSeries = new NPieSeries();
            pieChart.Series.Add(pieSeries);
            pieChart.DockSpiderLabelsToSides = false;

            NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
            dataLabelStyle.ArrowLength = 15;
            dataLabelStyle.ArrowPointerLength = 0;
            pieSeries.DataLabelStyle = dataLabelStyle;

            pieSeries.LabelMode = ENPieLabelMode.Spider;
            pieSeries.LegendView.Mode = ENSeriesLegendMode.DataPoints;
            pieSeries.LegendView.Format = "<label> <percent>";

            pieSeries.DataPoints.Add(new NPieDataPoint(24, "Cars"));
            pieSeries.DataPoints.Add(new NPieDataPoint(18, "Airplanes"));
            pieSeries.DataPoints.Add(new NPieDataPoint(32, "Trains"));
            pieSeries.DataPoints.Add(new NPieDataPoint(23, "Ships"));
            pieSeries.DataPoints.Add(new NPieDataPoint(19, "Buses"));

            return pieChart;
        }

        #endregion

        #region Event Handlers

        void OnUseTwoLegendsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            if (((NCheckBox)arg.TargetNode).Checked)
            {
                m_PieChart1.Legend = m_Legend1;
                m_PieChart2.Legend = m_Legend2;
            }
            else
            {
                m_PieChart1.Legend = m_Legend1;
                m_PieChart2.Legend = m_Legend1;
            }
        }

        #endregion

        #region Fields

        private NChartView m_ChartView;
        private NPieChart m_PieChart1;
        private NLegend m_Legend1;
        private NPieChart m_PieChart2;
        private NLegend m_Legend2;

		#endregion

		#region Schema

		public static readonly NSchema NMultipleLegendsExampleSchema;

		#endregion
	}
}