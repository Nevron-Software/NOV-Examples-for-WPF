using System;

using Nevron.Nov.Chart;
using Nevron.Nov.Dom;
using Nevron.Nov.Graphics;
using Nevron.Nov.UI;

namespace Nevron.Nov.Examples.Chart
{
	/// <summary>
	/// Axis ticks example.
	/// </summary>
	public class NAxisTicksExample : NExampleBase
	{
		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		public NAxisTicksExample()
		{
			
		}
		/// <summary>
		/// Static constructor
		/// </summary>
		static NAxisTicksExample()
		{
			NAxisTicksExampleSchema = NSchema.Create(typeof(NAxisTicksExample), NExampleBaseSchema);
		}

		#endregion

		#region Example

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleContent()
		{
			NChartView chartView = new NChartView();
			chartView.Surface.CreatePredefinedChart(ENPredefinedChartType.Cartesian);

			chartView.Surface.Titles[0].Text = "Axis Ticks";

			// configure chart
			m_Chart = (NCartesianChart)chartView.Surface.Charts[0];

			// configure axes
			m_Chart.SetPredefinedCartesianAxes(ENPredefinedCartesianAxis.XOrdinalYLinear);
			NLinearScale scaleY = (NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale;
			scaleY.MinorTickCount = 1;
			scaleY.InnerMinorTicks.Visible = true;
			scaleY.InnerMinorTicks.Stroke = new NStroke(1, NColor.Black);
			scaleY.InnerMinorTicks.Length = 5;

			scaleY.OuterMinorTicks.Visible = true;
			scaleY.OuterMinorTicks.Stroke = new NStroke(1, NColor.Black);
			scaleY.OuterMinorTicks.Length = 5;

			scaleY.InnerMajorTicks.Visible = true;
			scaleY.InnerMajorTicks.Stroke = new NStroke(1, NColor.Black);

			// add interlaced stripe
			NScaleStrip strip = new NScaleStrip(new NColorFill(NColor.Beige), null, true, 0, 0, 1, 1);
			strip.Interlaced = true;
			scaleY.Strips.Add(strip);

			NOrdinalScale scaleX = (NOrdinalScale)m_Chart.Axes[ENCartesianAxis.PrimaryX].Scale;

			// create dummy data
			NBarSeries bar = new NBarSeries();
			bar.Name = "Bars";
			Random random = new Random();
			for (int i = 0; i < 10; i++)
			{
				bar.DataPoints.Add(new NBarDataPoint(random.Next(100)));
			}

			m_Chart.Series.Add(bar);

			chartView.Document.StyleSheets.ApplyTheme(new NChartTheme(ENChartPalette.Bright, false));

			return chartView;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override NWidget CreateExampleControls()
		{
			NStackPanel stack = new NStackPanel();
			NUniSizeBoxGroup boxGroup = new NUniSizeBoxGroup(stack);

			stack.Add(new NLabel("Major Outer Ticks"));

			NColorBox majorOuterTickColor = new NColorBox();
			majorOuterTickColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnMajorOuterTickColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", majorOuterTickColor));
			majorOuterTickColor.SelectedColor = NColor.Black;

			NNumericUpDown majorOuterTicksLengthNumericUpDown = new NNumericUpDown();
			majorOuterTicksLengthNumericUpDown.ValueChanged +=new Function<NValueChangeEventArgs>(OnMajorOuterTicksLengthNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Length", majorOuterTicksLengthNumericUpDown));
			majorOuterTicksLengthNumericUpDown.Value = 10;


			stack.Add(new NLabel("Major Inner Ticks"));

			NColorBox majorInnerTickColor = new NColorBox();
			majorInnerTickColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnMajorInnerTickColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", majorInnerTickColor));
			majorInnerTickColor.SelectedColor = NColor.Black;

			NNumericUpDown majorInnerTicksLengthNumericUpDown = new NNumericUpDown();
			majorInnerTicksLengthNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMajorInnerTicksLengthNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Length", majorInnerTicksLengthNumericUpDown));
			majorInnerTicksLengthNumericUpDown.Value = 10;

			stack.Add(new NLabel("Minor Inner Ticks"));

			NColorBox minorInnerTickColor = new NColorBox();
			minorInnerTickColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnMinorInnerTickColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", minorInnerTickColor));
			minorInnerTickColor.SelectedColor = NColor.Black;

			NNumericUpDown minorInnerTicksLengthNumericUpDown = new NNumericUpDown();
			minorInnerTicksLengthNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMinorInnerTicksLengthNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Length", minorInnerTicksLengthNumericUpDown));
			minorInnerTicksLengthNumericUpDown.Value = 10;

			stack.Add(new NLabel("Minor Outer Ticks"));

			NColorBox minorOuterTickColor = new NColorBox();
			minorOuterTickColor.SelectedColorChanged += new Function<NValueChangeEventArgs>(OnMinorOuterTickColorSelectedColorChanged);
			stack.Add(NPairBox.Create("Color", minorOuterTickColor));
			minorOuterTickColor.SelectedColor = NColor.Black;
			
			NNumericUpDown minorOuterTicksLengthNumericUpDown = new NNumericUpDown();
			minorOuterTicksLengthNumericUpDown.ValueChanged += new Function<NValueChangeEventArgs>(OnMinorOuterTicksLengthNumericUpDownValueChanged);
			stack.Add(NPairBox.Create("Length", minorOuterTicksLengthNumericUpDown));
			minorOuterTicksLengthNumericUpDown.Value = 10;

			return boxGroup;
		}

		protected override string GetExampleDescription()
		{
			return @"<p>This example demonstrates how to configure axis ticks.</p>";
		}

		#endregion

		#region Event Handlers

		void OnMinorOuterTicksLengthNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).OuterMinorTicks.Length = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMinorInnerTicksLengthNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).InnerMinorTicks.Length = ((NNumericUpDown)arg.TargetNode).Value;
		}
																
		void OnMajorOuterTicksLengthNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).OuterMajorTicks.Length = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMajorInnerTicksLengthNumericUpDownValueChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).InnerMajorTicks.Length = ((NNumericUpDown)arg.TargetNode).Value;
		}

		void OnMajorInnerTickColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).InnerMajorTicks.Stroke = new NStroke(1, ((NColorBox)arg.TargetNode).SelectedColor);
		}

		void OnMinorInnerTickColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).InnerMinorTicks.Stroke = new NStroke(1, ((NColorBox)arg.TargetNode).SelectedColor);
		}

		void OnMajorOuterTickColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).OuterMajorTicks.Stroke = new NStroke(1, ((NColorBox)arg.TargetNode).SelectedColor);
		}

		void OnMinorOuterTickColorSelectedColorChanged(NValueChangeEventArgs arg)
		{
			((NLinearScale)m_Chart.Axes[ENCartesianAxis.PrimaryY].Scale).OuterMinorTicks.Stroke = new NStroke(1, ((NColorBox)arg.TargetNode).SelectedColor);
		}

		#endregion

		#region Fields

		NCartesianChart m_Chart;

		#endregion

		#region Schema

		public static readonly NSchema NAxisTicksExampleSchema;

		#endregion
	}
}
