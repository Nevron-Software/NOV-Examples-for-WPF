using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard 3D Area Example
	/// </summary>
	public class NStandard3DAreaExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandard3DAreaExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandard3DAreaExample()
		{
			NStandard3DAreaExampleSchema = NSchema.Create(typeof(NStandard3DAreaExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Area";

            // configure chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            chart.Enable3D = true;
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
			chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleX.InflateContentRange = false;
			scaleX.MajorTickMode = ENMajorTickMode.AutoMaxCount;
			scaleX.DisplayDataPointsBetweenTicks = false;
			scaleX.Labels.Visible = false;

            for (int i = 0; i < monthLetters.Length; i++)
			{
				scaleX.CustomLabels.Add(new NCustomValueLabel(i, monthLetters[i]));
			}

			// add interlaced stripe for Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;

			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.Strips.Add(stripStyle);

			// setup area series
			m_Area = new NAreaSeries();
			m_Area.Name = "Area Series";

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Visible = true;
			dataLabelStyle.Format = "<value>";

			m_Area.DataLabelStyle = dataLabelStyle;

			for (int i = 0; i < monthValues.Length; i++)
			{
				m_Area.DataPoints.Add(new NAreaDataPoint(monthValues[i]));
			}

			chart.Series.Add(m_Area);

			return chartViewWithCommandBars;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox originModeComboBox = new NComboBox();
			originModeComboBox.FillFromEnum<ENSeriesOriginMode>();
			originModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOriginModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Origin Mode: ", originModeComboBox));

			NNumericUpDown customOriginUpDown = new NNumericUpDown();
			m_CustomOriginUpDown = customOriginUpDown;
            m_CustomOriginUpDown.Enabled = m_Area.OriginMode == ENSeriesOriginMode.CustomOrigin;

            customOriginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnCustomOriginUpDownValueChanged);
			customOriginUpDown.Value = 17;
            stack.Add(NPairBox.Create("Custom Origin: ", customOriginUpDown));

            NNumericUpDown depthPercentUpDown = new NNumericUpDown();
            depthPercentUpDown.ValueChanged += OnDepthPercentUpDownValueChanged;
			depthPercentUpDown.Value = 50.0f;
            stack.Add(NPairBox.Create("Depth Percent: ", depthPercentUpDown));

            return boxGroup;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard 3D area chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnCustomOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Area.CustomOrigin = (double)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnOriginModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Area.OriginMode = (ENSeriesOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
			m_CustomOriginUpDown.Enabled = m_Area.OriginMode == ENSeriesOriginMode.CustomOrigin;
        }

        private void OnDepthPercentUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Area.DepthPercent = (float)((NNumericUpDown)arg.TargetNode).Value;
        }

        #endregion

        #region Fields

        NAreaSeries m_Area;
		NNumericUpDown m_CustomOriginUpDown;

        #endregion

        #region Schema

        public static readonly NSchema NStandard3DAreaExampleSchema;

		#endregion

		#region Constants

		internal static readonly double[] monthValues = new double[] { 16, 19, 16, 15, 18, 19, 24, 21, 22, 17, 19, 15 };
		internal static readonly string[] monthLetters = new string[] { "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };

		#endregion
	}
}