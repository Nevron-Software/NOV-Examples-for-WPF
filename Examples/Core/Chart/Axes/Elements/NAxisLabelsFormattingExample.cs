using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis labels orientation example
	/// </summary>
	public class NAxisLabelsFormattingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisLabelsFormattingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisLabelsFormattingExample()
		{
			NAxisLabelsFormattingExampleSchema = NSchema.Create(typeof(NAxisLabelsFormattingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Labels Formatting";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			//linearScale.Strips.Add(strip);

			// setup a bar series
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bar Series";
			bar.InflateMargins = true;
			bar.UseXValues = false;
			bar.DataLabelStyle = new NDataLabelStyle(false);

			// add some data to the bar series
			bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			bar.DataPoints.Add(new NBarDataPoint(18, "C++"));
			bar.DataPoints.Add(new NBarDataPoint(15, "Ruby"));
			bar.DataPoints.Add(new NBarDataPoint(21, "Python"));
			bar.DataPoints.Add(new NBarDataPoint(23, "Java"));
			bar.DataPoints.Add(new NBarDataPoint(27, "Javascript"));
			bar.DataPoints.Add(new NBarDataPoint(29, "C#"));
			bar.DataPoints.Add(new NBarDataPoint(26, "PHP"));

			m_Chart.Series.Add(bar);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NNumericUpDown yAxisDecimalPlaces = new NNumericUpDown();
			yAxisDecimalPlaces.Minimum = 0;
			yAxisDecimalPlaces.ValueChanged += OnYAxisDecimalPlacesValueChanged;
			stack.Add(NPairBox.Create("Y Axis Decimal Places:", yAxisDecimalPlaces));

			NCheckBox useCustomXAxisLabels = new NCheckBox("Use Custom X Axis Labels");
			useCustomXAxisLabels.CheckedChanged += OnUseCustomXAxisLabelsCheckedChanged;
			stack.Add(useCustomXAxisLabels);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to apply different formatting to axis labels.</p>";
		}

		#endregion

		#region Event Handlers

		void OnUseCustomXAxisLabelsCheckedChanged(NValueChangeEventArgs arg)
		{
			NOrdinalScale ordinalScale = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			if (((NCheckBox)arg.TargetNode).Checked)
			{
				ordinalScale.Labels.TextProvider = new NOrdinalScaleLabelTextProvider(new string[] { "C++",
																									 "Ruby",
																									 "Python",
																									 "Java",
																									 "Javascript",
																									 "C#",
																									 "PHP"});
			}
			else
			{
				ordinalScale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter(ENNumericValueFormat.Arabic));
			}
		}

		void OnYAxisDecimalPlacesValueChanged(NValueChangeEventArgs arg)
		{
			int decimalPlaces = (int)((NNumericUpDown)arg.TargetNode).Value;

			string format = ".";
			for (int i = 0; i < decimalPlaces; i++)
			{
				format += "0";
			}

			NLinearScale linearScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			linearScale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter(format));
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;
		
		#endregion

		#region Schema

		public static readonly NSchema NAxisLabelsFormattingExampleSchema;

		#endregion
	}
}