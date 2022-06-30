using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Range Fill Styles Example
	/// </summary>
	public class NRangeFillStylesExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRangeFillStylesExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRangeFillStylesExample()
		{
			NRangeFillStylesExampleSchema = NSchema.Create(typeof(NRangeFillStylesExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Tallest Buildings in the World";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NLinearScale xScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			xScale.Labels.Visible = false;
			xScale.InnerMajorTicks.Visible = false;
			xScale.OuterMajorTicks.Visible = false;
			xScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.Logical;
			xScale.LogicalInflate = new NRange(10, 10);

			// setup Y axis
			NLinearScale yScale = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.MajorGridLines.Visible = true;
			
			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			yScale.Strips.Add(strip);

			// setup shape series
			NRangeSeries rangeSeries = new NRangeSeries();
			chart.Series.Add(rangeSeries);
			chart.FitMode = ENCartesianChartFitMode.Aspect;
			chart.Aspect = 1;

			rangeSeries.DataLabelStyle = new NDataLabelStyle(false);
			rangeSeries.UseXValues = true;
			rangeSeries.LegendView.Mode = ENSeriesLegendMode.None;

			// fill data
			string[] buildingNames = new string[] { "Jeddah Tower", "Burj Khalifa", "Abraj Al Bait", "Taipei 101", "Zifeng Tower" };
			string[] countryNames = new string[] { "Saudi Arabia", "UAE", "Saudi Arabia", "Taiwan", "China" };

			NLegend legend = (NLegend)chartView.Surface.Legends[0];
			legend.Mode = ENLegendMode.Custom;
			legend.Header = new NLabel("Some header");
			double xOffset = 0;

			for (int i = 0; i < buildingNames.Length; i++)
            {
				string buildingImageResourceName = "RIMG_Buildings_" + buildingNames[i].Replace(" ", "") + "_emf";
				NImage buildingImage = NImage.FromResource(NResources.Instance.GetResource(buildingImageResourceName));

				// add data point
				NRangeDataPoint rangeDataPoint = new NRangeDataPoint();

				double buildingWidth = buildingImage.Width / 2;
				double buildingHeight = buildingImage.Height / 2;

				rangeDataPoint.X = xOffset;
				rangeDataPoint.X2 = xOffset + buildingWidth;

				rangeDataPoint.Y = 0;
				rangeDataPoint.Y2 = buildingHeight;
				rangeDataPoint.Fill = new NImageFill(buildingImage);

				rangeSeries.DataPoints.Add(rangeDataPoint);

				NCustomRangeLabel customRangeLabel = new NCustomRangeLabel(new NRange(rangeDataPoint.X, rangeDataPoint.X2), buildingNames[i]);
				customRangeLabel.LabelStyle.TickMode = ENRangeLabelTickMode.Separators;
				customRangeLabel.LabelStyle.FitMode = ENRangeLabelFitMode.Wrap | ENRangeLabelFitMode.AutoFlip;
				xScale.CustomLabels.Add(customRangeLabel);

				xOffset += buildingWidth + 10;

				// add legend item
				string buildingCountryResourceName = "RIMG_Buildings_" + countryNames[i].Replace(" ", "") + "_emf";
				NImage buildingCountryImage = NImage.FromResource(NResources.Instance.GetResource(buildingCountryResourceName));
				NImageBox buildingCountryImageBox = new NImageBox(buildingCountryImage);
				buildingCountryImageBox.PreferredSize = new NSize(40, 30);

				legend.Items.Add(new NPairBox(buildingCountryImageBox, new NLabel(buildingNames[i] + ", " + countryNames[i])));
			}

			return chartView;
		}

        protected override NWidget CreateExampleControls()
        {
			return null;
        }

        protected override string GetExampleDescription()
		{
			return @"<p>The example demostrates how to apply different fill styles to range elements, as well as the ability of the control to use vector images in WMF, EMF and EMF+ formats.</p>";
		}

		#endregion

		#region Fields

		#endregion

		#region Schema

		public static readonly NSchema NRangeFillStylesExampleSchema;

		#endregion
	}
}