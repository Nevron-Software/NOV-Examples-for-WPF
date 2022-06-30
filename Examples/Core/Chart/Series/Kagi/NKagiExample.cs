using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Kagi Example
	/// </summary>
	public class NKagiExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>s
		public NKagiExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NKagiExample()
		{
			NKagiExampleSchema = NSchema.Create(typeof(NKagiExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Kagi";

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
			m_KagiSeries = new NKagiSeries();
			m_KagiSeries.UseXValues = true;
			chart.Series.Add(m_KagiSeries);

			GenerateData(m_KagiSeries);
			
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NNumericUpDown reversalAmountUpDown = new NNumericUpDown();
			reversalAmountUpDown.Minimum = 1;
			reversalAmountUpDown.Maximum = 100;
			reversalAmountUpDown.Value = m_KagiSeries.ReversalAmount;
			reversalAmountUpDown.ValueChanged += OnReversalAmountUpDownValueChanged;
			stack.Add(NPairBox.Create("Reversal Amount:", reversalAmountUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the functionality of the kagi series.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateData(NKagiSeries KagiSeries)
		{
			NStockDataGenerator dataGenerator = new NStockDataGenerator(new NRange(50, 350), 0.002, 2);
			dataGenerator.Reset();

			DateTime dt = DateTime.Now;

			for (int i = 0; i < 100; i++)
			{
				KagiSeries.DataPoints.Add(new NKagiDataPoint(NDateTimeHelpers.ToOADate(dt), dataGenerator.GetNextValue()));

				dt = dt.AddDays(1);
			}
		}

		#endregion

		#region Event Handlers

		void OnReversalAmountUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_KagiSeries.ReversalAmount = ((NNumericUpDown)arg.TargetNode).Value;
		}

		#endregion

		#region Fields

		NKagiSeries m_KagiSeries;

		#endregion

		#region Schema

		public static readonly NSchema NKagiExampleSchema;

		#endregion
	}
}