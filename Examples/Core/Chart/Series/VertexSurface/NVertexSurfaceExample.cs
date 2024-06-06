using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;
using System.IO;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Vertex Surface Example
    /// </summary>
    public class NVertexSurfaceExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NVertexSurfaceExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NVertexSurfaceExample()
        {
            NVertexSurfaceExampleSchema = NSchema.Create(typeof(NVertexSurfaceExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            m_Label = chartView.Surface.Titles[0];
            chartView.Surface.Titles[0].Text = "Vertex Surface Series";

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
            NOrdinalScale ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            ordinalScale = (NOrdinalScale)chart.Axes[ENCartesianAxis.Depth].Scale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            ordinalScale.DisplayDataPointsBetweenTicks = false;

            // add the surface series
            m_Surface = new NVertexSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FlatPositionValue = 10.0;
            
            m_Surface.Palette.InterpolateColors = false;

            m_Surface.ValueFormatter = new NNumericValueFormatter("0.00");
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);
            m_Surface.FillMode = ENSurfaceFillMode.Zone;
            m_Surface.FrameMode = ENSurfaceFrameMode.Contour;
            m_Surface.FrameColorMode = ENSurfaceFrameColorMode.Uniform;
            m_Surface.ShadingMode = ENShadingMode.Smooth;

            return chartViewWithCommandBars;
        }

        protected override NWidget CreateExampleControls()
        {
            NStackPanel stack = new NStackPanel();
            NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NComboBox vertexPrimitiveCombo = new NComboBox();
            vertexPrimitiveCombo.FillFromEnum<ENVertexPrimitive>();
            vertexPrimitiveCombo.SelectedIndexChanged += OnVertexPrimitiveComboSelectedIndexChanged;
            vertexPrimitiveCombo.SelectedIndex = (int)m_Surface.VertexPrimitive;
            stack.Add(NPairBox.Create("Vertex Primitive:", vertexPrimitiveCombo));

            NCheckBox smoothShadingCheckBox = new NCheckBox();
            smoothShadingCheckBox.CheckedChanged += OnSmoothShadingCheckBoxCheckedChanged;
            stack.Add(NPairBox.Create("Smooth Shading:", smoothShadingCheckBox));


            return group;
        }

        private void OnSmoothShadingCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.ShadingMode = (bool)arg.NewValue ? ENShadingMode.Smooth : ENShadingMode.Flat;
        }

        private void OnVertexPrimitiveComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.VertexPrimitive = (ENVertexPrimitive)arg.NewValue;
            m_Surface.UseIndices = false;

            Random rand = new Random();
            string descriptionText = string.Empty;

            m_Surface.Data.Clear();

            switch (m_Surface.VertexPrimitive)
            {
                case ENVertexPrimitive.Points:
                    {
                        descriptionText = "Each vertex represents a 3d point";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Dots;

                        for (int i = 0; i < 10000; i++)
                        {
                            m_Surface.Data.AddValue(rand.Next(100),
                                                    rand.Next(100),
                                                    rand.Next(100));
                        }
                    }
                    break;
                case ENVertexPrimitive.Lines:
                    {
                        descriptionText = "Each consecutive pair of vertices represents a line segment";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Dots;

                        for (int i = 0; i < 200; i++)
                        {
                            m_Surface.Data.AddValue(rand.Next(100),
                                                    rand.Next(100),
                                                    rand.Next(100));

                            m_Surface.Data.AddValue(rand.Next(100),
                                                    rand.Next(100),
                                                    rand.Next(100));
                        }
                    }
                    break;

                case ENVertexPrimitive.LineLoop:
                case ENVertexPrimitive.LineStrip:
                    {
                        descriptionText = "Adjacent vertices are connected with a line segment";

                        for (int i = 0; i < 5; i++)
                        {
                            m_Surface.Data.AddValue(rand.Next(100),
                                                    rand.Next(100),
                                                    rand.Next(100));
                        }
                    }
                    break;

                case ENVertexPrimitive.Triangles:
                    {
                        descriptionText = "Each three consequtive vertices are considered a triangle";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;

                        NVector3DD top = new NVector3DD(0.5, 1, 0.5);
                        NVector3DD baseA = new NVector3DD(0, 0, 0);
                        NVector3DD baseB = new NVector3DD(1, 0, 0);
                        NVector3DD baseC = new NVector3DD(1, 0, 1);
                        NVector3DD baseD = new NVector3DD(0, 0, 1);

                        m_Surface.Data.AddValue(top);
                        m_Surface.Data.AddValue(baseA);
                        m_Surface.Data.AddValue(baseB);

                        m_Surface.Data.AddValue(top);
                        m_Surface.Data.AddValue(baseB);
                        m_Surface.Data.AddValue(baseC);

                        m_Surface.Data.AddValue(top);
                        m_Surface.Data.AddValue(baseC);
                        m_Surface.Data.AddValue(baseD);

                        m_Surface.Data.AddValue(top);
                        m_Surface.Data.AddValue(baseD);
                        m_Surface.Data.AddValue(baseA);
                    }
                    break;
                case ENVertexPrimitive.TriangleStrip:
                    {
                        descriptionText = "A series of connected triangles that share common vertices";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;

                        NVector3DD A = new NVector3DD(0, 0, 0);
                        NVector3DD B = new NVector3DD(1, 0, 0);
                        NVector3DD C = new NVector3DD(0, 1, 1);
                        NVector3DD D = new NVector3DD(1, 1, 1);

                        m_Surface.Data.AddValue(A);
                        m_Surface.Data.AddValue(B);
                        m_Surface.Data.AddValue(C);
                        m_Surface.Data.AddValue(D);
                    }
                    break;
                case ENVertexPrimitive.TriangleFan:
                    {
                        descriptionText = "A series of connected triangles that share a common vertex";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;

                        m_Surface.Data.AddValue(0, 100, 0);

                        int steps = 10;

                        for (int i = 0; i < 3000; i++)
                        {
                            double angle = i * 2 * Math.PI / steps;

                            m_Surface.Data.AddValue(Math.Cos(angle) * 100,
                                                        0,
                                                        Math.Sin(angle) * 100);
                        }
                    }
                    break;
                case ENVertexPrimitive.Quads:
                    {
                        descriptionText = "Each for consecutive vertices form a quad";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;
                        m_Surface.FillMode = ENSurfaceFillMode.Zone;

                        NVector3DD A = new NVector3DD(0, 0, 0);
                        NVector3DD B = new NVector3DD(1, 0, 0);
                        NVector3DD C = new NVector3DD(0, 1, 1);
                        NVector3DD D = new NVector3DD(1, 1, 1);


                        m_Surface.Data.AddValue(A);
                        m_Surface.Data.AddValue(B);
                        m_Surface.Data.AddValue(D);
                        m_Surface.Data.AddValue(C);
                    }
                    break;
                case ENVertexPrimitive.QuadStrip:
                    {
                        descriptionText = "A series of connected quads that share common vertices";

                        m_Surface.FrameMode = ENSurfaceFrameMode.Mesh;
                        m_Surface.FillMode = ENSurfaceFillMode.Zone;

                        NVector3DD A = new NVector3DD(0, 0, 0);
                        NVector3DD B = new NVector3DD(1, 0, 0);
                        NVector3DD C = new NVector3DD(0, 1, 1);
                        NVector3DD D = new NVector3DD(1, 1, 1);
                        NVector3DD E = new NVector3DD(0, 2, 2);
                        NVector3DD F = new NVector3DD(1, 2, 2);


                        m_Surface.Data.AddValue(A);
                        m_Surface.Data.AddValue(B);
                        m_Surface.Data.AddValue(C);
                        m_Surface.Data.AddValue(D);
                        m_Surface.Data.AddValue(E);
                        m_Surface.Data.AddValue(F);
                    }

                    break;
            }

            m_Label.Text = "Vertex Surface " + descriptionText;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates the capabilities of the Vertex Surface Series. This type of series allows you to plot an arbitrary OpenGL primitive.</p>";
        }

        #endregion

        #region Fields

        NLabel m_Label;
        NVertexSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NVertexSurfaceExampleSchema;

        #endregion
    }
}