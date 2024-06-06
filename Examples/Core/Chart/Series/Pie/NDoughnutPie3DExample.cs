using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Doughnut Pie Example
	/// </summary>
	public class NDoughnutPie3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NDoughnutPie3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NDoughnutPie3DExample()
		{
			NDoughnutPie3DExampleSchema = NSchema.Create(typeof(NDoughnutPie3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Pie);

			// configure title
			chartView.Surface.Titles[0].Text = "Doughnut Pie 3D";

			// configure chart
			m_PieChart = (NPieChart)chartView.Surface.Charts[0];
            m_PieChart.Enable3D = true;
            m_PieChart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_PieChart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveElevated);
            m_PieChart.Interactor = new NInteractor(new NTrackballTool());

            m_PieChart.BeginRadiusPercent = 20;
			m_PieChart.LabelLayout.EnableInitialPositioning = false;
			string[] labels = new string[] { "Ships", "Trains", "Automobiles", "Airplanes" };

			Random random = new Random();

			for (int i = 0; i < 4; i++)
			{
				NPieSeries pieSeries = new NPieSeries();
				
				// create a small detachment between pie rings
				pieSeries.BeginRadiusPercent = 10;
				pieSeries.PieSegmentShape = ENPieSegmentShape.Ring;


                m_PieChart.Series.Add(pieSeries);
				m_PieChart.DockSpiderLabelsToSides = true;

				NDataLabelStyle dataLabelStyle = new NDataLabelStyle(true);
				dataLabelStyle.ArrowLength = 0;
				dataLabelStyle.ArrowPointerLength = 0;
				dataLabelStyle.Format = "<percent>";
				dataLabelStyle.TextStyle.HorzAlign = ENTextHorzAlign.Center;
				dataLabelStyle.TextStyle.VertAlign = ENTextVertAlign.Center;

				pieSeries.DataLabelStyle = dataLabelStyle;

				if (i == 0)
				{
					pieSeries.LegendView.Mode = ENSeriesLegendMode.DataPoints;
					pieSeries.LegendView.Format = "<label>";
				}
				else
				{
					pieSeries.LegendView.Mode = ENSeriesLegendMode.None;
				}

				pieSeries.LabelMode = ENPieLabelMode.Center;

				for (int j = 0; j < labels.Length; j++)
				{
					pieSeries.DataPoints.Add(new NPieDataPoint(20 + random.Next(100), labels[j]));
				}
			}	
			
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

            NComboBox pieShapeComboBox = new NComboBox();
            pieShapeComboBox.FillFromEnum<ENPieSegmentShape>();
            pieShapeComboBox.SelectedIndexChanged += OnPieShapeComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Pie Shape:", pieShapeComboBox));
            pieShapeComboBox.SelectedIndex = (int)ENPieSegmentShape.SmoothEdgeRing;

            NNumericUpDown pieEdgePercentUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("Pie Edge Percent:", pieEdgePercentUpDown));
            pieEdgePercentUpDown.ValueChanged += OnPieEdgePercentUpDownValueChanged;

            NCheckBox enableLabelAdjustmentCheckBox = new NCheckBox("Enable Label Adjustment");
            enableLabelAdjustmentCheckBox.CheckedChanged += OnEnableLabelAdjustmentCheckBoxCheckedChanged;
            enableLabelAdjustmentCheckBox.Checked = true;
            stack.Add(enableLabelAdjustmentCheckBox);

            return group;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a doughnut pie chart.</p>";
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

		void OnEnableLabelAdjustmentCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_PieChart.LabelLayout.EnableLabelAdjustment = ((NCheckBox)arg.TargetNode).Checked;
		}

        void OnPieShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
			for (int i = 0; i < m_PieChart.Series.Count; i++)
			{
				((NPieSeries)m_PieChart.Series[i]).PieSegmentShape = (ENPieSegmentShape)(((NComboBox)arg.TargetNode).SelectedIndex);
            }
        }

        private void OnPieEdgePercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
            for (int i = 0; i < m_PieChart.Series.Count; i++)
            {
                ((NPieSeries)m_PieChart.Series[i]).PieEdgePercent = (float)((NNumericUpDown)arg.TargetNode).Value;
            }
        }

        #endregion

        #region Fields

        NPieChart m_PieChart;

		#endregion

		#region Schema

		public static readonly NSchema NDoughnutPie3DExampleSchema;

		#endregion
	}
}