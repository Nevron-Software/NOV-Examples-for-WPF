using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Grid Surface Projected Contour Example
	/// </summary>
	public class NGridSurfaceRealTimeExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NGridSurfaceRealTimeExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NGridSurfaceRealTimeExample()
        {
            NGridSurfaceRealTimeExampleSchema = NSchema.Create(typeof(NGridSurfaceRealTimeExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
            chart.Enable3D = true;
            chart.ModelWidth = 55.0f;
            chart.ModelDepth = 55.0f;
            chart.ModelHeight = 45.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // add a surface series
            m_Surface = new NGridSurfaceSeries();
            chart.Series.Add(m_Surface);
            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.None;
            m_Surface.Fill = new NColorFill(NColor.FromRGB(160, 170, 212));
            m_Surface.FillMode = ENSurfaceFillMode.Uniform;
            m_Surface.FrameMode = ENSurfaceFrameMode.None;
            m_Surface.CellTriangulationMode = ENSurfaceCellTriangulationMode.Diagonal1;
            m_Surface.Data.SetGridSize(500, 500);
            SetupCommonSurfaceProperties(m_Surface);

            // setup axes
            chart.Axes[ENCartesianAxis.PrimaryY].ViewRangeMode = ENAxisViewRangeMode.FixedRange;
            chart.Axes[ENCartesianAxis.PrimaryY].MinViewRangeValue = -3;
            chart.Axes[ENCartesianAxis.PrimaryY].MaxViewRangeValue = 3;

            NOrdinalScale xScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
            xScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            xScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            xScale.DisplayDataPointsBetweenTicks = false;

            NOrdinalScale zScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.Depth].Scale;
            zScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            zScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            zScale.DisplayDataPointsBetweenTicks = false;

            return chartViewWithCommandBars;
        }

        double phase = 0;
        Random random = new Random();

        private void GenerateWaves()
        {
            NGridSurfaceSeries surface = m_Surface;

            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            double step = Math.PI * 10 / nCountX;

            unsafe
            {
                fixed (byte* pData = &surface.Data.Data[0])
                {
                    float* pValue = (float*)pData + 1;
                    int itemSize = surface.Data.DataItemSize;

                    double randomAmplitude = Math.Sin(phase) * 2 + random.NextDouble();

                    for (int z = 0; z < nCountZ; z++)
                    {
                        for (int x = 0; x < nCountX; x++)
                        {
                            float zVal = (float)(randomAmplitude * Math.Sin(phase + x * step) * Math.Cos(phase + z * step));

                            *pValue = zVal;

                            pValue += itemSize;
                        }
                    }

                    surface.Data.OnDataChanged();
                }
            }

            phase += 3 * step;
        }

        private void SetupCommonSurfaceProperties(NGridSurfaceSeries surface)
        {
            surface.Palette = new NRangeMultiColorPalette();
            surface.XValuesMode = ENGridSurfaceValuesMode.OriginAndStep;
            surface.OriginX = -150;
            surface.StepX = 10;
            surface.ZValuesMode = ENGridSurfaceValuesMode.OriginAndStep;
            surface.OriginZ = -150;
            surface.StepZ = 10;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox gridSizeComboBox = new NComboBox();
            gridSizeComboBox.Items.Add(new NComboBoxItem("250x250"));
            gridSizeComboBox.Items.Add(new NComboBoxItem("500x500"));
            gridSizeComboBox.Items.Add(new NComboBoxItem("1000x1000"));
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
            return @"<p>This example demonstrates the ability of the NGridSurfaceSeries to render large dynamically changing datasets with a minimal amount of CPU load. The example also shows how to efficiently fill data to the series.</p>";
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
            GenerateWaves();
        }

        private void OnEnableShaderRenderingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.EnableShaderRendering = (bool)arg.NewValue;
        }

        private void OnGridSizeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            int gridSize = 0;
            switch ((int)arg.NewValue)
            {
                case 0:
                    gridSize = 250;
                    break;
                case 1:
                    gridSize = 500;
                    break;
                case 2:
                    gridSize = 1000;
                    break;
            }

            m_Surface.Data.SetSize(gridSize, gridSize);
            GenerateWaves();
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
        NGridSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceRealTimeExampleSchema;

        #endregion
    }
}