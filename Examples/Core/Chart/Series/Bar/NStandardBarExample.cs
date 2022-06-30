using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Bar Example
	/// </summary>
	public class NStandardBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardBarExample()
		{
			NStandardBarExampleSchema = NSchema.Create(typeof(NStandardBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Bar";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

            // add interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
            strip.Interlaced = true;
            //linearScale.Strips.Add(strip);

			// setup a bar series
			m_Bar = new NBarSeries();
			m_Bar.Name = "Bar Series";
			m_Bar.InflateMargins = true;
			m_Bar.UseXValues = false;

			m_Bar.Shadow = new NShadow(NColor.LightGray, 2, 2);

			// add some data to the bar series
			m_Bar.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			m_Bar.DataPoints.Add(new NBarDataPoint(18, "C++"));
			m_Bar.DataPoints.Add(new NBarDataPoint(15, "Ruby"));
			m_Bar.DataPoints.Add(new NBarDataPoint(21, "Python"));
			m_Bar.DataPoints.Add(new NBarDataPoint(23, "Java"));
			m_Bar.DataPoints.Add(new NBarDataPoint(27, "Javascript"));
			m_Bar.DataPoints.Add(new NBarDataPoint(29, "C#"));
			m_Bar.DataPoints.Add(new NBarDataPoint(26, "PHP"));

			chart.Series.Add(m_Bar);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox originModeComboBox = new NComboBox();
			originModeComboBox.FillFromEnum<ENSeriesOriginMode>();
			originModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOriginModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Origin Mode: ", originModeComboBox));

			NNumericUpDown customOriginUpDown = new NNumericUpDown();
			customOriginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnCustomOriginUpDownValueChanged);
			stack.Add(NPairBox.Create("Custom Origin: ", customOriginUpDown));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard bar chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnCustomOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Bar.CustomOrigin = (double)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnOriginModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Bar.OriginMode = (ENSeriesOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		NBarSeries m_Bar;

		#endregion

		#region Schema

		public static readonly NSchema NStandardBarExampleSchema;

		#endregion
	}
}
