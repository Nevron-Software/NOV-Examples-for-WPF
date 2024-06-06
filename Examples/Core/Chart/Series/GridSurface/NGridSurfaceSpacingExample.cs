using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Grid Surface Projected Contour Example
    /// </summary>
    public class NGridSurfaceSpacingExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NGridSurfaceSpacingExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NGridSurfaceSpacingExample()
        {
            NGridSurfaceSpacingExampleSchema = NSchema.Create(typeof(NGridSurfaceSpacingExample), NExampleBaseSchema);
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

            // configure title
            chartView.Surface.Titles[0].Text = "Grid Surface Grid Spacing";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.Enable3D = true;
            chart.ModelWidth = 55.0f;
            chart.ModelDepth = 55.0f;
            chart.ModelHeight = 45.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.SoftCameraLight);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // add a surface series
            m_Surface = new NGridSurfaceSeries();
            chart.Series.Add(m_Surface);
            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.None;
            m_Surface.Fill = new NColorFill(NColor.FromRGB(160, 170, 212));
            m_Surface.FillMode = ENSurfaceFillMode.Uniform;
            m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;
            m_Surface.CellTriangulationMode = ENSurfaceCellTriangulationMode.Diagonal1;
            m_Surface.Data.SetGridSize(500, 500);

            // setup axes
            NLinearScale scaleX = new NLinearScale();
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;
            scaleX.MajorGridLines.ShowAtWalls = ENChartWall.Bottom |ENChartWall.Back;
            scaleX.InflateViewRangeBegin = false;
            scaleX.InflateViewRangeEnd = false;

            NLinearScale scaleZ = new NLinearScale();
            chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;
            scaleZ.MajorGridLines.ShowAtWalls = ENChartWall.Bottom | ENChartWall.Left;
            scaleZ.InflateViewRangeBegin = false;
            scaleZ.InflateViewRangeEnd = false;

            // specify that the surface should use custom X and Z values
            m_Surface.XValuesMode = ENGridSurfaceValuesMode.CustomValues;
            m_Surface.ZValuesMode = ENGridSurfaceValuesMode.CustomValues;

            m_Surface.Data.SetGridSize(40, 40);

            GenerateCustomXValues(m_Surface);
            GenerateCustomZValues(m_Surface);
            FillData(m_Surface);

            return chartViewWithCommandBars;
        }

        private void GenerateCustomXValues(NGridSurfaceSeries surface)
        {
            int sizeX = surface.Data.GridSizeX;

            double x = 0;
            double[] xValues = new double[sizeX];
            Random random = new Random();

            for (int i = 0; i < sizeX; i++)
            {
                xValues[i] = x;
                x += random.NextDouble() * 10.0;
            }

            surface.XValues = new NDomArray<double>(xValues);
        }

        private void GenerateCustomZValues(NGridSurfaceSeries surface)
        {
            int sizeZ = surface.Data.GridSizeZ;

            double[] zValues = new double[sizeZ];
            double z = 0;
            Random random = new Random();

            for (int i = 0; i < sizeZ; i++)
            {
                zValues[i] = z;
                z += random.NextDouble() * 10;
            }

            surface.ZValues = new NDomArray<double>(zValues);
        }

        private void FillData(NGridSurfaceSeries surface)
        {
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            double sizeXOrigin = -2.3;
            double sizeXScale = 4.6 / nCountX;


            for (int z = 0; z < nCountZ; z++)
            {
                for (int x = 0; x < nCountX; x++)
                {
                    double xVal = (x * sizeXScale) + sizeXOrigin;
                    double yVal = (z * sizeXScale) + sizeXOrigin;

                    double zVal = xVal * Math.Exp(-(xVal * xVal + yVal * yVal));

                    surface.Data.SetValue(x, z, zVal);
                }
            }
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox spacingComboBox = new NComboBox();
            spacingComboBox.FillFromEnum<ENGridSurfaceValuesMode>();
            stack.Add(NPairBox.Create("Spacing Mode:", spacingComboBox));

            m_XGridOriginNumericUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("X Origin:", m_XGridOriginNumericUpDown));
            m_XGridOriginNumericUpDown.ValueChanged += OnXOriginNumericUpDownValueChanged;

            m_XGridStepNumericUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("X Grid Step:", m_XGridStepNumericUpDown));
            m_XGridStepNumericUpDown.ValueChanged += OnXGridStepNumericUpDownValueChanged;

            m_ZGridOriginNumericUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("Z Origin:", m_ZGridOriginNumericUpDown));
            m_ZGridOriginNumericUpDown.ValueChanged += OnZOriginNumericUpDownValueChanged;

            m_ZGridStepNumericUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("Z Grid Step:", m_ZGridStepNumericUpDown));
            m_ZGridStepNumericUpDown.ValueChanged += OnZGridStepNumericUpDownValueChanged;

            spacingComboBox.SelectedIndexChanged += OnSpacingModeComboBoxSelectedIndexChanged;
            spacingComboBox.SelectedIndex = 1;

            m_XGridOriginNumericUpDown.Value = 0;
            m_ZGridOriginNumericUpDown.Value = 0;
            m_XGridStepNumericUpDown.Value = 1;
            m_ZGridStepNumericUpDown.Value = 1;

            return group;
        }

        private void OnXGridStepNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Surface.StepX = (double)arg.NewValue;
        }

        private void OnXOriginNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Surface.OriginX = (double)arg.NewValue;
        }

        private void OnZOriginNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Surface.OriginZ = (double)arg.NewValue;
        }

        private void OnZGridStepNumericUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_Surface.StepZ = (double)arg.NewValue;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to control the spacing between the grid cells in a grid surface series.</p>";
        }

        #endregion

        #region Events

        private void OnEnableShaderRenderingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.EnableShaderRendering = (bool)arg.NewValue;
        }

        private void OnSpacingModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            ENGridSurfaceValuesMode valuesMode = (ENGridSurfaceValuesMode)arg.NewValue;
            m_Surface.XValuesMode = valuesMode;
            m_Surface.ZValuesMode = valuesMode;

            bool originAndStepMode = valuesMode == ENGridSurfaceValuesMode.OriginAndStep;

            m_XGridOriginNumericUpDown.Enabled = originAndStepMode;
            m_XGridStepNumericUpDown.Enabled = originAndStepMode;
            m_ZGridOriginNumericUpDown.Enabled = originAndStepMode;
            m_ZGridStepNumericUpDown.Enabled = originAndStepMode;
        }

        #endregion

        #region Fields

        NGridSurfaceSeries m_Surface;
        NNumericUpDown m_XGridOriginNumericUpDown;
        NNumericUpDown m_XGridStepNumericUpDown;
        NNumericUpDown m_ZGridOriginNumericUpDown;
        NNumericUpDown m_ZGridStepNumericUpDown;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceSpacingExampleSchema;

        #endregion
    }
}