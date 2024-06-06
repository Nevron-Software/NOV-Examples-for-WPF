using Nevron.Nov.Barcode;
using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Diagram;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Diagram
{
	public class NChartsInDiagramExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor.
		/// </summary>
		public NChartsInDiagramExample()
		{
		}

		/// <summary>
		/// Static constructor.
		/// </summary>
		static NChartsInDiagramExample()
		{
			NChartsInDiagramExampleSchema = NSchema.Create(typeof(NChartsInDiagramExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			// Create a simple drawing
			NDrawingViewWithRibbon drawingViewWithRibbon = new NDrawingViewWithRibbon();
			m_DrawingView = drawingViewWithRibbon.View;

			m_DrawingView.Document.HistoryService.Pause();
			try
			{
				InitDiagram(m_DrawingView.Document);
			}
			finally
			{
				m_DrawingView.Document.HistoryService.Resume();
			}

			return drawingViewWithRibbon;
		}
		protected override NWidget CreateExampleControls()
		{
			return null;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>Demonstrates how to create and host charts in diagram shapes.</p>";
		}

		private void InitDiagram(NDrawingDocument drawingDocument)
		{
			NPage activePage = drawingDocument.Content.ActivePage;

            // Create a barcode widget
            NChartView chartView = new NChartView();
            chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

            // configure title
            chartView.Surface.Titles[0].Text = "Manhattan Bar Chart";

            // configure chart

            chart.Enable3D = true;
            chart.ModelWidth = 60;
            chart.ModelHeight = 25;
            chart.ModelDepth = 45;
            chart.FitMode = ENCartesianChartFitMode.Aspect;
            chart.Interactor = new NInteractor(new NTrackballTool());

            // apply predefined projection and lighting
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);

            // add axis labels
            NOrdinalScale ordinalScale = chart.Axes[ENCartesianAxis.Depth].Scale as NOrdinalScale;

            ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(new string[] {    "Chicago",
                                                                                                    "Los Angeles",
                                                                                                    "Miami",
                                                                                                    "New York" });
            ordinalScale.DisplayDataPointsBetweenTicks = true;
            ordinalScale.Labels.Style.Angle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);

            ordinalScale = chart.Axes[ENCartesianAxis.PrimaryX].Scale as NOrdinalScale;
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Bottom, true);
            ordinalScale.MajorGridLines.SetShowAtWall(ENChartWall.Back, true);

            // add interlace stripe to the Y axis
            NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
            NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            stripStyle.Interlaced = true;
            stripStyle.SetShowAtWall(ENChartWall.Back, true);
            stripStyle.SetShowAtWall(ENChartWall.Left, true);
            linearScale.Strips.Add(stripStyle);

            // add the first bar
            NBarSeries bar1 = new NBarSeries();
            chart.Series.Add(bar1);
            bar1.MultiBarMode = ENMultiBarMode.Series;
            bar1.Name = "Bar1";
            bar1.Stroke = new NStroke(1, NColor.FromRGB(210, 210, 255));

            // add the second bar
            NBarSeries bar2 = new NBarSeries();
            chart.Series.Add(bar2);
            bar2.MultiBarMode = ENMultiBarMode.Series;
            bar2.Name = "Bar2";
            bar2.Stroke = new NStroke(1, NColor.FromRGB(210, 255, 210));

            // add the third bar
            NBarSeries bar3 = new NBarSeries();
            chart.Series.Add(bar3);
            bar3.MultiBarMode = ENMultiBarMode.Series;
            bar3.Name = "Bar3";
            bar3.Stroke = new NStroke(1, NColor.FromRGB(255, 255, 210));

            // add the second bar
            NBarSeries bar4 = new NBarSeries();
            chart.Series.Add(bar4);
            bar4.MultiBarMode = ENMultiBarMode.Series;
            bar4.Name = "Bar4";
            bar4.Stroke = new NStroke(1, NColor.FromRGB(255, 210, 210));

            // add some data
            Random random = new Random();
            for (int i = 0; i < 6; i++)
            {
                bar1.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                bar2.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                bar3.DataPoints.Add(new NBarDataPoint(random.Next(100)));
                bar4.DataPoints.Add(new NBarDataPoint(random.Next(100)));
            }

            // Create a shape and place the barcode widget in it
            NShape shape = new NShape();
			shape.SetBounds(100, 100, 600, 400);
			shape.Widget = chartView;
			activePage.Items.Add(shape);
		}

		#endregion

		#region Fields

		private NDrawingView m_DrawingView;

		#endregion

		#region Schema

		/// <summary>
		/// Schema associated with NChartsInDiagramExample.
		/// </summary>
		public static readonly NSchema NChartsInDiagramExampleSchema;

		#endregion
	}
}