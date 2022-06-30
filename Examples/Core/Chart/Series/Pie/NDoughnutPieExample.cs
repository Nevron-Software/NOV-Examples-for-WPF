using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Doughnut Pie Example
	/// </summary>
	public class NDoughnutPieExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NDoughnutPieExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NDoughnutPieExample()
		{
			NDoughnutPieExampleSchema = NSchema.Create(typeof(NDoughnutPieExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreatePieChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Doughnut Pie";

			// configure chart
			m_PieChart = (NPieChart)chartView.Surface.Charts[0];

			m_PieChart.BeginRadiusPercent = 20;
			m_PieChart.LabelLayout.EnableInitialPositioning = false;
			string[] labels = new string[] { "Ships", "Trains", "Automobiles", "Airplanes" };

			Random random = new Random();

			for (int i = 0; i < 4; i++)
			{
				NPieSeries pieSeries = new NPieSeries();
				
				// create a small detachment between pie rings
				pieSeries.BeginRadiusPercent = 10;

				m_PieChart.Series.Add(pieSeries);
				m_PieChart.DockSpiderLabelsToSides = true;

				NDataLabelStyle dataLabelStyle = new NDataLabelStyle(false);
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


		#endregion

		#region Fields

		NPieChart m_PieChart;

		#endregion

		#region Schema

		public static readonly NSchema NDoughnutPieExampleSchema;

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