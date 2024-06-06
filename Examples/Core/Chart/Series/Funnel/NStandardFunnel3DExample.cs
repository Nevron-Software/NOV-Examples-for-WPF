using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Funnel 3D Example
	/// </summary>
	public class NStandardFunnel3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardFunnel3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardFunnel3DExample()
		{
			NStandardFunnel3DExampleSchema = NSchema.Create(typeof(NStandardFunnel3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Funnel);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Funnel";

			NFunnelChart funnelChart = (NFunnelChart)chartView.Surface.Charts[0];
			funnelChart.Enable3D = true;
			funnelChart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
			funnelChart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective);
            funnelChart.Interactor = new NInteractor(new NTrackballTool());

            m_FunnelSeries = new NFunnelSeries();
			funnelChart.Series.Add(m_FunnelSeries);

			m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(20.0, "Awareness"));
			m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(10.0, "First Hear"));
			m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(15.0, "Further Learn"));
			m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(7.0, "Liking"));
			m_FunnelSeries.DataPoints.Add(new NFunnelDataPoint(28.0, "Decision"));
            m_FunnelSeries.DataLabelStyle = new NDataLabelStyle(true);
            m_FunnelSeries.DataLabelStyle.VertAlign = ENVerticalAlignment.Center;

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
			funnelShapeCombo.SelectedIndex = (int)ENFunnelShape.Trapezoid;
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

            NNumericUpDown neckHeightPercentUpDown = new NNumericUpDown();
            neckHeightPercentUpDown.Value = m_FunnelSeries.NeckHeightPercent;
            neckHeightPercentUpDown.ValueChanged += OnNeckHeightPercentUpDownValueChanged;
            stack.Add(NPairBox.Create("Neck Height Percent:", neckHeightPercentUpDown));

            NNumericUpDown neckWidthPercentUpDown = new NNumericUpDown();
            neckWidthPercentUpDown.Value = m_FunnelSeries.NeckWidthPercent;
            neckWidthPercentUpDown.ValueChanged += OnNeckWidthPercentUpDownValueChanged;
            stack.Add(NPairBox.Create("Neck Width Percent:", neckWidthPercentUpDown));

            return group;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard 3D funnel chart.</p>";
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

        private void OnNeckHeightPercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
			m_FunnelSeries.NeckHeightPercent = ((NNumericUpDown)arg.TargetNode).Value;
        }

        private void OnNeckWidthPercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_FunnelSeries.NeckWidthPercent = ((NNumericUpDown)arg.TargetNode).Value;
        }

        #endregion

        #region Fields

        NFunnelSeries m_FunnelSeries;

		#endregion

		#region Schema

		public static readonly NSchema NStandardFunnel3DExampleSchema;

		#endregion
	}
}