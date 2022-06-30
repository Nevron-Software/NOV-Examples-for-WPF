using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Multi Measure Radar example
	/// </summary>
	public class NMultiMeasureRadarExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NMultiMeasureRadarExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NMultiMeasureRadarExample()
		{
			NMultiMeasureRadarExampleSchema = NSchema.Create(typeof(NMultiMeasureRadarExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = CreateRadarChartView();

			// configure title
			chartView.Surface.Titles[0].Text = "Radar Axis Titles";

			// configure chart
			m_Chart = (NRadarChart)chartView.Surface.Charts[0];

			m_Chart.RadarMode = ENRadarMode.MultiMeasure;
			m_Chart.InnerRadius = 60;

			// set some axis labels
			AddAxis(m_Chart, "Population", true);
			AddAxis(m_Chart, "Housing Units", true);
			AddAxis(m_Chart, "Water", false);
			AddAxis(m_Chart, "Land", true);
			AddAxis(m_Chart, "Population\r\nDensity", false);
			AddAxis(m_Chart, "Housing\r\nDensity", false);

			// sample data
			object[] data = new object[]{ 
				"Cascade County", 80357, 35225, 13.75, 2697.90, 29.8, 13.1,
				"Custer County", 11696, 5360, 10.09, 3783.13, 3.1, 1.4,
				"Dawson County", 9059, 4168, 9.99, 2373.14, 3.8, 1.8,
				"Jefferson County", 10049, 4199, 2.19, 1656.64, 6.1, 2.5,
				"Missoula County", 95802, 41319, 20.37, 2597.97, 36.9, 15.9,
				"Powell County", 7180, 2930, 6.74, 2325.94, 3.1, 1.3 };

			for (int i = 0; i < 6; i++)
			{
				NRadarLineSeries radarLine = new NRadarLineSeries();
				m_Chart.Series.Add(radarLine);

				int baseIndex = i * 7;
				radarLine.Name = data[baseIndex].ToString();
				baseIndex = baseIndex + 1;

				for (int j = 0; j < 6; j++)
				{
					radarLine.DataPoints.Add(new NRadarLineDataPoint(System.Convert.ToDouble(data[baseIndex])));
					baseIndex = baseIndex + 1;
				}

				radarLine.DataLabelStyle = new NDataLabelStyle(false);

				NMarkerStyle markerStyle = new NMarkerStyle();
				markerStyle.Size = new NSize(4, 4);
				markerStyle.Visible = true;
				markerStyle.Fill = new NColorFill(NChartTheme.BrightPalette[i]);
				radarLine.MarkerStyle = markerStyle;

				radarLine.Stroke = new NStroke(2, NChartTheme.BrightPalette[i]);
			}
			
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a multi measure radar chart.</p>";
		}

		#endregion

		#region Event Handlers

	

		#endregion

		#region Implementation

		private void AddAxis(NRadarChart radar, string title, bool applyKFormatting)
		{
			NRadarAxis axis = new NRadarAxis();

			// set title
			axis.Title = title;
			radar.Axes.Add(axis);

			NLinearScale linearScale = axis.Scale as NLinearScale;
			linearScale.MajorGridLines.Visible = false;

			if (applyKFormatting)
			{
				linearScale.Labels.TextProvider = new NFormattedScaleLabelTextProvider(new NNumericValueFormatter("0,K"));
			}
		}

		#endregion

		#region Fields

		NRadarChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NMultiMeasureRadarExampleSchema;

		#endregion

		#region Static Methods

		private static NChartView CreateRadarChartView()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Radar);
			return chartView;
		}

		#endregion
	}
}