using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Area Example
	/// </summary>
	public class NAreaLabelsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAreaLabelsExample()
		{

		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAreaLabelsExample()
		{
			NAreaLabelsExampleSchema = NSchema.Create(typeof(NAreaLabelsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Area Labels";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlaced stripe for Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;

			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.Strips.Add(stripStyle);

			scaleY.MajorGridLines.Visible = true;

			m_Area = new NAreaSeries();
			m_Chart.Series.Add(m_Area);

			// setup area series
			m_Area.InflateMargins = true;
			m_Area.UseXValues = true;
			m_Area.ValueFormatter = new NNumericValueFormatter("0.000");

			m_Area.Fill = new NColorFill(NColor.DarkOrange);
			m_Area.Stroke = new NStroke(NColor.Black);

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();
			dataLabelStyle.Visible = true;
			dataLabelStyle.VertAlign = ENVerticalAlignment.Top;
			dataLabelStyle.ArrowLength = 20;
			dataLabelStyle.ArrowStroke  = new NStroke(NColor.Black);
			dataLabelStyle.Format = "<value>";
			dataLabelStyle.TextStyle.Background.Visible = true;
			dataLabelStyle.TextStyle.Background.Padding = new NMargins(0);

			m_Area.DataLabelStyle = dataLabelStyle;

			// disable initial label positioning
			m_Chart.LabelLayout.EnableInitialPositioning = false;

			// enable label adjustment
			m_Chart.LabelLayout.EnableLabelAdjustment = true;

			// enable the data point safesuard and set its size
			m_Area.LabelLayout.EnableDataPointSafeguard = true;
			m_Area.LabelLayout.DataPointSafeguardSize = new NSize(2,2);

			// fill with random data
			OnGenerateDataButtonClick(null);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox enableLabelAdjustmentCheckBox = new NCheckBox("Enable Label Adjustment");
			enableLabelAdjustmentCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableLabelAdjustmentCheckBoxCheckedChanged);
			stack.Add(enableLabelAdjustmentCheckBox);
			enableLabelAdjustmentCheckBox.Checked = true;

			NCheckBox enableDataPointSafeGuardCheckBox = new NCheckBox("Enable Data Point Safeguard");
			enableDataPointSafeGuardCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnEnableDataPointSafeGuardCheckBoxCheckedChanged);
			stack.Add(enableDataPointSafeGuardCheckBox);
			enableDataPointSafeGuardCheckBox.Checked = true;

			NButton generateDataButton = new NButton("Generate Data");
			generateDataButton.Click += new Function<NEventArgs>(OnGenerateDataButtonClick);
			stack.Add(generateDataButton);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how the automatic data label layout works with area data labels.</p>";
		}

		#endregion

		#region Event Handlers

		void OnEnableLabelAdjustmentCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Chart.LabelLayout.EnableLabelAdjustment = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnEnableDataPointSafeGuardCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Area.LabelLayout.EnableDataPointSafeguard = ((NCheckBox)arg.TargetNode).Checked;
		}

		void OnGenerateDataButtonClick(NEventArgs arg)
		{
			m_Area.DataPoints.Clear();

			double xvalue = 10;
			Random random = new Random();

			for (int i = 0; i < 24; i++)
			{
				double value = Math.Sin(i * 0.4) * 5 + random.NextDouble() * 3;
				xvalue += 1 + random.NextDouble() * 20;

				m_Area.DataPoints.Add(new NAreaDataPoint(xvalue, value));
			}
		}


		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NAreaSeries m_Area;

		#endregion

		#region Static Fields

		internal static double[] monthValues = new double[] { 16, 19, 16, 15, 18, 19, 24, 21, 22, 17, 19, 15 };
		internal static string[] monthLetters = new string[] { "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };

		#endregion

		#region Schema

		public static readonly NSchema NAreaLabelsExampleSchema;

		#endregion
	}
}