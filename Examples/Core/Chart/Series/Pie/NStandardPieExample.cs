using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Pie Example
	/// </summary>
	public class NStandardPieExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardPieExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardPieExample()
		{
			NStandardPieExampleSchema = NSchema.Create(typeof(NStandardPieExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePieChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Pie";

			// configure chart
			m_PieChart = (NPieChart)chartView.Surface.Charts[0];

			m_PieSeries = new NPieSeries();
			m_PieChart.Series.Add(m_PieSeries);
			m_PieChart.DockSpiderLabelsToSides = true;

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.ArrowLength = 15;
			dataLabelStyle.ArrowPointerLength = 0;
			m_PieSeries.DataLabelStyle = dataLabelStyle;

			m_PieSeries.LabelMode = ENPieLabelMode.Spider;
			m_PieSeries.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			m_PieSeries.LegendView.Format = "<label> <percent>";

			m_PieSeries.DataPoints.Add(new NPieDataPoint(24, "Cars"));
			m_PieSeries.DataPoints.Add(new NPieDataPoint(18, "Airplanes"));
			m_PieSeries.DataPoints.Add(new NPieDataPoint(32, "Trains"));
			m_PieSeries.DataPoints.Add(new NPieDataPoint(23, "Ships"));
			m_PieSeries.DataPoints.Add(new NPieDataPoint(19, "Buses"));

			// detach airplanes
			m_PieSeries.DataPoints[1].DetachmentPercent = 10;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

			NNumericUpDown beginAngleUpDown = new NNumericUpDown();
			beginAngleUpDown.Value = m_PieChart.BeginAngle;
			beginAngleUpDown.ValueChanged += OnBeginAngleUpDownValueChanged;
			stack.Add(NPairBox.Create("Begin Angle:", beginAngleUpDown));

			NNumericUpDown sweepAngleUpDown = new NNumericUpDown();
			sweepAngleUpDown.Value = m_PieChart.SweepAngle;
			sweepAngleUpDown.Minimum = -360;
			sweepAngleUpDown.Maximum = 360;
			sweepAngleUpDown.ValueChanged += OnSweepAngleUpDownValueChanged;
			stack.Add(NPairBox.Create("Sweep Angle:", sweepAngleUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard pie chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PieChart.BeginAngle = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnSweepAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PieChart.SweepAngle = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnPieLabelModeSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_PieSeries.LabelMode = (ENPieLabelMode)(((NComboBox)arg.TargetNode).SelectedIndex);
		}

		#endregion

		#region Fields

		NPieSeries m_PieSeries;
		NPieChart m_PieChart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardPieExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreatePieChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Pie);
			return chartView;
		}

		#endregion
	}
}