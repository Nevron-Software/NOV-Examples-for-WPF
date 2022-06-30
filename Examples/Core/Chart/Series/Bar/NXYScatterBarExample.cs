using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XY Scatter Bar Example
	/// </summary>
	public class NXYScatterBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYScatterBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYScatterBarExample()
		{
			NXYScatterBarExampleSchema = NSchema.Create(typeof(NXYScatterBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XY Scatter Bar";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlaced stripe to the Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(stripStyle);

			m_Bar = new NBarSeries();
			chart.Series.Add(m_Bar);

			m_Bar.DataLabelStyle = new NDataLabelStyle(false);
			m_Bar.InflateMargins = true;
			m_Bar.WidthMode = ENBarWidthMode.FixedWidth;
			m_Bar.Width = 20;

			m_Bar.Name = "Bar Series";
			m_Bar.UseXValues = true;

			// add xy values
			m_Bar.DataPoints.Add(new NBarDataPoint(15, 10));
			m_Bar.DataPoints.Add(new NBarDataPoint(25, 23));
			m_Bar.DataPoints.Add(new NBarDataPoint(45, 12));
			m_Bar.DataPoints.Add(new NBarDataPoint(55, 21));
			m_Bar.DataPoints.Add(new NBarDataPoint(61, 16));
			m_Bar.DataPoints.Add(new NBarDataPoint(67, 19));
			m_Bar.DataPoints.Add(new NBarDataPoint(72, 11));

			return chartView;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
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
			return @"<p>This example demonstrates how to create a xy scatter bar chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnChangeXValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();
			m_Bar.DataPoints[0].X = random.Next(10);

			for (int i = 1; i < m_Bar.DataPoints.Count; i++)
			{
				m_Bar.DataPoints[i].X = m_Bar.DataPoints[i - 1].X + random.Next(1, 10);
			}
		}

		#endregion

		#region Fields

		NBarSeries m_Bar;

		#endregion

		#region Schema

		public static readonly NSchema NXYScatterBarExampleSchema;

		#endregion
	}
}
