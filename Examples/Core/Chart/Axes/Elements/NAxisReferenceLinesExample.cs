using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis docking example
	/// </summary>
	public class NAxisReferenceLinesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisReferenceLinesExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisReferenceLinesExample()
		{
			NAxisReferenceLinesExampleSchema = NSchema.Create(typeof(NAxisReferenceLinesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Reference Lines";

			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// configure the y scale
			NLinearScale yScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			yScale.Strips.Add(stripStyle);

			yScale.MajorGridLines.Visible = true;

			// Create a point series
			NPointSeries point = new NPointSeries();
			point.InflateMargins = true;
			point.UseXValues = true;
			point.Name = "Series 1";
			chart.Series.Add(point);

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = false;
			point.DataLabelStyle = dataLabelStyle;

			// Add some data
			double[] yValues = new double[] { 31, 67, 12, 84, 90 };
			double[] xValues = new double[] { 5, 36, 51, 76, 93 };

			for (int i = 0; i < yValues.Length; i++)
			{
				point.DataPoints.Add(new NPointDataPoint(xValues[i], yValues[i]));
			}

			// Add a constline for the left axis
			m_XReferenceLine = new NAxisReferenceLine();
			m_XReferenceLine.Stroke = new NStroke(1, NColor.SteelBlue);
			m_XReferenceLine.Value = 40;
			m_XReferenceLine.Text = "X Reference Line";
			chart.Axes[ENCartesianAxis.PrimaryX].ReferenceLines.Add(m_XReferenceLine);

			// Add a constline for the bottom axis
			m_YReferenceLine = new NAxisReferenceLine();
			m_YReferenceLine.Stroke = new NStroke(1, NColor.SteelBlue);
			m_YReferenceLine.Value = 60;
			m_YReferenceLine.Text = "Y Reference Line";
			chart.Axes[ENCartesianAxis.PrimaryY].ReferenceLines.Add(m_YReferenceLine);

			// apply style sheet
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Y Axis Reference Line"));

			NNumericUpDown yReferenceLineValueUpDown = new NNumericUpDown();
			yReferenceLineValueUpDown.ValueChanged +=OnYReferenceLineValueUpDownValueChanged;
			yReferenceLineValueUpDown.Value = m_YReferenceLine.Value;
			stack.Add(NPairBox.Create("Value:", yReferenceLineValueUpDown));

			NCheckBox yReferenceLineIncludeInAxisRangeCheckBox = new NCheckBox("Include Value in Axis Range");
			yReferenceLineIncludeInAxisRangeCheckBox.Checked = m_YReferenceLine.IncludeValueInAxisRange;
			yReferenceLineIncludeInAxisRangeCheckBox.CheckedChanged += OnYReferenceLineIncludeInAxisRangeCheckBoxCheckedChanged;
			stack.Add(yReferenceLineIncludeInAxisRangeCheckBox);

			NCheckBox yReferenceLinePaintAfterSeriesCheckBox = new NCheckBox("Paint After Series");
			yReferenceLinePaintAfterSeriesCheckBox.Checked = m_YReferenceLine.PaintAfterChartContent;
			yReferenceLinePaintAfterSeriesCheckBox.CheckedChanged += OnYReferenceLinePaintAfterSeriesCheckBoxCheckedChanged;
			stack.Add(yReferenceLinePaintAfterSeriesCheckBox);

			NComboBox yTextAlignmentComboBox = new NComboBox();
			yTextAlignmentComboBox.FillFromEnum<ENContentAlignment>();
			yTextAlignmentComboBox.SelectedIndex = (int)m_YReferenceLine.TextAlignment;
			yTextAlignmentComboBox.SelectedIndexChanged += OnYTextAlignmentComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Text Alignment:", yTextAlignmentComboBox));

			stack.Add(new NLabel("X Axis Reference Line"));

			NNumericUpDown xReferenceLineValueUpDown = new NNumericUpDown();
			xReferenceLineValueUpDown.Value = m_XReferenceLine.Value;
			xReferenceLineValueUpDown.ValueChanged += OnXReferenceLineValueUpDownValueChanged;
			stack.Add(NPairBox.Create("Value:", xReferenceLineValueUpDown));

			NCheckBox xReferenceLinePaintAfterSeriesCheckBox = new NCheckBox("Paint After Series");
			xReferenceLinePaintAfterSeriesCheckBox.Checked = m_XReferenceLine.PaintAfterChartContent;
			xReferenceLinePaintAfterSeriesCheckBox.CheckedChanged += OnXReferenceLinePaintAfterSeriesCheckBoxCheckedChanged;
			stack.Add(xReferenceLinePaintAfterSeriesCheckBox);

			NCheckBox xReferenceLineIncludeInAxisRangeCheckBox = new NCheckBox("Include Value in Axis Range");
			xReferenceLineIncludeInAxisRangeCheckBox.Checked = m_YReferenceLine.IncludeValueInAxisRange;
			xReferenceLineIncludeInAxisRangeCheckBox.CheckedChanged += OnXReferenceLineIncludeInAxisRangeCheckBoxCheckedChanged;
			stack.Add(xReferenceLineIncludeInAxisRangeCheckBox);

			NComboBox xTextAlignmentComboBox = new NComboBox();
			xTextAlignmentComboBox.FillFromEnum<ENContentAlignment>();
			xTextAlignmentComboBox.SelectedIndex = (int)m_XReferenceLine.TextAlignment;
			xTextAlignmentComboBox.SelectedIndexChanged += OnXTextAlignmentComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Text Alignment:", xTextAlignmentComboBox));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to add axis reference lines.</p>";
		}

		#endregion

		#region Event Handlers

		void OnXReferenceLineValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_XReferenceLine.Value = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnXReferenceLineIncludeInAxisRangeCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_XReferenceLine.IncludeValueInAxisRange = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnXReferenceLinePaintAfterSeriesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_XReferenceLine.PaintAfterChartContent = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnXTextAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_XReferenceLine.TextAlignment = (ENContentAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnYReferenceLineValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_YReferenceLine.Value = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnYReferenceLineIncludeInAxisRangeCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_YReferenceLine.IncludeValueInAxisRange = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnYReferenceLinePaintAfterSeriesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_YReferenceLine.PaintAfterChartContent = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnYTextAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_YReferenceLine.TextAlignment = (ENContentAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		NAxisReferenceLine m_XReferenceLine;
		NAxisReferenceLine m_YReferenceLine;

		#endregion

		#region Schema

		public static readonly NSchema NAxisReferenceLinesExampleSchema;

		#endregion
	}
}