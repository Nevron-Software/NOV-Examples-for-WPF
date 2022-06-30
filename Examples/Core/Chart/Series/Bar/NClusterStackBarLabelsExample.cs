using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Cluster Stack Bar Labels Example
	/// </summary>
	public class NClusterStackBarLabelsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NClusterStackBarLabelsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NClusterStackBarLabelsExample()
		{
			NClusterStackBarLabelsExampleSchema = NSchema.Create(typeof(NClusterStackBarLabelsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Cluster Stack Bar Labels";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// configure Y axis
			NLinearScale scaleY = new NLinearScale();
			scaleY.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dash;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale = scaleY;

			// add interlaced stripe for Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			scaleY.Strips.Add(stripStyle);

			NSize dataPointSafeguardSize = new NSize(2, 2);

			// series 1
			m_Bar1 = new NBarSeries();
			m_Chart.Series.Add(m_Bar1);
			m_Bar1.Name = "Bar 1";
			m_Bar1.Fill = new NColorFill(NColor.DarkOrange);
			m_Bar1.DataLabelStyle = CreateDataLabelStyle(ENVerticalAlignment.Center);

			// series 2
			m_Bar2 = new NBarSeries();
			m_Chart.Series.Add(m_Bar2);
			m_Bar2.Name = "Bar 2";
			m_Bar2.MultiBarMode = ENMultiBarMode.Stacked;
			m_Bar2.Fill = new NColorFill(NColor.OrangeRed);
			m_Bar2.DataLabelStyle = CreateDataLabelStyle(ENVerticalAlignment.Center);

			// series 3
			m_Bar3 = new NBarSeries();
			m_Chart.Series.Add(m_Bar3);
			m_Bar3.Name = "Bar 3";
			m_Bar3.MultiBarMode = ENMultiBarMode.Clustered;
			m_Bar3.Fill = new NColorFill(NColor.LightGreen);
			m_Bar3.DataLabelStyle = CreateDataLabelStyle(ENVerticalAlignment.Top);

			// enable initial labels positioning
			m_Chart.LabelLayout.EnableInitialPositioning = true;

			// enable label adjustment
			m_Chart.LabelLayout.EnableLabelAdjustment = true;

			// series 1 data points must not be overlapped
			m_Bar1.LabelLayout.EnableDataPointSafeguard = true;
			m_Bar1.LabelLayout.DataPointSafeguardSize = dataPointSafeguardSize;

			// do not use label location proposals for series 1
			m_Bar1.LabelLayout.UseLabelLocations = false;

			// series 2 data points must not be overlapped
			m_Bar2.LabelLayout.EnableDataPointSafeguard = true;
			m_Bar2.LabelLayout.DataPointSafeguardSize = dataPointSafeguardSize;

			// do not use label location proposals for series 2
			m_Bar2.LabelLayout.UseLabelLocations = false;

			// series 3 data points must not be overlapped
			m_Bar3.LabelLayout.EnableDataPointSafeguard = true;
			m_Bar3.LabelLayout.DataPointSafeguardSize = dataPointSafeguardSize;

			// series 3 data labels can be placed above and below the origin point
			m_Bar3.LabelLayout.UseLabelLocations = true;
			m_Bar3.LabelLayout.LabelLocations = new NDomArray<ENLabelLocation>(new ENLabelLocation[] { ENLabelLocation.Top, ENLabelLocation.Bottom });
			m_Bar3.LabelLayout.InvertLocationsIfIgnored = false;
			m_Bar3.LabelLayout.OutOfBoundsLocationMode = ENOutOfBoundsLocationMode.PushWithinBounds;

			// fill with random data
			OnGenerateDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			m_EnableInitialPositioningCheckBox = new NCheckBox("Enable Initial Positioning");
			m_EnableInitialPositioningCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableInitialPositioningCheckBoxCheckedChanged);
			stack.Add(m_EnableInitialPositioningCheckBox);
			
			m_EnableLabelAdjustmentCheckBox = new NCheckBox("Enable Label Adjustment");
			m_EnableLabelAdjustmentCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableLabelAdjustmentCheckBoxCheckedChanged);
			stack.Add(m_EnableLabelAdjustmentCheckBox);

			NButton generateDataButton = new NButton("Generate Data");
			generateDataButton.Click += new Function<NEventArgs>(OnGenerateDataButtonClick);
			stack.Add(generateDataButton);

			m_EnableInitialPositioningCheckBox.Checked = true;
			m_EnableLabelAdjustmentCheckBox.Checked = true;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how automatic data label layout works with cluster stack bar labels.</p>";
		}

		#endregion

		#region Event Handlers

		void OnEnableLabelAdjustmentCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Chart.LabelLayout.EnableLabelAdjustment = m_EnableLabelAdjustmentCheckBox.Checked;
		}

		void OnEnableInitialPositioningCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Chart.LabelLayout.EnableInitialPositioning = m_EnableInitialPositioningCheckBox.Checked;
		}

		void OnGenerateDataButtonClick(NEventArgs arg)
		{
			m_Bar1.DataPoints.Clear();
			m_Bar2.DataPoints.Clear();
			m_Bar3.DataPoints.Clear();

			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(5, 20)));
				m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(5, 20)));
				m_Bar3.DataPoints.Add(new NBarDataPoint(random.Next(5, 20)));
			}
		}

		#endregion

		#region Implementation

		NDataLabelStyle CreateDataLabelStyle(ENVerticalAlignment vertAlign)
		{
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Visible = true;
			dataLabelStyle.VertAlign = ENVerticalAlignment.Top;
			dataLabelStyle.ArrowLength = 20;
			dataLabelStyle.Format = "<value>";

			return dataLabelStyle;

		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NBarSeries m_Bar1;
		NBarSeries m_Bar2;
		NBarSeries m_Bar3;

		NCheckBox m_EnableInitialPositioningCheckBox;
		NCheckBox m_EnableLabelAdjustmentCheckBox;

		#endregion

		#region Schema

		public static readonly NSchema NClusterStackBarLabelsExampleSchema;

		#endregion
	}
}