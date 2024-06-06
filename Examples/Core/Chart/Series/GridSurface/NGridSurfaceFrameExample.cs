using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Grid Frame Example
	/// </summary>
	public class NGridSurfaceFrameExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NGridSurfaceFrameExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NGridSurfaceFrameExample()
		{
			NGridSurfaceFrameExampleSchema = NSchema.Create(typeof(NGridSurfaceFrameExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Grid Surface Wireframe";

            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Interactor = new NInteractor(new NTrackballTool());
            m_Chart.Projection.SetPredefinedProjection(Nov.Graphics.ENPredefinedProjection.PerspectiveTilted);
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 60.0f;
            m_Chart.ModelDepth = 60.0f;
            m_Chart.ModelHeight = 25.0f;

            // setup axes
            NOrdinalScale ordinalScaleX = new NOrdinalScale();
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = ordinalScaleX;
            ordinalScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScaleX.DisplayDataPointsBetweenTicks = false;

            NOrdinalScale ordinalScaleY = new NOrdinalScale();
            m_Chart.Axes[ENCartesianAxis.Depth].Scale = ordinalScaleY;
            ordinalScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScaleY.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScaleY.DisplayDataPointsBetweenTicks = false;

            // add the surface series
            m_Surface = new NGridSurfaceSeries();
            m_Chart.Series.Add(m_Surface);
            m_Surface.Name = "Surface";
            m_Surface.FillMode = ENSurfaceFillMode.None;
            m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;
            m_Surface.Data.HasColor = true;
            m_Surface.Data.SetGridSize(30, 30);

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

            z = -(dIntervalZ / 2);

            float semiWidth = (float)Math.Min(nCountX / 2, nCountZ / 2);
            int centerX = nCountX / 2;
            int centerZ = nCountZ / 2;
            NColor startColor = NColor.Red;
            NColor endColor = NColor.Green;

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
                    float distance = (float)Math.Sqrt(dx * dx + dz * dz);
                    surface.Data.SetColor(i, j, InterpolateColors(startColor, endColor, distance / semiWidth));
                }
            }
        }

        public static NColor InterpolateColors(NColor color1, NColor color2, float factor)
        {
            if (factor > 1.0f)
                factor = 1.0f;
            else if (factor < 0.0)
                factor = 0.0f;

            int r1 = ((int)color1.R);
            int g1 = ((int)color1.G);
            int b1 = ((int)color1.B);

            int r2 = ((int)color2.R);
            int g2 = ((int)color2.G);
            int b2 = ((int)color2.B);

            byte num7 = (byte)((((float)r1) + (((float)(r2 - r1)) * factor)));
            byte num8 = (byte)((((float)g1) + (((float)(g2 - g1)) * factor)));
            byte num9 = (byte)((((float)b1) + (((float)(b2 - b1)) * factor)));

            return NColor.FromRGB(num7, num8, num9);
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

            if (m_Surface.FrameMode == ENSurfaceFrameMode.Dots)
            {
                m_Surface.Stroke = new NStroke(3, NColor.Black);
            }
            else
            {
                m_Surface.Stroke = new NStroke(1, NColor.Black);
            }
        }

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates different settings for the surface frame.</p>";
		}

        #endregion

        #region Fields

        NCartesianChart m_Chart;
        NGridSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceFrameExampleSchema;

		#endregion
	}
}