using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Triangulated Surface Projected Contour Example
	/// </summary>
	public class NTriangulatedSurfaceRealTimeExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NTriangulatedSurfaceRealTimeExample()
        {
            m_Random = new Random();
        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NTriangulatedSurfaceRealTimeExample()
        {
            NTriangulatedSurfaceRealTimeExampleSchema = NSchema.Create(typeof(NTriangulatedSurfaceRealTimeExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            chartView.Registered += OnChartViewRegistered;
            chartView.Unregistered += OnChartViewUnregistered;

            // configure title
            chartView.Surface.Titles[0].Text = "Realtime Grid Surface";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            chart.Enable3D = true;
            chart.ModelWidth = 50;
            chart.ModelDepth = 50;
            chart.ModelHeight = 30;
            chart.FitMode = ENCartesianChartFitMode.Aspect;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.OrthogonalTop);
            chart.Interactor = new NInteractor(new NTrackballTool());

            for (int i = 0; i < chart.Walls.GetChildrenCount(); i++)
            {
                ((NChartWall)chart.Walls.GetChildAt(i)).VisibilityMode = ENWallVisibilityMode.Hidden;
            }

            // setup Y axis
            NLinearScale scaleY = new NLinearScale();
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleY.MinTickDistance = 10;
            scaleY.MajorGridLines.ShowAtWalls = ENChartWall.Left | ENChartWall.Back;
            chart.Axes[ENCartesianAxis.PrimaryY].Scale = scaleY;

            // setup X axis
            NLinearScale scaleX = new NLinearScale();
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleX.MajorGridLines.ShowAtWalls = ENChartWall.Bottom | ENChartWall.Back;
            chart.Axes[ENCartesianAxis.PrimaryX].Scale= scaleX;

            // setup Z axis
            NLinearScale scaleZ = new NLinearScale();
            scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            scaleZ.MajorGridLines.ShowAtWalls = ENChartWall.Bottom | ENChartWall.Left;
            chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;

            // add the surface series
            NTriangulatedSurfaceSeries surface = new NTriangulatedSurfaceSeries();
            chart.Series.Add(surface);
            m_Surface = surface;
            surface.Name = "Surface";

            surface.FillMode = ENSurfaceFillMode.Zone;
            surface.FrameMode = ENSurfaceFrameMode.Mesh;
            surface.ShadingMode = ENShadingMode.Flat;

            return chartViewWithCommandBars;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox gridSizeComboBox = new NComboBox();
            gridSizeComboBox.Items.Add(new NComboBoxItem("10x10"));
            gridSizeComboBox.Items.Add(new NComboBoxItem("100x100"));
            gridSizeComboBox.Items.Add(new NComboBoxItem("200x200"));
            gridSizeComboBox.Items.Add(new NComboBoxItem("500x500"));
            stack.Add(NPairBox.Create("Grid Size:", gridSizeComboBox));
            gridSizeComboBox.SelectedIndexChanged += OnGridSizeComboBoxSelectedIndexChanged;
            gridSizeComboBox.SelectedIndex = 1;

            NCheckBox enableShaderRenderingCheckBox = new NCheckBox();
            stack.Add(NPairBox.Create("Enable Shader Rendering:", enableShaderRenderingCheckBox));
            enableShaderRenderingCheckBox.CheckedChanged += OnEnableShaderRenderingCheckBoxCheckedChanged;

            NButton toggleTimerButton = new NButton("Stop Timer");
            toggleTimerButton.Click += OnToggleTimerButtonClick;
            toggleTimerButton.Tag = 0;
            stack.Add(toggleTimerButton);

            return group;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates the ability of the NTriangulatedSurfaceSeries to render large dynamically changing datasets with a minimal amount of CPU load. The example also shows how to efficiently fill data to the series.</p>";
        }

        #endregion

        #region Events

        private void OnChartViewRegistered(NEventArgs arg)
        {
            m_Timer = new NTimer();
            m_Timer.Tick += OnTimerTick;
            m_Timer.Start();
        }

        private void OnChartViewUnregistered(NEventArgs arg)
        {
            m_Timer.Stop();
            m_Timer.Tick -= OnTimerTick;
            m_Timer = null;
        }

        private void OnTimerTick()
        {
            if (m_GridSize == 0)
                return;

            const double phaseStep = Math.PI / 10;

            unsafe
            {
                fixed (byte* pData = &m_Surface.Data.Data[0])
                {
                    float* pValues = (float*)pData;
                    int itemSize = m_Surface.Data.DataItemSize;
                    int count = m_Surface.Data.Count;

                    for (int x = 0; x < m_GridSize; x++)
                    {
                        for (int y = 0; y < m_GridSize; y++)
                        {
                            // The order of the values is x, y, z. In this case we dynamically modify only x and z.
                            pValues[0] = (float)(x + Math.Cos(m_Phase[x, y]) * m_Radius[x, y]);
                            pValues[2] = (float)(y + Math.Sin(m_Phase[x, y]) * m_Radius[x, y]);

                            m_Phase[x, y] += phaseStep;

                            pValues += itemSize;
                        }
                    }

                    m_Surface.Data.OnDataChanged();
                }
            }
        }


        private void OnEnableShaderRenderingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.EnableShaderRendering = (bool)arg.NewValue;
        }

        private void OnGridSizeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_GridSize = 100;

            switch (arg.NewValue)
            {
                case 0:
                    m_GridSize = 10;
                    break;
                case 1:
                    m_GridSize = 100;
                    break;
                case 2:
                    m_GridSize = 200;
                    break;
                case 3:
                    m_GridSize = 500;
                    break;
            }

            NTriangulatedSurfaceData data = m_Surface.Data;
            data.Clear();

            const float dIntervalX = 1.0f;
            const float dIntervalZ = 1.0f;

            float dIncrementX = (dIntervalX / m_GridSize);
            float dIncrementZ = (dIntervalZ / m_GridSize);

            m_Radius = new double[m_GridSize, m_GridSize];
            m_Phase = new double[m_GridSize, m_GridSize];

            Random random = new Random();

            float gridPhase = (float)(Math.PI * 5 / m_GridSize);

            for (int x = 0; x < m_GridSize; x++)
            {
                float zVar = -(dIntervalZ / 2) + x * dIncrementZ;

                for (int y = 0; y < m_GridSize; y++)
                {
                    float xVar = -(dIncrementX / 2) + y * dIncrementX;

                    m_Radius[x, y] = random.NextDouble();
                    m_Phase[x, y] = random.NextDouble() * Math.PI * 2;

                    float yPos = (float)(Math.Sin(y * gridPhase) + Math.Cos(x * gridPhase));
                    float xPos = (float)(x + Math.Cos(m_Phase[x, y]) * m_Radius[x, y]);
                    float zPos = (float)(y + Math.Sin(m_Phase[x, y]) * m_Radius[x, y]);

                    data.AddValue(new NVector3DF(xPos, yPos, zPos));
                }
            }
        }

        private void OnToggleTimerButtonClick(NEventArgs arg)
        {
            NButton button = (NButton)arg.TargetNode;
            if ((int)button.Tag == 0)
            {
                m_Timer.Stop();

                button.Content = new NLabel("Start Timer");
                button.Tag = 1;
            }
            else
            {
                m_Timer.Start();
                button.Content = new NLabel("Stop Timer");
                button.Tag = 0;
            }
        }

        #endregion

        #region Fields

        NTimer m_Timer;
        NTriangulatedSurfaceSeries m_Surface;
        int m_GridSize;
        Random m_Random;
        double[,] m_Phase;
        double[,] m_Radius;

        #endregion

        #region Schema

        public static readonly NSchema NTriangulatedSurfaceRealTimeExampleSchema;

        #endregion
    }
}