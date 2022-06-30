using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard HighLow Example
	/// </summary>
	public class NStandardHighLowExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardHighLowExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardHighLowExample()
		{
			NStandardHighLowExampleSchema = NSchema.Create(typeof(NStandardHighLowExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard High Low";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlace stripe
			NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;

			m_HighLow = new NHighLowSeries();
			m_HighLow.Name = "High-Low Series";
			m_HighLow.HighFill = new NColorFill(NColor.Gray);
			m_HighLow.LowFill = new NColorFill(NColor.Orange);
			m_HighLow.HighStroke = new NStroke(2, NColor.Black);
			m_HighLow.LowStroke = new NStroke(2, NColor.Red);
			m_HighLow.Stroke = new NStroke(2, NColor.Black);
			m_HighLow.DataLabelStyle = new NDataLabelStyle(false);
			m_HighLow.Palette = new NTwoColorPalette(NColor.Red, NColor.Green);

			GenerateData();

			m_Chart.Series.Add(m_HighLow);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
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
			return @"<p>This example demonstrates how to create a standard high low series.</p>";
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

		public static readonly NSchema NStandardHighLowExampleSchema;

		#endregion
	}
}
