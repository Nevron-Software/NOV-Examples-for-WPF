using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Line Labels Example
	/// </summary>
	public class NLineLabelsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NLineLabelsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NLineLabelsExample()
		{
			NLineLabelsExampleSchema = NSchema.Create(typeof(NLineLabelsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Line Labels";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// configure Y axis
			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			// add interlaced stripe for Y axis
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			scaleY.Strips.Add(strip);

			// line series
			m_Line = new NLineSeries();
			m_Chart.Series.Add(m_Line);

			m_Line.InflateMargins = true;

			NMarkerStyle markerStyle = new NMarkerStyle();
			markerStyle.Visible = true;
			markerStyle.Fill = new NColorFill(NColor.DarkOrange);
			markerStyle.Shape = ENPointShape.Ellipse;
			markerStyle.Size = new NSize(2, 2);
			m_Line.MarkerStyle = markerStyle;
			m_Line.ValueFormatter = new NNumericValueFormatter("0.000");

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = true;
			dataLabelStyle.VertAlign = ENVerticalAlignment.Top;
			dataLabelStyle.ArrowLength = 10;
			dataLabelStyle.ArrowStroke = new NStroke(NColor.DarkOrange);
			dataLabelStyle.TextStyle.Background.Border = new NStroke(NColor.DarkOrange);
			dataLabelStyle.Format = "<value>";

			m_Chart.LabelLayout.EnableInitialPositioning = true;
			m_Chart.LabelLayout.EnableLabelAdjustment = true;

			m_Line.LabelLayout.EnableDataPointSafeguard = true;
			m_Line.LabelLayout.DataPointSafeguardSize = new NSize(2, 2);
			m_Line.LabelLayout.UseLabelLocations = true;
			m_Line.LabelLayout.OutOfBoundsLocationMode = ENOutOfBoundsLocationMode.PushWithinBounds;
			m_Line.LabelLayout.InvertLocationsIfIgnored = true;

			// fill with random data
			OnGenerateDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox enableInitialPositioningCheckBox = new NCheckBox("Enable Initial Positioning");
			enableInitialPositioningCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableInitialPositioningCheckBoxCheckedChanged);
			stack.Add(enableInitialPositioningCheckBox);

			NCheckBox enableLabelAdjustmentCheckBox = new NCheckBox("Enable Label Adjustment");
			enableLabelAdjustmentCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableLabelAdjustmentCheckBoxCheckedChanged);
			stack.Add(enableLabelAdjustmentCheckBox);

			NButton generateDataButton = new NButton("Generate Data");
			generateDataButton.Click += new Function<NEventArgs>(OnGenerateDataButtonClick);
			stack.Add(generateDataButton);

			enableInitialPositioningCheckBox.Checked = true;
			enableLabelAdjustmentCheckBox.Checked = true;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how automatic data label layout works with line data labels.</p>";
		}

		#endregion

		#region Event Handlers

		void OnEnableLabelAdjustmentCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Chart.LabelLayout.EnableLabelAdjustment = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnEnableInitialPositioningCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Chart.LabelLayout.EnableInitialPositioning = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnGenerateDataButtonClick(NEventArgs arg)
		{
			m_Line.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 30; i++)
			{
				double value = Math.Sin(i * 0.2) * 10 + random.NextDouble() * 2;
				m_Line.DataPoints.Add(new NLineDataPoint(value));
			}
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NLineLabelsExampleSchema;

		#endregion
	}
}
