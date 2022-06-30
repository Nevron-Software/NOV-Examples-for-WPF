using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// XY Scatter Point Example
	/// </summary>
	public class NXYScatterPointExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYScatterPointExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYScatterPointExample()
		{
			NXYScatterPointExampleSchema = NSchema.Create(typeof(NXYScatterPointExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XY Scatter Point";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NLinearScale scaleX = new NLinearScale();
			scaleX.MajorGridLines = new NScaleGridLines();
			scaleX.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale = scaleX;

			// setup Y axis
			NLinearScale scaleY = new NLinearScale();
			scaleY.MajorGridLines = new NScaleGridLines();
			scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = scaleY;

			// add a point series
			m_Point = new NPointSeries();
			m_Point.Name = "Point Series";
			m_Point.DataLabelStyle = new NDataLabelStyle(false);
			m_Point.Fill = new NColorFill(new NColor(NColor.DarkOrange, 160));
			m_Point.Size = 5;
			m_Point.Shape = ENPointShape.Ellipse;
			m_Point.UseXValues = true;
			m_Chart.Series.Add(m_Point);

			OnNewDataButtonClick(null);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NCheckBox inflateMarginsCheckBox = new NCheckBox();
			inflateMarginsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInflateMarginsCheckBoxCheckedChanged);
			stack.Add(NPairBox.Create("Inflate Margins: ", inflateMarginsCheckBox));

			NCheckBox verticalAxisRoundToTick = new NCheckBox();
			verticalAxisRoundToTick.CheckedChanged += new Function<NValueChangeEventArgs>(OnAxesRoundToTickCheckedChanged);
			stack.Add(NPairBox.Create("Axes Round To Tick: ", verticalAxisRoundToTick));

			NButton newDataButton = new NButton("New Data");
			newDataButton.Click += new Function<NEventArgs>(OnNewDataButtonClick);
			stack.Add(newDataButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter point chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnNewDataButtonClick(NEventArgs arg)
		{
			m_Point.DataPoints.Clear();
			NDataPointCollection<NPointDataPoint> dataPoints = m_Point.DataPoints;

			Random random = new Random();

			for (int i = 0; i < 1000; i++)
			{
				double u1 = random.NextDouble();
				double u2 = random.NextDouble();

				if (u1 == 0)
					u1 += 0.0001;

				if(u2 == 0)
					u2 += 0.0001;

				double z0 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
				double z1 = Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);

				dataPoints.Add(new NPointDataPoint(z0, z1));
			}
		}

		void OnAxesRoundToTickCheckedChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				NLinearScale linearScale = m_Chart.Axes[i].Scale as NLinearScale;

				if (linearScale != null)
				{
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
			}
		}

		void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Point.InflateMargins = (arg.TargetNode as NCheckBox).Checked;
		}

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NXYScatterPointExampleSchema;

		#endregion
	}
}
