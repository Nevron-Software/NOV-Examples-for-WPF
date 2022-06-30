using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Scatter Funnel Example
	/// </summary>
	public class NScatterFunnelExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NScatterFunnelExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NScatterFunnelExample()
		{
			NScatterFunnelExampleSchema = NSchema.Create(typeof(NScatterFunnelExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateFunnelChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Scatter Funnel";

			NFunnelChart funnelChart = (NFunnelChart)chartView.Surface.Charts[0];

			m_FunnelSeries = new NFunnelSeries();
			m_FunnelSeries.UseXValues = true;
			m_FunnelSeries.Shape = ENFunnelShape.Rectangle;
			funnelChart.Series.Add(m_FunnelSeries);

			GenerateData();

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			return chartView;
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
			return @"<p>This example demonstrates how to create a scatter funnel chart.</p>";
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

		public static readonly NSchema NScatterFunnelExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreateFunnelChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Funnel);
			return chartView;
		}

		#endregion
	}
}