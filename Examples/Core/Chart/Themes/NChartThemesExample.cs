using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart Themes Example
	/// </summary>
	public class NChartThemesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NChartThemesExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NChartThemesExample()
		{
			NChartThemesExampleSchema = NSchema.Create(typeof(NChartThemesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			m_ChartView = chartViewWithCommandBars.View;
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "Chart Themes";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			// add interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// add a bar series
			Random random = new Random();
			for (int i = 0; i < 6; i++)
			{
				NBarSeries bar = new NBarSeries();
				bar.Name = "Bar" + i.ToString();
				bar.MultiBarMode = ENMultiBarMode.Clustered;
				bar.DataLabelStyle = new NDataLabelStyle(false);
				bar.ValueFormatter = new NNumericValueFormatter("0.###");
				chart.Series.Add(bar);

				for (int j = 0; j < 6; j++)
				{
					bar.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
				}
			}

			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme(DefaultChartPalette, ENChartPaletteTarget.Series));

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();

			m_ChartPaletteComboBox = new NComboBox();
			m_ChartPaletteComboBox.FillFromEnum<ENChartPalette>();
			m_ChartPaletteComboBox.SelectedIndexChanged += OnChartPaletteComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Palette:", m_ChartPaletteComboBox));

			m_ChartPaletteTargetComboBox = new NComboBox();
			m_ChartPaletteTargetComboBox.FillFromEnum<ENChartPaletteTarget>();
			m_ChartPaletteTargetComboBox.SelectedIndexChanged += OnChartPaletteComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Target:", m_ChartPaletteTargetComboBox));

			m_ChartPaletteComboBox.SelectedIndex = (int)DefaultChartPalette;
			m_ChartPaletteTargetComboBox.SelectedIndex = (int)ENChartPaletteTarget.DataPoints;

			return new NUniSizeBoxGroup(stack);
		}

		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to apply different chart color themes.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnChartPaletteComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_ChartView.Document.StyleSheets.ApplyTheme(new NChartTheme((ENChartPalette)m_ChartPaletteComboBox.SelectedIndex, (ENChartPaletteTarget)m_ChartPaletteTargetComboBox.SelectedIndex));
		}

		#endregion

		#region Fields

		private NChartView m_ChartView;
		private NComboBox m_ChartPaletteComboBox;
		private NComboBox m_ChartPaletteTargetComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NChartThemesExampleSchema;

		#endregion

		#region Constants

		private const ENChartPalette DefaultChartPalette = ENChartPalette.Autumn;

		#endregion
	}
}