using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Logarithmic Scale Example
	/// </summary>
	public class NLogarithmicScaleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLogarithmicScaleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLogarithmicScaleExample()
		{
			NLogarithmicScaleExampleSchema = NSchema.Create(typeof(NLogarithmicScaleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Logarithmic Scale";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			NLogarithmicScale logarithmicScale = new NLogarithmicScale();

			logarithmicScale.MinorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			logarithmicScale.MinorTickCount = 3;
			logarithmicScale.MajorTickMode = ENMajorTickMode.CustomStep;

			// add interlaced stripe 
			NScaleStrip strip = new NScaleStrip();
			strip.Fill = new NColorFill(NColor.Beige);
			strip.Interlaced = true;
			logarithmicScale.Strips.Add(strip);

			logarithmicScale.CustomStep = 1;

			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = logarithmicScale;

			NLineSeries line = new NLineSeries();
			m_Chart.Series.Add(line);

			line.LegendView.Mode = ENSeriesLegendMode.None;
			line.InflateMargins = false;

			NMarkerStyle markerStyle = new NMarkerStyle();
			line.MarkerStyle = markerStyle;

			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(15, 15);

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = true;
			dataLabelStyle.Format = "<value>";
			line.DataLabelStyle = dataLabelStyle;

			line.DataPoints.Add(new NLineDataPoint(12));
			line.DataPoints.Add(new NLineDataPoint(100));
			line.DataPoints.Add(new NLineDataPoint(250));
			line.DataPoints.Add(new NLineDataPoint(500));
			line.DataPoints.Add(new NLineDataPoint(1500));
			line.DataPoints.Add(new NLineDataPoint(5500));
			line.DataPoints.Add(new NLineDataPoint(9090));

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NNumericUpDown logarithmBaseUpDown = new NNumericUpDown();
 			logarithmBaseUpDown.Minimum = 1;
			logarithmBaseUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnLogarithmBaseUpDownValueChanged);
			stack.Add(NPairBox.Create("Logarithm Base:", logarithmBaseUpDown));

			NCheckBox invertedCheckBox = new NCheckBox("Inverted");
			invertedCheckBox.CheckedChanged += OnInvertedCheckBoxCheckedChanged;
			invertedCheckBox.Checked = false;
			stack.Add(invertedCheckBox);

			logarithmBaseUpDown.Value = 10;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a logarithmic scale.</p>";
		}

		#endregion

		#region Event Handlers

		void OnLogarithmBaseUpDownValueChanged(NValueChangeEventArgs arg)
		{
			NLogarithmicScale logScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLogarithmicScale;

			logScale.LogarithmBase = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnInvertedCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			((NLogarithmicScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NLogarithmicScaleExampleSchema;

		#endregion
	}
}
