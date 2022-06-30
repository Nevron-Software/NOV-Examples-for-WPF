using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Radar line example
	/// </summary>
	public class NRadarLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRadarLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRadarLineExample()
		{
			NRadarLineExampleSchema = NSchema.Create(typeof(NRadarLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateRadarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Radar Line";

			// configure chart
			m_Chart = (NRadarChart)chartView.Surface.Charts[0];

			// set some axis labels
			AddAxis("Vitamin A");
			AddAxis("Vitamin B1");
			AddAxis("Vitamin B2");
			AddAxis("Vitamin B6");
			AddAxis("Vitamin B12");
			AddAxis("Vitamin C");
			AddAxis("Vitamin D");
			AddAxis("Vitamin E");

			NLinearScale radarScale = (NLinearScale)m_Chart.Axes[0].Scale;
			radarScale.MajorGridLines.Visible = true;

			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			radarScale.Strips.Add(strip);

			m_RadarLine1 = new NRadarLineSeries();
			m_Chart.Series.Add(m_RadarLine1);
			m_RadarLine1.Stroke = new NStroke(2, NChartTheme.BrightPalette[0]);
			m_RadarLine1.Name = "Series 1";
			m_RadarLine1.DataLabelStyle = new NDataLabelStyle(false);
			NMarkerStyle markerStyle1 = new NMarkerStyle();
			markerStyle1.Visible = true;
			markerStyle1.Shape = ENPointShape.Ellipse;
			markerStyle1.Size = new NSize(10, 10);
			m_RadarLine1.MarkerStyle = markerStyle1;

			m_RadarLine2 = new NRadarLineSeries();
			m_Chart.Series.Add(m_RadarLine2);
			m_RadarLine2.Stroke = new NStroke(2, NChartTheme.BrightPalette[1]);
			m_RadarLine2.Name = "Series 2";
			m_RadarLine2.DataLabelStyle = new NDataLabelStyle(false);
			NMarkerStyle markerStyle2 = new NMarkerStyle();
			markerStyle2.Visible = true;
			markerStyle2.Shape = ENPointShape.Ellipse;
			markerStyle2.Size = new NSize(10, 10);
			m_RadarLine2.MarkerStyle = markerStyle2;

			// fill random data
			Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_RadarLine1.DataPoints.Add(new NRadarLineDataPoint(random.Next(50, 90)));
				m_RadarLine2.DataPoints.Add(new NRadarLineDataPoint(random.Next(0, 100)));
			}
			
			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NNumericUpDown beginAngleUpDown = new NNumericUpDown();
			beginAngleUpDown.ValueChanged +=OnBeginAngleUpDownValueChanged;
			beginAngleUpDown.Value = 90;
			stack.Add(NPairBox.Create("Begin Angle:", beginAngleUpDown));

			NComboBox titleAngleModeComboBox = new NComboBox();
			titleAngleModeComboBox.FillFromEnum<ENScaleLabelAngleMode>();
			titleAngleModeComboBox.SelectedIndexChanged +=OnTitleAngleModeComboBoxSelectedIndexChanged;
			titleAngleModeComboBox.SelectedIndex = (int)ENScaleLabelAngleMode.Scale;
			stack.Add(NPairBox.Create("Title Angle Mode:", titleAngleModeComboBox));

			NNumericUpDown titleAngleUpDown = new NNumericUpDown();
			titleAngleUpDown.ValueChanged += OnTitleAngleUpDownValueChanged;
			titleAngleUpDown.Value = 0;
			stack.Add(NPairBox.Create("Title Angle:", titleAngleUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a radar line chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnBeginAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Chart.BeginAngle = new NAngle(((NNumericUpDown)arg.TargetNode).Value, NUnit.Degree);
		}

		void OnTitleAngleModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				NScaleLabelAngle oldTitleAngle = m_Chart.Axes[i].TitleAngle;
				m_Chart.Axes[i].TitleAngle = new NScaleLabelAngle((ENScaleLabelAngleMode)((NComboBox)arg.TargetNode).SelectedIndex, oldTitleAngle.CustomAngle);
			}
		}

		void OnTitleAngleUpDownValueChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				NScaleLabelAngle oldTitleAngle = m_Chart.Axes[i].TitleAngle;
				m_Chart.Axes[i].TitleAngle = new NScaleLabelAngle(oldTitleAngle.LabelAngleMode, ((NNumericUpDown)arg.TargetNode).Value);
			}
		}

		#endregion

		#region Implementation

		private void AddAxis(string title)
		{
			NRadarAxis axis = new NRadarAxis();

			// set title
			axis.Title = title;
			axis.TitleAngle = new NScaleLabelAngle(ENScaleLabelAngleMode.Scale, 0);
//			axis.TitleTextStyle.TextFormat = TextFormat.XML;

			// setup scale
			NLinearScale linearScale = (NLinearScale)axis.Scale;

			if (m_Chart.Axes.Count == 0)
			{
				// if the first axis then configure grid style and stripes
				linearScale.MajorGridLines.Stroke = new NStroke(1, NColor.Gainsboro, ENDashStyle.Dot);

				// add interlaced stripe
				NScaleStrip strip = new NScaleStrip();
				strip.Fill = new NColorFill(NColor.FromRGBA(200, 200, 200, 64));
				strip.Interlaced = true;
				linearScale.Strips.Add(strip);
			}
			else
			{
				// hide labels
				linearScale.Labels.Visible = false;
			}

			m_Chart.Axes.Add(axis);
		}

		#endregion

		#region Fields

		NRadarChart m_Chart;
		NRadarLineSeries m_RadarLine1;
		NRadarLineSeries m_RadarLine2;

		#endregion

		#region Schema

		public static readonly NSchema NRadarLineExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreateRadarChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Radar);
			return chartView;
		}

		#endregion
	}
}