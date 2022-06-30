using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Cluster Bar Example
	/// </summary>
	public class NClusterBarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NClusterBarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NClusterBarExample()
		{
			NClusterBarExampleSchema = NSchema.Create(typeof(NClusterBarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Cluster Bar Labels";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			// add interlace stripe
			NLinearScale linearScale = chart.Axes[ENCartesianAxis.PrimaryY].Scale as NLinearScale;
			NScaleStrip strip = new NScaleStrip(new NColorFill(ENNamedColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			linearScale.Strips.Add(strip);

			// add a bar series
			m_Bar1 = new NBarSeries();
			m_Bar1.Name = "Bar1";
			m_Bar1.MultiBarMode = ENMultiBarMode.Series;
			m_Bar1.DataLabelStyle = CreateDataLabelStyle();
			m_Bar1.ValueFormatter = new NNumericValueFormatter("0.###");
			chart.Series.Add(m_Bar1);

			// add another bar series
			m_Bar2 = new NBarSeries();
			m_Bar2.Name = "Bar2";
			m_Bar2.MultiBarMode = ENMultiBarMode.Clustered;
			m_Bar2.DataLabelStyle = CreateDataLabelStyle();
			m_Bar2.ValueFormatter = new NNumericValueFormatter("0.###");
			chart.Series.Add(m_Bar2);

			FillRandomData();

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel propertyStack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(propertyStack);

			NNumericUpDown gapPercentNumericUpDown = new NNumericUpDown();
			propertyStack.Add(NPairBox.Create("Gap Percent: ", gapPercentNumericUpDown));

			gapPercentNumericUpDown.Value = m_Bar1.GapFactor * 100.0;
			gapPercentNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(gapPercentNumericUpDown_ValueChanged);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a cluster bar chart.</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// Creates a new data label style object
		/// </summary>
		/// <returns></returns>
		private NDataLabelStyle CreateDataLabelStyle()
		{
			NDataLabelStyle dataLabelStyle = new NDataLabelStyle();

			dataLabelStyle.Format = "<value>";

			return dataLabelStyle;
		}
		private void FillRandomData()
		{
			Random random = new Random();
			for (int i = 0; i < 5; i++)
			{
				m_Bar1.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
				m_Bar2.DataPoints.Add(new NBarDataPoint(random.Next(10, 100)));
			}
		}

		#endregion

		#region Event Handlers

		void gapPercentNumericUpDown_ValueChanged(NValueChangeEventArgs arg)
		{
			m_Bar1.GapFactor = ((NNumericUpDown)arg.TargetNode).Value / 100.0;
		}

		#endregion

		#region Fields

		NBarSeries m_Bar1;
		NBarSeries m_Bar2;

		#endregion

		#region Schema

		public static readonly NSchema NClusterBarExampleSchema;

		#endregion
	}
}
