using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// Standard Bar Example
    /// </summary>
    public class NManhattanBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NManhattanBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NManhattanBarExample()
		{
			NManhattanBarExampleSchema = NSchema.Create(typeof(NManhattanBarExample), NExampleBaseSchema);
		}

        #endregion

        #region Example

        protected override NWidget CreateExampleContent()
        {
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            chartView.Surface.Titles[0].Text = "Manhattan Bar Chart";

            // configure chart
            m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 60;
            m_Chart.ModelHeight = 25;
            m_Chart.ModelDepth = 45;
            m_Chart.FitMode = ENCartesianChartFitMode.Aspect;
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // apply predefined projection and lighting
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);

            // add axis labels
            NOrdinalScale ordinalScale = m_Chart.Axes[ENCartesianAxis.Depth].Scale as NOrdinalScale;

            ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(new string[] {    "Chicago",
                                                                                                    "Los Angeles",
                                                                                                    "Miami",
                                                                                                    "New York" });
            ordinalScale.DisplayDataPointsBetweenTicks = true;
            ordinalScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

            ordinalScale = m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale as NOrdinalScale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

            // add interlace stripe to the Y axis
            NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            stripStyle.Interlaced = true;
            stripStyle.SetShowAtWall(ENChartWall.Back, true);
            stripStyle.SetShowAtWall(ENChartWall.Left, true);
            linearScale.Strips.Add(stripStyle);

            // add the first bar
            m_Bar1 = new NBarSeries();
            m_Chart.Series.Add(m_Bar1);
            m_Bar1.MultiBarMode = ENMultiBarMode.Series;
            m_Bar1.Name = "Bar1";
            m_Bar1.Stroke = new NStroke(1, NColor.FromRGB(210, 210, 255));

            // add the second bar
            m_Bar2 = new NBarSeries();
            m_Chart.Series.Add(m_Bar2);
            m_Bar2.MultiBarMode = ENMultiBarMode.Series;
            m_Bar2.Name = "Bar2";
            m_Bar2.Stroke = new NStroke(1, NColor.FromRGB(210, 255, 210));

            // add the third bar
            m_Bar3 = new NBarSeries();
            m_Chart.Series.Add(m_Bar3);
            m_Bar3.MultiBarMode = ENMultiBarMode.Series;
            m_Bar3.Name = "Bar3";
            m_Bar3.Stroke = new NStroke(1, NColor.FromRGB(255, 255, 210));

            // add the second bar
            m_Bar4 = new NBarSeries();
            m_Chart.Series.Add(m_Bar4);
            m_Bar4.MultiBarMode = ENMultiBarMode.Series;
            m_Bar4.Name = "Bar4";
            m_Bar4.Stroke = new NStroke(1, NColor.FromRGB(255, 210, 210));

            OnPositiveDataButtonClick(null);

            return chartViewWithCommandBars;
        }
        protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			{
				NComboBox barShapeComobo = new NComboBox();
				barShapeComobo.FillFromEnum<ENBarShape>();
                barShapeComobo.SelectedIndexChanged += OnBarShapeComoboSelectedIndexChanged;
                barShapeComobo.SelectedIndex = (int)ENBarShape.Rectangle;
                stack.Add(NPairBox.Create("Origin Mode: ", barShapeComobo));
			}

            {
                NHScrollBar barWidthScrollBar = new NHScrollBar();
                barWidthScrollBar.Minimum = 0;
                barWidthScrollBar.Maximum = 100;
                barWidthScrollBar.ValueChanged += OnBarWidthScrollBarValueChanged;
                barWidthScrollBar.Value = 50;
                stack.Add(NPairBox.Create("Width Gap %: ", barWidthScrollBar));
            }

            {
                NHScrollBar barDepthScrollBar = new NHScrollBar();
                barDepthScrollBar.Minimum = 0;
                barDepthScrollBar.Maximum = 100;
                barDepthScrollBar.ValueChanged += OnBarDepthScrollBarValueChanged;
                barDepthScrollBar.Value = 50;
                stack.Add(NPairBox.Create("Depth Gap %: ", barDepthScrollBar));
            }

            {
				NButton positiveDataButton = new NButton("Positive Data");
                positiveDataButton.Click += OnPositiveDataButtonClick;
				stack.Add(positiveDataButton);
			}

            {
                NButton positiveAndNegativeData = new NButton("Positive and Negative Data");
                positiveAndNegativeData.Click += OnPositiveAndNegativeDataButtonClick;
                stack.Add(positiveAndNegativeData);
            }

            return boxGroup;
		}
        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates a Manhattan bar chart. This type of chart is created with several bar series displayed with MultiBarMode set to Series.</p>";
        }

        #endregion

        #region Event Handlers

        private void OnBarWidthScrollBarValueChanged(NValueChangeEventArgs arg)
        {
            double gapFactor = (double)arg.NewValue / 100;

            m_Bar1.WidthGapFactor = gapFactor;
            m_Bar2.WidthGapFactor = gapFactor;
            m_Bar3.WidthGapFactor = gapFactor;
            m_Bar4.WidthGapFactor = gapFactor;
        }

        private void OnBarDepthScrollBarValueChanged(NValueChangeEventArgs arg)
        {
            double gapFactor = (double)arg.NewValue / 100;

            m_Bar1.DepthGapFactor = gapFactor;
            m_Bar2.DepthGapFactor = gapFactor;
            m_Bar3.DepthGapFactor = gapFactor;
            m_Bar4.DepthGapFactor = gapFactor;
        }

        private void OnPositiveDataButtonClick(NEventArgs arg)
        {
            m_Bar1.DataPoints.Clear();
            m_Bar2.DataPoints.Clear();
            m_Bar3.DataPoints.Clear();
            m_Bar4.DataPoints.Clear();

            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                m_Bar3.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                m_Bar4.DataPoints.Add(new NBarDataPoint(random.Next(100)));
            }
        }

        private void OnPositiveAndNegativeDataButtonClick(NEventArgs arg)
        {
            m_Bar1.DataPoints.Clear();
            m_Bar2.DataPoints.Clear();
            m_Bar3.DataPoints.Clear();
            m_Bar4.DataPoints.Clear();

            Random random = new Random();
            for (int i = 0; i < 12; i++)
            {
                m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(100) - 50));
                m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(100) - 50));
                m_Bar3.DataPoints.Add(new NBarDataPoint(random.Next(100) - 50));
                m_Bar4.DataPoints.Add(new NBarDataPoint(random.Next(100) - 50));
            }
        }

        private void OnBarShapeComoboSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            m_Bar1.Shape = (ENBarShape)arg.NewValue;
            m_Bar2.Shape = (ENBarShape)arg.NewValue;
            m_Bar3.Shape = (ENBarShape)arg.NewValue;
            m_Bar4.Shape = (ENBarShape)arg.NewValue;
        }

        #endregion

        #region Fields

        private NCartesianChart m_Chart;
        private NBarSeries m_Bar1;
        private NBarSeries m_Bar2;
        private NBarSeries m_Bar3;
        private NBarSeries m_Bar4;

        #endregion

        #region Schema

        public static readonly NSchema NManhattanBarExampleSchema;

		#endregion
	}
}
