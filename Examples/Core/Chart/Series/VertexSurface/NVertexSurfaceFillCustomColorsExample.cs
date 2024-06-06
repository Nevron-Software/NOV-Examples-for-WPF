using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Vertex Surface Example
	/// </summary>
	public class NVertexSurfaceFillCustomColorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NVertexSurfaceFillCustomColorsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NVertexSurfaceFillCustomColorsExample()
        {
            NVertexSurfaceFillCustomColorsExampleSchema = NSchema.Create(typeof(NVertexSurfaceFillCustomColorsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Vertex Surface Series Fill With Custom Colors";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 50.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.SoftTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup axes
            NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            NLinearScale scaleX = new NLinearScale();
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;

            NLinearScale scaleZ = new NLinearScale();
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;

            // add the surface series
            m_Surface = new NVertexSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FlatPositionValue = 10.0;
            m_Surface.Palette.InterpolateColors = false;
            m_Surface.ValueFormatter = new NNumericValueFormatter("0.00");
            m_Surface.FillMode = ENSurfaceFillMode.CustomColors;
            m_Surface.FrameMode = ENSurfaceFrameMode.None;
            m_Surface.VertexPrimitive = ENVertexPrimitive.Triangles;
            m_Surface.Data.HasColor = true;
            m_Surface.UseIndices = true;

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox cubeSizeComboBox = new NComboBox();
            for (int i = 1; i < 10; i++)
            {
                cubeSizeComboBox.Items.Add(new NComboBoxItem(i.ToString() + "x" + i.ToString()));
            }

            cubeSizeComboBox.SelectedIndexChanged += OnCubeSizeComboBoxSelectedIndexChanged;
            cubeSizeComboBox.SelectedIndex = 4;
            stack.Add(NPairBox.Create("Vertex Primitive:", cubeSizeComboBox));

            return group;
        }

        private void OnCubeSizeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            int cubeSide = ((int)arg.NewValue + 1);
            int dataPointCount = 8 * (int)Math.Pow(cubeSide, 3);

            m_Surface.Data.Clear();
            m_Surface.Indices.Clear();
            m_Surface.Data.SetCapacity(dataPointCount);

            uint currentIndex = 0;

            uint[] cubeIndices = new uint[] {   // bottom
                                            0, 1, 3,
                                            0, 3, 2,

                                            // left
                                            2, 0, 4,
                                            2, 4, 6,

                                            // right
                                            1, 3, 5,
                                            3, 7, 5,

                                            // front
                                            0, 1, 4,
                                            1, 5, 4,

                                            // back
                                            2, 6, 3,
                                            3,6, 7,

                                            // top
                                            4, 5, 6,
                                            5, 7, 6 };

            // generate all vertexes and colors
            for (int x = 0; x < cubeSide; x++)
            {
                double x1 = x + 0.1;
                double x2 = x + 1 - 0.1;

                byte r1 = (byte)(x1 * 255.0 / cubeSide);
                byte r2 = (byte)(x1 * 255.0 / cubeSide);

                for (int y = 0; y < cubeSide; y++)
                {
                    double y1 = y + 0.1;
                    double y2 = y + 1 - 0.1;

                    byte g1 = (byte)(y1 * 255.0 / cubeSide);
                    byte g2 = (byte)(y1 * 255.0 / cubeSide);

                    for (int z = 0; z < cubeSide; z++)
                    {
                        double z1 = z + 0.1;
                        double z2 = z + 1 - 0.1;

                        byte b1 = (byte)(z1 * 255.0 / cubeSide);
                        byte b2 = (byte)(z1 * 255.0 / cubeSide);

                        m_Surface.Data.AddValueColor(new NVector3DD(x1, y1, z1), NColor.FromRGB(r1, g1, b1));
                        m_Surface.Data.AddValueColor(new NVector3DD(x2, y1, z1), NColor.FromRGB(r2, g1, b1));
                        m_Surface.Data.AddValueColor(new NVector3DD(x1, y2, z1), NColor.FromRGB(r1, g2, b1));
                        m_Surface.Data.AddValueColor(new NVector3DD(x2, y2, z1), NColor.FromRGB(r2, g2, b1));
                        m_Surface.Data.AddValueColor(new NVector3DD(x1, y1, z2), NColor.FromRGB(r1, g1, b2));
                        m_Surface.Data.AddValueColor(new NVector3DD(x2, y1, z2), NColor.FromRGB(r2, g1, b2));
                        m_Surface.Data.AddValueColor(new NVector3DD(x1, y2, z2), NColor.FromRGB(r1, g2, b2));
                        m_Surface.Data.AddValueColor(new NVector3DD(x2, y2, z2), NColor.FromRGB(r2, g2, b2));

                        // add indicess
                        for (int i = 0; i < cubeIndices.Length; i++)
                        {
                            m_Surface.Indices.Add(currentIndex + cubeIndices[i]);
                        }

                        currentIndex += 8;
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates how to use the Vertex surface series to plot objects with a large amount of vertices, and with a custom color per vertex.</p>";
        }

        #endregion

        #region Fields

        NVertexSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NVertexSurfaceFillCustomColorsExampleSchema;

        #endregion
    }
}