using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XY Scatter Line Example
	/// </summary>
	public class NXYScatterLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYScatterLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYScatterLineExample()
		{
			NXYScatterLineExampleSchema = NSchema.Create(typeof(NXYScatterLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XY Scatter Line";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlaced stripe to the Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(stripStyle);

			m_Line = new NLineSeries();
			chart.Series.Add(m_Line);

			m_Line.DataLabelStyle = new NDataLabelStyle(false);
			m_Line.InflateMargins = true;

			NMarkerStyle markerStyle = new NMarkerStyle();
			m_Line.MarkerStyle = markerStyle;

			markerStyle.Visible = true;
			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(10, 10);
			markerStyle.Fill = new NColorFill(ENNamedColor.Red);

			m_Line.Name = "Line Series";
			m_Line.UseXValues = true;

			// add xy values
			m_Line.DataPoints.Add(new NLineDataPoint(15, 10));
			m_Line.DataPoints.Add(new NLineDataPoint(25, 23));
			m_Line.DataPoints.Add(new NLineDataPoint(45, 12));
			m_Line.DataPoints.Add(new NLineDataPoint(55, 21));
			m_Line.DataPoints.Add(new NLineDataPoint(61, 16));
			m_Line.DataPoints.Add(new NLineDataPoint(67, 19));
			m_Line.DataPoints.Add(new NLineDataPoint(72, 11));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NButton changeXValuesButton = new NButton("Change X Values");
			changeXValuesButton.Click += OnChangeXValuesButtonClick;
			stack.Add(changeXValuesButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter line chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnChangeXValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();
			m_Line.DataPoints[0].X = random.Next(10);

			for (int i = 1; i < m_Line.DataPoints.Count; i++)
			{
				m_Line.DataPoints[i].X = m_Line.DataPoints[i - 1].X + random.Next(1, 10);
			}
		}

		#endregion

		#region Fields

		NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NXYScatterLineExampleSchema;

		#endregion
	}
}
