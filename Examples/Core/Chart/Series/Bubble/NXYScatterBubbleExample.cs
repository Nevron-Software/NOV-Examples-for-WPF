using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Bubble Example
	/// </summary>
	public class NXYScatterBubbleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NXYScatterBubbleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NXYScatterBubbleExample()
		{
			NXYScatterBubbleExampleSchema = NSchema.Create(typeof(NXYScatterBubbleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "XY Scatter Bubble";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// configure the chart
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			yScale.MajorGridLines = new NScaleGridLines();
			yScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			// add interlace stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			NLinearScale xScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			xScale.MajorGridLines = new NScaleGridLines();
			xScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			// add a bubble series
			m_Bubble = new NBubbleSeries();

			m_Bubble = new NBubbleSeries();
			m_Bubble.DataLabelStyle = new NDataLabelStyle(false);
			m_Bubble.LegendView.Format = "<label>";
			m_Bubble.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			m_Bubble.UseXValues = true;

			m_Bubble.DataPoints.Add(new NBubbleDataPoint(27, 51, 1147995904, "India"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(50, 67, 1321851888, "China"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(76, 22, 109955400, "Mexico"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(210, 9, 142008838, "Russia"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(360, 4, 305843000, "USA"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(470, 5, 33560000, "Canada"));

			m_Chart.Series.Add(m_Bubble);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox bubbleShapeComboBox = new NComboBox();
			bubbleShapeComboBox.FillFromEnum<ENPointShape>();
			bubbleShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnBubbleShapeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Bubble Shape: ", bubbleShapeComboBox));
			bubbleShapeComboBox.SelectedIndex = (int)ENPointShape.Ellipse;

			NNumericUpDown minBubbleSizeUpDown = new NNumericUpDown();
			minBubbleSizeUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMinBubbleSizeUpDownValueChanged);
			stack.Add(NPairBox.Create("Min Bubble Size:", minBubbleSizeUpDown));
			minBubbleSizeUpDown.Value = 50;

			NNumericUpDown maxBubbleSizeUpDown = new NNumericUpDown();
			maxBubbleSizeUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMaxBubbleSizeUpDownValueChanged);
			stack.Add(NPairBox.Create("Max Bubble Size:", maxBubbleSizeUpDown));
			maxBubbleSizeUpDown.Value = 200;

			NCheckBox inflateMarginsCheckBox = new NCheckBox();
			inflateMarginsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInflateMarginsCheckBoxCheckedChanged);
			stack.Add(NPairBox.Create("Inflate Margins: ", inflateMarginsCheckBox));
			inflateMarginsCheckBox.Checked = true;

			NButton changeYValuesButton = new NButton("Change Y Values");
			changeYValuesButton.Click += new Function<NEventArgs>(OnChangeYValuesButtonClick);
			stack.Add(changeYValuesButton);

			NButton changeXValuesButton = new NButton("Change X Values");
			changeXValuesButton.Click += new Function<NEventArgs>(OnChangeXValuesButtonClick);
			stack.Add(changeXValuesButton);

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a xy scatter bubble chart.</p>";
		}

		#endregion

		#region Implementation

		private NComboBox CreateLegendFormatCombo()
		{
			NComboBox comboBox = new NComboBox();

			NComboBoxItem item = new NComboBoxItem("Value and Label");
			item.Tag = "<value> <label>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Value");
			item.Tag = "<value>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Label");
			item.Tag = "<label>";
			comboBox.Items.Add(item);

			item = new NComboBoxItem("Size");
			item.Tag = "<size>";
			comboBox.Items.Add(item);

			return comboBox;
		}

		#endregion

		#region Event Handlers

		void OnBubbleShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Bubble.Shape = (ENPointShape)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Bubble.InflateMargins = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnMaxBubbleSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Bubble.MaxSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMinBubbleSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Bubble.MinSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnChangeXValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();

			for (int i = 0; i < m_Bubble.DataPoints.Count; i++)
			{
				m_Bubble.DataPoints[i].X = random.Next(-100, 100);
			}
		}

		void OnChangeYValuesButtonClick(NEventArgs arg)
		{
			Random random = new Random();

			for (int i = 0; i < m_Bubble.DataPoints.Count; i++)
			{
				m_Bubble.DataPoints[i].Value = random.Next(-100, 100);
			}
		}


		#endregion

		#region Fields

		NBubbleSeries m_Bubble;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NXYScatterBubbleExampleSchema;

		#endregion
	}
}