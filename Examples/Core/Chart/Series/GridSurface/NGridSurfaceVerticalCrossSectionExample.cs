using System;
using System.IO;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.Layout;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Grid Surface Vertical Cross Section Example
	/// </summary>
	public class NGridSurfaceVerticalCrossSectionExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NGridSurfaceVerticalCrossSectionExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NGridSurfaceVerticalCrossSectionExample()
        {
            NGridSurfaceVerticalCrossSectionExampleSchema = NSchema.Create(typeof(NGridSurfaceVerticalCrossSectionExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;

			// configure title
			chartView.Surface.Titles[0].Text = "Grid Surface Series Vertical Cross Section";

            NDockPanel dockPanel = new NDockPanel();
            chartView.Surface.Content = dockPanel;

            NLabel label = new NLabel();
            label.Margins = new NMargins(10, 10, 10, 10);
            label.Font = new NFont(NFontDescriptor.DefaultSansFamilyName, 12);
            label.TextFill = new NColorFill(NColor.Black);
            label.TextAlignment = ENContentAlignment.MiddleCenter;
            NDockLayout.SetDockArea(label, ENDockArea.Top);
            dockPanel.AddChild(label);

            // configure the chart
            NCartesianChart crossSectionChart = new NCartesianChart();
            crossSectionChart.PreferredHeight = 200;
            crossSectionChart.Margins = new NMargins(10, 0, 0, 10);
            NDockLayout.SetDockArea(crossSectionChart, ENDockArea.Bottom);

            crossSectionChart.FitMode = ENCartesianChartFitMode.Stretch;
            crossSectionChart.Margins = new NMargins(0, 10, 0, 0);

            crossSectionChart.Axes[ENCartesianAxis.PrimaryX].Scale = new NLinearScale();
            crossSectionChart.Axes[ENCartesianAxis.PrimaryX].Scale.Title.Text = "Distance";
            crossSectionChart.Axes[ENCartesianAxis.PrimaryY].Scale.Title.Text = "Value";

            m_CrossSection2DSeries = new NLineSeries();
            m_CrossSection2DSeries.DataLabelStyle = new NDataLabelStyle(false);
            m_CrossSection2DSeries.UseXValues = true;

            crossSectionChart.Series.Add(m_CrossSection2DSeries);

            dockPanel.AddChild(crossSectionChart);

            // Configure surface chart
            NCartesianChart surfaceChart = new NCartesianChart();
            dockPanel.AddChild(surfaceChart);
            surfaceChart.Margins = new NMargins(10, 0, 0, 10);
            NDockLayout.SetDockArea(surfaceChart, ENDockArea.Center);

            surfaceChart.Enable3D = true;
            surfaceChart.ModelWidth = 60.0f;
            surfaceChart.ModelDepth = 60.0f;
            surfaceChart.ModelHeight = 15.0f;
            surfaceChart.FitMode = ENCartesianChartFitMode.Aspect;
            surfaceChart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            surfaceChart.Projection.Elevation = 22;
            surfaceChart.Projection.Rotation = -68;
            surfaceChart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyTopLeft);

            // setup axes
            NOrdinalScale ordinalScale = (NOrdinalScale)surfaceChart.Axes[ENCartesianAxis.PrimaryX].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            ordinalScale = (NOrdinalScale)surfaceChart.Axes[ENCartesianAxis.Depth].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            // add the surface series
            m_SurfaceSeries = new NGridSurfaceSeries();
            surfaceChart.Series.Add(m_SurfaceSeries);
            m_SurfaceSeries.Name = "Surface";
            m_SurfaceSeries.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_SurfaceSeries.FlatPositionValue = 10.0;
            m_SurfaceSeries.Data.SetGridSize(1000, 1000);
            m_SurfaceSeries.Fill = new NColorFill(NColor.YellowGreen);
            m_SurfaceSeries.ShadingMode = ENShadingMode.Smooth;
            m_SurfaceSeries.FrameMode = ENSurfaceFrameMode.None;
            m_SurfaceSeries.FillMode = ENSurfaceFillMode.Zone;

            // add the cross section line series
            m_CrossSection3DSeries = new NPointSeries();
            surfaceChart.Series.Add(m_CrossSection3DSeries);
            m_CrossSection3DSeries.Size = 10;
            m_CrossSection3DSeries.Shape = ENPointShape3D.Sphere;
            m_CrossSection3DSeries.Fill = new NColorFill(NColor.Red);
            m_CrossSection3DSeries.DataLabelStyle = new NDataLabelStyle(false);
            m_CrossSection3DSeries.UseXValues = true;
            m_CrossSection3DSeries.UseZValues = true;

            FillData(m_SurfaceSeries);

            NRange xAxesRange = new NRange(0, m_SurfaceSeries.Data.GridSizeX);
            NRange yAxesRange = new NRange(0, 400);
            NRange zAxesRange = new NRange(0, m_SurfaceSeries.Data.GridSizeZ);

            m_DragPlane = new NDragPlane(surfaceChart,
                        new NVector3DD(0, yAxesRange.End, 0),
                        new NVector3DD(xAxesRange.End, yAxesRange.End, zAxesRange.End),
                        new NVector3DD(xAxesRange.End, 0, zAxesRange.End),
                        new NVector3DD(0, 0, 0));

            m_DragPlane.DragPlaneChanged += OnDragPlaneDragPlaneChanged;

            m_DragPlaneTool = new NDragPlaneTool(m_DragPlane);
            surfaceChart.Interactor = new NInteractor(new NTool[] { m_DragPlaneTool, new NTrackballTool() });

            // enable fixed Axes ranges
            surfaceChart.Axes[ENCartesianAxis.PrimaryX].SetFixedViewRange(xAxesRange);
            surfaceChart.Axes[ENCartesianAxis.PrimaryY].SetFixedViewRange(yAxesRange);
            surfaceChart.Axes[ENCartesianAxis.Depth].SetFixedViewRange(zAxesRange);

            // turn off plot box clipping
            surfaceChart.Axes[ENCartesianAxis.PrimaryX].ClipMode = ENAxisClipMode.Never;
            surfaceChart.Axes[ENCartesianAxis.PrimaryY].ClipMode = ENAxisClipMode.Never;
            surfaceChart.Axes[ENCartesianAxis.Depth].ClipMode = ENAxisClipMode.Never;

            OnDragPlaneDragPlaneChanged(null, null);

            return chartViewWithCommandBars;
        }

        private void FillData(NGridSurfaceSeries surface)
        {
            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                // fill the XYZ data from a binary resource
                stream = new MemoryStream(NResources.RBIN_SampleData_DataY_bin.Data);
                reader = new BinaryReader(stream);

                int dataPointsCount = (int)(stream.Length / 4);
                int sizeX = (int)Math.Sqrt(dataPointsCount);
                int sizeZ = sizeX;

                surface.Data.SetGridSize(sizeX, sizeZ);

                for (int z = 0; z < sizeZ; z++)
                {
                    for (int x = 0; x < sizeX; x++)
                    {
                        surface.Data.SetValue(x, z, reader.ReadSingle());
                    }
                }
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (stream != null)
                    stream.Close();
            }
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            m_DragPlaneComboBox = new NComboBox();
            stack.Add(NPairBox.Create("Drag Plane:", m_DragPlaneComboBox));

            m_DragPlaneComboBox.FillFromEnum<DragPlaneSurface>();
            m_DragPlaneComboBox.SelectedIndexChanged += OnDragPlaneComboBoxSelectedIndexChanged;
            m_DragPlaneComboBox.SelectedIndex = (int)DragPlaneSurface.XZ;

            return group;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates how to get the cross section of a grid surface given begin and end point on the XY grid. Press the mouse over the red or blue surface end points and begin to drag. On the right side you can control the plane in which the point dragging occurs.</p>";
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDragPlaneDragPlaneChanged(object sender, EventArgs e)
        {
            NList<NVector3DD> intersections3D = m_SurfaceSeries.Get3DIntersections(new NPoint(m_DragPlane.PointA.X, m_DragPlane.PointA.Z), new NPoint(m_DragPlane.PointB.X, m_DragPlane.PointB.Z));
            NList<NVector2DD> intersections2D = m_SurfaceSeries.Get2DIntersections(new NPoint(m_DragPlane.PointA.X, m_DragPlane.PointA.Z), new NPoint(m_DragPlane.PointB.X, m_DragPlane.PointB.Z));

            m_CrossSection3DSeries.DataPoints.Clear();

            for (int i = 0; i < intersections3D.Count; i++)
            {
                NVector3DD intersection3D = intersections3D[i];

                m_CrossSection3DSeries.DataPoints.Add(new NPointDataPoint(intersection3D.X, intersection3D.Z + 1, intersection3D.Y));
            }

            m_CrossSection2DSeries.DataPoints.Clear();

            for (int i = 0; i < intersections2D.Count; i++)
            {
                NVector2DD intersection2D = intersections2D[i];

                m_CrossSection2DSeries.DataPoints.Add(new NLineDataPoint(intersection2D.X, intersection2D.Y));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnDragPlaneComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_DragPlaneTool.DragPlaneSurface = (DragPlaneSurface)arg.NewValue;
        }

        #endregion

        #region Fields

        NDragPlane m_DragPlane;
        NPointSeries m_CrossSection3DSeries;
        NLineSeries m_CrossSection2DSeries;
        NGridSurfaceSeries m_SurfaceSeries;
        NDragPlaneTool m_DragPlaneTool;

        NComboBox m_DragPlaneComboBox;

        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceVerticalCrossSectionExampleSchema;

        #endregion
    }
}