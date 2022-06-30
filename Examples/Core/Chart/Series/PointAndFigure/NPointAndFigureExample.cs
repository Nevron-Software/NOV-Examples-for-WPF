using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Point and Figure Example
	/// </summary>
	public class NPointAndFigureExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NPointAndFigureExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NPointAndFigureExample()
		{
			NPointAndFigureExampleSchema = NSchema.Create(typeof(NPointAndFigureExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Point And Figure";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];

			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			// setup X axis
			NPriceTimeScale priceScale = new NPriceTimeScale();
			priceScale.InnerMajorTicks.Stroke = new NStroke(0.0, NColor.Black);
			chart.Axes[ENCartesianAxis.PrimaryX].Scale = priceScale;

			const int nInitialBoxSize = 5;

			// setup Y axis
			NLinearScale scaleY = (NLinearScale)chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.MajorTickMode = ENMajorTickMode.CustomStep;
			scaleY.CustomStep = nInitialBoxSize;
			scaleY.OuterMajorTicks.Width = 0;
			scaleY.InnerMajorTicks.Width = 0;
			scaleY.AutoMinorTicks = true;
			scaleY.MinorTickCount = 1;
			scaleY.InflateViewRangeBegin = false;
			scaleY.InflateViewRangeEnd = false;
			scaleY.MajorGridLines.Stroke = new NStroke(0, NColor.Black);
			scaleY.MinorGridLines.Stroke = new NStroke(1, NColor.Black);

			float[] highValues = new float[20] { 21.3F, 42.4F, 11.2F, 65.7F, 38.0F, 71.3F, 49.54F, 83.7F, 13.9F, 56.12F, 27.43F, 23.1F, 31.0F, 75.4F, 9.3F, 39.12F, 10.0F, 44.23F, 21.76F, 49.2F };
			float[] lowValues = new float[20] { 12.1F, 14.32F, 8.43F, 36.0F, 13.5F, 47.34F, 24.54F, 68.11F, 6.87F, 23.3F, 12.12F, 14.54F, 25.0F, 37.2F, 3.9F, 23.11F, 1.9F, 14.0F, 8.23F, 34.21F };

			// setup Point & Figure series
			m_PointAndFigure = new NPointAndFigureSeries();
			m_PointAndFigure.UseXValues = true;
			chart.Series.Add(m_PointAndFigure);

			DateTime dt = DateTime.Now;

			// fill data
			int count = highValues.Length;
			for (int i = 0; i < count; i++)
			{
				m_PointAndFigure.DataPoints.Add(new NPointAndFigureDataPoint(NDateTimeHelpers.ToOADate(dt), highValues[i], lowValues[i]));
				dt = dt.AddDays(1);
			}
		
			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup group = new NUniSizeBoxGroup(stack);
			
			NNumericUpDown boxSizeUpDown = new NNumericUpDown();
			boxSizeUpDown.Minimum = 1;
			boxSizeUpDown.Maximum = 100;
			boxSizeUpDown.Value = m_PointAndFigure.BoxSize;
			boxSizeUpDown.ValueChanged += OnBoxSizeUpDownValueChanged;
			stack.Add(NPairBox.Create("Box Size:", boxSizeUpDown));

			NNumericUpDown reversalAmountUpDown = new NNumericUpDown();
			reversalAmountUpDown.Minimum = 1;
			reversalAmountUpDown.Maximum = 100;
			reversalAmountUpDown.Value = m_PointAndFigure.ReversalAmount;
			reversalAmountUpDown.ValueChanged += OnReversalAmountUpDownValueChanged;
			stack.Add(NPairBox.Create("Reversal Amount:", reversalAmountUpDown));

			return group;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates the functionality of the point and figure series.</p>";
		}

		#endregion

		#region Event Handlers

		void OnReversalAmountUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PointAndFigure.ReversalAmount = (int)((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnBoxSizeUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_PointAndFigure.BoxSize = (double)((NNumericUpDown)arg.TargetNode).Value;
		}

		#endregion

		#region Fields

		NPointAndFigureSeries m_PointAndFigure;

		#endregion

		#region Schema

		public static readonly NSchema NPointAndFigureExampleSchema;

		#endregion
	}
}