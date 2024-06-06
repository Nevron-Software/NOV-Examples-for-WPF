using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Mesh Surface Example
    /// </summary>
    public class NMeshSurfaceEmptyDataPointsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NMeshSurfaceEmptyDataPointsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NMeshSurfaceEmptyDataPointsExample()
		{
			NMeshSurfaceEmptyDataPointsExampleSchema = NSchema.Create(typeof(NMeshSurfaceEmptyDataPointsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Mesh Surface Chart";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart = chart;

            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 25.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.SoftTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            NLinearScale scale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            // setup axes
            NLinearScale scaleX = new NLinearScale();
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

            NLinearScale scaleZ = new NLinearScale();
            chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

            // add the surface series
            m_Surface = new NMeshSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FillMode = ENSurfaceFillMode.Zone;
            m_Surface.FlatPositionValue = 0.5;
            m_Surface.Data.SetGridSize(20, 20);
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);

            FillData(m_Surface);

            return chartViewWithCommandBars;
        }

        private void FillData(NMeshSurfaceSeries surface)
        {
            double x, y, z;
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            const double dIntervalX = 8.0;
            const double dIntervalZ = 8.0;
            double dIncrementX = (dIntervalX / nCountX);
            double dIncrementZ = (dIntervalZ / nCountZ);

            for (int j = 0; j < nCountZ; j++)
            {
                for (int i = 0; i < nCountX; i++)
                {
                    x = -(dIntervalX / 2) + (i * dIncrementX);
                    z = -(dIntervalZ / 2) + (j * dIncrementZ);

                    y = Math.Log(Math.Abs(x) * Math.Abs(z));

                    x += Math.Sin(j / 2.0) / 2.2;
                    z += Math.Cos(i / 2.0) / 2.2;

                    if (y > -7)
                    {
                        surface.Data.SetValue(i, j, y, x, z);
                    }
                    else
                    {
                        surface.Data.SetValue(i, j, DBNull.Value, DBNull.Value, DBNull.Value);
                    }
                }
            }
        }

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NCheckBox smoothShadingCheckBox = new NCheckBox();
            smoothShadingCheckBox.CheckedChanged += OnSmoothShadingCheckBoxCheckedChanged;
            smoothShadingCheckBox.Checked = false;
            smoothShadingCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Smooth Shading:", smoothShadingCheckBox));

            return group;
		}

        private void OnSmoothShadingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.ShadingMode = (bool)arg.NewValue ? ENShadingMode.Smooth : ENShadingMode.Flat;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the mesh surface support for Empty Data Points.</p>";
		}

        #endregion

        #region Fields

        NChart m_Chart;
        NMeshSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NMeshSurfaceEmptyDataPointsExampleSchema;

		#endregion
	}
}