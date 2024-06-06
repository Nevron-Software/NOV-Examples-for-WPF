using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Grid Surface Empty data points example
	/// </summary>
	public class NGridSurfaceEmptyDataPointsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NGridSurfaceEmptyDataPointsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NGridSurfaceEmptyDataPointsExample()
		{
			NGridSurfaceEmptyDataPointsExampleSchema = NSchema.Create(typeof(NGridSurfaceEmptyDataPointsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Grid Surface With Empty Data Points";

            // set a chart title
            chartView.Surface.Titles[0].Text = "Surface With Empty Data Points";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 25.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup axes
            NOrdinalScale ordinalScaleX = new NOrdinalScale();
            ordinalScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScaleX.DisplayDataPointsBetweenTicks = false;

            NOrdinalScale ordinalScaleY = new NOrdinalScale();
            ordinalScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScaleY.DisplayDataPointsBetweenTicks = false;

            // add the surface series
            m_Surface = new NGridSurfaceSeries();
            chart.Series.Add(m_Surface);
            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FlatPositionValue = 10.0;
            m_Surface.Data.SetGridSize(40, 40);
            m_Surface.Palette = new NAxisTicksPalette();
            m_Surface.Palette.InterpolateColors = false;

            FillData(m_Surface);

            return chartViewWithCommandBars;
        }

        private void FillData(NGridSurfaceSeries surface)
        {
            double y, x, z;
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            const double dIntervalX = 8.0;
            const double dIntervalZ = 8.0;

            double dIncrementX = (dIntervalX / nCountX);
            double dIncrementZ = (dIntervalZ / nCountZ);

            z = -(dIntervalZ / 2);

            for (int j = 0; j < nCountZ; j++, z += dIncrementZ)
            {
                x = -(dIntervalX / 2);

                for (int i = 0; i < nCountX; i++, x += dIncrementX)
                {
                    y = Math.Log(Math.Abs(x) * Math.Abs(z));

                    if (y > -3)
                    {
                        surface.Data.SetValue(i, j, y);
                    }
                    else
                    {
                        surface.Data.SetValue(i, j, DBNull.Value);
                    }
                }
            }
        }

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox frameModeComboBox = new NComboBox();
			frameModeComboBox.FillFromEnum<ENSurfaceFrameMode>();
            frameModeComboBox.SelectedIndexChanged += OnFrameModeComboBoxSelectedIndexChanged;
            frameModeComboBox.SelectedIndex = (int)m_Surface.FrameMode;
            stack.Add(NPairBox.Create("Frame Mode:", frameModeComboBox));

			NComboBox frameColorModeComboBox = new NComboBox();
			frameColorModeComboBox.FillFromEnum<ENSurfaceFrameColorMode>();
            frameColorModeComboBox.SelectedIndexChanged += OnFrameColorModeComboBoxSelectedIndexChanged;
            frameColorModeComboBox.SelectedIndex = (int)m_Surface.FrameColorMode;
			stack.Add(NPairBox.Create("Frame Color Mode:", frameColorModeComboBox));

            return group;
		}

        private void OnFrameColorModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FrameColorMode = (ENSurfaceFrameColorMode)arg.NewValue;
        }

        private void OnFrameModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FrameMode = (ENSurfaceFrameMode)arg.NewValue;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the surface support for Empty Data Points.</p>";
		}

        #endregion

        #region Fields

        NGridSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceEmptyDataPointsExampleSchema;

		#endregion
	}
}