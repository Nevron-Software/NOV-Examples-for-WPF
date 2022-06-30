using System;

using Nevron.Nov.Chart;
using Nevron.Nov.DataStructures;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Combo Chart Example
	/// </summary>
	public class NComboChartExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NComboChartExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NComboChartExample()
		{
			NComboChartExampleSchema = NSchema.Create(typeof(NComboChartExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Combo Chart";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// Setup the primary Y axis
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Title.Text = "Number of Occurences";

			// add interlace stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			// Setup the secondary Y axis
			NLinearScale scaleY2 = new NLinearScale();
			scaleY2.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter("0%"));
			scaleY2.Title.Text = "Cumulative Percent";

			NCartesianAxis axisY2 = new NCartesianAxis();
			m_Chart.Axes.Add(axisY2);

			axisY2.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Right);
			axisY2.Visible = true;
			axisY2.Scale = scaleY2;
			axisY2.ViewRangeMode = ENAxisViewRangeMode.FixedRange;
			axisY2.MinViewRangeValue = 0;
			axisY2.MaxViewRangeValue = 1;

			// add the bar series
			NBarSeries bar = new NBarSeries();
			m_Chart.Series.Add(bar);
			bar.Name = "Bar Series";
			bar.DataLabelStyle = new NDataLabelStyle(false);

			// add the line series
			NLineSeries line = new NLineSeries();
			m_Chart.Series.Add(line);
			line.Name = "Cumulative %";
			line.DataLabelStyle = new NDataLabelStyle(false);

			NMarkerStyle markerStyle = new NMarkerStyle();
			markerStyle.Visible = true;
			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(10, 10);
			markerStyle.Fill = new NColorFill(NColor.Orange);

			line.MarkerStyle = markerStyle;

			line.VerticalAxis = axisY2;

			// fill with random data and sort in descending order
			int count = 10;
			NList<double> randomValues = new NList<double>();
			Random random = new Random();
			for (int i = 0; i < count; i++)
			{
				randomValues.Add(random.Next(100, 700));
			}

			randomValues.Sort();

			for (int i = 0; i < randomValues.Count; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(randomValues[i]));
			}

			// calculate cumulative sum of the bar values
			double cs = 0;
			double[] arrCumulative = new double[count];

			for (int i = 0; i < count; i++)
			{
				cs += randomValues[i];
				arrCumulative[i] = cs;
			}

			if (cs > 0)
			{
				for (int i = 0; i < count; i++)
				{
					arrCumulative[i] /= cs;
					line.DataPoints.Add(new NLineDataPoint(arrCumulative[i]));
				}
			}

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a combo chart consisting of two series (NOV Chart supports an unlimited number of series).</p>";
		}

		#endregion

		#region Implementation

		private NComboBox CreateLegendFormatCombo()
		{
			NComboBox comboBox = new NComboBox();

			NComboBoxItem item = new NComboBoxItem("Value and Label");
			item.Tag = "<value> <label>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Value");
			item.Tag = "<value>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Label");
			item.Tag = "<label>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Size");
			item.Tag = "<size>";
			comboBox.Items.Add(item);

			return comboBox;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NComboChartExampleSchema;

		#endregion
	}
}