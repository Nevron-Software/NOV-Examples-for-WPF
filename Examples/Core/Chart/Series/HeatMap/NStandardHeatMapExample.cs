using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Standard HeatMap Example
	/// </summary>
	public class NStandardHeatMapExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NStandardHeatMapExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NStandardHeatMapExample()
		{
			NStandardHeatMapExampleSchema = NSchema.Create(typeof(NStandardHeatMapExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Standard Heat Map";

			// configure chart
			NCartesianChart chart = (NCartesianChart)chartView.Surface.Charts[0];
			chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XYLinear);

            m_HeatMap = new NHeatMapSeries();
            chart.Series.Add(m_HeatMap);

            NGridData data = m_HeatMap.Data;

            m_HeatMap.Palette = new NColorValuePalette( new NColorValuePair[] { new NColorValuePair(0.0, NColor.Purple),
                                                        new NColorValuePair(1.5, NColor.MediumSlateBlue),
                                                        new NColorValuePair(3.0, NColor.CornflowerBlue),
                                                        new NColorValuePair(4.5, NColor.LimeGreen),
                                                        new NColorValuePair(6.0, NColor.LightGreen),
                                                        new NColorValuePair(7.5, NColor.Yellow),
                                                        new NColorValuePair(9.0, NColor.Orange),
                                                        new NColorValuePair(10.5, NColor.Red) });

			int gridSizeX = 100;
			int gridSizeY = 100;
			data.Size = new NSizeI(gridSizeX, gridSizeY);
			double y, x, z;

			const double dIntervalX = 10.0;
			const double dIntervalZ = 10.0;
			double dIncrementX = (dIntervalX / gridSizeX);
			double dIncrementZ = (dIntervalZ / gridSizeY);

			z = -(dIntervalZ / 2);

			for (int j = 0; j < gridSizeY; j++, z += dIncrementZ)
			{
				x = -(dIntervalX / 2);

				for (int i = 0; i < gridSizeX; i++, x += dIncrementX)
				{
					y = 10 - Math.Sqrt((x * x) + (z * z) + 2);
					y += 3.0 * Math.Sin(x) * Math.Cos(z);

					data.SetValue(i, j, y);
				}
			}

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

            NCheckBox smoothPaletteCheckBox = new NCheckBox("Smooth Palette");
            smoothPaletteCheckBox.CheckedChanged += OnSmoothPaletteCheckBoxCheckedChanged;
            smoothPaletteCheckBox.Checked = true;
            stack.Add(smoothPaletteCheckBox);

			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to create a standard heat map chart.</p>";
		}

		#endregion

		#region Event Handlers

        void OnSmoothPaletteCheckBoxCheckedChanged(NValueChangeEventArgs arg)
        {
            m_HeatMap.Palette.SmoothColors = ((NCheckBox)arg.TargetNode).Checked;
        }

		#endregion

		#region Fields

        NHeatMapSeries m_HeatMap;

		#endregion

		#region Schema

		public static readonly NSchema NStandardHeatMapExampleSchema;

		#endregion
	}
}
