using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Aspect 3D example
	/// </summary>
	public class NChartAspect3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NChartAspect3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NChartAspect3DExample()
		{
			NChartAspect3DExampleSchema = NSchema.Create(typeof(NChartAspect3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Chart Aspect 3D";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.Interactor = new NInteractor(new NTrackballTool());

			chart.Enable3D = true;
			chart.ModelWidth = 50;
			chart.ModelHeight = 50;
			chart.ModelDepth = 50;
			chart.FitMode = ENCartesianChartFitMode.Aspect;
			chart.Walls[ENChartWall.Back].Width = 0.01f;
			chart.Walls[ENChartWall.Bottom].Width = 0.01f;
			chart.Walls[ENChartWall.Left].Width = 0.01f;

			// apply predefined projection and lighting
			chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
			chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);

			// add axis labels
			NOrdinalScale ordinalScale = chart.Axes[ENCartesianAxis.Depth].Scale as NOrdinalScale;
			ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(new string[] { "Miami", "Chicago", "Los Angeles", "New York" });
            ordinalScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

            ordinalScale = chart.Axes[ENCartesianAxis.PrimaryX].Scale as NOrdinalScale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

            // add interlace stripe to the Y axis
            NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            strip.SetShowAtWall(ENChartWall.Back, true);
            strip.SetShowAtWall(ENChartWall.Left, true);
            linearScale.Strips.Add(strip);

            // add the first bar
            NBarSeries bar1 = new NBarSeries();
            chart.Series.Add(bar1);
            bar1.MultiBarMode = ENMultiBarMode.Series;
            bar1.Name = "Bar1";
            bar1.DataLabelStyle = new NDataLabelStyle(false);
            bar1.Stroke = new NStroke(NColor.FromRGB(210, 210, 255));

            // add the second bar
            NBarSeries bar2 = new NBarSeries();
            chart.Series.Add(bar2);
            bar2.MultiBarMode = ENMultiBarMode.Series;
            bar2.Name = "Bar2";
            bar2.DataLabelStyle = new NDataLabelStyle(false);
            bar2.Stroke = new NStroke(NColor.FromRGB(210, 255, 210));

            // add the third bar
            NBarSeries bar3 = new NBarSeries();
            chart.Series.Add(bar3);
            bar3.MultiBarMode = ENMultiBarMode.Series;
            bar3.Name = "Bar3";
            bar3.DataLabelStyle = new NDataLabelStyle(false);
            bar3.Stroke = new NStroke(NColor.FromRGB(255, 255, 210));

            // add the second bar
            NBarSeries bar4 = new NBarSeries();
            chart.Series.Add(bar4);
            bar4.MultiBarMode = ENMultiBarMode.Series;
            bar4.Name = "Bar4";
            bar4.DataLabelStyle = new NDataLabelStyle(false);
            bar4.Stroke = new NStroke(NColor.FromRGB(255, 210, 210));

            // fill with random data
            Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				bar1.DataPoints.Add(new NBarDataPoint((random.Next() + 1.0) * 30 + 10));
                bar2.DataPoints.Add(new NBarDataPoint((random.Next() + 1.0) * 30 + 30));
                bar3.DataPoints.Add(new NBarDataPoint((random.Next() + 1.0) * 30 + 50));
                bar4.DataPoints.Add(new NBarDataPoint((random.Next() + 1.0) * 30 + 70));
            }

            // apply theme
            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			NComboBox proportionXComboBox = CreateProportionComboBox();
            proportionXComboBox.SelectedIndexChanged += OnProportionXComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Proportion X:", proportionXComboBox));

            NComboBox proportionYComboBox = CreateProportionComboBox();
            proportionYComboBox.SelectedIndexChanged += OnProportionYComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Proportion Y:", proportionYComboBox));

            NComboBox proportionZComboBox = CreateProportionComboBox();
            proportionZComboBox.SelectedIndexChanged += OnProportionZComboBoxSelectedIndexChanged;
            stack.Add(NPairBox.Create("Proportion Z:", proportionZComboBox));

			return boxGroup;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how change the chart aspect ratio in 2D.</p>";
		}

        #endregion

        #region Implementation

        private void NormalizeProportions()
        {
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
            float max = Math.Max(Math.Max(chart.ModelWidth, chart.ModelHeight), chart.ModelDepth);

            float scale = 50 / max;

            chart.ModelWidth *= scale;
            chart.ModelHeight *= scale;
            chart.ModelDepth *= scale;
        }

        private NComboBox CreateProportionComboBox()
		{
			NComboBox comboBox = new NComboBox();

			for (int i = 0; i < 5; i++)
			{
				comboBox.Items.Add(new NComboBoxItem((i + 1).ToString()));
			}

			comboBox.SelectedIndex = 0;

			return comboBox;
		}

        #endregion

        #region Event Handlers

        private void OnProportionXComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
            chart.ModelWidth = ((int)arg.NewValue + 1);
            NormalizeProportions();
        }

        private void OnProportionYComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
            chart.ModelHeight = ((int)arg.NewValue + 1);
            NormalizeProportions();
        }

        private void OnProportionZComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
        {
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
            chart.ModelDepth = ((int)arg.NewValue + 1);
            NormalizeProportions();
        }

        #endregion

        #region Fields

        private NChartView m_ChartView;
			
		#endregion

		#region Schema

		public static readonly NSchema NChartAspect3DExampleSchema;

		#endregion
	}
}