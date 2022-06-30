using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XY Scatter Area Example
	/// </summary>
	public class NXYScatterAreaExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYScatterAreaExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYScatterAreaExample()
		{
			NXYScatterAreaExampleSchema = NSchema.Create(typeof(NXYScatterAreaExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			m_ChartView = new NChartView();
			m_ChartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			m_ChartView.Surface.Titles[0].Text = "XY Scatter Area";

			// configure chart
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

            // add interlace stripe
			NLinearScale linearScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
            NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
            stripStyle.Interlaced = true;
            linearScale.Strips.Add(stripStyle);

            // show the x axis grid lines
			linearScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			NScaleGridLines majorGrid = new NScaleGridLines();
			majorGrid.Visible = true;
			linearScale.MajorGridLines = majorGrid;

			// add the area series
			m_Area = new NAreaSeries();
			m_Area.Name = "Area Series";

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = true;
			dataLabelStyle.ArrowStroke.Width = 0;
			dataLabelStyle.Format = "<value>";
			m_Area.DataLabelStyle = dataLabelStyle;

			m_Area.UseXValues = true;

			// add xy values
			m_Area.DataPoints.Add(new NAreaDataPoint(12, 10));
			m_Area.DataPoints.Add(new NAreaDataPoint(25, 23));
			m_Area.DataPoints.Add(new NAreaDataPoint(45, 12));
			m_Area.DataPoints.Add(new NAreaDataPoint(55, 24));
			m_Area.DataPoints.Add(new NAreaDataPoint(61, 16));
			m_Area.DataPoints.Add(new NAreaDataPoint(69, 19));
			m_Area.DataPoints.Add(new NAreaDataPoint(78, 17));

			chart.Series.Add(m_Area);

			return m_ChartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NButton changeXValuesButton = new NButton("Change X Values");
			changeXValuesButton.Click +=new Function<NEventArgs>(OnChangeXValuesButtonClick);
			stack.Add(changeXValuesButton);

			NCheckBox xAxisRoundToTickCheckBox = new NCheckBox("X Axis Round To Tick");
			xAxisRoundToTickCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnXAxisRoundToTickCheckBoxCheckedChanged);
			stack.Add(xAxisRoundToTickCheckBox);

			NCheckBox invertXAxisCheckBox = new NCheckBox("Invert X Axis");
			invertXAxisCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInvertXAxisCheckBoxCheckedChanged);
			stack.Add(invertXAxisCheckBox);

			NCheckBox invertYAxisCheckBox = new NCheckBox("Invert Y Axis");
			invertYAxisCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInvertYAxisCheckBoxCheckedChanged);
			stack.Add(invertYAxisCheckBox);

		
			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create an xy scatter area chart.</p>";
		}

		#endregion

		#region Implementation

		#endregion

		#region Event Handlers

		void OnChangeXValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();
			int dataPointCount = m_Area.DataPoints.Count;
			m_Area.DataPoints[0].X = (double)random.Next(10);

			for (int i = 1; i < dataPointCount; i++)
			{
				m_Area.DataPoints[i].X = m_Area.DataPoints[i - 1].X + random.Next(1, 10);
			}
		}

		void OnInvertXAxisCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
			chart.Axes[ENCartesianAxis.PrimaryX].Scale.Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnInvertYAxisCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.Invert = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnXAxisRoundToTickCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			NCartesianChart chart = (NCartesianChart)m_ChartView.Surface.Charts[0];
			NLinearScale linearScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			if ((arg.TargetNode as NCheckBox).Checked)
			{
				linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
				linearScale.InflateViewRangeBegin = true;
				linearScale.InflateViewRangeEnd = true;
			}
			else
			{
				linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.Logical;
			}

		}

		#endregion

		#region Fields

		private NChartView m_ChartView;
		private NAreaSeries m_Area;

		#endregion

		#region Schema

		public static readonly NSchema NXYScatterAreaExampleSchema;

		#endregion
	}
}