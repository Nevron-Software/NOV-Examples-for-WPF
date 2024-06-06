using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Scatter Cluster Bar Example
	/// </summary>
	public class NXYZScatterClusterStackBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYZScatterClusterStackBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYZScatterClusterStackBarExample()
		{
			NXYZScatterClusterStackBarExampleSchema = NSchema.Create(typeof(NXYZScatterClusterStackBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XYZ Scatter Cluster Stack";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);

            // configure title
            chartView.Surface.Titles[0].Text = "XYZ Scatter Cluster Stack Bar";

            // set predefined projection and lighting
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_Chart.ModelWidth = 50;
            m_Chart.ModelHeight = 35;
            m_Chart.ModelDepth = 50;
            m_Chart.Enable3D = true;
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // configure the axes
            NLinearScale linearScale = new NLinearScale();
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = linearScale;
			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);

			linearScale = new NLinearScale();
            m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = linearScale;
			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

			// add interlace stripes to the Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.SetShowAtWall(ENChartWall.Back, true);
			stripStyle.SetShowAtWall(ENChartWall.Left, true);
			stripStyle.Interlaced = true;
			linearScale.Strips.Add(stripStyle);

			linearScale = new NLinearScale();
            m_Chart.Axes[ENCartesianAxis.Depth].Scale = new NOrdinalScale();

			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
			linearScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

			AddBarSeries(m_Chart, ENMultiBarMode.Series);
			AddBarSeries(m_Chart, ENMultiBarMode.Stacked);
            AddBarSeries(m_Chart, ENMultiBarMode.Clustered);
            AddBarSeries(m_Chart, ENMultiBarMode.Stacked);

            OnNewDataButtonClick(null);

            return chartViewWithCommandBars;
		}
        protected override NWidget CreateExampleControls()
		{
			NStackPanel propertyStack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(propertyStack);

			NButton newDataButton = new NButton("New Data");
			newDataButton.Click += OnNewDataButtonClick;
            propertyStack.Add(newDataButton);

            return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a cluster bar chart.</p>";
		}

		#endregion

		#region Implementation

        private void AddBarSeries(NCartesianChart chart, ENMultiBarMode mode)
        {
			NBarSeries bar = new NBarSeries();
			chart.Series.Add(bar);
			bar.WidthSizeMode = ENBarSizeMode.Fixed;
			bar.DepthSizeMode = ENBarSizeMode.Fixed;
            bar.MultiBarMode = mode;
            bar.UseXValues = true;
            bar.UseZValues = true;
            bar.InflateMargins = true;
        }
		/// <summary>
		/// Creates a new data label style object
		/// </summary>
		/// <returns></returns>
		private NDataLabelStyle CreateDataLabelStyle()
		{
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Format = "<value>";

			return dataLabelStyle;
		}

        void GenerateXYZData(NBarSeries bar)
        {
            bar.DataPoints.Clear();

			Random random = new Random();

            for (int i = 0; i < c_DataPointCount; i++)
            {
				NBarDataPoint dataPoint = new NBarDataPoint();

				dataPoint.X = random.NextDouble() * 5;
                dataPoint.Z = random.NextDouble() * 5;
				dataPoint.Y = random.NextDouble();

				bar.DataPoints.Add(dataPoint);
            }
        }

        void GenerateYData(NBarSeries bar)
        {
            bar.DataPoints.Clear();

			Random random = new Random();

            for (int i = 0; i < c_DataPointCount; i++)
            {
                bar.DataPoints.Add(new NBarDataPoint(random.NextDouble()));
            }
        }

        #endregion

        #region Event Handlers

        private void OnNewDataButtonClick(NEventArgs arg)
        {
			for (int i = 0; i < m_Chart.Series.Count; i++)
            {
                NBarSeries bar = (NBarSeries)m_Chart.Series[i];

                if (i == 0)
                {
                    // the master series needs Y, X and Z values
                    GenerateXYZData(bar);
                }
                else
                {
                    // the other series need only Y values
                    GenerateYData(bar);
                }
            }
        }

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Static Fields

		private const int c_DataPointCount = 10;

		#endregion

		#region Schema

		public static readonly NSchema NXYZScatterClusterStackBarExampleSchema;

		#endregion
	}
}
