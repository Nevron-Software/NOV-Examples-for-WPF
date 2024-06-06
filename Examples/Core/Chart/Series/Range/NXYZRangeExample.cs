using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XYZ Range Example
	/// </summary>
	public class NXYZRangeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZRangeExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZRangeExample()
		{
			NXYZRangeExampleSchema = NSchema.Create(typeof(NXYZRangeExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Range Series";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.Enable3D = true;
			m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.Projection.Rotation = -18;
            m_Chart.Projection.Elevation = 13;
			m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_Chart.ModelDepth = 55.0f;
            m_Chart.ModelWidth = 55.0f;
            m_Chart.ModelHeight = 55.0f;
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            {
				NLinearScale scaleX = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
				scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
				scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
				scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
				m_Chart.Axes[ENCartesianAxis.PrimaryX].SetFixedViewRange(0, 20);
			}

			// setup Y axis
			{
				NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
                scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
				scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
				scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

				// add interlaced stripe
				NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
				stripStyle.SetShowAtWall(ENChartWall.Back, true);
				stripStyle.SetShowAtWall(ENChartWall.Left, true);
				stripStyle.Interlaced = true;
                scaleY.Strips.Add(stripStyle);

                m_Chart.Axes[ENCartesianAxis.PrimaryY].SetFixedViewRange(0, 20);
            }

			// setup Depth axis
			{
				NLinearScale scaleZ = (NLinearScale)m_Chart.Axes[ENCartesianAxis.Depth].Scale;
                scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
				scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
				scaleZ.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
                m_Chart.Axes[ENCartesianAxis.Depth].SetFixedViewRange(0, 20);
            }

			// setup shape series
			m_Range = new NRangeSeries();
			m_Chart.Series.Add(m_Range);

            m_Range.Fill = new NColorFill(NColor.Red);
            m_Range.Stroke = new NStroke(NColor.DarkRed);
            m_Range.LegendView.Mode = ENSeriesLegendMode.None;
            m_Range.UseXValues = true;
            m_Range.UseZValues = true;

            // add data
            m_Range.DataPoints.Add(new NRangeDataPoint(1, 11, 5, 5, 17, 9));
            m_Range.DataPoints.Add(new NRangeDataPoint(4, 15, 16, 7, 19, 19));
            m_Range.DataPoints.Add(new NRangeDataPoint(5, 6, 12, 15, 11, 18));
            m_Range.DataPoints.Add(new NRangeDataPoint(9, 2, 3, 14, 5, 5));
            m_Range.DataPoints.Add(new NRangeDataPoint(15, 2, 3, 19, 5, 5));

            chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.DataPoints));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox rangeShapeComboBox = new NComboBox();
			rangeShapeComboBox.FillFromEnum<ENBarShape>();
			rangeShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnRangeShapeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Range Shape: ", rangeShapeComboBox));

			NCheckBox showDataLabels = new NCheckBox("Show Data Labels");
			showDataLabels.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowDataLabelsCheckedChanged);
			showDataLabels.Checked = false;
            stack.Add(showDataLabels);

			rangeShapeComboBox.SelectedIndex = (int)ENBarShape.Rectangle;
			showDataLabels.Checked = false;

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a 3D range m_Chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnShowDataLabelsCheckedChanged(NValueChangeEventArgs arg)
		{
			if ((arg.TargetNode as NCheckBox).Checked)
			{
				m_Range.DataLabelStyle = new NDataLabelStyle(true);
				m_Range.DataLabelStyle.Format = "<y2>";
			}
			else
			{
				m_Range.DataLabelStyle = new NDataLabelStyle(false);
			}
		}

		void OnRangeShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Range.Shape = (ENBarShape)(arg.TargetNode as NComboBox).SelectedIndex;
		}

		#endregion

		#region Fields

		NRangeSeries m_Range;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NXYZRangeExampleSchema;

		#endregion
	}
}