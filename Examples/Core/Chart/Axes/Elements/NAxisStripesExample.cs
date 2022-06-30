using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis stripes example
	/// </summary>
	public class NAxisStripesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisStripesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisStripesExample()
		{
			NAxisStripesExampleSchema = NSchema.Create(typeof(NAxisStripesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Axis Stripes";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

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
			m_XStripe = new NAxisStripe();
			m_XStripe.Fill = new NColorFill(NColor.FromColor(NColor.SteelBlue, 0.5f));
			m_XStripe.Range = new NRange (40, 60);
			m_XStripe.Text = "X Axis Stripe";
			chart.Axes[ENCartesianAxis.PrimaryX].Stripes.Add(m_XStripe);

			// Add a constline for the bottom axis
			m_YStripe = new NAxisStripe();
			m_YStripe.Fill = new NColorFill(NColor.FromColor(NColor.SteelBlue, 0.5f));
			m_YStripe.Range = new NRange(40, 60);
			m_YStripe.Text = "Y Axis Stripe";
			chart.Axes[ENCartesianAxis.PrimaryY].Stripes.Add(m_YStripe);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);
			stack.Add(new NLabel("Y Axis Stripe"));

			NNumericUpDown yStripeBeginValueUpDown = new NNumericUpDown();
			yStripeBeginValueUpDown.ValueChanged += OnYStripeBeginValueUpDownValueChanged;
			yStripeBeginValueUpDown.Value = m_YStripe.Range.Begin;
			stack.Add(NPairBox.Create("Begin Value:", yStripeBeginValueUpDown));

			NNumericUpDown yStripeEndValueUpDown = new NNumericUpDown();
			yStripeEndValueUpDown.ValueChanged += OnYStripeEndValueUpDownValueChanged;
			yStripeEndValueUpDown.Value = m_YStripe.Range.End;
			stack.Add(NPairBox.Create("End Value:", yStripeEndValueUpDown));

			NCheckBox yStripeIncludeInAxisRangeCheckBox = new NCheckBox("Include in Axis Range");
			yStripeIncludeInAxisRangeCheckBox.Checked = false;
			yStripeIncludeInAxisRangeCheckBox.CheckedChanged += OnStripeIncludeInAxisRangeCheckBoxCheckedChanged;
			stack.Add(yStripeIncludeInAxisRangeCheckBox);

			NCheckBox yStripePaintAfterSeriesCheckBox = new NCheckBox("Paint After Series");
			yStripePaintAfterSeriesCheckBox.Checked = m_YStripe.PaintAfterChartContent;
			yStripePaintAfterSeriesCheckBox.CheckedChanged += OnYStripePaintAfterSeriesCheckBoxCheckedChanged;
			stack.Add(yStripePaintAfterSeriesCheckBox);

			NComboBox yTextAlignmentComboBox = new NComboBox();
			yTextAlignmentComboBox.FillFromEnum<ENContentAlignment>();
			yTextAlignmentComboBox.SelectedIndex = (int)m_YStripe.TextAlignment;
			yTextAlignmentComboBox.SelectedIndexChanged += OnYTextAlignmentComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Text Alignment:", yTextAlignmentComboBox));

			stack.Add(new NLabel("X Axis Stripe"));

			NNumericUpDown xStripeBeginValueUpDown = new NNumericUpDown();
			xStripeBeginValueUpDown.Value = m_XStripe.Range.Begin;
			xStripeBeginValueUpDown.ValueChanged += OnXStripeBeginValueUpDownValueChanged;
			stack.Add(NPairBox.Create("Begin Value:", xStripeBeginValueUpDown));

			NNumericUpDown xStripeEndValueUpDown = new NNumericUpDown();
			xStripeEndValueUpDown.Value = m_XStripe.Range.End;
			xStripeEndValueUpDown.ValueChanged += OnXStripeEndValueUpDownValueChanged;
			stack.Add(NPairBox.Create("End Value:", xStripeEndValueUpDown));

			NCheckBox xStripeIncludeInAxisRangeCheckBox = new NCheckBox("Include in Axis Range");
			xStripeIncludeInAxisRangeCheckBox.Checked = false;
			xStripeIncludeInAxisRangeCheckBox.CheckedChanged += OnXStripeIncludeInAxisRangeCheckBoxCheckedChanged;
			stack.Add(xStripeIncludeInAxisRangeCheckBox);

			NCheckBox xStripePaintAfterSeriesCheckBox = new NCheckBox("Paint After Series");
			xStripePaintAfterSeriesCheckBox.Checked = m_XStripe.PaintAfterChartContent;
			xStripePaintAfterSeriesCheckBox.CheckedChanged += OnXStripePaintAfterSeriesCheckBoxCheckedChanged;
			stack.Add(xStripePaintAfterSeriesCheckBox);

			NComboBox xTextAlignmentComboBox = new NComboBox();
			xTextAlignmentComboBox.FillFromEnum<ENContentAlignment>();
			xTextAlignmentComboBox.SelectedIndex = (int)m_XStripe.TextAlignment;
			xTextAlignmentComboBox.SelectedIndexChanged += OnXTextAlignmentComboBoxSelectedIndexChanged;
			stack.Add(NPairBox.Create("Text Alignment:", xTextAlignmentComboBox));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to configure axis stripes.</p>";
		}

		#endregion

		#region Event Handlers

		void OnXStripeBeginValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_XStripe.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_XStripe.Range.End);
		}

		void OnXStripeEndValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_XStripe.Range = new NRange(m_XStripe.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnXStripeIncludeInAxisRangeCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool isChecked = ((NCheckBox)arg.TargetNode).Checked;

			m_XStripe.IncludeRangeBeginInAxisRange = isChecked;
			m_XStripe.IncludeRangeEndInAxisRange = isChecked;
		}


		void OnXStripePaintAfterSeriesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_XStripe.PaintAfterChartContent = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnXTextAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_XStripe.TextAlignment = (ENContentAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnYStripeBeginValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_YStripe.Range = new NRange(((NNumericUpDown)arg.TargetNode).Value, m_YStripe.Range.End);
		}

		void OnYStripeEndValueUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_YStripe.Range = new NRange(m_YStripe.Range.Begin, ((NNumericUpDown)arg.TargetNode).Value);
		}

		void OnStripeIncludeInAxisRangeCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			bool isChecked = ((NCheckBox)arg.TargetNode).Checked;

			m_YStripe.IncludeRangeBeginInAxisRange = isChecked;
			m_YStripe.IncludeRangeEndInAxisRange = isChecked;
		}

		void OnYStripePaintAfterSeriesCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_YStripe.PaintAfterChartContent = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnYTextAlignmentComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_YStripe.TextAlignment = (ENContentAlignment)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		NAxisStripe m_XStripe;
		NAxisStripe m_YStripe;


		#endregion

		#region Schema

		public static readonly NSchema NAxisStripesExampleSchema;

		#endregion
	}
}