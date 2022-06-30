using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Radar axis titles example
	/// </summary>
	public class NRadarAxisTitlesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRadarAxisTitlesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRadarAxisTitlesExample()
		{
			NRadarAxisTitlesExampleSchema = NSchema.Create(typeof(NRadarAxisTitlesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateRadarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Radar Axis Titles";

			// configure chart
			m_Chart = (NRadarChart)chartView.Surface.Charts[0];

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

			// create the radar series
			m_RadarArea1 = new NRadarAreaSeries();
			m_Chart.Series.Add(m_RadarArea1);
			m_RadarArea1.Name = "Series 1";
			m_RadarArea1.DataLabelStyle = new NDataLabelStyle(false);
			m_RadarArea1.DataLabelStyle.Format = "<value>";

			m_RadarArea2 = new NRadarAreaSeries();
			m_Chart.Series.Add(m_RadarArea2);
			m_RadarArea2.Name = "Series 2";
			m_RadarArea2.DataLabelStyle = new NDataLabelStyle(false);

			// fill random data
			Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_RadarArea1.DataPoints.Add(new NRadarAreaDataPoint(random.Next(50, 90)));
				m_RadarArea2.DataPoints.Add(new NRadarAreaDataPoint(random.Next(0, 100)));
			}
			
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NNumericUpDown beginAngleUpDown = new NNumericUpDown();
			beginAngleUpDown.ValueChanged += OnBeginAngleUpDownValueChanged;
			beginAngleUpDown.Value = 90;
			stack.Add(NPairBox.Create("Begin Angle:", beginAngleUpDown));

			NComboBox titleAngleModeComboBox = new NComboBox();
			titleAngleModeComboBox.FillFromEnum<ENScaleLabelAngleMode>();
			titleAngleModeComboBox.SelectedIndexChanged += OnTitleAngleModeComboBoxSelectedIndexChanged;
			titleAngleModeComboBox.SelectedIndex = (int)ENScaleLabelAngleMode.Scale;
			stack.Add(NPairBox.Create("Title Angle Mode:", titleAngleModeComboBox));

			NNumericUpDown titleAngleUpDown = new NNumericUpDown();
			titleAngleUpDown.ValueChanged += OnTitleAngleUpDownValueChanged;
			titleAngleUpDown.Value = 0;
			stack.Add(NPairBox.Create("Title Angle:", titleAngleUpDown));

			NCheckBox allowLabelsToFlipCheckBox = new NCheckBox("Allow Labels to Flip");
			allowLabelsToFlipCheckBox.CheckedChanged += OnAllowLabelsToFlipCheckBoxCheckedChanged;
			allowLabelsToFlipCheckBox.Checked = false;
			stack.Add(allowLabelsToFlipCheckBox);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to set radar axis titles.</p>";
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
		void OnAllowLabelsToFlipCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			for (int i = 0; i < m_Chart.Axes.Count; i++)
			{
				NScaleLabelAngle titleAngle = m_Chart.Axes[i].TitleAngle;
				titleAngle.AllowTextFlip = ((NCheckBox)arg.TargetNode).Checked;
				m_Chart.Axes[i].TitleAngle = titleAngle;
			}
		}	

		#endregion

		#region Implementation

		private void AddAxis(string title)
		{
			NRadarAxis axis = new NRadarAxis();

			// set title
			axis.Title = title;
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
		NRadarAreaSeries m_RadarArea1;
		NRadarAreaSeries m_RadarArea2;

		#endregion

		#region Schema

		public static readonly NSchema NRadarAxisTitlesExampleSchema;

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