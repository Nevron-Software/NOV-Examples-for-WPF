using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Point Drop Lines Example
	/// </summary>
	public class NPointDropLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPointDropLinesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPointDropLinesExample()
		{
			NPointDropLinesExampleSchema = NSchema.Create(typeof(NPointDropLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Point Drop Lines";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

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
			m_Point.Shape = ENPointShape.Ellipse;
			m_Point.UseXValues = true;
			m_Chart.Series.Add(m_Point);

            for (int i = 0; i < 360; i += 5)
            {
                double value = Math.Sin(NAngle.Degree2Rad * i);

                m_Point.DataPoints.Add(new NPointDataPoint(i, value));
            }

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
            NCheckBox showVerticalDropLinesCheckBox = new NCheckBox("Show Vertical Drop Lines");
            showVerticalDropLinesCheckBox.CheckedChanged += OnShowVerticalDropLinesCheckBoxCheckedChanged;
            stack.Add(showVerticalDropLinesCheckBox);

            NComboBox verticalDropLinesOriginModeCombo = new NComboBox();
            verticalDropLinesOriginModeCombo.FillFromEnum<ENDropLineOriginMode>();
            verticalDropLinesOriginModeCombo.SelectedIndexChanged += OnVerticalDropLinesOriginModeComboSelectedIndexChanged;
            verticalDropLinesOriginModeCombo.SelectedIndex = (int)ENDropLineOriginMode.CustomValue;
            stack.Add(NPairBox.Create("Origin Mode:", verticalDropLinesOriginModeCombo));

            NNumericUpDown verticalDropLinesOriginUpDown = new NNumericUpDown();
            verticalDropLinesOriginUpDown.ValueChanged += OnVerticalDropLinesOriginUpDownValueChanged;
            verticalDropLinesOriginUpDown.Value = 180.0;
            stack.Add(NPairBox.Create("Origin", verticalDropLinesOriginUpDown));

            NCheckBox showHorizontalDropLinesCheckBox = new NCheckBox("Show Horizontal Drop Lines");
            showHorizontalDropLinesCheckBox.CheckedChanged += OnShowHorizontalDropLinesCheckBoxCheckedChanged;
            showHorizontalDropLinesCheckBox.Checked = true;
            stack.Add(showHorizontalDropLinesCheckBox);

            NComboBox horizontalDropLinesOriginModeCombo = new NComboBox();
            horizontalDropLinesOriginModeCombo.FillFromEnum<ENDropLineOriginMode>();
            horizontalDropLinesOriginModeCombo.SelectedIndexChanged += OnHorizontalDropLinesOriginModeComboSelectedIndexChanged;
            horizontalDropLinesOriginModeCombo.SelectedIndex = (int)ENDropLineOriginMode.CustomValue;
            stack.Add(NPairBox.Create("Origin Mode:", horizontalDropLinesOriginModeCombo));

            NNumericUpDown horizontalDropLinesOriginUpDown = new NNumericUpDown();
            horizontalDropLinesOriginUpDown.ValueChanged +=OnHorizontalDropLinesOriginUpDownValueChanged;
            stack.Add(NPairBox.Create("Origin:", horizontalDropLinesOriginUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to enable point drop lines.</p>";
		}

		#endregion

		#region Event Handlers

        void OnHorizontalDropLinesOriginUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Point.HorizontalDropLineOrigin = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnVerticalDropLinesOriginUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Point.VerticalDropLineOrigin = ((NNumericUpDown)arg.TargetNode).Value;
        }

        void OnHorizontalDropLinesOriginModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Point.HorizontalDropLineOriginMode = (ENDropLineOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        void OnVerticalDropLinesOriginModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Point.VerticalDropLineOriginMode = (ENDropLineOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
        }

        void OnShowHorizontalDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Point.ShowHorizontalDropLines = ((NCheckBox)arg.TargetNode).Checked;
        }

        void OnShowVerticalDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Point.ShowVerticalDropLines = ((NCheckBox)arg.TargetNode).Checked;
        }

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NPointDropLinesExampleSchema;

		#endregion
	}
}