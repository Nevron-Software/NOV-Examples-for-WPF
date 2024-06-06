using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
    /// <summary>
    /// HighLow 3D Example.
    /// </summary>
    public class NHighLow3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NHighLow3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NHighLow3DExample()
		{
            NHighLow3DExampleSchema = NSchema.Create(typeof(NHighLow3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard 3D High Low";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.Enable3D = true;
            m_Chart.Enable3D = true;
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.BrightCameraLight);
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.PerspectiveTilted);
            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);
            m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // add interlace stripe
            NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;

			m_HighLow = new NHighLowSeries();
			m_HighLow.Name = "High-Low Series";
			m_HighLow.HighFill = new NColorFill(NColor.LightGreen);
			m_HighLow.LowFill = new NColorFill(NColor.Crimson);

			m_HighLow.HighStroke = new NStroke(2, NColor.DarkGreen);
			m_HighLow.LowStroke = new NStroke(2, NColor.DarkRed);
			m_HighLow.Stroke = new NStroke(2, NColor.Black);
			m_HighLow.DataLabelStyle = new NDataLabelStyle(false);
			m_HighLow.Palette = new NTwoColorPalette(NColor.Red, NColor.Green);

			GenerateData();

			m_Chart.Series.Add(m_HighLow);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox appearanceModeComboBox = new NComboBox();
			appearanceModeComboBox.FillFromEnum<ENHighLowAppearanceMode>();
			appearanceModeComboBox.SelectedIndexChanged += OnAppearanceModeComboBoxSelectedIndexChanged;
			appearanceModeComboBox.SelectedIndex = (int)ENHighLowAppearanceMode.HighLow;
			stack.Add(NPairBox.Create("Appearance Mode:", appearanceModeComboBox));

			NCheckBox showDropLinesCheckBox = new NCheckBox("Show Droplines");
			showDropLinesCheckBox.CheckedChanged += OnShowDropLinesCheckBoxCheckedChanged;
			stack.Add(showDropLinesCheckBox);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a 3D high low chart.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateData()
		{
			m_HighLow.DataPoints.Clear();

			Random random = new Random();

			for (int i = 0; i < 20; i++)
			{
				double d1 = Math.Log(i + 1) + 0.1 * random.NextDouble();
				double d2 = d1 + Math.Cos(0.33 * i) + 0.1 * random.NextDouble();

				m_HighLow.DataPoints.Add(new NHighLowDataPoint(d1, d2));
			}
		}

		#endregion

		#region Event Handlers

		void OnAppearanceModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_HighLow.AppearanceMode = (ENHighLowAppearanceMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnShowDropLinesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_HighLow.ShowDropLines = ((NCheckBox)arg.TargetNode).Checked;
		}

		#endregion

		#region Fields

		NHighLowSeries m_HighLow;
		NCartesianChart m_Chart;

        #endregion

        #region Schema

        /// <summary>
        /// Schema associated with NHighLow3DExample.
        /// </summary>
        public static readonly NSchema NHighLow3DExampleSchema;

		#endregion
	}
}
