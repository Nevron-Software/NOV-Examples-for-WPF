using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Editors;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;
using System;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Three Line Break Example
	/// </summary>
	public class NThreeLineBreakExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NThreeLineBreakExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NThreeLineBreakExample()
		{
			NThreeLineBreakExampleSchema = NSchema.Create(typeof(NThreeLineBreakExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Three Line Break";

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
			m_ThreeLineBreak = new NThreeLineBreakSeries();
			m_ThreeLineBreak.UseXValues = true;
			chart.Series.Add(m_ThreeLineBreak);

			GenerateData(m_ThreeLineBreak);
			
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
			boxWidthPercentUpDown.Value = m_ThreeLineBreak.BoxWidthPercent;
			boxWidthPercentUpDown.ValueChanged += OnBoxWidthPercentUpDownValueChanged;
			stack.Add(NPairBox.Create("Box Width Percent:", boxWidthPercentUpDown));

			NNumericUpDown numberOfLinesToBreakUpDown = new NNumericUpDown();
			numberOfLinesToBreakUpDown.Minimum = 1;
			numberOfLinesToBreakUpDown.Maximum = 100;
			numberOfLinesToBreakUpDown.Value = m_ThreeLineBreak.NumberOfLinesToBreak;
			numberOfLinesToBreakUpDown.ValueChanged += OnNumberOfLinesToBreakUpDownValueChanged;
			numberOfLinesToBreakUpDown.DecimalPlaces = 0;
			stack.Add(NPairBox.Create("Number of Lines to Break:", numberOfLinesToBreakUpDown));
						
			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the functionality of the three line break series.</p>";
		}

		#endregion

		#region Implementation

		private void GenerateData(NThreeLineBreakSeries threeLineBreak)
		{
			NStockDataGenerator dataGenerator = new NStockDataGenerator(new NRange(50, 350), 0.002, 2);
			dataGenerator.Reset();

			DateTime dt = DateTime.Now;

			for (int i = 0; i < 100; i++)
			{
				threeLineBreak.DataPoints.Add(new NThreeLineBreakDataPoint(NDateTimeHelpers.ToOADate(dt), dataGenerator.GetNextValue()));

				dt = dt.AddDays(1);
			}
		}

		#endregion

		#region Event Handlers

		void OnBoxWidthPercentUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ThreeLineBreak.BoxWidthPercent = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnNumberOfLinesToBreakUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ThreeLineBreak.NumberOfLinesToBreak = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		#endregion

		#region Fields

		NThreeLineBreakSeries m_ThreeLineBreak;

		#endregion

		#region Schema

		public static readonly NSchema NThreeLineBreakExampleSchema;

		#endregion
	}
}