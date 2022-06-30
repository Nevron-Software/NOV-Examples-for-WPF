using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Step Line Example
	/// </summary>
	public class NStepLineExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStepLineExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStepLineExample()
		{
			NStepLineExampleSchema = NSchema.Create(typeof(NStepLineExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Step Line";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlaced stripe to the Y axis
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale.Strips.Add(strip);

			m_Line = new NLineSeries();
			m_Line.Name = "Line Series";
			m_Line.InflateMargins = true;
			m_Line.DataLabelStyle = new NDataLabelStyle("<value>");
			m_Line.MarkerStyle = new NMarkerStyle(new NSize(4, 4));

			Random random = new Random();

			for (int i = 0; i < 8; i++)
			{
				m_Line.DataPoints.Add(new NLineDataPoint(random.Next(80) + 20));
			}

			m_Chart.Series.Add(m_Line);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NComboBox lineSegmentModeComboBox = new NComboBox();

			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("HV Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("VH Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("HV Ascending VH Descending Step Line"));
			lineSegmentModeComboBox.Items.Add(new NComboBoxItem("VH Ascending HV Descending Step Line"));

			lineSegmentModeComboBox.SelectedIndexChanged += OnLineSegmentModeComboBoxSelectedIndexChanged;
			lineSegmentModeComboBox.SelectedIndex = 0;

			stack.Add(NPairBox.Create("Mode:", lineSegmentModeComboBox));
			
			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create step line charts.</p>";
		}

		#endregion

		#region Implementation

		void OnLineSegmentModeComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			switch (((NComboBox)arg.TargetNode).SelectedIndex)
			{
				case 0:
					m_Line.LineSegmentMode = ENLineSegmentMode.HVStep;
					break;
				case 1:
					m_Line.LineSegmentMode = ENLineSegmentMode.VHStep;
					break;
				case 2:
					m_Line.LineSegmentMode = ENLineSegmentMode.HVAscentVHDescentStep;
					break;
				case 3:
					m_Line.LineSegmentMode = ENLineSegmentMode.VHAscentHVDescentStep;
					break;
			}
		}

		#endregion

		#region Event Handlers


		#endregion

		#region Fields

		NCartesianChart m_Chart;
		NLineSeries m_Line;

		#endregion

		#region Schema

		public static readonly NSchema NStepLineExampleSchema;

		#endregion
	}
}
