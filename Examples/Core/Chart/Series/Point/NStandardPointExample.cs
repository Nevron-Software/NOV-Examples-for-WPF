using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Point Example
	/// </summary>
	public class NStandardPointExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardPointExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardPointExample()
		{
			NStandardPointExampleSchema = NSchema.Create(typeof(NStandardPointExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Point";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// add interlace stripe
			NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;

			// setup point series
			m_Point = new NPointSeries();

			//m_Point.DataLabelStyle.ArrowLength = 20;

			m_Point.Name = "Point Series";
			m_Point.InflateMargins = true;
			m_Point.DataLabelStyle = new NDataLabelStyle(false);

			m_Point.DataPoints.Add(new NPointDataPoint(23, "Item1"));
			m_Point.DataPoints.Add(new NPointDataPoint(67, "Item2"));
			m_Point.DataPoints.Add(new NPointDataPoint(78, "Item3"));
			m_Point.DataPoints.Add(new NPointDataPoint(12, "Item4"));
			m_Point.DataPoints.Add(new NPointDataPoint(56, "Item5"));
			m_Point.DataPoints.Add(new NPointDataPoint(43, "Item6"));
			m_Point.DataPoints.Add(new NPointDataPoint(37, "Item7"));
			m_Point.DataPoints.Add(new NPointDataPoint(51, "Item8"));

			m_Point.Fill = new NColorFill(NColor.Red);

			m_Chart.Series.Add(m_Point);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, true));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NCheckBox inflateMarginsCheckBox = new NCheckBox();
			inflateMarginsCheckBox.CheckedChanged += new Function<NValueChangeEventArgs>(OnInflateMarginsCheckBoxCheckedChanged);
			stack.Add(NPairBox.Create("Inflate Margins: ", inflateMarginsCheckBox));

			NCheckBox verticalAxisRoundToTick = new NCheckBox();
			verticalAxisRoundToTick.CheckedChanged += new Function<NValueChangeEventArgs>(OnverticalAxisRoundToTickCheckedChanged);
			stack.Add(NPairBox.Create("Left Axis Round To Tick: ", verticalAxisRoundToTick));

			NNumericUpDown pointSizeNumericUpDown = new NNumericUpDown();
			pointSizeNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnPointSizeNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Point Size: ", pointSizeNumericUpDown));

			NComboBox pointShapeComboBox = new NComboBox();
			pointShapeComboBox.FillFromEnum<ENPointShape>();
			pointShapeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPointShapeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Point Shape: ", pointShapeComboBox));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard point chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnPointShapeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Point.Shape = (ENPointShape)(arg.TargetNode as NComboBox).SelectedIndex;
		}

		void OnPointSizeNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Point.Size = (arg.TargetNode as NNumericUpDown).Value;
		}

		void OnverticalAxisRoundToTickCheckedChanged(NValueChangeEventArgs arg)
		{
			NLinearScale linearScale = m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;

			if (linearScale != null)
			{
				if ((arg.TargetNode as NCheckBox).Checked)
				{
					linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;
					linearScale.InflateViewRangeBegin = true;
					linearScale.InflateViewRangeEnd = true;
				}
				else
				{
					linearScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.Logical;
				}
			}
		}

		void OnInflateMarginsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
		{
			m_Point.InflateMargins = (arg.TargetNode as NCheckBox).Checked;
		}

		#endregion

		#region Fields

		NPointSeries m_Point;
		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NStandardPointExampleSchema;

		#endregion
	}
}
