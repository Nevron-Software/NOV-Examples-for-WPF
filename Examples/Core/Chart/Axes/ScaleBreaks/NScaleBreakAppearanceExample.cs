using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Scale Break Appearance Examples
	/// </summary>
	public class NScaleBreakAppearanceExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NScaleBreakAppearanceExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NScaleBreakAppearanceExample()
		{
			NScaleBreakAppearanceExampleSchema = NSchema.Create(typeof(NScaleBreakAppearanceExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			// configure title
			chartView.Surface.Titles[0].Text = "Scale Breaks Appearance";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];
			m_Chart.Padding = new NMargins(20);

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);

			m_Chart.PlotFill = new NStockGradientFill(NColor.White, NColor.DarkGray);

			// configure scale
			NLinearScale yScale = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			yScale.ViewRangeInflateMode = ENScaleViewRangeInflateMode.MajorTick;

			m_ScaleBreak = new NAutoScaleBreak(0.4f);
			m_ScaleBreak.PositionMode = ENScaleBreakPositionMode.Percent;
			yScale.ScaleBreaks.Add(m_ScaleBreak);

			// add an interlaced strip to the Y axis
			NScaleStrip interlacedStrip = new NScaleStrip();
			interlacedStrip.Interlaced = true;
			interlacedStrip.Fill = new NColorFill(NColor.Beige);
			yScale.Strips.Add(interlacedStrip);

			// Create some data with a peak in it
			NBarSeries bar = new NBarSeries();
			m_Chart.Series.Add(bar);
			bar.DataLabelStyle = new NDataLabelStyle(false);

			// fill in some data so that it contains several peaks of data
			Random random = new Random();
			for (int i = 0; i < 8; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(30)));
			}

			for (int i = 0; i < 5; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(300 + random.Next(50)));
			}

			for (int i = 0; i < 8; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(30)));
			}	

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			NComboBox styleComboBox = new NComboBox();
			styleComboBox.FillFromEnum<ENScaleBreakStyle>();
			styleComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnStyleComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Style:", styleComboBox));
			styleComboBox.SelectedIndex = (int)ENScaleBreakStyle.Wave;

			NComboBox patternComboBox = new NComboBox();
			patternComboBox.FillFromEnum<ENScaleBreakPattern>();
			patternComboBox.SelectedIndexChanged += new Function<NValueChangeEventArgs>(OnPatternComboBoxSelectedIndexChanged);
			stack.Add(NPairBox.Create("Pattern:", patternComboBox));

			NNumericUpDown horzStepUpDown = new NNumericUpDown();
			horzStepUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnHorzStepUpDownValueChanged);
			stack.Add(NPairBox.Create("Horz Step:", horzStepUpDown));
			horzStepUpDown.Value = 20;

			NNumericUpDown vertStepUpDown = new NNumericUpDown();
			vertStepUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnVertStepUpDownValueChanged);
			stack.Add(NPairBox.Create("Vert Step:", vertStepUpDown));
			vertStepUpDown.Value = 10;

			NNumericUpDown lengthUpDown = new NNumericUpDown();
			lengthUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnLengthUpDownValueChanged);
			stack.Add(NPairBox.Create("Length:", lengthUpDown));
			lengthUpDown.Value = 10;
			
			return boxGroup;
		}
		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to change the appearance of scale breaks.</p>";
		}

		#endregion

		#region Event Handlers

		void OnLengthUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ScaleBreak.Length = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnVertStepUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ScaleBreak.VertStep = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnHorzStepUpDownValueChanged(NValueChangeEventArgs arg)
		{
			m_ScaleBreak.HorzStep = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnPatternComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_ScaleBreak.Pattern = (ENScaleBreakPattern)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		void OnStyleComboBoxSelectedIndexChanged(NValueChangeEventArgs arg)
		{
			m_ScaleBreak.Style = (ENScaleBreakStyle)((NComboBox)arg.TargetNode).SelectedIndex;
		}

		#endregion

		#region Fields

		private NCartesianChart m_Chart;
		private NScaleBreak m_ScaleBreak;

		#endregion

		#region Schema

		public static readonly NSchema NScaleBreakAppearanceExampleSchema;

		#endregion
	}
}