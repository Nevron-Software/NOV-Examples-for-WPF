using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Mesh Surface Example
	/// </summary>
	public class NMeshSurfaceExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NMeshSurfaceExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NMeshSurfaceExample()
		{
			NMeshSurfaceExampleSchema = NSchema.Create(typeof(NMeshSurfaceExample), NExampleBaseSchema);
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

            for (int j = 0; j < nCountZ; j++)
            {
                for (int i = 0; i < nCountX; i++)
                {
                    x = 2 + i + Math.Sin(j / 4.0) * 2;
                    z = 1 + j + Math.Cos(i / 4.0);

                    y = Math.Sin(i / 3.0) * Math.Sin(j / 3.0);

                    if (y < 0)
                    {
                        y = Math.Abs(y / 2.0);
                    }

                    surface.Data.SetValue(i, j, y, x, z);
                }
            }
        }

        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox fillModeCombo = new NComboBox();
			fillModeCombo.FillFromEnum<ENSurfaceFillMode>();
            fillModeCombo.SelectedIndexChanged += OnFillModeComboSelectedIndexChanged;
            fillModeCombo.SelectedIndex = (int)m_Surface.FillMode;
            stack.Add(NPairBox.Create("Fill Mode:", fillModeCombo));

			NComboBox frameModeCombo = new NComboBox();
			frameModeCombo.FillFromEnum<ENSurfaceFrameMode>();
            frameModeCombo.SelectedIndexChanged += OnFrameModeComboSelectedIndexChanged;
            frameModeCombo.SelectedIndex = (int)m_Surface.FrameMode;
            stack.Add(NPairBox.Create("Frame Mode:", frameModeCombo));

            NComboBox frameColorModeCombo = new NComboBox();
            frameColorModeCombo.FillFromEnum<ENSurfaceFrameColorMode>();
            frameColorModeCombo.SelectedIndexChanged += OnFrameColorModeComboSelectedIndexChanged;
            frameColorModeCombo.SelectedIndex = (int)m_Surface.FrameColorMode;
            stack.Add(NPairBox.Create("Frame Color Mode:", frameColorModeCombo));

            NCheckBox smoothShadingCheckBox = new NCheckBox();
            smoothShadingCheckBox.CheckedChanged += OnSmoothShadingCheckBoxCheckedChanged;
            smoothShadingCheckBox.Checked = false;
            smoothShadingCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Smooth Shading:", smoothShadingCheckBox));

            NCheckBox drawFlatCheckBox = new NCheckBox();
            drawFlatCheckBox.CheckedChanged += OnDrawFlatCheckBoxCheckedChanged;
            drawFlatCheckBox.Checked = false;
            drawFlatCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Draw Flat:", drawFlatCheckBox));

            return group;
		}

        private void OnDrawFlatCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.DrawFlat = (bool)arg.NewValue;
        }

        private void OnFrameColorModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FrameColorMode = (ENSurfaceFrameColorMode)arg.NewValue;
        }

        private void OnSmoothShadingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.ShadingMode = (bool)arg.NewValue ? ENShadingMode.Smooth : ENShadingMode.Flat;
        }

        private void OnFrameModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FrameMode = (ENSurfaceFrameMode)arg.NewValue;
        }

        private void OnFillModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FillMode = (ENSurfaceFillMode)arg.NewValue;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a scatter funnel chart.</p>";
		}

        #endregion

        #region Fields

        NMeshSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NMeshSurfaceExampleSchema;

		#endregion
	}
}