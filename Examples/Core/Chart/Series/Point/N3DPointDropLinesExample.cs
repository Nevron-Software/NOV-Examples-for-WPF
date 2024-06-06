using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Point Drop Lines Example
	/// </summary>
	public class N3DPointDropLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public N3DPointDropLinesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static N3DPointDropLinesExample()
		{
			N3DPointDropLinesExampleSchema = NSchema.Create(typeof(N3DPointDropLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Point Drop Lines";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Enable3D = true;
            m_Chart.FitMode = ENCartesianChartFitMode.Aspect;
            m_Chart.ModelWidth = m_Chart.ModelHeight = m_Chart.ModelHeight = 50;

            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.NorthernLights);
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            NLinearScale scaleX = new NLinearScale();
			scaleX.MajorGridLines = new NScaleGridLines();
            scaleX.MajorGridLines.Visible = true;
			scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;

			// setup Y axis
			NLinearScale scaleY = new NLinearScale();
			scaleY.MajorGridLines = new NScaleGridLines();
            scaleY.MajorGridLines.Visible = true;
			scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = scaleY;

			// add a point series
			m_Point = new NPointSeries();
			m_Point.Name = "Point Series";
			m_Point.DataLabelStyle = new NDataLabelStyle(false);
			m_Point.Fill = new NColorFill(NColor.DarkOrange);
			m_Point.Size = 10;
			m_Point.Shape = ENPointShape3D.Sphere;
			m_Point.UseXValues = true;
            m_Point.UseZValues = true;
			m_Chart.Series.Add(m_Point);

            Random random = new Random();
            NDataPointCollection<NPointDataPoint> dataPoints = m_Point.DataPoints;

            for (int i = 0; i < 100; i += 5)
            {
                dataPoints.Add(new NPointDataPoint(random.Next(200) - 100, random.Next(200) - 100, random.Next(200) - 100));
            }

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NCheckBox showHorizontalDropLinesCheckBox = new NCheckBox("Show Horizontal Drop Lines");
            showHorizontalDropLinesCheckBox.CheckedChanged += OnShowHorizontalDropLinesCheckBoxCheckedChanged;
            showHorizontalDropLinesCheckBox.Checked = true;
            stack.Add(showHorizontalDropLinesCheckBox);

            NComboBox horizontalDropLinesOriginModeCombo = new NComboBox();
            horizontalDropLinesOriginModeCombo.FillFromEnum<ENDropLineOriginMode>();
            horizontalDropLinesOriginModeCombo.SelectedIndexChanged += OnHorizontalDropLinesOriginModeComboSelectedIndexChanged;
            horizontalDropLinesOriginModeCombo.SelectedIndex = (int)ENDropLineOriginMode.ScaleMin;
            stack.Add(NPairBox.Create("Origin Mode:", horizontalDropLinesOriginModeCombo));

            NNumericUpDown horizontalDropLinesOriginUpDown = new NNumericUpDown();
            horizontalDropLinesOriginUpDown.ValueChanged += OnHorizontalDropLinesOriginUpDownValueChanged;
            stack.Add(NPairBox.Create("Origin:", horizontalDropLinesOriginUpDown));

            NCheckBox showVerticalDropLinesCheckBox = new NCheckBox("Show Vertical Drop Lines");
            showVerticalDropLinesCheckBox.CheckedChanged += OnShowVerticalDropLinesCheckBoxCheckedChanged;
            showVerticalDropLinesCheckBox.Checked = true;
            stack.Add(showVerticalDropLinesCheckBox);

            NComboBox verticalDropLinesOriginModeCombo = new NComboBox();
            verticalDropLinesOriginModeCombo.FillFromEnum<ENDropLineOriginMode>();
            verticalDropLinesOriginModeCombo.SelectedIndexChanged += OnVerticalDropLinesOriginModeComboSelectedIndexChanged;
            verticalDropLinesOriginModeCombo.SelectedIndex = (int)ENDropLineOriginMode.ScaleMin;
            stack.Add(NPairBox.Create("Origin Mode:", verticalDropLinesOriginModeCombo));

            NNumericUpDown verticalDropLinesOriginUpDown = new NNumericUpDown();
            verticalDropLinesOriginUpDown.ValueChanged += OnVerticalDropLinesOriginUpDownValueChanged;
            verticalDropLinesOriginUpDown.Value = 0;
            stack.Add(NPairBox.Create("Origin", verticalDropLinesOriginUpDown));

            NCheckBox showDepthDropLinesCheckBox = new NCheckBox("Show Depth Drop Lines");
            showDepthDropLinesCheckBox.CheckedChanged += OnShowDepthDropLinesCheckBoxCheckedChanged;
            showDepthDropLinesCheckBox.Checked = true;
            stack.Add(showDepthDropLinesCheckBox);

            NComboBox depthDropLinesOriginModeCombo = new NComboBox();
            depthDropLinesOriginModeCombo.FillFromEnum<ENDropLineOriginMode>();
            depthDropLinesOriginModeCombo.SelectedIndexChanged += OnDepthDropLinesOriginModeComboSelectedIndexChanged;
            depthDropLinesOriginModeCombo.SelectedIndex = (int)ENDropLineOriginMode.ScaleMax;
            stack.Add(NPairBox.Create("Origin Mode:", depthDropLinesOriginModeCombo));

            NNumericUpDown depthDropLinesOriginUpDown = new NNumericUpDown();
            depthDropLinesOriginUpDown.ValueChanged += OnDepthDropLinesOriginUpDownValueChanged;
            stack.Add(NPairBox.Create("Origin:", depthDropLinesOriginUpDown));

            return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to enable point drop lines in 3D.</p>";
		}

		#endregion

		#region Event Handlers

        void OnHorizontalDropLinesOriginUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Point.HorizontalDropLineOrigin = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnHorizontalDropLinesOriginModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Point.HorizontalDropLineOriginMode = (ENDropLineOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        void OnShowHorizontalDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Point.ShowHorizontalDropLines = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnVerticalDropLinesOriginUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Point.VerticalDropLineOrigin = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnVerticalDropLinesOriginModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Point.VerticalDropLineOriginMode = (ENDropLineOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        void OnShowVerticalDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Point.ShowVerticalDropLines = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnDepthDropLinesOriginUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Point.DepthDropLineOrigin = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnDepthDropLinesOriginModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Point.DepthDropLineOriginMode = (ENDropLineOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        void OnShowDepthDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Point.ShowDepthDropLines = ((NCheckBox)arg.TargetNode).Checked;
        }

        #endregion

        #region Fields

        NPointSeries m_Point;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema N3DPointDropLinesExampleSchema;

		#endregion
	}
}