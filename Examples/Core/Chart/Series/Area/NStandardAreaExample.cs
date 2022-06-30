using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard Area Example
	/// </summary>
	public class NStandardAreaExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardAreaExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardAreaExample()
		{
			NStandardAreaExampleSchema = NSchema.Create(typeof(NStandardAreaExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Area";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// setup X axis
			NOrdinalScale scaleX = (NOrdinalScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;
			scaleX.InflateContentRange = false;
			scaleX.MajorTickMode = ENMajorTickMode.AutoMaxCount;
			scaleX.DisplayDataPointsBetweenTicks = false;
			scaleX.Labels.Visible = false;

			for (int i = 0; i < monthLetters.Length; i++)
			{
				scaleX.CustomLabels.Add(new NCustomValueLabel(i, monthLetters[i]));
			}

			// add interlaced stripe for Y axis
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;

			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.Strips.Add(stripStyle);

			// setup area series
			m_Area = new NAreaSeries();
			m_Area.Name = "Area Series";

			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Visible = true;
			dataLabelStyle.Format = "<value>";

			m_Area.DataLabelStyle = dataLabelStyle;

			for (int i = 0; i < monthValues.Length; i++)
			{
				m_Area.DataPoints.Add(new NAreaDataPoint(monthValues[i]));
			}

			chart.Series.Add(m_Area);

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox originModeComboBox = new NComboBox();
			originModeComboBox.FillFromEnum<ENSeriesOriginMode>();
			originModeComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnOriginModeComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Origin Mode: ", originModeComboBox));

			NNumericUpDown customOriginUpDown = new NNumericUpDown();
			customOriginUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnCustomOriginUpDownValueChanged);
			stack.Add(NPairBox.Create("Custom Origin: ", customOriginUpDown));

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard area chart.</p>";
		}

		#endregion

		#region Event Handlers

		void OnCustomOriginUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_Area.CustomOrigin = (double)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnOriginModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_Area.OriginMode = (ENSeriesOriginMode)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		NAreaSeries m_Area;

		#endregion

		#region Schema

		public static readonly NSchema NStandardAreaExampleSchema;

		#endregion

		#region Constants

		internal static readonly double[] monthValues = new double[] { 16, 19, 16, 15, 18, 19, 24, 21, 22, 17, 19, 15 };
		internal static readonly string[] monthLetters = new string[] { "J", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };

		#endregion
	}
}
