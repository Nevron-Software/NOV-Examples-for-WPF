using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Vertex Surface Example
    /// </summary>
    public class NVertexSurfaceFrameCustomColorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NVertexSurfaceFrameCustomColorsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NVertexSurfaceFrameCustomColorsExample()
        {
            NVertexSurfaceFrameCustomColorsExampleSchema = NSchema.Create(typeof(NVertexSurfaceFrameCustomColorsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Vertex Surface Series Line With Custom Colors";

            // setup chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 60.0f;
            m_Chart.ModelDepth = 60.0f;
            m_Chart.ModelHeight = 50.0f;
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.SoftTopLeft);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // setup axes
            NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            NLinearScale scaleX = new NLinearScale();
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;

            NLinearScale scaleZ = new NLinearScale();
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);
            scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            m_Chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;

            // add the surface series
            m_Surface = new NVertexSurfaceSeries();
            m_Chart.Series.Add(m_Surface);

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

            m_NumberOfLinesUpDown = new NNumericUpDown();
            m_NumberOfLinesUpDown.Minimum = 1;
            m_NumberOfLinesUpDown.Maximum = 10;
            m_NumberOfLinesUpDown.ValueChanged += OnNumberOfLinesUpDownValueChanged;
            stack.Add(NPairBox.Create("Number of Lines:", m_NumberOfLinesUpDown));

            m_DataCountPerLineComboBox = new NComboBox();

            m_DataCountPerLineComboBox.Items.Add(new NComboBoxItem("10K"));
            m_DataCountPerLineComboBox.Items.Add(new NComboBoxItem("100K"));
            m_DataCountPerLineComboBox.Items.Add(new NComboBoxItem("500K"));

            m_DataCountPerLineComboBox.SelectedIndexChanged += OnDataCountPerLineComboBoxSelectedIndexChanged;
            
            stack.Add(NPairBox.Create("Data Count Per Line:", m_DataCountPerLineComboBox));

            m_DataCountPerLineComboBox.SelectedIndex = 1;
            m_NumberOfLinesUpDown.Value = 7;

            return group;
        }

        private void UpdateData()
        {
            int dataPointCount = 0;

            switch (m_DataCountPerLineComboBox.SelectedIndex)
            {
                case 0:
                    dataPointCount = 10000;
                    break;
                case 1:
                    dataPointCount = 100000;
                    break;
                case 2:
                    dataPointCount = 500000;
                    break;
            }

            int lineCount = (int)m_NumberOfLinesUpDown.Value;
            m_Chart.Series.Clear();
            Random random = new Random();

            NColor[] palette = NChartPalette.GetColors(ENChartPalette.Fresh);

            for (int lineIndex = 0; lineIndex < lineCount; lineIndex++)
            {
                // setup surface series
                NVertexSurfaceSeries surface = new NVertexSurfaceSeries();
                m_Chart.Series.Add(surface);

                surface.Name = "Surface";
                surface.FillMode = ENSurfaceFillMode.CustomColors;
                surface.FrameMode = ENSurfaceFrameMode.Dots;
                surface.FrameColorMode = ENSurfaceFrameColorMode.CustomColors;
                surface.VertexPrimitive = ENVertexPrimitive.LineStrip;
                surface.Data.HasColor = true;
                surface.Data.SetCapacity(dataPointCount);

                double x = 0.1;
                double y = 0;
                double z = 0;
                double a = 10.0;
                double b = 18 + lineIndex; // 28.0 - ;
                double c = (lineIndex + 3) / 3.0; //8.0
                double t = lineIndex * (0.01 / lineCount) + 0.01;

                NColor color1 = palette[lineIndex % palette.Length];
                NColor color2 = palette[(lineIndex + 1) % palette.Length];

                unsafe
                {
                    fixed (byte* pData = &surface.Data.Data[0])
                    {
                        float* pVertex = (float*)pData;
                        uint* pColor = (uint*)(pData + surface.Data.ColorOffset * 4);

                        for (int dataPointIndex = 0; dataPointIndex < dataPointCount; dataPointIndex++)
                        {
                            float xt = (float)(x + t * a * (y - x));
                            float yt = (float)(y + t * (x * (b - z) - y));
                            float zt = (float)(z + t * (x * y - c * z));

                            pVertex[0] = xt;
                            pVertex[1] = yt;
                            pVertex[2] = zt;

                            NColor color = NColor.InterpolateColors(color1, color2, (float)((yt + 40.0) / 80.0));
                            *pColor = color.PackedARGB;

                            pVertex += 4;
                            pColor += 4;

                            x = xt;
                            y = yt;
                            z = zt;
                        }
                    }
                }

                // notify series that data has changed as we've modified it directly using pointers
                surface.Data.SetCount(dataPointCount);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnNumberOfLinesUpDownValueChanged(NValueChangeEventArgs arg)
        {
            UpdateData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void OnDataCountPerLineComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            UpdateData();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates how to use the Vertex Surface series to plot lines with large amounts of vertices, and with custom color per vertex.</p>";
        }

        #endregion

        #region Fields

        NNumericUpDown m_NumberOfLinesUpDown;
        NComboBox m_DataCountPerLineComboBox;
        NVertexSurfaceSeries m_Surface;
        NCartesianChart m_Chart;

        #endregion

        #region Schema

        public static readonly NSchema NVertexSurfaceFrameCustomColorsExampleSchema;

        #endregion
    }
}