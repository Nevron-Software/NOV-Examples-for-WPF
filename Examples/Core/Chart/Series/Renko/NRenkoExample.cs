using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Three Renko Example
	/// </summary>
	public class NRenkoExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NRenkoExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NRenkoExample()
		{
			NRenkoExampleSchema = NSchema.Create(typeof(NRenkoExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Renko";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;

			// add interlace stripe
			NScaleStrip stripStyle = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			stripStyle.Interlaced = true;
			scaleY.Strips.Add(stripStyle);

			// setup X axis
			NPriceTimeScale priceScale = new NPriceTimeScale();
			priceScale.InnerMajorTicks.Stroke = new NStroke(0.0, NColor.Black);
			chart.Axes[ENCartesianAxis.PrimaryX].Scale = priceScale;

			// setup line break series
			m_RenkoSeries = new NRenkoSeries();
			m_RenkoSeries.BoxSize = 1;
			m_RenkoSeries.UseXValues = true;
			chart.Series.Add(m_RenkoSeries);

			GenerateData(m_RenkoSeries);
			
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NNumericUpDown boxWidthPercentUpDown = new NNumericUpDown();
			boxWidthPercentUpDown.Minimum = 0;
			boxWidthPercentUpDown.Maximum = 100;
			boxWidthPercentUpDown.Value = m_RenkoSeries.BoxWidthPercent;
			boxWidthPercentUpDown.ValueChanged += OnBoxWidthPercentUpDownValueChanged;
			stack.Add(NPairBox.Create("Box Width Percent:", boxWidthPercentUpDown));

			NNumericUpDown boxSizePercentUpDownUpDown = new NNumericUpDown();
			boxSizePercentUpDownUpDown.Minimum = 1;
			boxSizePercentUpDownUpDown.Maximum = 100;
			boxSizePercentUpDownUpDown.Value = m_RenkoSeries.BoxSize;
			boxSizePercentUpDownUpDown.ValueChanged += OnBoxSizePercentUpDownUpDownValueChanged;
			boxSizePercentUpDownUpDown.DecimalPlaces = 0;
			stack.Add(NPairBox.Create("Number of Lines to Break:", boxSizePercentUpDownUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the functionality of the renko series.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateData(NRenkoSeries renkoSeries)
		{
			NStockDataGenerator dataGenerator = new NStockDataGenerator(new NRange(50, 350), 0.002, 2);
			dataGenerator.Reset();

			DateTime dt = DateTime.Now;

			for (int i = 0; i < 100; i++)
			{
				renkoSeries.DataPoints.Add(new NRenkoDataPoint(NDateTimeHelpers.ToOADate(dt), dataGenerator.GetNextValue()));

				dt = dt.AddDays(1);
			}
		}

		#endregion

		#region Event Handlers


		void OnBoxSizePercentUpDownUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RenkoSeries.BoxSize = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnBoxWidthPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_RenkoSeries.BoxWidthPercent = ((NNumericUpDown)arg.TargetNode).Value;
		}

		#endregion

		#region Fields

		NRenkoSeries m_RenkoSeries;

		#endregion

		#region Schema

		public static readonly NSchema NRenkoExampleSchema;

		#endregion
	}
}