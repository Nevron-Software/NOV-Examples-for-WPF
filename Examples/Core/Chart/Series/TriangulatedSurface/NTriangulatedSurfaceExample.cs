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
	public class NTriangulatedSurfaceExample : NExampleBase
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public NTriangulatedSurfaceExample()
        {

        }
        /// <summary>
        /// Static constructor
        /// </summary>
        static NTriangulatedSurfaceExample()
        {
            NTriangulatedSurfaceExampleSchema = NSchema.Create(typeof(NTriangulatedSurfaceExample), NExampleBaseSchema);
        }

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Triangulated Surface Chart";

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

            m_Surface.FillMode = ENSurfaceFillMode.Zone;
            m_Surface.FrameMode = ENSurfaceFrameMode.Contour;
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

                m_Surface.Data.SetCapacity(nDataPointsCount);
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
                    m_Surface.Data.AddValue(data[i]);
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
            smoothShadingCheckBox.Checked = false;
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
            return @"<p>This example demonstrates how to create a scatter funnel chart.</p>";
        }

        #endregion

        #region Fields

        NTriangulatedSurfaceSeries m_Surface;

        #endregion

        #region Schema

        public static readonly NSchema NTriangulatedSurfaceExampleSchema;

        #endregion
    }
}