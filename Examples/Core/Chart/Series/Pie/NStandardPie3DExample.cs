using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard 3D Pie Example
	/// </summary>
	public class NStandardPie3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardPie3DExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardPie3DExample()
		{
			NStandardPie3DExampleSchema = NSchema.Create(typeof(NStandardPie3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Pie);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Pie";

			// configure chart
			m_PieChart = (NPieChart)chartView.Surface.Charts[0];

			m_PieSeries = new NPieSeries();
			m_PieChart.Series.Add(m_PieSeries);
			m_PieChart.DockSpiderLabelsToSides = true;
            m_PieChart.Enable3D = true;
            m_PieChart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_PieChart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveElevated);
            m_PieChart.Interactor = new NInteractor(new NTrackballTool());

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

			for (int i = 0; i < m_PieSeries.DataPoints.Count; i++)
			{
				m_PieSeries.DataPoints[i].Tooltip = new NTooltip("This is a test tooltip for dataPoint [" + i.ToString() + "]");
            }

            // detach airplanes
            m_PieSeries.DataPoints[1].DetachmentPercent = 10;

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

			return chartViewWithCommandBars;
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

            NNumericUpDown beginRadiusPercentUpDown = new NNumericUpDown();
            beginRadiusPercentUpDown.Value = m_PieSeries.BeginRadiusPercent;
            beginRadiusPercentUpDown.ValueChanged += OnBeginRadiusPercentValueChanged;
            stack.Add(NPairBox.Create("Begin Radius %:", beginRadiusPercentUpDown));
            beginRadiusPercentUpDown.Value = 30;

            NNumericUpDown endRadiusPercentUpDown = new NNumericUpDown();
            endRadiusPercentUpDown.Value = m_PieSeries.EndRadiusPercent;
            endRadiusPercentUpDown.ValueChanged += OnEndRadiusPercentValueChanged;
            stack.Add(NPairBox.Create("End Radius %:", endRadiusPercentUpDown));

            NComboBox pieShapeComboBox = new NComboBox();
			pieShapeComboBox.FillFromEnum<ENPieSegmentShape>();
            pieShapeComboBox.SelectedIndexChanged += OnPieShapeComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Pie Shape:", pieShapeComboBox));
			pieShapeComboBox.SelectedIndex = (int)ENPieSegmentShape.SmoothEdgePie;

			NNumericUpDown pieEdgePercentUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("Pie Edge Percent:", pieEdgePercentUpDown));
            pieEdgePercentUpDown.ValueChanged += OnPieEdgePercentUpDownValueChanged;

            NComboBox pieLabelMode = new NComboBox();
            pieLabelMode.FillFromEnum<ENPieLabelMode>();
            pieLabelMode.SelectedIndexChanged += OnPieLabelModeSelectedIndexChanged;
            stack.Add(NPairBox.Create("Pie Label Mode:", pieLabelMode));
            pieLabelMode.SelectedIndex = (int)ENPieLabelMode.Spider;

            return group;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard pie chart.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PieChart.BeginAngle = ((NNumericUpDown)arg.TargetNode).Value;
		}

        private void OnSweepAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PieChart.SweepAngle = ((NNumericUpDown)arg.TargetNode).Value;
		}

        private void OnPieShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
			m_PieSeries.PieSegmentShape = (ENPieSegmentShape)(((NComboBox)arg.TargetNode).SelectedIndex);
        }

        private void OnPieEdgePercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_PieSeries.PieEdgePercent = (float)((NNumericUpDown)arg.TargetNode).Value;
        }

        private void OnPieLabelModeSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_PieSeries.LabelMode = (ENPieLabelMode)(((NComboBox)arg.TargetNode).SelectedIndex);
		}

		private void OnBeginRadiusPercentValueChanged(NValueChangeEventArgs arg)
		{
            m_PieSeries.BeginRadiusPercent = ((NNumericUpDown)arg.TargetNode).Value;
        }

		private void OnEndRadiusPercentValueChanged(NValueChangeEventArgs arg)
		{
			m_PieSeries.EndRadiusPercent = ((NNumericUpDown)arg.TargetNode).Value;
        }

        #endregion

        #region Fields

        NPieSeries m_PieSeries;
		NPieChart m_PieChart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardPie3DExampleSchema;

		#endregion
	}
}