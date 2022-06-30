using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Chart orientation example
	/// </summary>
	public class NChartOrientationExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NChartOrientationExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NChartOrientationExample()
		{
			NChartOrientationExampleSchema = NSchema.Create(typeof(NChartOrientationExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Chart Orientation";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.Orientation = ENCartesianChartOrientation.LeftToRight;
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// add a bar series
			m_Bar1 = new NBarSeries();
			m_Bar1.MultiBarMode = ENMultiBarMode.Series;
			m_Bar1.Name = "Bar 1";

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Format = "<value>";
			dataLabelStyle.Visible = true;

			m_Bar1.DataLabelStyle = dataLabelStyle;

			m_Bar1.LegendView.Mode = ENSeriesLegendMode.DataPoints;

			m_Chart.Series.Add(m_Bar1);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			OnPositiveDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox orientationComboBox = new NComboBox();
			orientationComboBox.FillFromEnum<ENCartesianChartOrientation>();
			orientationComboBox.SelectedIndex = (int)m_Chart.Orientation;
			orientationComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOrientationComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Orientation:", orientationComboBox));

			NButton positiveDataButton = new NButton("Positive Values");
			positiveDataButton.Click += new Function<NEventArgs>(OnPositiveDataButtonClick);
			stack.Add(positiveDataButton);

			NButton positiveAndNegativeDataButton = new NButton("Positive and Negative Values");
			positiveAndNegativeDataButton.Click += new Function<NEventArgs>(OnPositiveAndNegativeDataButtonClick);
			stack.Add(positiveAndNegativeDataButton);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how change the chart orientation. This feature allows you to display left to right and right to left charts</p>";
		}

		#endregion

		#region Event Handlers

		void OnOrientationComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Orientation = (ENCartesianChartOrientation)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnPositiveAndNegativeDataButtonClick(NEventArgs arg)
		{
			m_Bar1.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 12; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(100) - 50));
			}
		}

		void OnPositiveDataButtonClick(NEventArgs arg)
		{
			m_Bar1.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 12; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(90) + 10));
			}
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NBarSeries m_Bar1;

		#endregion

		#region Schema

		public static readonly NSchema NChartOrientationExampleSchema;

		#endregion
	}
}