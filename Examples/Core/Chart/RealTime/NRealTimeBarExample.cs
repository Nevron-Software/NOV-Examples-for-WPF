using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Real Time Bar Example.
	/// </summary>
	public class NRealTimeBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRealTimeBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRealTimeBarExample()
		{
			NRealTimeBarExampleSchema = NSchema.Create(typeof(NRealTimeBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			chartView.Registered += OnChartViewRegistered;
			chartView.Unregistered += OnChartViewUnregistered;

			// configure title
			chartView.Surface.Titles[0].Text = "Real Time Bar";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleY.InflateViewRangeBegin = false;
			scaleY.InflateViewRangeEnd = false;

			// add interlaced stripe to the Y axis
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			m_Random = new Random();

			m_Bar = new NBarSeries();
			m_Bar.Name = "Line Series";
			m_Bar.InflateMargins = true;
			m_Bar.DataLabelStyle = new NDataLabelStyle(false);
			m_Bar.UseXValues = true;
			m_Bar.WidthMode = ENBarWidthMode.ScaleWidth;
			m_Bar.Width = 0.5;
			m_CurXValue = 0;

			m_Chart.Series.Add(m_Bar);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NButton toggleTimerButton = new NButton("Stop Timer");
			toggleTimerButton.Click += OnToggleTimerButtonClick;
			toggleTimerButton.Tag = 0;
			stack.Add(toggleTimerButton);

			NButton resetButton = new NButton("Reset Data");
			resetButton.Click += OnResetButtonClick;
			stack.Add(resetButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a bar chart that updates in real time.</p>";
		}

		#endregion

		#region Event Handlers

		private void OnChartViewRegistered(NEventArgs arg)
		{
			m_Timer = new NTimer();
			m_Timer.Tick += OnTimerTick;
			m_Timer.Start();
		}

		private void OnChartViewUnregistered(NEventArgs arg)
		{
			m_Timer.Stop();
			m_Timer.Tick -= OnTimerTick;
			m_Timer = null;
		}

		private void OnTimerTick()
		{
			const int dataPointCount = 40;
			if (m_Bar.DataPoints.Count < dataPointCount)
			{
				m_Bar.DataPoints.Add(new NBarDataPoint(m_CurXValue++, m_Random.Next(80) + 20));
			}
			else
			{
				m_Bar.DataPoints[m_Bar.OriginIndex].X = m_CurXValue++;
				m_Bar.DataPoints[m_Bar.OriginIndex].Value = m_Random.Next(80) + 20;

				m_Bar.OriginIndex++;

				if (m_Bar.OriginIndex >= m_Bar.DataPoints.Count)
				{
					m_Bar.OriginIndex = 0;
				}
			}			
		}

		private void OnResetButtonClick(NEventArgs arg)
		{
			m_Bar.DataPoints.Clear();
			m_Bar.OriginIndex = 0;
			m_CurXValue = 0;
		}

		private void OnToggleTimerButtonClick(NEventArgs arg)
		{
			NButton button = (NButton)arg.TargetNode;
			if ((int)button.Tag == 0)
			{
				m_Timer.Stop();

				button.Content = new NLabel("Start Timer");
				button.Tag = 1;
			}
			else
			{
				m_Timer.Start();
				button.Content = new NLabel("Stop Timer");
				button.Tag = 0;
			}
		}

		#endregion

		#region Fields

		private NCartesianChart m_Chart;
		private NBarSeries m_Bar;

		private Random m_Random;
		private NTimer m_Timer;
		private int m_CurXValue;

		#endregion

		#region Schema

		public static readonly NSchema NRealTimeBarExampleSchema;

		#endregion
	}
}