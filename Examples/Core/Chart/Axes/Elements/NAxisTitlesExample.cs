using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis titles example.
	/// </summary>
	public class NAxisTitlesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisTitlesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisTitlesExample()
		{
			NAxisTitlesExampleSchema = NSchema.Create(typeof(NAxisTitlesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			chartView.Surface.Titles[0].Text = "Axis Titles";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			scaleY.Strips.Add(strip);

			NOrdinalScale scaleX = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			// create dummy data
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bars";
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(100)));
			}

			m_Chart.Series.Add(bar);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("X Axis Title"));
			NTextBox xAxisTitleTextBox = new NTextBox();
			xAxisTitleTextBox.TextChanged += new Function<NValueChangeEventArgs>(OnXAxisTitleTextBoxTextChanged);
			stack.Add(NPairBox.Create("Text:", xAxisTitleTextBox));
			xAxisTitleTextBox.Text = "X Axis Title";

			NNumericUpDown xOffsetUpDown = new NNumericUpDown();
			xOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnXOffsetUpDownValueChanged);
			stack.Add(NPairBox.Create("Offset:", xOffsetUpDown));
			xOffsetUpDown.Value = 10;

			NComboBox xAlignmentCombo = new NComboBox();
			xAlignmentCombo.FillFromEnum<ENHorizontalAlignment>();
			xAlignmentCombo.SelectedIndexChanged +=new Function<NValueChangeEventArgs>(OnXAlignmentComboSelectedIndexChanged);
			stack.Add(NPairBox.Create("Offset:", xAlignmentCombo));
			xAlignmentCombo.SelectedIndex = (int)ENHorizontalAlignment.Center;

			stack.Add(new NLabel("Y Axis Title"));

			NTextBox yAxisTitleTextBox = new NTextBox();
			yAxisTitleTextBox.TextChanged += new Function<NValueChangeEventArgs>(OnYAxisTitleTextBoxTextChanged);
			stack.Add(NPairBox.Create("Text:", yAxisTitleTextBox));
			yAxisTitleTextBox.Text = "Y Axis Title";

			NNumericUpDown yOffsetUpDown = new NNumericUpDown();
			yOffsetUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnYOffsetUpDownValueChanged);
			stack.Add(NPairBox.Create("Offset:", yOffsetUpDown));
			yOffsetUpDown.Value = 10;

			NComboBox yAlignmentCombo = new NComboBox();
			yAlignmentCombo.FillFromEnum<ENHorizontalAlignment>();
			yAlignmentCombo.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnYAlignmentComboSelectedIndexChanged);
			stack.Add(NPairBox.Create("Offset:", yAlignmentCombo));
			yAlignmentCombo.SelectedIndex = (int)ENHorizontalAlignment.Center;

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the most important features of the axis titles.</p>";
		}

		#endregion

		#region Implementation

		private static void SetAlignment(NScaleTitle title, ENHorizontalAlignment alignment)
		{
			title.RulerAlignment = alignment;

			switch (alignment)
			{
				case ENHorizontalAlignment.Left:
					title.ContentAlignment = ENContentAlignment.MiddleLeft;
					break;
				case ENHorizontalAlignment.Center:
					title.ContentAlignment = ENContentAlignment.MiddleCenter;
					break;
				case ENHorizontalAlignment.Right:
					title.ContentAlignment = ENContentAlignment.MiddleRight;
					break;
			}
		}

		#endregion

		#region Event Handlers

		void OnXAxisTitleTextBoxTextChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale.Title.Text = ((NTextBox)arg.TargetNode).Text;
		}

		void OnXOffsetUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale.Title.Offset = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnXAlignmentComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			SetAlignment(m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale.Title, (ENHorizontalAlignment)((NComboBox)arg.TargetNode).SelectedIndex);
		}

		void OnYAxisTitleTextBoxTextChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Title.Text = ((NTextBox)arg.TargetNode).Text;
		}

		void OnYOffsetUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Title.Offset = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnYAlignmentComboSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			SetAlignment(m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Title, (ENHorizontalAlignment)((NComboBox)arg.TargetNode).SelectedIndex);
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NAxisTitlesExampleSchema;

		#endregion
	}
}
