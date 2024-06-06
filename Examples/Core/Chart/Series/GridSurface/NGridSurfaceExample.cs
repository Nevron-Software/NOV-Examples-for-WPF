using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Grid Surface Example
	/// </summary>
	public class NGridSurfaceExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NGridSurfaceExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NGridSurfaceExample()
		{
			NGridSurfaceExampleSchema = NSchema.Create(typeof(NGridSurfaceExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Grid Surface Chart";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 25.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            NLinearScale scale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            // setup axes
            NOrdinalScale  ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.Depth].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            // add the surface series
            m_Surface = new NGridSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FlatPositionValue = 10.0;
            m_Surface.Data.HasColor = true;
            m_Surface.Data.SetGridSize(100, 100);
            m_Surface.Palette.InterpolateColors = false;

            m_Surface.ValueFormatter = new NNumericValueFormatter("0.00");
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);

            m_Surface.FillMode = ENSurfaceFillMode.Zone;
            m_Surface.FrameMode = ENSurfaceFrameMode.Contour;
            m_Surface.FrameColorMode = ENSurfaceFrameColorMode.Uniform;

            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            chartView.Surface.Legends[0].Visibility = ENVisibility.Visible;

            FillData(m_Surface);

            return chartViewWithCommandBars;
        }

        private void FillData(NGridSurfaceSeries surface)
        {
            double y, x, z;
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            const double dIntervalX = 30.0;
            const double dIntervalZ = 30.0;
            double dIncrementX = (dIntervalX / nCountX);
            double dIncrementZ = (dIntervalZ / nCountZ);

            float semiWidth = (float)Math.Min(nCountX / 2, nCountZ / 2);

            NColor startColor = NColor.Red;
            NColor endColor = NColor.Green;

            int centerX = nCountX / 2;
            int centerZ = nCountZ / 2;

            z = -(dIntervalZ / 2);

            for (int j = 0; j < nCountZ; j++, z += dIncrementZ)
            {
                x = -(dIntervalX / 2);

                for (int i = 0; i < nCountX; i++, x += dIncrementX)
                {
                    y = (x * z / 64.0) - Math.Sin(z / 2.4) * Math.Cos(x / 2.4);
                    y = 10 * Math.Sqrt(Math.Abs(y));

                    if (y <= 0)
                    {
                        y = 1 + Math.Cos(x / 2.4);
                    }

                    surface.Data.SetValue(i, j, y);

                    int dx = centerX - i;
                    int dz = centerZ - j;
                    double factor = (float)Math.Sqrt(dx * dx + dz * dz) / semiWidth;

                    if (factor > 1)
                    {
                        factor = 1;
                    }

                    surface.Data.SetColor(i, j, NColor.InterpolateColors(startColor, endColor, factor));
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

            if (m_Surface.FrameMode == ENSurfaceFrameMode.Dots)
            {
                m_Surface.Stroke = new NStroke(3, NColor.Black);
            }
            else
            {
                m_Surface.Stroke = new NStroke(1, NColor.Black);
            }
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

        NGridSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceExampleSchema;

		#endregion
	}
}