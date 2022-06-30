using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Bubble Example
	/// </summary>
	public class NStandardBubbleExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardBubbleExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardBubbleExample()
		{
			NStandardBubbleExampleSchema = NSchema.Create(typeof(NStandardBubbleExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Bubble";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// configure the chart
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			yScale.MajorGridLines = new NScaleGridLines();
			yScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			// add interlace stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			NOrdinalScale xScale = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			xScale.MajorGridLines = new NScaleGridLines();
			xScale.MajorGridLines.Stroke.DashStyle = ENDashStyle.Dot;

			// add a bubble series
			m_Bubble = new NBubbleSeries();

			m_Bubble.DataLabelStyle = new NDataLabelStyle();
			m_Bubble.DataLabelStyle.VertAlign = ENVerticalAlignment.Center;
			m_Bubble.DataLabelStyle.Visible = false;
			m_Bubble.LegendView.Mode = ENSeriesLegendMode.DataPoints;
			m_Bubble.MinSize = 20;
			m_Bubble.MaxSize = 100;

			m_Bubble.DataPoints.Add(new NBubbleDataPoint(10, 10, "Company 1"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(15, 20, "Company 2"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(12, 25, "Company 3"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(8, 15, "Company 4"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(14, 17, "Company 5"));
			m_Bubble.DataPoints.Add(new NBubbleDataPoint(11, 12, "Company 6"));

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
            inflateMarginsCheckBox.Checked = true;
			stack.Add(NPairBox.Create("Inflate Margins: ", inflateMarginsCheckBox));

			NComboBox legendFormatComboBox = CreateLegendFormatCombo();
			legendFormatComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnLegendFormatComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Legend Format:", legendFormatComboBox));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard bubble chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnLegendFormatComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Bubble.LegendView.Format = (string)((NComboBox)arg.TargetNode).SelectedItem.Tag;
		}

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

		#region Fields

		NBubbleSeries m_Bubble;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardBubbleExampleSchema;

		#endregion
	}
}