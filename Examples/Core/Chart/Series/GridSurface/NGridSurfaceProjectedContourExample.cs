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
    /// Grid Surface Projected Contour Example
    /// </summary>
    public class NGridSurfaceProjectedContourExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NGridSurfaceProjectedContourExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NGridSurfaceProjectedContourExample()
		{
			NGridSurfaceProjectedContourExampleSchema = NSchema.Create(typeof(NGridSurfaceProjectedContourExample), NExampleBaseSchema);
		}

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Grid Surface Projected Contour Chart";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
            chart.Enable3D = true;
            chart.ModelWidth = 55.0f;
            chart.ModelDepth = 55.0f;
            chart.ModelHeight = 45.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.ShinyCameraLight);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup Y axis
            NLinearScale scaleY = new NLinearScale();
            scaleY.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            NCartesianAxis axisY = chart.Axes[ENCartesianAxis.PrimaryY];
            axisY.MinViewRangeValue = 100;
            axisY.MaxViewRangeValue = 100;
            axisY.Scale = scaleY;

            // setup X axis
            NLinearScale scaleX = new NLinearScale();
            scaleX.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            NCartesianAxis axisX = chart.Axes[ENCartesianAxis.PrimaryX];
            axisX.Scale = scaleX;

            // setup Z axis
            NLinearScale scaleZ = new NLinearScale();
            scaleZ.MajorGridLines.ShowAtWalls = ENChartWall.NoneMask;
            scaleZ.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;
            NCartesianAxis axisZ = chart.Axes[ENCartesianAxis.Depth];
            axisZ.Scale = scaleZ;

            // add a surface series
            NGridSurfaceSeries surface = new NGridSurfaceSeries();
            chart.Series.Add(surface);
            surface.Name = "Surface";
            surface.LegendView.Mode = ENSeriesLegendMode.None;
            surface.Fill = new NColorFill(NColor.FromRGB(160, 170, 212));
            surface.FillMode = ENSurfaceFillMode.Uniform;
            surface.FrameMode = ENSurfaceFrameMode.None;
            surface.DrawFlat = false;
            surface.ShadingMode = ENShadingMode.Smooth;
            SetupCommonSurfaceProperties(surface);

            // fill both surfaces with the same data
            FillData(surface);

            // add a surface series
            NGridSurfaceSeries contour = new NGridSurfaceSeries();
            chart.Series.Add(contour);
            contour.Name = "Contour";
            contour.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            contour.FillMode = ENSurfaceFillMode.Zone;
            contour.FrameMode = ENSurfaceFrameMode.Contour;
            contour.DrawFlat = true;
            contour.FlatPositionMode = ENSurfaceFlatPositionMode.CustomValue;
            contour.FlatPositionValue = 0;
            contour.ShadingMode = ENShadingMode.Flat;
            SetupCommonSurfaceProperties(contour);

            // fill both surfaces with the same data
            FillData(contour);

            NColorValuePalette palette = new NColorValuePalette();
            palette.ColorValuePairs = new NDomArray<NColorValuePair>(new NColorValuePair[] {
            new NColorValuePair(250, NColor.FromRGB(112, 211, 162)),
            new NColorValuePair(311, NColor.FromRGB(113, 197, 212)),
            new NColorValuePair(328, NColor.FromRGB(114, 162, 212)),
            new NColorValuePair(344, NColor.FromRGB(196, 185, 206)),
            new NColorValuePair(358, NColor.FromRGB(161, 130, 191)),
            new NColorValuePair(370, NColor.FromRGB(198, 170, 165)),
            new NColorValuePair(400, NColor.FromRGB(255, 0, 0)) } );

            // contour.Palette.Add(0, Color.Red);
            // contour.Palette.Add(100, Color.Blue);

            contour.Palette = palette;

            FillData(surface);

            return chartViewWithCommandBars;
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
                        double value = 300 + 0.3 * (double)reader.ReadSingle();
                        surface.Data.SetValue(x, z, value);
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

            return group;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates a 2D contour chart projection displayed with the a Grid Surface Series with flat rendering.</p>";
		}

        #endregion

        #region Fields


        #endregion

        #region Schema

        public static readonly NSchema NGridSurfaceProjectedContourExampleSchema;

		#endregion
	}
}