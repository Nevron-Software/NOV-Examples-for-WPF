using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Vector Example
	/// </summary>
	public class NXYZVectorExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZVectorExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZVectorExample()
		{
			NXYZVectorExampleSchema = NSchema.Create(typeof(NXYZVectorExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Vector";

            // configure chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.Enable3D = true;

            chart.Enable3D = true;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            chart.ModelDepth = 55.0f;
            chart.ModelWidth = 55.0f;
            chart.ModelHeight = 55.0f;
            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            {
                NLinearScale scaleX = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
                scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
                scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
                scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
            }

            // setup Y axis
            {
                NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
                scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
                scaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
                scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

                // add interlaced stripe
                NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
                stripStyle.SetShowAtWall(ENChartWall.Back, true);
                stripStyle.SetShowAtWall(ENChartWall.Left, true);
                stripStyle.Interlaced = true;
                scaleY.Strips.Add(stripStyle);
            }

            // setup Depth axis
            {
                NLinearScale scaleZ = (NLinearScale)chart.Axes[ENCartesianAxis.Depth].Scale;
                scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
                scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
                scaleZ.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
            }

            // setup X axis
            NLinearScale linearScale = new NLinearScale();
			chart.Axes[ENCartesianAxis.PrimaryX].Scale = linearScale;

			// setup Y axis
			linearScale = new NLinearScale();
			chart.Axes[ENCartesianAxis.PrimaryY].Scale = linearScale;

			// setup shape series
			m_VectorSeries = new NVectorSeries();
			chart.Series.Add(m_VectorSeries);
			m_VectorSeries.DataLabelStyle = new NDataLabelStyle(false);
			m_VectorSeries.InflateMargins = false;
			m_VectorSeries.UseXValues = true;
			m_VectorSeries.UseZValues = true;
            m_VectorSeries.MinArrowheadSize = new NSize(2, 3);
			m_VectorSeries.MaxArrowheadSize = new NSize(4, 6);

			// fill data
			FillData(m_VectorSeries);
		
			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            m_ArrowheadShapeComboBox = new NComboBox();
			m_ArrowheadShapeComboBox.FillFromEnum<ENVectorArrowheadShape>();
            m_ArrowheadShapeComboBox.SelectedIndexChanged += OnArrowHeadShapeComboBoxSelectedIndexChanged;
            m_ArrowheadShapeComboBox.SelectedIndex = (int)ENVectorArrowheadShape.Arrow;

            stack.Add(NPairBox.Create("Arrowhead shape:", m_ArrowheadShapeComboBox));

            return group;
		}

        private void OnArrowHeadShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_VectorSeries.ArrowheadShape = (ENVectorArrowheadShape)m_ArrowheadShapeComboBox.SelectedIndex;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard 2D vector chart.</p>";
		}

        #endregion

        #region Implementation

        private void FillData(NVectorSeries vectorSeries)
        {
            double x = 0, y = 0, z = 0;

            for (int w = 0; w < 5; w++)
            {
                y = 0;
                z += 1;

                for (int i = 0; i < 5; i++)
                {
                    x = 0;
                    y += 1;

                    for (int j = 0; j < 5; j++)
                    {
                        x += 1;

                        double dx = Math.Sin(x / 4.0) * Math.Sin(x / 4.0);
                        double dy = Math.Cos(y / 8.0) * Math.Cos(w / 4.0);

                        NStroke stroke = new NStroke(1, ColorFromVector(dx, dy));

						NVectorDataPoint dataPoint = new NVectorDataPoint(x, y, z, x + dx, y + dy, z - 0.5, null);
						dataPoint.Stroke = stroke;
                        vectorSeries.DataPoints.Add(dataPoint);
                    }
                }
            }
        }
               
		private NColor ColorFromVector(double dx, double dy)
		{
			double length = Math.Sqrt(dx * dx + dy * dy);

			double sq2 = Math.Sqrt(2);

			int r = (int)((255 / sq2) * length);
			int g = 20;
			int b = (int)((255 / sq2) * (sq2 - length));

			return NColor.FromRGB((byte)r, (byte)g, (byte)b);
		}

		#endregion

		#region Fields

        NVectorSeries m_VectorSeries;
        NComboBox m_ArrowheadShapeComboBox;

        #endregion

        #region Schema

        public static readonly NSchema NXYZVectorExampleSchema;

		#endregion
	}
}
