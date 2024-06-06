using System.IO;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Mesh Surface Contour Lines Example
	/// </summary>
	public class NTriangulatedSurfaceContourLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NTriangulatedSurfaceContourLinesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NTriangulatedSurfaceContourLinesExample()
		{
            NTriangulatedSurfaceContourLinesExampleSchema = NSchema.Create(typeof(NTriangulatedSurfaceContourLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Triangulated Surface Isolines";

            // setup chart
            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            chart.Enable3D = true;
            chart.ModelWidth = 60.0f;
            chart.ModelDepth = 60.0f;
            chart.ModelHeight = 25.0f;
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.SoftTopLeft);
            chart.Interactor = new NInteractor(new NTrackballTool());

            NLinearScale scale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            scale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.None;

            // setup axes
            NLinearScale scaleX = new NLinearScale();
            chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleX.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

            NLinearScale scaleZ = new NLinearScale();
            chart.Axes[ENCartesianAxis.Depth].Scale = scaleZ;
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            scaleZ.MajorGridLines.SetShowAtWall(ENChartWall.Left, true);

            // add the surface series
            m_Surface = new NTriangulatedSurfaceSeries();
            chart.Series.Add(m_Surface);

            m_Surface.Name = "Surface";
            m_Surface.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
            m_Surface.FillMode = ENSurfaceFillMode.Zone;
            m_Surface.FrameMode = ENSurfaceFrameMode.None;
            m_Surface.FlatPositionValue = 0.5;
            m_Surface.Fill = new NColorFill(NColor.YellowGreen);

            m_RedIsoline = new NContourLine();
            m_RedIsoline.Value = 100;
            m_RedIsoline.Stroke = new NStroke(2.0f, NColor.Red);
            m_Surface.ContourLines.Add(m_RedIsoline);

            m_BlueIsoline = new NContourLine();
            m_BlueIsoline.Value = 50;
            m_BlueIsoline.Stroke = new NStroke(2.0f, NColor.Blue);
            m_Surface.ContourLines.Add(m_BlueIsoline);

            FillData();

            return chartViewWithCommandBars;
        }

        private void FillData()
        {
            NTriangulatedSurfaceSeries surface = m_Surface;

            Stream stream = null;
            BinaryReader reader = null;

            try
            {
                // fill the XYZ data from a binary resource
                stream = new MemoryStream(NResources.RBIN_SampleData_DataXYZ_bin.Data);
                reader = new BinaryReader(stream);

                int nDataPointsCount = (int)stream.Length / 12;

                //surface.Data.SetCapacity(nDataPointsCount);
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

                surface.Data.Clear();
                surface.Data.AddValues(data);
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

            NNumericUpDown redIsolineValueUpDown = new NNumericUpDown();
            redIsolineValueUpDown.Value = m_RedIsoline.Value;
            redIsolineValueUpDown.ValueChanged += OnRedIsolineValueUpDownValueChanged;
            redIsolineValueUpDown.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Red Isoline Value:", redIsolineValueUpDown));

            NNumericUpDown blueIsolineValueUpDown = new NNumericUpDown();
            blueIsolineValueUpDown.Value = m_BlueIsoline.Value;
            blueIsolineValueUpDown.ValueChanged += OnBlueIsolineValueUpDownValueChanged;
            blueIsolineValueUpDown.HorizontalPlacement = Layout.ENHorizontalPlacement.Left;
            stack.Add(NPairBox.Create("Blue Isoline Value:", blueIsolineValueUpDown));

            return group;
		}

        private void OnRedIsolineValueUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_RedIsoline.Value = (double)arg.NewValue;
        }

        private void OnBlueIsolineValueUpDownValueChanged(NValueChangeEventArgs arg)
        {
            m_BlueIsoline.Value = (double)arg.NewValue;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates how to create isolines on a triangulated surface chart.</p>";
		}

        #endregion

        #region Fields

        NTriangulatedSurfaceSeries m_Surface;
        NContourLine m_RedIsoline;
        NContourLine m_BlueIsoline;

        #endregion

        #region Schema

        public static readonly NSchema NTriangulatedSurfaceContourLinesExampleSchema;

		#endregion
	}
}