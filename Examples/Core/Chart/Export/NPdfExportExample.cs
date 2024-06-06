using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Export;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Pdf Export Example.
	/// </summary>
	public class NPdfExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPdfExportExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPdfExportExample()
		{
			NPdfExportExampleSchema = NSchema.Create(typeof(NPdfExportExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

            // configure title
            m_ChartView.Surface.Titles[0].Text = "PDF Export Example";

            // configure chart
            NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

            chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYZLinear);
            chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            chart.Interactor = new NInteractor(new NTrackballTool());

            // setup X axis
            NLinearScale xScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

            xScale.MajorGridLines.Visible = true;

            // setup Y axis
            NLinearScale yScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            yScale.MajorGridLines.Visible = true;

            // add interlaced stripe
            NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            yScale.Strips.Add(strip);

            // setup shape series
            NRangeSeries range = new NRangeSeries();
            chart.Series.Add(range);

            range.DataLabelStyle = new NDataLabelStyle(false);
            range.UseXValues = true;
            range.Fill = new NColorFill(NColor.DarkOrange);
            range.Stroke = new NStroke(NColor.DarkRed);

            // fill data
            double[] intervals = new double[] { 5, 5, 5, 5, 5, 5, 5, 5, 5, 15, 30, 60 };
            double[] values = new double[] { 4180, 13687, 18618, 19634, 17981, 7190, 16369, 3212, 4122, 9200, 6461, 3435 };

            int count = Math.Min(intervals.Length, values.Length);
            double x = 0;

            for (int i = 0; i < count; i++)
            {
                double interval = intervals[i];
                double value = values[i];

                double x1 = x;
                double y1 = 0;

                x += interval;
                double x2 = x;
                double y2 = value / interval;

                range.DataPoints.Add(new NRangeDataPoint(x1, y1, x2, y2));
            }

            m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

            return chartViewWithCommandBars;
        }
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);

            NCheckBox enable3DCheckBox = new NCheckBox("Enable 3D");
            enable3DCheckBox.CheckedChanged += OnEnable3DCheckBoxCheckedChanged;
            enable3DCheckBox.Checked = true;
            stack.Add(enable3DCheckBox);

            NButton saveAsPdfFileButton = new NButton("Save as PDF File...");
			saveAsPdfFileButton.Click += SaveAsPdfFileButton_Click;
			stack.Add(saveAsPdfFileButton);

			NButton showPdfExportDialog = new NButton("Show PDF Export Dialog...");
			showPdfExportDialog.Click += ShowPdfExportDialog_Click;
			stack.Add(showPdfExportDialog);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to export the chart to PDF file or stream.</p>";
		}

        #endregion

        #region Event Handlers

        private void OnEnable3DCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_ChartView.Surface.Charts[0].Enable3D = (bool)arg.NewValue;
        }
        private void SaveAsPdfFileButton_Click(NEventArgs arg)
		{
			NChartPdfExporter pdfExporter = new NChartPdfExporter(m_ChartView.Content);
			pdfExporter.SaveAsPdf();
        }
		private void ShowPdfExportDialog_Click(NEventArgs arg)
		{
			NChartPdfExporter pdfExporter = new NChartPdfExporter(m_ChartView.Content);
			pdfExporter.ShowDialog(m_ChartView.DisplayWindow, true);
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;

		#endregion

		#region Schema

		public static readonly NSchema NPdfExportExampleSchema;

		#endregion
	}
}
