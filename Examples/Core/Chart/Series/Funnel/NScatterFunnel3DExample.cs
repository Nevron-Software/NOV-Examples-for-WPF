using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Scatter Funnel Example
	/// </summary>
	public class NScatterFunnel3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NScatterFunnel3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NScatterFunnel3DExample()
		{
			NScatterFunnel3DExampleSchema = NSchema.Create(typeof(NScatterFunnel3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Funnel);

			// configure title
			chartView.Surface.Titles[0].Text = "Scatter Funnel 3D";

			NFunnelChart funnelChart = (NFunnelChart)chartView.Surface.Charts[0];
            funnelChart.Enable3D = true;
            funnelChart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            funnelChart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            funnelChart.Interactor = new NInteractor(new NTrackballTool());

            m_FunnelSeries = new NFunnelSeries();
			m_FunnelSeries.UseXValues = true;
			m_FunnelSeries.Shape = ENFunnelShape.Rectangle;
            m_FunnelSeries.DataLabelStyle = new NDataLabelStyle(true);
			m_FunnelSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Center;
			m_FunnelSeries.LabelMode = ENFunnelLabelMode.LeftAligned;

            funnelChart.Series.Add(m_FunnelSeries);

            GenerateData();

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox funnelShapeCombo = new NComboBox();
			funnelShapeCombo.FillFromEnum<ENFunnelShape>();
			funnelShapeCombo.SelectedIndexChanged += OnFunnelShapeComboSelectedIndexChanged;
			funnelShapeCombo.SelectedIndex = (int)ENFunnelShape.Rectangle;
			stack.Add(NPairBox.Create("Funnel Shape:", funnelShapeCombo));

			NComboBox labelAligmentModeCombo = new NComboBox();
			labelAligmentModeCombo.FillFromEnum<ENFunnelLabelMode>();
			labelAligmentModeCombo.SelectedIndexChanged += OnLabelAligmentModeComboSelectedIndexChanged;
			labelAligmentModeCombo.SelectedIndex = (int)ENFunnelLabelMode.Center;
			stack.Add(NPairBox.Create("Label Alignment:", labelAligmentModeCombo));

			NNumericUpDown labelArrowLengthUpDown = new NNumericUpDown();
			labelArrowLengthUpDown.Value = m_FunnelSeries.LabelArrowLength;
			labelArrowLengthUpDown.ValueChanged += OnLabelArrowLengthUpDownValueChanged;
			stack.Add(NPairBox.Create("Label Arrow Length:", labelArrowLengthUpDown));

			NNumericUpDown pointGapUpDown = new NNumericUpDown();
			pointGapUpDown.Value = m_FunnelSeries.PointGapPercent;
			pointGapUpDown.ValueChanged += OnPointGapUpDownValueChanged;
			stack.Add(NPairBox.Create("Point Gap Percent:", pointGapUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a scatter 3D funnel chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnLabelArrowLengthUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_FunnelSeries.LabelArrowLength = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnLabelAligmentModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_FunnelSeries.LabelMode = (ENFunnelLabelMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnPointGapUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_FunnelSeries.PointGapPercent = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnFunnelShapeComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_FunnelSeries.Shape = (ENFunnelShape)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Implementation

		private void GenerateData()
		{
			m_FunnelSeries.DataPoints.Clear();

			double dSizeX = 100;

			Random random = new Random();

			for (int i = 0; i < 12; i++)
			{
				m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(random.Next(100) + 1, dSizeX));

				dSizeX -= random.NextDouble() * 9;
			}
		}

		#endregion

		#region Fields

		NFunnelSeries m_FunnelSeries;

		#endregion

		#region Schema

		public static readonly NSchema NScatterFunnel3DExampleSchema;

		#endregion
	}
}