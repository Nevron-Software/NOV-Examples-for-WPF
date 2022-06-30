using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis grid lines example
	/// </summary>
	public class NAxisGridLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisGridLinesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisGridLinesExample()
		{
			NAxisGridLinesExampleSchema = NSchema.Create(typeof(NAxisGridLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Grid";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			scaleY.Strips.Add(strip);

			// enable the major y grid lines
			scaleY.MajorGridLines = new NScaleGridLines();

			NOrdinalScale scaleX = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			// enable the major x grid lines
			scaleX.MajorGridLines = new NScaleGridLines();

			// create dummy data
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bars";
			bar.DataLabelStyle = new NDataLabelStyle(false);
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(100)));
			}

			m_Chart.Series.Add(bar);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Y Axis Grid"));

			NColorBox yAxisGridColor = new NColorBox();
			yAxisGridColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnYAxisGridColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", yAxisGridColor));
			yAxisGridColor.SelectedColor = NColor.Black;

			NComboBox yAxisGridStyle = new NComboBox();
			yAxisGridStyle.FillFromEnum<ENDashStyle>();
			yAxisGridStyle.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnYAxisGridStyleSelectedIndexChanged);
			stack.Add(NPairBox.Create("Style:", yAxisGridStyle));
			yAxisGridStyle.SelectedIndex = (int)ENDashStyle.Solid;

			stack.Add(new NLabel("X Axis Grid"));

			NColorBox xAxisGridColor = new NColorBox();
			xAxisGridColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnXAxisGridColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", xAxisGridColor));
			xAxisGridColor.SelectedColor = NColor.Black;

			NComboBox xAxisGridStyle = new NComboBox();
			xAxisGridStyle.FillFromEnum<ENDashStyle>();
			xAxisGridStyle.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnXAxisGridStyleSelectedIndexChanged);
			stack.Add(NPairBox.Create("Style:", xAxisGridStyle));
			xAxisGridStyle.SelectedIndex = (int)ENDashStyle.Solid;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to configure the axis grid.</p>";
		}

		#endregion

		#region Event Handlers

		void OnXAxisGridStyleSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale.MajorGridLines.Stroke.DashStyle = (ENDashStyle)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnXAxisGridColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale.MajorGridLines.Stroke.Color = ((NColorBox)arg.TargetNode).SelectedColor;
		}

		void OnYAxisGridStyleSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.MajorGridLines.Stroke.DashStyle = (ENDashStyle)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnYAxisGridColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.MajorGridLines.Stroke.Color = ((NColorBox)arg.TargetNode).SelectedColor;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NAxisGridLinesExampleSchema;

		#endregion
	}
}