using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Grid Surface Horizontal Cross Section Example
    /// </summary>
    public class NGridSurfaceHorizontalCrossSectionExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NGridSurfaceHorizontalCrossSectionExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NGridSurfaceHorizontalCrossSectionExample()
        {
            NGridSurfaceHorizontalCrossSectionExampleSchema = NSchema.Create(typeof(NGridSurfaceHorizontalCrossSectionExample), NExampleBaseSchema);
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
            chartView.Surface.Titles[0].Text = "Grid Surface Series Horizontal Cross Section";

            // Configure surface chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 15.0f;
            chart.FitMode = ENCartesianChartFitMode.Aspect;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.Projection.Elevation = 22;
            chart.Projection.Rotation = -68;
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyTopLeft);

            // setup axes
            NOrdinalScale ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.Depth].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            chart.Axes[ENCartesianAxis.PrimaryY].SetFixedViewRange(new NRange(0, 15.001));

            // add the surface series
            m_SurfaceSeries = new NGridSurfaceSeries();
            chart.Series.Add(m_SurfaceSeries);
            m_SurfaceSeries.Name = "Surface";
            m_SurfaceSeries.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_SurfaceSeries.FlatPositionValue = 10.0;
            m_SurfaceSeries.Data.SetGridSize(1000, 1000);
            m_SurfaceSeries.Fill = new NColorFill(NColor.YellowGreen);
            m_SurfaceSeries.ShadingMode = ENShadingMode.Smooth;
            m_SurfaceSeries.FrameMode = ENSurfaceFrameMode.None;
            m_SurfaceSeries.FillMode = ENSurfaceFillMode.Zone;
            m_SurfaceSeries.ContourLines.Add(new NContourLine(10, new NStroke(2.0f, NColor.Blue)));

            m_ContourLineSeries = new NLineSeries();
            chart.Series.Add(m_ContourLineSeries);

            m_ContourLineSeries.UseXValues = true;
            m_ContourLineSeries.UseZValues = true;
            m_ContourLineSeries.Stroke = new NStroke(2, NColor.Red);

            m_ContourLineSeries.DataLabelStyle = new NDataLabelStyle(false);
            m_ContourLineSeries.LegendView.Mode = ENSeriesLegendMode.None;

            m_CrossSectionPlane = new NAxisReferenceLine();
            m_CrossSectionPlane.DisplayMode = ENReferenceLineDisplayMode.Plane;
            m_CrossSectionPlane.Fill = new NColorFill(NColor.FromARGB(25, 0, 0, 255));
            chart.Axes[ENCartesianAxis.PrimaryY].ReferenceLines.Add(m_CrossSectionPlane);

            FillData(m_SurfaceSeries);

            // make sure min / max for all axes is calculated
            chartView.Document.Evaluate();

            chart.Interactor = new NInteractor(new NTool[] { new NTrackballTool() });

            return chartViewWithCommandBars;
        }

        private void FillData(NGridSurfaceSeries surface)
        {
            double y, x, z;
            int nCountX = surface.Data.GridSizeX;
            int nCountZ = surface.Data.GridSizeZ;

            const double dIntervalX = 10.0;
            const double dIntervalZ = 10.0;
            double dIncrementX = (dIntervalX / nCountX);
            double dIncrementZ = (dIntervalZ / nCountZ);

            z = -(dIntervalZ / 2);

            for (int j = 0; j < nCountZ; j++, z += dIncrementZ)
            {
                x = -(dIntervalX / 2);

                for (int i = 0; i < nCountX; i++, x += dIncrementX)
                {
                    y = 10 - Math.Sqrt((x * x) + (z * z) + 2);
                    y += 3.0 * Math.Sin(x) * Math.Cos(z);

                    surface.Data.SetValue(i, j, y);
                }
            }
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NNumericUpDown horizontalPlaneValueUpDown = new NNumericUpDown();
            stack.Add(NPairBox.Create("Horizontal Plane Value:", horizontalPlaneValueUpDown));
            horizontalPlaneValueUpDown.ValueChanged += OnHorizontalPlaneValueValueChanged;
            horizontalPlaneValueUpDown.Value = 10;
            horizontalPlaneValueUpDown.Minimum = 0;
            horizontalPlaneValueUpDown.Maximum = m_ContourYValue;

            return group;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>This example demonstrates how to get the cross section of a grid surface at specified Y value. Change the horizontal plane value on the right. The cross section will be displayed both as an isoline and projected contour at the top of the chart. </p>";
        }

        #endregion

        #region Events

        private void OnHorizontalPlaneValueValueChanged(NValueChangeEventArgs arg)
        {
            double value = (double)arg.NewValue;

            if (m_SurfaceSeries.ContourLines.Count > 0)
            {
                m_SurfaceSeries.ContourLines[0].Value = value;
            }

            m_CrossSectionPlane.Value = value;

            NLevelPath path = m_SurfaceSeries.GetContourForValue(value);

            m_ContourLineSeries.DataPoints.Clear();

            for (int i = 0; i < path.Count; i++)
            {
                NLevelContour contour = path[i];

                if (contour.Count > 0)
                {
                    int index = m_ContourLineSeries.DataPoints.Count + 1;
                    int pointCount = contour.Count;
                    for (int j = 0; j < pointCount; j++)
                    {
                        NPoint point = contour[j];
                        m_ContourLineSeries.DataPoints.Add(new NLineDataPoint(point.X, m_ContourYValue, point.Y)); 
                    }

                    m_ContourLineSeries.DataPoints.Add(new NLineDataPoint(double.NaN, double.NaN, double.NaN));
                }
            }
        }

        #endregion

        #region Fields

        NGridSurfaceSeries m_SurfaceSeries;
        NLineSeries m_ContourLineSeries;
        NAxisReferenceLine m_CrossSectionPlane;
        const double m_ContourYValue = 15;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceHorizontalCrossSectionExampleSchema;

        #endregion
    }
}