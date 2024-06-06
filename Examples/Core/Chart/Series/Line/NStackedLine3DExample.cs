using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Chart.Tools;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Stacked Line 3D Example
	/// </summary>
	public class NStackedLine3DExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStackedLine3DExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStackedLine3DExample()
		{
			NStackedLine3DExampleSchema = NSchema.Create(typeof(NStackedLine3DExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Stacked Line 3D";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
            m_Chart.Enable3D = true;
            m_Chart.ModelWidth = 65;
            m_Chart.ModelHeight = 40;

            m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.Standard);
            m_Chart.Projection.SetPredefinedProjection(ENPredefinedProjection.Perspective2);
            m_Chart.LightModel.SetPredefinedLightModel(ENPredefinedLightModel.GlitterLeft);
			m_Chart.Interactor = new NInteractor(new NTrackballTool());

            // add interlaced stripe to the Y axis
            NScaleStrip stripStyle = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(stripStyle);

			// add the first line
			m_Line1 = CreateLineSeries("Line 1", ENMultiLineMode.Series);
			m_Line1.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			m_Chart.Series.Add(m_Line1);

			// add the second line
			m_Line2 = CreateLineSeries("Line 2", ENMultiLineMode.Stacked);
			m_Line2.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			m_Chart.Series.Add(m_Line2);

			// add the third line
			m_Line3 = CreateLineSeries("Line 3", ENMultiLineMode.Stacked);
			m_Line3.LegendView.Mode = ENSeriesLegendMode.SeriesVisibility;
			m_Chart.Series.Add(m_Line3);

			// positive data
			OnPositiveDataButtonClick(null);

			return chartViewWithCommandBars;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NButton positiveDataButton = new NButton("Positive Values");
			positiveDataButton.Click += new Function<NEventArgs>(OnPositiveDataButtonClick);
			stack.Add(positiveDataButton);

			NButton positiveAndNegativeDataButton = new NButton("Positive and Negative Values");
			positiveAndNegativeDataButton.Click += new Function<NEventArgs>(OnPositiveAndNegativeDataButtonClick);
			stack.Add(positiveAndNegativeDataButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a stacked line chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnPositiveAndNegativeDataButtonClick(NEventArgs arg)
		{
			m_Line1.DataPoints.Clear();
			m_Line2.DataPoints.Clear();
			m_Line3.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 12; i++)
			{
				m_Line1.DataPoints.Add(new NLineDataPoint(random.Next(100) - 50));
				m_Line2.DataPoints.Add(new NLineDataPoint(random.Next(100) - 50));
				m_Line3.DataPoints.Add(new NLineDataPoint(random.Next(100) - 50));
			}
		}

		void OnPositiveDataButtonClick(NEventArgs arg)
		{
			m_Line1.DataPoints.Clear();
			m_Line2.DataPoints.Clear();
			m_Line3.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 12; i++)
			{
				m_Line1.DataPoints.Add(new NLineDataPoint(random.Next(90) + 10));
				m_Line2.DataPoints.Add(new NLineDataPoint(random.Next(90) + 10));
				m_Line3.DataPoints.Add(new NLineDataPoint(random.Next(90) + 10));
			}
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates a new line series
		/// </summary>
		/// <param name="name"></param>
		/// <param name="multiLineMode"></param>
		/// <returns></returns>
		private NLineSeries CreateLineSeries(string name, ENMultiLineMode multiLineMode)
		{
			NLineSeries line = new NLineSeries();

			line.Name = name;
			line.LineSegmentShape = ENLineSegmentShape.Tape;

            line.MultiLineMode = multiLineMode;

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.ArrowLength = 0;
			line.DataLabelStyle = dataLabelStyle;

			return line;
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		NLineSeries m_Line1;
		NLineSeries m_Line2;
		NLineSeries m_Line3;

		#endregion

		#region Schema

		public static readonly NSchema NStackedLine3DExampleSchema;

		#endregion
	}
}
