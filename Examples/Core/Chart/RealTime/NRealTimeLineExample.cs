using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Real Time Line Example.
	/// </summary>
	public class NRealTimeLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRealTimeLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRealTimeLineExample()
		{
			NRealTimeLineExampleSchema = NSchema.Create(typeof(NRealTimeLineExample), NExampleBaseSchema);
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
			chartView.Surface.Titles[0].Text = "Real Time Line";

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

			m_Line = new NLineSeries();
			m_Line.Name = "Line Series";
			m_Line.InflateMargins = true;
			m_Line.DataLabelStyle = new NDataLabelStyle(false);
			m_Line.MarkerStyle = new NMarkerStyle(new NSize(4, 4));
			m_Line.UseXValues = true;
			m_CurXValue = 0;

			m_Chart.Series.Add(m_Line);

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
			return @"<p>This example demonstrates how to create a line chart that updates in real time.</p>";
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
			if (m_Line.DataPoints.Count < dataPointCount)
			{
				m_Line.DataPoints.Add(new NLineDataPoint(m_CurXValue++, m_Random.Next(80) + 20));
			}
			else
			{
				m_Line.DataPoints[m_Line.OriginIndex].X = m_CurXValue++;
				m_Line.DataPoints[m_Line.OriginIndex].Value = m_Random.Next(80) + 20;

				m_Line.OriginIndex++;

				if (m_Line.OriginIndex >= m_Line.DataPoints.Count)
				{
					m_Line.OriginIndex = 0;
				}
			}			
		}

		private void OnResetButtonClick(NEventArgs arg)
		{
			m_Line.DataPoints.Clear();
			m_Line.OriginIndex = 0;
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

		NCartesianChart m_Chart;
		NLineSeries m_Line;

		Random m_Random;
		NTimer m_Timer;
		int m_CurXValue;

		#endregion

		#region Schema

		public static readonly NSchema NRealTimeLineExampleSchema;

		#endregion
	}
}