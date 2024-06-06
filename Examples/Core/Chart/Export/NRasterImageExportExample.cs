using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Export;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	public class NRasterImageExportExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRasterImageExportExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRasterImageExportExample()
		{
			NRasterImageExportExampleSchema = NSchema.Create(typeof(NRasterImageExportExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Raster Image Export Example";

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

            NButton copyImageToClipboardButton = new NButton("Copy Image to Clipboard");
			copyImageToClipboardButton.Click += CopyImageToClipboardButton_Click;
			stack.Add(copyImageToClipboardButton);

			NButton saveAsImageFileButton = new NButton("Save as Image File...");
			saveAsImageFileButton.Click += SaveAsImageFileButton_Click;
			stack.Add(saveAsImageFileButton);

			NButton showExportDialogButton = new NButton("Show Export Dialog...");
			showExportDialogButton.Click += ShowExportDialogButton_Click;
			stack.Add(showExportDialogButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"
<p>
	This example demonstrates how to export images to the clipboard or file selected from an open file dialog.
	You can also use the <b>SaveToFile</b> and <b>SaveToStream</b> methods of the image exporter class to save
	to stream or file directly. The format of the image in this case is determined by the file extension.
</p>";
		}

        #endregion

        #region Event Handlers

        private void OnEnable3DCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_ChartView.Surface.Charts[0].Enable3D = (bool)arg.NewValue;
        }
        private void CopyImageToClipboardButton_Click(NEventArgs arg)
		{
			NEditorWindow.CreateForInstance(m_ChartView.Surface.Charts[0], null, null, null).Open();
			//NChartRasterImageExporter rasterImageExporter = new NChartRasterImageExporter(m_ChartView.Content);
			//rasterImageExporter.CopyToClipboard();
		}
		private void SaveAsImageFileButton_Click(NEventArgs arg)
		{
			NChartRasterImageExporter rasterImageExporter = new NChartRasterImageExporter(m_ChartView.Content);
			rasterImageExporter.SaveAsImage();

        }
		private void ShowExportDialogButton_Click(NEventArgs arg)
		{
			NChartRasterImageExporter rasterImageExporter = new NChartRasterImageExporter(m_ChartView.Content);
			rasterImageExporter.ShowDialog(m_ChartView.DisplayWindow, true);
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;

		#endregion

		#region Schema

		public static readonly NSchema NRasterImageExportExampleSchema;

		#endregion
	}
}