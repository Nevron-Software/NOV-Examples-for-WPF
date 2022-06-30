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
	public class NAxisDockingExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisDockingExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisDockingExample()
		{
			NAxisDockingExampleSchema = NSchema.Create(typeof(NAxisDockingExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Docking";

			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_RedAxis = CreateLinearAxis(ENCartesianAxisDockZone.Left, NColor.Red);
			chart.Axes.Add(m_RedAxis);

			m_GreenAxis = CreateLinearAxis(ENCartesianAxisDockZone.Left, NColor.Green);
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

		private NComboBox CreateAxisZoneCombo()
		{
			NComboBox axisZoneComboBox = new NComboBox();

			axisZoneComboBox.Items.Add(new NComboBoxItem("Left"));
			axisZoneComboBox.Items.Add(new NComboBoxItem("Right"));

			axisZoneComboBox.SelectedIndex = 0;
			axisZoneComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnAxisZoneComboBoxSelectedIndexChanged);

			return axisZoneComboBox;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_RedAxisZoneComboBox = CreateAxisZoneCombo();
			stack.Add(NPairBox.Create("Red Axis Dock Zone:", m_RedAxisZoneComboBox));

			m_GreenAxisZoneComboBox = CreateAxisZoneCombo();
			stack.Add(NPairBox.Create("Green Axis Dock Zone:", m_GreenAxisZoneComboBox));

			m_BlueAxisZoneComboBox = CreateAxisZoneCombo();
			stack.Add(NPairBox.Create("Blue Axis Dock Zone:", m_BlueAxisZoneComboBox));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to dock axes to different axis dock zones.</p>";
		}

		#endregion

		#region Implementation

		private NCartesianAxis CreateLinearAxis(ENCartesianAxisDockZone dockZone, NColor color)
		{
			NCartesianAxis axis = new NCartesianAxis();

			axis.Scale = CreateLinearScale(color);
			axis.Anchor = new NDockCartesianAxisAnchor(dockZone);

			return axis;
		}

		private NLinearScale CreateLinearScale(NColor color)
		{
			NLinearScale linearScale = new NLinearScale();

			linearScale.Ruler.Stroke = new NStroke(1, color);
			linearScale.InnerMajorTicks.Stroke = new NStroke(color);
			linearScale.OuterMajorTicks.Stroke = new NStroke(color);
			linearScale.MajorGridLines.Visible = false;
			linearScale.Labels.Style.TextStyle.Fill = new NColorFill(color);

			return linearScale;
		}
 
		private NLineSeries CreateLineSeries(NColor lightColor, NColor color, int begin, int end)
		{
			// Add a line series
			NLineSeries line = new NLineSeries();

			for (int i = 0; i < 5; i++)
			{
				line.DataPoints.Add(new NLineDataPoint(m_Random.Next(begin, end)));
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

		void OnAxisZoneComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			if (m_RedAxisZoneComboBox.SelectedIndex == 0)
			{
				m_RedAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Left);
			}
			else
			{
				m_RedAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Right);
			}

			if (m_GreenAxisZoneComboBox.SelectedIndex == 0)
			{
				m_GreenAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Left);
			}
			else
			{
				m_GreenAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Right);
			}

			if (m_BlueAxisZoneComboBox.SelectedIndex == 0)
			{
				m_BlueAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Left);
			}
			else
			{
				m_BlueAxis.Anchor = new NDockCartesianAxisAnchor(ENCartesianAxisDockZone.Right);
			}
		}

		#endregion

		#region Fields

		Random m_Random = new Random();

		NCartesianAxis m_RedAxis;
		NCartesianAxis m_GreenAxis;
		NCartesianAxis m_BlueAxis;

		NComboBox m_RedAxisZoneComboBox;
		NComboBox m_GreenAxisZoneComboBox;
		NComboBox m_BlueAxisZoneComboBox;

		#endregion

		#region Schema

		public static readonly NSchema NAxisDockingExampleSchema;

		#endregion
	}
}
