using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Range Example
	/// </summary>
	public class NStandardRangeExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardRangeExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardRangeExample()
		{
			NStandardRangeExampleSchema = NSchema.Create(typeof(NStandardRangeExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Range";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NLinearScale xScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			xScale.MajorGridLines.Visible = true;

			// setup Y axis
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.MajorGridLines.Visible = true;
			
			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			// setup shape series
			m_Range = new NRangeSeries();
			m_Chart.Series.Add(m_Range);

			m_Range.DataLabelStyle = new NDataLabelStyle(false);
			m_Range.UseXValues = true;
			m_Range.Fill = new NColorFill(NColor.DarkOrange);
			m_Range.Stroke = new NStroke(NColor.DarkRed);

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

				m_Range.DataPoints.Add(new NRangeDataPoint(x1, y1, x2, y2));
			}			

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox rangeShapeComboBox = new NComboBox();
			rangeShapeComboBox.FillFromEnum<ENBarShape>();
			rangeShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnRangeShapeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Range Shape: ", rangeShapeComboBox));

			NCheckBox showDataLabels = new NCheckBox("Show Data Labels");
			showDataLabels.CheckedChanged += new Function<NValueChangeEventArgs>(OnShowDataLabelsCheckedChanged);
			stack.Add(showDataLabels);

			rangeShapeComboBox.SelectedIndex = (int)ENBarShape.Rectangle;
			showDataLabels.Checked = false;

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard range chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnShowDataLabelsCheckedChanged(NValueChangeEventArgs arg)
		{
			if ((arg.TargetNode as NCheckBox).Checked)
			{
				m_Range.DataLabelStyle = new NDataLabelStyle(true);
				m_Range.DataLabelStyle.Format = "<y2>";
			}
			else
			{
				m_Range.DataLabelStyle = new NDataLabelStyle(false);
			}
		}

		void OnRangeShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Range.Shape = (ENBarShape)(arg.TargetNode as NComboBox).SelectedIndex;
		}

		#endregion

		#region Fields

		NRangeSeries m_Range;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardRangeExampleSchema;

		#endregion
	}
}