using System;
using System.IO;

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
	public class NTriangulatedSurfaceCustomColorsExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NTriangulatedSurfaceCustomColorsExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NTriangulatedSurfaceCustomColorsExample()
        {
            NTriangulatedSurfaceCustomColorsExampleSchema = NSchema.Create(typeof(NTriangulatedSurfaceCustomColorsExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Mesh Surface With Custom Colors";

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
            m_Surface = new NTriangulatedSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FlatPositionValue = 10.0;
            
            m_Surface.Palette.InterpolateColors = false;

            m_Surface.ValueFormatter = new NNumericValueFormatter("0.00");
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);

            m_Surface.FillMode = ENSurfaceFillMode.CustomColors;
            m_Surface.FrameMode = ENSurfaceFrameMode.None;
            m_Surface.FrameColorMode = ENSurfaceFrameColorMode.Uniform;

            FillData();

            return chartViewWithCommandBars;
        }

        private void FillData()
        {
            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                // fill the XYZ data from a binary resource
                stream = new MemoryStream(NResources.RBIN_SampleData_DataXYZ_bin.Data);
                reader = new BinaryReader(stream);

                int nDataPointsCount = (int)stream.Length / 12;

                m_Surface.Data.HasColor = true;
                m_Surface.Data.SetCapacity(nDataPointsCount);

                NTriangulatedSurfaceData surfaceData = m_Surface.Data;
                
                NVector3DF[] data = new NVector3DF[nDataPointsCount];

                // fill Y values
                for (int i = 0; i < nDataPointsCount; i++)
                {
                    data[i].Y = reader.ReadSingle();
               }

                // fill X values
                for (int i = 0; i < nDataPointsCount; i++)
                {
                    data[i].X = reader.ReadSingle();
                }

                // fill Z values
                for (int i = 0; i < nDataPointsCount; i++)
                {
                    data[i].Z = reader.ReadSingle();
                }

                for (int i = 0; i < nDataPointsCount; i++)
                {
                    surfaceData.AddValue(data[i]);
                    surfaceData.SetColor(i, GetColorFromValue(data[i].Y));
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
        /// <summary>
        /// Gets a 
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        private NColor GetColorFromValue(float y)
        {
            NColor color = NColor.Black;

            if (y < 100)
            {
                color = NColor.FromRGB(20, 30, 180);
            }
            else if (y < 150)
            {
                color = NColor.FromRGB(20, 100, 100);
            }
            else if (y < 200)
            {
                color = NColor.FromRGB(20, 140, 80);
            }
            else if (y < 250)
            {
                color = NColor.FromRGB(80, 140, 60);
            }
            else if (y < 300)
            {
                color = NColor.FromRGB(140, 140, 40);
            }

            return NColor.FromRGB(
                (byte)Math.Min(255, color.R + m_Random.NextDouble() * 50),
                (byte)Math.Min(255, color.G + m_Random.NextDouble() * 50),
                (byte)Math.Min(255, color.B + m_Random.NextDouble() * 50));
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
            smoothShadingCheckBox.Checked = true;
            smoothShadingCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Smooth Shading:", smoothShadingCheckBox));

            NCheckBox drawFlatCheckBox = new NCheckBox();
            drawFlatCheckBox.CheckedChanged += OnDrawFlatCheckBoxCheckedChanged;
            drawFlatCheckBox.Checked = false;
            drawFlatCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Draw Flat:", drawFlatCheckBox));

            NCheckBox smoothPaletteCheckBox = new NCheckBox();
            smoothPaletteCheckBox.CheckedChanged += OnSmoothPaletteCheckBoxCheckedChanged;
            smoothPaletteCheckBox.Checked = false;
            smoothPaletteCheckBox.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Smooth Palette:", smoothPaletteCheckBox));

            return group;
        }

        private void OnSmoothPaletteCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_Surface.Palette.InterpolateColors = (bool)arg.NewValue;
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
        }

        private void OnFillModeComboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Surface.FillMode = (ENSurfaceFillMode)arg.NewValue;
        }

        protected override string GetExampleDescription()
        {
            return @"<p>The example demonstrates the ability of the Triangulated Surface Series to assign custom color per each individual surface vertex.</p>";
        }

        #endregion

        #region Fields

        NTriangulatedSurfaceSeries m_Surface;
        Random m_Random = new Random();

        #endregion

        #region Schema

        public static readonly NSchema NTriangulatedSurfaceCustomColorsExampleSchema;

        #endregion
    }
}