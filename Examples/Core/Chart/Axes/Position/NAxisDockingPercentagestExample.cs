using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis docking example
	/// </summary>
	public class NAxisDockingPercentagestExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisDockingPercentagestExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisDockingPercentagestExample()
		{
			NAxisDockingPercentagestExampleSchema = NSchema.Create(typeof(NAxisDockingPercentagestExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Docking Percentages";

			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_RedAxis = CreateLinearAxis(ENCartesianAxisDockZone.Left, NColor.Red);
			chart.Axes.Add(m_RedAxis);

			m_GreenAxis = CreateLinearAxis(ENCartesianAxisDockZone.Right, NColor.Green);
			chart.Axes.Add(m_GreenAxis);

			// Add a custom vertical axis
			m_BlueAxis = CreateLinearAxis(ENCartesianAxisDockZone.Left, NColor.Blue);
			chart.Axes.Add(m_BlueAxis);

			chart.Axes.Add(NCartesianChart.CreateDockedAxis(ENCartesianAxisDockZone.Bottom, ENScaleType.Orindal));

			// create three line series and dispay them on three vertical axes (red, green and blue axis)
			NLineSeries line1 = CreateLineSeries(NColor.Red, NColor.DarkRed, 10, 20);
			chart.Series.Add(line1);

			NLineSeries line2 = CreateLineSeries(NColor.Green, NColor.DarkGreen, 50, 100);
			chart.Series.Add(line2);

			NLineSeries line3 = CreateLineSeries(NColor.Blue, NColor.DarkBlue, 100, 200);
			chart.Series.Add(line3);

			line1.VerticalAxis = m_RedAxis;
			line2.VerticalAxis = m_GreenAxis;
			line3.VerticalAxis = m_BlueAxis;

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_RedAxisEndPercentUpDown = new NNumericUpDown();
			m_RedAxisEndPercentUpDown.Minimum = 10;
			m_RedAxisEndPercentUpDown.Maximum = 60;
			m_RedAxisEndPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnRedAxisEndPercentUpDownValueChanged);
			stack.Add(NPairBox.Create("Red Axis End Percent:", m_RedAxisEndPercentUpDown));

			m_BlueAxisBeginPercentUpDown = new NNumericUpDown();
			m_BlueAxisBeginPercentUpDown.Minimum = 20;
			m_BlueAxisBeginPercentUpDown.Maximum = 90;
			m_BlueAxisBeginPercentUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnBlueAxisEndPercentUpDownValueChanged);
			stack.Add(NPairBox.Create("Blue Axis Begin Percent:", m_BlueAxisBeginPercentUpDown));

			m_RedAxisEndPercentUpDown.Value = 30;
			m_BlueAxisBeginPercentUpDown.Value = 70;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to change the area occupied by an axis when docked in an axis dock zone.</p>";
		}

		#endregion

		#region Implementation

		private NCartesianAxis CreateLinearAxis(ENCartesianAxisDockZone dockZone, NColor color)
		{
			NCartesianAxis axis = new NCartesianAxis();

			axis.Scale = CreateLinearScale(color);

			axis.Anchor = new NDockCartesianAxisAnchor(dockZone, false);

			return axis;
		}
		private NLinearScale CreateLinearScale(NColor color)
		{
			NLinearScale linearScale = new NLinearScale();

			linearScale.Ruler.Stroke = new NStroke(1, color);
			linearScale.InnerMajorTicks.Stroke = new NStroke(color);
			linearScale.OuterMajorTicks.Stroke = new NStroke(color);
			linearScale.Labels.Style.TextStyle.Fill = new NColorFill(color);

			NScaleGridLines grid = new NScaleGridLines();
			grid.Stroke.Color = color;
			grid.Visible = true;
			linearScale.MajorGridLines = grid;

			NScaleStrip strip = new NScaleStrip();
			strip.Length = 1;
			strip.Interval = 1;
			strip.Fill = new NColorFill(new NColor(color, 50));
			linearScale.Strips.Add(strip);

			return linearScale;
		}
		private NLineSeries CreateLineSeries(NColor lightColor, NColor color, int begin, int end)
		{
			// Add a line series
			NLineSeries line = new NLineSeries();

			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				line.DataPoints.Add(new NLineDataPoint(random.Next(begin, end)));
			}

			line.Stroke = new NStroke(2, color);

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Format = "<value>";
			dataLabelStyle.TextStyle.Background.Visible = false;
			dataLabelStyle.ArrowStroke.Width = 0;
			dataLabelStyle.ArrowLength = 10;
			dataLabelStyle.TextStyle.Font = new NFont("Arial", 8);
			dataLabelStyle.TextStyle.Background.Visible = true;

			line.DataLabelStyle = dataLabelStyle;

			NMarkerStyle markerStyle = new NMarkerStyle();

			markerStyle.Visible = true;
			markerStyle.Border = new NStroke(color);
			markerStyle.Fill = new NColorFill(lightColor);
			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(5, 5);

			line.MarkerStyle = markerStyle;

			return line;
		}

		#endregion

		#region Event Handlers

		void OnRedAxisEndPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_BlueAxisBeginPercentUpDown.Minimum = m_RedAxisEndPercentUpDown.Value + 10;

			RecalcAxes();
		}

		void OnBlueAxisEndPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RedAxisEndPercentUpDown.Maximum = m_BlueAxisBeginPercentUpDown.Value - 10;

			RecalcAxes();
		}

		private void RecalcAxes()
		{
			int middleAxisBegin = (int)m_RedAxisEndPercentUpDown.Value;
			int middleAxisEnd = (int)m_BlueAxisBeginPercentUpDown.Value;

			// red axis
			m_RedAxis.Anchor.EndPercent = middleAxisBegin;

			// green axis
			m_GreenAxis.Anchor.BeginPercent = middleAxisBegin;
			m_GreenAxis.Anchor.EndPercent = middleAxisEnd;

			// blue axis
			m_BlueAxis.Anchor.BeginPercent = middleAxisEnd;
		}

		#endregion

		#region Fields

		NCartesianAxis m_RedAxis;
		NCartesianAxis m_GreenAxis;
		NCartesianAxis m_BlueAxis;

		NNumericUpDown m_RedAxisEndPercentUpDown;
		NNumericUpDown m_BlueAxisBeginPercentUpDown;

		#endregion

		#region Schema

		public static readonly NSchema NAxisDockingPercentagestExampleSchema;

		#endregion
	}
}
