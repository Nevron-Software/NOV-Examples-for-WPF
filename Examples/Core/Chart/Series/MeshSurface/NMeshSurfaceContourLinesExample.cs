using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Mesh Surface Contour lines example
	/// </summary>
	public class NMeshSurfaceContourLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NMeshSurfaceContourLinesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NMeshSurfaceContourLinesExample()
		{
			NMeshSurfaceContourLinesExampleSchema = NSchema.Create(typeof(NMeshSurfaceContourLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Mesh Surface Contour Lines";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

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
            m_Surface.FrameMode = ENSurfaceFrameMode.None;
            m_Surface.FlatPositionValue = 0.5;
            m_Surface.Data.SetGridSize(20, 20);
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);

            m_RedIsoline = new NContourLine();
            m_RedIsoline.Value = 100;
            m_RedIsoline.Stroke = new NStroke(2.0f, NColor.Red);
            m_Surface.ContourLines.Add(m_RedIsoline);

            m_BlueIsoline = new NContourLine();
            m_BlueIsoline.Value = 50;
            m_BlueIsoline.Stroke = new NStroke(2.0f, NColor.Blue);
            m_Surface.ContourLines.Add(m_BlueIsoline);

            FillData(m_Surface);

            return chartViewWithCommandBars;
        }
        private void FillData(NMeshSurfaceSeries surface)
        {
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            const double dIntervalX = 20.0;
            const double dIntervalZ = 20.0;
            double dIncrementX = (dIntervalX / nCountX);
            double dIncrementZ = (dIntervalZ / nCountZ);

            double pz = -(dIntervalZ / 2);

            for (int j = 0; j < nCountZ; j++, pz += dIncrementZ)
            {
                double px = -(dIntervalX / 2);

                for (int i = 0; i < nCountX; i++, px += dIncrementX)
                {
                    double x = px + Math.Sin(pz) * 0.4;
                    double z = pz + Math.Cos(px) * 0.4;
                    double y = Math.Sin(px * 0.33) * Math.Sin(pz * 0.33) * 200;

                    if (y < 0)
                    {
                        y = -y * 0.7;
                    }

                    double tmp = (1 - x * x - z * z);
                    y -= tmp * tmp * 0.000001;

                    surface.Data.SetValue(i, j, y, x, z);
                }
            }
        }

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NNumericUpDown redIsolineValueUpDown = new NNumericUpDown();
            redIsolineValueUpDown.Value = m_RedIsoline.Value;
            redIsolineValueUpDown.ValueChanged += OnRedIsolineValueUpDownValueChanged;
            redIsolineValueUpDown.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Red Isoline Value:", redIsolineValueUpDown));

            NNumericUpDown blueIsolineValueUpDown = new NNumericUpDown();
            blueIsolineValueUpDown.Value = m_BlueIsoline.Value;
            blueIsolineValueUpDown.ValueChanged += OnBlueIsolineValueUpDownValueChanged;
            blueIsolineValueUpDown.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Blue Isoline Value:", blueIsolineValueUpDown));

            return group;
		}

        private void OnRedIsolineValueUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_RedIsoline.Value = (double)arg.NewValue;
        }

        private void OnBlueIsolineValueUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_BlueIsoline.Value = (double)arg.NewValue;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create contour lines (isolines) on a mesh surface chart.</p>";
		}

        #endregion

        #region Fields

        NMeshSurfaceSeries m_Surface;
        NContourLine m_RedIsoline;
        NContourLine m_BlueIsoline;

        #endregion

        #region Schema

        public static readonly NSchema NMeshSurfaceContourLinesExampleSchema;

		#endregion
	}
}