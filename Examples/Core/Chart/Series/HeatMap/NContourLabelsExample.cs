using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Controur Labels Example
	/// </summary>
	public class NContourLabelsExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NContourLabelsExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NContourLabelsExample()
		{
			NContourLabelsExampleSchema = NSchema.Create(typeof(NContourLabelsExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartViewWithCommandBars chartViewWithCommandBars = new NChartViewWithCommandBars();
			NChartView chartView = chartViewWithCommandBars.View;
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Contour Chart";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

			m_HeatMap = new NHeatMapSeries();
			chart.Series.Add(m_HeatMap);

			m_HeatMap.Palette = new NColorValuePalette(new NColorValuePair[] { new NColorValuePair(0.0, NColor.Purple),
																	new NColorValuePair(1.5, NColor.MediumSlateBlue),
																	new NColorValuePair(3.0, NColor.CornflowerBlue),
																	new NColorValuePair(4.5, NColor.LimeGreen),
																	new NColorValuePair(6.0, NColor.LightGreen),
																	new NColorValuePair(7.5, NColor.Yellow),
																	new NColorValuePair(9.0, NColor.Orange),
																	new NColorValuePair(10.5, NColor.Red) });

			// make sure contour labels are visible
			m_HeatMap.ContourLabelStyle.Visible = true;
			m_HeatMap.ContourDisplayMode = ENContourDisplayMode.Contour;
			m_HeatMap.LegendView.Mode = ENSeriesLegendMode.SeriesLogic;
			m_HeatMap.LegendView.Format = "<level_value>";

			GenerateData();

			return chartViewWithCommandBars;
		}

		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NCheckBox showContourLabelsCheckBox = new NCheckBox();
			showContourLabelsCheckBox.CheckedChanged += OnShowContourLabelsCheckBoxCheckedChanged;
			showContourLabelsCheckBox.Checked = true;
			stack.Add(NPairBox.Create("Show Contour Labels:", showContourLabelsCheckBox));

			NCheckBox allowLabelsToFlipCheckBox = new NCheckBox();
			allowLabelsToFlipCheckBox.CheckedChanged += AllowLabelsToFlipCheckBoxCheckedChanged;
			allowLabelsToFlipCheckBox.Checked = false;
			stack.Add(NPairBox.Create("Allow Labels To Flip:", allowLabelsToFlipCheckBox));

			NCheckBox showLabelBackgroundCheckBox = new NCheckBox();
			showLabelBackgroundCheckBox.CheckedChanged += OnShowLabelBackgroundCheckBoxCheckedChanged;
			showLabelBackgroundCheckBox.Checked = true;
			stack.Add(NPairBox.Create("Show Label Background:", showLabelBackgroundCheckBox));

			NCheckBox clipContourCheckBox = new NCheckBox();
			clipContourCheckBox.CheckedChanged += OnClipContourCheckBoxCheckedChanged;
			clipContourCheckBox.Checked = true;
			stack.Add(NPairBox.Create("Clip Contour:", clipContourCheckBox));

			NNumericUpDown labelDistanceUpDown = new NNumericUpDown();
            labelDistanceUpDown.ValueChanged += OnLabelDistanceUpDownValueChanged;
            labelDistanceUpDown.Value = 80;
            stack.Add(NPairBox.Create("Label Distance:", labelDistanceUpDown));

			return boxGroup;
		}

        private void AllowLabelsToFlipCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.AllowLabelToFlip = (bool)arg.NewValue;
		}

        private void OnLabelDistanceUpDownValueChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.LabelDistance = (double)arg.NewValue;
        }

        private void OnClipContourCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.ClipContour = (bool)arg.NewValue;
		}

        private void OnShowLabelBackgroundCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.TextStyle.Background.Visible = (bool)arg.NewValue;
		}

        private void OnAllowLabelsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.AllowLabelToFlip = (bool)arg.NewValue;
		}

        private void OnShowContourLabelsCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
			m_HeatMap.ContourLabelStyle.Visible = (bool)arg.NewValue;
		}

        protected override string GetExampleDescription()
		{
			return @"<p>The example demonstrates the capabilities of the heat map to display labels at each contour line.</p>";
		}

		#endregion

		#region Implementation

		/// <summary>
		/// 
		/// </summary>
		private void GenerateData()
		{
			NHeatMapData data = m_HeatMap.Data;

			int GridStepX = 300;
			int GridStepY = 300;
			data.Size = new NSizeI(GridStepX, GridStepY);

			const double dIntervalX = 10.0;
			const double dIntervalZ = 10.0;
			double dIncrementX = (dIntervalX / GridStepX);
			double dIncrementZ = (dIntervalZ / GridStepY);

			double y, x, z;

			z = -(dIntervalZ / 2);

			for (int col = 0; col < GridStepX; col++, z += dIncrementZ)
			{
				x = -(dIntervalX / 2);

				for (int row = 0; row < GridStepY; row++, x += dIncrementX)
				{
					y = 10 - Math.Sqrt((x * x) + (z * z) + 2);
					y += 3.0 * Math.Sin(x) * Math.Cos(z);

					data.SetValue(row, col, y);
				}
			}
		}

		#endregion

		#region Fields

		NHeatMapSeries m_HeatMap;

		#endregion

		#region Schema

		public static readonly NSchema NContourLabelsExampleSchema;

		#endregion
	}
}
